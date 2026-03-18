Imports TelefoniaLib

Partial Public Class Gestor
    Inherits PageBase

    Dim oGestor As ELL.Telefono.GestorTlfno
    Dim gestorComp As BLL.TelefonoComponent.GestorTlfnoComponent

    Private Const NUEVO As String = "nuevoGestor"
    Private Const INFO As String = "infoGestor"

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelGestor) : itzultzaileWeb.Itzuli(btnGuardar)
            itzultzaileWeb.Itzuli(btnEliminar)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de grupos de extensiones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarGestores()
            End If
            ConfigurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para configurar ejecucion de scripts a controles, asignacion de textos,..
    ''' </summary>
    Private Sub ConfigurarEventos()
        Dim sms As String = itzultzaileWeb.Itzuli("confirmarEliminar")
        cfEliminar.ConfirmText = sms
    End Sub

#End Region

#Region "Botones Ver info e nuevo"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de un grupo, donde se puede registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        mostrarDetalle(Integer.MinValue, Integer.MinValue)
    End Sub

    ''' <summary>
    ''' Carga los datos de un gestor
    ''' </summary>
    ''' <param name="idGest">Identificador del gestor</param>
    ''' <param name="idPlanta">Identificador de la planta</param>
    Private Sub mostrarDetalle(ByVal idGest As Integer, ByVal idPlanta As Integer)
        Try
            Dim titulo As String
            inicializarControles()

            If (idGest = Integer.MinValue) Then  'nuevo
                titulo = NUEVO
                ddlGestor.SelectedIndex = 0
                btnGuardar.Visible = True
                btnEliminar.Visible = False
                setPaneles(True, False, False)
            Else 'modificar
                titulo = INFO
                Dim ouser As New SABLib.ELL.Usuario
                Dim userComp As New SABLib.BLL.UsuariosComponent
                ouser.Id = idGest
                lblGestor.Text = userComp.GetUsuario(ouser).NombreCompleto
                btnGuardar.Visible = False
                btnEliminar.Visible = True
                setPaneles(False, True, False)
            End If

            titPopPup.Texto = titulo

            btnEliminar.CommandArgument = idGest            

            mpeGestor.Show()
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos los usuarios con acceso al recurso
    ''' </summary>
    Private Sub cargarGestores()
        Try
            Dim listGestores As List(Of ELL.Telefono.GestorTlfno)
            Dim gestComp As New BLL.TelefonoComponent.GestorTlfnoComponent

            listGestores = gestComp.getGestores(Master.Ticket.IdPlantaActual)

            If (listGestores.Count > 0) Then
                listGestores.Sort(New ELL.Telefono.GestorTlfno.SortClass(GridViewSortExpresion, GridViewSortDirection))
            End If

            gvGestor.DataSource = listGestores
            gvGestor.DataBind()
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarGestores", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga todos los usuarios con acceso al recurso cuya planta, sea la del usuario conectado
    ''' </summary>
    Private Sub cargarUsuarios()
        Try
            If (ddlGestor.Items.Count = 0) Then
                Dim listUsuario As List(Of SABLib.ELL.Usuario)
                Dim oUser As New SABLib.ELL.Usuario
                Dim oPlanta As New SABLib.ELL.Planta
                Dim userComp As New SABLib.BLL.UsuariosComponent

                oUser.Plantas = New List(Of SABLib.ELL.Planta)
                oPlanta.Id = Master.Ticket.IdPlantaActual
                oUser.Plantas.Add(oPlanta)
                oUser.IdPlanta = oPlanta.Id

                listUsuario = userComp.GetUsuariosPlanta(oUser)

                If (listUsuario.Count > 0) Then
                    Ordenar(listUsuario, SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO, SortDirection.Ascending)                    
                End If

                ddlGestor.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno")))

                For Each oUser In listUsuario
                    If (oUser.NombreCompleto.Trim <> String.Empty) Then
                        ddlGestor.Items.Add(New ListItem(oUser.NombreCompleto, oUser.Id))
                    End If
                Next

                ddlGestor.SelectedIndex = 0
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarGestores", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Limpiar controles y gestion paneles"

    ''' <summary>
    ''' Limpia los controles para poder registrar un nuevo item
    ''' </summary>
    Private Sub inicializarControles()
        cargarUsuarios()

        lblGestor.Text = String.Empty        

        setPaneles(False, False, False)
    End Sub

    ''' <summary>
    ''' Visualiza u oculta los paneles parametrizados
    ''' </summary>
    ''' <param name="pNuevo">Visualizar u ocultar el panel de nuevo gestor</param>
    ''' <param name="pExistente">Visualizar u ocultar el panel de un gestor exitente</param>
    ''' <param name="pError">Visualizar u ocultar el panel de error</param>
    Private Sub setPaneles(ByVal pNuevo As Boolean, ByVal pExistente As Boolean, ByVal pError As Boolean)
        pnlNuevoG.Visible = pNuevo
        pnlExistenteG.Visible = pExistente
        pnlError.Visible = pError
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el nuevo gestor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            gestorComp = New BLL.TelefonoComponent.GestorTlfnoComponent
            oGestor = New ELL.Telefono.GestorTlfno

            oGestor.IdUsuarioGestor = ddlGestor.SelectedValue
            oGestor.IdPlanta = Master.Ticket.IdPlantaActual

            gestorComp.Save(oGestor)
            Master.MensajeInfo = "gestorGuardado"
            log.Info("Se ha insertado el gestor - " & ddlGestor.SelectedItem.Text)            
            cargarGestores()
        Catch batzEx As BatzException
            mpeGestor.Show()
            WriteTitulo()
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        Catch ex As Exception
            mpeGestor.Show()
            WriteTitulo()
            Dim batzEx As New BatzException("errGuardarGestor", ex)
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Eliminar"

    ''' <summary>
    ''' Se elimina el registro seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            Dim bSave As Boolean = True
            'Se comprueba que se pueda borrar comprobando si esta asociado a algun telefono
            If (pnlExistenteG.Visible) Then
                Dim tlfnoComp As New BLL.TelefonoComponent
                Dim oTlfno As New ELL.Telefono With {.IdUsuarioGestor = btnEliminar.CommandArgument, .IdPlanta = Master.Ticket.IdPlantaActual}
                bSave = (tlfnoComp.getTelefonos(oTlfno).Count = 0)
            End If

            If (bSave) Then
                gestorComp = New BLL.TelefonoComponent.GestorTlfnoComponent
                oGestor = New ELL.Telefono.GestorTlfno
                oGestor.IdUsuarioGestor = btnEliminar.CommandArgument
                oGestor.IdPlanta = Master.Ticket.IdPlantaActual
                gestorComp.Delete(oGestor)
                Master.MensajeInfo = "gestorBorrado"
                log.Info("Se ha quitado al gestor de telefonos - " & lblGestor.Text)
                cargarGestores()
            Else
                mpeGestor.Show()
                WriteTitulo()
                pnlError.Visible = True
                lblError.Text = itzultzaileWeb.Itzuli("noSePuedeBorrarEstaRelacionadoConTelefono")
            End If
        Catch batzEx As BatzException
            mpeGestor.Show()
            WriteTitulo()
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Escribe el titulo de la popup
    ''' </summary>
    Private Sub WriteTitulo()
        If (lblGestor.Visible) Then
            titPopPup.Texto = INFO
        Else
            titPopPup.Texto = NUEVO
        End If
    End Sub

#End Region

#Region "GridView"

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGestor_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGestor.Sorting
        Try
            If (GridViewSortDirection = SortDirection.Ascending) Then
                GridViewSortDirection = SortDirection.Descending
            Else
                GridViewSortDirection = SortDirection.Ascending
            End If

            If (GridViewSortExpresion Is Nothing) Then
                GridViewSortExpresion = "UsuarioGestor"
            Else
                GridViewSortExpresion = e.SortExpression
            End If

            cargarGestores()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Ascending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la expresion de ordenacion
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property GridViewSortExpresion() As String
        Get
            If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = "USUARIOGESTOR"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

    ''' <summary>
    ''' Ordena la lista de gestores
    ''' </summary>
    ''' <param name="lUsuarios">Lista de gestores</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    ''' <remarks></remarks>
    Private Sub Ordenar(ByRef lUsuarios As List(Of SABLib.ELL.Usuario), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr            
            Case SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                lUsuarios.Sort(Function(oUser1 As SABLib.ELL.Usuario, oUser2 As SABLib.ELL.Usuario) _
                                   If(sortDir = SortDirection.Ascending, oUser1.NombreCompleto < oUser2.NombreCompleto, oUser1.NombreCompleto > oUser2.NombreCompleto))
        End Select
    End Sub

#End Region

#Region "Paginación"

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGestor_Paging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGestor.PageIndexChanging
        Try
            gvGestor.PageIndex = e.NewPageIndex
            cargarGestores()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "RowDataBound"

    ''' <summary>
    ''' <para>Añade la gestion de ordenes y estilos al pasar el raton por las lineas</para>
    ''' <para>Marca en verde, aquellos grupos libres</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGestor_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvGestor.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                itzultzaileWeb.TraducirWebControls(e.Row.Controls)
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                Dim oGestor As ELL.Telefono.GestorTlfno = e.Row.DataItem

                'Estilo para que al posicionarse sobre la fila, se pinte de un color
                e.Row.Attributes.Add("onmouseover", "SartuN(this);")
                e.Row.Attributes.Add("onmouseout", "IrtenN(this);")
                e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvGestor, "Select$" + CStr(oGestor.IdUsuarioGestor))
            End If
        Catch ex As Exception
            Dim batz As New BatzException("errMostrarGestores", ex)
        End Try
    End Sub

#End Region

#Region "RowCommand"

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del gestor seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvGestor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGestor.RowCommand
        'Cuando se vaya a ordenar no entrara en el if que solo sirve para redirigir al detalle cuando se pincha en una fila
        Try            
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument), Master.Ticket.IdPlantaActual)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#End Region

End Class