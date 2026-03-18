Public Class CuentasOtrasPlantas
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
                Master.SetTitle = "Cuentas contables de las plantas filiales"
                cargarCuentasPlantas()
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
            itzultzaileWeb.Itzuli(labelPlantas) : itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(btnCancelM)
            itzultzaileWeb.Itzuli(labelCtaNormal) : itzultzaileWeb.Itzuli(rfvCuenta18) : itzultzaileWeb.Itzuli(labelCtaReducida)
            itzultzaileWeb.Itzuli(rfvCuenta8) : itzultzaileWeb.Itzuli(labelCtaExenta) : itzultzaileWeb.Itzuli(rfvCuenta0)
            itzultzaileWeb.Itzuli(labelTitleModal) : itzultzaileWeb.Itzuli(btnSaveM)
        End If
    End Sub

    ''' <summary>
    ''' Carga todas las cuentas de todas las plantas filiales existentes menos la de Igorre
    ''' </summary>    
    Private Sub cargarCuentasPlantas()
        Try
            Dim cuentasBLL As New BLL.DepartamentosBLL
            Dim lCuentas As List(Of ELL.CuentaContable) = cuentasBLL.loadCuentasPlantasFilialesList(Master.IdPlantaGestion)
            If (lCuentas IsNot Nothing) Then
                lCuentas = lCuentas.FindAll(Function(o As ELL.CuentaContable) o.IdPlantaCuenta <> 1)
                lCuentas = lCuentas.OrderBy(Of String)(Function(o) o.NombrePlanta).ToList
            End If
            gvCuentas.DataSource = lCuentas
            gvCuentas.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar las cuentas de las plantas filiales", ex)
        End Try
    End Sub

#End Region

#Region "Eventos gridview"

    ''' <summary>
    ''' Se enlazan los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCuentas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvCuentas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oCuenta As ELL.CuentaContable = e.Row.DataItem
            DirectCast(e.Row.FindControl("lblPlanta"), Label).Text = oCuenta.NombrePlanta
            If (oCuenta.Cuenta18 > 0) Then
                DirectCast(e.Row.FindControl("lblCuenta18"), Label).Text = oCuenta.Cuenta18
                DirectCast(e.Row.FindControl("lblCuenta8"), Label).Text = oCuenta.Cuenta8
                DirectCast(e.Row.FindControl("lblCuenta0"), Label).Text = oCuenta.Cuenta0
            Else
                e.Row.CssClass = "warning"
                e.Row.ToolTip = itzultzaileWeb.Itzuli("Introduzca las cuentas contables")
            End If
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvCuentas, "Select$" + oCuenta.IdPlantaCuenta.ToString)
        End If
    End Sub

    ''' <summary>
    ''' Evento que surge al hacer click en la fila
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCuentas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvCuentas.RowCommand
        Try
            Dim cuentasBLL As New BLL.DepartamentosBLL
            Dim plantasBLL As New SabLib.BLL.PlantasComponent
            lblPlanta.Text = plantasBLL.GetPlanta(CInt(e.CommandArgument)).Nombre
            Dim cuentas As ELL.CuentaContable = cuentasBLL.loadCuentaPlantaFilial(Master.IdPlantaGestion, CInt(e.CommandArgument))
            If (cuentas IsNot Nothing) Then
                txtCuenta18.Text = cuentas.Cuenta18
                txtCuenta8.Text = cuentas.Cuenta8
                txtCuenta0.Text = cuentas.Cuenta0
            Else
                txtCuenta18.Text = String.Empty
                txtCuenta8.Text = String.Empty
                txtCuenta0.Text = String.Empty
            End If
            btnSaveM.CommandArgument = e.CommandArgument
            ShowModalBox(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el panel modal
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBox(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#pageModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#pageModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True)
        End If
    End Sub

    ''' <summary>
    ''' Guarda la cuenta de la planta seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSaveM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveM.Click
        Try
            If (txtCuenta18.Text = String.Empty OrElse txtCuenta8.Text = String.Empty OrElse txtCuenta0.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
                ShowModalBox(True)
            Else
                Dim navBLL As New BLL.NavisionBLL
                Dim mensaCuentas, mensa As String
                mensaCuentas = String.Empty
                Dim lCuentasCont As New List(Of Integer)
                lCuentasCont.Add(CInt(txtCuenta18.Text))
                If (Not lCuentasCont.Exists(Function(o) o = CInt(txtCuenta8.Text))) Then lCuentasCont.Add(CInt(txtCuenta8.Text))
                If (Not lCuentasCont.Exists(Function(o) o = CInt(txtCuenta0.Text))) Then lCuentasCont.Add(CInt(txtCuenta0.Text))
                If (Not navBLL.existenCuentasContable(Master.IdPlantaGestion, lCuentasCont, mensaCuentas)) Then
                    mensa = itzultzaileWeb.Itzuli("Las siguientes cuentas contables no existen en Navision") & ":" & mensaCuentas
                    log.Warn("PLANTAS_FILIALES:" & mensa)
                    Master.MensajeAdvertencia = mensa
                    ShowModalBox(True)
                Else
                    Dim cuentasBLL As New BLL.DepartamentosBLL
                    Dim oCuenta As New ELL.CuentaContable With {.IdPlantaCuenta = CInt(btnSaveM.CommandArgument), .Cuenta18 = CInt(txtCuenta18.Text), .Cuenta8 = CInt(txtCuenta8.Text), .Cuenta0 = CInt(txtCuenta0.Text), .IdPlantaGestion = Master.IdPlantaGestion}
                    cuentasBLL.UpdateCuentaPlantaFilial(oCuenta)
                    ShowModalBox(False)
                    log.Info("PLANTAS_FILIALES:Se ha actualizado la cuenta de la planta (" & oCuenta.IdPlantaCuenta & ") con 18% - " & txtCuenta18.Text & " | 8% - " & txtCuenta8.Text & " | 0% - " & txtCuenta0.Text)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Cuenta actualizada")
                    cargarCuentasPlantas()
                End If
            End If
        Catch batzEx As BatzException
            ShowModalBox(True)
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            ShowModalBox(True)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

#End Region

End Class