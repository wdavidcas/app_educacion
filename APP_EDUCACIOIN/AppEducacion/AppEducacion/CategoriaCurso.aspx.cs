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
    public partial class CategoriaCurso : System.Web.UI.Page
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
        public static List<ModelCategoriaCurso> ObtenerListado(int inicio, int paginacion, string busqueda, int estado)
        {
            ControllerCategoriaCurso categoria = new ControllerCategoriaCurso();
            List<ModelCategoriaCurso> listado = new List<ModelCategoriaCurso>();

            if (!string.IsNullOrEmpty(busqueda))
                listado = categoria.Listar(inicio, paginacion, busqueda, estado);
            else
                listado = categoria.Listar(inicio, paginacion, estado);
            return listado;
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="PK"> identificador</param>
        /// <returns>objeto Categoria Nivel</returns>
        [WebMethod]
        public static List<ModelCategoriaCurso> Get(int PK)
        {
            List<ModelCategoriaCurso> lista = new List<ModelCategoriaCurso>();
            ControllerCategoriaCurso categoria = new ControllerCategoriaCurso();
            return categoria.Listar(PK);
        }

        /// <summary>
        /// Metodo para el autocomplete en la busqueda
        /// </summary>
        /// <param name="TextoBusqueda">filtro</param>
        /// <returns>Arrays de nombres coincidentes</returns>
        [WebMethod]
        public static List<string> GetBusqueda(string TextoBusqueda, int Estado)
        {
            ControllerCategoriaCurso categoria = new ControllerCategoriaCurso();
            return categoria.Listar(TextoBusqueda, Estado);
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
            ModelCategoriaCurso miCategoria = new ModelCategoriaCurso(Pk, Nombre, Descripcion, Estado);
            ControllerCategoriaCurso categoria = new ControllerCategoriaCurso();
            if (validarCategoriaNivel(miCategoria, Operacion))
            {
                return categoria.Insertar(miCategoria, Operacion);
            }
            else
                return categoria.Error;
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
            ControllerCategoriaCurso categoria = new ControllerCategoriaCurso();
            int cant = categoria.Count(TextoBusqueda, Estado);
            return cant;
        }
        #endregion

        #region VALIDACIONES

        /// <summary>
        /// valida los campos del objeto del modelo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        static bool validarCategoriaNivel(ModelCategoriaCurso categoria, bool Operacion)
        {
            if (string.IsNullOrEmpty(categoria.Nombre))
            {
                Error = "Nombre vacío";
                return false;
            }
            if (categoria.Estado <= 0)
            {
                Error = "Estado no permitido";
                return false;
            }
            return true;
        }

        #endregion
    }
}