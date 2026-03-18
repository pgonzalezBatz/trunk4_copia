<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_Plantas.Master" CodeBehind="Default2.aspx.vb" Inherits="AdokWeb._Default2" %>

<%@ MasterType VirtualPath="~/Adok_Plantas.Master" %>
  


<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">

      <div class="container-fluid" style="background-color:#ebebeb;">
    <div class="navbar-header">
      <a class="navbar-brand" href="#" ><span class="glyphicon glyphicon-globe"></span>&nbsp;Selección de Planta</a>
    </div>
  </div>
       <div class="container" >




                <div class=" panel-header">
                    
                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label1" runat="server" Text="Selección de planta" /></h2>

                    
                </div>
                      
                <br />
           

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


