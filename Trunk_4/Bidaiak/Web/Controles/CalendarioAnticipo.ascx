<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CalendarioAnticipo.ascx.vb" Inherits="WebRaiz.CalendarioAnticipo" %>   
<div class="input-group date" id="dtFecha">
    <asp:TextBox runat="server" ID="txtFecha" style="background-color:#EEE" contenteditable="false" CssClass="form-control required"></asp:TextBox>    
    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
</div>