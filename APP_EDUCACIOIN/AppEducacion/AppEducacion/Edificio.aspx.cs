using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DAL;
using BLL;

namespace AppEducacion
{
    public partial class Edificio : System.Web.UI.Page
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
        public static List<ModelEdificio> ObtenerListado(int inicio, int paginacion, string busqueda, int estado)
        {
            ControllerEdificio edificio = new ControllerEdificio();
            List<ModelEdificio> listado = new List<ModelEdificio>();

            if (!string.IsNullOrEmpty(busqueda))
                listado = edificio.Listar(inicio, paginacion, busqueda, estado);
            else
                listado = edificio.Listar(inicio, paginacion, estado);
            return listado;
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="PK"> identificador</param>
        /// <returns>objeto de edificio</returns>
        [WebMethod]
        public static List<ModelEdificio> Get(int PK)
        {
            List<ModelEdificio> lista = new List<ModelEdificio>();
            ControllerEdificio edificio = new ControllerEdificio();
            return edificio.Listar(PK);
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="Modulo">Modulo</param>
        /// <returns>objeto de edificio</returns>
        [WebMethod]
        public static List<ModelEdificio> GetPorModulo(int Modulo)
        {
            List<ModelEdificio> lista = new List<ModelEdificio>();
            ControllerEdificio edificio = new ControllerEdificio();
            return edificio.ListarPorModulo(Modulo);
        }

        /// <summary>
        /// Metodo para el autocomplete en la busqueda
        /// </summary>
        /// <param name="TextoBusqueda">filtro</param>
        /// <returns>Arrays de nombres coincidentes</returns>
        [WebMethod]
        public static List<string> GetBusqueda(string TextoBusqueda, int Estado)
        {
            ControllerEdificio edificio = new ControllerEdificio();
            return edificio.Listar(TextoBusqueda, Estado);
        }

        /// <summary>
        /// Metodo para insertar
        /// </summary>
        /// <param name="Nombre">Nombre</param>
        /// <param name="Descripcion">Descripcion</param>
        /// <param name="Estado">Estado</param>
        /// <returns>True=Operacion correcta, False=Operacion Incorrecta</returns>
        [WebMethod]
        public static string Guardar(int Pk, string Nombre, string Descripcion, int Estado, bool Operacion)
        {
            ModelEdificio miEdificio = new ModelEdificio(Pk, Nombre, Descripcion, Estado);
            ControllerEdificio edificio = new ControllerEdificio();
            if (ValidarModelo(miEdificio, Operacion))
            {
                return edificio.Insertar(miEdificio, Operacion);
            }
            else
                return Error;
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
            ControllerEdificio edificio = new ControllerEdificio();
            int cant = edificio.Count(TextoBusqueda, Estado);
            return cant;
        }
        #endregion

        #region VALIDACIONES

        /// <summary>
        /// valida los campos del objeto del modelo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        static bool ValidarModelo(ModelEdificio edificio, bool Operacion)
        {
            ControllerEdificio controlador = new ControllerEdificio();

            if (string.IsNullOrEmpty(edificio.Nombre))
            {
                Error = "Por favor, ingrese nombre del edificio.";
                return false;
            }

            if (edificio.Nombre.Trim().Length > 50) 
            {
                Error="El nombre supera la longitud permitida.";
                return false;
            }

            if (!Operacion && controlador.Count(edificio.Nombre.Trim().ToUpper()) > 0) 
            {
                Error = "Existe un edificio con el mismo nombre.";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(edificio.Nombre.Trim())) 
            {
                Error = "El nombre incluye palabras no permitidas.";
                return false;
            }

            /*if (Validador.VerificarCaracteresEspeciales(edificio.Nombre.Trim()))
            {
                Error = "No se permiten carácteres especiales en el nombre";
                return false;
            }*/

            if (edificio.Descripcion.Trim().Length > 50) {
                Error = "La descripción supera la longitud permitida";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(edificio.Descripcion.Trim()))
            {
                Error = "La descripción incluye palabras no permitidas.";
                return false;
            }

            /*if(Validador.VerificarCaracteresEspeciales(edificio.Descripcion.Trim()))
            {
                Error = "No se permiten caracteres especiales en la descripción.";
                return false;
            }*/

            if (edificio.Estado <= 0)
            {
                Error = "Estado no permitido";
                return false;
            }
            return true;
        }

        #endregion
    }
}