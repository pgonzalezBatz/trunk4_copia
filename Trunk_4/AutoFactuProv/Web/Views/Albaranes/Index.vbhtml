@Imports AutoFactuProvLib

@code
    Dim seccion As Web.Configuration.HttpRuntimeSection = CType(ConfigurationManager.GetSection("system.web/httpRuntime"), Web.Configuration.HttpRuntimeSection)
End Code

<script type="text/javascript">
    $(function () {
        $('#submit').click(function () {
            var mensajeError = "";
            if($('#fuAdjunto').val() != ''){
                var fileSize = -1;
                try {
                    fileSize = $('#fuAdjunto')[0].files[0].size; // Para navegadores que soporten HTML5
                } catch (ex) {
                    var strFileName = $('#fuAdjunto').val();
                    var objFSO
                    try{
                        objFSO = new ActiveXObject("Scripting.FileSystemObject");
                    } catch (ex) {
                    }

                    if(objFSO)
                    {
                        var e = objFSO.getFile(strFileName);
                        fileSize = e.size;
                    }
                    else
                    {
                        return false;
                    }
                }

                mensajeError = "@Html.Raw(String.Format("{0}: {1}MB", Utils.Traducir("Tamaño máximo del fichero"), seccion.MaxRequestLength / 1024))";
                var maxRequestLength = @seccion.MaxRequestLength;

                if (fileSize / 1024 > maxRequestLength) {
                    alert(mensajeError);
                    return false;
                }
            }

            mensajeError = "@Html.Raw(Utils.Traducir("Debe seleccionar al menos un albarán"))";
            if( $('.chk:checked').length == 0){
                alert(mensajeError);
                return false;
            }

            return confirm("@Html.Raw(Utils.Traducir("Una vez enviada la factura no se podrá modificar. ¿Desea continuar?"))");
        });

        $(".chk").click(function(){
            if ($(this).is(':checked')) {
                var albaran = $(this).attr("albaran");
                $(".chk[albaran='" + albaran + "']").prop('checked', true);
            }

            var total = 0;
            $(".chk:checked").each(function(){
                total = total + parseFloat($(this).attr("precio"));
            });

            $("#txtTotal").val(parseFloat(total).toFixed(2));
        });
    });

</script>

@code
    Dim albaranes As IEnumerable(Of ELL.Albaran) = CType(ViewData("Albaranes"), IEnumerable(Of ELL.Albaran))
    If (ViewData("Tipo") = ELL.Albaran.ALBARAN_FACTURABLE) Then
        If (ViewData("Origen") = ELL.Albaran.OrigenAlbaran.Proveedor) Then
            @<h3>@Utils.Traducir("Albaranes facturables por proveedor")</h3>
            @<hr />
            @<div>@Utils.Traducir("AutoFactuProv_AvisoAlbaranesFacturables") @Html.ActionLink(Utils.Traducir("Pedidos sin recepcionar"), "Index", "Albaranes", New With {.Tipo = "P"}, Nothing) </div>
        ElseIf (ViewData("Origen") = ELL.Albaran.OrigenAlbaran.Batz) Then
            @<h3>@Utils.Traducir("Albaranes autofacturables por Batz")</h3>
            @<hr />
            @<div>@Utils.Traducir("AutoFactuProv_AvisoAlbaranesFacturables") @Html.ActionLink(Utils.Traducir("Pedidos sin recepcionar"), "Index", "Albaranes", New With {.Tipo = "P"}, Nothing) </div>
        End If
    ElseIf (ViewData("Tipo") = ELL.Albaran.PEDIDO_SIN_RECEPCIONAR) Then
        @<h3>@Utils.Traducir("Pedidos sin recepcionar")</h3>
        @<hr />
        @<div>@Utils.Traducir("AutoFactuProv_AvisoPedidosSinRecepcionar")</div>
    End If
    @<br />
End Code

@Using Html.BeginForm("Index", "Albaranes", FormMethod.Post, New With {.enctype = "multipart/form-data", .class = "form-horizontal"})
    If (albaranes.Count = 0) Then
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
    Else
        @<table class="table table-striped table-responsive table-hover table-condensed">
            <thead>
                <tr>
                    <th></th>
                    <th class="text-right">@Html.ActionLink(Utils.Traducir("Albaran"), "Index", "Albaranes", New With {.sortParameter = "Albaran", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                    @code
                        If (ViewData("Tipo") = ELL.Albaran.PEDIDO_SIN_RECEPCIONAR) Then
                            @<th>@Html.ActionLink(Utils.Traducir("Solicitante"), "Index", "Albaranes", New With {.sortParameter = "Solicitante", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                        End If
                    End Code
                    <th class="text-right">@Html.ActionLink(Utils.Traducir("Pedido"), "Index", "Albaranes", New With {.sortParameter = "Pedido", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                    <th class="text-right">@Html.ActionLink(Utils.Traducir("Linea"), "Index", "Albaranes", New With {.sortParameter = "Linea", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                    <th>@Html.ActionLink(Utils.Traducir("Ref.artículo"), "Index", "Albaranes", New With {.sortParameter = "RefArticulo", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                    <th>@Html.ActionLink(Utils.Traducir("Concepto"), "Index", "Albaranes", New With {.sortParameter = "Concepto", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                    @code
                        If (ViewData("Tipo") = ELL.Albaran.ALBARAN_FACTURABLE) Then
                            @<th class="text-right">@Html.ActionLink(Utils.Traducir("Cantidad recepcionada"), "Index", "Albaranes", New With {.sortParameter = "CantRecep", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                        ElseIf (ViewData("Tipo") = ELL.Albaran.PEDIDO_SIN_RECEPCIONAR)Then
                            @<th class="text-right">@Html.ActionLink(Utils.Traducir("Cant. pdte. recepcionar"), "Index", "Albaranes", New With {.sortParameter = "CantPendRecep", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                        End If
                    End Code
                    <th class="text-right">@Html.ActionLink(Utils.Traducir("Precio unitario"), "Index", "Albaranes", New With {.sortParameter = "Precio", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                    <th>@Html.ActionLink(Utils.Traducir("Moneda"), "Index", "Albaranes", New With {.sortParameter = "Moneda", .sortOrder = ViewData("SortOrder"), .Tipo = ViewData("Tipo"), .Origen = CInt(ViewData("Origen"))}, Nothing)</th>
                </tr>
            </thead>
            <tbody>
                @code
                    Dim albaran As String = String.Empty
                End Code

                @For indice = 0 To albaranes.Count - 1
                    @<tr>
                        <td align="center">
                            @code
                                If (ViewData("Tipo") = ELL.Albaran.ALBARAN_FACTURABLE AndAlso ViewData("Origen") = ELL.Albaran.OrigenAlbaran.Proveedor) Then
                                    @Html.CheckBox("albaranExt[" & indice & "].Checked", New With {.class = "chk", .albaran = albaranes(indice).Albaran, .precio = albaranes(indice).PrecioTotalRecibidos})
                                    @Html.Hidden("albaranExt[" & indice & "].Albaran", albaranes(indice).Albaran)
                                    @Html.Hidden("albaranExt[" & indice & "].Pedido", albaranes(indice).Pedido)
                                    @Html.Hidden("albaranExt[" & indice & "].Linea", albaranes(indice).Linea)
                                @*Else
                                    @Html.CheckBox("albaranExt[" & indice & "].Checked", New With {.disabled = "disabled"})*@
                            End If
                            End Code
                        </td>
                        <td class="text-right">@Html.Label(albaranes(indice).Albaran)</td>
                        @code
                            If (ViewData("Tipo") = ELL.Albaran.PEDIDO_SIN_RECEPCIONAR) Then
                                @<td>@albaranes(indice).Solicitante</td>
                            End If
                        End Code
                        <td class="text-right">@albaranes(indice).Pedido</td>
                        <td class="text-right"> @albaranes(indice).Linea</td>
                         <td>@albaranes(indice).RefArticulo</td>
                        <td>@albaranes(indice).Concepto</td>
                        @code
                            If (ViewData("Tipo") = ELL.Albaran.ALBARAN_FACTURABLE) Then
                                @<td class="text-right">@albaranes(indice).CantRecibida</td>
                            ElseIf (ViewData("Tipo") = ELL.Albaran.PEDIDO_SIN_RECEPCIONAR)
                                @<td class="text-right">@albaranes(indice).CantPendiente</td>
                            End If
                        End Code
                        <td class="text-right">@albaranes(indice).PrecioUnitario</td>
                        <td>@albaranes(indice).Moneda</td>
                    </tr>
                            Next
            </tbody>
        </table>


            @code
                If (ViewData("Tipo") = ELL.Albaran.ALBARAN_FACTURABLE AndAlso ViewData("Origen") = ELL.Albaran.OrigenAlbaran.Proveedor) Then
                    @<hr/>
                    @<div class="panel panel-default">
                         <div Class="panel-body">
                             <div Class="form-group">
                                 <label class="col-sm-2 control-label">@Utils.Traducir("Total")</label>
                                 <div class="col-sm-2">
                                     @Html.TextBox("txtTotal", "0.00", New With {.class = "form-control text-success", .disabled = "disabled"})
                                 </div>
                                 <div class="col-sm-1 control-label" style="text-align:left;">
                                     @code
                                         If (albaranes IsNot Nothing AndAlso albaranes.Count > 0) Then
                     @<label>@albaranes(0).Moneda</label>
                                         Else
                     @<label>EUR</label>
                                         End If
                                     end code
                                 </div>
                             </div>
                             <div Class="form-group">
                                 <label class="col-sm-2 control-label">@Utils.Traducir("Factura")</label>
                                 <div class="col-sm-2">
                                     @Html.TextBox("txtFactura", Nothing, New With {.required = "required", .maxlength = "20", .class = "form-control"})
                                 </div>
                             </div>
                             <div Class="form-group">
                                 <label class="col-sm-2 control-label">@Utils.Traducir("Adjunto")</label>
                                 <div class="col-sm-6">
                                     @Html.TextBox("fuAdjunto", Nothing, New With {.type = "file", .required = "required", .class = "form-control"})
                                 </div>
                             </div>
                             <div Class="form-group">
                                 <div class="col-sm-offset-2 col-sm-6">
                                     @Html.Label(String.Format("{0}: {1} MB", Utils.Traducir("Tamaño máximo del fichero"), seccion.MaxRequestLength / 1024), New With {.class = "label label-info"})
                                 </div>
                             </div>
                             <div Class="form-group">
                                 <div class="col-sm-offset-2 col-sm-6">
                                     <input type="submit" id="submit" value="@Utils.Traducir("Enviar")" class="btn btn-primary input-block-level form-control" />
                                 </div>
                             </div>
                         </div>
            </div>
                                         End If
End Code
                                             End if
                                         End Using
