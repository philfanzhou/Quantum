using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using PF.Domain.FilterTasks;
using PF.Domain.FilterTasks.Entities;
using Timer = System.Timers.Timer;

namespace PF.Domain.FilterEngine
{
    /// <summary>
    /// 过滤任务执行引擎
    /// </summary>
    public sealed class Engine
    {
        private static readonly Lazy<Engine> _lazyEngine = new Lazy<Engine>(() => new Engine()); 
        private readonly Timer _timer;
        private readonly AutoResetEvent _autoReset;
        private readonly FilterTaskFactory _queryTaskFactory;
        private readonly FilterTaskFactory _execTaskFactory;
        private readonly ConcurrentQueue<FilterTask> _filterTasks;
        private FilterTaskService _filterTaskService;

        private bool _stopEngine;

        public static Engine Instance { get { return _lazyEngine.Value; } }

        private Engine()
        {
            _autoReset = new AutoResetEvent(false);
            _timer = new Timer { Enabled = false, Interval = 30000 };
            _timer.Elapsed += QueryFilterTasks;
            _queryTaskFactory = new FilterTaskFactory(1);
            _execTaskFactory = new FilterTaskFactory(10);
            _filterTasks = new ConcurrentQueue<FilterTask>();
        }

        /// <summary>
        /// 启动引擎
        /// </summary>
        public static void StartEngine(FilterTaskService filterTaskService)
        {
            if (_lazyEngine.Value._stopEngine == false)
            {
                return;
            }

            _lazyEngine.Value._filterTaskService = filterTaskService;
            _lazyEngine.Value._stopEngine = false;
            _lazyEngine.Value._timer.Start();
            Task.Factory.StartNew(_lazyEngine.Value.ScheduleFilterTask);
        }

        /// <summary>
        /// 停止引擎
        /// </summary>
        public static void StopEngine()
        {
            if (_lazyEngine.Value._stopEngine)
            {
                return;
            }

            _lazyEngine.Value._stopEngine = true;
            _lazyEngine.Value._timer.Stop();
            _lazyEngine.Value._autoReset.Set();
        }

        /// <summary>
        /// 立即执行一个过滤任务
        /// </summary>
        /// <param name="filterTask">过滤任务</param>
        public void ExecFilterTaskImmediatly<TTask>(TTask filterTask) where TTask : FilterTask
        {
            var task = Task.Factory.StartNew(() => ExecFilterTask(filterTask));
            task.ContinueWith(FilterTaskExecEnd);
        }

        /// <summary>
        /// 获取待执行任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryFilterTasks(object sender, ElapsedEventArgs e)
        {
            var endtime = DateTime.Now;
            var starttime = endtime - new TimeSpan(0, 0, 5, 0);
            var querytask = _queryTaskFactory.CreateNew(() => _filterTaskService.QueryFilterTasksByTime(starttime, endtime));
            querytask.ContinueWith(QueryEnd);
            querytask.Start();
        }

        /// <summary>
        /// 添加待执行任务到任务队列
        /// </summary>
        /// <param name="task">任务集合</param>
        private void QueryEnd(Task<IEnumerable<ScheduledFilterTask>> task)
        {
            foreach (var filterTask in task.Result)
            {
                _filterTaskService.UpdateFilterTaskStatus(filterTask, FilterTaskStatus.Queuing);
                _filterTasks.Enqueue(filterTask);
            }

            _autoReset.Set();
        }

        /// <summary>
        /// 启动过滤任务执行调度
        /// </summary>
        private void ScheduleFilterTask()
        {
            while (_stopEngine == false)
            {
                FilterTask filtertask;
                if (_filterTasks.TryDequeue(out filtertask) == false)
                {
                    _autoReset.WaitOne();
                    continue;
                }

                var task = _execTaskFactory.CreateNew(() => ExecFilterTask(filtertask));
                task.ContinueWith(FilterTaskExecEnd);
                task.Start();
            }
        }

        /// <summary>
        /// 执行一个过滤任务
        /// </summary>
        /// <param name="filterTask">过滤任务</param>
        /// <returns></returns>
        private FilterTask ExecFilterTask<TTask>(TTask filterTask) where TTask : FilterTask
        {
            //更新状态为正在执行
            _filterTaskService.UpdateFilterTaskStatus(filterTask, FilterTaskStatus.Running);

            //创建新的结果并保存
            var result = new FilterResult(filterTask.Id) { ExecStartTime = DateTime.Now, ExecEndTime = DateTime.Now };
            _filterTaskService.UpdateFilterTaskResult(filterTask, result);

            //执行任务
            _filterTaskService.ExecFilterTask(filterTask);
            return filterTask;
        }

        /// <summary>
        /// 过滤任务执行完成
        /// </summary>
        /// <param name="task">任务</param>
        private void FilterTaskExecEnd<TTask>(Task<TTask> task) where TTask : FilterTask
        {
            var filtertask = task.Result;

            //更新任务状态为完成
            _filterTaskService.UpdateFilterTaskStatus(filtertask, FilterTaskStatus.Completed);

            //更新任务结果
            filtertask.Result.ExecEndTime = DateTime.Now;
            _filterTaskService.UpdateFilterTaskResult(filtertask, filtertask.Result);
        }
    }
}
