<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Alumno.aspx.cs" Inherits="AppEducacion.Alumno" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/alumno.css" rel="stylesheet" />
    <script src="js/alumno.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--***********************Contenedor principal*********************-->
    <div class="container" style="width:90%">
        <!--titulo de la pagina-->
        <h4 style="text-align:center;color:#08088A" class="container">ADMINISTRACIÓN DE ALUMNOS</h4>
        <br />

        <div class="row">
             
            <div class="col-md-10">
                <!--********************Contenedor de la vista principal del formulario**************************-->
        <div class="panel panel-success" style="width:100%" id="panel">
            <!--**********filtros de la data**********-->
            <div class="panel-heading" style="width:100%">
                <div class="row">
                    <div class="col-md-9">
                        <label>Busqueda</label>&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                        <input type="text" id="txtBusqueda" placeholder="Ingresar texto a buscar" maxlength="100" style="width:400px" />
                        <label id="lblTipo">Tipo</label>
                <select id="ddlTiposBusquedas">
                    <option value="0">::Seleccione Tipo::</option>
                    <option value="1">Nombre</option>
                    <option value="2">Descripci&oacute;n</option>
                </select>

                                    <label>Estado</label>
                <select id="ddlEstados">
                    <option value="0">::Seleccione estado::</option>
                    <option value="1">Habilitado</option>
                    <option value="2">No Habilitado</option>                    
                </select>
                    </div>
                        <div class="col-md-3">                       
                        
                        <input type="button" class="btn btn-primary btn-xs" id="btnBuscar" value="Buscar"/> 
                        <input type="button" class="btn btn-primary btn-xs" id="btnRefrescar" value="Refrescar" />
                        <input type="button" class="btn btn-danger btn-xs" id="btnNuevo" value="Nuevo" />
                        <input type="button" class="btn btn-success btn-xs" id="btnExportar" value="Exportar" />
                    </div>
                </div>                  
            </div>

            <!--**********contenedor de la data**********-->
            <div id="data" class="panel-body">

            </div>

            <!--**********contenido de la paginacion, ayuda visual**********-->
            <div id="nombrepaginas" class="row">
                <div class="col-md-6" id="mensajePagina">
                </div>
                <div class="col-md-3" id="nombrepagina">
                </div>
                <div class="col-md-3" id="cantidadRegistros">
                </div>
            </div>

            <!--**********--paginacion**********-->
           <div id="paginacion" class="panel-footer">               
            </div>

        </div>
        <!--*********fin de la vista principal*********-->
            </div>

            <div class="col-md-2">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        OPCIONES
                    </div>                   
                    <div class="panel-body">
                         <input type="button" class="btn btn-info"  value="INFORMACION" />
                        <br />
                        <br />
                        <input type="button" class="btn btn-success"  value="COLEGIATURAS" />
                        <br />
                        <br />
                        <input type="button" class="btn btn-success" value="CALIFICACIONES" />
                        <br />
                        <br />
                        <input type="button" class="btn btn-success"  value="BIBLIOTECA" />
                        <br />
                        <br />
                         <input type="button" class="btn btn-success"  value="ENCARGADO(S)" />
                        <br />
                        <br />
                        <input type="button" class="btn btn-danger"  value="SUSPENSIONES" />
                        <br />
                        <br />
                       
                    </div>
                    <div class="panel-footer">

                    </div>
                </div>
            </div>
           
        </div>
        
        
        <!--#####################################################################################################-->
        <!--*********formulario para el registro de datos*********-->
        <div id="formulario" title="Informaci&oacute;n del Alumno" class="modal">            
            <div class="row">
                <input type="text" id="pk"/>
                <div class="col-md-2">
                    <label for="txtNombre">Nombre</label>
                </div>
                <div class="col-md-4">
                    <input id="txtNombre" type="text" placeholder="Ingrese nombre" maxlength="20" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,20)" />
                </div>
                <div class="col-md-2">
                    <label for="txtApellido">Apellido</label>
                </div>
                <div class="col-md-4">
                    <input id="txtApellido" type="text" placeholder="Ingrese apellido" maxlength="20" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,20)" />
                </div>
            </div>                    
            <div class="row">                
                <div class="col-md-2">
                    <label for="txtCui">DPI</label>
                </div>
                <div class="col-md-4">
                    <input id="txtCui" type="text" placeholder="Ingrese DPI" maxlength="13" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this);revisarLongitud(this,13)" />
                </div>
                <div class="col-md-2">
                    <label for="txtCarnet">Carnet</label>
                </div>
                <div class="col-md-4">
                    <input id="txtCarnet" type="text" placeholder="Ingrese Carnet" maxlength="10" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,10)" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <label for="txtDireccion">Dirección</label>
                </div>
                <div class="col-md-10">
                    <input type="text" maxlength="100" placeholder="Ingrese direccion" style="width:530px" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,10)"/>                    
                </div>
            </div>
            <div class="row">                
                <div class="col-md-2">
                    <label for="txtCorreo">Correo</label>
                </div>
                <div class="col-md-10">                    
                    <input id="txtCorreo" style="width:530px" type="text" placeholder="Ingrese correo electrónico" maxlength="50" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this);"/>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-2">
                    <label for="txtFechaNac">Nacimiento</label>
                </div>
                <div class="col-md-4">
                    <input id="txtFechaNac" type="text" placeholder="Seleccione fecha" maxlength="10" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,10)" />
                </div>

                 <div class="col-md-2">
                    <label for="ddlEstado">Estado</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlEstado" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)">
                        <option value="0">::Seleccionar:</option>
                        <option value="1">Habilitado</option>
                        <option value="2">No Habilitado</option>
                    </select>
                </div>               
            </div>

            <div class="row">
               <div class="col-md-2">
                    <label for="txtFoto">Foto</label>
                </div>
                <div class="col-md-10">
                    <input type="file" style="width:530px" id="txtFoto" placeholder="Seleccione fotografía"/>                    
                </div>
            </div>           
        </div>
        <!--********fin del formulario**********--> 
        
        <!--#####################################################################################################-->
        <!--dialogo par eliminar-->
        <div id="dialog-eliminar" title="¿Eliminar registro?">
            <p class="alert alert-danger" ><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>¿Est&aacute; seguro de eliminar el registro?</p>
        </div>
        
        <!--dialogo para registrar-->
        <div id="dialog-guardar" title="Inserción de datos">
            <p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 20px 0;"></span>La informaci&oacute;n se ha registrado correctamente</p>
        </div> 
          

         <!--dialogo para actualizar-->
        <div id="dialog-editar" title="Edición de datos">
            <p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 20px 0;"></span>La informaci&oacute;n se ha editado correctamente</p>
        </div> 

        <!--dialogo para actualizar-->
        <div id="dialog-msj" title="Validación">
            <p id="dialog-msj-texto"><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 20px 0;"></span>Por favor, verique que ha sucedido un error</p>
        </div> 
    </div>
    <!--**********fin del contenedor**********-->
</asp:Content>
