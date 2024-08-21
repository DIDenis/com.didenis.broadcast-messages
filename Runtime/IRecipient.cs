namespace BroadcastMessages {

    public interface IRecipient<in TMessage> where TMessage : struct {

        public void GetMessage (TMessage message);

    }


    public interface IRecipient<in TMessage1, in TMessage2> where TMessage1 : struct where TMessage2 : struct {

        public void GetMessage (TMessage1 message);
        public void GetMessage (TMessage2 message);

    }


    public interface IRecipient<in TMessage1, in TMessage2, in TMessage3>
        where TMessage1 : struct
        where TMessage2 : struct
        where TMessage3 : struct {

        public void GetMessage (TMessage1 message);
        public void GetMessage (TMessage2 message);
        public void GetMessage (TMessage3 message);

    }


    public interface IRecipient<in TMessage1, in TMessage2, in TMessage3, in TMessage4>
        where TMessage1 : struct
        where TMessage2 : struct
        where TMessage3 : struct
        where TMessage4 : struct {

        public void GetMessage (TMessage1 message);
        public void GetMessage (TMessage2 message);
        public void GetMessage (TMessage3 message);
        public void GetMessage (TMessage4 message);

    }

}