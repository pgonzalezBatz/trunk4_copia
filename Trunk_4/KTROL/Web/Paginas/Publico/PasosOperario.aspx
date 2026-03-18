<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PasosOperario.aspx.vb" Inherits="WebRaiz.PasosOperario" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <!--js de Jquery-->
    <script type="text/javascript" src="../../js/jQuery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery-ui.js"></script>

    <!--Css y Js del teclado virtual -->
    <script type="text/javascript" src="../../js/jQuery/Keyboard/jquery.keyboard.js"></script>
    <link href="../../App_Themes/Tema1/Keyboard/Keyboard.css" rel="stylesheet" />

    <link type="text/css" href="../../App_Themes/Tema1/jquery-ui.css" rel="stylesheet" />
    <%--<link href="~/App_Themes/Tema1/jquery-ui.css" rel="stylesheet" />--%>

    <!--Css de Ktrol -->
    <link type="text/css" href="../../App_Themes/Tema1/Ktrol.css" rel="stylesheet" />
    <%--<link href="~/App_Themes/Tema1/Ktrol.css" rel="stylesheet" />--%>
    <!--Css y Js del ThickBox -->
    <!--<script type="text/javascript" src="../../App_Themes/Tema1/ThickBox/thickbox-compressed.js"></script>-->
    <link type="text/css" href="../../App_Themes/Tema1/ThickBox/thickbox.css" rel="stylesheet" media="screen" />

    <!-- Js de numeric -->
    <script type="text/javascript" src="../../js/jQuery/jquery.numeric.js"></script>

    <style type="text/css">
        body, html {
            margin: 1px;
            padding: 0;
            border: 0;
        }
        .NavigationTemplate {
            position: absolute; top: 80%;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            var altura = $(document).height();
            altura = altura * 95 / 100;
            $('#wizard_Ktrol').height(altura);

            var alturaWizard = $('#wizard_Ktrol').height();
            var alturaCabecera = alturaWizard * 15 / 100;
            var alturaMain = (alturaWizard * 75) / 100;

            var alturaDatosCaracteristica = (alturaMain * 20) / 100;
            var alturaImagen = (alturaMain * 45) / 100;
            var alturaValor = (alturaMain * 35) / 100;

            $('.divAlturaCabecera').height(alturaCabecera);
            $('.divAlturaMain').height(alturaMain);
            $('.divAlturaDatosCaracteristica').height(alturaDatosCaracteristica);
            $('.divAlturaImagen').height(alturaImagen);
            $('.divAlturaValor').height(alturaValor);

            $('.alturaFilaInfo').height((alturaWizard * 15 / 2) / 100);

            tb_init('a.thickbox');

            $('.keyboard').keyboard({
                openOn: null,
                stayOpen: true,
                autoAccept: true,
                usePreview: false,
                lockInput: true,
                layout: 'custom',
                customLayout: {
                    //'default': ['7 8 9', '4 5 6', '1 2 3', '0 . {clear} {bksp}', '{a} {c}']
                    'default': ['7 8 9', '4 5 6', '1 2 3', '{sign} 0 .', '{clear} {bksp}', '{a} {c}']
                },
                position: {
                    of: null,
                    my: 'right top',
                    at: 'left top'
                }
            });

            $('.botonOK').click(function () {
                $(this).attr("src", "..\\..\\App_Themes\\Tema1\\Imagenes\\boton_OK_Seleccionado.png");
                var idControl = $(this).attr('id');
                var posicionOK = idControl.lastIndexOf("OK");
                var id = idControl.substr(posicionOK + 2, idControl.length - posicionOK - 2);
                var botonNOK = idControl.replace('OK', 'NOK');
                $('#' + botonNOK).attr("src", "..\\..\\App_Themes\\Tema1\\Imagenes\\boton_NOK_NoSeleccionado.png");

                var hf = "hfCheck" + id;
                var hfCheck = $("[id$='" + hf + "']").attr('id');
                $('#' + hfCheck).val('OK');

                var chk = "chkControlador" + id;
                var chkControlador = $("[id$='" + chk + "']").attr('id');
                $('#' + chkControlador).attr('checked', 'true');

                var rfv = "rfvControlador" + id;
                var rfvControlador = $("[id$='" + rfv + "']").attr('id');
                $('#' + rfvControlador).text('');

                return false;
            });

            $('.botonNOK').click(function () {
                var obj = $(this);
                $("#modal_dialog").dialog({
                    resizable: false,
                    draggable: false,
                    height: 270,
                    width: 600,
                    buttons: {
                        SI: function () {
                            obj.attr("src", "..\\..\\App_Themes\\Tema1\\Imagenes\\boton_NOK_Seleccionado.png");
                            var idControl = obj.attr('id');
                            var posicionNOK = idControl.lastIndexOf("NOK");
                            var id = idControl.substr(posicionNOK + 3, idControl.length - posicionNOK - 3);
                            var botonOK = idControl.replace('NOK', 'OK');
                            $('#' + botonOK).attr("src", "..\\..\\App_Themes\\Tema1\\Imagenes\\boton_OK_NoSeleccionado.png");

                            var hf = "hfCheck" + id;
                            var hfCheck = $("[id$='" + hf + "']").attr('id');
                            $('#' + hfCheck).val('NOK');

                            var chk = "chkControlador" + id;
                            var chkControlador = $("[id$='" + chk + "']").attr('id');
                            $('#' + chkControlador).attr('checked', 'true');

                            var rfv = "rfvControlador" + id;
                            var rfvControlador = $("[id$='" + rfv + "']").attr('id');
                            $('#' + rfvControlador).text('');
                            $(this).dialog("close");
                        },
                        NO: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
                $('.ui-dialog-buttonset>button:first-child').css('float', 'left');
                $('.ui-dialog-buttonset>button:first-child').css('margin-left', '20%')
                $('.ui-dialog-buttonset>button:last-child').css('float', 'right');
                $('.ui-dialog-buttonset>button:last-child').css('margin-right', '20%')
                return false;
            });

            $('.salir').click(function () {
                $("#modal_Salir").dialog({
                    resizable: false,
                    draggable: false,
                    height: 270,
                    width: 600,
                    buttons: {
                        SI: function () {
                            window.location.href = 'SeleccionRefOp.aspx';
                            $(this).dialog("close");
                        },
                        NO: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
                $('.ui-dialog-buttonset>button:first-child').css('float', 'left');
                $('.ui-dialog-buttonset>button:first-child').css('margin-left', '20%')
                $('.ui-dialog-buttonset>button:last-child').css('float', 'right');
                $('.ui-dialog-buttonset>button:last-child').css('margin-right', '20%')
                return false;
            });

            $('.datos-numericos').bind("click", function () {
                $('.keyboard').getkeyboard().reveal();
                $('.ui-widget-content .ui-state-default').css('background-color', 'rgb(230, 239, 240)');
                $('.ui-widget-content .ui-state-default').css('color', '#000000');
                $('.ui-widget-content .ui-state-cancel').css('background-color', 'rgb(255, 60, 60)');
                $('.ui-widget-content .ui-state-cancel').css('color', '#ffffff');
            });
        });

        $(document).ready(function () {
            $('.datos-numericos').numeric({ negative: false, decimal: '.' });

            function updateTime() {
                var idPaso = $("[id$='hfPasoActual']").val();
                var paso = "lblFecha" + idPaso;
                var lblFecha = $("[id$='" + paso + "']").attr('id');
                var now = new Date();
                var hora = now.getHours().toString();
                var minutos = now.getMinutes().toString();
                var segundos = now.getSeconds().toString();
                if (hora.length == 1) { hora = "0" + hora; }
                if (minutos.length == 1) { minutos = "0" + minutos; }
                if (segundos.length == 1) { segundos = "0" + segundos; }
                var horaActual = hora + ":" + minutos + ":" + segundos;
                $('#' + lblFecha).text(horaActual);
            }

            updateTime();
            setInterval(updateTime, 1000);
        });


        function ComprobarCheck(sender, e) {
            e.IsValid = jQuery(".chkControladorClass input:checkbox").is(':checked');
        }

        function ComprobarCheckTextbox(sender, e) {
            var hfPaso = $("[id$='hfPasoActual']").attr('id');
            var id = $('#' + hfPaso).val();
            var hfErrorCaracteristica = "hfErrorCaracteristica" + id;
            var tieneError = $("[id$='" + hfErrorCaracteristica + "']").attr('id');
            var valor = $('#' + tieneError).val();
            if (valor == "0") {
                var hfMinId = "hfMin" + id;
                var hfMin = $("[id$='" + hfMinId + "']").attr('id');
                var hfMaxId = "hfMax" + id;
                var hfMax = $("[id$='" + hfMaxId + "']").attr('id');
                var valorMin = parseFloat(($('#' + hfMin).val()).replace(",", "."));
                var valorMax = parseFloat(($('#' + hfMax).val()).replace(",", "."));
                var txtRegistroDatosId = "txtRegistroDatos" + id;
                var txtRegistroDatos = $("[id$='" + txtRegistroDatosId + "']").attr('id');
                var imgMensajeErrorId = "imgMensajeError" + id;
                var imgMensajeError = $("[id$='" + imgMensajeErrorId + "']").attr('id');
                var lblMensajeErrorId = "lblMensajeError" + id;
                var lblMensajeError = $("[id$='" + lblMensajeErrorId + "']").attr('id');
                var lblMensajeVacio = "lblMensajeVacio" + id;
                var mensajeVacio = $("[id$='" + lblMensajeVacio + "']").attr('id');
                var valorTxtRegistroDatos = $('#' + txtRegistroDatos).val();
                if (valorTxtRegistroDatos.length == 0) {
                    $('#' + mensajeVacio).css('display', 'inline');
                    e.IsValid = false;
                }
                else {
                    var valorTxtRegistroDatos = parseFloat((valorTxtRegistroDatos).replace(",", "."));
                    $('#' + mensajeVacio).css('display', 'none');
                    if (valorTxtRegistroDatos >= valorMin & valorTxtRegistroDatos <= valorMax) {
                        //No hacer nada
                    }
                    else {
                        e.IsValid = false;
                        $('#' + txtRegistroDatos).css('background-color', '#D99694')
                        $("#modal_dialog").dialog({
                            resizable: false,
                            draggable: false,
                            height: 270,
                            width: 600,
                            buttons: {
                                SI: function () {
                                    $('#' + tieneError).val("1");
                                    $('#' + txtRegistroDatos).attr('readonly', true);
                                    $('#' + txtRegistroDatos).css('color', '#000000');
                                    $('#' + imgMensajeError).css('display', 'inline');
                                    $('#' + lblMensajeError).css('display', 'inline');
                                    $(this).dialog("close");
                                },
                                NO: function () {
                                    $('#' + txtRegistroDatos).css('background-color', '#ffffff')
                                    $(this).dialog('close');
                                }
                            },
                            modal: true
                        });
                        $('.ui-dialog-buttonset>button:first-child').css('float', 'left');
                        $('.ui-dialog-buttonset>button:first-child').css('margin-left', '20%')
                        $('.ui-dialog-buttonset>button:last-child').css('float', 'right');
                        $('.ui-dialog-buttonset>button:last-child').css('margin-right', '20%')
                    }
                }

            }
            else {
                e.IsValid = true;
            }
        }

        function ConfirmarSalir() {
            var pregunta = confirm("Desea volver a la selección de referencia y código de operación?")
            if (pregunta) {
                return true;
            }
            else {
                return false;
            }
        }

        function ValidarMaxMin(sender, args) {
            var idCV = sender.id
            var posicionCV = idCV.lastIndexOf("cv");
            var id = idCV.substr(posicionCV + 2, idCV.length - posicionCV - 2);
            var hfErrorCaracteristica = "hfErrorCaracteristica" + id;
            var tieneError = $("[id$='" + hfErrorCaracteristica + "']").attr('id');
            var valor = $('#' + tieneError).val();
            if (valor == "0") {
                var hfMinId = "hfMin" + id;
                var hfMin = $("[id$='" + hfMinId + "']").attr('id');
                var hfMaxId = "hfMax" + id;
                var hfMax = $("[id$='" + hfMaxId + "']").attr('id');
                var valorMin = parseFloat(($('#' + hfMin).val()).replace(",", "."));
                var valorMax = parseFloat(($('#' + hfMax).val()).replace(",", "."));
                var txtRegistroDatosId = "txtRegistroDatos" + id;
                var txtRegistroDatos = $("[id$='" + txtRegistroDatosId + "']").attr('id');
                var imgMensajeErrorId = "imgMensajeError" + id;
                var imgMensajeError = $("[id$='" + imgMensajeErrorId + "']").attr('id');
                var lblMensajeErrorId = "lblMensajeError" + id;
                var lblMensajeError = $("[id$='" + lblMensajeErrorId + "']").attr('id');
                var valorTxtRegistroDatos = parseFloat(($('#' + txtRegistroDatos).val()).replace(",", "."));
                if (valorTxtRegistroDatos >= valorMin & valorTxtRegistroDatos <= valorMax) {
                    //No hacer nada
                }
                else {
                    args.IsValid = false;
                    $('#' + txtRegistroDatos).css('background-color', '#D99694')
                    $("#modal_dialog").dialog({
                        resizable: false,
                        draggable: false,
                        height: 270,
                        width: 600,
                        buttons: {
                            SI: function () {
                                $('#' + tieneError).val("1");
                                $('#' + txtRegistroDatos).attr('readonly', true);
                                $('#' + txtRegistroDatos).css('color', '#000000');
                                $('#' + imgMensajeError).css('display', 'inline');
                                $('#' + lblMensajeError).css('display', 'inline');
                                $(this).dialog("close");
                            },
                            NO: function () {
                                $('#' + txtRegistroDatos).css('background-color', '#ffffff')
                                $(this).dialog('close');
                            }
                        },
                        modal: true
                    });
                    $('.ui-dialog-buttonset>button:first-child').css('float', 'left');
                    $('.ui-dialog-buttonset>button:first-child').css('margin-left', '20%')
                    $('.ui-dialog-buttonset>button:last-child').css('float', 'right');
                    $('.ui-dialog-buttonset>button:last-child').css('margin-right', '20%')
                }
            }
            else {
                args.IsValid = true;
            }
        }
    </script>
</head>
<body style="height: 100%">
    <script language="javascript" type="text/javascript">
        var tb_pathToImage = '<%= ResolveUrl("~/App_Themes/Tema1/ThickBox/loadingAnimation.gif") %>';
    </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="actScriptManager" runat="server">
            <Scripts>
                <asp:ScriptReference Path="~/App_Themes/Tema1/ThickBox/thickbox-compressed.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:Wizard ID="wizard_Ktrol" CellPadding="0" CellSpacing="0" BorderStyle="Solid" BorderWidth="0px" runat="server" Width="100%" DisplaySideBar="false" Height="100%">

           <%-- <LayoutTemplate>
                <asp:PlaceHolder ID="sideBarPlaceHolder" runat="server" />
                <asp:PlaceHolder ID="headerPlaceHolder" runat="server" />
                <asp:PlaceHolder ID="navigationPlaceHolder" runat="server" />
                <asp:PlaceHolder ID="WizardStepPlaceHolder" runat="server" />
            </LayoutTemplate>--%>

            <HeaderTemplate>
                <table style="width: 100%; background-color: white">
                    <tr>
                        <td class="wizardExit">
                            <asp:LinkButton ID="lnbtSalir" runat="server" Text="Salir" CssClass="salir" CausesValidation="false" OnClick="lnbtSalir_Click" />
                        </td>
                        <td>
                            <table style="width: 99%; border-collapse: collapse;">
                                <tr>
                                    <td style="text-align: right">
                                        <span class="wizardProgress">Característica:</span>
                                    </td>
                                    <asp:Repeater ID="SideBarList" runat="server">
                                        <ItemTemplate>
                                            <td class="stepBreak">&nbsp;</td>
                                            <td class="<%# ClasePasoWizard(Container.DataItem)%>" title="<%# DataBinder.Eval(Container, "DataItem.Name") + 1%>">
                                                <%# wizard_Ktrol.WizardSteps.IndexOf(Container.DataItem) + 1%>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </HeaderTemplate>

            <StartNavigationTemplate>
                <div class="NavigationTemplate" style=" margin-left: 92%;">
                    <%--<div style="background-color: white">--%>
                    <asp:ImageButton ID="btnFirstStep" runat="server" AlternateText="Siguiente" ImageUrl="~/App_Themes/Tema1/Imagenes/imgSiguiente.png" OnClick="btnFirstStep_Click" CausesValidation="true"  />
                </div>
            </StartNavigationTemplate>
            <StepNavigationTemplate>
                <%--<div class="NavigationTemplate" style="float: left; text-align: left; width:30%;">--%>
                    <div style="float: left; padding: 0; background-color: white">
                    <asp:ImageButton ID="btnAnterior" runat="server" AlternateText="Anterior" ImageUrl="~/App_Themes/Tema1/Imagenes/imgAnterior.png" OnClick="btnAnterior_Click" CausesValidation="false" />
                </div>
                <%--<div class="NavigationTemplate" style="margin-left: 92%;">--%>
                    <div style="float: right; background-color: white">
                    <asp:ImageButton ID="btnSiguiente" runat="server" AlternateText="Siguiente" ImageUrl="~/App_Themes/Tema1/Imagenes/imgSiguiente.png" OnClick="btnSiguiente_Click" CausesValidation="true" />
                </div>
            </StepNavigationTemplate>
            <FinishNavigationTemplate>
                <%--<div class="NavigationTemplate" style="float: left; text-align: left; width:30%;">--%>
                    <div style="float: left; padding: 0; background-color: white">
                    <asp:ImageButton ID="btnAnterior" runat="server" AlternateText="Anterior" ImageUrl="~/App_Themes/Tema1/Imagenes/imgAnterior.png" OnClick="btnAnterior_Click" CausesValidation="false" />
                </div>
                <%--<div class="NavigationTemplate" style="margin-left: 96%;background-color:white;">--%>
                    <div style="float: right; background-color: white">
                    <asp:ImageButton ID="btnFinalizar" runat="server" AlternateText="Finalizar" ImageUrl="~/App_Themes/Tema1/Imagenes/Finalizar_paso.png" OnClick="btnFinalizar_Click" CausesValidation="true" />
                </div>
            </FinishNavigationTemplate>

            <SideBarTemplate>
            </SideBarTemplate>
        </asp:Wizard>

        <div id="modal_dialog" title="Confirmación" style="display: none; text-align: center">
            <div style="float: left; width: 50%">
                <asp:Label ID="lblConfirmacionNOK" runat="server" Text="Característica NOK" Font-Size="16px" />:
            </div>
            <div style="float: left; width: 50%">
                <br />
                <asp:Label ID="lblPregunta" runat="server" Text="¿CONFIRMAR?" Font-Size="16px" />
            </div>
        </div>

        <div id="modal_Salir" title="Salir" style="display: none; text-align: center">
            <asp:Label ID="lblSalir" runat="server" Text="¿Estás seguro que desea ir a la pantalla de elección de código de operación?" />
        </div>

        <div id="modal_Error" style="display: none" runat="server">
            <asp:Label ID="lblError" runat="server" Text="Ha ocurrido un error al cargar los datos de la operación. Haz click en el botón 'Salir' para volver a la pantalla anterior" Font-Bold="true" Font-Size="18px" ForeColor="red" />
            <br />
            <asp:Button ID="btnError" runat="server" Text="Salir" OnClick="btnError_Click" CssClass="botonera" Font-Size="18px" />
        </div>

        <asp:HiddenField ID="hfPasoActual" runat="server" />
        <ascx:Mensajes runat="server" ID="ascx_Mensajes" />
    </form>

</body>
</html>
