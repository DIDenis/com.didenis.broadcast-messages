using UnityEngine;

namespace BroadcastMessages.Tests {

    public class Recipient : IRecipient<TestMessage> {

        public bool DidGetMessage { get; private set; }


        public Recipient () {
            DidGetMessage = false;
            Orchestrator.Subscribe(this);
        }


        public void GetMessage (TestMessage message) {
            if (!string.IsNullOrEmpty(message.value))
                DidGetMessage = true;
            Debug.Log($"{nameof(Recipient)} got message: {message.value}");
            Orchestrator.Unsubscribe(this);
        }

    }

}