using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLL;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class ModelAula
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int Modulo_Id { get; set; }
        public string ModuloNombre { get; set; }
        public string ModuloDescripcion { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelAula(int pk, string nombre, string descripcion, int estado, int aulaId)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Modulo_Id = aulaId;
            this.Error = string.Empty;
        }

        /// <summary>
        /// constructor para devolver constructor join
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="estado"></param>
        /// <param name="aulaId"></param>
        public ModelAula(int pk, string nombre, string descripcion, int estado, int aulaId,string moduloName,string moduloDescripcion)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Modulo_Id = aulaId;
            this.ModuloNombre = moduloName;
            this.ModuloDescripcion = moduloDescripcion;
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
        public ModelAula(int pk, string nombre, string descripcion, int estado, int aulaId, string error)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Modulo_Id = aulaId;
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelAula()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerAula
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
        public ControllerAula()
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
        public string Insertar(ModelAula aula, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    query = "UPDATE aula SET nombre=@nombre,descripcion=@descripcion,estado=@estado,Modulo_Id=@moduloId,aula=@aula WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", aula.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = "INSERT INTO aula(nombre,descripcion,estado,Modulo_Id,user) VALUES(@nombre,@descripcion,@estado,@moduloId,@user)";
                }

                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(aula.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(aula.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramEstado = new MySqlParameter("estado", aula.Estado);
                parametros.Add(paramEstado);
                MySqlParameter paramModulo = new MySqlParameter("moduloId", aula.Modulo_Id);
                parametros.Add(paramModulo);
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
        public bool Eliminar(ModelAula aula)
        {
            try
            {
                string query = "DELETE FROM aula WHERE id=@id";
                MySqlParameter paramPk = new MySqlParameter("id", aula.PK);
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
                    string query = "SELECT COUNT(*) FROM aula WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.DecryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM aula WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="estado"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public int Count(string nombre, int estado,int modulo)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM aula WHERE nombre='"+nombre.ToUpper()+"' AND Modulo_id="+modulo+" AND estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
        public List<ModelAula> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelAula> lista = new List<ModelAula>();
            try
            {
                ModelAula aula;
                listado.Clear();
                listado = conexion.EjecutarSelect(@"SELECT a.id,a.nombre,a.descripcion,a.estado,a.modulo_id,m.Nombre as ModuloNombre,m.Descripcion as ModuloDescripcion 
                FROM aula a INNER JOIN Modulo m ON m.id=a.Modulo_Id WHERE a.estado=" + estado + " AND a.estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY a.nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    aula = new ModelAula(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["Modulo_Id"]),Encryption.DecryptString(fila["ModuloNombre"].ToString()),Encryption.DecryptString(fila["ModuloDescripcion"].ToString()));
                    lista.Add(aula);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelAula(0, ex.Message.ToString(), ex.Message.ToString(), 0, 0, ex.Message.ToString()));
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
        public List<ModelAula> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelAula> lista = new List<ModelAula>();
            try
            {
                ModelAula aula;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect(@"SELECT a.id,a.nombre,a.descripcion,a.estado,a.modulo_id,m.Nombre as ModuloNombre,m.Descripcion as ModuloDescripcion FROM aula a
                INNER JOIN Modulo m ON m.id=a.Modulo_Id WHERE a.estado=" + estado + " AND (a.nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");

                foreach (DataRow fila in listado.Rows)
                {
                    aula = new ModelAula(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["Modulo_Id"]), Encryption.DecryptString(fila["ModuloNombre"].ToString()), Encryption.DecryptString(fila["ModuloDescripcion"].ToString()));
                    lista.Add(aula);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelAula(0, ex.Message.ToString(), ex.Message.ToString(), 0, 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelAula> Listar(int PK)
        {
            List<ModelAula> lista = new List<ModelAula>();
            try
            {
                ModelAula aula;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,Modulo_Id FROM AULA WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    aula = new ModelAula(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["Modulo_Id"]));
                    lista.Add(aula);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelAula aula = new ModelAula(0, string.Empty, string.Empty, 0, 0, ex.Message.ToString());
                lista.Add(aula);
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,modulo_id FROM aula WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
