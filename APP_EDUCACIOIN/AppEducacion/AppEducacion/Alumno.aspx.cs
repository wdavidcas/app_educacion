using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DAL;

namespace AppEducacion
{
    public partial class Alumno : System.Web.UI.Page
    {
        #region CAMPOS_PAGINA
        /// <summary>
        /// CAMPOS DE LA CLASE
        /// </summary>
        private static string Error = string.Empty;

        #endregion

        #region EVENTOS_PAGINA
        /// <summary>
        /// carga de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        #endregion

        #region WEB_METHODOS
        /// <summary>
        /// Obtiene el listado
        /// </summary>
        /// <param name="inicio">inicio</param>
        /// <param name="paginacion">cantidad de registros</param>
        /// <param name="busqueda">filtro</param>
        /// <param name="estado">estado</param>
        /// <returns>Array de objetos de categoraia nivel</returns>
        [WebMethod]
        public static List<ModelAlumno> ObtenerListado(int inicio, int paginacion, string busqueda, int estado)
        {
            ControllerAlumno controlador = new ControllerAlumno();
            List<ModelAlumno> listado = new List<ModelAlumno>();

            if (!string.IsNullOrEmpty(busqueda))
                listado = controlador.Listar(inicio, paginacion, busqueda, estado);
            else
                listado = controlador.Listar(inicio, paginacion, estado);
            return listado;
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="PK"> identificador</param>
        /// <returns>objeto Categoria Nivel</returns>
        [WebMethod]
        public static List<ModelAlumno> Get(int PK)
        {
            List<ModelAlumno> lista = new List<ModelAlumno>();
            ControllerAlumno controlador = new ControllerAlumno();
            return controlador.Listar(PK);
        }

        /// <summary>
        /// Metodo para el autocomplete en la busqueda
        /// </summary>
        /// <param name="TextoBusqueda">filtro</param>
        /// <returns>Arrays de nombres coincidentes</returns>
        [WebMethod]
        public static List<string> GetBusqueda(string TextoBusqueda, int Estado)
        {
            ControllerAlumno controlador = new ControllerAlumno();
            return controlador.Listar(TextoBusqueda, Estado);
        }

        /// <summary>
        /// Metodo para insertar
        /// </summary>
        /// <param name="Nombre">Nombre</param>
        /// <param name="Descripcion">Descripcion</param>
        /// <param name="Estado">Estado</param>
        /// <returns>True=Operacion correcta, False=Operacion Incorrecta</returns>
        [WebMethod]
        public static string Guardar(int Pk, string Nombre, string Apellido,string Direccion,string Carnet,string Correo,DateTime FechaNacimiento,string Fotografia,string Cui, int Estado, bool Operacion)
        {
            ModelAlumno modelo = new ModelAlumno(Pk, Nombre, Apellido,Direccion,Carnet,Correo,FechaNacimiento,Fotografia,Cui,Estado);
            ControllerAlumno controlador = new ControllerAlumno();
            if (validarModelo(modelo, Operacion))
            {
                return controlador.Insertar(modelo, Operacion);
            }
            else
                return controlador.Error;
        }

        /// <summary>
        /// Obtiene la cantidad de registros
        /// </summary>
        /// <param name="TextoBusqueda">filtro si existe</param>
        /// <param name="Estado">estado</param>
        /// <returns>cantidad de registros</returns>
        [WebMethod]
        public static int ObtenerCount(string TextoBusqueda, int Estado)
        {
            ControllerAlumno controlador = new ControllerAlumno();
            int cant = controlador.Count(TextoBusqueda, Estado);
            return cant;
        }
        #endregion

        #region VALIDACIONES

        /// <summary>
        /// valida los campos del objeto del modelo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        static bool validarModelo(ModelAlumno modelo, bool Operacion)
        {
            if (string.IsNullOrEmpty(modelo.Nombre))
            {
                Error = "Nombre vacío";
                return false;
            }
            if (modelo.Estado <= 0)
            {
                Error = "Estado no permitido";
                return false;
            }
            return true;
        }

        #endregion        
    }
}