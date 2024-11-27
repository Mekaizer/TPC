
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using NPOI.SS.Formula.Functions;
using System.Net.Mail;

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
        public async Task<string> CorreoProveedores(string productos, Proveedores P, string subject)
        {
            // Configuración del servidor SMTP
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "portaladquisiones@tpc.cl"; // Cambia esto por la dirección del remitente
            string archivo = @"C:\Users\drako\Desktop\PRO4.xlsx"; //Direccion del archivo a enviar
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
                    <li>{productos}</li> 
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
                    if (File.Exists(archivo))
                    {
                        mail.Attachments.Add(new Attachment(archivo));

                    }
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
            string archivo = @"C:\Users\drako\Desktop\PRO4.xlsx"; //Direccion del archivo a enviar
            // Pedir al usuario que ingrese el asunto del correo
            string toEmail = U.Correo_Usuario;
            // Cuerpo del mensaje en HTML sobre la liberación urgente
            string htmlBody = @"
            <html>
                <body>
                    <div style='background-color: #002060; color: white; padding: 10px; text-align: center;'>
                        <h1>Portal de Adquisiciones - TPC</h1>
                    </div>
                    <div style='padding: 20px; font-family: Arial, sans-serif;'>
                        <p>Estimado(a):</p>
                        <p>Por favor, confirmar recepción de la siguiente Orden de Compra:</p>
                        <ul>
                            <li><strong>N° ticket:</strong> 15376</li>
                            <li><strong>Orden de compra N°:</strong> 4700508073</li>
                        </ul>
                        <table border='1' style='border-collapse: collapse; width: 100%; text-align: left;'>
                            <thead>
                                <tr style='background-color: #f2f2f2;'>
                                    <th>Pos</th>
                                    <th>Cantidad</th>
                                    <th>Mon</th>
                                    <th>Prc.neto</th>
                                    <th>Proveedor</th>
                                    <th>Material</th>
                                    <th>Fecha</th>
                                    <th>Texto breve</th>
                                    <th>Valor neto</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>10</td>
                                    <td>1</td>
                                    <td>CLP</td>
                                    <td>1.694.926</td>
                                    <td>1050 PORTUARIA TUNQUÉN LIMITADA</td>
                                    <td>4000008269</td>
                                    <td>2024-08-26</td>
                                    <td>Arriendo maquinarias P6939</td>
                                    <td>1.694.926</td>
                                </tr>
                            </tbody>
                        </table>
                                         
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
                    if (File.Exists(archivo))
                    {
                        mail.Attachments.Add(new Attachment(archivo));

                    }
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
        public async Task<string> CorreoRecepciones(Usuario U, string subject)
        {
            //al solicitante no fue recepcionada, no se sabe el estado, parcialmente y cuando no hay respuesta
            // Configuración del servidor SMTP
            //tienes una orden de recepcion pendiente a confirmar 
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "recepciones@tpc.cl"; // Cambia esto por la dirección del remitente
            // Pedir al usuario que ingrese el asunto del correo    
            string toEmail = U.Correo_Usuario;
            // Cuerpo del mensaje en HTML con el bien o servicio ingresado
            string htmlBody = $@"
            <html>
            <head></head>
            <body>
                <p>Estimado/a {U.Nombre_Usuario},</p>
                <p>Junto con saludar, nos dirigimos a usted para confirar recepción de la sigientes Ordenes de Compra</p>
                <p>N° Ticket: {U.Id_Usuario}</p>
               
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
            string fromEmail = "liberaciones@tpc.cl"; // Cambia esto por la dirección del remitente
            string archivo = @"C:\Users\drako\Desktop\PRO4.xlsx"; //Direccion del archivo a enviar
            // Pedir al usuario que ingrese el asunto del correo
            string toEmail = U.Correo_Usuario;
            //Nombre del mensaje
            string subject = "Contraseña nueva";
            // Cuerpo del mensaje en HTML sobre la liberación urgente
            string htmlBody = $@"
            <html>
            <head></head>
            <body>
                <p>Estimado/a {U.Nombre_Usuario},</p>
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
                    if (File.Exists(archivo))
                    {
                        mail.Attachments.Add(new Attachment(archivo));

                    }
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


