<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Kaplan_plantas.Master" CodeBehind="CalculoRef.aspx.vb" Inherits="KaplanNew.CalculoRef" %>

<%@ MasterType VirtualPath="~/Kaplan_plantas.Master" %>


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
            var hfIdUsuario = document.getElementById('<%=hfComponente.ClientID%>');
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
                        <asp:Label  runat="server" ID="ex12" Text="Componente" Visible="false" />


                        <asp:TextBox Visible="false" class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarReferencia"
                            runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfComponente" runat="server" />
                        <asp:HiddenField ID="hfReferencia" runat="server" />

                    </div>
                    <div class="col-xs-6">

                        <asp:Label ID="MessageLabelxx" Text="Componente: " runat="server" />
                  <asp:TextBox class="form-control" type="text" ID="txtDescref" runat="server" />
                        </div>
                </div>

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


                        <asp:TemplateField HeaderText="Referencia" >
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion6" Text='<%# Eval("referencia")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

   

                     <asp:TemplateField HeaderText="componente" >
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion3" Text='<%# Eval("componente")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

     
                        <asp:TemplateField HeaderText="Process" >
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion33" Text='<%# Eval("desc_process")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Step">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion5" Text='<%# Eval("Textolibre")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Work" >
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion2" Text='<%# Eval("Textolibre2")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>




                       <asp:TemplateField HeaderText="Elementos" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="lblDescripcion" Text="Ver Elementos" ToolTip="Ver Elementos" title="Ver Elementos" />
                           <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="lblDescripcion">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="lblDescripcion"
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Elementos" />
                                    </div>
                                    <div class="body"   style="text-align: left; margin-left: 135px;">
                                        <br />
                                        <asp:Label ID="lblHijos" runat="server" Text='<%# Eval("Hijos")%>' />

                                        <br />
                                    </div>
                                    <div class="footer" style="text-align: center;">

                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="OK" />
                                    </div>
                                </asp:Panel>

                            </ItemTemplate>

                        </asp:TemplateField>





                        
                       <asp:TemplateField HeaderText="Caracteristicas y Modo Fallo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="lblDescripcionw" Text="Caracteristicas y Modo Fallo"  title="Caracteristicas" />
                           <act:ConfirmButtonExtender ID="cbeEliminar2w" runat="server" DisplayModalPopupID="mpeEliminar2w"
                                    TargetControlID="lblDescripcionw">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2w" runat="server" PopupControlID="pnlConfirmDeletew" TargetControlID="lblDescripcionw" 
                                    CancelControlID="btnCancelar2w" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                  <asp:Panel ID="pnlConfirmDeletew" runat="server" CssClass="modalPopup"  style="display: flex;  justify-content: center; overflow-y: scroll; max-height:85%; max-width:85%; width: 1100px;  margin-top: 1px; margin-bottom:50px;">
                           
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacionw" runat="server" Text="Caracteristicas y Modo Fallo" />
                                    </div>
                                    <div class="body"   style=" display: flex;  justify-content: center; margin-right: auto; margin-left: auto;">
                                     
                                        <asp:Label ID="lblHijosw" runat="server" Text='<%# Eval("tClaseC")%>' />

                                        <br />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <br />
                                        <asp:Button ID="btnCancelar2w" runat="server" CssClass="btn btn-primary" Text="OK" />
                                    </div>
                                </asp:Panel>

                            </ItemTemplate>

                        </asp:TemplateField>



<%--
                       <asp:TemplateField HeaderText="Modo Fallo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="lblDescripcionc" Text="Modo Fallo"  title="Modo Fallo" />
                           <act:ConfirmButtonExtender ID="cbeEliminar2c" runat="server" DisplayModalPopupID="mpeEliminar2c"
                                    TargetControlID="lblDescripcionc">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2c" runat="server" PopupControlID="pnlConfirmDeletec" TargetControlID="lblDescripcionc"
                                    CancelControlID="btnCancelar2c" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                  <asp:Panel ID="pnlConfirmDeletec" runat="server" CssClass="modalPopup"  style=" display: flex;  justify-content: center; overflow-y: scroll; max-height:85%; max-width:85%; width: 1100px;  margin-top: 1px; margin-bottom:50px;">
                           
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacionc" runat="server" Text="Modo Fallo" />
                                    </div>
                                    <div class="body"   style=" display: flex;  justify-content: center; margin-right: auto; margin-left: auto;">



                                        <asp:Label ID="lblHijosc" runat="server" Text='<%# Eval("Condicion1")%>' />

                                        <br /> <br />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <br />
                                        <asp:Button ID="btnCancelar2c" runat="server" CssClass="btn btn-primary" Text="OK" />
                                    </div>
                                </asp:Panel>

                            </ItemTemplate>

                        </asp:TemplateField>
--%>






                       <asp:TemplateField HeaderText="Atributos" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="lblDescripcionq" Text="Atributos"  title="Atributos" />
                           <act:ConfirmButtonExtender ID="cbeEliminar2q" runat="server" DisplayModalPopupID="mpeEliminar2q"
                                    TargetControlID="lblDescripcionq">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2q" runat="server" PopupControlID="pnlConfirmDeleteq" TargetControlID="lblDescripcionq"
                                    CancelControlID="btnCancelar2q" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                  <asp:Panel ID="pnlConfirmDeleteq" runat="server" CssClass="modalPopup"  style=" display: flex;  justify-content: center; overflow-y: scroll; max-height:85%; max-width:85%; width: 1100px;  margin-top: 1px; margin-bottom:50px;">
                           
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacionq" runat="server" Text="Atributos" />
                                    </div>
                                    <div class="body"   style=" display: flex;  justify-content: center; margin-right: auto; margin-left: auto;">



                                        <asp:Label ID="lblHijosq" runat="server" Text='<%# Eval("tTipoC")%>' />

                                        <br /> <br />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <br />
                                        <asp:Button ID="btnCancelar2q" runat="server" CssClass="btn btn-primary" Text="OK" />
                                    </div>
                                </asp:Panel>

                            </ItemTemplate>

                        </asp:TemplateField>







                        <asp:TemplateField HeaderText="Descripcion" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="lblDescripcionz" Text="Descripcion"  title="Descripcion" />
                           <act:ConfirmButtonExtender ID="cbeEliminar2z" runat="server" DisplayModalPopupID="mpeEliminar2z"
                                    TargetControlID="lblDescripcionz">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2z" runat="server" PopupControlID="pnlConfirmDeletez" TargetControlID="lblDescripcionz" 
                                    CancelControlID="btnCancelar2z" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                  <asp:Panel ID="pnlConfirmDeletez" runat="server" CssClass="modalPopup"  style="display: flex;  justify-content: center; overflow-y: scroll; max-height:85%; max-width:85%; width: 1100px;  margin-top: 1px; margin-bottom:50px;">
                           
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacionz" runat="server" Text="Descripcion" />
                                    </div>
                                    <div class="body"   style=" display: flex;  justify-content: center; margin-right: auto; margin-left: auto;">
                                     
                                        <asp:Label ID="lblHijosz" runat="server" Text='<%# Eval("textolibre3")%>' />

                                        <br />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <br />
                                        <asp:Button ID="btnCancelar2z" runat="server" CssClass="btn btn-primary" Text="OK" />
                                    </div>
                                </asp:Panel>

                            </ItemTemplate>

                        </asp:TemplateField>





                        <asp:TemplateField HeaderText="Borrar" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" class="btn  bg-info" ToolTip="Borrar" runat="server" Text="Borrar" CommandName="Operacion"  AutoPostBack="False" />
                            </ItemTemplate>

                        </asp:TemplateField>
        



                        <asp:TemplateField HeaderText="Modificar" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar2" class="btn  bg-info" ToolTip="Modificar" runat="server" Text="Modificar" CommandName="Operacion2"  AutoPostBack="False" />
                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>


                </asp:GridView>

                               <hr />
                <p style="text-align: center;">
<asp:Button class="btn btn-primary" ID="btnVolver" runat="server" Text="Volver a Asignación Process Steps" UseSubmitBehavior="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       


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
                                <asp:Label ID="lblDescripcion5" Text='<%# Eval("Nombre")%>' runat="server" />
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

