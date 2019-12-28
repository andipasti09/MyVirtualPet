using Microsoft.Extensions.Logging;
using MyVirtualPet.Models;
using System;
using System.Collections.Generic;

namespace MyVirtualPet.Services
{
    /// <summary>
    /// The UserService manages all user related features, like e.g. creating a new user.
    /// </summary>
    public class UserService : IUserService
    {
        private ILogger<IUserService> logger;

        private readonly IDatabaseService databaseService;

        public UserService(IDatabaseService databaseService, ILogger<IUserService> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
        }

        public User GetUser(ulong id)
        {
            return databaseService.GetUser(id);
        }

        public List<User> GetUsers()
        {
            return databaseService.GetAllUsers();
        }

        public User CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.AccountName))
            {
                throw new ArgumentException("account name must not be null for new user");
            }
            // check if user with the name already exists
            User alreadyNamedUser = databaseService.GetAllUsers().Find(us => us.AccountName.Equals(user.AccountName));
            if (alreadyNamedUser != null)
            {
                throw new ArgumentException("user with the same account name already exists.");
            }

            User newUser = databaseService.AddUser(user);
            if (newUser == null)
                throw new ApplicationException("user could not be created");

            logger.LogInformation("new user with account {0} was added.", user.AccountName);
            return newUser;
        }
    }
}
