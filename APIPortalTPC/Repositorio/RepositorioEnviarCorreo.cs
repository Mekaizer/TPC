
using BaseDatosTPC;
using ClasesBaseDatosTPC;
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
        public async Task<string> CorreoLiberador(Usuario U,string subject) {
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
            <head></head>
            <body>
                <p>Estimado,</p>
                <p>Le recordamos que tiene <strong>órdenes de compra pendientes por liberar</strong> con urgencia.</p>
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
    }
}


