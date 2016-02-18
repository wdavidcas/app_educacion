<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoriaNivel.aspx.cs" Inherits="AppEducacion.CategoriaNivel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/categoriaNivel.js"></script>
    <link href="css/categoriaNivel.css" rel="stylesheet" />    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--***********************Contenedor principal*********************-->
    <div class="container" style="width:90%">
        <!--titulo de la pagina-->
        <h3 id="titulo">ADMINISTRACIÓN DE CATEGORÍAS DE NIVEL</h3>
        <br />

        <!--********************Contenedor de la vista principal del formulario**************************-->
        <div class="panel panel-success" style="width:100%" id="panel">
            <!--**********filtros de la data**********-->
            <div class="panel-heading" style="width:100%">
                <div class="row">
                    <div class="col-md-9">
                        <label>Busqueda</label>&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                        <input type="text" id="txtBusqueda" title="Ingrese texto a buscar" placeholder="Ingresar texto a buscar" maxlength="100" style="width:400px"  onkeypress="convertir(this,'may')"/>
                        <label id="lblTipo">Tipo</label>
                <select id="ddlTiposBusquedas" title="Tipo de búsqueda">
                    <option value="0">::Seleccione Tipo::</option>
                    <option value="1">Nombre</option>
                    <option value="2">Descripci&oacute;n</option>
                </select>

                <label>Estado</label>
                <select id="ddlEstados" title="Tipo de búsqueda">
                    <option value="0">::Seleccione estado::</option>
                    <option value="1">Habilitado</option>
                    <option value="2">No Habilitado</option>                    
                </select>
                    </div>
                        <div class="col-md-3">                       
                        
                        <input type="button" class="btn btn-primary btn-xs" id="btnBuscar" value="Buscar" title="Efectuar búsqueda"/> 
                        <input type="button" class="btn btn-primary btn-xs" id="btnRefrescar" value="Refrescar" title="Refrescar información"/>
                        <input type="button" class="btn btn-danger btn-xs" id="btnNuevo" value="Nuevo" title="Nueva categoría"/>
                        <input type="button" class="btn btn-success btn-xs" id="btnExportar" value="Exportar" title="Exportar datos" />
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
        
        <!--#####################################################################################################-->
        <!--*********formulario para el registro de datos*********-->
        <div id="formulario" title="Informaci&oacute;n de la categoría" class="modal">            
            <div class="row">
                <input type="text" id="pk"/>
                <div class="col-md-4">
                    <label>Nombre</label>
                </div>
                <div class="col-md-4">
                    <input id="txtNombre" title="Nombre de la categoría de nivel" type="text" placeholder="Ingrese nombre" maxlength="15" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,15);" onkeypress="convertir(this,'may')" />
                </div>
            </div>
            <br />
            
            <div class="row">
                <div class="col-md-4">
                    <label>Descripci&oacute;n</label>
                </div>
                <div class="col-md-4">
                    <textarea id="txtDescripcion" title="Descripción de la categoría de nivel" rows="4" placeholder="Ingrese descripci&oacute;n" maxlength="50" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this);revisarLongitud(this,50)" onkeypress="convertir(this,'may')"></textarea>
                </div>
            </div>
            <br />

            <div class="row">
                <div class="col-md-4">
                    <label>Estado</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlEstado" title="Estado" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)">
                        <option value="0">::Seleccionar:</option>
                        <option value="1">Habilitado</option>
                        <option value="2">No Habilitado</option>
                    </select>
                </div>
            </div>  
            <br />
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
