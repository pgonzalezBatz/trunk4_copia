Public Class ModificarCaracteristicas
    Inherits PageBase

    Dim oControlesBLL As New BLL.ControlesBLL

#Region "Properties"

    ''' <summary>
    ''' Código de trabajador del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdControl() As String
        Get
            Return ViewState("IdControl")
        End Get
        Set(ByVal value As String)
            ViewState("IdControl") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvControles_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            ' Forzamos la traducción del details view ya que de inicio está visible = false y no se estaba traduciendo
            ItzultzaileWeb.Itzuli(Me.gvControles)
            ItzultzaileWeb.Itzuli(Me.dvResumen)
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
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End Try

            Dim curObj As ScriptManager = ScriptManager.GetCurrent(Page)
            If curObj IsNot Nothing Then
                curObj.RegisterPostBackControl(btnAdjuntar)
            End If

            If Not Page.IsPostBack Then                
                Initialize()
            End If

            AgregarScriptBusquedaUsuarios()
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
        If (Session("Ticket") Is Nothing AndAlso Session("PerfilUsuario") Is Nothing) Then
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
    End Sub

    ''' <summary>
    ''' Obtiene la cultura del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CultureUser() As String
        Dim persona As New Sablib.ELL.Ticket
        Dim culture As String = "es-ES"

        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), Sablib.ELL.Ticket)
            culture = persona.Culture
        End If

        Return culture
    End Function

    ''' <summary>
    ''' Devuelve el index de la columna pasándole el nombre de la columna
    ''' </summary>
    ''' <param name="grid"></param>
    ''' <param name="columnName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetColumnIndex(ByVal grid As GridView, ByVal columnName As String) As Integer
        Dim index As Integer = Integer.MinValue
        For Each column As DataControlField In grid.Columns
            If column.AccessibleHeaderText.Trim().ToUpper() = columnName.Trim().ToUpper() Then
                index = grid.Columns.IndexOf(column)
                Exit For
            End If
        Next
        Return index

    End Function

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearDataViews()
        BindDataViews()
    End Sub

    ''' <summary>
    ''' Agrega un script de búsqueda de usuario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AgregarScriptBusquedaUsuarios()
        Try
            Dim txtInsertUsuario As TextBox = CType(Me.dvErrores.FindControl("txtInsertUsuario"), TextBox)
            Dim hfIdUsuario As HiddenField = CType(Me.dvErrores.FindControl("hfIdUsuario"), HiddenField)
            Dim helper As HtmlGenericControl = CType(Me.dvErrores.FindControl("helper"), HtmlGenericControl)
            Dim imgSeleccion As HtmlGenericControl = CType(Me.dvErrores.FindControl("imgSeleccion"), HtmlGenericControl)

            If (Not hfIdUsuario Is Nothing AndAlso Not String.IsNullOrEmpty(hfIdUsuario.Value)) Then
                imgSeleccion.Attributes("class") = "imagen-seleccionado"
            Else
                imgSeleccion.Attributes("class") = "imagen-no-seleccionado"
            End If

            Dim cultura As String = CultureUser()

            Dim script As String = "initBusquedaUsuarios('" & txtInsertUsuario.ClientID & "', '" & hfIdUsuario.ClientID & "', '" & helper.ClientID & "','../BuscarUsuarios.aspx',false,'','" & imgSeleccion.ClientID & "', '" & cultura & "');"
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "usuarios", script, True)
        Catch ex As Exception
        End Try        
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearDataViews()
        'DetailsView
        dvResumen.DataSource = Nothing
        dvResumen.DataBind()
        'Gridview
        gvControles.DataSource = Nothing
        gvControles.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataViews()
        Dim listaControles As List(Of ELL.ControlesValoresResumen) = Nothing
        Dim control As ELL.Controles = Nothing
        Dim errorControl As ELL.ControlesErrores
        Dim listaErroresControl As New List(Of ELL.ControlesErrores)
        Dim listaControl As New List(Of ELL.Controles)
        Dim oUsuariosComponentBLL As New BLL.UsuariosComponent

        '1)Cargar los datos del usuario que realizó el control
        If Not (String.IsNullOrEmpty(txtIdControl.Text)) Then
            control = oControlesBLL.ObtenerControl(txtIdControl.Text.Trim)
            listaControles = oControlesBLL.ObtenerControlValores(txtIdControl.Text.Trim)
        Else
            control = oControlesBLL.ObtenerControl(0)
            listaControles = oControlesBLL.ObtenerControlValores(0)
        End If

        If (control IsNot Nothing) Then
            listaControl.Add(control)
            lnbtUsuario.Visible = True
        Else : lnbtUsuario.Visible = False
        End If
        dvResumen.DataSource = listaControl
        dvResumen.DataBind()

        '2)Cargar los datos de las características del control
        If (listaControles.Count > 0) Then
            gvControles.DataSource = listaControles
            gvControles.DataBind()
            gvControles.Caption = String.Empty
            btnEliminarControl.Visible = True
            lblAdvertenciaCaracteristicas.Visible = True
            If (control.IdTipo = ELL.Usuarios.RolesUsuario.Operario) Then
                gvControles.Columns(GetColumnIndex(gvControles, "imgSubirFichero")).Visible = False
            Else
                gvControles.Columns(GetColumnIndex(gvControles, "imgSubirFichero")).Visible = True
            End If
        Else
            gvControles.DataSource = Nothing
            gvControles.DataBind()
            gvControles.Caption = ItzultzaileWeb.Itzuli("noExisteNingunRegistro")
            btnEliminarControl.Visible = False
            lblAdvertenciaCaracteristicas.Visible = False
        End If

        If Not (String.IsNullOrEmpty(txtIdControl.Text)) Then
            errorControl = oControlesBLL.ObtenerControlErrores(txtIdControl.Text.Trim)
        Else
            errorControl = oControlesBLL.ObtenerControlErrores(0)
        End If
        listaErroresControl.Add(errorControl)

        If (errorControl IsNot Nothing) Then
            dvErrores.DataSource = listaErroresControl
            dvErrores.DataBind()

            If (errorControl.Validado) Then
                dvErrores.Fields(1).Visible = True
                Dim hidden As HiddenField = CType(dvErrores.FindControl("hfIdUsuario"), HiddenField)

                hidden.Value = oUsuariosComponentBLL.GetUsuario(New ELL.Usuario With {.CodPersona = errorControl.ValidacionUsuario, .IdPlanta = CType(Session("Ticket"), SabLib.ELL.Ticket).IdPlanta}).Id
            Else
                dvErrores.Fields(1).Visible = False
            End If
        Else
            dvErrores.DataSource = Nothing
            dvErrores.DataBind()
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
    ''' Muestra en el repeater los adjuntos
    ''' </summary>
    ''' <param name="dirInfoTemporal"></param>
    ''' <param name="dirInfoReal"></param>
    ''' <remarks></remarks>
    Private Sub mostrarAdjuntos(Optional ByVal dirInfoTemporal As IO.DirectoryInfo = Nothing, Optional ByVal dirInfoReal As IO.DirectoryInfo = Nothing)
        Try
            Dim lAdjuntos As List(Of String) = Nothing

            If (dirInfoTemporal IsNot Nothing) Then
                Dim lAttachm As IO.FileInfo() = dirInfoTemporal.GetFiles()
                If (lAttachm IsNot Nothing) Then
                    lAdjuntos = New List(Of String)
                    For Each attach As IO.FileInfo In lAttachm
                        If Not (attach.Name.ToLower.Contains("thumbs")) Then
                            lAdjuntos.Add(attach.Name)
                        End If
                    Next
                End If
                rptAdjuntosSubir.DataSource = lAdjuntos
                rptAdjuntosSubir.DataBind()
            End If
            If (dirInfoReal IsNot Nothing) Then
                Dim lAttachm As IO.FileInfo() = dirInfoReal.GetFiles()
                If (lAttachm IsNot Nothing) Then
                    lAdjuntos = New List(Of String)
                    For Each attach As IO.FileInfo In lAttachm
                        If Not (attach.Name.ToLower.Contains("thumbs")) Then
                            lAdjuntos.Add(attach.Name)
                        End If
                    Next
                    If (lAdjuntos.Count > 0) Then
                        pnlAdjuntosVacio.Visible = False
                    Else
                        pnlAdjuntosVacio.Visible = True
                    End If
                End If
                rptAdjuntos.DataSource = lAdjuntos
                rptAdjuntos.DataBind()
            End If
        Catch ex As Exception
            Master.MensajeError = "Error al mostrar los adjuntos"
        End Try
    End Sub

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Buscar las características de un control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnBuscarControl_Click(sender As Object, e As EventArgs)
        Try
            IdControl = txtIdControl.Text.Trim
            gvControles.EditIndex = -1
            BindDataViews()
            AgregarScriptBusquedaUsuarios()
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Habilitar la opción de edición del control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvControles_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvControles.EditIndex = e.NewEditIndex
        BindDataViews()
    End Sub

    ''' <summary>
    ''' Guardar el nuevo valor de la característica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvControles_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim txtEditarValor As TextBox = DirectCast(gvControles.Rows(e.RowIndex).FindControl("txtEditarValor"), TextBox)
        Dim ddlEditarValor As DropDownList = DirectCast(gvControles.Rows(e.RowIndex).FindControl("ddlEditarValor"), DropDownList)
        Dim lblTipo As Label = DirectCast(gvControles.Rows(e.RowIndex).FindControl("lblTipo"), Label)
        Dim chkCambiarControl As CheckBox = DirectCast(gvControles.Rows(e.RowIndex).FindControl("chkCambiarControl"), CheckBox)
        Dim lblIdRegistro As Label = DirectCast(gvControles.Rows(e.RowIndex).FindControl("lblIdRegistro"), Label)

        If (lblIdRegistro IsNot Nothing) Then
            Dim control As New ELL.ControlesValoresResumen With {.IdControl = CInt(IdControl), .IdRegistro = lblIdRegistro.Text}
            Select Case lblTipo.Text
                Case "A"
                    If (ddlEditarValor IsNot Nothing) Then
                        control.OkNok = ddlEditarValor.SelectedValue
                        Dim sinErroresAntes As Boolean = oControlesBLL.ControlSinErrores(control.IdControl)
                        If (ddlEditarValor.SelectedValue.Equals("0") AndAlso sinErroresAntes) Then
                            'Al editar la caracteristica y no haber controles anteriormente, el usuario tendrá 
                            'que reportar el error
                            hfIdRegistro.Value = lblIdRegistro.Text
                            hfValor.Value = ddlEditarValor.SelectedValue
                            mpeReporteErrores.Show()
                        Else
                            If (ddlEditarValor.SelectedValue.Equals("0")) Then
                                control.OkNok = "0"
                            Else
                                control.OkNok = "1"
                            End If
                            control.Valor = "0"
                            If (oControlesBLL.ModificarValorControl(control)) Then
                                'Tenemos que comprobar si después de guardar el control no hay características NOK y si
                                'antes de guardar había. Entonces, hay que eliminar el error
                                Dim sinErroresDespues As Boolean = oControlesBLL.CaracteristicasSinErrores(control.IdControl)
                                If (Not sinErroresAntes AndAlso sinErroresDespues) Then
                                    'hay que eliminar de la tabla de Controles_errores el registro del control
                                    oControlesBLL.EliminarControlError(control.IdControl)
                                End If
                                Master.MensajeInfo = "La carasterística ha sido modificada correctamente"
                            Else
                                Master.MensajeError = "Ha ocurrido un error al modificar la carasterística"
                            End If
                            gvControles.EditIndex = -1
                            BindDataViews()
                        End If                        
                    End If
                Case "V"
                    If (txtEditarValor IsNot Nothing) Then
                        If (chkCambiarControl IsNot Nothing) Then
                            Dim sinErroresAntes As Boolean = oControlesBLL.ControlSinErrores(control.IdControl)
                            If (chkCambiarControl.Checked AndAlso sinErroresAntes) Then
                                control.OkNok = "0"
                                'Al editar la caracteristica y no haber controles anteriormente, el usuario tendrá 
                                'que reportar el error
                                hfIdRegistro.Value = lblIdRegistro.Text
                                hfValor.Value = txtEditarValor.Text.Trim
                                mpeReporteErrores.Show()
                            Else
                                If (chkCambiarControl.Checked) Then
                                    control.OkNok = "0"
                                Else
                                    control.OkNok = "1"
                                End If
                                control.Valor = txtEditarValor.Text.Trim
                                If (oControlesBLL.ModificarValorControl(control)) Then
                                    Dim sinErroresDespues As Boolean = oControlesBLL.CaracteristicasSinErrores(control.IdControl)
                                    If (Not sinErroresAntes AndAlso sinErroresDespues) Then
                                        'hay que eliminar de la tabla de Controles_errores el registro del control
                                        oControlesBLL.EliminarControlError(control.IdControl)
                                    End If
                                    Master.MensajeInfo = "La carasterística ha sido modificada correctamente"
                                Else
                                    Master.MensajeError = "Ha ocurrido un error al modificar la carasterística"
                                End If
                                gvControles.EditIndex = -1
                                BindDataViews()
                            End If
                        End If
                    End If
            End Select
            
        End If
    End Sub

    ''' <summary>
    ''' Cancelar la edición del valor de la característica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvControles_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvControles.EditIndex = -1
        BindDataViews()        
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvControles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvControles.RowDataBound
        Try
            'Sólo se tienen en cuenta las filas con datos
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim imagenCarac As Image = CType(e.Row.FindControl("imgCaracteristicaValor"), Image)
                Dim valorCarac As Label = CType(e.Row.FindControl("lblCaracteristicaValor"), Label)
                Dim okNok As Label = CType(e.Row.FindControl("lblOkNok"), Label)
                Dim ddlEditarValor As DropDownList = CType(e.Row.FindControl("ddlEditarValor"), DropDownList)
                Dim chkCambiarControl As CheckBox = CType(e.Row.FindControl("chkCambiarControl"), CheckBox)

                If (ddlEditarValor IsNot Nothing) Then
                    ddlEditarValor.ForeColor = Drawing.Color.White
                    ddlEditarValor.Items(0).Attributes.Add("style", "background-color: #62CE00")
                    ddlEditarValor.Items(1).Attributes.Add("style", "background-color: red")
                    Dim lblValor As Label = CType(e.Row.FindControl("lblValor"), Label)
                    Select Case okNok.Text
                        Case "0"
                            If (lblValor.Text.Equals("0")) Then
                                ddlEditarValor.Items(1).Selected = True
                            End If
                        Case "1"
                            If (lblValor.Text.Equals("0")) Then
                                ddlEditarValor.Items(0).Selected = True
                            End If
                    End Select
                End If

                If (chkCambiarControl IsNot Nothing) Then
                    Select Case okNok.Text
                        Case "0"
                            chkCambiarControl.Checked = True
                        Case "1"
                            chkCambiarControl.Checked = False
                    End Select
                End If

                If (imagenCarac IsNot Nothing AndAlso valorCarac IsNot Nothing And okNok IsNot Nothing) Then
                    Select Case okNok.Text
                        Case "0"
                            If (valorCarac.Text.Equals("0")) Then
                                imagenCarac.Visible = True
                                valorCarac.Visible = False
                                imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
                                If (ddlEditarValor IsNot Nothing) Then
                                    ddlEditarValor.Items(1).Selected = True
                                End If
                            Else
                                imagenCarac.Visible = False
                                valorCarac.Visible = True
                                valorCarac.Style.Add("color", "red")
                                valorCarac.Style.Add("font-size", "14px")
                                valorCarac.Style.Add("font-weight", "bold")
                            End If
                        Case "1"
                            If (valorCarac.Text.Equals("0")) Then
                                imagenCarac.Visible = True
                                valorCarac.Visible = False
                                imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                                If (ddlEditarValor IsNot Nothing) Then
                                    ddlEditarValor.Items(0).Selected = True
                                End If
                            Else
                                imagenCarac.Visible = False
                                valorCarac.Visible = True
                                valorCarac.Style.Add("color", "#62CE00")
                                valorCarac.Style.Add("font-size", "14px")
                                valorCarac.Style.Add("font-weight", "bold")
                            End If
                        Case "2"
                            imagenCarac.Visible = False
                            valorCarac.Visible = False
                        Case Else
                            imagenCarac.Visible = False
                            valorCarac.Visible = False
                    End Select
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Abrir modal cuando se quiere adjuntar un fichero
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub imgSubir_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim rowIndex As Integer = DirectCast(DirectCast(DirectCast(sender, System.Web.UI.WebControls.ImageButton).Parent, System.Web.UI.WebControls.DataControlFieldCell).Parent, System.Web.UI.WebControls.GridViewRow).DataItemIndex
            Dim dkCaracteristica As String = gvControles.DataKeys(rowIndex).Value
            hfIdCaracteristica.Value = dkCaracteristica

            Dim dirInfoReales As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntos") & "\" & IdControl & "\" & dkCaracteristica)
            Dim dirInfoTemporales As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID & "\" & dkCaracteristica)
            mostrarAdjuntos(If(dirInfoTemporales.Exists(), dirInfoTemporales, Nothing), If(dirInfoReales.Exists(), dirInfoReales, Nothing))

            mpeModalFicheros.Show()
        Catch ex As Exception
            Master.MensajeError = "Ha ocurrido un error al cargar la ventana modal para poder añadir ficheros a la característica"
        End Try
    End Sub

    ''' <summary>
    ''' Adjuntar un fichero a una acción
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdjuntar_Click(sender As Object, e As EventArgs)
        If (fUpload.HasFile) Then
            '1º Se comprueba si existe el directorio con el idSession e id característica. Si no, se crea
            Dim dirInfo As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID & "\" & hfIdCaracteristica.Value)
            If (Not dirInfo.Exists()) Then dirInfo.Create()
            '2º Se guarda el documento
            fUpload.SaveAs(dirInfo.FullName & "/" & fUpload.FileName)
            BindDataViews()
            mostrarAdjuntos(dirInfo)
            mpeModalFicheros.Show()
        Else
            Master.MensajeAdvertencia = ItzultzaileWeb.Itzuli("Seleccione un fichero primero")
        End If
    End Sub

    ''' <summary>
    ''' Guardar el reporte de error del control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarError_Click(sender As Object, e As EventArgs)        
        If (Not String.IsNullOrEmpty(hfValor.Value) AndAlso Not String.IsNullOrEmpty(hfIdRegistro.Value)) Then
            Dim controlErrores As New ELL.ControlesErrores
            Select Case ddlErrores.SelectedValue
                Case ELL.ControlesErrores.TiposErrores.Validacion
                    controlErrores.Validado = True
                    controlErrores.Reparado = False
                    controlErrores.CambioReferencia = False
                    controlErrores.Comentario = txtComentario.Text.Trim
                    controlErrores.ValidacionUsuario = txtValidadoPor.Text.Trim
                Case ELL.ControlesErrores.TiposErrores.Reparacion
                    controlErrores.Validado = False
                    controlErrores.Reparado = True
                    controlErrores.CambioReferencia = False
                    controlErrores.Comentario = txtComentario.Text.Trim
                Case ELL.ControlesErrores.TiposErrores.Mantenimiento
                    controlErrores.Validado = False
                    controlErrores.Reparado = False
                    controlErrores.CambioReferencia = True
                    controlErrores.Comentario = String.Empty
            End Select
            Dim controlValor As New ELL.ControlesValoresResumen With {.IdControl = CInt(IdControl), .OkNok = "0", .IdRegistro = hfIdRegistro.Value, .Valor = hfValor.Value}
            If (oControlesBLL.ModificarErrorYValorControl(controlErrores, controlValor)) Then
                Master.MensajeInfo = "La carasterística ha sido modificada correctamente"
                gvControles.EditIndex = -1
                BindDataViews()
                AgregarScriptBusquedaUsuarios()
            Else
                Master.MensajeInfo = "Ha ocurrido un error al modificar la carasterística"
            End If
        Else
            Master.MensajeError = "Ha ocurrido un error al modificar la carasterística"
        End If
    End Sub

    ''' <summary>
    ''' Cambio de opción en el dropdownlist del tipo de error
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlErrores_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlErrores.SelectedIndexChanged
        Select Case ddlErrores.SelectedValue
            Case ELL.ControlesErrores.TiposErrores.Validacion
                lblValidadoPor.Visible = True
                txtValidadoPor.Visible = True
                txtComentario.Enabled = True
            Case ELL.ControlesErrores.TiposErrores.Reparacion
                lblValidadoPor.Visible = False
                txtValidadoPor.Visible = False
                txtComentario.Enabled = True
            Case ELL.ControlesErrores.TiposErrores.Mantenimiento
                lblValidadoPor.Visible = False
                txtValidadoPor.Visible = False
                txtComentario.Enabled = False
        End Select
        mpeReporteErrores.Show()
    End Sub

    ''' <summary>
    ''' Eliminar el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEliminarControl_Click(sender As Object, e As EventArgs)
        If Not (String.IsNullOrEmpty(IdControl)) Then
            If (oControlesBLL.EliminarControl(IdControl)) Then
                Master.MensajeInfo = String.Format("Se ha eliminado correctamente el control con Id {0}", IdControl)
                txtIdControl.Text = String.Empty
                BindDataViews()
            Else
                Master.MensajeError = String.Format("Ha ocurrido un Error al eliminar el control con Id {0}", IdControl)
            End If
        Else
            Master.MensajeError = "Ha ocurrido un Error. No se ha podido acceder al Id del control. Salga y vuelva a entrar"
        End If
    End Sub

    ''' <summary>
    ''' Mover los ficheros temporales al directorio destino (el del control y característica correspondiente)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAceptarFicheros_Click(sender As Object, e As EventArgs) Handles btnAceptarFicheros.Click
        Try
            Dim dirInfoDestino As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntos") & IdControl.ToString)
            Dim dirInfoOrigen As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID & "\" & hfIdCaracteristica.Value)

            If (dirInfoOrigen.Exists()) Then
                For Each fichero As System.IO.FileInfo In dirInfoOrigen.GetFiles
                    System.IO.File.Move(fichero.FullName, dirInfoDestino.FullName + "\" + hfIdCaracteristica.Value + "\" + fichero.Name)
                Next
            End If
        Catch ex As Exception
            Master.MensajeError = String.Format("Ha ocurrido un Error al guardar los ficheros. Inténtelo de nuevo.")
        End Try
    End Sub

    ''' <summary>
    ''' Databound del DetailsView de errores
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dvErrores_DataBound(sender As Object, e As EventArgs) Handles dvErrores.DataBound
        If (dvErrores.DataItem IsNot Nothing) Then
            Dim rbListTipoError As RadioButtonList = CType(dvErrores.FindControl("rbListTipoError"), RadioButtonList)
            Dim control As ELL.ControlesErrores = oControlesBLL.ObtenerControlErrores(txtIdControl.Text.Trim)
            If (control IsNot Nothing) Then
                If (rbListTipoError IsNot Nothing) Then
                    If (control.Validado) Then
                        rbListTipoError.Items(0).Selected = True
                        rbListTipoError.Items(1).Selected = False
                        rbListTipoError.Items(2).Selected = False
                    ElseIf (control.Reparado) Then
                        rbListTipoError.Items(0).Selected = False
                        rbListTipoError.Items(1).Selected = True
                        rbListTipoError.Items(2).Selected = False
                    ElseIf (control.CambioReferencia) Then
                        rbListTipoError.Items(0).Selected = False
                        rbListTipoError.Items(1).Selected = False
                        rbListTipoError.Items(2).Selected = True
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Guardar los datos de los errores
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarErrores_Click(sender As Object, e As EventArgs)
        Dim controlErrores As New ELL.ControlesErrores
        Dim oUsuariosComponentBLL As New BLL.UsuariosComponent
        Try
            Dim hfIdUsuario As HiddenField = CType(dvErrores.FindControl("hfIdUsuario"), HiddenField)
            Dim txtComentario As TextBox = CType(dvErrores.FindControl("txtComentario"), TextBox)
            Dim rbListTipoError As RadioButtonList = CType(dvErrores.FindControl("rbListTipoError"), RadioButtonList)

            controlErrores.IdControl = ViewState("IdControl")
            Select Case rbListTipoError.SelectedIndex + 1
                Case ELL.ControlesErrores.TiposErrores.Validacion
                    If (Not String.IsNullOrEmpty(hfIdUsuario.Value)) Then
                        controlErrores.Validado = True
                        controlErrores.Reparado = False
                        controlErrores.CambioReferencia = False
                        controlErrores.Comentario = txtComentario.Text.Trim
                        controlErrores.ValidacionUsuario = oUsuariosComponentBLL.GetUsuario(New ELL.Usuario With {.Id = hfIdUsuario.Value}).CodPersona
                    Else
                        Master.MensajeError = "Ha ocurrido un Error con los datos del usuario. Vuelva a introducir el nombre del validador"
                    End If
                Case ELL.ControlesErrores.TiposErrores.Reparacion
                    controlErrores.Validado = False
                    controlErrores.Reparado = True
                    controlErrores.CambioReferencia = False
                    controlErrores.Comentario = txtComentario.Text.Trim
                    controlErrores.ValidacionUsuario = Integer.MinValue
                Case ELL.ControlesErrores.TiposErrores.Mantenimiento
                    controlErrores.Validado = False
                    controlErrores.Reparado = False
                    controlErrores.CambioReferencia = True
                    controlErrores.Comentario = String.Empty
                    controlErrores.ValidacionUsuario = Integer.MinValue
            End Select
            If (oControlesBLL.ModificarError(controlErrores)) Then
                Master.MensajeInfo = "Los datos de Error del control se han guardado satisfactoriamente"
            Else
                Master.MensajeError = "Ha ocurrido un Error al modificar los datos de Error del control . Inténtelo de nuevo"
            End If
        Catch ex As Exception
            Master.MensajeError = "Ha ocurrido un Error al modificar los datos de Error del control . Inténtelo de nuevo"
        End Try        
    End Sub

    ''' <summary>
    ''' Cambio de Index del RadioButtonList
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rbListTipoError_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rbListTipoError As RadioButtonList = CType(dvErrores.FindControl("rbListTipoError"), RadioButtonList)
        If (rbListTipoError IsNot Nothing) Then
            Dim txtComentario As TextBox = CType(dvErrores.FindControl("txtComentario"), TextBox)
            'Dim txtInsertUsuario As TextBox = CType(dvErrores.FindControl("txtInsertUsuario"), TextBox)
            If (rbListTipoError.Items(0).Selected) Then
                txtComentario.Enabled = True
                dvErrores.Fields(1).Visible = True
            ElseIf (rbListTipoError.Items(1).Selected) Then
                txtComentario.Enabled = True
                dvErrores.Fields(1).Visible = False
            ElseIf (rbListTipoError.Items(2).Selected) Then
                txtComentario.Enabled = False
                dvErrores.Fields(1).Visible = False
            End If
        End If
    End Sub

#Region "REPEATERS"

    ''' <summary>
    ''' Al pulsar en el icono de eliminar en los ficheros temporales
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>                              
    Private Sub rptAdjuntosSubir_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptAdjuntosSubir.ItemCommand
        Try
            If (e.CommandName = "Quitar") Then
                Dim dirInfo As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntosTemp") & Session.SessionID)
                Dim fileDel As IO.FileInfo() = dirInfo.GetFiles(e.CommandArgument, IO.SearchOption.AllDirectories)
                If (fileDel Is Nothing) Then
                    Master.MensajeError = "No se ha encontrado el adjunto a borrar"
                Else
                    fileDel.First.Delete()
                    mostrarAdjuntos(dirInfo, Nothing)
                    mpeModalFicheros.Show()
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = "Error al borrar el adjunto"
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los adjuntos temporales
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptAdjuntosSubir_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptAdjuntosSubir.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As String = e.Item.DataItem
            Dim lblAdjunto As Label = CType(e.Item.FindControl("lblAdjunto"), Label)
            Dim imgQuitarAdj As ImageButton = CType(e.Item.FindControl("imgQuitarAdj"), ImageButton)
            lblAdjunto.Text = item
            imgQuitarAdj.CommandArgument = item
            imgQuitarAdj.CommandName = "Quitar"
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los adjuntos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rptAdjuntos_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptAdjuntos.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As String = e.Item.DataItem
            Dim lblAdjunto As Label = CType(e.Item.FindControl("lblAdjunto"), Label)
            Dim imgQuitarAdj As ImageButton = CType(e.Item.FindControl("imgQuitarAdj"), ImageButton)
            lblAdjunto.Text = item
            imgQuitarAdj.CommandArgument = item
            imgQuitarAdj.CommandName = "Quitar"
            imgQuitarAdj.OnClientClick = "Return confirm('" & ItzultzaileWeb.Itzuli("confirmarEliminar") & "');"
        End If
    End Sub

    ''' <summary>
    ''' Al pulsar en el icono de eliminar en los ficheros
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rptAdjuntos_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptAdjuntos.ItemCommand
        Try
            If (e.CommandName = "Quitar") Then
                Dim dirInfo As New IO.DirectoryInfo(ConfigurationManager.AppSettings("rutaAdjuntos") & IdControl & "\" & hfIdCaracteristica.Value)
                Dim fileDel As IO.FileInfo() = dirInfo.GetFiles(e.CommandArgument, IO.SearchOption.AllDirectories)
                If (fileDel Is Nothing) Then
                    Master.MensajeError = "No se ha encontrado el adjunto a borrar"
                Else
                    fileDel.First.Delete()
                    mostrarAdjuntos(Nothing, dirInfo)
                    mpeModalFicheros.Show()
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = "Error al borrar el adjunto"
        End Try
    End Sub

#End Region

#End Region

End Class