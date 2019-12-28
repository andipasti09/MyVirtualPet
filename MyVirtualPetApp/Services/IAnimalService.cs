using MyVirtualPet.Controllers;
using MyVirtualPet.Models;
using System.Collections.Generic;

namespace MyVirtualPet.Services
{
    // Interface for a service that handles creation and actions on animals
    public interface IAnimalService
    {
        /// <summary>
        /// Adds an animal to the database
        /// </summary>
        /// <param name="animalRequest">the <c>animalRequest</c> object that holds the parameters</param>
        /// <returns>the newly created animals</returns>
        Animal AddAnimal(AnimalRequest animalRequest);

        /// <summary>
        /// Gets a list of all saved animals.
        /// </summary>
        /// <returns>a <list type="Animal">the animal list</list> </returns>
        List<Animal> GetAllAnimals();

        /// <summary>
        /// Gets an animal for the given id
        /// </summary>
        /// <param name="animalId">the internal id of the animal</param>
        Animal GetAnimal(int animalId);

        /// <summary>
        /// Gets all <c>Animals</c> of a <c>User</c>
        /// </summary>
        /// <param name="userId">the internal user id</param>
        List<Animal> GetAnimalsOfUser(ulong userId);

        /// <summary>
        /// Stroke an animal to make it more happier
        /// </summary>
        /// <param name="animalId">animal's id</param>
        /// <returns>The updated animal</returns>
        Animal StrokeAnimal(int animalId);

        /// <summary>
        /// Feed the animal to make it less hungry
        /// </summary>
        /// <param name="animalId">animal's id</param>
        /// <returns>The updated animal</returns>
        Animal FeedAnimal(int animalId);

        /// <summary>
        /// Updates an animal entity, e.g. when parameters were calculated.
        /// </summary>
        /// <param name="animal">the <c>Animal</c> entity</param>
        /// <returns></returns>
        Animal UpdateAnimal(Animal animal);
    }
}
