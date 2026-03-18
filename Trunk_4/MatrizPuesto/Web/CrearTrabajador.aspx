<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_plantas.Master" CodeBehind="CrearTrabajador.aspx.vb" Inherits="AdokWeb.CrearTrabajador" %>

<%@ MasterType VirtualPath="~/Adok_plantas.Master" %>


<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">


    <script type="text/javascript">




        function ValidateTarjeta(source, args) {

            var dni = args.Value;
            var numero
            var letr
            var letra
            var expresion_regular_dni

            expresion_regular_dni = /^\d{8}[a-zA-Z]$/;

            if (expresion_regular_dni.test(dni) == true) {
                numero = dni.substr(0, dni.length - 1);
                letr = dni.substr(dni.length - 1, 1);
                numero = numero % 23;
                letra = 'TRWAGMYFPDXBNJZSQVHLCKET';
                letra = letra.substring(numero, numero + 1);
                if (letra != letr.toUpperCase()) {
                    alert('Dni erroneo, la letra del NIF no se corresponde. Correcta sería: ' + letra);
                    args.IsValid = false;
                    return false;
                } else {
                    //  alert('Dni correcto');
                    args.IsValid = true;
                    return true;
                }
            } else {
                //  alert('Dni erroneo, formato no válido');
                args.IsValid = false;
                return false;
            }


        }


<%--        function ValidateTarjetaFinal(source, args) {

            var tarjeta = document.getElementById('<%=txtTarjeta.ClientID%>').value;

            var chkTarjeta = document.getElementById('<%= chkTarjeta.ClientID%>');

            if (chkTarjeta.checked) {

                if (tarjeta == "") {
                    alert('Indique número de tarjeta');
                    return false;
                } else {


                    return true;
                }


            }
            return true;
        }--%>


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
            } else {ValidateDNI
                args.IsValid = false;
                return false;
            }

        }




        function ValidateDNI(source, args) {
            var dni = args.Value;
            var numero
            var letr
            var letra
            var expresion_regular_dni

            expresion_regular_dni = /^\d{8}[a-zA-Z]$/;

            if (expresion_regular_dni.test(dni) == true) {
                numero = dni.substr(0, dni.length - 1);
                letr = dni.substr(dni.length - 1, 1);
                numero = numero % 23;
                letra = 'TRWAGMYFPDXBNJZSQVHLCKET';
                letra = letra.substring(numero, numero + 1);
                if (letra != letr.toUpperCase()) {
                    alert('Dni erroneo, la letra del NIF no se corresponde. Correcta sería: ' + letra);
                    args.IsValid = false;
                    return false;
                } else {
                    //  alert('Dni correcto');
                    args.IsValid = true;
                    return true;
                }
            } else {
                //  alert('Dni erroneo, formato no válido');
                args.IsValid = false;
                return false;
            }

        }



        function ValidateModeNumber(source, args) {
            return;

        };


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
            var hfIdUsuario = document.getElementById('<%=hfTra.ClientID%>');
            var hfIdUsuario2 = document.getElementById('<%=hfEmpresa.ClientID%>');
            hfIdUsuario2.value = eventArgs.get_value();
            hfIdUsuario.value = "";
        }

        function RecogerTra(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresa.ClientID%>');
            var hfIdUsuario2 = document.getElementById('<%=hfTra.ClientID%>');
            hfIdUsuario2.value = eventArgs.get_value();
            hfIdUsuario.value = "";
        }

        function RecogerResponsable(source, eventArgs) {
            var hfResponsable = document.getElementById('<%=hfResponsable.ClientID%>');
            hfResponsable.value = eventArgs.get_value();
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">


    <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="Mantenimiento de trabajadores" /></a>
        </div>
    </div>

    <div class="container">

        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">

            <asp:View ID="viewListado" runat="server">


                <%--          <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblEmpresa" runat="server" Text="Mantenimiento de trabajadores" /></h2>


                </div>--%>

                <br />






                <asp:GridView Visible="false" ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-condensed small-top-margin"
                    AllowPaging="false"
                    GridLines="None" Width="132%" OnRowCommand="gvType_OnRowCommand" DataKeyNames="Id" ShowFooter="false"
                    OnRowCancelingEdit="gvType_RowCancelingEdit" OnRowEditing="gvType_RowEditing" OnRowUpdating="gvType_RowUpdating" Height="10px" PageSize="5" Style="margin-left: 1px">
                    <%--<RowStyle CssClass="RowStyle td a" HorizontalAlign="Left" />--%>
                    <%--<HeaderStyle CssClass="HeaderStyle" />--%>


                    <SelectedRowStyle CssClass="SelectedRowStyle" />


                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="left">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" CommandName="Edit" />
                            </ItemTemplate>


                        </asp:TemplateField>



                    </Columns>


                </asp:GridView>





                <%--<hr class="auto-style29" />--%>

                <div class="row">


                    <div class="col-xs-4" style="text-align: left;">
                        <asp:Label runat="server" ID="lblTra" Text="Búsqueda por trabajador" />:



                        <asp:TextBox ID="txtTra" runat="server" class="form-control" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="CargarTra"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerTra"
                            TargetControlID="txtTra" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfTra" runat="server" />




                    </div>

                    <div class="col-xs-5">

                        <asp:Label runat="server" ID="ex11" Text="Búsqueda por puesto" />:

                        <asp:TextBox ID="txtEmpresa" runat="server" class="form-control" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarEmpresas"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfEmpresa" runat="server" />


                    </div>
                </div>



                <br />



                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="true"   PageSize="10"
                    GridLines="None" OnRowCommand="gvType2_OnRowCommand" DataKeyNames="Id" ShowFooter="false" OnDataBound="gvType2_DataBound"
                    OnRowCancelingEdit="gvType2_RowCancelingEdit" OnRowEditing="gvType2_RowEditing" OnRowUpdating="gvType2_RowUpdating">
                    <%--<HeaderStyle  CssClass="HeaderStyle"  />--%>
                    <%--<RowStyle  HorizontalAlign="Center"  Font-Bold="True"  />--%>
                    <EditRowStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#ffffcc" />
                     <PagerStyle CssClass="PagerStyle" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
                 <%--   <PagerTemplate>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-2" style="text-align: right;">

                                <h5>
                                    <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                            </div>
                            <div class="col-lg-1" style="text-align: left;">

                                <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged2" runat="server" CssClass="form-control" /></h3>
                            </div>
                            <div class="col-lg-8" style="text-align: right;">
                                <h4>
                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                            </div>
                        </div>
                    </PagerTemplate>--%>
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" Visible="false" />
                            </ItemTemplate>

                        </asp:TemplateField>


<%--                        <asp:TemplateField HeaderText="Num.">
                            <ItemTemplate>
                                             <asp:CheckBox ID="chkTarjeta" Checked='<%# If(Eval("tarjeta") <> "", 1, 0) %>' Enabled="false" runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtModTarjeta" runat="server" Text='<%# Eval("tarjeta")%>' class="form-control required" />
                  
                            </EditItemTemplate>
                        </asp:TemplateField>--%>

<%--                        <asp:TemplateField HeaderText="Tarjeta" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Tarjeta" CssClass="btn btn-info" CommandName="Edit" CausesValidation="false" UseSubmitBehavior="false" />

                             </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="Grabar" CssClass="btn btn-success" CommandName="Update" CommandArgument='<%# Container.DataItemIndex%>' />                            
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="btn btn-default" CommandName="Cancel" CausesValidation="false" UseSubmitBehavior="false" />
                                <asp:Button ID="btnBorrarTar" runat="server" Text="Baja Tarjeta" CssClass= "  btn btn-warning" CommandName="BorrarTar" CommandArgument='<%# Container.DataItemIndex%>' />


                            </EditItemTemplate>
                        </asp:TemplateField>--%>





                        <asp:TemplateField HeaderText=" Seleccione el trabajador a modificar">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" Font-Underline="true" CommandName="Modificar" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <%--<asp:Label ID="lblActivado" Text='<%# If(Eval("Activo") = 1, "Activo", "Desactivado") %>' runat="server"  />--%>
                        <asp:TemplateField HeaderText="Fecha Caducidad">
                            <ItemTemplate>
                                    <asp:Label ID="lblFechaCad" Text='<%# Eval("FechaFin")%>' runat="server"  />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActivo" Checked='<%# If(Eval("Activo") = 1, 0, 1) %>' Enabled="false" runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <%--                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" runat="server" Text="Modificar" CommandName="Modificar" />
                            </ItemTemplate>

                        </asp:TemplateField>--%>





                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Desactivar" Visible='<%# If(Eval("Activo") = 1, "false", "true") %>' />
                                <act:ConfirmButtonExtender ID="cbeEliminar3" runat="server" DisplayModalPopupID="mpeEliminar3"
                                    TargetControlID="btnEliminar3">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar3" runat="server" PopupControlID="pnlConfirmDelete2" TargetControlID="btnEliminar3" OkControlID="btnBorrar3"
                                    CancelControlID="btnCancelar3" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete2" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion3" runat="server" Text="Confirmación" />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado3" runat="server" Text="¿Estás seguro que quieres marcar como inactivo al trabajador?" />
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
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Activar" Text="Activar" Visible='<%# If(Eval("Activo") = 1, "true", "false") %>' />
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
                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text="¿Estás seguro que quieres marcar como activo al trabajador?" />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <asp:Button ID="btnBorrar2" runat="server" CssClass="btn btn-primary" Text="Si" />
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="No" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>


                    </Columns>


                </asp:GridView>




            </asp:View>




            <asp:View ID="view2" runat="server">



                <%--              <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="Mantenimiento de datos" /></h2>


                </div>--%>

                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label runat="server" ID="ex21" Text="DNI" />



                            <asp:TextBox ID="txtCIF" runat="server" class="form-control required" MaxLength="10" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ErrorMessage="Añade el DNI" ControlToValidate="txtCIF" ValidationGroup="CamposVacios" Display="None" />
                    
          <%--  lo quito porque hay dni raros que no lo cumplen                          <asp:RegularExpressionValidator ID="revDNI" runat="server" ControlToValidate="txtCIF" ValidationGroup="CamposVacios" Font-Italic="true" Display="None"
                                SetFocusOnError="true" ErrorMessage="DNI erróneo. Formato: nnnnnnnnA" ValidationExpression="^((([0-9]|[0-9])\d{7}[A-Z]|[a-z])|(\d{8}([A-Z]|[a-z])))$" />

                            <act:ValidatorCalloutExtender ID="vceCIF" runat="server" TargetControlID="revDNI" PopupPosition="Right" />--%>
                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1" PopupPosition="Right" />


                            <asp:Label runat="server" ID="Label23" Text="Nombre" />:
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control required" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="*" ErrorMessage="Añade el nombre del trabajador" ControlToValidate="txtNombre" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="Right" />


                            <asp:Label runat="server" ID="Label22" Text="Apellidos" />:
                            <asp:TextBox ID="txtApellidos" runat="server" class="form-control required" />
                            <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" Text="*" ErrorMessage="Añade los apellidos" ControlToValidate="txtApellidos" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceCodigo" runat="server" TargetControlID="rfvCodigo" PopupPosition="Right" />



                            <asp:Label runat="server" ID="lblAsignacionC" Text="Puesto" />:
                            <asp:DropDownList ID="DdlEmpresa" runat="server" class="form-control required">
                            </asp:DropDownList>


                            <%--  </div>
                          <div class="col-lg-1  col-lg-offset-2" style="text-align:center">--%>

                            <asp:Label runat="server" ID="Label13" Text="Autónomo" />:
                                
                            <asp:DropDownList ID="DdlAutonomo" runat="server" class="form-control required">
                                <asp:ListItem Text="No " Value="0" />
                                <asp:ListItem Text="Si" Value="1" />
                            </asp:DropDownList>
                            <%--<br />
                                </div>

                          <div class="col-lg-8 col-md-8 col-lg-offset-2">--%>


                            <asp:Label runat="server" ID="Label8" Text="Fecha inicio" />:&nbsp;&nbsp;&nbsp;&nbsp;
                           
 <div class='input-group date' id='imgCalendarioResolucionDesde3'>
     <asp:TextBox ID="TxtFechaIni" runat="server" class="form-control required" />
     <span class="input-group-addon">
         <span class="glyphicon glyphicon-calendar"></span>
     </span>
 </div>
                            <act:CalendarExtender ID="imgCalendarioResolucionDesde3_CalendarExtender" runat="server" TargetControlID="TxtFechaIni" PopupButtonID="imgCalendarioResolucionDesde3" />

                        <%--    <asp:CustomValidator ID="cvFechaRec" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaIni" ValidationGroup="CamposVacios" Display="None" />

                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="cvFechaRec" PopupPosition="Right" />--%>

                            <asp:Label ID="Label5" Text="Fecha fin" runat="server" />:&nbsp;&nbsp;&nbsp;&nbsp;
      
 <div class='input-group date' id='imgCalendarioResolucionDesde2'>
     <asp:TextBox ID="TxtFechaFin" runat="server" class="form-control" CausesValidation="True" />
     <span class="input-group-addon">
         <span class="glyphicon glyphicon-calendar"></span>
     </span>
 </div>

                            <act:CalendarExtender ID="imgCalendarioResolucionDesde2_CalendarExtender" runat="server" TargetControlID="TxtFechaFin" PopupButtonID="imgCalendarioResolucionDesde2" />


                        <%--    <asp:CustomValidator ID="cvFechaEnv" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaFin" ValidationGroup="CamposVacios" Display="None" />

                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="cvFechaEnv" PopupPosition="Right" />--%>


<%--                            <asp:Label runat="server" ID="Label16" Text="Puesto de trabajo" />:
                            <asp:TextBox ID="TxtPuesto" runat="server" class="form-control" />--%>

                            <asp:Label runat="server" ID="lblDescC" Text="Función a realizar" />:
                            <asp:TextBox ID="txtFuncion" runat="server" class="form-control" Rows="6" TextMode="MultiLine" />

                            <asp:Label runat="server" ID="Label19" Text="Responsable" />:
                           <asp:TextBox ID="txtResponsable" runat="server" class="form-control" />

                            <act:AutoCompleteExtender ID="AutoCompleteExtender2" ServiceMethod="CargarResponsable"
                                runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerResponsable"
                                TargetControlID="txtResponsable" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />

                            <asp:HiddenField ID="hfResponsable" runat="server" />


<%--                            <asp:Label  runat="server" ID="lblSolicitud" Text="Solicitudes" />

                           <asp:TextBox ID="txtSolicitud" runat="server" class="form-control"  TextMode="MultiLine"  ReadOnly="true">
                            </asp:TextBox>--%>




                            <%--
                            <asp:Label  runat="server" ID="Label18">¿Tendrá tarjeta?  />
                            <asp:CheckBox ID="chkTarjeta" class="form-control" runat="server" AutoPostBack="True" />





                            <%if (chkTarjeta.Checked = True) Then  %>


                            <asp:Label  runat="server" ID="Label20">Número de tarjeta:  />
                            <asp:TextBox class="form-control" ID="txtTarjeta" runat="server" />
                            
                            <asp:RequiredFieldValidator ID="cvTarje" runat="server" Text="*" ErrorMessage="Tarjeta Errónea" ControlToValidate="TxtTarjeta" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceTarjet" runat="server" TargetControlID="cvTarje" PopupPosition="Right" />


                            <%end If%>
                            --%>

                            <asp:HiddenField runat="server" ID="flag_Modificar" />
                            <asp:HiddenField runat="server" ID="flag_Actualizar" />
                            <asp:HiddenField runat="server" ID="HdnNombre" />
                            <asp:HiddenField runat="server" ID="HdnCIF" />
                            <asp:HiddenField runat="server" ID="hdnCodResp" />
                            <asp:HiddenField ID="hdnchecked" runat="server" />


                        </div>
                    </div>

                </div>

                <div class="panel-footer">


                    <div class="text-center">
                        <asp:Button ID="btnCancelar" class="btn btn-primary" runat="server" Text="Cancelar" UseSubmitBehavior="false" />
                        <asp:Button ID="btnGuardarNuevaSolicitud" class="btn btn-primary" runat="server" OnClick="btnGuardarNuevaReferencia_Click" Text="Grabar" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />
                        <asp:Button ID="btnLimpiarCampos" runat="server" CausesValidation="false" class="btn btn-primary" OnClick="btnLimpiarCampos_Click" Text="Limpiar campos" />



                    </div>

                </div>

            </asp:View>

        </asp:MultiView>



    </div>



</asp:Content>

