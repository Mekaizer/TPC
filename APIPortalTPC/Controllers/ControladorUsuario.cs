using APIPortalTPC.Repositorio;
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
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RU">Interface de RepositorioUsuario</param>

        public ControladorUsuario(IRepositorioUsuario RU)
        {
            this.RU = RU;
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
                if (resultado.Id_Usuario== 0)
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

                int rut = 0;
                if (U.Rut_Usuario_Sin_Digito.HasValue)
                {
                    rut = U.Rut_Usuario_Sin_Digito.Value;

                }
                string res = await RU.Existe(rut, U.Correo_Usuario);
                if (res == "ok")
                {
                    Usuario nuevo = await RU.NuevoUsuario(U);
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

                return await RU.ModificarUsuario(U);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
  

    }
}