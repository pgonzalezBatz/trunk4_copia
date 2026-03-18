<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Kaplan_plantas.Master" CodeBehind="OperacionesRef.aspx.vb" Inherits="KaplanNew.OperacionesRef" %>

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
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label3" runat="server" Text="ASOCIACIÓN" /></a>
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
                        <asp:Label ID="Label1" runat="server" Text="OPERACIONES" />

                        <asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarEmpresas2"
                            runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfEmpresa" runat="server" />

                    </div>

                </div>

                <br />



            </asp:View>







            <asp:View ID="view1" runat="server">

                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">

                            <div class="row">
                                <div class="col-xs-4">
                                    <asp:Label runat="server" ID="ex1e" Text="Referencia" />

                                    <asp:TextBox   class="form-control" type="text" ID="txtDescref" runat="server" ReadOnly="True" />
                                </div>
                                    <div class="col-xs-8">
                       <br />
               <asp:Button ID="btnGuardarNuevaSolicitud" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia_Click" Text="Grabar Todo" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />

                        <asp:Button ID="btnCalculo" runat="server" OnClick="calculo_Click" Text="Asignar Modo Fallo y Causas" class="btn btn-primary" UseSubmitBehavior="true" />
                    </div>

                            </div>
                    


                                        <asp:Label  runat="server" ID="Label6" Text="Proceso" />:  

                                    <asp:DropDownList ID="DdlWork" runat="server" class="form-control" AutoPostBack="true">
                                    </asp:DropDownList>
                    
                                                <asp:Label runat="server" ID="Label2" Text="Step" />: 

                                    <asp:DropDownList ID="DdlSteps" runat="server" class="form-control" AutoPostBack="true">
                                    </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ErrorMessage="Añade Código de usuario" ControlToValidate="DdlSteps"  Display="None" />
                              <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="DdlSteps"  Font-Italic="true" Display="None"
                        SetFocusOnError="true" ErrorMessage="Solo se admiten números" ValidationExpression="\d+" />

                


            <br />
             
                            
           

         




                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped"
                    GridLines="None" OnRowCommand="gvType_OnRowCommand" DataKeyNames="componente" ShowFooter="false" OnDataBound="gvType_DataBound"
                    OnRowCancelingEdit="gvType_RowCancelingEdit" OnRowEditing="gvType_RowEditing" OnRowUpdating="gvType_RowUpdating" PageSize="990">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
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
                        <asp:BoundField DataField="componente" HeaderText="componente" Visible="false" />
                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("componente")%>' runat="server" />
                            </ItemTemplate>




                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Referencia" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" Text='<%# Eval("referencia")%>' runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>

                     <asp:TemplateField HeaderText="Padre" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion2" Text='<%# Eval("padre")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                     <asp:TemplateField HeaderText="componente">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion3" Text='<%# Eval("componente")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                     <asp:TemplateField HeaderText="Desc. componente">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion4" Text='<%# Eval("desc_comp")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Asignado" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkMarcado" Checked='<%# Eval("check1")%>' Enabled="true" runat="server"  />
                            </ItemTemplate>

                        </asp:TemplateField>



                     <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion5" Text='<%# Eval("Cantidad")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Activa" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkObsoleto" Checked='<%# Eval("Check1")%>' Enabled="false" runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Cálculos" Visible="false" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" class="btn  bg-info" ToolTip="Cálculos" runat="server" Text="Cálculos" Enabled="false" CommandName="Operacion"  Visible='<%# Eval("Check1")%>' />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Siguiente Nivel" Visible="false" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbTrabajadores" runat="server" class="btn  bg-info" ToolTip="Siguiente Nivel" Text="Siguiente Nivel" CommandName="Nivel" Visible='<%# Eval("Check1")%>' />
                            </ItemTemplate>

                        </asp:TemplateField>


                    </Columns>


                </asp:GridView>

<br />
                                      <hr />




                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped" 
                    GridLines="None" OnRowCommand="gvType2_OnRowCommand" OnRowEditing="gvType2_RowEditing" OnRowUpdating="gvType2_RowUpdating"  OnRowCancelingEdit="gvType2_RowCancelingEdit"  DataKeyNames="id" ShowFooter="false"  PageSize="990">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
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

                        <asp:TemplateField HeaderText=""   >
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("id")%>' runat="server" Width="1px" Style="color:white;" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Caracteristica" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblCaracte" Text='<%# Eval("Caracteristica")%>' runat="server" />
                            </ItemTemplate>

                            
                            <EditItemTemplate>
                                <asp:TextBox ID="txtModTarjetaxx" runat="server" Text='<%# Eval("Caracteristica")%>' class="BatzFont"  Width="130px" />
                            </EditItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Specificación" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" Text='<%# Eval("Caracteristica2")%>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtModTarjetab" runat="server" Text='<%# Eval("Caracteristica2")%>' class="BatzFont"  Width="130px" />
                            </EditItemTemplate>

                        </asp:TemplateField>

                   <asp:TemplateField HeaderText="Clase" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small "  >
                            <ItemTemplate>
                                <asp:Label ID="lblTipo" Text='<%# Eval("componente")%>' ControlStyle-Font-Size="Small" ItemStyle-Font-Size="Small"  runat="server" ControlStyle-CssClass="BatzFont" />
                            </ItemTemplate>
              <EditItemTemplate>

                   <asp:DropDownList OnInit="ddlTheDropDownList_Init" ID="TipoCOpc" runat="server" class="form-control" ControlStyle-CssClass="BatzFont">
    
                                    </asp:DropDownList>




                           </EditItemTemplate>
                        </asp:TemplateField>


    
                          <asp:TemplateField HeaderText="Tipo" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small " >
                            <ItemTemplate>
                                <asp:Label ID="lblClase" Text='<%# Eval("referencia")%>' runat="server" />
                            </ItemTemplate>
              <EditItemTemplate>


                           <asp:DropDownList OnInit="ddlTheDropDownList2_Init" ID="ClaseCOpc" runat="server" class="form-control" ControlStyle-CssClass="BatzFont">
    
                                    </asp:DropDownList>


                                <%--<asp:TextBox ID="txtModTarjetad" runat="server" Text='<%# Eval("referencia")%>' class="BatzFont"  Width="130px"   />--%>
                            </EditItemTemplate>
                        </asp:TemplateField>

                  <asp:TemplateField HeaderText="Max." ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small " >
                            <ItemTemplate>
                                <asp:Label ID="lblMax" Text='<%# Eval("Max")%>' runat="server" />
                            </ItemTemplate>
            <EditItemTemplate>
                                <asp:TextBox ID="txtModTarjetae" runat="server" Text='<%# Eval("Max")%>' class="BatzFont"  Width="40px" />
                            </EditItemTemplate>
                        </asp:TemplateField>


                  <asp:TemplateField HeaderText="Min." ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small " >
                            <ItemTemplate>
                                <asp:Label ID="lblMin" Text='<%# Eval("Min")%>' runat="server"/>
                            </ItemTemplate>


                            <EditItemTemplate>
                                <asp:TextBox ID="txtModTarjetaf" runat="server" Text='<%# Eval("Min")%>' class="BatzFont"  Width="40px"  />
                            </EditItemTemplate>


                        </asp:TemplateField>


     





                        <asp:TemplateField HeaderText="Modificar" HeaderStyle-HorizontalAlign="Center" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Modificar" CssClass="btn btn-info" CommandName="Edit" AutoPostBack="false" CausesValidation="false" UseSubmitBehavior="false" />

                          </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="Grabar" CssClass="btn btn-success" CommandName="Activar" CausesValidation="false" UseSubmitBehavior="false" CommandArgument='<%# Container.DataItemIndex%>' />                            
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="btn btn-default" CommandName="Cancel" CausesValidation="false" UseSubmitBehavior="false" />


                            </EditItemTemplate>
                        </asp:TemplateField>


                        
                        <asp:TemplateField ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Borrar" Visible='<%# If(Eval("obsoleto") = 0, "true", "false") %>' />
                                <act:ConfirmButtonExtender ID="cbeEliminar3" runat="server" DisplayModalPopupID="mpeEliminar3"
                                    TargetControlID="btnEliminar3">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar3" runat="server" PopupControlID="pnlConfirmDelete2" TargetControlID="btnEliminar3" OkControlID="btnBorrar4"
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
                                        <asp:Button ID="btnBorrar4" runat="server" CssClass="btn btn-primary" Text="Yes" />
                                        <asp:Button ID="btnCancelar3" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2a" runat="server" CssClass="btn btn-default" CommandName="Activar2" Text="Modo Fallo" Visible="true" />
                                <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="btnEliminar2a">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminar2a" 
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Modo Fallo" />
                                    </div>
                                    <div class="body">




                <div class="col-xs-4">

                       <asp:Label runat="server" ID="Label18xa" Text="Fallo Tipo 1" />:  

                                 <asp:TextBox ID="TrabajoSTDa" Text="No existe el componente" runat="server" class="form-control"  />
                             </div>
                           <div class="col-xs-4">

            
                       <asp:Label runat="server" ID="Label19xa" Text="Fallo Tipo 2" />:  

                         <asp:TextBox ID="Parametroa"  Text='<%# Eval("Textolibre")%>' runat="server" class="form-control"  />
                               </div>
                       
                            <div class="col-xs-4">
                           <asp:Label  runat="server" ID="Label20xa" Text="Fallo Tipo 3" />:  
                 <asp:TextBox ID="txtEspecificaciona" Text='<%# Eval("Textolibre2")%>'  runat="server" class="form-control"  />

                                </div>
                         <br />  <br />
                            <div class="col-xs-4">
                     <asp:Label  runat="server" ID="Label21xa" Text="Fallo Tipo 4" />:  

                 <asp:TextBox ID="txtClasea"  runat="server" class="form-control"  />


            </div>
                            <div class="col-xs-4">

        <asp:Label  runat="server" ID="Label22xa" Text="Fallo Tipo 5" />:  
                 <asp:TextBox ID="txtMaximo2a"  runat="server" class="form-control"  />

         </div>
                            <div class="col-xs-4">

               <asp:Label  runat="server" ID="Label24xa" Text="Fallo Tipo 6" />:  
                 <asp:TextBox ID="txtMinimo2a"  runat="server" class="form-control"  />


                        <br />




                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="OK" />
                                        <asp:Button ID="btnBorrar3" runat="server" CssClass="btn btn-primary" Text="Grabar" Visible="false"  />

                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>








                    </Columns>


                </asp:GridView>






                    






                               <p style="text-align: center;">
                       <asp:LinkButton ID="btnQuitarSeleccionados" runat="server" CssClass="btn btn-default btn-info"><span class="glyphicon glyphicon-pushpin"></span>&nbsp;<asp:Label ID="Label53" runat="server" Text="Añadir Característica" /></asp:LinkButton></p><act:ConfirmButtonExtender ID="cbeEliminar" runat="server" DisplayModalPopupID="mpeEliminar" TargetControlID="btnQuitarSeleccionados"></act:ConfirmButtonExtender>
                <act:ModalPopupExtender ID="mpeEliminar" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnQuitarSeleccionados" OkControlID="btnBorrar"
                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                </act:ModalPopupExtender>
                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        <asp:Label ID="lblConfirmacion" runat="server" Text="Características" />
                    </div>
                    <div class="body">
                      <div class="col-xs-4">

                       <asp:Label runat="server" ID="Label8" Text="Clase" />:  <br />

                                    <asp:DropDownList ID="TipoC" runat="server" class="form-control" ControlStyle-CssClass="BatzFont">
                                    </asp:DropDownList>
                             </div>
                           <div class="col-xs-4">

            
                       <asp:Label runat="server" ID="Label9" Text="Tipo" />:  

                                    <asp:DropDownList ID="ClaseC" runat="server" class="form-control" >
                                    </asp:DropDownList>
                               </div>
                       
                            <div class="col-xs-4">
                           <asp:Label  runat="server" ID="Label10" Text="Descripción" />:  
                 <asp:TextBox ID="Car1"  runat="server" class="form-control"  />

                                </div>
                         <br />  <br />
                            <div class="col-xs-4">
                 <asp:Label  runat="server" ID="Label11" Text="Especificación" />:  
                 <asp:TextBox ID="car11"  runat="server" class="form-control"  />


            </div>
                            <div class="col-xs-4">

        <asp:Label  runat="server" ID="Label12" Text="Max" />:  
                 <asp:TextBox ID="txtMax"  runat="server" class="form-control"  placeholder="n,nnn"   />

         </div>
                            <div class="col-xs-4">

               <asp:Label  runat="server" ID="Label13" Text="Min" />:  
                 <asp:TextBox ID="txtMin"  runat="server" class="form-control"  placeholder="n,nnn"   />


                        <br />
                                  <br />


                        
  </div>
                    </div>
                    <div class="footer" style="text-align: center;">
                                                <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="Cancelar" />
                        <asp:Button ID="btnBorrar" runat="server" CssClass="btn btn-primary" Text="Grabar" AutoPostBack="true" />

                    </div>
                </asp:Panel>










<br />


                            

                <asp:GridView ID="gvType3" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped"
                    GridLines="None" OnRowCommand="gvType3_OnRowCommand" OnRowEditing="gvType3_RowEditing" OnRowUpdating="gvType3_RowUpdating"  OnRowCancelingEdit="gvType3_RowCancelingEdit"   DataKeyNames="id" ShowFooter="false"  PageSize="990">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
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

                      <asp:TemplateField HeaderText="" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("id")%>' runat="server" Width="1px" Style="color:white;" />
                            </ItemTemplate>
                            
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Atributo" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblCaractere" Text='<%# Eval("TrabajoSTD")%>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt2ModTarjetaxx" runat="server" Text='<%# Eval("TrabajoSTD")%>' class="BatzFont"  Width="130px" />
                            </EditItemTemplate>
                        </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Parámetro Proceso" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblTipop" Text='<%# Eval("Parametro")%>' runat="server" />
                            </ItemTemplate>
                          <EditItemTemplate>
                                <asp:TextBox ID="txt3ModTarjetaxx" runat="server" Text='<%# Eval("Parametro")%>' class="BatzFont"  Width="130px" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Specificación" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcionp" Text='<%# Eval("Caracteristica")%>' runat="server" />
                            </ItemTemplate>
                         <EditItemTemplate>
                                <asp:TextBox ID="txt4ModTarjetaxx" runat="server" Text='<%# Eval("Caracteristica")%>' class="BatzFont"  Width="130px" />
                            </EditItemTemplate>                      
                        </asp:TemplateField>


              
    
                          <asp:TemplateField HeaderText="Clase" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblClasep" Text='<%# Eval("Caracteristica2")%>' runat="server" />
                            </ItemTemplate>
                   <EditItemTemplate>
                                <asp:TextBox ID="txt5ModTarjetaxx" runat="server" Text='<%# Eval("Caracteristica2")%>' class="BatzFont"  Width="130px" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                  <asp:TemplateField HeaderText="Max." ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small " >
                            <ItemTemplate>
                                <asp:Label ID="lblMaxp" Text='<%# Eval("Max")%>' runat="server" />
                            </ItemTemplate>
                   <EditItemTemplate>
                                <asp:TextBox ID="txt6ModTarjetaxx" runat="server" Text='<%# Eval("Max")%>' class="BatzFont"  Width="40px" />
                            </EditItemTemplate>
                        </asp:TemplateField>


                  <asp:TemplateField HeaderText="Min." ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Label ID="lblMinp" Text='<%# Eval("Min")%>' runat="server" />
                            </ItemTemplate>
                   <EditItemTemplate>
                                <asp:TextBox ID="txt7ModTarjetaxx" runat="server" Text='<%# Eval("Min")%>' class="BatzFont"  Width="40px" />
                            </EditItemTemplate>
                        </asp:TemplateField>


        


                        <asp:TemplateField HeaderText="Modificar" HeaderStyle-HorizontalAlign="Center" ControlStyle-Font-Size="Small" ItemStyle-Font-Size=" Small ">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Modificar" CssClass="btn btn-info" CommandName="Edit" AutoPostBack="false" CausesValidation="false" UseSubmitBehavior="false" />

                          </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="Grabar" CssClass="btn btn-success" CommandName="Activar" CausesValidation="false" UseSubmitBehavior="false" CommandArgument='<%# Container.DataItemIndex%>' />                            
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="btn btn-default" CommandName="Cancel" CausesValidation="false" UseSubmitBehavior="false" />


                            </EditItemTemplate>
                        </asp:TemplateField>
                        

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Borrar"  />
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
                                                                                <asp:Button ID="btnCancelar3" runat="server" CssClass="btn btn-primary" Text="Cancelar" />
                                        <asp:Button ID="btnBorrar3" runat="server" CssClass="btn btn-primary" Text="Grabar" />

                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>




                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Activar" Text="Enable" Visible="false" />
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
                       <asp:LinkButton ID="btnQuitarSeleccionadosx" runat="server" CssClass="btn btn-default btn-info"><span class="glyphicon glyphicon-pushpin"></span>&nbsp;<asp:Label ID="Label14" runat="server" Text="Atributos del Proceso" /></asp:LinkButton></p><act:ConfirmButtonExtender ID="cbeEliminarx" runat="server" DisplayModalPopupID="mpeEliminarx" TargetControlID="btnQuitarSeleccionadosx"></act:ConfirmButtonExtender>
                <act:ModalPopupExtender ID="mpeEliminarx" runat="server" PopupControlID="pnlConfirmDeletex" TargetControlID="btnQuitarSeleccionadosx" OkControlID="btnBorrarx"
                    CancelControlID="btnCancelar2x" BackgroundCssClass="modalBackground">
                </act:ModalPopupExtender>
                <asp:Panel ID="pnlConfirmDeletex" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        <asp:Label ID="Label17x" runat="server" Text="Características" />
                    </div>
                    <div class="body">
                      <div class="col-xs-4">

                       <asp:Label runat="server" ID="Label18x" Text="Trabajo STD" />:  

                                 <asp:TextBox ID="TrabajoSTD"  runat="server" class="form-control"  />
                             </div>
                           <div class="col-xs-4">

            
                       <asp:Label runat="server" ID="Label19x" Text="Parámetro Proceso" />:  

                         <asp:TextBox ID="Parametro"  runat="server" class="form-control"  />
                               </div>
                       
                            <div class="col-xs-4">
                           <asp:Label  runat="server" ID="Label20x" Text="Especificación" />:  
                 <asp:TextBox ID="txtEspecificacion"  runat="server" class="form-control"  />

                                </div>
                         <br />  <br />
                            <div class="col-xs-4">
                                                      <asp:Label  runat="server" ID="Label21x" Text="Clase" />:  
                 <asp:TextBox ID="txtClase"  runat="server" class="form-control"  />


            </div>
                            <div class="col-xs-4">

        <asp:Label  runat="server" ID="Label22x" Text="Max." />:  
                 <asp:TextBox ID="txtMaximo2"  runat="server" class="form-control"  />

         </div>
                            <div class="col-xs-4">

               <asp:Label  runat="server" ID="Label24x" Text="Min." />:  
                 <asp:TextBox ID="txtMinimo2"  runat="server" class="form-control"  />


                        <br />
                                  <br />


                        
  </div>
                    </div>
                    <div class="footer" style="text-align: center;">
                                                <asp:Button ID="btnCancelar2x" runat="server" CssClass="btn btn-primary" Text="Cancelar" />
                        <asp:Button ID="btnBorrarx" runat="server" CssClass="btn btn-primary" Text="Grabar" AutoPostBack="false" />

                    </div>
                </asp:Panel>









                           <asp:Label Visible ="false" runat="server" ID="Label4" Text="Tipo Característica" /> 
                                          <asp:TextBox Visible ="false" ID="TxtCaracteristicas"  runat="server" class="form-control"  />
          
                                    <asp:DropDownList Visible ="false" ID="DdlCaracterist" runat="server" class="form-control" AutoPostBack="false" >
                                    </asp:DropDownList>
                          <br />     
                               <asp:Label runat="server"  ID="Label5" Text="Clase "  Visible ="false" />
                 <asp:TextBox ID="TxtCaracteristicas2"  runat="server" class="form-control" Visible ="false"  />
          <asp:Label runat="server"  ID="lblCaracteristicas2" Text="Descripción" />:  
        
 <asp:TextBox ID="TxtCaracteristicas3" runat="server" class="form-control"  />

<br />

                        <asp:HiddenField ID="hfcomponente" runat="server" />
                        <asp:HiddenField ID="hfReferencia" runat="server" />
                           <asp:HiddenField ID="hfwork" runat="server" />
                           <asp:HiddenField ID="hfstep" runat="server" />
                            <asp:HiddenField ID="hfcont" runat="server" />
                           <asp:HiddenField ID="hfTipoC" runat="server" />
                           <asp:HiddenField ID="hfClaseC" runat="server" />
                                         <asp:HiddenField ID="hfTipoC2" runat="server" />
                                         <asp:HiddenField ID="hfClaseC2" runat="server" />

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

    
                   </div>
                </div>
                <br />
                <br />

            </asp:View>




            <asp:View ID="view2" runat="server">


                <%--               <div class=" panel-header">

                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label8" runat="server" Text="Crear Subcontrata" /></h2>


                </div>--%>
                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label runat="server" ID="ex1v" Text="Nombre" />


                            <asp:TextBox class="form-control required" ID="txtCIF" runat="server" />
              

                            <asp:Label visible="false"  runat="server" ID="Label23" Text="Description" />

                            <asp:TextBox  visible="false"  ID="txtNombre" Text='<%# Eval("Descripcion")%>' class="form-control required" runat="server" />
            
                           <asp:Label runat="server" ID="Label7" Text="Step" />: 

                                    <asp:DropDownList ID="DdlPreventiva5" runat="server" class="form-control">
                                    </asp:DropDownList>

                           <asp:Label runat="server" ID="lblemail" Text="Work" />:  

                                    <asp:DropDownList ID="DdlPreventiva2" runat="server" class="form-control">
                                    </asp:DropDownList>


                        </div>
                    </div>

                    <br />
                    <br />

                </div>


                <div class="panel-footer">
                    <div class="text-center">



                        <asp:Button ID="CancelVista2" runat="server" class="btn btn-primary" Text="Volver" UseSubmitBehavior="false" />

               
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
                            <asp:Label runat="server" ID="ex1" Text="Código en el ERP" />:

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


                            <asp:Label runat="server" ID="Label16" Text="Nombre" />:

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


                        <asp:Button ID="CancelVista3" runat="server" class="btn btn-primary" Text="Cancelar" UseSubmitBehavior="false" />
                        <asp:Button ID="NuevaBusqueda" runat="server" class="btn btn-primary"  Text="Nueva Búsqueda" UseSubmitBehavior="false" />
                        <asp:Button ID="GrabarVista3" runat="server" class="btn btn-primary" Text="Grabar" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />


                    </div>
                </div>


            </asp:View>









        </asp:MultiView>

        <%--         <div class="row" style="margin-top: 5px;">

          <div class="col-lg-12" style="text-align: center;">
                   <h4>
       <asp:Label ID="lblInfo" runat="server" />               
                </h4>
              </div></div>--%>
    </div>

</asp:Content>

