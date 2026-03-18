<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_Plantas9.Master" CodeBehind="Default9.aspx.vb" Inherits="AdokWeb._Default9" %>

<%@ MasterType VirtualPath="~/Adok_Plantas9.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cuerpoPrincipal">        
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnEnt">
        <div class="row">
            <br><br><br>
        </div>  
        <div class="row">
                
           <div class="col-xs-8">
            <asp:Label ID="labelIdTrab" runat="server" text="Id o email" CssClass="col-xs-2 negrita hidden-xs"></asp:Label>
            <div class="h3 visible-xs"><span class="glyphicon glyphicon-user text-primary col-xs-2"></span></div>
            <div class="col-xs-10">
                <asp:TextBox runat="server" ID="txtUsuario" TabIndex="1" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ValidationGroup="Login" ErrorMessage="*" CssClass="text-danger"></asp:RequiredFieldValidator>
            </div> 
          </div>  
    </div>


        <div class="row">
            <div class="col-xs-8">
            <asp:Label ID="labelPassword" runat="server" text="Contraseña" CssClass="col-xs-2 negrita hidden-xs"></asp:Label>
            <div class="h3 visible-xs"><span class="glyphicon glyphicon-lock text-primary col-xs-2"></span></div>            
            <div class="col-xs-10">
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" TabIndex="2" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="Login" ErrorMessage="*" CssClass="text-danger"></asp:RequiredFieldValidator>
            </div> 
                 </div>
        </div>
         <div class="row">
            <asp:Label Visible="false" ID="labelPlanta" runat="server" text="Planta" CssClass="col-xs-2 negrita hidden-xs"></asp:Label>
            <div class="h3 visible-xs"><span class="glyphicon glyphicon-globe text-primary col-xs-2"></span></div>            
            <div class="col-xs-10">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlPlantaInfo">
                            <asp:Label Visible="false" ID="lblPlanta" runat="server"></asp:Label>
                            <asp:LinkButton Visible="false" runat="server" ID="lnkSelPlanta" Text="Cambiar" style="margin-left:10px;"></asp:LinkButton>                                
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlSelPlanta">
                            <asp:DropDownList Visible="false" runat="server" ID="ddlPlantas" AutoPostBack="true" CssClass="dropdown form-control"></asp:DropDownList>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>                 
            </div>            
        </div><br />
        <div class="row rowbuttons">
            <div class="col-sm-2 col-md-offset-2"><asp:Button runat="server" ID="btnEnt" text="entrar" ValidationGroup="Login" TabIndex="3" CssClass="btn btn-primary col-xs-12" style="margin:2px;" /></div>
            <div class="col-sm-5"><p><asp:Button Visible="false" runat="server" ID="btnChp" text="cambiarContraseña" CausesValidation="false" CssClass="btn btn-primary col-xs-12" style="margin:2px;" TabIndex="4" data-toggle="modal" /></p></div>
        </div>       
    </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div id="myModal" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><asp:Label runat="server" ID="labelCambio" Text="Cambio password"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                        <asp:Panel runat="server" ID="pnlErrorCP" CssClass="alert alert-danger">
                            <asp:Label runat="server" ID="lblMensajeCP"></asp:Label>
                        </asp:Panel>
                        <div class="row">
                            <asp:Label ID="chpIdTra" runat="server" text="IdTrabajador" CssClass="col-sm-2 negrita"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="chpTxTUsuario" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMUser" runat="server" ControlToValidate="chpTxTUsuario" ValidationGroup="chpGroup" ErrorMessage="*" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>            
                        </div>
                         <div class="row">
                            <asp:Label ID="chpOldPassword" runat="server" text="contraseñaAntigua" CssClass="col-sm-2 negrita"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="chpTxTOPassword" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMOldPass" runat="server" ControlToValidate="chpTxTOPassword" ValidationGroup="chpGroup" ErrorMessage="*" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>            
                        </div>
                         <div class="row">
                            <asp:Label ID="chpTxTNPassword" runat="server" text="contraseñaNueva" CssClass="col-sm-2 negrita"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="TxTNPassword" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMNewPass" runat="server" ControlToValidate="TxTNPassword" ValidationGroup="chpGroup" ErrorMessage="*" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>            
                        </div>
                        <div class="row">
                            <asp:Label ID="chpTxTNPassword2" runat="server" text="repitaLaContraseñaNueva" CssClass="col-sm-2 negrita"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="TxTNPassword2" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMConfPass" runat="server" ControlToValidate="TxTNPassword2" ValidationGroup="chpGroup" ErrorMessage="*" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>            
                        </div>
                         <div class="row">
                            <asp:Label ID="chpPlanta" runat="server" text="Planta" CssClass="col-sm-2 negrita"></asp:Label>
                            <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlPlantasChange" CssClass="dropdown"></asp:DropDownList></div>            
                        </div>
                        <div class="row">    
                            <div class="col-sm-3"></div>
                            <div class="col-sm-3"></div>
                        </div>
                        <asp:CompareValidator ValidationGroup="chpGroup" runat="server" ControlToCompare="TxTNPassword" ControlToValidate="TxTNPassword2" ErrorMessage="No coinciden las contraseñas"></asp:CompareValidator>                         
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="lbCambiar" OnClick="continuaCHP" ValidationGroup="chpGroup" text="Aceptar" CssClass="btn btn-primary"  />
                            <asp:Button runat="server" ID="lbCancelar" CausesValidation="false" text="Cancelar" CssClass="btn btn-primary" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>                                     
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnChp" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>
