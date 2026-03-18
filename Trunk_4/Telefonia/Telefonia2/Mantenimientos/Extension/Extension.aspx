<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master"
    CodeBehind="Extension.aspx.vb" Inherits="Telefonia.Extension" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">

    <script src="../../js/Utiles.js" type="text/javascript"></script>

    <script language="javascript">
        function seleccionarItem()
        {            
            var lista=document.getElementById('<%=lbLista.ClientId%>');
            document.getElementById('<%=txtBuscar.ClientId%>').value=lista[lista.selectedIndex].text;            
        }    
        
         function dobleClick()
        {
          var btnVerDatos=document.getElementById('<%=btnModificar.ClientId%>').name;                              
           __doPostBack(btnVerDatos,'');
        }
        
        function onKeyUpBusqueda()
        {
            var listaAux = document.getElementById('<%=lbAuxiliar.ClientId%>');
            var lista = document.getElementById('<%=lbLista.ClientId%>');
            var patron = document.getElementById('<%=txtBuscar.ClientId%>').value;               
            var text;      
            var encontrados;       
            var regEx=new RegExp(patron,"i");
            
            lista.length=0;                                              
                              
             for (var idx = 0; idx < listaAux.length; idx ++) {                                                                        
                text=listaAux[idx].text;                                                
                encontrados =text.match(regEx);                        

                if(encontrados!=null) 
                {                  
                    if(encontrados.length>0) 
                        lista.options[lista.length]= new Option (text, listaAux[idx].value);              
                }                               
            }
                        
           if(lista.length==1)
           {
              lista.selectedIndex=0;
           }
           else
              lista.selectedIndex=-1;
        }  
        
    </script>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:MultiView ID="mvExtensiones" runat="server" ActiveViewIndex="0">
                <asp:View runat="server" ID="vListado">
                    <tit:Titulo runat="server" ID="titulo" Texto="listadoExtensiones" />
                    <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnModificar">
                        <table width="25%">
                            <tr>
                                <td width="10%">
                                    <asp:Label runat="server" ID="labelExt" Text="extension"></asp:Label>
                                </td>
                                <td nowrap>
                                    <asp:TextBox runat="server" ID="txtBuscar" onkeyup="javascript:onKeyUpBusqueda();"></asp:TextBox>&nbsp;&nbsp;                                
                                    <asp:CheckBox runat="server" ID="chbMostrarObsoletos" Text="Mostrar Obsoletos" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="lbLista" Rows="6" Width="100%"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnModificar" Text="verDatos" OnClick="btnVerDetalle"
                                        CommandName="modificar" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnNuevo" Text="nuevo" OnClick="btnVerDetalle"
                                        CommandName="nuevo" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View runat="server" ID="vDetalle">
                    <fieldset>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <tit:Titulo runat="server" ID="titPopPup"></tit:Titulo>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="labelNoExt" Text="noExtension"></asp:Label>
                                </td>
                                <td>
                                    <asp:Panel runat="server" ID="pnlExistente">
                                        <asp:Label runat="server" ID="lblNumero"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlNuevo">
                                        <asp:TextBox runat="server" ID="txtNumero"></asp:TextBox>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="labelTipo" Text="tipo"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTipo" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <asp:Panel runat="server" ID="pnlInterna">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelExt2" Text="extension"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlExtension" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelFactA" Text="facturadoA"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFacturado" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelNombre" Text="Nombre"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtNombre" Columns="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelVisible" Text="visible"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chbVisibleInt" />
                                    </td>
                                </tr>                                
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelNumDirecto" Text="numeroDirecto"></asp:Label>
                                    </td>
                                    <td nowrap>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:CheckBox runat="server" ID="chkNumDirecto" AutoPostBack="true" TextAlign="Right" />
                                                </td>
                                                <td>
                                                    <asp:Panel runat="server" ID="pnlNumero">
                                                        <asp:DropDownList runat="server" ID="ddlNumero" AppendDataBoundItems="true">
                                                        </asp:DropDownList>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelTipoLinea" Text="tipoLinea"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTipoLinea" AppendDataBoundItems="true" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <asp:Panel runat="server" ID="pnlAlveolo">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="labelAlveolo" Text="alveolo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAlveolo" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlMovil">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelVisible2" Text="visible"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chbVisibleMov" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelNoMovil" Text="noMovil"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMovil" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelPrestamo" Text="Prestamo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chbPrestamo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="labelAsigA" Text="asignadaA"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlAsignarA" AppendDataBoundItems="true" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlAsignarVPersona" runat="server">
                                            <asp:Label runat="server" ID="labelPerso" Text="persona"></asp:Label>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAsignarVExtension" runat="server">
                                            <asp:Label runat="server" ID="labelExt3" Text="extension"></asp:Label>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAsignarVOtros" runat="server">
                                            <asp:Label runat="server" ID="labelOtros" Text="otros"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlAsignarV" runat="server">
                                            <asp:DropDownList runat="server" ID="ddlAsignarV" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlObsoleto" runat="server">
                                 <td colspan="2">
                                    <asp:CheckBox runat="server" ID="chkObsoleto" Text="obsoleto" TextAlign="Left" />                                        
                                </td>
                            </asp:Panel>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" />&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" Visible="false" />
                                    <asp:Button runat="server" ID="btnVolver" Text="volver" />
                                    <cc1:ConfirmButtonExtender ID="cfEliminar" runat="server" TargetControlID="btnEliminar">
                                    </cc1:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:View>
            </asp:MultiView>
            <div style="visibility: hidden;">
                <asp:ListBox runat="server" ID="lbAuxiliar"></asp:ListBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
