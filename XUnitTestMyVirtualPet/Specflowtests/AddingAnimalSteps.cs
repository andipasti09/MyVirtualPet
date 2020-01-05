using MyVirtualPet.Controllers;
using System;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Logging;
using MyVirtualPet.Services;
using MyVirtualPet.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace XUnitTestMyVirtualPet.Specflowtests
{
    [Binding]
    public class AddingAnimalSteps
    {
        private AnimalsController animalsController;

        private UsersController userController;

        private User createdUser;

        private AnimalRequest request;

        private ActionResult<Animal> result;

        private int requestedAnimalType;

        /// <summary>
        /// Setting up all class instances. A better way would be to let the framework do the dependency injection.
        /// </summary>
        public AddingAnimalSteps()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            ILogger<InMemoryDatabaseService> logger = loggerFactory.CreateLogger<InMemoryDatabaseService>();
            IDatabaseService databaseService = new InMemoryDatabaseService(loggerFactory.CreateLogger<InMemoryDatabaseService>());
            IUserService userService = new UserService(databaseService, loggerFactory.CreateLogger<UserService>());
            userController = new UsersController(userService, loggerFactory.CreateLogger<UsersController>());
            IAnimalService animalService = new AnimalService(databaseService, userService, loggerFactory.CreateLogger<AnimalService>());
            animalsController = new AnimalsController(animalService);
        }

        [Given(@"I have a valid user named ""(.*)""")]
        public void GivenIHaveAValidUserNamed(string userName)
        {
            User user = new User();
            user.AccountName = userName;

            ActionResult<User> result = userController.CreateUser(user);

            CreatedAtActionResult r = (CreatedAtActionResult)result.Result;
            createdUser = (User)r.Value;
        }
        
        [Given(@"I have put a request with type (.*)")]
        public void GivenIHavePutARequestWithType(int animalType)
        {
            request = new AnimalRequest();
            request.Type = (Animal.AnimalType)animalType;
            request.UserId = createdUser.ID;
            request.Name = "Garfield";

            requestedAnimalType = animalType;
        }
        
        [When(@"I press POST animal")]
        public void WhenIPressPOSTAnimal()
        {
            result = animalsController.Post(request);
        }
        
        [Then(@"the result should be an animal of type cat")]
        public void ThenTheResultShouldBeAnAnimalOfTypeCat()
        {
            CreatedAtActionResult r = (CreatedAtActionResult)result.Result;
            Animal animal = (Animal)r.Value;
            Assert.Equal(((Animal.AnimalType)requestedAnimalType).ToString(), animal.AnimalTypus);
            Assert.Equal(createdUser.ID, animal.UserId);

        }
    }
}
