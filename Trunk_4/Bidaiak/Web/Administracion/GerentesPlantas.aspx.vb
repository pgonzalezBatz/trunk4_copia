Public Class GerentesPlantas
    Inherits PageBase

    ''' <summary>
    ''' Se cargan los gerentes de las plantas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Gerentes de las plantas filiales"
                searchUser.PlaceHolder = itzultzaileWeb.Itzuli("Gerente")
                cargarGerentes()
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
            itzultzaileWeb.Itzuli(labelPlantas) : itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(labelTitleModal)
            itzultzaileWeb.Itzuli(btnSaveM) : itzultzaileWeb.Itzuli(btnCancelM) : itzultzaileWeb.Itzuli(labelGerente)
        End If
    End Sub

    ''' <summary>
    ''' Carga todas gerentes de las plantas filiales existentes menos la de Igorre
    ''' </summary>    
    Private Sub cargarGerentes()
        Try
            Dim gerenteBLL As New BLL.BidaiakBLL
            Dim lGerentes As List(Of String()) = gerenteBLL.loadGerentesPlantas
            If (lGerentes IsNot Nothing) Then '0:idUser,1:idPlanta,2:Nombre planta,3:nombre gerente                                
                lGerentes = lGerentes.OrderBy(Of String)(Function(o) o(2)).ToList
            End If
            gvGerentes.DataSource = lGerentes
            gvGerentes.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar los gerentes de las plantas filiales", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Evento que surge al hacer click en la fila
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvGerentes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvGerentes.RowCommand
        Try
            Dim gerenteBLL As New BLL.BidaiakBLL
            Dim plantasBLL As New SabLib.BLL.PlantasComponent
            lblPlanta.Text = plantasBLL.GetPlanta(CInt(e.CommandArgument)).Nombre
            Dim oGerente As SabLib.ELL.Usuario = gerenteBLL.loadGerentePlanta(CInt(e.CommandArgument))
            If (oGerente Is Nothing) Then
                searchUser.Limpiar()
            Else
                searchUser.SelectedId = oGerente.Id
                searchUser.Texto = oGerente.NombreCompleto
            End If
            btnSaveM.CommandArgument = e.CommandArgument
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
    Private Sub gvGerentes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvGerentes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim sGerente As String() = e.Row.DataItem
            DirectCast(e.Row.FindControl("lblPlanta"), Label).Text = sGerente(2)
            DirectCast(e.Row.FindControl("lblGerente"), Label).Text = sGerente(3)
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvGerentes, "Select$" + sGerente(1))
        End If
    End Sub

    ''' <summary>
    ''' Muestra el panel modal de la pregunta
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
    ''' Guarda el gerente de la planta seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSaveM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveM.Click
        Try
            If (searchUser.SelectedId <> String.Empty) Then
                Dim gerenteBLL As New BLL.BidaiakBLL
                gerenteBLL.saveGerentePlanta(CInt(searchUser.SelectedId), CInt(btnSaveM.CommandArgument))
                ShowModalBox(False)
                log.Info("GERENTES_PLANTAS:Se ha actualizado el gerente de la planta " & lblPlanta.Text & " (" & btnSaveM.CommandArgument & ") a " & searchUser.Texto)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                cargarGerentes()
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un usuario")
                ShowModalBox(True)
            End If
        Catch batzEx As BatzException
            ShowModalBox(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

End Class