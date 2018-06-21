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
    public class EventValue
    {
        public static int NO_STREAM_VERSION = -1;

        public string Body { get; private set; }
        public string Snapshot { get; private set; }
        public string StreamName { get; private set; }
        public int StreamVersion { get; private set; }
        public string Type { get; private set; }

        public bool HasSnapshot()
        {
            return Snapshot.Length > 0;
        }

        public override int GetHashCode()
        {
            return StreamName.GetHashCode() + StreamVersion;
        }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(EventValue))
            {
                return false;
            }

            EventValue otherEventValue = (EventValue)other;

            return StreamName.Equals(otherEventValue.StreamName) &&
                StreamVersion == otherEventValue.StreamVersion &&
                Type.Equals(otherEventValue.Type) &&
                Body.Equals(otherEventValue.Body) &&
                Snapshot.Equals(otherEventValue.Snapshot);
        }

        public override string ToString()
        {
            return "EventValue[streamName=" + StreamName + " StreamVersion=" + StreamVersion +
                " type=" + Type + " body=" + Body + " snapshot=" + Snapshot + "]";
        }

        internal EventValue(
            string streamName,
            int streamVersion,
            string type,
            string body,
            string snapshot)
        {

            StreamName = streamName;
            StreamVersion = streamVersion;
            Type = type;
            Body = body;
            Snapshot = snapshot;
        }
    }
}