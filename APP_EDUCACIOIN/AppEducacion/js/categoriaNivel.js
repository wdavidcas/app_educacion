$(document).ready(function () {
    
    $("#btnBuscar").click(getContactos);
    
    function getContactos() {
        var param = { "nombre": "hola" };
        $.ajax({
            type: "POST",
            url: "CategoriaNivel.aspx/Listar",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                var contactos = (typeof response.d) == 'string' ?
                                   eval('(' + response.d + ')') :
                                   response.d;

                $('#listado').empty();
                //$('#tablaContactos').append('<table id="example" class="display" border="1" cellspacing="0" width="100%">');
                //$('#tablaContactos').append('<thead><tr><td><b>ID</b></td><td><b>Nombre</b></td><td><b>Telefono</b></td><td><b>EMail</b></td></tr></thead>');
                //$('#tablaContactos').append('<tfoot><tr><td><b>ID</b></td><td><b>Nombre</b></td><td><b>Telefono</b></td><td><b>EMail</b></td></tr></tfoot>');


                for (var i = 0; i < contactos.length; i++) {
                    $('#listado').append('<table><tr>' +
                                          '<td>' + contactos[i].PK + '</td>' +
                                          '<td>' + contactos[i].Nombre + '</td>' +
                                          '<td>' + contactos[i].Descripcion + '</td>' +
                                        '</tr></table>');
                }
                $('#listado').append('</table>');

                //$("#tablaContactos").DataTable();
            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }
        });
    }
});