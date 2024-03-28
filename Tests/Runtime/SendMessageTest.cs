using System;
using NUnit.Framework;

namespace BroadcastMessages.Tests {

    public class SendMessageTest {

        [Test]
        public void SendMessage () {
            var sender = new Sender();
            var recipient = new Recipient();
            sender.SendMessage(Guid.NewGuid().ToString());
            Assert.True(recipient.DidGetMessage);
        }

    }

}