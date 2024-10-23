using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    /// <summary>
    /// Interface que contiene todos los metodos para leer excel en especificos y generar un objeto correspondiente a su lectura
    /// </summary>
    public interface InterfaceExcel
    {
        public Task<Proveedores> LeerExcelProveedor(string filePath);
    }
}
