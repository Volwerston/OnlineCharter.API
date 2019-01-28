using System;
using System.Collections.Generic;

namespace Utils
{
    public static class Ensure
    {
        public static void NotNull<T>(T field, string fieldName)
        {
            if (field == null)
            {
                throw new ArgumentNullException($"'{fieldName}' cannot be null");
            }
        }

        public static void NotNullOrEmpty(string field, string fieldName)
        {
            NotNull(field, fieldName);

            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentException($"'{fieldName}' cannot be empty");
            }
        }

        public static void NotNullOrEmpty<T>(ICollection<T> collection, string fieldName)
        {
            NotNull(collection, fieldName);

            if (collection.Count == 0)
            {
                throw new ArgumentException($"'{fieldName}' cannot be empty");
            }
        }
    }
}
