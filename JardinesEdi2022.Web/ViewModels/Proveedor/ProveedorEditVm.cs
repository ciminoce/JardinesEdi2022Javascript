using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JardinesEdi2022.Web.ViewModels.Proveedor
{
    public class ProveedorEditVm
    {
        public int ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public int CiudadId { get; set; }
        public int PaisId { get; set; }

    }
}