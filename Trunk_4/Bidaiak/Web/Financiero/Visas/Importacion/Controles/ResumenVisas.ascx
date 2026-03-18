<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ResumenVisas.ascx.vb" Inherits="WebRaiz.ResumenVisas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script src="../../../js/jQuery/responsables.js" type="text/javascript"></script>
<script type="text/javascript">
    //Autorrellena el resto de nombres de personas con el mismo nombre
    //indexFirst es el indice de la primera aparicion en la que aparece ese nombre. Se le ha sumando + 1 en la llamada porque parece que en el javascript, empieza por 1
    function Autorrellenar(nombrePersona, indexFirst) {
        var grid = document.getElementById('<%=gvVisas.ClientID %>');
        var txtNombre, idPerso, lblNombre;
        for (var index = indexFirst; index < grid.rows.length; index++) {
            if (index == indexFirst) {
                //Si es el primero (la persona a buscar), se recoge el nombre escrito y el id de la persona
                txtNombre = grid.rows[index].cells[2].children[0].value;
                idPerso = grid.rows[index].cells[2].children[1].value;
            }
            else {
                //Para las siguientes personas, se busca si en el label, esta el mismo que en el nombre en curso, y si coincide, se le asigan el nombre del textbox y el id en el hidden
                lblNombre = grid.rows[index].cells[1].firstChild.innerHTML;  //grid.rows[index].cells[1].children[0].childNodes[0].value;
                if (lblNombre == nombrePersona) {
                    grid.rows[index].cells[2].children[0].value = txtNombre;
                    grid.rows[index].cells[2].children[1].value = idPerso;  //Aqui es el children 1 porque no tiene la imagen para autorrellenar
                }

            }
        }
    }

    //Marca para que la visa se guarde como excepcion
    //numTarjeta se guardara en el hidden, texto en el cuadro de texto e indice de la fila del grid
    function Exception(numTarjeta, texto,index) {
        var grid = document.getElementById('<%=gvVisas.ClientID %>');
        var txtNombre, idPerso, lblNombre;
        grid.rows[index].cells[2].children[0].value=texto;
        grid.rows[index].cells[2].children[1].value = "E|" + numTarjeta;        
    }   
</script>
<fieldset style="width:700px">
    <table>
        <tr>
            <td><asp:Label runat="server" ID="labelNReg" Text="Nº de registros leidos"></asp:Label></td>
            <td><asp:Label runat="server" ID="lblNumRegProc" CssClass="negrita"></asp:Label></td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="labelRegAsoc" Text="Registros asociados a viajes"></asp:Label></td>
            <td><asp:Label runat="server" ID="lblRegViajes" CssClass="negrita"></asp:Label></td>
        </tr>
         <tr>
            <td><asp:Label runat="server" ID="labelRegSinAsoc" Text="Registros sin asociar"></asp:Label></td>
            <td><asp:Label runat="server" ID="lblRegSinViaje" CssClass="negrita"></asp:Label></td>
        </tr>
    </table>
</fieldset>
<asp:Panel runat="server" ID="pnlPendientes" GroupingText="Pendientes de procesar">
    <br />
    <asp:GridView runat="server" ID="gvVisas" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" CssClass="GridViewB" Width="900px">
		<HeaderStyle CssClass="GridViewBHeaderStyle" />
		<AlternatingRowStyle CssClass="GridViewBAlternatingRowStyle" />
		<RowStyle CssClass="GridViewBRowStyle" />			       
		<FooterStyle CssClass="GridViewBFooterStyle" />
		<Columns>	
           <asp:TemplateField Visible="false">
                <ItemTemplate><asp:Label runat="server" ID="lblId"></asp:Label></ItemTemplate>
            </asp:TemplateField>                     
            <asp:TemplateField Visible="false">
                <ItemTemplate><asp:Label runat="server" ID="lblFecha"></asp:Label></ItemTemplate>
            </asp:TemplateField>  
			 <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="Num Tarjeta"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblTarjeta"></asp:Label></ItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField>
                <HeaderTemplate><asp:Label runat="server" Text="persona"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:Label runat="server" ID="lblPersona"></asp:Label></ItemTemplate>
            </asp:TemplateField>             
			<asp:TemplateField>
				<HeaderTemplate>
					<asp:Label runat="server" Text="Asociar a"></asp:Label>
				</HeaderTemplate>
				<ItemTemplate>
                    <asp:TextBox runat="server" ID="txtNombreUsuario" Columns="35" CssClass="campoObligatorio" AutoComplete="Off"></asp:TextBox>&nbsp;
                    <asp:HiddenField ID="hfIdresponsable" runat="server" />
                    <asp:HiddenField ID="hfTarjetaExcep" runat="server" />
                    <asp:ImageButton runat="server" ID="imgAutoRellenar" ToolTip="Cuando rellene el nombre, pulse aqui para rellenar con el nombre introducido las personas repetidas" ImageUrl="~/App_Themes/Tema1/Images/autoFill.png" ImageAlign="AbsMiddle" />
                    <br /><div runat="server" id="helper" style="position:absolute;z-index:9;"></div>                                           
				</ItemTemplate>
			</asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate><asp:ImageButton runat="server" ID="imgException" ToolTip="Marque esta visa como excepcion para que se trate con una cuenta diferente" ImageUrl="~/App_Themes/Tema1/Images/Exception.png" ImageAlign="AbsMiddle" /></ItemTemplate>
            </asp:TemplateField>	           													
		</Columns>
	</asp:GridView><br />
    <div id="botones"><asp:ImageButton runat="server" ID="imgGuardar" ToolTip="Guardar" ImageUrl="~/App_Themes/Tema1/IconosBotones/Guardar.png" /></div>  
</asp:Panel><br />
<asp:Panel runat="server" ID="pnlTodosOk" GroupingText="Procesados con exito">
     <asp:Image runat="server" id="imgResul" ImageAlign="AbsMiddle" ImageUrl="~/App_Themes/Tema1/Images/ok_big.gif" />&nbsp;&nbsp;
     <asp:Label ID="Label10" runat="server" CssClass="labelDetalle" Text="Todos los registros se han procesado con exito"></asp:Label><br /><br />
     <asp:Button runat="server" ID="btnContinuar" Text="Continuar" />
</asp:Panel>