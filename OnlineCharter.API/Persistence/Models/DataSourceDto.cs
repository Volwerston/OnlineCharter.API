﻿using System;
using System.Collections.Generic;
using DataSource.Entities;

namespace Persistence.Models
{
    public class DataSourceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public byte[] Value { get; set; }
        public string UserId { get; set; }
        public List<DataTypeDefinition> Schema { get; set; }
    }
}
