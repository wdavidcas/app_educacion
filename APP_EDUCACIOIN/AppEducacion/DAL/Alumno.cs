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
    /// Modelo de la tabla CategoriaNivel
    /// </summary>
    public class ModelAlumno
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cui { get; set; }
        public string Direccion { get; set; }
        public string Carnet { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Fotografía { get; set; }        
        public int Estado { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelAlumno(int pk, string nombres, string apellidos, string direccion,string carnet,string correo,DateTime fechaNacimiento,string foto,string cui,int estado)
        {
            this.PK = pk;
            this.Nombre = nombres;
            this.Apellido = apellidos;
            this.Direccion=direccion;
            this.Carnet=carnet;
            this.Correo=correo;
            this.FechaNacimiento=fechaNacimiento;
            this.Fotografía=foto;
            this.Cui=cui;
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
        public ModelAlumno(int pk, string nombres, string apellidos, string direccion,string carnet,string correo,DateTime fechaNacimiento,string foto,string cui,int estado,string error)
        {
            this.PK = pk;
            this.Nombre = nombres;
            this.Apellido = apellidos;
            this.Direccion=direccion;
            this.Carnet=carnet;
            this.Correo=correo;
            this.FechaNacimiento=fechaNacimiento;
            this.Fotografía=foto;
            this.Cui=cui;
            this.Estado = estado;
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelAlumno()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerAlumno
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
        public ControllerAlumno()
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
        public string Insertar(ModelAlumno modelo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {                    
                    query = "UPDATE ALUMNO SET nombre=@nombre,apellido=@apellido,direccion=@direccion,carnet=@carnet,correo=@correo,fechaNacimiento=@fechaNacimiento,fotografia=@foto,cui=@cui,estado=@estado,user=@user WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", modelo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = "INSERT INTO ALUMNO(nombre,apellido,direccion,carnet,correo,fechaNacimiento,fotografia,cui,estado,user) VALUES(@nombre,@aapellido,@direccion,@carnet,@correo,@fechaNacimiento,@foto,@cui,@estado,@user)";
                }
                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modelo.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramApellido = new MySqlParameter("descripcion", Encryption.EncryptString(modelo.Apellido));
                parametros.Add(paramApellido);
                MySqlParameter paramDireccion = new MySqlParameter("direccion", Encryption.EncryptString(modelo.Direccion));
                parametros.Add(paramDireccion);
                MySqlParameter paramCarnet = new MySqlParameter("carnet", Encryption.EncryptString(modelo.Carnet));
                parametros.Add(paramCarnet);
                MySqlParameter paramCorreo = new MySqlParameter("correo", Encryption.EncryptString(modelo.Correo));
                parametros.Add(paramCorreo);
                MySqlParameter paramFechaNac = new MySqlParameter("fechaNacimiento", Encryption.EncryptString(modelo.FechaNacimiento.ToShortDateString()));
                parametros.Add(paramFechaNac);
                MySqlParameter paramFoto=new MySqlParameter("foto",modelo.Fotografía);
                parametros.Add(paramFoto);
                MySqlParameter paramCui = new MySqlParameter("cui", Encryption.EncryptString(modelo.Cui));
                parametros.Add(paramCui);
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
        public bool Eliminar(ModelAlumno modelo)
        {
            try
            {
                string query = "DELETE FROM ALUMNO WHERE id=@id";
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
                    string query = "SELECT COUNT(*) FROM ALUMNO WHERE Estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR apellido LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM ALUMNO WHERE Estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
        public List<ModelAlumno> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelAlumno> lista = new List<ModelAlumno>();
            try
            {
                ModelAlumno modelo;
                listado.Clear();
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,direccion,carnet,correo,fechaNacimiento,cui,fotografia,estado FROM ALUMNO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {                    
                    //modelo = new ModelAlumno(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()),Encryption.DecryptString(fila["direccion"].ToString()),Encryption.DecryptString(fila["carnet"].ToString()),Encryption.DecryptString(fila["correo"].ToString()),Convert.ToDateTime(fila["fechaNacimiento"].ToString()),Encryption.DecryptString(fila["fotografia"].ToString()),Encryption.DecryptString(fila["cui"].ToString()), Convert.ToInt32(fila["estado"]));
                    modelo = new ModelAlumno(Convert.ToInt32(fila["id"].ToString()), fila["nombre"].ToString(), (fila["apellido"].ToString()), fila["direccion"].ToString(), fila["carnet"].ToString(), fila["correo"].ToString(), DateTime.Now.Date, fila["fotografia"].ToString(), fila["cui"].ToString(), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelAlumno(0,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,DateTime.Now,string.Empty,string.Empty,0,ex.Message.ToString()));
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
        public List<ModelAlumno> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelAlumno> lista = new List<ModelAlumno>();
            try
            {
                ModelAlumno modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,direccion,carnet,correo,fechaNacimiento,cui,fotografia,estado FROM ALUMNO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR apellido LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelAlumno(Convert.ToInt32(fila["id"].ToString()),fila["nombre"].ToString(),(fila["apellido"].ToString()),fila["direccion"].ToString(),fila["carnet"].ToString(),fila["correo"].ToString(),DateTime.Now.Date,fila["fotografia"].ToString(),fila["cui"].ToString(), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelAlumno(0,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,DateTime.Now,string.Empty,string.Empty,0,ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del catalogo
        /// </summary>
        /// <param name="estado">estado</param>
        /// <returns>array de objetos</returns>
        public List<ModelAlumno> Listar(int estado, string orden, string campoOrden)
        {
            List<ModelAlumno> lista = new List<ModelAlumno>();
            try
            {
                ModelAlumno modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,direccion,carnet,correo,fechaNacimiento,cui FROM ALUMNO WHERE estado=" + estado + " ORDER BY '" + campoOrden + "' '" + orden + "'");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelAlumno(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()),Encryption.DecryptString(fila["direccion"].ToString()),Encryption.DecryptString(fila["carnet"].ToString()),Encryption.DecryptString(fila["correo"].ToString()),Convert.ToDateTime(fila["fechaNacimiento"].ToString()),"FOTOGRAFIA",Encryption.DecryptString(fila["cui"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelAlumno(0,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,DateTime.Now,string.Empty,string.Empty,0,ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelAlumno> Listar(int PK)
        {
            List<ModelAlumno> lista = new List<ModelAlumno>();
            try
            {
                ModelAlumno modelo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,direccion,carnet,correo,fechaNacimiento,cui,fotografia FROM ALUMNO WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelAlumno(Convert.ToInt32(fila["id"].ToString()),Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()),Encryption.DecryptString(fila["direccion"].ToString()),Encryption.DecryptString(fila["carnet"].ToString()),Encryption.DecryptString(fila["correo"].ToString()),Convert.ToDateTime(fila["fechaNacimiento"].ToString()),Encryption.DecryptString(fila["fotografia"].ToString()),Encryption.DecryptString(fila["cui"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelParentesco parentesco = new ModelParentesco(0, string.Empty, string.Empty, 0, ex.Message.ToString());
                lista.Add(new ModelAlumno(0,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,DateTime.Now,string.Empty,string.Empty,0,ex.Message.ToString()));
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,direccion,carnet,correo,fechaNacimiento,cui FROM ALUMNO WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
