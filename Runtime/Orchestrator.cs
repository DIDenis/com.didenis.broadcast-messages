using System;
using System.Collections.Generic;

namespace BroadcastMessages {

    /// <summary>
    /// Messenger for sending of messages to all subscribers
    /// </summary>
    public static class Orchestrator {

        /// <summary>
        /// Adds subscriber to sending
        /// </summary>
        /// <param name="recipient">Subscriber with parameters</param>
        /// <typeparam name="TMessage">Type of message for sending to subscribers</typeparam>
        public static void Subscribe<TMessage> (IRecipient<TMessage> recipient) where TMessage : struct {
            if (Subscribers<TMessage>.Sending)
                Subscribers<TMessage>.OnCompleteSending += () => Subscribers<TMessage>.Add(recipient);
            else
                Subscribers<TMessage>.Add(recipient);
        }


        /// <summary>
        /// Removes subscriber from sending
        /// </summary>
        /// <param name="recipient">Subscriber with parameters</param>
        /// <typeparam name="TMessage">Type of message for sending to subscribers</typeparam>
        public static void Unsubscribe<TMessage> (IRecipient<TMessage> recipient) where TMessage : struct {
            if (Subscribers<TMessage>.Sending)
                Subscribers<TMessage>.OnCompleteSending += () => Subscribers<TMessage>.Remove(recipient);
            else
                Subscribers<TMessage>.Remove(recipient);
        }


        /// <summary>
        /// Sends message to all subscribers
        /// </summary>
        /// <param name="message"></param>
        public static void Send<TMessage> (TMessage message = default) where TMessage : struct {
            Subscribers<TMessage>.Send(message);
        }



        /// <summary>
        /// Keeps in itself subscribers with patameters
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения, отправляемый подписчикам</typeparam>
        private static class Subscribers<TMessage> where TMessage : struct {

            private static readonly List<IRecipient<TMessage>> Recipients = new();
            public static event Action OnCompleteSending;
            public static bool Sending { get; private set; }


            public static void Add (IRecipient<TMessage> recipient) {
                Recipients.Add(recipient);
            }


            public static void Remove (IRecipient<TMessage> recipient) {
                Recipients.Remove(recipient);
            }


            public static void Send (TMessage message) {
                Sending = true;
                foreach (IRecipient<TMessage> recipient in Recipients)
                    recipient.GetMessage(message);
                Sending = false;

                OnCompleteSending?.Invoke();
                OnCompleteSending = null;
            }

        }

    }

}