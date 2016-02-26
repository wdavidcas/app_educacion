using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLL;
using MySql.Data.MySqlClient;

namespace DAL
{
    
    public class ModelNivel
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int CategoriaNivel_Id {get;set;}
        public string CategoriaNivel { get; set; }
        public string CategoriaNivelDescripcion { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelNivel(int pk,string codigo, string nombre, string descripcion,int estado,int CategoriaNivel)
        {
            this.PK = pk;
            this.Codigo = codigo;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.CategoriaNivel_Id=CategoriaNivel;
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
        public ModelNivel(int pk,string codigo, string nombre, string descripcion, int estado,int CategoriaNivel, string error) {
            this.PK = pk;
            this.Codigo = codigo;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.CategoriaNivel_Id=CategoriaNivel;
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
        public ModelNivel(int pk,string codigo, string nombre, string descripcion, int estado, int CategoriaNivel, string categoriaNivelName, string categoriaNivelDescripcion) {
            this.PK = pk;
            this.Codigo = codigo;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.CategoriaNivel_Id = CategoriaNivel;
            this.CategoriaNivel = categoriaNivelName;
            this.CategoriaNivelDescripcion = CategoriaNivelDescripcion;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelNivel()
        { 
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerNivel
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
        public ControllerNivel()
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
        public string Insertar(ModelNivel nivel,bool Operacion)
        {
            try
            {
                string query=string.Empty;

                if (!Operacion)
                {
                    MySqlParameter paramPk = new MySqlParameter("id", nivel.PK);
                    query = "UPDATE Nivel SET nombre=@nombre,descripcion=@descripcion,estado=@estado,CategoriaNivel_id=@categoriaNivelId,user=@user,codigo=@codigo WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id",nivel.PK);
                    parametros.Add(paramId);
                }
                else{
                    query="INSERT INTO Nivel(nombre,descripcion,estado,CategoriaNivel_id,user,codigo) VALUES(@nombre,@descripcion,@estado,@CategoriaNivelId,@user,@codigo)";
                }
                MySqlParameter paramCodigo = new MySqlParameter("codigo", Encryption.DecryptString(nivel.Codigo));
                parametros.Add(paramCodigo);
                MySqlParameter paramNombre = new MySqlParameter("nombre",Encryption.EncryptString(nivel.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion",Encryption.EncryptString(nivel.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramEstado = new MySqlParameter("estado",nivel.Estado);
                parametros.Add(paramEstado);
                MySqlParameter paramCategoriaNivel = new MySqlParameter("categoriaNivelId", nivel.CategoriaNivel_Id);
                parametros.Add(paramCategoriaNivel);
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
        public bool Eliminar(ModelNivel nivel)
        {
            try
            {
                string query = "DELETE FROM Nivel WHERE id=@id";
                MySqlParameter paramPk = new MySqlParameter("id", nivel.PK);
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
                    string query = "SELECT COUNT(*) FROM nivel WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.DecryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query="SELECT COUNT(*) FROM nivel WHERE estado="+estado+" AND estado<>"+(int)Estados.Tipos.Eliminado+"";
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
        public int Count(string nombre)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM LIBROS WHERE nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
                return conexion.EjecutarQueryCount(query);
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return 0;
            }
        }

        /// <summary>
        /// verifica la existencia de un objeto con el mismo codigo
        /// </summary>
        /// <param name="codigo">codigo del objeto del modelo</param>
        /// <returns></returns>
        public int Count(string codigo,bool operacion)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM LIBROS WHERE codigo='" + Encryption.EncryptString(codigo.ToUpper()) + "'";
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
        public List<ModelNivel> Listar(int inicio,int paginacion,int estado)
        {
            List<ModelNivel> lista = new List<ModelNivel>();
            try
            {                
                ModelNivel Nivel;
                listado.Clear();                
                listado = conexion.EjecutarSelect(@"SELECT a.id,a.codigo,a.nombre,a.descripcion,a.estado,a.categoriaNivel_id,b.nombre as categoriaNivelName,b.descripcion as categoriaNivelDescripcion FROM NIVEL a
                INNER JOIN CategoriaNivel b ON a.CategoriaNivel_id=b.id WHERE a.estado="+estado+" AND a.estado<>" + (int) Estados.Tipos.Eliminado + " ORDER BY a.nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    Nivel = new ModelNivel(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["codigo"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["categoriaNivel_id"]), Encryption.DecryptString(fila["categoriaNivelName"].ToString()), Encryption.DecryptString(fila["categoriaNivelDescripcion"].ToString()));
                    lista.Add(Nivel);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelNivel(0,string.Empty, ex.Message.ToString(),ex.Message.ToString(),0,0,ex.Message.ToString()));
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
        public List<ModelNivel> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelNivel> lista = new List<ModelNivel>();
            try
            {                
                ModelNivel nivel;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect(@"SELECT a.id,a.codigo,a.nombre,a.descripcion,a.estado,a.categoriaNivel_id,b.nombre as categoriaNivelName,b.descripcion as categoriaNivelDescripcion FROM NIVEL a
                INNER JOIN CategoriaNivel b ON a.CategoriaNivel_id=b.id WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");

                foreach (DataRow fila in listado.Rows)
                {
                    nivel = new ModelNivel(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["codigo"].ToString()),Encryption.DecryptString( fila["nombre"].ToString() ),Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToInt32(fila["estado"]),Convert.ToInt32(fila["CategoriaNivel_id"]),Encryption.DecryptString(fila["categoriaNivelName"].ToString()),Encryption.DecryptString(fila["categoriaNivelDescripcion"].ToString()));
                    lista.Add(nivel);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelNivel(0,string.Empty, ex.Message.ToString(),ex.Message.ToString(),0,0,ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelNivel> Listar(int PK) {
            List<ModelNivel> lista = new List<ModelNivel>();
            try
            {
                ModelNivel nivel;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,codigo,nombre,descripcion,estado,CategoriaNivel_id FROM nivel WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    nivel=new ModelNivel(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["codigo"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()),Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToInt32(fila["estado"]),Convert.ToInt32(fila["CategoriaNivel_id"]));
                    lista.Add(nivel);
                }
                return lista;

            }
            catch (Exception ex) {                
                ModelNivel nivel = new ModelNivel(0,string.Empty, string.Empty, string.Empty, 0,0,ex.Message.ToString());
                lista.Add(nivel);
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado,CategoriaNivel_id FROM nivel WHERE estado="+Estado+" AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
