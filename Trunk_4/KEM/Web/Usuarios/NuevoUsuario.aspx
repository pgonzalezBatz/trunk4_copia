<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPKEM.Master" CodeBehind="NuevoUsuario.aspx.vb" Inherits="KEM.NuevoUsuario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ Register Src="~/Controls/SeleccionUsuarios.ascx" TagName="SelUsuarios" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/Master/MPKEM.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
     <script type="text/javascript" src="../js/Utiles.js"></script>    
     <script type="text/javascript" src="../js/ajax.js" ></script>    
    <script language="javascript">
        var ModalProgress ='<%= ModalProgress.ClientID %>';                   
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>     
            <tit:Titulo ID="tit" runat="server" Texto="altaUsuarios" />
             <asp:Panel runat="server" ID="pnlNuevoUsuario">
            <fieldset style="width: 500px">
		        <br /><asp:Panel runat="server" ID="pnlTipo" GroupingText="tipo">
			        <asp:Label runat="server" ID="labelTieneDom" Text="¿Tiene usuario de dominio?"></asp:Label>&nbsp;
			        <asp:RadioButtonList runat="server" ID="rblUsuarioDominio" AutoPostBack="true" RepeatDirection="Horizontal"></asp:RadioButtonList>
                </asp:Panel><br />
                <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                    <asp:Label runat="server" ID="labelBuscar" Text="buscaUsuariosDirectorioActivo"></asp:Label><br /><br />            
                    <asp:Panel runat="server" ID="pnlConUsuarioDominio">
				        <table width="100%">
					        <tr>
						        <td>
							        <asp:Label runat="server" ID="labelUsuario" Text="usuario"></asp:Label>&nbsp;
							        <asp:TextBox runat="server" ID="txtUsuario"></asp:TextBox>
						        </td>
						        <td>&nbsp;</td>
						        <td>
							        <asp:Button runat="server" ID="btnBuscar" Text="crear" />&nbsp;
							        <asp:Button runat="server" ID="btnVolver" Text="volver" />
						        </td>
					        </tr>
				        </table>
                    </asp:Panel>
                </asp:Panel>
            </fieldset><br />
            <asp:Panel ID="pnlSinResultados" runat="server">
                <div id="centrado">
                    <asp:Label runat="server" CssClass="negrita" ID="lblMensaje"></asp:Label>
                    <asp:LinkButton runat="server" ID="lnkVerInfo" Text="Ver info" style="margin-left:15px"></asp:LinkButton>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlResultados" runat="server">
                <table width="80%">
			        <asp:Panel runat="server" ID="pnlUsuarioLabel">
                    <tr>
                        <th align="left" width="100px"><asp:Label runat="server" ID="labelUsuarioDet" Text="usuario"></asp:Label></th>
                        <td colspan="3"><asp:Label runat="server" ID="lblNombreUsuario"></asp:Label></td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <th align="left" width="100px"><asp:Label runat="server" ID="labelNombre" Text="nombre"></asp:Label></th>
                        <td colspan="3"><asp:Textbox runat="server" ID="txtNombrePersona" CssClass="mayusculas"></asp:Textbox></td>                
                    </tr>
                    <tr>
                        <th align="left" width="100px"><asp:Label runat="server" ID="labelAp1" Text="primerApellido"></asp:Label></th>
                        <td><asp:Textbox runat="server" ID="txtApellido1" CssClass="mayusculas"></asp:Textbox></td>
                        <th align="left"><asp:Label runat="server" ID="labelAp2" Text="segundoApellido"></asp:Label></th>
                        <td><asp:Textbox runat="server" ID="txtApellido2" CssClass="mayusculas"></asp:Textbox></td>
                    </tr>
                    <asp:Panel runat="server" ID="pnlEmailLabel">
				        <tr>				
					        <th align="left" width="100px"><asp:Label runat="server" ID="labelEmail" Text="email"></asp:Label></th>
					        <td colspan="2"><asp:Label runat="server" ID="lblEmail"></asp:Label></td>
				        </tr>   
                    </asp:Panel>            
                    <tr>
                        <th align="left" width="100px"><asp:Label runat="server" ID="labelDept" Text="departamento"></asp:Label></th>
                        <td><asp:Dropdownlist runat="server" ID="ddlDepartamento" AppendDataBoundItems="true"></asp:Dropdownlist></td>
                        <th align="left"><asp:Label runat="server" ID="labelResp" Text="responsable"></asp:Label></th>
                        <td>
					        <asp:TextBox runat="server" Enabled="false" ID="txtResponsable" Columns="35" ToolTip="Seleccione un responsable"></asp:TextBox>&nbsp;
					        <asp:ImageButton runat="server" ID="imgBuscarResp" ToolTip="Buscar responsable" ImageUrl="~/App_Themes/Tema1/Images/buscarUsuario.gif" ImageAlign="Middle" />
                        </td>
                    </tr> 
                    <tr>
                        <th align="left" width="100px"><asp:Label runat="server" ID="labelDNI" Text="DNI"></asp:Label></th>
                        <td><asp:Textbox runat="server" ID="txtDNI" MaxLength="14"></asp:Textbox></td>
                    </tr>
                </table><br />
                <div id="centrado">
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" />&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnNuevo" Text="nuevo" />
                </div>        
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlNuevoUsuarioNV"><asp:Label runat="server" ID="labelInfo2" Text="No se puede dar de alta un nuevo usuario hasta que no se informe el campo 'Ruta del LDAP' de la planta. Vaya a la administracion de plantas e informelo"></asp:Label></asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hfPathLDAP" />
	  <asp:Panel runat="server" ID="pnlUsuarios" Style="display: none;" CssClass="modalBox">
        <table>
            <tr>
                <td><uc:SelUsuarios runat="server" ID="selUsuario" Vigentes="true" TipoSeleccion="Single" Trabajador="Todos"></uc:SelUsuarios></td>
                <td valign="top"><asp:ImageButton runat="server" ID="imgCerrarDetalle" ImageUrl="~/App_Themes/Tema1/images/cerrar.gif" /></td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:Button runat="server" ID="btnAceptarResp" Text="aceptar" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label runat="server" ID="lblHiddenUsuario" Style="display: none"></asp:Label>
    <cc1:ModalPopupExtender ID="mpeUsuario" runat="server" TargetControlID="lblHiddenUsuario" PopupControlID="pnlUsuarios" CancelControlID="imgCerrarDetalle" BackgroundCssClass="modalBackground" />    
     <asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress">
       <asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server" >
        <ProgressTemplate>
          <div style="position: relative; top: 30%; text-align:center;">    
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Tema1/Images/loadin.gif" />                            
            <asp:Label ID="lblFiltrando" runat="server" Text="cargandoDatos"></asp:Label>   
          </div>
        </ProgressTemplate>
      </asp:UpdateProgress>
     </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground2" PopupControlID="panelUpdateProgress" />     
</asp:Content>
