<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DatosGenerales.aspx.vb" Inherits="GTK_Troqueleria._DatosGenerales" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="~/App_Themes/Batz/bootstrapmodal.css" />

    <script type="text/javascript">        
        var count = 0;
        var RutaAplicacion = "<%= If(Request.ApplicationPath = "/", String.Empty, Request.ApplicationPath)%>";
        /************************************************************************/

        function pageLoad() {
            var treeViewData = window["<%=tvAreas.ClientID%>" + "_Data"];

            $(function () {
                //alert("ready!");
                Borrar_Procedencia = function () {
                    $("#lvProducto_UL").empty();
                    var hf_IdProducto = $('#<%=hf_IdProducto.ClientID%>');
                    hf_IdProducto.val(null);
                    hf_IdProducto.next().remove();
                }
                Borrar_Caracteristica = function () {
                    $("#lvCaracteristica_UL").empty();
                    var hf_IdCaracteristica = $('#<%=hf_IdCaracteristica.ClientID%>');
                    hf_IdCaracteristica.val(null);
                    hf_IdCaracteristica.next().remove();
                }

                Seleccionar_Capacidad = function (Id_Origen) {
                    Id_Origen = Id_Origen.text().replaceAll('N/A', 'kaixo');
                    console.log(Id_Origen + "->" + $.isNumeric(Id_Origen));

                    if ($.isNumeric(Id_Origen)) {
                    }
                }

                Seleccionar_Procedencia = function (Id_Origen) {
                    var NodeValue;
                    if (Id_Origen) {
                        NodeValue = Id_Origen;
                    } else {
                        if (treeViewData.selectedNodeID.value) {
                            var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);
                            NodeValue = GetNodeValue(selectedNode);
                        }
                    }

                    if (NodeValue) {
                        Borrar_Procedencia();
                        Borrar_Caracteristica();

                        $.ajax({
                            type: 'POST',
                            contentType: "application/json; charset=utf-8",
                            url: RutaAplicacion + "/Controles/ServiciosWeb.asmx/get_Relaciones",
                            dataType: 'json',
                            data: JSON.stringify({ Id_Origen: NodeValue, Id_Relacion: IdProducto_TV }),
                            beforeSend: function () { },
                            success: function (Respuesta) {
                                if (Respuesta.d.length) {
                                    $.each($.parseJSON(Respuesta.d), function (index, element) {
                                        $("#lvProducto_UL").append('<li style="white-space: nowrap;"><a href="#" onclick="Seleccionar_Producto(' + element.idEstructura + ', \'' + element.text + '\')" class="ContactarCon">' + element.text + '</a></li>');
                                    });
                                };
                                $find('<%=mpe_pnlProducto.ClientID%>').show();
                            },
                            error: function (ex) {
                                //debugger;
                                alert("ERR: " + ex.statusText + ": " + ex.responseText);
                            },
                            complete: function () { }
                        });
                    }
                }
                Seleccionar_Producto = function (ID, Descripcion) {
                    $find('<%=mpe_pnlProducto.ClientID%>').hide();
                    var hf_IdProducto = $('#<%=hf_IdProducto.ClientID%>');
                    hf_IdProducto.val(ID);
                    hf_IdProducto.next().remove();
                    hf_IdProducto.after('<ul><li>' + Descripcion + '</li></ul>');

                    Borrar_Caracteristica();

                    $.ajax({
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        url: RutaAplicacion + "/Controles/ServiciosWeb.asmx/get_Relaciones",
                        dataType: 'json',
                        data: JSON.stringify({ Id_Origen: ID, Id_Relacion: IdProducto_TV }),
                        beforeSend: function () { },
                        success: function (Respuesta) {
                            if (Respuesta.d.length) {
                                $.each($.parseJSON(Respuesta.d), function (index, element) {
                                    $("#lvCaracteristica_UL").append('<li style="white-space: nowrap;" title="' + element.extra + '"><a href="#" onclick="Seleccionar_Caracteristica(' + element.idEstructura + ', \'' + element.text + '\')" class="ContactarCon">' + element.text + '</a></li>');
                                });
								/*****************************************************/
                            };
                            $find('<%=mpe_pnlCaracteristica.ClientID%>').show();
                        },
                        error: function (ex) { alert(ex.statusText + ": " + ex.responseText); },
                        complete: function () { }
                    });
                }
                <%--Seleccionar_Caracteristica = function (ID, Descripcion) {
                    $find('<%=mpe_pnlCaracteristica.ClientID%>').hide();
                    var hf_IdCaracteristica = $('#<%=hf_IdCaracteristica.ClientID%>');
                    hf_IdCaracteristica.val(ID);
                    hf_IdCaracteristica.next().remove();
                    hf_IdCaracteristica.after('<ul><li>' + Descripcion + '</li></ul>');
                }--%>                
                Seleccionar_Caracteristica = function (ID, Descripcion) {
                    $find('<%=mpe_pnlCaracteristica.ClientID%>').hide();
                    var mensaje = $('#<%= mensajeRotura.ClientID%>');
                    if (ID == <%=idRoturaOk%>) {
                        mensaje.text("Rotura debida a una  mala manipulación del elemento (Reponer el mismo elemento)");
                        openModalRotura(ID,Descripcion);
                    } else if (ID == <%= idRoturaNOOK%>) {
                        mensaje.text("Rotura debida a que el elemento comprado NO es el adecuado");
                        openModalRotura(ID,Descripcion);
                    } else {                        
                        AplicarCaracteristica(ID, Descripcion);
                    }
                }
<%--                AplicarCaracteristica = function (ID, Descripcion) {  
                    if (ID == '' && Descripcion == '') {
                        ID = $('#<%=hf_IDCARAC.ClientID%>').val();
                        Descripcion = $('#<%=hf_DESCRIP.ClientID%>').val();
                    }
                    var hf_IdCaracteristica = $('#<%=hf_IdCaracteristica.ClientID%>');
                    hf_IdCaracteristica.val(ID);
                    hf_IdCaracteristica.next().remove();
                    hf_IdCaracteristica.after('<ul><li>' + Descripcion + '</li></ul>');
                }
                function openModalRotura(ID, Descripcion) {                  
                    var hf_IDCARAC = $('#<%=hf_IDCARAC.ClientID%>');
                    hf_IDCARAC.val(ID);
                    var hf_DESCRIP = $('#<%=hf_DESCRIP.ClientID%>');
                    hf_DESCRIP.val(Descripcion);
                    $('#Modal_Rotura').modal('show');
                }--%>
            });

            $find('<%=pce_OFOPM.ClientID%>').add_shown(function () { var obj = $get('<%= txt_OFOPM.ClientID%>'); obj.focus(); obj.select(); });

            $find('<%=pce_pnlCreador.ClientID%>').add_shown(function () { var obj = $get('<%= txtCreador.ClientID%>'); obj.focus(); obj.select(); });

            $find('<%=pce_pnlResponsable.ClientID%>').add_shown(function () { var obj = $get('<%= txtResponsable.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlRespResolucion.ClientID%>').add_shown(function () { var obj = $get('<%= txtRespResolucion.ClientID%>'); obj.focus(); obj.select(); });

            $find('<%=pce_pnlGestor.ClientID%>').add_shown(function () { var obj = $get('<%= txtGestor.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlCoordinador_Fabricacion.ClientID%>').add_shown(function () { var obj = $get('<%= txtCoordinador_Fabricacion.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlCalidad_Fabricacion.ClientID%>').add_shown(function () { var obj = $get('<%= txtCalidad_Fabricacion.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlCalidad_proveedores.ClientID%>').add_shown(function () { var obj = $get('<%= txtCalidad_proveedores.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlCalidad_Cliente.ClientID%>').add_shown(function () { var obj = $get('<%= txtCalidad_Cliente.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlAlmacen.ClientID%>').add_shown(function () { var obj = $get('<%= txtAlmacen.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlIngenieriaFabricacion.ClientID%>').add_shown(function () { var obj = $get('<%= txtIngenieriaFabricacion.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlOtros.ClientID%>').add_shown(function () { var obj = $get('<%= txtOtros.ClientID%>'); obj.focus(); obj.select(); });

            $find('<%=pce_pnlAjuste.ClientID%>').add_shown(function () { var obj = $get('<%= txtAjuste.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlSeguimiento.ClientID%>').add_shown(function () { var obj = $get('<%= txtSeguimiento.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlMedicion.ClientID%>').add_shown(function () { var obj = $get('<%= txtMedicion.ClientID%>'); obj.focus(); obj.select(); });
            $find('<%=pce_pnlHomologacion.ClientID%>').add_shown(function () { var obj = $get('<%= txtHomologacion.ClientID%>'); obj.focus(); obj.select(); });

            Set_ddlProveedor_OF();

            $('#<%=ddlProcedencia.ClientID%>').change(function () {
                //$('#<=pnlDepartamentos.ClientID%>').attr("style", "display:normal"); //SI. $ = sys.system...
                //$get('<=pnlDepartamentos.ClientID%>').style.display = "normal"; //SI.  $get = document.getElemenById
                var pnlDepartamentos = $('#<%=pnlDepartamentos.ClientID%>');
                var pnlPlantas = $('#<%=pnlPlantas.ClientID%>');
                var pnlProveedores = $('#<%=pnlProveedores.ClientID%>');

                
                var img_tvAreas = $('#<%=img_tvAreas.ClientID%>');
                var lblOrigen = $('#<%=lblOrigen.ClientID%>');
                var lblCapacidad = $('#<%=lblCapacidad.ClientID%>');

                var rfv_ddlPlantas = $get('<%=rfv_ddlPlantas.ClientID%>');
                var cv_tvAreas = $get('<%=cv_tvAreas.ClientID%>');
                var rfv_ddlCapacidad = $get('<%=rfv_ddlCapacidad.ClientID%>');

                ValidatorEnable(rfv_ddlPlantas, false);
                ValidatorEnable(cv_tvAreas, false);
                ValidatorEnable(rfv_ddlCapacidad, false);

                pnlDepartamentos.attr("style", "display:none");
                pnlPlantas.attr("style", "display:none");
                pnlProveedores.attr("style", "display:none");

                switch ($("#<%=ddlProcedencia.ClientID%>").val()) {
                    case "1": case "4":
                        img_tvAreas.attr("style", "display:normal");
                        lblOrigen.attr("style", "display:normal");
                        lblCapacidad.attr("style", "display:none");

                        pnlDepartamentos.attr("style", "display:normal");
                        ValidatorEnable(cv_tvAreas, true);
                        cv_tvAreas.style.display = "none";
                        break;
                    case "2":
                        img_tvAreas.attr("style", "display:none");
                        lblOrigen.attr("style", "display:none");
                        lblCapacidad.attr("style", "display:normal");

                        pnlProveedores.attr("style", "display:normal");
                        ValidatorEnable(rfv_ddlCapacidad, true);
                        rfv_ddlCapacidad.style.display = "none";
                        break;
                    case "3":
                        pnlPlantas.attr("style", "display:normal");
                        ValidatorEnable(rfv_ddlPlantas, true);
                        rfv_ddlPlantas.style.display = "none";
                        break;
                }

                //----------------------------------------------------------------
                //Quitamos las seleccines previas
                //----------------------------------------------------------------
                $('#<%=ddlCapacidad.ClientID%>').prop('selectedIndex', 0);
                $("#lvAreas_UL").empty();
                Borrar_Procedencia();
                Borrar_Caracteristica();
                //----------------------------------------------------------------
            });

            $('#<%=ddlCapacidad.ClientID%>').on("change", function () {
                Set_ddlProveedor_OF();

                Borrar_Procedencia();
                Borrar_Caracteristica();

                var ddlCapacidad_val = $("#<%=ddlCapacidad.ClientID%>").val();
                if (ddlCapacidad_val) { Seleccionar_Procedencia(ddlCapacidad_val); };
            });

            $('#<%=img_BuscarProducto.ClientID%>').on("click", function () { $find('<%=mpe_pnlProducto.ClientID%>').show(); });
            $('#<%=img_BuscarCaracteristica.ClientID%>').on("click", function () { $find('<%=mpe_pnlCaracteristica.ClientID%>').show(); });
        }

        /**************************************************************************************************
         * Procedencia Proveedor
         */
        function Set_ddlProveedor_OF() {
            var ddlCapacidad = $('#<%=ddlCapacidad.ClientID%>');
            var hd_OFOPM = [];
            $.each($('input:hidden[name=hd_OFOPM]'), function () { hd_OFOPM.push($(this).val()); });
            var myObj = { "IdCap": ddlCapacidad.val(), "hd_OFOPM": hd_OFOPM.join(";") };
            var cdd_ddlProveedor_OF = $find('<%=cdd_ddlProveedor_OF.ClientID%>');
            cdd_ddlProveedor_OF._contextKey = JSON.stringify(myObj);
            cdd_ddlProveedor_OF._category = cdd_ddlProveedor_OF._selectedValue;
            cdd_ddlProveedor_OF.initialize();
        }
        <%--function Set_ddlCapacidad() {
            var ddlProveedor_OF = $('#<%=ddlProveedor_OF.ClientID%>');
            var hd_OFOPM = [];
            $.each($('input:hidden[name=hd_OFOPM]'), function () { hd_OFOPM.push($(this).val()); });
            var myObj = { "IdProv": ddlProveedor_OF.val(), "hd_OFOPM": hd_OFOPM.join(";") };
            var cdd_ddlCapacidad = $find('<%=cdd_ddlCapacidad.ClientID%>');
            cdd_ddlCapacidad._contextKey = JSON.stringify(myObj);
            cdd_ddlCapacidad._category = cdd_ddlCapacidad._selectedValue;
            //debugger;
            cdd_ddlCapacidad.initialize();
        }--%>
        /************************************************************************************************/

        function Set_OFOPM(source, eventArgs) {
            $find('<%=pce_OFOPM.ClientID%>').hidePopup();
            $("#lv_OFOPM_UL").append('<li style="white-space: nowrap;"><input name="hd_OFOPM" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txt_OFOPM.ClientID%>').value + ' <a href="#hd_OFOPM" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txt_OFOPM.ClientID%>').value = "";

            Set_ddlProveedor_OF();
            //Set_ddlCapacidad();
        }

        /* Creador NC ************************************************************************************/
        function Set_Creador(source, eventArgs) {
            $find('<%=pce_pnlCreador.ClientID%>').hidePopup();
            $("#lvCreador_UL").empty();
            $("#lvCreador_UL").append('<li><input name="hd_IdCreador" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtCreador.ClientID%>').value + ' <a href="#hd_IdCreador" onclick="Borrar_Elemento(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrarCreador" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtCreador.ClientID%>').value = "";
        }
        /* Responsable NC (Perseguidor) ************************************************************************************/
        function Set_Responsable(source, eventArgs) {
            <%--$get('<%= hd_IdResponsable.ClientID%>').value = eventArgs.get_value();--%>
        <%--$get('<%= lblResponsable.ClientID%>').innerHTML = $get('<%= txtResponsable.ClientID%>').value;--%>
            $find('<%=pce_pnlResponsable.ClientID%>').hidePopup();
            $("#lvResponsables_UL").append('<li><input name="hd_IdResponsables" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtResponsable.ClientID%>').value + ' <a href="#hd_IdResponsables" onclick="Borrar_Responsable(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrarResp" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtResponsable.ClientID%>').value = "";
            
            count = parseInt($("#<%=numberOfResponsibles.ClientID %>").val());
            if (count == null || count == undefined) {
                count = 0;
            }
            count = count + 1;
            $("#<%=numberOfResponsibles.ClientID %>").val(count);
            //alert("numero de responsables: " + count);
        }


        function Borrar_Responsable(e) {
            //debugger;
            e.parentNode.parentNode.removeChild(e.parentNode);
            count = parseInt($("#<%=numberOfResponsibles.ClientID %>").val());
            count = count - 1;
            $("#<%=numberOfResponsibles.ClientID %>").val(count);
            //alert("numero de responsables: " + count);
        }

        function Borrar_Elemento(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
        }
        /*******************************************************************************************************************/
        /* Responsables Resolucion *****************************************************************************************/
        function Set_RespResolucion(source, eventArgs) {
            $find('<%=pce_pnlRespResolucion.ClientID%>').hidePopup();
            $("#lvRespResolucion_UL").append('<li><input name="hd_IdRespResolucion" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtRespResolucion.ClientID%>').value + ' <a href="#hd_IdRespResolucion" onclick="Borrar_Elemento(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrarRespResolucion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtRespResolucion.ClientID%>').value = "";
        }
        //function Borrar_Elemento(e) {
        //    e.parentNode.parentNode.removeChild(e.parentNode);
        //}
        /*******************************************************************************************************************/

        /*******************************************************************************************************************/
        /* Asistentes a la reunion preliminar.
        /*******************************************************************************************************************/
        /* Gestor **********************************************************************************************************/
        function Set_Gestor(source, eventArgs) {
            $find('<%=pce_pnlGestor.ClientID%>').hidePopup();
            $("#lvGestor_UL").append('<li><input name="hd_IdGestor" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtGestor.ClientID%>').value + ' <a href="#hd_IdGestor" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Gestor" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtGestor.ClientID%>').value = "";
        }
        /* Coordinador Fabricacion *****************************************************************************************/
        function Set_Coordinador_Fabricacion(source, eventArgs) {
            $find('<%=pce_pnlCoordinador_Fabricacion.ClientID%>').hidePopup();
            $("#lvCoordinador_Fabricacion_UL").append('<li><input name="hd_IdCoordinador_Fabricacion" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtCoordinador_Fabricacion.ClientID%>').value + ' <a href="#hd_IdCoordinador_Fabricacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Coordinador_Fabricacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtCoordinador_Fabricacion.ClientID%>').value = "";
        }
        /* Calidad Fabricacion ********************************************************************************************/
        function Set_Calidad_Fabricacion(source, eventArgs) {
            $find('<%=pce_pnlCalidad_Fabricacion.ClientID%>').hidePopup();
            $("#lvCalidad_Fabricacion_UL").append('<li><input name="hd_IdCalidad_Fabricacion" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtCalidad_Fabricacion.ClientID%>').value + ' <a href="#hd_IdCalidad_Fabricacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Calidad_Fabricacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtCalidad_Fabricacion.ClientID%>').value = "";
        }
        /* Calidad proveedores *********************************************************************************************/
        function Set_Calidad_proveedores(source, eventArgs) {
            $find('<%=pce_pnlCalidad_proveedores.ClientID%>').hidePopup();
            $("#lvCalidad_proveedores_UL").append('<li><input name="hd_IdCalidad_proveedores" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtCalidad_proveedores.ClientID%>').value + ' <a href="#hd_IdCalidad_proveedores" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Calidad_proveedores" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtCalidad_proveedores.ClientID%>').value = "";
        }
        /* Calidad Cliente *********************************************************************************************/
        function Set_Calidad_Cliente(source, eventArgs) {
            $find('<%=pce_pnlCalidad_Cliente.ClientID%>').hidePopup();
            $("#lvCalidad_Cliente_UL").append('<li><input name="hd_IdCalidad_Cliente" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtCalidad_Cliente.ClientID%>').value + ' <a href="#hd_IdCalidad_Cliente" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Calidad_Cliente" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtCalidad_Cliente.ClientID%>').value = "";
        }
        /* Almacen *********************************************************************************************/
        function Set_Almacen(source, eventArgs) {
            $find('<%=pce_pnlAlmacen.ClientID%>').hidePopup();
            $("#lvAlmacen_UL").append('<li><input name="hd_IdAlmacen" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtAlmacen.ClientID%>').value + ' <a href="#hd_IdAlmacen" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Almacen" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtAlmacen.ClientID%>').value = "";
        }
        /* Ingeniria Fabricacion *******************************************************************************/
        function Set_IngenieriaFabricacion(source, eventArgs) {
            $find('<%=pce_pnlIngenieriaFabricacion.ClientID%>').hidePopup();
            $("#lvIngenieriaFabricacion_UL").append('<li><input name="hd_IdIngenieriaFabricacion" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtIngenieriaFabricacion.ClientID%>').value + ' <a href="#hd_IdIngenieriaFabricacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_IngenieriaFabricacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtIngenieriaFabricacion.ClientID%>').value = "";
        }
        /* Otros **********************************************************************************************/
        function Set_Otros(source, eventArgs) {
            $find('<%=pce_pnlOtros.ClientID%>').hidePopup();
            $("#lvOtros_UL").append('<li><input name="hd_IdOtros" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtOtros.ClientID%>').value + ' <a href="#hd_IdOtros" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Otros" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtOtros.ClientID%>').value = "";
        }
        /* Ajuste **********************************************************************************************/
        function Set_Ajuste(source, eventArgs) {
            $find('<%=pce_pnlAjuste.ClientID%>').hidePopup();
            $("#lvAjuste_UL").append('<li><input name="hd_IdAjuste" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtAjuste.ClientID%>').value + ' <a href="#hd_IdAjuste" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Ajuste" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtAjuste.ClientID%>').value = "";
        }
        /* Seguimiento ******************************************************************************************/
        function Set_Seguimiento(source, eventArgs) {
            $find('<%=pce_pnlSeguimiento.ClientID%>').hidePopup();
            $("#lvSeguimiento_UL").append('<li><input name="hd_IdSeguimiento" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtSeguimiento.ClientID%>').value + ' <a href="#hd_IdSeguimiento" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Seguimiento" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtSeguimiento.ClientID%>').value = "";
        }
        /* Medicion *********************************************************************************************/
        function Set_Medicion(source, eventArgs) {
            $find('<%=pce_pnlMedicion.ClientID%>').hidePopup();
            $("#lvMedicion_UL").append('<li><input name="hd_IdMedicion" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtMedicion.ClientID%>').value + ' <a href="#hd_IdMedicion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Medicion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtMedicion.ClientID%>').value = "";
        }
        /* Homologacion *********************************************************************************************/
        function Set_Homologacion(source, eventArgs) {
            $find('<%=pce_pnlHomologacion.ClientID%>').hidePopup();
            $("#lvHomologacion_UL").append('<li><input name="hd_IdHomologacion" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txtHomologacion.ClientID%>').value + ' <a href="#hd_IdHomologacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Homologacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
            $get('<%= txtHomologacion.ClientID%>').value = "";
        }
        /*******************************************************************************************************************/
        function Borrar_Item(e) {
            e.parentNode.parentNode.removeChild(e.parentNode);
            if (e.hash == "#hd_OFOPM") {
                Set_ddlProveedor_OF();
                //Set_ddlCapacidad();
            };
        }
        /*******************************************************************************************************************/

        function SetContextKey() {
        //$find('<=ace_txtRespResolucion.ClientID%>').set_contextKey($("#<%=ddlProcedencia.ClientID%>").val());
            var ContextKey = $("#<%=ddlProcedencia.ClientID%>").val() + ";" + $("#<%=ddlProveedor_OF.ClientID%>").val();
            //alert(ContextKey);
            $find('<%=ace_txtRespResolucion.ClientID%>').set_contextKey(ContextKey);
        }

        /* Validacion del TreeView de las Areas *****************************************************************************/
        function ClientValidate_TreeView(source, arguments) {
            var treeView = document.getElementById("<%= tvAreas.ClientID%>");
            var checkBoxes = treeView.getElementsByTagName("input");
            var checkedCount = 0;
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].checked) {
                    checkedCount++;
                }
            }
            if (checkedCount > 0) {
                arguments.IsValid = true;
            } else {
                arguments.IsValid = false;
            }
        }
        function ClientValidate_tvAreas(source, arguments) {
            var treeViewData = window["<%=tvAreas.ClientID%>" + "_Data"];
            arguments.IsValid = (treeViewData.selectedNodeID.value);
        }
        /*******************************************************************************************************************/

        /* Proveedor de la Procedencia *************************************************************************************/
        //function Set_Proveedor(source, eventArgs) {
        //    $get('<= hdIdProveedor.ClientID%>').value = eventArgs.get_value();
        //    $get('<= lblProveedor.ClientID%>').innerHTML = eventArgs.get_text();
        //    $find('<=pce_pnlProveedor.ClientID%>').hidePopup();

        //    if (source) {
        //        //Access the selected value to the CascadingdDropDown's contextkey.
        //        $find('<=cdd_ddlCapacidades.ClientID%>')._contextKey = eventArgs.get_value();
        //        //Initialize the CascadingDropDown to recall its ServiceMethod.
        //        $find('<=cdd_ddlCapacidades.ClientID%>').initialize();
        //    }

        //}
        //function ValidarProveedor(source, arguments) {
        //    arguments.IsValid = ($get('<= hdIdProveedor.ClientID%>').value != "");
        //}

        function ValidarProveedor_OF(source, arguments) {
            arguments.IsValid = ($("#<%=ddlProveedor_OF.ClientID%>").val() != "");
        }
        /*******************************************************************************************************************/

        /****************************************************************************************************/
        /* Funcion para que solo se puede seleccionar un CheckBox */
        /****************************************************************************************************/
        function TreeView_ExclusiveCheckBox_Deteccion(evt, tvNodes) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            var isNodeClick = (src.tagName.toLowerCase() == "a" && src.previousSibling);
            var nodeValue, nodeText;

            /* ------------------------------------------------------------- */
            var chBoxes = tvNodes.getElementsByTagName("input");
            for (var i = 0; i < chBoxes.length; i++) {
                var chk = chBoxes[i];
                if ((chk.type == "checkbox")) {
                    var Nodo = chk.nextSibling;
                    var chkValue = GetNodeValue(Nodo);
                    if (isChkBoxClick) {
                        nodeValue = GetNodeValue(src.nextSibling);
                        nodeText = src.nextSibling.innerText;
                        chk.checked = ((chkValue == nodeValue) && (chk.checked));
                    } else if (isNodeClick) {
                        nodeValue = GetNodeValue(src);
                        nodeText = src.innerText;
                        chk.checked = ((chkValue == nodeValue) && !(chk.checked));
                    }

                    /* ------------------------------------------------------------- */
                    //Creamos dinamicamente los elementos del ListView
                    /* ------------------------------------------------------------- */
                    if (chk.checked && (isChkBoxClick || isNodeClick)) {
                        $("#lvDeteccion_UL").append('<li><input name="hd_IdDeteccion" type="hidden" value="' + nodeValue + '">' + nodeText + '</li>');
                    } else if (isChkBoxClick || isNodeClick) {
                        var allInputs = document.getElementsByName("hd_IdDeteccion");
                        for (var x = 0; x < allInputs.length; x++)
                            if (allInputs[x].value == chkValue)
                                allInputs[x].parentNode.parentNode.removeChild(allInputs[x].parentNode);
                    }
                    /* ------------------------------------------------------------- */
                }
            }
            /* ------------------------------------------------------------- */
            $find('pce_pnl_tvDeteccion').hidePopup();
            return isChkBoxClick; //comment this if you want postback on node click
        }

    //function TreeView_ExclusiveCheckBox_tvAreas(evt, tvNodes) {
    //    var src = window.event != window.undefined ? window.event.srcElement : evt.target;
    //    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    //    var isNodeClick = (src.tagName.toLowerCase() == "a" && src.previousSibling);
    //    var nodeValue, nodeText;

    ////    var treeViewData = window["<%=tvAreas.ClientID%>" + "_Data"];
        //    if (treeViewData.selectedNodeID.value != "") {
        //        var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);
        //        var NodeValue2 = GetNodeValue(selectedNode);
        //    }

        //    /* ------------------------------------------------------------- */
        //    var chBoxes = tvNodes.getElementsByTagName("input");
        //    for (var i = 0; i < chBoxes.length; i++) {
        //        var chk = chBoxes[i];
        //        if ((chk.type == "checkbox")) {
        //            var Nodo = chk.nextSibling;
        //            var chkValue = GetNodeValue(Nodo);
        //            if (isChkBoxClick) {
        //                nodeValue = GetNodeValue(src.nextSibling);
        //                nodeText = src.nextSibling.innerText;
        //                chk.checked = ((chkValue == nodeValue) && (chk.checked));
        //            } else if (isNodeClick) {
        //                nodeValue = GetNodeValue(src);
        //                nodeText = src.innerText;
        //                chk.checked = ((chkValue == nodeValue) && !(chk.checked));
        //            }

        //            /* ------------------------------------------------------------- */
        //            //Creamos dinamicamente los elementos del ListView
        //            /* ------------------------------------------------------------- */
        //            if (chk.checked && (isChkBoxClick || isNodeClick)) {
        //                $("#lvAreas_UL").empty();
        //                $("#lvAreas_UL").append('<li><input name="hd_IdArea" type="hidden" value="' + nodeValue + '">' + nodeText + '</li>');
        //            } else if (isChkBoxClick || isNodeClick) {
        //                var allInputs = document.getElementsByName("hd_IdArea");
        //                for (var x = 0; x < allInputs.length; x++)
        //                    if (allInputs[x].value == chkValue)
        //                        allInputs[x].parentNode.parentNode.removeChild(allInputs[x].parentNode);
        //            }
        //            /* ------------------------------------------------------------- */
        //        }
        //    }
        //    /* ------------------------------------------------------------- */
        //    $find('pce_pnl_tvAreas').hidePopup();

        //    //Seleccionar_Procedencia(nodeValue);

        //    return isChkBoxClick; //comment this if you want postback on node click
        //}

		function tvAreas_SeleccionarNodo(evt, tvNodes) {
            var treeViewData = window["<%=tvAreas.ClientID%>" + "_Data"];
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var nodeText = src.innerText
            if (treeViewData.selectedNodeID.value) {
                $("#lvAreas_UL").empty();
                $("#lvAreas_UL").append('<li>' + nodeText + '</li>');
            }
            $find('pce_pnl_tvAreas').hidePopup();
            Seleccionar_Procedencia();
            return false;
        }

        function TreeView_ExclusiveCheckBox_tvFunciones(evt, tvNodes) {
            //console.log("TreeView_ExclusiveCheckBox_tvFunciones");
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            var isNodeClick = (src.tagName.toLowerCase() == "a" && src.previousSibling);
            var nodeValue, nodeText;

            /* ------------------------------------------------------------- */
            var chBoxes = tvNodes.getElementsByTagName("input");
            for (var i = 0; i < chBoxes.length; i++) {
                var chk = chBoxes[i];
                if ((chk.type == "checkbox")) {
                    var Nodo = chk.nextSibling;
                    var chkValue = GetNodeValue(Nodo);
                    if (isChkBoxClick) {
                        nodeValue = GetNodeValue(src.nextSibling);
                        chk.checked = ((chkValue == nodeValue) && (chk.checked));
                        nodeText = src.nextSibling.innerText;
                    } else if (isNodeClick) {
                        nodeValue = GetNodeValue(src);
                        nodeText = src.innerText;
                        chk.checked = ((chkValue == nodeValue) && !(chk.checked));
                    }
                }
            }
            /* ------------------------------------------------------------- */
            //$find('pce_pnl_tvAreas').hidePopup();
            return isChkBoxClick; //comment this if you want postback on node click
        }

        function GetNodeValue(node) {
            var nodeValue = "";
            var nodePath = node.href.substring(node.href.indexOf(",") + 2, node.href.length - 2);
            var nodeValues = nodePath.split("\\");
            if (nodeValues.length > 1)
                nodeValue = nodeValues[nodeValues.length - 1];
            else
                nodeValue = nodeValues[0].substr(1);
            return nodeValue;
        }
        /****************************************************************************************************/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_FORM" runat="server">

    <ascx:titulo id="TituloNumNC" runat="server" texto="Nº" />

    <table class="GridViewASP" style="width: 1%;">
        <tr class="HeaderStyle">
            <th style="white-space: nowrap;">
                <asp:Image ID="imgOPOFM" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                <asp:Label ID="Label24" runat="server" Text="OF-OP (Marca)"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Label ID="Label60" runat="server" Text="Proyecto"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Image ID="img_tvDeteccion" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                <asp:Label ID="Label8" runat="server" Text="Deteccion"></asp:Label>
            </th>
        </tr>
        <tr class="RowStyle">
            <td>
                <act:popupcontrolextender id="pce_OFOPM" runat="server" targetcontrolid="imgOPOFM" popupcontrolid="pnl_OFOPM" position="Bottom" />
                <asp:Panel ID="pnl_OFOPM" runat="server" Width="100%">
                    <asp:TextBox ID="txt_OFOPM" runat="server" AutoCompleteType="Search" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender9" runat="server" targetcontrolid="txt_OFOPM" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txt_OFOPM" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txt_OFOPM" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="10" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_OFOPM" onclientitemselected="Set_OFOPM"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lv_OFOPM" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lv_OFOPM_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li style="white-space: nowrap;">
                            <input name="hd_OFOPM" type="hidden" value='<%#Eval("Id")%>'><%#Eval("Descripcion")%>
                            <a href="#hd_OFOPM" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lv_OFOPM_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
            <td style="white-space: nowrap;">
                <asp:Label runat="server" ID="lblProyecto"></asp:Label>
            </td>
            <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                <asp:ListView ID="lvDeteccion" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvDeteccion_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdDeteccion" type="hidden" value='<%#Eval("ID")%>'><%#Eval("DESCRIPCION")%>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvDeteccion_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>

    <table class="GridViewASP" style="width: 1%;">
        <tr class="HeaderStyle">
            <th>
                <asp:Label ID="Label1" runat="server" Text="Procedencia"></asp:Label>
            </th>

            <th style="white-space: nowrap;">
                <asp:Image ID="img_tvAreas" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" 
                    Style="display: none;"/>
                <asp:Label ID="lblOrigen" runat="server" Text="Origen NC" Style="display: none;"></asp:Label>
                <asp:Label ID="lblCapacidad" runat="server" Text="Capacidad" Style="display: none;"></asp:Label>
            </th>

            <th style="white-space: nowrap;">
                <asp:Image ID="img_BuscarProducto" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                <asp:Label ID="Label26" runat="server" Text="Producto" ToolTip="PRODUCTO CAUSA NO CONFORMIDAD"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Image ID="img_BuscarCaracteristica" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                <asp:Label ID="Label106" runat="server" Text="Caracteristicas / Tipo Error"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Label ID="Label2" runat="server" Text="Fecha Inicio"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Label ID="Label4" runat="server" Text="Fecha Cierre"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Label ID="Label40" runat="server" Text="Retraso" ToolTip="Semanas"></asp:Label>
            </th>
        </tr>
        <tr>
            <td class="RowStyle">
                <asp:DropDownList ID="ddlProcedencia" runat="server">
                                <asp:ListItem Value="" Text="(Seleccione uno)" />
                                <asp:ListItem Value="1" Text="Interna (Torrea)" />
                                <%--<asp:ListItem Value="4" Text="Interna (Araluce)" />--%>
                                <%--<asp:ListItem Value="3" Text="A planta Batz" Enabled="false" />--%>
                                <asp:ListItem Value="2" Text="A proveedor" />
                            </asp:DropDownList>
                <%--<table border="0">
                    <tr>
                        <td>
                            
                        </td>
                        <td></td>
                    </tr>
                </table>--%>
            </td>
            <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                <asp:Panel ID="pnlDepartamentos" runat="server" Style="display: none;">
                   <%-- <div style="float: left;">
                        <asp:Image ID="img_tvAreas" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                    </div>--%>
                    <asp:ListView ID="lvAreas" runat="server" DataKeyNames="Id">
                        <LayoutTemplate>
                            <ul id="lvAreas_UL">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li>
                                <%#Eval("DESCRIPCION")%>
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <ul id="lvAreas_UL"></ul>
                        </EmptyDataTemplate>
                        <EmptyItemTemplate>
                            ??
                        </EmptyItemTemplate>
                    </asp:ListView>
                </asp:Panel>
                <asp:Panel ID="pnlPlantas" runat="server" Style="display: none;">
                    <asp:DropDownList runat="server" ID="ddlPlantas" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value">
                        <asp:ListItem Value="" Text="(Seleccione uno)"></asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
                <asp:Panel ID="pnlProveedores" runat="server" Style="display: none;">
                    <table class="GridViewASP">
                        <thead>
                            <tr class="HeaderStyle">
                                <th>
                                    <asp:Label ID="Label25" runat="server" Text="Proveedor"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label20" runat="server" Text="Capacidad"></asp:Label></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="RowStyle">
                                <td>
                                    <asp:DropDownList ID="ddlProveedor_OF" runat="server"></asp:DropDownList>
                                    <act:cascadingdropdown id="cdd_ddlProveedor_OF" runat="server" targetcontrolid="ddlProveedor_OF"
                                        category="Proveedor" usecontextkey="true" prompttext="(Seleccione uno)" loadingtext="Cargando"
                                        servicepath="~/Controles/ServiciosWeb.asmx" servicemethod="get_Proveedor_OF"
                                        enabled="true" enableatloading="true" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCapacidad" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" ToolTip="Capacidades"></asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </td>
            <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                <asp:HiddenField ID="hf_IdProducto" runat="server" />
                <asp:BulletedList ID="blProducto" runat="server" DisplayMode="Text" ViewStateMode="Disabled" />
            </td>
            <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                <asp:HiddenField ID="hf_IdCaracteristica" runat="server" />
                <asp:BulletedList ID="blCaracteristica" runat="server" DisplayMode="Text" ViewStateMode="Disabled" />
            </td>
            <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                <asp:TextBox ID="txtFechaApertura" runat="server" Columns="10"></asp:TextBox>
                <act:calendarextender id="txtFechaApertura_CalendarExtender" runat="server" targetcontrolid="txtFechaApertura" />
                <act:calendarextender id="imgCalendario_CalendarExtender" runat="server" targetcontrolid="txtFechaApertura" popupbuttonid="imgCalendario" />
                &nbsp;<asp:ImageButton ID="imgCalendario" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" />
            </td>
            <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                <asp:TextBox ID="txtFechaCierre" runat="server" Columns="10"></asp:TextBox>
                <act:calendarextender id="ce_txtFechaCierre" runat="server" targetcontrolid="txtFechaCierre" />
                <act:calendarextender id="ce_imgCalendario2" runat="server" targetcontrolid="txtFechaCierre" popupbuttonid="imgCalendario2" />
                &nbsp;<asp:ImageButton ID="imgCalendario2" runat="server" AlternateText="Calendario" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/CalendarExtender/Calendario.gif" />
            </td>
            <td class="RowStyle" style="text-wrap: none; white-space: nowrap;">
                <asp:TextBox ID="txtRetraso" runat="server" AutoCompleteType="None" MaxLength="2" TextMode="SingleLine"></asp:TextBox>
            </td>
        </tr>
    </table>

    <table class="GridViewASP">
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap;">
                <asp:Image ID="imgBuscarCreador" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" CssClass="in" />
                <asp:Label ID="Label3" runat="server" Text="Creador"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Image ID="imgBuscarResponsable" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
                <asp:Label ID="Label7" runat="server" Text="Perseguidor" ToolTip="Encargado de Validar las etapas del 8D"></asp:Label>
            </th>
            <th style="white-space: nowrap;">
                <asp:Image ID="imgBuscarRespResulucion" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle"
                    onclick="SetContextKey();" />
                <asp:Label ID="Label9" runat="server" Text="Responsable" ToolTip="Responsable de Resolucion"></asp:Label>
            </th>
        </tr>
        <tr>
            <td class="RowStyle" style="width: 1%; white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlCreador" runat="server" targetcontrolid="imgBuscarCreador" popupcontrolid="pnlCreador" position="Bottom" />
                <asp:Panel ID="pnlCreador" runat="server" Width="100%">
                    <asp:TextBox ID="txtCreador" runat="server" AutoCompleteType="Search" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender14" runat="server" targetcontrolid="txtCreador" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtCreador" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtCreador" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios" onclientitemselected="Set_Creador" contextkey="1"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>

                <asp:ListView ID="lvCreador" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvCreador_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdCreador" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdCreador" onclick="Borrar_Elemento(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrarCreador" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvCreador_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
            <td class="RowStyle">
                <act:popupcontrolextender id="pce_pnlResponsable" runat="server" targetcontrolid="imgBuscarResponsable" popupcontrolid="pnlResponsable" position="Bottom" />
                <asp:Panel ID="pnlResponsable" runat="server" Width="100%">
                    <asp:TextBox ID="txtResponsable" runat="server" AutoCompleteType="Search" />
                    <act:textboxwatermarkextender id="wmResponsables" runat="server" targetcontrolid="txtResponsable" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtResponsable" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtResponsable" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Perfil" onclientitemselected="Set_Responsable"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvResponsables" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvResponsables_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdResponsables" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdResponsables" onclick="Borrar_Responsable(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrarResp" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvResponsables_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
            <td class="RowStyle">
                <act:popupcontrolextender id="pce_pnlRespResolucion" runat="server" targetcontrolid="imgBuscarRespResulucion" popupcontrolid="pnlRespResolucion" position="Bottom" />
                <asp:Panel ID="pnlRespResolucion" runat="server" Width="100%">
                    <asp:TextBox ID="txtRespResolucion" runat="server" AutoCompleteType="Search" ToolTip="Buscar" />
                    <act:textboxwatermarkextender id="wmRespResolucion" runat="server" targetcontrolid="txtRespResolucion" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtRespResolucion" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtRespResolucion" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="false" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios" onclientitemselected="Set_RespResolucion"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvRespResolucion" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvRespResolucion_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdRespResolucion" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdRespResolucion" onclick="Borrar_Elemento(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrarRespResolucion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvRespResolucion_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>

    <table class="GridViewASP">
        <tr class="HeaderStyle">
            <th style="width: 1%;">
                <asp:Label ID="Label5" runat="server" Text="Descripcion"></asp:Label>
            </th>
            <td class="RowStyle">
                <asp:TextBox ID="txtDescripcion" runat="server" Width="95%" Rows="5" MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>

    <table>
        <tr class="HeaderStyle">
            <th style="width: 1%;">
                <asp:Label ID="Label14" runat="server" Text="Caracteristicas"></asp:Label>
            </th>
        </tr>
        <tr>
            <td>
                <asp:DataList ID="dlEstructuras" runat="server" GridLines="None" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4" Width="96%">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
                    <ItemTemplate>
                        <asp:TreeView ID="tvEstructura" runat="server" SkinID="TreeView" ExpandDepth="1" ShowCheckBoxes="Leaf" />
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>

    <!-- aquí metemos la etapa 2 -->
    <table style="width:100%">
		<tr>
			<td>
				<asp:Panel ID="pnlEtapa_4" runat="server" CssClass="recuadro">
					<table class="GridViewASP">
						<caption>
							<asp:Label ID="Label53" runat="server" Text="Acciones de contencion/inmediatas"></asp:Label></caption>
						<tr class="HeaderStyle">
							<th colspan="3">
								<asp:Label ID="Label54" runat="server" Text="¿Qué acciones de contención/inmediatas han sido llevadas a cabo para garantizar la continuidad de la fabricación del troquel?"></asp:Label></th>
						</tr>
						<tr class="HeaderStyle">
							<th style="width: 1%; white-space: nowrap;">
								<asp:Label ID="Label55" runat="server" Text="Considerar:"></asp:Label></th>
							<th colspan="2">
								<asp:Label ID="Label56" runat="server" Text="Acciones tomadas"></asp:Label></th>
						</tr>
						<tr class="AlternatingRowStyle">
							<th align="left" style="white-space: nowrap;">
								<asp:Label ID="Label61" runat="server" Text="Detalles de las acciones de contención/inmediatas:"></asp:Label></th>
							<th align="left" style="white-space: nowrap; width: 1%;">
								<asp:Label ID="Label62" runat="server" Text="Fecha de cierre de acciones inmediatas"></asp:Label></th>
							<td>
								<asp:TextBox ID="txt_E1_DESCRIPCION_6" runat="server" Width="99%" MaxLength="1000" /></td>
						</tr>
						<tr class="RowStyle">
							<td rowspan="3">
								<asp:TextBox ID="txt_E1_DESCRIPCION_5" runat="server" Width="99%" TextMode="MultiLine" Rows="5" /></td>
						</tr>
					</table>
				</asp:Panel>
			</td>
		</tr>
    </table>
    <!-- -->

    <table class="GridViewASP" style="width: 1%;">
        <caption>
            <asp:Label ID="Label16" runat="server" Text="PERSONAS A INFORMAR" /></caption>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label13" runat="server" Text="Gestor" />
                <asp:Image ID="imgGestor" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlGestor" runat="server" targetcontrolid="imgGestor" popupcontrolid="pnlGestor" position="Right" />
                <asp:Panel ID="pnlGestor" runat="server" Width="100%">
                    <asp:TextBox ID="txtGestor" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender8" runat="server" targetcontrolid="txtGestor" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtGestor" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtGestor" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Gestor"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvGestor" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvGestor_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li style="white-space: nowrap;">
                            <input name="hd_IdGestor" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdGestor" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Gestor" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvGestor_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label15" runat="server" Text="PROVEEDOR (T cerrado, Mec, diseños…)" />
                <asp:Image ID="imgCoordinador_Fabricacion" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlCoordinador_Fabricacion" runat="server" targetcontrolid="imgCoordinador_Fabricacion" popupcontrolid="pnlCoordinador_Fabricacion" position="Right" />
                <asp:Panel ID="pnlCoordinador_Fabricacion" runat="server" Width="100%">
                    <asp:TextBox ID="txtCoordinador_Fabricacion" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender1" runat="server" targetcontrolid="txtCoordinador_Fabricacion" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtCoordinador_Fabricacion" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtCoordinador_Fabricacion" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Coordinador_Fabricacion"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvCoordinador_Fabricacion" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvCoordinador_Fabricacion_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdCoordinador_Fabricacion" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdCoordinador_Fabricacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Coordinador_Fabricacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvCoordinador_Fabricacion_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label17" runat="server" Text="ALMACÉN DE MATERIALES /CHAPA" />
                <asp:Image ID="imgCalidad_Fabricacion" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlCalidad_Fabricacion" runat="server" targetcontrolid="imgCalidad_Fabricacion" popupcontrolid="pnlCalidad_Fabricacion" position="Right" />
                <asp:Panel ID="pnlCalidad_Fabricacion" runat="server" Width="100%">
                    <asp:TextBox ID="txtCalidad_Fabricacion" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender2" runat="server" targetcontrolid="txtCalidad_Fabricacion" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtCalidad_Fabricacion" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtCalidad_Fabricacion" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Calidad_Fabricacion"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvCalidad_Fabricacion" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvCalidad_Fabricacion_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdCalidad_Fabricacion" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdlvCalidad_Fabricacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Calidad_Fabricacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvCalidad_Fabricacion_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label18" runat="server" Text="EXPEDICIONES/LOGÍSTICA" />
                <asp:Image ID="imgCalidad_proveedores" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlCalidad_proveedores" runat="server" targetcontrolid="imgCalidad_proveedores" popupcontrolid="pnlCalidad_proveedores" position="Right" />
                <asp:Panel ID="pnlCalidad_proveedores" runat="server" Width="100%">
                    <asp:TextBox ID="txtCalidad_proveedores" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender3" runat="server" targetcontrolid="txtCalidad_proveedores" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtCalidad_proveedores" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtCalidad_proveedores" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Calidad_proveedores"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvCalidad_proveedores" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvCalidad_proveedores_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdCalidad_proveedores" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdCalidad_proveedores" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Calidad_proveedores" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvCalidad_proveedores_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label19" runat="server" Text="DISEÑO" />
                <asp:Image ID="imgCalidad_Cliente" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlCalidad_Cliente" runat="server" targetcontrolid="imgCalidad_Cliente" popupcontrolid="pnlCalidad_Cliente" position="Right" />
                <asp:Panel ID="pnlCalidad_Cliente" runat="server" Width="100%">
                    <asp:TextBox ID="txtCalidad_Cliente" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender4" runat="server" targetcontrolid="txtCalidad_Cliente" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtCalidad_Cliente" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtCalidad_Cliente" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Calidad_Cliente"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvCalidad_Cliente" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvCalidad_Cliente_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdCalidad_Cliente" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdCalidad_Cliente" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Calidad_Cliente" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvCalidad_Cliente_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label21" runat="server" Text="COMPRAR" />
                <asp:Image ID="imgAlmacen" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlAlmacen" runat="server" targetcontrolid="imgAlmacen" popupcontrolid="pnlAlmacen" position="Right" />
                <asp:Panel ID="pnlAlmacen" runat="server" Width="100%">
                    <asp:TextBox ID="txtAlmacen" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender5" runat="server" targetcontrolid="txtAlmacen" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtAlmacen" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtAlmacen" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Almacen"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvAlmacen" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvAlmacen_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdAlmacen" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdAlmacen" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Almacen" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvAlmacen_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label22" runat="server" Text="MÁQUINAS" />
                <asp:Image ID="imgIngenieriaFabricacion" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlIngenieriaFabricacion" runat="server" targetcontrolid="imgIngenieriaFabricacion" popupcontrolid="pnlIngenieriaFabricacion" position="Right" />
                <asp:Panel ID="pnlIngenieriaFabricacion" runat="server" Width="100%">
                    <asp:TextBox ID="txtIngenieriaFabricacion" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender6" runat="server" targetcontrolid="txtIngenieriaFabricacion" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="AutoCompleteExtender1" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtIngenieriaFabricacion" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_IngenieriaFabricacion"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvIngenieriaFabricacion" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvIngenieriaFabricacion_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdIngenieriaFabricacion" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdIngenieriaFabricacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_IngenieriaFabricacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvIngenieriaFabricacion_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>

        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label6" runat="server" Text="AJUSTE" />
                <asp:Image ID="imgAjuste" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlAjuste" runat="server" targetcontrolid="imgAjuste" popupcontrolid="pnlAjuste" position="Right" />
                <asp:Panel ID="pnlAjuste" runat="server" Width="100%">
                    <asp:TextBox ID="txtAjuste" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender10" runat="server" targetcontrolid="txtAjuste" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtAjuste" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtAjuste" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Ajuste"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvAjuste" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvAjuste_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdAjuste" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdAjuste" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Ajuste" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvAjuste_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label10" runat="server" Text="SEGUIMIENTO" />
                <asp:Image ID="imgSeguimiento" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlSeguimiento" runat="server" targetcontrolid="imgSeguimiento" popupcontrolid="pnlSeguimiento" position="Right" />
                <asp:Panel ID="pnlSeguimiento" runat="server" Width="100%">
                    <asp:TextBox ID="txtSeguimiento" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender11" runat="server" targetcontrolid="txtSeguimiento" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtSeguimiento" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtSeguimiento" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Seguimiento"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvSeguimiento" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvSeguimiento_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdSeguimiento" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdSeguimiento" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Seguimiento" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvSeguimiento_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label11" runat="server" Text="MEDICIÓN" />
                <asp:Image ID="imgMedicion" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlMedicion" runat="server" targetcontrolid="imgMedicion" popupcontrolid="pnlMedicion" position="Right" />
                <asp:Panel ID="pnlMedicion" runat="server" Width="100%">
                    <asp:TextBox ID="txtMedicion" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender12" runat="server" targetcontrolid="txtMedicion" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="ace_txtMedicion" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtMedicion" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Medicion"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvMedicion" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvMedicion_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdMedicion" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdMedicion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Medicion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvMedicion_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label12" runat="server" Text="HOMOLOGACIÓN" />
                <asp:Image ID="imgHomologacion" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlHomologacion" runat="server" targetcontrolid="imgHomologacion" popupcontrolid="pnlHomologacion" position="Right" />
                <asp:Panel ID="pnlHomologacion" runat="server" Width="100%">
                    <asp:TextBox ID="txtHomologacion" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender13" runat="server" targetcontrolid="txtHomologacion" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="AutoCompleteExtender3" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtHomologacion" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Homologacion"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvHomologacion" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvHomologacion_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdHomologacion" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdHomologacion" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Homologacion" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvHomologacion_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>

        <tr class="HeaderStyle">
            <th style="width: 1%; white-space: nowrap; text-align: right;">
                <asp:Label ID="Label23" runat="server" Text="Otros" />
                <asp:Image ID="imgOtros" runat="server" class="ImageButton" AlternateText="Buscar" ToolTip="Buscar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Buscador/Filtrar16.png" onmouseover="this.style.cursor='pointer'" ImageAlign="Middle" />
            </th>
            <td class="RowStyle" style="white-space: nowrap;">
                <act:popupcontrolextender id="pce_pnlOtros" runat="server" targetcontrolid="imgOtros" popupcontrolid="pnlOtros" position="Right" />
                <asp:Panel ID="pnlOtros" runat="server" Width="100%">
                    <asp:TextBox ID="txtOtros" runat="server" />
                    <act:textboxwatermarkextender id="TextBoxWatermarkExtender7" runat="server" targetcontrolid="txtOtros" watermarktext="textoabuscar" watermarkcssclass="TextBoxWatermarkExtender" />
                    <!-- Texto predictivo -->
                    <act:autocompleteextender id="AutoCompleteExtender2" runat="server" enabled="True" servicepath="~/Controles/ServiciosWeb.asmx" targetcontrolid="txtOtros" usecontextkey="True" minimumprefixlength="1" completioninterval="200" enablecaching="true" completionsetcount="0" completionlistcssclass="CompletionListCssClass" completionlistitemcssclass="CompletionListItemCssClass" completionlisthighlighteditemcssclass="CompletionListHighlightedItemCssClass" firstrowselected="true"
                        servicemethod="get_Usuarios_Aplicacion" onclientitemselected="Set_Otros"
                        onclientpopulating="Rellenando" onclientpopulated="Rellenado" onclienthiding="Rellenado" />
                </asp:Panel>
                <asp:ListView ID="lvOtros" runat="server" DataKeyNames="Id">
                    <LayoutTemplate>
                        <ul id="lvOtros_UL">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="hd_IdOtros" type="hidden" value='<%#Eval("Id")%>'><%#Eval("NombreCompleto")%>
                            <a href="#hd_IdOtros" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;">
                                <asp:Image ID="imgBorrar_Otros" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" />
                            </a>
                        </li>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <ul id="lvOtros_UL"></ul>
                    </EmptyDataTemplate>
                    <EmptyItemTemplate>
                        ??
                    </EmptyItemTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>

    <div style="text-align: center">
        <asp:Panel ID="pnlBotones" runat="server" CssClass="PanelBotones">
<%--            <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" OnClientClick="return checkCaracteristicaRotura();" />--%>
                <asp:ImageButton ID="btnAceptar" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" AlternateText="Aceptar" ToolTip="Aceptar" ValidationGroup="btnGuardar" />
        </asp:Panel>
    </div>

    <%--<act:PopupControlExtender ID="pce_pnl_tvEstadoProyecto" BehaviorID="pce_pnl_tvEstadoProyecto" runat="server" TargetControlID="img_tvEstadoProyecto" PopupControlID="pnl_tvEstadoProyecto" Position="Right" />
    <asp:Panel ID="pnl_tvEstadoProyecto" runat="server" CssClass="modalBox">
        <table>
            <tr>
                <td>
                    <asp:TreeView ID="tvEstadoProyecto" runat="server" SkinID="TreeView" ViewStateMode="Enabled"
                        ShowCheckBoxes="Leaf" onclick="return TreeView_ExclusiveCheckBox(event, this)" />
                </td>
            </tr>
        </table>
    </asp:Panel>--%>
    <act:popupcontrolextender id="pce_pnl_tvDeteccion" behaviorid="pce_pnl_tvDeteccion" runat="server" targetcontrolid="img_tvDeteccion" popupcontrolid="pnl_tvDeteccion" position="Right" />
    <asp:Panel ID="pnl_tvDeteccion" runat="server" CssClass="modalBox">
        <table>
            <tr>
                <td>
                    <asp:TreeView ID="tvDeteccion" runat="server" SkinID="TreeView" ViewStateMode="Enabled"
                        ShowCheckBoxes="Leaf" onclick="return TreeView_ExclusiveCheckBox_Deteccion(event, this)" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <act:popupcontrolextender id="pce_pnl_tvAreas" behaviorid="pce_pnl_tvAreas" runat="server" targetcontrolid="img_tvAreas" popupcontrolid="pnl_tvAreas" position="Right" />
    <asp:Panel ID="pnl_tvAreas" runat="server" CssClass="modalBox">
        <table>
            <tr>
                <td>
                    <asp:TreeView ID="tvAreas" runat="server" SkinID="TreeView" ViewStateMode="Enabled"
                        onclick="return tvAreas_SeleccionarNodo(event, this);" />
                    <%--onclick="return TreeView_ExclusiveCheckBox_tvAreas_onclick(event, this);" />--%>
                    <!--ShowCheckBoxes="Leaf" onclick="TreeView_ExclusiveCheckBox_tvAreas(event, this); Seleccionar_Procedencia(); return false;" />-->
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlMensaje_Adv" runat="server" CssClass="modalBox" Style="display: none;">
        <table style="border-collapse: collapse; margin: 5px;">
            <tr class="BarraTitulo">
                <th style="text-align: left">
                    <asp:Image ID="imgVentana" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon24.png" ImageAlign="Middle" />
                    <%--<asp:Label ID="Label102" runat="server" Text="?"></asp:Label>--%>
                </th>
                <th style="text-align: right">

                    <asp:Panel ID="Panel4" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgCancelar_Adv" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                    </asp:Panel>
                </th>
            </tr>
            <tr>
                <td colspan="2" class="MensajeError">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/Marvin.jpg" ToolTip="Don't Panic" /></td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblMensaje_Adv" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="BarraTitulo">
                <td style="text-align: center" colspan="2">
                    <asp:Panel ID="Panel9" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="btnAceptar_Adv" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div style="display: none;">
        <asp:TextBox ID="hf_pnlMensaje_Adv" runat="server"></asp:TextBox>
    </div>
    <act:modalpopupextender id="mpe_pnlMensaje_Adv" runat="server" targetcontrolid="hf_pnlMensaje_Adv" popupcontrolid="pnlMensaje_Adv" cancelcontrolid="imgCancelar_Adv" backgroundcssclass="modalBackground" />

    <!------------------------------------------------------------------------------------------------------>
    <!-- Selector de Producto -->
    <!------------------------------------------------------------------------------------------------------>
    <div style="display: none;">
        <asp:TextBox ID="hf_Producto" runat="server"></asp:TextBox>
    </div>
    <act:modalpopupextender id="mpe_pnlProducto" runat="server" targetcontrolid="hf_Producto" popupcontrolid="pnlProducto" cancelcontrolid="imgCancelar_Producto" backgroundcssclass="modalBackground">
    </act:modalpopupextender>

    <asp:Panel ID="pnlProducto" runat="server" CssClass="modalBox" Style="display: none;">
        <table style="border-collapse: collapse; margin: 5px;">
            <tr class="BarraTitulo">
                <th style="text-align: left">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon24.png" ImageAlign="Middle" />
                    <asp:Label ID="Label27" runat="server" Text="PRODUCTO CAUSA NO CONFORMIDAD"></asp:Label>
                </th>
                <th style="text-align: right">
                    <asp:Panel ID="pnlBotonesCabecera" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgCancelar_Producto" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                    </asp:Panel>
                </th>
            </tr>
            <tr>
                <td colspan="2">

                    <asp:ListView ID="lvProducto" runat="server" DataKeyNames="Id">
                        <LayoutTemplate>
                            <ul id="lvProducto_UL">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li>
                                <a href="#" onclick="Seleccionar_Producto(<%#Eval("Id")%>, '<%#Eval("Descripcion")%>')" class="ContactarCon"><%#Eval("Descripcion")%></a>
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <ul id="lvProducto_UL"></ul>
                        </EmptyDataTemplate>
                        <EmptyItemTemplate>
                            ??
                        </EmptyItemTemplate>
                    </asp:ListView>

                </td>
            </tr>
            <%--
                <act:ModalPopupExtender OkControlID="imgAceptar" />
                <tr class="BarraTitulo">
                <td style="text-align: center" colspan="2">
                    <asp:Panel ID="Panel1" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgAceptar" runat="server" AlternateText="Aceptar" ToolTip="Aceptar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Aceptar24.png" />
                    </asp:Panel>
                </td>
            </tr>--%>
        </table>
    </asp:Panel>
    <!------------------------------------------------------------------------------------------------------>

    <!------------------------------------------------------------------------------------------------------>
    <!-- Selector de Caracteristicas / Tipo Error -->
    <!------------------------------------------------------------------------------------------------------>
    <div style="display: none;">
        <asp:TextBox ID="hf_Caracteristica" runat="server"></asp:TextBox>
    </div>
    <act:modalpopupextender id="mpe_pnlCaracteristica" runat="server" targetcontrolid="hf_Caracteristica" popupcontrolid="pnlCaracteristica" cancelcontrolid="imgCancelar_Caracteristica" backgroundcssclass="modalBackground">
    </act:modalpopupextender>
    <asp:Panel ID="pnlCaracteristica" runat="server" CssClass="modalBox" Style="display: none;">
        <table style="border-collapse: collapse; margin: 5px;">
            <tr class="BarraTitulo">
                <th style="text-align: left">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon24.png" ImageAlign="Middle" />
                    <asp:Label ID="Label28" runat="server" Text="Caracteristicas / Tipo Error"></asp:Label>
                </th>
                <th style="text-align: right">
                    <asp:Panel ID="Panel2" runat="server" CssClass="PanelBotones">
                        <asp:ImageButton ID="imgCancelar_Caracteristica" runat="server" AlternateText="Cancelar" ToolTip="Cancelar" ImageUrl="~/App_Themes/Batz/IconosAcciones/Cancelar24.png" />
                    </asp:Panel>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ListView ID="lvCaracteristica" runat="server" DataKeyNames="Id">
                        <LayoutTemplate>
                            <ul id="lvCaracteristica_UL">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li>
                                <a href="#" onclick="Seleccionar_Caracteristica(<%#Eval("Id")%>, '<%#Eval("Descripcion")%>')" class="ContactarCon"><%#Eval("Descripcion")%></a>
                            </li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <ul id="lvCaracteristica_UL"></ul>
                        </EmptyDataTemplate>
                        <EmptyItemTemplate>
                            ??
                        </EmptyItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!------------------------------------------------------------------------------------------------------>

    <!------------------------------------------------------------------------------------------------------>
    <!-- Validacion de Campos ------------------------------------------------------------------------------>
    <!------------------------------------------------------------------------------------------------------>
    <!-- Fecha de Apertura Obligatorio --------------------------------------------------------------------->
    <asp:RequiredFieldValidator ID="rfvFechaApertura" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
    <act:validatorcalloutextender id="vce_rfvFechaApertura" runat="server" targetcontrolid="rfvFechaApertura" />
    <!------------------------------------------------------------------------------------------------------>
    <!-- Fecha de Apertura solo fecha ---------------------------------------------------------------------->
    <asp:CompareValidator ID="cvFechaApertura" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaApertura" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
    <act:validatorcalloutextender id="vce_cvFechaApertura" runat="server" targetcontrolid="cvFechaApertura" viewstatemode="Enabled" enableviewstate="true" />
    <!------------------------------------------------------------------------------------------------------>
    <!-- Fecha de Cierre solo fecha ------------------------------------------------------------------------>
    <asp:CompareValidator ID="cvFechaCierre" runat="server" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Type="Date" Operator="DataTypeCheck" Display="None" ValidationGroup="btnGuardar" />
    <act:validatorcalloutextender id="vce_cvFechaCierre" runat="server" targetcontrolid="cvFechaCierre" />
    <!------------------------------------------------------------------------------------------------------>
    <!-- FechaInicio <= FechaCierre ------------------------------------------------------------------------>
    <asp:CompareValidator ID="cvFechaCierre2" runat="server" Type="Date" ErrorMessage="fechaIncorrecta" ControlToValidate="txtFechaCierre" Operator="GreaterThanEqual" ControlToCompare="txtFechaApertura" Display="None" ValidationGroup="btnGuardar" />
    <act:validatorcalloutextender id="vce_cvFechaCierre2" runat="server" targetcontrolid="cvFechaCierre2" />
    <!------------------------------------------------------------------------------------------------------>
    <!-- Procedencia Obligatorio --------------------------------------------------------------------------->
    <asp:RequiredFieldValidator ID="rfv_ddlProcedencia" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="ddlProcedencia" Display="None" ValidationGroup="btnGuardar" SetFocusOnError="true"/>
    <act:validatorcalloutextender id="vce_rfv_ddlProcedencia" runat="server" targetcontrolid="rfv_ddlProcedencia" />
    <asp:RequiredFieldValidator ID="rfv_ddlPlantas" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="ddlPlantas" Display="None" ValidationGroup="btnGuardar" SetFocusOnError="true"
        Enabled="false" />
    <act:validatorcalloutextender id="vce_rfv_ddlPlantas" behaviorid="vce_rfv_ddlPlantas_BehaviorID" runat="server" targetcontrolid="rfv_ddlPlantas" />
    <asp:CustomValidator runat="server" ID="cv_tvAreas" ControlToValidate="ddlProcedencia" Display="None" ErrorMessage="Campo obligatorio" ValidationGroup="btnGuardar"
        ClientValidationFunction="ClientValidate_tvAreas" SetFocusOnError="true" Enabled="false" />
    <%--ClientValidationFunction="ClientValidate_TreeView" Enabled="false" />--%>

    <act:validatorcalloutextender id="vce_cv_tvAreas" behaviorid="vce_cv_tvAreas_BehaviorID" runat="server" targetcontrolid="cv_tvAreas" popupposition="TopRight" />
    <%--<asp:CustomValidator runat="server" ID="cv_hdIdProveedor" ControlToValidate="ddlProcedencia" Display="None" ErrorMessage="Campo obligatorio" ValidationGroup="btnGuardar"
        ClientValidationFunction="ValidarProveedor" Enabled="false" />
        <act:ValidatorCalloutExtender ID="vce_cv_hdIdProveedor" BehaviorID="vce_cv_hdIdProveedor_BehaviorID" runat="server" TargetControlID="cv_hdIdProveedor" PopupPosition="TopRight" />--%>

    <%--<asp:CustomValidator runat="server" ID="cv_ddlProveedor_OF" ControlToValidate="ddlProveedor_OF" Display="None" ErrorMessage="Campo obligatorio" ValidationGroup="btnGuardar"
        ClientValidationFunction="ValidarProveedor_OF" Enabled="true" />
    <act:ValidatorCalloutExtender ID="vce_cv_ddlProveedor_OF" BehaviorID="vce_cv_ddlProveedor_OF_BehaviorID" runat="server" TargetControlID="cv_ddlProveedor_OF" PopupPosition="TopRight" />--%>

    <%--<asp:RequiredFieldValidator ID="rfv_ddlProveedor_OF" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="ddlProveedor_OF" Display="None" ValidationGroup="btnGuardar"
        Enabled="false" />
    <act:ValidatorCalloutExtender ID="vce_rfv_ddlProveedor_OF" BehaviorID="rfv_ddlProveedor_OF_BehaviorID" runat="server" TargetControlID="rfv_ddlProveedor_OF" />--%>

    <asp:RequiredFieldValidator ID="rfv_ddlCapacidad" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="ddlCapacidad" Display="None" ValidationGroup="btnGuardar" SetFocusOnError="true"
        Enabled="false" />
    <act:validatorcalloutextender id="vce_rfv_ddlCapacidad" behaviorid="rfv_ddlCapacidad_BehaviorID" runat="server" targetcontrolid="rfv_ddlCapacidad" />
    <!------------------------------------------------------------------------------------------------------>

    <!-- Campos Obligatorios ------------------------------------------------------------------------------->
    <%--<asp:RequiredFieldValidator ID="rfv_txtNumPiezas" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="txtNumPiezas" Display="None" ValidationGroup="btnGuardar" />--%>
    <%--<act:ValidatorCalloutExtender ID="vce_rfv_txtNumPiezas" runat="server" TargetControlID="rfv_txtNumPiezas" />--%>
    <!------------------------------------------------------------------------------------------------------>
    <!-- Campo Numerico ------------------------------------------------------------------------------------>
    <asp:RegularExpressionValidator ID="rev_txtRetraso" ControlToValidate="txtRetraso" ValidationExpression="[0-9]*" Display="None" runat="server" ErrorMessage="advDebeSerNumerico" ValidationGroup="btnGuardar" SetFocusOnError="true"></asp:RegularExpressionValidator>
    <act:validatorcalloutextender id="vce_rev_txtRetraso" targetcontrolid="rev_txtRetraso" runat="server" />
    <!------------------------------------------------------------------------------------------------------>
    <!------------------------------------------------------------------------------------------------------>

    <%--<asp:HiddenField ID="hf_IdRecepcion" runat="server" />
    <asp:HiddenField ID="hf_Planta" runat="server" />--%>
    
    <asp:HiddenField ID="numberOfResponsibles" runat="server" Value="0" />


    
    <script src="../../Scripts/modal.js" language="javascript" type="text/javascript"></script>

    <script type="text/javascript">
        function AplicarCaracteristica(ID, Descripcion) {  
            if (ID == '' && Descripcion == '') {
                ID = $('#<%=hf_IDCARAC.ClientID%>').val();
                Descripcion = $('#<%=hf_DESCRIP.ClientID%>').val();
            }
            var hf_IdCaracteristica = $('#<%=hf_IdCaracteristica.ClientID%>');
            hf_IdCaracteristica.val(ID);
            hf_IdCaracteristica.next().remove();
            hf_IdCaracteristica.after('<ul><li>' + Descripcion + '</li></ul>');
        }
        function openModalRotura(ID, Descripcion) {                  
            var hf_IDCARAC = $('#<%=hf_IDCARAC.ClientID%>');
            hf_IDCARAC.val(ID);
            var hf_DESCRIP = $('#<%=hf_DESCRIP.ClientID%>');
            hf_DESCRIP.val(Descripcion);
            $('#Modal_Rotura').modal('show');
        }

        function mostrarCaracteristicas() {            
            $find('<%=mpe_pnlCaracteristica.ClientID%>').show();
        }
    </script>
    <div id="Modal_Rotura" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-title">
                    <asp:Image runat="server" ImageUrl="~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/warning-icon24.png" style="vertical-align:middle"/>
                    <asp:Label runat="server" Text="Atención!" style="vertical-align:middle;font-size:20px;font-variant:small-caps;font-weight:700"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="alert alert-success">
                        <asp:Label ID="mensajeRotura" runat="server"></asp:Label>
                    </div>
                    <asp:HiddenField ID="hf_IDCARAC" runat="server"/>
                    <asp:HiddenField ID="hf_DESCRIP" runat="server"/>
                </div>
                <div class="modal-footer">
                    <button type="button" runat="server" id="btnAceptarRotura" class="btn btn-success" data-dismiss="modal" onclick="AplicarCaracteristica('','')">
                        <asp:Label ID="Label29" runat="server" Text="Continuar" />
                    </button>
                    <button type="button" runat="server" id="btnCancelarRotura" class="btn btn-danger" data-dismiss="modal" onclick="mostrarCaracteristicas();">
                        <asp:Label ID="Label30" runat="server" Text="Cancelar" />
                    </button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
