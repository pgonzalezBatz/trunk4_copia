<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MPLogin.Master" CodeBehind="ConsultaSolicitudesComite.aspx.vb" Inherits="Web.ConsultaSolicitudesComite" %>

<%@ MasterType VirtualPath="~/MPLogin.Master" %>


<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="cphContenido">

     

    <asp:Button Visible="false" ID="Button4" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton3" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender4" BehaviorID="mpe4" runat="server"
        PopupControlID="pnlPopup4" TargetControlID="LinkButton3" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup4" runat="server" CssClass="modalPopup" style="width:500px" >
         <div class="header">
                   <asp:Label ID="Label19" runat="server" Text="Rechazo" />
           
        </div>
        <div class="body" style="overflow: auto; width: 90%; margin-left: 5%; " >
          
            <br />
            <div class="row" style="overflow: auto; width: 80%; margin-left: 5%; ">

            <asp:Label ID="Label18" runat="server" Text="Motivo del rechazo" />
            <asp:TextBox ID="TextBox1" runat="server" class="form-control" Rows="8" TextMode="MultiLine" />
            </div>
            <br />
            <br />

         <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar1_Click" />
         <asp:Button ID="Button6" class="btn btn-primary" runat="server" Text="Cancelar" OnClick="btnCancelar4_Click" />
        </div>
        <br />
        <br />
    </asp:Panel>

 
     <asp:HiddenField ID="hfclave" runat="server" />



    <asp:Button Visible="false" ID="Button2" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe1" runat="server"
        PopupControlID="pnlPopup1" TargetControlID="LinkButton1" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup1" runat="server" CssClass="modalPopup"  style="width:500px">
         <div class="header">
                   <asp:Label ID="Label132" runat="server" Text="Traduccion" />
           
        </div>
         <div class="body" style="overflow: auto; width: 90%; margin-left: 5%; " >
            <br />

   <div class="row" style="overflow: auto; width: 80%; margin-left: 5%; ">

            <asp:Label ID="Label142" runat="server" Text="Traduccion" />
                  
            <asp:TextBox ID="TextBox2" runat="server" class="form-control" Rows="8" TextMode="MultiLine" />
          </div>
            <br />
            <br />

        <asp:Button ID="Button3" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar2_Click" />
         <asp:Button ID="Button5" class="btn btn-primary" runat="server" Text="Cancelar" OnClick="btnCancelar4_Click" />
        </div>
        <br />
        <br />
    </asp:Panel>


  

    
    <asp:Button Visible="false" ID="Button10" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton2" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender2" BehaviorID="mpew1" runat="server"
        PopupControlID="pnlPopupw1" TargetControlID="LinkButton2" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopupw1" runat="server" CssClass="modalPopup"  style="width:500px">
         <div class="header">
                   <asp:Label ID="Label15" runat="server" Text="Traduccion" />
           
        </div>
         <div class="body" style="overflow: auto; width: 90%; margin-left: 5%; " >
            <br />

   <div class="row" style="overflow: auto; width: 80%; margin-left: 5%; ">

            <asp:Label ID="Label16" runat="server" Text="Traduccion" />
                  
            <asp:TextBox ID="TextBox4" runat="server" class="form-control" Rows="8" TextMode="MultiLine" />
          </div>
            <br />
            <br />

        <asp:Button ID="Buttonw3" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabarw2_Click" />
         <asp:Button ID="Buttonw5" class="btn btn-primary" runat="server" Text="Cancelar" OnClick="btnCancelar4_Click" />
        </div>
        <br />
        <br />
    </asp:Panel>



    
    <asp:Button Visible="false" ID="Button7" runat="server" Text="" />
    <asp:LinkButton ID="LinkButton5" runat="server"></asp:LinkButton>
    <act:ModalPopupExtender ID="ModalPopupExtender5" BehaviorID="mpe5" runat="server"
        PopupControlID="pnlPopup5" TargetControlID="LinkButton5" BackgroundCssClass="modalBackground">
    </act:ModalPopupExtender>
    <asp:Panel ID="pnlPopup5" runat="server" CssClass="modalPopup" style="width:500px" >
         <div class="header">
                   <asp:Label ID="Label13" runat="server" Text="Cierre" />
           
        </div>
        <div class="body" style="overflow: auto; width: 90%; margin-left: 5%; " >
          
            <br />
            <div class="row" style="overflow: auto; width: 80%; margin-left: 5%; ">

            <asp:Label ID="Label14" runat="server" Text="Comentario de cierre" />
            <asp:TextBox ID="TextBox3" runat="server" class="form-control" Rows="8" TextMode="MultiLine" />
            </div>
            <br />
            <br />

         <asp:Button ID="Button8" class="btn btn-primary" runat="server" Text="Grabar" OnClick="btnGrabar3_Click" />
         <asp:Button ID="Button9" class="btn btn-primary" runat="server" Text="Cancelar" OnClick="btnCancelar4_Click" />
        </div>
        <br />
        <br />
    </asp:Panel>



    <div class="container-fluid" >
        <div class="navbar-header">
            <a class="navbar-brand" href="./SolicitudesCR.aspx"><asp:Label ID="Label1" runat="server" Text="Canal Ético" Font-Underline="true" /></a>           
        </div>
<%--         <div  class="col-lg-6" style="text-align: right;" >
         <asp:CheckBox   ID="CheckBox1" runat="server" AutoPostBack="True" text="Todos: "  Font-Size="Small" TextAlign="Left" />
        </div>--%>
    </div>



        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">

            <asp:View ID="viewListado" runat="server">




                <div class="row">
                 <div  class="col-lg-10" style="text-align: right;" >
   <asp:CheckBox   ID="CheckBox2" runat="server" AutoPostBack="True" text="Todos"    Font-Size= "Smaller" TextAlign="Right" visible="false"/> &nbsp;&nbsp;
                        <asp:HiddenField ID="hfEmpresa" runat="server" />

                           </div>
                        <div class="col-xs-3">

                        <asp:HiddenField ID="hfDocumento" runat="server" />

                                   </div>

                     </div>
               

                <br />

                



                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="true"   PageSize="15"
                    GridLines="None" OnRowCommand="gvType2_OnRowCommand" DataKeyNames="Id" ShowFooter="false" OnDataBound="gvType2_DataBound"
                    OnRowCancelingEdit="gvType2_RowCancelingEdit" OnRowEditing="gvType2_RowEditing" OnRowUpdating="gvType2_RowUpdating">

                       <PagerStyle CssClass="PagerStyle" />
                    <EditRowStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#ffffcc" />

                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Label ID="lblID" Text='<%# Eval("id")%>' runat="server" Visible="false" /></ItemTemplate></asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Trabajador"><ItemTemplate><asp:Label ID="lblTipo" Text='<%# Eval("idTra")%>' runat="server"  /></ItemTemplate></asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Planta"><ItemTemplate><asp:Label ID="lblPlanta" Text='<%# Eval("plantaDesc")%>' runat="server"  /></ItemTemplate></asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Categoría"><ItemTemplate><asp:Label ID="lblCategoria" Text='<%# Eval("codCategoria")%>' runat="server" /></ItemTemplate></asp:TemplateField>
                       
                        
                        
                         <asp:TemplateField HeaderText="Comentario"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                            
                 <%--           <asp:Label ID="lblDesc" Text='<%# Eval("comentario")%>' runat="server" />--%>



                            <asp:linkButton runat="server"    ID="lblDescripcion" Text='<%# Eval("comentariocorto")%>'  ToolTip='<%# Eval("comentario")%>'   />
                           
   <act:ConfirmButtonExtender ID="cbeEliminar2" runat="server" DisplayModalPopupID="mpeEliminar2"
                                                    TargetControlID="lblDescripcion">
                                                </act:ConfirmButtonExtender>
                                                <act:ModalPopupExtender ID="mpeEliminar2" runat="server" PopupControlID="pnlConfirmDelete" TargetControlID="lblDescripcion" 
                                                    CancelControlID="btnCancelar3" BackgroundCssClass="modalBackground">
                                                </act:ModalPopupExtender>
                                                <asp:Panel  ID="pnlConfirmDelete" runat="server"   CssClass="modalPopup" Style="display: none">
                                                    <div class="header">
                                                        <asp:Label ID="lblConfirmacion" runat="server" Text="Comentario" />
                                                    </div>
                                                    <div class="body">
                                                        <asp:Label ID="lblConfirmarBorrado2" runat="server" Text='<%# Eval("comentario")%>' />
                                                    </div>
                                                    <div class="footer" style="text-align: center;">
                                         
                                                        <asp:Button ID="btnCancelar3" runat="server" CssClass="btn btn-primary" Text="OK" />
                                                    </div>
                                                </asp:Panel>


                            </ItemTemplate></asp:TemplateField>
                        







                         <asp:TemplateField HeaderText="Descripción"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                            
                      <asp:linkButton runat="server"    ID="lblDescripcion2" Text='<%# Eval("traduccioncorto")%>'  ToolTip='<%# Eval("traduccion")%>'   />
                           
   <act:ConfirmButtonExtender ID="cbeEliminar22" runat="server" DisplayModalPopupID="mpeEliminar22"
                                                    TargetControlID="lblDescripcion2">
                                                </act:ConfirmButtonExtender>
                                                <act:ModalPopupExtender ID="mpeEliminar22" runat="server" PopupControlID="pnlConfirmDelete2" TargetControlID="lblDescripcion2" 
                                                    CancelControlID="btnCancelar32" BackgroundCssClass="modalBackground">
                                                </act:ModalPopupExtender>
                                                <asp:Panel  ID="pnlConfirmDelete2" runat="server"   CssClass="modalPopup" Style="display: none">
                                                    <div class="header">
                                                        <asp:Label ID="lblConfirmacion2" runat="server" Text="Descripción" />
                                                    </div>
                                                    <div class="body">
                                                        <asp:Label ID="lblConfirmarBorrado22" runat="server" Text='<%# Eval("traduccion")%>' />
                                                    </div>
                                                    <div class="footer" style="text-align: center;">
                                         
                                                        <asp:Button ID="btnCancelar32" runat="server" CssClass="btn btn-primary" Text="OK" />
                                                    </div>
                                                </asp:Panel>


                            </ItemTemplate></asp:TemplateField>






                        <asp:TemplateField HeaderText="Fecha"><ItemTemplate><asp:Label ID="lblFecha" Text='<%# Eval("Fecha")%>' runat="server" /></ItemTemplate></asp:TemplateField>



                        <asp:TemplateField HeaderText="Accion">
                            <ItemTemplate>
                                 <asp:DropDownList    SelectedValue='<%# Eval("Accion")%>' class="btn btn-default dropdown-toggle" type="button" runat="server" ID="ddlUni" data-toggle="dropdown"  >
                                    <asp:ListItem Text=" " Value="0" />
                                    <asp:ListItem Text="Rechazar" Value="1" />
                                    <asp:ListItem Text="Cerrado" Value="2" />
                                    <asp:ListItem Text="RRHH" Value="3" />
                                    <asp:ListItem Text="Presidencia" Value="4" />
                                    <asp:ListItem Text="Comite de Cumplimiento" Value="5" />
                                                                       

                               </asp:DropDownList>
                                

                            
                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Grabar" visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" class="btn  bg-info" runat="server" ToolTip="Grabar" Text="Grabar" CommandName="Edit" visible="false" />
                            </ItemTemplate>

                        </asp:TemplateField>

          
                    </Columns>


                </asp:GridView>




            </asp:View>




            <asp:View ID="view2" runat="server">




                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">


                            <asp:Label runat="server" ID="Label20" Text="Descripción de la Solicitud" />:
                            <asp:TextBox class="form-control required" ID="txtSolicitud" runat="server" />

                            <asp:RequiredFieldValidator ID="cvTarje" runat="server" Text="*" ErrorMessage="Descripción requerida" ControlToValidate="txtSolicitud" ValidationGroup="CamposVacios" />
                            <act:ValidatorCalloutExtender ID="vceTarjet" runat="server" TargetControlID="cvTarje" PopupPosition="Right" />


                            <asp:Label runat="server" ID="Label11" Text="Solicitante" />:
                            <asp:TextBox class="form-control" ID="Solicitante" runat="server" ReadOnly="true" />





                            <asp:Label runat="server" ID="lblAsignacionC" Text="Proveedor" />:

                            <asp:TextBox ID="txtProveedor" runat="server" class="form-control required" AutoPostBack="True" />

                            <act:AutoCompleteExtender ID="AutoCompleteExtender3" ServiceMethod="CargarEmpresasXBATNombre"
                                runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerProveedor"
                                TargetControlID="txtProveedor" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />

                            <asp:HiddenField ID="hfProveedor" runat="server" />
                                         
                             <%--<asp:HiddenField ID="hfEmpresa" runat="server" />--%>
                            <asp:HiddenField ID="hfEmpresaNombre" runat="server" />
                            <asp:HiddenField ID="hfEmpresaCodigo" runat="server" />

                            <div class="help-block">Nota: Si el proveedor no existe no continúe, envíe un correo a jrevuelta@batz.es con todos los datos</div>




                            <%--       <asp:DropDownList ID="DdlEmpresa" runat="server" class="form-control required">
                            </asp:DropDownList>--%>


                            <%--          <asp:DropDownList ID="DdlResponsable" runat="server" class="form-control">
                            </asp:DropDownList>--%>



                            <asp:Label runat="server" ID="Label8" Text="Fecha inicio" />:&nbsp;&nbsp;&nbsp;&nbsp;
                           
 <div class='input-group date' id='imgCalendarioResolucionDesde3'>
     <asp:TextBox ID="TxtFechaIni" runat="server" class="form-control required" />
     <span class="input-group-addon">
         <span class="glyphicon glyphicon-calendar"></span>
     </span>
 </div>
                            <act:CalendarExtender ID="imgCalendarioResolucionDesde3_CalendarExtender" runat="server" TargetControlID="TxtFechaIni" PopupButtonID="imgCalendarioResolucionDesde3" />

                      <%--      <asp:CustomValidator ID="cvFechaRec" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaIni" ValidationGroup="CamposVacios" Display="None" />

                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="cvFechaRec" PopupPosition="Right" />--%>

                            <asp:Label ID="Label5" Text="Fecha fin" runat="server" />:&nbsp;&nbsp;&nbsp;&nbsp;
      
 <div class='input-group date' id='imgCalendarioResolucionDesde2'>
     <asp:TextBox ID="TxtFechaFin" runat="server" class="form-control" CausesValidation="True" />
     <span class="input-group-addon">
         <span class="glyphicon glyphicon-calendar"></span>
     </span>
 </div>

                            <act:CalendarExtender ID="imgCalendarioResolucionDesde2_CalendarExtender" runat="server" TargetControlID="TxtFechaFin" PopupButtonID="imgCalendarioResolucionDesde2" />


                       

                            <asp:Label runat="server" ID="Label12" Text="Descripción del puesto de trabajo" />:                            
                            <asp:TextBox ID="txtFuncion" runat="server" class="form-control" Rows="3" TextMode="MultiLine" />




                
                      

        

                            <asp:HiddenField runat="server" ID="flag_Modificar" />
                            <asp:HiddenField runat="server" ID="flag_Actualizar" />
                            <asp:HiddenField runat="server" ID="HdnPedido" />
                            <asp:HiddenField runat="server" ID="HdnFecCad" />
                            <asp:HiddenField runat="server" ID="HdnCIF" />
                            <asp:HiddenField ID="hdnchecked" runat="server" />


                        </div>
                    </div>

                </div>

                <div class="panel-footer">


                    <div class="text-center">
                        <asp:Button ID="btnCancelar" class="btn btn-primary" runat="server" Text="Cancelar" UseSubmitBehavior="false" />
                    
                    </div>

                </div>

            </asp:View>






            <asp:View ID="view1" runat="server">

                <br />


                <div class="row">


                    <div class="col-xs-4" style="text-align: left;">
                        <asp:Label runat="server" ID="lblSolicit" Text="Solicitud" />:


                        <asp:TextBox ID="txtPedido" runat="server" class="form-control" />


                    </div>


                </div>



                <br />



                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="true" PageSize="99"
                    GridLines="None" DataKeyNames="Id" ShowFooter="false" OnDataBound="gvType_DataBound"
                    OnRowCancelingEdit="gvType_RowCancelingEdit" OnRowEditing="gvType_RowEditing" OnRowUpdating="gvType_RowUpdating">
                    <%--<HeaderStyle  CssClass="HeaderStyle"  />--%>
                    <%--<RowStyle  HorizontalAlign="Center"  Font-Bold="True"  />--%>
                    <EditRowStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#ffffcc" />

                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc1" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
                    </EmptyDataTemplate>
                    <%--Paginador...--%>
 <%--                   <PagerTemplate>
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
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" Visible="false" /></ItemTemplate></asp:TemplateField>


                        <%--                    <asp:TemplateField  HeaderText="Num." >
                         <ItemTemplate>
                         
                              <asp:CheckBox ID="chkTarjeta" Checked='<%# If(Eval("tarjeta") <> "", 1, 0) %>' Enabled="false" runat="server"  />
                          </ItemTemplate>
                        <EditItemTemplate >
                              <asp:TextBox ID="txtModTarjeta" runat="server" Text='<%# Eval("tarjeta")%>' class="form-control required"     />
                      
                        </EditItemTemplate>
                    </asp:TemplateField>


         <asp:TemplateField HeaderText="Tarjeta">
                        <ItemTemplate>
                             <asp:Button ID="btnEdit" runat="server" Text="Tarjeta" CssClass="btn btn-info" CommandName="Edit" CausesValidation="false" UseSubmitBehavior="false" />

                        
                        </ItemTemplate>
                        <EditItemTemplate>
               <asp:Button ID="btnUpdate" runat="server" Text="Grabar" CssClass="btn btn-success" CommandName="Update"  CommandArgument='<%# Container.DataItemIndex%>'  /> 
                   <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="btn btn-default" CommandName="Cancel" CausesValidation="false" UseSubmitBehavior="false" />

                          
                        </EditItemTemplate>               
                    </asp:TemplateField>--%>





                        <asp:TemplateField HeaderText=" Trabajadores de esta solicitud"><ItemTemplate><asp:LinkButton ID="lblDescripcion" Text='<%# Eval("Nombre")%>' runat="server" Font-Underline="true" CommandName="Modificar" /></ItemTemplate></asp:TemplateField>


                        <asp:TemplateField HeaderText="N.I.F."><ItemTemplate><asp:LinkButton ID="lblDNI" Text='<%# Eval("tDNI")%>' runat="server" Font-Underline="true" CommandName="Modificar" /></ItemTemplate></asp:TemplateField>

                        <asp:TemplateField HeaderText="Asignado" ><ItemTemplate><asp:CheckBox ID="chkMarcado" Checked='<%# Eval("activo2")%>' Enabled="True" runat="server" /></ItemTemplate></asp:TemplateField>


                        <%--                        <asp:TemplateField HeaderText="Activo"  >
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActivo" Checked='<%# If(Eval("Activo") = 1, 0, 1) %>' Enabled="false" runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>--%>





                        <%--
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Desactivar" Visible='<%# If(Eval("Activo") = 0, "true", "false") %>' />
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

                        --%>

                        <%--
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Activar" Text="Activar" Visible='<%# If(Eval("Activo") = 0, "false", "true") %>' />
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
                        --%>
                    </Columns>


                </asp:GridView>



                   <hr />
                <p style="text-align: center;">
               <asp:Button class="btn btn-primary" ID="btnVolver" runat="server" Text="Volver" UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
   
                    <asp:LinkButton ID="btnQuitarSeleccionados" runat="server" CssClass="btn btn-lg btn-info" UseSubmitBehavior="true"><span class="glyphicon glyphicon-pushpin"></span>&nbsp;<asp:Label ID="Label53" runat="server" Text="Asignar los trabajadores seleccionados" /></asp:LinkButton></p></asp:View><asp:View ID="view3" runat="server">

             <asp:HiddenField runat="server" ID="idTrabajador" />
                <asp:HiddenField runat="server" ID="idDocumento" />
                <asp:HiddenField runat="server" ID="hdnCaducidad" />
                <asp:HiddenField runat="server" ID="chkMarcados" />

                <%--              <div class=" panel-header">


                    <h2 class="text-center"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp; 
                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="Mantenimiento de datos" /></h2>


                </div>--%>

                <br />
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">
                            <asp:Label runat="server" ID="ex21" Text="DNI" />



                            <asp:TextBox ID="txtCIF" runat="server" class="form-control required" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ErrorMessage="Añade el DNI" ControlToValidate="txtCIF" ValidationGroup="CamposVacios" Display="None" />
                            <%--<asp:CustomValidator ID="rfvDNI" ClientValidationFunction="ValidateDNI" runat="server" Text="*" ErrorMessage="DNI erróneo. Formato: nnnnnnnnA" ControlToValidate="txtCIF" ValidationGroup="CamposVacios" Display="None" />--%>
                       <%--     <asp:RegularExpressionValidator ID="revDNI" runat="server" ControlToValidate="txtCIF" ValidationGroup="CamposVacios" Font-Italic="true" Display="None"
                                SetFocusOnError="true" ErrorMessage="DNI erróneo. Formato: nnnnnnnnA" ValidationExpression="^((([0-9]|[0-9])\d{7}[A-Z]|[a-z])|(\d{8}([A-Z]|[a-z])))$" />--%>

                        <%--    <act:ValidatorCalloutExtender ID="vceCIF" runat="server" TargetControlID="revDNI" PopupPosition="Right" />--%>
                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1" PopupPosition="Right" />


                            <asp:Label runat="server" ID="Label23" Text="Nombre" />: <asp:TextBox ID="txtNombre" runat="server" class="form-control required" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" Text="*" ErrorMessage="Añade el nombre del trabajador" ControlToValidate="txtNombre" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="Right" />


                            <asp:Label runat="server" ID="Label22" Text="Apellidos" />: <asp:TextBox ID="txtApellidos" runat="server" class="form-control required" />
                            <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" Text="*" ErrorMessage="Añade los apellidos" ControlToValidate="txtApellidos" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceCodigo" runat="server" TargetControlID="rfvCodigo" PopupPosition="Right" />



                            <asp:Label runat="server" ID="Label2" Text="Empresa" />: <asp:DropDownList ID="DdlEmpresa2" runat="server" class="form-control required">
                            </asp:DropDownList>


                            <%--  </div>
                          <div class="col-lg-1  col-lg-offset-2" style="text-align:center">--%>

                            <asp:Label runat="server" ID="Label3" Text="Autónomo" />: <asp:DropDownList ID="DdlAutonomo2" runat="server" class="form-control required">
                                <asp:ListItem Text="No " Value="0" />
                                <asp:ListItem Text="Si" Value="1" />
                            </asp:DropDownList>
                            <%--<br />
                                </div>

                          <div class="col-lg-8 col-md-8 col-lg-offset-2">--%>


                            <asp:Label runat="server" ID="Label4" Text="Fecha inicio" />:&nbsp;&nbsp;&nbsp;&nbsp; <div class='input-group date' id='imgCalendarioResolucionDesde32'>
     <asp:TextBox ID="TxtFechaIni2" runat="server" class="form-control required" />
     <span class="input-group-addon">
         <span class="glyphicon glyphicon-calendar"></span>
     </span>
 </div>
                            <act:CalendarExtender ID="imgCalendarioResolucionDesde32_CalendarExtender" runat="server" TargetControlID="TxtFechaIni2" PopupButtonID="imgCalendarioResolucionDesde32" />

                      <%--      <asp:CustomValidator ID="cvFechaRec2" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaIni2" ValidationGroup="CamposVacios" Display="None" />

                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender32" runat="server" TargetControlID="cvFechaRec2" PopupPosition="Right" />--%>

                            <asp:Label ID="Label6" Text="Fecha fin" runat="server" />:&nbsp;&nbsp;&nbsp;&nbsp; <div class='input-group date' id='imgCalendarioResolucionDesde22'>
     <asp:TextBox ID="TxtFechaFin2" runat="server" class="form-control" CausesValidation="True" />
     <span class="input-group-addon">
         <span class="glyphicon glyphicon-calendar"></span>
     </span>
 </div>

                            <act:CalendarExtender ID="imgCalendarioResolucionDesde22_CalendarExtender" runat="server" TargetControlID="TxtFechaFin2" PopupButtonID="imgCalendarioResolucionDesde22" />


                          <%--  <asp:CustomValidator ID="cvFechaEnv2" ClientValidationFunction="ValidateFecha" runat="server" Text="*" ErrorMessage="Fecha Errónea" ControlToValidate="TxtFechaFin2" ValidationGroup="CamposVacios" Display="None" />

                            <act:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" TargetControlID="cvFechaEnv2" PopupPosition="Right" />--%>


                            <asp:Label runat="server" ID="Label7" Text="Puesto de trabajo" />: <asp:TextBox ID="TxtPuesto2" runat="server" class="form-control" />

                            <asp:Label runat="server" ID="Label9" Text="Función a realizar" />: <asp:TextBox ID="txtFuncion2" runat="server" class="form-control" Rows="6" TextMode="MultiLine" />

                            <asp:Label runat="server" ID="Label10" Text="Responsable" />: <asp:TextBox ID="txtResponsable2" runat="server" class="form-control" />

                            <act:AutoCompleteExtender ID="AutoCompleteExtender1" ServiceMethod="CargarResponsable"
                                runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                                CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerResponsable2"
                                TargetControlID="txtResponsable2" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                                CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                CompletionListItemCssClass="CompletionListItemCssClass" />

                            <asp:HiddenField ID="hfResponsable2" runat="server" />


                           <asp:Label  runat="server" ID="lblSolicitud" Text="Solicitudes" />

                            <asp:HiddenField runat="server" ID="HiddenField3" />
                            <asp:HiddenField runat="server" ID="HdnNombre" />
                            <asp:HiddenField runat="server" ID="HiddenField4" />
                            <asp:HiddenField runat="server" ID="hdnCodResp" />
                            <asp:HiddenField ID="HiddenField5" runat="server" />


                        </div>
                    </div>

                </div>

                <div class="panel-footer">


                    <div class="text-center">
                        <asp:Button ID="btnCancelar2" class="btn btn-primary" runat="server" Text="Cancelar" UseSubmitBehavior="false" />
                    
                    </div>

                </div>

            </asp:View>






        </asp:MultiView>



    </div>



</asp:Content>

