using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    /// <summary>
    /// Interface que contiene todos los metodos para leer excel en especificos y generar un objeto correspondiente a su lectura
    /// </summary>
    public interface InterfaceExcel
    {
        public Task<Proveedores> LeerExcelProveedor(string filePath);
        public Task<List<Proveedores>> LeerProveedores(string filePath);
        //public Task<> LeerReporteSap(string filePath);
        public Task<List<CentroCosto>> LeerExcelCeCo(string filePath);
        public Task<string> LeerExcelOC(string filePath);
        public Task<List<BienServicio>> LeerBienServicio(string filePath);


    }
}
