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
        public string Location { get; }
        public byte[] Value { get; }
        public int UserId { get; }
        public List<DataTypeDefinition> Schema { get; set; }

        public DataSource(
            Guid id,
            string name,
            DateTime created,
            string location,
            int userId,
            List<DataTypeDefinition> schema,
            byte[] value = null)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(location, nameof(location));

            Id = id;
            Name = name;
            Created = created;
            Location = location;
            Value = value;
            UserId = userId;
            Schema = schema;
        }

        public static DataSource Create(string name, string location, byte[] value, int userId, List<DataTypeDefinition> schema) 
            => new DataSource(Guid.NewGuid(), name, SystemDateTime.Now, location, userId, schema, value);
    }
}
