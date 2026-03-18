<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="MantServicios.aspx.vb" Inherits="WebRaiz.MantServicios" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">	
    <asp:UpdatePanel runat="server">
        <ContentTemplate>        
	        <asp:MultiView runat="server" ID="mvServicios" ActiveViewIndex="0">
		        <asp:View runat="server" ID="vListado">	                    
                    <asp:Label runat="server" ID="labelListadoServ" Text="Servicios que podrán solicitar los usuarios al realizar una solicitud de viaje. Los marcados como obsoletos no les apareceran"></asp:Label><br /><br />
                    <asp:LinkButton runat="server" id="lnkNuevo" Text="Nuevo"></asp:LinkButton><br /><br />
			        <asp:GridView runat="server" ID="gvServicios" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="true" CssClass="table table-striped table-hover" GridLines="None" PageSize="20">				        
                        <PagerStyle CssClass="bs-pagination" HorizontalAlign="Center" />
                        <PagerSettings PageButtonCount="5" />
				        <EmptyDataTemplate><asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
				        <Columns>	
                            <asp:BoundField DataField="Nombre" HeaderText="servicio" />
                            <asp:TemplateField HeaderText="Requiere usuario" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><asp:CheckBox runat="server" Checked='<%#Eval("RequiereUsuario") %>' Enabled="false" /></ItemTemplate>
                            </asp:TemplateField> 
				        </Columns>
			        </asp:GridView>
		        </asp:View>
		        <asp:View runat="server" ID="vDetalle">
                   <div class="row">
                       <div class="col-sm-2"><asp:Label runat="server" ID="labelNombre" Text="Nombre"></asp:Label></div>
                       <div class="col-sm-10">
                           <asp:Textbox runat="server" ID="txtNombre" CssClass="campoObligatorio form-control" MaxLength="30"></asp:Textbox> 
						   <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Display="None" ControlToValidate="txtNombre" ValidationGroup="Guardar" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
						   <ajax:ValidatorCalloutExtender runat="server" ID="vceNombre" TargetControlID="rfvNombre" />
                       </div>
                   </div>
                    <div class="row">
                        <div class="col-sm-12"><asp:Label runat="server" id="labelDescr" Text="descripcion"></asp:Label></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:TextBox runat="server" id="txtDescripcion" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revDescripcion" runat="server" ControlToValidate="txtDescripcion" ValidationGroup="Guardar" Display="None" ErrorMessage="100 caracteres maximo" ValidationExpression="[\s\S]{0,100}"></asp:RegularExpressionValidator>
                            <ajax:ValidatorCalloutExtender ID="vceDescripcion" TargetControlID="revDescripcion" runat="server" PopupPosition="TopLeft" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbReqUsuario" Text="Requiere usuario" TextAlign="Left" CssClass="form-control" /></div>
                        <div class="col-sm-3"><asp:CheckBox runat="server" ID="chbObsoleto" Text="obsoleto" TextAlign="Left" Tooltip="Cuando se seleccione este servicio, tendran que seleccionar un usuario" CssClass="form-control" /></div>
                    </div><br />			                          			        
			        <div class="row">                        
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Guardar" CssClass="form-control btn btn-primary" /></div>
                        <div class="col-sm-2"><asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="form-control btn btn-default" /></div>
			        </div>
		        </asp:View>
	        </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:CargandoDatos runat="server" />
 </asp:Content>
