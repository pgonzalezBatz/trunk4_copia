Partial Public Class Excepciones
    Inherits PageBase

#Region "Page Load e inicializaciones"

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Excepcion de Visas"
                gvVisas.Attributes("CurrentSortField") = "NumTarjeta"
                gvVisas.Attributes("CurrentSortDirection") = SortDirection.Ascending
                cargarVisasExc()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AsignacionVisas_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(labelNumTarj)
            itzultzaileWeb.Itzuli(lnkQuitarM) : itzultzaileWeb.Itzuli(btnCerrarM) : itzultzaileWeb.Itzuli(labelTitleModal)
            itzultzaileWeb.Itzuli(labelConfirmTitle) : itzultzaileWeb.Itzuli(labelConfirmMessage) : itzultzaileWeb.Itzuli(btnConfirmDelete)
            itzultzaileWeb.Itzuli(labelConfirmCerrar)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de visas de excepcion
    ''' </summary>	
    Private Sub cargarVisasExc()
        Dim visasBLL As New BLL.VisasBLL
        Dim lVisas As List(Of ELL.Visa) = Nothing
        Dim myVisa As New ELL.Visa With {.IdPlanta = Master.IdPlantaGestion}
        If (txtFilter.Text.Trim <> String.Empty) Then myVisa.NumTarjeta = txtFilter.Text.Trim
        lVisas = visasBLL.loadListExcepcion(myVisa, Master.IdPlantaGestion)
        If (lVisas IsNot Nothing AndAlso lVisas.Count > 0) Then OrdenarLista(lVisas, gvVisas.Attributes("CurrentSortField"), gvVisas.Attributes("CurrentSortDirection"))
        gvVisas.DataSource = lVisas
        gvVisas.DataBind()
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

#End Region

#Region "Eventos Gridview"

    ''' <summary>
    '''  Carga del listado de integrantes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvVisas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvVisas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim visa As ELL.Visa = DirectCast(e.Row.DataItem, ELL.Visa)
            DirectCast(e.Row.FindControl("lblPersona"), Label).Text = visa.Propietario.Nombre
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvVisas, "Select$" + CStr(visa.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub gvVisas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvVisas.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(e.CommandArgument)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisas_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvVisas.PageIndexChanging
        Try
            gvVisas.PageIndex = e.NewPageIndex
            cargarVisasExc()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisas_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvVisas.Sorting
        Try
            gvVisas.Attributes("CurrentSortField") = e.SortExpression
            If (gvVisas.Attributes("CurrentSortDirection") Is Nothing) Then
                gvVisas.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvVisas.Attributes("CurrentSortDirection") = If(gvVisas.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            cargarVisasExc()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena una lista sin conocer el tipo que es
    ''' </summary>
    ''' <param name="lista"></param>    
    ''' <param name="CampoOrden"></param>
    ''' <param name="DireccionCampo"></param>        
    Private Sub OrdenarLista(ByRef lista As List(Of ELL.Visa), Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
        Select Case CampoOrden
            Case "NumTarjeta"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.NumTarjeta).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.NumTarjeta).ToList
                End If
            Case "Persona"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Propietario.Nombre).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Propietario.Nombre).ToList
                End If
        End Select
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Busca el usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Try
            cargarVisasExc()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Detalle"

    ''' <summary>
    ''' Muestra el detalle de la visa
    ''' </summary>
    ''' <param name="id"></param>	
    Private Sub mostrarDetalle(ByVal id As Integer)
        Dim visasBLL As New BLL.VisasBLL
        Dim oVisa As ELL.Visa = visasBLL.loadInfoExcepcion(New ELL.Visa With {.Id = id})
        lblNumTarjeta.Text = oVisa.NumTarjeta
        lnkQuitarM.CommandArgument = oVisa.Id
        lnkQuitarM.Attributes.Add("href", "#confirmDelete")
        ShowModalBox(True)
    End Sub

    ''' <summary>
    ''' Elimina el perfil del usuario y se convierte en consultor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnConfirmDelete_Click(sender As Object, e As EventArgs) Handles btnConfirmDelete.Click
        Try
            Dim visasBLL As New BLL.VisasBLL
            Dim oVisa As New ELL.Visa
            visasBLL.DeleteExcepcion(CInt(lnkQuitarM.CommandArgument))
            log.Info("EXCEPCION_VISA: Se ha eliminado la excepcion de visa " & lblNumTarjeta.Text)
            ShowModalBox(False)
            cargarVisasExc()
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Visa eliminada")
        Catch batzEx As BatzException
            ShowModalBox(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class