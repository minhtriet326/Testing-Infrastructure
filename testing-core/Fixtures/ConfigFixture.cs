using core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_core.Fixtures
{
    public class ConfigFixture
    {
        public TestConfig Config { get; private set; }

        public ConfigFixture() { 
            var configLoader = new ConfigLoader();
            Config = configLoader.Load();
        }
    }
}
