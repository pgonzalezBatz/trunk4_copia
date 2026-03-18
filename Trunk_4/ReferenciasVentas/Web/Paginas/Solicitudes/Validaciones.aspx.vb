Imports System.Net.Mail

Public Class Validaciones
    Inherits PageBase

    Dim solicitudesBLL As New BLL.SolicitudesBLL    
    Dim solicitud As ELL.Solicitudes
    Dim oReferenciasVentaBLL As New BLL.ReferenciaFinalVentaBLL

#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.DocumentationTechnician)
            roles.Add(ELL.Roles.RolUsuario.ProductManager)
            roles.Add(ELL.Roles.RolUsuario.ProjectLeader)
            roles.Add(ELL.Roles.RolUsuario.ProjectLeaderManager)
            Return roles
        End Get
    End Property

    ''' <summary>
    ''' identificador de la incidencia
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdSolicitud() As Integer
        Get
            Return ViewState("IdSolicitud")
        End Get
        Set(ByVal value As Integer)
            ViewState("IdSolicitud") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el identificador del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property IdUser() As Integer
        Get
            Dim persona As New SabLib.ELL.Ticket
            Dim id As Integer = Integer.MinValue

            If (Session("Ticket") IsNot Nothing) Then
                persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
                id = persona.IdUser
            End If
            Return id
        End Get
    End Property
    
    ''' <summary>
    ''' Obtiene el identificador del rol del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property IdRol() As Integer
        Get
            Dim perfil As New ELL.PerfilUsuario
            Dim idRolUsuario As Integer = Integer.MinValue

            If (Session("PerfilUsuario") IsNot Nothing) Then
                perfil = CType(Session("PerfilUsuario"), ELL.PerfilUsuario)
                idRolUsuario = perfil.IdRol
            End If
            Return idRolUsuario
        End Get
    End Property

    ''' <summary>
    ''' Nombre del usuario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property UserName() As String
        Get
            Dim persona As New SabLib.ELL.Ticket

            If (Session("Ticket") IsNot Nothing) Then
                persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
                Return persona.NombreUsuario
            Else
                Return String.Empty
            End If
        End Get
    End Property

#End Region

#Region "Constantes"

    'Private Const KB_MAX_FICHERO As Integer = 1024

#End Region

#Region "Miembros"

    Private m_ViewMode As ViewMode = ViewMode.GridView

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
    ''' Muestra los casos asignados
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

            If Not Page.IsPostBack Then
                If (Request.QueryString("IdSol") IsNot Nothing) Then
                    SetBehavior(ViewMode.DetailsView)
                    IdSolicitud = Request.QueryString("IdSol")
                    BindDataViews(IdSolicitud)
                Else
                    Initialize()
                End If
            End If

            Page.MaintainScrollPositionOnPostBack = True
        Catch ex As Exception
            log.Error("Error al cargar la página", ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

        ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
        If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
        End If
    End Sub

    ''' <summary>
    ''' Comprueba que el rol del usuario está contenido en la lista de roles de acceso de la pagina
    ''' </summary>
    ''' <param name="rolUsuario"></param>
    ''' <returns>True si existe alguno. False en caso contrario</returns>
    ''' <remarks></remarks>
    Private Function ExisteRolEnPagina(ByVal rolUsuario As Integer) As Boolean
        Dim idRol As Integer = Integer.MinValue
        Dim existe As Boolean = False
        If (RolesAcceso IsNot Nothing) Then
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        Try
            ClearDataViews()
            BindDataViews(0)
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
        Select Case m_ViewMode
            Case ViewMode.Unknown
                pnlGridView.Visible = True
                pnlDetalleSolicitud.Visible = False
            Case ViewMode.GridView
                pnlGridView.Visible = True
                pnlDetalleSolicitud.Visible = False
            Case ViewMode.DetailsView
                pnlGridView.Visible = False
                pnlDetalleSolicitud.Visible = True
        End Select
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearDataViews()
        gvSolicitudes.DataSource = Nothing
        gvSolicitudes.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <param name="idSolicitud">Identificador de la solicitud</param>
    ''' <remarks></remarks>
    Protected Sub BindDataViews(ByVal idSolicitud As Integer)
        Dim listaSolicitudes As List(Of ELL.Solicitudes) = Nothing

        If (idSolicitud > 0) Then
            Dim oReferenciaVentaBLL As New BLL.ReferenciaFinalVentaBLL
            Dim lista As List(Of ELL.ReferenciaVenta) = oReferenciaVentaBLL.CargarReferenciasSolicitud(idSolicitud)
            dlReferencia.DataSource = lista
            dlReferencia.DataBind()
            CargarSolicitud(idSolicitud)            
        Else
            listaSolicitudes = solicitudesBLL.CargarValidacionesPendientesReferenciasVenta(IdUser, IdRol)
            If (listaSolicitudes.Count > 0) Then
                Ordenar(listaSolicitudes, GridViewSortExpresion, GridViewSortDirection)
                gvSolicitudes.DataSource = listaSolicitudes
                gvSolicitudes.DataBind()
                'gvSolicitudes.Caption = "PENDING VALIDATIONS"
                gvSolicitudes.SelectedIndex = -1
            Else
                gvSolicitudes.DataSource = Nothing
                gvSolicitudes.DataBind()
                gvSolicitudes.Caption = "No pending validations found"
            End If
            SetBehavior()
        End If
    End Sub

    ''' <summary>
    ''' Cargar los datos de la solicitud
    ''' </summary>
    ''' <param name="idSolicitud"></param>
    ''' <remarks></remarks>
    Private Sub CargarSolicitud(ByVal idSolicitud As Integer)
        Dim solicitud As New ELL.Solicitudes
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent

        solicitud = solicitudesBLL.CargarSolicitud(idSolicitud)

        txtIdSolicitud.Text = idSolicitud.ToString
        txtApplicant.Text = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = solicitud.IdSolicitante}, False).NombreCompleto
        txtApplDate.Text = solicitud.FechaAlta
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
    ''' Abrir los datos de la solicitud
    ''' </summary>
    ''' <param name="id">Identificador de la solicitud</param>
    ''' <remarks></remarks>
    Private Sub AbrirSolicitud(ByVal id As Integer)
        IdSolicitud = id.ToString()
        BindDataViews(id)
        SetBehavior(ViewMode.DetailsView)
    End Sub


    ''' <summary>
    ''' Enviar email de notificación de nueva solicitud al DocumentationTechnician, la solicitud ha sido validada
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnviarEmailDocumentationTechnician(ByVal idSolicitud As Integer)
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim oBonosisBLL As New BLL.BonoSisBLL
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim listaReferenciasVenta As List(Of ELL.ReferenciaVenta)
        Dim altaUsuario As String = String.Empty
        Dim listaUsuariosDT As New List(Of ELL.UsuarioRol)
        Dim oSolicitudesBLL As New BLL.SolicitudesBLL

        Try
            'Definir dereccion
            mail.From = New MailAddress("""Selling Part Numbers""  <" & "referenciasventa@batz.es" & ">")

            mail.Subject = "New selling part numbers request"

            solicitud = oSolicitudesBLL.CargarSolicitud(idSolicitud)
            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = solicitud.IdSolicitante}, False)
            'Quién da de alta la solicitud
            altaUsuario = gtkUsuario.NombreCompleto.ToString()

            'Usuarios con rol DT en Bonos de sistemas
            listaUsuariosDT = oBonosisBLL.CargarUsuariosRol(ELL.Roles.RolUsuario.DocumentationTechnician)
            'Usuarios con rol de DT en la aplicación de referencias de venta
            Dim users As String = System.Configuration.ConfigurationManager.AppSettings("Documentation Technician").ToString()
            If Not (String.IsNullOrEmpty(users)) Then
                Dim usuarios As String() = users.Split(New Char() {","c})
                For Each usuario In usuarios
                    listaUsuariosDT.Add(New ELL.UsuarioRol With {.IdSab = usuario})
                Next
            End If
            For Each usuario In listaUsuariosDT
                gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = usuario.IdSab}, False)
                If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                    If Not (System.Configuration.ConfigurationManager.AppSettings("ExcepcionesEmail").ToString().Contains(usuario.IdSab)) Then
                        mail.To.Add(gtkUsuario.Email.ToString.Trim)
                    End If
                End If
            Next

            listaReferenciasVenta = oReferenciasVentaBLL.CargarReferenciasSolicitud(idSolicitud)
            mail.Body = Utils.GenerarEmailComun(altaUsuario, idSolicitud, 1, listaReferenciasVenta)

            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)
        Catch ex As Exception
            log.Error("Problema al enviar email a Documentation Technician", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Enviar un correo al solicitante tras la aprobación o rechazo de la solicitud
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnviarEmailSolicitante(ByVal idSolicitud As Integer, ByVal motivo As String)
        Dim mail As New MailMessage()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim comentarioFinal As String = String.Empty
        Dim altaUsuario As String = String.Empty
        Dim referenciasVenta As List(Of ELL.ReferenciaVenta)
        Dim referenciaVentaBLL As New BLL.ReferenciaFinalVentaBLL
        Dim oSolicitudesBLL As New BLL.SolicitudesBLL

        Try
            solicitud = oSolicitudesBLL.CargarSolicitud(idSolicitud)

            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = solicitud.IdSolicitante}, False)

            'Quién da de alta la solicitud
            altaUsuario = gtkUsuario.NombreCompleto.ToString()

            'Definir dereccion
            mail.From = New MailAddress("""Selling Part Numbers""  <" & "referenciasventa@batz.es" & ">")

            mail.Subject = String.Format("Request {0} rejected", idSolicitud.ToString)

            If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                mail.To.Add(gtkUsuario.Email.ToString.Trim)
            End If

            mail.Body = ""
            mail.Body += "<b><font color='red'>The request of selling part numbers has been rejected</font></b>"
            mail.Body += "<br /><br />"
            mail.Body += "<b>Reason: </b>" + motivo

            mail.Body += "<br /><br />"

            referenciasVenta = referenciaVentaBLL.CargarReferenciasSolicitud(idSolicitud)
            mail.Body += Utils.GenerarEmailComun(altaUsuario, idSolicitud, 0, referenciasVenta)

            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpClient").ToString())
            smtp.Send(mail)
        Catch ex As Exception
            log.Error("Problema al enviar email al solicitante", ex)
        End Try
    End Sub

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Databoun del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvSolicitudesHelpdesk_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSolicitudes.RowDataBound

        Dim numIncidenciasProblema As Integer = 0

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lblId As Label = CType(e.Row.FindControl("lblId"), Label)
                Dim id As Integer = Integer.MinValue

                If (lblId IsNot Nothing AndAlso Integer.TryParse(lblId.Text, id)) Then
                    e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvSolicitudes, "Select$" + e.Row.RowIndex.ToString)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al modo grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbtnVolverListadoSolicitudes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnVolverListadoSolicitudes.Click
        gvSolicitudes.SelectedIndex = -1
        SetBehavior(ViewMode.GridView)
        BindDataViews(0)
    End Sub

    ''' <summary>
    ''' Editando la fila en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvSolicitudes_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvSolicitudes.RowEditing
        Try
            Dim id As Integer = GetGridViewRowId(gvSolicitudes.Rows(e.NewEditIndex))
            AbrirSolicitud(id)
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Evento al hacer click sobre cualquier fila del grid de casos asignados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvIncidencias_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvSolicitudes.SelectedIndexChanged
        Dim id As String = gvSolicitudes.DataKeys(gvSolicitudes.SelectedIndex).Value.ToString()
        AbrirSolicitud(id)
    End Sub

    ''' <summary>
    ''' ItemDataBound del DataList de referencias
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dlReferencia_ItemDataBound(sender As Object, e As DataListItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oPlantas As New BLL.PlantasBLL
            Dim plantasReferencia As New List(Of ELL.ReferenciaPlantas)
            Dim plantas As New List(Of ELL.Plantas)

            Dim dlReferencias As DataList = DirectCast(sender, System.Web.UI.WebControls.DataList)
            Dim itemID As Integer = Integer.Parse(dlReferencias.DataKeys(e.Item.ItemIndex).ToString())

            Dim chkPlantsToCharge As CheckBoxList = TryCast(e.Item.FindControl("chkPlantsToCharge"), CheckBoxList)
            plantas = oPlantas.CargarLista()
            chkPlantsToCharge.DataSource = plantas
            chkPlantsToCharge.DataBind()

            plantasReferencia = oReferenciasVentaBLL.CargarPlantasReferencia(itemID)
            For Each planta In plantasReferencia
                chkPlantsToCharge.Items.FindByValue(planta.IdPlanta).Selected = True
            Next
        End If
    End Sub

    ''' <summary>
    ''' Validar la solicitud
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnValidarSolicitud_Click(sender As Object, e As EventArgs)
        If (solicitudesBLL.ValidarSolicitud(IdSolicitud, IdUser)) Then
            Master.MensajeInfo = "The request has been approved successfully".ToUpper
            EnviarEmailDocumentationTechnician(IdSolicitud)
            BindDataViews(0)
            SetBehavior(ViewMode.GridView)
        Else
            Master.MensajeError = "An error occurred while approving the request. Please, try again".ToUpper
        End If
    End Sub

    ''' <summary>
    ''' Rechazar la solicitud, se abrirá un modal para confirmación previa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRechazarSolicitud_Click(sender As Object, e As EventArgs)
        txtMotivoRechazo.Text = String.Empty
        If (IdSolicitud <> Integer.MinValue) Then
            hfIdSolicitud.Value = IdSolicitud.ToString()
            mpe_RechazarSolicitud.Show()
        Else
            Master.MensajeError = "An error occurred while rejecting the request. Please, try again. If this error persists, please contact the site administrator".ToUpper
        End If
    End Sub

    ''' <summary>
    ''' Confirmación de rechazo de la solicitud. Se notifica al solicitante de este rechazo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnConfirmarRechazo_Click(sender As Object, e As EventArgs)
        If (solicitudesBLL.RechazarSolicitud(IdSolicitud, IdUser, txtMotivoRechazo.Text.Trim)) Then
            Master.MensajeInfo = "The request has been rejected successfully".ToUpper
            EnviarEmailSolicitante(IdSolicitud, txtMotivoRechazo.Text.Trim)
            BindDataViews(0)
            SetBehavior(ViewMode.GridView)
        Else
            Master.MensajeError = "An error occurred while rejecting the request. Please, try again. If this error persists, please contact the site administrator".ToUpper
        End If
    End Sub

#End Region

#Region "Ordenar"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvSolicitudes_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSolicitudes.Sorting
        Try
            If (GridViewSortDirection = SortDirection.Ascending) Then
                GridViewSortDirection = SortDirection.Descending
            Else
                GridViewSortDirection = SortDirection.Ascending
            End If

            If (GridViewSortExpresion Is Nothing) Then
                GridViewSortExpresion = String.Empty
            Else
                GridViewSortExpresion = e.SortExpression
            End If

            BindDataViews(0)
        Catch batzEx As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ordenar las solicitudes")
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
                ViewState("sortExpresion") = "Descripcion"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

    ''' <summary>
    ''' Ordena la lista de Areas
    ''' </summary>
    ''' <param name="lIncidencias">Lista de incidencias</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>	
    Private Sub Ordenar(ByRef lIncidencias As List(Of ELL.Solicitudes), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "Id"
                lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                 IIf(sortDir = SortDirection.Ascending, oA1.Id < oA2.Id, oA1.Id > oA2.Id))
                'Case "Asunto"
                '    lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                '     IIf(sortDir = SortDirection.Ascending, oA1.Asunto.ToLower < oA2.Asunto.ToLower, oA1.Asunto.ToLower > oA2.Asunto.ToLower))
                'Case "IdUsuario"
                '    lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                '     IIf(sortDir = SortDirection.Ascending, oA1.Usuario < oA2.Usuario, oA1.Usuario > oA2.Usuario))
                'Case "NombrePlantaUsuario"
                '    lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                '     IIf(sortDir = SortDirection.Ascending, oA1.NombrePlantaUsuario < oA2.NombrePlantaUsuario, oA1.NombrePlantaUsuario > oA2.NombrePlantaUsuario))
                'Case "Responsable"
                '    lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                '     IIf(sortDir = SortDirection.Ascending, oA1.Responsable < oA2.Responsable, oA1.Responsable > oA2.Responsable))
                'Case "FechaCreacion"
                '    lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                '     IIf(sortDir = SortDirection.Ascending, oA1.FechaCreacion < oA2.FechaCreacion, oA1.FechaCreacion > oA2.FechaCreacion))
                'Case "Estado"
                '    lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                '     IIf(sortDir = SortDirection.Ascending, oA1.Estado < oA2.Estado, oA1.Estado > oA2.Estado))
        End Select
    End Sub

#End Region

End Class