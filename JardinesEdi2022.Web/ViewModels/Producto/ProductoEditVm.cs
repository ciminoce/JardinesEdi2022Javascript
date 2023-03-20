using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JardinesEdi2022.Web.ViewModels.Producto
{
    public class ProductoEditVm
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string NombreLatin { get; set; }
        public int ProveedorId { get; set; }
        public int CategoriaId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int UnidadesEnStock { get; set; }
        public int NivelDeReposicion { get; set; }
        public bool Suspendido { get; set; }
        public string Imagen { get; set; }
        public HttpPostedFileBase ImagenFile { get; set; }

    }
}