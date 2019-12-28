using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using MyVirtualPet.Controllers;
using MyVirtualPet.Models;
using MyVirtualPet.Services;
using Xunit;

namespace XUnitTestMyVirtualPet
{
    public class AnimalServiceTest
    {
        AnimalService underTest;

        Mock<IUserService> userServiceMock;

        Mock<IDatabaseService> databaseServiceMock;

        public AnimalServiceTest()
        {
            var logger = new Mock<ILogger<AnimalService>>();
            databaseServiceMock = new Mock<IDatabaseService>();
            userServiceMock = new Mock<IUserService>();
            underTest = new AnimalService(databaseServiceMock.Object, userServiceMock.Object, logger.Object);

        }

        [Fact]
        public void TestAddAnimal()
        {
            User testUser = new User();
            testUser.AccountName = "Andy";
            testUser.ID = 1234;

            AnimalRequest request = new AnimalRequest();
            request.Name = "Bello";
            request.Type = Animal.AnimalType.Dog;
            request.UserId = testUser.ID;

            userServiceMock.Setup(us => us.GetUser(1234)).Returns(testUser);

            Animal newAnimal = underTest.AddAnimal(request);

            Assert.NotNull(newAnimal);
            Assert.Equal(Animal.AnimalType.Dog.ToString(), newAnimal.AnimalTypus);
            Assert.Equal("Bello", newAnimal.Name);
            Assert.Equal(testUser.ID, newAnimal.UserId);
        }

        [Fact]
        public void TestAddAnimalWithInvalidUser()
        {
            AnimalRequest request = new AnimalRequest();
            request.UserId = 1234;
            userServiceMock.Setup(us => us.GetUser(1234)).Returns<User>(null);

            Assert.Throws<ArgumentException>(() => underTest.AddAnimal(request));
        }

        [Fact]
        public void TestGetAllAnimals()
        {
            List<Animal> allPets = new List<Animal>();
            allPets.Add(CreateTestAnimal(1));
            allPets.Add(CreateTestAnimal(1));
            allPets.Add(CreateTestAnimal(2));
            allPets.Add(CreateTestAnimal(2));
            allPets.Add(CreateTestAnimal(2));

            databaseServiceMock.Setup(dbs => dbs.GetAllAnimals()).Returns(allPets);

            Assert.Equal(5, underTest.GetAllAnimals().Count);
        }

        [Fact]
        public void TestGettingAllAnimalsOfUser()
        {
            List<Animal> allPets = new List<Animal>();
            allPets.Add(CreateTestAnimal(1));
            allPets.Add(CreateTestAnimal(1));
            allPets.Add(CreateTestAnimal(2));
            allPets.Add(CreateTestAnimal(2));
            allPets.Add(CreateTestAnimal(2));

            databaseServiceMock.Setup(dbs => dbs.GetAllAnimals()).Returns(allPets);

            Assert.Equal(2, underTest.GetAnimalsOfUser(1).Count);
            Assert.Equal(3, underTest.GetAnimalsOfUser(2).Count);
        }

        [Fact]
        public void TestStrokeAnimal()
        {
            Animal cat = new Cat();
            cat.ID = 3;
            Assert.Equal(0, cat.Happy);

            databaseServiceMock.Setup(dbs => dbs.GetAnimal(3)).Returns(cat);

            Animal animal = underTest.StrokeAnimal(3);

            databaseServiceMock.Verify(dbs => dbs.GetAnimal(3));

            Assert.True(animal.Happy > 0);
        }

        [Fact]
        public void TestFeedAnimal()
        {
            Animal cat = new Cat();
            cat.ID = 3;
            Assert.Equal(0, cat.Hunger);

            databaseServiceMock.Setup(dbs => dbs.GetAnimal(3)).Returns(cat);

            Animal animal = underTest.FeedAnimal(3);

            databaseServiceMock.Verify(dbs => dbs.GetAnimal(3));

            Assert.True(animal.Hunger < 0);
        }

        private Animal CreateTestAnimal(ulong userId)
        {
            Animal anim = new Parrot();
            anim.UserId = userId;
            return anim;
        }
    }
}
