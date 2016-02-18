function crearDropDrownLIst(parametros, dirUrl, tipo,controlPrincipal,valueMemeber,displayMember)
{
    $.ajax({
        type:tipo,
        url: dirUrl,
        data: JSON.stringify(parametros),
        async:false,
        contentType:"application/json charset=utf-8",
        dataType:"json",
        success: function (response) {
            //se captura el listado
            var listado = (typeof response.d) == 'string' ?
                                       eval('(' + response.d + ')') :
                                       response.d;
            //se captura el control contenedor
            var contenedor = document.getElementById(controlPrincipal);

            var opcion = document.createElement("OPTION");
            opcion.value = 0;
            opcion.textContent = "Seleccione valor::";
            contenedor.appendChild(opcion);

            for (var i = 0; i < listado.length; i++) {
                var opcion = document.createElement("OPTION");
                var valor = listado + "[" + i + "]" + valueMemeber;
                var display = listado + "[" + i + "]" + displayMember;
                opcion.value = valor;
                opcion.textContent = display
                contenedor.appendChild(opcion);
            }
        
        },
        error:function(result){
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
    });
}