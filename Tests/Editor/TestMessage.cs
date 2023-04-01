using BroadcastMessages;

namespace Plugins.DIDenis.BroadcastMessages.Tests.Editor {

    public class TestMessage : Message {

        public readonly string value;


        public TestMessage (string value) {
            this.value = value;
        }

    }

}