<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contactos.aspx.cs" Inherits="AppRotulacionPruebas.Contactos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/contactos.js"></script>
    <link href="css/contactos.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container">
        <!--<input type="button" id="btnObtContactos" value="Obtener Contactos" /> <br/><br/> -->
        <h3 style="text-align:center" class="container">Administraci&oacute;n de contactos</h3>

        <div class="panel panel-info">
            <div class="panel-heading">
                <label>Busqueda</label>
                <input type="text" />
                <label>Tipo</label>
                <select>
                    <option value="0">::Seleccione Tipo</option>
                    <option value="1">Tipo B</option>
                    <option value="2">Tipo C</option>
                </select>
                <label>Estado</label>
                <select>
                    <option value="0">::Seleccione estado::</option>
                    <option value="1">Habilitado</option>
                    <option value="2">No Habilitado</option>                    
                </select>
                <!--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-->
                <button id="btnBuscar" class="btn btn-warning">Buscar</button>
                <button id="Button1" class="btn btn-warning">Refrescar</button>
                

                <button class="btn btn-success" id="btnNuevo">Nuevo</button>
                    
            </div>

            <div id="data" class="panel-body">

            </div>

           <div id="paginacion" class="panel-footer">

            </div>

        </div>    
    </div>
</asp:Content>
