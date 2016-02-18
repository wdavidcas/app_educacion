using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLL;
using MySql.Data.MySqlClient;


namespace DAL
{
    //*********************************MODELO*******************************************
    /// <summary>
    /// Modelo de la tabla PROFESORES
    /// </summary>
    public class ModelProfesor
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DPI { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int ProfesionId { get; set; }
        public int Estado { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor del objeto del modelo
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="dpi"></param>
        /// <param name="direccion"></param>
        /// <param name="correo"></param>
        /// <param name="descripcion"></param>
        /// <param name="fechaNac"></param>
        /// <param name="profesionId"></param>
        /// <param name="estado"></param>
        public ModelProfesor(int pk, string nombre,string apellido,string dpi,string direccion,string correo,string descripcion,DateTime fechaNac,int profesionId , int estado)
        {
            this.PK = pk;
            this.Nombre=nombre;
            this.Apellido=apellido;
            this.DPI=dpi;
            this.Direccion=direccion;
            this.Correo=correo;
            this.Descripcion=descripcion;
            this.FechaNacimiento=fechaNac;
            this.ProfesionId=profesionId;
            this.Estado = estado;
            this.Error = string.Empty;
        }

        /// <summary>
        /// constructor del objeto del modelo
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="error"></param>
        public ModelProfesor(int pk, string error)
        {
            this.PK = pk;            
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelProfesor()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerProfesor
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
        public ControllerProfesor()
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
        public string Insertar(ModelProfesor modelo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    query = "UPDATE PROFESOR SET nombre=@nombre,apellido=@apellido,direccion=@direccion,correo=@correo,descripcion=@descripcion,fechaNacimiento=@fechaNacimiento,cui=@cui,PROFESION_idPROFESION=@profesion,estado=@estado,user=@user WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", modelo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = @"INSERT INTO PROFESOR(nombre,apellido,direccion,correo,descripcion,fechaNacimiento,cui,PROFESION_idPROFESION,estado,user) 
                            VALUES(@nombre,@apellido,@direccion,@correo,@descripcion,@fechaNacimiento,@cui,@profesion,@estado,@user)";
                }
                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modelo.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramApellido = new MySqlParameter("apellido", Encryption.DecryptString(modelo.Apellido));
                parametros.Add(paramApellido);
                MySqlParameter paramDireccion = new MySqlParameter("direccion", Encryption.DecryptString(modelo.Direccion));
                parametros.Add(paramDireccion);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(modelo.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramCorreo = new MySqlParameter("correo", Encryption.DecryptString(modelo.Correo));
                parametros.Add(paramCorreo);
                MySqlParameter paramCui = new MySqlParameter("cui", modelo.DPI);
                parametros.Add(paramCui);
                MySqlParameter paramProfesion = new MySqlParameter("profesion", modelo.ProfesionId);
                parametros.Add(paramProfesion);
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
        public bool Eliminar(ModelTiposPagos modelo)
        {
            try
            {
                string query = "DELETE FROM PROFESOR WHERE id=@id";
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
                    string query = "SELECT COUNT(*) FROM PROFESOR WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR apellido LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM PROFESOR WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
                string query = "SELECT COUNT(*) FROM PROFESOR WHERE nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
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
        public List<ModelProfesor> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelProfesor> lista = new List<ModelProfesor>();
            try
            {
                ModelProfesor modelo;
                listado.Clear();
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,cui,direccion,correo,descripcion,fechaNacimiento,Profesion_idProfesion as profesion,estado FROM PROFESOR WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelProfesor(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()), Encryption.DecryptString(fila["cui"].ToString()), Encryption.DecryptString(fila["direccion"].ToString()),Encryption.DecryptString(fila["correo"].ToString()),Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToDateTime(fila["fechaNacimiento"].ToString()),Convert.ToInt32(fila["profesion"]),Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelProfesor(0, ex.Message.ToString()));
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
        public List<ModelProfesor> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelProfesor> lista = new List<ModelProfesor>();
            try
            {
                ModelProfesor modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,cui,direccion,correo,descripcion,fechaNacimiento,Profesion_idProfesion as profesion,estado FROM PROFESOR WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR apellido LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");// LIMIT " + inicio + "," + cantidad + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelProfesor(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()), Encryption.DecryptString(fila["cui"].ToString()), Encryption.DecryptString(fila["direccion"].ToString()), Encryption.DecryptString(fila["correo"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToDateTime(fila["fechaNacimiento"].ToString()), Convert.ToInt32(fila["profesion"]), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelProfesor(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del catalogo
        /// </summary>
        /// <param name="estado">estado</param>
        /// <returns>array de objetos</returns>
        public List<ModelProfesor> Listar(int estado, string orden, string campoOrden)
        {
            List<ModelProfesor> lista = new List<ModelProfesor>();
            try
            {
                ModelProfesor modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,cui,direccion,correo,descripcion,fechaNacimiento,Profesion_idProfesion as profesion,estado FROM PROFESOR WHERE estado=" + estado + " ORDER BY '" + campoOrden + "' '" + orden + "'");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelProfesor(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()), Encryption.DecryptString(fila["cui"].ToString()), Encryption.DecryptString(fila["direccion"].ToString()), Encryption.DecryptString(fila["correo"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToDateTime(fila["fechaNacimiento"].ToString()), Convert.ToInt32(fila["profesion"]), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelProfesor(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelProfesor> Listar(int PK)
        {
            List<ModelProfesor> lista = new List<ModelProfesor>();
            try
            {
                ModelProfesor modelo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,cui,direccion,correo,descripcion,fechaNacimiento,Profesion_idProfesion as profesion,estado FROM PROFESOR WHERE WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelProfesor(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()), Encryption.DecryptString(fila["cui"].ToString()), Encryption.DecryptString(fila["direccion"].ToString()), Encryption.DecryptString(fila["correo"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToDateTime(fila["fechaNacimiento"].ToString()), Convert.ToInt32(fila["profesion"]), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelProfesor modelo = new ModelProfesor(0, string.Empty);
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,cui,direccion,correo,descripcion,fechaNacimiento,Profesion_idProfesion as profesion,estado FROM PROFESOR WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
