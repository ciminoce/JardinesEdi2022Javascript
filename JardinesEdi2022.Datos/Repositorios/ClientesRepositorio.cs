﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using JardinesEdi2022.Datos.Facades;
using JardinesEdi2022.Entidades.Entidades;

namespace JardinesEdi2022.Datos.Repositorios
{
    public class ClientesRepositorio:IClientesRepositorio
    {
        private readonly ViveroSqlDbContext _context;

        public ClientesRepositorio(ViveroSqlDbContext context)
        {
            _context = context;
        }
        public Cliente GetTEntityPorId(int id)
        {
            try
            {
                return _context.Clientes
                    .Include(c=>c.Pais)
                    .Include(c=>c.Ciudad)
                    .SingleOrDefault(c=>c.ClienteId == id);
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer");
            }
        }

        public void Guardar(Cliente cliente)
        {
            try
            {
                if (cliente.Pais!=null)
                {
                    _context.Paises.Attach(cliente.Pais);

                }

                if (cliente.Ciudad!=null)
                {
                    _context.Ciudades.Attach(cliente.Ciudad);
                    
                }
                if (cliente.ClienteId == 0)
                {
                    //Cuando el id=0 entonces la entidad es nueva ==>alta
                    _context.Clientes.Add(cliente);
                }
                else
                {
                    var clienteInDb =
                        _context.Clientes.SingleOrDefault(c=>c.ClienteId == cliente.ClienteId);
                    if (clienteInDb == null)
                    {
                        throw new Exception("Cliente inexistente");
                    }
                    clienteInDb.Nombres = cliente.Nombres;
                    clienteInDb.Apellido = cliente.Apellido;
                    clienteInDb.CiudadId = cliente.CiudadId;
                    clienteInDb.PaisId = cliente.PaisId;
                    clienteInDb.CodigoPostal = cliente.CodigoPostal;
                    clienteInDb.Direccion = cliente.Direccion;
                    
                    _context.Entry(clienteInDb).State = EntityState.Modified;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al intentar guardar un registro");
            }
        }

        public bool Existe(Cliente cliente)
        {
            try
            {
                if (cliente.ClienteId == 0)
                {
                    return _context.Clientes.Any(c=>c.Nombres == cliente.Nombres && c.Apellido==cliente.Apellido);
                }

                return _context.Clientes.Any(c=>c.Nombres == cliente.Nombres && c.Apellido == cliente.Apellido
                                                                             && c.ClienteId != cliente.ClienteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool EstaRelacionado(Cliente cliente)
        {
            try
            {
                return _context.Ordenes.Any(o => o.ClienteId == cliente.ClienteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int GetCantidad()
        {
            try
            {
                return _context.Clientes.Count();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Borrar(int id)
        {
            Cliente clienteInDb = null;
            try
            {
                clienteInDb = _context.Clientes
                    .SingleOrDefault(c=>c.ClienteId == id);
                if (clienteInDb == null) return;
                _context.Entry(clienteInDb).State = EntityState.Deleted;
                //_context.SaveChanges();
            }
            catch (Exception e)
            {
                _context.Entry(clienteInDb).State = EntityState.Unchanged;
                throw new Exception(e.Message);
            }
        }

        public List<Cliente> GetLista()
        {
            try
            {
                return _context.Clientes.OrderBy(c=>c.Apellido).ThenBy(c=>c.Nombres).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
