using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JardinesEdi2022.Web.ViewModels.Proveedor
{
    public class ProveedorListVm
    {
        public int ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        [Display(Name = "País")]
        public string Pais { get; set; }
        public string Ciudad { get; set; }


    }
}