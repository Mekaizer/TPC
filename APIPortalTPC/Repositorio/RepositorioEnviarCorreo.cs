using APIPortalTPC.Datos;
using BaseDatosTPC;
using System.Data.SqlClient;
using System.Net.Mail;

namespace APIPortalTPC.Repositorio
{
    public class RepositorioEnviarCorreo : InterfaceEnviarCorreo
    {

        public async Task<string> CorreoCotizacion()
        {
            // Configuración del servidor SMTP
            string smtpServer = "tpc-cl.mail.protection.outlook.com"; // Cambia esto según el servidor SMTP  
            int smtpPort = 25; // Cambia esto según el puerto que uses
            string fromEmail = "portaladquisiones@tpc.cl"; // Cambia esto por la dirección del remitente
            string toEmail = "paolocapdeville@gmail.com";
            string toEmail1 = "paolo.capdeville@alumnos.ucn.cl";

            // Pedir al usuario que ingrese el asunto del correo
        
            string subject = "Mensaje de texto";

            // Pedir al usuario que ingrese los productos o servicios
            
            string productos = "Bien/Servicio";

            // Cuerpo del mensaje en HTML con el bien o servicio ingresado
            string htmlBody = $@"
        <html>
            <head></head>
            <body>
                <p>Estimado,</p>
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
                    mail.To.Add(toEmail1);

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


