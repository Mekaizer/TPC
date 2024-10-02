﻿using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Usuario: " + ex.Message);
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
                if (resultado == null)
                    return NotFound();

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

                Usuario nuevo = await RU.NuevoUsuario(U);
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
        /// <param name="U">Objeto del tipo Usuario que se reemplazará por su homonimo</param>
        /// <param name="id">Id del objeto Usuario a modifcar</param>
        /// <returns>Retorna el nuevo objeto Usuario</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Usuario>> Modificar(Usuario U, int id)
        {
            try
            {
                if (id != U.Id_Usuario)
                    return BadRequest("La Id no coincide");

                var Modificar = await RU.GetUsuario(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");

                return await RU.ModificarUsuario(U);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos"+ex);
            }
        }
    }
}