using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace testing_core.Attributes.Retry
{
    public class RetryTestCase : XunitTestCase
    {
        private int maxRetries;

        [EditorBrowsable(EditorBrowsableState.Never)] // Ẩn nhắc code của IntelliSense, để user khỏi gõ nhầm constructor rỗng này
        // Attribute này là 1 "hàng rào bảo vệ", đề phòng người code gõ
        // new RetryTestCase() (tức constructor rỗng bên dưới) thì báo msg và gạch đỏ(error: true)
        [Obsolete("Called by the de-serializer", error:true)]
        // Constructor rỗng này không dành cho bạn, mà dành cho xUnit
        // xUnit phải lưu test case ra dạng dữ liệu rồi dựng lại
        // Để dựng lại, xUnit phải tạo một RetryTestCase trống trước,
        // rồi mới Gọi Deserialize để đổ dữ liệu đã lưu vào.
        // Nó không biết giá trị nào để truyền vào constructor 5 tham số, nên cần một constructor không tham số.
        public RetryTestCase() { } 

        public RetryTestCase(
            IMessageSink diagnosticMessageSink, 
            TestMethodDisplay defaultMethodDisplay, 
            TestMethodDisplayOptions defaultMethodDisplayOptions, 
            ITestMethod testMethod, 
            int maxRetries,
            object[]? testMethodArguments = null)  // testMethodArguments là mảng các tham số của test method LOẠI theory, fact thì = null vì không có tham số
            
            : base(diagnosticMessageSink, 
                  defaultMethodDisplay, 
                  defaultMethodDisplayOptions, 
                  testMethod, 
                  testMethodArguments)
        {
            this.maxRetries = maxRetries;
        }

        // IXunitSerializationInfo data: một “cái thùng” xUnit đưa cho bạn.
        // Bạn bỏ đồ vào thùng khi đóng gói (Serialize), và lấy đồ ra khi mở thùng (Deserialize). Mỗi món đồ có một nhãn (chuỗi) để tìm lại.
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data); // gọi bản của lớp lớp cha để cất những data RẤT QUAN TRỌNG XÁC ĐỊNH "đây test nào" (test method, tên hiển thị...)
            data.AddValue("MaxRetries", maxRetries); // bỏ giá trị maxRetries vào thùng, dán nhãn "MaxRetries".
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);
            maxRetries = data.GetValue<int>("MaxRetries"); // lấy giá trị maxRetries ra khỏi thùng, tìm theo nhãn "MaxRetries".
        }

        public override async Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            int runCount = 0;

            while (true)
            {
                // tạo mới mỗi lần chạy test, nếu dùng chung thì msg fail của lần trước vẫn còn và bị xả ra cùng lần cuối, báo cáo lại hiện fail.
                var delayMessageBus = new DelayMessageBus(messageBus);

                // chạy test bằng logic gốc của lớp cha, nhưng truyền delayMessageBus thay vì bus thật.
                // Nhờ vậy mọi message của lần chạy này bị giữ lại
                var summary = await base.RunAsync(
                    diagnosticMessageSink,
                    delayMessageBus,
                    constructorArguments,
                    aggregator,
                    cancellationTokenSource);// await đứng trước base.RunAsync để CHỜ summary có giá trị, và để Dispose() ở dòng sau chỉ chạy khi test đã chạy xong và message đã được giữ đủ

                // RunSummary có field Failed đếm số test fail. Bằng 0 nghĩa là lần này pass
                // ExceptionAggregator là cái “giỏ” xUnit dùng để gom các exception xảy ra ở tầng hạ tầng
                // Dừng luôn khi "giỏ" có ex, vì ta dùng chung 1 "giỏ" nên nếu có ex thì các lần chạy sau vô nghĩa.
                if (aggregator.HasExceptions || summary.Failed == 0 || ++runCount > maxRetries)
                {
                    delayMessageBus.Dispose(); // truyền msg từ delayMessageBus sang messageBus thật

                    return summary; // chỉ là 1 bảng đếm số test pass/fail/skip, không phải là kết quả chi tiết từng test case. Kết quả chi tiết từng test case được gửi qua messageBus.
                }
                
                // chỉ hữu ích khi debug tại sao test retry
                diagnosticMessageSink.OnMessage(
                    new DiagnosticMessage($"Retrying test {DisplayName}, attempt {runCount}/{maxRetries}"));
            }
        }
    }
}
