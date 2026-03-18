<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="menuempleado.aspx.vb" Inherits="Web.menuempleado" MasterPageFile="~/MPLogin.Master"%>
<%@ MasterType VirtualPath="~/MPLogin.Master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContenido" class="data" >    
    <asp:Panel runat="server" ID="pnlAlertas"></asp:Panel> 
    <div runat="server" ID="pnlRecursos" class="panel panel-default">
        <div class="panel-heading"  style="height:auto">
            <h4 class="panel-title">
				<a runat="server" id="aTxokoaTitle"><%= itzultzaileWeb.Itzuli("Portal del empleado") %></a>
			</h4>
        </div>
        <div class="panel-body text-center">

            <div runat="server" class="hidden-xs hidden-sm">
                            <asp:Repeater runat="server" ID="rptRec">
                                <ItemTemplate>
                                    <div class="col-sm-2 col-md-2 text-center">
                                        <div class="panel panel-default">
                                            <div class="panel-body" style="height:50px;padding:1px">
                                                <a runat="server" id="aBody" style="display:block;overflow:hidden;width:100%;height:100%;">
                                                    <asp:Image runat="server" ID="imgRecurso" />
                                                </a>
                                            </div>
                                            <div id="tituloRecursoSmMd" runat="server" class="panel-heading" style="height:45px;">
                                                <a runat="server" id="aHeading" class="anchorRecurso" style="display:block;overflow:hidden;width:100%;height:100%;">
                                                    <asp:Label runat="server" ID="lblRecurso" class="labelRecurso"></asp:Label>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            
            </div>
            <div runat="server" class="visible-xs visible-sm hidden-md text-uncenter">
                                <asp:Repeater runat="server" ID="rptRecMovil">
                                    <ItemTemplate>
                                        <div class="col12 col-xs-12" style="padding:0px;">
                                                <a runat="server" ID="aBody" style="overflow:hidden;width:16%;margin-left:5%;">
                                                    <asp:Image runat="server" ID="imgRecurso" class="resize" />                                                        
                                                </a>      
                                                <a runat="server" ID="aHeading" style="display:inline;overflow:hidden;width:84%;margin-left:5%;font-size:18px">  
                                                    <asp:Label runat="server" ID="lblRecurso" CssClass="itemTitle"></asp:Label>
                                                </a> 

                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>


            </div>
        </div>
    </div>
    <div class="panel panel-default" runat="server" id="divNikEuskaraz">
        <div class="panel-heading">
            <h4 class="panel-title">
                <a runat="server" ID="aNikEuskaraz">"Nik euskaraz"</a>
            </h4>
        </div>
        <div class="panel-body">
            <div class="row hidden-xs hidden-sm text-center">
                <div class="col-sm-12"><asp:image runat="server" ID="imgEuskaraz" ImageUrl="~/App_Themes/Tema1/Images/nikEuskaraz.png" /></div>
            </div>
            <div class="form-inline text-center">
                <div class="form-group">
                    <asp:CheckBox runat="server" ID="chbEuskaraz" Text="Nik euskaraz" AutoPostBack="true" CssClass="negrita" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblInfoEuskaraz" Text="texto informativo Nik Euskaraz" style="margin-left:20px"></asp:Label>                    
                </div>
            </div>
        </div>        
     </div>
    <asp:Button runat="server" ID="btnAction" CssClass="action" style="visibility:hidden" />                     
</asp:Content>