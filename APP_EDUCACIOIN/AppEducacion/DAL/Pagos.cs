using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using BLL;

namespace DAL
{
    //*********************************MODELO*******************************************
    /// <summary>
    /// Modelo de la tabla PAGOS
    /// </summary>
    public class ModelPagos
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public int PK { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }
        public DateTime Fecha { get; set; }
        public int MesCorrespondencia { get; set; }
        public string Descripcion { get; set; }
        public int TipoPagoId { get; set; }
        public int AlumnoId { get; set; }
        public int Estado { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="pk">identificador</param>
        /// <param name="nombre">nombre</param>
        /// <param name="descripcion">descripcion</param>
        public ModelPagos(int pk, int cantidad,double precio,double total,DateTime fecha,int mes,string descripcion,int tipoPago,int alumno, int estado)
        {
            this.PK = pk;
            this.Cantidad = cantidad;
            this.Precio = precio;
            this.Total = total;
            this.Fecha = fecha;
            this.MesCorrespondencia = mes;
            this.Descripcion = descripcion;
            this.TipoPagoId = tipoPago;
            this.AlumnoId = alumno;
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
        public ModelPagos(int pk, string error)
        {
            this.PK = pk;            
            this.Error = error;
        }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelPagos()
        {
        }
    }

    //*************************CONTROLADOR DEL MODELO***********************************

    /// <summary>
    /// clase controladora del modelo y sus operaciones
    /// </summary>
    public class ControllerPagos
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
        public ControllerPagos()
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
        public string Insertar(ModelPagos modelo, bool Operacion)
        {
            try
            {
                string query = string.Empty;

                if (!Operacion)
                {
                    query = "UPDATE PAGOS SET descripcion=@descripcion,cantidad=@cantidad,precio=@precio,total=@total,estado=@estado,fecha=@fecha,mesCorresponde=@mes,TIPOPAGO_idTIPOPAGO=@tipopago,Alumno_id=@alumno,user=@user WHERE idPAGOS=@id";
                    MySqlParameter paramId = new MySqlParameter("id", modelo.PK);
                    parametros.Add(paramId);
                }
                else
                {
                    query = @"INSERT INTO PAGOS(descripcion,cantidad,precio,total,estado,fecha,mesCorresponde,TIPOPAGO_idTIPOPAGO,Alumno_id,user) 
                    VALUES(@descripcion,@cantidad,@precio,@total,@estado,@fecha,@mes,@tipopago,@alumno,@user)";
                }
                
                MySqlParameter paramDescripcion = new MySqlParameter("descripcion", Encryption.EncryptString(modelo.Descripcion));
                parametros.Add(paramDescripcion);
                MySqlParameter paramCantidad=new MySqlParameter("cantidad",Encryption.EncryptString("@cantidad"));
                parametros.Add(paramCantidad);
                MySqlParameter paramPrecio = new MySqlParameter("precio", modelo.Precio);
                parametros.Add(paramPrecio);
                MySqlParameter paramTotal = new MySqlParameter("total", modelo.Total);
                parametros.Add(paramTotal);
                MySqlParameter paramFecha = new MySqlParameter("fecha", modelo.Fecha);
                parametros.Add(paramFecha);
                MySqlParameter paramMes = new MySqlParameter("mes",modelo.MesCorrespondencia);
                parametros.Add(paramMes);
                MySqlParameter paramTipoPago = new MySqlParameter("tipopago", modelo.TipoPagoId);
                parametros.Add(paramTipoPago);
                MySqlParameter paramAlumno = new MySqlParameter("alumno", modelo.AlumnoId);
                parametros.Add(paramAlumno);
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
        public bool Eliminar(ModelPagos modelo)
        {
            try
            {
                string query = "DELETE FROM PAGOS WHERE idPAGOS=@id";
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
                    string query = "SELECT COUNT(*) FROM PAGOS WHERE estado=" + estado + " AND nombre LIKE'%" + Encryption.EncryptString(busqueda) + "%' AND estado<>" + (int)Estados.Tipos.Eliminado + "";
                    return conexion.EjecutarQueryCount(query);
                }
                else
                {
                    string query = "SELECT COUNT(*) FROM PAGOS WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + "";
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
        public List<ModelPagos> Listar(int inicio, int paginacion, int estado)
        {
            List<ModelPagos> lista = new List<ModelPagos>();
            try
            {
                ModelPagos modelo;
                listado.Clear();
                listado = conexion.EjecutarSelect("SELECT idPAGOS as id,descripcion,cantidad,precio,total,fecha,mesCorresponde as mes,TIPOPAGO_idTIPOPAGO as tipopago,Alumno_id as alumno,estado FROM PAGOS WHERE estado=" + estado + " AND estado<>" + (int)Estados.Tipos.Eliminado + " ORDER BY descripcion ASC limit " + inicio + "," + paginacion + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelPagos(Convert.ToInt32(fila["id"].ToString()),Convert.ToInt32(fila["cantidad"].ToString()),Convert.ToDouble(fila["precio"].ToString()),Convert.ToDouble(fila["total"].ToString()),Convert.ToDateTime(fila["fecha"].ToString()),Convert.ToInt32(fila["mes"].ToString()),Encryption.DecryptString(fila["descripcion"].ToString()),Convert.ToInt32(fila["tipopago"].ToString()),Convert.ToInt32(fila["alumno"].ToString()),Convert.ToInt32(fila["estado"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelPagos(0, ex.Message.ToString()));
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
        public List<ModelPagos> Listar(int inicio, int paginacion, string busqueda, int estado)
        {
            List<ModelPagos> lista = new List<ModelPagos>();
            try
            {
                ModelPagos modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idPAGOS as id,descripcion,cantidad,precio,total,fecha,mesCorresponde as mes,TIPOPAGO_idTIPOPAGO as tipopago,Alumno_id as alumno,estado FROM PAGOS WHERE estado=" + estado + " AND descripcion LIKE'%" + Encryption.EncryptString(busqueda) + "%' AND (estado<>" + (int)Estados.Tipos.Eliminado + ") ORDER BY descripcion ASC LIMIT " + paginacion + "");// LIMIT " + inicio + "," + cantidad + " ");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelPagos(Convert.ToInt32(fila["id"].ToString()), Convert.ToInt32(fila["cantidad"].ToString()), Convert.ToDouble(fila["precio"].ToString()), Convert.ToDouble(fila["total"].ToString()), Convert.ToDateTime(fila["fecha"].ToString()), Convert.ToInt32(fila["mes"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["tipopago"].ToString()), Convert.ToInt32(fila["alumno"].ToString()), Convert.ToInt32(fila["estado"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelPagos(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// lista los elementos del catalogo
        /// </summary>
        /// <param name="estado">estado</param>
        /// <returns>array de objetos</returns>
        public List<ModelPagos> Listar(int estado, string orden, string campoOrden)
        {
            List<ModelPagos> lista = new List<ModelPagos>();
            try
            {
                ModelPagos modelo;
                listado.Clear();
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idPAGOS as id,descripcion,cantidad,precio,total,fecha,mesCorresponde as mes,TIPOPAGO_idTIPOPAGO as tipopago,Alumno_id as alumno,estado FROM PAGOS WHERE estado=" + estado + " ORDER BY '" + campoOrden + "' '" + orden + "'");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelPagos(Convert.ToInt32(fila["id"].ToString()), Convert.ToInt32(fila["cantidad"].ToString()), Convert.ToDouble(fila["precio"].ToString()), Convert.ToDouble(fila["total"].ToString()), Convert.ToDateTime(fila["fecha"].ToString()), Convert.ToInt32(fila["mes"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["tipopago"].ToString()), Convert.ToInt32(fila["alumno"].ToString()), Convert.ToInt32(fila["estado"].ToString()));
                    lista.Add(modelo);
                }
                return lista;
            }
            catch (Exception ex)
            {
                lista.Add(new ModelPagos(0, ex.Message.ToString()));
                return lista;
            }
        }

        /// <summary>
        /// Retorna un objeto de Categoria Nivel
        /// </summary>
        /// <param name="PK">Identificador</param>
        /// <returns>objeto de Categoria nivel</returns>
        public List<ModelPagos> Listar(int PK)
        {
            List<ModelPagos> lista = new List<ModelPagos>();
            try
            {
                ModelPagos modelo;
                ModelCredenciales credenciales = new ModelCredenciales();
                Conexion conexion = new Conexion(credenciales);
                listado = conexion.EjecutarSelect("SELECT idPAGOS as id,descripcion,cantidad,precio,total,fecha,mesCorresponde as mes,TIPOPAGO_idTIPOPAGO as tipopago,Alumno_id as alumno,estado FROM PAGOS WHERE id=" + PK + " AND estado<>" + (int)Estados.Tipos.Eliminado + "");

                foreach (DataRow fila in listado.Rows)
                {
                    modelo = new ModelPagos(Convert.ToInt32(fila["id"].ToString()), Convert.ToInt32(fila["cantidad"].ToString()), Convert.ToDouble(fila["precio"].ToString()), Convert.ToDouble(fila["total"].ToString()), Convert.ToDateTime(fila["fecha"].ToString()), Convert.ToInt32(fila["mes"].ToString()), Encryption.DecryptString(fila["descripcion"].ToString()), Convert.ToInt32(fila["tipopago"].ToString()), Convert.ToInt32(fila["alumno"].ToString()), Convert.ToInt32(fila["estado"].ToString()));
                    lista.Add(modelo);
                }
                return lista;

            }
            catch (Exception ex)
            {
                ModelPagos modelo = new ModelPagos(0, string.Empty);
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
                listado = conexion.EjecutarSelect("SELECT idPAGOS as id,descripcion,cantidad,precio,total,fecha,mesCorresponde as mes,TIPOPAGO_idTIPOPAGO as tipopago,Alumno_id as alumno,estado FROM PAGOS WHERE estado=" + Estado + " AND descripcion LIKE '%" + Encryption.EncryptString(TextoBusqueda) + "%'");

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
