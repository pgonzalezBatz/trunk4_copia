<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MPWeb.Master" CodeBehind="PlantasNominas.aspx.vb" Inherits="Nominas.PlantasNominas" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ MasterType VirtualPath="~/MPWeb.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cp" runat="server">
    <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">       
        <asp:View runat="server" ID="vListado">            
            <asp:Label runat="server" id="labelInfo" text="Se establecen los directorios donde se cogeran las nominas para encriptar"></asp:Label><br /><br />
            <asp:LinkButton runat="server" ID="lnkNuevo" text="nuevo"></asp:LinkButton><br /><br />
            <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-condensed"
                 AllowSorting="false" AllowPaging="true" PageSize="15" PagerSettings-Mode="NumericFirstLast">                
                <PagerStyle HorizontalAlign="Center" />
                <PagerSettings PageButtonCount="5" />                
                <EmptyDataTemplate><br />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" text="noExisteNingunRegistro"></asp:Label></EmptyDataTemplate>
                <Columns>                                       
                    <asp:TemplateField HeaderText="Planta">
                        <ItemTemplate><asp:Label runat="server" ID="lblPlanta" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ruta">
                        <ItemTemplate><asp:Label runat="server" ID="lblRuta" /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View runat="server" ID="vDetalle">            
            <div class="row">
                <asp:Label runat="server" ID="labelPlanta" Text="Planta" CssClass="col-sm-1"></asp:Label>
                <div class="col-sm-11">                    
                    <asp:Panel runat="server" ID="pnlNew">                            
                        <asp:DropDownList runat="server" ID="ddlPlantas" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlOld">
                        <strong><asp:Label runat="server" ID="lblPlanta" /></strong>
                    </asp:Panel>
                </div>
            </div>
            <div class="row">
                <asp:Label runat="server" ID="labelRuta" Text="Ruta" CssClass="col-sm-1"></asp:Label>
                <div class="col-sm-11">
                    <asp:TextBox runat="server" ID="txtRuta" MaxLength="100" CssClass="required form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRuta" runat="server" Display="None" ControlToValidate="txtRuta" ValidationGroup="Save" ErrorMessage="Introduzca el dato"></asp:RequiredFieldValidator>
                    <ajax:ValidatorCalloutExtender runat="server" ID="vceRuta" TargetControlID="rfvRuta" />
                </div>
            </div><br />                        
            <div class="form-inline">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="Save" CssClass="btn btn-primary" />                
                <asp:Button runat="server" ID="btnVolver" Text="Volver" CssClass="btn btn-primary" />
                <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" CssClass="btn btn-primary" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
