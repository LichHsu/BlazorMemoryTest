using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorMemoryTest.Data
{
    public class LogService
    {
        private ConcurrentQueue<LogPresenter> presentQueue { get; } = new ConcurrentQueue<LogPresenter>();

        public event EventHandler<LogPresenter> OnLog = null;
        public List<LogPresenter> Logs { get; } = new List<LogPresenter>();
        public string Filter { get; set; } = string.Empty;

        public LogService()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    SpinWait.SpinUntil(() => false, 10);
                    if (!presentQueue.TryDequeue(out var item)) { continue; }

                    if (Logs.Count > 500)
                    {
                        var temp = Logs.Take(50);
                        Logs.Clear();
                        Logs.AddRange(temp);
                    }

                    Logs.Insert(0, item);
                    OnLog?.Invoke(this, item);
                }
            });
        }

        public IEnumerable<LogPresenter> GetLog() => string.IsNullOrEmpty(Filter) ? Logs : Logs.Where(o => o.ToString().ToLower().Contains(Filter))
                                                                                               .Take(500);

        public void PushMessage(string message)
        {
            presentQueue.Enqueue(new LogPresenter()
            {
                Message = message,
            });
        }
    }
}
