<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Kaplan_plantas.Master" CodeBehind="CrearCausas.aspx.vb" Inherits="KaplanNew.CrearCausas" %>

<%@ MasterType VirtualPath="~/Kaplan_plantas.Master" %>


<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">


    <script type="text/javascript">




        function ValidateFecha(source, args) {

            var fecha = args.Value;

            //var fechaf = fecha.split("/");
            //var d = fechaf[0];
            //var m = fechaf[1];
            //var y = fechaf[2];
            //args.IsValid = m > 0 && m < 13 && y > 0 && y < 32768 && d > 0 && d <= (new Date(y, m, 0)).getDate();

            var fechaArr = fecha.split('/');
            var dia = fechaArr[0];
            var mes = fechaArr[1];
            var aho = fechaArr[2];

            var plantilla = new Date(aho, mes - 1, dia);//mes empieza de cero Enero = 0

            if (!plantilla || plantilla.getFullYear() == aho && plantilla.getMonth() == mes - 1 && plantilla.getDate() == dia) {
                args.IsValid = true;
                return true;
            } else {
                args.IsValid = false;
                return false;
            }




        }




        function ValidateModeNumber(source, args) {
            return;
        }


        function ValidarAltaReferencia() {

            return true;
        }




        function ConditionsPreviousBatzNumberValid() {
            return true;

        }

        function ConditionsDevelopment() {
            var modeNumbers = GetSelectedModeNumbers();
            if (modeNumbers.indexOf("3") > -1) {
                return true;
            }
            else {
                return false;
            }
        }


        function GetSelectedModeNumbers() {
            var modosSeleccionados = "";

            return modosSeleccionados;
        };


        function SelectModeNumber() {


        }


        function RecogerEmpresa(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresa.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }

        function RecogerEmpresaXBAT(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresaXBAT.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }
        function RecogerEmpresaXBATNombre(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresaXBATNombre.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }
        function RecogerEmpresaXBATCIF(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresaXBATCIF.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">

    <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label3" runat="server" Text="CAUSAS" /></a>
        </div>
    </div>

    <div class="container ">



        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">

            <asp:View ID="viewListado" runat="server">


                <%--                <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblEmpresa" runat="server" Text="Mantenimiento de empresas" /></h2>


                </div>--%>
                <br />
                <div class="row">
                    <div class="col-xs-3">
                        <asp:Label ID="Label1" runat="server" Text="Causa" />

                        <asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarEmpresas"
                            runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfEmpresa" runat="server" />

                    </div>

                </div>

                <br />



                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="true"  PageSize="990"
                    GridLines="None" OnRowCommand="gvType_OnRowCommand" DataKeyNames="Id" ShowFooter="false" OnDataBound="gvType_DataBound"
                    OnRowCancelingEdit="gvType_RowCancelingEdit" OnRowEditing="gvType_RowEditing" OnRowUpdating="gvType_RowUpdating">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc2" runat="server" CssClass="label label-warning" Text="¡No hay causas a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
                    <PagerTemplate>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-1" style="text-align: right;">
                                <%--   <h4>   <span class="label label-info">Ir a la página.</span></h4>--%>
                                <h5>
                                    <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                            </div>
                            <div class="col-lg-1" style="text-align: left;">
                                <asp:DropDownList ID="PageDropDownList" Width="70px" AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
                            </div>
                            <div class="col-lg-8" style="text-align: right;">
                                <h4>
                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                            </div>
                        </div>
                    </PagerTemplate>
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="" ItemStyle-Width="90px">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" Width="1px" Style="color:white;" />
                            </ItemTemplate>
                        </asp:TemplateField>

                      <asp:TemplateField HeaderText="Process" ItemStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkProces" Text='<%# Eval("textolibre")%>' Font-Underline="true" runat="server" ToolTip="Modify Data" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Step" ItemStyle-Width="30%" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDescripcion" Text='<%# Eval("Nombre")%>' Font-Underline="true" runat="server" ToolTip="Modify Data" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Work" ItemStyle-Width="30%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkWork" Text='<%# Eval("textolibre2")%>' Font-Underline="true" runat="server" ToolTip="Modify Data" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Modify Data">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" runat="server" Text="Modify Data" class="btn  bg-info" ToolTip="Modify Data" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>






                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Borrar" Visible='<%# If(Eval("obsoleto") = 0, "true", "false") %>' />
                                <act:ConfirmButtonExtender ID="cbeEliminar3" runat="server" DisplayModalPopupID="mpeEliminar3"
                                    TargetControlID="btnEliminar3">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar3" runat="server" PopupControlID="pnlConfirmDelete2" TargetControlID="btnEliminar3" OkControlID="btnBorrar3"
                                    CancelControlID="btnCancelar3" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete2" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion3" runat="server" Text="Confirmation" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado3" runat="server" Text="Are you sure?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar3" runat="server" CssClass="btn btn-primary" Text="Yes" />
                                        <asp:Button ID="btnCancelar3" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>




                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Activar" Text="Activate" Visible="false" />
                                <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="btnEliminar2">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminar2" OkControlID="btnBorrar2"
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Confirmation" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text="Are you sure?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar2" runat="server" CssClass="btn btn-primary" Text="Yes" />
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>




                    </Columns>


                </asp:GridView>


            </asp:View>







            <asp:View ID="view1" runat="server">
                <%--           <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="Mantenimiento de datos" /></h2>

                </div>


          
                <br />--%>
                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">


                             <asp:Label Visible="false" runat="server" ID="Label2" Text="Process" />

                                    <asp:DropDownList Visible="false" ID="DdlWork" runat="server" class="form-control" ReadOnly="True">
                                    </asp:DropDownList>



                            <div class="row">
                                <div class="col-xs-6">
                                    <asp:Label  runat="server" ID="ex1e" Text="Modo Fallo"  />

                                    <asp:DropDownList Visible="false" ID="DdlSteps" runat="server" class="form-control" ReadOnly="True">
                                    </asp:DropDownList>

                                    <asp:TextBox  class="form-control" type="text" ID="NomEmp" runat="server"  />
                                </div>
                                 <div class="col-xs-2">
                                    <br />
                                <asp:LinkButton Visible="false" ID="imgEstado1" Text="L" CommandName="Extranet"  runat="server" ToolTip="Low"
                                    class="btn alert-success"></asp:LinkButton>
                                                                     <asp:LinkButton Visible="false" ID="imgEstado2" Text="M" CommandName="Extranet"  runat="server" ToolTip="Medium"
                                    class="btn alert-warning"></asp:LinkButton>
                                                                     <asp:LinkButton Visible="false" ID="imgEstado3" Text="H" CommandName="Extranet"  runat="server" ToolTip="High"
                                    class="btn alert-danger"></asp:LinkButton>
                                 </div>
                            </div>
               <br />


         <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><asp:Label ID="Label7ss" runat="server" Text="Efecto" /></a>
        </div>
    </div>

<br />
                        
                            
                                  <div class="row">
						     <div class="col-xs-1">
        
                                </div>
                                <div class="col-xs-3">
                                    <asp:Label  runat="server" ID="ex1ef" Text="Definir Efecto Planta:"  />
                                </div>
                              <div class="col-xs-5">
                                    <asp:TextBox  class="form-control" type="text" ID="NomEmpf" runat="server"  />
                              </div>
								 <div class="col-xs-2">
                                              <asp:DropDownList ID="DdlPrimero" runat="server" class="form-control" >
													   <asp:ListItem Value="1">1</asp:ListItem>
											     <asp:ListItem Value="2">2</asp:ListItem>
                                                 <asp:ListItem Value="3">3</asp:ListItem>
                                                  				   <asp:ListItem Value="4">4</asp:ListItem>
											     <asp:ListItem Value="5">5</asp:ListItem>
                                                 <asp:ListItem Value="6">6</asp:ListItem>
                                                  				   <asp:ListItem Value="7">7</asp:ListItem>
											     <asp:ListItem Value="8">8</asp:ListItem>
                                                 <asp:ListItem Value="9">9</asp:ListItem>
                                                   <asp:ListItem Value="10">10</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>       
                            
                                                     <div class="row">
						     <div class="col-xs-1">
        
                                </div>
                                <div class="col-xs-3">
                                    <asp:Label  runat="server" ID="ex1ef2" Text="Definir Efecto Cliente:"  />
                                </div>
                              <div class="col-xs-5">
                                    <asp:TextBox  class="form-control" type="text" ID="NomEmpf2" runat="server"  />
                              </div>
								 <div class="col-xs-2">
                                              <asp:DropDownList ID="DdlSegundo" runat="server" class="form-control" >
											   <asp:ListItem Value="1">1</asp:ListItem>
											     <asp:ListItem Value="2">2</asp:ListItem>
                                                 <asp:ListItem Value="3">3</asp:ListItem>
                                                  				   <asp:ListItem Value="4">4</asp:ListItem>
											     <asp:ListItem Value="5">5</asp:ListItem>
                                                 <asp:ListItem Value="6">6</asp:ListItem>
                                                  				   <asp:ListItem Value="7">7</asp:ListItem>
											     <asp:ListItem Value="8">8</asp:ListItem>
                                                 <asp:ListItem Value="9">9</asp:ListItem>
                                                   <asp:ListItem Value="10">10</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>  


                                                 <div class="row">
						     <div class="col-xs-1">
        
                                </div>
                                <div class="col-xs-3">
                                    <asp:Label  runat="server" ID="Label6" Text="Definir Efecto Usuario Final:"  />
                                </div>
                              <div class="col-xs-5">
                                    <asp:TextBox  class="form-control" type="text" ID="NomEmpf3" runat="server"  />
                              </div>
								 <div class="col-xs-2">
                                              <asp:DropDownList ID="DdlTercero" runat="server" class="form-control" >
											   <asp:ListItem Value="1">1</asp:ListItem>
											     <asp:ListItem Value="2">2</asp:ListItem>
                                                 <asp:ListItem Value="3">3</asp:ListItem>
                                                  				   <asp:ListItem Value="4">4</asp:ListItem>
											     <asp:ListItem Value="5">5</asp:ListItem>
                                                 <asp:ListItem Value="6">6</asp:ListItem>
                                                  				   <asp:ListItem Value="7">7</asp:ListItem>
											     <asp:ListItem Value="8">8</asp:ListItem>
                                                 <asp:ListItem Value="9">9</asp:ListItem>
                                                   <asp:ListItem Value="10">10</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div> 


                            <br />




                            <asp:Label Visible="false" runat="server" ID="lblNombre" Text="Control en Prevención" />

                            <asp:TextBox  Visible="false"  type="text" class="form-control" ID="CifEmp" runat="server"/>

                           

                                 <asp:Label Visible="false" runat="server" ID="Label4" Text="Work" />

                            <asp:DropDownList Visible="false"  ID="DdlSubcontrata" runat="server" class="form-control">
                            </asp:DropDownList>

            

                    <asp:Label Visible="false" runat="server" ID="Label5" Text="Control en Producción" />

                            <asp:TextBox  Visible="false" type="text" class="form-control" ID="DescWork" runat="server"/>





                            <br />
                                      <hr />

         <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><asp:Label ID="Label7ww2" runat="server" Text="Causa" /></a>
        </div>
    </div>

<br />

                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped" 
                    GridLines="None" OnRowCommand="gvType2_OnRowCommand"  DataKeyNames="id" ShowFooter="false"  PageSize="990">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay causas a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
                    <PagerTemplate>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-1" style="text-align: right;">

                                <h5>
                                    <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                            </div>
                            <div class="col-lg-1" style="text-align: left;">
                                <asp:DropDownList ID="PageDropDownList" Width="70px" AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
                            </div>
                            <div class="col-lg-8" style="text-align: right;">
                                <h4>
                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                            </div>
                        </div>
                    </PagerTemplate>
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="" visible="false" />

                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("id")%>' runat="server" Width="1px" Style="color:white;"/>
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Calificación" >
                            <ItemTemplate>
                                <asp:Label ID="lblCaracte" Text='<%# Eval("Textolibre2")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Causa" >
                            <ItemTemplate>
                                <asp:Label ID="lblCaractexx" Text='<%# Eval("Condicion1")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Control en Prevención"  >
                            <ItemTemplate>
                                <asp:Label ID="txtDescripcionw" Text='<%# Eval("Condicion2")%>' runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>

                   <asp:TemplateField  HeaderText="Ocurrencia"   >
                            <ItemTemplate>
                        
                                              <asp:Label ID="DdlidTipow" runat="server" Text='<%# Eval("Work")%>' >
								<%--			   
											   <asp:ListItem Value="1">1</asp:ListItem>
											     <asp:ListItem Value="2">2</asp:ListItem>
                                                 <asp:ListItem Value="3">3</asp:ListItem>
                                                  				   <asp:ListItem Value="4">4</asp:ListItem>
											     <asp:ListItem Value="5">5</asp:ListItem>
                                                 <asp:ListItem Value="6">6</asp:ListItem>
                                                  				   <asp:ListItem Value="7">7</asp:ListItem>
											     <asp:ListItem Value="8">8</asp:ListItem>
                                                 <asp:ListItem Value="9">9</asp:ListItem>
                                                   <asp:ListItem Value="10">10</asp:ListItem>--%>
                                    </asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>


    
                          <asp:TemplateField HeaderText="Tipo" Visible="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblClase" Text='<%# Eval("referencia")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>

                  <asp:TemplateField HeaderText="Max."  Visible="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblMax" Text='<%# Eval("Max")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                  <asp:TemplateField HeaderText="Min."  Visible="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblMin" Text='<%# Eval("Min")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


     



                        <asp:TemplateField  Visible="false">
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-success" runat="server" CommandName="Desactivar" Text="C.P."  />
                                <act:ConfirmButtonExtender ID="cbeEliminar3" runat="server" DisplayModalPopupID="mpeEliminar3"
                                    TargetControlID="btnEliminar3">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar3" runat="server" PopupControlID="pnlConfirmDelete2" TargetControlID="btnEliminar3" OkControlID="btnBorrar3"
                                    CancelControlID="btnCancelar3" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete2" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion3" runat="server" Text="Control en Producción" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado3" runat="server" Text="En desarrollo" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar3" runat="server" CssClass="btn btn-primary" Text="Si" />
                                        <asp:Button ID="btnCancelar3" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>




                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Activar" Text="Borrar"  />
                                <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="btnEliminar2">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminar2" OkControlID="btnBorrar2"
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Confirm" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text="Are you sure?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar2" runat="server" CssClass="btn btn-primary" Text="Yes" />
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>








                    </Columns>


                </asp:GridView>






                    






                               <p style="text-align: center;">
                       <asp:LinkButton ID="btnQuitarSeleccionados" runat="server" CssClass="btn btn-default btn-info"><span class="glyphicon glyphicon-pushpin"></span>&nbsp;<asp:Label ID="Label53" runat="server" Text="Añadir Causa" /></asp:LinkButton></p><act:ConfirmButtonExtender ID="cbeEliminar" runat="server" DisplayModalPopupID="mpeEliminar" TargetControlID="btnQuitarSeleccionados"></act:ConfirmButtonExtender>
                <act:ModalPopupExtender ID="mpeEliminar" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnQuitarSeleccionados" OkControlID="btnGrav"
                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                </act:ModalPopupExtender>
                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        <asp:Label ID="lblConfirmacion" runat="server" Text="Causa" />
                    </div>
                    <div class="body">
                      <div class="col-xs-1">

                       <asp:Label Visible="false" runat="server" ID="Label8" Text="Clase" />  <br />

                                    <asp:DropDownList Visible="false" ID="TipoC" runat="server" class="form-control" ControlStyle-CssClass="BatzFont">
                                    </asp:DropDownList>
                             </div>
             
              

                            <div class="col-xs-10">
                                  <br />
                                 <asp:Label  runat="server" ID="Label9" Text="Causa" />
                                         <asp:DropDownList visible="false"  ID="ClaseC" runat="server" class="form-control" >
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlCausas2" runat="server" class="form-control"  AutoPostBack="False">
     
                                    </asp:DropDownList>
                           <asp:Label visible="false" runat="server" ID="Label10" Text="Descripción" /> 
                 <asp:TextBox  visible="false"  ID="Car1"  runat="server" class="form-control"  />

                                </div>
                           <div class="col-xs-2">
                           -    
                               </div>
                 <div class="col-xs-12">
                      </div>  


                        <br />


           <div class="col-xs-8">

                       <asp:Label runat="server" ID="Label7" Text="Control en Prevención" />:  

                                 <asp:TextBox ID="txtCP"  runat="server" class="form-control"  />
                             </div>


           
                            <div class="col-xs-4">
                                <br />
    <asp:Label runat="server" ID="Label12ocu" Text="Ocurrencia" />:  
                                
                                          
                          <asp:DropDownList ID="ddlOcu" runat="server"  >
											   <asp:ListItem Value="1">1</asp:ListItem>
											     <asp:ListItem Value="2">2</asp:ListItem>
                                                 <asp:ListItem Value="3">3</asp:ListItem>
                                                  				   <asp:ListItem Value="4">4</asp:ListItem>
											     <asp:ListItem Value="5">5</asp:ListItem>
                                                 <asp:ListItem Value="6">6</asp:ListItem>
                                                  				   <asp:ListItem Value="7">7</asp:ListItem>
											     <asp:ListItem Value="8">8</asp:ListItem>
                                                 <asp:ListItem Value="9">9</asp:ListItem>
                                                   <asp:ListItem Value="10">10</asp:ListItem>
                                    </asp:DropDownList>
      

                                </div>
                  

      <div class="col-xs-12">
                      </div>  

                         <br />  <br />
   
  <%--                          <div class="col-xs-4">

        <asp:Label  runat="server" ID="Label12" Text="Control en Producción" /> 
                                <asp:Button  ID="btnControlP" runat="server" CssClass="btn btn-primary" Text="Control en Producción"  />
                 <asp:TextBox Visible="false"  ID="txtMax"  runat="server" class="form-control"     />
  <asp:Label  runat="server" ID="Label776" Text="" /> 
         </div>--%>


                    </div>

                            <div class="col-xs-12">
                 <asp:Label visible="false"   runat="server" ID="Label11" Text="Control en Prevención" />
                 <asp:TextBox visible="false"  ID="car11"  runat="server" class="form-control"  />


            </div>
                    <div class="footer" style="text-align: center;">
                              <div class="col-xs-12">
                      </div>  
                                            <div class="footer" style="text-align: center;">
                              <div class="col-xs-12">
                      </div>  <br />
                                          <div  style="text-align: center;">
                          <asp:Label  runat="server" ID="Label336" Text="" /> 
                         <br /><br />
                   </div>
                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="Cancelar" />
                        <asp:Button ID="btnGrav" runat="server" CssClass="btn btn-primary" Text="Grabar" OnClick="btnGrav_Click"  UseSubmitBehavior="true"  />
          
                    </div>


                                               <div class="col-xs-4">

               <asp:Label Visible="false"  runat="server" ID="Label13" Text="Min" />
                 <asp:TextBox Visible="false" ID="txtMin"  runat="server" class="form-control"  placeholder="n,nnn"   />


                        <br />
                                  <br />


                        
  </div> 
                          </div> 
                </asp:Panel>




<br /><br />
                                     <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><asp:Label ID="Label7yy" runat="server" Text="Control en Detección" /></a>
        </div>
    </div>


                            <br />

                 

                <asp:GridView ID="gvType3" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped"
                    GridLines="None" OnRowCommand="gvType3_OnRowCommand"  DataKeyNames="id" ShowFooter="false"  PageSize="990">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay control a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
                    <PagerTemplate>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-1" style="text-align: right;">

                                <h5>
                                    <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                            </div>
                            <div class="col-lg-1" style="text-align: left;">
                                <asp:DropDownList ID="PageDropDownList" Width="70px" AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
                            </div>
                            <div class="col-lg-8" style="text-align: right;">
                                <h4>
                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                            </div>
                        </div>
                    </PagerTemplate>
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="" visible="false" />

                              <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("id")%>' runat="server" Width="1px" Style="color:white;"/>
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Método Control" >
                            <ItemTemplate>
                                <asp:Label ID="lblCaracte" Text='<%# Eval("Condicion1")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Nivel Control" >
                            <ItemTemplate>
                                <asp:Label ID="lblTipo" Text='<%# Eval("Condicion2")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Detectabilidad" >
                            <ItemTemplate>
                                           <asp:Label ID="lblTipoCaus" Text='<%# Eval("Work")%>' runat="server" />
                               
                            </ItemTemplate>
                            
                        </asp:TemplateField>


              


        



                        

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Disable" Visible='<%# If(Eval("obsoleto") = 0, "true", "false") %>' />
                                <act:ConfirmButtonExtender ID="cbeEliminar3" runat="server" DisplayModalPopupID="mpeEliminar3"
                                    TargetControlID="btnEliminar3">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar3" runat="server" PopupControlID="pnlConfirmDelete2" TargetControlID="btnEliminar3" OkControlID="btnBorrar3"
                                    CancelControlID="btnCancelar3" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete2" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion3" runat="server" Text="Confirmation" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado3" runat="server" Text="Are you sure?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar3" runat="server" CssClass="btn btn-primary" Text="Yes" />
                                        <asp:Button ID="btnCancelar3" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>




                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Activar" Text="Borrar" Visible='<%# If(Eval("obsoleto") = 0, "false", "true") %>' />
                                <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="btnEliminar2">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminar2" OkControlID="btnBorrar2"
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Confirm" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text="Are you sure?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar2" runat="server" CssClass="btn btn-primary" Text="Yes" />
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>







                    </Columns>


                </asp:GridView>










                           <p style="text-align: center;">
                       <asp:LinkButton  ID="btnQuitarSeleccionadosx" runat="server" CssClass="btn btn-default btn-info"><span class="glyphicon glyphicon-pushpin"></span>&nbsp;<asp:Label ID="Label14" runat="server" Text="Añadir Control" /></asp:LinkButton></p><act:ConfirmButtonExtender ID="cbeEliminarx" runat="server" DisplayModalPopupID="mpeEliminarx" TargetControlID="btnQuitarSeleccionadosx"></act:ConfirmButtonExtender>
                <act:ModalPopupExtender ID="mpeEliminarx" runat="server" PopupControlID="pnlConfirmDeletex" TargetControlID="btnQuitarSeleccionadosx" OkControlID="btnBorrarx"
                    CancelControlID="btnCancelar2x" BackgroundCssClass="modalBackground">
                </act:ModalPopupExtender>
                <asp:Panel ID="pnlConfirmDeletex" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        <asp:Label ID="Label17x" runat="server" Text="Control en Detección" />
                    </div>
                    <div class="body">
                      <div class="col-xs-8">

                       <asp:Label runat="server" ID="Label18x" Text="Método Control" />:  

                                 <asp:TextBox ID="TrabajoSTD"  runat="server" class="form-control"  />
                             </div>


           
                            <div class="col-xs-4">
                                <br />
    <asp:Label runat="server" ID="Label7ff" Text="Detectabilidad" />:  
                                
                                          
                          <asp:DropDownList ID="ddlcontrol" runat="server"  >
											   <asp:ListItem Value="1">1</asp:ListItem>
											     <asp:ListItem Value="2">2</asp:ListItem>
                                                 <asp:ListItem Value="3">3</asp:ListItem>
                                                  				   <asp:ListItem Value="4">4</asp:ListItem>
											     <asp:ListItem Value="5">5</asp:ListItem>
                                                 <asp:ListItem Value="6">6</asp:ListItem>
                                                  				   <asp:ListItem Value="7">7</asp:ListItem>
											     <asp:ListItem Value="8">8</asp:ListItem>
                                                 <asp:ListItem Value="9">9</asp:ListItem>
                                                   <asp:ListItem Value="10">10</asp:ListItem>
                                    </asp:DropDownList>
      

                                </div>



                         <br />  
          



                        
                           <div class="col-xs-12">

            




                              
                            <div class="form-inline row">
                                <div class="form-group col-sm-12">
                                     <br />
                                    <asp:Label runat="server" ID="lblPlantillaC"  Text="Nivel Control (Fallo o Causa):" />
                                    <br />

                                    <asp:DropDownList ID="ddlPlantilla" runat="server" class="form-control" Width="15%" AutoPostBack="False">
                                        <asp:ListItem Text="" Value="0" />
                                        <asp:ListItem Text="Modo Fallo" Value="1" />
                                    </asp:DropDownList>


                                    &nbsp;
                                                &nbsp; 
                                                                        

                                      <%--OnClick="ponerPanel"--%>

                                    <%--<%if (ddlPlantilla.SelectedIndex > 0) Then  %>--%>

                          <asp:DropDownList ID="ddlCausas" runat="server" class="form-control" Width="25%" AutoPostBack="False">
                                        </asp:DropDownList>



                                    <%--<%end If%>--%>

          
                                </div>

                            </div>










                               </div>  <br />

            

                    </div>  <br />

                       <div class="col-xs-12">
                           <br />
                     </div>
                    <div class="footer" style="text-align: center;">
                        <asp:Button ID="btnCancelar2x" runat="server" CssClass="btn btn-primary" Text="Volver" />
                        <asp:Button    ID="btnBorrarx" runat="server" CssClass="btn btn-primary" Text="Grabar" AutoPostBack="false" />

                    </div>
                </asp:Panel>













                        </div>
                    </div>



                </div>

                <div class="panel-footer">
                    <div class="text-center">
                        <asp:HiddenField runat="server" ID="flag_Modificar" />
                        <asp:HiddenField runat="server" ID="HdnNombre" />
                        <asp:HiddenField runat="server" ID="HdnCIF" />
                        <asp:HiddenField runat="server" ID="txtFechaSol2" />
                        <asp:HiddenField runat="server" ID="txtFechaEnv2" />
                        <asp:HiddenField runat="server" ID="txtFechaRec2" />
                        <asp:HiddenField runat="server" ID="TxtMedio22" />
                        <asp:HiddenField runat="server" ID="TxtMediob" />

                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-primary" Text="Volver" UseSubmitBehavior="false" />

                        <asp:Button  ID="btnGuardarNuevaSolicitud" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia_Click" Text="Save" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />

                        <asp:Button ID="btnLimpiarCampos" Visible="false" runat="server" CausesValidation="false" class="btn btn-primary" OnClick="btnLimpiarCampos_Click" Text="Limpiar campos" />

                    </div>
                </div>


            </asp:View>




            <asp:View ID="view2" runat="server">


                <%--               <div class=" panel-header">

                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label8" runat="server" Text="Crear Subcontrata" /></h2>


                </div>--%>
       
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">

                                         <asp:Label runat="server" ID="lblemail" Text="Process" />:  

                                    <asp:DropDownList ID="DdlPreventiva2" runat="server" class="form-control">
                                    </asp:DropDownList>


                            <asp:Label runat="server" ID="ex1v" Text="Step Name" />:


                            <asp:TextBox class="form-control required" ID="txtNombre" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvCIF" runat="server" Text="*" ErrorMessage="Insert Name" ControlToValidate="txtCIF" ValidationGroup="CamposVacios" Display="None" />



                            <asp:Label runat="server" ID="Label23" Text="Description" />:

                            <asp:TextBox ID="txtCIF" class="form-control required" runat="server" />



              


                        </div>
                    </div>

     

                </div>


                <div class="panel-footer">
                    <div class="text-center">



                        <asp:Button ID="CancelVista2" runat="server" class="btn btn-primary" Text="Volver" UseSubmitBehavior="false" />

                        <asp:Button ID="GrabarVista2"  Visible="false"  runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia2_Click" Text="Save" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />

                    </div>
                </div>





            </asp:View>






            <asp:View ID="view3" runat="server">

                <%--                <div class=" panel-header">

                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label2" runat="server" Text="Importar empresa del ERP" /></h2>

                </div>--%>
                <br />

                <div class="panel-body">


                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label runat="server" ID="ex1" Text="Código" />:

                        <asp:TextBox ID="txtCodXBAT" runat="server" class="form-control required" AutoPostBack="True" />
                            <act:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="CargarEmpresasXBATCod"
                                runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresaXBAT"
                                TargetControlID="txtCodXBAT" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />


                            <asp:HiddenField ID="hfEmpresaXBAT" runat="server" />

                            <asp:Label runat="server" ID="Label15" Text="Cif" />:

                        <asp:TextBox ID="txtCIFXBAT" runat="server" class="form-control required" AutoPostBack="True" />
                            <act:AutoCompleteExtender ID="AutoCompleteExtender2" ServiceMethod="CargarEmpresasXBATCIF"
                                runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresaXBATCIF"
                                TargetControlID="txtCIFXBAT" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />
                            <asp:HiddenField ID="hfEmpresaXBATCIF" runat="server" />


                            <asp:Label runat="server" ID="Label16" Text="Name" />:

                        <asp:TextBox ID="txtNombreXBAT" runat="server" class="form-control required" AutoPostBack="True" />
                            <asp:RequiredFieldValidator ID="rfvNombreXBAT" runat="server" Text="*" ErrorMessage="Inserta el nombre de empresa" ControlToValidate="txtNombreXBAT" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvNombreXBAT" PopupPosition="Right" />

                            <act:AutoCompleteExtender ID="AutoCompleteExtender3" ServiceMethod="CargarEmpresasXBATNombre"
                                runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresaXBATNombre"
                                TargetControlID="txtNombreXBAT" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />
                            <asp:HiddenField ID="hfEmpresaXBATNombre" runat="server" />




                              <asp:HiddenField ID="hfEmpresaXBATCodigo" runat="server" />

                            <br />

                        </div>
                    </div>
                </div>

                <div class="panel-footer">
                    <div class="text-center">


                        <asp:Button ID="CancelVista3" runat="server" class="btn btn-primary" Text="Cancel" UseSubmitBehavior="false" />
                        <asp:Button ID="NuevaBusqueda" runat="server" class="btn btn-primary"  Text="New Search" UseSubmitBehavior="false" />
                        <asp:Button ID="GrabarVista3" runat="server" class="btn btn-primary" Text="Save" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />


                    </div>
                </div>


            </asp:View>









        </asp:MultiView>

                        <asp:HiddenField ID="hfcomponente" runat="server" />
                        <asp:HiddenField ID="hfReferencia" runat="server" />
                           <asp:HiddenField ID="hfwork" runat="server" />
                           <asp:HiddenField ID="hfstep" runat="server" />

    </div>

</asp:Content>

