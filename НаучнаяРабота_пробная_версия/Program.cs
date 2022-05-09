using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace НаучнаяРабота_пробная_версия
{
    
    class Program
    {
        static void Case1()
        {
            /*            Material ironOre = new Material("iron ore", 200);
                Material copperOre = new Material("copper ore", 300);

                Product ironIngot = new Product("iron ingot", 1000, new List<ProductPart> { new ProductPart( ironOre, 10) });
                Product copperIngot = new Product("copper ingot", 1000, new List<ProductPart> { new ProductPart(copperOre, 10) });
                Product wire = new Product("wire", 10000, new List<ProductPart>() { new ProductPart(ironIngot, 2), new ProductPart(copperIngot, 1) });

                Console.WriteLine(ironOre);
                Console.WriteLine(copperOre);
                Console.WriteLine(ironIngot);
                Console.WriteLine(copperIngot);
                Console.WriteLine(wire);

                InitialComponents avaiable = new InitialComponents(new List<ProductPart> { new ProductPart(ironOre, 10000), new ProductPart(copperOre, 5000), new ProductPart(wire, 100) });
                Console.WriteLine("availavle: " + avaiable);

                JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(avaiable, jsonOptions);
                Console.WriteLine(jsonString);

                InitialComponents deserialized =
                    JsonSerializer.Deserialize<InitialComponents>(jsonString);
                Console.WriteLine("deserialized: " + deserialized);

                jsonString = JsonSerializer.Serialize(deserialized, jsonOptions);
                Console.WriteLine(jsonString);*/

            Product wood = new Product("wood", 100, null);
            Product ironOre = new Product("iron ore", 200, null);
            Product copperOre = new Product("copper ore", 300, null);

            Product ironIngot = new Product("iron ingot", 1000, new ProductDict { { ironOre, 10 } });
            Product copperIngot = new Product("copper ingot", 1000, new ProductDict { { copperOre, 10 } });
            Product wire = new Product("wire", 10000, new ProductDict
                { {ironIngot, 2 }, {copperIngot, 1 } });
            Product ironShovel = new Product("iron showel", 20000, new ProductDict
                { {ironIngot, 10 }, { wood, 100 } });

            List<Product> availavleProductTypes = new List<Product>
                { wood, ironOre, copperOre, ironIngot, copperIngot, wire, ironShovel };

            Console.WriteLine("Availavle product types:\n " + string.Join("\n ", availavleProductTypes));

            Product weirdProduct = new Product("weird product", 404, new ProductDict
            {
                { wood, 5 },
                { ironIngot, 10 },
                { wire, 5 },
                { ironShovel, 2 }
            });

            Product plank = new Product("plank", 200, new ProductDict { { wood, 10 } });
            Product stick = new Product("stick", 500, new ProductDict { { plank, 2 } });

            ProductDict initialList = new ProductDict {
                { ironOre, 10000 },
                { copperOre, 5000 },
                { wood, 1000 },
                { plank, 20 },
                { stick, 10 }
            };

            Console.WriteLine("Initial list of products:\n" + initialList);

            ProductManufacturer manufacturer = new ProductManufacturer(initialList);

            Console.WriteLine(manufacturer.MaxCanBeManufactured(stick));


            /*            manufacturer.Manufacture(wire, 500);
                        Console.WriteLine("Current list of products:\n" + initialList);*/
        }


        static void Case2()
        {
            Product ironOre = new Product("iron ore", 10, null);
            Product ironIngot = new Product("iron ingot", 30, new ProductDict { { ironOre, 2 } });
            Product ironRod = new Product("iron rod", 50, new ProductDict { { ironIngot, 1 } });
            Product awl = new Product("awl", 100, new ProductDict { { ironIngot, 1 }, { ironRod, 1 } });

            ProductDict initialList = new ProductDict {
                { ironOre, 10 },
                { ironIngot, 3 },
                { ironRod, 1 }
            };

            ProductManufacturer manufacturer = new ProductManufacturer(initialList);

            Console.WriteLine(manufacturer.MaxCanBeManufactured(ironOre));
            Console.WriteLine(manufacturer.MaxCanBeManufactured(ironIngot));
            Console.WriteLine(manufacturer.MaxCanBeManufactured(ironRod));
            Console.WriteLine(manufacturer.MaxCanBeManufactured(awl));

        }

        static void Main(string[] args)
        {
            Case2();
        }
    }
}
