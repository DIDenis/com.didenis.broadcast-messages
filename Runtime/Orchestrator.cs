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
        /// <param name="oneTimeSubscription">  </param>
        public static void Subscribe<TMessage> (IRecipient<TMessage> recipient, bool oneTimeSubscription = false)
            where TMessage : struct {
            Subscribe<TMessage>(recipient.GetMessage, oneTimeSubscription);
        }


        /// <inheritdoc cref="Subscribe{TMessage}(IRecipient{TMessage},bool)"/>
        public static void Subscribe<TMessage1, TMessage2> (
            IRecipient<TMessage1, TMessage2> recipient, bool oneTimeSubscription = false
        ) where TMessage1 : struct where TMessage2 : struct {
            Subscribe<TMessage1>(recipient.GetMessage, oneTimeSubscription);
            Subscribe<TMessage2>(recipient.GetMessage, oneTimeSubscription);
        }


        /// <inheritdoc cref="Subscribe{TMessage}(IRecipient{TMessage},bool)"/>
        public static void Subscribe<TMessage1, TMessage2, TMessage3> (
            IRecipient<TMessage1, TMessage2, TMessage3> recipient, bool oneTimeSubscription = false
        ) where TMessage1 : struct where TMessage2 : struct where TMessage3 : struct {
            Subscribe<TMessage1>(recipient.GetMessage, oneTimeSubscription);
            Subscribe<TMessage2>(recipient.GetMessage, oneTimeSubscription);
            Subscribe<TMessage3>(recipient.GetMessage, oneTimeSubscription);
        }


        /// <inheritdoc cref="Subscribe{TMessage}(IRecipient{TMessage},bool)"/>
        public static void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4> (
            IRecipient<TMessage1, TMessage2, TMessage3, TMessage4> recipient, bool oneTimeSubscription = false
        ) where TMessage1 : struct where TMessage2 : struct where TMessage3 : struct where TMessage4 : struct {
            Subscribe<TMessage1>(recipient.GetMessage, oneTimeSubscription);
            Subscribe<TMessage2>(recipient.GetMessage, oneTimeSubscription);
            Subscribe<TMessage3>(recipient.GetMessage, oneTimeSubscription);
            Subscribe<TMessage4>(recipient.GetMessage, oneTimeSubscription);
        }


        public static void Subscribe<TMessage> (Action<TMessage> recipientChannel, bool oneTimeSubscription = false)
            where TMessage : struct {
            if (Channel<TMessage>.Sending) {
                Channel<TMessage>.OnCompleteSending += () => {
                    Channel<TMessage>.Add(recipientChannel, oneTimeSubscription);
                };
            }
            else {
                Channel<TMessage>.Add(recipientChannel, oneTimeSubscription);
            }
        }


        /// <summary>
        /// Removes subscriber from sending
        /// </summary>
        /// <param name="recipient">Subscriber with parameters</param>
        public static void Unsubscribe<TMessage> (IRecipient<TMessage> recipient) where TMessage : struct {
            Unsubscribe<TMessage>(recipient.GetMessage);
        }


        /// <inheritdoc cref="Unsubscribe{TMessage}(IRecipient{TMessage})"/>
        public static void Unsubscribe<TMessage1, TMessage2> (IRecipient<TMessage1, TMessage2> recipient)
            where TMessage1 : struct where TMessage2 : struct {
            Unsubscribe<TMessage1>(recipient.GetMessage);
            Unsubscribe<TMessage2>(recipient.GetMessage);
        }


        /// <inheritdoc cref="Unsubscribe{TMessage}(IRecipient{TMessage})"/>
        public static void Unsubscribe<TMessage1, TMessage2, TMessage3> (
            IRecipient<TMessage1, TMessage2, TMessage3> recipient
        ) where TMessage1 : struct where TMessage2 : struct where TMessage3 : struct {
            Unsubscribe<TMessage1>(recipient.GetMessage);
            Unsubscribe<TMessage2>(recipient.GetMessage);
            Unsubscribe<TMessage3>(recipient.GetMessage);
        }


        /// <inheritdoc cref="Unsubscribe{TMessage}(IRecipient{TMessage})"/>
        public static void Unsubscribe<TMessage1, TMessage2, TMessage3, TMessage4> (
            IRecipient<TMessage1, TMessage2, TMessage3, TMessage4> recipient
        ) where TMessage1 : struct where TMessage2 : struct where TMessage3 : struct where TMessage4 : struct {
            Unsubscribe<TMessage1>(recipient.GetMessage);
            Unsubscribe<TMessage2>(recipient.GetMessage);
            Unsubscribe<TMessage3>(recipient.GetMessage);
            Unsubscribe<TMessage4>(recipient.GetMessage);
        }


        public static void Unsubscribe<TMessage> (Action<TMessage> recipientChannel) where TMessage : struct {
            if (Channel<TMessage>.Sending)
                Channel<TMessage>.OnCompleteSending += () => Channel<TMessage>.Remove(recipientChannel);
            else
                Channel<TMessage>.Remove(recipientChannel);
        }


        /// <summary>
        /// Sends message to all subscribers
        /// </summary>
        /// <param name="message"></param>
        public static void Send<TMessage> (TMessage message = default) where TMessage : struct {
            Channel<TMessage>.Send(message);
        }


        /// <summary>
        /// Keeps in itself subscribers with patameters
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения, отправляемый подписчикам</typeparam>
        private static class Channel<TMessage> where TMessage : struct {

            public static event Action OnCompleteSending;
            public static bool Sending { get; private set; }
            private static readonly List<(Action<TMessage>, bool)> s_data = new();


            public static void Add (Action<TMessage> recipient, bool oneTimeSubscription) {
                s_data.Add((recipient, oneTimeSubscription));
            }


            public static void Remove (Action<TMessage> recipient) {
                foreach ((Action<TMessage>, bool) tuple in s_data) {
                    if (tuple.Item1 == recipient) {
                        s_data.Remove(tuple);
                        break;
                    }
                }
            }


            public static void Send (TMessage message) {
                s_data.RemoveAll(data =>
                    data.Item1.Target == null || data.Item1.Target is Object unityRecipient && unityRecipient == null
                );

                Sending = true;

                foreach ((Action<TMessage> recipientChannel, bool _) in s_data)
                    recipientChannel(message);

                for (var i = 0; i < s_data.Count; i++)
                    if (s_data[i].Item2)
                        s_data.RemoveAt(i);

                Sending = false;

                OnCompleteSending?.Invoke();
                OnCompleteSending = null;
            }

        }

    }

}