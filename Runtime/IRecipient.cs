namespace BroadcastMessages {

    public interface IRecipient<in TMessage> where TMessage : struct {

        public void GetMessage (TMessage message);

    }

}