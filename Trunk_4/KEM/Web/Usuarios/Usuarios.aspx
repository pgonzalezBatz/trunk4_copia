<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/MPKEM.Master" CodeBehind="Usuarios.aspx.vb" Inherits="KEM.Usuarios" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ Register Src="~/Controls/SeleccionUsuarios.ascx" TagName="SelUsuarios" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/Master/MPKEM.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <script type="text/javascript" src="../js/Utiles.js"></script>
    <script type="text/javascript" src="../js/ajax.js"></script>
    <script language="javascript" type="text/javascript">
        var ModalProgress ='<%= ModalProgress.ClientID %>';            
        function DisableGuardar()
        {           
           var txtMail=document.getElementById('<%=txtEmail.ClientId%>');
           var btnGuardar=document.getElementById('<%=btnGuardar.ClientId%>');
           var pnlValidar=document.getElementById('<%=pnlValidar.ClientId%>');
           if(txtMail.value.length==0)
           {
              btnGuardar.disabled=false;
              pnlValidar.visibility='visible';
           }
           else
           {
             btnGuardar.disabled=true;
             pnlValidar.visibility='hidden';
           }
        }
        
         var ventanaHija=null; 
       ///Abre una popup centrada a la pagina padre
       function abrirVentanaCentrada(url,name,width,heigh) {
               
              var window_width = width;
              var window_height = heigh;
              var newfeatures= 'scrollbars=yes,resizable=no';
              var window_top = (screen.height-window_height)/2;
              var window_left = (screen.width-window_width)/2;                            
              ventanaHija=window.open(url, name,'width=' + window_width + ',height=' + window_height + ',top=' + window_top + ',left=' + window_left + ',' + newfeatures + '');
       }
    </script>
    <tit:Titulo ID="tit" runat="server" Texto="busqueda" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:MultiView ID="mvUsuarios" runat="server" ActiveViewIndex="0">
                <asp:View ID="vListado" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Filtro" Width="500px" DefaultButton="btnBuscar">                                    
                                    <table width="50%">
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="labelUsuario" Text="usuario"></asp:Label>&nbsp;
                                                <asp:TextBox runat="server" ID="txtUsuario"></asp:TextBox>
                                            </td>                                    
                                            <td style="padding-left:15px"><asp:Button runat="server" ID="btnBuscar" Text="Buscar" /></td>                                    
                                            <td style="padding-left:40px"><asp:LinkButton runat="server" ID="lnkNuevo" Text="nuevo" CssClass="font13"></asp:LinkButton></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3"><asp:CheckBox runat="server" ID="chbVerTodos" Text="Ver tambien los usuarios dados de baja" AutoPostBack="true" /></td>
                                        </tr>
                                    </table>                                    
                                </asp:Panel>
                            </td>
                            <td style="padding-left:30px;">
                                <asp:Panel runat="server" ID="pnlUserInfo" GroupingText="Usuarios" Width="200px">
                                    <table>
                                        <tr>
                                            <td><asp:Label runat="server" ID="labelUserActivos" Text="Activos"></asp:Label></td>
                                            <td style="padding-left:30px;"><asp:Label runat="server" ID="lblUsersActivos" style="font-weight:bold;"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="labelUsersBaja" Text="Dados de baja"></asp:Label></td>
                                            <td style="padding-left:30px;"><asp:Label runat="server" ID="lblUsersBaja" style="font-weight:bold;"></asp:Label></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>                    
                    <div><br />
                        <asp:GridView runat="server" ID="gvUsuarios" AutoGenerateColumns="false" Width="50%"
                            AllowSorting="true" AllowPaging="true" PageSize="15" PagerSettings-Mode="NumericFirstLast">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <PagerStyle HorizontalAlign="Center" />
                            <PagerSettings PageButtonCount="5" />
                            <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                            <EmptyDataTemplate><br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" Text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField Visible="false" DataField="Id" />
                                <asp:TemplateField SortExpression="nombrecompleto">
                                    <HeaderTemplate><asp:LinkButton runat="server" Text="nombre" CommandName="Sort" CommandArgument="nombrecompleto"></asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" Text='<%#Eval("NombreCompleto").ToString().ToUpper()%>' CssClass="nowrap"></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="nombreusuario">
                                    <HeaderTemplate><asp:LinkButton runat="server" Text="usuario" CommandName="Sort" CommandArgument="nombreusuario"></asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" Text='<%#Eval("nombreUsuario")%>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="codpersona" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%" HeaderStyle-Wrap="false">
                                    <HeaderTemplate><asp:LinkButton runat="server" Text="CodigoDeTrabajador" CommandName="Sort" CommandArgument="codpersona"></asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate><asp:Label runat="server" Text='<%#Eval("codPersona")%>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:View>
                <asp:View runat="server" ID="vDetalle">
                    <fieldset style="width: 900px">
                        <table style="width: 85%">
                            <asp:Panel runat="server" ID="pnlExistente">
                                <tr>
                                    <th  style="text-align:left;"><asp:Label runat="server" ID="labelUserDet" Text="usuario"></asp:Label></th>
                                    <td><asp:Label runat="server" ID="lblNombreUsuario" Width="250px" CssClass="minusculas"></asp:Label></td>
                                    <th  style="text-align:left;" width="100px" nowrap="nowrap"><asp:Label runat="server" ID="labelIdTrab" Text="IdTrabajador"></asp:Label></th>
                                    <td><asp:TextBox runat="server" ID="txtNumTrabajador" /></td>
                                </tr>                               
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlUsuarioLDAP">
                                <tr>
                                    <th colspan="4"><asp:Label ID="lblUserLDAP" runat="server"></asp:Label></th>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td colspan="4"><hr /></td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <table cellpadding="2px" cellspacing="2px">
                                        <tr>
                                            <th width="100px" style="text-align:left;"><asp:Label runat="server" ID="labelNombre" Text="nombre"></asp:Label></th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNombrePersona" Width="250px" CssClass="mayusculas"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombrePersona" ErrorMessage="!!!" ValidationGroup="save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th  style="text-align:left;"><asp:Label runat="server" ID="labelAp1" Text="primerApellido"></asp:Label></th>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtApellido1" Width="250px" CssClass="mayusculas"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtApellido1" ErrorMessage="!!!" ValidationGroup="save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th  style="text-align:left;"><asp:Label runat="server" ID="labelAp2" Text="segundoApellido"></asp:Label></th>
                                            <td><asp:TextBox runat="server" ID="txtApellido2" Width="250px" CssClass="mayusculas"></asp:TextBox></td>
                                            <td colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <th  style="text-align:left;"><asp:Label runat="server" ID="labelEmail" Text="email"></asp:Label></th>
                                            <td colspan="2">
                                                <asp:TextBox runat="server" ID="txtEmail" Width="250px" AutoPostBack="true"></asp:TextBox>&nbsp;&nbsp;
                                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="save"
                                                    ErrorMessage="!!!" ValidationExpression="^(([^<;>;()[\]\\.,;:\s@]+(\.[^<;>;()[\]\\.,;:\s@]+)*)|(.+))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"></asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:Panel runat="server" ID="pnlValidar">
                                                    <asp:Button runat="server" ID="btnValidar" Text="validar" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="text-align:left;"><asp:Label runat="server" ID="labelDNI" Text="DNI"></asp:Label></th>
                                            <td><asp:TextBox runat="server" ID="txtDNI" MaxLength="14"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td colspan="3"><asp:HyperLink runat="server" ID="hlViewSAB" Text="Ver datos de SAB" Target="_blank"></asp:HyperLink></td>
                                        </tr>
                                     </table>
                                </td>
                                <td valign="middle" colspan="2"><asp:Image runat="server" ID="imgFoto" ImageAlign="AbsMiddle" Height="160px" /></td>
                            </tr>                        
                        </tr>
                        <tr>
                            <td colspan="4"><hr /></td>
                        </tr>
                        <tr>
                            <th style="text-align:left;"><asp:Label runat="server" ID="labelDept" Text="departamento"></asp:Label></th>
                            <td><asp:DropDownList runat="server" ID="ddlDepartamentos" AppendDataBoundItems="true" /></td>
                            <th style="text-align:left;"><asp:Label runat="server" ID="labelResp" Text="responsable"></asp:Label></th>
							<td nowrap="nowrap">
								<asp:TextBox runat="server" Enabled="false" ID="txtResponsable" Columns="35" ToolTip="Seleccione un responsable"></asp:TextBox>&nbsp;
								<asp:ImageButton runat="server" ID="imgBuscarResp" ToolTip="Buscar responsable" ImageUrl="~/App_Themes/Tema1/Images/buscarUsuario.gif" ImageAlign="Middle" />
							</td>
                        </tr>
                        <asp:Panel runat="server" ID="pnlFechas">    
                            <tr>
                                <th style="text-align:left;"><asp:Label runat="server" ID="labelFAlta" Text="fechaAlta"></asp:Label></th>
                                <td><asp:Label runat="server" ID="lblFechaAlta"></asp:Label></td>
                                <th width="100px"  style="text-align:left;"><asp:Label runat="server" ID="labelFBaja" Text="fechaBaja"></asp:Label></th>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFechaBaja"></asp:TextBox>
                                    <cc1:CalendarExtender ID="calFechaBaja" runat="server" TargetControlID="txtFechaBaja" EnabledOnClient="true" />
                                    <asp:HiddenField runat="server" ID="hfFBaja" />
                                </td>
                            </tr>
                        </asp:Panel>
                        </table>
                    </fieldset>
                    <asp:HiddenField runat="server" ID="hfNombreUsuario" /><br /><br />
                    <div>
                        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" style="margin-left:20px;" ValidationGroup="save" />
                        <asp:Button runat="server" ID="btnVolver" Text="volver" style="margin-left:15px;" />
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
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
        <asp:UpdateProgress ID="UpdateProg1" DisplayAfter="0" runat="server">
            <ProgressTemplate>
                <div style="position: relative; top: 30%; text-align: center;">
                    <asp:Image runat="server" ImageUrl="~/App_Themes/Tema1/Images/loadin.gif" />
                    <asp:Label ID="lblFiltrando" runat="server" Text="cargandoDatos"></asp:Label>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress" BackgroundCssClass="modalBackground2" PopupControlID="panelUpdateProgress" />
    <asp:Button runat="server" ID="btnRefrescarPagina" style="visibility:hidden" />
    <asp:HiddenField runat="server" ID="hfIdUser" />
</asp:Content>
