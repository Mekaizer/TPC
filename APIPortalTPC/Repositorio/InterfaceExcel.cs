namespace APIPortalTPC.Repositorio
{
    public interface InterfaceExcel
    {
        public Task<string> LeerExcel(string filePath);
    }
}
