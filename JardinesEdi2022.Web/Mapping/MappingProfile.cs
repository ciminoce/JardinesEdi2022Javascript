using AutoMapper;
using JardinesEdi2022.Entidades.Entidades;
using JardinesEdi2022.Web.ViewModels.Pais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JardinesEdi2022.Web.ViewModels.Categoria;
using JardinesEdi2022.Web.ViewModels.Ciudad;
using JardinesEdi2022.Web.ViewModels.Producto;
using JardinesEdi2022.Web.ViewModels.Proveedor;
using JardinesEdi2022.Web.ViewModels.Carrito;

namespace JardinesEdi2022.Web.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            LoadPaisesMapping();
            LoadCategoriasMapping();
            LoadCiudadesMapping();
            LoadProductosMapping();
            LoadProveedoresMapping();
            LoadCarritosMapping();
        }

        private void LoadCarritosMapping()
        {
            CreateMap<Carrito, CarritoDetalleVm>()
                .ForMember(dest=>dest.NombreProducto,opt=>opt.MapFrom(src=>src.Producto.NombreProducto))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Producto.Categoria.NombreCategoria))
                .ForMember(dest=>dest.PrecioUnitario,opt=>opt.MapFrom(src=>src.Producto.PrecioUnitario));
        }

        private void LoadProveedoresMapping()
        {
            CreateMap<Proveedor, ProveedorListVm>()
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Pais.NombrePais))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad.NombreCiudad));
            CreateMap<Proveedor, ProveedorEditVm>();
        }

        private void LoadProductosMapping()
        {
            CreateMap<Producto, ProductoListVm>().ForMember(dest => dest.Categoria,
                opt => opt.MapFrom(src => src.Categoria.NombreCategoria));
            CreateMap<Producto, ProductoEditVm>().ReverseMap();
            CreateMap<Producto, ProductoDetalleVm>().ForMember(dest => dest.Categoria,
                opt => opt.MapFrom(src => src.Categoria.NombreCategoria));
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