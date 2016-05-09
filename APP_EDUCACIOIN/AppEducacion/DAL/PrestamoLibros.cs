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
    /// Modelo de la tabla Libros
    /// </summary>
    public class ModelPrestamoLibros
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public DataTable FechaPrestamo { get;set; }
        public DataTable FechaDevolucion { get;set; }
        public int Cantidad { get;set; }
        public int Alumno_Id { get;set; }
        public int Libros_IdLibros { get;set; }
        public int DiasRetraso { get;set; }
        public string Descripcion { get;set; }        
        public int Estado { get; set; }        
        public string Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="fechaPrestamo"></param>
        /// <param name="fechaDevolucion"></param>
        /// <param name="cantidad"></param>
        /// <param name="alumnoId"></param>
        /// <param name="LibrosId"></param>
        /// <param name="diasRetraso"></param>
        /// <param name="descripcion"></param>
        /// <param name="estado"></param>
        public ModelPrestamoLibros(int pk, DateTime fechaPrestamo,DateTime fechaDevolucion,int cantidad,int alumnoId,int LibrosId,int diasRetraso,string descripcion,int estado)
        {
            this.PK = pk;
            this.FechaPrestamo = FechaPrestamo;
            this.FechaDevolucion = FechaDevolucion;
            this.Cantidad = cantidad;
            this.Alumno_Id = alumnoId;
            this.Libros_IdLibros = LibrosId;
            this.DiasRetraso = diasRetraso;
            this.Descripcion = descripcion;
            this.Estado = estado;
            this.Error = string.Empty;
        }

        /// <summary>
        /// constructor del modelo para capturar errores
        /// </summary>
        /// <param name="pk">llave</param>
        /// <param name="error">error</param>
        public ModelPrestamoLibros(int pk, string error)
        {
            this.PK = pk;
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelPrestamoLibros()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerPrestamoLibros
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
        public ControllerPrestamoLibros()
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
        /// Metodo para insertar/actualizar registros del modelo
        /// </summary>
        /// <param name="modelo">objeto del modelo</param>
        /// <param name="Operacion">operacion a realizar: True=Insertar, False=Actualizar</param>
        /// <returns></returns>
        public string Insertar(ModelLibros modelo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    query = "UPDATE LIBROS SET nombre=@nombre,autor=@autor,editorial=@editorial,codigo=@codigo,descripcion=@descripcion,cantidad=@cantidad,cantidadDisponible=@cantidadDisponible,paginas=@paginas,estado=@estado,TIPOLIBRO_idTIPOLIBRO=@tipolibro,user=@user WHERE idLIBROS=@id";
                    MySqlParameter paramId = new MySqlParameter("idLIBROS", modelo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = @"INSERT INTO LIBROS(nombre,autor,editorial,codigo,descripcion,cantidad,cantidadDisponible,paginas,estado,TIPOLIBRO_idTIPOLIBRO,user) 
                            VALUES(@nombre,@autor,@editorial,@codigo,@descripcion,@cantidad,@cantidadDisponible,@paginas,@estado,@tipolibro,@user)";
                }
                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modelo.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramAutor = new MySqlParameter("autor", Encryption.EncryptString(modelo.Autor));
                parametros.Add(paramAutor);
                MySqlParameter paramEditorial = new MySqlParameter("editorial", Encryption.EncryptString(modelo.Editorial));
                parametros.Add(paramEditorial);
                MySqlParameter paramCodigo = new MySqlParameter("codigo", Encryption.EncryptString(modelo.Codigo));
                parametros.Add(paramCodigo);
                MySqlParameter paramCantidad = new MySqlParameter("cantidad", modelo.Cantidad);
                parametros.Add(paramCantidad);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(modelo.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramcantidadDisponible = new MySqlParameter("cantidadDisponible", modelo.CantidadDisponible);
                parametros.Add(paramcantidadDisponible);
                MySqlParameter paramPaginas = new MySqlParameter("paginas", modelo.Paginas);
                parametros.Add(paramPaginas);
                MySqlParameter paramEstado = new MySqlParameter("estado", modelo.Estado);
                parametros.Add(paramEstado);
                MySqlParameter paramTipoLibro = new MySqlParameter("tipolibro", modelo.TipoLibro);
                parametros.Add(paramTipoLibro);
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
        public bool Eliminar(ModelTiposTelefono modelo)
        {
            try
            {
                string query = "DELETE FROM LIBROS WHERE idLIBROS=@id";
                MySqlParameter paramPk = new MySqlParameter("idLIBROS", modelo.PK);
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
                    string query = "SELECT COUNT(*) FROM LIBROS WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM LIBROS WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
                string query = "SELECT COUNT(*) FROM LIBROS WHERE nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
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
        public List<ModelLibros> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelLibros> lista = new List<ModelLibros>();
            try
            {
                ModelLibros modelo;
                listado.Clear();
                listado = conexion.EjecutarSelect("SELECT idLIBROS as id,nombre,autor,editorial,codigo,descripcion,cantidad,cantidadDisponible,paginas,TIPOLIBRO_idTIPOLIBRO as tipoLibro,estado FROM LIBROS WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelLibros(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["autor"].ToString()), Encryption.DecryptString(fila["editorial"].ToString()), Encryption.DecryptString(fila["codigo"].ToString()), Convert.ToInt32(fila["paginas"].ToString()), Convert.ToInt32(fila["cantidad"].ToString()), Convert.ToInt32(fila["cantidadDisponible"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"].ToString()), Convert.ToInt32(fila["tipolibro"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelLibros(0, ex.Message.ToString()));
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
        public List<ModelLibros> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelLibros> lista = new List<ModelLibros>();
            try
            {
                ModelLibros modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idLIBROS as id,nombre,autor,editorial,codigo,descripcion,cantidad,cantidadDisponible,paginas,TIPOLIBRO_idTIPOLIBRO as tipoLibro,estado FROM LIBROS WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");// LIMIT " + inicio + "," + cantidad + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelLibros(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["autor"].ToString()), Encryption.DecryptString(fila["editorial"].ToString()), Encryption.DecryptString(fila["codigo"].ToString()), Convert.ToInt32(fila["paginas"].ToString()), Convert.ToInt32(fila["cantidad"].ToString()), Convert.ToInt32(fila["cantidadDisponible"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"].ToString()), Convert.ToInt32(fila["tipolibro"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelLibros(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del catalogo
        /// </summary>
        /// <param name="estado">estado</param>
        /// <returns>array de objetos</returns>
        public List<ModelLibros> Listar(int estado, string orden, string campoOrden)
        {
            List<ModelLibros> lista = new List<ModelLibros>();
            try
            {
                ModelLibros modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idLIBROS as id,nombre,autor,editorial,codigo,descripcion,cantidad,cantidadDisponible,paginas,TIPOLIBRO_idTIPOLIBRO as tipoLibro,estado FROM LIBROS WHERE estado=" + estado + " ORDER BY '" + campoOrden + "' '" + orden + "'");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelLibros(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["autor"].ToString()), Encryption.DecryptString(fila["editorial"].ToString()), Encryption.DecryptString(fila["codigo"].ToString()), Convert.ToInt32(fila["paginas"].ToString()), Convert.ToInt32(fila["cantidad"].ToString()), Convert.ToInt32(fila["cantidadDisponible"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"].ToString()), Convert.ToInt32(fila["tipolibro"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelLibros(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelLibros> Listar(int PK)
        {
            List<ModelLibros> lista = new List<ModelLibros>();
            try
            {
                ModelLibros modelo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idLIBROS as id,nombre,autor,editorial,codigo,descripcion,cantidad,cantidadDisponible,paginas,TIPOLIBRO_idTIPOLIBRO as tipoLibro,estado FROM LIBROS WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelLibros(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["autor"].ToString()), Encryption.DecryptString(fila["editorial"].ToString()), Encryption.DecryptString(fila["codigo"].ToString()), Convert.ToInt32(fila["paginas"].ToString()), Convert.ToInt32(fila["cantidad"].ToString()), Convert.ToInt32(fila["cantidadDisponible"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"].ToString()), Convert.ToInt32(fila["tipolibro"].ToString()));
                    lista.Add(modelo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                lista.Add(new ModelLibros(0, ex.Message.ToString()));
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
                listado = conexion.EjecutarSelect("SELECT idTIPOLIBRO as id,nombre,descripcion,estado FROM LIBROS WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
