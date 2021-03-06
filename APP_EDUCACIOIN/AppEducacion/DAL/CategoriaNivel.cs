﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BLL;
using MySql.Data.MySqlClient;


namespace DAL
{
    //*********************************MODELO*******************************************
    /// <summary>
    /// Modelo de la tabla CategoriaNivel
    /// </summary>
    public class ModelCategoriaNivel
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Codigo { get; set; }
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
        public ModelCategoriaNivel(int pk,string codigo, string nombre, string descripcion,int estado)
        {
            this.PK = pk;
            this.Codigo = codigo;
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
        public ModelCategoriaNivel(int pk,string codigo, string nombre, string descripcion, int estado, string error) {
            this.PK = pk;
            this.Codigo = codigo;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelCategoriaNivel()
        { 
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerCategoriaNivel
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
        public ControllerCategoriaNivel()
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
        public string Insertar(ModelCategoriaNivel categoria,bool Operacion)
        {
            try
            {
                string query=string.Empty;

                if (!Operacion)
                {
                    MySqlParameter paramPk = new MySqlParameter("id", categoria.PK);
                    query = "UPDATE CategoriaNivel SET codigo=@codigo,nombre=@nombre,descripcion=@descripcion,estado=@estado,user=@user WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id",categoria.PK);
                    parametros.Add(paramId);
                }
                else{
                    query="INSERT INTO CategoriaNivel(nombre,descripcion,estado,user,codigo) VALUES(@nombre,@descripcion,@estado,@user,@codigo)";
                }

                MySqlParameter paramCodigo = new MySqlParameter("codigo",Encryption.DecryptString(categoria.Codigo));
                parametros.Add(paramCodigo);
                MySqlParameter paramNombre = new MySqlParameter("nombre",Encryption.EncryptString(categoria.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion",Encryption.EncryptString(categoria.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramEstado = new MySqlParameter("estado",categoria.Estado);
                parametros.Add(paramEstado);
                bool respuesta=conexion.EjecutarQuery(query, parametros);
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
        public bool Eliminar(ModelCategoriaNivel categoria)
        {
            try
            {
                string query = "DELETE FROM CategoriaNivel WHERE id=@id";
                MySqlParameter paramPk = new MySqlParameter("id", categoria.PK);
                parametros.Add(paramPk);
                return conexion.EjecutarQuery(query,parametros);
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
        public int Count(string busqueda,int estado) {
            try
            {

                if (!string.IsNullOrEmpty(busqueda))
                {
                    string query = "SELECT COUNT(*) FROM categorianivel WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query="SELECT COUNT(*) FROM categorianivel WHERE estado="+estado+" AND estado<>"+(int)Estados.Tipos.Eliminado+"";
                    return conexion.EjecutarQueryCount(query);
                }                
            }
            catch (Exception ex) {
                this.Error = ex.Message.ToString();
                return 0;
            }
        }

        /// <summary>
        /// verifica la existencia de un objeto con el mismo nombre
        /// </summary>
        /// <param name="nombre">nombre del objeto del modelo</param>
        /// <returns></returns>
        public int Count(string nombre) {
            try
            {
                string query = "SELECT COUNT(*) FROM categorianivel WHERE nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
                return conexion.EjecutarQueryCount(query);
            }
            catch (Exception ex) {
                this.Error = ex.Message.ToString();
                return 0;
            }            
        }

        /// <summary>
        /// verifica la existencia de un objeto con el mismo CODIGO
        /// </summary>
        /// <param name="codigo">codigo del objeto del modelo</param>
        /// <returns></returns>
        public int Count(string codigo,bool operacion)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM categorianivel WHERE codigo='" + Encryption.EncryptString(codigo.ToUpper()) + "'";
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
        /// retorna los elementos del modelo
        /// </summary>
        /// <param name="inicio">Inicio</param>
        /// <param name="paginacion">registros a obtener</param>
        /// <param name="estado">estado de los registros</param>
        /// <returns></returns>
        public List<ModelCategoriaNivel> Listar(int inicio,int paginacion,int estado)
        {
            List<ModelCategoriaNivel> lista = new List<ModelCategoriaNivel>();
            try
            {                
                ModelCategoriaNivel categoriaNivel;
                listado.Clear();                
                listado = conexion.EjecutarSelect("SELECT id,codigo,nombre,descripcion,estado FROM categorianivel WHERE estado="+estado+" AND estado<>" + (int) Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    categoriaNivel = new ModelCategoriaNivel(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["codigo"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()),Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToInt32(fila["estado"]));
                    lista.Add(categoriaNivel);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelCategoriaNivel(0,string.Empty,ex.Message.ToString(),ex.Message.ToString(),0,ex.Message.ToString()));
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
        public List<ModelCategoriaNivel> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelCategoriaNivel> lista = new List<ModelCategoriaNivel>();
            try
            {                
                ModelCategoriaNivel categoriaNivel;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,codigo,nombre,descripcion,estado FROM categorianivel WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda)+ "%') AND (estado<>"+(int)Estados.Tipos.Eliminado+") ORDER BY nombre ASC LIMIT "+paginacion+"");// LIMIT " + inicio + "," + cantidad + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    categoriaNivel = new ModelCategoriaNivel(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["codigo"].ToString()),Encryption.DecryptString( fila["nombre"].ToString() ),Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToInt32(fila["estado"]));
                    lista.Add(categoriaNivel);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelCategoriaNivel(0,string.Empty,ex.Message.ToString(),ex.Message.ToString(),0,ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del modelo
        /// </summary>
        /// <param name="estado">estado</param>
        /// <param name="orden">orden a ordenar [ASC o DESC]</param>
        /// <param name="campoOrden">campo sobre el cual se ordenaran los datos [Campo del modelo]</param>
        /// <returns></returns>
        public List<ModelCategoriaNivel> Listar(int estado,string orden,string campoOrden) {
            List<ModelCategoriaNivel> lista = new List<ModelCategoriaNivel>();
            try
            {
                ModelCategoriaNivel categoriaNivel;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,codigo,nombre,descripcion,estado FROM categorianivel WHERE estado=" + estado + " ORDER BY '"+campoOrden+"' '"+orden+"'");

                foreach (DataRow fila in listado.Rows)
                {
                    categoriaNivel = new ModelCategoriaNivel(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["codigo"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(categoriaNivel);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelCategoriaNivel(0,string.Empty, ex.Message.ToString(), ex.Message.ToString(), 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelCategoriaNivel> Listar(int PK) {
            List<ModelCategoriaNivel> lista = new List<ModelCategoriaNivel>();
            try
            {
                ModelCategoriaNivel categoria;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,codigo,descripcion,estado FROM categorianivel WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    categoria=new ModelCategoriaNivel(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["codigo"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()),Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToInt32(fila["estado"]));
                    lista.Add(categoria);
                }
                return lista;

            }
            catch (Exception ex) {                
                ModelCategoriaNivel categoria = new ModelCategoriaNivel(0,string.Empty ,string.Empty, string.Empty, 0,ex.Message.ToString());
                lista.Add(categoria);
                return lista;
            }
        }

        

        /// <summary>
        /// Metodo para buscar los nombres de la entidad similares al dado
        /// </summary>
        /// <param name="TextoBusqueda">Nombre a Buscar</param>
        /// <returns>Lista de nombres coincidentes</returns>
        public List<string> Listar(string TextoBusqueda,int Estado)
        {
            List<string> ListaAutocomplete = new List<string>();
            try
            {
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM categorianivel WHERE estado="+Estado+" AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
        
    }
}

