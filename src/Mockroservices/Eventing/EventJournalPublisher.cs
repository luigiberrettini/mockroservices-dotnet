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
using System.Threading;
using System.Threading.Tasks;
using VaughnVernon.Mockroservices.Messaging;

namespace VaughnVernon.Mockroservices.Eventing
{
    public class EventJournalPublisher
    {
        private EventJournalReader reader;
        private Topic topic;
        private CancellationTokenSource cts;
        private TaskCompletionSource<object> tcs;

        public static EventJournalPublisher From(string eventJournalName, string messageBusName, string topicName)
        {
            return new EventJournalPublisher(eventJournalName, messageBusName, topicName);
        }

        public Task<object> CloseAsync()
        {
            cts.Cancel();
            return tcs.Task;
        }

        protected EventJournalPublisher(string eventJournalName, string messageBusName, string topicName)
        {
            reader = EventJournal.Open(eventJournalName).Reader("topic-" + topicName + "-reader");
            topic = MessageBus.Start(messageBusName).OpenTopic(topicName);
            cts = new CancellationTokenSource();
            tcs = new TaskCompletionSource<object>();
            Task.Run(() => DispatchEach());
        }

        internal void DispatchEach()
        {
            while (true)
            {
                StoredEvent storedEvent = reader.ReadNext();

                if (storedEvent.IsValid())
                {
                    var id = Convert.ToString(storedEvent.Id);
                    var type = storedEvent.EventValue.Type;
                    var body = storedEvent.EventValue.Body;
                    Message message = new Message(id, type, body);
                    topic.Publish(message);
                    reader.Acknowledge(storedEvent.Id);
                }
                else
                {
                    Thread.Sleep(100);
                    if (cts.Token.IsCancellationRequested)
                        tcs.SetResult(new object());
                }
            }
        }
    }
}