using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using JardinesEdi2022.Entidades.Entidades;
using JardinesEdi2022.Servicios.Facades;
using JardinesEdi2022.Utilidades;
using JardinesEdi2022.Web.App_Start;
using JardinesEdi2022.Web.ViewModels.Categoria;
using JardinesEdi2022.Web.ViewModels.Producto;
using JardinesEdi2022.Web.ViewModels.Proveedor;
using Newtonsoft.Json;

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

        [HttpPost]
        public JsonResult Guardar(string objeto, HttpPostedFileBase imagenArchivo)
        {

            bool respuesta = true;
            string mensaje = string.Empty;

            Producto producto=new Producto();
            producto = JsonConvert.DeserializeObject<Producto>(objeto);


            mensaje = ValidarProducto(producto);
            if (mensaje != string.Empty)
            {
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (_productosServicios.Existe(producto))
                {
                    respuesta = false;
                    mensaje = "Producto existente!!!";
                    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
                if (imagenArchivo != null)
                {
                    var extension = Path.GetExtension(imagenArchivo.FileName);
                    var filename = Guid.NewGuid().ToString();
                    FileHelper.UploadPhoto(imagenArchivo, WC.ProductImageFolder, filename + extension);
                    producto.Imagen = filename + extension;
                }

                _productosServicios.Guardar(producto);
                respuesta = true;
                mensaje = "Registro guardado satisfactoriamente";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                respuesta = false;
                mensaje = "Error al intentar guardar el registro de un producto";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
        }

        private string ValidarProducto(Producto productod)
        {
            StringBuilder mensaje = new StringBuilder();

            return mensaje.ToString();
        }
    }
}