
namespace BaseDatosTPC
{
    /// <summary>
    /// clase que guarda los archivos
    /// </summary>
    public class Archivo
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        public int Id_Archivo { get; set; }
        /// <summary>
        /// Define si es el primero en leerse
        /// </summary>
        public bool? IsPrincipal { get; set; }
        /// <summary>
        /// Guarda el archivo
        /// </summary>
        public byte[]? ArchivoDoc { get; set; }
        /// <summary>
        /// Permite ver quienes estan relacionados
        /// </summary>
        public int? Grupo_Archivo { get; set; }
 
    }
}
