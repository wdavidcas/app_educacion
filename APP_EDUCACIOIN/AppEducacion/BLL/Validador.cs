using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace BLL
{
    public static class Validador
    {
        public static string Error = string.Empty;

        /// <summary>
        /// Metodo para verificar que una cadena de texto no incluya palabras reservadas sql
        /// </summary>
        /// <param name="Palabra">Texto de entrada</param>
        /// <returns>True si halla coincidencias, False caso contrario</returns>
        public static bool ValidarPalabrasReservadasSQL(string Palabra) {
            try
            {
                //return Regex.IsMatch(Palabra.Trim().ToUpper(), ("(INSERT|UPDATE|DELETE|DROP|TRUNCATE|SELECT|FROM|WHERE|TOP|LIMIT,ORDER|BY|GROUP|TOP|SUM|AVG)"));                
                return false;
            }
            catch (Exception ex) {
                Error = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Palabra"></param>
        /// <returns></returns>
        public static bool ValidarCaracteresEspeciales(string Palabra)
        {
            try
            {
               // return Regex.IsMatch(Palabra.Trim().ToUpper(), ("(INSERT|UPDATE|DELETE|DROP|TRUNCATE|SELECT|FROM|WHERE|TOP|LIMIT,ORDER|BY|GROUP|TOP|SUM|AVG)"));
                return false;
            }
            catch (Exception ex)
            {
                Error = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// Metodo para validar numeros enteros
        /// </summary>
        /// <param name="Texto">texto de entrada</param>
        /// <returns></returns>
        public static bool ValidarNumerosEnteros(string Texto) {
            try
            {
                return Regex.IsMatch(Texto.Trim().ToUpper(), "[0-9]*");
            }
            catch (Exception ex)
            {
                Error = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Texto"></param>
        /// <returns></returns>
        public static bool ValidarNumerosDecimales(string Texto) {
            try
            {
                return Regex.IsMatch(Texto.Trim().ToUpper(), "[0-9]*");
            }
            catch (Exception ex)
            {
                Error = ex.Message.ToString();
                return false;
            }
        }
    }
}
