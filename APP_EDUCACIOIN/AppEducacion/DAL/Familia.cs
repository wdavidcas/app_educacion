using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using BLL;


/*
namespace DAL
{    
        //*********************************MODELO*******************************************
        /// <summary>
        /// Modelo de la tabla CategoriaNivel
        /// </summary>
        public class ModelFamiliares
        {
            /// <summary>
            /// campos de la clase
            /// </summary>
            public int PK { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Direccion { get; set; }
            public string Correo { get; set; }
            public string Cui { get; set; }
            public bool ResponsableDirecto { get; set; }
            public int NivelResponsabilidad { get; set; }
            public int ParentescoId {get;set;}
            public int AlumnoId { get; set;}
            public int Estado_Id { get; set; }
            public string Error { get; set; }

            /// <summary>
            /// constructor de la clase
            /// </summary>
            /// <param name="pk">identificador</param>
            /// <param name="nombre">nombre</param>
            /// <param name="descripcion">descripcion</param>
            public ModelFamiliares(int pk, string nombre, string apellido,string cui,string direccion,string correo,bool responsableDirecto,int nivelResponsabilidad,int parentescoId,int AlumnoId ,int estado)
            {
                this.PK = pk;
                this.Nombre = nombre;
                this.Apellido = apellido;
                this.Cui = cui;
                this.Direccion = direccion;
                this.Correo = correo;
                this.ResponsableDirecto = responsableDirecto;
                this.NivelResponsabilidad = nivelResponsabilidad;
                this.ParentescoId=parentescoId;
                this.AlumnoId = AlumnoId;
                this.Estado_Id = estado;
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
            public ModelFamiliares(int pk,string error)
            {
                this.PK = pk;                
                this.Error = error;
            }

            /// <summary>
            /// constructor por defecto
            /// </summary>
            public ModelFamiliares()
            {
            }
        }

        //*************************CONTROLADOR DEL MODELO***********************************

        /// <summary>
        /// clase controladora del modelo y sus operaciones
        /// </summary>
        public class ControllerFamiliares
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
            public ControllerFamiliares()
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
            public string Insertar(ModelFamiliares modelo, bool Operacion)
            {
                try
                {
                    string query = string.Empty;
                    if (!Operacion)
                    {                        
                        query = "UPDATE FAMILIARES SET nombre=@nombre,apellido=@apellido,cui=@cui,direccion=@direccion,correo=@correo,Parentesco_id=@parentescoid,Alumno_Id=@alumnoid,esResponsableDirecto=@responsableDirecto,nivelResponsabilidad=@nivelResponsabilidad,Estado_id=@estado WHERE id=@id";
                        MySqlParameter paramId = new MySqlParameter("id", modelo.PK);
                        parametros.Add(paramId);
                    }
                    else
                    {
                        query = "INSERT INTO FAMILIARES(nombre,apellido,direccion,correo,Parentesco_id,Alumno_id,esResponsableDirecto,nivelResponsabilidad,Estado_id) VALUES(@ombre,@apellido,@direccion,@correo,@parentescoid,@alumnoid,@responsableDirecto,@nivelResponsabilidad,@estado)";
                    }
                    MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modelo.Nombre));
                    parametros.Add(paramNombre);
                    MySqlParameter paramApellido = new MySqlParameter("apellido", Encryption.EncryptString(modelo.Apellido));
                    parametros.Add(paramApellido);
                    MySqlParameter paramDireccion = new MySqlParameter("direccion",Encryption.EncryptString(modelo.Direccion));
                    parametros.Add(paramDireccion);
                    MySqlParameter paramCui = new MySqlParameter("cui", Encryption.EncryptString(modelo.Cui));
                    parametros.Add(paramCui);
                    MySqlParameter paramCorreo = new MySqlParameter("correo",Encryption.EncryptString(modelo.Correo));
                    parametros.Add(paramCorreo);
                    MySqlParameter paramParentesco = new MySqlParameter("parentescoid",modelo.ParentescoId);
                    parametros.Add(paramParentesco);
                    MySqlParameter paramAlumno = new MySqlParameter("alumnoid",modelo.AlumnoId);
                    parametros.Add(paramAlumno);
                    MySqlParameter paramResponsableDirecto = new MySqlParameter("responsabledirecto",modelo.ResponsableDirecto);
                    parametros.Add(paramResponsableDirecto);
                    MySqlParameter paramNivelResponsabilidad = new MySqlParameter("nivelResponsabilidad", modelo.NivelResponsabilidad);
                    parametros.Add(paramNivelResponsabilidad);
                    MySqlParameter paramEstado = new MySqlParameter("estado", modelo.Estado_Id);
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
            public bool Eliminar(ModelProfesiones profesion)
            {
                try
                {
                    string query = "DELETE FROM FAMILIARES WHERE id=@id";
                    MySqlParameter paramPk = new MySqlParameter("id", profesion.PK);
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
                        string query = "SELECT COUNT(*) FROM FAMILIARES WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR apellido LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                        return conexion.EjecutarQueryCount(query);
                    }
                    else
                    {
                        string query = "SELECT COUNT(*) FROM FAMILIARES WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
            public List<ModelFamiliares> Listar(int inicio, int paginacion, int estado)
            {
                List<ModelFamiliares> lista = new List<ModelFamiliares>();
                try
                {
                    ModelFamiliares modelo;
                    listado.Clear();
                    listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,cui,direccion,correo,cui,Parentesco_id,Alumno_id,esResponsableDirecto,nivelResponsabilidad,Estado_id FROM FAMILIARES WHERE Estado_id=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                    foreach (DataRow fila in listado.Rows)
                    {
                        modelo = new ModelFamiliares(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()),Encryption.DecryptString(fila["cui"].ToString()),Encryption.DecryptString(fila["direccion"].ToString()),Encryption.DecryptString(fila["correo"].ToString()),modelo.ResponsableDirecto,modelo.NivelResponsabilidad,modelo.ParentescoId,modelo.AlumnoId,modelo.Estado_Id);
                        lista.Add(modelo);
                    }
                    return lista;
                }
                catch (Exception ex)
                {
                    lista.Add(new ModelFamiliares(0,ex.Message.ToString()));
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
            public List<ModelFamiliares> Listar(int inicio, int paginacion, string busqueda, int estado)
            {
                List<ModelFamiliares> lista = new List<ModelFamiliares>();
                try
                {
                    ModelFamiliares modelo;
                    listado.Clear();
                    ModelCredenciales credenciales = new ModelCredenciales();
                    Conexion conexion = new Conexion(credenciales);
                    listado = conexion.EjecutarSelect("SELECT id,nombre,apellido,cui,direccion,correo,cui,Parentesco_id,Alumno_id,esResponsableDirecto,nivelResponsabilidad,Estado_id FROM FAMILIARES WHERE Estado_id=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");// LIMIT " + inicio + "," + cantidad + " ");

                    foreach (DataRow fila in listado.Rows)
                    {
                        modelo = new ModelFamiliares(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["apellido"].ToString()), Encryption.DecryptString(fila["cui"].ToString()), Encryption.DecryptString(fila["direccion"].ToString()), Encryption.DecryptString(fila["correo"].ToString()), modelo.ResponsableDirecto, modelo.NivelResponsabilidad, modelo.ParentescoId, modelo.AlumnoId, modelo.Estado_Id);
                        lista.Add(modelo);
                    }
                    return lista;
                }
                catch (Exception ex)
                {
                    lista.Add(new ModelFamiliares(0,string.Empty));
                    return lista;
                }
            }

            /// <summary>
            /// lista los elementos del catalogo
            /// </summary>
            /// <param name="estado">estado</param>
            /// <returns>array de objetos</returns>
            public List<ModelFamiliares> Listar(int estado, string orden, string campoOrden)
            {
                List<ModelFamiliares> lista = new List<ModelFamiliares>();
                try
                {
                    ModelFamiliares modelo;
                    listado.Clear();
                    ModelCredenciales credenciales = new ModelCredenciales();
                    Conexion conexion = new Conexion(credenciales);
                    listado = conexion.EjecutarSelect("SELECT idProfesion as id,nombre,descripcion,estado FROM PROFESION WHERE estado=" + estado + " ORDER BY '" + campoOrden + "' '" + orden + "'");

                    foreach (DataRow fila in listado.Rows)
                    {
                        profesion = new ModelProfesiones(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                        lista.Add(profesion);
                    }
                    return lista;
                }
                catch (Exception ex)
                {
                    lista.Add(new ModelProfesiones(0, ex.Message.ToString(), ex.Message.ToString(), 0, ex.Message.ToString()));
                    return lista;
                }
            }

            /// <summary>
            /// Retorna un objeto de Categoria Nivel
            /// </summary>
            /// <param name="PK">Identificador</param>
            /// <returns>objeto de Categoria nivel</returns>
            public List<ModelProfesiones> Listar(int PK)
            {
                List<ModelProfesiones> lista = new List<ModelProfesiones>();
                try
                {
                    ModelProfesiones profesion;
                    ModelCredenciales credenciales = new ModelCredenciales();
                    Conexion conexion = new Conexion(credenciales);
                    listado = conexion.EjecutarSelect("SELECT idProfesion as id,nombre,descripcion,estado FROM PROFESION WHERE idProfesion=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                    foreach (DataRow fila in listado.Rows)
                    {
                        profesion = new ModelProfesiones(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["estado"]));
                        lista.Add(profesion);
                    }
                    return lista;

                }
                catch (Exception ex)
                {
                    ModelProfesiones profesion = new ModelProfesiones(0, string.Empty, string.Empty, 0, ex.Message.ToString());
                    lista.Add(profesion);
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
                    listado = conexion.EjecutarSelect("SELECT idProfesion as id,nombre,descripcion,estado FROM PROFESION WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
*/