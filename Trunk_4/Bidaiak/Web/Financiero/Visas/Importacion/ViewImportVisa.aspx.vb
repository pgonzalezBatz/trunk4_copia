Public Class ViewImportVisa
    Inherits PageBase

    Private idUserActual As Integer = 0
    Private CtaContable As String = String.Empty
    Private CtaCuotas As String = String.Empty
    Private Departamento As String = String.Empty
    Private hViajes As Hashtable = Nothing

#Region "Page Load"

    ''' <summary>
    ''' Se carga el contenido del control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Resumen de la importacion de visas"
                hfIdImportacion.Value = Request.QueryString("id")
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim oImp As BLL.BidaiakBLL.Importacion = bidaiakBLL.loadImportacionDoc(CInt(hfIdImportacion.Value))
                lblFechaFichero.Text = TraducirMes(oImp.Mes) & "/" & oImp.Anno
                mostrarDatos()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelFecha) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelInfo)
            itzultzaileWeb.Itzuli(labelMovVisa) : itzultzaileWeb.Itzuli(labelMovVisaExcep)
        End If
    End Sub

    ''' <summary>
    ''' Muestra los datos correspondientes
    ''' </summary>    
    Private Sub mostrarDatos()
        If (tabPaneles.ActiveTabIndex = 0) Then 'Asientos
            resumen.Iniciar(CInt(hfIdImportacion.Value))
        Else 'Movimientos o Movimientos agrupados por persona
            Try
                Dim movBLL As New BLL.VisasBLL
                Dim xbatBLL As New BLL.XbatBLL
                Dim bidaiBLL As New BLL.BidaiakBLL
                Dim viajesBLL As New BLL.ViajesBLL
                Dim lMovs As List(Of ELL.Visa.Movimiento) = movBLL.loadMovimientos(New ELL.Visa.Movimiento With {.IdImportacion = CInt(hfIdImportacion.Value)}, Master.IdPlantaGestion, tipoMov:=-1)
                lMovs = lMovs.OrderBy(Function(o) o.NombreUsuario).ToList
                idUserActual = 0 : Departamento = String.Empty : CtaContable = String.Empty
                If (tabPaneles.ActiveTabIndex = 1) Then
                    Dim oCta As ELL.CuentaContrapartida = bidaiBLL.loadCuentaContrapartida(Master.IdPlantaGestion, Master.IdPlantaGestion)
                    CtaCuotas = oCta.CtaCuota
                    hViajes = New Hashtable
                    gvMovimientos.DataSource = lMovs
                    gvMovimientos.DataBind()
                    'Se miran las visas de excepcion
                    lMovs = movBLL.loadMovimientosExcepcion(New ELL.Visa.Movimiento With {.IdImportacion = CInt(hfIdImportacion.Value)}, Master.IdPlantaGestion)
                    gvMovimientosExcep.DataSource = lMovs
                    gvMovimientosExcep.DataBind()
                Else
                    Dim lMovsAgrup = lMovs.GroupBy(Function(x) New With {Key x.NombreUsuario}).Select(Function(group) New With {.NombreUsuario = group.Key.NombreUsuario, .Items = group.ToList}).OrderBy(Function(z) z.NombreUsuario)
                    gvImportesPersona.DataSource = lMovsAgrup
                    gvImportesPersona.DataBind()
                End If
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al mostrar los movimientos de visa", ex)
            End Try
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
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvMovimientos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMovimientos.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMov As ELL.Visa.Movimiento = e.Row.DataItem
            Dim lblCtaContable As Label = CType(e.Row.FindControl("lblCtaContable"), Label)
            Dim lblDpto As Label = CType(e.Row.FindControl("lblDpto"), Label)
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oMov.ImporteEuros & " " & oMov.Moneda.Abreviatura
            CType(e.Row.FindControl("lblImporteMonGasto"), Label).Text = oMov.ImporteMonedaGasto & " " & oMov.MonedaGasto.Abreviatura
            'If (oMov.IdUsuario = idUserActual) Then
            '    lblCtaContable.Text = If(oMov.Tipo = 0, CtaContable, CtaCuotas)
            '    lblDpto.Text = Departamento
            'Else
            'Segun la fecha, pueden tener diferentes departamentos.
            '090721: Si estuviera asociado a un viaje, tomará en cuenta la fecha de creacion del viaje para asignarle el departamento
            Dim departBLL As New BLL.DepartamentosBLL
            Dim fechaConsulta As Date = oMov.Fecha
            If (oMov.IdViaje > 0) Then
                If (hViajes.ContainsKey(oMov.IdViaje)) Then
                    fechaConsulta = CDate(hViajes.Item(oMov.IdViaje))
                Else
                    Dim viajesBLL As New BLL.ViajesBLL
                    Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(oMov.IdViaje, bSoloCabecera:=True)
                    fechaConsulta = oViaje.FechaSolicitud
                    hViajes.Add(oViaje.IdViaje, oViaje.FechaSolicitud)
                End If
            End If
            Dim oDepart As ELL.Departamento = departBLL.loadInfoCuentaPlantaActiva(New SabLib.ELL.Usuario With {.Id = oMov.IdUsuario}, fechaConsulta, Master.IdPlantaGestion)
            If (oDepart IsNot Nothing) Then
                CtaContable = If(oMov.Tipo = 0, oDepart.Cuenta0, CtaCuotas)
                Departamento = oDepart.Departamento
                lblCtaContable.Text = CtaContable
                lblDpto.Text = Departamento
            Else
                CtaContable = String.Empty : Departamento = String.Empty
            End If
            'idUserActual = oMov.IdUsuario
            'End If
            If (oMov.IdViaje > 0) Then CType(e.Row.FindControl("lblViaje"), Label).Text = oMov.IdViaje
            If (oMov.Tipo = 1) Then e.Row.CssClass = "info"
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos de las visas de excepcion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvMovimientosExcep_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMovimientosExcep.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMov As ELL.Visa.Movimiento = e.Row.DataItem
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oMov.ImporteEuros & " " & oMov.Moneda.Abreviatura
            CType(e.Row.FindControl("lblImporteMonGasto"), Label).Text = oMov.ImporteMonedaGasto & " " & oMov.MonedaGasto.Abreviatura
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvImportesPersona_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvImportesPersona.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMov = e.Row.DataItem
            CType(e.Row.FindControl("lblPersona"), Label).Text = oMov.NombreUsuario
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = CType(oMov.Items, List(Of ELL.Visa.Movimiento)).Sum(Function(o As ELL.Visa.Movimiento) o.ImporteEuros)
        End If
    End Sub

    ''' <summary>
    ''' Vuelve al listado de importaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("ImportacionesVisas.aspx")
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