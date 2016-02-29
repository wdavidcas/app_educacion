/*
    SCRIPT para la manipulacion del lado del cliente
*/

$(window).load(function () {
    //*********************GLOBALES*************************************************
    //identifica el tipo de operacion
    var cargandoPagina = true;
    var esGuardar = false;
    var llave = 0;
    //total de registros en la tabla
    var cantidadRegistros = 0;
    //registros a mostrar por pagina
    var paginacion = 10;
    var paginaActual = 1;
    //cantidad de paginas
    var tamanioPagina = Math.floor(cantidadRegistros / paginacion);
    //arrays para el  nombre del encabezado
    var headColumnas = new Array();
    headColumnas.push(["NOMBRE", "DESCRIPCION", "EDITAR", "VER", "BORRAR"]);
    //array para el ancho de las columnas
    var widthColumnas = new Array();
    widthColumnas.push([45, 31, 8, 8, 8]);

    //**************************CONTENIDO INICIAL HTML********************************
    $("#ddlEstados").val(1);
    $("#ddlTiposBusquedas").css("display", "none");
    $("#lblTipo").css("display", "none");
    $("#pk").css("display", "none");
    $("#pk").val(0);
    $("#txtBusqueda").val("");
    $("#nombrepagina").html("Vista inicial");
    //***************************STRINGS***********************************************
    var informacionEstado = "Por favor, seleccione un estado";
    var informacionData = "No existe información almacenada en la BD";
    //**********************EJECUCION EN LOAD****************************************
    //realiza el conteo de los registros
    getCount();
    getData(0, paginacion);

    //*************************PETICIONES AJAX****************************************
    //funcion para realizar la llamadaAjax para obtener los datos
    function getData(filaInicial, cantidad) {
        //captura los parametros
        var busqueda = $("#txtBusqueda").val();
        var estado = $("select[id=ddlEstados]").val();

        //generar logica para los parametros
        var param = { "inicio": filaInicial, "paginacion": cantidad, "busqueda": busqueda, "estado": estado };

        //llamada ajax
        $.ajax({
            type: "POST",
            url: "CategoriaCurso.aspx/ObtenerListado",
            data: JSON.stringify(param),
            async: false,
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

    //funcion para recuperar un objeto del modelo de datos
    function getObjeto(idObjeto) {
        var param = { 'PK': idObjeto };
        $.ajax({
            type: "POST",
            url: "CategoriaCurso.aspx/Get",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //se captura el listado
                var listado = (typeof response.d) == 'string' ?
                                           eval('(' + response.d + ')') :
                                           response.d;

                if (listado.length > 0) {
                    var id = listado[0].PK;
                    var nombre = listado[0].Nombre;
                    var descripcion = listado[0].Descripcion;
                    var estado = listado[0].Estado;

                    $("#txtNombre").val(nombre);
                    $("#txtDescripcion").val(descripcion);
                    $("#ddlEstado").val(estado);
                    $("#pk").val(id);

                    if (esGuardar == null) {
                        $("#txtNombre").attr("disabled", -1);
                        $("#txtDescripcion").attr("disabled", -1);
                        $("#ddlEstado").attr("disabled", -1);
                    }
                    else {
                        $("#txtNombre").removeAttr("disabled");
                        $("#txtDescripcion").removeAttr("disabled");
                        $("#ddlEstado").removeAttr("disabled");
                    }
                }
                else {
                    alert("No se ha encontrado la información del objeto!!!");
                }
            },
            error: function (result) {
                alert('ERROR' + result.status + ' ' + result.statusText);
            }
        });
    }

    //funcion que realiza la peticion ajax para eliminar 
    function eliminarObjeto() {
        var param = { "PK": llave };

        $.ajax({
            type: "POST",
            url: "CategoriaCurso.aspx/Get",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //se captura el listado
                var listado = (typeof response.d) == 'string' ?
                                           eval('(' + response.d + ')') :
                                           response.d;

                if (listado.length > 0) {
                    var id = listado[0].PK;
                    var nombre = listado[0].Nombre;
                    var descripcion = listado[0].Descripcion;
                    var estado = listado[0].Estado;

                    llave = id;
                    estado = 3;
                    Guardar(nombre, descripcion, estado);

                }
                else {
                    alert("No se ha encontrado la información del objeto!!!");
                }
            },
            error: function (error) {
                alert('ERROR' + error.status + ' ' + error.statusText);
            }
        });
    }
    //funcion que realiza la peticion ajax para identificar la cantidad de registros recuperados
    function getCount() {
        //captura los parametros
        var busqueda = $("#txtBusqueda").val();
        var estado = $("select[id=ddlEstados]").val();
        var param = { "TextoBusqueda": busqueda, "Estado": estado };

        $.ajax({
            type: "POST",
            url: "CategoriaCurso.aspx/ObtenerCount",
            data: JSON.stringify(param),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: false,

            success: function (response) {
                cantidadRegistros = response.d;
                $("#cantidadRegistros").html("Cantidad de registros: " + cantidadRegistros);
            },
            error: function (result) {
                alert('ERROR' + result.status + ' ' + ref.statusText);
            }
        });
    }

    //***********************RESPONSE OF REQUEST*************************
    function crearTabla(response) {
        //se captura el listado
        var listado = (typeof response.d) == 'string' ?
                                   eval('(' + response.d + ')') :
                                   response.d;

        if (listado.length > 0) {
            //se crean los tags
            var table = document.createElement("TABLE");
            //table.border = 1;
            table.width = "100%";
            table.className = "table table-hover";
            //table.style.backgroundColor = "#C8D5B9";

            //var boton = document.createElement("BUTTON");
            var totalBotones = Math.ceil(cantidadRegistros / paginacion);

            var columnCount = headColumnas[0].length;

            //creacion del encabezado
            var row = table.insertRow(-1);
            row.className = "row";

            for (var i = 0; i < columnCount; i++) {
                var headerCell = document.createElement("TH");
                headerCell.innerHTML = headColumnas[0][i];
                headerCell.width = widthColumnas[0][i] + "%";
                row.appendChild(headerCell);
            }


            //creacion de las filas 
            for (var i = 0; i < listado.length; i++) {
                row = table.insertRow(-1);
                row.className = "row";
                for (var j = 0; j < columnCount; j++) {
                    var cell = row.insertCell(-1);

                    if (j == 2) {
                        cell.appendChild(crearEnlace(listado[i].PK, "Editar"));
                    }
                    else if (j == 3) {
                        cell.appendChild(crearEnlace(listado[i].PK, "Ver"));
                    }
                    else if (j == 4) {
                        cell.appendChild(crearEnlace(listado[i].PK, "Borrar"));
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
            dvTable.innerHTML = "";
            dvTable.appendChild(table);

            //creacion de la paginacion
            var pagi = document.getElementById("paginacion");
            pagi.className = "btn-group btn-group-xs";//"btn-toolbar";
            pagi.innerHTML = "";

            for (var i = 0; i < totalBotones; i++) {
                var miBoton = document.createElement("BUTTON");
                miBoton.className = "btn btn-success btn-xs";
                miBoton.id = i + 1;
                miBoton.value = i + 1;
                //miBoton.className = "btn-group btn-group-xs";
                miBoton.textContent = i + 1;
                miBoton.addEventListener("click", paginando, true);
                pagi.appendChild(miBoton);
            }
        }
        else {
            //alert("No se ha recuperado información de la base de datos");
            var dvTable = document.getElementById("data");
            dvTable.innerHTML = "No existe información almacenada en el sistema";
            dvTable.className = "errorCategoriaCurso";
        }
    }

    //**********para ir creando el aucomplete en la busqueda*********************
    $("#txtBusqueda").autocomplete({
        source: function (request, response) {
            //captura los parametros
            var busqueda = $("#txtBusqueda").val();
            var estado = $("select[id=ddlEstados]").val();

            var parametros = { "TextoBusqueda": $('#txtBusqueda').val(), "Estado": $('select[id=ddlEstados]').val() };
            $.ajax({
                type: "POST",
                url: "CategoriaCurso.aspx/GetBusqueda",
                data: JSON.stringify(parametros),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        minLength: 3
    });

    //***************************FIN PETICIONES AJAX************************
    //******************************FUNCTIONS AUXILIARES********************
    //funcion para redireccionar la paginacion
    function paginando(e) {
        //var seleccionado = document.getElementById(e.srcElement.id);
        //seleccionado.className = "btn btn-danger";
        var totalPaginas = Math.ceil(cantidadRegistros / paginacion);
        var pag = e.srcElement.value;
        paginaActual = pag;
        var ini = ((pag - 1) * paginacion);
        $("#nombrepagina").html("Vista " + pag + " de " + totalPaginas);
        getData(ini, paginacion);
    }

    //funcion para manipular acciones sobre el registro
    function accionRegistro(e) {
        var accion = e.srcElement.text;
        var elemento = e.srcElement.id;
        if (accion === "Borrar") {
            llave = elemento;
            $("#dialog-eliminar").dialog("open");
        }
        else if (accion == "Editar") {
            esGuardar = false;
            getObjeto(elemento);
            $("#formulario").dialog("open");
        }
        else if (accion == "Ver") {
            esGuardar = null;
            getObjeto(elemento);
            $("#formulario").dialog("open");
        }
    }

    //funcion para crear el enlace y agregarselo a la celda 
    function crearEnlace(id, etiqueta) {
        var enlace = document.createElement("A");
        enlace.href = "#";
        enlace.id = id;
        enlace.text = etiqueta;
        enlace.textContent = etiqueta;
        enlace.addEventListener("click", accionRegistro, true);
        return enlace;
    }

    //******************FIN FUNCIONES AUXILIARES************************    
    var buscar = document.getElementById("btnBuscar");
    var refrescar = document.getElementById("btnRefrescar");
    var agregar = document.getElementById("btnNuevo");

    agregar.addEventListener("click", abrirFormularioNuevo, false);
    buscar.addEventListener("click", ejecutarBusqueda, false);
    refrescar.addEventListener("click", ejecutarRefrescar, false);

    function abrirFormularioNuevo() {
        esGuardar = true;
        $("#pk").val(0);
        $("#txtNombre").val("");
        $("#txtDescripcion").val("");
        $("#ddlEstado").val(0);
        $("#txtNombre").removeAttr("disabled");
        $("#txtDescripcion").removeAttr("disabled");
        $("#ddlEstado").removeAttr("disabled");
        $('#formulario').dialog("open");
    }

    function ejecutarBusqueda() {
        getCount();
        getData(0, paginacion);
    }

    function ejecutarRefrescar() {
        $("#txtBusqueda").val("");
        $("#ddlEstados").val(1);
        getCount();
        getData(0, paginacion);
        $("#nombrepagina").html("Vista inicial");
    }


    //*******************FORMULARIOS**********************************
    //dialogo del formulario
    $('#formulario').dialog({
        autoOpen: false,
        height: 340,
        width: 400,
        modal: true,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");
            },
            "Aceptar": function () {
                //captura los campos para formar el objeto
                var nombre = $("#txtNombre").val();
                var descripcion = $("#txtDescripcion").val();
                var estado = $("select[id=ddlEstado]").val();
                llave = $("#pk").val();

                //guardar
                if (esGuardar == true) {
                    Guardar(nombre, descripcion, estado);
                    //getData((paginaActual-1)*paginacion,paginacion);
                } else if (esGuardar == false) {//editar
                    Guardar(nombre, descripcion, estado);
                    //getData((paginaActual - 1) * paginacion, paginacion);
                }
                else if (esGuardar == null) {
                    $("#formulario").dialog("close");
                }
            }
        }
    });

    //dialogo para eliminar
    $("#dialog-eliminar").dialog({
        autoOpen: false,
        resizable: false,
        height: 250,
        modal: true,
        buttons: {
            "Eliminar": function () {
                esGuardar = false;
                eliminarObjeto();
                $(this).dialog("close");
                getData((paginaActual - 1) * paginacion, paginacion);
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    //dialogo para editar
    $("#dialog-editar").dialog({
        autoOpen: false,
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "OK": function () {
                $(this).dialog("close");
            }
        }
    });


    //dialogo para guardar
    $("#dialog-guardar").dialog({
        autoOpen: false,
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "OK": function () {
                $(this).dialog("close");
            }
        }
    });

    //dialogo para guardar
    $("#dialog-msj").dialog({
        autoOpen: false,
        resizable: false,
        height: 200,
        modal: true,
        buttons: {
            "OK": function () {
                $(this).dialog("close");
            }
        }
    });

    //*************************OPERACIONES CRUD*******************
    //funcion para guardar
    function Guardar(nombre, descripcion, estado) {
        if (validarIngreso(nombre, descripcion, estado)) {
            var parametros = { "Pk": llave, "Nombre": nombre, "Descripcion": descripcion, "Estado": estado, "Operacion": esGuardar };

            $.ajax({
                type: "POST",
                url: "CategoriaCurso.aspx/Guardar",
                data: JSON.stringify(parametros),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    var r = response.d;
                    $("#ddlEstados").val(1);
                    if (r == "" && esGuardar == true) {
                        $("#dialog-guardar").dialog("open");
                        //alert("Informaci&oacute;n almacenada correctamente");
                        $("#formulario").dialog("close");
                        getCount();
                        getData((paginaActual - 1) * paginacion, paginacion);
                    } else if (r == "" && esGuardar == false) {
                        //alert("Informaci&oacute;n editada correctamente");
                        $("#dialog-editar").dialog("open");
                        $("#formulario").dialog("close");
                        $("#pk").val(0);
                        getCount();
                        getData((paginaActual - 1) * paginacion, paginacion);
                    }
                    else {
                        alert("Por favor, verifique");
                    }
                },
                error: function (result) {
                    alert()
                }
            });
        }
    }

   //**********************************validaciones**************
    function validarIngreso(nombre,descripcion,estado)
    {
        if (nombre == "" || nombre == null) {
            mostrarMensaje("Por favor, ingrese nombre");
            return false;
        }

        if (verificarPalabrasSQL(nombre)) {
            mostrarMensaje("El nombre incluye palabras no permitidas");
            return false;
        }

        if (verificarCaracteresEspeciales(nombre)) {
            mostrarMensaje("No se permiten carácteres especiales en el nombre");
            return false;
        }

        if (verificarCaracteresEspeciales(descripcion)) {
            mostrarMensaje("No se permiten carácteres especiales en la descripción");
            return false;
        }

        if (verificarTamanio(nombre, 25)) {
            mostrarMensaje("Nombre supera la longitud permitida");
            return false;
        }

        if (verificarTamanio(descripcion, 50)) {
            mostrarMensaje("Descripción supera la longitud permitida");
            return false;
        }

        if (estado <= 0) {
            mostrarMensaje("Por seleccione un estado");
            return false;
        }
        return true;
    }

    //funcion para mostrar un error en especifico
    function mostrarMensaje(mensaje) {
        $("#dialog-msj-texto").html(mensaje);
        $("#dialog-msj").dialog("open");
    }

});


