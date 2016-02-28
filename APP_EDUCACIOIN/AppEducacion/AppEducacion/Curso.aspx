<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Curso.aspx.cs" Inherits="AppEducacion.Curso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/curso.css" rel="stylesheet" />
    <script src="js/curso.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!--***********************Contenedor principal*********************-->
    <div class="container" style="width:90%">
        <!--titulo de la pagina-->
        <h4 style="text-align:center;color:#08088A" class="container">ADMINISTRACIÓN DE CURSOS</h4>
        <br />

        <!--********************Contenedor de la vista principal del formulario**************************-->
        <div class="panel panel-success" style="width:100%">
            <!--**********filtros de la data**********-->
            <div class="panel-heading" style="width:100%">
                <div class="row">
                    <div class="col-md-9">
                        <label>Busqueda</label>&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                        <input type="text" title="Texto a buscar" id="txtBusqueda" placeholder="Ingresar texto a buscar" maxlength="100" style="width:400px" onkeypress="convertir(this,'may')" />
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
                        
                        <input type="button" class="btn btn-primary btn-xs" id="btnBuscar" value="Buscar"  title="Efectuar busqueda"/> 
                        <input type="button" class="btn btn-primary btn-xs" id="btnRefrescar" value="Refrescar"  title="Mostrar toda la información"/>
                        <input type="button" class="btn btn-danger btn-xs" id="btnNuevo" value="Nuevo" title="Agregar nuevo registro"/>
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

            <!--**********--paginacion**********-->
           <div id="paginacion" class="panel-footer">               
            </div>

        </div>
        <!--*********fin de la vista principal*********-->
        
        <!--#####################################################################################################-->
        <!--*********formulario para el registro de datos*********-->
        <div id="formulario" title="Informaci&oacute;n del Curso" class="modal">   
               
            <div class="row">  
                <input type="text" id="pk"/>              
                <div class="col-md-4">
                    <label>Código</label>
                </div>
                <div class="col-md-4">
                    <input id="txtCodigoMineduc" title="Código del curso" type="text" placeholder="Ingrese código" maxlength="25" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,25)"  onkeypress="Convertir(this,'may')" />
                </div>
            </div>
            <br />

            <div class="row">
                <div class="col-md-4">
                    <label>Créditos</label>
                </div>
                <div class="col-md-4">
                    <input id="txtCreditos" title="Créditos del curso" type="text" placeholder="Ingrese código" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this)"/>
                </div>
            </div>
            <br />
                  
            <div class="row">                
                <div class="col-md-4">
                    <label>Nombre</label>
                </div>
                <div class="col-md-4">
                    <input id="txtNombre" title="Nombre del curso" type="text" placeholder="Ingrese nombre" maxlength="25" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this); revisarObligatorio(this); revisarLongitud(this,25)" onkeypress="convertir(this,'may')" />
                </div>
            </div>
            <br />
            
            <div class="row">
                <div class="col-md-4">
                    <label>Descripci&oacute;n</label>
                </div>
                <div class="col-md-4">
                    <textarea id="txtDescripcion" title="Descripción del curso" rows="4" placeholder="Ingrese descripci&oacute;n" maxlength="50" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this);revisarLongitud(this,50)"></textarea>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                   <label>Cat. Curso</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlCategoriasCurso" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)">
                        
                    </select>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label>Cat. Nivel</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlCategoriasNivel" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)">

                    </select>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label>Nivel</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlNiveles" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)">

                    </select>
                </div>
            </div>
            <br />

            <div class="row">
                <div class="col-md-4">
                    <label>Estado</label>
                </div>
                <div class="col-md-4">
                    <select id="ddlEstado" onfocus="entroEnFoco(this)" onblur="salioDeFoco(this)">
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
        <div id="dialog-guardar" title="Registrando">
            <p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 20px 0;"></span>La informaci&oacute;n se ha registrado correctamente</p>
        </div> 
          

         <!--dialogo para actualizar-->
        <div id="dialog-editar" title="Editando">
            <p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 20px 0;"></span>La informaci&oacute;n se ha editado correctamente</p>
        </div> 

        <!--dialogo para actualizar-->
        <div id="dialog-msj" title="Error">
            <p><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 20px 0;"></span>Por favor, verique que ha sucedido un error</p>
        </div> 
    </div>
    <!--**********fin del contenedor**********--> 
</asp:Content>
