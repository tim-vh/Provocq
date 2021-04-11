using System;
using System.Threading;

namespace Provocq
{
    public class ActionWrapper<T>
    {
        public EventWaitHandle OnCompleted { get; } = new EventWaitHandle(false, EventResetMode.ManualReset);

        public Action Action { get; set; }

        public Exception Exception { get; set; }
    }
}
