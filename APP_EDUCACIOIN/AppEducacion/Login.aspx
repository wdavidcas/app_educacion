<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AppEducacion.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/login.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="container" id="divRegistro">
        <section class="row">
            <section class="col-md-2">
                <label>Usuario</label>
            </section>    
            <section class="col-md-10">
                <input type="text" />
            </section>
            <br /><br />
        </section>
        <section class="row">
            <section class="col-md-2">
                <label>Clave</label>
            </section>    
            <section class="col-md-10">
                <input type="password" />
            </section>
            <br /><br />
         </section>
         <section class="row">
            <section class="col-md-2">
                <label>Confirmar clave</label>
            </section>    
         
            <section class="col-md-10">
                <input type="password" />
            </section>
            <br /><br />
        </section>
        <section class="row">
            <section class="col-md-4">

            </section>
            <section class="col-md-4">
                <input type="button" class="btn-primary" id="btnRegistrar" value="Registrar"/>
            </section>
            <section class="col-md-4">
                
            </section>                        
        </section>
        <br /><br />
   </section>               
</asp:Content>
