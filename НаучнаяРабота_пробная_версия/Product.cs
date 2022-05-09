using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace НаучнаяРабота_пробная_версия
{

    public class Product
    {
        public string Name { get; protected set; }
        public int Price { get; protected set; }

        public virtual ProductDict Components { get; protected set; }

        public Product(string name, int price, ProductDict components)
        {
            Name = name;
            Price = price;
            Components = components;
        }

        public override string ToString()
        {
            return $"{Name} : {Price}$ " + Components ?? "";
        }

    }

}
