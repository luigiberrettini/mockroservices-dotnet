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

namespace VaughnVernon.Mockroservices.Eventing
{
    public class EventStream
    {
        public string Snapshot { get; private set; }
        public List<EventValue> Stream { get; private set; }
        public string StreamName { get; private set; }
        public int StreamVersion { get; private set; }

        public bool HasSnapshot()
        {
            return Snapshot.Length > 0;
        }

        internal EventStream(string streamName, int streamVersion, List<EventValue> stream, string snapshot)
        {
            StreamName = streamName;
            StreamVersion = streamVersion;
            Stream = stream;
            Snapshot = snapshot;
        }
    }
}