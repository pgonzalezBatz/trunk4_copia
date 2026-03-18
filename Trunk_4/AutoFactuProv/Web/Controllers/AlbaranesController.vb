Imports System.Web.Mvc

Namespace Controllers
    Public Class AlbaranesController
        Inherits BaseController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sortOrder"></param> 
        ''' <returns></returns>
        Function Index(ByVal sortOrder As String, ByVal sortParameter As String) As ActionResult
            ViewData("SortOrder") = "asc"
            CargarAlbaranes(sortOrder, sortParameter)

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="albaranExt"></param>
        ''' <param name="txtFactura"></param>
        ''' <param name="fuAdjunto"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Index(ByVal albaranExt As List(Of AlbaranExt), ByVal txtFactura As String, ByVal fuAdjunto As HttpPostedFileBase) As ActionResult
            If (Not albaranExt.Exists(Function(f) f.Checked) _
                    OrElse String.IsNullOrEmpty(txtFactura) _
                    OrElse fuAdjunto Is Nothing _
                    OrElse (fuAdjunto IsNot Nothing AndAlso fuAdjunto.InputStream Is Nothing)) Then
                If (Not albaranExt.Exists(Function(f) f.Checked)) Then
                    ModelState.AddModelError("albaranExt", "Debe seleccionar al menos un albarán")
                    MensajeAlerta(Utils.Traducir("Debe seleccionar al menos un albarán"))
                End If

                If (String.IsNullOrEmpty(txtFactura)) Then
                    ModelState.AddModelError("txtFactura", "Factura es un campo obligatorio")
                    MensajeAlerta(Utils.Traducir("Factura es un campo obligatorio"))
                End If

                If (fuAdjunto Is Nothing _
                    OrElse (fuAdjunto IsNot Nothing AndAlso fuAdjunto.InputStream Is Nothing)) Then
                    ModelState.AddModelError("fuAdjunto", "Adjunto es un campo obligatorio")
                    MensajeAlerta(Utils.Traducir("Adjunto es un campo obligatorio"))
                End If

                'Cargamos los albaranes pendientes de facturar y los pedidos pendientes de recepcionar
                CargarAlbaranes()
                Return View()
            End If

            ' Recogemos los datos del usuario
            Dim buffer() As Byte = Nothing
            Dim nombreFichero As String = Nothing
            If (fuAdjunto IsNot Nothing AndAlso fuAdjunto.InputStream IsNot Nothing) Then
                Using binaryReader As New IO.BinaryReader(fuAdjunto.InputStream)
                    buffer = binaryReader.ReadBytes(fuAdjunto.ContentLength)
                End Using
                nombreFichero = IO.Path.GetFileName(fuAdjunto.FileName)
            End If
            Dim facturaProv As New ELL.FacturaProv With {.NumFactura = txtFactura, .Proveedor = TicketExt.Proveedor, .Empresa = TicketExt.Empresa, .Planta = TicketExt.Planta}

            Dim lineasfacturaProv As New List(Of ELL.LineasFacturaProv)
            For Each albaran In albaranExt.Where(Function(f) f.Checked)
                lineasfacturaProv.Add(New ELL.LineasFacturaProv With {.Albaran = albaran.Albaran, .Pedido = albaran.Pedido, .Linea = albaran.Linea})
            Next

            ' Guardamos la factura
            Try
                BLL.FacturasProvBLL.Guardar(facturaProv, lineasfacturaProv, buffer, nombreFichero)

                If (buffer Is Nothing) Then
                    MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Else
                    MensajeInfo(Utils.Traducir("Datos guardados correctamente") & ". " & String.Format("{0} {1}", Utils.Traducir("Factura enviada a"), ConfigurationManager.AppSettings("buzonFacturasSistemas")))
                End If

                Return RedirectToAction("Index")
            Catch ex As Exception
                MensajeInfo(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return View()
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sortOrder"></param>
        ''' <param name="sortParameter"></param> 
        Private Sub CargarAlbaranes(Optional ByVal sortOrder As String = Nothing, Optional ByVal sortParameter As String = Nothing)
            ' Dependiendo del modo cargamos albaranes pendientes de facturas o pedidos
            Dim tipo As String = ELL.Albaran.ALBARAN_FACTURABLE
            If (Request.QueryString.AllKeys.Contains("Tipo")) Then
                tipo = Request.QueryString("Tipo")
            End If

            Dim origen As Integer = ELL.Albaran.OrigenAlbaran.Proveedor
            If (Request.QueryString.AllKeys.Contains("Origen")) Then
                origen = CInt(Request.QueryString("Origen"))
            End If

            Dim albaranesAux As New List(Of ELL.Albaran)()

            If (tipo = ELL.Albaran.ALBARAN_FACTURABLE) Then
                If (origen = ELL.Albaran.OrigenAlbaran.Proveedor) Then
                    albaranesAux = BLL.AlbaranesBLL.CargarListadoFacturablesProveedor(TicketExt.Proveedor, TicketExt.Empresa, TicketExt.Planta)
                ElseIf (origen = ELL.Albaran.OrigenAlbaran.Batz) Then
                    albaranesAux = BLL.AlbaranesBLL.CargarListadoFacturablesBatz(TicketExt.Proveedor, TicketExt.Empresa, TicketExt.Planta)
                End If
            ElseIf (tipo = ELL.Albaran.PEDIDO_SIN_RECEPCIONAR) Then
                albaranesAux = BLL.AlbaranesBLL.CargarListadoPendRecep(TicketExt.Proveedor, TicketExt.Empresa, TicketExt.Planta)
            End If

            Dim albaranes As New List(Of ELL.Albaran)
            If (tipo = ELL.Albaran.ALBARAN_FACTURABLE) Then
                ' Tenemos que coger de la lista los albaranes que ya no estén guardados como facturados
                Dim lineasFacturaProv As List(Of ELL.LineasFacturaProv) = BLL.LineasFacturaProvBLL.CargarListado(TicketExt.Proveedor, TicketExt.Empresa, TicketExt.Planta)
                For Each albaran In albaranesAux
                    If (Not lineasFacturaProv.Exists(Function(f) f.Albaran = albaran.Albaran AndAlso f.Pedido = albaran.Pedido AndAlso f.Linea = albaran.Linea)) Then
                        albaranes.Add(albaran)
                    End If
                Next
            Else
                albaranes = albaranesAux
            End If

            If (sortOrder IsNot Nothing AndAlso sortParameter IsNot Nothing) Then
                Select Case sortParameter
                    Case "Albaran"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ToList
                        Else
                            albaranes = albaranes.OrderByDescending(Function(f) f.Albaran).ToList
                        End If
                    Case "Solicitante"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.Solicitante).ToList
                        Else
                            albaranes = albaranes.OrderByDescending(Function(f) f.Albaran).ThenByDescending(Function(f) f.Solicitante).ToList
                        End If
                    Case "Pedido"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.Pedido).ToList
                        Else
                            albaranes = albaranes.OrderByDescending(Function(f) f.Albaran).ThenByDescending(Function(f) f.Pedido).ToList
                        End If
                    Case "Linea"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.Linea).ToList
                        Else
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenByDescending(Function(f) f.Linea).ToList
                        End If
                    Case "RefArticulo"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.RefArticulo).ToList
                        Else
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenByDescending(Function(f) f.RefArticulo).ToList
                        End If
                    Case "Concepto"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.Concepto).ToList
                        Else
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenByDescending(Function(f) f.Concepto).ToList
                        End If
                    Case "CantRecep"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.CantRecibida).ToList
                        Else
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenByDescending(Function(f) f.CantRecibida).ToList
                        End If
                    Case "CantPendRecep"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.CantPendiente).ToList
                        Else
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenByDescending(Function(f) f.CantPendiente).ToList
                        End If
                    Case "Precio"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.PrecioUnitario).ToList
                        Else
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenByDescending(Function(f) f.PrecioUnitario).ToList
                        End If
                    Case "Moneda"
                        If (sortOrder = "asc") Then
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenBy(Function(f) f.Moneda).ToList
                        Else
                            albaranes = albaranes.OrderBy(Function(f) f.Albaran).ThenByDescending(Function(f) f.Moneda).ToList
                        End If
                End Select

                If (sortOrder = "asc") Then
                    ViewData("SortOrder") = "desc"
                Else
                    ViewData("SortOrder") = "asc"
                End If
            End If

            ViewData("Albaranes") = albaranes
            ViewData("Tipo") = tipo
            ViewData("Origen") = origen
        End Sub

#End Region

    End Class
End Namespace