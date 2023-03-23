using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using JardinesEdi2022.Entidades.Entidades;
using JardinesEdi2022.Servicios.Facades;
using JardinesEdi2022.Web.App_Start;
using JardinesEdi2022.Web.ViewModels.Carrito;
using JardinesEdi2022.Web.ViewModels.Categoria;
using JardinesEdi2022.Web.ViewModels.Producto;

namespace JardinesEdi2022.Web.Controllers
{
    public class TiendaController : Controller
    {
        private readonly ICarritosServicios _carritosServicios;
        private readonly ICategoriasServicios _categoriasServicios;
        private readonly IProductosServicios _productosServicios;
        private readonly IMapper _mapper;

        public TiendaController(ICarritosServicios carritosServicios, ICategoriasServicios categoriasServicios, IProductosServicios productosServicios)
        {
            _categoriasServicios = categoriasServicios;
            _carritosServicios = carritosServicios;
            _productosServicios = productosServicios;
            _mapper = AutoMapperConfig.Mapper;
        }

        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCategorias()
        {
            var listaCategoriasVm = _mapper.Map<List<CategoriaListVm>>(_categoriasServicios.GetLista());

            return Json(new { data = listaCategoriasVm }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductos(int categoriaId)
        {
            var listaVm = _mapper.Map<List<ProductoListVm>>(_productosServicios.GetLista(categoriaId));
            var jsonResultado = Json(new { data = listaVm }, JsonRequestBehavior.AllowGet);
            jsonResultado.MaxJsonLength = int.MaxValue;
            return jsonResultado;
        }

        public ActionResult DetalleProducto(int productoId)
        {
            var productoVm = _mapper
                .Map<ProductoDetalleVm>(_productosServicios.GetEntityPorId(productoId));
            return View(productoVm);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            var clienteId = ((Usuario)Session["usuario"]).UsuarioId;
            var cantidad = _carritosServicios.CantidadEnCarrito(clienteId);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        public JsonResult AgregarAlCarrito(int productoId)
        {
            var clienteId = ((Usuario)Session["usuario"]).UsuarioId;
            string mensaje = string.Empty;
            bool resultado = false;
            try
            {
                _carritosServicios.AgregarAlCarrito(clienteId,productoId,1);
               
                mensaje = "Producto agregado al Carrito";
                resultado = true;
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                resultado = false;

            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Carrito()
        {
            return View();
        }
        [HttpPost]
        public JsonResult MostrarCarrito()
        {
            var clienteId = ((Usuario)Session["usuario"]).UsuarioId;
            var listaVm = _mapper.Map<List<CarritoDetalleVm>>(_carritosServicios.GetItemsCarrito(clienteId));
            return Json(new { data = listaVm }, JsonRequestBehavior.AllowGet);

        }
    }
}