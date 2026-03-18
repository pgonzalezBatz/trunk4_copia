<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="Roles.aspx.vb" Inherits="Nominas.Roles" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
	<asp:UpdatePanel runat="server">
		<ContentTemplate>
			<asp:MultiView ID="mvUsuarios" runat="server" ActiveViewIndex="0">
				<asp:View ID="vListado" runat="server">					
					<asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar" CssClass="row">
                        <div class="col-sm-1"><asp:Label runat="server" ID="labelUsuario" Text="usuario"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox runat="server" ID="txtUsuario" CssClass="form-control"></asp:TextBox></div>
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnBuscar" CssClass="btn btn-primary col-xs-12" Text="Buscar" /></div>							
					</asp:Panel>					
					<div><br />
						<asp:Panel runat="server" ID="pnlResul">
							<asp:GridView runat="server" ID="gvUsuarios" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-condensed" AllowSorting="true" AllowPaging="true" PageSize="20" PagerSettings-Mode="NumericFirstLast">           
								<PagerStyle HorizontalAlign="Center" />
								<PagerSettings PageButtonCount="5" />
								<EmptyDataTemplate><br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label>
								</EmptyDataTemplate>
								<Columns>
									<asp:TemplateField Visible="false">
										<ItemTemplate><asp:Label runat="server" ID="lblIdUser"></asp:Label></ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField SortExpression="NombreCompleto">
										<HeaderTemplate>
                                            <asp:LinkButton runat="server" Text="nombrePersona" CommandName="Sort" CommandArgument="NombreCompleto" CssClass="hidden-xs"></asp:LinkButton>
                                            <span class="visible-xs glyphicon glyphicon-user"></span>
										</HeaderTemplate>
										<ItemTemplate><asp:Label runat="server" ID="lblNombreCompleto"></asp:Label></ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center">
										<HeaderTemplate><asp:Label runat="server" Text="Admin"></asp:Label></HeaderTemplate>
										<ItemTemplate><asp:CheckBox runat="server" ID="chbAdmin" />											
										</ItemTemplate>
									</asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
										<HeaderTemplate><asp:Label runat="server" Text="Encriptar"></asp:Label></HeaderTemplate>
										<ItemTemplate><asp:CheckBox ID="chbEncriptar" runat="server" Enabled="false"></asp:CheckBox></ItemTemplate>
									</asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
										<HeaderTemplate><asp:Label runat="server" Text="Doc 10T"></asp:Label></HeaderTemplate>
										<ItemTemplate><asp:CheckBox ID="chbDoc10T" runat="server" Enabled="false"></asp:CheckBox></ItemTemplate>
									</asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
										<HeaderTemplate><asp:Label runat="server" Text="Doc Intereses"></asp:Label></HeaderTemplate>
										<ItemTemplate><asp:CheckBox ID="chbDocInt" runat="server" Enabled="false"></asp:CheckBox></ItemTemplate>
									</asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
										<HeaderTemplate><asp:Label runat="server" Text="Planta"></asp:Label></HeaderTemplate>
										<ItemTemplate><asp:Label ID="lblPlanta" runat="server"></asp:Label></ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</asp:Panel>
					</div>
				</asp:View>
				<asp:View runat="server" ID="vDetalle">
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" id="labelPersona" Text="persona"></asp:Label></div>
                        <div class="col-sm-10">
                            <asp:Label ID="lblPersona" runat="server" CssClass="negrita"></asp:Label>
							<asp:HiddenField runat="server" ID="hfIdUsuario" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"><asp:Label runat="server" id="labelPlanta" Text="Planta"></asp:Label></div>
                        <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlPlantas" AppendDataBoundItems="true" DataTextField="Nombre" DataValueField="Id" Enabled="false" CssClass="form-control"></asp:DropDownList></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12"><asp:CheckBox runat="server" ID="chbEsAdmin" AutoPostBack="true" Text="Es administrador" /></div>
                    </div>
                    <div class="form-inline">
                        <div class="form-group"><asp:CheckBox runat="server" ID="chbEncriptar" Text="Encriptar" ToolTip="Habilitar si la persona podra encriptar nominas" /></div>
                        <div class="form-group"><asp:CheckBox runat="server" ID="chbDoc10T" Text="Documentos 10T" ToolTip="Habilitar si la persona podra tratar los documentos 10T" /></div>
                        <div class="form-group"><asp:CheckBox runat="server" ID="chbDocInt" Text="Documentos de intereses" ToolTip="Habilitar si la persona podra tratar los documentos de intereses" /></div>
                    </div><br /><br />
					<div class="form-inline">
                         <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-primary" />
                         <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="btn btn-primary" />							
					</div>
				</asp:View>
			</asp:MultiView>
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
		</Triggers>
	</asp:UpdatePanel>	
    <uc:CargandoDatos runat="server"></uc:CargandoDatos>
</asp:Content>
