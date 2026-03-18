<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Kaplan_Plantas.Master" CodeBehind="ControlP.aspx.vb" Inherits="KaplanNew.ControlP" %>

<%@ MasterType VirtualPath="~/Kaplan_Plantas.Master" %>
  


<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">


     

    
    <asp:Button Visible="false" ID="Button2" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender2" BehaviorID="mpe2" runat="server"
        PopupControlID="pnlPopup2" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup2" runat="server" CssClass="modalPopup" Style="display: none">

         <div class="header">
                   <asp:Label ID="Label2" runat="server" Text="Aviso" />
           
        </div>
        <div class="body">
            <br />
            <br />

            <asp:Label ID="Label3" runat="server" Text="Seleccione la planta a administrar" />

            <br />
            <br />
            
            <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Igorre"  />
            <asp:Button class="btn btn-primary btn-group-vertical" CausesValidation="True"  runat="server" ID="Button3" Text="Zamudio" />
        </div>
        <br />
        <br />
    </asp:Panel>












      <div class="container-fluid" style="background-color:#ebebeb;">
    <div class="navbar-header">
      <a class="navbar-brand" href="#" ><span class="glyphicon glyphicon-globe"></span>&nbsp;KAPLAN</a>
    </div>
  </div>
       <div class="container" >



           

                        <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False"  CssClass="table table-striped"  
                            GridLines="None" OnRowCommand="gvType_OnRowCommand" DataKeyNames="Id" ShowFooter="false"  OnRowEditing="gvType_RowEditing"
                            OnRowCancelingEdit="gvType_RowCancelingEdit" OnRowUpdating="gvType_RowUpdating">
                             <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        <emptydatatemplate>
            ¡No hay clientes con los parámetros seleccionados!  
        </emptydatatemplate> 
       
                            <Columns>
                                   <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">

           <%--                     <asp:BoundField DataField="Id" HeaderText="" Visible="false" />
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">--%>
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                                    </ItemTemplate>
                        
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" />
                                    </ItemTemplate>
                    
                                </asp:TemplateField>

                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbEditar" runat="server" Text="Selección de Planta" CommandName="Edit" />
                                    </ItemTemplate>

                                </asp:TemplateField>

                                
                            </Columns>

                        </asp:GridView>
            


  
                  </div>
</asp:Content>


