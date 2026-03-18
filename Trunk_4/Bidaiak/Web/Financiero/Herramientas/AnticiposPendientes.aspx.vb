Public Class AnticiposPendientes
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Anticipos pendientes de justificar"
                BuscarAnticiposPendientes()
            End If
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelResul)
        End If
    End Sub

#End Region

#Region "Ver listado"

    ''' <summary>
    ''' Busca los anticipos pendientes de justificar
    ''' </summary>    
    Private Sub BuscarAnticiposPendientes()
        Try
            Dim anticBLL As New BLL.AnticiposBLL
            Dim lAnticiposPen As List(Of Object) = anticBLL.loadAnticiposPendientes(Master.IdPlantaGestion)
            If (lAnticiposPen IsNot Nothing) Then
                lAnticiposPen = lAnticiposPen.OrderBy(Of Date)(Function(o) o.FechaIdaViaje).ToList
                labelResul.Visible = True : lblReg.Visible = True
                lblReg.Text = lAnticiposPen.Count
            Else
                labelResul.Visible = False : lblReg.Visible = False
            End If
            gvAnticPend.DataSource = lAnticiposPen
            gvAnticPend.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al buscar", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAnticPend_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAnticPend.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oAntic = e.Row.DataItem
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim hlViaje As HyperLink = CType(e.Row.FindControl("hlViaje"), HyperLink)
            Dim lblEstadoHoja As Label = CType(e.Row.FindControl("lblEstadoHoja"), Label)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim epsilonBLL As New BLL.Epsilon(Master.IdPlantaGestion)
            lblPersona.Text = oAntic.NombreLiq & " (" & oAntic.CodPersona & ")"
            If (oAntic.DadoBaja) Then lblPersona.Style.Add("color", "#FF0000")
            hlViaje.NavigateUrl = "~/Financiero/Anticipos/GestionAnticipos.aspx?idViaje=" & oAntic.IdAnticipo & "&orig=ANTPEND"
            hlViaje.Text = "V" & oAntic.IdAnticipo
            CType(e.Row.FindControl("lblFViaje"), Label).Text = oAntic.FechaIdaViaje.ToShortDateString & " - " & oAntic.FechaVueltaViaje.ToShortDateString
            If (String.IsNullOrEmpty(oAntic.EstadoHG)) Then
                lblEstadoHoja.Text = itzultzaileWeb.Itzuli("-")
            Else
                lblEstadoHoja.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.HojaGastos.eEstado), oAntic.EstadoHG))
                lblEstadoHoja.ToolTip = oAntic.FechaEstadoHG.ToShortDateString
            End If
            CType(e.Row.FindControl("lblPendiente"), Label).Text = oAntic.CantidadPendiente
            If (CDate(oAntic.FechaVueltaViaje).AddMonths(3).Subtract(Now).TotalMinutes < 0) Then
                e.Row.ToolTip = itzultzaileWeb.Itzuli("Anticipo con mas de tres meses sin devolver")
                e.Row.CssClass = "danger"  'Si ha pasado mas de dos meses de la fecha de vuelta, se marca en rojo
            End If
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim ArrayValues As New List(Of String)
            Dim labelTotal As Label = CType(e.Row.FindControl("labelTotal"), Label)
            Dim cont As Integer = 0
            Dim total As Decimal = 0
            For Each gvr As GridViewRow In gvAnticPend.Rows
                total += CDec(CType(gvr.FindControl("lblPendiente"), Label).Text.Trim)
            Next
            If (labelTotal IsNot Nothing) Then itzultzaileWeb.Itzuli(labelTotal)
            CType(e.Row.FindControl("lblTotal"), Label).Text = total & "€"
        End If
    End Sub

#End Region

End Class