using Azure.Core;
using ClassMate.Domain.Abstractions.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ClassMate.API.Controllers
{
    [Route("api/sync")]
    [ApiController]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly IGoogleService _googleService;

        public SyncController(IGoogleService googleService)
        {
            _googleService = googleService;
        }

        [HttpGet("google-auth-url")]
        public ActionResult GetAuthUrl()
        {
            return Ok(_googleService.GetAuthUrl());
        }

        [HttpGet("google-auth-response")]
        public async Task<ActionResult> HandleCallback([FromQuery] string code)
        {
            return Ok(await _googleService.Callback(code));
        }

    }
}
