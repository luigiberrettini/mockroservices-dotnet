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
    public class Product : EventSourcedRootEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public long Price { get; private set; }

        public Product(string name, string description, long price)
        {
            Apply(new ProductDefined(name, description, price));
        }

        public Product(List<DomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        public void ChangeDescription(string description)
        {
            Apply(new ProductDescriptionChanged(description));
        }

        public void ChangeName(string name)
        {
            Apply(new ProductNameChanged(name));
        }

        public void ChangePrice(long price)
        {
            Apply(new ProductPriceChanged(price));
        }

        public override bool Equals(Object other)
        {
            if (other == null || other.GetType() != typeof(Product))
            {
                return false;
            }

            Product otherProduct = (Product)other;

            return Name.Equals(otherProduct.Name) &&
                Description.Equals(otherProduct.Description) &&
                Price == otherProduct.Price;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public void When(ProductDefined e)
        {
            Name = e.Name;
            Description = e.Description;
            Price = e.Price;
        }

        public void When(ProductDescriptionChanged e)
        {
            Description = e.Description;
        }

        public void When(ProductNameChanged e)
        {
            Name = e.Name;
        }

        public void When(ProductPriceChanged e)
        {
            Price = e.Price;
        }
    }
}