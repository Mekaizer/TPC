﻿using BaseDatosTPC;
using Microsoft.AspNetCore.Mvc;

namespace APIPortalTPC.Repositorio
{
    public interface InterfaceCrearExcel
    {
        public  Task<string> DescargarExcel(List<OrdenCompra> LOC);
    }
}
