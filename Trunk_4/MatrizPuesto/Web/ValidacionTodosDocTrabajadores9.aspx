<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_plantas9.Master" CodeBehind="ValidacionTodosDocTrabajadores9.aspx.vb" Inherits="AdokWeb.ValidacionTodosDocTrabajadores9" %>

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
            var fich = document.getElementById('<%=FuDoc.ClientID%>').value;
            //alert(fich);
            if (fich) {
                return true;
            } else {
                return false;
            }

        }



        function RecogerEmpresa(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresa.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }



<%--        function DescargarPla() {

            var fich = document.getElementById('<%=idPlantilla0.ClientID%>').value;

            location.href = './Ficheros_Matriz_Puestos/' + fich;

        }--%>



</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">
      <div class="container-fluid" style="background-color:#ebebeb;">
    <div class="navbar-header">
      <a class="navbar-brand" href="#" ><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="Documentos sin validar de Trabajadores" /></a>
    </div>
  </div>

    <div class="container " >

        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">


            <asp:View ID="viewListado" runat="server">


<%--                <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label1" runat="server" Text="Documentos sin validar de Vigilancia de Salud" /></h2>


                </div>--%>



                <div class="row">
                    <div class="col-xs-3">
                        <asp:Label  runat="server" Visible="false" ID="ex177" Text="Trabajador"  />:

                        <asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarTrabajadores"
                            runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfEmpresa" runat="server" />



                    </div>


                </div>

                <br />


                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-striped"
                    GridLines="None" DataKeyNames="Id" ShowFooter="false"
                    OnDataBound="gvType_DataBound"
                    OnRowEditing="gvType_RowEditing"  PageSize="10">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc2" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />  
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
                    <PagerTemplate>
             <div class="row" style="margin-top: 20px;">
                           <div class="col-lg-2" style="text-align: right;">
                                
                                <h5>
                                    <asp:Label ID="MessageLabel" Text="Ir a la página." runat="server" /></h5>
                            </div>
                            <div class="col-lg-1" style="text-align: left;">
                           
                                <asp:DropDownList ID="PageDropDownList"   AutoPostBack="true" OnTextChanged="PageDropDownList_SelectedIndexChanged" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
                            </div>
                            <div class="col-lg-8" style="text-align: right;">
                                <h4>
                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h4>
                            </div>
                        </div>
                    </PagerTemplate>
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                            </ItemTemplate>


                            <ItemStyle HorizontalAlign="Center" />

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>






                     
                        <asp:TemplateField  HeaderText="Estado Trabajador">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" runat="server" class="btn  bg-info" ToolTip="Estado de Documentos de trabajador" Text="Estado de Documentos" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>





                    </Columns>


                </asp:GridView>

            <hr />
                

                <%--<div class="form-inline row">--%>
                                  <div class="form-group col-sm-12" style="text-align: center;">
                                             
    <asp:Button ID="botonVolver" runat="server" class="btn btn-primary" Text="Volver" UseSubmitBehavior="false" onclick="Volver" />
                
  </div> 
                 <hr />
 



            </asp:View>


            <asp:View ID="view1" runat="server">
                    <br />
<%--                <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label7" runat="server" Text="Documentos sin validar de Vigilancia de Salud" /></h2>


                </div>--%>


            


                <div class="row" visible="false">
                    <div class="col-xs-4" visible="false">
                        <asp:Label Visible="false"  runat="server" ID="ex61" Text="Trabajador" />

                  <asp:TextBox Visible="false" ID="txtNombre" runat="server" class="form-control" ReadOnly="True" />

                    </div>
                    <div class="col-xs-3">

                        <asp:Label Visible="false" runat="server" ID="Cif2" Text="Nif" />

                        <asp:TextBox Visible="false" ID="txtCIF" runat="server" class="form-control" ReadOnly="True" />



                    </div>
                </div>

                <br />



                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                    GridLines="None" DataKeyNames="Comentario" ShowFooter="false"
                  >
               <%--     <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" /> 
                    </EmptyDataTemplate>

                    <Columns>
                       <asp:BoundField DataField="Comentario" HeaderText="Comentario" Visible="false" />
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center"  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Comentario")%>' runat="server" />
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="N.I.F.">
                            <ItemTemplate>
                                <asp:Label ID="lblTipo" Text='<%# Eval("NIF")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Trabajador">
                            <ItemTemplate>
                                <asp:Label ID="lblNomTra" Text='<%# Eval("nomtra")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Documento">
                            <ItemTemplate>
                                <%--<asp:HyperLink ID="lnkDescripcion" CssClass="tips" runat="server" onclick="ibVer_Click" NavigateUrl='#' Text='<%# Eval("Nombre") %>' style="text-decoration:none; font-weight:normal; color:black" ></asp:HyperLink>--%>
                                <asp:Label ID="lblDescripcion" Text='<%# Eval("Abrev")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Fecha Recepción">
                            <ItemTemplate>
                                <asp:Label ID="lblFecRecepcion" Text='<%# If(Eval("FecRec") = Date.MinValue, "", Left(Eval("FecRec").ToShortDateString, 10)) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fecha Caducidad">
                            <ItemTemplate>
                                <asp:Label ID="lblFecCaducidad" Text='<%# If(Eval("FecCad") = Date.MinValue Or Eval("FecCad") > DateAdd(DateInterval.Month, -1, Date.MaxValue) Or Eval("FecCad") = CDate("01/01/1900"), "", Left(Eval("FecCad").ToShortDateString, 10)) %>' runat="server" />
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
     <asp:LinkButton  ID="imgEstado"  runat="server"    ToolTip='<%# Eval("txtcorrecto")%>'
    class='<%# If(Eval("txtcorrecto") = "Correcto", "btn alert-success", (If(Eval("txtcorrecto") = "Errores", "btn alert-danger", "btn alert-warning"))) %>'
     ><span 
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

                        <asp:TemplateField  HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

<asp:LinkButton ID="ibVer" OnClick="ibVer_Click" runat="server" class="btn  active" ToolTip="Ver Documento"  Visible='<%# Eval("ubicacion")%>' ><span class="glyphicon glyphicon-file" ></span></asp:LinkButton>

            
                            </ItemTemplate>
                        </asp:TemplateField>

             <%--           <asp:TemplateField  HeaderText="Subir" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>


<asp:LinkButton ID="ibSubir" OnClick="ibSubir_Click" runat="server" class="btn boton"  ToolTip="Subir Documento"   ><span class="glyphicon glyphicon-upload"></span></asp:LinkButton>


                            </ItemTemplate>

                        </asp:TemplateField>--%>

          <%--              <asp:TemplateField HeaderText="Hist." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
<asp:LinkButton ID="ibHistorico" OnClick="ibHistorico_Click" CausesValidation="false" runat="server" class="btn btn"  ToolTip="Ver Histórico"  Visible='<%# Eval("ubicacion")%>' ><span class="glyphicon glyphicon-folder-open"></span></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>--%>


                      
 

                        <asp:TemplateField  HeaderText="Validar" ItemStyle-HorizontalAlign="Center"    >
                            <ItemTemplate>
<asp:LinkButton ID="ibModificar" OnClick="ibModificar_Click" CausesValidation="false" runat="server" class="btn boton"  ToolTip="Validar"  Visible='<%# Eval("ubicacion")%>' ><span class="glyphicon glyphicon-edit"></span></asp:LinkButton>
                 
                            </ItemTemplate>
                        </asp:TemplateField>




                        <asp:TemplateField  HeaderText="Plantilla" Visible="false" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
<asp:LinkButton ID="ibPlantilla" OnClick="ibPlantilla_Click"  runat="server" class="btn bg-info"  ToolTip="Descargar Plantilla"  Visible='<%# If(Eval("plantilla") = 1, "True", "False") %>' ><span class="glyphicon glyphicon-list-alt"></span></asp:LinkButton>
  
                            </ItemTemplate>
                        </asp:TemplateField>



                    </Columns>


                </asp:GridView>


  


                 <br />


                     <hr />
                <div class="form-inline row">
                                  <div class="form-group col-sm-5">
                                               <asp:Button class="btn btn-primary" visible="false" runat="server"  ID="btoVolver" Text="Volver"  />
                      <%--<input  type="button"   class="btn bg-warning"  value=" &nbsp;&nbsp;Volver &nbsp;&nbsp;"  onclick="history.go(-1);" />--%>
  </div> <div class="form-group col-sm-7">

             <h4 >             <span  style="text-align: left;" class="label label-primary"><asp:Label ID="Label10" runat="server" Text="Total documentos:" /> </span>&nbsp;

                    <span class="label label-primary">
                        <asp:Label ID="Registros" runat="server" Text="" />
                    </span>
              
             </h4>
                    </div>
                </div>
                     <hr />
            
                    
              




            </asp:View>



            <asp:View ID="view2" runat="server">
                <div class=" panel-header">


    <%--                <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label8" runat="server" Text="Documentos sin validar de Vigilancia de Salud" /></h2>
--%>


                </div>
                   <br />
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

                          
                                       <asp:Label  runat="server" ID="lblNombre" Text="Documento" />:
                                          
                                 <asp:TextBox ReadOnly="True" ID="txtNombreDoc" runat="server" class="form-control" />
                            <br />
                            <asp:Label  runat="server" ID="txtfecrec" Text="Fecha recepción" />:


                            <asp:TextBox class="form-control required" ID="TxtFechaRec" runat="server" ReadOnly="True" />



                     <br />
                            <asp:Label  runat="server" ID="Label331" width="20%"  Text="Periodicidad" />&nbsp;&nbsp;&nbsp;
                            <br />

                            <asp:DropDownList class="form-control" ID="ddlCaducidad" runat="server" AutoPostBack="True" >
                            </asp:DropDownList>
                            <br /> 
                          



                           <asp:Label runat="server" ID="Label8" Text="Duración (Número de Horas)" />:
                              <br />             
                            <asp:TextBox ID="txtDuracion2" runat="server" class="form-control required" />
                            <asp:RequiredFieldValidator ID="rfvDuracion2" runat="server" Text="*" ErrorMessage="Añade la duración del curso" ControlToValidate="txtDuracion2" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceDuracion2" runat="server" TargetControlID="rfvDuracion2" PopupPosition="Right" />
                              <br />



                            <asp:Label  runat="server" ID="Label2" Text="Válido desde"  />:&nbsp;&nbsp;&nbsp;

<%          If (TxtFechaVal.Text <> "Documento sin caducidad") And (TxtFechaVal.Text <> "Documento de plantilla") Then  %>                            
<asp:LinkButton ID="imgCalendarioFechaVal"  runat="server" class="btn boton"  ToolTip="Fecha Validez"  ><span class="glyphicon glyphicon-calendar"></span></asp:LinkButton>
                            <act:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgCalendarioFechaVal" TargetControlID="TxtFechaVal" />
                        
                                 <%-- <asp:CustomValidator ID="CustomValidator1" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaVal" ValidationGroup="CamposVacios" Display="None" />
                                                <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="CustomValidator1" PopupPosition="Right" />--%>
 <%end If%>






                                <asp:TextBox ReadOnly="True" class="form-control" type="text" ID="TxtFechaVal" runat="server" AutoPostBack="True" required="required"/>
                            <br />


                            <asp:Label runat="server" ID="LblFechaVal2" Text="Fecha de caducidad" />:

                            <asp:TextBox class="form-control" ID="TxtFechaVal2" runat="server" ReadOnly="True" />

                            <br />


                            <asp:Label  runat="server" ID="Label3" Text="Subido por"  />:

                            <asp:TextBox class="form-control" ID="txtUbicacion" runat="server" ReadOnly="True" />

                            <br />
							
							
									
							
							
							
                            <asp:Label  runat="server" ID="Label4" Text="Estado" Font-Bold="true" Font-Underline="true"   />:

                    <asp:RadioButtonList  CssClass= " btn-group-justified " width="80%"    ID="rblCorrecto" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" >
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
                        <asp:Button ID="VerDoc" runat="server" class="btn btn-primary" OnClick="ibVer_Click2" Text="Ver Documento"  />
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
                        <div class="col-xs-6">
                            <asp:Label runat="server" ID="ex155" Text="Trabajador" />

                            <asp:TextBox class="form-control" type="text" ID="txtNombreHis" runat="server" ReadOnly="True" />
                        </div>
                        <div class="col-xs-2">

                            <asp:Label runat="server" ID="Cif" Text="Nif" Visible="false" />
                            <asp:TextBox type="text" class="form-control" ID="txtCIFHis" runat="server" ReadOnly="True" Visible="false"/>
                        </div>




                        <div class="col-xs-4">

                            <asp:Label runat="server" ID="ex1" Text="Documento" />

                            <asp:TextBox class="form-control" ID="txtDocuEmp" runat="server" ReadOnly="True" />

                        </div>
                    </div>


                </div>


                <div class="panel-body">

                    <asp:GridView ID="gvTypeHis" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                        GridLines="None" DataKeyNames="clave" ShowFooter="false">
                        <%--<HeaderStyle HorizontalAlign="Center" BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                        <EditRowStyle BackColor="#ffffcc" />
                        <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                        <EmptyDataTemplate>
                            <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" /> 
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
                                        runat="server" ToolTip="Ver Documento"  />

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


                <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="Label5" runat="server" Text="Subir documentos de trabajador" /></h2>



                </div>



                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label  runat="server" ID="textNombre" Text="Trabajador" />:
                            <asp:TextBox class="form-control" type="text" ID="txtEmpDoc" runat="server" ReadOnly="True" />
                               <br />
                            <asp:Label  runat="server" ID="txtApe1" Text="Tipo de Documento" />:
                            <asp:TextBox class="form-control" type="text" ID="txtDocEmp" runat="server" ReadOnly="True" />
                               <br />
                            <asp:Label  runat="server" ID="txtApe2" Text="Fecha del Ultimo Documento Subido" />:
                            <asp:TextBox class="form-control" type="text" ID="FecUltDoc" runat="server" ReadOnly="True" />
                               <br />
                            <asp:Label  runat="server" ID="txtFechVal" Text="Fecha inicio de Validez" />:
                         
<%          If (TxtFechaValidez.Text <> "Documento sin caducidad") And (TxtFechaValidez.Text <> "Documento de plantilla") Then  %>

<asp:LinkButton ID="imgCalendarioFechaValidez"  runat="server" class="btn boton"  ToolTip="Fecha Validez"  ><span class="glyphicon glyphicon-calendar"></span></asp:LinkButton>
                            <act:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgCalendarioFechaValidez" TargetControlID="TxtFechaValidez" />
                             <%--<asp:CustomValidator ID="cvFechaRec" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaValidez" ValidationGroup="CamposVacios" Display="None" />
                                                <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="cvFechaRec" PopupPosition="Right" />--%>
 <%end If%>

                            <asp:TextBox class="form-control required" type="text" ID="TxtFechaValidez" runat="server"  title="FechaValidez" required="required" />

                               <br />
                            <asp:Label  runat="server" ID="txtFich" Text="Selecciona el documento a subir" />:
                            <asp:FileUpload runat="server" ID="FuDoc" Height="37px" Width="579px" Font-Bold="True" ForeColor="White"  required="required" />
                               <br />
							   
							   


                              <%if (txtDocEmp.Text = "Carne de cualificacion") Then  %>



                            <asp:Label runat="server" ID="Labelx5"  Text="Selecciona los tipos de carné incluídos " />:

                            <div class="checkbox checkboxlist col-sm-9">
                                               <asp:CheckBoxList  ID="ListBox11" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" Rows="8" SelectionMode="Multiple"> 
<asp:ListItem>CCBT Básico</asp:ListItem>
<asp:ListItem>CCBT Especialidad: 1, 2, 3, 4, 5, 6, 7, 8 y 9</asp:ListItem>
<asp:ListItem>ILAT1 Instalador-mantenedor de AT líneas hasta 30 KV</asp:ListItem>
<asp:ListItem>ILAT2 Instalador-mantenedor de AT líneas superiores a 30 KV</asp:ListItem>
<asp:ListItem>IA Instalador de Fontanería</asp:ListItem>
<asp:ListItem>IC1 instalador de Calefacción y ACS - menos de 70 Kw </asp:ListItem>
<asp:ListItem>ICA Instalador Calefacción y ACS - cualquier potencia</asp:ListItem>
<asp:ListItem>ICB Instalador de Climatización</asp:ListItem>
<asp:ListItem>MCA Mantenedor de Calefacción y ACS</asp:ListItem>
<asp:ListItem>MCB Mantenedor de Climatización </asp:ListItem>
<asp:ListItem>IMT Instalador-mantenedor de Instalaciones Térmicas en Edificios (Nuevo R.I.T.E, RD 102712007) </asp:ListItem>
<asp:ListItem>ITI Inspector de instalaciones térmicas </asp:ListItem>
<asp:ListItem>IGA Instalador de GAS - IGA - RD 919/2006</asp:ListItem>
<asp:ListItem>IGB Instalador de GAS - IGB - RD 919/2006</asp:ListItem>
<asp:ListItem>IGC Instalador de GAS - IGC - RD 919/2006</asp:ListItem>
<asp:ListItem>IF Instalador Frigorista </asp:ListItem>
<asp:ListItem>MF Conservador-Reparador Frigorista</asp:ListItem>
<asp:ListItem>MIC Operador Industrial de Calderas </asp:ListItem>
<asp:ListItem>MGA1 Operador de Grúa Móvil Autopropulsada - A (hasta 130 toneladas)</asp:ListItem>
<asp:ListItem>MGA1Operador de Grúa Móvil Autopropulsada - 6 (sín limitación)</asp:ListItem>
<asp:ListItem>MGT Operador de Grúas Torre </asp:ListItem>
<asp:ListItem>GF1 Manipulador de sistemas frigoríficos con cualquier carga de refrigerante fluorado</asp:ListItem>
<asp:ListItem>GF2 Manipulador de sistemas frigoríficos con menos de 3 kg de refrigerante fluorado</asp:ListItem>
<asp:ListItem>GF2E Manipulador de gases fluorados-transporte refrigerado de mercancías</asp:ListItem>
<asp:ListItem>GF3 Manipulador de equipos de climatización con refrigerante fluorado para vehículos</asp:ListItem>
<asp:ListItem>GF4 Manipulador de sistemas contra incendios o extintores con refrigerante fluorado</asp:ListItem>
<asp:ListItem>GF5 Manipulador de disolventes con gases fluorados y sus equipos</asp:ListItem>
<asp:ListItem>GF6 Recuperador de SF6 en equipos de conmutación de alta tensión</asp:ListItem>
</asp:CheckBoxList>


                             <%end If%>
							   


							   
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
                <asp:HiddenField runat="server" ID="FechaValG" />
        <asp:HiddenField runat="server" ID="FechaValG2" />
        <asp:HiddenField runat="server" ID="hfurl" />
       <%--<asp:HiddenField runat="server" ID="extranet" />--%>



    </div>



</asp:Content>

