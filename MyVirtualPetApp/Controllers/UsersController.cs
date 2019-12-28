using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyVirtualPet.Models;
using MyVirtualPet.Services;

namespace MyVirtualPet.Controllers
{
    /// <summary>
    /// REST controller for handling the "user" resource.
    /// It uses the UserService by dependency injection.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserService userService;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        // GET api/users
        /// <summary>
        /// Gets all users from the app
        /// </summary>
        /// <returns>a list of users</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<User>> Get()
        {
            return userService.GetUsers();
        }

        // GET api/users/userid
        /// <summary>
        /// Gets a specific user
        /// </summary>
        /// <param name="id">the id of the user given by the create request</param>
        /// <returns>the user entity</returns>
        /// <response code="200">if user was found</response>
        /// <response code="404">if user was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> Get(ulong id)
        {
            User user = userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // POST api/users
        /// <summary>
        /// Adds a new user to the app.
        /// </summary>
        /// <remarks>Sample:
        /// {
        /// "accountName": "andy"
        /// }
        ///</remarks>
        /// <param name="user">the user entity with the desired name</param>
        /// <returns>the newly created user</returns>
        /// <response code="201">when user could be created. The location header points to the path of the new resource</response>
        /// <response code="400">when user creation failed</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User definition in http body must not be null");
            }

            logger.LogInformation("Going to create user with name: " + user.ID);

            try
            {
                User newUser = userService.CreateUser(user);
                return CreatedAtAction(nameof(Get), new { id = newUser.ID }, newUser);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}