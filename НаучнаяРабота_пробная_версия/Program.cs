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
