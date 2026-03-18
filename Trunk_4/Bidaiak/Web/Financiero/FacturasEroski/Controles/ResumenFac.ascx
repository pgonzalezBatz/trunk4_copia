<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ResumenFac.ascx.vb" Inherits="WebRaiz.ResumenFac" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script src="../../js/jQuery/responsables.js" type="text/javascript"></script>
<script type="text/javascript">
    //Autorrellena el resto de nombres de personas con el mismo nombre
    //indexFirst es el indice de la primera aparicion en la que aparece ese nombre. Se le ha sumando + 1 en la llamada porque parece que en el javascript, empieza por 1
    function Autorrellenar(nombrePersona, indexFirst) {
        var grid = document.getElementById('<%=gvFacturas.ClientID %>');
        var txtNombre, idPerso, lblNombre;
        for (var index = indexFirst; index < grid.rows.length; index++) {
            if (index == indexFirst) {
                if (grid.rows[index].cells[5].children[0].children[0].nodeName == 'INPUT')  //Cuadro de texto
                {
                    //Si es el primero (la persona a buscar), se recoge el nombre escrito y el id de la persona
                    txtNombre = grid.rows[index].cells[5].children[0].children[0].value;
                    idPerso = grid.rows[index].cells[5].children[0].children[1].value;
                }
                else  //Desplegable
                {
                    txtNombre = grid.rows[index].cells[5].children[0].children[0].value;
                    idPerso = grid.rows[index].cells[5].children[0].children[0].value;
                }
            }
            else {
                //Para las siguientes personas, se busca si en el label, esta el mismo que en el nombre en curso, y si coincide, se le asigna el nombre del textbox y el id en el hidden
                lblNombre = grid.rows[index].cells[3].firstChild.innerHTML;
                if (lblNombre == nombrePersona) {                    
                    if (grid.rows[index].cells[5].children[0].children[0].nodeName == 'INPUT')  //Cuadro de texto
                    {
                        grid.rows[index].cells[5].children[0].children[0].value = txtNombre;
                        grid.rows[index].cells[5].children[0].children[1].value = idPerso;  //Aqui es el children 1 porque no tiene la imagen para autorrellenar
                    }
                    else  //SELECT
                    {
                        grid.rows[index].cells[5].children[0].children[0].value = idPerso;
                    }                                            
                }

            }
        }
    }

    //Al pulsar el link, se añade el organizador al cuadro de texto
    function AutorrellenarLink(idSab, nombreOrganizador, index) {
        var grid = document.getElementById('<%=gvFacturas.ClientID %>');
        var txtNombre, idPerso, lblNombre;
        var longitud = grid.rows[index].cells[5].children[0].children.length;
        if (longitud == 1) {   //Desplegable
            var dropdown = grid.rows[index].cells[5].children[0].children[0];
            var encontrado = 0;
            for (var i = 0; i < dropdown.options.length; i++) {
                if (dropdown.options[i].value == idSab) {
                    dropdown.options[i].selected = true;
                    encontrado = 1;
                    break;
                }
            }
            if (encontrado == 0) { //Si no se encuentra, se selecciona el primero --seleccione uno--
                dropdown.value = 0;
            }
        }
        else {
            grid.rows[index].cells[5].children[0].children[0].value = nombreOrganizador;
            grid.rows[index].cells[5].children[0].children[1].value = idSab;
        }        
    }
</script>
<fieldset style="width:700px">
    <table>
        <tr>
            <td><asp:Label runat="server" ID="labelNReg" Text="Nº de movimientos leidos"></asp:Label></td>
            <td><asp:Label runat="server" ID="lblNumRegProc" CssClass="negrita"></asp:Label></td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="labelRegAsoc" Text="Procesados"></asp:Label></td>
            <td><asp:Label runat="server" ID="lblNumProcOK" CssClass="negrita"></asp:Label></td>
        </tr>
         <tr>
            <td><asp:Label runat="server" ID="labelAsociadosViaje" Text="Asociados a viajes"></asp:Label></td>
            <td><asp:Label runat="server" ID="lblAsociadosViaje" CssClass="negrita"></asp:Label></td>
        </tr>
         <tr>
            <td><asp:Label runat="server" ID="labelRegSinAsoc" Text="Sin procesar"></asp:Label></td>
            <td><asp:Label runat="server" ID="lblnumSinProc" CssClass="negrita"></asp:Label></td>
        </tr>
    </table>
</fieldset>
<asp:Panel runat="server" ID="pnlPendientes" GroupingText="Pendientes de procesar">
    <br />
    <asp:GridView runat="server" ID="gvFacturas" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="GridViewB" Width="900px">
		<HeaderStyle CssClass="GridViewBHeaderStyle" />
		<AlternatingRowStyle CssClass="GridViewBAlternatingRowStyle" />
		<RowStyle CssClass="GridViewBRowStyle" />			       
		<FooterStyle CssClass="GridViewBFooterStyle" />
		<Columns>	
            <asp:TemplateField Visible="false">
                <ItemTemplate><asp:Label runat="server" ID="lblBono"></asp:Label></ItemTemplate>
            </asp:TemplateField>                     
			 <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="factura"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblFactura"></asp:Label></ItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="albaran"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblAlbaran"></asp:Label></ItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Fecha servicio"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblFechaServ"></asp:Label></ItemTemplate>
            </asp:TemplateField>   
             <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Persona"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Nivel 1"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblNivel1"></asp:Label></ItemTemplate>
            </asp:TemplateField>
			<asp:TemplateField>
				<HeaderTemplate><asp:Label runat="server" Text="Cargar coste a"></asp:Label></HeaderTemplate>
				<ItemTemplate>
					<asp:Panel runat="server" ID="pnlSelUsuario">
                        <asp:TextBox runat="server" ID="txtNombreUsuario" Columns="35" CssClass="campoObligatorio" AutoComplete="Off"></asp:TextBox>&nbsp;
                        <asp:HiddenField ID="hfIdresponsable" runat="server" />
                        <asp:ImageButton runat="server" ID="imgAutoRellenar" ToolTip="Cuando rellene el nombre, pulse aqui para rellenar con el nombre introducido las personas repetidas" ImageUrl="~/App_Themes/Tema1/Images/autoFill.png" ImageAlign="AbsMiddle" />
                        <br />
                        <div runat="server" id="helper" style="position:absolute;z-index:9;"></div>                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlSelIntegrante">
                        <asp:DropDownList runat="server" ID="ddlIntegrantes" AppendDataBoundItems="true"></asp:DropDownList>
                        <asp:HiddenField runat="server" ID="hfIdViaje" />
                    </asp:Panel>
				</ItemTemplate>
			</asp:TemplateField>	
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Organizador viaje"></asp:Label></HeaderTemplate>
                <ItemTemplate>
                    <asp:Panel runat="server" ID="pnlLinkOrg">
                        <asp:LinkButton runat="server" ID="lnkOrganiz"></asp:LinkButton>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlLabelOrg">
                        <asp:Label runat="server" ID="lblOrganiz"></asp:Label>
                    </asp:Panel>                    
                </ItemTemplate>
            </asp:TemplateField>													
		</Columns>
	</asp:GridView>
    <br />
    <div id="botones"><asp:ImageButton runat="server" ID="imgGuardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosBotones/Guardar.png" /></div>  
</asp:Panel><br />
<asp:Panel runat="server" ID="pnlTodosOk" GroupingText="Procesados con exito">
     <asp:Image runat="server" id="imgResul" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/ok_big.gif" />&nbsp;&nbsp;
     <asp:Label runat="server" ID="labelMensa" CssClass="labelDetalle" Text="Todos los registros se han procesado con exito"></asp:Label><br /><br />
     <asp:Button runat="server" ID="btnContinuar" Text="Continuar" />
</asp:Panel>