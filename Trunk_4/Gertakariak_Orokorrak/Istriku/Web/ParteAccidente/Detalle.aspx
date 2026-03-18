<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/MPWeb.Master" codebehind="Detalle.aspx.vb" inherits="IstrikuWebRaiz.Detalle" %>

<%@ mastertype virtualpath="~/MPWeb.Master" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:content id="Content2" contentplaceholderid="cp" runat="server">
    <ascx:Titulo ID="Titulo_ID" runat="server" Texto="ID: {0}" />

    <act:TabContainer ID="tc_Detalle" runat="server" ActiveTabIndex="0">

        <act:TabPanel ID="tp_DatosSuceso" runat="server" HeaderText="Datos del Suceso">
            <ContentTemplate>
                <table class="GridViewASP">
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label5" runat="server" Text="Lugar del Suceso" /></th>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtLugarSuceso" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label6" runat="server" Text="Agente del Suceso" /></th>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtAgenteSuceso" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label1" runat="server" Text="Medio de manipulación" /></th>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtMedioManipulacion" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                         <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label7" runat="server" Text="Daños Materiales" /></th>
                        <td class="RowStyle">
                            <asp:CheckBox id="chkDañosMateriales" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </act:TabPanel>

        <act:TabPanel ID="tp_Antecedentes" runat="server" HeaderText="Antecedentes">
            <ContentTemplate>
                <table class="GridViewASP">
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label2" runat="server" Text="Descripcion" /></th>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtAntecedentes" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                 <style>
        .ajax__fileupload_button {
            background-color: green;
        }
    </style>

                <script type="text/javascript">

                    function onClientUploadComplete(sender, e) {
                        onImageValidated("TRUE", e);
                    }

                    function onImageValidated(arg, context) {

                        var test = document.getElementById("testuploaded");
                        test.style.display = 'block';

                        var fileList = document.getElementById("fileList");
                        var item = document.createElement('div');
                        item.style.padding = '4px';

                        if(arg == "TRUE") {
                            var url = context.get_postedUrl();
                            url = url.replace('&amp;', '&');
                            item.appendChild(createThumbnail(context, url));
                        } else {
                            item.appendChild(createFileInfo(context));
                        }

                        fileList.appendChild(item);
                    }

                    function createFileInfo(e) {
                        var holder = document.createElement('div');
                        holder.appendChild(document.createTextNode(e.get_fileName() + ' with size ' + e.get_fileSize() + ' bytes'));

                        return holder;
                    }

                    function createThumbnail(e, url) {
                        var holder = document.createElement('div');
                        var img = document.createElement("img");
                        img.style.width = '80px';
                        img.style.height = '80px';
                        img.setAttribute("src", url);

                        holder.appendChild(createFileInfo(e));
                        holder.appendChild(img);

                        return holder;
                    }

                    function onClientUploadStart(sender, e) {
                        document.getElementById('uploadCompleteInfo').innerHTML = 'Please wait while uploading ' + e.get_filesInQueue() + ' files...';
                    }

                    function onClientUploadError(sender, e) {
                        document.getElementById('uploadCompleteInfo').innerHTML = "There was an error while uploading.";
                    }

                    function onClientUploadCompleteAll(sender, e) {

                        var args = JSON.parse(e.get_serverArguments()),
                            unit = args.duration > 60 ? 'minutes' : 'seconds',
                            duration = (args.duration / (args.duration > 60 ? 60 : 1)).toFixed(2);

                        var info = 'At <b>' + args.time + '</b> server time <b>'
                            + e.get_filesUploaded() + '</b> of <b>' + e.get_filesInQueue()
                            + '</b> files were uploaded with status code <b>"' + e.get_reason()
                            + '"</b> in <b>' + duration + ' ' + unit + '</b>';

                        document.getElementById('uploadCompleteInfo').innerHTML = info;
                    }
                </script>

                Click <i>Select File</i> to select an image file to upload. You can upload a maximum of 10 jpeg files (files with the .jpg or .jpeg extension)
                <br />
                <asp:Label runat="server" ID="myThrobber" Style="display: none;"><img align="absmiddle" alt="" src="uploading.gif"/></asp:Label>
                <act:AjaxFileUpload ID="AjaxFileUpload1" runat="server" Padding-Bottom="4"
                    Padding-Left="2" Padding-Right="1" Padding-Top="4" ThrobberID="myThrobber" OnClientUploadComplete="onClientUploadComplete"
                    OnUploadComplete="AjaxFileUpload1_UploadComplete" MaximumNumberOfFiles="1"
                    OnClientUploadCompleteAll="onClientUploadCompleteAll" 
                    OnUploadCompleteAll="AjaxFileUpload1_UploadCompleteAll" 
                    OnUploadStart="AjaxFileUpload1_UploadStart" 
                    OnClientUploadStart="onClientUploadStart"
                    OnClientUploadError="onClientUploadError"
                    MaxFileSize="1"
                    AllowedFileTypes="jpg,jpeg,bmp,png,gif"/>

                <div id="uploadCompleteInfo"></div>
                <br />
                <div id="testuploaded" style="display: none; padding: 4px; border: gray 1px solid;">
                    <h4>list of uploaded files:</h4>
                    <hr />
                    <div id="fileList">
                    </div>
                </div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" />

            </ContentTemplate>
        </act:TabPanel>

        <act:TabPanel ID="tp_AgenteMaterialCausante" runat="server" HeaderText="Agente Material Causante">
            <ContentTemplate>
                <table class="GridViewASP">
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label13" runat="server" Text="Agente Material Causante" /></th>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtAgenteMateriaCausante" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </act:TabPanel>

        <act:TabPanel ID="tp_Descripcion" runat="server" HeaderText="Descripcion">
            <ContentTemplate>
                <table class="GridViewASP">
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label3" runat="server" Text="Secuencia de Operaciones" /></th>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtSecuenciasOperaciones" runat="server" Rows="5" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label4" runat="server" Text="Causas" /></th>
                        <td class="RowStyle">
                            <asp:TextBox ID="TextBox1" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="GridViewASP" title="Análisis">
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label8" runat="server" Text="Código de riesgo" /></th>
                        <td class="RowStyle" colspan="2">
                            <asp:TextBox ID="TextBox2" runat="server" Rows="5" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label9" runat="server" Text="Actos inseguros" /></th>
                        <td class="RowStyle"><asp:CheckBox id="chkActosInseguros" runat="server"></asp:CheckBox></td>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtActosInseguros" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label10" runat="server" Text="Condiciones Inseguras" /></th>
                        <td class="RowStyle"><asp:CheckBox id="chkCondicionesInseguras" runat="server"></asp:CheckBox></td>
                        <td class="RowStyle">
                            <asp:TextBox ID="txtCondicionesInseguras" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label11" runat="server" Text="EPIS Utilizados" /></th>
                        <td class="RowStyle" colspan="2">
                            <asp:TextBox ID="txtEpisUtilizados" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label12" runat="server" Text="EPIS Necesarios" /></th>
                        <td class="RowStyle" colspan="2">
                            <asp:TextBox ID="txtEpisNecesarios" runat="server" Rows="3" TextMode="MultiLine" MaxLength="2000" Width="98%"></asp:TextBox>
                        </td>
                    </tr>

                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label14" runat="server" Text="Nivel de riesgo del puesto" /></th>
                        <td class="RowStyle" colspan="2">
                            <asp:Label ID="Label15" runat="server" Text="¿Cómo estaba evaluado ese riesgo para ese puesto? (Marcar nivel de riesgo contemplado en la evaluación)" />
                            <asp:CheckBoxList runat="server">
                                <asp:ListItem Enabled="True" Selected="True" Text="Trivial" Value="Trivial"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="Moderado" Value="Moderado"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="Intolerable" Value="Intolerable"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="Tolerable" Value="Tolerable"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="Importante" Value="Importante"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="No evaluado" Value="No evaluado"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="No aplica" Value="No aplica"/>
                            </asp:CheckBoxList>
                        </td>
                    </tr>

                    <tr class="HeaderStyle">
                        <th style="width:1%; white-space:nowrap;"><asp:Label ID="Label16" runat="server" Text="Nivel de riesgo del suceso" /></th>
                        <td class="RowStyle" colspan="2">
                            <asp:Label ID="Label17" runat="server" Text="Nivel de riesgo del suceso, evaluando mediante la matriz Gravedad x Probabilidad" />
                            <table>
                                <tr><td></td><td></td><td></td><td></td></tr>
                                <tr><td></td><td></td><td></td><td></td></tr>
                                <tr><td></td><td></td><td></td><td></td></tr>
                                <tr><td></td><td></td><td></td><td></td></tr>
                                <tr><td></td><td></td><td></td><td></td></tr>
                                <tr><td></td><td></td><td></td><td></td></tr>
                            </table>
                            <asp:Label ID="Label18" runat="server" Text="¿Necesidad de modificar la evaluación de riesgos (tras revisión)?" />
                            <asp:CheckBoxList runat="server">
                                <asp:ListItem Enabled="True" Selected="True" Text="Si" Value="Si"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="No" Value="No"/>
                                <asp:ListItem Enabled="True" Selected="True" Text="No Aplica" Value="No Aplica"/>
                            </asp:CheckBoxList>
                        </td>
                    </tr>

                </table>
            </ContentTemplate>
        </act:TabPanel>

     </act:TabContainer>
<hr />    
    <div style="text-align: center">
		<asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
			<asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" />
		</asp:Panel>
	</div>
</asp:content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>
