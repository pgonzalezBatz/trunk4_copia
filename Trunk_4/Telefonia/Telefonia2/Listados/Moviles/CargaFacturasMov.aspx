<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Mantenimientos/MPTelefoniaAdm.Master"
    CodeBehind="CargaFacturasMov.aspx.vb" Inherits="Telefonia.CargaFacturasMov" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Titulo.ascx" TagName="Titulo" TagPrefix="tit" %>
<%@ MasterType VirtualPath="~/Mantenimientos/MPTelefoniaAdm.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <script src="../../js/Utiles.js" type="text/javascript"></script>

    <asp:MultiView runat="server" ID="mvCarga" ActiveViewIndex="0">
        <asp:View runat="server" ID="vSubirFich">
            <tit:Titulo runat="server" Texto="procesoCargaFacturasMoviles" />            
            <fieldset style="width:650px">
            <br />           
            <div>
                1.
                <asp:Label runat="server" ID="labelSelFichero" text="seleccioneFichero"></asp:Label>
                :
                <asp:FileUpload ID="fuFichero" runat="server" />
                &nbsp; (<asp:Label runat="server" ID="labelTamano" text="Tamaño maximo 20 Megas" CssClass="font11"></asp:Label>)
                <br />
            </div>
            <br />
            <div align="center">                
                <asp:Button runat="server" ID="btnSubir" text="subirFichero" /> &nbsp;
                <asp:Label runat="server" ID="lblNoSubir" Text="Se ha detectado que una factura se ha quedado en el directorio Temp. Consulte con el administrador" Visible="false"></asp:Label><br /><br />
                <asp:Button runat="server" ID="btnQuitarBloqueo" text="Quitar bloqueo" Visible="false" />
            </div>
            </fieldset>
        </asp:View>
        <asp:View runat="server" ID="vImportar">
            <tit:Titulo runat="server" Texto="procesoImportacionDatos" />
            <fieldset style="width:650px">            
                <div>
                    <asp:Label runat="server" ID="labelFich" text="fichero"></asp:Label>&nbsp;
                    <asp:Label runat="server" ID="lblFichero" CssClass="negrita"></asp:Label>&nbsp;
                    <asp:Label runat="server" ID="labelSubidoOK" text="subidoCorrectamente"></asp:Label>
                </div><br />
                <div>
                    <asp:Label runat="server" ID="labelResumen" Text="Resumen de las facturas que se van a importar"></asp:Label><br /><br />                    
                    <asp:GridView runat="server" ID="gvFacturas" AutoGenerateColumns="false" ShowFooter="true" CssClass="GridView" Width="75%">
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                        <RowStyle CssClass="GridViewRowStyle" />
                        <EmptyDataRowStyle CssClass="GridViewEmptyRowStyle" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <br />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>                            
                            <asp:TemplateField HeaderText="CIF">                                
                                <ItemTemplate><asp:Label ID="lblCif" runat="server" Text='<%#Eval("CifEmpresa")%>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Empresa">                                
                                <ItemTemplate><asp:Label ID="lblEmpresa" runat="server" Text='<%#Eval("NombrePlanta")%>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Num factura">                                
                                <ItemTemplate><asp:Label ID="lblNumFactura" runat="server" Text='<%#Eval("NumFactura")%>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total a pagar">                                
                                <ItemTemplate><asp:Label ID="lblTotalPagar" runat="server" Text='<%#Eval("TotalPagar")%>'></asp:Label></ItemTemplate>
                                <FooterTemplate><asp:Label runat="server" ID="lblTotal" style="font-weight:bold;"></asp:Label></FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView><br />
                </div>
                <asp:Panel runat="server" ID="pnlImportar">
                    <asp:Button runat="server" ID="btnImportar" text="Importar las facturas" style="font-size:18px;" /><br /><br />
                    <asp:Label runat="server" CssClass="font11" ID="lblMensa" text="Esperar hasta que se visualice un mensaje que indique que ha finalizado. Este proceso puede tardar algun minuto y se vera su avance en la barra de progreso de la pagina"></asp:Label>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlNoImportar">
                    <asp:Label runat="server" style="font-size:18px;font-weight:bold;" ID="labelAsociarCIF" text="Hay empresas sin asociar el CIF. Vaya a SAB-Plantas y asocie el CIF con la planta adecuada o creela en caso necesario. Despues, vuelva e ejecutar el proceso"></asp:Label>
                </asp:Panel>
           </fieldset>
        </asp:View>                 
        <asp:View runat="server" ID="mvResumen">                
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="labelFFactura" text="Fecha factura"></asp:Label>:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblFechaFactura"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="labelTotalFactura" text="Total factura"></asp:Label>:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblTotalFactura"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="labelTotalFacturado" text="Total facturado"></asp:Label>:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblTotalFacturado"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="labelRegInsert" text="Lineas insertados"></asp:Label>:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblRegistrosInsertados"></asp:Label>
                    </td>
                </tr>
            </table><br /><br />
            <asp:Label runat="server" ID="labelInfo" text="Pulse el boton para preparar los datos para que sea posible su visualizacion en Cognos y actualizacion de los costes"></asp:Label><br /><br />
            <asp:Button runat="server" ID="btnRefrescar" text="Preparar datos Cognos y Costes" style="margin-left:10px;" />&nbsp;&nbsp;
            <asp:Label runat="server" CssClass="font11" ID="lblUltimaEjecucion"></asp:Label>
            <asp:Panel runat="server" ID="pnlResulRefresco">
                <br /><br />
                <asp:Label runat="server" ID="lblResulRefresco" CssClass="Titulo" Text="En cuanto termine el proceso se le avisara por email para que pueda consultar los informes de Cognos. Puede tardar varios minutos..."></asp:Label>
            </asp:Panel>
            <asp:Panel runat="server" id="pnlError" />         
          </asp:View>                         
    </asp:MultiView>
</asp:Content>
