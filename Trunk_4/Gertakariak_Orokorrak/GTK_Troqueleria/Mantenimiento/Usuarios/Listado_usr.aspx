<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Listado_usr.aspx.vb" Inherits="GTK_Troqueleria.Listado_usr" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        <asp:Label ID="Label1" runat="server" Text="Listado de Usuarios" /></h3>
    <hr />

    <!-- Nav tabs -->
    <ul class="nav nav-tabs" role="tablist">
        <li role="presentation" class="active"><a href="#tb_Usuarios" aria-controls="tb_Usuarios" role="tab" data-toggle="tab">
            <asp:Label ID="Label7" runat="server" Text="Usuarios" /></a></li>
        <li role="presentation"><a href="#tb_Roles" aria-controls="tb_Roles" role="tab" data-toggle="tab">
            <asp:Label ID="Label9" runat="server" Text="Roles" /></a></li>
    </ul>
    <!-- Tab panes -->
    <div class="tab-content">
        <div role="tabpanel" class="tab-pane active" id="tb_Usuarios">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Panel ID="pnlBuscador" runat="server" DefaultButton="btnBuscar" CssClass="row">
                        <div class="col-xs-8 col-sm-offset-1 col-sm-6  col-sm-offset-2 col-sm-6">
                            <div class="input-group">
                                <asp:TextBox ID="txtBuscar" runat="server" TextMode="Search" CssClass="form-control" placeholder="Buscar"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <%-- <div class="panel-body">
                    </div>--%>

                <div class="panel panel-default">
                    <asp:Panel ID="pnlNuevoUsuario" runat="server" CssClass="panel-heading">
                        <div class="input-group col-sm-6">
                            <div class="input-group-btn">
                                <div id="btnNuevoUsuario" class="btn btn-default"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span></div>
                            </div>
                            <asp:TextBox ID="txtBuscarUsuario" runat="server" CssClass="form-control" AutoCompleteType="Search" Style="display: none;"></asp:TextBox>
                            <asp:Button ID="btnAgregarUsuario" runat="server" Text="Usuario" Style="display: none;" />
                            <asp:HiddenField ID="hd_NuevoUsuario" runat="server" />
                            <ajaxToolkit:TextBoxWatermarkExtender ID="tbwe_txtBuscarUsuario" runat="server" TargetControlID="txtBuscarUsuario" WatermarkText="Nuevo Usuario" WatermarkCssClass="TextBoxWatermarkExtender" />
                            <ajaxToolkit:AutoCompleteExtender ID="ace_txtBuscarUsuario" runat="server" Enabled="True"
                                TargetControlID="txtBuscarUsuario" UseContextKey="true" ContextKey="" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="300" EnableCaching="true"
                                CompletionListCssClass="list-group"
                                CompletionListItemCssClass="list-group-item"
                                CompletionListHighlightedItemCssClass="list-group-item list-group-item-info"
                                FirstRowSelected="true"
                                ServicePath="~/Controles/ServiciosWeb.asmx"
                                ServiceMethod="get_Usuarios"
                                OnClientItemSelected="Set_Usuario" />
                        </div>
                    </asp:Panel>
                    <asp:GridView SkinID="gvBootstrap" ID="gvUsuarios" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" AutoGenerateColumns="false"
                        PageSize="10" AllowPaging="true">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="10%" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <div class="btn-toolbar" role="toolbar" aria-label="...">
                                        <div class="btn-group" role="group" aria-label="...">
                                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-default btn-xs" ToolTip="Editar" CommandName="Edit"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></asp:LinkButton>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="Usuario" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="btn_Usuario" runat="server" Text="Usuario" CommandName="Sort" CommandArgument="NombreCompleto" CssClass="hidden-xs"></asp:LinkButton>
                                    <asp:LinkButton ID="btn_Usuario_xs" runat="server" CommandName="Sort" CommandArgument="NombreCompleto" CssClass="visible-xs">
                        <span class="glyphicon glyphicon-user" aria-hidden="true"></span>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUsuario" runat="server" Text='<%# Eval("NombreCompleto") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField AccessibleHeaderText="Rol" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left" HeaderText="Rol">
                                <ItemTemplate>
                                    <asp:BulletedList ID="blRol" runat="server" DataTextField="NOMBRE"></asp:BulletedList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="jumbotron">
                                <h3>
                                    <asp:Label ID="Label2" runat="server" Text="Sin Datos"></asp:Label></h3>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>

            </div>
        </div>
        <div role="tabpanel" class="tab-pane" id="tb_Roles">
            <div class="panel panel-default">
                <asp:Panel ID="pnlNuevo_Rol" runat="server" CssClass="panel-heading">
                    <div class="btn-group" role="group" aria-label="...">
                        <asp:LinkButton ID="btnNuevo_ROL" runat="server" ToolTip="Nuevo Rol" CssClass="btn btn-default">
                                <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
                <%--<div class="panel-body">
                        </div>--%>
                <asp:GridView SkinID="gvBootstrap" ID="gvRoles" runat="server" RowHeaderColumn="ID" DataKeyNames="ID" AutoGenerateColumns="false"
                    PageSize="10" AllowPaging="true">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <div class="btn-toolbar" role="toolbar" aria-label="...">
                                    <div class="btn-group" role="group" aria-label="...">
                                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-default btn-xs" ToolTip="Editar" CommandName="Edit"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></asp:LinkButton>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" />
                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION" />

                        <asp:TemplateField AccessibleHeaderText="Rol" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left" HeaderText="Permisos">
                            <ItemTemplate>
                                <asp:BulletedList ID="blRol_Permisos" runat="server"></asp:BulletedList>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>
                        <div class="jumbotron">
                            <h3>
                                <asp:Label ID="Label2" runat="server" Text="Sin Datos"></asp:Label></h3>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>

            </div>
        </div>
    </div>

    <!-- Modal -->
    <asp:Panel ID="pnl_Roles" runat="server" CssClass="modal fade" TabIndex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        <asp:Label ID="lblUsuario" runat="server" Text="?" />
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="Roles" CssClass="col-sm-2 control-label" AssociatedControlID="cbPermisos"></asp:Label>
                                <div class="checkbox  col-sm-10 col-sm-offset-2">
                                    <asp:CheckBoxList ID="cblRol" runat="server" AppendDataBoundItems="true" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnBorrar_Usuario" runat="server" ToolTip="Borrar" CssClass="btn btn-danger">
                        <span class="glyphicon glyphicon-remove"></span>
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="cbe_btnBorrar_Usuario" runat="server" TargetControlID="btnBorrar_Usuario" ConfirmText="Desea eliminar" />
                    <asp:LinkButton ID="btnGuardar_Usuario" runat="server" ToolTip="Guardar" CssClass="btn btn-primary">
                        <span class="glyphicon glyphicon-ok"></span>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Modal -->
    <asp:Panel ID="pnlPermisos" runat="server" CssClass="modal fade" TabIndex="-1" role="dialog" aria-labelledby="myModal_lblRol">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModal_lblRol">
                        <asp:Label ID="lblRol" runat="server" Text="?" />
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Rol" CssClass="col-sm-2 control-label" AssociatedControlID="txtNombre_Rol"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtNombre_Rol" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                            </div>
                            <asp:Label ID="Label5" runat="server" Text="Descripcion" CssClass="col-sm-2 control-label" AssociatedControlID="txtDescripcion_Rol"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtDescripcion_Rol" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Permisos" CssClass="col-sm-2 control-label" AssociatedControlID="cbPermisos"></asp:Label>
                            <div class="checkbox  col-sm-10 col-sm-offset-2">
                                <asp:CheckBoxList ID="cbPermisos" runat="server" AppendDataBoundItems="true" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnBorrarRol" runat="server" ToolTip="Borrar" CssClass="btn btn-danger">
                        <span class="glyphicon glyphicon-remove"></span>
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="cbe_btnBorrarRol" runat="server" TargetControlID="btnBorrarRol" ConfirmText="Desea eliminar" />
                    <asp:LinkButton ID="btnGuardarRol" runat="server" ToolTip="Guardar" CssClass="btn btn-primary">
                        <span class="glyphicon glyphicon-ok"></span>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </asp:Panel>

    <script type="text/javascript">
        /*******************************************************************************/
        /* Sistema para guardar y activar la pestaña en curso. */
        /*******************************************************************************/
        var UltimoPanelActivo = "lastTab_<%=Page.AppRelativeVirtualPath.GetHashCode%>"
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            // save the latest tab; use cookies if you like 'em better:
            localStorage.setItem(UltimoPanelActivo, $(this).attr('href'));
            //sessionStorage.setItem('lastTab', $(this).attr('href'));
        });

        // go to the latest tab, if it exists:
        var lastTab = localStorage.getItem(UltimoPanelActivo);
        //var lastTab = sessionStorage.getItem('lastTab');
        if (lastTab) { $('[href="' + lastTab + '"]').tab('show'); } //else { $('[href="#Certificados"]').tab('show');}
        /*******************************************************************************/

        /*******************************************************************************/
        /* Ventana de busqueda de usuarios */
        /*******************************************************************************/
        $("#btnNuevoUsuario").click(function () {
            $("#<%=txtBuscarUsuario.ClientID%>").toggle();
            $("#<%=txtBuscarUsuario.ClientID%>").focus();

        });
        function Set_Usuario(source, eventArgs) {
            $get('<%=hd_NuevoUsuario.ClientID%>').value = eventArgs.get_value();
            $get('<%=btnAgregarUsuario.ClientID%>').click();
        }
        /*******************************************************************************/
    </script>
</asp:Content>
