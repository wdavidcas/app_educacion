﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using BLL;

namespace DAL
{
    //*********************************MODELO*******************************************
    /// <summary>
    /// Modelo de la tabla CategoriaNivel
    /// </summary>
    public class ModelEdificio
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelEdificio(int pk, string nombre, string descripcion, int estado)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Error = string.Empty;
        }

        /// <summary>
        /// contructor para construir el objeto con un error
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="estado"></param>
        /// <param name="error"></param>
        public ModelEdificio(int pk, string nombre, string descripcion, int estado, string error)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelEdificio()
        {

        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerEdificio
    {
        /// <summary>
        /// campos de la clase
        /// </summary>

        //almacena la recuperacion de datos
        private DataTable listado = new DataTable();
        //almacena el error que puede suceder 
        public string Error = string.Empty;
        //objeto del modelo de datos
        ModelCredenciales credenciales = null;
        //objeto de la conexion
        Conexion conexion = null;
        //lista para los parametros
        List<MySqlParameter> parametros = null;


        #region CONSTRUCTORS

        /// <summary>
        /// constructor por defecto, inicializa los campos de las clases
        /// </summary>
        public ControllerEdificio()
        {
            this.listado.Clear();
            this.Error = string.Empty;
            credenciales = new ModelCredenciales();
            conexion = new Conexion(credenciales);
            parametros = new List<MySqlParameter>();
            parametros.Clear();
        }
        #endregion

        #region CRUD

        /// <summary>
        /// Metodo para Insertar/Actualizar registros del Modelo
        /// </summary>
        /// <param name="categoria">objeto del modelo</param>
        /// <param name="Operacion">operacion a realizar [false=editar, true=agregar]</param>
        /// <returns></returns>
        public string Insertar(ModelEdificio edificio, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    MySqlParameter paramPk = new MySqlParameter("id", edificio.PK);
                    query = "UPDATE EDIFICIO SET nombre=@nombre,descripcion=@descripcion,estado=@estado,user=@user WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", edificio.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = "INSERT INTO EDIFICIO(nombre,descripcion,estado,user) VALUES(@nombre,@descripcion,@estado,@user)";
                }
                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(edificio.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(edificio.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramEstado = new MySqlParameter("estado", edificio.Estado);
                parametros.Add(paramEstado);
                bool respuesta = conexion.EjecutarQuery(query, parametros);
                this.Error = conexion.Error;
                return this.Error;
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return Error;
            }
        }

        /// <summary>
        /// Metodo para eliminar un registro del modelo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public bool Eliminar(ModelEdificio edificio)
        {
            try
            {
                string query = "DELETE FROM EDIFICIO WHERE id=@id";
                MySqlParameter paramPk = new MySqlParameter("id", edificio.PK);
                parametros.Add(paramPk);
                return conexion.EjecutarQuery(query, parametros);
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return false;
            }
        }
        #endregion


        #region COUNT

        /// <summary>
        /// retorna la cantidad de registros encontrados en el query de listado
        /// </summary>
        /// <param name="busqueda">filtro de búsqueda</param>
        /// <param name="estado">estado</param>
        /// <returns></returns>
        public int Count(string busqueda, int estado)
        {
            try
            {

                if (!string.IsNullOrEmpty(busqueda))
                {
                    string query = "SELECT COUNT(*) FROM EDIFCIO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.DecryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM EDIFICIO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
                    return conexion.EjecutarQueryCount(query);
                }
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return 0;
            }
        }

        /// <summary>
        /// verifica la existencia de un objeto con el mismo nombre
        /// </summary>
        /// <param name="nombre">nombre del objeto del modelo</param>
        /// <returns></returns>
        public int Count(string nombre)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM EDIFICIO WHERE nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
                return conexion.EjecutarQueryCount(query);
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return 0;
            }
        }      

        #endregion

        #region LIST
        /// <summary>
        /// lista todos los elementos del catalogo
        /// </summary>
        /// <returns></returns>
        public List<ModelEdificio> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelEdificio> lista = new List<ModelEdificio>();
            try
            {
                ModelEdificio edificio;
                listado.Clear();
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM EDIFICIO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    edificio = new ModelEdificio(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(edificio);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelEdificio(0, ex.Message.ToString(), ex.Message.ToString(), 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del catalogo de acuerdo a 
        /// </summary>
        /// <param name="inicio">inicio</param>
        /// <param name="paginacion">cantidad de registros</param>
        ///<param name="busqueda">texto a buscar</param>    
        /// <param name="estado">estado del registro 1=Habilitado 2=No Habilitado 3=Eliminado</param>        
        /// <returns></returns>
        public List<ModelEdificio> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelEdificio> lista = new List<ModelEdificio>();
            try
            {
                ModelEdificio edificio;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM EDIFICIO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");// LIMIT " + inicio + "," + cantidad + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    edificio = new ModelEdificio(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(edificio);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelEdificio(0, ex.Message.ToString(), ex.Message.ToString(), 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los objetos del catalogo 
        /// </summary>
        /// <param name="Estado"></param>
        /// <param name="Orden"></param>
        /// <param name="CampoOrden"></param>
        /// <returns></returns>
        public List<ModelEdificio> Listar(int Estado, string Orden, string CampoOrden) {
            List<ModelEdificio> lista = new List<ModelEdificio>();
            try
            {
                ModelEdificio edificio;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM EDIFICIO WHERE estado=" + Estado + " ORDER BY "+CampoOrden+" "+Orden+"");

                foreach (DataRow fila in listado.Rows)
                {
                    edificio = new ModelEdificio(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(edificio);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelEdificio(0, ex.Message.ToString(), ex.Message.ToString(), 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelEdificio> Listar(int PK)
        {
            List<ModelEdificio> lista = new List<ModelEdificio>();
            try
            {
                ModelEdificio edificio;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM EDIFICIO WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    edificio = new ModelEdificio(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(edificio);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelEdificio edificio = new ModelEdificio(0, string.Empty, string.Empty, 0, ex.Message.ToString());
                lista.Add(edificio);
                return lista;
            }
        }

       /// <summary>
       /// Retorna lista de edificios por modulo
       /// </summary>
       /// <param name="modulo">Id del modulo</param>
       /// <returns></returns>
        public List<ModelEdificio> ListarPorModulo(int modulo)
        {
            List<ModelEdificio> lista = new List<ModelEdificio>();
            try
            {
                ModelEdificio edificio;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM EDIFICIO WHERE id=" + modulo + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    edificio = new ModelEdificio(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(edificio);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelEdificio edificio = new ModelEdificio(0, string.Empty, string.Empty, 0, ex.Message.ToString());
                lista.Add(edificio);
                return lista;
            }
        }

        /// <summary>
        /// Metodo para buscar los nombres de la entidad similares al dado
        /// </summary>
        /// <param name="TextoBusqueda">Nombre a Buscar</param>
        /// <returns>Lista de nombres coincidentes</returns>
        public List<string> Listar(string TextoBusqueda, int Estado)
        {
            List<string> ListaAutocomplete = new List<string>();
            try
            {
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM EDIFICIO WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

                foreach (DataRow fila in listado.Rows)
                {
                    ListaAutocomplete.Add(Encryption.DecryptString(fila["nombre"].ToString()));
                }

                return ListaAutocomplete;
            }
            catch (Exception ex)
            {
                ListaAutocomplete.Add(ex.Message.ToString());
                return ListaAutocomplete;
            }
        }
        #endregion

        #region VALIDACIONES

        #endregion
    }
}
