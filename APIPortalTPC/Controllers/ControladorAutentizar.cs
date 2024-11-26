using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorAutentizar : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioAutentizar RA;
        private readonly IRepositorioUsuario RU;
  
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RA">Interface de RepositorioUsuario</param>
        public ControladorAutentizar(IRepositorioAutentizar RA, IRepositorioUsuario RU)
        {
            this.RA = RA;
            this.RU = RU;
        }
        /// <summary>
        ///  Recibe una clase que contiene el correo electrónico y la contraseña para validar su existencia, tambien añade la clave MFA para
        ///  poder ser usada para confirmar
        /// </summary>
        /// <param name="postrq">Objeto que guarda el correo y contraseña a comprobar>
        /// <returns>User es el usuario confirmado </returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> ValidarCorreo(PostRq postrq)
        {
            Usuario User = await RA.ValidarCorreo(postrq.correo, postrq.pass);

            if (User.Id_Usuario == 0)
            {
                return NotFound("Clave o contraseña incorrecta");
            }

            ///int codigo = await RA.MFA(User.Correo_Usuario);
            ///User.CodigoMFA = codigo;
            ///await RU.ModificarUsuario(User);
            return User;

        }
        [HttpPost("MFA")]

        /// <summary>
        /// metodo que se encarga de comprobar su la contraseña ingresada es valida, para eso necesita la id del Usuario que
        /// se quiere comprobar
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ActionResult<Usuario>> MFA1(MFA mfa)
        {
            int id = mfa.Id_Usuario;
            Usuario U = await RU.GetUsuario(id);
            if(U.CodigoMFA== mfa.mfa)
            {
                U.CodigoMFA = 0;
                //RU.ModificarUsuario(U);
                return U;
            }
                
            else
            {
                return NotFound("Codigo Incorrecto");
            }
        }

        [HttpPost("pass")]
        //metodo para verificacion dos pasos
        public async Task<IActionResult> RecuperarContraseña()
        {
            //Logica: entregas el correo,
            //retorna el usuario y mandas un correo,
            //luego mandas un correo para confirmar y ahi se hace el cambio de contraseña
            throw new NotImplementedException();
        }
    }
}
