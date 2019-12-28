using MyVirtualPet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVirtualPet.Services
{
    /// <summary>
    /// Interface for abstracting the underlaying database
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Gets a user with the given id
        /// </summary>
        /// <param name="id">the user id</param>
        /// <returns></returns>
        User GetUser(ulong id);

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns></returns>
        List<User> GetAllUsers();

        /// <summary>
        /// Adds a user to the database. It needs to handle the auto-incremention of the key.
        /// </summary>
        /// <param name="user">the user entity, probably without id</param>
        /// <returns>the newly created user</returns>
        User AddUser(User user);

        /// <summary>
        /// Saves an animal.
        /// </summary>
        /// <param name="animal">the animal entity</param>
        /// <returns></returns>
        Animal AddAnimal(Animal animal);

        /// <summary>
        /// Finds one specific animal
        /// </summary>
        /// <param name="id">the id of the animal</param>
        /// <returns></returns>
        Animal GetAnimal(int id);

        /// <summary>
        /// Gets all registered animals
        /// </summary>
        /// <returns>a list of type <c>Animal</c></returns>
        List<Animal> GetAllAnimals();

        /// <summary>
        /// Updates an animal entity
        /// </summary>
        /// <param name="animal">the new version of the <c>Animal</c></param>
        Animal UpdateAnimal(Animal animal);
    }
}
