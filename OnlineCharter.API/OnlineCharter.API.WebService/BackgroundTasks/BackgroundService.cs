using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;

namespace OnlineCharter.API.WebService.BackgroundTasks
{
    public class BackgroundService : IHostedService
    {
        private readonly TasksToRun _tasks;
        private readonly IServiceScopeFactory _scopeFactory;

        private CancellationTokenSource _tokenSource;

        private Task _currentTask;

        public BackgroundService(
            TasksToRun tasks,
            IServiceScopeFactory scopeFactory)
        {
            _tasks = tasks;
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var orchestrator = scope.ServiceProvider.GetService<IDataSourceOrchestrator>();
                _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        var currentDataSource = _tasks.Dequeue();
                        _currentTask = orchestrator.Process(currentDataSource);
                        await _currentTask;
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
            }            
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel(); 

            if (_currentTask == null) return;

            await Task.WhenAny(_currentTask, Task.Delay(-1, cancellationToken));
        }
    }
}
