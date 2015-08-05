using System;
using System.Threading;
using System.Threading.Tasks;

namespace PF.Domain.FilterEngine
{
    /// <summary>
    /// 阻塞的任务工厂
    /// </summary>
    public class FilterTaskFactory
    {
        private readonly static object _lock = new object();
        private readonly AutoResetEvent _autoReset;
        private readonly int _maxTasks;

        private volatile int _currentRunning;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxTasks">最多可以同时执行的任务数</param>
        public FilterTaskFactory(int maxTasks)
        {
            _maxTasks = maxTasks;
            _autoReset = new AutoResetEvent(false);
        }

        /// <summary>
        /// 创建一个新的任务，如果已经达到最大同时执行任务数，方法会阻塞
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task CreateNew(Action action)
        {
            if (_currentRunning >= _maxTasks)
            {
                _autoReset.WaitOne();
            }

            lock (_lock)
            {
                _currentRunning++;
            }

            var task = new Task(action);
            task.ContinueWith(TaskEnd);
            return task;
        }

        /// <summary>
        /// 创建一个新任务，如果已经达到最大同时执行任务数，方法会阻塞
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public Task<T> CreateNew<T>(Func<T> func)
        {
            if (_currentRunning >= _maxTasks)
            {
                _autoReset.WaitOne();
            }

            lock (_lock)
            {
                _currentRunning++;
            }

            var task = new Task<T>(func);
            task.ContinueWith(TaskEnd);
            return task;
        }

        /// <summary>
        /// 一个任务执行完成后，当前正在执行任务数-1
        /// </summary>
        /// <param name="task"></param>
        private void TaskEnd(Task task)
        {
            lock (_lock)
            {
                _currentRunning--;
            }

            _autoReset.Set();
        }
    }
}