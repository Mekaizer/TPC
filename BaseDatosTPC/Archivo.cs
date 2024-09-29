
namespace BaseDatosTPC
{
    public class Archivo
    {
        public int Id_Archivo { get; set; }
        public int? Id_ArchivoCotizacion { get; set; }
        public bool? IsPrincipal { get; set; }
        public byte[]? ArchivoDoc { get; set; }
        public int? Grupo_Archivo { get; set; }

    }
}
