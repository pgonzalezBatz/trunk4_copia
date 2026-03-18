<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DuplicadoLista.aspx.vb" Inherits="Web.DuplicadoLista" 
    MasterPageFile="~/Site1.Master" EnableViewState="false" %>

<%@ Import Namespace="System.Globalization" %>        
<%@ Import Namespace="Web"%>

<asp:Content ContentPlaceHolderID="cphContenido" runat="server">
    <asp:ValidationSummary runat="server" ValidationGroup="duplicar" />
    <div class="alingWithImages">
        <%=h.traducir("Origen")%>
        <asp:DropDownList ID="ddlOfSource" runat="server" AutoPostBack="true" CssClass="fontGrande"></asp:DropDownList>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOfSource" Display="Dynamic" ValidationGroup="duplicar" Text="porFavorSeleccioneUnElementoDeLaLista" InitialValue="0"></asp:RequiredFieldValidator>
        <asp:DropDownList ID="ddlOpSource" runat="server" CssClass="fontGrande" ></asp:DropDownList>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOpSource" Display="Dynamic" ValidationGroup="duplicar" Text="porFavorSeleccioneUnElementoDeLaLista" InitialValue="0"></asp:RequiredFieldValidator>
        <img src="App_Themes/Theme1/Images/arrowRight.png" alt="Dirección de copia de ofs" />
        <%=h.traducir("Destino")%>
        <asp:DropDownList ID="ddlOfDestination" runat="server" AutoPostBack="true" CssClass="fontGrande"></asp:DropDownList>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOfDestination" Display="Dynamic" ValidationGroup="duplicar" Text="porFavorSeleccioneUnElementoDeLaLista" InitialValue="0"></asp:RequiredFieldValidator>
        <asp:DropDownList ID="ddlOpDestination" runat="server" CssClass="fontGrande"></asp:DropDownList>    
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOpDestination" Display="Dynamic" ValidationGroup="duplicar" Text="porFavorSeleccioneUnElementoDeLaLista" InitialValue="0"></asp:RequiredFieldValidator>
         <asp:CustomValidator ID="cvDuplicar" ValidationGroup="duplicar" runat="server" Text="La lista origen y destino no pueden contener las mismas marcas"></asp:CustomValidator>
        <br />
        <asp:button runat="server" OnClick="Duplicar" ValidationGroup="duplicar" Text="Duplicar" />
    </div>

</asp:Content>