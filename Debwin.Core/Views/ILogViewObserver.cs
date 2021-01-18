using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debwin.Core.Views
{
    public interface ILogViewObserver
    {

        void NotifyMessagesChanged();

        //void NotifyStateChange(LogViewState state, int progressPercentage);

    }
}
