using APIPortalTPC.Datos;
using BaseDatosTPC;
using ClasesBaseDatosTPC;
using System.Data.SqlClient;
using System.Net.Mail;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioEnviarCorreo : InterfaceEnviarCorreo
    {

        public async Task<string> CorreoCotizacion(string productos, Proveedores P, string subject)
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

        public async Task<string> CorreoLiberador(Usuario U,string subject) {
            // Configuración del servidor SMTP
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "portaladquisiones@tpc.cl"; // Cambia esto por la dirección del remitente
            string archivo = @"C:\Users\drako\Desktop\PRO4.xlsx"; //Direccion del archivo a enviar
            // Pedir al usuario que ingrese el asunto del correo
            string toEmail = U.Correo_Usuario;
            // Cuerpo del mensaje en HTML con el bien o servicio ingresado
            string htmlBody ="";
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
        public async Task<string> Correo2()
        {
            throw new NotImplementedException();
        }
    }
}


