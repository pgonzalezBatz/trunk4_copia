<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Relacionar.aspx.vb" Inherits="GTK_Troqueleria.Relacionar" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var RutaAplicacion = "<%= If(Request.ApplicationPath = "/", String.Empty, Request.ApplicationPath)%>";
        function TreeView_ExclusiveCheckBox_tvOrigen(evt, tvNodes) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            return isChkBoxClick; //comment this if you want postback on node click
        }

        function pageLoad() {
            $("#<%= tvOrigen.ClientID%> a").on('click', function () { if ($(this.previousSibling).is(':checkbox')) { $(this.previousSibling).trigger("click"); } })
            $("#<%= tvDestino.ClientID%> a").on('click', function () { if ($(this.previousSibling).is(':checkbox')) { $(this.previousSibling).trigger("click"); } })
          
            $("#<%= tvOrigen.ClientID%>").on('change', ':checkbox', function () {
                /* ------------------------------------------------------------- */
                //Marcamos un unico elemento del arbol origen.
                /* ------------------------------------------------------------- */
                //$find('pce_pnl_tvOrigen').hidePopup();
                var nodeValue = GetNodeValue_JQ(this.nextSibling);
                var nodeText = this.nextSibling.innerText;
                var tvOrigen = $("#<%= tvOrigen.ClientID%>")[0];
                var chBoxes = tvOrigen.getElementsByTagName('input');
                for (var i = 0; i < chBoxes.length; i++) {
                    var chk = chBoxes[i];
                    if ((chk.type == "checkbox")) {
                        var Nodo = chk.nextSibling;
                        //var chkValue = parseInt(GetNodeValue_JQ(Nodo));
                        var chkValue = GetNodeValue_JQ(Nodo);
                        chk.checked = (nodeValue.indexOf(chkValue) > -1);
                    }
                }
                /* ------------------------------------------------------------- */

                $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    url: RutaAplicacion + "/Controles/ServiciosWeb.asmx/get_Relaciones_Mant",
                    dataType: 'json',
                    data: JSON.stringify({ Id_Origen: nodeValue }),
                    success: function (Respuesta) {
                        if (Respuesta.d.length) {
                            var ItemDestino = JSON.parse(Respuesta.d);

                            /******************************************* */
                            var tvDestino = $("#<%= tvDestino.ClientID%>")[0];
                            var chBoxes = tvDestino.getElementsByTagName('input');
                            for (var i = 0; i < chBoxes.length; i++) {
                                var chk = chBoxes[i];
                                if ((chk.type == "checkbox")) {
                                    var Nodo = chk.nextSibling;
                                    var chkValue = parseInt(GetNodeValue_JQ(Nodo));
                                    //chk.checked = (nodeValue.indexOf(chkValue) > -1);
                                    chk.checked = (ItemDestino.indexOf(chkValue) > -1);
                                }
                            }
                            /******************************************* */
                        }
                    },
                    error: function (ex) {
                        alert(ex.statusText + ": " + ex.responseText);
                    }
                });
            })

            function GetNodeValue_JQ(node) {
                var nodeValue = "";
                var nodePath = node.href.substring(node.href.indexOf(",") + 2, node.href.length - 2);
                var nodeValues = nodePath.split("\\");
                if (nodeValues.length > 1)
                    nodeValue = nodeValues[nodeValues.length - 1];
                else
                    nodeValue = nodeValues[0].substr(1);
                return nodeValue;
            }
        }

        /* Validacion del TreeView de Origen *****************************************************************************/
        function ClientValidate_TreeView(source, arguments) {
            var treeView = document.getElementById("<%= tvOrigen.ClientID%>");
            var checkBoxes = treeView.getElementsByTagName("input");
            var checkedCount = 0;
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].checked) {
                    checkedCount++;
                }
            }
            arguments.IsValid = (checkedCount > 0);
        }
        /*******************************************************************************************************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">
    <table width="80%">
        <tr class="HeaderStyle">
            <th>
                <asp:Label ID="lblEstructura_Origen" runat="server" Text="?"></asp:Label>
            </th>
            <th>
                <asp:Label ID="lblEstructura_Destino" runat="server" Text="?" />
            </th>
        </tr>
        <tr>
            <td style="vertical-align: top;">
                <table style="margin-right: auto; margin-left: auto;">
                    <tr>
                        <td>
                            <asp:TreeView ID="tvOrigen" runat="server" SkinID="TreeView" ViewStateMode="Enabled"
                                ShowCheckBoxes="Leaf" onclick="return TreeView_ExclusiveCheckBox_tvOrigen(event, this)" />
                        </td>
                        <td>
                            <div style="visibility: hidden">
                                <asp:TextBox runat="server" ID="txt_TreeView" Text="0" Width="1"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table style="margin-right: auto; margin-left: auto;">
                    <tr>
                        <td>
                            <asp:TreeView ID="tvDestino" runat="server" SkinID="TreeView" ViewStateMode="Enabled"
                                ShowCheckBoxes="Leaf" onclick="return TreeView_ExclusiveCheckBox_tvOrigen(event, this)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="FooterStyle">
            <td colspan="2">
                <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
                    <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" />
                </asp:Panel>
            </td>
        </tr>
    </table>

    <!-- Origen Obligatorio --------------------------------------------------------------------------->
    <asp:CustomValidator runat="server" ID="cv_TreeView" ControlToValidate="txt_TreeView" ClientValidationFunction="ClientValidate_TreeView" Display="None" ErrorMessage="Campo obligatorio" ValidationGroup="btnGuardar"></asp:CustomValidator>
    <act:ValidatorCalloutExtender ID="vce_cv_TreeView" runat="server" TargetControlID="cv_TreeView" PopupPosition="Right"></act:ValidatorCalloutExtender>
    <!--------------------------------------------------------------------------------------------------->
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>--%>