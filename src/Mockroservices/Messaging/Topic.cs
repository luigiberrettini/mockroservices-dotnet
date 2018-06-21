//   Copyright © 2017 Vaughn Vernon. All rights reserved.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VaughnVernon.Mockroservices.Messaging
{
    public class Topic
    {
        private ConcurrentQueue<Message> queue;
        private HashSet<ISubscriber> subscribers;
        private CancellationTokenSource cts;
        private TaskCompletionSource<object> tcs;

        public string Name { get; private set; }

        public void Publish(Message message)
        {
            if (cts.Token.IsCancellationRequested)
                return;
            queue.Enqueue(message);
        }

        public void Subscribe(ISubscriber subscriber)
        {
            lock (subscribers)
            {
                subscribers.Add(subscriber);
            }
        }

        public Task CloseAsync()
        {
            cts.Cancel();
            return tcs.Task;
        }

        internal Topic(string name)
        {
            queue = new ConcurrentQueue<Message>();
            subscribers = new HashSet<ISubscriber>();
            cts = new CancellationTokenSource();
            tcs = new TaskCompletionSource<object>();
            Name = name;
            Task.Run(() => DispatchEach());
        }

        internal void DispatchEach()
        {
            while (!cts.Token.IsCancellationRequested || queue.Count > 0)
            {
                lock (subscribers)
                {
                    if (subscribers.Count > 0)
                    {
                        if (queue.TryDequeue(out Message message))
                        {
                            foreach (ISubscriber subscriber in subscribers)
                            {
                                try
                                {
                                    subscriber.Handle(message);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error dispatching message to handler: " + e.Message);
                                    Console.WriteLine(e.ToString());
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }
            tcs.SetResult(new object());
        }
    }
}