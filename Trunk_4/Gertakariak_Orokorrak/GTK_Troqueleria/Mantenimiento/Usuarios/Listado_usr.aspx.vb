Imports System.ComponentModel
Imports System.Reflection

Public Class Listado_usr
    Inherits PageBase
#Region "Propiedades"
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Lista_Permisos As IEnumerable(Of ListItem) = From Permiso As Global_asax.Permisos In [Enum].GetValues(GetType(Global_asax.Permisos)) Select New ListItem With {.Value = Permiso.GetHashCode, .Text = GetType(Global_asax.Permisos).GetMember(Permiso.ToString()).First().GetCustomAttribute(Of DescriptionAttribute)().Description}

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView para realiza la seleccion, paginacion y ordenacion.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property gvUsuarios_Propiedades() As gtkGridView
        Get
            If (Session("gvUsuarios_Propiedades") Is Nothing) Then Session("gvUsuarios_Propiedades") = New gtkGridView
            Return CType(Session("gvUsuarios_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvUsuarios_Propiedades") = value
        End Set
    End Property
    Property gvRoles_Propiedades() As gtkGridView
        Get
            If (Session("gvRoles_Propiedades") Is Nothing) Then Session("gvRoles_Propiedades") = New gtkGridView
            Return CType(Session("gvRoles_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvRoles_Propiedades") = value
        End Set
    End Property

    Public Property Filtro_Usuarios() As gtkFiltro
        Get
            If (Session("Filtro_Usuarios") Is Nothing) Then Session("Filtro_Usuarios") = New gtkFiltro
            Return CType(Session("Filtro_Usuarios"), gtkFiltro)
        End Get
        Set(value As gtkFiltro)
            Session("Filtro_Usuarios") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub Listado_usr_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If Not ((ROL_Usuario And Global_asax.Permisos.Gestionar_Roles_Usuaios) = Global_asax.Permisos.Gestionar_Roles_Usuaios) Then Response.Redirect("~/Default.aspx", True)
    End Sub
#If DEBUG Then
    Private Sub Listado_usr_Init(sender As Object, e As EventArgs) Handles Me.Init
        gvUsuarios.PageSize = 5
    End Sub
#End If
    Private Sub Listado_usr_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            Cargar_GridView()
            CargarPanelBuscador()
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub Listado_usr_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        pnlNuevoUsuario.Visible = ((ROL_Usuario And Global_asax.Permisos.Gestionar_Roles_Usuaios) = Global_asax.Permisos.Gestionar_Roles_Usuaios)
        pnlNuevo_Rol.Visible = ((ROL_Usuario And Global_asax.Permisos.Gestionar_Roles_Usuaios) = Global_asax.Permisos.Gestionar_Roles_Usuaios)
    End Sub
#End Region

#Region "Eventos de Objetos"
#Region "Panel Usuarios"
    Private Sub gvUsuarios_Init(sender As Object, e As EventArgs) Handles gvUsuarios.Init
        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gvUsuarios_Propiedades.CampoOrdenacion) Then
            gvUsuarios_Propiedades.CampoOrdenacion = "NombreCompleto"
            gvUsuarios_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub gvUsuarios_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvUsuarios.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvUsuarios_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvUsuarios_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvUsuarios.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Reg As Object = Fila.DataItem
            Dim IDUSR As Integer = Reg.ID

            Fila.FindControl("btnEditar").Visible = ((ROL_Usuario And Global_asax.Permisos.Gestionar_Roles_Usuaios) = Global_asax.Permisos.Gestionar_Roles_Usuaios)
            '----------------------------------------------------------------------------------------------
            'Indicamos si es el registro seleccionado.
            '----------------------------------------------------------------------------------------------
            If Reg.ID = gvUsuarios_Propiedades.IdSeleccionado Then
                Fila.RowState = DataControlRowState.Selected
                Fila.CssClass &= " info"
            End If
            '----------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------
            'Buscamos los roles de los usuarios
            '----------------------------------------------------------------------------------------------
            Dim lRol = From Rol As BatzBBDD.ROLES_USR In BBDD.ROLES_USR Where Rol.IDUSR = IDUSR Select Rol.ROLES
            If lRol IsNot Nothing AndAlso lRol.Any Then
                Dim blRol As BulletedList = Fila.FindControl("blRol")
                blRol.DataSource = lRol.ToList.OrderByDescending(Function(o) o.PERMISOS)
            End If
            '----------------------------------------------------------------------------------------------
        End If
    End Sub
    Private Sub gvUsuarios_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvUsuarios.RowEditing
        Try
            Dim Tabla As GridView = sender
            e.Cancel = True
            gvUsuarios_Propiedades.IdSeleccionado = CType(Tabla.DataKeys(e.NewEditIndex).Value, Integer)

            Dim Usuario As BatzBBDD.SAB_USUARIOS = BBDD.SAB_USUARIOS.Where(Function(o) o.ID = gvUsuarios_Propiedades.IdSeleccionado).FirstOrDefault
            If Usuario Is Nothing Then Response.Redirect("~/Mantenimiento/Usuarios/Listado.aspx", True)

            '----------------------------------------------------------------------------------------------------
            'Panel de Detalle
            '----------------------------------------------------------------------------------------------------
            Dim NombreCompleto As String = If(Usuario Is Nothing, "?", String.Format("{0} {1} {2}",
                                                                                 If(String.IsNullOrWhiteSpace(Usuario.NOMBRE), String.Empty, Usuario.NOMBRE.Trim),
                                                                                 If(String.IsNullOrWhiteSpace(Usuario.APELLIDO1), String.Empty, Usuario.APELLIDO1.Trim),
                                                                                 If(String.IsNullOrWhiteSpace(Usuario.APELLIDO2), String.Empty, Usuario.APELLIDO2.Trim)))
            lblUsuario.Text = NombreCompleto
            lblUsuario.ToolTip = Usuario.NOMBREUSUARIO
            btnBorrar_Usuario.Visible = (gvUsuarios_Propiedades.IdSeleccionado IsNot Nothing)

            '-----------------------------------------------------------------
            'Roles de Usuario
            '-----------------------------------------------------------------
            Dim lRol As List(Of BatzBBDD.ROLES) = BBDD.ROLES.OrderBy(Function(o) o.NOMBRE).ToList
            cblRol.Items.Clear()
            cblRol.DataValueField = "ID"
            cblRol.DataTextField = "NOMBRE"
            cblRol.DataSource = If(lRol.Any, lRol, Nothing)
            cblRol.DataBind()

            'If Usuario.ROLES_USR IsNot Nothing AndAlso Usuario.ROLES_USR.Any Then
            '    For Each Reg As BatzBBDD.ROLES_USR In Usuario.ROLES_USR
            '        Dim li As ListItem = If(cblRol.Items.FindByValue(Reg.IDROL), Nothing)
            '        If li IsNot Nothing Then li.Selected = True
            '    Next
            'End If

            If Usuario.ROLES_USR IsNot Nothing AndAlso Usuario.ROLES_USR.Any Then
                For Each Item As ListItem In cblRol.Items
                    Item.Selected = (Usuario.ROLES_USR.Select(Function(o) o.IDROL).Contains(Item.Value))
                Next
            End If
            '-----------------------------------------------------------------
            'Abrimos el panel de Editar
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "informe", "$('#" & pnl_Roles.ClientID & "').modal('show');", True)
            'btnBorrar_Compensacion.Visible = (Compensacion IsNot Nothing)
            'cbe_btnBorrar_Compensacion.Enabled = btnBorrar_Compensacion.Visible
            '----------------------------------------------------------------------------------------------------

        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gvUsuarios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvUsuarios.SelectedIndexChanged
        Dim Tabla As GridView = sender
        gvUsuarios_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
    End Sub
    Private Sub gvUsuarios_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvUsuarios.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvUsuarios_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvUsuarios_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvUsuarios_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvUsuarios_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvUsuarios_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvUsuarios_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvUsuarios_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvUsuarios_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvUsuarios_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvUsuarios_PreRender(sender As Object, e As EventArgs) Handles gvUsuarios.PreRender
        Try
            Dim Tabla As GridView = sender
            Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvUsuarios_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvUsuarios_Propiedades.CampoOrdenacion), If(gvUsuarios_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvUsuarios_Propiedades.DireccionOrdenacion.GetValueOrDefault))
            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvUsuarios_Propiedades.IdSeleccionado IsNot Nothing _
                AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
                AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then

                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvUsuarios_Propiedades.IdSeleccionado)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    gvUsuarios_Propiedades.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(gvUsuarios_Propiedades.Pagina, 0)
            Tabla.DataBind()

            If Tabla.Rows.Count > 0 Then
                'Ponemos los encabezados en un 'tHead'
                Tabla.UseAccessibleHeader = True
                Tabla.HeaderRow.TableSection = TableRowSection.TableHeader

                'Tabla.ShowFooter = True
                'Tabla.FooterRow.TableSection = TableRowSection.TableFooter
            End If
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try

    End Sub

    Private Sub btnGuardar_Usuario_Click(sender As Object, e As EventArgs) Handles btnGuardar_Usuario.Click
        Try
            Dim lRoles_Usr As IQueryable(Of BatzBBDD.ROLES_USR) = BBDD.ROLES_USR.Where(Function(o) o.IDUSR = gvUsuarios_Propiedades.IdSeleccionado)
            BBDD.ROLES_USR.RemoveRange(lRoles_Usr)

            For Each Item As ListItem In cblRol.Items.Cast(Of ListItem).OrderBy(Function(o) o.Text).Where(Function(o) o.Selected)
                BBDD.ROLES_USR.Add(New BatzBBDD.ROLES_USR With {.IDROL = Item.Value, .IDUSR = gvUsuarios_Propiedades.IdSeleccionado})
            Next

            BBDD.SaveChanges()

            Definir_Rol()
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarFiltroGTK()
    End Sub
    Private Sub btnBorrar_Usuario_Click(sender As Object, e As EventArgs) Handles btnBorrar_Usuario.Click
        Try
            BBDD.GRUPOS.Find(Global_asax.GrupoRecurso).USUARIOS.Remove(BBDD.USUARIOS.Find(gvUsuarios_Propiedades.IdSeleccionado))
            BBDD.ROLES_USR.RemoveRange(BBDD.ROLES_USR.Where(Function(o) o.IDUSR = gvUsuarios_Propiedades.IdSeleccionado))
            BBDD.SaveChanges()

            gvUsuarios_Propiedades.IdSeleccionado = Nothing
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnAgregarUsuario_Click(sender As Object, e As EventArgs) Handles btnAgregarUsuario.Click
        Try
            Filtro_Usuarios.Descripcion = txtBuscarUsuario.Text
            gvUsuarios_Propiedades.IdSeleccionado = hd_NuevoUsuario.Value
            Dim Usuario As BatzBBDD.USUARIOS = (From Usr As BatzBBDD.USUARIOS In BBDD.USUARIOS
                                                From Gru As BatzBBDD.GRUPOS In Usr.GRUPOS
                                                From Rec As BatzBBDD.RECURSOS In Gru.RECURSOS
                                                Where Rec.ID = Global_asax.RecursoWeb And Usr.ID = gvUsuarios_Propiedades.IdSeleccionado Select Usr).SingleOrDefault
            If Usuario Is Nothing Then
                BBDD.GRUPOS.Find(Global_asax.GrupoRecurso).USUARIOS.Add(BBDD.USUARIOS.Find(gvUsuarios_Propiedades.IdSeleccionado))
                BBDD.SaveChanges()
            End If
            hd_NuevoUsuario.Value = String.Empty
            txtBuscarUsuario.Text = String.Empty
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Panel Roles"
    Private Sub gvRoles_Init(sender As Object, e As EventArgs) Handles gvRoles.Init
        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gvRoles_Propiedades.CampoOrdenacion) Then
            gvRoles_Propiedades.CampoOrdenacion = "Nombre"
            gvRoles_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub gvRoles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvRoles.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvRoles_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvRoles_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvRoles.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        If Fila.DataItem IsNot Nothing Then
            Dim Reg As BatzBBDD.ROLES = Fila.DataItem

            Fila.FindControl("btnEditar").Visible = ((ROL_Usuario And Global_asax.Permisos.Gestionar_Roles_Usuaios) = Global_asax.Permisos.Gestionar_Roles_Usuaios)

            '----------------------------------------------------------------------------------------------
            'Indicamos si es el registro seleccionado.
            '----------------------------------------------------------------------------------------------
            If Reg.ID = gvRoles_Propiedades.IdSeleccionado Then
                Fila.RowState = DataControlRowState.Selected
                Fila.CssClass &= " info"
            End If
            '----------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------
            'Buscamos los permisos del ROL
            '----------------------------------------------------------------------------------------------
            '((Rol.PERMISOS And CInt(Item.Value)) = CInt(Item.Value))
            Dim Permisos_ROL As IEnumerable(Of ListItem) = From Permiso As ListItem In Lista_Permisos Where ((Reg.PERMISOS And CInt(Permiso.Value)) = CInt(Permiso.Value))

            If Permisos_ROL IsNot Nothing AndAlso Permisos_ROL.Any Then
                Dim blRol_Permisos As BulletedList = Fila.FindControl("blRol_Permisos")
                blRol_Permisos.DataTextField = "Text"
                blRol_Permisos.DataSource = Permisos_ROL
                blRol_Permisos.DataBind()
            End If
            '----------------------------------------------------------------------------------------------

        End If
    End Sub
    Private Sub gvRoles_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvRoles.RowEditing
        Try
            Dim Tabla As GridView = sender
            e.Cancel = True
            gvRoles_Propiedades.IdSeleccionado = CType(Tabla.DataKeys(e.NewEditIndex).Value, Integer)

            'pnlNombreRol.Visible = (gvRoles_Propiedades.IdSeleccionado Is Nothing)

            Dim Rol As BatzBBDD.ROLES = BBDD.ROLES.Where(Function(o) o.ID = gvRoles_Propiedades.IdSeleccionado).FirstOrDefault
            If Rol Is Nothing Then Response.Redirect("~/Mantenimiento/Usuarios/Listado.aspx", True)

            '----------------------------------------------------------------------------------------------------
            'Panel de Detalle
            '----------------------------------------------------------------------------------------------------
            lblRol.Text = Rol.NOMBRE
            lblRol.ToolTip = Rol.DESCRIPCION
            txtNombre_Rol.Text = Rol.NOMBRE
            txtDescripcion_Rol.Text = Rol.DESCRIPCION
            btnBorrarRol.Visible = (gvRoles_Propiedades.IdSeleccionado IsNot Nothing)

            'Permisos del ROL
            cbPermisos.Items.Clear()
            cbPermisos.DataValueField = "Value"
            cbPermisos.DataTextField = "Text"
            cbPermisos.DataSource = Lista_Permisos
            cbPermisos.DataBind()

            If Rol.PERMISOS IsNot Nothing Then
                For Each Item As ListItem In cbPermisos.Items
                    Item.Selected = ((Rol.PERMISOS And CInt(Item.Value)) = CInt(Item.Value))
                Next
            End If

            'Abrimos el panel de Editar
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "informe", "$('#" & pnlPermisos.ClientID & "').modal('show');", True)
            ''btnBorrar_Compensacion.Visible = (Compensacion IsNot Nothing)
            ''cbe_btnBorrar_Compensacion.Enabled = btnBorrar_Compensacion.Visible
            '----------------------------------------------------------------------------------------------------

        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gvRoles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRoles.SelectedIndexChanged
        Dim Tabla As GridView = sender
        gvRoles_Propiedades.IdSeleccionado = CType(Tabla.SelectedDataKey.Value, Integer)
    End Sub
    Private Sub gvRoles_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvRoles.Sorting
        Dim Tabla As GridView = sender
        '-------------------------------------------------------------------------------------------------------------
        'Criterio de Ordenacion:
        'Ascendente si no hay criterio o es Descendente el criterio que existe o el campo de ordenacion es distinto.
        '-------------------------------------------------------------------------------------------------------------
        If IsPostBack Then
            If gvRoles_Propiedades.DireccionOrdenacion IsNot Nothing _
             AndAlso gvRoles_Propiedades.DireccionOrdenacion.Value = SortDirection.Ascending _
             And gvRoles_Propiedades.CampoOrdenacion = e.SortExpression Then
                gvRoles_Propiedades.DireccionOrdenacion = SortDirection.Descending
            ElseIf gvRoles_Propiedades.DireccionOrdenacion.GetValueOrDefault = SortDirection.Descending _
             Or gvRoles_Propiedades.DireccionOrdenacion Is Nothing _
             Or gvRoles_Propiedades.CampoOrdenacion <> e.SortExpression Then
                gvRoles_Propiedades.DireccionOrdenacion = SortDirection.Ascending
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------
        gvRoles_Propiedades.CampoOrdenacion = e.SortExpression
    End Sub
    Private Sub gvRoles_PreRender(sender As Object, e As EventArgs) Handles gvRoles.PreRender
        Try
            Dim Tabla As GridView = sender
            Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvRoles_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvRoles_Propiedades.CampoOrdenacion), If(gvRoles_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvRoles_Propiedades.DireccionOrdenacion.GetValueOrDefault))
            '--------------------------------------------------------------------------------------------------------
            'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
            '--------------------------------------------------------------------------------------------------------
            If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvRoles_Propiedades.IdSeleccionado IsNot Nothing _
                AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
                AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0 Then
                Dim Lista As List(Of Object) = Tabla.DataSource
                Dim TipoObjeto As Type = Lista.First.GetType
                Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
                Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
                Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvRoles_Propiedades.IdSeleccionado)
                If PosicionReg >= 0 Then
                    Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                    gvRoles_Propiedades.Pagina = PaginaActual
                End If
            End If
            '--------------------------------------------------------------------------------------------------------
            Tabla.PageIndex = If(gvRoles_Propiedades.Pagina, 0)
            Tabla.DataBind()

            If Tabla.Rows.Count > 0 Then
                'Ponemos los encabezados en un 'tHead'
                Tabla.UseAccessibleHeader = True
                Tabla.HeaderRow.TableSection = TableRowSection.TableHeader

                'Tabla.ShowFooter = True
                'Tabla.FooterRow.TableSection = TableRowSection.TableFooter
            End If
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try

    End Sub

    Private Sub btnNuevo_ROL_Click(sender As Object, e As EventArgs) Handles btnNuevo_ROL.Click
        gvRoles_Propiedades.IdSeleccionado = Nothing

        lblRol.Text = "Nuevo Rol" : lblRol.ToolTip = lblRol.Text
        btnBorrarRol.Visible = (gvRoles_Propiedades.IdSeleccionado IsNot Nothing)
        txtNombre_Rol.Text = String.Empty
        txtDescripcion_Rol.Text = String.Empty

        'Permisos del ROL
        cbPermisos.Items.Clear()
        cbPermisos.DataValueField = "Value"
        cbPermisos.DataTextField = "Text"
        cbPermisos.DataSource = Lista_Permisos
        cbPermisos.DataBind()

        'Abrimos el panel de Editar
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "informe", "$('#" & pnlPermisos.ClientID & "').modal('show');", True)
    End Sub
    Private Sub btnGuardarRol_Click(sender As Object, e As EventArgs) Handles btnGuardarRol.Click
        Try
            Dim Rol As BatzBBDD.ROLES = BBDD.ROLES.Where(Function(o) o.ID = gvRoles_Propiedades.IdSeleccionado).FirstOrDefault
            If Rol Is Nothing Then
                Rol = New BatzBBDD.ROLES
                BBDD.ROLES.Add(Rol)
            End If

            Rol.NOMBRE = If(String.IsNullOrWhiteSpace(txtNombre_Rol.Text), Nothing, txtNombre_Rol.Text)
            Rol.DESCRIPCION = If(String.IsNullOrWhiteSpace(txtDescripcion_Rol.Text), Nothing, txtDescripcion_Rol.Text)

            Dim Permisos As Integer = cbPermisos.Items.Cast(Of ListItem).Where(Function(o) o.Selected).Select(Function(o) CInt(o.Value)).Sum
            Rol.PERMISOS = If(Permisos <= 0, New Nullable(Of Decimal), Permisos)

            BBDD.SaveChanges()

            Definir_Rol()

        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnBorrarRol_Click(sender As Object, e As EventArgs) Handles btnBorrarRol.Click
        Try
            Dim Rol As BatzBBDD.ROLES = BBDD.ROLES.Where(Function(o) o.ID = gvRoles_Propiedades.IdSeleccionado).FirstOrDefault
            If Rol Is Nothing Then Response.Redirect("~/Mantenimiento/Usuarios/Listado.aspx", True)
            BBDD.ROLES.Remove(Rol)
            BBDD.SaveChanges()

            gvRoles_Propiedades.IdSeleccionado = Nothing

            Definir_Rol()
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#End Region

#Region "Funciones y Procesos"
    Sub CargarFiltroGTK()
        Filtro_Usuarios.Descripcion = txtBuscar.Text.Trim
    End Sub
    Sub Cargar_GridView()
        '-------------------------------------------------------------------------------------------------------------
        'Panel de Usuarios
        '-------------------------------------------------------------------------------------------------------------
        Dim ListaGV = (From Usr As BatzBBDD.USUARIOS In BBDD.USUARIOS
                       From Gru As BatzBBDD.GRUPOS In Usr.GRUPOS
                       From Rec As BatzBBDD.RECURSOS In Gru.RECURSOS
                       Where Rec.ID = Global_asax.RecursoWeb Select Usr).ToList

        If Not String.IsNullOrWhiteSpace(Filtro_Usuarios.Descripcion) Then
            Dim aPrefixText As String() = Filtro_Usuarios.Descripcion.Split(New String() {" ", "-", "/", "\", "(", ")"}, StringSplitOptions.RemoveEmptyEntries)
            For Each TxTBusqueda As String In aPrefixText
                Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(TxTBusqueda), RegexOptions.IgnoreCase)

                ListaGV = (From Reg In ListaGV
                           Where ExpReg.IsMatch(Reg.ID) _
                               Or If(String.IsNullOrWhiteSpace(Reg.NOMBRE), Nothing, ExpReg.IsMatch(Reg.NOMBRE)) _
                               Or If(String.IsNullOrWhiteSpace(Reg.APELLIDO1), Nothing, ExpReg.IsMatch(Reg.APELLIDO1)) _
                               Or If(String.IsNullOrWhiteSpace(Reg.APELLIDO2), Nothing, ExpReg.IsMatch(Reg.APELLIDO2)) _
                               Or ExpReg.IsMatch(Reg.CODPERSONA) _
                               Or If(String.IsNullOrWhiteSpace(Reg.DNI), Nothing, ExpReg.IsMatch(Reg.DNI)) _
                               Or If(String.IsNullOrWhiteSpace(Reg.EMAIL), Nothing, ExpReg.IsMatch(Reg.EMAIL)) _
                               Or If(String.IsNullOrWhiteSpace(Reg.NOMBREUSUARIO), Nothing, ExpReg.IsMatch(Reg.NOMBREUSUARIO))
                           Select Reg).ToList
            Next
        End If

        gvUsuarios.DataSource = If(ListaGV IsNot Nothing AndAlso ListaGV.Any, ListaGV.Distinct.Select(Function(o) New With {o.ID, .NombreCompleto = If(o Is Nothing, "?", String.Format("{0} {1} {2}",
                                                                                                                              If(String.IsNullOrWhiteSpace(o.NOMBRE), String.Empty, o.NOMBRE.Trim),
                                                                                                                              If(String.IsNullOrWhiteSpace(o.APELLIDO1), String.Empty, o.APELLIDO1.Trim),
                                                                                                                              If(String.IsNullOrWhiteSpace(o.APELLIDO2), String.Empty, o.APELLIDO2.Trim)))}).ToList, Nothing)
        '-------------------------------------------------------------------------------------------------------------

        '-------------------------------------------------------------------------------------------------------------
        'Panel de Roles
        '-------------------------------------------------------------------------------------------------------------
        Dim ListaGV_Roles = BBDD.ROLES.ToList
        gvRoles.DataSource = If(ListaGV_Roles IsNot Nothing AndAlso ListaGV_Roles.Any, ListaGV_Roles, Nothing)
        '-------------------------------------------------------------------------------------------------------------
    End Sub
    Sub CargarPanelBuscador()
        txtBuscar.Text = Filtro_Usuarios.Descripcion
    End Sub
#End Region
End Class