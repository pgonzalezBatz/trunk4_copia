Imports BidaiakLib.ELL
Imports SabLib.ELL

Public Class Alertas
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Comprueba si tiene alertas a mostrar
    ''' 1: Hojas de gastos sin validar    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Alertas"
                Dim lAlertas As New List(Of String())
                Dim numAlertas, numAux As Integer
                numAlertas = 0 : numAux = 0
                Dim lHGReimprimir As List(Of String()) = AlertasReimpresionHG()
                If (lHGReimprimir IsNot Nothing AndAlso lHGReimprimir.Count > 0) Then
                    numAlertas += lHGReimprimir.Count
                    For Each sHG As String() In lHGReimprimir
                        lAlertas.Add(New String() {"HG_IMP", itzultzaileWeb.Itzuli("Debido a la insercion de nuevos movimientos de visa, debe imprimir la hoja de gastos del viaje " & sHG(0)), 0, "~/Viaje/HojaGastos.aspx?IdViaje=" & sHG(0) & "&IdUser=" & sHG(1) & "&orig=ALER"})
                    Next
                End If
                numAux += AlertasHojasGastos()
                If (numAux > 0) Then lAlertas.Add(New String() {"HG", itzultzaileWeb.Itzuli("Validar hojas de gastos"), numAux, "~/Validacion/ValHojasGastos.aspx"})
                numAlertas += numAux
                numAux = AlertasGastosVISA()
                If (numAux > 0) Then lAlertas.Add(New String() {"GV", itzultzaileWeb.Itzuli("Validar gastos de visa"), numAux, "~/Validacion/ValGastosVisa.aspx"})
                numAlertas += numAux
                numAux = AlertasViajes()
                If (numAux > 0) Then lAlertas.Add(New String() {"VI", itzultzaileWeb.Itzuli("Validar viajes"), numAux, "~/Validacion/ValViajes.aspx"})
                numAlertas += numAux
                If (Session("ValPresup") IsNot Nothing) Then
                    numAux = AlertasPresupuestos()
                    If (numAux > 0) Then lAlertas.Add(New String() {"PR", itzultzaileWeb.Itzuli("Validar presupuestos"), numAux, "~/Publico/Presupuestos/Presupuestos.aspx"})
                    numAlertas += numAux
                End If
                If (Session("Gerente") IsNot Nothing) Then
                    numAux = AlertasSolicitudViaje()
                    If (numAux > 0) Then lAlertas.Add(New String() {"SV", itzultzaileWeb.Itzuli("Validar solicitud de viaje"), numAux, "~/Publico/SolPlantaFilial/SolicitudPlantaFilial.aspx"})
                    numAlertas += numAux
                End If
                If (numAlertas = 0) Then
                    Response.Redirect("~/Viaje/Viajes.aspx", False)
                Else
                    rptAlertas.DataSource = lAlertas
                    rptAlertas.DataBind()
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar las alertas")
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelExistAcc)
        End If
    End Sub

#End Region

#Region "Comprueba si tiene alertas"

    ''' <summary>
    ''' Comprueba si tiene hojas de gastos que se tienen que reimprimir
    ''' </summary>
    Private Function AlertasReimpresionHG() As List(Of String())
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Return bidaiakBLL.loadListHGReimprimir(Master.Ticket.IdUser)
    End Function

    ''' <summary>
    ''' Comprueba si tiene hojas de gastos sin validar en estado enviado en las que sea validador
    ''' </summary>
    ''' <returns></returns>    
    Private Function AlertasHojasGastos() As Integer
        Dim hojasBLL As New BLL.HojasGastosBLL
        Dim lHojas As List(Of ELL.HojaGastos) = hojasBLL.loadHojas(Master.IdPlantaGestion, Master.Ticket.IdUser, Integer.MinValue, Date.MinValue, Date.MinValue, False, False, False, False)
        lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Estado = ELL.HojaGastos.eEstado.Enviada AndAlso (o.Validador.Id = Master.Ticket.IdUser))
        Return lHojas.Count
    End Function

    ''' <summary>
    ''' Comprueba si tiene movimientos de Visa sin validar ya justificados
    ''' </summary>
    ''' <returns></returns>    
    Private Function AlertasGastosVISA() As Integer
        Dim visaBLL As New BLL.VisasBLL
        Dim lMovVisa As List(Of ELL.Visa.Movimiento) = visaBLL.loadMovimientos(Master.Ticket.IdUser, Integer.MinValue, Master.IdPlantaGestion, bSinValidar:=True)
        lMovVisa = lMovVisa.FindAll(Function(o As ELL.Visa.Movimiento) (o.Estado = ELL.Visa.Movimiento.eEstado.Justificado) AndAlso o.IdResponsable = Master.Ticket.IdUser)
        Return lMovVisa.Count
    End Function

    ''' <summary>
    ''' Comprueba si tiene viajes sin validar
    ''' </summary>
    ''' <returns></returns>    
    Private Function AlertasViajes() As Integer
        Dim lViajes As New List(Of ELL.Viaje)
        Dim lIdUsersVal As List(Of String) = ConfigurationManager.AppSettings("validadoresViajes").ToString.Split(";").ToList
        If (lIdUsersVal.Exists(Function(o) o = Master.Ticket.IdUser)) Then
            Dim viaBLL As New BLL.ViajesBLL
            lViajes = viaBLL.loadListViajes(New ELL.Viaje With {.IdPlanta = Master.IdPlantaGestion, .Estado = ELL.Viaje.eEstadoViaje.Pendiente_validacion}, True, Master.IdPlantaGestion, bSoloCabeceras:=True)
        End If
        Return lViajes.Count
    End Function

    ''' <summary>
    ''' Comprueba si tiene presupuestos sin validar
    ''' </summary>
    ''' <returns></returns>    
    Private Function AlertasPresupuestos() As Integer
        Dim presupBLL As New BLL.PresupuestosBLL
        Dim lPresup As List(Of String()) = presupBLL.loadPresupuestos(Master.Ticket.IdUser, Date.MinValue, Date.MinValue, ELL.Presupuesto.EstadoPresup.Enviado)
        Return If(lPresup Is Nothing, 0, lPresup.Count)
    End Function

    ''' <summary>
    ''' Comprueba si tiene alguna solicitud de viaje pendiente de validar
    ''' </summary>
    ''' <returns></returns>    
    Private Function AlertasSolicitudViaje() As Integer
        Dim solicitBLL As New BLL.ViajesBLL
        Dim lSolicitudes As List(Of String()) = solicitBLL.loadSolPlantasFiliales(Master.Ticket.IdUser, Date.MinValue, Date.MinValue, ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado)
        Return If(lSolicitudes Is Nothing, 0, lSolicitudes.Count)
    End Function

#End Region

#Region "Evento repeater"

    ''' <summary>
    ''' Enlace con las alertas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptAlertas_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptAlertas.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim alerta As String() = e.Item.DataItem
            Dim lnkAccion As LinkButton = CType(e.Item.FindControl("lnkAccion"), LinkButton)
            lnkAccion.Text = alerta(1) 'Descripcion
            lnkAccion.CommandArgument = alerta(3) 'url
            CType(e.Item.FindControl("lblInfo"), Label).Text = alerta(2)
        End If
    End Sub

    ''' <summary>
    ''' Pincha en el link
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub LinkAlerta(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Response.Redirect(lnk.CommandArgument)
    End Sub

#End Region

End Class