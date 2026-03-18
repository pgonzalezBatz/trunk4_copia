<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Kaplan_Plantas.Master" CodeBehind="Default.aspx.vb" Inherits="KaplanNew._Default" %>

<%@ MasterType VirtualPath="~/Kaplan_Plantas.Master" %>
  


<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">


    <script type="text/javascript">



         
        function ValidateFecha(source, args) {

            var fecha = args.Value;

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



        function RecogerEmpresa(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresa.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }

 


    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">



         <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="KAPLAN" /></a>
        </div>
    </div>
    <div class="container">


        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">

            <asp:View ID="viewListado" runat="server">


                <%--                <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label1" runat="server" Text="Asignación de documentos a empresas" /></h2>


                </div>--%>

                <br />
                <div class="row">
                    <div class="col-xs-3">
                        <asp:Label runat="server" ID="ex12" Text="Referencia" />


                        <asp:TextBox class="form-control"  Text="810068" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarReferencia"
                            runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false"  OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfEmpresa" runat="server" />
                        <asp:HiddenField ID="hfReferencia" runat="server" />

                    </div>
                    <div class="col-xs-4">
                     <br />
                  <asp:TextBox class="form-control" type="text" ID="txtDescref" runat="server" />
                        </div>
                      <div class="col-xs-5">
                             <br />
               <asp:LinkButton ID="lbEditarRef" class="btn  bg-info" ToolTip="Asociar Operación a Referencia" runat="server" Text="Asociar Operación a Referencia" CommandName="Operacion2"   />
                                       <asp:LinkButton ID="lbImprimir" class="btn  bg-info" ToolTip="Generar Informe" runat="server" Text="Generar Informe" CommandName="Operacion3"   />
                          </div>
                </div>

                <br />


                <asp:GridView visible="false" ID="gvType" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped"
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


                        <asp:TemplateField HeaderText="Asociar Operación" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" class="btn  bg-info" ToolTip="Asociar Operación" runat="server" Text="Asociar Operación" CommandName="Operacion"  Visible='<%# Eval("Check1")%>' />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Siguiente Nivel" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbTrabajadores" runat="server" class="btn  bg-info" ToolTip="Siguiente Nivel" Text="Siguiente Nivel" CommandName="Nivel" Visible='<%# Eval("Check1")%>' />
                            </ItemTemplate>

                        </asp:TemplateField>


                    </Columns>


                </asp:GridView>

                <br /> <br /> <br /> <asp:Label runat="server" ID="Label1" Text="DESGLOSE POR COMPONENTES" />:
                
                <asp:GridView ID="gvTypeX" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped"
                    GridLines="None"  DataKeyNames="componente" ShowFooter="false" 
                      PageSize="990">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />

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
                                <asp:Label ID="lblDescripcion2" Text='<%# Eval("textolibre")%>' runat="server" />
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

                     <asp:TemplateField HeaderText="Cantidad"  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion5" Text='<%# Eval("hijos")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Activa" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkObsoleto" Checked="false" Enabled="false" runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Asociar Operación" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" class="btn  bg-info" ToolTip="Asociar Operación" runat="server" Text="Asociar Operación" CommandName="Operacion"  Visible="true" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Siguiente Nivel" Visible="false" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbTrabajadores" runat="server" class="btn  bg-info" ToolTip="Siguiente Nivel" Text="Siguiente Nivel" CommandName="Nivel" Visible="true" />
                            </ItemTemplate>

                        </asp:TemplateField>


                    </Columns>


                </asp:GridView>
				
            </asp:View>


            <asp:View ID="view1" runat="server">


                <%--                <div class=" panel-header">

                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="Asignación de documentos a empresas" /></h2>

                </div>--%>

                <%--<br />--%>

                <div class="row">
                    <div class="col-xs-3">

                        <asp:Label runat="server" ID="ex19" Text="Cif" />:
                        <asp:TextBox ID="txtCIF" class="form-control" runat="server" ReadOnly="True" />

                    </div>
                    <div class="col-xs-4">

                        <asp:Label runat="server" ID="ex18" Text="Nombre" />:

                        <asp:TextBox ID="txtNombre" runat="server" class="form-control" ReadOnly="True" />

                    </div>
                    <div class="text-center">
                        <br />

                        <%-- <asp:Button ID="btnMarcarTodos" runat="server" CausesValidation="false" class="btn btn-primary" OnClick="btnMarcarTodos_Click" Text="Marcar/Desmarcar Todos" />--%>
                        <asp:Button ID="btnMarcarTodos" runat="server" CausesValidation="false" class="btn btn-primary" OnClick="btnMarcarTodos_Click" Text="Marcar/Desmarcar Todos" />


                    </div>
                </div>

                <br />



                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                    GridLines="None" OnRowCommand="gvType2_OnRowCommand" DataKeyNames="Id" ShowFooter="false"
                    OnRowCancelingEdit="gvType2_RowCancelingEdit" OnRowEditing="gvType2_RowEditing" OnRowUpdating="gvType2_RowUpdating">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo">
                            <ItemTemplate>
                                <asp:Label ID="lblTipo" Text='<%# If(Eval("EsDocumento") = 0, "Documento", (If(Eval("EsDocumento") = 1, "Carné", (If(Eval("EsDocumento") = 2, "Formación", (If(Eval("EsDocumento") = 3, "Certificado", (If(Eval("EsDocumento") = 4, "Instrucción Norma", (If(Eval("EsDocumento") = 5, "Manual", "Carné de cualificacion"))))))))))) %>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Documento">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Asignado" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkMarcado" Checked='<%# Eval("Asignada")%>' Enabled="true" runat="server"  />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <%--                   <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" runat="server" Text="Asignar/Desasignar" CommandName="Edit" />

                            </ItemTemplate>

                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Caducidad">
                            <ItemTemplate>


                                <asp:LinkButton ID="lbCaducidad" runat="server" Text='<%# If(Eval("Plantilla") = 0 And Eval("Asignada") = 1, "Modificar Caducidad", If(Eval("Asignada") = 1, "Doc de Plantilla", "")) %>'
                                    CommandName='<%# If(Eval("Plantilla") = 1 And Eval("Asignada") = 1, "Plantilla", "Caducidad") %>' UseSubmitBehavior="false" />



                                <asp:HiddenField ID="idPlantilla0" runat="server" />



                            </ItemTemplate>

                        </asp:TemplateField>


                    </Columns>


                </asp:GridView>

                <hr />
                <p style="text-align: center;">
<asp:Button class="btn btn-primary" ID="btnVolver" runat="server" Text="Volver" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="btnQuitarSeleccionados" runat="server" CssClass="btn btn-lg btn-info"><span class="glyphicon glyphicon-pushpin"></span>&nbsp;<asp:Label ID="Label53" runat="server" Text="Asignar los documentos seleccionados" /></asp:LinkButton></p><act:ConfirmButtonExtender ID="cbeEliminar" runat="server" DisplayModalPopupID="mpeEliminar" TargetControlID="btnQuitarSeleccionados"></act:ConfirmButtonExtender>
                <act:ModalPopupExtender ID="mpeEliminar" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnQuitarSeleccionados" OkControlID="btnBorrar"
                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                </act:ModalPopupExtender>
                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        <asp:Label ID="lblConfirmacion" runat="server" Text="Confirmación" />
                    </div>
                    <div class="body">
                        <asp:Label ID="lblConfirmarBorrado" runat="server" Text="Asignar los documentos seleccionados" />
                    </div>
                    <div class="footer" style="text-align: center;">
                        <asp:Button ID="btnBorrar" runat="server" CssClass="btn btn-primary" Text="Si" />
                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="No" />
                    </div>
                </asp:Panel>


                <%--                <h4 style="text-align: center;"><span class="label label-info">Total documentos: </span>

                    <span class="badge">
                        <asp:Label ID="Registros" runat="server" Text="" />
                    </span>
                </h4>--%>
                <hr />

                <asp:HiddenField runat="server" ID="idEmpresa" />
                <asp:HiddenField runat="server" ID="idDocumento" />
                <asp:HiddenField runat="server" ID="hdnCaducidad" />
                <asp:HiddenField runat="server" ID="chkMarcados" />



            </asp:View>





            <asp:View ID="view2" runat="server">



                <%--            <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label8" runat="server" Text="Modificación de caducidad" /></h2>



                </div>--%>
                <br />
                <div class="panel-body">


                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <br />
                            <asp:Label runat="server" ID="txtfecrec5" Text="Documento" />: <asp:TextBox ID="TxtDocCad" runat="server" class="form-control required" ReadOnly="True" />

                            <br />
                            <asp:Label runat="server" ID="txtfecrec3" Text="Empresa" />: <asp:TextBox ID="txtEmpCad" runat="server" class="form-control required" ReadOnly="True" />


                            <br />
                            <asp:Label runat="server" ID="txtfecrec2" Text="Caducidad" />: <asp:DropDownList ID="ddlCaducidad" AutoPostBack="false" runat="server" class="form-control required">
                            </asp:DropDownList>

                            <br />
                            <br />

                            <asp:HiddenField runat="server" ID="flag_Modificar" />

                            <asp:Button ID="btnCancelar" runat="server" class="btn btn-primary" Text="Cancelar" OnClick="btnCancelar_Click" />

                            <asp:Button ID="btnGuardarNuevaSolicitud" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia_Click" Text="Grabar" UseSubmitBehavior="true" />


                        </div>
                    </div>
                </div>

            </asp:View>







        </asp:MultiView>

    </div>


</asp:Content>


