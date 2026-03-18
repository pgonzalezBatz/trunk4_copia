<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPKEM.Master" CodeBehind="Departamentos.aspx.vb" Inherits="KEM.Departamentos"%>
<%@ MasterType VirtualPath="~/Master/MPKEM.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="saveDepartamento" DisplayMode="BulletList" HeaderText="Errores" />    
   <asp:MultiView runat="server" ActiveViewIndex="0" ID="mvDeptos">
        <asp:View runat="server" ID="vwListado">
            <asp:Panel runat="server" ID="pnlDeptoProgNominas">
                <br /><b><asp:Label runat="server" ID="lblMensaProgNominas"></asp:Label></b>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDeptoKEM">
                <asp:Label runat="server" ID="labelTitulo" Text="listadoDepartamentos" CssClass="Titulo"></asp:Label><br /> <br />
                <div ><asp:LinkButton id="lnkCrearDepto"  runat="server" Text="crearDepartamento"></asp:LinkButton></div><br />
                <asp:GridView runat="server" ID="gvDeptos" AutoGenerateColumns="false" Width="50%" AllowSorting="true" AllowPaging="true" PageSize="20" PagerSettings-Mode="NumericFirstLast">
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <RowStyle CssClass="GridViewRowStyle" />
                    <PagerStyle HorizontalAlign="Center" />
                    <PagerSettings PageButtonCount="5" />
                    <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                    <EmptyDataTemplate><br /><asp:Label runat="server" Text="noExisteNingunRegistro" style="margin-left:25px;"></asp:Label></EmptyDataTemplate>                     
                    <Columns>
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate><asp:LinkButton ID="lbtnDepartamento" runat="server" CommandArgument='<%#Eval("id")%>' Text='<%#Eval("nombre")%>' OnClick="SelectDepartamento"></asp:LinkButton></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Elim" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
                            <ItemTemplate><asp:ImageButton ID="imgDel" runat="server" CommandArgument='<%#Eval("id") %>' CommandName='<%#Eval("nombre")%>' ImageUrl="~/App_Themes/Tema1/Images/borrar.gif" OnClick="DeleteDepartamento" ToolTip="Eliminar" /></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </asp:View>        
        <asp:View runat="server" ID="vwDetalle">
            <fieldset style="width:650px"> 
             <table>     
                    <tr>
                        <th colspan="2" align="left"><asp:Label ID="lblCabecera" runat="server"  /><br /></th>
                    </tr>                    
                    <asp:Panel ID="pnlExistente" runat="server">
                    <tr>
                        <td><asp:Literal runat="server" ID="labelId" Text="id"></asp:Literal></td>
                        <td><asp:Label ID="lblIdDepto" runat="server"></asp:Label></td>                        
                    </tr>  
                    </asp:Panel>                        
                    <tr>
                        <td><asp:Literal runat="server" ID="labelNombre" Text="Nombre"></asp:Literal></td>
                        <td><asp:TextBox ID="txtNombre" runat="server" Width="300px"></asp:TextBox></td>                        
                    </tr>                                                                                                                                 
                </table>                                       
              </fieldset><br /><br />
            <asp:Button ID="btnSave" runat="server" ValidationGroup="saveDepartamento" Text="Guardar" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server"  Text="volver" OnClick="btnCancel_Click" style="margin-left:15px;" />                                 
            <asp:RequiredFieldValidator runat="server" ID="rfvNombre" ControlToValidate="txtNombre" Display="None" ValidationGroup="saveDepartamento" Text="introduzcaNombre"></asp:RequiredFieldValidator>
        </asp:View>
   </asp:MultiView>
</asp:Content>
