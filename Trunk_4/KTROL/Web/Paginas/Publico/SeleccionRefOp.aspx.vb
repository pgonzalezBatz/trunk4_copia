Public Class _SeleccionRefOp
    Inherits PageBase

#Region "Propiedades"

    ''' <summary>
    ''' Devuelve el perfil de usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property PerfilUsuario() As ELL.PerfilUsuario
        Get
            If (Session("PerfilUsuario") IsNot Nothing) Then
                Dim perfil As ELL.PerfilUsuario = Session("PerfilUsuario")
                Return perfil
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Carga de la página
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim CodOperacion As String = Nothing

        Try
            If Not Page.IsPostBack Then
                CodOperacion = If(Context.Request.QueryString.AllKeys.Contains("CodOperacion"), Context.Request.QueryString("CodOperacion").ToString(), Nothing)
#If DEBUG Then
                If CodOperacion Is Nothing Then CodOperacion = Session("CodOperacion")
#End If

                txtSelOperacion.Focus()
                CargarRolesUsuario()
                'Si se le pasa por url, ponemos los valores en los campos
                'If (Context.Request.QueryString.AllKeys.Contains("CodOperacion")) Then
                If CodOperacion IsNot Nothing Then
                    'txtSelOperacion.Text = Context.Request.QueryString("CodOperacion").ToString()
                    txtSelOperacion.Text = CodOperacion
                    If (Session("Info") IsNot Nothing) Then
                        txtInfoPieza.Text = Session("Info").ToString()
                    End If
                    mostrarDetalle(txtSelOperacion.Text)
                Else
                    inicializarPagina(True)
                    If Not (String.IsNullOrEmpty(txtSelOperacion.Text)) Then
                        CapturarOperacionSeleccionada()
                    End If
                End If
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cargar el combo de los roles de usuario
    ''' </summary>
    Private Sub CargarRolesUsuario()
        Try
            Dim Items As String() = System.Enum.GetNames(GetType(ELL.Usuarios.RolesUsuarioControl))
            For Each item As String In Items
                Dim val = DirectCast([Enum].Parse(GetType(ELL.Usuarios.RolesUsuarioControl), item), ELL.Usuarios.RolesUsuarioControl)
                ddlRoles.Items.Add(New ListItem(System.Text.RegularExpressions.Regex.Replace(item, "([A-Z])", " $1").Trim(), val))
            Next
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar los tipos de roles de usuario que pueden realizar controles", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Viene aqui cuando el control de seleccion de operacion, lanza un evento de operacion no existente
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OperacionNoExistente()
        Try
            pnlGlobal.Attributes.Add("style", "display:none")
            btnAceptar.Attributes.Add("style", "display:none")
        Catch ex As Exception
           Global_asax.log.Error("Error al indicar al usuario que el código de operación no existe", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Evento que surge cuando el control de seleccion de operaciones, lanza un evento de operacion seleccionada
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CapturarOperacionSeleccionada()
        Try
            mostrarDetalle(txtSelOperacion.Text)
        Catch ex As Exception
           Global_asax.log.Error("Error al capturar el códgio de operación seleccionado", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Confirmación de elección de código de operación
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnConfirmarOperacion_Click(sender As Object, e As EventArgs) Handles btnConfirmarOperacion.Click
        'Dim CodigoOperacion As String = txtSelOperacion.Text
        'If (txtSelOperacion.Text <> String.Empty) Then
        '    Dim linqComp As New KaPlanLib.BLL.LinqComponent
        '    Dim regOperacion = consultarCodigoOperacion(CodigoOperacion)
        '    If (regOperacion IsNot Nothing) Then
        '        lblDescripcion.Text = regOperacion.OPERACION_GENERAL & " - " & regOperacion.OPERACION_TIPO
        '        btnCambiarOperacion.Attributes.Add("style", "display:block")
        '        'mostrarDetalle(regOperacion.OPERACION_GENERAL)
        '        CapturarOperacionSeleccionada()
        '    Else
        '        Master.MensajeError = "Codigo de operacion no existente"
        '        OperacionNoExistente()
        '    End If
        'Else
        '    lblDescripcion.Text = String.Empty
        '    OperacionNoExistente()
        '    Master.MensajeError = "Debes introducir un código de operación"
        'End If
        Dim consultasBLL As New BLL.ConsultasBLL
        Dim CodigoOperacion As String = txtSelOperacion.Text
        If (txtSelOperacion.Text <> String.Empty) Then
            Dim linqComp As New KaPlanLib.BLL.LinqComponent
            Dim regOperacion As String() = consultasBLL.consultarCodigoOperacion(CodigoOperacion)
            If (regOperacion IsNot Nothing) Then
                lblDescripcion.Text = regOperacion(1) & " / " & regOperacion(2)
                btnCambiarOperacion.Attributes.Add("style", "display:block")
                'mostrarDetalle(regOperacion.OPERACION_GENERAL)
                CapturarOperacionSeleccionada()
            Else
                Master.MensajeError = "Codigo de operacion no existente"
                OperacionNoExistente()
            End If
        Else
            lblDescripcion.Text = String.Empty
            OperacionNoExistente()
            Master.MensajeError = "Debes introducir un código de operación"
        End If
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        If (Session("Ticket") Is Nothing OrElse Session("PerfilUsuario") Is Nothing) Then
			Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
		End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles de la pagina
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub inicializarPagina(ByVal operacionOK As Boolean)
        Try
            'Inicializamos todas las variables de sesión a vacío
            Session("CodOperacion") = Nothing
            Session("DescripcionOpe") = Nothing
            Session("OperacionGeneral") = Nothing
            Session("IdSeccionOpe") = Nothing
            Session("EsValidoOpe") = Nothing
            Session("DescripcionRef") = Nothing
            Session("EsValidaRef") = Nothing
            'Variables de sesión que yo guardo
            Session("Valores") = Nothing
            Session("Registros") = Nothing
            Session("esOperario") = Nothing

            txtSelOperacion.Focus()
            divInfoPieza.Attributes.Add("style", "display:block")
            pnlGlobal.Attributes.Add("style", "display:none")
            pnlReparacion.Attributes.Add("style", "display:none")
            btnAceptar.Attributes.Add("style", "display:none")
            btnCambiarOperacion.Attributes.Add("style", "display:none")

            If Not operacionOK Then
                divInfoPieza.Attributes.Add("style", "display:none")
                pnlGlobal.Attributes.Add("style", "display:none")
                pnlReparacion.Attributes.Add("style", "display:block")
                btnAceptar.Attributes.Add("style", "display:none")
                btnCambiarOperacion.Attributes.Add("style", "display:none")
            End If

        Catch ex As Exception
           Global_asax.log.Error("Error al inicializar la pagina", ex)
        End Try
    End Sub

#End Region

#Region "Funciones y Procesos"

    ''' <summary>
    ''' Carga los datos del codigo de operacion seleccionado
    ''' </summary>
    ''' <param name="cod"></param>
    ''' <remarks></remarks>
    Private Sub mostrarDetalle(ByVal cod As String)
        Dim oControlesBLL As New BLL.ControlesBLL
        Try
            If (cod = String.Empty) Then
                inicializarPagina(True)
            Else
                Dim perfil As ELL.PerfilUsuario = PerfilUsuario()
                If (perfil IsNot Nothing) Then
                    'Comprobamos que el último control realizado no se haya abierto parte a mantenimiento y el usuario no es gestor
                    If (oControlesBLL.UltimoControlConErrores(cod)) Then
                        'Ha habido errores con el último control de esta operación
                        If (perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Administrador OrElse
                            perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Gestor OrElse
                            perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Operario) Then
                            pnlGlobal.Attributes.Add("style", "display:block")
                            pnlReparacion.Attributes.Add("style", "display:block")
                            pnlReparacion.Attributes.Add("class", "MensajeAdvertenciaControl")
                            lblReparacion.Text = "En el último control realizado de esta operación se abrió un parte a mantenimiento. En caso de realizar un control con todas sus características OK, se dará por validado el último control." ' "En el último control realizado de esta operación se abrió un parte a mantenimiento. Debes validar este control si deseas que se puedan hacer más controles sobre esta operación"
                            divInfoPieza.Attributes.Add("style", "display:block")
                            btnAceptar.Attributes.Add("style", "display:block")
                            btnCambiarOperacion.Attributes.Add("style", "display:block")
                            txtSelOperacion.Enabled = False
                            btnConfirmarOperacion.Enabled = False
                            divControles.Attributes.Add("style", "display:block")
                            divOperario.Attributes.Add("style", "display:none")
                            BindDivControles(True)
                            BindDivOperario(True)
                        Else
                            divInfoPieza.Attributes.Add("style", "display:none")
                            pnlGlobal.Attributes.Add("style", "display:none")
                            pnlReparacion.Attributes.Add("style", "display:block")
                            pnlReparacion.Attributes.Add("class", "MensajeErrorControl")
                            btnAceptar.Attributes.Add("style", "display:none")
                            btnCambiarOperacion.Attributes.Add("style", "display:none")
                            lblReparacion.Text = "En el último control realizado de esta operación se abrió un parte de mantenimiento y no se podrá realizar otro control hasta que un gestor u operario lo valide"
                            divControles.Attributes.Add("style", "display:none")
                            divOperario.Attributes.Add("style", "display:none")
                            BindDivControles(False)
                            BindDivOperario(False)
                        End If

                        'Excepciones
                        If (perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Administrador) Then
                            divAdministrador.Visible = True
                        Else
                            divAdministrador.Visible = False
                        End If

                        If (perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Operario) Then
                            divControles.Attributes.Add("style", "display:none")
                        End If

                        If (perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Administrador OrElse perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Gestor) Then
                            divGestor.Visible = True
                            administrarGestor("gestor")
                        Else
                            divGestor.Visible = False
                        End If
                    Else
                        'No ha habido errores con el último control de esta operación

                        'Si es administrador mostramos la capa correspondiente
                        If (perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Administrador) Then
                            divAdministrador.Visible = True
                            divGestor.Visible = True

                            'Calendar1.Visible = True
                            'Calendar1.Enabled = True
                            'Calendar1.SelectedDate = Date.Today
                            'TextBox1.Text = Date.Today.ToShortDateString
                            'TextBox1.Enabled = True

                            If (ddlRoles IsNot Nothing) Then
                                If (ddlRoles.SelectedValue = ELL.Usuarios.RolesUsuario.Calidad OrElse ddlRoles.SelectedValue = ELL.Usuarios.RolesUsuario.Gestor) Then
                                    divControles.Attributes.Add("style", "display:block")
                                    divOperario.Attributes.Add("style", "display:none")
                                    BindDivControles(True)
                                    BindDivOperario(True)
                                Else
                                    divControles.Attributes.Add("style", "display:none")
                                    divOperario.Attributes.Add("style", "display:none")
                                    BindDivControles(False)
                                    BindDivOperario(False)

                                    'Calendar1.Enabled = False
                                    'TextBox1.Enabled = False
                                End If
                            End If
                        ElseIf (perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Calidad) Then
                            'Si es de calidad mostramos la capa correspondiente
                            divControles.Attributes.Add("style", "display:block")
                            divOperario.Attributes.Add("style", "display:none")
                            BindDivControles(True)
                            BindDivOperario(True)
                            divAdministrador.Visible = False
                            divGestor.Visible = False
                        ElseIf (perfil.IdTipoTrabajador = ELL.Usuarios.RolesUsuario.Gestor) Then
                            'Si es de gestor mostramos la capa correspondiente
                            divControles.Attributes.Add("style", "display:block")
                            divOperario.Attributes.Add("style", "display:none")
                            BindDivControles(True)
                            BindDivOperario(True)
                            divAdministrador.Visible = False
                            divGestor.Visible = True
                        Else
                            divControles.Attributes.Add("style", "display:none")
                            divOperario.Attributes.Add("style", "display:none")
                            BindDivControles(False)
                            BindDivOperario(False)
                            divAdministrador.Visible = False
                            divGestor.Visible = False
                        End If

                        'Cargar los paneles y botones
                        pnlGlobal.Attributes.Add("style", "display:block")
                        pnlReparacion.Attributes.Add("style", "display:none")
                        divInfoPieza.Attributes.Add("style", "display:block")
                        btnAceptar.Attributes.Add("style", "display:block")
                        btnCambiarOperacion.Attributes.Add("style", "display:block")
                        txtSelOperacion.Enabled = False
                        btnConfirmarOperacion.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
           Global_asax.log.Error("Error al mostrar el detalle", ex)
        End Try
    End Sub

    '''' <summary>
    '''' Bind el gridview de calidad
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub BindDivControles(ByVal bind As Boolean)
    '    If (bind) Then
    '        Dim listaCaracteristicas As List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
    '        listaCaracteristicas = cargarCodigosOperacionCalidadGestor(txtSelOperacion.Text)
    '        If (listaCaracteristicas.Count > 0) Then
    '            gvCaracteristicasCalidadGestor.DataSource = listaCaracteristicas
    '            gvCaracteristicasCalidadGestor.DataBind()
    '        Else
    '            gvCaracteristicasCalidadGestor.DataSource = Nothing
    '            gvCaracteristicasCalidadGestor.DataBind()
    '        End If
    '    Else
    '        gvCaracteristicasCalidadGestor.DataSource = Nothing
    '        gvCaracteristicasCalidadGestor.DataBind()
    '    End If
    'End Sub    

    ''' <summary>
    ''' Bind el gridview de calidad
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindDivControles(ByVal bind As Boolean)
        Dim oConsultasBLL As New BLL.ConsultasBLL
        Dim listaCaracteristicas As New List(Of ELL.Caracteristicas_Plan_Fabricacion)

        Select Case PerfilUsuario.IdTipoTrabajador
            Case ELL.Usuarios.RolesUsuario.Administrador
                Select Case CInt(ddlRoles.SelectedValue)
                    Case ELL.Usuarios.RolesUsuarioControl.Calidad
                        If (bind) Then
                            listaCaracteristicas = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(txtSelOperacion.Text, ELL.Usuarios.RolesUsuarioControl.Calidad)
                            gvCaracteristicasCalidadGestor.DataSource = listaCaracteristicas
                            gvCaracteristicasCalidadGestor.DataBind()
                        Else
                            gvCaracteristicasCalidadGestor.DataSource = Nothing
                            gvCaracteristicasCalidadGestor.DataBind()
                        End If
                    Case ELL.Usuarios.RolesUsuarioControl.Gestor
                        If (bind) Then
                            listaCaracteristicas = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(txtSelOperacion.Text, ELL.Usuarios.RolesUsuarioControl.Gestor)
                            gvCaracteristicasCalidadGestor.DataSource = listaCaracteristicas
                            gvCaracteristicasCalidadGestor.DataBind()
                        Else
                            gvCaracteristicasCalidadGestor.DataSource = Nothing
                            gvCaracteristicasCalidadGestor.DataBind()
                        End If
                End Select
            Case ELL.Usuarios.RolesUsuario.Calidad
                If (bind) Then
                    listaCaracteristicas = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(txtSelOperacion.Text, ELL.Usuarios.RolesUsuarioControl.Calidad)
                    gvCaracteristicasCalidadGestor.DataSource = listaCaracteristicas
                    gvCaracteristicasCalidadGestor.DataBind()
                Else
                    gvCaracteristicasCalidadGestor.DataSource = Nothing
                    gvCaracteristicasCalidadGestor.DataBind()
                End If
            Case ELL.Usuarios.RolesUsuario.Gestor
                If (bind) Then
                    listaCaracteristicas = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(txtSelOperacion.Text, ELL.Usuarios.RolesUsuarioControl.Gestor)
                    gvCaracteristicasCalidadGestor.DataSource = listaCaracteristicas
                    gvCaracteristicasCalidadGestor.DataBind()
                Else
                    gvCaracteristicasCalidadGestor.DataSource = Nothing
                    gvCaracteristicasCalidadGestor.DataBind()
                End If
            Case ELL.Usuarios.RolesUsuario.Operario
                If (bind) Then
                    listaCaracteristicas = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(txtSelOperacion.Text, ELL.Usuarios.RolesUsuarioControl.Operario)
                    gvCaracteristicasOperario.DataSource = listaCaracteristicas
                    gvCaracteristicasOperario.DataBind()
                Else
                    gvCaracteristicasOperario.DataSource = Nothing
                    gvCaracteristicasOperario.DataBind()
                End If
        End Select
    End Sub

    '''' <summary>
    '''' Bind el gridview de calidad
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub BindDivOperario(ByVal bind As Boolean)
    '    If (bind) Then
    '        Dim listaCaracteristicas As List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
    '        listaCaracteristicas = cargarCodigosOperacionOperario(txtSelOperacion.Text)
    '        If (listaCaracteristicas.Count > 0) Then
    '            gvCaracteristicasOperario.DataSource = listaCaracteristicas
    '            gvCaracteristicasOperario.DataBind()
    '        End If
    '    Else
    '        gvCaracteristicasOperario.DataSource = Nothing
    '        gvCaracteristicasOperario.DataBind()
    '    End If
    'End Sub

    ''' <summary>
    ''' Bind el gridview de calidad
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindDivOperario(ByVal bind As Boolean)
        If (bind) Then
            Dim oConsultasBLL As New BLL.ConsultasBLL
            Dim listaCaracteristicas As New List(Of ELL.Caracteristicas_Plan_Fabricacion)

            listaCaracteristicas = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(txtSelOperacion.Text, ELL.Usuarios.RolesUsuarioControl.Operario)
            If (listaCaracteristicas.Count > 0) Then
                gvCaracteristicasOperario.DataSource = listaCaracteristicas
                gvCaracteristicasOperario.DataBind()
            End If
        Else
            gvCaracteristicasOperario.DataSource = Nothing
            gvCaracteristicasOperario.DataBind()
        End If
    End Sub
#End Region

#Region "Handlers"

    '''' <summary>
    '''' RowDataBound del grid características de calidad
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Protected Sub gvCaracteristicasCalidad_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCaracteristicasCalidadGestor.RowDataBound
    '    Try
    '        If (e.Row.RowType = DataControlRowType.DataRow) Then
    '            Dim tamañoFoto As New System.Drawing.Size(400, 600)
    '            Dim imgVerAyudaVisual As Image = CType(e.Row.FindControl("imgVerAyudaVisual"), Image)
    '            Dim hlDescargarAyudaVisual As HyperLink = CType(e.Row.FindControl("hlDescargarAyudaVisual"), HyperLink)
    '            Dim imgbDescargarAyudaVisual As Image = CType(e.Row.FindControl("imgbDescargarAyudaVisual"), Image)
    '            Dim idRegistro As Label = CType(e.Row.FindControl("lblId"), Label)
    '            Dim hlVerAyudaVisual As HyperLink = CType(e.Row.FindControl("hlVerAyudaVisual"), HyperLink)

    '            If (imgVerAyudaVisual IsNot Nothing AndAlso hlVerAyudaVisual IsNot Nothing) Then
    '                Dim imagenes As List(Of KaPlanLib.Registro.Archivos) = GetAyudaVisual(idRegistro.Text)
    '                If (imagenes.Count > 0) Then
    '                    tamañoFoto = Utils.GetSizeFromImage(imagenes(0).Archivo.ToArray())
    '                    hlVerAyudaVisual.NavigateUrl = String.Format("ImagenAyudaVisual.aspx?idRegistro={0}&altura={1}", idRegistro.Text, tamañoFoto.Height)
    '                    hlDescargarAyudaVisual.NavigateUrl = String.Format(hlDescargarAyudaVisual.NavigateUrl, idRegistro.Text)
    '                Else
    '                    imgVerAyudaVisual.Visible = False
    '                    imgbDescargarAyudaVisual.Visible = False
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '       Global_asax.log.Error("Error al cargar las características de calidad", ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' RowDataBound del grid características de calidad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvCaracteristicasCalidad_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCaracteristicasCalidadGestor.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim tamañoFoto As New System.Drawing.Size(400, 600)
                Dim imgVerAyudaVisual As Image = CType(e.Row.FindControl("imgVerAyudaVisual"), Image)
                Dim hlDescargarAyudaVisual As HyperLink = CType(e.Row.FindControl("hlDescargarAyudaVisual"), HyperLink)
                Dim imgbDescargarAyudaVisual As Image = CType(e.Row.FindControl("imgbDescargarAyudaVisual"), Image)
                Dim idRegistro As Label = CType(e.Row.FindControl("lblId"), Label)
                Dim hlVerAyudaVisual As HyperLink = CType(e.Row.FindControl("hlVerAyudaVisual"), HyperLink)

                If (imgVerAyudaVisual IsNot Nothing AndAlso hlVerAyudaVisual IsNot Nothing) Then
                    Dim imagenes As ELL.AyudaVisual = GetAyudaVisual(idRegistro.Text)
                    If (imagenes IsNot Nothing) Then
                        tamañoFoto = Utils.GetSizeFromImage(imagenes.ARCHIVO.ToArray())
                        hlVerAyudaVisual.NavigateUrl = String.Format("ImagenAyudaVisual.aspx?idRegistro={0}&altura={1}", idRegistro.Text, tamañoFoto.Height)
                        hlDescargarAyudaVisual.NavigateUrl = String.Format(hlDescargarAyudaVisual.NavigateUrl, idRegistro.Text)
                    Else
                        imgVerAyudaVisual.Visible = False
                        imgbDescargarAyudaVisual.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar las características de calidad", ex)
        End Try
    End Sub

    '''' <summary>
    '''' RowDataBound del grid características de operario
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Protected Sub gvCaracteristicasOperario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCaracteristicasOperario.RowDataBound
    '    Try
    '        If (e.Row.RowType = DataControlRowType.DataRow) Then
    '            Dim tamañoFoto As New System.Drawing.Size(400, 600)
    '            Dim imgVerAyudaVisual As Image = CType(e.Row.FindControl("imgVerAyudaVisual"), Image)
    '            Dim hlDescargarAyudaVisual As HyperLink = CType(e.Row.FindControl("hlDescargarAyudaVisual"), HyperLink)
    '            Dim idRegistro As Label = CType(e.Row.FindControl("lblId"), Label)
    '            Dim hlVerAyudaVisual As HyperLink = CType(e.Row.FindControl("hlVerAyudaVisual"), HyperLink)

    '            If (imgVerAyudaVisual IsNot Nothing AndAlso hlVerAyudaVisual IsNot Nothing) Then
    '                Dim imagenes As List(Of KaPlanLib.Registro.Archivos) = GetAyudaVisual(idRegistro.Text)
    '                If (imagenes.Count > 0) Then
    '                    tamañoFoto = Utils.GetSizeFromImage(imagenes(0).Archivo.ToArray())
    '                    hlVerAyudaVisual.NavigateUrl = String.Format("ImagenAyudaVisual.aspx?idRegistro={0}&altura={1}", idRegistro.Text, tamañoFoto.Height)
    '                    hlDescargarAyudaVisual.NavigateUrl = String.Format(hlDescargarAyudaVisual.NavigateUrl, idRegistro.Text)
    '                Else
    '                    imgVerAyudaVisual.Visible = False
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '       Global_asax.log.Error("Error al cargar las características de calidad", ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' RowDataBound del grid características de operario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvCaracteristicasOperario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCaracteristicasOperario.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim tamañoFoto As New System.Drawing.Size(400, 600)
                Dim imgVerAyudaVisual As Image = CType(e.Row.FindControl("imgVerAyudaVisual"), Image)
                Dim hlDescargarAyudaVisual As HyperLink = CType(e.Row.FindControl("hlDescargarAyudaVisual"), HyperLink)
                Dim idRegistro As Label = CType(e.Row.FindControl("lblId"), Label)
                Dim hlVerAyudaVisual As HyperLink = CType(e.Row.FindControl("hlVerAyudaVisual"), HyperLink)

                If (imgVerAyudaVisual IsNot Nothing AndAlso hlVerAyudaVisual IsNot Nothing) Then
                    Dim imagenes As ELL.AyudaVisual = GetAyudaVisual(idRegistro.Text)
                    If (imagenes IsNot Nothing) Then
                        tamañoFoto = Utils.GetSizeFromImage(imagenes.ARCHIVO.ToArray())
                        hlVerAyudaVisual.NavigateUrl = String.Format("ImagenAyudaVisual.aspx?idRegistro={0}&altura={1}", idRegistro.Text, tamañoFoto.Height)
                        hlDescargarAyudaVisual.NavigateUrl = String.Format(hlDescargarAyudaVisual.NavigateUrl, idRegistro.Text)
                    Else
                        imgVerAyudaVisual.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar las características de calidad", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cambio de index del DropDownList de roles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlRoles_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlRoles.SelectedIndex <> -1) Then
            Select Case CInt(ddlRoles.SelectedValue)
                Case ELL.Usuarios.RolesUsuarioControl.Operario
                    divControles.Attributes.Add("style", "display:none")
                    divOperario.Attributes.Add("style", "display:none")
                    BindDivControles(False)
                    BindDivOperario(False)

                    'Calendar1.Enabled = False
                    'TextBox1.Enabled = False
                    'TextBox1.Text = Date.Today.ToShortDateString
                Case ELL.Usuarios.RolesUsuarioControl.Calidad, ELL.Usuarios.RolesUsuarioControl.Gestor
                    divControles.Attributes.Add("style", "display:block")
                    divOperario.Attributes.Add("style", "display:none")
                    BindDivControles(True)
                    BindDivOperario(True)

                    'Calendar1.Enabled = True
                    'TextBox1.Enabled = True
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Cambiar de operación
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCambiarOperacion_Click(sender As Object, e As EventArgs)
        btnConfirmarOperacion.Enabled = True
        txtSelOperacion.Enabled = True
        lblDescripcion.Text = String.Empty
        inicializarPagina(True)
    End Sub

    ''' <summary>
    ''' Guardamos los datos seleccionados y redirigimos a la pantalla de control correcta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
		Try
			'-------------------------------------------------------------------------------------------------------------
			'Comprobamos que los metodos de control de las caracterisitcas seleccionadas son por "Atributo" o "Variable"
			'-------------------------------------------------------------------------------------------------------------
			For Each gvRow As GridViewRow In gvCaracteristicasCalidadGestor.Rows
				Dim chk As CheckBox = DirectCast(gvRow.FindControl("chkCaracteristica"), CheckBox)
				If (chk.Checked) Then
					Dim METODO_CONTROL As String = gvCaracteristicasCalidadGestor.DataKeys(gvRow.RowIndex).Values(Array.Find(gvCaracteristicasCalidadGestor.DataKeyNames, Function(o) String.Compare(o, "METODO_CONTROL", True) = 0)).ToString.ToLower
					Dim POSICION As String = gvCaracteristicasCalidadGestor.DataKeys(gvRow.RowIndex).Values(Array.Find(gvCaracteristicasCalidadGestor.DataKeyNames, Function(o) String.Compare(o, "POSICION", True) = 0)).ToString.ToLower
					If Not (METODO_CONTROL.Contains("atributos") OrElse METODO_CONTROL.Contains("atr") _
						OrElse METODO_CONTROL.Contains("variables") OrElse METODO_CONTROL.Contains("var")) Then
						Dim msg As String = String.Format("Metodo de control '{0}' no valido para la caracteristica con posicion '{1}'", METODO_CONTROL.ToUpper, POSICION.ToUpper)
						Throw New ApplicationException(msg)
					End If
				End If
			Next
			'-------------------------------------------------------------------------------------------------------------

			Dim control As String = String.Empty
			If (PerfilUsuario IsNot Nothing) Then
				Session("Info") = txtInfoPieza.Text.Trim()
				Session("CodOperacion") = txtSelOperacion.Text.Trim()
				Session("DescripcionOpe") = lblDescripcion.Text.Trim()
				Select Case PerfilUsuario.IdTipoTrabajador
					Case ELL.Usuarios.RolesUsuario.Administrador
						If (ddlRoles.SelectedIndex <> -1) Then
							Dim rolSeleccionado As Integer = ddlRoles.SelectedValue()
							If (rolSeleccionado.Equals(ELL.Usuarios.RolesUsuarioControl.Operario)) Then
								Response.Redirect("PasosOperario.aspx", False)
							ElseIf (rolSeleccionado = ELL.Usuarios.RolesUsuarioControl.Calidad OrElse rolSeleccionado = ELL.Usuarios.RolesUsuarioControl.Gestor) Then
								Dim countCar As Integer = 0
								For Each gvRow In gvCaracteristicasCalidadGestor.Rows
									Dim chk As CheckBox = TryCast(gvRow.FindControl("chkCaracteristica"), CheckBox)
									Dim lbl As Label = TryCast(gvRow.FindControl("lblId"), Label)
									If (chk.Checked) Then
										countCar += 1
										control += lbl.Text + ","
									End If
								Next
								If (countCar = 0) Then
									Throw New ApplicationException("Debes seleccionar por lo menos una característica")
								Else
									'Quitamos la última coma
									control = control.Substring(0, control.Length - 1)
									Session("Registros") = control
									If (rolSeleccionado = ELL.Usuarios.RolesUsuarioControl.Calidad) Then
										Response.Redirect("PasosCalidad.aspx", False)
									Else
										Response.Redirect("PasosGestor.aspx", False)
									End If
								End If
							End If
						End If
					Case ELL.Usuarios.RolesUsuario.Calidad
						Dim countCar As Integer = 0
						For Each gvRow In gvCaracteristicasCalidadGestor.Rows
							Dim chk As CheckBox = TryCast(gvRow.FindControl("chkCaracteristica"), CheckBox)
							Dim lbl As Label = TryCast(gvRow.FindControl("lblId"), Label)
							If (chk.Checked) Then
								countCar += 1
								control += lbl.Text + ","
							End If
						Next
						If (countCar = 0) Then
							Throw New ApplicationException("Debes seleccionar por lo menos una característica")
						Else
							'Quitamos la última coma
							control = control.Substring(0, control.Length - 1)
							Session("Registros") = control
							Response.Redirect("PasosCalidad.aspx", False)
						End If
					Case ELL.Usuarios.RolesUsuario.Gestor
						If (hfTipoControl.Value.Equals("1")) Then
							'El tipo de control seleccionado es el del gestor.
							Dim countCar As Integer = 0
							For Each gvRow In gvCaracteristicasCalidadGestor.Rows
								Dim chk As CheckBox = TryCast(gvRow.FindControl("chkCaracteristica"), CheckBox)
								Dim lbl As Label = TryCast(gvRow.FindControl("lblId"), Label)
								If (chk.Checked) Then
									countCar += 1
									control += lbl.Text + ","
								End If
							Next
							If (countCar = 0) Then
								Throw New ApplicationException("Debes seleccionar por lo menos una característica")
							Else
								'Quitamos la última coma
								control = control.Substring(0, control.Length - 1)
								Session("Registros") = control
								Session("esOperario") = Nothing
								Response.Redirect("PasosGestor.aspx", False)
							End If
						Else
							'El tipo de control seleccionado es el del operario.
							Dim listaCaracteristicas As New List(Of ELL.Caracteristicas_Plan_Fabricacion)
							Dim oConsultasBLL As New BLL.ConsultasBLL
							Dim registros As String = String.Empty

							listaCaracteristicas = oConsultasBLL.cargarDatosCodigosOperacionFabricacionPorRol(txtSelOperacion.Text, ELL.Usuarios.RolesUsuarioControl.Operario)
							For Each caracteristica In listaCaracteristicas
								registros &= caracteristica.ID_REGISTRO & ","
							Next
							registros = registros.Substring(0, registros.Length - 1)
							Session("Registros") = registros
							Session("esOperario") = 1
							Response.Redirect("PasosGestor.aspx", False)
						End If
					Case ELL.Usuarios.RolesUsuario.Operario
						Response.Redirect("PasosOperario.aspx", False)
				End Select
			Else
				'Se hace el logout
				Session("PerfilUsuario") = Nothing
				Response.Redirect("~/Login.aspx", False)
			End If
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub

#End Region

#Region "Consultas KaPlan"

    '''' <summary>
    '''' Devuelve el tipo de operacion
    '''' </summary>
    '''' <param name="codOpe">Codigo de operacion</param>
    '''' <returns></returns>
    'Function consultarCodigoOperacion(ByVal codOpe As String)
    '    Try
    '        Dim BBDD As New KaPlanLib.DAL.ELL

    '        Dim OperacionTipo As KaPlanLib.Registro.OPERACIONES_TIPO =
    '                (From Opt As KaPlanLib.Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO
    '                 Join Art As KaPlanLib.Registro.OPERACIONES_DE_UN_ARTICULO In BBDD.OPERACIONES_DE_UN_ARTICULO On Opt.COD_OPERACION Equals Art.COD_OPERACION
    '                 Where Opt.COD_OPERACION = codOpe Select Opt Distinct).SingleOrDefault

    '        Return OperacionTipo
    '    Catch ex As ApplicationException
    '        Throw ex
    '    Catch ex As Exception
    '       Global_asax.log.Error("Error en consultarCodigoOperacion", ex)
    '    End Try
    'End Function

    '''' <summary>
    '''' Carga las lineas de las caracteristicas del plan para calidad o gestor
    '''' </summary>
    '''' <remarks></remarks>
    'Private Function cargarCodigosOperacionCalidadGestor(ByVal codigoOperacion As String) As List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
    '    Dim lista As New List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
    '    Try
    '        Dim conexion As New KaPlanLib.DAL.ELL

    '        Select Case PerfilUsuario.IdTipoTrabajador
    '            Case ELL.Usuarios.RolesUsuario.Administrador
    '                Select Case CInt(ddlRoles.SelectedValue)
    '                    Case ELL.Usuarios.RolesUsuarioControl.Calidad
    '                        lista = (From reg As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION In conexion.CARACTERISTICAS_DEL_PLAN_FABRICACION
    '                                 Where reg.CODIGO = codigoOperacion AndAlso reg.Responsable_Reg_Cal = True Order By reg.ORDEN_CARAC Ascending Select reg).ToList()
    '                    Case ELL.Usuarios.RolesUsuarioControl.Gestor
    '                        lista = (From reg As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION In conexion.CARACTERISTICAS_DEL_PLAN_FABRICACION
    '                                 Where reg.CODIGO = codigoOperacion AndAlso reg.Responsable_Reg_Gestor = True Order By reg.ORDEN_CARAC Ascending Select reg).ToList()
    '                End Select
    '            Case ELL.Usuarios.RolesUsuario.Calidad
    '                lista = (From reg As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION In conexion.CARACTERISTICAS_DEL_PLAN_FABRICACION
    '                         Where reg.CODIGO = codigoOperacion AndAlso reg.Responsable_Reg_Cal = True Order By reg.ORDEN_CARAC Ascending Select reg).ToList()
    '            Case ELL.Usuarios.RolesUsuario.Gestor
    '                lista = (From reg As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION In conexion.CARACTERISTICAS_DEL_PLAN_FABRICACION
    '                         Where reg.CODIGO = codigoOperacion AndAlso reg.Responsable_Reg_Gestor = True Order By reg.ORDEN_CARAC Ascending Select reg).ToList()
    '        End Select
    '    Catch ex As Exception
    '        Master.MensajeError = "Error al cargar las caracteristicas de calidad"
    '    End Try
    '    Return lista
    'End Function

    '''' <summary>
    '''' Carga las lineas de las caracteristicas del plan para operario
    '''' </summary>
    '''' <remarks></remarks>
    'Private Function cargarCodigosOperacionOperario(ByVal codigoOperacion As String) As List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
    '    Dim lista As New List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
    '    Try
    '        Dim conexion As New KaPlanLib.DAL.ELL

    '        lista = (From reg As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION In conexion.CARACTERISTICAS_DEL_PLAN_FABRICACION
    '                 Where reg.CODIGO = codigoOperacion AndAlso reg.Responsable_Reg_Ope = True And (reg.Responsable_Reg_Gestor Is Nothing OrElse reg.Responsable_Reg_Gestor = False) Order By reg.ORDEN_CARAC Ascending Select reg).ToList()
    '    Catch ex As Exception
    '        Master.MensajeError = "Error al cargar las caracteristicas de operario"
    '    End Try
    '    Return lista
    'End Function

    '''' <summary>
    '''' Obtiene la imagen de la caracerística
    '''' </summary>
    '''' <param name="idRegistro">Identificador del registro</param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function GetAyudaVisual(ByVal idRegistro As Integer) As List(Of KaPlanLib.Registro.Archivos)
    '    Dim registro As New List(Of KaPlanLib.Registro.Archivos)
    '    Try
    '        Dim BBDD As New KaPlanLib.DAL.ELL

    '        registro = (From reg As KaPlanLib.Registro.Archivos In BBDD.Archivos
    '                    Join ArchivosFab In BBDD.Archivos_Caracteristicas_Plan_FAB On reg.ID Equals ArchivosFab.Id_Archivo
    '                    Join caracPlan In BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION On ArchivosFab.Id_Carac_Plan Equals caracPlan.ID_REGISTRO
    '                    Where caracPlan.ID_REGISTRO = idRegistro Select reg).ToList()
    '    Catch ex As Exception
    '       Global_asax.log.Error("Error al cargar el registro con Id " & idRegistro.ToString() + " ", ex)
    '    End Try
    '    Return registro
    'End Function

    Private Function GetAyudaVisual(ByVal idRegistro As Integer) As ELL.AyudaVisual
        Dim consultasBLL As New BLL.ConsultasBLL
        Return consultasBLL.cargarAyudaVisual(idRegistro)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnAdministracionGestor_Command(sender As Object, e As CommandEventArgs)
        'Select Case e.CommandName.ToLower
        '    Case "gestor"
        '        hfTipoControl.Value = "1"
        '        btnControlesOperario.CssClass = ""
        '        btnControlesGestor.CssClass = "controlGestorActivo"
        '        divControles.Attributes.Add("style", "display:block")
        '        divOperario.Attributes.Add("style", "display:block")
        '        BindDivControles(True)
        '        BindDivOperario(True)
        '    Case "operario"
        '        hfTipoControl.Value = "0"
        '        btnControlesGestor.CssClass = ""
        '        btnControlesOperario.CssClass = "controlGestorActivo"
        '        divControles.Attributes.Add("style", "display:none")
        '        divOperario.Attributes.Add("style", "display:none")
        '        BindDivControles(False)
        '        BindDivOperario(False)
        'End Select
        administrarGestor(e.CommandName.ToLower)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tipo"></param>
    Private Sub administrarGestor(ByVal tipo As String)
        Select Case tipo
            Case "gestor"
                hfTipoControl.Value = "1"
                btnControlesOperario.CssClass = ""
                btnControlesGestor.CssClass = "controlGestorActivo"
                divControles.Attributes.Add("style", "display:block")
                divOperario.Attributes.Add("style", "display:block")
                BindDivControles(True)
                BindDivOperario(True)
            Case "operario"
                hfTipoControl.Value = "0"
                btnControlesGestor.CssClass = ""
                btnControlesOperario.CssClass = "controlGestorActivo"
                divControles.Attributes.Add("style", "display:none")
                divOperario.Attributes.Add("style", "display:none")
                BindDivControles(False)
                BindDivOperario(False)
        End Select
    End Sub

    'Protected Sub Calendar1_SelectionChanged(sender As Object, e As EventArgs) Handles Calendar1.SelectionChanged
    '    TextBox1.Text = Calendar1.SelectedDate.ToLongDateString
    'End Sub

#End Region


End Class