<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Kaplan_plantas.Master" CodeBehind="CrearCharacteristics.aspx.vb" Inherits="KaplanNew.CrearCharacteristics" %>

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
            <a class="navbar-brand" href="#"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label3" runat="server" Text="CLASE CHARACTERISTICS" /></a>
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
                        <asp:Label ID="Label1" runat="server" Text="Clase Characteristics" />

                        <asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarEmpresasCharacteristics"
                            runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfEmpresa" runat="server" />

                    </div>

                </div>

                <br />



                <asp:GridView ID="gvType" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="true"  PageSize="10"
                    GridLines="None" OnRowCommand="gvType_OnRowCommand" DataKeyNames="Id" ShowFooter="false" OnDataBound="gvType_DataBound"
                    OnRowCancelingEdit="gvType_RowCancelingEdit" OnRowEditing="gvType_RowEditing" OnRowUpdating="gvType_RowUpdating">
                    <%--<HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />--%>
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc2" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" />
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
                        <asp:TemplateField HeaderText="Codigo" ItemStyle-Width="90px">
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Id")%>' runat="server" />
                            </ItemTemplate>

                            <%--Font-Underline="true"--%>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" ItemStyle-Width="70%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblDescripcion" Text='<%# Eval("Nombre")%>' Font-Underline="true" runat="server" ToolTip="Modify Data" CommandName="Edit" />
                            </ItemTemplate>


                        </asp:TemplateField>




                        <asp:TemplateField HeaderText="Modify Data">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEditar" runat="server" Text="Modify Data" class="btn  bg-info" ToolTip="Modify Data" CommandName="Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>






                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar3" CssClass="btn btn-default" runat="server" CommandName="Desactivar" Text="Deactivate" Visible='<%# If(Eval("obsoleto") = 0, "true", "false") %>' />
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
                                <asp:Button ID="btnEliminar2" runat="server" CssClass="btn btn-default" CommandName="Activar" Text="Activate" Visible='<%# If(Eval("obsoleto") = 0, "false", "true") %>' />
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

                            <div class="row">
                                <div class="col-xs-6">
                                                    <asp:Label runat="server" ID="lblNombre" Text="Simbolo" />:
                                         <asp:TextBox ControlStyle-CssClass="BatzFont"  ID="txtSimbolo" runat="server" class="form-control" />
                          

             
                                </div>
                                   <div class="col-xs-2">
                                         </div>
                          <div class="col-xs-4">
                          
                              		<asp:HyperLink ID="HyperLink1" runat="server" Text="Tipo de letra 'Batz Font' requerida." NavigateUrl="~/App_Themes/Batz_Font.TTF" Target="_blank" />
		<asp:ImageButton ID="btnBatzFont" runat="server" ImageUrl="~/App_Themes/Ayuda16.png" CausesValidation="False" EnableViewState="False" OnClientClick="return false;" ImageAlign="AbsMiddle" />
		<asp:Panel ID="pnlCaracteresBatzFont" runat="server">
			<uc:CaracteresBatzFont ID="CaracteresBatzFont2" runat="server" />
		</asp:Panel>
		<act:BalloonPopupExtender ID="BalloonPopupExtender2" runat="server" TargetControlID="btnBatzFont" BalloonPopupControlID="pnlCaracteresBatzFont" Position="Auto" UseShadow="true" BalloonStyle="Cloud" BalloonSize="Large" />

               
                              </div>

                            </div>
                            <br />
                      <div class="row">
                                <div class="col-xs-6">

                                  <asp:Label runat="server" ID="ex1e" Text="Name" />

                                    <asp:TextBox class="form-control" type="text" ID="NomEmp" runat="server"  /> 

                                    <br /><br />


                          



           <asp:Label runat="server" ID="Label5" Text="Description" />:            
  <asp:TextBox type="text" class="form-control" ID="CifEmp" runat="server"/>

                                        <br />
                      <asp:Label  runat="server" ID="Label12" Text="Clientes" />

                            <asp:TextBox   ID="txtClientes" runat="server" class="form-control" />

                                    <br /><br />
                            <asp:Label visible="false" runat="server" ID="lblAplicacionC" Text="Obsolete" />



                                                <asp:Label visible="false"  runat="server" ID="Label2" Text="Process" />  

                                    <asp:DropDownList visible="false"  ID="DdlPreventiva3" runat="server" class="form-control">
                                    </asp:DropDownList>
                                    <br /><br />
                                 <asp:Label runat="server" ID="Label4" Text="Obsolete" />:  

                            <asp:DropDownList   ID="DdlSubcontrata" runat="server" class="form-control">

                                <asp:ListItem Text="No" Value="0" />
                                <asp:ListItem Text="Yes" Value="1" />
                            </asp:DropDownList>

            










            </div>
                                <div class="col-xs-2">
                                         </div>
                           <div class="col-xs-4" style="text-align: center;">
                         

              	<table>
		<tr>
			<td>
				<asp:DetailsView ID="dvTabla" runat="server" Height="50px" Width="500px" AutoGenerateRows="False" DataKeyNames="ID_CLASE"  Caption="Maestro de Clases" CssClass="DetailsView" GridLines="None" AllowPaging="false">
					<HeaderStyle CssClass="HeaderStyleDetailsView" />
					<FieldHeaderStyle CssClass="FieldHeaderStyle" />
					<AlternatingRowStyle CssClass="AlternatingRowStyleDetailsView" />
					<RowStyle CssClass="RowStyleDetailsView" />
					<FooterStyle CssClass="FooterStyle" />
					<PagerStyle CssClass="PagerStyle" />
					<EditRowStyle CssClass="EditRowStyle" />
					<CommandRowStyle CssClass="CommandRowStyleDetailsView" />
					<Fields>
						<asp:BoundField DataField="ID_CLASE" HeaderText="ID_CLASE" InsertVisible="False" ReadOnly="True" SortExpression="ID_CLASE" />
						<asp:TemplateField HeaderText="Clasificación característica especial" SortExpression="CLASE" ControlStyle-CssClass="BatzFont">
							<EditItemTemplate>
								<asp:TextBox ID="txtClase" runat="server" Text="dd" MaxLength="50" Width="100%"></asp:TextBox>
							</EditItemTemplate>
							<InsertItemTemplate>
								<asp:TextBox ID="txtClase" runat="server" Text="ss" MaxLength="50" Width="100%"></asp:TextBox>
							</InsertItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblClase" runat="server" Text="44"></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
				
						<asp:TemplateField HeaderText="DESCRIPCION" SortExpression="DESCRIPCION">
							<EditItemTemplate>
								<asp:TextBox ID="txtDescripcion" runat="server" Text="ee" MaxLength="50" Width="100%"></asp:TextBox>
							</EditItemTemplate>
							<InsertItemTemplate>
								<asp:TextBox ID="txtDescripcion" runat="server" Text="ee" MaxLength="50" Width="100%"></asp:TextBox>
							</InsertItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblDescripcion" runat="server" Text="ww"></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
				
						<asp:TemplateField HeaderText="frecuencia" SortExpression="frecuencia">
							<EditItemTemplate>
								<asp:TextBox ID="TextBox1" runat="server" Text="ww" MaxLength="9"></asp:TextBox>
								<act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBox1" FilterType="Numbers" />
							</EditItemTemplate>
							<InsertItemTemplate>
								<asp:TextBox ID="TextBox1" runat="server" Text="ww" MaxLength="9"></asp:TextBox>
								<act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBox1" FilterType="Numbers" />
							</InsertItemTemplate>
							<ItemTemplate>
								<asp:Label ID="Label1" runat="server" Text="ss"></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField ShowHeader="False">
							<EditItemTemplate>
								<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Update" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Actualizar" ToolTip="Actualizar" />
								&nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" AlternateText="Cancelar" ToolTip="Cancelar" />
							</EditItemTemplate>
							<InsertItemTemplate>
								<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Insert" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Insertar" ToolTip="Insertar" />
								&nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" AlternateText="Cancelar" ToolTip="Cancelar" />
							</InsertItemTemplate>
							<ItemTemplate>
								<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="~/App_Themes/Batz/IconosAcciones/Editar24.png" AlternateText="Editar" ToolTip="Editar" />
								&nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="New" ImageUrl="~/App_Themes/Batz/IconosAcciones/Nuevo24.png" AlternateText="Nuevo" ToolTip="Nuevo" />
								&nbsp;<asp:ImageButton ID="imgEliminar" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Eliminar" ToolTip="Eliminar" />
								<act:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="imgEliminar" ConfirmText="Desea eliminar" />
							</ItemTemplate>
						</asp:TemplateField>
					</Fields>
				</asp:DetailsView>

			</td>
			<td>
				<div style="display: inline">
					<uc:CaracteresBatzFont ID="CaracteresBatzFont1" runat="server" />
				</div>
			</td>
		</tr>
	</table>

                             
                          </div>
            </div>




                        </div>
                    </div>



                    <br />
                    <br />

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

                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-primary" Text="Cancel" UseSubmitBehavior="false" />

                        <asp:Button ID="btnGuardarNuevaSolicitud" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia_Click" Text="Save" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />

                        <asp:Button ID="btnLimpiarCampos" Visible="false" runat="server" CausesValidation="false" class="btn btn-primary" OnClick="btnLimpiarCampos_Click" Text="Limpiar campos" />

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
                            <asp:Label runat="server" ID="ex1v" Text="Name" />:


                            <asp:TextBox class="form-control required" ID="txtNombre" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvCIF" runat="server" Text="*" ErrorMessage="Insert Name" ControlToValidate="txtCIF" ValidationGroup="CamposVacios" Display="None" />



                            <asp:Label runat="server" ID="Label23" Text="Description" />:

                            <asp:TextBox ID="txtCIF" class="form-control required" runat="server" />



                           <asp:Label visible="false"  runat="server" ID="lblemail" Text="Process" />

                                    <asp:DropDownList visible="false" ID="DdlPreventiva2" runat="server" class="form-control">
                                    </asp:DropDownList>


                        </div>
                    </div>

                    <br />
                    <br />

                </div>


                <div class="panel-footer">
                    <div class="text-center">



                        <asp:Button ID="CancelVista2" runat="server" class="btn btn-primary" Text="Cancel" UseSubmitBehavior="false" />

                        <asp:Button ID="GrabarVista2" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia2_Click" Text="Save" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />

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

        <%--         <div class="row" style="margin-top: 5px;">

          <div class="col-lg-12" style="text-align: center;">
                   <h4>
       <asp:Label ID="lblInfo" runat="server" />               
                </h4>
              </div></div>--%>
    </div>

</asp:Content>

