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
            if (ValidarModelo(miModulo, Operacion))
            {
                return nivel.Insertar(miModulo, Operacion);
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
        static bool ValidarModelo(ModelModulo modulo, bool Operacion)
        {
            ControllerModulo controlador = new ControllerModulo();

            if (string.IsNullOrEmpty(modulo.Nombre))
            {
                Error = "Por favor, ingrese nombre del módulo.";
                return false;
            }

            if (modulo.Nombre.Trim().Length > 50)
            {
                Error = "El nombre supera la longitud permitida.";
                return false;
            }

            if (controlador.Count(modulo.Nombre.Trim().ToUpper(), modulo.Edificio_Id, modulo.Estado) > 0 && !Operacion)
            {
                Error = "Existe un nombre de módulo vinculado al mismo edificioo y estado.";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(modulo.Nombre.Trim()))
            {
                Error = "El nombre incluye palabras no permitidas.";
                return false;
            }

            /*if (Validador.VerificarCaracteresEspeciales(edificio.Nombre.Trim()))
            {
                Error = "No se permiten carácteres especiales en el nombre";
                return false;
            }*/

            if (modulo.Descripcion.Trim().Length > 50)
            {
                Error = "La descripción supera la longitud permitida";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(modulo.Descripcion.Trim()))
            {
                Error = "La descripción incluye palabras no permitidas.";
                return false;
            }

            /*if(Validador.VerificarCaracteresEspeciales(edificio.Descripcion.Trim()))
            {
                Error = "No se permiten caracteres especiales en la descripción.";
                return false;
            }*/

            if (modulo.Edificio_Id < 0)
            {
                Error = "Debe seleccionar un edificio.";
                return false;
            }

            if (modulo.Estado <= 0)
            {
                Error = "Estado no permitido";
                return false;
            }
            return true;
        }

        #endregion
    }
}