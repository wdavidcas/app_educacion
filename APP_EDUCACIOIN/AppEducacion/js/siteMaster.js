
$(document).ready(function () {

    //captura del dom
    var btnIngresar = document.getElementById("btnIngreso");
    var btnRecuperar = document.getElementById("btnRecuperar");
    var btnRegistrar = document.getElementById("btnRegistrar");

    btnRecuperar.addEventListener("click", mostrarRecuperar, true);
    btnIngresar.addEventListener("click",mostrarLogin,true);
    btnRegistrar.addEventListener("click", function () { window.open("www.google.com")}, true);

    function mostrarRecuperar() {
        $('#Login').dialog('close');
        $('#Recuperar').dialog('open');
    }

    function mostrarLogin(){
        console.info("Dentro de funcion MostrarLogin");
        $('#Login').dialog("open");
        
    }

    //configuracion del dialogo del login
    $('#Login').dialog({
        autoOpen: false,
        height: 300,
        width: 450,
        modal: true,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");
            },
            "Ingresar": function () {
                alert("Verificar ingreso");
            }
            }
    });

    //configuracion del dialogo de recuperacion de credenciales
    $('#Recuperar').dialog({
        autoOpen: false,
        height: 300,
        width: 400,
        modal: true,
        buttons: {
            "Cerrar": function () {
                $(this).dialog('close');
            },
            "Recuperar": function () {
                alert("Revisar su email para sus credenciales");
            }
        }            
    });
});