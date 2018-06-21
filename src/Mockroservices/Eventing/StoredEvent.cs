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

namespace VaughnVernon.Mockroservices.Eventing
{
    public class StoredEvent
    {
        public static long NO_ID = -1L;

        public EventValue EventValue { get; private set; }
        public long Id { get; private set; }

        public bool IsValid()
        {
            return Id != NO_ID;
        }

        public override int GetHashCode()
        {
            return EventValue.GetHashCode() + (int)Id;
        }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(StoredEvent))
            {
                return false;
            }

            StoredEvent otherStoredEvent = (StoredEvent)other;

            return Id == otherStoredEvent.Id && EventValue.Equals(otherStoredEvent.EventValue);
        }

        public override string ToString()
        {
            return "StoredEvent[id=" + Id + " eventValue=" + EventValue + "]";
        }

        internal StoredEvent(long id, EventValue eventValue)
        {
            Id = id;
            EventValue = eventValue;
        }
    }
}