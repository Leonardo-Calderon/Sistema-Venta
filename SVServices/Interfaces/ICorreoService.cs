
namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para el envío de correos electrónicos.
    /// </summary>
    /// <remarks>
    /// Esta interfaz proporciona métodos para enviar correos electrónicos utilizando el servicio
    /// de correo configurado en la aplicación. Utiliza la biblioteca MailKit para el envío
    /// de correos de forma segura y confiable.
    /// </remarks>
    public interface ICorreoService
    {
        /// <summary>
        /// Envía un correo electrónico a un destinatario específico.
        /// </summary>
        /// <param name="para">Dirección de correo electrónico del destinatario.</param>
        /// <param name="asunto">Asunto del correo electrónico.</param>
        /// <param name="mensajeHTML">Contenido del correo en formato HTML.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método envía un correo electrónico utilizando la configuración de SMTP
        /// definida en la aplicación. El contenido del correo puede incluir HTML para
        /// formateo avanzado del mensaje.
        /// </remarks>
        Task Enviar(string para, string asunto, string mensajeHTML);
    }
}
