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

namespace VaughnVernon.Mockroservices.TestSuite
{
    public class ClientPersonWithPrivateSetter
    {
        public String Name { get; private set; }
        public DateTime BirthDate { get; private set; }

        public ClientPersonWithPrivateSetter() { }

        public static ClientPersonWithPrivateSetter Instance(
            string name, DateTime birthDate)
        {
            return new ClientPersonWithPrivateSetter(
                name, birthDate);
        }
        private ClientPersonWithPrivateSetter(string name, DateTime birthDate)
        {
            Name = name;
            BirthDate = birthDate;
        }
    }
}