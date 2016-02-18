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
    public partial class CategoriaNivel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static List<ModelCategoriaNivel> Listar(string nombre)
        {
            CategoriaNivels cat = new CategoriaNivels();
            return cat.Listar();
            
        }
    }
}