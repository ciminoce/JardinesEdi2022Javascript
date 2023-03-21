using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using JardinesEdi2022.Entidades.Entidades;
using JardinesEdi2022.Servicios.Facades;
using JardinesEdi2022.Web.App_Start;
using JardinesEdi2022.Web.ViewModels.Ciudad;
using JardinesEdi2022.Web.ViewModels.Pais;
using JardinesEdi2022.Web.ViewModels.Proveedor;
using Newtonsoft.Json;

namespace JardinesEdi2022.Web.Controllers
{
    public class ProveedoresController : Controller
    {
        // GET: Proveedores
        private readonly IProveedoresServicios _proveedoresServicios;
        private readonly IPaisesServicios _paisesServicios;
        private readonly ICiudadesServicios _ciudadesServicios;
        private readonly IMapper _mapper;

        public ProveedoresController(IProveedoresServicios proveedoresServicios, IPaisesServicios paisesServicios, ICiudadesServicios ciudadesServicios)
        {
            _proveedoresServicios = proveedoresServicios;
            _paisesServicios = paisesServicios;
            _ciudadesServicios = ciudadesServicios;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProveedores()
        {
            var listaProveedoresVm = _mapper.Map<List<ProveedorListVm>>(_proveedoresServicios.GetLista());
            return Json(new {data = listaProveedoresVm}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProveedor(int proveedorId)
        {
            var proveedorVm =_mapper.Map<ProveedorEditVm>(_proveedoresServicios.GetEntityPorId(proveedorId));
            return Json(proveedorVm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPaises()
        {
            var listaPaisesVm = _mapper.Map<List<PaisListVm>>(_paisesServicios.GetLista());
            return Json(new {data = listaPaisesVm}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCiudades(int paisId)
        {
           var listaCiudadesVm = _mapper.Map<List<CiudadListVm>>(_ciudadesServicios.GetLista(paisId));
            return Json(new {data = listaCiudadesVm}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(string objeto)
        {

            bool respuesta = true;
            string mensaje = string.Empty;

            Proveedor proveedor = new Proveedor();
            proveedor = JsonConvert.DeserializeObject<Proveedor>(objeto);


            mensaje = ValidarProveedor(proveedor);
            if (mensaje != string.Empty)
            {
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (_proveedoresServicios.Existe(proveedor))
                {
                    respuesta = false;
                    mensaje = "Proveedor existente!!!";
                    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
                _proveedoresServicios.Guardar(proveedor);
                respuesta = true;
                mensaje = "Registro guardado satisfactoriamente";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                respuesta = false;
                mensaje = "Error al intentar guardar el registro de un Proveedor";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
        }

        private string ValidarProveedor(Proveedor proveedor)
        {
            StringBuilder mensaje = new StringBuilder();
            if (string.IsNullOrEmpty(proveedor.NombreProveedor))
            {
                mensaje.AppendLine("Nombre del proveedor es requerido");
            }
            else if (proveedor.NombreProveedor.Length > 50)
            {
                mensaje.AppendLine("Nombre  con más de 50 caracteres" + "<br>");
            }

            if (proveedor.PaisId == 0)
            {
                mensaje.AppendLine("Debe seleccionar un país");
            }

            if (proveedor.CiudadId==0)
            {
                mensaje.AppendLine("Debe seleccionar una ciudad");
                
            }

            return mensaje.ToString();
        }
        [HttpGet]
        public JsonResult Eliminar(int proveedorId)
        {
            bool respuesta = true;
            string mensaje = string.Empty;
            try
            {
                var proveedor = _proveedoresServicios.GetEntityPorId(proveedorId);
                if (_proveedoresServicios.EstaRelacionado(proveedor))
                {
                    respuesta = false;
                    mensaje = "Registro relacionada... baja denegada!!!";
                    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
                _proveedoresServicios.Borrar(proveedor.ProveedorId);
                respuesta = true;
                mensaje = "Registro borrado satisfactoriamente!!!";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                respuesta = false;
                mensaje = "Error al intentar dar de baja una ciudad";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}