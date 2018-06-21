﻿//   Copyright © 2017 Vaughn Vernon. All rights reserved.
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

using Xunit;
using System;

namespace VaughnVernon.Mockroservices.TestSuite
{
    public class RepositoryTest
    {
        [Fact]
        public void TestSaveFind()
        {
            PersonRepository repository = new PersonRepository();
            PersonAggregate person = new PersonAggregate("Joe", 30);
            repository.Save(person);
            PersonAggregate joe = repository.PersonOfId(person.Id);
            Console.WriteLine("PERSON: " + person);
            Console.WriteLine("JOE: " + joe);
            Assert.Equal(person, joe);
        }
    }
}