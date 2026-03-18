<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_plantas.Master" CodeBehind="CrearDocumento.aspx.vb" Inherits="AdokWeb.CrearDocumento" %>

<%@ MasterType VirtualPath="~/Adok_plantas.Master" %>


<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">

    <script type="text/javascript">





  


         



        function ValidateFich() {
            //return true;

            var fich = document.getElementById('<%=fuDoc.ClientID%>').value;
            //alert(fich);
            if (fich) {

                return true;
            } else {

                return false;
            }


        }

        function ValidateFich2() {
            //return true;

            var fich = document.getElementById('<%=fuDoc2.ClientID%>').value;
            //alert(fich);
            if (fich) {

                return true;
            } else {

                return false;
            }


        }


    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">

    <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label5" runat="server" Text="Mantenimiento de Documentos" /></a>
        </div>
    </div>

    <%--pongo el panel arriba porque no coge el fichero si no--%>
    <div class="container">
        <asp:Panel ID="pnlConfirmSubir" HorizontalAlign="Center" runat="server" CssClass="modalPopup" Style="display: none">

            <div class="header">
                <asp:Label runat="server" ID="label2" Text="Subir plantillas al servidor" />

            </div>

            <div class="body">
                <div class="row">
                    <div class="col-lg-12">

                        <asp:Label runat="server" ID="lblConfirmarBorrado" Text="Selecciona la plantilla a subir" />:

            <br />

                        <asp:FileUpload runat="server" ID="fuDoc" CausesValidation="True" class="form-control" />

                    </div>
                </div>
            </div>

            <div class="footer" style="text-align: center;">

                <br />

                <asp:Button class="btn btn-primary" runat="server" OnClientClick="javascript: return ValidateFich();" ID="btnSubir2" Text="Subir Plantilla" CausesValidation="true" />
                <asp:Button class="btn btn-primary" ID="btnBorrar" runat="server" Text="Volver" UseSubmitBehavior="false" CausesValidation="false" />

                <br />
            </div>


        </asp:Panel>





        <asp:Panel ID="pnlConfirmSubir2" HorizontalAlign="Center" runat="server" CssClass="modalPopup" Style="display: none">

            <div class="header">
                <asp:Label runat="server" ID="label1" Text="Subir plantilla de lectura obligatoria" />

            </div>

            <div class="body">
                <div class="row">
                    <div class="col-lg-12">

                        <asp:Label runat="server" ID="Label6" Text="Selecciona la plantilla a subir" />:

            <br />

                        <asp:FileUpload runat="server" ID="fuDoc2" CausesValidation="True" class="form-control" />

                    </div>
                </div>
            </div>

            <div class="footer" style="text-align: center;">

                <br />

                <asp:Button class="btn btn-primary" runat="server" OnClientClick="javascript: return ValidateFich2();" ID="btnSubirx2" Text="Subir Plantilla de lectura obligatoria" CausesValidation="true" />
                <asp:Button class="btn btn-primary" ID="btnBorrar2" runat="server" Text="Volver" UseSubmitBehavior="false" CausesValidation="false" />

                <br />
            </div>


        </asp:Panel>




    </div>







    <div class="container">

        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">

            <asp:View ID="viewListado" runat="server">

                <%--               <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblEmpresa" runat="server" Text="Mantenimiento de documentos" /></h2>


                </div>
                    <br />--%>

                <br />
                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                    GridLines="None" DataKeyNames="Id" ShowFooter="false"
                    OnRowEditing="gvType_RowEditing">
                    <%--<RowStyle CssClass="RowStyle" HorizontalAlign="Center" />--%>
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" Height="40" VerticalAlign="Middle" />
                    <PagerStyle CssClass="PagerStyle" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                    <%--<HeaderStyle CssClass="HeaderStyle" />--%>
                    <EditRowStyle CssClass="EditRowStyle" />
                    <AlternatingRowStyle CssClass="AlternatingRowStyle" />
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

                        <%--Font-Underline="true"--%>
                        <asp:TemplateField HeaderText="Documento">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" Font-Underline="true" ToolTip="Modificar Documento" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActivo" Checked='<%# If(Eval("Activo") = 1, 0, 1) %>' Enabled="false" runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <%--                   <asp:TemplateField HeaderText="Plantilla">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPlantilla" Checked='<%# Eval("Plantilla")%>' Enabled="false" runat="server" />
                                            </ItemTemplate>

                    
                                        </asp:TemplateField>--%>



                        <%--
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbEditar" runat="server" Text="Modificar" CommandName="Edit" />
                                            </ItemTemplate>

                                        </asp:TemplateField>--%>




                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Desactivar" Text="Desactivar" Visible='<%# If(Eval("Activo") = 1, "false", "true") %>' />
                                <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="btnEliminar2">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="btnEliminar2" OkControlID="btnBorrar2"
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Confirmación" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text="¿Estás seguro que quieres desactivar el documento?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar2" runat="server" CssClass="btn btn-primary" Text="Si" />
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Activar" Text="Activar" Visible='<%# If(Eval("Activo") = 1, "true", "false") %>' />
                                <act:ConfirmButtonExtender ID="cbeEliminar3" runat="server" DisplayModalPopupID="mpeEliminar3"
                                    TargetControlID="btnEliminar3">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar3" runat="server" PopupControlID="pnlConfirmDelete3" TargetControlID="btnEliminar3" OkControlID="btnBorrar3"
                                    CancelControlID="btnCancelar3" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete3" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion3" runat="server" Text="Confirmación" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado3" runat="server" Text="¿Estás seguro que quieres activar el documento?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar3" runat="server" CssClass="btn btn-primary" Text="Si" />
                                        <asp:Button ID="btnCancelar3" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>


                    </Columns>

                </asp:GridView>


            </asp:View>


            <asp:View ID="view1" runat="server">

                <%--                                           <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="Mantenimiento de datos" /></h2>

                </div>--%>

                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">

                            <asp:Label runat="server" ID="lblNombre" Text="Nombre" />:
                                          
                                                <asp:TextBox ID="txtNombre" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="*" ErrorMessage="Añade Nombre" ControlToValidate="txtNombre" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="Right" />


                            <asp:Label runat="server" ID="lblAbrev" Text="Nombre abreviado" />:
                                          
                                                <asp:TextBox ID="TextAbrev" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvAbrev" runat="server" Text="*" ErrorMessage="Añade Nombre Abreviado" ControlToValidate="TextAbrev" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceAbrev" runat="server" TargetControlID="rfvAbrev" PopupPosition="Right" />




                            <asp:Label ID="lblPlantillaSubida" runat="server" Text="No hay ninguna plantilla subida  " Visible="false" Font-Bold="True" ForeColor="#0099FF" />





                            <asp:Label runat="server" ID="lblDescC" Text="Descripción" />:
                                       
                                                <asp:TextBox ID="txtComentario" runat="server" class="form-control" Rows="6" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="rfvComentario" runat="server" Text="*" ErrorMessage="Añade Comentario" ControlToValidate="txtComentario" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceComentario" runat="server" TargetControlID="rfvComentario" PopupPosition="Right" />


                            <asp:Label runat="server" ID="lblDuracion" Text="Duración (Número de Horas)" />:
                                          
                            <asp:TextBox ID="txtDuracion" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvDuracion" runat="server" Text="*" ErrorMessage="Añade la duración del curso" ControlToValidate="txtDuracion" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceDuracion" runat="server" TargetControlID="rfvDuracion" PopupPosition="Right" />






                            <br />
                            <div class="form-inline row">
                                <div class="form-group col-sm-12">

                                 <br />
                                    <asp:Label runat="server" ID="lblPeriodicidadC" Width="18%" Text="Caducidad:" />
                                    <br />
            <asp:DropDownList ID="ddlCaducidad" runat="server" class="form-control">
                                    </asp:DropDownList>

                                                     

           
                                </div>

                            </div>



                            
                               <div class="form-inline row">
                                <div class="form-group col-sm-12">

                            <asp:Label Visible="false" runat="server" ID="lblAsignacionC" Text="Tipo dcto" />  
                                    <br />
                                                <asp:DropDownList Visible="false" ID="DdlEsDocumento" runat="server" class="form-control" AutoPostBack="True">
                                                    <asp:ListItem Text="Documento " Value="0" />
                                                    <asp:ListItem Text="Carné" Value="1" />
                                                    <asp:ListItem Text="Formación" Value="2" />
                                                    <asp:ListItem Text="Certificado" Value="3" />
                                                    <asp:ListItem Text="Instrucción Norma" Value="4" />
                                                    <asp:ListItem Text="Manual" Value="5" />
                                                    <asp:ListItem Text="Carné de cualificación" Value="6" />
                                                </asp:DropDownList>





                                                 <asp:DropDownList ID="DdlTipoTrabajo" runat="server" class="form-control" visible="false" >
                                                </asp:DropDownList>





      </div>
                            </div>
                            <br />


                            <asp:Label runat="server" ID="Label4" Text="Obligatorio" />:
                                                <asp:DropDownList ID="DdlObligatorio" runat="server" class="form-control">
                                                    <asp:ListItem Text="No " Value="0" />
                                                    <asp:ListItem Text="Si" Value="1" />

                                                </asp:DropDownList>

                            <asp:Label Visible="false" runat="server" ID="lblTipoC" Text="Trabajador/Empresa" />
                                                <asp:DropDownList Visible="false" ID="DdlTrabajador" runat="server" class="form-control" AutoPostBack="True">

                                                    <asp:ListItem Text="Empresa" Value="0" />
                                                    <asp:ListItem Text="Trabajador" Value="1" />

                                                </asp:DropDownList>

         


       
                                                        <br />
       



                        </div>
                    </div>




                </div>

                <div class="panel-footer">
                    <div class="text-center">



                        <asp:HiddenField runat="server" ID="flag_Modificar" />
                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-primary" Text="Cancelar" UseSubmitBehavior="false" />

                        <asp:Button ID="btnGuardarNuevaSolicitud" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia_Click"  Text="Grabar" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />


                        <asp:Button visible="false" ID="btnLimpiarCampos" runat="server" CausesValidation="false" class="btn btn-primary" OnClick="btnLimpiarCampos_Click" Text="Limpiar campos" />

                    </div>
                </div>
                <br />
                <br />

            </asp:View>

        </asp:MultiView>


    </div>



</asp:Content>

