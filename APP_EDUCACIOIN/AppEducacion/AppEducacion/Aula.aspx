<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Aula.aspx.cs" Inherits="AppEducacion.Aula" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/aula.css" rel="stylesheet" />
    <script src="js/aula.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--***********************Contenedor principal*********************-->
    <div class="container" style="width:90%">
        <!--titulo de la pagina-->
        <h4 style="text-align:center;color:#08088A" class="container">ADMINISTRACIÓN DE AULAS</h4>
        <br />

        <!--********************Contenedor de la vista principal del formulario**************************-->
        <div class="panel panel-success" style="width:100%">
            <!--**********filtros de la data**********-->
            <div class="panel-heading" style="width:100%">
                <div class="row">
                    <div class="col-md-9">
                        <label>Busqueda</label>&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                        <input type="text" title="Texto a buscar" id="txtBusqueda" placeholder="Ingresar texto a buscar" maxlength="100" style="width:400px" onkeypress="convertir(this,'may')" onkeydown="convertir(this,'may')"/>
                        

                        <label>Estado</label>
                        <select id="ddlEstados">
                            <option value="0">::Seleccionar estado::</option>
                            <option value="1">Habilitado</option>
                            <option value="2">No Habilitado</option>                    
                        </select>
                     </div>
                     <div class="col-md-3">                                               
                        <input type="button" class="btn btn-primary btn-xs" id="btnBuscar" value="Buscar" title="Realizar busqueda"/> 
                        <input type="button" class="btn btn-primary btn-xs" id="btnRefrescar" value="Refrescar" title="Mostrar toda la información" />
                        <input type="button" class="btn btn-danger btn-xs" id="btnNuevo" value="Nuevo" title="Agregar nuevo registro" />
                        <input type="button" class="btn btn-success btn-xs" id="btnExportar" value="Exportar" title="Exportar la información"/>
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

            
           <div id="paginacion" class="panel-footer">               
            </div>

        </div>
        <!--*********fin de la vista principal*********-->
        
        <!--#####################################################################################################-->
        <!--*********formulario para el registro de datos*********-->
        <div id="formulario" title="Informaci&oacute;n del Aula" class="modal">            
            <div class="row">
                <input type="text" id="pk"/>
                <div class="col-md-4">
                    <label>Nombre</label>
                </div>
                <div class="col-md-4">
                    <input id="txtNombre" title="Nombre del aula" type="text" placeholder="Ingrese nombre" maxlength="25" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,25)" onkeypress="convertir(this,'may')"/>
                </div>
            </div>
            <br />
            
            <div class="row">
                <div class="col-md-4">
                    <label>Descripci&oacute;n</label>
                </div>
                <div class="col-md-4">
                    <textarea id="txtDescripcion" title="Descripción del aula" rows="4" placeholder="Ingrese descripci&oacute;n" maxlength="50" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this);revisarLongitud(this,50)" onkeypress="convertir(this,'may')" onkeydown="convertir(this,'may')"></textarea>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label>Edificio</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlEdificios" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)" title="Nombre del edificio" >
                       
                    </select>
                </div>
            </div>
            <br />
            
            <div class="row">
                <div class="col-md-4">
                    <label>Módulo</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlModulos" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)" title="Módulos del edificio seleccionado">
                       
                    </select>
                </div>
            </div>
            <br />
              

            <div class="row">
                <div class="col-md-4">
                    <label>Estado</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlEstado" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)" >
                        <option value="0">::Seleccionar::</option>
                        <option value="1">Habilitado</option>
                        <option value="2">No Habilitado</option>
                    </select>
                </div>
            </div>  
            <br />
        </div>
        
        
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
