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

namespace VaughnVernon.Mockroservices.Eventing
{
    public class EventJournalReader
    {
        private EventJournal eventJournal;
        private int readSequence;

        public void Acknowledge(long id)
        {
            if (id == readSequence)
            {
                ++readSequence;
            }
            else if (id > readSequence)
            {
                throw new InvalidOperationException("The id is out of range.");
            }
            else if (id < readSequence)
            {
                // allow for this case; don't throw IllegalArgumentException("The id has already been acknowledged.");
            }
        }

        public string Name { get; private set; }

        public StoredEvent ReadNext()
        {
            if (readSequence <= eventJournal.GreatestId)
            {
                return new StoredEvent(readSequence, eventJournal.EventValueAt(readSequence));
            }

            return new StoredEvent(StoredEvent.NO_ID, new EventValue("", EventValue.NO_STREAM_VERSION, "", "", ""));
        }

        public void Reset()
        {
            readSequence = 0;
        }

        internal EventJournalReader(string name, EventJournal eventJournal)
        {
            Name = name;
            this.eventJournal = eventJournal;
            readSequence = 0;
        }
    }
}