﻿using Microsoft.AspNetCore.Mvc;
using NetCoreMongo.Web.Domain.Users;
using NetCoreMongo.Web.Domain.Users.Commands;
using NetCoreMongo.Web.Domain.Users.Repository;

namespace NetCoreMongo.Web.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        #region Fields

        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructors

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IActionResult Get()
        {
            // Get from repository

            var usersList = _userRepository.List();

            // Return 'Ok' with the userList

            return Ok(usersList);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateUser createUser)
        {
            // Mapping

            var user = new User(createUser.Name, createUser.ProfessionId, createUser.CountryId);

            foreach (var email in createUser.Emails)
            {
                user.AddEmail(email.Address);
            }

            // Create in repository

            _userRepository.CreateUser(user);

            // Return Created

            return Created($"api/users/{user.Id}", user);
        }

        [HttpGet("{userId}")]
        public IActionResult Get(string userId)
        {
            // Get from repository

            var user = _userRepository.GetById(userId);

            // Return 'Not Found' if null

            if (user == null)
            {
                return NotFound();
            }

            // Return 'Ok' with the user

            return Ok(user);
        }

        #endregion
    }
}