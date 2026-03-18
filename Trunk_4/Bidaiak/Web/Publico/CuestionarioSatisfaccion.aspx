<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master.Master" CodeBehind="CuestionarioSatisfaccion.aspx.vb" Inherits="WebRaiz.CuestionarioSatisfaccion" %>
<%@ MasterType VirtualPath="~/Master.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="server">
    <asp:MultiView runat="server" ID="mv">
        <asp:View runat="server" ID="vCuestionario">
            <div class="form-group">
                <asp:Label runat="server" ID="labelInfo"></asp:Label>
            </div>
            <div class="form-group">
                <asp:Label runat="server" ID="labelQuestion1"></asp:Label>
                <div class="checkboxlist col-sm-12">
                    <asp:Repeater runat="server" ID="rptAnswer1">
                        <ItemTemplate>
                            <div class="col-sm-12"><asp:Radiobutton runat="server" ID="rbAnswer" /></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>        
            </div>
            <div class="form-group">
                <asp:Label runat="server" ID="labelComenQuestion1" Text="Puede añadir algunos comentarios si lo desea"></asp:Label><br />
                <asp:TextBox runat="server" ID="txtComenQuestion1" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server" ID="labelQuestion2"></asp:Label>
                <div class="checkboxlist col-sm-12">
                    <asp:Repeater runat="server" ID="rptAnswer2">
                        <ItemTemplate>
                            <div class="col-sm-12"><asp:Radiobutton runat="server" ID="rbAnswer" /></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>        
            </div>
            <div class="form-group">
                <asp:Label runat="server" ID="labelQuestion3" Text="3. Si has tenido que ponerte en contacto con la agencia de viajes por algun incidente durante el viaje, ¿como lo han solucionado?"></asp:Label>
                <div class="checkboxlist col-sm-12">
                    <asp:Repeater runat="server" ID="rptAnswer3">
                        <ItemTemplate>
                            <div class="col-sm-12"><asp:Radiobutton runat="server" ID="rbAnswer" /></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>        
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="form-control btn btn-primary" />
            </div>
        </asp:View>
        <asp:View runat="server" ID="vResul">
            <div runat="server" id="divResul">
                <b><asp:label runat="server" ID="labelMensaje"></asp:label></b>
            </div>
        </asp:View>
    </asp:MultiView>    
</asp:Content>
