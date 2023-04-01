using BroadcastMessages;
using UnityEngine;

namespace Plugins.DIDenis.BroadcastMessages.Tests.Editor {

    public class Recipient {
        
        public bool DidGetMessage { get; private set; }

        public Recipient () {
            DidGetMessage = false;
            Messenger.SubscribeTo<TestMessage>(GetMessage);
        }


        private void GetMessage (TestMessage message) {
            if (message.value != string.Empty)
                DidGetMessage = true;
            Debug.Log($"{nameof(Recipient)} получил сообщение: {message.value}");
            Messenger.UnsubscribeFrom<TestMessage>(GetMessage);
        }

    }

}