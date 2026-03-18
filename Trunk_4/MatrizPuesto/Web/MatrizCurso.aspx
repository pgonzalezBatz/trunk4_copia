<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Adok_plantas.Master" CodeBehind="MatrizCurso.aspx.vb" Inherits="AdokWeb.MatrizCurso" %>

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

        function RecogerProfesion(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfProfesion.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }

        function RecogerResponsable(source, eventArgs) {
            var hfIdUsuario = document.getElementById('<%=hfResponsable.ClientID%>');
            hfIdUsuario.value = eventArgs.get_value();
        }



    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpoPrincipal" runat="server">
      <div class="container-fluid" style="background-color:#ebebeb;">
    <div class="navbar-header">
      <a class="navbar-brand" href="#" ><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="Matriz de Cursos Profesión" /></a>
    </div>
  </div>

    <div class="container " >

        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">


            <asp:View ID="viewListado" runat="server">



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
                

     
                                  <div class="form-group col-sm-12" style="text-align: center;">
                                             
    <asp:Button ID="botonVolver" runat="server" class="btn btn-primary" Text="Volver" UseSubmitBehavior="false" onclick="Volver" />
                
  </div> 
                 <hr />
 



            </asp:View>


            <asp:View ID="view1" runat="server">
                    <br />
           


                <div class="row" visible="false">
                    <div class="col-xs-4" visible="false">
                        <asp:Label Visible="false"  runat="server" ID="ex61" Text="Curso" />

                  <asp:TextBox Visible="false" ID="txtNombre" runat="server" class="form-control" ReadOnly="True" />

                    </div>
                    <div class="col-xs-3">

                        <asp:Label Visible="false" runat="server" ID="Cif2" Text="Nif" />

                        <asp:TextBox Visible="false" ID="txtCIF" runat="server" class="form-control" ReadOnly="True" />



                    </div>
                </div>


                <div class="row">
                    <div class="col-xs-3">

                       <asp:Label runat="server" ID="Label8" Text="Curso" />:

                        <asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceAlmacen" ServiceMethod="CargarCursos"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerEmpresa"
                            TargetControlID="txtEmpresa" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />
                         
                        <asp:HiddenField ID="hfEmpresa" runat="server" />    

                                          </div>
                                <div class="col-xs-3">

                      <asp:Label runat="server" ID="Label9" Text="Profesión" />:

                        <asp:TextBox class="form-control" type="text" ID="txtProfesion" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceProfesion" ServiceMethod="CargarProfesion"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerProfesion"
                            TargetControlID="txtProfesion" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />
                         
                        <asp:HiddenField ID="hfProfesion" runat="server" /> 

                                     
                    </div> 



                      <div class="col-xs-3"> 

                      <asp:Label runat="server" ID="Label12" Text="Responsable" />:

                        <asp:TextBox class="form-control" type="text" ID="txtResponsable" runat="server" AutoPostBack="True" />
                        <act:AutoCompleteExtender ID="aceResponsable" ServiceMethod="CargarResponsables"
                            runat="server" MinimumPrefixLength="2" Enabled="True" FirstRowSelected="true"
                            CompletionInterval="100" EnableCaching="false" OnClientItemSelected="RecogerResponsable"
                            TargetControlID="txtResponsable" UseContextKey="true" ServicePath="~/WS/adok_ws.asmx" CompletionSetCount="0"
                            CompletionListCssClass="CompletionListCssClass" CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                            CompletionListItemCssClass="CompletionListItemCssClass" />
                         
                        <asp:HiddenField ID="hfResponsable" runat="server" /> 


                    </div>


                                          <div class="col-xs-2">

                      <asp:Label runat="server" ID="Label13" Text="TODOS" />

                                 <asp:DropDownList ID="ddlTodos" runat="server" class="form-control"  AutoPostBack="True">
                                        <asp:ListItem Text="TODOS" Value="0" />
                                        <asp:ListItem Text="Caducados" Value="1" />
                                        <asp:ListItem Text="Caduca en 3 meses" Value="2" />
                                    </asp:DropDownList>

                    </div>

                </div>

                <br />

                   

                <asp:GridView ID="gvType2" runat="Server" AutoGenerateColumns="False" CssClass="table table-striped"
                    GridLines="None" DataKeyNames="Comentario" ShowFooter="false"
                  >
   
                    <EditRowStyle BackColor="#ffffcc" />
                    <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Nodoc" runat="server" CssClass="label label-warning" Text="¡No hay documentos a mostrar!" /> 
                    </EmptyDataTemplate>

                    <Columns>
                        <asp:BoundField DataField="Comentario" HeaderText="Comentario" Visible="false" />
                        <asp:TemplateField HeaderText="Cod." ItemStyle-HorizontalAlign="Center" Visible="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblID" Text='<%# Eval("Comentario")%>' runat="server" Visible="false"/>
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Curso">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblTipo" Text='<%# Eval("NIF")%>' runat="server" />--%>
                                 <asp:Label ID="lblTipo" runat="server" ToolTip='<%# Eval("NIF")%>' Text='<%# Eval("NIF")%>'  />
                            </ItemTemplate>

                        </asp:TemplateField>



                   <asp:TemplateField HeaderText="Trabajador"> 
                            <ItemTemplate>
                                <asp:Label ID="lblNomTrabajador" Text='<%# Eval("txtcorrecto")%>' runat="server" />
                            </ItemTemplate>

                     </asp:TemplateField>





                        <asp:TemplateField HeaderText="Horas">
                            <ItemTemplate>
                                <asp:Label ID="lblNomTra" Text='<%# Eval("nomtra")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc2") = "", "false", "True") %>' ID="imgDoc1" Text='<%# Eval("xdoc2")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc2")%>'/>        
                            </ItemTemplate>
                        </asp:TemplateField>

                         
                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
      <asp:LinkButton visible='<%# If(Eval("xdoc3") = "", "false", "True") %>' ID="imgDoc2" Text='<%# Eval("xdoc3")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc3")%>' />      
                            </ItemTemplate>
                        </asp:TemplateField>



                    <asp:TemplateField HeaderText="">
                         <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc4") = "", "false", "True") %>' ID="imgDoc3" Text='<%# Eval("xdoc4")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc4")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc5") = "", "false", "True") %>' ID="imgDoc4" Text='<%# Eval("xdoc5")%>' runat="server"  CommandName="Curso2" CommandArgument='<%# Eval("fdoc5")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>


                     <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton  visible='<%# If(Eval("xdoc6") = "", "false", "True") %>' ID="imgDoc5" Text='<%# Eval("xdoc6")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc6")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField> 


                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc7") = "", "false", "True") %>'  ID="imgDoc6" Text='<%# Eval("xdoc7")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc7")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc8") = "", "false", "True") %>'  ID="imgDoc7" Text='<%# Eval("xdoc8")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc8")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc9") = "", "false", "True") %>'  ID="imgDoc8" Text='<%# Eval("xdoc9")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc9")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc10") = "", "false", "True") %>'  ID="imgDoc9" Text='<%# Eval("xdoc10")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc10")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc11") = "", "false", "True") %>'  ID="imgDoc10" Text='<%# Eval("xdoc11")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc11")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
     <asp:LinkButton visible='<%# If(Eval("xdoc12") = "", "false", "True") %>'  ID="imgDoc11" Text='<%# Eval("xdoc12")%>' runat="server" CommandName="Curso2" CommandArgument='<%# Eval("fdoc12")%>' />           
                            </ItemTemplate>
                        </asp:TemplateField>                        






                       <asp:TemplateField HeaderText="Responsable">
                            <ItemTemplate>
                                <asp:Label ID="lblNomResponsabler" Text='<%# Eval("Abrev")%>' runat="server" />
                            </ItemTemplate>

                        </asp:TemplateField>


                    </Columns>


                </asp:GridView>


  


                 <br />


                     <hr />
                <div class="form-inline row">
                                  <div class="form-group col-sm-5">
                                               <asp:Button class="btn btn-primary" visible="false" runat="server"  ID="btoVolver" Text="Volver"  />
     </div> <div class="form-group col-sm-7">

             <h4 >             <span  style="text-align: left;" class="label label-primary"><asp:Label ID="Label10" runat="server" Text="Total trabajadores" />: </span>&nbsp;

                    <span class="label label-primary">
                        <asp:Label ID="Registros" runat="server" Text="" />
                    </span>
              
             </h4>
                    </div>
                </div>
                     <hr />
            
                    
              




            </asp:View>



 



        </asp:MultiView>




        <asp:HiddenField runat="server" ID="idTrabajador" />
        <asp:HiddenField runat="server" ID="DescEmpresa" />
        <asp:HiddenField runat="server" ID="idDoc" />
        <asp:HiddenField runat="server" ID="idPlantilla" />
                <asp:HiddenField runat="server" ID="FechaValG" />
        <asp:HiddenField runat="server" ID="FechaValG2" />
        <asp:HiddenField runat="server" ID="hfurl" />


    </div>



</asp:Content>

