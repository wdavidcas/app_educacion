using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;


namespace DAL
{
    /// <summary>
    /// ***************************CLASE DEL MODELO DEL OBJETO DE CREDENCIALES********************************
    /// </summary>
    public class ModelCredenciales
    {
        /// <summary>
        /// campos de la clase
        /// </summary>
        public string Servidor { get; set; }
        public string BaseDatos { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }

        /// <summary>
        /// constructor por defecto
        /// </summary>
        public ModelCredenciales()
        {
            this.Servidor = "localhost";
            this.BaseDatos = "bdcolegio";
            this.Usuario = "root";
            this.Clave = "";
        }

        /// <summary>
        /// constructor parametrizado
        /// </summary>
        /// <param name="server"></param>
        /// <param name="bd"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        public ModelCredenciales(string server, string bd, string user, string pass)
        {
            this.Servidor = server;
            this.BaseDatos = bd;
            this.Usuario = user;
            this.Clave = pass;
        }
    }

   
    /// <summary>
    /// ****************************CLASE DE CONEXION Y EJECUCION DE SENTENCIAS MYSQL**********************************
    /// </summary>
    public class Conexion
    {
        string CadenaConexion { get; set; }
        public string Error { get; set; }
        private string UsuarioConexin { get; set; }
        MySqlConnection ConexionMysql { get; set; }
        MySqlDataAdapter DataAdapter { get; set; }
        MySqlCommand ComandMysql { get; set; }
        DataTable Data { get; set; }
        /// <summary>
        /// constructor por defecto
        /// </summary>
        /// <param name="credenciales"></param>
        public Conexion(ModelCredenciales credenciales)
        {
            this.CadenaConexion = "Data Source='" + credenciales.Servidor + "';Initial Catalog='" + credenciales.BaseDatos + "';User id='" + credenciales.Usuario + "';Password='" + credenciales.Clave + "'";
            ConexionMysql = new MySqlConnection(this.CadenaConexion);
            DataAdapter = new MySqlDataAdapter();
            Data = new DataTable();
            this.Error = string.Empty;
            this.UsuarioConexin = credenciales.Usuario;

        }

        #region TESTEO

        /// <summary>
        /// Test de conectividad
        /// </summary>
        /// <returns></returns>
        public bool TestConexion()
        {
            try
            {
                if (ConexionMysql.Ping())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// Abre la conexion
        /// </summary>
        /// <returns></returns>
        public bool AbrirConexion()
        {
            try
            {
                if (ConexionMysql.State == ConnectionState.Closed)
                {
                    ConexionMysql.Open();                    
                    return true;
                }
                else 
                {
                    return false;
                }                
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// Cierra la conexion
        /// </summary>
        /// <returns>resultado de la ejecucion</returns>
        public bool CerrarConexion()
        {
            try
            {
                if (ConexionMysql.State == ConnectionState.Open)
                {
                    ConexionMysql.Close();
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return false;
            }
        }
        #endregion

        #region QUERYS_ESCALARES
        /// <summary>
        /// Ejecuta el query para encontrar la cantidad de registros
        /// </summary>
        /// <param name="query">query a ejecutar</param>
        /// <returns>cantidad de registros del query</returns>
        public int EjecutarQueryCount(string query)
        {
            try
            {
                if (this.AbrirConexion())
                {
                    ComandMysql = new MySqlCommand();
                    ComandMysql.Connection = this.ConexionMysql;
                    ComandMysql.CommandText = query;
                    var result = ComandMysql.ExecuteScalar();
                    this.CerrarConexion();
                    return Convert.ToInt32(result);
                }
                else
                    return 0;
            }
            catch (Exception ex) {
                this.Error = ex.Message.ToString();
                this.CerrarConexion();
                return 0;
            }
        }
        /// <summary>
        /// Ejecutar query
        /// </summary>
        /// <param name="query">query a ejecutarse</param>
        /// <returns>true=ejecución correcta, false=ejecució incorrecta</returns>
        public bool EjecutarQuery(string query)
        {
            try
            {
                if (this.AbrirConexion())
                {
                    ComandMysql = new MySqlCommand();
                    ComandMysql.CommandText = query;
                    int result = ComandMysql.ExecuteNonQuery();
                    this.CerrarConexion();
                    if (result > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                this.CerrarConexion();
                this.Error = ex.Message.ToString();
                return false;
            }
        }
                
        /// <summary>
        /// Ejecuta Querys evitando SQLINJECTIONS
        /// </summary>
        /// <param name="query">query a ejecutarse</param>
        /// <param name="parametros">listado de parametros</param>
        /// <returns>true=ejecución correcta, false=error en ejecución</returns>
        public bool EjecutarQuery(string query, List<MySqlParameter> parametros)
        {
            try
            {
                if (this.AbrirConexion())
                {
                    ComandMysql = new MySqlCommand();
                    ComandMysql.Connection = this.ConexionMysql;
                    ComandMysql.CommandTimeout = 60;
                    ComandMysql.CommandText = query;
                    
                    foreach (var param in parametros)
                    {
                        ComandMysql.Parameters.AddWithValue(param.ParameterName,param.Value);
                    }
                    //se le agrega el usuario que este realizando la operacion CRUD
                    ComandMysql.Parameters.AddWithValue("user",this.UsuarioConexin);

                    int result = (int)ComandMysql.ExecuteNonQuery();
                    this.CerrarConexion();

                    if (result > 0)
                        return true;
                    else
                        return false;

                }
                else
                {
                    this.Error = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.CerrarConexion();
                this.Error = ex.Message.ToString();
                return false;
            }
        }

        #endregion

        #region QUERYS_SELECCION
        /// <summary>
        /// Ejecutar query
        /// </summary>
        /// <param name="query">query a ejecutarse</param>
        /// <returns>resultado de la ejecucion</returns>
        public DataTable EjecutarSelect(string query)
        {
            try
            {
                this.Data = new DataTable();
                DataAdapter = new MySqlDataAdapter(query, this.ConexionMysql);
                DataAdapter.Fill(this.Data);
                return this.Data;
            }
            catch (Exception ex)
            {
                this.Error = ex.Message.ToString();
                return this.Data;
            }
        }

        #endregion
    }

}
