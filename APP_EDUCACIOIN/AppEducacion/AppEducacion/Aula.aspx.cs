﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DAL;

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
            if (validarCategoriaNivel(miAula, Operacion))
            {
                return aula.Insertar(miAula, Operacion);
            }
            else
                return aula.Error;
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
        static bool validarCategoriaNivel(ModelAula nivel, bool Operacion)
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