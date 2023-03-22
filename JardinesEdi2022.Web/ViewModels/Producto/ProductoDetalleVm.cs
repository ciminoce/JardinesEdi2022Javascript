using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JardinesEdi2022.Web.ViewModels.Producto
{
    public class ProductoDetalleVm
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int UnidadesEnStock { get; set; }
        public bool Suspendido { get; set; }
        public string Imagen { get; set; }


    }
}