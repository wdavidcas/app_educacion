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
    public partial class Aula : System.Web.UI.Page
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
        public static List<ModelAula> ObtenerListado(int inicio, int paginacion, string busqueda, int estado)
        {
            ControllerAula aula = new ControllerAula();
            List<ModelAula> listado = new List<ModelAula>();

            if (!string.IsNullOrEmpty(busqueda))
                listado = aula.Listar(inicio, paginacion, busqueda, estado);
            else
                listado = aula.Listar(inicio, paginacion, estado);
            return listado;
        }

        /// <summary>
        /// Recupera la informacion de un objeto
        /// </summary>
        /// <param name="PK"> identificador</param>
        /// <returns>objeto Categoria Nivel</returns>
        [WebMethod]
        public static List<ModelAula> Get(int PK)
        {
            List<ModelAula> lista = new List<ModelAula>();
            ControllerAula aula = new ControllerAula();
            return aula.Listar(PK);
        }

        /// <summary>
        /// Metodo para el autocomplete en la busqueda
        /// </summary>
        /// <param name="TextoBusqueda">filtro</param>
        /// <returns>Arrays de nombres coincidentes</returns>
        [WebMethod]
        public static List<string> GetBusqueda(string TextoBusqueda, int Estado)
        {
            ControllerAula aula = new ControllerAula();
            return aula.Listar(TextoBusqueda, Estado);
        }

        /// <summary>
        /// Metodo para insertar
        /// </summary>
        /// <param name="Nombre">Nombre</param>
        /// <param name="Descripcion">Descripcion</param>
        /// <param name="Estado">Estado</param>
        /// <returns>True=Operacion correcta, False=Operacion Incorrecta</returns>
        [WebMethod]
        public static string Guardar(int Pk, string Nombre, string Descripcion, int Estado, int Modulo_Id, bool Operacion)
        {
            ModelAula miAula = new ModelAula(Pk, Nombre, Descripcion, Estado, Modulo_Id);
            ControllerAula aula = new ControllerAula();
            if (ValidarModelo(miAula, Operacion))
            {
                return aula.Insertar(miAula, Operacion);
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
            ControllerAula aula = new ControllerAula();
            int cant = aula.Count(TextoBusqueda, Estado);
            return cant;
        }
        #endregion


        [WebMethod]
        public static List<ModelModulo> ObtenerModulos(int EdificioId,int Estado, string Orden, string CampoOrden)
        {
            ControllerModulo modulo = new ControllerModulo();
            return modulo.Listar(EdificioId,Estado, Orden, CampoOrden);
        }

        [WebMethod]
        public static List<ModelEdificio> ObtenerEdificios(int Estado, string Orden, string CampoOrden) {
            ControllerEdificio edificio = new ControllerEdificio();
            return edificio.Listar(Estado, Orden, CampoOrden);
        }
        #region VALIDACIONES

        /// <summary>
        /// valida los campos del objeto del modelo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        static bool ValidarModelo(ModelAula aula, bool Operacion)
        {
            ControllerAula controlador = new ControllerAula();

            if (string.IsNullOrEmpty(aula.Nombre))
            {
                Error = "Por favor, ingrese nombre del aula.";
                return false;
            }

            if (aula.Nombre.Trim().Length > 50)
            {
                Error = "El nombre supera la longitud permitida.";
                return false;
            }

            if ((Operacion==false) && (controlador.Count(aula.Nombre, aula.Estado, aula.Modulo_Id)>0))
            {
                Error = "Existe un aula con el mismo nombre en el módulo y estado seleccionado.";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(aula.Nombre.Trim()))
            {
                Error = "El nombre incluye palabras no permitidas.";
                return false;
            }

            /*if (Validador.VerificarCaracteresEspeciales(edificio.Nombre.Trim()))
            {
                Error = "No se permiten carácteres especiales en el nombre";
                return false;
            }*/

            if (aula.Descripcion.Trim().Length > 50)
            {
                Error = "La descripción supera la longitud permitida";
                return false;
            }

            if (Validador.ValidarPalabrasReservadasSQL(aula.Descripcion.Trim()))
            {
                Error = "La descripción incluye palabras no permitidas.";
                return false;
            }

            /*if(Validador.VerificarCaracteresEspeciales(edificio.Descripcion.Trim()))
            {
                Error = "No se permiten caracteres especiales en la descripción.";
                return false;
            }*/

            if (aula.Modulo_Id < 0)
            {
                Error = "Debe seleccionar un módulo.";
                return false;
            }

            if (aula.Estado <= 0)
            {
                Error = "Estado no permitido";
                return false;
            }
            return true;
        }

        #endregion
    }
}