Imports NominasLib

Public Class PlantasNominas
    Inherits PageBase

#Region "Vista listado"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Plantas Epsilon"
                cargarListado()
            End If
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo)
            itzultzaileWeb.Itzuli(lnkNuevo) : itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelRuta) : itzultzaileWeb.Itzuli(rfvRuta) : itzultzaileWeb.Itzuli(btnGuardar)
            itzultzaileWeb.Itzuli(btnEliminar)
        End If
    End Sub

    ''' <summary>
    ''' Indica si recargara el listado o no
    ''' </summary>
    ''' <param name="reload">Indica si se recargara el listado o no</param>    
    Private Sub cargarListado(Optional ByVal reload As Boolean = True)
        Try
            mView.ActiveViewIndex = 0
            If (reload) Then
                Dim negBLL As New Nomina                
                gvItems.DataSource = negBLL.ConsultarPlantasNominas()
                gvItems.DataBind()
            End If
        Catch ex As Exception
            Throw New Sablib.BatzException("errMostrarListado", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se prepara el formulario para un nuevo item
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkNuevo_Click(sender As Object, e As EventArgs) Handles lnkNuevo.Click
        Try
            mostrarDetalle(Integer.MinValue)
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Evento que surge cuando se enlazan los items
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvItems_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvItems.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As String() = e.Row.DataItem
            Dim lblPlanta As Label = CType(e.Row.FindControl("lblPlanta"), Label)
            Dim lblRuta As Label = CType(e.Row.FindControl("lblRuta"), Label)
            lblPlanta.Text = oItem(2)
            lblRuta.Text = oItem(1)
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvItems, "Select$" + oItem(0))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvItems_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvItems.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub


    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvItems_Paging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvItems.PageIndexChanging
        Try
            gvItems.PageIndex = e.NewPageIndex
            cargarListado()
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Vista detalle"

    ''' <summary>
    ''' Se muestra el detalle del item
    ''' </summary>
    ''' <param name="id">Id del item</param>    
    Private Sub mostrarDetalle(ByVal id As Integer)
        Try
            inicializarDetalle()
            If (id > 0) Then
                Dim negBLL As New Nomina
                Dim oItem As String() = negBLL.ConsultarPlantaNomina(id)
                pnlOld.Visible = True : pnlNew.Visible = False : btnEliminar.Visible = True
                btnGuardar.CommandArgument = oItem(0)
                lblPlanta.Text = oItem(2)
                txtRuta.Text = oItem(1)
            End If
        Catch ex As Exception
            Throw New Sablib.BatzException("errCompDetalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa el detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        mView.ActiveViewIndex = 1
        lblPlanta.Text = String.Empty : txtRuta.Text = String.Empty
        pnlNew.Visible = True : pnlOld.Visible = False : btnEliminar.Visible = False
        cargarPlantasEpsilon()
        btnGuardar.CommandArgument = String.Empty
        btnEliminar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "')"
    End Sub

    ''' <summary>
    ''' Carga las plantas de Epsilon que quedan por registrar
    ''' </summary>    
    Private Sub cargarPlantasEpsilon()
        Dim plantBLL As New Sablib.BLL.PlantasComponent
        Dim negBLL As New Nomina
        Dim lPlantasEpsilon As List(Of Sablib.ELL.Planta) = plantBLL.GetPlantas(True, Nothing, Nothing, True)
        Dim lPlantasReg As List(Of String()) = negBLL.ConsultarPlantasNominas()
        ddlPlantas.Items.Clear()
        For Each oPlantEps As Sablib.ELL.Planta In lPlantasEpsilon
            If (Not lPlantasReg.Exists(Function(o As String()) o(0) = oPlantEps.IdEpsilon)) Then
                ddlPlantas.Items.Add(New ListItem(oPlantEps.Nombre, oPlantEps.IdEpsilon))
            End If
        Next
        If (ddlPlantas.Items.Count = 0) Then
            ddlPlantas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("No existen mas plantas"), Integer.MinValue))
        Else
            ddlPlantas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
        End If
        ddlPlantas.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Se guardan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtRuta.Text = String.Empty OrElse (pnlNew.Visible AndAlso CInt(ddlPlantas.SelectedValue) = Integer.MinValue)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca los datos")
            Else
                Dim negBLL As New Nomina
                Dim idPlanta As Integer = If(pnlNew.Visible, CInt(ddlPlantas.SelectedValue), btnGuardar.CommandArgument)
                Dim sItem As String() = {idPlanta, txtRuta.Text}
                negBLL.SavePlantaNomina(sItem, pnlNew.Visible)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                cargarListado()
            End If
        Catch batzEx As Sablib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
        End Try
    End Sub

    ''' <summary>
    ''' Se elimina el item
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            Dim negBLL As New Nomina
            negBLL.DeletePlantaNomina(CInt(btnGuardar.CommandArgument))
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Item borrado")
            cargarListado()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            Volver(False)
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errMostrarListado")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>    
    ''' <param name="reload">Indica si se recargara el listado</param>
    Private Sub Volver(ByVal reload As Boolean)
        cargarListado(reload)
    End Sub

#End Region

End Class