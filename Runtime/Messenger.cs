using System;
using System.Collections.Generic;
using UnityEngine;

namespace BroadcastMessages {

    /// <summary>
    /// Messenger for sending of <see cref="Message">messages</see> to all subscribers
    /// </summary>
    public static class Messenger {

        private const string MESSAGE_HEADER = "<color=yellow><b> Messenger Warning: </b></color>";



        #region Subscription

        /// <summary>
        /// Adds subscriber to sending
        /// </summary>
        /// <param name="subscriber">Subscriber with parameters</param>
        /// <typeparam name="T">Type of message for sending to subscribers</typeparam>
        public static void SubscribeTo<T> (Action<T> subscriber) where T : Message {
            Subscribers<T>.Container.Add(subscriber);
        }


        /// <summary>
        /// Adds subscriber to sending
        /// </summary>
        /// <param name="subscriber">Subscriber without parameters</param>
        /// <typeparam name="T">Type of message for sending to subscribers</typeparam>
        public static void SubscribeTo<T> (Action subscriber) where T : Message {
            var messageType = typeof(T);
            if (!Subscribers.Container.ContainsKey(messageType))
                Subscribers.Container.Add(messageType, new List<Action>());
            Subscribers.Container[messageType].Add(subscriber);
        }

        #endregion



        #region Unsubscription

        /// <summary>
        /// Removes subscriber from sending
        /// </summary>
        /// <param name="subscriber">Subscriber with parameters</param>
        /// <typeparam name="T">Type of message for sending to subscribers</typeparam>
        public static void UnsubscribeFrom<T> (Action<T> subscriber) where T : Message {
            if (!Subscribers<T>.Container.Contains(subscriber)) {
                Debug.LogWarning(
                    $"{MESSAGE_HEADER} Subscriber <b> {subscriber.Method.Name} </b> " +
                    $"is not subscribe to message <b> {typeof(T).Name} </b>");
                return;
            }

            Subscribers<T>.Container.Remove(subscriber);
        }


        /// <summary>
        /// Removes subscriber from sending
        /// </summary>
        /// <param name="subscriber">Subscriber without parameters</param>
        /// <typeparam name="T">Type of message for sending to subscribers</typeparam>
        public static void UnsubscribeFrom<T> (Action subscriber) where T : Message {
            var messageType = typeof(T);

            if (!Subscribers.Container.ContainsKey(messageType)) {
                Debug.LogWarning(
                    $"{MESSAGE_HEADER} Nobody subscribe to message " +
                    $"<b> {messageType.Name} </b> ");
                return;
            }

            if (!Subscribers.Container[messageType].Contains(subscriber)) {
                Debug.LogWarning(
                    $"{MESSAGE_HEADER} Subscriber <b> {subscriber.Method.Name} </b> " +
                    $"is not subscribe to message <b> {messageType.Name} </b>");
                return;
            }

            Subscribers.Container[messageType].Remove(subscriber);
        }

        #endregion



        #region Sending

        /// <summary>
        /// Sends message to all subscribers
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage<T> (T message) where T : Message {
            if (Subscribers<T>.Container.Count == 0) {
                SendLogInDebugConsole(typeof(T));

                return;
            }

            var subscribers = new List<Action<T>>(Subscribers<T>.Container);
            foreach (var subscriber in subscribers)
                subscriber.Invoke(message);
        }


        /// <summary>
        /// Sends message to all subscribers
        /// </summary>
        public static void SendMessage<T> () where T : Message {
            var messageType = typeof(T);

            if (!Subscribers.Container.ContainsKey(messageType)) {
                SendLogInDebugConsole(messageType);

                return;
            }

            if (Subscribers.Container[messageType].Count == 0) {
                SendLogInDebugConsole(messageType);

                return;
            }

            var subscribers = new List<Action>(Subscribers.Container[messageType]);
            foreach (var subscriber in subscribers)
                subscriber.Invoke();
        }


        private static void SendLogInDebugConsole (Type type) {
            var logMessage =
                $"{MESSAGE_HEADER} Message <b> {type.Name} </b> " +
                "was sent, but nobody doesn't get it";
            Debug.LogWarning(logMessage);
        }

        #endregion



        /// <summary>
        /// Keeps in itself subscribers with patameters
        /// </summary>
        /// <typeparam name="T">Тип сообщения, отправляемый подписчикам</typeparam>
        private static class Subscribers<T> {

            public static readonly List<Action<T>> Container = new List<Action<T>>();

        }



        /// <summary>
        /// Keeps in itself subscribers without patameters
        /// </summary>
        private static class Subscribers {

            public static readonly Dictionary<Type, List<Action>> Container =
                new Dictionary<Type, List<Action>>();

        }

    }

}