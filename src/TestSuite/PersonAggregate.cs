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
using VaughnVernon.Mockroservices.Domain;
using VaughnVernon.Mockroservices.Eventing;

namespace VaughnVernon.Mockroservices.TestSuite
{
    public class PersonAggregate : EventSourcedRootEntity
    {
        public PersonAggregate(string name, int age)
        {
            Apply(new PersonNamed(Guid.NewGuid().ToString(), name, age));
        }

        public PersonAggregate(List<DomainEvent> stream, int streamVersion)
            : base(stream, streamVersion)
        {
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }

        public void When(PersonNamed personNamed)
        {
            Id = personNamed.Id;
            Name = personNamed.Name;
            Age = personNamed.Age;
        }

        public override bool Equals(object other)
        {
            if (other != null && other.GetType() != typeof(PersonAggregate))
            {
                return false;
            }

            PersonAggregate otherPerson = (PersonAggregate)other;

            return Id.Equals(otherPerson.Id) && Name.Equals(otherPerson.Name) && Age == otherPerson.Age;
        }

        public override string ToString()
        {
            return "Person [Id=" + Id + " Name=" + Name + " Age=" + Age + "]";
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Name, Age).GetHashCode();
        }
    }
}