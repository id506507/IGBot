using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace HelloWorld
{
    public class Program
    {
        static void Main(string[] args)
        {
            IWebDriver webDriver = ConnectChromeDriver();
            FollowNLike(webDriver);
        }

        private static void FollowNLike(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            Login(driver);

            Console.WriteLine("Please input ig username(s) [Use ',' for separate]");
            string usernames = Console.ReadLine();
            usernames = usernames.Replace('，', ',');
            List<string> usernameList = usernames.Split(',').ToList();

            usernameList.ForEach(user =>
            {
                Console.WriteLine($"{user}: \n\n");
                #region search and follow
                Thread.Sleep(3000);
                IWebElement searchElement = driver.FindElement(By.ClassName("_aauy"));
                searchElement.SendKeys(user);
                Thread.Sleep(2000);
                searchElement.SendKeys(Keys.Enter);
                Thread.Sleep(1000);
                searchElement.SendKeys(Keys.Enter);

                Thread.Sleep(2000);
                try
                {
                    IWebElement followBtnElement = driver.FindElement(By.XPath("//div[text()='追蹤' or text()='Follow' or text()='追蹤對方']"));
                    followBtnElement.Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Search and follow error: {e}");
                }
                #endregion


                #region like post
                try
                {
                    Thread.Sleep(500);
                    IWebElement postCountElement = driver.FindElement(By.ClassName("_ac2a"));
                    int post = int.Parse(postCountElement.Text);
                    if (post > 10)
                    {
                        post = 10;
                    }

                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    driver.FindElement(By.XPath("//div[@class='_aabd _aa8k _aanf']")).Click();


                    ClickLike(driver);

                    for (int i = 1; i < post; i++)
                    {
                        Thread.Sleep(500);
                        driver.FindElement(By.XPath("//div[@class=' _aaqg _aaqh']")).Click();
                        ClickLike(driver);
                    }

                    driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"like error: {e}");
                }
                

                #endregion
            });



            Console.WriteLine("Finished.");
        }

        private static void ClickLike(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
            IWebElement likeBtn = driver.FindElement(By.ClassName("_aamw"));
            likeBtn.Click();
        }

        private static IWebDriver ConnectChromeDriver()
        {
            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;
            string path = $"{projectDirectory}/Resources";

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--incognito");
            IWebDriver driver = new ChromeDriver(path, options);

            return driver;
        }
        private static void Login(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl("https://www.instagram.com/");

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            IWebElement usernameElement = driver.FindElement(By.Name("username"));
            IWebElement passwordElement = driver.FindElement(By.Name("password"));
            IWebElement submitBtnElement = driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div/div[3]/button"));

            Console.WriteLine("Please input username.");
            string username = Console.ReadLine();
            usernameElement.SendKeys(username);

            Console.WriteLine("Please input password");
            string password = Console.ReadLine();
            passwordElement.SendKeys(password);
            
            submitBtnElement.Click();
            
        }
    }
}