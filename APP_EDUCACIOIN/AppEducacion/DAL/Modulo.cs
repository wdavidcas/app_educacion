﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using BLL;

namespace DAL
{
    public class ModelModulo
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int Edificio_Id { get; set; }
        public string EdificioNombre { get; set; }
        public string EdificioDescripcion { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelModulo(int pk, string nombre, string descripcion, int estado, int EdificioId)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Edificio_Id = EdificioId;
            this.Error = string.Empty;
        }

        /// <summary>
        /// constructor para devolver edificio
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="estado"></param>
        /// <param name="EdificioId"></param>
        /// <param name="EdificioName"></param>
        public ModelModulo(int pk, string nombre, string descripcion, int estado, int EdificioId,string EdificioName,string EdificioDescripcion)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Edificio_Id = EdificioId;
            this.EdificioNombre = EdificioName;
            this.EdificioDescripcion = EdificioDescripcion;
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
        public ModelModulo(int pk, string nombre, string descripcion, int estado, int edificioId, string error)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Edificio_Id = edificioId;
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelModulo()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerModulo
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
        public ControllerModulo()
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
        /// <param name="nivel">objeto del modelo</param>
        /// <param name="Operacion">operacion a realizar [false=editar, true=agregar]</param>
        /// <returns></returns>
        public string Insertar(ModelModulo modulo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {                    
                    query = "UPDATE MODULO SET nombre=@nombre,descripcion=@descripcion,estado=@estado,Edificio_Id=@edificioId,user=@user WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", modulo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = "INSERT INTO MODULO(nombre,descripcion,estado,Edificio_Id,user) VALUES(@nombre,@descripcion,@estado,@EdificioId,@user)";
                }

                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modulo.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(modulo.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramEstado = new MySqlParameter("estado", modulo.Estado);
                parametros.Add(paramEstado);
                MySqlParameter paramEdificioId = new MySqlParameter("edificioId", modulo.Edificio_Id);
                parametros.Add(paramEdificioId);
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
        public bool Eliminar(ModelNivel nivel)
        {
            try
            {
                string query = "DELETE FROM Modulo WHERE id=@id";
                MySqlParameter paramPk = new MySqlParameter("id", nivel.PK);
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
                    string query = "SELECT COUNT(*) FROM MODULO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.DecryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM MODULO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
        public int Count(string nombre,int edificio,int estado)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM MODULO WHERE Edificio_id="+edificio+" AND estado="+estado+" AND nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
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
        public List<ModelModulo> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelModulo> lista = new List<ModelModulo>();
            try
            {
                ModelModulo modulo;
                listado.Clear();
                listado = conexion.EjecutarSelect(@"SELECT m.id,m.nombre,m.descripcion,m.estado,m.Edificio_Id,e.Nombre as EdificioNombre,e.Descripcion as EdificioDescripcion FROM MODULO m
                INNER JOIN Edificio e ON e.id=m.Edificio_id WHERE m.estado=" + estado + " AND m.estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY m.nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modulo = new ModelModulo(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["Edificio_Id"]),Encryption.DecryptString(fila["EdificioNombre"].ToString()),Encryption.DecryptString(fila["EdificioDescripcion"].ToString()));
                    lista.Add(modulo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelModulo(0, ex.Message.ToString(), ex.Message.ToString(), 0, 0, ex.Message.ToString()));
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
        public List<ModelModulo> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelModulo> lista = new List<ModelModulo>();
            try
            {
                ModelModulo nivel;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect(@"SELECT m.id,m.nombre,m.descripcion,m.estado,m.Edificio_Id,e.Nombre as EdificioNombre,e.Descripcion as EdificioDescripcion FROM MODULO m
                INNER JOIN Edificio e ON e.id=m.Edificio_id  WHERE m.estado=" + estado + " AND (m.nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR m.descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (m.estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY m.nombre ASC LIMIT " + paginacion + "");

                foreach (DataRow fila in listado.Rows)
                {
                    nivel = new ModelModulo(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["Edificio_Id"]),Encryption.DecryptString(fila["EdificioNombre"].ToString()),Encryption.DecryptString(fila["EdificioDescripcion"].ToString()));
                    lista.Add(nivel);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelModulo(0, ex.Message.ToString(), ex.Message.ToString(), 0, 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelModulo> Listar(int PK)
        {
            List<ModelModulo> lista = new List<ModelModulo>();
            try
            {
                ModelModulo modulo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,Edificio_Id FROM MODULO WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modulo = new ModelModulo(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["Edificio_id"]));
                    lista.Add(modulo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelModulo nivel = new ModelModulo(0, string.Empty, string.Empty, 0, 0, ex.Message.ToString());
                lista.Add(nivel);
                return lista;
            }
        }

        /// <summary>
        /// Lista los objetos del catalogo segun estado
        /// </summary>
        /// <param name="Estado"></param>
        /// <param name="Orden"></param>
        /// <param name="CampoOrden"></param>
        /// <returns></returns>
        public List<ModelModulo> Listar(int EdificioId,int Estado,string Orden,string CampoOrden)
        {
            List<ModelModulo> lista = new List<ModelModulo>();
            try
            {
                ModelModulo modulo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,Edificio_Id FROM MODULO WHERE Edificio_Id="+EdificioId+" and estado=" + Estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY "+CampoOrden+" "+Orden+"");

                foreach (DataRow fila in listado.Rows)
                {
                    modulo = new ModelModulo(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["Edificio_id"]));
                    lista.Add(modulo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelModulo nivel = new ModelModulo(0, string.Empty, string.Empty, 0, 0, ex.Message.ToString());
                lista.Add(nivel);
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,Edificio_id FROM MODULO WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
