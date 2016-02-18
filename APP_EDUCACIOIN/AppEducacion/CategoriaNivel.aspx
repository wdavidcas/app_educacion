<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoriaNivel.aspx.cs" Inherits="AppEducacion.CategoriaNivel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script src="js/contactos.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <section class="row" id="titulo" style="text-align:center">
            <h3>Cat&aacute;logo de Categor&iacute;as Nivel</h3>
        </section>
        <section class="row" id="busqueda">
            <label>B&uacute;squeda &nbsp;&nbsp;&nbsp;</label><input type="text" id="txtBuscar" placeholder="Ingrese el nombre a buscar"/>&nbsp;&nbsp;&nbsp;<label>Estado</label>
            <select>
                <option value="0">::Seleccione estado::</option>
                <option value="1">Habilitado</option>
                <option value="2">No habilitado</option>
            </select>
            &nbsp;&nbsp;&nbsp;
            <button id="btnBuscar" class="btn btn-primary">Buscar</button>
            &nbsp;&nbsp;&nbsp;
            <button id="btnNuevo" class="btn btn-primary">Nuevo</button>
        </section>
        <br />
        <section id="listado">

        </section>
        <!--
        <section class="row" id="grid">
            <table id="" style="width:100%;border:1">
                <thead>
                    <th style="width:40%">Nombre</th>
                    <th style="width:50%">Descripci&oacute;n</th>
                    <th style="width:10%">Estado</th>
                </thead>
                <tbody>
                    <tr>
                        <td>Basico</td>
                        <td>Basico</td>
                        <td>Habilitado</td>
                    </tr>
                    <tr>                        
                        <td>Primaria</td>
                        <td>Primaria</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>                        
                        <td>PrePrimaria</td>
                        <td>PrePrimaria</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>                        
                        <td>Diversificado</td>
                        <td>Diversificado</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>
                        <td>Basico</td>
                        <td>Basico</td>
                        <td>Habilitado</td>
                    </tr>
                    <tr>                        
                        <td>Primaria</td>
                        <td>Primaria</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>                        
                        <td>PrePrimaria</td>
                        <td>PrePrimaria</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>                        
                        <td>Diversificado</td>
                        <td>Diversificado</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>
                        <td>Basico</td>
                        <td>Basico</td>
                        <td>Habilitado</td>
                    </tr>
                    <tr>                        
                        <td>Primaria</td>
                        <td>Primaria</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>                        
                        <td>PrePrimaria</td>
                        <td>PrePrimaria</td>
                        <td>Habilitado</td>                    
                    </tr>
                    <tr>                        
                        <td>Diversificado</td>
                        <td>Diversificado</td>
                        <td>Habilitado</td>                    
                    </tr>
                </tbody>                
            </table>
        </section>-->
        <section class="row" id="acciones">            
        </section>
        <br /><br />
        <!--formulario para agregar o editar-->
        <div id="frmCategoria" title="Categor&iacute;a" class="container">

        </div>
        
    </div>

    
</asp:Content>
