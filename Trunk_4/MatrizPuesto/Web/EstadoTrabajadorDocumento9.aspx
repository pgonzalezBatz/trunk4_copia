<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_plantas9.Master" CodeBehind="EstadoTrabajadorDocumento9.aspx.vb" Inherits="AdokWeb.EstadoTrabajadorDocumento9" %>

<%@ MasterType VirtualPath="~/Adok_plantas9.Master" %>


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



        function ValidateFich() {
            var fich = document.getElementById('<%=fuDoc.ClientID%>').value;
            //alert(fich);
            if (fich) {
                return true;
            } else {
                return false;
            }

        }

        function RecogerTra(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresa.ClientID%>');
        var hfIdUsuario2 = document.getElementById('<%=hfTra.ClientID%>');
        hfIdUsuario2.value = eventArgs.get_value();
        hfIdUsuario.value = "";
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
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe">&nbsp;</span>&nbsp;<asp:Label ID="Label10" runat="server" Text="Mantenimiento de Documentos de Trabajador" /></a>
        </div>
    </div>
    <div class="container">

        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">


            <asp:View ID="viewListado" runat="server">


                <%--               <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label1" runat="server" Text="Estado de documentos de trabajador" /></h2>


                </div>--%>
                <br />

                 
                <div class="row">
                    <div class="col-xs-4">
                        <asp:Label runat="server" ID="ex177" Text="Trabajador" />:

                        <asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarTrabajadores"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />
                         
                        <asp:HiddenField ID="hfEmpresa" runat="server" />    

                        </div>

                    <div class="col-xs-4" style="text-align: left;">
                        <asp:Label runat="server" ID="lblTra" Text="Trabajador" />



                        <asp:TextBox ID="txtTra" runat="server" class="form-control" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="CargarTraEmp"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerTra"
                            TargetControlID="txtTra" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfTra" runat="server" />




                    </div>

                </div>

                <br />


                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="true" PageSize="15"
                    GridLines="None" DataKeyNames="Id" ShowFooter="false"
                    OnDataBound="gvType_DataBound"
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
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                            </ItemTemplate>


                            <ItemStyle HorizontalAlign="Center" />

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Puesto">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpresa" Text='<%# Eval("DescEmpresa")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fecha Caducidad" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFechaCad" Text='<%# Eval("FechaFin")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Estado Trabajador">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" runat="server" class="btn  bg-info" ToolTip="Estado de Documentos de trabajador" Text="Estado de Documentos" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <asp:LinkButton ID="imgEstado" runat="server" ToolTip='<%# Eval("responsable")%>'
                                    class='<%# If(Eval("responsable") <> "0", "btn alert-danger", (If(Eval("activo") <> "0", "btn alert-warning", "btn alert-success"))) %>'><span 
    class='<%# If(Eval("responsable") <> "0", "glyphicon glyphicon-ban-circle", (If(Eval("activo") <> "0", "glyphicon glyphicon-warning-sign", "glyphicon glyphicon-check"))) %>'
         
         '></span></asp:LinkButton>

                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Errores">
                            <ItemTemplate>
                                <asp:Label ID="lblErrores" Text='<%# Eval("puesto")%>' CssClass='<%# If(Eval("responsable") <> "0", "text-danger", (If(Eval("activo") <> "0", "text-warning", "text-success"))) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>



                    </Columns>


                </asp:GridView>

                <hr />


                <%--<div class="form-inline row">--%>
                <div class="form-group col-sm-12" style="text-align: center;">

                    <asp:Button ID="botonVolver" runat="server" class="btn btn-primary" Text="Volver" UseSubmitBehavior="false" OnClick="Volver" Visible="false" />

                </div>
                <hr />
                <%--          <div class="form-group col-sm-7">

             <h4 >             <span  style="text-align: left;" class="label label-info">Total documentos: </span>

                    <span class="badge">
                        <asp:Label ID="Label2" runat="server" Text="" />
                    </span>
              
             </h4>
                    </div>--%>
                <%--</div>--%>



                <%--<hr />--%>

                <%--   <h4 style="text-align: center;"><span class="label label-info">Total trabajadores: </span>

                    <span class="badge">
                        <asp:Label ID="Registros2" runat="server" Text="" />
                    </span>
                </h4>--%>
                <%--<hr />--%>
            </asp:View>


            <asp:View ID="view1" runat="server">

                <%--                <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label7" runat="server" Text="Estado de documentos de trabajador" /></h2>


                </div>


                <br />--%>

                <br />
                <div class="row">
                    <div class="col-xs-4">
                        <asp:Label runat="server" ID="ex61" Text="Trabajador" />

                        <asp:TextBox ID="txtNombre" runat="server" class="form-control" ReadOnly="True" />

                    </div>
                    <div class="col-xs-3">

                        <asp:Label runat="server" ID="Cif2" Text="Nif" />

                        <asp:TextBox ID="txtCIF" runat="server" class="form-control" ReadOnly="True" />



                    </div>

                       <div class="col-xs-3">
                              <asp:Label runat="server" ID="Label8" Text=" " /><br />
                     <asp:Button class="btn btn-primary" runat="server" OnClick="btoVolverEmpresa_click" ID="btnVolverEmpresa" Text="Volver a Puesto" />

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
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("clave")%>' runat="server" />
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tipo" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTipo" Text='<%# If(Eval("EsDocumento") = 0, "Documento", (If(Eval("EsDocumento") = 1, "Carné", (If(Eval("EsDocumento") = 2, "Formación", (If(Eval("EsDocumento") = 3, "Certificado", (If(Eval("EsDocumento") = 4, "Instrucción Norma", (If(Eval("EsDocumento") = 5, "Manual", "Carné de cualificacion"))))))))))) %>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Documento" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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


                        <asp:TemplateField HeaderText="Fecha Recepción">
                            <ItemTemplate>
                                <asp:Label ID="lblFecRecepcion" Text='<%# If(Eval("FecRec") = Date.MinValue, "", Left(Eval("FecRec").ToShortDateString, 10)) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fecha Caducidad">
                            <ItemTemplate>
    <asp:Label ID="lblFecCaducidad" Text='<%# If(Eval("FecCad") > CDate("01/01/2098"), "No aplica", If(Eval("FecCad") = Date.MinValue Or Eval("FecCad") > DateAdd(DateInterval.Month, -1, Date.MaxValue) Or Eval("FecCad") = CDate("01/01/1900"), "", Left(Eval("FecCad").ToShortDateString, 10))) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Autor">
                            <ItemTemplate>
                                <asp:Label ID="lblUbicación" Text='<%# Eval("ubicacionfisica")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Observaciones">
                            <ItemTemplate>
                                <asp:Label ID="lblObservaciones" Text='<%# Eval("estado")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <asp:LinkButton ID="imgEstado" runat="server" ToolTip='<%# Eval("txtcorrecto")%>'
                                    class='<%# If(Eval("txtcorrecto") = "Correcto", "btn alert-success", (If(Eval("txtcorrecto") = "Errores", "btn alert-danger", "btn alert-warning"))) %>'><span 
    class='<%# If(Eval("txtcorrecto") = "Correcto", "glyphicon glyphicon-check", (If(Eval("txtcorrecto") = "Errores", "glyphicon glyphicon-ban-circle", "glyphicon glyphicon-warning-sign"))) %>'
         
         '></span></asp:LinkButton>

                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton ID="imgEstado2" runat="server"><span 
    class='<%# If(Eval("txtcorrecto") = "Correcto", "", (If(Eval("txtcorrecto") = "Errores", "glyphicon glyphicon-arrow-right", ""))) %>'         
         '></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton ID="ibVer" OnClick="ibVer_Click" runat="server" class="btn  active" ToolTip="Ver Documento" Visible='<%# Eval("ubicacion")%>'><span class="glyphicon glyphicon-file" ></span></asp:LinkButton>


                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Subir" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>


                                <asp:LinkButton ID="ibSubir" OnClick="ibSubir_Click" runat="server" class="btn boton" ToolTip="Subir Documento"><span class="glyphicon glyphicon-upload"></span></asp:LinkButton>


                            </ItemTemplate>

                        </asp:TemplateField>
                         
                        <asp:TemplateField HeaderText="Hist." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="ibHistorico" OnClick="ibHistorico_Click" CausesValidation="false" runat="server" class="btn btn" ToolTip="Ver historico" Visible='<%# Eval("ubicacionHist")%>'><span class="glyphicon glyphicon-folder-open"></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>





                        <asp:TemplateField HeaderText="Validar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="ibModificar" OnClick="ibModificar_Click" CausesValidation="false" runat="server" class="btn boton" ToolTip='<%# Eval("Comentario")%>' Visible='<%# Eval("ubicacion")%>'><span class="glyphicon glyphicon-edit"></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>




                        <asp:TemplateField visible="false" HeaderText="Plantilla" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="ibPlantilla" OnClick="ibPlantilla_Click" runat="server" class="btn bg-info" ToolTip="Descargar Plantilla" Visible='<%# If(Eval("plantilla") = 1, "True", "False") %>'><span class="glyphicon glyphicon-list-alt"></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>



                    </Columns>


                </asp:GridView>





                <br />


                <hr />
                <div class="form-inline row">
                    <div class="form-group col-sm-5">
                        <asp:Button class="btn btn-primary" runat="server" OnClick="btoVolver_click" ID="btoVolver" Text="Volver" />
                        <%--<input  type="button"   class="btn bg-warning"  value=" &nbsp;&nbsp;Volver &nbsp;&nbsp;"  onclick="history.go(-1);" />--%>
                    </div>
                    <div class="form-group col-sm-7">

                        <h4><span style="text-align: left;" class="label label-info">
                            <asp:Label ID="Label11" runat="server" Text="Total documentos:" />
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


                <div class="panel-body">





                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label runat="server" ID="Label1" Text="Trabajador" />

                            <asp:TextBox ID="txtNombre2" runat="server" class="form-control" ReadOnly="True" />

                        </div>

                    </div>

                    <br />
                    <br />









                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">


                            <asp:Label runat="server" ID="lblNombre" Text="Documento" />:
                                          
                                 <asp:TextBox ReadOnly="True" ID="txtNombreDoc" runat="server" class="form-control" />
   
                            <br />
                            <asp:Label runat="server" ID="txtfecrec" Text="Fecha recepción" />:


                            <asp:TextBox class="form-control required" ID="TxtFechaRec" runat="server" ReadOnly="True" />



                            <br />
                            <asp:Label runat="server" ID="Label331" Width="20%" Text="Periodicidad" />&nbsp;&nbsp;&nbsp;
                            <br />

                            <asp:DropDownList class="form-control" ID="ddlCaducidad2" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                            <br />


                             <asp:Label runat="server" ID="Label6" Text="Duración (Número de Horas)" />:
                              <br />             
                            <asp:TextBox ID="txtDuracion2" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvDuracion2" runat="server" Text="*" ErrorMessage="Añade la duración del curso" ControlToValidate="txtDuracion2" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceDuracion2" runat="server" TargetControlID="rfvDuracion2" PopupPosition="Right" />
                              <br />


                            <asp:Label runat="server" ID="Label2" Text="Válido desde" />:&nbsp;&nbsp;&nbsp;

                            <%          If (TxtFechaVal.Text <> "Documento sin caducidad") And (TxtFechaVal.Text <> "Documento de plantilla") Then  %>


                            <asp:LinkButton ID="imgCalendarioFechaVal" runat="server" class="btn boton" ToolTip="Fecha Validez"><span class="glyphicon glyphicon-calendar"></span></asp:LinkButton>


                            <act:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgCalendarioFechaVal" TargetControlID="TxtFechaVal" />


                            <%end If%>
                            <asp:TextBox ReadOnly="True" class="form-control" type="text" ID="TxtFechaVal" runat="server" AutoPostBack="True" required="required" />
                            <br />


                            <asp:Label runat="server" ID="LblFechaVal2" Text="Fecha de caducidad" />:

                            <asp:TextBox class="form-control" ID="TxtFechaVal2" runat="server" ReadOnly="True" />

                            <br />


                            <asp:Label runat="server" ID="Label3" Text="Subido por" />:

                            <asp:TextBox class="form-control" ID="txtUbicacion" runat="server" ReadOnly="True" />

                            <br />









                            <asp:Label runat="server" ID="Label4" Text="Estado" Font-Bold="true" Font-Underline="true" />:
                         

                            <asp:RadioButtonList CssClass=" btn-group-justified " Width="80%" ID="rblCorrecto" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Correcto" Value="0" Selected="True"  enabled="true"/>
                                <asp:ListItem Text="Incorrecto" Value="1"  enabled="true" />
                                <asp:ListItem Text="No Validado." Value="2"  enabled="true" />
                                <asp:ListItem Text="NO CADUCA." Value="3"  enabled="false"/>
                            </asp:RadioButtonList>

            

                            <asp:TextBox class="form-control" ID="txtComentario" runat="server" Rows="6" TextMode="MultiLine" />

        
                            <br />




                        </div>
                    </div>












                </div>





                <div class="panel-footer">
                    <div class="text-center">


                        <asp:Button ID="CancelVista3" runat="server" class="btn btn-primary" Text="Cancelar" UseSubmitBehavior="false" />

                        <asp:Button ID="GrabarMod" runat="server" class="btn btn-primary" OnClick="GrabarMod_Click" Text="Grabar" UseSubmitBehavior="true" OnClientClick="return true;" ValidationGroup="CamposVacios" />
                        <asp:Button ID="VerDoc" runat="server" class="btn btn-primary" OnClick="ibVer_Click2" Text="Ver Documento" />
                        <asp:Button ID="VerHis" runat="server" class="btn btn-primary" OnClick="ibVerHis_Click" Text="Ver historico" />

                    </div>
                </div>
                <br />

            </asp:View>




            <asp:View ID="view3" runat="server">
                <div class=" panel-header">

                    <%--                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label9" runat="server" Text="Histórico de documentos de trabajador" /></h2>



                    <br />--%>
                    <br />
                    <div class="row">
                        <div class="col-xs-4">
                            <asp:Label runat="server" ID="ex155" Text="Trabajador" />

                            <asp:TextBox class="form-control" type="text" ID="txtNombreHis" runat="server" ReadOnly="True" />
                        </div>
                        <div class="col-xs-2">

                            <asp:Label runat="server" ID="Cif" Text="Nif" />
                            <asp:TextBox type="text" class="form-control" ID="txtCIFHis" runat="server" ReadOnly="True" />
                        </div>




                        <div class="col-xs-4">

                            <asp:Label runat="server" ID="ex1" Text="Documento" />

                            <asp:TextBox class="form-control" ID="txtDocuEmp" runat="server" ReadOnly="True" />

                        </div>
                         <asp:TextBox class="form-control" ID="flagHist" runat="server" visible="false" />
                    </div>


                </div>


                <div class="panel-body">

                    <asp:GridView ID="gvTypeHis" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                        GridLines="None" DataKeyNames="clave" ShowFooter="false">
                        <%--<HeaderStyle HorizontalAlign="Center" BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                        <EditRowStyle BackColor="#ffffcc" />
                        <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Nodoc2" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                        </EmptyDataTemplate>

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

                            <asp:TemplateField HeaderText="Documento">
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
                        <asp:Label runat="server" ID="Label5" Text=" " />
       <asp:Button Visible="false" class="btn btn-primary" runat="server" OnClick="btoSubirDocHist_click" ID="SubirDocHist" Text="Subir Documento Histórico" />                        
                    </div>
                </div>


            </asp:View>







            <asp:View ID="view4" runat="server">


                <%--                <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label5" runat="server" Text="Subir documentos de trabajador" /></h2>



                </div>--%>

                <br />

                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label runat="server" ID="textNombre" Text="Trabajador" />:
                            <asp:TextBox class="form-control" type="text" ID="txtEmpDoc" runat="server" ReadOnly="True" />
                            <br />
                            <asp:Label runat="server" ID="txtApe1" Text="Tipo de Documento" />:
                            <asp:TextBox class="form-control" type="text" ID="txtDocEmp" runat="server" ReadOnly="True" />
                            <br />

                            <asp:Label runat="server" ID="lblPeriodicidadC" Text="Caducidad:" />
                            <br />

                            <asp:DropDownList ID="ddlCaducidad" runat="server" class="form-control" Width="100%" Height="32">
                            </asp:DropDownList>
                            <br />
                           
                            <asp:Label runat="server" ID="lblDuracion" Text="Duración (Número de Horas)" />:
                              <br />             
                            <asp:TextBox ID="txtDuracion" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvDuracion" runat="server" Text="*" ErrorMessage="Añade la duración del curso" ControlToValidate="txtDuracion" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceDuracion" runat="server" TargetControlID="rfvDuracion" PopupPosition="Right" />
                              <br />

                            <asp:Label runat="server" ID="txtApe2" Text="Fecha del Ultimo Documento Subido" />:
                            <asp:TextBox class="form-control" type="text" ID="FecUltDoc" runat="server" ReadOnly="True" />
                            <br />
                            <asp:Label runat="server" ID="txtFechVal" Text="Fecha inicio de Validez" />:
                         
                            <%          If (TxtFechaValidez.Text <> "Documento sin caducidad") And (TxtFechaValidez.Text <> "Documento de plantilla") Then  %>

                            <asp:LinkButton ID="imgCalendarioFechaValidez" runat="server" class="btn boton" ToolTip="Fecha Validez"><span class="glyphicon glyphicon-calendar"></span></asp:LinkButton>
                            <act:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgCalendarioFechaValidez" TargetControlID="TxtFechaValidez" />
                            <%-- <asp:CustomValidator ID="cvFechaRec" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaValidez" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="cvFechaRec" PopupPosition="Right" />--%>
                            <%end If%>

                            <asp:TextBox class="form-control required" type="text" ID="TxtFechaValidez" runat="server" ToolTip="Fecha Validez" required="required" />

                            <br />
                            <asp:Label runat="server" ID="txtFich" Text="Selecciona el documento a subir" />:
                            <asp:FileUpload runat="server" ID="FuDoc" Height="37px" Width="579px" Font-Bold="True" ForeColor="White" required="required" />
                            <br /> 



                             



                        </div>

                    </div>
                </div>
                <div class="panel-footer">
                    <div class="text-center">

                        <asp:Button class="btn btn-primary" ID="btnVolver" runat="server" Text="Cancelar" UseSubmitBehavior="false" />
                        <asp:Button class="btn btn-primary" runat="server" ID="btnSubir2" Text="Subir Documento" OnClientClick="return ValidateFich();" />

                        <%--<asp:Button class="btn btn-primary" runat="server" ID="btnDescargarPlantilla" Text="Descargar Plantilla" OnClientClick="javascript: return DescargarPla();" ValidationGroup="CamposVacios" />--%>
                        <asp:HiddenField ID="idPlantilla0" runat="server" />

                    </div>
                </div>







            </asp:View>


        </asp:MultiView>




        <asp:HiddenField runat="server" ID="idTrabajador" />
        <asp:HiddenField runat="server" ID="DescEmpresa" />
        <asp:HiddenField runat="server" ID="idDoc" />
        <asp:HiddenField runat="server" ID="idPlantilla" />

        <asp:HiddenField runat="server" ID="hfurl" />
        <asp:HiddenField runat="server" ID="FechaValG" />
        <asp:HiddenField runat="server" ID="FechaValG2" />
        <asp:HiddenField runat="server" ID="flagHistorico" />
      
    </div>



</asp:Content>

