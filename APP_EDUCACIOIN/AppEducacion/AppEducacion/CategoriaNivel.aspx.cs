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
    public partial class CategoriaNivel : System.Web.UI.Page
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
            if (!IsPostBack) { 
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
        public static List<ModelCategoriaNivel> ObtenerListado(int inicio, int paginacion, string busqueda, int estado)
        {
            ControllerCategoriaNivel categoria = new ControllerCategoriaNivel();
            List<ModelCategoriaNivel> listado = new List<ModelCategoriaNivel>();

            if (!string.IsNullOrEmpty(busqueda))
                listado = categoria.Listar(inicio, paginacion, busqueda, estado);
            else
                listado = categoria.Listar(inicio, paginacion,estado);
            return listado;
        }

        /// <summary>
        ///Lista los objetos del modelo 
        /// </summary>
        /// <param name="Estado">Estado</param>
        /// <param name="Orden">[ASC O DESC]</param>
        /// <param name="CampoOrden">Campo sobre el cual se ordenara la informacion</param>
        /// <returns></returns>
        [WebMethod]
        public static List<ModelCategoriaNivel> ObtenerListado(int Estado, string Orden, string CampoOrden)
        {
            ControllerCategoriaNivel controlador = new ControllerCategoriaNivel();
            return controlador.Listar(Estado, Orden, CampoOrden);
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="PK"> identificador</param>
        /// <returns>objeto Categoria Nivel</returns>
        [WebMethod]
        public static List<ModelCategoriaNivel> Get(int PK)
        {
            List<ModelCategoriaNivel> lista = new List<ModelCategoriaNivel>();
            ControllerCategoriaNivel categoria = new ControllerCategoriaNivel();
            return categoria.Listar(PK);
        }

        /// <summary>
        /// Metodo para el autocomplete en la busqueda
        /// </summary>
        /// <param name="TextoBusqueda">filtro</param>
        /// <returns>Arrays de nombres coincidentes</returns>
        [WebMethod]
        public static List<string> GetBusqueda(string TextoBusqueda,int Estado)
        {
            ControllerCategoriaNivel categoria = new ControllerCategoriaNivel();
            return categoria.Listar(TextoBusqueda,Estado);
        }

        /// <summary>
        /// Metodo para insertar
        /// </summary>
        /// <param name="Nombre">Nombre</param>
        /// <param name="Descripcion">Descripcion</param>
        /// <param name="Estado">Estado</param>
        /// <returns>True=Operacion correcta, False=Operacion Incorrecta</returns>
        [WebMethod]
        public static string Guardar(int Pk,string Codigo,string Nombre, string Descripcion, int Estado,bool Operacion){            
            ModelCategoriaNivel miCategoria = new ModelCategoriaNivel(Pk,Codigo,Nombre, Descripcion, Estado);
            ControllerCategoriaNivel categoria = new ControllerCategoriaNivel();
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
        public static int ObtenerCount(string TextoBusqueda, int Estado) {
            ControllerCategoriaNivel categoria = new ControllerCategoriaNivel();
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
        static bool validarCategoriaNivel(ModelCategoriaNivel categoria,bool Operacion)
        {
            ControllerCategoriaNivel controlador = new ControllerCategoriaNivel();
            if (string.IsNullOrEmpty(categoria.Nombre))
            {
                Error = "Nombre vacío";
                return false;
            }
            if (categoria.Nombre.Length > 15)
            {
                Error = "Nombre supera la longitud permitida";
                return false;
            }
            if (categoria.Descripcion.Length > 50) 
            {
                Error = "Descripción supera la longitud permitida";
                return false;
            }
            if ((controlador.Count(categoria.Nombre) > 0) && Operacion==false) {
                Error = "El nombre de la categoría ya existe. Verificar estado.";
                return false;
            }
            if (categoria.Estado <= 0) {
                Error = "Estado no permitido";
                return false;
            }
            return true;
        }
        #endregion

    }
}