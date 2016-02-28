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
        /// 
        /// </summary>
        /// <param name="Estado"></param>
        /// <param name="Orden"></param>
        /// <param name="CampoOrden"></param>
        /// <returns></returns>
        [WebMethod]
        public static List<ModelCategoriaCurso> ObtenerListado(int Estado, string Orden, string CampoOrden)
        {
            ControllerCategoriaCurso controlador = new ControllerCategoriaCurso();
            return controlador.Listar(Estado, Orden, CampoOrden);
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
            ControllerCategoriaCurso controlador = new ControllerCategoriaCurso();

            if (string.IsNullOrEmpty(categoria.Nombre))
            {
                Error = "Por favor, ingrese nombre del edificio.";
                return false;
            }

            if (categoria.Nombre.Trim().Length > 25)
            {
                Error = "El nombre supera la longitud permitida.";
                return false;
            }
            
            if (!Operacion && controlador.Count(categoria.Nombre.Trim().ToUpper()) > 0)
            {
                Error = "Existe un edificio con el mismo nombre.";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(categoria.Nombre.Trim()))
            {
                Error = "El nombre incluye palabras no permitidas.";
                return false;
            }

            /*if (Validador.VerificarCaracteresEspeciales(edificio.Nombre.Trim()))
            {
                Error = "No se permiten carácteres especiales en el nombre";
                return false;
            }*/

            if (categoria.Descripcion.Trim().Length > 50)
            {
                Error = "La descripción supera la longitud permitida";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(categoria.Descripcion.Trim()))
            {
                Error = "La descripción incluye palabras no permitidas.";
                return false;
            }

            /*if(Validador.VerificarCaracteresEspeciales(edificio.Descripcion.Trim()))
            {
                Error = "No se permiten caracteres especiales en la descripción.";
                return false;
            }*/

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