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

using VaughnVernon.Mockroservices.Eventing;

namespace VaughnVernon.Mockroservices.TestSuite
{
    public class ProductDefined : DomainEvent
    {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public long Price { get; private set; }

        public ProductDefined(string name, string description, long price)
            : base()
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(ProductDefined))
            {
                return false;
            }

            ProductDefined otherProductDefined = (ProductDefined)other;

            return Name.Equals(otherProductDefined.Name) &&
                Description.Equals(otherProductDefined.Description) &&
                Price == otherProductDefined.Price &&
                EventVersion == otherProductDefined.EventVersion;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Description.GetHashCode() + (int)Price;
        }
    }
}