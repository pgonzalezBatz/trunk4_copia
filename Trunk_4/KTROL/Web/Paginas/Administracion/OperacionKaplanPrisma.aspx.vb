Public Class OperacionKaplanPrisma
    Inherits PageBase


    Dim oOperacionKaplanPrismaBLL As New BLL.OperacionKaplanPrismaBLL

#Region "Miembros"

    Private m_ViewMode As ViewMode = ViewMode.GridView

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Permite al usuario introducir nuevos registros
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property AllowRecordInserting As Boolean
        Get
            Return dvKaplanPrisma.AutoGenerateInsertButton
        End Get
    End Property

#End Region

#Region "Enumerados"

    Protected Enum ViewMode
        Unknown
        GridView
        DetailsView
    End Enum

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Operaciones_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.dvKaplanPrisma)
        End If
    End Sub

    ''' <summary>
    ''' Muestra las colas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Try
                ComprobarAcceso()
            Catch ex As Exception
                Response.Redirect("~/PermisoDenegado.aspx")
            End Try

            If Not Page.IsPostBack Then
                Initialize()
            End If

            ' Indico el mensaje de confirmación de borrado
            If (Page.ClientScript.IsClientScriptBlockRegistered("confirmBorradoOperacionKaplanPrisma") <> True) Then
                Dim msgConfirm As String = String.Format("var msg_confirm = '{0}';", ItzultzaileWeb.Itzuli("¿Desea eliminar la relación seleccionada?"))
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "confirmBorradoOperacionKaplanPrisma", msgConfirm, True)
            End If

            Page.MaintainScrollPositionOnPostBack = True
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar la página", ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

   ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        If (Session("ticket") Is Nothing AndAlso Session("PerfilUsuario") Is Nothing) Then
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        Try
            ClearDataViews()
            BindDataViews()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Establece el modo de funcionamiento del grid y el details
    ''' </summary>
    ''' <param name="vmNewViewMode"></param>
    ''' <remarks></remarks>
    Protected Sub SetBehavior(ByVal vmNewViewMode As ViewMode)
        m_ViewMode = vmNewViewMode
        SetBehavior()
    End Sub

    ''' <summary>
    ''' Gestiona el modo de funcionamiento del grid y el details
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetBehavior()
        Dim bHasGridRows As Boolean = (gvKaplanPrisma.Rows.Count > 0)
        If ((bHasGridRows = False) AndAlso (Me.AllowRecordInserting = True)) Then
            m_ViewMode = ViewMode.DetailsView
        End If

        Select Case m_ViewMode
            Case ViewMode.Unknown
                pnlGridView.Visible = bHasGridRows
                pnlDetailsView.Visible = Not bHasGridRows
            Case ViewMode.GridView
                pnlGridView.Visible = True
                pnlDetailsView.Visible = False
            Case ViewMode.DetailsView
                pnlGridView.Visible = False
                pnlDetailsView.Visible = True
        End Select

        lbtnVolver.Visible = bHasGridRows
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearDataViews()
        gvKaplanPrisma.DataSource = Nothing
        gvKaplanPrisma.DataBind()

        dvKaplanPrisma.DataSource = Nothing
        dvKaplanPrisma.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataViews()
        BindDataViews(0)
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <param name="id">Identificador de relación</param>
    ''' <remarks></remarks>
    Protected Sub BindDataViews(ByVal id As Integer)
        'Dim gruposBLL As New BLL.GruposSoftwareBLL
        Dim listaOperacionesKaplanPrisma As List(Of ELL.OperacionKaplanPrisma) = Nothing
        If (id > 0) Then
            listaOperacionesKaplanPrisma = New List(Of ELL.OperacionKaplanPrisma)
            listaOperacionesKaplanPrisma.Add(oOperacionKaplanPrismaBLL.CargarOperacionKaplanPrisma(id))
            dvKaplanPrisma.DataSource = listaOperacionesKaplanPrisma
            dvKaplanPrisma.DataBind()

            SetBehavior(ViewMode.DetailsView)
        Else
            listaOperacionesKaplanPrisma = oOperacionKaplanPrismaBLL.CargarOperacionesKaplanPrisma()

            If (listaOperacionesKaplanPrisma.Count > 0) Then
                gvKaplanPrisma.DataSource = listaOperacionesKaplanPrisma
                gvKaplanPrisma.DataBind()
            Else
                If (Me.AllowRecordInserting) Then
                    dvKaplanPrisma.DataSource = listaOperacionesKaplanPrisma
                    dvKaplanPrisma.DataBind()
                Else
                    gvKaplanPrisma.DataSource = Nothing
                    gvKaplanPrisma.DataBind()
                End If
            End If

            SetBehavior()
        End If
    End Sub

    ''' <summary>
    ''' Se borran los valores de los campos del details
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BorrarCampos()
        Dim txt As TextBox = CType(dvKaplanPrisma.FindControl("txtInsertCodOperacionKaplan"), TextBox)
        If (txt IsNot Nothing) Then
            txt.Text = String.Empty
        End If

        txt = CType(dvKaplanPrisma.FindControl("txtInsertCodOperacionPrisma"), TextBox)
        If (txt IsNot Nothing) Then
            txt.Text = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Devuelve la clave primaria del grid de fila
    ''' </summary>
    ''' <param name="row"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetGridViewRowId(ByVal row As GridViewRow) As Integer
        Dim id As Integer = Integer.MinValue
        Dim _ctl As Label = CType(row.FindControl("lblId"), Label)
        If (_ctl IsNot Nothing) Then
            id = CInt(_ctl.Text)
        End If

        Return id
    End Function

    ''' <summary>
    ''' Devuelve la clave primaria del details
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDetailsViewId() As Integer
        Dim id As Integer = Integer.MinValue
        Dim _ctl As Label = CType(dvKaplanPrisma.FindControl("lblId"), Label)
        If (_ctl IsNot Nothing) Then
            id = CInt(_ctl.Text)
        End If

        Return id
    End Function

    ''' <summary>
    ''' Devuelve el valor del control de la fila del details
    ''' </summary>
    ''' <param name="sControlName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTextFieldValue(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As TextBox = CType(dvKaplanPrisma.FindControl(sControlName), TextBox)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.Text.Trim()
        End If

        Return sFieldValue
    End Function

    '' <summary>
    ''' Devuelve el valor del control de la fila del details
    ''' </summary>
    ''' <param name="sControlName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDropDownListFieldValue(ByVal sControlName As String) As String
        Dim sFieldValue As String = String.Empty
        Dim _ctl As DropDownList = CType(dvKaplanPrisma.FindControl(sControlName), DropDownList)
        If (_ctl IsNot Nothing) Then
            sFieldValue = _ctl.SelectedValue
        End If

        Return sFieldValue
    End Function

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Cambia a modo inserción en el details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnAgregarNuevaRelacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAgregarNuevaRelacion.Click
        dvKaplanPrisma.ChangeMode(DetailsViewMode.Insert)
        SetBehavior(ViewMode.DetailsView)

        'Borramos los campos
        BorrarCampos()
    End Sub

    ''' <summary>
    ''' Vuelve al modo grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnVolverListaGrupos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnVolver.Click
        BindDataViews()
        SetBehavior(ViewMode.GridView)
    End Sub

    ''' <summary>
    ''' Cambio de página en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvKaplanPrisma_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvKaplanPrisma.PageIndexChanging
        gvKaplanPrisma.PageIndex = e.NewPageIndex
        BindDataViews()
    End Sub

    ''' <summary>
    ''' Editando la fila en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvKaplanPrisma_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvKaplanPrisma.RowEditing
        dvKaplanPrisma.ChangeMode(DetailsViewMode.Edit)
        Dim id As Integer = GetGridViewRowId(gvKaplanPrisma.Rows(e.NewEditIndex))
        BindDataViews(id)
        SetBehavior(ViewMode.DetailsView)
    End Sub

    ' ''' <summary>
    ' ''' DataBound del DetailsView
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub dvGrupos_DataBound(sender As Object, e As EventArgs) Handles dvGrupos.DataBound
    '    Dim grupo As ELL.GruposSoftware = CType(dvGrupos.DataItem, ELL.GruposSoftware)

    '    Dim ddlLeerFicheroSalida As DropDownList = CType(dvGrupos.FindControl("ddlEditLeerFicheroSalida"), DropDownList)

    '    If (ddlLeerFicheroSalida IsNot Nothing AndAlso grupo IsNot Nothing) Then
    '        ddlLeerFicheroSalida.SelectedValue = grupo.LeerFicheroSalida
    '    End If
    'End Sub

    ''' <summary>
    ''' Insertando un nuevo registro en el details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvKaplanPrisma_ItemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs) Handles dvKaplanPrisma.ItemInserting
        'Dim gruposBLL As New BLL.GruposSoftwareBLL()
        Try

            Dim operacionKaplanPrisma As New ELL.OperacionKaplanPrisma With {
            .CodOperacionKaplan = GetTextFieldValue("txtInsertCodOperacionKaplan"),
            .CodOperacionPrisma = GetTextFieldValue("txtInsertCodOperacionPrisma")}

            If (oOperacionKaplanPrismaBLL.GuardarOperacionKaplanPrisma(operacionKaplanPrisma)) Then
                'Master.MensajeInfo = ItzultzaileWeb.Itzuli("La relación se ha creado correctamente")
                Master.MensajeInfo = "La relación se ha creado correctamente"
            Else
                'Master.MensajeError = ItzultzaileWeb.Itzuli("Se ha producido un error al crear el grupo")
                Master.MensajeError = "Se ha producido un error al crear la relación"
            End If
            BindDataViews()
            dvKaplanPrisma.ChangeMode(DetailsViewMode.Insert)
            SetBehavior(ViewMode.GridView)
        Catch ex As Exception
           Global_asax.log.Error("Se ha producido un error al crear la relación", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cambio de modo del details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dvKaplanPrisma_ModeChanging(ByVal sender As Object, ByVal e As DetailsViewModeEventArgs) Handles dvKaplanPrisma.ModeChanging
        If (e.CancelingEdit = False) Then
            dvKaplanPrisma.ChangeMode(e.NewMode)
            Dim id As Integer = GetGridViewRowId(gvKaplanPrisma.SelectedRow)
            BindDataViews(id)
        Else
            dvKaplanPrisma.ChangeMode(DetailsViewMode.Insert)
            SetBehavior(ViewMode.GridView)
        End If
    End Sub

    ''' <summary>
    ''' Evento del botón Eliminar correspondiente a la fila del grid seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkEliminar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim resultado As Integer
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim dataKeyRelacion As String = gvKaplanPrisma.DataKeys(fila.RowIndex).Value.ToString()
            resultado = oOperacionKaplanPrismaBLL.EliminarOperacionKaplanPrisma(dataKeyRelacion)
            Select Case (resultado)
                Case 0
                    'Master.MensajeInfo = ItzultzaileWeb.Itzuli("El grupo se elimino correctamente")
                    Master.MensajeInfo = "La relación se eliminó correctamente"
                Case Else
                    Master.MensajeError = "Se ha producido un error al eliminar la relación"
            End Select
            BindDataViews(0)
            dvKaplanPrisma.ChangeMode(DetailsViewMode.Insert)
            SetBehavior(ViewMode.GridView)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    ''' <summary>
    ''' Borrando la fila en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvKaplanPrisma_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles gvKaplanPrisma.RowDeleting
        Try
            Dim row As GridViewRow = gvKaplanPrisma.Rows(e.RowIndex)
            Dim dataKeyRelacion As Integer = GetGridViewRowId(row)
            If (oOperacionKaplanPrismaBLL.EliminarOperacionKaplanPrisma(dataKeyRelacion)) Then
                'Master.MensajeInfo = ItzultzaileWeb.Itzuli("El grupo se elimino correctamente")
                Master.MensajeInfo = "La relación se eliminó correctamente"
            Else
                Master.MensajeError = "Se ha producido un error al eliminar la relación"
            End If

            BindDataViews(0)
            dvKaplanPrisma.ChangeMode(DetailsViewMode.Insert)
            SetBehavior(ViewMode.GridView)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

End Class