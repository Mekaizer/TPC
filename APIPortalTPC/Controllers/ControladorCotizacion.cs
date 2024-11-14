using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft. AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorCotizacion : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioCotizacion RC;
        private readonly InterfaceCrearExcel ICE;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RC">Interface de RepositorioCotizacion</param>

        public ControladorCotizacion(IRepositorioCotizacion RC, InterfaceCrearExcel ICE)
        {
            this.RC = RC;
            this.ICE = ICE;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Imprimir")]
        public async Task<ActionResult> GetExcel()
        {

            var lista = await RC.GetAllCotizacion();

            // Assuming DescargarExcel returns a byte array and a filename
            return Ok(await ICE.DescargarExcel((List<Cotizacion>)lista));
        }

        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RC.GetAllCotizacion());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la cotizacion: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna e objeto de la Id a buscar</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado= await RC.GetCotizacion(id);
                if (resultado.ID_Cotizacion == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro la cotizacion");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la cotizacion: " + ex.Message+ id);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="c">Objeto tipo cotizacion que se quiere agregar a la base de datos</param>
        /// <returns>Se muestra el objeto agregado</returns>
        [HttpPost]
        public async Task<ActionResult<Cotizacion>> Nuevo(Cotizacion c)
        {
            try
            {
                if (c == null)
                    return BadRequest();

                Cotizacion nuevac = await RC.NuevaCotizacion(c);
                return nuevac;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID
        /// </summary>
        /// <param name="c">Objeto Cotizacion que tiene el mismo Id que el objeto existente en la base de datos</param>
        /// <param name="id">Id del objeto a cambiar</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Cotizacion>> Modificar(Cotizacion c, int id)
        {
            try
            {
                var Modificar = await RC.GetCotizacion(id);
                if (Modificar == null)
                    return NotFound($"Cotizacion con = {id} no encontrado");

                return await RC.ModificarCotizacion(c);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos"+id);
            }
        }
    }
}
