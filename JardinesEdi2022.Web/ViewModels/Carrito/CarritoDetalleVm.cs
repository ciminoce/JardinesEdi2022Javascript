using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JardinesEdi2022.Web.ViewModels.Carrito
{
    public class CarritoDetalleVm
    {
        public int ProductoId { get; set; }
        public string Categoria { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public string Imagen { get; set; }

    }
}