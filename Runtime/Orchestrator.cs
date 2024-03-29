using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace BroadcastMessages {

    /// <summary>
    /// Messenger for sending of messages to all subscribers
    /// </summary>
    public static class Orchestrator {

        /// <summary>
        /// Adds subscriber to sending
        /// </summary>
        /// <param name="recipient">Subscriber with parameters</param>
        /// <param name="oneTimeSubscription"></param>
        /// <typeparam name="TMessage">Type of message for sending to subscribers</typeparam>
        public static void Subscribe<TMessage> (IRecipient<TMessage> recipient, bool oneTimeSubscription = false)
            where TMessage : struct {
            if (Subscribers<TMessage>.Sending)
                Subscribers<TMessage>.OnCompleteSending +=
                    () => Subscribers<TMessage>.Add(recipient, oneTimeSubscription);
            else
                Subscribers<TMessage>.Add(recipient, oneTimeSubscription);
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

            private static readonly List<(IRecipient<TMessage>, bool)> Data = new();
            public static event Action OnCompleteSending;
            public static bool Sending { get; private set; }


            public static void Add (IRecipient<TMessage> recipient, bool oneTimeSubscription) {
                Data.Add((recipient, oneTimeSubscription));
            }


            public static void Remove (IRecipient<TMessage> recipient) {
                foreach ((IRecipient<TMessage>, bool) tuple in Data) {
                    if (tuple.Item1 == recipient) {
                        Data.Remove(tuple);
                        break;
                    }
                }
            }


            public static void Send (TMessage message) {
                Data.RemoveAll(data =>
                    data.Item1 == null || data.Item1 is Object unityRecipient && unityRecipient == null
                );

                Sending = true;

                foreach ((IRecipient<TMessage> recipient, bool _) in Data)
                    recipient.GetMessage(message);

                for (var i = 0; i < Data.Count; i++)
                    if (Data[i].Item2)
                        Data.RemoveAt(i);

                Sending = false;

                OnCompleteSending?.Invoke();
                OnCompleteSending = null;
            }

        }

    }

}