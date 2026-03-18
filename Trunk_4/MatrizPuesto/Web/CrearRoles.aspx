<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_plantas.Master" CodeBehind="CrearRoles.aspx.vb" Inherits="AdokWeb.CrearRoles" %>

<%@ MasterType VirtualPath="~/Adok_plantas.Master" %>


<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">

    <script type="text/javascript">

        function RecogerResponsable(source, eventArgs) {
            var hfResponsable = document.getElementById('<%=hfResponsable.ClientID%>');
            hfResponsable.value = eventArgs.get_value();
        }



    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">

    <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="Mantenimiento de Roles" /></a>
        </div>
    </div>







    <div class="container">

        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">

            <asp:View ID="viewListado" runat="server">

<%--                <div class=" panel-header">


                    <h2   class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblEmpresa" runat="server" Text="Mantenimiento de Roles" /></h2>


                </div>--%>
                <br />


                  <div class="form-inline row">
                                <div class="form-group col-sm-12">

                            <asp:Label runat="server" ID="Label19" Text="Usuario" />:
                           <asp:TextBox ID="txtResponsable" runat="server"  Width="40%" class="form-control required" required="required" />

                            <act:AutoCompleteExtender ID="AutoCompleteExtender2" ServiceMethod="CargarResponsable"
                                runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerResponsable"
                                TargetControlID="txtResponsable" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />

                            <asp:HiddenField ID="hfResponsable" runat="server" />

                                <asp:DropDownList Visible="false" class="btn btn-default dropdown-toggle" type="button" runat="server" ID="ddlDepartamento" data-toggle="dropdown">

                                    <asp:ListItem Text="PREVISIÓN" Value="1" />
                                    <asp:ListItem Text="MEDIO AMBIENTE" Value="9" />
                                   </asp:DropDownList>

                               <asp:DropDownList class="btn btn-default dropdown-toggle" type="button" runat="server" ID="ddlUni" data-toggle="dropdown">

                                    <asp:ListItem Text="Administrador " Value="2" />
                                    <asp:ListItem Text="Consulta " Value="9" />
                                   </asp:DropDownList>


  



                             <asp:LinkButton ID="btnNuevo" OnClick="btnNuevo_Click" runat="server" Text="Añadir nuevo" class="btn btn-primary" ValidationGroup="foot" />
                 
       </div>

                            </div>


                 <br />
                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                    GridLines="None" DataKeyNames="Id" ShowFooter="true" OnRowCommand="gvType_OnRowCommand"
                    OnRowEditing="gvType_RowEditing">
                    <%--<RowStyle CssClass="RowStyle" HorizontalAlign="Center" />--%>
                    <%--<FooterStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" Height="40" VerticalAlign="Middle" />--%>
                    <PagerStyle CssClass="PagerStyle" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                    <%--<HeaderStyle CssClass="HeaderStyle" />--%>
                    <EditRowStyle CssClass="EditRowStyle" />
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="Cod." visible="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Nombre del Usuario">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblDescripcion" Text='<%# Eval("Nombreuser")%>' runat="server" Font-Underline="true" ToolTip="Modificar" CommandName="Edit" />
                            </ItemTemplate>
<%--                            <FooterTemplate>


                             <asp:TextBox ID="txtResponsable" runat="server" class="form-control" />

                            <act:AutoCompleteExtender ID="AutoCompleteExtender2" ServiceMethod="CargarResponsable"
                                runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerResponsable"
                                TargetControlID="txtResponsable" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />

                            <asp:HiddenField ID="hfResponsable" runat="server" />


                               <asp:TextBox ID="txtDescript" runat="server" class="form-control" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="*" ErrorMessage="Añade Código de usuario" ControlToValidate="txtDescript" ValidationGroup="foot" Display="None" />
                                <act:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="Right" />

                       <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtDescript" ValidationGroup="foot" Font-Italic="true" Display="None"
                        SetFocusOnError="true" ErrorMessage="Solo se admiten números" ValidationExpression="\d+" />

                     
                                  <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="REV1" PopupPosition="Right" />
                            </FooterTemplate>--%>

                        </asp:TemplateField>


 <%--                       <asp:TemplateField HeaderText="Departamento">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblAbrev2" Text='<%# Eval("NombreDepto")%>' runat="server" Font-Underline="true" CommandName="Edit" />

                            </ItemTemplate>

                            <FooterTemplate>
   

  
                            </FooterTemplate>
                        </asp:TemplateField>--%>


                        <asp:TemplateField HeaderText="Rol Asignado">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblAbrev" Text='<%# Eval("nombrerol")%>' runat="server" Font-Underline="true" CommandName="Edit" />

                            </ItemTemplate>

                            <FooterTemplate>
   

  
                            </FooterTemplate>
                        </asp:TemplateField>






                        <asp:TemplateField>

                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Desactivar" Text="Eliminar" Visible="true" />
                                <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="btnEliminar2">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminar2" OkControlID="btnBorrar2"
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Confirmación" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text="¿Estás seguro que quieres borrar el rol?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar2" runat="server" CssClass="btn btn-primary" Text="Si" />
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>

                            <FooterTemplate>
                              
                            </FooterTemplate>
                        </asp:TemplateField>

                        

                    </Columns>

                </asp:GridView>



            </asp:View>


            <asp:View ID="view1" runat="server">

 <%--               <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="Mantenimiento de datos" /></h2>

                </div>--%>

                 <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">

                            <asp:Label runat="server" ID="lblNombre" Text="Nombre del usuario" />:
                                          
                                                <asp:TextBox ID="txtNombre" runat="server" class="form-control" ReadOnly="true" />
           <%--                 <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="*" ErrorMessage="Añade Código de usuario" ControlToValidate="txtNombre" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="Right" />--%>

                            <asp:Label runat="server" ID="Label1" Text="Rol" />:
                            <div class="dropdown">
                                <asp:DropDownList class="btn btn-default dropdown-toggle" type="button" runat="server" ID="ddlUnidades" data-toggle="dropdown">

                                    <asp:ListItem Text="Administrador " Value="2" />
                                </asp:DropDownList>
                            </div>

                            

                        </div>
                    </div>




                </div>

                <div class="panel-footer">
                    <div class="text-center">



                        <asp:HiddenField runat="server" ID="flag_Modificar" />
                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-primary" Text="Cancelar" UseSubmitBehavior="false" />

                        <asp:Button ID="btnGuardarNuevaSolicitud" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia_Click" Text="Grabar" UseSubmitBehavior="true"  />


                    </div>
                </div>
                <br />
                <br />

            </asp:View>

        </asp:MultiView>


    </div>



</asp:Content>

