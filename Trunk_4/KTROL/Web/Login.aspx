<%@ Page Language="VB" MasterPageFile="~/MPSinMenu.Master" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="WebRaiz.Login" %>

<%@ MasterType VirtualPath="~/MPSinMenu.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>

<asp:Content ID="Content_Head" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="js/jQuery/jquery-1.7.2.min.js" ></script>
    <script type="text/javascript" src="js/jQuery/jquery-ui.js"></script>

    <link type="text/css" href="App_Themes/Tema1/jquery-ui.css" rel="stylesheet" />


    <script src="js/jQuery/jquery.numeric.js" type="text/javascript"></script>

    <!--Css y Js del teclado virtual -->
    <link type="text/css" href="App_Themes/Tema1/Keyboard/Keyboard.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jQuery/Keyboard/jquery.keyboard.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.datos-numericos').numeric({ decimal: false, negative: false });

            $('.keyboardNumeros').keyboard({
                openOn: null,
                stayOpen: true,
                autoAccept: true,
                layout: 'custom',
                usePreview: false,
                customLayout: {
                    //'default': ['7 8 9', '4 5 6', '1 2 3', '0 {clear} {bksp}', '{a} {c}']
                    'default': ['7 8 9', '4 5 6', '1 2 3', '{empty} 0 {empty}', '{clear} {bksp}', '{a} {c}']
                },
                position: {
                    of: null,
                    my: 'center top',
                    at: 'center bottom'
                }
            });

            $('.keyboard').keyboard({
                openOn: null,
                stayOpen: true,
                autoAccept: true,
                layout: 'qwerty',
                usePreview: false,
                position: {
                    of: null,
                    my: 'center top',
                    at: 'center bottom'
                }
            });

            $('#imgKeyboardUsuario').click(function () {
                $('.keyboardNumeros').getkeyboard().reveal();
                $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
                $('.ui-widget-content .ui-state-default').css('color', '#000000');
                $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
                $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
            });

            $('#imgKeyboardPassword').click(function () {
                $('.keyboard').getkeyboard().reveal();
                $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
                $('.ui-widget-content .ui-state-default').css('color', '#000000');
                $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
                $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
            });

            //Ponemos el foco en el input del código de trabajador.
            $("#" + '<%=txtUsuario.ClientID%>').focus();
        });

        function ComprobarCampos() {
            var txtUsuario = $("[id$='txtUsuario']").attr('id');
            var usuario = $('#' + txtUsuario).val();
            var lblErrorUsuario = $("[id$='lblErrorUsuario']").attr('id');
            $('#' + lblErrorUsuario).css('display', 'none');
            if (usuario.length != 0) {
                return true;
            }
            else {
                if (usuario.length == 0) {
                    $('#' + lblErrorUsuario).css('display', 'inline');
                }
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cp" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnEntrar">
                <table width="100%">
                    <tr>
                        <td>
                            <td align="right">
                                <asp:ImageButton runat="server" ID="imgCerrar" ToolTip="Salir" ImageUrl="App_Themes/Tema1/Imagenes/cerrar.png" CausesValidation="false" OnClientClick="window.close();return false;" />
                            </td>
                        </td>
                    </tr>
                </table>
                <div style="height: 100px"></div>
                <div class="centrado">

                    <table width="50%">
                        <tr style="width: 100%; color: #000000; font-size: 24px; font-weight: bold; background-image: none; background-attachment: scroll; background-repeat: repeat; background-color: #CCCCCC; margin-bottom: 5px">
                            <th colspan="2" style="width: 100%">
                                <asp:Label ID="labelAccesoKtrol" runat="server" Text="Acceso a Ktrol" />
                            </th>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 30%">
                                <asp:Label runat="server" Text="IdTrabajador" ID="labelIdTra" Font-Size="18px" Width="100%"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 65%">
                                <asp:TextBox runat="server" ID="txtUsuario" CssClass="keyboardNumeros loginTextBoxStyle datos-numericos" Font-Size="20px" Width="70%"></asp:TextBox>
                                <img id="imgKeyboardUsuario" alt="" class="tooltip" title="Haz click para ver el teclado virtual" src="App_Themes/Tema1/Keyboard/keyboard.png" />
                            </td>
                            <td style="text-align: left; width: 5%">
                                <asp:Label ID="lblErrorUsuario" runat="server" Text="*" Style="display: none" Font-Size="18px" ForeColor="red" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp
                            </td>
                        </tr>
                        <asp:Panel runat="server" ID="pnlError">
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblError" CssClass="MensajeError" Font-Size="16px"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td align="center" colspan="2">
                                <br />
                                <asp:Button runat="server" ID="btnEntrar" Text="entrar" CssClass="loginButtonStyle" Font-Size="18px" OnClientClick="return ComprobarCampos()" /><%--  --%>
                            </td>
                        </tr>
                    </table>
                    <div style="height: 100px"></div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <PanelCargandoDatos:PanelCargandoDatos ID="PanelCargandoDatos1" runat="server" />
</asp:Content>
