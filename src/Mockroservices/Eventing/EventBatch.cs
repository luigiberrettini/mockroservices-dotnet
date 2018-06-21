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
using System.Collections.Generic;
using VaughnVernon.Mockroservices.Extensions;

namespace VaughnVernon.Mockroservices.Eventing
{
    public class EventBatch
    {
        public List<BatchEntry> Entries { get; private set; }

        public static EventBatch Of(String type, String body)
        {
            return new EventBatch(type, body);
        }

        public static EventBatch Of(String type, String body, String snapshot)
        {
            return new EventBatch(type, body, snapshot);
        }

        public EventBatch()
            : this(2)
        {
        }

        public EventBatch(int entries)
        {
            Entries = new List<BatchEntry>(entries);
        }

        public EventBatch(String type, String body)
            : this(type, body, "")
        {
        }

        public EventBatch(String type, String body, String snapshot)
            : this()
        {
            AddEntry(type, body, snapshot);
        }

        public void AddEntry(String type, String body)
        {
            Entries.Add(new BatchEntry(type, body));
        }

        public void AddEntry(String type, String body, String snapshot)
        {
            Entries.Add(new BatchEntry(type, body, snapshot));
        }

        protected EventBatch ToBatch(List<DomainEvent> domainEvents)
        {
            EventBatch batch = new EventBatch(domainEvents.Count);
            foreach (DomainEvent domainEvent in domainEvents)
            {
                string eventBody = domainEvent.Serialize();
                batch.AddEntry(domainEvent.GetType().AssemblyQualifiedName, eventBody);
            }
            return batch;
        }

        protected List<DomainEvent> ToEvents(List<EventValue> stream)
        {
            List<DomainEvent> eventStream = new List<DomainEvent>(stream.Count);
            foreach (EventValue value in stream)
            {
                Type eventType = Type.GetType(value.Type);
                DomainEvent domainEvent = (DomainEvent)value.Body.Deserialize(eventType);
                eventStream.Add(domainEvent);
            }
            return eventStream;
        }
    }
}