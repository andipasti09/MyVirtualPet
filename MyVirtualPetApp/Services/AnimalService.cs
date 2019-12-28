using Microsoft.Extensions.Logging;
using MyVirtualPet.Controllers;
using MyVirtualPet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVirtualPet.Services
{
    /// <summary>
    /// Implementation of <c>IAnimalService</c>. Serves as a service layer between the REST controller and the database.
    /// </summary>
    public class AnimalService : IAnimalService
    {
        readonly ILogger<AnimalService> logger;
        readonly IDatabaseService databaseService;
        readonly IUserService userService;

        public AnimalService(IDatabaseService databaseService, IUserService userService, ILogger<AnimalService> logger)
        {
            this.databaseService = databaseService;
            this.userService = userService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new animal for a user based on the <c>AnimalRequest</c>.
        /// The enum value given there for the type of animal determines what kind of animal class is created
        /// </summary>
        /// <param name="animalRequest">the request that contains which type of animal and for which user.</param>
        /// <returns>the new <c>Animal</c> entity</returns>
        public Animal AddAnimal(AnimalRequest animalRequest)
        {
            assertUserExists(animalRequest);

            //int type = animalRequest.Type;
            //TODO: better with string instead of enum numbers?
            Animal.AnimalType animalType = animalRequest.Type; //(Animal.AnimalType)Enum.ToObject(typeof(Animal.AnimalType), type);

            Animal newPet;
            switch (animalType)
            {
                case Animal.AnimalType.Cat:
                    newPet = new Cat();
                    break;
                case Animal.AnimalType.Dog:
                    newPet = new Dog();
                    break;
                case Animal.AnimalType.Parrot:
                    newPet = new Parrot();
                    break;
                default:
                    throw new ArgumentException("Could not create specific animal type. Enum value is not handled.");
            }
            logger.LogInformation("Creating a new animal of type {0} for user {1}", new object[] { animalType.ToString(), animalRequest.UserId.ToString() });
            
            newPet.Name = animalRequest.Name;
            newPet.UserId = animalRequest.UserId;
            
            databaseService.AddAnimal(newPet);
            return newPet;
        }

        private void assertUserExists(AnimalRequest animalRequest)
        {
            if (null == userService.GetUser(animalRequest.UserId))
            {
                throw new ArgumentException("User with given userId does not exist");
            }
        }

        public List<Animal> GetAllAnimals()
        {
            return databaseService.GetAllAnimals();
        }

        public Animal GetAnimal(int animalId)
        {
            return databaseService.GetAnimal(animalId);
        }

        /// <summary>
        /// Gets only the animals of one user.
        /// With a real database this would be smarter to realize
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Animal> GetAnimalsOfUser(ulong userId)
        {
            List<Animal> animals = GetAllAnimals();

            return animals.FindAll(delegate (Animal animal) { return animal.UserId.Equals(userId); } );
        }

        public Animal UpdateAnimal(Animal animal)
        {
            return databaseService.UpdateAnimal(animal);
        }

        public Animal StrokeAnimal(int animalId)
        {
            Animal animal = databaseService.GetAnimal(animalId);
            if (animal == null)
                throw new ArgumentNullException(animalId.ToString(), "animal with id {0} not found");

            animal.IncreaseHappiness();
            return animal;
        }

        public Animal FeedAnimal(int animalId)
        {
            Animal animal = databaseService.GetAnimal(animalId);
            if (animal == null)
                throw new ArgumentNullException(animalId.ToString(), "animal with id {0} not found");
            
            animal.DecreaseHunger();
            return animal;
        }
    }
}
