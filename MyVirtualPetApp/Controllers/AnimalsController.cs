using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyVirtualPet.Models;
using MyVirtualPet.Services;

namespace MyVirtualPet.Controllers
{
    /// <summary>
    /// A REST controller to handle requests for Animals Resources.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService animalService;

        public AnimalsController(IAnimalService animalService)
        {
            this.animalService = animalService;
        }

        // GET: api/Animals?userId=id
        /// <summary>
        /// Gets all pets registered by a user
        /// </summary>
        /// <param name="userId">the user id as query parameter</param>
        /// <returns>a list of animals</returns>
        /// <response code="400">when the user id is not set in the request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Animal>> Get([FromQuery] ulong userId)
        {
            if (userId == 0)
            {
                return BadRequest("no user id given");
            }
            
            return animalService.GetAnimalsOfUser(userId);
        }

        // GET: api/Animals/5
        /// <summary>
        /// Gets an animal from the app with the given id
        /// </summary>
        /// <param name="id">the id of the animal</param>
        /// <returns>the animal with the given id
        /// Example:
        /// {
        ///  "hunger": 0,
        ///  "happy": 0,
        ///  "id": 1,
        ///  "userId": 1,
        ///  "name": "Cat",
        ///  "animalTypus": "Cat"
        /// }
        /// </returns>
        /// <response code="200">when the animal was found</response>
        /// <response code="404">when no animal with the given id was found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Animal> Get(int id)
        {
            Animal animal = animalService.GetAnimal(id);
            if (animal == null)
            {
                return NotFound();
            }
            return animal;
        }

        // POST: api/Animals
        /// <summary>
        /// Creates a virtual animal in the app for a user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST /Animals
        /// {
        ///  "type": 0,
        ///  "name": "Cat",
        ///  "userId": 1
        /// }
        /// </remarks>
        /// <param name="animalRequest"></param>
        /// <returns>a newly created Animal</returns>
        /// <response code="201">when the animal could be created</response>
        /// <response code="400">when the animal could not be created because of wrong user input</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Animal> Post([FromBody] AnimalRequest animalRequest)
        {
            try
            {
                Animal animal = animalService.AddAnimal(animalRequest);
                return CreatedAtAction(nameof(Get), new { id = animal.ID }, animal);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // Patch: api/Animals/feed/5
        /// <summary>
        /// Calling this patch method feeds the animal, decreasing the hunger index 
        /// </summary>
        /// <param name="id">the animal id</param>
        /// <returns>the updated animal</returns>
        /// <response code="200">when the action was successful</response>
        /// <response code="404">when the animal with the given id could not be found</response>
        [HttpPatch("/feed/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Animal> Feed(int id)
        {
            Animal animal;
            try
            {
                animal = animalService.FeedAnimal(id);
            } catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }

            return animal;           
        }

        // Patch: api/Animals/stroke/5
        /// <summary>
        /// This patch method strokes the animal and increases the happy index
        /// </summary>
        /// <param name="id">the animal id</param>
        /// <returns>the updated animal</returns>
        /// <response code="200">when the action was successful</response>
        /// <response code="404">when the animal with the given id could not be found</response>
        [HttpPatch("/stroke/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Animal> Stroke(int id)
        {
            Animal animal;
            try
            {
                animal = animalService.StrokeAnimal(id);
            } catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            return animal;
        }
    }
}
