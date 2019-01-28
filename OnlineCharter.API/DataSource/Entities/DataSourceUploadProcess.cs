using System;
using Utils;

namespace DataSource.Entities
{
    public class DataSourceUploadProcess
    {
        public enum DataSourceUploadProcessState : byte
        {
            INIT,
            FILE_STORED,
            SCHEMA_GENERATED,
            DONE
        }

        public int Id { get; }
        public Guid DataSourceId { get; }
        public DateTime Created { get; }
        public DateTime LastChanged { get; set; }
        public DateTime? Settled { get; set; }
        public DataSourceUploadProcessState State { get; set; }

        public DataSourceUploadProcess(
            int id,
            Guid dataSourceId,
            DateTime created,
            DateTime lastChanged,
            DataSourceUploadProcessState state)
        {
            Id = id;
            DataSourceId = dataSourceId;
            Created = created;
            LastChanged = lastChanged;
            State = state;
        }

        public static DataSourceUploadProcess Create(int id, Guid dataSourceId)
        {
            var now = SystemDateTime.Now;

            return new DataSourceUploadProcess(id, dataSourceId, now, now, DataSourceUploadProcessState.INIT);
        }
    }
}
