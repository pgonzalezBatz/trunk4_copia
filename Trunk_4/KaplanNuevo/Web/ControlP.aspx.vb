Imports iTextSharp.text.pdf

Imports System.Data
Public Class ControlP
    Inherits PageBase

    ' Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New KaplanLib.BLL.DocumentosBLL
    '   Dim oPerfilBLL As New BLL.perfilUsuarioBLL

#Region "METODOS"


    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()
        BindDataView()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvType.DataSource = Nothing
        gvType.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
        Dim lista As List(Of KaplanLib.ELL.Documentos)

        lista = oDocBLL.CargarListaPlanta()

        If (lista.Count > 0) Then



            If (lista.Count = 1) Then    'Solo tiene una planta a administrar
                plantaAdmin = lista(0).Id
                plantaAdminNombre = lista(0).Nombre
                Response.Redirect("EstadoEmpresaDocumento.aspx", False)
            Else
                gvType.DataSource = lista
                gvType.DataBind()
                gvType.Caption = String.Empty
            End If
        Else
            gvType.DataSource = Nothing
            gvType.DataBind()
            gvType.Caption = "No hay registros"
        End If
    End Sub



#End Region




#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of KaplanLib.ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of KaplanLib.ELL.Roles.RolUsuario)
            'roles.Add(roles.RolUsuario.Administrador)
            'roles.Add(KaplanLib.ELL.Roles.RolUsuario.Administrador2)
            'roles.Add(roles.RolUsuario.Recepcion)
            'roles.Add(Kaplanlib.ELL.Roles.RolUsuario.Prevencion)
            Return roles
        End Get
    End Property

#End Region


#Region "METODOS"


    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        If (PerfilUsuario) Is Nothing Then  'es un usuario no identificado en web.config. solo ira a aquello que no lo necesite. extranet que no pondre esto.  comprobare el id de depto
            '    If (RolesAcceso Is Nothing ) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
            Response.Redirect("~/PermisoDenegado.aspx", True)
            '    End If

        Else


            Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

            ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = KaplanLib.ELL.Roles.RolUsuario.Administrador)) Then
                Dim segmentos As String() = Page.Request.Url.Segments
                Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
                'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
                Response.Redirect("~/PermisoDenegado.aspx", True)
            End If



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
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(KaplanLib.ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
        Dim lista As List(Of KaplanLib.ELL.Empresas)

        'lista = oDocBLL.CargarEmp()
        'Dim listaType2 As List(Of KaplanLib.ELL.Documentos)
        'listaType2 = oDocBLL.CargarListaEmpHist()
        Dim prueba As String
        prueba = Request.QueryString("idCond")


        'FIN PRUEBA COGER LOS DATOS DEL HISTORICO DE ADOK_EMD


        Dim sacarpopup As Integer = 0
        Dim plantatmp As Integer = 0

        Session("Ticket").IdEmpresa = 5435


        'despues le pido que elija planta, si zamudio le mando a default9.aspx
        'mirar si esta en bbdd entonces rol 2, si no rol 66 sin acceso




        Dim persona As New SabLib.ELL.Ticket

        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
        End If

        Dim plantas As Int32 = persona.Plantas.Count
        '    Derio = 1 'jon temporalmente
        If plantas = 0 Then
            MsgBox("El usuario no está asignado a ninguna planta")
        Else

            persona.Plantas(0).Id = 1



            'roles
            Session("PerfilUsuario") = oDocumentosBLL.CargarPerfilUsuario(persona.IdUser, persona.Plantas(0).Id)

            Dim vista As String
            vista = Request.QueryString("id")
            If vista = "1" Then
                Response.Redirect("Solicitudes.aspx", True)
            End If



            'ComprobarAcceso()


            'Si es rol de consulta que vaya a trabajadores
            If PerfilUsuario.IdRol = 3 Then
                plantaAdmin = persona.Plantas(0).Id
                plantaAdminNombre = persona.Plantas(0).Nombre
                Response.Redirect("EstadoTrabajadorDocumentoC2.aspx", True)
            End If
            If PerfilUsuario.IdRol = 21 Then
                plantaAdmin = persona.Plantas(0).Id
                plantaAdminNombre = persona.Plantas(0).Nombre
                Response.Redirect("EstadoEmpresaDocumentoC.aspx", True)
            End If

            If plantas > 1 Then
                Initialize()
                'CargarPlantas()

            Else '=1

                plantaAdmin = persona.Plantas(0).Id
                plantaAdminNombre = persona.Plantas(0).Nombre
                'ir a pantalla de documentos
                'si se conecta por internet, es de una empresa pasar parametro

                '    Response.Redirect("EstadoEmpresaDocumento.aspx?extranet=1", True) 'de momento


            End If

        End If




        'Catch ex As Exception
        '    Throw New SabLib.BatzException("Error al mostrar una empresa", ex)
        'End Try


    End Sub





    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    ''' <summary>
    ''' Eliminación de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub
    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand

        plantaAdmin = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
        plantaAdminNombre = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).nombre

        'roles miramos roles para la planta elegida
        Dim persona As New SabLib.ELL.Ticket
        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
        End If
        Session("PerfilUsuario") = oDocumentosBLL.CargarPerfilUsuario(persona.IdUser, plantaAdmin)

        Response.Redirect("EstadoEmpresaDocumento.aspx", False)

    End Sub
    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub

    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

    End Sub


    Public operacion As Integer
    Public totalItemSeleccionados As Integer = 0




End Class