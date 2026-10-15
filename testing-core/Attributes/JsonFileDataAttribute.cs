using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using testing_core.Dataloaders.Strategies;
using Xunit.Sdk;

namespace testing_core.Attributes
{
    // Class này là một custom attribute(~ annotation) để đọc dữ liệu từ file JSON và truyền cho các test method trong xUnit.
    public class JsonFileDataAttributes : DataAttribute
    {
        private readonly string _filePath;
        private readonly string? _propertyName;
        private readonly Type? _type;

        public JsonFileDataAttributes(string filePath)
        {
            _filePath = filePath;
            _propertyName = null;
        }
        public JsonFileDataAttributes(string filePath, string? propertyName)
        {
            _filePath = filePath;
            _propertyName = propertyName;
        }

        public JsonFileDataAttributes(string filePath, Type type)
        {
            _filePath = filePath;
            _type = type;
        }
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) // Check annotation này có được đặt đúng trên 1 method hay không
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            // Nếu path truyền vào là absolute path thì dùng luôn,
            // nếu là relative path thì kết hợp với base directory của project để tạo thành absolute path
            string path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.Combine(AppContext.BaseDirectory, _filePath);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find file at path: {path}");
            }

            JsonDataLoader loader = new JsonDataLoader();

            if (_type == null)
            {
                return loader.LoadData(path);
            }
            else
            {
                return loader.LoadData(path, _type);
            }
        }
    }
}
