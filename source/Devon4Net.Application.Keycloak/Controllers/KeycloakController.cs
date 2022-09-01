using Devon4Net.Application.Keycloak.Implementation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeycloakPOC.Application.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeycloakController : ControllerBase
    {
        private KeycloakService _keycloakService { get; set; }

        public KeycloakController(KeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }

        // Public end-point for authentication
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string username, string password)
        {
            return Ok(await _keycloakService.GetToken(username, password).ConfigureAwait(false));
        }
    }
}