using Debwin.Core.MessageSources;
using Debwin.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Debwin.Core.Controller
{

    public interface IDebwinController
    {
        event EventHandler<AddRemoveLogControllerEventArgs> AddedLogController;
        event EventHandler<AddRemoveLogControllerEventArgs> RemovedLogController;

        void AddLogController(ILogController logController);

        void RemoveLogController(ILogController logController);

        ReadOnlyCollection<ILogController> GetLogControllers();
    }


    public class DebwinController : IDebwinController
    {
        public event EventHandler<AddRemoveLogControllerEventArgs> AddedLogController;
        public event EventHandler<AddRemoveLogControllerEventArgs> RemovedLogController;

        private readonly List<ILogController> _controllers;

        public DebwinController()
        {
            _controllers = new List<ILogController>();
        }


        public void AddLogController(ILogController logController)
        {
            _controllers.Add(logController);
            if (AddedLogController != null)
                AddedLogController(this, new AddRemoveLogControllerEventArgs() { LogController = logController });
        }

        public ReadOnlyCollection<ILogController> GetLogControllers()
        {
            return _controllers.AsReadOnly();
        }

        public void RemoveLogController(ILogController logController)
        {
            _controllers.Remove(logController);
            if (RemovedLogController != null)
                RemovedLogController(this, new AddRemoveLogControllerEventArgs() { LogController = logController });
        }


        public static ILogController GetNewLogController(IDebwinController debwin, string logName, IMessageSource messageSource, IMessageParser messageParser)
        {
            ILogController logController = new LogController() { Name = logName };
            debwin.AddLogController(logController);

            logController.AddMessageCollector(new DefaultMessageCollector()
            {
                Source = messageSource,
                Parser = messageParser
            });
            return logController;
        }
    }

    public class AddRemoveLogControllerEventArgs : EventArgs
    {
        public ILogController LogController { get; set; }
    }
}
