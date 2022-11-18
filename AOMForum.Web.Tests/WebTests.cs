using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace AOMForum.Web.Tests
{
    //public class WebTests : IClassFixture<WebApplicationFactory<Program>>
    //{
    //    private readonly WebApplicationFactory<Program> server;

    //    public WebTests(WebApplicationFactory<Program> server)
    //    {
    //        this.server = server;
    //    }

    //    [Fact]
    //    public async Task IndexPageShouldReturnStatusCode200WithTitle()
    //    {
    //        HttpClient client = this.server.CreateClient();
    //        HttpResponseMessage response = await client.GetAsync("/");

    //        response.EnsureSuccessStatusCode();
    //        string responseContent = await response.Content.ReadAsStringAsync();

    //        Assert.Contains("<title>", responseContent);
    //    }

    //    [Fact]
    //    public async Task AccountManagePageRequiresAuthorization()
    //    {
    //        HttpClient client = this.server.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    //        HttpResponseMessage response = await client.GetAsync("Identity/Account/Manage");

    //        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    //    }
    //}
}