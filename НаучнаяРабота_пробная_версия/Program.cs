using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace НаучнаяРабота_пробная_версия
{
    
    class Program
    {
        static void TestCase()
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


        static void DemoCase()
        {
            Product ironOre = new Product("iron ore", 10, null);
            Product ironIngot = new Product("iron ingot", 30, new ProductDict { { ironOre, 2 } });
            Product ironRod = new Product("iron rod", 50, new ProductDict { { ironIngot, 1 } });
            Product hammer = new Product("hammer", 110, new ProductDict { { ironIngot, 1 }, { ironRod, 1 } });

            var existingProducts = new List<Product> { ironOre, ironIngot, ironRod, hammer };

            ProductDict availableProducts = new ProductDict {
                { ironOre, 10 },
                { ironIngot, 3 },
                { ironRod, 1 }
            };

            ProductManufacturer manufacturer = new ProductManufacturer(availableProducts);

            //Console.WriteLine(manufacturer.MaxCanBeManufactured(ironOre));
            //Console.WriteLine(manufacturer.MaxCanBeManufactured(ironIngot));
            //Console.WriteLine(manufacturer.MaxCanBeManufactured(ironRod));
            //Console.WriteLine(manufacturer.MaxCanBeManufactured(hammer));

            UseWebsiteToSolve(existingProducts, availableProducts);

        }

        static void UseWebsiteToSolve(List<Product> existingProducts, ProductDict availableProducts)
        {
            int n = existingProducts.Count;

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://matworld.ru/calculator/simplex-method-online.php");

            ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementsByClassName('mycontleft')[0].style.width = '100%';");

            driver.FindElement(By.Id("sel_1")).SendKeys($"{2*n}"); //m field
            driver.FindElement(By.Id("sel_2")).SendKeys($"{2*n}"); //n field

            //F(x) fields
            for (int i = n; i < 2*n; i++)
            {
                driver.FindElement(By.Id($"inp_A_0_{i}")).SendKeys(existingProducts[i - n].Price.ToString());
            }

            var selects = driver.FindElements(By.CssSelector("td div.iil select")); //все select 
            new SelectElement(selects[0]).SelectByText("max"); // выбор min/max 

            //A
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!(existingProducts[j].Components is null) && existingProducts[j].Components.ContainsKey(existingProducts[i]))
                        driver.FindElement(By.Id($"inp_A_{i+1}_{j}")).SendKeys($"{existingProducts[j].Components[existingProducts[i]]}");

                    if (i == j)
                    {
                        driver.FindElement(By.Id($"inp_A_{i + 1}_{j}")).SendKeys($"{-1}");
                        driver.FindElement(By.Id($"inp_A_{i + 1}_{n + j}")).SendKeys($"{1}");
                        driver.FindElement(By.Id($"inp_A_{n + i + 1}_{j}")).SendKeys($"{1}"); //нижние n неравнеств
                        new SelectElement(selects[i+1]).SelectByText("="); //знаки в первых n ограничениях
                    }
                }

                if (availableProducts.ContainsKey(existingProducts[i]))
                    driver.FindElement(By.Id($"inp_A_{i + 1}_{2 * n}")).SendKeys($"{availableProducts[existingProducts[i]]}"); //после знака равно

                driver.FindElement(By.Id($"inp_A_{n + i + 1}_{2 * n}")).
                    SendKeys($"{new ProductManufacturer(availableProducts).MaxCanBeManufactured(existingProducts[i])}");
            }

            driver.FindElement(By.CssSelector("[value='Вычислить']")).Click();
        }

        static void Main(string[] args)
        {
            DemoCase();
        }
    }
}
