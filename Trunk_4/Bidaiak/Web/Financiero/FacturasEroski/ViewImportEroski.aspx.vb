Public Class ViewImportEroski
    Inherits PageBase

    Private idUserActual As Integer = 0
    Private CtaIvaNormal As String = String.Empty
    Private CtaIvaReducido As String = String.Empty
    Private CtaIvaExento As String = String.Empty
    Private Departamento As String = String.Empty
    Private hViajes As Hashtable = Nothing

#Region "Page Load"

    ''' <summary>
    ''' Se carga el contenido del control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Resumen de la importacion de facturas Eroski"
            hfIdImportacion.Value = Request.QueryString("id")
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim oImp As BLL.BidaiakBLL.Importacion = bidaiakBLL.loadImportacionDoc(CInt(hfIdImportacion.Value))
            lblFechaFichero.Text = TraducirMes(oImp.Mes) & "/" & oImp.Anno
            mostrarDatos()
        End If
    End Sub

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelFecha) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelInfo)
        End If
    End Sub

    ''' <summary>
    ''' Muestra los datos correspondientes
    ''' </summary>    
    Private Sub mostrarDatos()
        If (tabPaneles.ActiveTabIndex = 0) Then 'Asientos
            resumen.Iniciar(CInt(hfIdImportacion.Value))
        Else
            Try
                Dim agenciaBLL As New BLL.SolicAgenciasBLL
                Dim lAlbaranes As List(Of ELL.FakturaEroski) = agenciaBLL.loadFacturasEroski(CInt(hfIdImportacion.Value))
                If (lAlbaranes IsNot Nothing AndAlso lAlbaranes.Count > 0) Then lAlbaranes = lAlbaranes.OrderBy(Function(o) o.Factura).ThenBy(Function(p) p.Persona).ToList
                idUserActual = 0 : CtaIvaNormal = String.Empty : CtaIvaReducido = String.Empty : CtaIvaExento = String.Empty : Departamento = String.Empty
                hViajes = New Hashtable
                gvAlbaranes.DataSource = lAlbaranes
                gvAlbaranes.DataBind()
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al mostrar los albaranes de Eroski", ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAlbaranes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAlbaranes.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oAlbar As ELL.FakturaEroski = e.Row.DataItem
            Dim idUser As Integer = oAlbar.IdUser
            CType(e.Row.FindControl("lblFactura"), Label).Text = oAlbar.Factura
            CType(e.Row.FindControl("lblAlbaran"), Label).Text = oAlbar.Albaran
            CType(e.Row.FindControl("lblFecha"), Label).Text = oAlbar.FechaServicio.ToShortDateString
            CType(e.Row.FindControl("lblProducto"), Label).Text = oAlbar.Producto
            CType(e.Row.FindControl("lblDestino"), Label).Text = oAlbar.Destino
            CType(e.Row.FindControl("lblProveedor"), Label).Text = oAlbar.Proveedor
            CType(e.Row.FindControl("lblPersona"), Label).Text = oAlbar.Persona
            CType(e.Row.FindControl("lblBaseIG"), Label).Text = oAlbar.BaseIG
            CType(e.Row.FindControl("lblCuotaG"), Label).Text = oAlbar.CuotaG
            CType(e.Row.FindControl("lblBaseIR"), Label).Text = oAlbar.BaseIR
            CType(e.Row.FindControl("lblCuotaR"), Label).Text = oAlbar.CuotaR
            CType(e.Row.FindControl("lblBaseExe"), Label).Text = oAlbar.BaseExe
            CType(e.Row.FindControl("lblRegEsp"), Label).Text = oAlbar.RegEsp
            CType(e.Row.FindControl("lblCuotaRE"), Label).Text = oAlbar.CuotaRE
            CType(e.Row.FindControl("lblImporte"), Label).Text = oAlbar.Importe
            CType(e.Row.FindControl("lblTasas"), Label).Text = oAlbar.Tasas
            'If (idUser <> idUserActual) Then
            Try
                Dim departBLL As New BLL.DepartamentosBLL
                Dim fechaConsulta As Date = oAlbar.FechaServicio
                If (oAlbar.IdViajes <> String.Empty) Then
                    If (hViajes.ContainsKey(CInt(oAlbar.IdViajes))) Then
                        fechaConsulta = CDate(hViajes.Item(CInt(oAlbar.IdViajes)))
                    Else
                        Dim viajesBLL As New BLL.ViajesBLL
                        Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(CInt(oAlbar.IdViajes), bSoloCabecera:=True)
                        fechaConsulta = oViaje.FechaSolicitud
                        hViajes.Add(oViaje.IdViaje, oViaje.FechaSolicitud)
                    End If
                End If
                Dim oDepart As ELL.Departamento = departBLL.loadInfoCuentaPlantaActiva(New SabLib.ELL.Usuario With {.Id = idUser}, fechaConsulta, Master.IdPlantaGestion)
                If (oDepart IsNot Nothing) Then
                    CtaIvaNormal = oDepart.Cuenta18 : CtaIvaReducido = oDepart.Cuenta8 : CtaIvaExento = oDepart.Cuenta0
                    Departamento = oDepart.Departamento
                Else
                    CtaIvaNormal = String.Empty : CtaIvaReducido = String.Empty : CtaIvaExento = String.Empty : Departamento = String.Empty
                End If
            Catch batzEx As BatzException
                CtaIvaNormal = String.Empty : CtaIvaReducido = String.Empty : CtaIvaExento = String.Empty : Departamento = String.Empty
            End Try
            '    idUserActual = idUser
            'End If
            CType(e.Row.FindControl("lblCtaIvaNormal"), Label).Text = CtaIvaNormal
            CType(e.Row.FindControl("lblCtaIvaReducido"), Label).Text = CtaIvaReducido
            CType(e.Row.FindControl("lblCtaIvaExento"), Label).Text = CtaIvaExento
            CType(e.Row.FindControl("lblDpto"), Label).Text = Departamento
        End If
    End Sub

    ''' <summary>
    ''' Se muestran los datos de la pestaña clickeada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub tabPaneles_ActiveTabChanged(sender As Object, e As EventArgs) Handles tabPaneles.ActiveTabChanged
        Try
            mostrarDatos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' El control ha generado un error
    ''' </summary>
    ''' <param name="mensaje"></param>    
    Private Sub resumen_ErrorGenerado(mensaje As String) Handles resumen.ErrorGenerado
        Master.MensajeError = mensaje
    End Sub

    ''' <summary>
    ''' Vuelve al listado de importaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("ImportacionesEroski.aspx")
    End Sub

    ''' <summary>
    ''' Traduce el mes
    ''' </summary>
    ''' <param name="mes">Numero del mes</param>
    ''' <returns></returns>    
    Private Function TraducirMes(mes As Integer) As String
        Select Case mes
            Case 1 : Return itzultzaileWeb.Itzuli("ene")
            Case 2 : Return itzultzaileWeb.Itzuli("feb")
            Case 3 : Return itzultzaileWeb.Itzuli("mar")
            Case 4 : Return itzultzaileWeb.Itzuli("abr")
            Case 5 : Return itzultzaileWeb.Itzuli("may")
            Case 6 : Return itzultzaileWeb.Itzuli("jun")
            Case 7 : Return itzultzaileWeb.Itzuli("jul")
            Case 8 : Return itzultzaileWeb.Itzuli("ago")
            Case 9 : Return itzultzaileWeb.Itzuli("sep")
            Case 10 : Return itzultzaileWeb.Itzuli("oct")
            Case 11 : Return itzultzaileWeb.Itzuli("nov")
            Case 12 : Return itzultzaileWeb.Itzuli("dic")
            Case Else : Return String.Empty
        End Select
    End Function

#End Region

End Class