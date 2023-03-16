using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using JardinesEdi2022.Entidades.Entidades;
using JardinesEdi2022.Servicios.Facades;
using JardinesEdi2022.Web.App_Start;
using JardinesEdi2022.Web.ViewModels.Categoria;

namespace JardinesEdi2022.Web.Controllers
{
    public class CategoriasController : Controller
    {
        // GET: Categorias
        private readonly ICategoriasServicios _categoriasServicios;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriasServicios categoriasServicios)
        {
            _categoriasServicios = categoriasServicios;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCategorias()
        {
            var listaCategoriasVm = _mapper.Map<List<CategoriaListVm>>(_categoriasServicios.GetLista());
            return Json(new {data = listaCategoriasVm}, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCategoria(int categoriaId)
        {
            var categoriaVm = _mapper.Map<CategoriaEditVm>(_categoriasServicios.GetEntityPorId(categoriaId));
            return Json(categoriaVm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(Categoria categoria)
        {
            var mensaje = string.Empty;
            var respuesta = true;


            if (string.IsNullOrEmpty(categoria.NombreCategoria))
            {
                mensaje = "El nombre de la categoría es requerido!!";
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }else if (categoria.NombreCategoria.Length > 50)
            {
                mensaje = "El nombre de la categoría tiene más de 50 caracteres!!";
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }

            if (string.IsNullOrEmpty(categoria.Descripcion))
            {
                if (categoria.Descripcion.Length > 250)
                {
                    mensaje = "La descripción tiene más de 250 caracteres!!";
                    respuesta = false;
                    return Json(new {respuesta = respuesta, mensaje = mensaje}, JsonRequestBehavior.AllowGet);

                }

            }

            if (_categoriasServicios.Existe(categoria))
            {
                mensaje = "Categoría existente!!";
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
            _categoriasServicios.Guardar(categoria);
            mensaje = "Registro guardardo satisfactoriamente!!!";
            respuesta = true;
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult Update(Categoria categoria)
        {
            var mensaje = string.Empty;
            var respuesta = true;


            if (string.IsNullOrEmpty(categoria.NombreCategoria))
            {
                mensaje = "El nombre de la categoría es requerido!!";
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(categoria.Descripcion))
            {
                if (categoria.Descripcion.Length > 255)
                {
                    mensaje = "La descripción superó la cantidad máxima de caracteres !!";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

                }

            }

            if (_categoriasServicios.Existe(categoria))
            {
                mensaje = "Categoría existente!!";
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
            _categoriasServicios.Guardar(categoria);
            mensaje = "Registro editado satisfactoriamente!!!";
            respuesta = true;
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult Eliminar(int categoriaId)
        {
            string mensaje = string.Empty;
            bool respuesta = true;
            try
            {
                var categoria = _categoriasServicios.GetEntityPorId(categoriaId);
                if (!_categoriasServicios.EstaRelacionado(categoria))
                {
                    _categoriasServicios.Borrar(categoriaId);
                    mensaje = "Registro eliminado satisfactoriamente";
                    respuesta = true;
                    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    mensaje = "Registro relacionado...Baja denegada";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception e)
            {
                mensaje = "Error al intentar dar de baja una categoría";
                respuesta = false;
                return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

            }
        }

    }
}