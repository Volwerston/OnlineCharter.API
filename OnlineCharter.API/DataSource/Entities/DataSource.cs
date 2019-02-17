using System;
using System.Collections.Generic;
using Utils;

namespace DataSource.Entities
{
    public enum DataTypeOrigin
    {
        Attribute,
        Element
    }

    public class DataTypeDefinition
    {
        public DataTypeOrigin Origin { get; set; }
        public string FullName { get; set; }
        public string DataType { get; set; }
    }

    public class DataSource
    {
        public Guid Id { get; }
        public string Name { get; }
        public DateTime Created { get; }
        public byte[] Value { get; }
        public int UserId { get; }
        public List<DataTypeDefinition> Schema { get; set; }

        public DataSource(
            Guid id,
            string name,
            DateTime created,
            int userId,
            List<DataTypeDefinition> schema,
            byte[] value = null)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));

            Id = id;
            Name = name;
            Created = created;
            Value = value;
            UserId = userId;
            Schema = schema;
        }

        public static DataSource Create(string name, byte[] value, int userId, List<DataTypeDefinition> schema) 
            => new DataSource(Guid.NewGuid(), name, SystemDateTime.Now, userId, schema, value);
    }
}
