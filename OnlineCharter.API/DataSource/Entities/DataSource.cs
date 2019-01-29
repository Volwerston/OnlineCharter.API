using Newtonsoft.Json.Linq;
using System;
using Utils;

namespace DataSource.Entities
{
    public class DataSource
    {
        public Guid Id { get; }
        public string Name { get; }
        public DateTime Created { get; }
        public string Location { get; }
        public byte[] Value { get; }
        public int UserId { get; }
        public JObject Schema { get; }

        public DataSource(
            Guid id,
            string name,
            DateTime created,
            string location,
            byte[] value,
            int userId,
            JObject schema)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));
            Ensure.NotNullOrEmpty(location, nameof(location));
            Ensure.NotNullOrEmpty(value, nameof(value));
            Ensure.NotNull(schema, nameof(schema));

            Id = id;
            Name = name;
            Created = created;
            Location = location;
            Value = value;
            UserId = userId;
            Schema = schema;
        }

        public static DataSource Create(string name, string location, byte[] value, int userId, JObject schema) 
            => new DataSource(Guid.NewGuid(), name, SystemDateTime.Now, location, value, userId, schema);
    }
}
