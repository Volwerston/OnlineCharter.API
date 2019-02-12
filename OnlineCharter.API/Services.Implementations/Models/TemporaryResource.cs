using System;
using System.IO;
using System.Threading.Tasks;
using Utils;

namespace Services.Implementations.Models
{
    public class TemporaryResource : IDisposable
    {
        public string Path { get; }
        public byte[] Resource { get; }

        private string _tempPath;

        public TemporaryResource(string path, byte[] resource)
        {
            Ensure.NotNullOrEmpty(path, nameof(path));
            Ensure.NotNullOrEmpty(resource, nameof(resource));

            Resource = resource;
            Path = path;
        }

        public async Task<Uri> Save()
        {
            _tempPath = @"E:\storage" + Path;

            //if (!Directory.Exists(_tempPath))
            //{
            //    Directory.CreateDirectory(_tempPath);
            //}

            using (var fs = new FileStream(_tempPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await fs.WriteAsync(Resource, 0, Resource.Length);
            }

            return new Uri(_tempPath);
        }

        public void Close()
        {
            if (File.Exists(_tempPath))
            {
                File.Delete(_tempPath);
            }
        }

        public void Dispose() => Close();
    }
}
