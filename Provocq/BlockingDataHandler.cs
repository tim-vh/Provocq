using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Provocq
{
    public class BlockingDataHandler<T>
    {
        private readonly BlockingCollection<ActionWrapper<T>> _actionWrappers = new BlockingCollection<ActionWrapper<T>>();
        private readonly IPersistor<T> _persistor;

        private readonly T _datacontext;

        public BlockingDataHandler(IPersistor<T> persistor)
        {
            _persistor = persistor;
            _datacontext = LoadDataContext();
            Task.Run(ProcessActions);
        }

        public Task ExecuteCommand(ICommand<T> command)
        {
            var actionWrapper = new ActionWrapper<T>
            {
                Action = () =>
                {
                    command.Execute(_datacontext);
                    SaveDataContext();
                }
            };

            _actionWrappers.Add(actionWrapper);

            actionWrapper.OnCompleted.WaitOne();

            if (actionWrapper.Exception != null)
            {
                ExceptionDispatchInfo.Throw(actionWrapper.Exception);
            }

            return Task.CompletedTask;
        }

        public Task<TResult> ExecuteQuery<TResult>(IQuery<T, TResult> query)
        {
            var queryActionWrapper = new QueryActionWrapper<T, TResult>();
            queryActionWrapper.Action = () => queryActionWrapper.Result = query.Execute(_datacontext);

            _actionWrappers.Add(queryActionWrapper);

            queryActionWrapper.OnCompleted.WaitOne();

            if (queryActionWrapper.Exception != null)
            {
                ExceptionDispatchInfo.Throw(queryActionWrapper.Exception);
            }

            return Task.FromResult(queryActionWrapper.Result);
        }

        private void ProcessActions()
        {
            while (true)
            {
                var wrapper = _actionWrappers.Take();
                try
                {
                    wrapper.Action.Invoke();
                }
                catch (Exception exception)
                {
                    wrapper.Exception = exception;
                }
                finally
                {
                    wrapper.OnCompleted.Set();
                }
            }
        }

        private void SaveDataContext()
        {
            _persistor.Save(_datacontext);
        }

        private T LoadDataContext()
        {
            return _persistor.Load();
        }
    }
}
