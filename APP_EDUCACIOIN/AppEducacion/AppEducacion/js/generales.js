/*
Scritp para la parte visual de los formularios
*/

//funcion que aplica ayuda visual al elemento cuando el control html tiene el foco
function entroEnFoco(elemento) {
    elemento.className = "enfoco";
    /*var texto = "ayuda";
    var padre = elemento.parentNode;

    var help = document.getElementById("ayuda");
    if (help!=undefined) {
        help.parentNode.removeChild(help);        
    }
    padre.innerHTML += "<p id='ayuda' class='ayuda'>466546</p>";*/
}

//funcion que elimina la ayuda visual aplicada por la funcion entroEnFoco cuando pierde el foco el control
 function salioDeFoco(elemento) {
     elemento.className = "";     
 }

//funcion que aplica una clase css a campos obligatorios
 function revisarObligatorio(elemento) {
     if (elemento.value == "") {
         elemento.className = "error";
     }
     else {
         elemento.className = "";
     }
 }

//funcion que revisa la longitud minima de un campo
 function revisarLongitud(elemento, minimoDeseado) {
     if (elemento.value != "") {
         if (elemento.value.length > minimoDeseado)
             elemento.className = "error";
         else
             elemento.className = "";
     }
 }

//funcion que convierte a mayusculas o minusculas
 function convertir(elemento, opcion) {
     if (elemento.value != "") {

         if (elemento.value.length > 0) {
             if (opcion == "min") {
                 elemento.value = elemento.value.toLowerCase();
             }
             else if (opcion == "may") {
                 elemento.value = elemento.value.toUpperCase();                 
             }
         }         
     }
 }

//**********************validaciones****************************
 //funcion para mostrar un error en especifico
 function mostrarMensaje(mensaje) {
     $("#dialog-msj-texto").html(mensaje);
     $("#dialog-msj").dialog("open");
 }

//valida el no permitir palabras reservadas sql
 function verificarPalabrasSQL(texto)
 {
     var expreg = RegExp("(INSERT|UPDATE|DELETE|TRUNCATE|DROP|ALTER|SELECT|LIMIT|SUM|AVG|FROM|JOIN|INNER)");
     return expreg.test(texto);
 }

//valida el tamaño del control
 function verificarTamanio(texto, tamanio) {
     if (texto.length > tamanio)
         return true;
     else
         return false;
         
 }

//funcion que valida caracteres especiales
 function verificarCaracteresEspeciales(texto) {
     //var expreg = new RegExp("(Á|É|Í|Ó|Ú|á|é|í|ó|ú|ñ|\.|,|:|;|-|\*|\+|}|{|[|]|_)");
     //return expreg.test(texto);
     return false;
 }