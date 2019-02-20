using System;
using System.Collections.Generic;
using DataSource.Entities;

namespace OnlineCharter.API.WebService.Controllers.DataSource
{
    internal class DataSourceGetAllResponse
    {
        public DataSourceDto[] DataSources { get; set; }

        internal class DataSourceDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public List<DataTypeDefinition> Schema { get; set; }
        }
    }
}
