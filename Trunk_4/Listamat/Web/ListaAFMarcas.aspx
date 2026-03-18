<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ListaAFMarcas.aspx.vb" Inherits="Web.ListaAFMarcas" 
    MasterPageFile="~/Site1.Master"%>

<%@ Import Namespace="System.Globalization" %>        
<%@ Import Namespace="Web"%>

<asp:Content ContentPlaceHolderID="cphContenido" runat="server">
    <div id="form1">
        <asp:ValidationSummary runat="server" ValidationGroup="lista" />
        <div id="divcl1">
            <div>
                <label class="formLabel"><%=h.traducir("Marca")%></label>
                <asp:TextBox ID="txtMarca" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="txtMarca" runat="server"
                                 Text="PorFavorIntroduzcaTexto" ValidationGroup="lista" Display="Dynamic">
                </asp:RequiredFieldValidator>  
                <asp:CustomValidator ID="cvMarca" runat="server" EnableClientScript="False" ControlToValidate="txtMarca" 
                            Display="Dynamic" ValidationGroup="lista" Text="laMarcaIntroducidaEsIncorrectaOYaExiste">
                </asp:CustomValidator>    
            </div>
            <div>
                <label class="formLabel"><%=h.traducir("Cantidad")%></label>
                <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="txtCantidad" runat="server"
                                 Text="PorFavorIntroduzcaTexto" ValidationGroup="lista" Display="Dynamic">
                </asp:RequiredFieldValidator>            
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationGroup="lista"
                                    ValidationExpression="^(\d)+(,(\d)+)?$" ControlToValidate="txtCantidad" Text="numeroIncorrecto">
                </asp:RegularExpressionValidator>
            </div>
            <div id="divDiam" runat="server">
                <label class="formLabel">
                    <%=h.traducir("Diametro")%>
                </label>
                <asp:TextBox ID="txtDiametro" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDiametro" ControlToValidate="txtDiametro" runat="server"
                                 Text="PorFavorIntroduzcaTexto" ValidationGroup="lista" Display="Dynamic">
                </asp:RequiredFieldValidator>            
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationGroup="lista"
                                    ValidationExpression="^(\d)+(,(\d)+)?$" ControlToValidate="txtDiametro" Text="numeroIncorrecto">
                </asp:RegularExpressionValidator>
            </div>
            <div id="divLargo" runat="server">
                <label class="formLabel">
                    <%=h.traducir("Largo")%>
                </label>
                <asp:TextBox ID="txtLargo" runat="server" ></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationGroup="lista"
                                    ValidationExpression="^(\d)+(,(\d)+)?$" ControlToValidate="txtLargo" Text="numeroIncorrecto">
                </asp:RegularExpressionValidator>
                <asp:CustomValidator ID="cvLargo" runat="server" ControlToValidate="txtLargo" EnableClientScript="false"
                                             ValidationGroup="lista"  Display="Dynamic"
                                             Text="El diametro del cuerpo o caña es un dato obligatorio para este elemento">
                </asp:CustomValidator>
            </div>
            <div id="divAncho" runat="server">
                <label class="formLabel">
                    <%=h.traducir("Ancho")%>
                </label>
                <asp:TextBox ID="txtAncho" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationGroup="lista"
                                    ValidationExpression="^(\d)+(,(\d)+)?$" ControlToValidate="txtAncho" Text="numeroIncorrecto">
                </asp:RegularExpressionValidator>
            </div>
            <div id="divGrueso" runat="server">
                <label class="formLabel">
                    <%=h.traducir("Grueso")%>
                </label>
                <asp:TextBox ID="txtGrueso" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvGrueso" runat="server" ControlToValidate="txtGrueso"
                                ValidationGroup="lista" Display="Dynamic" Text="PorFavorIntroduzcaTexto" >
                </asp:RequiredFieldValidator>
            </div>
             <div>
                <label class="formLabel"><%=h.traducir("Conjunto")%></label>
                <asp:TextBox ID="txtConjunto" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="divcl2">
            <div>
                <label class="formLabel"><%=h.traducir("Denominación")%></label>
                <asp:DropDownList ID="ddlMateriala" runat="server" AutoPostBack="true"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMateriala" ValidationGroup="lista" 
                                                InitialValue="0" Display="Dynamic" Text="porFavorSeleccioneUnElementoDeLaLista">
                </asp:RequiredFieldValidator>
            </div>
            <div id="divDenominacion2" runat="server">
                <label class="formLabel"><%=h.traducir("Denominación2")%></label>
                <asp:DropDownList ID="ddlOtmardes" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div id="divDenominacion3" runat="server" visible="false">
                <label class="formLabel"><%=h.traducir("Denominación3")%></label>
                <asp:TextBox ID="txtAñadido" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="ddlMateriala" runat="server" InitialValue="0"
                                 Text="porFavorSeleccioneUnElementoDeLaLista" ValidationGroup="lista" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>
            <div id="divmat1" runat="server">
                <label class="formLabel"><%=h.traducir("Material")%></label>
                <asp:DropDownList ID="ddlOtmatespe" runat="server" AutoPostBack="true"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOtmatespe" ValidationGroup="lista" 
                                                InitialValue="0" Display="Dynamic" Text="porFavorSeleccioneUnElementoDeLaLista">
                </asp:RequiredFieldValidator>
            </div>
            <div id="divmat2" runat="server">
                <label class="formLabel"><%=h.traducir("Tratamiento")%></label>
                <asp:DropDownList ID="ddlTratamiento" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div id="divmat3" runat="server">
                <label class="formLabel"><%=h.traducir("Dureza")%></label>
                <asp:DropDownList ID="ddlDureza" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="ddlDureza" runat="server" InitialValue="0"
                                     Text="porFavorSeleccioneUnElementoDeLaLista" ValidationGroup="lista" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>
            <div id="divmat4" runat="server">
                <label class="formLabel"><%=h.traducir("Tratamiento secundario")%></label>
                <asp:DropDownList ID="ddlTraSec" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div id="divmat5" runat="server">
                <label class="formLabel"><%=h.traducir("Realiza")%></label>
                <asp:DropDownList ID="ddlRealiza" runat="server">
                    <asp:ListItem Value="B" Text="Batz"></asp:ListItem>
                    <asp:ListItem Value="C" Text="Cliente"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div id="divNoMat1" runat="server">
                <label class="formLabel"><%=h.traducir("Código")%></label>    
                <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCodigo" Display="Dynamic" 
                                    Text="PorFavorIntroduzcaTexto" ValidationGroup="lista"></asp:RequiredFieldValidator>
                <asp:DropDownList ID="ddlOtmatespeCodigo" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div id="divNoMat2" runat="server">
                <label class="formLabel"><%=h.traducir("Proveedor")%></label>    
                <asp:TextBox ID="txtProveedor" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtProveedor" Display="Dynamic" 
                                    Text="PorFavorIntroduzcaTexto" ValidationGroup="lista"></asp:RequiredFieldValidator>
            </div>  
        </div>
        <asp:HiddenField ID="hpeso" runat="server" />
        <asp:HiddenField ID="hnorma" runat="server" />
        <div style="text-align:center;clear:both;margin-right:5%;"> 
            <a href='ListaAF.aspx?<%="tipolista="+request("tipolista")+ "&ord=" + request("ord") + "&op=" + request("op") %>' style="padding-right:1em;" title="Volver a la lista">
                <%=h.traducir("Cancelar")%>
            </a>
            <asp:Button ID="btnSave" runat="server" OnClick="Save" Text="Guardar" ValidationGroup="lista" />
        </div>
    </div>
</asp:Content>