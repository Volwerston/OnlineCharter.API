using Microsoft.Extensions.Configuration;

namespace OnlineCharter.API.WebService.Infrastructure.Settings
{
    public class Settings
    {
        private readonly IConfigurationSection _applicationSettings;

        public Settings(IConfigurationSection appSettings)
        {
            _applicationSettings = appSettings;
        }
    }
}
