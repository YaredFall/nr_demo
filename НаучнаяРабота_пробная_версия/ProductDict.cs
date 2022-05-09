using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace НаучнаяРабота_пробная_версия
{
    public class ProductDict : Dictionary<Product, int>
    {
        public ProductDict Copy()
        {
            ProductDict newDict = new ProductDict();
            foreach (var item in this)
            {
                newDict.Add(item.Key, item.Value);
            }

            return newDict;
        }

        public ProductDict Without(Product product, int amount)
        {
            if (this.ContainsAtLeast(product, amount))
            {
                ProductDict newDict = this.Copy();
                newDict[product] -= amount;
                if (newDict[product] == 0)
                    newDict.Remove(product);
                return newDict;
            }
            else
            {
                return this;
            }
        }

        public bool ContainsAtLeast(Product product, int amount)
        {
            return this.ContainsKey(product) && this[product] >= amount;
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", this.Select(pair => pair.Key + " - " + pair.Value).ToArray())}]";
        }

    }
}
