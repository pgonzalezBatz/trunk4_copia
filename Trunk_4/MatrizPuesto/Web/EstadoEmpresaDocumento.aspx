<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_plantas.Master" CodeBehind="EstadoEmpresaDocumento.aspx.vb" Inherits="AdokWeb.EstadoEmpresaDocumento" %>

<%@ MasterType VirtualPath="~/Adok_plantas.Master" %>


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


  <%--      function RecogerTra(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresa.ClientID%>');
        var hfIdUsuario2 = document.getElementById('<%=hfTra.ClientID%>');
        hfIdUsuario2.value = eventArgs.get_value();
        hfIdUsuario.value = "";
    }--%>





        function ValidateFich() {
            var fich = document.getElementById('<%=FuDoc.ClientID%>').value;
            //alert(fich);
            if (fich) {
                return true;
            } else {
                return false;
            }

        }

<%--        function DescargarPla() {

            var fich = document.getElementById('<%=idPlantilla0.ClientID%>').value;

            location.href = './Ficheros_Matriz_Puestos/' + fich;

        }--%>


        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }


    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">





    <asp:Button Visible="false" ID="btnShow" runat="server" Text="" OnClientClick="return ShowModalPopup()" />
    <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server"
        PopupControlID="pnlPopup" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="header">
            Certificado pendiente de entrega
        </div>
        <div class="body">
            <br />
            <br />

            <asp:Label ID="Label13" runat="server" Text="dddd" />

            <br />
            <br />

            <asp:Button ID="btnCancelar" class="btn btn-primary" runat="server" Text="No" OnClick="btnCancelar_Click" />
            <asp:Button class="btn btn-primary btn-group-vertical" CausesValidation="True" OnClick="btnGuardarNuevaReferencia_Click" runat="server" ID="Button1" Text="Si" />
        </div>
        <br />
        <br />
    </asp:Panel>










    <asp:Button Visible="false" ID="Button2" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender2" BehaviorID="mpe2" runat="server"
        PopupControlID="pnlPopup2" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup2" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="header">
            Certificado pendiente de entrega
        </div>
        <div class="body">
            <br />
            <br />
            <asp:Label runat="server" ID="Label16" Text="Descargue la plantilla. <br> Una vez leida debe subir el documento de recibo firmado" />:&nbsp;<br />
            <%--<asp:Label ID="Label14" runat="server" Text="" />--%>

            <br />
            <br />
            <asp:Button class="btn btn-primary btn-group-vertical" CausesValidation="True" OnClick="Descargar2_Click" runat="server" ID="botonSubir2" Text="Descargar Plantilla a leer" />
            <br />
            <%--<asp:Button class="btn btn-primary btn-group-vertical"  CausesValidation="True" OnClick="Descargar1_Click"  runat="server" ID="Button4" Text="Descargar confirmación de lectura"  />--%>
            <br />
            <%--<asp:Button ID="btnCancelar2" class="btn btn-primary" runat="server" Text="Hecho" OnClick="btnCancelar2_Click"   />--%>
        </div>
        <br />
        <br />
    </asp:Panel>






    <asp:Button Visible="false" ID="Button3" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton2" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender3" BehaviorID="mpe3" runat="server"
        PopupControlID="pnlPopup3" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup3" runat="server" CssClass="modalPopup" Style="display: none">
      <div class="header">
        Descarga de documentación
    </div>
        <div class="body">
            <br />
            <br />

            <asp:Label ID="Label15" runat="server" Text="Puedes descargarlo desde aquí" />

            <br />
            <br />


            <asp:Button class="btn btn-primary btn-group-vertical" CausesValidation="True" OnClick="btnGuardarNuevaReferencia3_Click" runat="server" ID="Button5" Text="Descargar" />
        </div>
        <br />
        <br />
    </asp:Panel>





    <asp:Button Visible="false" ID="Button4" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton3" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender4" BehaviorID="mpe4" runat="server"
        PopupControlID="pnlPopup4" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup4" runat="server" CssClass="modalPopup" Style="display: none">
         <div class="header">
                   <asp:Label ID="Label19" runat="server" Text="Aviso" />
           
        </div>
        <div class="body">
            <br />
            <br />

            <asp:Label ID="Label18" runat="server" Text="Tienes solicitudes sin trabajadores asignados" />

            <br />
            <br />
            
         <asp:Button ID="Button6" class="btn btn-primary" runat="server" Text="Cancelar" OnClick="btnCancelar4_Click" />
        </div>
        <br />
        <br />
    </asp:Panel>



    <asp:Button Visible="false" ID="Button7" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton4" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender5" BehaviorID="mpe5" runat="server"
        PopupControlID="pnlPopup7" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup7" runat="server" CssClass="modalPopup" Style="display: none">
         <div class="header">
                   <asp:Label ID="Label20" runat="server" Text="Aviso" />
           
        </div>
        <div class="body">
            <br />
            <br />

            <asp:Label ID="Label21" runat="server" Text="Seleccione la planta a administrar" />

            <br />
            <br />
            
            <asp:Button ID="Button8" class="btn btn-primary" runat="server" Text="Batz Coop" OnClick="btnBatz_Click" />
            <asp:Button class="btn btn-primary btn-group-vertical" CausesValidation="True" OnClick="btnArraluce_Click" runat="server" ID="Button9" Text="Arraluce" />
        </div>
        <br />
        <br />
    </asp:Panel>



    <div class="container-fluid" style="background-color: #ebebeb;">
        <div class="navbar-header">
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="Mantenimiento de Documentos por puesto" /></a>
        </div>
    </div>

    <div class="container">


        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">


            <asp:View ID="viewListado" runat="server">




                <br />

                <div class="row">
                    <div class="col-xs-4">
                        <asp:Label runat="server" ID="ex551" Text="Puesto" />

                        <asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True"  />
      <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarEmpresasActivas"
                            runat="server" MinimumPrefixLength="2" 
                             OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" 
                            CompletionListCssClass="CompletionListCssClass"  />	
		   


                        <asp:HiddenField ID="hfEmpresa" runat="server" />

                    </div>


 <%--       jon de momento, luego igual hago link a otro aspx            <div class="col-xs-4" style="text-align: left;">
                        <asp:Label runat="server" ID="lblTra" Text="Trabajador" />



                        <asp:TextBox ID="txtTra" runat="server" class="form-control" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="CargarTraEmp"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerTra"
                            TargetControlID="txtTra" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfTra" runat="server" />




                    </div>--%>



                </div>

                <br />





                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="true" PageSize="10"
                    GridLines="None" DataKeyNames="Id" ShowFooter="false" OnDataBound="gvType_DataBound" OnPageIndexChanging="PageDropDownList_SelectedIndexChanged"
                    OnRowEditing="gvType_RowEditing">
                    <%--<HeaderStyle  CssClass="HeaderStyle"  />--%>
                    <%--<RowStyle  HorizontalAlign="Center"  Font-Bold="True"  />--%>
                    <EditRowStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#ffffcc" />
                    <PagerStyle CssClass="PagerStyle" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc3" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
                    <%--                    <PagerTemplate>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-2" style="text-align: right;">

                                <h5>
                                    <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                            </div>
                            <div class="col-lg-1" style="text-align: left;">

                                <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
                            </div>
                            <div class="col-lg-8" style="text-align: right;">
                                <h4>
                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                            </div>
                        </div>
                    </PagerTemplate>--%>
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Descripción">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>







                        <asp:TemplateField HeaderText="Activa" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkObsoleto" Checked='<%# Eval("recibi")%>' Enabled="false" runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Documentos por Puesto">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" class="btn  bg-info" runat="server" ToolTip="Ver Documentos del puesto" Text="Documentos" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField Visible="false" HeaderText="Estado">
                            <ItemTemplate>
                                <asp:LinkButton ID="imgEstado" runat="server" ToolTip='<%# Eval("recibi")%>'
                                    class='<%# If(Eval("recibi") <> "0", "btn alert-danger", (If(Eval("activo") <> "0", "btn alert-warning", "btn alert-success"))) %>'><span 
    class='<%# If(Eval("recibi") <> "0", "glyphicon glyphicon-ban-circle", (If(Eval("activo") <> "0", "glyphicon glyphicon-warning-sign", "glyphicon glyphicon-check"))) %>'
         
         '></span></asp:LinkButton>

                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Errores">
                            <ItemTemplate>
                                <asp:Label ID="lblErrores" Text='<%# Eval("medio")%>' CssClass='<%# If(Eval("recibi") <> "0", "text-danger", (If(Eval("activo") <> "0", "text-warning", "text-success"))) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Estado Trabajadores">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbTrabajadores" runat="server" class="btn  bg-info" ToolTip="Ver el Estado de Documentos de los trabajadores" Text="Estado trabajadores" CommandName="Trabajadores" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField Visible="false" HeaderText="Con Errores">
                            <ItemTemplate>
                                <asp:Label ID="lblErroresT" Text='<%# Eval("medio2")%>' CssClass='<%# If(Eval("autonomo") > 0, "text-danger", (If(Eval("autonomo") > 1, "text-warning", "text-success"))) %>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>


                </asp:GridView>

                <asp:Panel runat="server" ID="pnlEmpresasInfo" Visible="false">
                    <div class="form-group col-sm-12">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <h4><span style="text-align: right;" class="label label-info">
                            <asp:Label ID="Label7" runat="server" Text="Total puestos" />:
                        </span>&nbsp;

                    <span class="label label-info">
                        <asp:Label ID="Label8" runat="server" Text="12" />
                    </span>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%--                     <span style="text-align: right;" class="label label-info" visible="false">
                         <asp:Label ID="Label11" runat="server" Text="Total Trabajadores" />:
                     </span>&nbsp;

                    <span class="label label-info" visible="false">
                        <asp:Label ID="Label12" runat="server" Text="123" />
                    </span>--%>

                        </h4>
                    </div>
                </asp:Panel>


            </asp:View>


            <asp:View ID="view1" runat="server">

                <%--               <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="Estado de documentos de " /></h2>




                </div>--%>


                <br />
                <div class="row">
                    <div class="col-xs-4">
                        <asp:Label runat="server" ID="ex16" Text="Puesto" />

                        <asp:TextBox class="form-control" type="text" ID="txtNombre" runat="server" ReadOnly="True" />
                    </div>
                    <div class="col-xs-3">

                        <asp:Label Visible="false" runat="server" ID="Cif2" Text="Cif" />
                        <asp:TextBox Visible="false" type="text" class="form-control" ID="txtCIF" runat="server" ReadOnly="True" />
                    </div>
                    <div class="text-center">
                        <br />
                        <asp:Button Visible="false" ID="btnTrabajadores" runat="server" OnClick="trabajadores_Click" Text="Ver trabajadores" class="btn btn-primary" UseSubmitBehavior="true" />
                    </div>
                </div>

                <br />

                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                    GridLines="None" DataKeyNames="clave" ShowFooter="false">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
                    <PagerTemplate>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-2" style="text-align: right;">

                                <h5>
                                    <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                            </div>
                            <div class="col-lg-1" style="text-align: left;">

                                <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
                            </div>
                            <div class="col-lg-8" style="text-align: right;">
                                <h4>
                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                            </div>
                        </div>
                    </PagerTemplate>

                    <Columns>
                        <asp:BoundField DataField="clave" HeaderText="clave" Visible="false" />
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Visible='<%# Eval("vacio")%>' Text='<%# Eval("clave")%>' runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText=" Tipo" >
                            <ItemTemplate>
                                <asp:Label ID="lblTipo" Text='<%# Eval("Nombre")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField Visible="false" HeaderText="Documento" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="lblDescripcion" Text='<%# Eval("Abrev")%>' ToolTip='<%# Eval("Nombre")%>' title='<%# Eval("Nombre")%>' />
                                <%--<asp:button ID="lblDescripcion" runat="server" Text="Bottom tooltip" CssClass="btn btn-warning btn-sm tooltips" data-placement="bottom" title="Tooltip on bottom" />--%>
                                <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                    TargetControlID="lblDescripcion">
                                </act:ConfirmButtonExtender>
                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="lblDescripcion"
                                    CancelControlID="btnCancelar2" BackgroundCssClass="modalBackground">
                                </act:ModalPopupExtender>
                                <asp:Panel ID="pnlConfirmDelete" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        <asp:Label ID="lblConfirmacion" runat="server" Text='<%# Eval("Abrev")%>' />
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text='<%# Eval("Nombre")%>' />
                                    </div>
                                    <div class="footer" style="text-align: center;">
                                        <%--<asp:Button ID="btnBorrar2" visible="false" runat="server" CssClass="btn btn-primary" Text="Si" />--%>
                                        <asp:Button ID="btnCancelar2" runat="server" CssClass="btn btn-primary" Text="OK" />
                                    </div>
                                </asp:Panel>

                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField Visible="false" HeaderText="Fecha Recepción" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblFecRecepcion" Text='<%# If(Eval("FecRec") = Date.MinValue, "", Left(Eval("FecRec").ToShortDateString, 10)) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Fecha Caducidad" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblFecCaducidad" Text='<%# If(Eval("FecCad") = Date.MinValue Or Eval("FecCad") > DateAdd(DateInterval.Month, -1, Date.MaxValue) Or Eval("FecCad") = CDate("01/01/1900"), "", Left(Eval("FecCad").ToShortDateString, 10)) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField Visible="false" HeaderText="Autor" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblUbicación" Text='<%# Eval("ubicacionfisica")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Observaciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblObservaciones" Text='<%# Eval("estado")%>' runat="server" Visible='<%# Eval("nomemp")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>



                        <asp:TemplateField Visible="false" HeaderText="Estado" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>



                                <asp:LinkButton Visible='<%# Eval("vacio")%>' ID="imgEstado" runat="server" ToolTip='<%# Eval("txtcorrecto")%>'
                                    class='<%# If(Eval("txtcorrecto") = "Correcto", "btn alert-success", (If(Eval("txtcorrecto") = "Errores", "btn alert-danger", "btn alert-warning"))) %>'><span 
    class='<%# If(Eval("txtcorrecto") = "Correcto", "glyphicon glyphicon-check", (If(Eval("txtcorrecto") = "Errores", "glyphicon glyphicon-ban-circle", "glyphicon glyphicon-warning-sign"))) %>'
         
         '></span></asp:LinkButton>


                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField Visible="false" HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton ID="imgEstado2" runat="server"><span 
    class='<%# If(Eval("txtcorrecto") = "Correcto", "", (If(Eval("txtcorrecto") = "Errores", "glyphicon glyphicon-arrow-right", ""))) %>'         
         '></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton ID="ibVer" OnClick="ibVer_Click" runat="server" class="btn  active" ToolTip="Ver Documento" Visible='<%# Eval("ubicacion")%>'><span class="glyphicon glyphicon-file" ></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Subir" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton ID="ibSubir" OnClick="ibSubir_Click" runat="server" class="btn boton" ToolTip="Subir Documento" Visible='<%# Eval("nomemp")%>'><span class="glyphicon glyphicon-upload"></span></asp:LinkButton>



                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Hist." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>


                                <asp:LinkButton ID="ibHistorico" OnClick="ibHistorico_Click" CausesValidation="false" runat="server" class="btn  btn" ToolTip="Ver historico" Visible='<%# Eval("ubicacion")%>'><span class="glyphicon glyphicon-folder-open"></span></asp:LinkButton>



                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Validación" ItemStyle-HorizontalAlign="Center">

                            <ItemTemplate>

                                <asp:LinkButton ID="ibModificar" OnClick="ibModificar_Click" CausesValidation="false" runat="server" class="btn boton" ToolTip='<%# Eval("Comentario")%>' Visible='<%# Eval("ubicacion")%>'><span class="glyphicon glyphicon-edit"></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Plantilla" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton ID="ibPlantilla" OnClick="ibPlantilla_Click" runat="server" class="btn bg-info" ToolTip="Descargar Plantilla" Visible='<%# If(Eval("plantilla") = 1, "True", "False") %>'><span class="glyphicon glyphicon-list-alt"></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>


                        <%--HeaderText="Plantilla lectura" ItemStyle-HorizontalAlign="Center"--%>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>

                                <asp:LinkButton ID="ibPlantilla2" OnClick="ibPlantilla2_Click" runat="server" class="btn bg-info" ToolTip="Descargar Plantilla de Lectura" Visible='<%# If(Eval("EsDocumento") = 4, "True", "False") %>'><span class="glyphicon glyphicon-list-alt"></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>




                    </Columns>


                </asp:GridView>



                <hr />
                <div class="form-inline row">
                    <div class="form-group col-sm-5">
                        <asp:Button class="btn btn-primary" runat="server" OnClick="btoVolver_click" ID="btoVolver" Text="&nbsp;&nbsp;Volver &nbsp;&nbsp;" />
                        <%--<input  type="button"   class="btn bg-warning"  value=" &nbsp;&nbsp;Volver &nbsp;&nbsp;"  onclick="history.go(-1);" />--%>
                    </div>
                    <div class="form-group col-sm-7">

                        <h4><span style="text-align: left;" class="label label-info">
                            <asp:Label ID="Label10" runat="server" Text="Total documentos:" />
                        </span>&nbsp;

                    <span class="label label-info">
                        <asp:Label ID="Registros" runat="server" Text="" />
                    </span>

                        </h4>
                    </div>
                </div>
                <hr />








            </asp:View>



            <asp:View ID="view2" runat="server">


                <%--          <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label5" runat="server" Text="Modificación de datos" /></h2>



                </div>
                --%>
                <br />
                <div class="panel-body">


                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label runat="server" ID="Label14" Text="Puesto" />

                            <asp:TextBox ID="txtNombre3" runat="server" class="form-control" ReadOnly="True" />

                        </div>

                    </div>

                    <br />
                    <br />




                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">


                            <asp:Label runat="server" ID="lblNombre" Text="Documento" />:
                                          
                                 <asp:TextBox ReadOnly="True" ID="txtNombreDoc" runat="server" class="form-control" />
                            <br />

                            <%  If System.Configuration.ConfigurationManager.AppSettings("extranet").ToString = "0" Then   %>


                            <asp:Label runat="server" ID="txtfecrec" Text="Fecha recepción" />:
                            
                            <asp:TextBox ReadOnly="True" class="form-control required" type="text" ID="TxtFechaRec" runat="server" title="FechaValidez" required="required" />




                            <br />
                            <asp:Label runat="server" ID="Label1" Text="Periodicidad" />:
                            <br />




                            <asp:DropDownList class="form-control" ID="ddlCaducidad" Width="100%" Height="32" runat="server" Enabled="true" AutoPostBack="True">
                            </asp:DropDownList>



                            <br />

                            <asp:Label runat="server" ID="Label2" Text="Válido desde" />:

                           

                            <%          If (TxtFechaVal.Text <> "Documento sin caducidad") And (TxtFechaVal.Text <> "Documento de plantilla") Then  %>

                            <asp:LinkButton ID="imgCalendarioFechaVal" runat="server" class="btn boton" ToolTip="Fecha Validez" Visible='<%# Eval("ubicacion")%>'><span class="glyphicon glyphicon-calendar"></span></asp:LinkButton>
                            <act:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgCalendarioFechaVal" TargetControlID="TxtFechaVal" />
                            <%--    <asp:CustomValidator ID="CustomValidator1" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaVal" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="CustomValidator1" PopupPosition="Right" />--%>
                            <%end If%>

                            <asp:TextBox ReadOnly="True" class="form-control" type="text" ID="TxtFechaVal" runat="server" required="required" />
                            <br />

                            <asp:Label runat="server" ID="LblFechaVal2" Text="Fecha de caducidad" />:

                            <asp:TextBox class="form-control" ID="TxtFechaVal2" runat="server" ReadOnly="True" />

                            <br />

                            <asp:Label runat="server" ID="Label3" Text="Subido por" />:

                            <asp:TextBox class="form-control" ID="txtUbicacion" runat="server" ReadOnly="True" />
                            <%end If%>
                            <br />
                            <asp:Label runat="server" ID="Label4" Text="Estado" Font-Bold="true" Font-Underline="true" />:


                            <asp:RadioButtonList CssClass=" btn-group-justified " Width="50%" ID="rblCorrecto" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Correcto." Value="0" Selected="True" />
                                <asp:ListItem Text="Incorrecto." Value="1" />
                                <asp:ListItem Text="No Validado." Value="2" />
                            </asp:RadioButtonList>
                            <%--<%          If rblCorrecto.SelectedValue = 1 Then  %>--%>

                            <asp:TextBox class="form-control" ID="txtComentario" runat="server" Rows="6" TextMode="MultiLine" />

                            <%--<%end If%>--%>
                            <br />


                         


                            <asp:Panel ID="PanelX" runat="server">




                                <asp:Label runat="server" ID="Label6" Text="Deuda" Font-Bold="true" Font-Underline="true" />:


                                <asp:RadioButtonList CssClass=" btn-group-justified " Width="70%" ID="rblImpuestos" runat="server" AutoPostBack="false">
                                    <asp:ListItem Text="Sin deuda." Value="1" />
                                    <asp:ListItem Text="Deuda." Value="2" />
                                    <asp:ListItem Text="Deuda aplazada" Value="3" />
                                    <asp:ListItem Text="No validado" Value="4" Selected="True" />
                                </asp:RadioButtonList>



                            </asp:Panel>

                         
                        </div>
                    </div>




                    <br />







                </div>





                <div class="panel-footer">
                    <div class="text-center">


                        <asp:Button ID="CancelVista3" runat="server" class="btn btn-primary" Text="Cancelar" UseSubmitBehavior="false" />

                        <asp:Button ID="GrabarMod" runat="server" class="btn btn-primary" OnClick="GrabarMod_Click" Text="Grabar" OnClientClick="return true;" ValidationGroup="CamposVacios" />

                        <asp:Button ID="VerDoc" runat="server" class="btn btn-primary" OnClick="ibVer_Click2" Text="Ver Documento" />

                    </div>
                </div>
                <br />
            </asp:View>




            <asp:View ID="view3" runat="server">



                <div class=" panel-header">

                    <%--                <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label7" runat="server" Text="Histórico de documentos de " /></h2>




                    <br />--%>
                    <br />
                    <div class="row">
                        <div class="col-xs-4">
                            <asp:Label runat="server" ID="ex12" Text="Puesto" />

                            <asp:TextBox class="form-control" type="text" ID="txtNombreHis" runat="server" ReadOnly="True" />
                        </div>
                        <div class="col-xs-2">

                            <asp:Label runat="server" ID="Cif" Text="Cif" />
                            <asp:TextBox type="text" class="form-control" ID="txtCIFHis" runat="server" ReadOnly="True" />
                        </div>




                        <div class="col-xs-4">

                            <asp:Label runat="server" ID="ex17" Text="Documento" />

                            <asp:TextBox class="form-control" ID="txtDocuEmp" runat="server" ReadOnly="True" />

                        </div>
                    </div>


                </div>


                <div class="panel-body">


                    <asp:GridView ID="gvTypeHis" runat="Server" AutoGenerateColumns="False" AllowPaging="true" PageSize="15" CssClass="table table-striped"
                        GridLines="None" DataKeyNames="clave" ShowFooter="false" EmptyDataText="No data in the data source.">
                        <%--<HeaderStyle HorizontalAlign="Center" BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                        <EditRowStyle BackColor="#ffffcc" />
                        <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                        </EmptyDataTemplate>
                        <%--Paginador...--%>
                        <%--                        <PagerTemplate>
                            <div class="row" style="margin-top: 20px;">
                                <div class="col-lg-2" style="text-align: right;">

                                    <h5>
                                        <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                                </div>
                                <div class="col-lg-1" style="text-align: left;">

                                    <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
                                </div>
                                <div class="col-lg-8" style="text-align: right;">
                                    <h4>
                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                                </div>
                            </div>
                        </PagerTemplate>--%>
                        <Columns>
                            <asp:BoundField DataField="clave" HeaderText="clave" Visible="false" />
                            <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" Text='<%# Eval("clave")%>' runat="server" />
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>




                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lblFecha" Text='<%# Eval("Abrev")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Documento" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescripcion" Text='<%# Eval("ubicacion")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>



                            <asp:TemplateField HeaderText="Ver Documento">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibVer" OnClick="ibVer_HIST_Click" ImageUrl="~/App_Themes/Batz/IconosAcciones/dcto0.gif"
                                        runat="server" ToolTip="Ver Documento" />

                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Borrar" />
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
                                            <asp:Label ID="lblConfirmarBorrado3" runat="server" Text="¿Estás seguro que quieres borrar el documento?" />
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


                </div>

                <div class="panel-footer">
                    <div class="text-center">



                        <asp:Button class="btn btn-primary" ID="CancelVista" runat="server" Text="Volver" UseSubmitBehavior="false" />



                    </div>
                </div>





            </asp:View>







            <asp:View ID="view4" runat="server">



                <%--               <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label8" runat="server" Text="Subir documentos de " /></h2>



                </div>--%>
                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <br />
                            <asp:Label runat="server" ID="txtNombre2" Text="Puesto" />:
                            <asp:TextBox class="form-control" type="text" ID="txtEmpDoc" runat="server" ReadOnly="True" />
                            <br />
                            <asp:Label runat="server" ID="txtApe1" Text="Tipo de Documento" />:
                            <asp:TextBox class="form-control" type="text" ID="txtDocEmp" runat="server" ReadOnly="True" />
                            <br />
                            <asp:Label runat="server" ID="txtApe2" Text="Fecha del Ultimo Documento Subido" />:
                            <asp:TextBox class="form-control" type="text" ID="FecUltDoc" runat="server" ReadOnly="True" />
                            <br />



                            <%          If (txtDocEmp.Text <> "Recibo de Liquidacion de cotizaciones (TC1)" And txtDocEmp.Text <> "Relacion Nominal de Trabajadores (RNT)") Then  %>



                            <asp:Label runat="server" ID="txtFechVal" Text="Fecha inicio de Validez" />:&nbsp;&nbsp;&nbsp;



                            <%          If (TxtFechaValidez.Text <> "Documento sin caducidad") Then  %>

                            <asp:LinkButton ID="imgCalendarioFechaValidez" runat="server" class="btn boton" ToolTip="Fecha Validez"><span class="glyphicon glyphicon-calendar"></span></asp:LinkButton>
                            <act:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgCalendarioFechaValidez" TargetControlID="TxtFechaValidez" />
                            <%end If%>



                            <asp:TextBox class="form-control required" type="text" ID="TxtFechaValidez" runat="server" ToolTip="FechaValidez" required="required" />

                            <br />


                            <%else%>

                            <asp:Label runat="server" ID="Label17" Text="Seleccione el periodo de validez" />:&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList class="form-control" ID="ddlMes" runat="server" Enabled="true" required="required">
                                    <asp:ListItem Text="" Value="0" />
                                    <asp:ListItem Text="01-Enero" Value="1" />
                                    <asp:ListItem Text="02-Febrero" Value="2" />
                                    <asp:ListItem Text="03-Marzo" Value="3" />
                                    <asp:ListItem Text="04-Abril" Value="4" />
                                    <asp:ListItem Text="05-Mayo" Value="5" />
                                    <asp:ListItem Text="06-Junio" Value="6" />
                                    <asp:ListItem Text="07-Julio" Value="7" />
                                    <asp:ListItem Text="08-Agosto" Value="8" />
                                    <asp:ListItem Text="09-Septiembre" Value="9" />
                                    <asp:ListItem Text="10-Octubre" Value="10" />
                                    <asp:ListItem Text="11-Noviembre" Value="11" />
                                    <asp:ListItem Text="12-Diciembre" Value="12" />

                                </asp:DropDownList>
                            <br />
                            <%end If%>



                   

                            <asp:Label runat="server" ID="Label5" Text="Periodicidad" />:
                            <br />


                            <asp:DropDownList class="form-control" ID="ddlCaducidad2" runat="server" Enabled="true">
                            </asp:DropDownList>

                           

                            <br />



                            <asp:Label runat="server" ID="txtFich" Text="Selecciona el documento a subir" />:&nbsp;
                            <asp:FileUpload runat="server" ID="FuDoc" Height="37px" Width="579px" Font-Bold="True" ForeColor="White" required="required" />
                            <br />
                        </div>

                    </div>
                </div>
                <div class="panel-footer">
                    <div class="text-center">

                        <asp:Button ID="btnVolver" runat="server" class="btn btn-primary" Text="Cancelar" UseSubmitBehavior="false" />

                        <asp:Button class="btn btn-primary" runat="server" ID="btnSubir2" Text="Subir Documento" OnClientClick="return ValidateFich();" ValidationGroup="CamposVacios" />

                        <%--<asp:Button class="btn btn-primary" ID="btnVolver" runat="server" Text="Volver" CausesValidation="false" OnClientClick="javascript: return true;" />--%>

                        <%--<asp:Button class="btn btn-primary" runat="server" ID="btnDescargarPlantilla" Text="Descargar Plantilla" OnClientClick="javascript: return DescargarPla();" />--%>
                        &nbsp;<asp:HiddenField ID="idPlantilla0" runat="server" />

                    </div>
                </div>







            </asp:View>



            <asp:View ID="view5" runat="server">
            </asp:View>

        </asp:MultiView>




        <asp:HiddenField runat="server" ID="idEmpresa" />
        <asp:HiddenField runat="server" ID="DescEmpresa" />
        <asp:HiddenField runat="server" ID="idDoc" />
        <asp:HiddenField runat="server" ID="idPlantilla" />

        <asp:HiddenField runat="server" ID="hfurl" />

        <asp:HiddenField runat="server" ID="FechaValG" />
        <asp:HiddenField runat="server" ID="FechaValG2" />




    </div>

</asp:Content>

