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
    public class ProductPriceChanged : DomainEvent
    {
        public long Price { get; private set; }

        public ProductPriceChanged(long price)
            : base()
        {
            Price = price;
        }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(ProductPriceChanged))
            {
                return false;
            }

            ProductPriceChanged otherProductPriceChanged = (ProductPriceChanged)other;

            return Price == otherProductPriceChanged.Price &&
                EventVersion == otherProductPriceChanged.EventVersion;
        }

        public override int GetHashCode()
        {
            return (int)Price;
        }
    }
}