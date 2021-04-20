using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Followers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reactivities.API.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command { TargetUsername = username }));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowings(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new FollowList.Query { Username = username, Predicate = predicate }));
        }
    }
}
