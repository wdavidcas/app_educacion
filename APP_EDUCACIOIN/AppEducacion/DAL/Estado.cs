using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using BLL;

namespace DAL
{
    public class ModelEstado
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int TipoEstado_Id { get; set; }
        public string TipoEstadoName { get; set; }
        public string TipoEstadoNivelDescripcion { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelEstado(int pk, string nombre, string descripcion, int estado, int tipoEstado_Id)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.TipoEstado_Id = tipoEstado_Id;
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
        public ModelEstado(int pk, string nombre, string descripcion, int estado, int tipoEstado_Id, string error)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.TipoEstado_Id = tipoEstado_Id;
            this.Error = error;
        }

        /// <summary>
        /// constructor para regresar en la vista
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="estado"></param>
        /// <param name="CategoriaNivel"></param>
        /// <param name="categoriaNivelName"></param>
        /// <param name="categoriaNivelDescripcion"></param>
        public ModelEstado(int pk, string nombre, string descripcion, int estado, int tipoEstado_Id, string tipoEstadoNombre, string tipoEstadoDescripcion)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.TipoEstado_Id = tipoEstado_Id;
            this.TipoEstadoName = tipoEstadoNombre;
            this.TipoEstadoNivelDescripcion = tipoEstadoDescripcion;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelEstado()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerEstado
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
        public ControllerEstado()
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
        public string Insertar(ModelEstado modelo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {                    
                    query = "UPDATE ESTADO SET nombre=@nombre,descripcion=@descripcion,estado=@estado,TipoEstado_IdTipoEstado=@tipoEstadoId Id WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", modelo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = "INSERT INTO ESTADO(nombre,descripcion,estado,TipoEstado_IdTipoEstado) VALUES(@nombre,@descripcion,@estado,@tipoEstadoId)";
                }

                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modelo.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(modelo.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramEstado = new MySqlParameter("estado", modelo.Estado);
                parametros.Add(paramEstado);
                MySqlParameter paramCategoriaNivel = new MySqlParameter("tipoEstadoId", modelo.TipoEstado_Id);
                parametros.Add(paramCategoriaNivel);
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
        public bool Eliminar(ModelEstado modelo)
        {
            try
            {
                string query = "DELETE FROM ESTADO WHERE id=@id";
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
                    string query = "SELECT COUNT(*) FROM ESTADO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.DecryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM ESTADO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
                    return conexion.EjecutarQueryCount(query);
                }
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
        public List<ModelEstado> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelEstado> lista = new List<ModelEstado>();
            try
            {
                ModelEstado modelo;
                listado.Clear();
                listado = conexion.EjecutarSelect(@"SELECT a.id,a.nombre,a.descripcion,a.estado,a.TipoEstado_IdTipoEstado as TipoEstado_Id,b.nombre as TipoEstadoName,b.descripcion as TipoEstadoDescripcion FROM ESTADO a
                INNER JOIN TIPOESTADO b ON a.TipoEstado_idTipoEstado=b.idTipoEstado WHERE a.estado=" + estado + " AND a.estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY a.nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelEstado(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["TipoEstado_Id"]), Encryption.DecryptString(fila["TipoEstadoNombre"].ToString()), Encryption.DecryptString(fila["TipoEstadoDescripcion"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelEstado (0, ex.Message.ToString(), ex.Message.ToString(), 0, 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos .del catalogo de acuerdo a 
        /// </summary>
        /// <param name="inicio">inicio</param>
        /// <param name="paginacion">cantidad de registros</param>
        ///<param name="busqueda">texto a buscar</param>    
        /// <param name="estado">estado del registro 1=Habilitado 2=No Habilitado 3=Eliminado</param>        
        /// <returns></returns>
        public List<ModelEstado> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelEstado> lista = new List<ModelEstado>();
            try
            {
                ModelEstado modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect(@"SELECT a.id,a.nombre,a.descripcion,a.estado,a.TipoEstado_IdTipoEstado as TipoEstado_Id,b.nombre as TipoEstadoNombre,b.descripcion as tipoEstadoDescripcion FROM ESTADO a
                INNER JOIN TIPOESTADO b ON a.TipoEstado_idTipoEstado=b.idTipoEstado WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelEstado(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["TipoEstado_Id"]), Encryption.DecryptString(fila["TipoEstadoNombre"].ToString()), Encryption.DecryptString(fila["TipoEstadoDescripcion"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelEstado(0, ex.Message.ToString(), ex.Message.ToString(), 0, 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelEstado> Listar(int PK)
        {
            List<ModelEstado> lista = new List<ModelEstado>();
            try
            {
                ModelEstado modelo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,CategoriaNivel_id FROM nivel WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelEstado(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["CategoriaNivel_id"]));
                    lista.Add(modelo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelEstado modelo = new ModelEstado(0, string.Empty, string.Empty, 0, 0, ex.Message.ToString());
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,TipoEstado_IdTipoEstado FROM ESTADO WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
