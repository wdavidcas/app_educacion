using System;
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
    /// Modelo de la tabla TIPOPAGOS
    /// </summary>
    public class ModelActividadUnidad
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get;set; }
        public int PuntosAsignados { get;set; }
        public int PuntosGanados { get;set; }
        public int Unidad_id { get;set; }
        public int TIPO_ACTIVIDAD_id {get;set; }
        public int Estado { get; set; }
        public string Error { get; set; }


        /// <summary>
        /// constructor del modelo
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="fecha"></param>
        /// <param name="puntosAsignados"></param>
        /// <param name="puntosGanados"></param>
        /// <param name="unidadId"></param>
        /// <param name="tipoActividad"></param>
        /// <param name="estado"></param>
        public ModelActividadUnidad(int pk, string nombre, string descripcion, DateTime fecha,int puntosAsignados,int puntosGanados,int unidadId,int tipoActividad, int estado)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Fecha=fecha;
            this.PuntosAsignados=puntosAsignados;
            this.PuntosGanados=puntosGanados;
            this.Unidad_id=unidadId;
            this.TIPO_ACTIVIDAD_id=tipoActividad;
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
        public ModelActividadUnidad(int pk, string error)
        {
            this.PK = pk;
            this.Nombre = string.Empty;
            this.Descripcion = string.Empty;
            this.Estado = 0;
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelActividadUnidad()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerActividaUnidad
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
        public ControllerActividaUnidad()
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
        public string Insertar(ModelActividadUnidad modelo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    query = "UPDATE ACTIVIDAD SET nombre=@nombre,descripcion=@descripcion,fecha=@fecha,puntosAsignados=@puntosasignados,puntosGanados=@puntosganados, estado=@estado,Unidad_id=@unidad,TIPO_ACTIVIDAD_id=@tipoactividad,user=@user WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", modelo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = @"INSERT INTO ACTIVIDAD(nombre,descripcion,fecha,puntosAsignados,puntosGanados,Unidad_id,TIPO_ACTIVIDAD_id,estado,user) 
                            VALUES(@nombre,@descripcion,@puntosasignados,@puntosganados,@unidad,@tipoactividad,@estado,@user)";
                }
                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modelo.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(modelo.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramAsignados = new MySqlParameter("puntosasignados", modelo.PuntosAsignados);
                parametros.Add(paramAsignados);
                MySqlParameter paramGanados = new MySqlParameter("puntosganados", modelo.PuntosGanados);
                parametros.Add(paramGanados);
                MySqlParameter paramFecha = new MySqlParameter("fecha", modelo.Fecha);
                parametros.Add(paramFecha);
                MySqlParameter paramUnidad = new MySqlParameter("unidad",modelo.Unidad_id);
                parametros.Add(paramUnidad);
                MySqlParameter paramTipoActividad = new MySqlParameter("tipoactividad",modelo.TIPO_ACTIVIDAD_id);
                parametros.Add(paramTipoActividad);
                MySqlParameter paramEstado = new MySqlParameter("estado", modelo.Estado);
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
        public bool Eliminar(ModelActividadUnidad modelo)
        {
            try
            {
                string query = "DELETE FROM ACTIVIDAD WHERE id=@id";
                MySqlParameter paramPk = new MySqlParameter("id", modelo.PK);
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
                    string query = "SELECT COUNT(*) FROM ACTIVIDAD WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM ACTIVIDAD WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
                string query = "SELECT COUNT(*) FROM ACTIVIDAD WHERE nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
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
        public List<ModelTiposPagos> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelTiposPagos> lista = new List<ModelTiposPagos>();
            try
            {
                ModelTiposPagos modelo;
                listado.Clear();
                listado = conexion.EjecutarSelect("SELECT idTIPOPAGO as id,nombre,descripcion,valor,descuento,montoMora,estado FROM TIPOPAGOS WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelTiposPagos(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToDouble(fila["valor"].ToString()), Convert.ToDouble(fila["montoMora"].ToString()), Convert.ToDouble(fila["descuento"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelTiposPagos(0, ex.Message.ToString()));
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
        public List<ModelTiposPagos> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelTiposPagos> lista = new List<ModelTiposPagos>();
            try
            {
                ModelTiposPagos modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idTIPOPAGO as id,nombre,descripcion,valor,descuento,montoMora,estado FROM TIPOPAGOS WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");// LIMIT " + inicio + "," + cantidad + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelTiposPagos(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToDouble(fila["valor"].ToString()), Convert.ToDouble(fila["montoMora"].ToString()), Convert.ToDouble(fila["descuento"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelTiposPagos(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del catalogo
        /// </summary>
        /// <param name="estado">estado</param>
        /// <returns>array de objetos</returns>
        public List<ModelTiposPagos> Listar(int estado, string orden, string campoOrden)
        {
            List<ModelTiposPagos> lista = new List<ModelTiposPagos>();
            try
            {
                ModelTiposPagos modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idTIPOPAGO as id,nombre,descripcion,valor,descuento,montoMora,estado FROM TIPOPAGOS WHERE estado=" + estado + " ORDER BY '" + campoOrden + "' '" + orden + "'");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelTiposPagos(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToDouble(fila["valor"].ToString()), Convert.ToDouble(fila["montoMora"].ToString()), Convert.ToDouble(fila["descuento"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelTiposPagos(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelTiposPagos> Listar(int PK)
        {
            List<ModelTiposPagos> lista = new List<ModelTiposPagos>();
            try
            {
                ModelTiposPagos modelo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idTIPOPAGO as id,nombre,descripcion,valor,descuento,montoMora,estado FROM TIPOPAGOS WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelTiposPagos(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToDouble(fila["valor"].ToString()), Convert.ToDouble(fila["montoMora"].ToString()), Convert.ToDouble(fila["descuento"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelTiposPagos modelo = new ModelTiposPagos(0, string.Empty);
                lista.Add(modelo);
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
                listado = conexion.EjecutarSelect("SELECT idTIPOPAGO as id,nombre,descripcion,valor,descuento,montoMora,estado FROM TIPOPAGOS FROM TIPOLIBROS WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
