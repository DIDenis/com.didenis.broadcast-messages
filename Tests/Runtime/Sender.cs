using UnityEngine;

namespace BroadcastMessages.Tests {

    public class Sender {

        public void SendMessage (string message) {
            Orchestrator.Send(new TestMessage(message));
            Debug.Log($"{nameof(Sender)} sent message: {message}");
        }

    }

}