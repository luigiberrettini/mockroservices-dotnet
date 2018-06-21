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

using Xunit;
using VaughnVernon.Mockroservices.Eventing;
using VaughnVernon.Mockroservices.Messaging;

namespace VaughnVernon.Mockroservices.TestSuite
{
    public class EventJournalPublisherTest
    {
        [Fact]
        public async void TestEventJournalPublisher()
        {
            EventJournal eventJournal = EventJournal.Open("test-ej");
            MessageBus messageBus = MessageBus.Start("test-bus");
            Topic topic = messageBus.OpenTopic("test-topic");
            EventJournalPublisher journalPublisher = EventJournalPublisher.From(eventJournal.Name, messageBus.Name, topic.Name);
            EventJournalPublisherTestSubscriber subscriber = new EventJournalPublisherTestSubscriber();
            topic.Subscribe(subscriber);

            EventBatch batch1 = new EventBatch();
            for (int idx = 0; idx < 3; ++idx)
                batch1.AddEntry("test1type", "test1instance" + idx);
            eventJournal.Write("test1", 0, batch1);

            EventBatch batch2 = new EventBatch();
            for (int idx = 0; idx < 3; ++idx)
                batch2.AddEntry("test2type", "test2instance" + idx);
            eventJournal.Write("test2", 0, batch2);

            await journalPublisher.CloseAsync();
            await topic.CloseAsync();

            Assert.Equal(6, subscriber.handledMessages.Count);
        }
    }
}