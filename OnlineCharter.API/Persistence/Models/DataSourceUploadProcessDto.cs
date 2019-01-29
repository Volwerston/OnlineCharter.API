using System;

using DataSourceUploadProcessState = DataSource.Entities.DataSourceUploadProcess.DataSourceUploadProcessState;

namespace Persistence.Models
{
    public class DataSourceUploadProcessDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastChanged { get; set; }
        public DateTime? Settled { get; set; }
        public DataSourceUploadProcessState State { get; set; }
        public Guid DataSourceId { get; set; }
    }
}
