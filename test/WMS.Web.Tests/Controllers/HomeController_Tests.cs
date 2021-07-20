using System.Threading.Tasks;
using WMS.Models.TokenAuth;
using WMS.Web.Controllers;
using Shouldly;
using Xunit;

namespace WMS.Web.Tests.Controllers
{
    public class HomeController_Tests: WMSWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}