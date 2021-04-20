using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Profiles;
using Reactivities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reactivities.API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfilesController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditProfile(Edit.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new ListActivities.Query { Username = username, Predicate = predicate }));
        }
    }
}
