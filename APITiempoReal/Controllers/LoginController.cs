using APITiempoReal.Models;
using System;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace APITiempoReal.Controllers
{
    /// <summary>
    /// Controlador para gestionar los inicios de sesión
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //TODO: La autenticación se está haciendo por ahora con la hora UTC del servidor como password, sin validar usuario.
            string pass = DateTimeOffset.UtcNow.ToString("HHmm");            
            
            if (login.Password.Equals(pass))
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Username);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
