using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RssFilter.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        Thread _thread;
        private Timer _timer;

        public TimedHostedService(Thread thread, ThreadStart func)
        {
            _thread = thread;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (_thread.ThreadState == ThreadState.Unstarted)
            {
                _thread.Start();
            }

            if (_thread.IsAlive)
            {
                _thread = new Thread(Startup.DbThreadProc);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {


            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
