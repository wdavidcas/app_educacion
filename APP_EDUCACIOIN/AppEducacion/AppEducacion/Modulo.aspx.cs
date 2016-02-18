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
    public partial class Modulo : System.Web.UI.Page
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
        public static List<ModelModulo> ObtenerListado(int inicio, int paginacion, string busqueda, int estado)
        {
            ControllerModulo modulo = new ControllerModulo();
            List<ModelModulo> listado = new List<ModelModulo>();

            if (!string.IsNullOrEmpty(busqueda))
                listado = modulo.Listar(inicio, paginacion, busqueda, estado);
            else
                listado = modulo.Listar(inicio, paginacion, estado);
            return listado;
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="PK"> identificador</param>
        /// <returns>objeto Categoria Nivel</returns>
        [WebMethod]
        public static List<ModelModulo> Get(int PK)
        {
            List<ModelModulo> lista = new List<ModelModulo>();
            ControllerModulo modulo = new ControllerModulo();
            return modulo.Listar(PK);
        }

        /// <summary>
        /// Metodo para el autocomplete en la busqueda
        /// </summary>
        /// <param name="TextoBusqueda">filtro</param>
        /// <returns>Arrays de nombres coincidentes</returns>
        [WebMethod]
        public static List<string> GetBusqueda(string TextoBusqueda, int Estado)
        {
            ControllerModulo modulo = new ControllerModulo();
            return modulo.Listar(TextoBusqueda, Estado);
        }

        /// <summary>
        /// Metodo para insertar
        /// </summary>
        /// <param name="Nombre">Nombre</param>
        /// <param name="Descripcion">Descripcion</param>
        /// <param name="Estado">Estado</param>
        /// <returns>True=Operacion correcta, False=Operacion Incorrecta</returns>
        [WebMethod]
        public static string Guardar(int Pk, string Nombre, string Descripcion, int Estado, int Edificio_Id, bool Operacion)
        {
            ModelModulo miModulo = new ModelModulo(Pk, Nombre, Descripcion, Estado, Edificio_Id);
            ControllerModulo nivel = new ControllerModulo();
            if (validarCategoriaNivel(miModulo, Operacion))
            {
                return nivel.Insertar(miModulo, Operacion);
            }
            else
                return nivel.Error;
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
            ControllerModulo edificio = new ControllerModulo();
            int cant = edificio.Count(TextoBusqueda, Estado);
            return cant;
        }
        #endregion

        [WebMethod]
        public static List<ModelEdificio> ObtenerEdificios(int Estado, string Orden, string CampoOrden)
        {
            ControllerEdificio edificio = new ControllerEdificio();
            return edificio.Listar(Estado, Orden, CampoOrden);
        }
        #region VALIDACIONES

        /// <summary>
        /// valida los campos del objeto del modelo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        static bool validarCategoriaNivel(ModelModulo nivel, bool Operacion)
        {
            if (string.IsNullOrEmpty(nivel.Nombre))
            {
                Error = "Nombre vacío";
                return false;
            }
            if (nivel.Estado <= 0)
            {
                Error = "Estado no permitido";
                return false;
            }
            return true;
        }

        #endregion
    }
}