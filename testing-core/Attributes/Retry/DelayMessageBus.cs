using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace testing_core.Attributes.Retry
{
    // class này implement IMessageBus nên xUnit coi nó là bus thật và gửi message vào
    // nó sẽ delay message lại và không gửi vào innerBus ngay lập tức
    // đến khi nào test xong thì nó mới gửi message vào innerBus
    public class DelayMessageBus : IMessageBus
    {
        private readonly IMessageBus innerBus; // -> đây là bus thật, do DelayMessageBus bọc nó nên gọi là inner, 
        private readonly List<IMessageSinkMessage> messages = new(); // list msg tạm để giữ các message

        public DelayMessageBus(IMessageBus innerBus)
        {
            this.innerBus = innerBus;
        }

        // xUnit gọi method này mỗi lần có một message
        // Thay vì chuyển đi, ta chỉ Add vào danh sách messages để giữ lại. Đây là chỗ “trì hoãn” thật sự.
        public bool QueueMessage(IMessageSinkMessage message)
        {
            messages.Add(message);
            return true; // luôn trả về true để xUnit tiếp tục chạy test và gửi message vào bus này
        }

        public void Dispose()
        {
            foreach (var message in messages)
            {
                // chuyển hết msg sang bus thật
                innerBus.QueueMessage(message); // chính là method bên trên
            }
        }
    }
}
