using Microservice.Web.Frontend.Models.Links;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace End_To_End_Test_FrontEnd;

public class Frontend_E2E_Test : IDisposable
{
    private readonly IWebDriver webDriver;
    public Frontend_E2E_Test()
    {
        webDriver = new ChromeDriver();
    }

    [Fact]
    public void Check_HomePage_Title()
    {

        //act
        webDriver.Navigate().GoToUrl(LinkServices.FrontEndUser);
        //assert 
        Assert.Equal("Home Page - فرانت سایه بان", webDriver.Title);
    }

    [Fact]
    public void Check_HomePage_Text()
    {
        webDriver.Navigate()
         .GoToUrl($"{LinkServices.FrontEndUser}Home/Index");
        Assert.Contains("Welcome", webDriver.PageSource);
    }

    [Fact]
    public void Check_ProductList_Text()
    {
        webDriver.Navigate()
         .GoToUrl($"{LinkServices.FrontEndUser}Product");
        Assert.Contains("محصولات", webDriver.PageSource);
    }

    [Fact]
    public void Check_ProductList_Data()
    {
        webDriver.Navigate()
        .GoToUrl($"{LinkServices.FrontEndUser}Product");

        IWebElement elementTable = webDriver.FindElement(By.XPath("//div[@class='container']//table[1]"));
        List<IWebElement> listTR = new List<IWebElement>(elementTable.FindElements(By.TagName("tr")));
        Assert.NotEqual(1, listTR.Count);
    }

    [Fact]
    public void Check_Add_Product_To_Basket()
    {
        webDriver.Navigate()
         .GoToUrl($"{LinkServices.FrontEndUser}Product");

        webDriver.FindElement(By.Id("addtobasket")).Click();

        Assert.Equal($"{LinkServices.FrontEndUser}basket", webDriver.Url.ToLower());

    }

    [Fact]
    public void Check_Empty_Discount_Code_In_Basket()
    {
        webDriver.Navigate()
      .GoToUrl($"{LinkServices.FrontEndUser}basket");

        var txtDiscountCode = webDriver.FindElement(By.Id("txtDiscountCode"));
        txtDiscountCode.Clear();


        var btnApplyDiscountCode = webDriver.FindElement(By.Id("btnApplyDiscountCode"));
        btnApplyDiscountCode.Click();
        Thread.Sleep(1000);
        string allerttitle = webDriver.FindElement(By.XPath("//div[@class='swal-title']")).Text;
        Assert.Equal("هشدار!", allerttitle);

    }


    [Fact]
    public void Check_Apply_Invalid_Discount_Code_In_Basket()
    {
        webDriver.Navigate()
         .GoToUrl($"{LinkServices.FrontEndUser}basket");

        var txtDicounCode = webDriver.FindElement(By.Id("txtDiscountCode"));
        txtDicounCode.Clear();
        txtDicounCode.SendKeys("off");

        var btnApplyDiscountCode = webDriver.FindElement(By.Id("btnApplyDiscountCode"));

        btnApplyDiscountCode.Click();
        Thread.Sleep(1500);
        string allertmessage = webDriver.FindElement(By.XPath("//div[@class='swal-text']")).Text;
        Assert.Equal("این کد تخفیف قبلا استفاده شده است", allertmessage);
    }

    public void Dispose()
    {
        webDriver.Quit();
        webDriver.Dispose();
    }
}