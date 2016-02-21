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
    /// Modelo de la tabla INVENTARIO FISICO
    /// </summary>
    public class ModelInventarioFisico
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Existencias {get;set;}
        public double Precio {get;set;}
        public double Depreciacion {get;set;}
        public string Marca { get;set;}
        public string Modelo { get;set;}
        public int AulaId{ get;set; }
        public int ModuloId { get;set; }
        public int EdificioId { get;set; }
        public int Estado { get; set; }
        public string Error { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk">identificadod</param>
        /// <param name="nombre">Nombre del producto</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="existencias">Existencias en bodega</param>
        /// <param name="precio">Precio</param>
        /// <param name="depreciacion">Depreciacion Anual del producto</param>
        /// <param name="marca">Marca</param>
        /// <param name="modelo">Modelo</param>
        /// <param name="aulaid">Aula a la que pertenece (aceptar nulls)</param>
        /// <param name="moduloid">Modulo al que pertenece ()</param>
        /// <param name="edificioid"></param>
        /// <param name="estado"></param>
        public ModelInventarioFisico(int pk, string nombre, string descripcion,int existencias,double precio,double depreciacion,string marca,string modelo,int aulaid,int moduloid,int edificioid, int estado)
        {
            this.PK = pk;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Existencias=existencias;
            this.Precio=precio;
            this.Depreciacion=depreciacion;
            this.Marca=marca;
            this.Modelo=modelo;
            this.AulaId=aulaid;
            this.ModuloId=moduloid;
            this.EdificioId=edificioid;
            this.Estado = estado;
            this.Error = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="error"></param>
        public ModelInventarioFisico(int pk, string error)
        {
            this.PK = pk;            
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelInventarioFisico()
        {

        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerInventarioFisico
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
        public ControllerInventarioFisico()
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
        public string Insertar(ModelInventarioFisico modelo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    query = "UPDATE INVENTARIOFISICO SET nombre=@nombre,descripcion=@descripcion,existencias=@existencias,precio=@precio,depreciacion=@depreciacion,marca=@marca,modelo=@modelo,Aula_id=@aulaid,Modulo_id=@moduloid,Edificio_id=@edificioid,estado=@estado,user=@user WHERE id=@id";
                    MySqlParameter paramId = new MySqlParameter("id", modelo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = @"INSERT INTO INVENTARIOFISICO(nombre,descripcion,existencias,precio,depreciacion,marca,modelo,Aula_id,Modulo_id,Edificio_id,estado,user) 
                            VALUES(@nombre,@descripcion,@existencias,@precio,@depreciacion,@amarca,@modelo,@aulaid,@moduloid,@edificioid,@estado,@user)";
                }
                MySqlParameter paramNombre = new MySqlParameter("nombre", Encryption.EncryptString(modelo.Nombre));
                parametros.Add(paramNombre);
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(modelo.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramExistencias = new MySqlParameter("existencias", modelo.Existencias);
                parametros.Add(paramExistencias);
                MySqlParameter paramPrecio = new MySqlParameter("precio", modelo.Precio);
                parametros.Add(paramPrecio);
                MySqlParameter paramDepreciacion = new MySqlParameter("mora", modelo.Depreciacion);
                parametros.Add(paramDepreciacion);
                MySqlParameter paramMarca = new MySqlParameter("marca", Encryption.EncryptString(modelo.Marca));
                parametros.Add(paramMarca);
                MySqlParameter paramModelo = new MySqlParameter("modelo", Encryption.EncryptString(modelo.Modelo));
                parametros.Add(paramModelo);
                MySqlParameter paramAulaId = new MySqlParameter("aulaid", modelo.AulaId);
                parametros.Add(paramAulaId);
                MySqlParameter paramModuloId = new MySqlParameter("moduloid", modelo.Modelo);
                parametros.Add(paramModuloId);
                MySqlParameter paramEdificioId = new MySqlParameter("edifiicoid", modelo.EdificioId);
                parametros.Add(paramEdificioId);
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
                string query = "DELETE FROM INVENTARIOFISICO WHERE id=@id";
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
                    string query = "SELECT COUNT(*) FROM INVENTARIOFISICO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ")";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM INVENTARIOFISICO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
                string query = "SELECT COUNT(*) FROM INVENTARIOFISICO WHERE nombre='" + Encryption.EncryptString(nombre.ToUpper()) + "'";
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
        public List<ModelInventarioFisico> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelInventarioFisico> lista = new List<ModelInventarioFisico>();
            try
            {
                ModelInventarioFisico modelo;
                listado.Clear();
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,existencias,precio,depreciacion,marca,modelo,Aula_id,Modulo_id,Edificio_id,estado FROM INVENTARIOFISICO WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY nombre ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelInventarioFisico(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToInt32(fila["existencias"].ToString()),Convert.ToDouble(fila["precio"].ToString()), Convert.ToDouble(fila["depreciacion"].ToString()),Encryption.DecryptString(fila["marca"].ToString()),Encryption.DecryptString(fila["modelo"].ToString()),Convert.ToInt32(fila["Aula_id"].ToString()),Convert.ToInt32(fila["Modulo_id"].ToString()),Convert.ToInt32(fila["Edificio_id"].ToString()),Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelInventarioFisico(0, ex.Message.ToString()));
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
        public List<ModelInventarioFisico> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelInventarioFisico> lista = new List<ModelInventarioFisico>();
            try
            {
                ModelInventarioFisico modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,existencias,precio,depreciacion,marca,modelo,Aula_id,Modulo_id,Edificio_id,estado FROM INVENTARIOFISICO WHERE estado=" + estado + " AND (nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' OR descripcion LIKE '%" + Encryption.EncryptString(busqueda) + "%') AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY nombre ASC LIMIT " + paginacion + "");// LIMIT " + inicio + "," + cantidad + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelInventarioFisico(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["existencias"].ToString()), Convert.ToDouble(fila["precio"].ToString()), Convert.ToDouble(fila["depreciacion"].ToString()), Encryption.DecryptString(fila["marca"].ToString()), Encryption.DecryptString(fila["modelo"].ToString()), Convert.ToInt32(fila["Aula_id"].ToString()), Convert.ToInt32(fila["Modulo_id"].ToString()), Convert.ToInt32(fila["Edificio_id"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelInventarioFisico(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del catalogo
        /// </summary>
        /// <param name="estado">estado</param>
        /// <returns>array de objetos</returns>
        public List<ModelInventarioFisico> Listar(int estado, string orden, string campoOrden)
        {
            List<ModelInventarioFisico> lista = new List<ModelInventarioFisico>();
            try
            {
                ModelInventarioFisico modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,existencias,precio,depreciacion,marca,modelo,Aula_id,Modulo_id,Edificio_id,estado FROM INVENTARIOFISICO WHERE estado=" + estado + " ORDER BY '" + campoOrden + "' '" + orden + "'");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelInventarioFisico(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["existencias"].ToString()), Convert.ToDouble(fila["precio"].ToString()), Convert.ToDouble(fila["depreciacion"].ToString()), Encryption.DecryptString(fila["marca"].ToString()), Encryption.DecryptString(fila["modelo"].ToString()), Convert.ToInt32(fila["Aula_id"].ToString()), Convert.ToInt32(fila["Modulo_id"].ToString()), Convert.ToInt32(fila["Edificio_id"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelInventarioFisico(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de del Modelo
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelInventarioFisico> Listar(int PK)
        {
            List<ModelInventarioFisico> lista = new List<ModelInventarioFisico>();
            try
            {
                ModelInventarioFisico modelo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,existencias,precio,depreciacion,marca,modelo,Aula_id,Modulo_id,Edificio_id,estado FROM INVENTARIOFISICO WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelInventarioFisico(Convert.ToInt32(fila["id"].ToString()), Encryption.DecryptString(fila["nombre"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["existencias"].ToString()), Convert.ToDouble(fila["precio"].ToString()), Convert.ToDouble(fila["depreciacion"].ToString()), Encryption.DecryptString(fila["marca"].ToString()), Encryption.DecryptString(fila["modelo"].ToString()), Convert.ToInt32(fila["Aula_id"].ToString()), Convert.ToInt32(fila["Modulo_id"].ToString()), Convert.ToInt32(fila["Edificio_id"].ToString()), Convert.ToInt32(fila["estado"]));
                    lista.Add(modelo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                lista.Add(new ModelInventarioFisico(0, ex.Message.ToString()));                
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
                listado = conexion.EjecutarSelect("SELECT id,nombre,descripcion,existencias,precio,depreciacion,marca,modelo,Aula_id,Modulo_id,Edificio_id,estado FROM INVENTARIOFISICO WHERE estado=" + Estado + " AND nombre LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
