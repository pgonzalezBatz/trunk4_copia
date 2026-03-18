<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ListaFundicionMarcas.aspx.vb" Inherits="Web.ListaFundicionMarcas" 
    MasterPageFile="~/Site1.Master" %>

<%@ Import Namespace="System.Globalization" %>        
<%@ Import Namespace="Web"%>

<asp:Content ContentPlaceHolderID="cphContenido" runat="server">
    <div id="form1">
        <asp:ValidationSummary runat="server" ValidationGroup="fundicion" />
        <div id="divcl1">
            <div>
                <label class="formLabel"><%=h.traducir("Marca")%></label>
                <asp:TextBox ID="txtMarca" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="LocalizedRequiredFieldValidator1" ControlToValidate="txtMarca" runat="server"
                                     text="PorFavorIntroduzcaTexto" ValidationGroup="fundicion" Display="Dynamic">
                </asp:RequiredFieldValidator>      
                <asp:CustomValidator ID="cvMarca" runat="server" EnableClientScript="False" ControlToValidate="txtMarca" 
                            Display="Dynamic" ValidationGroup="fundicion" text="laMarcaIntroducidaEsIncorrectaOYaExiste" 
                            ErrorMessage="La marca no es valida o ya existe">
                </asp:CustomValidator>          
            </div>
            <div>
                <label class="formLabel"><%=h.traducir("Cantidad")%></label>
                <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="LocalizedRequiredFieldValidator3" ControlToValidate="txtCantidad" runat="server"
                                     text="PorFavorIntroduzcaTexto" ValidationGroup="fundicion" Display="Dynamic">
                </asp:RequiredFieldValidator>            
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationGroup="fundicion"
                                    ValidationExpression="(\d)+(,(\d)+)?$" ControlToValidate="txtCantidad" text="numeroIncorrecto">
                </asp:RegularExpressionValidator>
            </div>
            <div>
                <label class="formLabel"><%=h.traducir("Denominacion")%></label>
                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            </div>
            <div>
                <label class="formLabel"><%=h.traducir("Conjunto")%></label>
                <asp:TextBox ID="txtConjunto" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="divcl2">
            <div>
                <label class="formLabel"><%=h.traducir("Materiala")%></label>
                <asp:DropDownList ID="ddlMateriala" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="LocalizedRequiredFieldValidator2" ControlToValidate="ddlMateriala" runat="server" InitialValue="0"
                                     text="porFavorSeleccioneUnElementoDeLaLista" ValidationGroup="fundicion" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>
            <div>
                <label class="formLabel"><%=h.traducir("Tratamiento Temple")%></label>
                <asp:DropDownList ID="ddlTTemple" runat="server" AutoPostBack="true"></asp:DropDownList>
                
            </div>
            <div id="divDureza" runat="server">
                <label class="formLabel">
                    <%=h.traducir("Dureza")%>
                    </label>
                <asp:DropDownList ID="ddlDureza" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="LocalizedRequiredFieldValidator4" ControlToValidate="ddlDureza" runat="server" InitialValue="0"
                                     text="porFavorSeleccioneUnElementoDeLaLista" ValidationGroup="fundicion" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>
            <div id="divTratSec" runat="server">
                <label class="formLabel"><%=h.traducir("Tratamiento secundario")%></label>
                <asp:DropDownList ID="ddlTSecundario" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div id="divRealiza" runat="server">
                    <label class="formLabel" id="lblRealiza" runat="server">
                    <%=h.traducir("Realiza")%>
                    </label>
                <asp:DropDownList ID="ddlRealiza" runat="server">
                    <asp:ListItem Value="B" Text="Batz"></asp:ListItem>
                    <asp:ListItem Value="C" Text="Cliente"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:HiddenField ID="hpeso" runat="server" />
            <asp:HiddenField ID="hnorma" runat="server" />
        </div>
    </div>
    <div style="text-align:center;clear:both; margin-right:5%;">
        <a href='ListaFundicion.aspx?<%="ord=" + request("ord") + "&op=" + request("op") %>' style="padding-right:1em;" title="Volver a la lista">
            <%=h.traducir("Cancelar")%>
        </a>
        <asp:Button ID="btnSave" runat="server" OnClick="Save" text="Guardar" ValidationGroup="fundicion" />
    </div>
</asp:Content>