
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using System.Net.Mail;
using System.Text;




namespace APIPortalTPC.Repositorio
{
    public class RepositorioEnviarCorreo : InterfaceEnviarCorreo
    {
        /// <summary>
        /// Metodo que envia un correo al proveedor, indicando su correo y nombre
        /// </summary>
        /// <param name="productos"></param>
        /// <param name="P"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> CorreoProveedores(Proveedores P, string subject)
        {
            // Configuración del servidor SMTP
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "portaladquisiones@tpc.cl"; // Cambia esto por la dirección del remitente
            //string archivo = @"C:\\Desktop\PRO4.xlsx"; //Direccion del archivo a enviar
            // Pedir al usuario que ingrese el asunto del correo
            string toEmail = P.Correo_Proveedor;


            // Cuerpo del mensaje en HTML con el bien o servicio ingresado
            string htmlBody = $@"
            <html>
            <head></head>
            <body>
                <p>Estimado/a {P.Nombre_Representante},</p>
                <p>Junto con saludar, nos dirigimos a usted para realizar cotización por el siguiente bien o servicio:</p>
                <ul>
                    <li>{P.ID_Bien_Servicio}</li> 
                </ul>
                <p>Por favor, tenga en cuenta que este es un mensaje generado automáticamente. No responda a este correo. Para enviar su cotización o cualquier consulta, favor de contactarnos a través del correo electrónico: <strong>adquisicionestpc@tpc.cl</strong>.</p>
                <p>Agradecemos su pronta colaboración.</p>
                <p>Saludos cordiales,</p>
                <p>Equipo de Adquisiciones<br/>
                <strong>Terminal Puerto de Coquimbo</strong></p>
            </body>
        </html>";

            try
            {
                // Crear el mensaje de correo
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);

                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true; // Indica que el cuerpo del mensaje es HTML
                    /*if (File.Exists(archivo))
                    {
                        mail.Attachments.Add(new Attachment(archivo));

                    }
                    */
                    // Configurar el cliente SMTP
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        // No se especifica credenciales ni SSL
                        smtpClient.EnableSsl = false; // Ajusta esto si es necesario

                        // Enviar el mensaje
                        smtpClient.Send(mail);
                    }
                }

                return "Correo enviado exitosamente.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar el correo: {ex.Message}");
            }
        }
        /// <summary>
        /// Metodo que permite enviar correo a un Usuario que debe liberar ordenes de compra, usando su correo y nombre
        /// </summary>
        /// <param name="U"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string>   CorreoLiberador(Usuario U,string subject) {
            // Configuración del servidor SMTP
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "liberaciones@tpc.cl"; // Cambia esto por la dirección del remitente
            // Pedir al usuario que ingrese el asunto del correo
            string toEmail = U.Correo_Usuario;
            // Cuerpo del mensaje en HTML sobre la liberación urgente
            string htmlBody = @"
              <html>
                      <p>Por favor, proceda con la liberación lo antes posible para evitar retrasos en el proceso de adquisiciones.</p>
                      <p>Agradecemos su pronta colaboración.</p>
                      <p>Saludos cordiales,</p>
                      <p>Equipo de Adquisiciones<br/>
                      <strong>Terminal Puerto de Coquimbo</strong></p>
                  </body>
              </html>";
            try
            {
                // Crear el mensaje de correo
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);

                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true; // Indica que el cuerpo del mensaje es HTML

                    // Configurar el cliente SMTP
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        // No se especifica credenciales ni SSL
                        smtpClient.EnableSsl = false; // Ajusta esto si es necesario

                        // Enviar el mensaje
                        smtpClient.Send(mail);
                    }
                }

                return "Correo enviado exitosamente.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar el correo: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Metodo que envia correo al responsable del ticket
        /// </summary>
        /// <param name="U"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> CorreoRecepciones(Usuario U, string subject, List<int> Id_Ticket)
        {
            //al solicitante no fue recepcionada, no se sabe el estado, parcialmente y cuando no hay respuesta
            // Configuración del servidor SMTP
            //tienes una orden de compra pendiente a confirmar 
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "recepciones@tpc.cl"; // Cambia esto por la dirección del remitente
            // Pedir al usuario que ingrese el asunto del correo    
            string toEmail = U.Correo_Usuario;
            // Cuerpo del mensaje en HTML con el bien o servicio ingresado
            StringBuilder sb = new StringBuilder();

            string resultado = "";
            foreach (int numero in Id_Ticket)
            {
                resultado += $"N° Ticket: {numero} \n\n\n";
            }
            string htmlBody = $@"<html>
                <body>
                    <div style='background-color: #002060; color: white; padding: 10px; text-align: center;'>
                        <h1>Portal de Adquisiciones - TPC</h1>
                    </div>
                    <div style='padding: 20px; font-family: Arial, sans-serif;'>
                        <p>Estimado(a):</p>
                        <p>Por favor, confirmar recepción de la siguiente Orden de Compra:</p>
                        <ul>
                            <li><strong> {resultado} </strong> </li>
                        </ul>                                         
                    </div>
                    <div style='background-color: #002060; color: white; padding: 10px; text-align: center;'>
                        <p>© 2024 Portal de Adquisiciones TPC. Todos los derechos reservados.</p>
                    </div>
                </body>
            </html>";
            try
            {
                // Crear el mensaje de correo
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);

                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true; // Indica que el cuerpo del mensaje es HTML

                    // Configurar el cliente SMTP
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        // No se especifica credenciales ni SSL
                        smtpClient.EnableSsl = false; // Ajusta esto si es necesario

                        // Enviar el mensaje
                        smtpClient.Send(mail);
                    }
                }

                return "Correo enviado exitosamente.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar el correo: {ex.Message}");
            }
        }


        public async Task<string> CorreoUsuarioPass(Usuario U)
        {
            // Configuración del servidor SMTP
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "portaladquisiones@tpc.cl"; // Cambia esto por la dirección del remitente
            // Pedir al usuario que ingrese el asunto del correo
            string toEmail = U.Correo_Usuario;
            //Nombre del mensaje
            string subject = "Contraseña nueva";
            // Cuerpo del mensaje en HTML sobre la liberación urgente
            string htmlBody = $@"<html>
            <head></head>
            <body>
            <p style='font-size: 16px; color: #333;'>Estimado/a {U.Nombre_Usuario},</p>

                <p>Su contraseña para activar su cuenta es la siguiente: </p>
                <ul>
                <li>{U.Contraseña_Usuario}</li> 
                </ul>
                <p>Por favor, tenga en cuenta que este es un mensaje generado automáticamente. No responda a este correo.En caso de cualquier consulta, favor de contactarnos a través del correo electrónico: <strong>adquisicionestpc@tpc.cl</strong>.</p>
                <strong>Terminal Puerto de Coquimbo</strong></p>
            </body>
        </html>";

            try
            {
                // Crear el mensaje de correo
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);

                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true; // Indica que el cuerpo del mensaje es HTML

                    // Configurar el cliente SMTP
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        // No se especifica credenciales ni SSL
                        smtpClient.EnableSsl = false; // Ajusta esto si es necesario

                        // Enviar el mensaje
                        smtpClient.Send(mail);
                    }
                }

                return "Correo enviado exitosamente.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar el correo: {ex.Message}");
            }
        }
        public async Task<string> RecuperarPass(Usuario U)
        {
            // Configuración del servidor SMTP
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "portaladquisiones@tpc.cl"; // Cambia esto por la dirección del remitente
            // Pedir al usuario que ingrese el asunto del correo
            string toEmail = U.Correo_Usuario;
            //Nombre del mensaje
            string subject = "Recuperar contraseña";
            // Cuerpo del mensaje en HTML sobre la liberación urgente
            string htmlBody = $@"<html>
            <head></head>
            <body>
            <p style='font-size: 16px; color: #333;'>Estimado/a {U.Nombre_Usuario},</p>

                <p>Su contraseña: </p>
                <ul>
                <li>{U.Contraseña_Usuario}</li> 
                </ul>
                <p>Por favor, tenga en cuenta que este es un mensaje generado automáticamente. No responda a este correo.En caso de cualquier consulta, favor de contactarnos a través del correo electrónico: <strong>adquisicionestpc@tpc.cl</strong>.</p>
                <strong>Terminal Puerto de Coquimbo</strong></p>
            </body>
        </html>";

            try
            {
                // Crear el mensaje de correo
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);

                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true; // Indica que el cuerpo del mensaje es HTML

                    // Configurar el cliente SMTP
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        // No se especifica credenciales ni SSL
                        smtpClient.EnableSsl = false; // Ajusta esto si es necesario

                        // Enviar el mensaje
                        smtpClient.Send(mail);
                    }
                }

                return "Correo enviado exitosamente.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}


