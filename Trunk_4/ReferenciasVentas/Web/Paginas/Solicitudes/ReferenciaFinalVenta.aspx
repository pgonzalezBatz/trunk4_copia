<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReferenciaFinalVenta.aspx.vb" MasterPageFile="~/RefSis.Master" Inherits="ReferenciasVentas.ReferenciaFinalVenta" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/RefSis.Master" %>

<%@ Register Src="~/Controles/PanelCargandoDatos.ascx" TagName="PanelCargandoDatos" TagPrefix="PanelCargandoDatos" %>
<%@ Register Src="~/Controles/Titulo.ascx" TagName="Titulo" TagPrefix="Titulo" %>
<%@ Register Src="~/Controles/SelectorProyectoPTKSIS.ascx" TagName="Proyecto" TagPrefix="Proyecto" %> 
<%@ Register Assembly="OptionGroupDropDownList" Namespace="WebControlsDropDown" TagPrefix="ogd" %>

<asp:Content ID="Contenido_HEAD" runat="server" ContentPlaceHolderID="head">
    <style>
        input[type=text]:disabled {
            background: #e5e5e5;
        }
    </style>

    <%--<script src="../../js/jquery-1.11.1.js"></script>--%>

    <script type="text/javascript">
        //$(function () {
        //    debugger;
        //    $("input[name*='chklPlantToCharge']").click(function () {
        //        if ($(this).val() == '47' && $(this).is(":checked")) {
        //            alert("Zamudio");
        //        }
        //    });
        //})

        function ValidatePlantToCharge(source, args) {
            if (ConditionsDevelopment()) {
                args.IsValid = true;
                return;
            }
            else {
                var chkListModules = document.getElementById('<%= chklPlantToCharge.ClientID%>');
                var chkListinputs = chkListModules.getElementsByTagName("input");
                for (var i = 0; i < chkListinputs.length; i++) {
                    if (chkListinputs[i].checked) {
                        args.IsValid = true;
                        return;
                    }
                }
                args.IsValid = false;
            }            
        };

        function ValidateModeNumber(source, args) {
            var chkModes = document.getElementById('<%= chklModeNumber.ClientID%>');
            var chkList = chkModes.getElementsByTagName("input");
            for (var i = 0; i < chkList.length; i++) {
                if (chkList[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        function ProyectoPtksisElegido(source, eventArgs) {                  
            if (source.get_element().id.indexOf("txtCPN") > -1) {                
                var hdnValueId = document.getElementById('<%=txtCPN.FindControl("hfCustProjectName").ClientID%>');
                var hdnValueIdProjectPtksis = document.getElementById('<%=txtCPN.FindControl("hfIdProjectPtksis").ClientID%>');
                var arrayValores = eventArgs.get_value().split("@");
                var programa = arrayValores[0];
                var idPtksis = arrayValores[1];
                hdnValueId.value = programa;
                hdnValueIdProjectPtksis.value = idPtksis;
                ActualizarNombreReferencia();
            }           
        };

        function ActualizarNombreReferencia() {
            var ddlProducto = document.getElementById('<%=ddlProduct.ClientID%>');
            var ddlDH = document.getElementById('<%=ddlType.ClientID%>');
            var ddlTM = document.getElementById('<%=ddlTransmissionMode.ClientID%>');
            var especificacion = document.getElementById('<%=txtSpecification.ClientID%>');
            var nombre = document.getElementById('<%=txtNombreReferencia.ClientID%>');
           
            nombre.value = ddlProducto.options[ddlProducto.selectedIndex].text
            if (ddlDH.options[ddlDH.selectedIndex].text != '-') {
                nombre.value = nombre.value + " " + ddlDH.options[ddlDH.selectedIndex].text
            }
            if (ddlTM.disabled == false && ddlTM.options[ddlTM.selectedIndex].text != '-') {
                nombre.value = nombre.value + " " + ddlTM.options[ddlTM.selectedIndex].text
            }
            var IdProyecto = document.getElementById('<%=txtCPN.FindControl("hfCustProjectName").ClientID%>');
            if (IdProyecto.value != '') {
               nombre.value = nombre.value + " " + IdProyecto.value;
            }
            else {
                nombre.value = nombre.value + " " + "'Project Key'";
            }            
            if (especificacion.value != '') {
                var longitud = nombre.value.length + especificacion.value.length;
                if (longitud > 52) {                    
                    var restarEspecificacion = longitud - 52;
                    especificacion.value = especificacion.value.substr(0, especificacion.value.length - restarEspecificacion);
                }
                nombre.value = nombre.value + " " + especificacion.value;
            }
            else {
                nombre.value = nombre.value + " " + "'Specification'";
            }
        };       

        function GetSelectedPlants() {
            var plantasSeleccionadas = "";
            $('#<%=chklPlantToCharge.ClientID%> input[type=checkbox]:checked').each(function () {
                plantasSeleccionadas += this.value + ",";
            });
            if (plantasSeleccionadas != '') {
                plantasSeleccionadas = plantasSeleccionadas.substr(0, plantasSeleccionadas.length - 1);
            }

            return plantasSeleccionadas;
        };

        function GetSelectedModeNumbers() {
            var modosSeleccionados = "";
            $('#<%=chklModeNumber.ClientID%> input[type=checkbox]:checked').each(function () {
                modosSeleccionados += this.value + ",";
            });
            if (modosSeleccionados != '') {
                modosSeleccionados = modosSeleccionados.substr(0, modosSeleccionados.length - 1);
            }
            return modosSeleccionados;
        };

        function CargarProyectoBrain(idProyecto) {            
            if (idProyecto != '') {
                $.getJSON('BuscarProyectoBrain.aspx?pro=' + idProyecto, function (json) {
                    if (json != null) {
                        if (json.Nombre != '') {                            
                            var hdnCPN = document.getElementById('<%=txtCPN.FindControl("hfCustProjectName").ClientID%>');
                            hdnCPN.value = json.Id;
                            var txtCPN = document.getElementById('<%=txtCPN.FindControl("txtCustProjectName").ClientID%>');
                            txtCPN.value = json.Nombre;
                            ActualizarNombreReferencia();
                        }
                    }
                });
            }
        }

        function ProyectoElegidoCliente(text, key, id) {                   
            document.getElementById('<%=txtCPN.FindControl("txtCustProjectName").ClientID%>').value = text;
            document.getElementById('<%=txtCPN.FindControl("hfCustProjectName").ClientID%>').value = key; 
            document.getElementById('<%=txtCPN.FindControl("hfIdProjectPtksis").ClientID%>').value = id; 
            ActualizarNombreReferencia();
            $find("mpe").hide();
        }
        function showPopup() {
            $find("mpe").show();
            return false;
        }
        function HidePopup() {
            $find("mpe").hide();
            return false;
        }

        function ConditionsPreviousBatzNumberValid() {
            var modeNumbers = GetSelectedModeNumbers();
            if ((modeNumbers.indexOf("1") > -1) && (modeNumbers.indexOf("2") < 0) && document.getElementById('<%=ddlDrawingType.ClientID%>').value == '1') {
                return true;
            }
            else {
                return false;
            }
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

        function SelectModeNumber() {
            if (ConditionsDevelopment()) {
                var modeNumbers = $('#<% = chklModeNumber.ClientID%> input:checkbox');
                modeNumbers[0].checked = false;
                modeNumbers[1].checked = false;
                DeshabilitarRFV_CustomerPartNumber();
                DeshabilitarRFV_DrawingNumber();
                DeshabilitarRFV_PrevBatzPartNumber();
                $("[id*=chklPlantToCharge] input:checkbox").prop('checked', false);                
                $('#' + '<%= txtDrawingNumber.ClientID%>').val($('#' + '<%= txtCustomerPN.ClientID%>').val());
                $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                $('#' + '<%= txtPlanoWeb.ClientID%>').val('');
                $('#' + '<%= txtNivelIngenieria.ClientID%>').val('');
                document.getElementById('<%=txtCPN.FindControl("txtCustProjectName").ClientID%>').value = 'DEVELOPMENT';
                document.getElementById('<%=txtCPN.FindControl("hfCustProjectName").ClientID%>').value = 'DVLPMT'; 
                ActualizarNombreReferencia();
            }
            else {                
                ValidatorEnable(document.getElementById("<%=rfvCustomerPN.ClientID%>"), true);
                ValidatorEnable(document.getElementById("<%=rfvDrawingNumber.ClientID%>"), true);
                ValidatorEnable(document.getElementById("<%=rfvPrevBatzPN.ClientID%>"), true);
                if (ConditionsPreviousBatzNumberValid()) {                
                    CheckPrevBatzPN();
                }            
                else {                              
                    DeshabilitarRFV_PrevBatzPartNumber()
                    DeshabilitarRFV_EvolutionChanges()
                    $('#' + '<%= txtEvolutionChanges.ClientID%>').val('');
                    CheckReferenceNumber();
                }           
            }                       
        }

        function DeshabilitarRFV_EvolutionChanges() {
          ValidatorEnable(document.getElementById("<%=rfvEvolutionChanges.ClientID%>"), false);
          var vceControl = $find("<%=vceEvolutionChanges.ClientID%>");
          if (vceControl && vceControl._invalid) {
              Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout = vceControl;
              Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout.hide();
          }
        }

        function DeshabilitarRFV_DrawingNumber() {
          ValidatorEnable(document.getElementById("<%=rfvDrawingNumber.ClientID%>"), false);
          var vceControl = $find("<%=vceDrawingNumber.ClientID%>");
          if (vceControl && vceControl._invalid) {
                Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout = vceControl;
              Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout.hide();
          }
        }

        function DeshabilitarRFV_PrevBatzPartNumber() {
            ValidatorEnable(document.getElementById("<%=rfvPrevBatzPN.ClientID%>"), false);
            var vceControl = $find("<%=vcePrevBatzPN.ClientID%>");
            if (vceControl && vceControl._invalid) {
                Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout = vceControl;
                Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout.hide();
            }
        }

        function DeshabilitarRFV_CustomerPartNumber() {
            ValidatorEnable(document.getElementById("<%=rfvCustomerPN.ClientID%>"), false);
            var vceControl = $find("<%=vceCustomerPN.ClientID%>");
            if (vceControl && vceControl._invalid) {
                Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout = vceControl;
                Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout.hide();
            }
        }

        function ComprobarDrawingType() {    
            if (document.getElementById('<%=ddlDrawingType.ClientID%>').value == '1') {
                //Seleccionado Number Remains/New Level
                var modeNumbers = $('#<% = chklModeNumber.ClientID%> input:checkbox');
                modeNumbers[0].checked = true;
                modeNumbers[1].checked = false;
                //modeNumbers[2].checked = false;
                ValidatorEnable(document.getElementById("<%=rfvPrevBatzPN.ClientID%>"), true);
                $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                DeshabilitarRFV_DrawingNumber()
                $("[id*=chklModeNumber]").find('input:checkbox[value=2]').attr('disabled', true);
                $("[id*=chklModeNumber]").find('input:checkbox[value=3]').attr('disabled', true);
                if (ConditionsPreviousBatzNumberValid()) {
                    CheckPrevBatzPN();
                }
            }
            else {
                //Seleccionado New Number 
                $("[id*=chklModeNumber]").find('input:checkbox[value=2]').attr('disabled', false);
                $("[id*=chklModeNumber]").find('input:checkbox[value=3]').attr('disabled', false);
                $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", false);
                $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(255,255,255)");
                ValidatorEnable(document.getElementById("<%=rfvDrawingNumber.ClientID%>"), true);
                DeshabilitarRFV_PrevBatzPartNumber();
                
            }
        }
        
        function CheckReferenceNumber() {
            document.getElementById('<%= hfValidacionReferenceNumber.ClientID%>').value = '';         
            if ($('#' + '<%= txtCustomerPN.ClientID%>').val().trim().length != 0) {
                var existe = false;
                var customers = document.getElementById('<%=hfCustomerNumbers.ClientID%>').value;
                var temp = new Array();
                var cust = $('#' + '<%= txtCustomerPN.ClientID%>').val();
                temp = customers.split(',');
                var num = temp.length;
                for (elem in temp) {
                    if (temp[elem] != '' && temp[elem] == cust) {
                        existe = true;
                    }
                }
                if (!existe) {
                    if (ConditionsDevelopment()) {
                        $('#' + '<%= txtDrawingNumber.ClientID%>').val($('#' + '<%= txtCustomerPN.ClientID%>').val());
                        $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                        $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                        DeshabilitarRFV_DrawingNumber();
                    }
                    else {
                        var plantas = GetSelectedPlants();
                        if (plantas.length != 0) {
                            $.getJSON('BuscarCustomerPN.aspx?ref=' + encodeURIComponent($('#' + '<%= txtCustomerPN.ClientID%>').val().trim()) + '&pl=' + plantas, function (json) {
                                if (json != null) {
                                    if (json.Pieza != '') {
                                        if (!ConditionsPreviousBatzNumberValid()) {
                                            $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                                            $('#' + '<%= txtPlanoWeb.ClientID%>').val('');
                                            $('#' + '<%= txtNivelIngenieria.ClientID%>').val('');
                                        }
                                        alert("Customer part number already exist for the selected plant(s)")
                                        //document.getElementById('<%= hfValidacionReferenceNumber.ClientID%>').value = "Customer part number already exists for the selected plant(s)";
                                        $('#' + '<%= txtCustomerPN.ClientID%>').focus();
                                    }
                                    else {
                                        if (!ConditionsPreviousBatzNumberValid()) {
                                            $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                                            $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                                            DeshabilitarRFV_DrawingNumber();
                                            $('#' + '<%= txtPlanoWeb.ClientID%>').val('');
                                            $('#' + '<%= txtNivelIngenieria.ClientID%>').val('');
                                            $('#' + '<%= txtNivelIngenieria.ClientID%>').attr("disabled", true);
                                            $('#' + '<%= txtNivelIngenieria.ClientID%>').css("background-color", "rgb(229,229,229)");
                                            var modeNumbers = GetSelectedModeNumbers();
                                            if (modeNumbers.indexOf("2") > -1) {
                                                //Drawing está clickado
                                                $('#' + '<%= txtDrawingNumber.ClientID%>').val($('#' + '<%= txtCustomerPN.ClientID%>').val());
                                                document.getElementById('<%= hfValidacionDrawingNumber.ClientID%>').value = '';
                                            }
                                            else {
                                                //Drawing no está clickado
                                                if (modeNumbers.indexOf("1") > -1) {
                                                    if (document.getElementById('<%=ddlDrawingType.ClientID%>').value == '1') {
                                                        //Seleccionado Old Drawing Type
                                                        $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                                                        $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                                                    }
                                                    else {
                                                        //Seleccionado New drawing type
                                                        $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                                                        $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", false);
                                                        $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(255,255,255)");
                                                        ValidatorEnable(document.getElementById("<%=rfvDrawingNumber.ClientID%>"), true);
                                                        $('#' + '<%= txtDrawingNumber.ClientID%>').focus();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            });
                        }
                        else {
                            if (!ConditionsPreviousBatzNumberValid()) {
                                $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                            }
                            alert("Choose at least one plant");
                        }
                    }                    
                }
                else {
                    alert("Customer part number already previously added in this request");
                    //document.getElementById('<%= hfValidacionReferenceNumber.ClientID%>').value = "Customer part number already previously added in this request";
                }
            }
            else {
                if (!ConditionsPreviousBatzNumberValid()) {
                    $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                    $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                    DeshabilitarRFV_DrawingNumber();
                    $('#' + '<%= txtPlanoWeb.ClientID%>').val('');
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').val('');
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').attr("disabled", true);
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').css("background-color", "rgb(229,229,229)");
                }
                if (ConditionsDevelopment()) {
                    $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                }
            }
        }

        function CheckDrawingNumber() {
            document.getElementById('<%= hfValidacionDrawingNumber.ClientID%>').value = "";
            var prueba = $('#' + '<%= txtDrawingNumber.ClientID%>').val();
            var plantas = GetSelectedPlants();
            if (plantas.length != 0) {                
                if ($('#' + '<%= txtDrawingNumber.ClientID%>').val().trim().length > 0) {
                    var existe = false;
                    var drawings = document.getElementById('<%=hfDrawingNumbers.ClientID%>').value;
                    var temp = new Array();                    
                    var draw = $('#' + '<%= txtDrawingNumber.ClientID%>').val();
                    temp = drawings.split(',');
                    var num = temp.length;
                    for (elem in temp) {
                        if (temp[elem] != '' && temp[elem] == draw) {
                            existe = true;
                        }
                    }
                    if (!existe) {
                        //$.getJSON('BuscarPrevBatzPN.aspx?ref=' + encodeURIComponent($('#' + '<%= txtDrawingNumber.ClientID%>').val().trim()) + '&pl=' + plantas, function (json) {
                        $.getJSON('BuscarPrevBatzPN.aspx?ref=' + encodeURIComponent($('#' + '<%= txtDrawingNumber.ClientID%>').val().trim()), function (json) {
                            if (json != null) {
                                if (json.RefPieza == '') {                                
                                    alert("The entered Drawing Number does not exist");
                                    document.getElementById('<%= hfValidacionDrawingNumber.ClientID%>').value = "The entered Drawing Number does not exist";
                                    $('#' + '<%= txtDrawingNumber.ClientID%>').focus();                                    
                                }
                            }
                        });
                    }
                }
                else {
                    //Si no hay nada en el drawing number
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').val('');
                    $('#' + '<%= txtPlanoWeb.ClientID%>').val('');
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').attr("disabled", true);
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').css("background-color", "rgb(229,229,229)");
                }
            }
            else {
                alert("Choose at least one plant");
            }
        }

        function CheckPrevBatzPN() {            
            document.getElementById('<%= hfValidacionPrevBatzPN.ClientID%>').value = '';
            DeshabilitarRFV_EvolutionChanges();
            ValidatorEnable(document.getElementById("<%=rfvPrevBatzPN.ClientID%>"), true);            
            var plantas = GetSelectedPlants();
            if (plantas.length != 0) {                
                if (($('#' + '<%= txtPrevBatzPN.ClientID%>').val().trim().length > 0)) {
                    $.getJSON('BuscarPrevBatzPN.aspx?ref=' + encodeURIComponent($('#' + '<%= txtPrevBatzPN.ClientID%>').val().trim()) + '&pl=' + plantas, function (json) {
                        if (json != null) {
                            var modeNumbers = GetSelectedModeNumbers();
                            if (json.RefPieza != '') {
                                //Existe El Prev Batz Part Number
                                if ((modeNumbers.indexOf("1") > -1) && (modeNumbers.indexOf("2") < 0)) {
                                    ValidatorEnable(document.getElementById("<%=rfvEvolutionChanges.ClientID%>"), true);
                                    $('#' + '<%= txtEvolutionChanges.ClientID%>').focus();
                                    DeshabilitarRFV_PrevBatzPartNumber();
                                    $('#' + '<%= txtPrevBatzPN.ClientID%>').val(json.RefPieza);
                                    CargarProyectoBrain(json.IdCustomerProject);
                                    document.getElementById('<%=txtCPN.FindControl("hfIdProjectPtksis").ClientID%>').value = json.IdCustomerProject;
                                    if (document.getElementById('<%=ddlDrawingType.ClientID%>').value == '1') {
                                        $('#' + '<%= txtDrawingNumber.ClientID%>').val(json.RefClientePlanoBatz);
                                        $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                                        $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                                        document.getElementById('<%= hfValidacionDrawingNumber.ClientID%>').value = '';
                                    }
                                    else {
                                        $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                                        $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", false);        
                                        $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(255,255,255)");
                                    }
                                    $('#' + '<%= txtPlanoWeb.ClientID%>').val(json.PlanoWeb);
                                    $('#' + '<%= txtNivelIngenieria.ClientID%>').val(json.NivelIngenieria);
                                    $('#' + '<%= txtNivelIngenieria.ClientID%>').attr("disabled", false);   
                                    $('#' + '<%= txtNivelIngenieria.ClientID%>').css("background-color", "rgb(255,255,255)");
                                }
                            }
                            else {
                                //No existe el Prev Batz Part Number
                                var modeNumbers = GetSelectedModeNumbers();
                                if ((modeNumbers.indexOf("1") > -1) && (modeNumbers.indexOf("2") < 0)) {
                                    $('#' + '<%= txtPlanoWeb.ClientID%>').val('');
                                    $('#' + '<%= txtEvolutionChanges.ClientID%>').val('');
                                    $('#' + '<%= txtNivelIngenieria.ClientID%>').val('');
                                    $('#' + '<%= txtNivelIngenieria.ClientID%>').attr("disabled", true);
                                    $('#' + '<%= txtNivelIngenieria.ClientID%>').css("background-color", "rgb(229,229,229)");
                                    $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                                    $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true);
                                    $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");                                                                        
                                }
                                document.getElementById('<%=txtCPN.FindControl("hfCustProjectName").ClientID%>').value = '';
                                document.getElementById('<%=txtCPN.FindControl("txtCustProjectName").ClientID%>').value = '';
                                ActualizarNombreReferencia();
                                document.getElementById('<%= hfValidacionPrevBatzPN.ClientID%>').value = "Previous Batz Part Number is not valid";
                                alert("Previous Batz Part Number is not valid");
                                $('#' + '<%= txtPrevBatzPN.ClientID%>').focus();
                            }
                        }
                    });
                }
                else {
                    //Vacíamos los campos
                    var modeNumbers = GetSelectedModeNumbers();
                    if ((modeNumbers.indexOf("1") > -1) && (modeNumbers.indexOf("2") < 0)) {
                        //Drawing number es válido dependiendo en algunos casos, mirarlo el lunes!
                        $('#' + '<%= txtDrawingNumber.ClientID%>').val('');
                        $('#' + '<%= txtDrawingNumber.ClientID%>').attr("disabled", true); 
                        $('#' + '<%= txtDrawingNumber.ClientID%>').css("background-color", "rgb(229,229,229)");
                    }
                    if (document.getElementById('<%=ddlDrawingType.ClientID%>').value == '2') {
                        DeshabilitarRFV_PrevBatzPartNumber();
                    }
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').val('');
                    $('#' + '<%= txtPlanoWeb.ClientID%>').val('');
                    $('#' + '<%= txtEvolutionChanges.ClientID%>').val('');
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').attr("disabled", true);
                    $('#' + '<%= txtNivelIngenieria.ClientID%>').css("background-color", "rgb(229,229,229)");
                    ActualizarNombreReferencia();
                }
            }
            else {
                $('#' + '<%= txtPrevBatzPN.ClientID%>').val('');
                $('#' + '<%= txtPrevBatzPN.ClientID%>').focus();
                alert("Choose at least one plant");

            }
        }

        function BindTypes() {
            var tipos = $.getJSON('BuscarTiposProducto.aspx?idProducto=' + encodeURIComponent($('#' + '<%= ddlProduct.ClientID%>').val()), function (json) {
                if (json != null) {
                    $('#' + '<%= ddlType.ClientID%>').empty();
                    $.each(json, function (key, val) {
                        $('#' + '<%= ddlType.ClientID%>').append($("<option />").attr("value", val.Id).text(val.Nombre));
                    });
                    $('#' + '<%= ddlType.ClientID%>')[0].selectedIndex = 0;
                    $('#<%=ddlType.ClientID%> option').css({ 'width': '100' });
                }
            })

            tipos.done(function () {
                BindTM();
            });
        }

        function BindTM() {
            var TM = $.getJSON('BuscarProducto.aspx?idProducto=' + encodeURIComponent($('#' + '<%= ddlProduct.ClientID%>').val()), function (json) {
                if (json != null) {
                    if (json.TransmissionModeVisible) {
                        var TM2 =  $.getJSON('BuscarTransmissionMode.aspx', function (json) {
                            $('#' + '<%= ddlTransmissionMode.ClientID%>').attr("disabled", false);
                             $('#' + '<%= ddlTransmissionMode.ClientID%>').empty();
                             $.each(json, function (key, val) {
                                $('#' + '<%= ddlTransmissionMode.ClientID%>').append($("<option />").attr("value", val.Id).text(val.Nombre));
                            });
                            $('#' + '<%= ddlTransmissionMode.ClientID%>').css("background-color", "rgb(255,255,255)");
                            $('#' + '<%= ddlTransmissionMode.ClientID%>')[0].selectedIndex = 0;
                        })

                        TM2.done(function () {
                            ActualizarNombreReferencia();
                        });
                    }
                    else {
                        $('#' + '<%= ddlTransmissionMode.ClientID%>').attr("disabled", true);
                        $('#' + '<%= ddlTransmissionMode.ClientID%>').css("background-color", "rgb(229,229,229)");
                        $('#' + '<%= ddlTransmissionMode.ClientID%>').val("-");
                    }
                }
            })

            TM.done(function () {
                ActualizarNombreReferencia();
            });
        }

        function ValidarAltaReferencia() {              
            if (document.getElementById('<%=txtCPN.FindControl("hfCustProjectName").ClientID%>').value.length == 0) {
                alert("The project does not exist. Please, choose a valid project");
                return false;
            }
            <%--if (document.getElementById('<%= hfValidacionReferenceNumber.ClientID%>').value.length > 0) {
                alert(document.getElementById('<%= hfValidacionReferenceNumber.ClientID%>').value);
                $('#' + '<%= txtCustomerPN.ClientID%>').focus();
                return false;
            }--%>
            if (document.getElementById('<%= hfValidacionDrawingNumber.ClientID%>').value.length > 0) {
                alert(document.getElementById('<%= hfValidacionDrawingNumber.ClientID%>').value);
                $('#' + '<%= txtDrawingNumber.ClientID%>').focus();
                return false;
            }
            if (document.getElementById('<%= hfValidacionPrevBatzPN.ClientID%>').value.length > 0) {
                alert(document.getElementById('<%= hfValidacionPrevBatzPN.ClientID%>').value);
                $('#' + '<%= txtPrevBatzPN.ClientID%>').focus();
                return false;
            }
            return true;
        }
    </script>

</asp:Content>

<asp:Content ID="Contenido_PRINCIPAL" ContentPlaceHolderID="cuerpoPrincipal" runat="server">    
    <script type="text/javascript">
        Sys.Application.add_load(init);
        function init() {
            $('textarea').keypress(function (e) {
                var maxlength = $(this).attr('longMax');
                if ($(this).val().length >= maxlength) {
                    e.preventDefault();                    
                }
            });

            $('textarea').bind('paste', function () {
                var self = this
                setTimeout(function () {
                    var maxlength = $(self).attr('longMax');
                    if ($(self).val().length >= maxlength) {
                        $(self).val($(self).val().substr(0, maxlength));
                    }
                }, 0);
            });

            $('.keyup').keyup(function (e) {
                var nombre = document.getElementById('<%=txtNombreReferencia.ClientID%>');
                var especificacion = document.getElementById('<%=txtSpecification.ClientID%>');
                if (e.keyCode == 8) {
                    ActualizarNombreReferencia();
                }
                else {
                    if (nombre.value.length >= 52) {
                        especificacion.value = especificacion.value.substr(0, especificacion.value.length - 1);
                        alert("You have reached the maximum length of characters");
                    }
                    else {
                        ActualizarNombreReferencia();
                    }
                }                
            });
        
            $('.CambioCombo').change(function () {
                ActualizarNombreReferencia();
            });

            $('.CambioDrawingType').change(function () {
                ComprobarDrawingType();
            });            

            $(".divConRegistros").click(function () {
                $(".solicitudes").slideToggle("slow");
            });
            
            $('#' + '<%= txtCustomerPN.ClientID%>').change(function () {
                CheckReferenceNumber();
            });

            $('#' + '<%= txtCustomerPN.ClientID%>').keyup(function (e) {
                if (e.keyCode === 13) {
                    CheckReferenceNumber();
                }
            });

            $('#' + '<%= txtDrawingNumber.ClientID%>').change(function () {
                CheckDrawingNumber();
            });

            $('#' + '<%= txtDrawingNumber.ClientID%>').keyup(function (e) {
                if (e.keyCode === 13) {
                    CheckDrawingNumber();
                }
            });

            $('#' + '<%= txtPrevBatzPN.ClientID%>').change(function () {                
                if (!ConditionsDevelopment()) {
                    CheckPrevBatzPN();
                }                
            });

            $('#' + '<%= txtPrevBatzPN.ClientID%>').keyup(function (e) {
                if (e.keyCode === 13) {
                    if (!ConditionsDevelopment()) {
                        CheckPrevBatzPN();
                    }
                }
            });

            $('#' + '<%= ddlProduct.ClientID%>').change(function () {
                BindTypes();
            });
        };
    </script>

    <asp:UpdatePanel ID="upGlobal" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlNuevaSolicitud" runat="server" CssClass="nuevaSolicitud">
                <table style="width: 100%; border:1px solid #66CD00; margin-bottom: 10px" class="tablaNuevaSolicitud" cellspacing="0" cellpadding="0">
                    <tr style="background-color: #AAF6C3">
                        <td style="width:10%">
                            &nbsp
                        </td>
                        <td style="width:80%">
                            <table style="background-color: #AAF6C3; border: 1px solid #AAF6C3; width:100%; text-align:center" ><%-- height:40px  --%>
                                <tr>
                                    <td style="padding:5px">
                                        <asp:Label ID="lblNuevaSolicitud" runat="server" Text="New Selling Part Number" ForeColor="Black" Font-Bold="true" Font-Size="14px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:10%; text-align: right; padding-right:5px">
                            <asp:Button ID="btnOcultarNuevaSolicitud" runat="server" Text="Hide" BackColor="White" CssClass="btnOcultarNuevaSolicitud" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; border:1px solid black" cellspacing="0">                                 
                    <tr style="background-color: #91A0A7; text-align:center; border-top:1px dotted #91A0A7"> 
                        <td colspan="2">
                            <asp:Label ID="lblPlantToCharge" Text="Plants to charge" runat="server" ForeColor="white" Font-Size="14px" />
                        </td>               
                        <td colspan="6" style="text-align:center">
                            <asp:Label ID="lblTitCustomerPartNumber" Text="Part Number" runat="server" ForeColor="white" Font-Size="14px" />
                        </td>                  
                    </tr>                    
                    <tr style="border-bottom:1px solid black">
                        <td rowspan="2" colspan="2" style="vertical-align:middle">
                            <div style="float:left; width:1%">
                                <asp:TextBox ID="txtPlantasOculto" runat="server" BorderStyle="None" ReadOnly="true" Width="100%" Text="-" Font-Size="1px" ValidationGroup="CamposVacios" />
                            </div>
                            <div style="float:left; width:99%"><br />
                                <asp:CheckBoxList ID="chklPlantToCharge" runat="server" DataTextField="Nombre" DataValueField="Codigo" RepeatColumns="3" RepeatDirection="Horizontal" onclick="SelectModeNumber();" />
                                <asp:CustomValidator  ID="cvPlantToCharge" Display="None" ControlToValidate="txtPlantasOculto"
                                 ClientValidationFunction="ValidatePlantToCharge" runat="server" ForeColor="red"
                                 ValidationGroup="CamposVacios" ErrorMessage="Choose at least one plant" />  
                                <act:ValidatorCalloutExtender ID="vcePlantTocharge" runat="server" TargetControlID="cvPlantToCharge" PopupPosition="TopLeft" />
                            </div>
                            <div class="clear-float" />                            
                        </td>       
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblDrawingType" Text="Drawing" runat="server" /> 
                        </td>
                        <td style="width:20%; text-align:left">
                            <asp:DropDownList ID="ddlDrawingType" runat="server" Width="99%" CssClass="CambioDrawingType">
                                <asp:ListItem Text="Num. Remains/New Level" Value="1" />
                                <asp:ListItem Text="New Number" Value="2" />                                
                            </asp:DropDownList>
                        </td>
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblPartNumber" Text="Customer No." runat="server" />
                        </td>      
                        <td style="width:15%;">
                            <asp:TextBox ID="txtCustomerPN" runat="server" Font-Size="16px" MaxLength="19" Width="95%" style="text-transform:uppercase" />
                            <asp:RequiredFieldValidator ID="rfvCustomerPN" runat="server" Text="*" ErrorMessage="Insert a Part Number" ControlToValidate="txtCustomerPN" ValidationGroup="CamposVacios" Display="None" />                       
                            <act:ValidatorCalloutExtender ID="vceCustomerPN" runat="server" TargetControlID="rfvCustomerPN" PopupPosition="Right" />        
                            <asp:HiddenField ID="hfCustomerNumbers" runat="server" />                    
                        </td>
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblDrawingNumber" Text="Previous Draw-Part No." runat="server" />
                        </td>      
                        <td style="width:15%;">
                            <asp:TextBox ID="txtDrawingNumber" runat="server" Font-Size="16px" MaxLength="19" Width="95%" style="text-transform:uppercase" />
                            <asp:RequiredFieldValidator ID="rfvDrawingNumber" runat="server" Text="*" ErrorMessage="Insert a Part Number" ControlToValidate="txtDrawingNumber" ValidationGroup="CamposVacios" Display="None" />                       
                            <act:ValidatorCalloutExtender ID="vceDrawingNumber" runat="server" TargetControlID="rfvDrawingNumber" PopupPosition="TopLeft" />
                            <asp:HiddenField ID="hfDrawingNumbers" runat="server" />
                        </td>                                              
                    </tr>
                    <tr style="border-bottom:1px solid black">
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblMode" Text="No. Type" runat="server" /> 
                        </td>
                        <td style="width:20%; text-align:left,">
                            <div style="float:left; width:1%">
                                <asp:TextBox ID="txtModeNumber" runat="server" BorderStyle="None" ReadOnly="true" Width="100%" Text="-" Font-Size="1px" ValidationGroup="CamposVacios" />
                            </div>
                            <div style="float:left; width:99%">
                                <asp:CheckBoxList ID="chklModeNumber" runat="server" RepeatDirection="Horizontal" Width="95%" onclick="SelectModeNumber();">
                                    <asp:ListItem Text="Customer" Value="1" />
                                    <asp:ListItem Text="Drawing" Value="2" />
                                    <asp:ListItem Text="Development or Batz Zamudio" Value="3" />
                                </asp:CheckBoxList>
                                <asp:CustomValidator ID="cvModeNumber" Display="None" ControlToValidate="txtModeNumber"
                                 ClientValidationFunction="ValidateModeNumber" runat="server" ForeColor="red" SetFocusOnError="false"
                                 ValidationGroup="CamposVacios" ErrorMessage="Choose a No. type" />
                                <act:ValidatorCalloutExtender ID="vceModeNumber" runat="server" TargetControlID="cvModeNumber" PopupPosition="TopLeft" />
                            </div>
                            <div class="clear-float" />
                        </td>                        
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblPlanoweb" Text="Drawing  No." runat="server" />
                        </td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtPlanoWeb" runat="server" Width="95%" Enabled="false" Font-Size="16px" BackColor="#e5e5e5" />
                        </td>
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblNivelIngenieria" Text="Engineering Level" runat="server" />
                        </td>
                        <td colspan="2">
                            <asp:Textbox ID="txtNivelIngenieria" runat="server" Width="95%" Enabled="false" Font-Size="16px" BackColor="#e5e5e5" MaxLength="19" />
                        </td>                          
                    </tr>
                    <tr id="filaTipoEvolucion" runat="server">
                        <td colspan="2">&nbsp</td>
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                           <asp:Label ID="lblPreviousBatzPartNumber" Text="Previous Batz Part Number" runat="server" />                                                           
                        </td>
                        <td style="width:20%; text-align:left">
                            <asp:TextBox ID="txtPrevBatzPN" runat="server" Width="99%" style="text-transform:uppercase" MaxLength="13" />
                            <asp:RequiredFieldValidator ID="rfvPrevBatzPN" runat="server" ErrorMessage="Insert a part number" ControlToValidate="txtPrevBatzPN" ValidationGroup="CamposVacios" Display="None" />
                            <act:ValidatorCalloutExtender ID="vcePrevBatzPN" runat="server" TargetControlID="rfvPrevBatzPN" PopupPosition="BottomRight" />                            
                        </td>
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblEvolutionChanges" Text="Evolution Changes" runat="server" />
                        </td>
                        <td colspan="3">
                            <asp:Textbox ID="txtEvolutionChanges" runat="server" Width="99%" MaxLength="120" longMax="120" />
                            <asp:RequiredFieldValidator ID="rfvEvolutionChanges" runat="server" Text="*" ErrorMessage="Required field" ControlToValidate="txtEvolutionChanges" ValidationGroup="CamposVacios" Display="None" Enabled="false" />                       
                            <act:ValidatorCalloutExtender ID="vceEvolutionChanges" runat="server" TargetControlID="rfvEvolutionChanges" PopupPosition="TopLeft" />
                        </td>
                    </tr>
                    <tr style="background-color: #91A0A7; border-top:1px dotted #5D7B9D; text-align:center">
                        <td colspan="8">
                            <asp:Label ID="lblTitPartName" Text="Part Name" runat="server" ForeColor="white" Font-Size="14px" />
                        </td>
                    </tr>
                    <tr>      
                        <td style="background-color:#EBEFF0; width: 5%; text-align:center" rowspan="3">
                            <asp:Label ID="lblComentario" Text="Comment" runat="server" />
                        </td>
                        <td  rowspan="3"> 
                            <asp:textbox ID="txtComentario" runat="server" TextMode="MultiLine" Width="98%" Height="100%" Rows="6" MaxLength="200" longMax="200" />
                            <asp:RequiredFieldValidator ID="rfvComentario" runat="server" Text="*" ErrorMessage="Insert a comment" ControlToValidate="txtComentario" ValidationGroup="CamposVacios" Display="None" />                       
                            <act:ValidatorCalloutExtender ID="vceComentario" runat="server" TargetControlID="rfvComentario" PopupPosition="TopRight" />
                        </td>                   
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblProduct" Text="Product" runat="server" />
                        </td>
                        <td style="width:20%">
                            <asp:DropDownList ID="ddlProduct" runat="server" DataTextField="Nombre" DataValueField="Id" Width="100%" />
                        </td>                                
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblCustProjectName" Text="Customer´s Project Name" runat="server" />
                        </td>
                        <td colspan="5"> 
                            <Proyecto:Proyecto ID="txtCPN" runat="server" />
                        </td>              
                    </tr>
                    <tr>                
                        <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblType" Text="Type" runat="server" />
                        </td>
                        <td style="width:20%">
                            <asp:DropDownList ID="ddlType" runat="server" DataTextField="Nombre" DataValueField="Id" Width="100%" CssClass="CambioCombo" />
                        </td>
                        <td style="background-color:#EBEFF0; width:10%; text-align:center">
                            <asp:Label ID="lblSpecification" runat="server" Text="Specification" />
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtSpecification" runat="server" Width="99%" MaxLength="52" CssClass="keyup" style="text-transform:uppercase" />
                            <asp:RequiredFieldValidator ID="rfvSpecification" runat="server" Text="*" ErrorMessage="Insert a specification" ControlToValidate="txtSpecification" ValidationGroup="CamposVacios" Display="None" />                       
                            <act:ValidatorCalloutExtender ID="vceSpecification" runat="server" TargetControlID="rfvSpecification" PopupPosition="TopLeft" />
                        </td>
                    </tr>
                    <tr>
                         <td style="background-color:#EBEFF0; width: 10%; text-align:center">
                            <asp:Label ID="lblTransmissionMode" Text="Transmission Mode" runat="server" />
                        </td>
                        <td style="width:20%">
                            <asp:DropDownList ID="ddlTransmissionMode" runat="server" DataTextField="Nombre" DataValueField="Id" Width="100%" CssClass="CambioCombo" BackColor="#e5e5e5" />
                        </td>
                         <td style="background-color:#EBEFF0; width:10%; text-align:center">
                            <asp:Label ID="lblNombreReferencia" runat="server" Text="Part name" />
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtNombreReferencia" runat="server" Width="99%" MaxLength="52" Enabled="false" style="text-transform:uppercase" BackColor="#e5e5e5" />
                        </td>
                     </tr>                    
                    <tr>
                        <td style="width:5%; padding-top:20px; padding-bottom: 10px;">
                            &nbsp
                        </td>
                        <td style="width:15%; padding-top:20px; padding-bottom: 10px;">
                            &nbsp
                        </td>
                        <td colspan="4" style="text-align:center; padding-top:20px; padding-bottom: 10px; padding-left:5%">
                           <asp:Button ID="btnGuardarNuevaSolicitud" runat="server" Text="Save Part Number" BackColor="#ebf3de" Font-size="14px" BorderColor="black" UseSubmitBehavior="true" OnClientClick="if(Page_ClientValidate('CamposVacios')) return ValidarAltaReferencia();" ValidationGroup="CamposVacios" OnClick="btnGuardarNuevaReferencia_Click" CssClass="btnGuardarNuevaSolicitud" /><%--  --%>
                           <asp:HiddenField ID="hfValidacionReferenceNumber" runat="server" />
                           <asp:HiddenField ID="hfValidacionDrawingNumber" runat="server" />
                           <asp:HiddenField ID="hfValidacionPrevBatzPN" runat="server" />
                        </td>
                        <td style="width:10%; padding-top:20px; padding-bottom: 10px;">
                            &nbsp
                        </td>
                        <td style="width:15%; text-align:right; padding-top:20px; padding-bottom: 10px; padding-right: 5px;">
                            <asp:Button ID="btnLimpiarCampos" runat="server" CausesValidation="false" Text="Clean data" OnClick="btnLimpiarCampos_Click" />
                        </td>
                    </tr>
                </table>
                <br /><br />
                <hr/>               
            </asp:Panel>
   
            <table style=" width:100%; border:0px solid #5D7B9D; margin-top:20px" class="tablaSolicitudes" cellspacing="0" cellpadding="0">
                <tr style="background-color:#5D7B9D; color:White">
                    <td style="width:20%;padding:5px">
                        &nbsp
                    </td>
                    <td style="text-align:center; width:60%;padding:5px">
                        <b><asp:Label id="lblReferenciasFinales" runat="server" Text="Selling Part Numbers" Font-Size="14px" /></b>
                    </td>
                    <td style="text-align:right; width:20%;padding:5px">
                        <asp:Button ID="btnMostrarNuevaSolicitud" runat="server" Text="Add new part number" CssClass="btnMostrarNuevaSolicitud" BackColor="White" />                
                    </td>
                </tr>
            </table>
     
            <asp:Panel ID="pnlSolicitudes" runat="server" > 
                <asp:DataList ID="dlSolicitudes" runat="server" Width="100%" OnItemDataBound="dlSolicitudes_ItemDataBound" DataKeyField="Id"
                    OnCancelCommand="dlSolicitudes_CancelCommand" OnDeleteCommand="dlSolicitudes_DeleteCommand" CssClass="solicitudes">
                    <HeaderTemplate>
                        <tr style="background-color: #91A0A7; border-top:1px dotted #5D7B9D;">
                            <td style="background-color: #F9E1D2; width:5%; text-align:center">
                                <asp:Label ID="lblTitNumReferencia" runat="server" Text="Part" ForeColor="Black" Font-Size="16px" Font-Underline="true" />
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblTitTypeReferencia" runat="server" Text="Type Name"  ForeColor="white"/>
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblTitCustomerPartNumber" Text="Part No." runat="server" ForeColor="white"  />
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblTitDrawingNumber" runat="server" Text="Drawing No." ForeColor="white"/>
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="lblTitNumberType" runat="server" Text="No. Type" ForeColor="white"/>
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblTitPlanoWeb" runat="server" Text="Drawing No." ForeColor="white"/>
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblTitNivelIngenieria" runat="server" Text="Engineering Level" ForeColor="white"/>
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblTitPreviousBatzNumber" runat="server" Text="Previous Batz Part Number" ForeColor="white" />
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="lblTitEvolutionChanges" runat="server" Text="Evolution Changes" ForeColor="white" />
                            </td>
                            <td style="width:15%; text-align:center">
                                <asp:Label ID="lblTitReferenceName" Text="Part Name" runat="server" ForeColor="white" />
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="lblPlantToCharge" Text="Plants to charge" runat="server" ForeColor="white" />
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="lblComentario" runat="server" Text="Comment" ForeColor="white" />
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label id="lblAccionesItem" Text="Actions" runat="server" Font-Bold="true" />
                            </td>                        
                        </tr>
                    </HeaderTemplate>                       
                    <ItemTemplate>
                        <tr style='<%# If((Container.ItemIndex) Mod 2 = 0, "background-color:#F7F6F3; color:#333333;", "background-color:#FFFFFF; color:#284775;")%>'>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblNumReferencia" runat="server" Text='<%# (Convert.ToInt32(Container.ItemIndex) + 1)%>' ForeColor="Black" Font-Size="16px" />
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblTypeReferencia" runat="server" Text='<%# Eval("TipoReferenciaNombre")%>' />                                
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="txtCustomerPN" runat="server" Font-Size="14px" Text='<%# Eval("CustomerPartNumber")%>' style="text-transform:uppercase" />
                            </td>                            
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblDrawingNumber" runat="server" Text='<%# Eval("DrawingNumber")%>' style="text-transform:uppercase" />
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="lblNumberType" runat="server" Text='<%# Eval("TipoNumeroNombre")%>' />
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblPlanoWeb" runat="server" Text='<%# Eval("PlanoWeb")%>' />
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblNivelIngeniera" runat="server" Text='<%# Eval("NivelIngenieria")%>' />
                            </td>
                            <td style="width:5%; text-align:center">
                                <asp:Label ID="lblPreviousBatzNumber" runat="server" Text='<%# Eval("PreviousBatzPartNumber")%>' style="text-transform:uppercase"  />
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="lblEvolutionChanges" runat="server" Text='<%# Eval("EvolutionChanges")%>'  />
                            </td>
                            <td style="width:15%; text-align:center">
                                <asp:Label ID="lblReferenceName" Text='<%# Eval("FinalNameBrain")%>' runat="server" />
                            </td>                           
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="txtPlantToCharge" runat="server" />
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:Label ID="lblComentario" runat="server" Text='<%# Eval("Comentario")%>' />
                            </td>
                            <td style="width:10%; text-align:center">
                                <asp:ImageButton ID="imgbEliminar"  runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar24.png" AlternateText="Delete" ToolTip="Delete selling part" CommandName="delete" />
                            </td>
                        </tr>                                                  
                    </ItemTemplate>                                
                </asp:DataList>                                                                        
            </asp:Panel>          
              
            <p id="divSinRegistros" runat="server" style="height:20px; vertical-align:middle; text-align:center; background-color:gray">
                <asp:Label ID="lblNoRecord" runat="server" Text="There are no selling part numbers" ForeColor="White"></asp:Label> 
            </p>
            <table style="width:100%; margin-top:20px; text-align:center">
                <tr>
                    <td>
                        <asp:Button ID="btnFinalizar" runat="server" Text="Send Request" Font-Size="16px" BackColor="Gray" ForeColor="white" />
                    </td>
                </tr>
            </table>
            
            <asp:Button ID="btnConfirmacionBorrado" runat="server" style="display:none" />
            <act:ModalPopupExtender ID="mpeConfirmacionBorrado" runat="server" PopupControlID="pnlConfirmacionBorrado" TargetControlID="btnConfirmacionBorrado"
                CancelControlID="btnCancelar" BackgroundCssClass="modalBackground">
            </act:ModalPopupExtender>

            <asp:Panel ID="pnlConfirmacionBorrado" runat="server" CssClass="modalPopup" Style="display: none" Width="50%">
                <asp:HiddenField ID="hfIdRef" runat="server" />
                <asp:HiddenField ID="hfDrawing" runat="server" />
                <div class="header">
                    <asp:Label ID="lblEliminacion" runat="server" Text="Delete" />
                </div>
                <div class="body">
                    <asp:Label ID="lblMensajeEliminacion" runat="server" Text="Do you want to delete the part number?" />                    
                </div>
                <div class="footer" align="center">
                    <asp:Button ID="btnEliminarReferencia" runat="server" CssClass="si" Text="Yes" OnClick="btnEliminarReferencia_Click" />
                    <asp:Button ID="btnCancelar" runat="server" CssClass="no" Text="No" />
                </div>
            </asp:Panel>

            <asp:Button ID="btnOcultoFinalizar" runat="server" Style="display: none" />
            <act:ModalPopupExtender ID="mpeFinalizar" runat="server" PopupControlID="pnlFinalizarSolicitud" TargetControlID="btnOcultoFinalizar"
                CancelControlID="btnCancelarSolicitud" BackgroundCssClass="modalBackground">
            </act:ModalPopupExtender>
            <asp:Panel ID="pnlFinalizarSolicitud" runat="server" CssClass="modalPopup" Style="display: none" Width="50%">
                <div class="header">
                    <asp:Label ID="lblApprovement" runat="server" Text="Approvement" />
                </div>
                <div class="body">
                    <div>
                        <asp:Label ID="lblAprobacionPL" runat="server" Text="Before Documentation Technician processes the request, a project leader must approve your request. Please, select a project leader from the list below" />                    
                    </div>
                    <div id="divValidadores" runat="server" style="margin-top:20px; margin-bottom:20px">
                        <%--<asp:DropDownList ID="ddlValidadores" runat="server" DataTextField="Nombre" DataValueField="Id" />--%>
                        <ogd:OptionGroupDropDownList ID="ddlValidadores" runat="server" />
                    </div>
                </div>
                <div class="footer" align="center">
                    <asp:Button ID="btnFinalizarSolicitud" runat="server" CssClass="si" Text="Save request" OnClick="btnFinalizarSolicitud_Click" style="text-transform:uppercase" />&nbsp;&nbsp                    
                </div>
                <div class="footer">
                    <asp:Button ID="btnCancelarSolicitud" runat="server" CssClass="no" Text="Cancel" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>          
                
    <PanelCargandoDatos:PanelCargandoDatos ID="panelCargandoDatos1" runat="server" />
</asp:Content>
