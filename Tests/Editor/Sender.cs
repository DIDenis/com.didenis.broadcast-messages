using BroadcastMessages;
using UnityEngine;

namespace Plugins.DIDenis.BroadcastMessages.Tests.Editor {

    public class Sender {

        public void SendMessage (string message) {
            Messenger.SendMessage(new TestMessage(message));
            Debug.Log($"{nameof(Sender)} отправил сообщение: {message}");
        }

    }

}