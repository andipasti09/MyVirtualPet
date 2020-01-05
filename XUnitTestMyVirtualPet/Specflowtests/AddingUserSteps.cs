using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyVirtualPet.Controllers;
using MyVirtualPet.Models;
using MyVirtualPet.Services;
using TechTalk.SpecFlow;
using Xunit;

namespace XUnitTestMyVirtualPet
{
    [Binding]
    public class AddingUserSteps
    {
        private UsersController userController;

        private User userRequest = new User();

        ActionResult<User> result;

        /// <summary>
        /// Preparing Test Data
        /// </summary>
        public AddingUserSteps()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            ILogger<InMemoryDatabaseService> logger = loggerFactory.CreateLogger<InMemoryDatabaseService>();
            IDatabaseService databaseService = new InMemoryDatabaseService(loggerFactory.CreateLogger<InMemoryDatabaseService>());
            IUserService userService = new UserService(databaseService, loggerFactory.CreateLogger<UserService>());
            userController = new UsersController(userService, loggerFactory.CreateLogger<UsersController>());
        }

        [Given(@"I have entered ""(.*)"" as username")]
        public void GivenIHaveEnteredAsUsername(string givenName)
        {
            userRequest.AccountName = givenName;
        }
        
        [When(@"I POST the user")]
        public void WhenIPOSTTheUser()
        {
            result = userController.CreateUser(userRequest);
        }
        
        [Then(@"the result should be a new user with this name")]
        public void ThenTheResultShouldBeANewUserWithThisName()
        {
            CreatedAtActionResult r = (CreatedAtActionResult)result.Result;
            User createdUser = (User) r.Value;
            Assert.Equal(userRequest.AccountName, createdUser.AccountName);
            Assert.True(createdUser.ID > 0);
        }
    }
}
