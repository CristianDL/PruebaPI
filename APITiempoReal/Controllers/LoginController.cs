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
        /// <summary>
        /// Sirve para comprobar que la API es accesible.
        /// </summary>
        /// <returns>True si la API es accesible. En caso contrario retornará error 404.</returns>
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }
        
        /// <summary>
        /// Sirve para comprobar si el usuario se encuentra autenticado.
        /// </summary>
        /// <returns>La identidad del usuario y si se encuentra autenticado.</returns>
        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        /// <summary>
        /// Autentica al usuario para el uso de las demás operaciones de la API.
        /// </summary>
        /// <param name="login">Información de usuario (nombre de usuario y contraseña).</param>
        /// <returns>Token de autenticación que se debe usar para consumir las operaciones de la API.</returns>
        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null || login.Username == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //TODO: La autenticación se está haciendo por ahora con la hora UTC del servidor como password, validando que el usuario no sea nulo.
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
