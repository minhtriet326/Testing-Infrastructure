using core.DataLoaders.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_core.Dataloaders.Strategies
{
    public class JsonDataLoader : IDataLoaderStrategy
    {
        // Phương thức nhận đường dẫn file và trả về một tập hợp các mảng object.
        // Mỗi object[] là một bộ tham số cho một lần chạy test
        public IEnumerable<object[]> LoadData(string filePath)
        {
            string json = File.ReadAllText(filePath); 
            // json có dạng: "[ {\"a\":1,...}, {...}, {...} ]"

            JArray jArray = JArray.Parse(json);// chỉ chấp nhận mảng JSON (nghĩa là bắt đầu bằng '[' và kết thúc bằng ']'), nếu không sẽ ném lỗi
            // Chuỗi json ở trên được chuyển thành cấu trúc có thể DUYỆT
            //           array (JArray, Count = 3)
            //├─ [0] JObject
            //│     ├─ "username"      → JValue "user_ok"
            //│     ├─ "password"      → JValue "pass_ok"
            //│     └─ "expectSuccess" → JValue true
            //├─ [1] JObject
            //│     ├─ "username"      → JValue "user_ok"
            //│     ├─ "password"      → JValue "sai_pass"
            //│     └─ "expectSuccess" → JValue false
            //└─ [2] JObject
            //      ├─ "username"      → JValue ""
            //      ├─ "password"      → JValue "abc"
            //      └─ "expectSuccess" → JValue falsearray (JArray, Count = 3)

            foreach (JObject jobj in jArray)
            {
                // Chuyển từng JObject thành một mảng object
                yield return jobj.Properties() // lặp qua các cặp key-value của từng object trong mảng JSON
                                 .Select(p => p.Value.ToObject<object>()) // nếu chỉ p.Value thì sẽ là JToken, không phải int, string,... nên cần ToObject<object>()
                                 .ToArray();
            }
        }

        public IEnumerable<object[]> LoadData(string filePath, Type type)
        {
            string json = File.ReadAllText(filePath);

            // Sử dụng JsonConvert để deserialize JSON thành một List các obj của type được chỉ định (ex: List<MyModel>)
            var items = JsonConvert.DeserializeObject(json, typeof(List<>).MakeGenericType(type)) // make để "điền" type vào List<...>
                        as System.Collections.IEnumerable;

            if (items == null)
            {
                throw new Exception("After deserialize json file, array is null");
            }

            foreach (var item in items)
            {
                yield return new object[] { item };
            }
        }
    }
}
