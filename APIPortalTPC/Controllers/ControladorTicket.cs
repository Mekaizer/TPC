﻿using APIPortalTPC.Repositorio;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
/*
 * Este controlador permite conectar Base datos y el repositorio correspondiente para ejecutar los metodos necesarios
 * **/
namespace APIPortalTPC.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ControladorTicket : ControllerBase
    {

        //Se usa readonly para evitar que se pueda modificar pero se necesita inicializar y evitar que se reemplace por otra instancia
        private readonly IRepositorioTicket RT;
        private readonly IRepositorioOrdenCompra ROC;
        private readonly IRepositorioCorreo IRC;
        /// <summary>
        /// Se inicializa la Interface Repositorio
        /// </summary>
        /// <param name="RT">Interface de RepositorioTicket</param>

        public ControladorTicket(IRepositorioCorreo IRC,IRepositorioTicket RT, IRepositorioOrdenCompra ROC)
        {
            this.RT = RT;
            this.ROC = ROC;
            this.IRC = IRC;
        }
        /// <summary>
        /// Metodo asincrónico para obtener todos los objetos de la tabla
        /// </summary>
        /// <returns>Retorna una lista con todos los objetos de la clase Ticket de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await RT.GetAllTicket());
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo asincrónico para obtener UN objeto en especifico, se debe ingresar el ID del objeto
        /// </summary>
        /// <param name="id">Id del objeto a buscar</param>
        /// <returns>Retorna el objeto Ticket creado</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var resultado = await RT.GetTicket(id);
                if (resultado.ID_Ticket == 0)
                    return StatusCode(StatusCodes.Status404NotFound, "No se encontro el ticket");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo asincrónico para crear nuevo objeto
        /// </summary>
        /// <param name="T">Objeto del tipo Ticket que se quiere agregar a la base de datos</param>
        /// <returns>Retorna el objeto Ticket que se agrego a la base de datos</returns>
        [HttpPost]
        public async Task<ActionResult<Ticket>> Nuevo(Ticket T)
        {
            try
            {
                if (T == null)
                    return BadRequest();

                Ticket nuevo = await RT.NewTicket(T);
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
        /// <param name="T">Objeto del tipo Ticket que se quiere reemplazar por su homonimo</param>
        /// <param name="id">Id del objeto Ticket a modificar</param>
        /// <returns>Objeto Ticket modificado</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Ticket>> Modificar(Ticket T, int id)
        {
            try
            {
                if (id != T.ID_Ticket)
                    return BadRequest("La Id no coincide");

                var Modificar = await RT.GetTicket(id);

                if (Modificar == null)
                    return NotFound($"Centro de Costo con = {id} no encontrado");


                return await RT.ModificarTicket(T);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos "+ex.Message);
            }
        }
        /// <summary>
        /// Metodo para actualizar el estado del ticket, esto se hace revisando las ordenes de compra asociadas, luego se debe 
        /// actualizar las Ordenes de compra si Estado ticket es OC Recepcionada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Estado/{id:int}")]    
        public async Task<ActionResult<Ticket>> EstadoTicket(int id)
        {
            try
            {
                var Ticket = await RT.ActualizarEstadoTicket(id);
                if (Ticket.ID_Ticket != 0) 
                {
                    if ( Ticket.Estado.Equals("OC Parcial") || Ticket.Estado.Equals("OC Recepcionada") || Ticket.Estado.Equals("Espera de liberacion", StringComparison.OrdinalIgnoreCase))
                    {
                        var OC = await ROC.GetAllOCTicket(Ticket.ID_Ticket);
                        foreach (OrdenCompra cambia in OC)
                        {
                     
                            await ROC.ModificarOC(cambia);
                        }
                        return Ticket;
                    }
                   
 
                }

      
                    return NotFound("Ticket no encontrado");
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status400BadRequest,"Error no se de que "+ex);
            }
            
        }
        /// <summary>
        /// Deja el estado "invisible" para usar el ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Ticket>> Eliminar(int id)
        {
            try
            {
                var u = RT.GetTicket(id);
                if (u == null)
                {
                    return NotFound("No se encontro el ticket");
                }
                return Ok(await RT.EliminarTicket(id));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos" + ex);
            }
        }
        /// <summary>
        /// Permite ver las ordenes de compra asociadas al ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ListaOC/{id:int}")]
        public async Task<ActionResult<OrdenCompra>> ListaOC(int id)
        {
            try
            {
                var lista = await ROC.GetAllOC();
                var ticket = await RT.GetTicket(id);
                List<OrdenCompra> loc = new List<OrdenCompra>();
                if (ticket.ID_Ticket == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                foreach (OrdenCompra OC in lista)
                    if (OC.Id_Ticket == ticket.ID_Ticket)
                        loc.Add(OC);
                if(loc == null)
                    return BadRequest();

                return Ok(loc);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error de " + ex);
            }
        }

        /// <summary>
        /// Muestra los ticket del usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Usuario/{id:int}")]
        public async Task<ActionResult> GetAllUsuario(int id)
        {
            try
            {
                return Ok(await RT.GetAllTicketUsuario(id));
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener el Ticket: " + ex.Message);
            }
        }

        /// <summary>
        /// Dado un Id_Ticket, modifica todos las ordens de compras asociadas y cambia el estado a OC Recepcionada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost("RecepcionTotal/{id:int}")]
        public async Task<ActionResult> RecepcionTotal(int id)
        {
            try
            {
                var ListaOC = await ROC.GetAllOCTicket(id);
                Ticket T = await RT.GetTicket(id);
                T.Estado = "OC Recepcionada";
                foreach(OrdenCompra OC in ListaOC)
                {
                    //Pasar estado a true
                    OC.Recepcion = true;
                    await ROC.ModificarOC(OC);
                }
                await RT.ModificarTicket(T);
                //crear el objeto Correo
                Correo C = new Correo();
                C.Id_Ticket = T.ID_Ticket;
                C.Numero_OC = T.Numero_OC;
                C.Proveedor = T.ID_Proveedor;
                C.CeCo = T.Id_OE;
                C.CorreosEnviados = 0;
                C.detalle = T.Detalle;
                string res = await IRC.Existe((int)T.ID_Ticket);
                if (res.Equals("ok"))
                {
                    await IRC.NuevoCorreo(C);
                }
                return Ok("Recepcionado con exito");
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error : " + ex.Message);
            }
        }
    


    }
}