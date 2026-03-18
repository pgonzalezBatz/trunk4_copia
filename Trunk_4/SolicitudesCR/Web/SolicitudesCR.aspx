<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SolicitudesCR.aspx.vb" Inherits="Web.SolicitudesCR" MasterPageFile="~/MPLogin.Master"%>
<%@ MasterType VirtualPath="~/MPLogin.Master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContenido" >    
          
    <script type="text/javascript">

        function RecogerEmpresa(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfEmpresa.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }


    </script>     

  <%--      <div class="navbar navbar-default">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target=".navbar-collapse" aria-expanded="false">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                 <asp:Image runat="server" ID="imgLogo" AlternateText="Portal del empleado" CssClass="navbar-brand" />


            <ul class="nav navbar-nav navbar-right">

                    <li runat="server" id="li2">
                        <a runat="server" id="a1" href="./ConsultaSolicitudes.aspx">
                          
                            <asp:Label runat="server" ID="Label2" Text="Consulta de Solicitudes"></asp:Label>
                        </a>
                    </li>                    
                
                </ul> 


            </div>
            <div class="navbar-collapse collapse">               
                <ul class="nav navbar-nav navbar-right">
                    <li runat="server" id="liUserCon"><a><span class="glyphicon glyphicon-user"></span>&nbsp;<asp:Label runat="server" ID="lblUserCon"></asp:Label></a></li>
                    <li runat="server" id="liHome">
                        <a runat="server" id="aHome" href="#">
                            <span class='glyphicon glyphicon-off'></span>
                            <asp:Label runat="server" ID="lblSalir" Text="Salir"></asp:Label>
                        </a>
                    </li>                    
                    <li runat="server" id="liSessionOff">
                        <a runat="server" id="aSessionOff" href="#">
                            <span class='glyphicon glyphicon-off'></span>
                            <asp:Label runat="server" ID="lblSessionOff" Text="Cerrar sesion"></asp:Label>
                        </a>
                    </li>                     
                </ul>                
             </div>
         </div>         
    </div>   --%>
















        <div class="container-fluid" >
        <div class="navbar-header">
        
                <%--<a class="navbar-brand" href="./Default2.aspx"><asp:Label ID="Label2" runat="server" Text="Crear Solicitudes Otro Usuario" Font-Underline="true" /></a>--%>

      <%--      <ul class="nav navbar-nav navbar-right">

                    <li runat="server" id="li2">
                        <a runat="server" id="a1" href="./ConsultaSolicitudes.aspx">
                          
                            <asp:Label runat="server" ID="Label2" Text="Consulta de Solicitudes"></asp:Label>
                        </a>
                    </li>                    
                
                </ul> --%>

        </div>
    </div>

    <div class="container ">



        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">




            <asp:View ID="view1" runat="server">
                             
                <div class="panel-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-lg-offset-2">

                            <div class="row">
                                <div class="col-xs-8">


                   
     








                                    <asp:Label runat="server" ID="lblNombre" Text="Nombre" />

                                    <asp:TextBox class="form-control" type="text" ID="NomEmp" runat="server" ReadOnly="True" AutoPostBack="True" />
                        <act:AutoCompleteExtender  ID="aceAlmacen" ServiceMethod="CargarResponsable"
                            runat="server" MinimumPrefixLength="1" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="NomEmp" UseContextKey="true" ServicePath="~/WS/CE_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />


                        <asp:HiddenField ID="hfEmpresa" runat="server" />	
                                </div>
        <%--                        <div class="col-xs-3">    ReadOnly="True"

                                    <asp:Label runat="server" ID="lblDestino" Text="Enviar a" />
                                    <asp:TextBox type="text" class="form-control" ID="txtDestino" runat="server" ReadOnly="True" />
                                </div>--%>


                     <div class="text-center">
                        <br />
                        <asp:Button visible="false" ID="btnDocumento" runat="server" OnClick="documento_Click" Text="Ver Código de Conducta" class="btn btn-primary" UseSubmitBehavior="true" />
                    </div>

                            </div>
                            <br />
                            

                            <asp:Label visible="false" runat="server" ID="lblAsunto" Text="Asunto de la denuncia" />

                             <asp:DropDownList visible="false" ID="DdlAsunto" runat="server" class="form-control">
                                  
                            <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="SEGURIDAD Y SALUD LABORAL" Value="SEGURIDAD Y SALUD LABORAL" />
                                <asp:ListItem Text="PROHIBICIÓN DE TRABAJO FORZOSO" Value="PROHIBICIÓN DE TRABAJO FORZOSO" />
                                <asp:ListItem Text="IGUALDAD Y ENTORNO DE TRABAJO RESPETUOSO" Value="IGUALDAD Y ENTORNO DE TRABAJO RESPETUOSO" />
                                <asp:ListItem Text="PRIVACIDAD, USO DE DATOS PERSONALES" Value="PRIVACIDAD, USO DE DATOS PERSONALES" />
                                <asp:ListItem Text="COMPROMISO CON EL ENTORNO" Value="COMPROMISO CON EL ENTORNO" />
                                <asp:ListItem Text="ADMINISTRACIONES PÚBLICAS Y POLÍTICAS ANTICORRUPCIÓN" Value="ADMINISTRACIONES PÚBLICAS Y POLÍTICAS ANTICORRUPCIÓN" />
                                <asp:ListItem Text="CLIENTES Y PROVEEDORES" Value="CLIENTES Y PROVEEDORES" />
                                <asp:ListItem Text="GESTIÓN DE LA INFORMACIÓN" Value="GESTIÓN DE LA INFORMACIÓN" />
                                <asp:ListItem Text="PROTECCIÓN DE LA PROPIEDAD INDUSTRIAL/INTELECTUAL. USO RESPONSABLE DE ACTIVOS Y RECURSOS DE LA EMPRESA" Value="PROTECCIÓN DE LA PROPIEDAD INDUSTRIAL/INTELECTUAL. USO RESPONSABLE DE ACTIVOS Y RECURSOS DE LA EMPRESA" />
                                <asp:ListItem Text="FOMENTO DE LA IMAGEN Y REPUTACIÓN DEL GRUPO BATZ" Value="FOMENTO DE LA IMAGEN Y REPUTACIÓN DEL GRUPO BATZ" />
                            </asp:DropDownList>
   <%--                         <asp:RequiredFieldValidator ID="rfvAsunto" runat="server" Text="*" ErrorMessage="Selecciona Categoría" ControlToValidate="DdlAsunto" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceAsunto" runat="server" TargetControlID="rfvAsunto" PopupPosition="Right" />--%>
                   
                              
                            <br />

                                                        <asp:Label runat="server" ID="lblDescC" Text="Descripción" />:
                                       
                                                <asp:TextBox ID="txtComentario" runat="server" class="form-control" Rows="6" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="rfvComentario" runat="server" Text="*" ErrorMessage="Añade Comentario" ControlToValidate="txtComentario" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vceComentario" runat="server" TargetControlID="rfvComentario" PopupPosition="Right" />
                            
                    <br />




<div class="row">
                    <div class="col-lg-12">

                        <asp:Label runat="server" ID="lblConfirmarBorrado" Text="Fichero a subir" />:
              </div>
                </div>
          
    <div class="row">
         <div class="col-lg-12">
                        <asp:FileUpload runat="server" ID="fuDoc" class="form-control" />
                              <asp:Button visible="false" class="btn btn-primary" runat="server"  ID="btnSubir2" Text="Subir"  />
                    </div>
                </div>  



     <%--                                   <div class="footer" style="text-align: center;">

                <br />--%>

          
                <%--<asp:Button class="btn btn-primary" ID="btnBorrar" runat="server" Text="Volver" UseSubmitBehavior="false" CausesValidation="false" />--%>

                <br />
            <%--</div>--%>








                             <asp:Label Font-Italic="true" Font-Size="Small" runat="server" ID="lblComment" Text="Batz garantiza la confidencialidad de esta información" />.

                        </div>
                    </div>



                    <br />

                </div>

                <div class="panel-footer">
                    <div class="text-center">
                        <asp:HiddenField runat="server" ID="flag_Modificar" />


                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-primary" Text="Cancelar" OnClick="btnCancelar_Click" />

                        <asp:Button ID="btnGuardarNuevaSolicitud" runat="server" class="btn btn-primary" OnClick="btnGuardarNuevaReferencia_Click" Text="Grabar" UseSubmitBehavior="true" ValidationGroup="CamposVacios" />
          

                    </div>
                </div>
                <br />
                <br />

            </asp:View>

        </asp:MultiView>


    </div>

</asp:Content>

