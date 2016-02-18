using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using BLL;

namespace DAL
{
    public class ModelCurso
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string CodigoMineduc { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int Creditos {get;set;}
        public int Nivel_Id {get;set;}
        public string NivelNombre { get; set; }
        public int Categoriacurso_Id { get; set; }
        public string CategoriaCursoNombre { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelCurso(int pk,string codigoMineduc ,string nombre, string descripcion,int estado,int creditos,int nivelId,int categoriaCursoId)
        {
            this.PK = pk;
            this.CodigoMineduc = codigoMineduc;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Creditos=creditos;
            this.Nivel_Id=nivelId;
            this.Categoriacurso_Id = categoriaCursoId;
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
        public ModelCurso(int pk,string codigoMineduc, string nombre, string descripcion, int estado,int creditos,int nivelId,int categoriaCursoId, string error) {
            this.PK = pk;
            this.CodigoMineduc = codigoMineduc;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Creditos=creditos;
            this.Nivel_Id=nivelId;
            this.Categoriacurso_Id = categoriaCursoId;
            this.Error = error;
        }

        /// <summary>
        /// constructor para contruir objetos con entidades relacionadas
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="codigoMineduc"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="estado"></param>
        /// <param name="creditos"></param>
        /// <param name="nivelId"></param>
        /// <param name="categoriaCursoId"></param>
        /// <param name="NivelName"></param>
        /// <param name="categoriaCursoName"></param>
        /// <param name="error"></param>
        public ModelCurso(int pk, string codigoMineduc, string nombre, string descripcion, int estado, int creditos, int nivelId, int categoriaCursoId, string NivelName,string categoriaCursoName)
        {
            this.PK = pk;
            this.CodigoMineduc = codigoMineduc;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Creditos = creditos;
            this.Nivel_Id = nivelId;
            this.NivelNombre = NivelName;
            this.Categoriacurso_Id = categoriaCursoId;
            this.CategoriaCursoNombre = categoriaCursoName;
            this.Error = string.Empty;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelCurso()
        { 
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerCurso
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
        public ControllerCurso()
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
        public string Insertar(ModelCurso curso, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    MySqlParameter paramPk = new MySqlParameter("id", curso.PK);
                    query = "UPDATE CURSO SET nombre=@nombre,descripcion=@descripcion,estado=@estado,creditos=@creditos,Nivel_id=@nivel,categoriaCurso_id=@categoriaCurso WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", curso.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = "INSERT INTO CURSO(nombre,descripcion,estado,creditos,Nivel_Id,categoriaCurso_id) VALUES(@nombre,@descripcion,@estado,@creditos,@nivel,@categoriaCurso)";
                }
                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(curso.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(curso.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramEstado = new MySqlParameter("estado", curso.Estado);
                parametros.Add(paramEstado);
                MySqlParameter paramCreditos = new MySqlParameter("creditos", curso.Creditos);
                parametros.Add(paramCreditos);
                MySqlParameter paramNivel = new MySqlParameter("categoriaNivel", curso.Nivel_Id);
                parametros.Add(paramNivel);
                MySqlParameter paramCategoriaCurso = new MySqlParameter("categoriaCurso",curso.Categoriacurso_Id);
                parametros.Add(paramCategoriaCurso);

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
        public bool Eliminar(ModelCurso curso)
        {
            try
            {
                string query = "DELETE FROM CURSO WHERE id=@id";
                MySqlParameter paramPk = new MySqlParameter("id", curso.PK);
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
                    string query = "SELECT COUNT(*) FROM CURSO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.DecryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM CURSO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
        public List<ModelCurso> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelCurso> lista = new List<ModelCurso>();
            try
            {
                ModelCurso curso;
                listado.Clear();
                listado = conexion.EjecutarSelect(@"SELECT c.id,c.CodigoMineduc,c.nombre,c.descripcion,c.estado,c.credios,c.Nivel_Id,n.Nombre as NivelName,c.CategoriaCurso_Id,cc.NOmbre as CategoriaName FROM CURSO c 
                INNER JOIN Nivel n ON n.id=c.Nivel_Id INNER JOIN Categoria cc ON cc.id=c.Categoria_Id WHERE c.estado=" + estado + " AND c.estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY c.nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    curso = new ModelCurso(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["CodigoMineduc"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]),Convert.ToInt32(fila["creditos"]),Convert.ToInt32(fila["Nivel_Id"]),Convert.ToInt32(fila["CategoriaCurso_Id"]),Encryption.DecryptString(fila["NivelName"].ToString()),Encryption.DecryptString(fila["CategoriaName"].ToString()));
                    lista.Add(curso);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelCurso(0,ex.Message.ToString(), ex.Message.ToString(), ex.Message.ToString(), 0,0,0,0,ex.Message.ToString()));
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
        public List<ModelCurso> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelCurso> lista = new List<ModelCurso>();
            try
            {
                ModelCurso curso;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect(@"SELECT c.id,c.CodigoMineduc,c.nombre,c.descripcion,c.estado,c.creditos,c.Nivel_Id,n.Nombre as NivelName,c.CategoriaCurso_Id,cc.Nombre as CategoriaName FROM curso 
                INNER JOIN Nivel n ON n.id=c.Nivel_Id INNER JOIN Categoria cc ON cc.id=c.Categoria_Id WHERE c.estado=" + estado + " AND (c.nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR c.descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (c.estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY c.nombre ASC LIMIT " + paginacion + "");

                foreach (DataRow fila in listado.Rows)
                {
                    curso = new ModelCurso(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["CodigoMineduc"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]), Convert.ToInt32(fila["creditos"]), Convert.ToInt32(fila["Nivel_Id"]), Convert.ToInt32(fila["CategoriaCurso_Id"]), Encryption.DecryptString(fila["NivelName"].ToString()), Encryption.DecryptString(fila["CategoriaName"].ToString()));
                    lista.Add(curso);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelCurso(0, ex.Message.ToString(), ex.Message.ToString(), ex.Message.ToString(), 0, 0, 0, 0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelCurso> Listar(int PK)
        {
            List<ModelCurso> lista = new List<ModelCurso>();
            try
            {
                ModelCurso curso;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,codigoMineduc,nombre,descripcion,estado,Nivel_Id,CategoriaCurso_Id FROM CURSO WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    curso = new ModelCurso(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["CodigoMineduc"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]),Convert.ToInt32(fila["creditos"]),Convert.ToInt32(fila["Nivel_Id"]),Convert.ToInt32(fila["CategoriaCurso_Id"]));
                    lista.Add(curso);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelCurso curso = new ModelCurso(0,string.Empty, string.Empty, string.Empty, 0,0,0,0, ex.Message.ToString());
                lista.Add(curso);
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,estado FROM CURSO WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
