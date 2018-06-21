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

using System.Collections.Generic;
using VaughnVernon.Mockroservices.Eventing;

namespace VaughnVernon.Mockroservices.Domain
{
    public abstract class EventSourcedRootEntity
    {
        public List<DomainEvent> MutatingEvents { get; }

        public int MutatedVersion => UnmutatedVersion + 1;

        public int UnmutatedVersion { get; }

        protected EventSourcedRootEntity()
        {
            MutatingEvents = new List<DomainEvent>();
            UnmutatedVersion = 0;
        }

        protected EventSourcedRootEntity(List<DomainEvent> stream, int streamVersion) : this()
        {
            foreach (DomainEvent domainEvent in stream)
                DispatchWhen(domainEvent);
            UnmutatedVersion = streamVersion;
        }

        protected void Apply(DomainEvent domainEvent)
        {
            MutatingEvents.Add(domainEvent);
            DispatchWhen(domainEvent);
        }

        protected void DispatchWhen(DomainEvent domainEvent)
        {
            ((dynamic)this).When((dynamic)domainEvent);
        }
    }
}