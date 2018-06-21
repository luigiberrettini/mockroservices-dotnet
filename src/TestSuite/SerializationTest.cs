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
using Xunit;
using VaughnVernon.Mockroservices.Extensions;

namespace VaughnVernon.Mockroservices.TestSuite
{
    public class SerializationTest
    {
        [Fact]
        public void TestSerializationDeserialization()
        {
            Person person1 = new Person("First Middle Last, Jr.", DateTime.Now);
            Assert.NotNull(person1);
            string jsonPerson1 = person1.Serialize();
            Person person2 = jsonPerson1.Deserialize<Person>();
            Assert.NotNull(person2);
            Assert.Equal(person1, person2);
            string jsonPerson2 = person2.Serialize();
            Assert.Equal(jsonPerson1, jsonPerson2);
        }

        [Fact]
        public void TestDeserializationToClientClass()
        {
            Person person = new Person("First Middle Last, Jr.", DateTime.Now);
            string jsonPerson = person.Serialize();
            ClientPerson clientPerson = jsonPerson.Deserialize<ClientPerson>();
            Assert.Equal(person.Name, clientPerson.Name);
            Assert.Equal(person.BirthDate, clientPerson.BirthDate);
        }

        [Fact]
        public void TestDeserializationAgainstPrivateSetter()
        {
            Person person = new Person("First Middle Last, Jr.", DateTime.Now);
            string jsonPerson = person.Serialize();
            var clientPerson = jsonPerson.Deserialize<ClientPersonWithPrivateSetter>();
            Assert.Equal(person.Name, clientPerson.Name);
            Assert.Equal(person.BirthDate, clientPerson.BirthDate);
        }
    }
}