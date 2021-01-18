using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Debwin.Core.MessageSources
{

    public class FileMessageSource : MessageSourceBase
    {

        private readonly string _filePath;
        private Thread _listenerThread;
        private TextReader _reader;
        private FileStream _stream;
        private Encoding _encoding;
        private bool _isStopped;

        public FileMessageSource(String filePath, Encoding encoding)
        {
            _filePath = filePath;
            _encoding = encoding;
            _isStopped = true;
        }


        public override void Start()
        {
            if (_listenerThread != null)
                return;

            _stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024 * 1024);
            _reader = new StreamReader(_stream, _encoding, false);

            _listenerThread = new Thread(new ThreadStart(ReceiverLoop));
            _listenerThread.Name = "FileListener.ReceiverLoop";
            _listenerThread.IsBackground = true;
            _listenerThread.Start();
        }

        private void ReceiverLoop()
        {
            try
            {
                _isStopped = false;
                while (true)
                {
                    // Don`t read file when no one is listening
                    if (MessageObserver == null)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    string nextLine = _reader.ReadLine();

                    // If EOF is reached, nextLine is null. Since new data might be written
                    // to the file, try again later
                    if (nextLine == null)  // TODO   && options.MonitorFile
                    {
                        _isStopped = true;
                        CurrentProgress = 100;
                        return;
                        //Thread.Sleep(25);
                        //continue;
                    }

                    MessageObserver.NotifyNewRawMessage(nextLine);

                    // Calculate progress of file reading in %
                    CurrentProgress = Math.Min(99, (int)(100.0 * _stream.Position / _stream.Length));  // only if EOF is reached, we want to set it to 100

                }
            }
            catch (ObjectDisposedException) { /* expected on Stop() when filestream is closed */ }
            catch (ThreadAbortException) { }
            catch (Exception)
            {
                // Todo handle
                CurrentProgress = Constants.MESSAGESOURCE_PROGRESS_ERROR;
            }
            finally
            {
                _reader.Dispose();
                _stream.Dispose();
                _reader = null;
                _listenerThread = null;

                _isStopped = true;

                if (DeleteFileOnClose)
                    File.Delete(_filePath);
            }
        }


        public override void Stop()
        {
            if (_listenerThread == null)
                return;

            Thread tmpListenerThread = _listenerThread;  // make local copy of variable, ending thread will set it to null

            _reader.Dispose();  // trigger exception in (blocked) listener thread
            _stream.Dispose();
            tmpListenerThread.Join(TimeSpan.FromSeconds(1));
            tmpListenerThread.Abort();
        }

        /// <summary>If true, the opened file is deleted when the message source is closed.</summary>
        public bool DeleteFileOnClose { get; set; }

        public override bool IsStopped { get { return _isStopped; } }

        public override string GetName()
        {
            return Path.GetFileName(_filePath);
        }

    }
}
