using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using JardinesEdi2022.Entidades.Entidades;
using JardinesEdi2022.Servicios.Facades;
using JardinesEdi2022.Web.App_Start;
using JardinesEdi2022.Web.ViewModels.Pais;

namespace JardinesEdi2022.Web.Controllers
{
    public class PaisesController : Controller
    {
        // GET: Paises
        private readonly IPaisesServicios _paisesServicios;
        private readonly IMapper _mapper;

        public PaisesController(IPaisesServicios paisesServicios)
        {
            _paisesServicios = paisesServicios;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPaises()
        {
            var listaPaisesVm =_mapper.Map<List<PaisListVm>>(_paisesServicios.GetLista());

            return Json(new{data=listaPaisesVm}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPais(int paisId)
        {
            var paisVm = _mapper.Map<PaisEditVm>(_paisesServicios.GetEntityPorId(paisId));
            return Json(paisVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Pais pais)
        {
            bool respuesta = true;
            string mensaje = string.Empty;

            if (_paisesServicios.Existe(pais))
            {
                respuesta = false;
                mensaje = "País existente!!!";
                return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                _paisesServicios.Guardar(pais);
                respuesta = true;
                mensaje = "Registro guardado satisfactoriamente";
                return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                respuesta = false;
                mensaje = "Error al intentar guardar el registro";
                return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult Eliminar(int paisId)
        {
            var respuesta = true;
            var mensaje = string.Empty;
            try
            {
                var pais = _paisesServicios.GetEntityPorId(paisId);
                if (_paisesServicios.EstaRelacionado(pais))
                {
                    respuesta = false;
                    mensaje = "País con registros relacionados... \nBaja denetada";
                    return Json(new {resultado = respuesta, mensaje = mensaje}, JsonRequestBehavior.AllowGet);
                }
                _paisesServicios.Borrar(pais.PaisId);
                respuesta = true;
                mensaje = "País eliminado!!!";
                return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                respuesta = false;
                mensaje = "Error al intentar dar de baja un país";
                return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}