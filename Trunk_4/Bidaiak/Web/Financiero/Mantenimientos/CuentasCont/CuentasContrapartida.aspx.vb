Imports SabLib.ELL

Public Class CuentasContrapartida
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se cargan las cuentas de las plantas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Cuentas de contrapartida"
                cargarCuentas()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(labelTitleModal)
            itzultzaileWeb.Itzuli(labelCuota) : itzultzaileWeb.Itzuli(rfvCuota) : itzultzaileWeb.Itzuli(btnSaveM)
            itzultzaileWeb.Itzuli(labelContrapartida) : itzultzaileWeb.Itzuli(rfvContrapartida)
        End If
    End Sub

    ''' <summary>
    ''' Carga todas las cuentas productivas
    ''' </summary>    
    Private Sub cargarCuentas()
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim lCuentas As List(Of ELL.CuentaContrapartida) = bidaiakBLL.loadCuentasContrapartida(Master.IdPlantaGestion)
            If (lCuentas IsNot Nothing) Then lCuentas = lCuentas.OrderBy(Function(o) o.NombrePlanta).ToList
            gvCuentas.DataSource = lCuentas
            gvCuentas.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar las cuentas de contrapartida", ex)
        End Try
    End Sub

#End Region

#Region "Eventos gridview"

    ''' <summary>
    ''' Evento que surge al hacer click en la fila
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCuentas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvCuentas.RowCommand
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim plantasBLL As New SabLib.BLL.PlantasComponent
            Dim idPlanta As Integer = CInt(e.CommandArgument)
            lblPlanta.Text = plantasBLL.GetPlanta(idPlanta).Nombre
            Dim cuenta As ELL.CuentaContrapartida = bidaiakBLL.loadCuentaContrapartida(Master.IdPlantaGestion, idPlanta)
            txtContrapartida.Text = If(cuenta.CtaContrapartida > 0, cuenta.CtaContrapartida, String.Empty)
            txtCuota.Text = If(cuenta.CtaCuota > 0, cuenta.CtaCuota, String.Empty)
            btnSaveM.CommandArgument = idPlanta
            ShowModalBox(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCuentas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvCuentas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oCuenta As ELL.CuentaContrapartida = e.Row.DataItem
            DirectCast(e.Row.FindControl("lblPlanta"), Label).Text = oCuenta.NombrePlanta
            DirectCast(e.Row.FindControl("lblContrapartida"), Label).Text = If(oCuenta.CtaContrapartida > 0, oCuenta.CtaContrapartida, String.Empty)
            DirectCast(e.Row.FindControl("lblCuota"), Label).Text = If(oCuenta.CtaCuota > 0, oCuenta.CtaCuota, String.Empty)
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvCuentas, "Select$" + oCuenta.IdPlantaCuenta.ToString)
        End If
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para introducir la cuenta
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBox(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Guarda la cuenta de la planta seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSaveM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveM.Click
        Try
            If (String.IsNullOrEmpty(txtContrapartida.Text.Trim)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Rellene todos los datos")
                ShowModalBox(True)
            Else
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim cuentaCuota As Integer = If(txtCuota.Text = String.Empty, 0, CInt(txtCuota.Text))
                Dim navBLL As New BLL.NavisionBLL
                Dim mensaCuentas, mensa As String
                mensaCuentas = String.Empty
                Dim lCuentasCont As New List(Of Integer)
                lCuentasCont.Add(CInt(txtContrapartida.Text))
                If (cuentaCuota <> 0) Then lCuentasCont.Add(cuentaCuota)
                If (Not navBLL.existenCuentasContable(Master.IdPlantaGestion, lCuentasCont, mensaCuentas)) Then
                    mensa = itzultzaileWeb.Itzuli("Las siguientes cuentas contables no existen en Navision") & ":" & mensaCuentas
                    log.Warn("CUENTAS_CONTRAPARTIDA:" & mensa)
                    Master.MensajeAdvertencia = mensa
                    ShowModalBox(True)
                Else
                    bidaiakBLL.SaveCuentaContrapartida(New ELL.CuentaContrapartida With {.CtaContrapartida = txtContrapartida.Text, .CtaCuota = cuentaCuota, .IdPlantaGestion = Master.IdPlantaGestion, .IdPlantaCuenta = CInt(btnSaveM.CommandArgument)})
                    ShowModalBox(False)
                    log.Info("CUENTAS_CONTRAPARTIDA:Se ha guardado los datos de la cuenta de contrapartida (" & txtContrapartida.Text & ") y cuota (" & txtCuota.Text & ") de la planta " & btnSaveM.CommandArgument)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Cuenta actualizada")
                    cargarCuentas()
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
            ShowModalBox(True)
        End Try
    End Sub

#End Region

End Class