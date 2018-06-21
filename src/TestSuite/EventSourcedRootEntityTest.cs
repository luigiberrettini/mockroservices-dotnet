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

using Xunit;

namespace VaughnVernon.Mockroservices.TestSuite
{
    public class EventSourcedRootEntityTest
    {
        [Fact]
        public void TestProductDefinedEventKept()
        {
            Product product = new Product("dice-fuz-1", "Fuzzy dice.", 999);
            Assert.Single(product.MutatingEvents);
            Assert.Equal("dice-fuz-1", product.Name);
            Assert.Equal("Fuzzy dice.", product.Description);
            Assert.Equal(999, product.Price);
            Assert.Equal(new ProductDefined("dice-fuz-1", "Fuzzy dice.", 999), product.MutatingEvents[0]);
        }

        [Fact]
        public void TestProductNameChangedEventKept()
        {
            Product product = new Product("dice-fuz-1", "Fuzzy dice.", 999);

            product.MutatingEvents.Clear();

            product.ChangeName("dice-fuzzy-1");
            Assert.Single(product.MutatingEvents);
            Assert.Equal("dice-fuzzy-1", product.Name);
            Assert.Equal(new ProductNameChanged("dice-fuzzy-1"), product.MutatingEvents[0]);
        }

        [Fact]
        public void TestProductDescriptionChangedEventsKept()
        {
            Product product = new Product("dice-fuz-1", "Fuzzy dice.", 999);

            product.MutatingEvents.Clear();

            product.ChangeDescription("Fuzzy dice, and all.");
            Assert.Single(product.MutatingEvents);
            Assert.Equal("Fuzzy dice, and all.", product.Description);
            Assert.Equal(new ProductDescriptionChanged("Fuzzy dice, and all."), product.MutatingEvents[0]);
        }

        [Fact]
        public void TestProductPriceChangedEventKept()
        {
            Product product = new Product("dice-fuz-1", "Fuzzy dice.", 999);

            product.MutatingEvents.Clear();

            product.ChangePrice(995);
            Assert.Single(product.MutatingEvents);
            Assert.Equal(995, product.Price);
            Assert.Equal(new ProductPriceChanged(995), product.MutatingEvents[0]);
        }

        [Fact]
        public void TestReconstitution()
        {
            Product product = new Product("dice-fuz-1", "Fuzzy dice.", 999);
            product.ChangeName("dice-fuzzy-1");
            product.ChangeDescription("Fuzzy dice, and all.");
            product.ChangePrice(995);

            Product productAgain = new Product(product.MutatingEvents, product.MutatedVersion);
            Assert.Equal(product, productAgain);
        }
    }
}