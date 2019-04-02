using System.Collections.Concurrent;

namespace OnlineCharter.API.WebService.BackgroundTasks
{
    public class TasksToRun
    {
        private readonly BlockingCollection<DataSource.Entities.DataSource> _tasks;

        public TasksToRun() => _tasks = new BlockingCollection<DataSource.Entities.DataSource>();

        public void Enqueue(DataSource.Entities.DataSource dataSource) => _tasks.Add(dataSource);

        public DataSource.Entities.DataSource Dequeue() => _tasks.Take();
    }
}
