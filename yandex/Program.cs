using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Collections.Generic;

namespace yandex
{
    class Program
    {

        class Yandex
        {
            By elect;
            By market;
            By searthBut;
            By price;
            private By appButton;
            private By FirstElem;
            private By SearthBut;
            private By NameList;

            public int t { set; get; }

            public Yandex()
            {
                market = By.XPath("//html/body/div[1]/div[3]/div/div[2]/div/div[2]/div/div[1]/div/a[2]");//гиперссылка на "маркет" // понимаю что XPath далеко не самый лучший вариант, прим малейшем изменении формы работать не будет. Но по ID не находит.
                elect = By.XPath("//html/body/div[1]/div[2]/noindex/ul/li[1]/a");//гиперссылка на раздел "Электроника"
                searthBut = By.ClassName("black");//кнопка "Поиск"
                appButton = By.ClassName("filter-panel-aside__apply");//конпка "Применить"
                FirstElem = By.XPath("//html/body/div[1]/div[4]/div[2]/div[1]/div[1]/div/div[1]/div[3]/div/div[1]/div/h3/a/span");//Первый элемент в списке
                SearthBut = By.XPath("//html/body/div/div[1]/noindex/div[1]/div/div[2]/div/div[2]/div/form/span/button");//Кнопка поиска
                NameList = By.XPath("//html/body/div[1]/div[3]/div[2]/div[1]/div/div/h1");//Элемент с названием товара.
                price = By.Id("gf-pricefrom-var");

                t = 3000; // time after Click() milliseconds
            }
            public bool test(By electType, string priceDown, List<By> setList)
            {
                bool result = true;
                using (var driver = new ChromeDriver())
                {
                    IWindow win = driver.Manage().Window;
                    win.Maximize();
                    driver.Navigate().GoToUrl("http://yandex.ru");
                    var mark = driver.FindElement(market);
                    mark.Click();
                    Thread.Sleep(t);
                    var el = driver.FindElement(elect);
                    el.Click();
                    el.Click();
                    Thread.Sleep(t);
                    var tel = driver.FindElement(electType);
                    tel.Click();
                    Thread.Sleep(t);

                    var searBut = driver.FindElement(searthBut);
                    searBut.Click();
                    Thread.Sleep(t);

                    var pric = driver.FindElement(price);
                    pric.SendKeys(priceDown);

                    foreach (var p in setList)
                    {
                        driver.FindElement(p).Click();
                    }
                    var appBut = driver.FindElement(appButton);
                    appBut.Click();
                    Thread.Sleep(t);
                    var list = driver.FindElementsByClassName("snippet-card__content");
                    if (list.Count == 10)
                    {
                        System.Console.WriteLine("Кол-во верно.");
                    }
                    else
                    {
                        System.Console.WriteLine("Кол-во неверно.");
                        result = false;
                    }
                    var first = driver.FindElement(FirstElem);
                    string val = first.Text;
                    var searth = driver.FindElementById("header-search");// поле поиска
                    searth.SendKeys(val);
                    var sBut = driver.FindElement(SearthBut);
                    sBut.Click();
                    Thread.Sleep(t);
                    var res = driver.FindElement(NameList);
                    if (res.Text == val) System.Console.WriteLine("Верно.");
                    else System.Console.WriteLine("Неверно.");
                }

                return result;
            }
        }
        static void Main(string[] args)
        {
            By naushniki = By.XPath("//html/body/div/div[4]/div[1]/div/div[4]/div/a[1]");
            By tv = By.XPath("//html/body/div/div[4]/div[1]/div/div[3]/div/a[1]");
            By sam = By.Id("gf-1801946-1871447");
            By lg = By.Id("gf-1801946-1871499");
            By beats = By.Id("gf-1801946-8455647");
            List<By> list = new List<By>();
            list.Add(sam);
            list.Add(lg);
            Yandex ya = new Yandex();
            bool firstTest = ya.test(tv, "20000", list);
            if (firstTest) System.Console.WriteLine("Первый тест пройден успешно.");
            else System.Console.WriteLine("Первый тест не пройден.");
            list.Clear();
            list.Add(beats);
            bool secondTest = ya.test(naushniki, "5000", list);
            if (secondTest) System.Console.WriteLine("Второй тест пройден успешно.");
            else System.Console.WriteLine("Второй тест не пройден.");

            System.Console.ReadKey();
        }
    }
}