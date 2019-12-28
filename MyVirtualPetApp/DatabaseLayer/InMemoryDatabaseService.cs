using Microsoft.Extensions.Logging;
using MyVirtualPet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVirtualPet.Services
{
    /// <summary>
    /// This class serves as a in-memory storage for all models, instead of connecting to a real database.
    /// If the service is shut down, no entries are saved.
    /// 
    /// Implements the interface <c>IDatabaseService</c> to be swapped easily with a different database layer.
    /// Should be used as singleton in whole application.
    /// </summary>
    public class InMemoryDatabaseService : IDatabaseService
    {
        private ILogger logger;

        private Dictionary<ulong, User> userDictonary = new Dictionary<ulong, User>();
        private Dictionary<int, Animal> animalDictonary = new Dictionary<int, Animal>();

        public InMemoryDatabaseService(ILogger<InMemoryDatabaseService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Adds a user to the "database". The id is generated incrementally like a database would do it
        /// </summary>
        /// <param name="user">the <c>User</c> model</param>
        /// <returns>the saved <c>User</c> model with updated id.</returns>
        public User AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User must not be null");

            ulong maxId = 0;
            if (userDictonary.Count != 0)
            {
                maxId = userDictonary.Keys.Max();
            }           
            maxId += 1;
            user.ID = maxId;

            if (!userDictonary.TryAdd(maxId, user))
            {
                throw new InvalidOperationException("User with this id exists already");
            }
            return user;
        }

        public User GetUser(ulong id)
        {
            if (userDictonary.TryGetValue(id, out User foundUser))
                logger.LogDebug("user found with id " + id);

            return foundUser;
        }

        public List<User> GetAllUsers()
        {
            return userDictonary.Values.ToList<User>();
        }

        public Animal AddAnimal(Animal animal)
        {
            if (animal == null)
                throw new ArgumentNullException("Animal must not be null");

            int maxId = 0;
            if (animalDictonary.Keys.Count != 0)
            {
                maxId = animalDictonary.Keys.Max();
            }
            maxId += 1;
            animal.ID = maxId;

            animalDictonary.Add(maxId, animal);
            return animal;
        }

        public Animal GetAnimal(int id)
        {
            if (animalDictonary.TryGetValue(id, out Animal foundAnimal))
                logger.LogDebug("animal found with id {0}", id);

            return foundAnimal;
        }

        public List<Animal> GetAllAnimals()
        {
            return animalDictonary.Values.ToList<Animal>();
        }

        public Animal UpdateAnimal(Animal animal)
        {
            // do nothing as reference contains already the updated values. In a real database you would save the entity.
            return animal;
        }
    }
}
