<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AppEducacion.login.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--librerias externas-->    
    <script src="../frameworks/jquery/jquery-2.1.4.min.js"></script>
    <script src="../frameworks/jqueryui/external/jquery/jquery.js"></script>
     <link href="../frameworks/jqueryui/jquery-ui.css" rel="stylesheet" />
    <script src="../frameworks/jqueryui/jquery-ui.js"></script>        
    <link href="../frameworks/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <script src="../frameworks/bootstrap/js/bootstrap.js"></script>    
    <link href="../frameworks/jquery/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../frameworks/jquery/jquery.dataTables.min.js"></script>

    <script src="login.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" class="container">
    
    
    <section class="container" id="Registrarse">
        <section class="row">
            <section class="col-md-4">
                <label>Usuario</label>
            </section>    
            <section class="col-md-8">
                <input type="text" />
            </section>
            <br />
    
            <section class="col-md-4">
                <label>Clave</label>
            </section>    
            <section class="col-md-8">
            <input type="password" />
            </section>
            <br />

            <section class="col-md-4">
                <label>Confirmar clave</label>
            </section>    
            
            <section class="col-md-8">
                <input type="password" />
            </section>
        </section>
   </section>               
</asp:Content>
