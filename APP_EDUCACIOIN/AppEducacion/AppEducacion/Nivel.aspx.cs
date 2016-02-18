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
    public partial class Nivel : System.Web.UI.Page
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
        public static List<ModelNivel> ObtenerListado(int inicio, int paginacion, string busqueda, int estado)
        {
            ControllerNivel nivel = new ControllerNivel();
            List<ModelNivel> listado = new List<ModelNivel>();

            if (!string.IsNullOrEmpty(busqueda))
                listado = nivel.Listar(inicio, paginacion, busqueda, estado);
            else
                listado = nivel.Listar(inicio, paginacion, estado);
            return listado;
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="PK"> identificador</param>
        /// <returns>objeto Categoria Nivel</returns>
        [WebMethod]
        public static List<ModelNivel> Get(int PK)
        {
            List<ModelNivel> lista = new List<ModelNivel>();
            ControllerNivel nivel = new ControllerNivel();
            return nivel.Listar(PK);
        }

        /// <summary>
        /// Metodo para el autocomplete en la busqueda
        /// </summary>
        /// <param name="TextoBusqueda">filtro</param>
        /// <returns>Arrays de nombres coincidentes</returns>
        [WebMethod]
        public static List<string> GetBusqueda(string TextoBusqueda, int Estado)
        {
            ControllerNivel nivel = new ControllerNivel();
            return nivel.Listar(TextoBusqueda, Estado);
        }

        /// <summary>
        /// Metodo para insertar
        /// </summary>
        /// <param name="Nombre">Nombre</param>
        /// <param name="Descripcion">Descripcion</param>
        /// <param name="Estado">Estado</param>
        /// <returns>True=Operacion correcta, False=Operacion Incorrecta</returns>
        [WebMethod]
        public static string Guardar(int Pk,string Codigo, string Nombre, string Descripcion, int Estado,int CategoriaNivel, bool Operacion)
        {
            ModelNivel miNivel = new ModelNivel(Pk,Codigo, Nombre, Descripcion, Estado,CategoriaNivel);
            ControllerNivel nivel = new ControllerNivel();
            if (validarCategoriaNivel(miNivel, Operacion))
            {
                return nivel.Insertar(miNivel, Operacion);
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
            ControllerNivel edificio = new ControllerNivel();
            int cant = edificio.Count(TextoBusqueda, Estado);
            return cant;
        }
        #endregion

        [WebMethod]
        public static List<ModelCategoriaNivel> ObtenerCategoriasNivel(int Estado,string Orden,string CampoOrden) {
            ControllerCategoriaNivel categoria = new ControllerCategoriaNivel();
            return categoria.Listar(Estado, Orden, CampoOrden);
        }
        #region VALIDACIONES

        /// <summary>
        /// valida los campos del objeto del modelo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        static bool validarCategoriaNivel(ModelNivel nivel, bool Operacion)
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