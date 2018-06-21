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
    public abstract class EventSourceRepository
    {
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