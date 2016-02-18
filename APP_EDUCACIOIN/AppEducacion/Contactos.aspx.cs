using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace AppRotulacionPruebas
{
    public partial class Contactos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                
            }
        }

        [WebMethod]
        public static List<Contacto> ObtenerContactos(int inicio,int fin)
        {
            List<Contacto> devolver = new List<Contacto>();
            devolver.Clear();
            List<Contacto> lista = new List<Contacto>();
            lista.Clear();

            lista.Add(new Contacto { IdContacto=1, Nombre="Pedro", Telefono="601 01 01 01",
                               Email = "pedrog@gmail.com" });

            lista.Add(new Contacto { IdContacto=2, Nombre="Maria", Telefono="601 01 01 02",
                               Email = "maria69@gmail.com"  });

            lista.Add(new Contacto { IdContacto=3, Nombre="Juana", Telefono="601 01 01 02",
                               Email = "juan69@gmail.com"  });

            lista.Add(new Contacto { IdContacto=4, Nombre="Roberta", Telefono="601 01 01 03",
                               Email = "roberta33@gmail.com"  });

            lista.Add(new Contacto { IdContacto=5, Nombre="Pedro", Telefono="601 01 01 01",
                               Email = "pedrog@gmail.com" });

            lista.Add(new Contacto { IdContacto=6, Nombre="Alejandro", Telefono="601 01 01 02",
                               Email = "alejandro@gmail.com"  });

            lista.Add(new Contacto { IdContacto=7, Nombre="Pablo", Telefono="601 01 01 03",
                               Email = "pablo@gmail.com"  });

            lista.Add(new Contacto { IdContacto=8, Nombre="Iván", Telefono="601 01 01 03",
                               Email = "ivan33@gmail.com"  });

            lista.Add(new Contacto { IdContacto=9, Nombre="Walter", Telefono="601 01 01 03",
                               Email = "walter@gmail.com"  });

            lista.Add(new Contacto { IdContacto=10, Nombre="David", Telefono="601 01 01 03",
                               Email = "david@gmail.com"  });

            lista.Add(new Contacto { IdContacto=11, Nombre="Gustavo", Telefono="601 01 01 03",
                               Email = "gustavo@gmail.com"  });

            lista.Add(new Contacto { IdContacto=12, Nombre="Ricardo", Telefono="601 01 01 03",
                               Email = "Ricardo@gmail.com"  });

            lista.Add(new Contacto { IdContacto=13, Nombre="Desarrollo", Telefono="601 01 01 03",
                               Email = "desarrollo@gmail.com"  });

            lista.Add(new Contacto { IdContacto=14, Nombre="Informática", Telefono="601 01 01 03",
                               Email = "informatica@gmail.com"  });

            lista.Add(new Contacto { IdContacto=15, Nombre="Daniel", Telefono="601 01 01 03",
                               Email = "Daniel@gmail.com"  });

            lista.Add(new Contacto { IdContacto=16, Nombre="Luis", Telefono="601 01 01 03",
                               Email = "luis@gmail.com"  });

            lista.Add(new Contacto { IdContacto=17, Nombre="Humberto", Telefono="601 01 01 03",
                               Email = "humberto@gmail.com"  });

            lista.Add(new Contacto
            {
                IdContacto = 18,
                Nombre = "Melvin",
                Telefono = "601 01 01 03",
                Email = "melvin@gmail.com"
            });

            lista.Add(new Contacto
            {
                IdContacto = 19,
                Nombre = "Carlos",
                Telefono = "601 01 01 03",
                Email = "carlos@gmail.com"
            });

            lista.Add(new Contacto
            {
                IdContacto = 20,
                Nombre = "Douglas",
                Telefono = "601 01 01 03",
                Email = "douglas@gmail.com"
            });

            lista.Add(new Contacto
            {
                IdContacto = 21,
                Nombre = "Omar",
                Telefono = "601 01 01 03",
                Email = "omar@gmail.com"
            });

            lista.Add(new Contacto
            {
                IdContacto = 22,
                Nombre = "XXX",
                Telefono = "601 01 01 03",
                Email = "omar@gmail.com"
            });

            lista.Add(new Contacto
            {
                IdContacto = 23,
                Nombre = "YYYY",
                Telefono = "601 01 01 03",
                Email = "omar@gmail.com"
            });

            for (int i = inicio; i <= fin; i++)
            {
                Contacto con = new Contacto();
                con.IdContacto=Convert.ToInt32(lista[i-1].IdContacto);
                con.Nombre=lista[i-1].Nombre;
                con.Telefono=lista[i-1].Telefono;
                con.Email=lista[i-1].Email;
                devolver.Add(con);
            }

            return devolver;           
        }
    }

    /// <summary>
    /// clase de los contactos
    /// </summary>
    public class Contacto
    {
        public int IdContacto { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    } 
}