using Debwin.Core;
using Debwin.Core.MessageSources;
using Debwin.Core.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace combit.DebwinExtensions.MessageSources
{

    /// <summary>
    /// Implements special handling for control messages from combit Relationship Manager.
    /// </summary>
    public class CRMMessageCollector : DefaultMessageCollector
    {
        public string LogFilePath { get; set; }
        public bool EnableLongTermMonitoring { get; set; }

        protected override IList<LogMessage> ParseRawMessage(object rawMessage)
        {
            IList<LogMessage> logMessages = base.ParseRawMessage(rawMessage);

            if (logMessages == null ||logMessages[0] == null)
                return new List<LogMessage>();

            if (logMessages[0] != null && logMessages[0].Message == "Debwin.Command.ClearLogBufferAndFile" && EnableLongTermMonitoring == false)
            {
                // Send a handler through the pipeline that executes on all log views and clears them:
                this.Observer.NotifyControlMessage(new ClearAllViewsOfControllerControlMessage(logMessages[0]));
                return new List<LogMessage>();
            }
            else if (logMessages[0] != null && logMessages[0].Message == "Debwin.Command.LogToFile")
            {
                this.Observer.NotifyControlMessage(ControlMessageFactory.CreateForLogController(logMessages[0], logController =>
                {
                    if (logController.GetLogViews().Any(log => log is FileBasedLogView))   // do not attach a second file writer
                        return;

                    try
                    {
                        if (!Directory.Exists(LogFilePath))
                        {
                            Directory.CreateDirectory(LogFilePath);
                        }
                    }
                    catch (Exception e)
                    {
                        Observer.NotifyCollectorError(e);
                        return;
                    }


                    var fileBasedLog = new FileBasedLogView(Path.Combine(LogFilePath, "Debwin.log"), true, true);

                    // Copy previous messages of the log into the file
                    ILogView rootLog = logController.GetLogViews().FirstOrDefault();

                    if (rootLog is IQueryableLogView sourceLog)
                    {
                        sourceLog.CopyMessagesTo(fileBasedLog, null);
                    }

                    logController.AddView(fileBasedLog);
                }));
                return new List<LogMessage>();
            }


            return logMessages;
        }

    }

    public class ClearAllViewsOfControllerControlMessage : GenericControlMessage
    {
        public ClearAllViewsOfControllerControlMessage(LogMessage controlMessage) : base(controlMessage) { }
    }
}
