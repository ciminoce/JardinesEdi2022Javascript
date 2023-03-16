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
using Newtonsoft.Json;

namespace JardinesEdi2022.Web.Controllers
{
    public class CiudadesController : Controller
    {
        // GET: Ciudades
        private readonly ICiudadesServicios _ciudadesServicios;
        private readonly IPaisesServicios _paisesServicios;
        private readonly IMapper _mapper;

        public CiudadesController(ICiudadesServicios ciudadesServicios, IPaisesServicios paisesServicios)
        {
            _ciudadesServicios = ciudadesServicios;
            _paisesServicios = paisesServicios;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCiudades()
        {
            var listaCiudades = _mapper.Map<List<CiudadListVm>>(_ciudadesServicios.GetLista());
            return Json(new {data = listaCiudades}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPaises()
        {
            var listaPaisesVm = _mapper.Map<List<PaisListVm>>(_paisesServicios.GetLista());
            return Json(listaPaisesVm, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetCiudades(int paisId)
        //{
        //    var listaCiudadesVm = _mapper.Map<List<CiudadListVm>>(_ciudadesServicios.GetLista(paisId));
        //    return Json(listaCiudadesVm, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult GetCiudad(int ciudadId)
        {
            var ciudad = _ciudadesServicios.GetEntityPorId(ciudadId);
            var ciudadEncontrada = new Ciudad()
            {
                CiudadId = ciudad.CiudadId,
                NombreCiudad = ciudad.NombreCiudad,
                PaisId = ciudad.PaisId
            };
            return Json(ciudadEncontrada, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(string objeto)
        {
            bool respuesta = true;
            string mensaje = string.Empty;

            Ciudad ciudad = new Ciudad();
            ciudad = JsonConvert.DeserializeObject<Ciudad>(objeto);


            mensaje = ValidarCiudad(ciudad);
            if (mensaje!=string.Empty)
            {
                respuesta = false;
                return Json(new {respuesta = respuesta, mensaje = mensaje}, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (_ciudadesServicios.Existe(ciudad))
                {
                    respuesta = false;
                    mensaje = "Ciudad existente!!!";
                    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
                _ciudadesServicios.Guardar(ciudad);
                respuesta = true;
                mensaje = "Registro guardado satisfactoriamente";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                respuesta = false;
                mensaje = "Error al intentar guardar el registro de Ciudad";
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
        }

        private string ValidarCiudad(Ciudad ciudad)
        {
            StringBuilder mensaje = new StringBuilder();
            if (string.IsNullOrEmpty(ciudad.NombreCiudad))
            {
                mensaje.AppendLine("Nombre de Ciudad es requerido"+"<br>");
            }else if (ciudad.NombreCiudad.Length > 50)
            {
                mensaje.AppendLine("Nombre de ciudad con más de 50 caracteres" + "<br>");
            }

            if (ciudad.PaisId==0)
            {
                mensaje.AppendLine("Debe seleccionar un país");
            }

            return mensaje.ToString();
        }

        [HttpGet]
        public JsonResult Eliminar(int ciudadId)
        {
            bool respuesta = true;
            string mensaje = string.Empty;
            try
            {
                var ciudad = _ciudadesServicios.GetEntityPorId(ciudadId);
                if (_ciudadesServicios.EstaRelacionado(ciudad))
                {
                    respuesta = false;
                    mensaje = "Registro relacionada... baja denegada!!!";
                    return Json(new {respuesta = respuesta, mensaje = mensaje}, JsonRequestBehavior.AllowGet);
                }
                _ciudadesServicios.Borrar(ciudadId);
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