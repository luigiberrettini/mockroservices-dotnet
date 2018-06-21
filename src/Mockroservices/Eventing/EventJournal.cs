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

namespace VaughnVernon.Mockroservices.Eventing
{
    public class EventJournal
    {
        private static ConcurrentDictionary<string, EventJournal> eventJournals = new ConcurrentDictionary<string, EventJournal>();

        private Dictionary<string, EventJournalReader> readers;
        private List<EventValue> store;

        public static EventJournal Open(string name)
        {
            if (eventJournals.ContainsKey(name))
                return eventJournals[name];

            EventJournal eventJournal = new EventJournal(name);
            eventJournals.TryAdd(name, eventJournal);
            return eventJournal;
        }

        public void Close()
        {
            store.Clear();
            readers.Clear();
            eventJournals.TryRemove(Name, out EventJournal val);
        }

        public string Name { get; private set; }

        public EventJournalReader Reader(string name)
        {
            if (readers.ContainsKey(name))
            {
                return readers[name];
            }

            EventJournalReader reader = new EventJournalReader(name, this);

            readers.Add(name, reader);

            return reader;
        }

        public EventStreamReader StreamReader()
        {
            return new EventStreamReader(this);
        }

        public void Write(EventBatch batch)
        {
            lock (store)
            {
                foreach (BatchEntry entry in batch.Entries)
                {
                    store.Add(new EventValue("", 0, entry.Type, entry.Body, ""));
                }
            }
        }

        public void Write(String streamName, int streamVersion, EventBatch batch)
        {
            lock (store)
            {
                foreach (BatchEntry entry in batch.Entries)
                {
                    store.Add(new EventValue(streamName, streamVersion, entry.Type, entry.Body, entry.Snapshot));
                }
            }
        }

        protected EventJournal(string name)
        {
            Name = name;
            readers = new Dictionary<string, EventJournalReader>();
            store = new List<EventValue>();
        }

        internal EventValue EventValueAt(int id)
        {
            lock (store)
            {
                if (id >= store.Count)
                {
                    throw new InvalidOperationException("The id does not exist: " + id);
                }

                return store[id];
            }
        }

        internal int GreatestId { get { return store.Count - 1; } }

        internal EventStream ReadStream(string streamName)
        {
            lock (store)
            {
                List<EventValue> values = new List<EventValue>();
                List<EventValue> storeCopy = new List<EventValue>(store);
                EventValue latestSnapshotValue = null;

                foreach (EventValue value in storeCopy)
                {
                    if (value.StreamName.Equals(streamName))
                    {
                        if (value.HasSnapshot())
                        {
                            values.Clear();
                            latestSnapshotValue = value;
                        }
                        else
                        {
                            values.Add(value);
                        }
                    }
                }

                int snapshotVersion = latestSnapshotValue == null ? 0 : latestSnapshotValue.StreamVersion;
                int streamVersion = values.Count == 0 ? snapshotVersion : values[values.Count - 1].StreamVersion;

                return new EventStream(streamName, streamVersion, values, latestSnapshotValue == null ? "" : latestSnapshotValue.Snapshot);
            }
        }
    }
}