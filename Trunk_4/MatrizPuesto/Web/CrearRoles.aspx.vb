
Public Class CrearRoles
    Inherits PageBase
    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New BLL.DocumentosBLL



#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            'roles.Add(ELL.Roles.RolUsuario.Recepcion)
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
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
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
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function
#End Region


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
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim listaType As List(Of ELL.Rol)

            listaType = oDocBLL.CargarListaRol(PageBase.plantaAdmin)

            If (listaType.Count > 0) Then
                gvType.DataSource = listaType
                gvType.DataBind()
                gvType.Caption = String.Empty
            Else
                gvType.DataSource = Nothing
                gvType.DataBind()
                gvType.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar Periodicidad", ex)
        End Try
    End Sub



#End Region



    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ComprobarAcceso()
            '          pnlConfirmSubir.Visible = False  
            If Not (Page.IsPostBack) Then
                Dim s As String
                s = Request.QueryString("id")
                If s = "0" Then
                    mView.ActiveViewIndex = 0
                End If
                If s = "1" Then
                    BindDataView()
                    CargarDetalle(0)
                End If


                Initialize()

            End If




        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un Responsable", ex)
        End Try
    End Sub


    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand
        Try
            If e.CommandName = "Desactivar" Then
                Initialize()
                Dim idres As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
                Dim idrol As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).rol
                Dim idplant As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).planta

                oDocumentosBLL.DeleteRol(idplant, idres, idrol)
                Initialize()
            End If
            If e.CommandName = "Edit" Then
                'Initialize()

                'Dim idres As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
                'BindDataView()   'para limpiar el grid
                'CargarDetalle(idres)
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un Responsable", ex)
        End Try
    End Sub
    Protected Sub gvType_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Add" Then
            '    Dim idPlanta As String = DirectCast(gvType.FooterRow.FindControl("ddlPlanta"), DropDownList).SelectedValue

            'Dim txtDescript As String = DirectCast(gvType.FooterRow.FindControl("txtDescript"), TextBox).Text
            'Dim txtDescAbrev As String = DirectCast(gvType.FooterRow.FindControl("ddlUni"), DropDownList).SelectedValue



            'If Not (oDocumentosBLL.Existe(txtDescript)) Then
            '    GuardarRegistro(txtDescript, txtDescAbrev)
            'Else
            '    'El nombre ya existe en base de datos
            '    Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe otro elemento con el mismo nombre")
            'End If


        End If
    End Sub
    'Protected Sub gvType_OnRowCommand2(sender As Object, e As GridViewCommandEventArgs)
    '    If e.CommandName = "Add" Then


    '        Dim txtDescript As String = DirectCast(gvType.FooterRow.FindControl("txtDescript"), TextBox).Text
    '        Dim txtDescAbrev As String = DirectCast(gvType.FooterRow.FindControl("txtDescAbrev"), TextBox).Text


    '    End If
    'End Sub

    ''' <summary>
    ''' Guardar un nuevo registro de Tipo
    ''' </summary>
    ''' <param name="nombre"></param>
    ''' <remarks></remarks>
    Private Sub GuardarRegistro(ByVal nombre As String, ByVal txtDepto As String, ByVal txtDescAbrev As String, ByVal txtUsuario As String)
        Try

            If (txtResponsable.Text) = "" Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Campo Usuario es obligatorio")
                mView.ActiveViewIndex = 0
                Exit Sub
            End If

            Dim tipo As New ELL.Rol With {.Planta = txtDepto, .Id = nombre, .rol = txtDescAbrev, .NombreRol = txtUsuario}


            If (oDocumentosBLL.GuardarRol(tipo)) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El Rol se ha guardado correctamente").ToUpper

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el Rol").ToUpper
            End If


            mView.ActiveViewIndex = 0

            Initialize()


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar el Rol", ex)
        End Try


    End Sub

    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idDocumento"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idDocumento As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim lista As List(Of ELL.Rol)
            Dim userBLL As New SabLib.BLL.UsuariosComponent


            'si es nuevo elemento
            If idDocumento = 0 Then
                'lblNuevaSolicitud.Text = "Creación de un nuevo Rol"
                flag_Modificar.Value = "0"


            Else
                flag_Modificar.Value = idDocumento
                lista = oDocBLL.CargarRol(idDocumento, PageBase.plantaAdmin)

                '   lblNuevaSolicitud.Text = "Modificación del Rol " ' & lista(0).Nombre
                txtNombre.Text = lista(0).NombreUser

                ddlUnidades.SelectedValue = lista(0).Id
                ddlDepartamento.SelectedValue = lista(0).rol

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un Rol", ex)
        End Try

    End Sub




    ''' <summary>
    ''' Cargar el detalle de un portador de coste
    ''' </summary>
    Private Sub CargarDetalle(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1

        ConfiguracionProduct(idDocumento)

    End Sub

    'Private Sub btnBorrar2_Click(sender As Object, e As EventArgs) Handles btnBorrar.Click
    '    mView.ActiveViewIndex = 1

    'End Sub

    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try

            Dim iCodDocum As Int32 = CInt(flag_Modificar.Value)


            Dim tipo As New ELL.Rol With {.Planta = PageBase.plantaAdmin, .Id = iCodDocum, .rol = ddlUnidades.SelectedValue}

            If flag_Modificar.Value = "0" Then
                If (oDocumentosBLL.GuardarRol(tipo)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El Rol se ha guardado correctamente").ToUpper

                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el Rol").ToUpper
                End If
            Else



                If (oDocumentosBLL.ModificarRol(tipo, iCodDocum)) Then


                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El Rol se ha modificado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se modificaba el Rol").ToUpper
                End If
            End If

            mView.ActiveViewIndex = 0

            Initialize()


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar el Rol", ex)
        End Try

    End Sub




    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click

        mView.ActiveViewIndex = 0
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim txtUsuario As String = txtResponsable.Text
        Dim txtDescript As String = hfResponsable.Value
        Dim txtDescAbrev As String = ddlUni.SelectedValue
        Dim txtDepto As String = ddlDepartamento.SelectedValue



        If Not (oDocumentosBLL.Existe(txtDescript, CInt(txtDescAbrev), CInt(txtDepto))) Then
            GuardarRegistro(txtDescript, txtDepto, txtDescAbrev, txtUsuario)
        Else
            'El nombre ya existe en base de datos
            Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe otro elemento con el mismo nombre")
        End If

    End Sub


End Class