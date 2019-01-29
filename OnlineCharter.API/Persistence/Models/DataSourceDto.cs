using Newtonsoft.Json.Linq;
using System;

namespace Persistence.Models
{
    public class DataSourceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        public byte[] Value { get; set; }
        public int UserId { get; set; }
        public JObject Schema { get; set; }
    }
}
