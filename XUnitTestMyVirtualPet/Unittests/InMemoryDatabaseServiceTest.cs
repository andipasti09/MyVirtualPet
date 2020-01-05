using System;
using Xunit;
using MyVirtualPet.Services;
using Microsoft.Extensions.Logging;
using Moq;
using MyVirtualPet.Models;
using System.Collections.Generic;

namespace XUnitTestMyVirtualPet
{
    public class InMemoryDatabaseServiceTest
    {
        InMemoryDatabaseService underTest;

        public InMemoryDatabaseServiceTest() {
            var logger = new Mock<ILogger<InMemoryDatabaseService>>();

            underTest = new InMemoryDatabaseService(logger.Object);
        }

        [Fact]
        public void TestGetUser()
        {
            User user = underTest.GetUser(123);
            Assert.Null(user);

            user = underTest.AddUser(CreateUser("Name"));
            Assert.NotNull(user);
        }

        [Fact]
        public void TestAddingOneUser()
        {
            User user = underTest.AddUser(CreateUser("Name"));

            Assert.NotNull(user);
            Assert.Equal((ulong)1, user.ID);
        }

        [Fact]
        public void TestAddingMultipleUsers()
        {
            underTest.AddUser(CreateUser("User1"));
            underTest.AddUser(CreateUser("User2"));
            underTest.AddUser(CreateUser("User3"));

            List<User> list = underTest.GetAllUsers();
            Assert.Equal(3, list.Count);
        }

        private static User CreateUser(string name)
        {
            User user = new User();
            user.AccountName = name;
            return user;
        }

        [Fact]
        public void TestAddingOneAnimal()
        {
            Cat cat = new Cat();
            Animal animal = underTest.AddAnimal(cat);

            Assert.NotNull(animal);
            Assert.True(animal.ID > 0);

            Animal ret = underTest.GetAnimal(animal.ID);
            Assert.Equal(animal, ret);
        }

        [Fact]
        public void TestingAddingNullAnimal()
        {
            Assert.Throws<ArgumentNullException>(() => underTest.AddAnimal(null));
        }

        [Fact]
        public void TestGettingAllAnimals()
        {
            underTest.AddAnimal(CreateTestAnimal(1));
            underTest.AddAnimal(CreateTestAnimal(1));
            underTest.AddAnimal(CreateTestAnimal(2));

            Assert.Equal(3, underTest.GetAllAnimals().Count);
        }

        private Animal CreateTestAnimal(ulong userId)
        {
            Dog dog = new Dog();
            dog.UserId = userId;
            return dog;
        }
    }
}
