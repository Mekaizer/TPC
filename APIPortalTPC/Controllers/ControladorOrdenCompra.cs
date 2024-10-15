using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorOrdenCompra : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioOrdenCompra ROC;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="ROC">Interface de RepositorioOrdenCompra</param>

        public ControladorOrdenCompra(IRepositorioOrdenCompra ROC)
        {
            this.ROC = ROC;
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
                return Ok(await ROC.GetAllOC());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la Orden de compra: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await ROC.GetOC(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener la Orden de compra: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="OC">Objeto OrdenCompra que se agregará a la base</param>
        /// <returns>El obejeto creado</returns>
        [HttpPost]
        public async Task<ActionResult<OrdenCompra>> Nuevo(OrdenCompra OC)
        {
            try
            {
                if (OC == null)
                    return BadRequest();

                OrdenCompra nuevo = await ROC.NuevoOC(OC);
                return nuevo;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Metodo asincrónico para modificar un objeto por ID
        /// </summary>
        /// <param name="OC">Objeto OrdenCompra que se usará para reemplazar a su homonimo </param>
        /// <param name="id">Id del objeto a reemplazar</param>
        /// <returns>Retorna el objeto modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrdenCompra>> Modificar(OrdenCompra OC, int id)
        {
            try
            {
                if (id != OC.Id_Orden_Compra)
                    return BadRequest("La Id no coincide");

                var Modificar = await ROC.GetOC(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                return await ROC.ModificarOC(OC);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            }
        }
    }
}