$(document).ready(function () {

    function desplegar() {
        alert("clic sobre boton");
    }
    $('#btnObtContactos').click(getContactos);

    //$("#example").DataTable();

    function getContactos() {
        var param = { "nombre": "hola" };
        $.ajax({
            type: "POST",
            url: "Contactos.aspx/ObtenerContactos",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var contactos = (typeof response.d) == 'string' ?
                                   eval('(' + response.d + ')') :
                                   response.d;

                var encabezados = new Array();
                encabezados.push(["ID", "NOMBRE", "TELEFONO", "CORREO"]);

                var table = document.createElement("TABLE");
                var boton = document.createElement("BUTTON");

                table.border = 1;
                table.width = "100%";


                var columnCount = encabezados[0].length;
                //var row = table.insertRow(-1);

                /*var headerCell = document.createElement("TH");
                headerCell.innerHTML = "ID";
                row.appendChild(headerCell);
                var headerCell = document.createElement("TH");
                headerCell.innerHTML = "NOMBRE";
                var headerCell = document.createElement("TH");
                headerCell.innerHTML = "TELEFONO";
                var headerCell = document.createElement("TH");
                headerCell.innerHTML = "CORREO";*/


                //Add the header row.
                var row = table.insertRow(-1);
                for (var i = 0; i < columnCount; i++) {
                    var headerCell = document.createElement("TH");
                    headerCell.innerHTML = encabezados[0][i];
                    headerCell.width = "25%";
                    row.appendChild(headerCell);
                }

                //Add the data rows.
                for (var i = 1; i < contactos.length; i++) {
                    row = table.insertRow(-1);
                    for (var j = 0; j < columnCount; j++) {
                        var cell = row.insertCell(-1);
                        if (j == 0)
                            cell.innerHTML = contactos[i].IdContacto;
                        else if (j == 1)
                            cell.innerHTML = contactos[i].Nombre;
                        else if (j == 2)
                            cell.innerHTML = contactos[i].Telefono;
                        else
                            cell.innerHTML = contactos[i].Email;
                    }
                }

                var dvTable = document.getElementById("data");
                var pagi = document.getElementById("paginacion");
                pagi.innerHTML = "";
                for (var i = 0; i < 5; i++) {
                    var miBoton = document.createElement("BUTTON");
                    miBoton.id = i;
                    miBoton.value = i;
                    miBoton.className = "btn btn-primary";
                    miBoton.textContent = i;
                    miBoton.onclick = "desplegar";
                    miBoton.addEventListener("click", "desplegar", true);
                    pagi.appendChild(miBoton);
                }

                table.className = "table table-hover";
                dvTable.innerHTML = "";
                dvTable.appendChild(table);

                /*$('#tablaContactos').empty();
                //$('#tablaContactos').append('<table id="example" class="display" border="1" cellspacing="0" width="100%">');
                $('#tablaContactos').append('<thead><tr><td><b>ID</b></td><td><b>Nombre</b></td><td><b>Telefono</b></td><td><b>EMail</b></td></tr></thead>');
                $('#tablaContactos').append('<tfoot><tr><td><b>ID</b></td><td><b>Nombre</b></td><td><b>Telefono</b></td><td><b>EMail</b></td></tr></tfoot>');

            
                for (var i = 0; i < contactos.length; i++) { 
                    $('#tablaContactos').append('<tbody><tr>' + 
                                          '<td>' + contactos[i].IdContacto + '</td>' + 
                                          '<td>' + contactos[i].Nombre + '</td>' + 
                                          '<td>' + contactos[i].Telefono + '</td>' + 
                                          '<td>' + contactos[i].Email + '</td>' + 
                                        '</tr></tbody>'); 
                }
                $('#tablaContactos').append('</table>');

                $("#tablaContactos").DataTable();*/
            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }
        });
    }


});


