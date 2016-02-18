/*
    SCRIPT para la manipulacion del lado del cliente
*/
$(document).ready(function () {

    //*********************GLOBALES*************************************************

    //total de registros en la tabla
    var cantidadRegistros = 22;
    //registros a mostrar por pagina
    var paginacion = 5;
    //cantidad de paginas
    var tamanioPagina = Math.floor(cantidadRegistros / paginacion);
    //arrays para el  nombre del encabezado
    var headColumnas = new Array();
    headColumnas.push(["Nombre", "Descripci&oacute;n", "Editar", "Ver", "Borrar"]);
    //array para el ancho de las columnas
    var widthColumnas = new Array();    
    widthColumnas.push([45, 31, 8, 8, 8]);

    //**********************EJECUCION EN LOAD****************************************
    //la primera carga se hace en la primera pagina
    getData(1, paginacion - 1);

    //*************************PETICION AJAX****************************************

    //funcion para realizar la llamadaAjax
    function getData(filaInicial,cantidad) {                      
        
        if (filaInicial == 1)
            cantidad = cantidad + 1;

        //generar logica para los parametros
        var param = { "inicio": filaInicial, "fin": cantidad,"busqueda":'',"estado":1 };

        //llamada ajax
        $.ajax({ 
            type: "POST", 
            url: "CategoriaNivel.aspx/ObtenerListado", 
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

                if (j == 2)
                {
                   cell.appendChild(crearEnlace(listado[i].PK,"Editar"));                                       
                }
                else if (j == 3)
                {                    
                    cell.appendChild(crearEnlace(listado[i].PK,"Ver"));
                }
                else if (j == 4)
                {
                    cell.appendChild(crearEnlace(listado[i].PK,"Borrar"));
                }
                else if (j == 0) {
                    cell.innerHTML = listado[i].Nombre;
                }
                else if (j == 1) {
                    cell.innerHTML = listado[i].Descripcion;
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
        var fin = paginacion;//ini + paginacion - 1;
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


