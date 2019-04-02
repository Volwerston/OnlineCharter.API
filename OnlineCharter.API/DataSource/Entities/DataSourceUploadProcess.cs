using System;
using Utils;

namespace DataSource.Entities
{
    public class DataSourceUploadProcess
    {
        public enum DataSourceUploadProcessState : byte
        {
            Init,
            FileStored,
            SchemaGenerated,
            Done
        }

        private DataSourceUploadProcessState _state;

        public int Id { get; set; }
        public Guid DataSourceId { get; }
        public DateTime Created { get; }
        public DateTime LastChanged { get; private set; }
        public DateTime? Settled { get; private set; }

        public DataSourceUploadProcessState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                LastChanged = SystemDateTime.Now;

                if (_state == DataSourceUploadProcessState.Done)
                {
                    Settled = LastChanged;
                }
            }
        }

        public DataSourceUploadProcess(
            int id,
            Guid dataSourceId,
            DateTime created,
            DateTime lastChanged,
            DateTime? settled,
            DataSourceUploadProcessState state)
        {
            Id = id;
            DataSourceId = dataSourceId;
            Created = created;
            LastChanged = lastChanged;
            Settled = settled;
            State = state;
        }

        public static DataSourceUploadProcess Create(Guid dataSourceId)
        {
            var now = SystemDateTime.Now;

            return new DataSourceUploadProcess(0, dataSourceId, now, now, null, DataSourceUploadProcessState.Init);
        }
    }
}
