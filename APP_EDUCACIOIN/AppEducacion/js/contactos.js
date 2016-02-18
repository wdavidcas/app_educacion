/*
    SCRIPT para la manipulacion del lado del cliente
*/
$(document).ready(function () {

    //*********************GLOBALES*************************************************

    //total de registros en la tabla
    var cantidadRegistros = 23;
    //registros a mostrar por pagina
    var paginacion = 8;
    //cantidad de paginas
    var tamanioPagina = Math.floor(cantidadRegistros / paginacion);
    //arrays para el  nombre del encabezado
    var headColumnas = new Array();
    headColumnas.push(["NOMBRE", "TELEFONO", "CORREO", "Editar", "Visualizar", "Eliminar"]);
    //array para el ancho de las columnas
    var widthColumnas = new Array();    
    widthColumnas.push([45, 10, 15, 10, 10, 10]);

    //**********************EJECUCION EN LOAD****************************************
    //la primera carga se hace en la primera pagina
    getData(1, paginacion - 1);

    //*************************PETICION AJAX****************************************

    //funcion para realizar la llamadaAjax
    function getData(filaInicial,cantidad) {                      
        
        if (filaInicial == 1)
            cantidad = cantidad + 1;

        //generar logica para los parametros
        var param = { "inicio": filaInicial, "fin": cantidad };

        //llamada ajax
        $.ajax({ 
            type: "POST", 
            url: "Contactos.aspx/ObtenerContactos", 
            data: JSON.stringify(param),
            async:false,
            contentType: "application/json; charset=utf-8", 
            dataType: "json", 
            success: function (response) {
                //funcion handler de la respuesta
                crearTabla(response);
            }, 
            error: function (result) { 
                alert('ERROR ' + result.status + ' ' + result.statusText); 
            } 
        }); 
    }

    //***********************RESPONSE OF REQUEST*************************
    function crearTabla(response) {

        //se captura el listado
        var listado = (typeof response.d) == 'string' ?
                                   eval('(' + response.d + ')') :
                                   response.d;
        
        //se crean los tags
        var table = document.createElement("TABLE");
        table.border = 1;
        table.width = "100%";
        table.className = "table table-hover";

        var boton = document.createElement("BUTTON");
        var totalBotones = Math.ceil(cantidadRegistros / paginacion);
        
        var columnCount = headColumnas[0].length;

        //creacion del encabezado
        var row = table.insertRow(-1);

        for (var i = 0; i < columnCount; i++) {
            var headerCell = document.createElement("TH");
            headerCell.innerHTML = headColumnas[0][i];
            headerCell.width = widthColumnas[0][i] + "%";
            row.appendChild(headerCell);
        }


        //creacion de las filas 
        for (var i = 0; i < listado.length; i++) {
            row = table.insertRow(-1);

            for (var j = 0; j < columnCount; j++) {
                var cell = row.insertCell(-1);                

                if (j == 3)
                {
                    var enlace = document.createElement("A");
                    enlace.href = "#";

                    enlace.id = listado[i].IdContacto;
                    enlace.text = "Editar";
                    enlace.textContent = "Editar";
                    enlace.addEventListener("click", accionRegistro, true);
                    cell.appendChild(enlace);                   
                    
                }
                else if (j == 4)
                {                    
                    cell.appendChild(crearEnlace(listado[i].IdContacto,"Visualizar"));
                }
                else if (j == 5)
                {
                    var enlace = document.createElement("A");
                    enlace.href = "#";

                    enlace.id = listado[i].IdContacto;
                    enlace.text = "Eliminar";
                    enlace.textContent = "Eliminar";
                    enlace.addEventListener("click", accionRegistro, true);
                    cell.appendChild(enlace);
                }
                else if (j == 0) {
                    cell.innerHTML = listado[i].Nombre;
                }
                else if (j == 1) {
                    cell.innerHTML = listado[i].Telefono;
                }
                else if (j == 2) {
                    cell.innerHTML = listado[i].Email;
                }

            }
        }

        //añadiendo tabla creada                
        var dvTable = document.getElementById("data");
        dvTable.className = "panel-body";
        dvTable.innerHTML = "";
        dvTable.appendChild(table);

        //creacion de la paginacion
        var pagi = document.getElementById("paginacion");
        pagi.className = "btn-toolbar";        
        pagi.innerHTML = "";
        

        for (var i = 0; i < totalBotones; i++) {            
            var miBoton = document.createElement("BUTTON");
            miBoton.id = i+1;
            miBoton.value = i+1;
            miBoton.className = "btn-group btn-group-xs";            
            miBoton.textContent = i+1;            
            miBoton.addEventListener("click", paginando, true);            
            pagi.appendChild(miBoton);
        }                      
    }

    //******************************FUNCTIONS AUXILIARES********************
    //funcion para redireccionar la paginacion
    function paginando(e) {
        var pag = e.srcElement.value;
        var ini = ((pag - 1) * paginacion) + 1;
        var fin = ini + paginacion - 1;
        var tam = Math.ceil(cantidadRegistros / paginacion);
        if (pag == tam)
            fin = cantidadRegistros;
        getData(ini, fin);
    }

    //funcion para manipular acciones sobre el registro
    function accionRegistro(e) {
        var accion = e.srcElement.text;
        var elemento = e.srcElement.id;
        if (accion === "Eliminar") {
            alert(accion + " el identificador " + elemento);
        }
        else if (accion == "Editar") {
            alert(accion + " el identificador " + elemento);
        }
        else if (accion == "Visualizar") {
            alert(accion + " el identificador " + elemento);
        }
    }

    //funcion para crear el enlace y agregarselo a la celda 
    function crearEnlace(id,etiqueta) {
        var enlace = document.createElement("A");
        enlace.href = "#";
        enlace.id = id;
        enlace.text = etiqueta;
        enlace.textContent = etiqueta;
        enlace.addEventListener("click", accionRegistro, true);
        return enlace;
    }

});


