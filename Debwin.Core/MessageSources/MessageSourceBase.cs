using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debwin.Core.MessageSources
{
    public abstract class MessageSourceBase : IMessageSource
    {
        public event EventHandler ProgressChanged;

        private int _currentProgress = 0;

        public void SetMessageObserver(IMessageSourceObserver handler)
        {
            MessageObserver = handler;
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract bool IsStopped { get; }

        protected IMessageSourceObserver MessageObserver { get; set; }

        /// <summary>See <see cref="IMessageSource.CurrentProgress"/></summary>
        public int CurrentProgress
        {
            get
            {
                return _currentProgress;
            }
            protected set
            {
                int _oldProgress = _currentProgress;
                _currentProgress = value;
                if (_currentProgress != _oldProgress)
                    OnProgressChanged();
            }
        }

        public abstract string GetName();

        protected void OnProgressChanged()
        {
            if (ProgressChanged != null)
                ProgressChanged(this, null);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion


    }
}
