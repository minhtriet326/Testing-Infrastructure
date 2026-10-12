using core.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace core.Configuration
{
    public class ConfigLoader : IConfigLoader
    {
        public TestConfig Load()
        {
            string json = File.ReadAllText("config.json");

            TestConfig config = JsonSerializer.Deserialize<TestConfig>(json);

            if (config == null)
            {
                throw new ArgumentNullException("Config is unreadable.");
            }

            return config;
        }
    }
}
