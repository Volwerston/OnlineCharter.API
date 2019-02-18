using System;
using System.Collections.Generic;
using DataSource.Entities;

namespace OnlineCharter.API.WebService.Controllers.DataSource
{
    public class DataSourceGetResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public List<DataTypeDefinition> Schema { get; set; }
    }
}
