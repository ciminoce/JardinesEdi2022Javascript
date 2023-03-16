using AutoMapper;
using JardinesEdi2022.Entidades.Entidades;
using JardinesEdi2022.Web.ViewModels.Pais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JardinesEdi2022.Web.ViewModels.Categoria;
using JardinesEdi2022.Web.ViewModels.Ciudad;

namespace JardinesEdi2022.Web.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            LoadPaisesMapping();
            LoadCategoriasMapping();
            LoadCiudadesMapping();
        }

        private void LoadCiudadesMapping()
        {
            CreateMap<Ciudad, CiudadListVm>().ForMember(dest=>dest.Pais,opt=>opt.MapFrom(src=>src.Pais.NombrePais));
        }

        private void LoadCategoriasMapping()
        {
            CreateMap<Categoria, CategoriaListVm>();
            CreateMap<Categoria, CategoriaEditVm>();

        }

        private void LoadPaisesMapping()
        {
            CreateMap<Pais, PaisListVm>();
            CreateMap<Pais, PaisEditVm>().ReverseMap();
        }
    }
}