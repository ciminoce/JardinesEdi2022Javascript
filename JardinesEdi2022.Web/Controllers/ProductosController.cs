using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using JardinesEdi2022.Servicios.Facades;
using JardinesEdi2022.Web.App_Start;
using JardinesEdi2022.Web.ViewModels.Categoria;
using JardinesEdi2022.Web.ViewModels.Producto;
using JardinesEdi2022.Web.ViewModels.Proveedor;

namespace JardinesEdi2022.Web.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        private readonly IProductosServicios _productosServicios;
        private readonly ICategoriasServicios _categoriasServicios;
        private readonly IProveedoresServicios _proveedoresServicios;
        private readonly IMapper _mapper;

        public ProductosController(IProductosServicios productosServicios, ICategoriasServicios categoriasServicios, IProveedoresServicios proveedoresServicios)
        {
            _productosServicios = productosServicios;
            _categoriasServicios = categoriasServicios;
            _proveedoresServicios = proveedoresServicios;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProductos()
        {
            var listaProductosVm = _mapper.Map<List<ProductoListVm>>(_productosServicios.GetLista());
            return Json(new {data = listaProductosVm}, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProducto(int productoId)
        {
            var productoVm = _mapper.Map<ProductoEditVm>(_productosServicios.GetEntityPorId(productoId));
            return Json(productoVm, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetCategorias()
        {
            var listaCategoriasVm = _mapper.Map<List<CategoriaListVm>>(_categoriasServicios.GetLista());
            return Json(new { data = listaCategoriasVm }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProveedores()
        {
            var listaProveedoresVm = _mapper.Map<List<ProveedorListVm>>(_proveedoresServicios.GetLista());
            return Json(new { data = listaProveedoresVm }, JsonRequestBehavior.AllowGet);
        }

    }
}