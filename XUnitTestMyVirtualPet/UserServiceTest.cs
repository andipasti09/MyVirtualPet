using Microsoft.Extensions.Logging;
using Moq;
using MyVirtualPet.Models;
using MyVirtualPet.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestMyVirtualPet
{
    public class UserServiceTest
    {
        UserService underTest;

        Mock<IDatabaseService> databaseService;

        public UserServiceTest()
        {
            databaseService = new Mock<IDatabaseService>();
            var logger = new Mock<ILogger<IUserService>>();
            underTest = new UserService(databaseService.Object, logger.Object);
        }

        [Fact]
        public void TestGetUser()
        {
            User user = new User();
            user.ID = 1;

            databaseService.Setup(dbs => dbs.GetUser(1)).Returns(user);

            Assert.Equal(user, underTest.GetUser(1));
            Assert.Null(underTest.GetUser(9999));
        }

        [Fact]
        public void TestCreatingUser()
        {
            User user = new User();
            user.AccountName = "any";

            databaseService.Setup(dbs => dbs.AddUser(user)).Returns(user);
            databaseService.Setup(dbs => dbs.GetAllUsers()).Returns(new List<User>());

            User ret = underTest.CreateUser(user);

            Assert.Equal("any", ret.AccountName);
            databaseService.Verify(dbs => dbs.AddUser(user));
        }

        [Fact]
        public void TestCreatingUserWithEmpyName()
        {
            User user = new User();

            Assert.Throws<ArgumentException>(() => underTest.CreateUser(user));
        }

        [Fact]
        public void TestCreateUserWithFalseDatabaseResponse()
        {
            User user = new User();
            user.AccountName = "name";

            databaseService.Setup(dbs => dbs.GetAllUsers()).Returns(new List<User>());
            // no better mocking of database here for "Null" return value;
            databaseService.Setup(dbs => dbs.AddUser(user)).Returns<User>(null);

            Assert.Throws<ApplicationException>(() => underTest.CreateUser(user));
        }

        [Fact]
        public void TestCreate2UsersWithSameName()
        {
            User user1 = new User();
            user1.AccountName = "andy0903";

            List<User> users = new List<User>();
            users.Add(user1);

            databaseService.SetupSequence(dbs => dbs.GetAllUsers()).Returns(new List<User>()).Returns(users);
            databaseService.Setup(dbs => dbs.AddUser(user1)).Returns(user1);

            // first successful call
            underTest.CreateUser(user1);

            // second user shall not be created
            User user2 = new User();
            user2.AccountName = "andy0903";

            Assert.Throws<ArgumentException>(() => underTest.CreateUser(user2));
        }
    }
}
