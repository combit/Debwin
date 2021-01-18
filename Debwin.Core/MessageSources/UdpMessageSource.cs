using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace Debwin.Core.MessageSources
{


    public class UdpMessageSource : MessageSourceBase
    {

        private const int ERROR_WSAEINTR = 10004;   // Interrupted function call
        private const int ERROR_WSAEADDRINUSE = 10048;   // Address already in use

        private UdpClient _udpServer;
        private Thread _listenerThread;

        public override void Start()
        {
            if (_listenerThread != null)
                return;

            _listenerThread = new Thread(new ThreadStart(ReceiverLoop));
            _listenerThread.Name = "UdpListener.ReceiverLoop";
            _listenerThread.IsBackground = true;
            _listenerThread.Start();
        }

        public override void Stop()
        {
            if (_udpServer != null)
            {
                _udpServer.Close();  // lets the blocking Receive() cancel and throw an SocketException with code {ERROR_WSAEINTR}
            }

            if (_listenerThread != null)
            {
                _listenerThread.Join(TimeSpan.FromSeconds(1));
                _listenerThread.Abort();
                _listenerThread = null;
            }
            CurrentProgress = 0;
        }

        public static int GetAvailablePort()
        {
            Random r = new Random();
            int port = 9174; // use default Debwin path

            var properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            var tcpConnectionPorts = properties.GetActiveTcpConnections()
                                .Where(n => n.LocalEndPoint.Port >= 1024)
                                .Select(n => n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            var tcpListenerPorts = properties.GetActiveTcpListeners()
                                .Where(n => n.Port >= 1024)
                                .Select(n => n.Port);

            //getting active udp listeners
            var udpListenerPorts = properties.GetActiveUdpListeners()
                                .Where(n => n.Port >= 1024)
                                .Select(n => n.Port);

            while (tcpConnectionPorts.Contains(port) || tcpListenerPorts.Contains(port) || udpListenerPorts.Contains(port))
            {
                port = r.Next(1024, 49151);
            }
            return port;
        }

        private void ReceiverLoop()
        {
            try
            {
                try
                {
                    using (_udpServer = new UdpClient(this.Port))
                    {
                        _udpServer.Client.ReceiveBufferSize = 10 * 1024 * 1024;
                        IPEndPoint sender = new IPEndPoint(IPAddress.Any, this.Port);
                        this.CurrentProgress = Constants.MESSAGESOURCE_PROGRESS_MARQUEE;

                        while (true)
                        {
                            // #36626
                            // Receive could throw SocketException AND ObjectDisposedException... see also info from MSDN:
                            // https://docs.microsoft.com/de-de/dotnet/api/system.net.sockets.udpclient.receive?view=netframework-4.7.2
                            byte[] data = _udpServer.Receive(ref sender);

                            if (MessageObserver != null)
                            {
                                MessageObserver.NotifyNewRawMessage(HandleIncomingPacket(data));
                            }
                        }
                    }
                }
                catch (SocketException e)
                {
                    if (e.NativeErrorCode == ERROR_WSAEINTR) // expected when UdpClient is closed while listening
                    {
                        return;
                    }
                    else if (e.NativeErrorCode == ERROR_WSAEADDRINUSE)  // port in use
                    {
                        throw new DebwinUserException("The logging could not be started - it seems that this log source is already active.", e);
                    }
                }
                catch (ObjectDisposedException) // thrown by Socket.Receive if the underlying socket is closed
                {
                    return;
                }
            }
            catch (Exception e)   // listener thread should not throw any exceptions at all
            {
                this.CurrentProgress = Constants.MESSAGESOURCE_PROGRESS_ERROR;
                MessageObserver.NotifyMessageSourceError(e);
            }
            finally
            {
                _udpServer = null;
            }
        }

        protected virtual object HandleIncomingPacket(byte[] data)
        {
            return data;
        }


        public int Port { get; set; }

        public override bool IsStopped { get { return _listenerThread == null || _udpServer == null; } }

        public override string GetName()
        {
            return "UDP Port " + Port;
        }

    }
}
