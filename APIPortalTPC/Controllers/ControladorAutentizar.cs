using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorAutentizar :ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioAutentizar RA;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RA">Interface de RepositorioUsuario</param>

        public ControladorAutentizar(IRepositorioAutentizar RA)
        {
            this.RA = RA;
        }
        /// <summary>
        ///  Recibe una clase que contiene el correo electrónico y la contraseña para validar su existencia
        /// </summary>
        /// <param name="postrq">Objeto que guarda el correo y contraseña a comprobar>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> ValidarCorreo(PostRq postrq)
        {
            Usuario User = await RA.ValidarCorreo(postrq.correo, postrq.pass);

            if (User.Id_Usuario == 0)
            {
                return NotFound("Clave o contraseña incorrecta");
            }
            return User;
        }
    }
}
