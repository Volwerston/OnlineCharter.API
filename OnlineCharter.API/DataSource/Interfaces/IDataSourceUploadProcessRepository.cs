﻿using System;
using System.Threading.Tasks;
using DataSource.Entities;

namespace DataSource.Interfaces
{
    public interface IDataSourceUploadProcessRepository
    {
        Task Create(DataSourceUploadProcess uploadProcess);
        Task Update(DataSourceUploadProcess uploadProcess);
        Task<DataSourceUploadProcess> Find(Guid dataSourceId);
    }
}
