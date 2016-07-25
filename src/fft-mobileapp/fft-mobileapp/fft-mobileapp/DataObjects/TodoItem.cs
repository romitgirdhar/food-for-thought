using Microsoft.Azure.Mobile.Server;

namespace fft_mobileapp.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}