using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace НаучнаяРабота_пробная_версия
{
    class ProductManufacturer
    {

        public ProductDict AvailableProducts { get; set; }

        public ProductManufacturer(ProductDict availableProducts)
        {
            AvailableProducts = availableProducts;
        }

        public bool AvailableAtLeast(Product product, int amount)
        {
            return AvailableProducts.ContainsAtLeast(product, amount);
        }

        public bool CanBeManufactured(Product product, int amount = 1)
        {
            ProductDict remainingProducts = AvailableProducts.Copy();
            return CanBeManufactured(ref remainingProducts, product, amount);
        }

        private bool CanBeManufactured(ref ProductDict remainingProducts, Product product, int amount = 1)
        {

            //если продукт является базовым, по его нельзя произвести
            if (product.Components is null)
            {
                return false;
            }

            foreach (var c in product.Components)
            {
                int required = c.Value * amount;
                
                //если доступно достаточное количество
                if (remainingProducts.ContainsAtLeast(c.Key, required))
                {
                    //то просто используем нужное количество
                    remainingProducts = remainingProducts.Without(c.Key, required);
                }
                else
                {
                    //иначе узнаем, сколько доступно; вычитаем это кол-во из необходимого
                    int available = remainingProducts.ContainsKey(c.Key) ? remainingProducts[c.Key] : 0;
                    required -= available;
                    //пояснение: метод Without возвращает копию изначального словаря, если тот не содержит поданный ключ
                    remainingProducts = remainingProducts.Without(c.Key, available);
                    if (!CanBeManufactured(ref remainingProducts, c.Key, required))
                        return false;
                }
            }

            return true;
        }

        public int MaxCanBeManufactured(Product product)
        {
            int i = 0;
            while (CanBeManufactured(product, i))
                i += 2;

            //если продукт является базовым, то CanBeManufactured всегда возвращает false
            //т.е. продукт произвести нельзя
            if (i == 0)
                return 0;

            return CanBeManufactured(product, i-1) ? i-1 : i - 2;
        }

        public bool CanBeOrdered(Product product, int amount = 1)
        {
            if (AvailableAtLeast(product, amount))
            {
                return true;
            }
            else
            {
                return CanBeManufactured(product, amount -
                    (AvailableProducts.ContainsKey(product) ? AvailableProducts[product] : 0));
            }
        }

        public int MaxCanBeOrdered(Product product)
        {
            int i = 0;
            while (CanBeOrdered(product, i))
                i += 2;

            return CanBeOrdered(product, i - 1) ? i - 1 : i - 2;
        }

    }
}
