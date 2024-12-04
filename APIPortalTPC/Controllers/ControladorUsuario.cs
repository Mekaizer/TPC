using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorUsuario : ControllerBase
    {
        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioUsuario RU;
        private readonly InterfaceEnviarCorreo IEC;
        private readonly IRepositorioDepartamentoUsuario IRDU;
        private readonly InterfaceCrearExcel ICE;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RU">Interface de RepositorioUsuario</param>

        public ControladorUsuario(IRepositorioDepartamentoUsuario IRDU, IRepositorioUsuario RU, InterfaceEnviarCorreo IEC, InterfaceCrearExcel ICE)
        {
            this.IEC = IEC;
            this.RU = RU;
            this.ICE = ICE;
            this.IRDU = IRDU;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos del tipo Usuario de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RU.GetAllUsuario());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los Usuarios: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el objeto Usuario cuyo Id coincida con el buscado</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RU.GetUsuario(id);
                if (resultado.Id_Usuario == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el usuario");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Usuario: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="U">Objeto del tipo Usuario que se quiere agregar a la base de datos</param>
        /// <returns>Retorna el objeto Usuario agregado</returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> Nuevo(Usuario U)
        {

            try
            {
                if (U == null)
                    return BadRequest();

                string rut = U.Rut_Usuario;
                string res = await RU.Existe(rut, U.Correo_Usuario);
                if (res == "ok")
                {
                    Usuario nuevo = await RU.NuevoUsuario(U);
                    DepartamentoUsuario DU = new DepartamentoUsuario();
                    DU.Id_Departamento = U.Id_Departamento;
                    DU.Id_Usuario = U.Id_Usuario;
                    await IRDU.Nuevo(DU);
                    nuevo = await RU.ActivarUsuario(nuevo);
                    await RU.ModificarUsuario(nuevo);
                    await IEC.CorreoUsuarioPass(nuevo);
                    return nuevo;
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }


        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID, contiene un metodo para asegurarse que no exista el objeto
        /// </summary>
        /// <param name="U">Objeto del tipo Usuario que se reemplazará por su homonimo</param>
        /// <param name="id">Id del objeto Usuario a modifcar</param>
        /// <returns>Retorna el nuevo objeto Usuario</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Usuario>> Modificar(Usuario U, int id)
        {
            if (U.Id_Usuario != id)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Id no coincide");
            }
            try
            {
                var Modificar = await RU.GetUsuario(id);

                if (Modificar == null)
                    return NotFound($"Usuario = {id} no encontrado");

                return Ok(await RU.ModificarUsuario(U));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }

        }
        [HttpGet("Imprimir")]
        public async Task<ActionResult> GetExcel()
        {

            var lista = await RU.GetAllUsuario();
            Console.WriteLine(lista);
            // Assuming DescargarExcel returns a byte array and a filename
            return Ok(await ICE.DescargarExcel((List<Usuario>)lista));
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Usuario>> Eliminar(int id)
        {
            try
            {
                var u = RU.GetUsuario(id);
                if (u == null)
                {
                    return NotFound("No se encontro el Usuario");
                }
                return Ok(await RU.EliminarUsuario(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
    

    }
}