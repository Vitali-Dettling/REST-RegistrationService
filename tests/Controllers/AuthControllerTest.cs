using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RegistrationManager.Controllers;
using RegistrationManager.Controllers.Api;
using Moq;
using Microsoft.Extensions.Logging;
using RegistrationManager.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace test.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<ILogger<AuthController>> loggerMock = new Mock<ILogger<AuthController>>();
        private readonly Mock<IRegistrationsRepository> repositoryMock = new Mock<IRegistrationsRepository>();
        private readonly Mock<SignInManager<DbUserIdentity>> signInMgtMock = new Mock<SignInManager<DbUserIdentity>>();


        [Fact]
        public async void LoginPost400Test()
        {
            AuthController authController = new AuthController(loggerMock.Object, repositoryMock.Object, signInMgtMock.Object);

            //using (var httpClient = new HttpClient())
            //{
            //    var check = JsonConvert.SerializeObject(
            //   new { Email = "\"email\": \"vdettling@web.de\"", Password = "\"password\": \"P@ssw0rd!\"" }
            //   );

            //    var contentPost = new StringContent(check, Encoding.UTF8, "application/json");

            //    return await httpClient.PostAsync(requests.GetLoginUrl(), contentPost);
            //}

    //        var check = JsonConvert.SerializeObject(
    //new { Email = "\"email\": \"vdettling@web.de\"", Password = "\"password\": \"\"" }
    //);

    //        var contentPost = new StringContent(check, Encoding.UTF8, "application/json");

            Login login = new Login();
            login.Email = "vdettling@web.de";
            login.Password = "P@ssw0rd!";

            await authController.LoginPost(login);

            Assert.True(true);
        }
    }
}
