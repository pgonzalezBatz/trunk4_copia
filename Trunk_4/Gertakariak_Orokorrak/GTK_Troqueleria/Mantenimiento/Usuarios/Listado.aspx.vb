Imports TraduccionesLib

Public Class Listado
    Inherits PageBase

#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak

    Public Property gvAdministradores_Propiedades As gtkGridView
        Get
            If (Session("gvAdministradores_Propiedades") Is Nothing) Then Session("gvAdministradores_Propiedades") = New gtkGridView
            Return CType(Session("gvAdministradores_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvAdministradores_Propiedades") = value
        End Set
    End Property
    Public Property gvUsuarios_Propiedades As gtkGridView
        Get
            If (Session("gvUsuarios_Propiedades") Is Nothing) Then Session("gvUsuarios_Propiedades") = New gtkGridView
            Return CType(Session("gvUsuarios_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvUsuarios_Propiedades") = value
        End Set
    End Property

    Public Property gvConsultores_Propiedades As gtkGridView
        Get
            If (Session("gvConsultores_Propiedades") Is Nothing) Then Session("gvConsultores_Propiedades") = New gtkGridView
            Return CType(Session("gvConsultores_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvConsultores_Propiedades") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub Listado_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub btnAgregarAdministrador_Click(sender As Object, e As EventArgs) Handles btnAgregarAdministrador.Click
        Try
            GestionarUsuarios(Perfil.Administrador, hdIdAdministrador.Value)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub gvAdministradores_Init(sender As Object, e As EventArgs) Handles gvAdministradores.Init
        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gvAdministradores_Propiedades.CampoOrdenacion) Then
            gvAdministradores_Propiedades.CampoOrdenacion = "NombreCompleto"
            gvAdministradores_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub gvAdministradores_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvAdministradores.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        'Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim lblNombreUsuario As Label

        If Fila.DataItem IsNot Nothing Then
            'lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
            'Dim UsrGrup As BatzBBDD.USUARIOSGRUPOS = Fila.DataItem
            'Dim Usuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = UsrGrup.IDUSUARIO}, False)
            '         If Usuario IsNot Nothing Then lblNombreUsuario.Text = Usuario.NombreCompleto
            lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
            lblNombreUsuario.Text = Fila.DataItem.NombreCompleto
            'Indicamos si es el registro seleccionado.
            If Fila.DataItem.ID = gvAdministradores_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
        End If
    End Sub
    Private Sub gvAdministradores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvAdministradores.RowDeleting
        Dim Tabla As GridView = sender
        e.Cancel = True

        BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
    End Sub
    Private Sub gvAdministradores_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvAdministradores.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvAdministradores_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvAdministradores_PreRender(sender As Object, e As EventArgs) Handles gvAdministradores.PreRender
        Dim Tabla As GridView = sender
        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvAdministradores_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvAdministradores_Propiedades.CampoOrdenacion), If(gvAdministradores_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvAdministradores_Propiedades.DireccionOrdenacion.GetValueOrDefault))

        '--------------------------------------------------------------------------------------------------------
        'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
        '--------------------------------------------------------------------------------------------------------
        If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvAdministradores_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso (hdIdAdministrador.Value <> String.Empty Or String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0) Then

            Dim Lista As List(Of Object) = Tabla.DataSource
            Dim TipoObjeto As Type = Lista.First.GetType
            Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
            Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
            Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvAdministradores_Propiedades.IdSeleccionado)

            If PosicionReg >= 0 Then
                Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                gvAdministradores_Propiedades.Pagina = PaginaActual
            End If

            hdIdAdministrador.Value = String.Empty
        End If
        '--------------------------------------------------------------------------------------------------------

        Tabla.PageIndex = If(gvAdministradores_Propiedades.Pagina, 0)
        Tabla.DataBind()
    End Sub

    Private Sub btnAgregarUsuario_Click(sender As Object, e As EventArgs) Handles btnAgregarUsuario.Click
        Try
            GestionarUsuarios(Perfil.Gestor, hfIdUsuario.Value)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
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
    Private Sub gvUsuarios_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvUsuarios.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        Dim lblNombreUsuario As Label

        If Fila.DataItem IsNot Nothing Then
            lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
            lblNombreUsuario.Text = Fila.DataItem.NombreCompleto
            'Indicamos si es el registro seleccionado.
            If Fila.DataItem.ID = gvUsuarios_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
        End If
    End Sub
    Private Sub gvUsuarios_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvUsuarios.RowDeleting
        Dim Tabla As GridView = sender
        e.Cancel = True

        BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
    End Sub
    Private Sub gvUsuarios_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvUsuarios.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvUsuarios_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvUsuarios_PreRender(sender As Object, e As EventArgs) Handles gvUsuarios.PreRender
        Dim Tabla As GridView = sender
        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvUsuarios_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvUsuarios_Propiedades.CampoOrdenacion), If(gvUsuarios_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvUsuarios_Propiedades.DireccionOrdenacion.GetValueOrDefault))

        '--------------------------------------------------------------------------------------------------------
        'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
        '--------------------------------------------------------------------------------------------------------
        If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvUsuarios_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso (hfIdUsuario.Value <> String.Empty Or String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0) Then

            Dim Lista As List(Of Object) = Tabla.DataSource
            Dim TipoObjeto As Type = Lista.First.GetType
            Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
            Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
            Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvUsuarios_Propiedades.IdSeleccionado)

            If PosicionReg >= 0 Then
                Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                gvUsuarios_Propiedades.Pagina = PaginaActual
            End If

            hfIdUsuario.Value = String.Empty
        End If
        '--------------------------------------------------------------------------------------------------------

        Tabla.PageIndex = If(gvUsuarios_Propiedades.Pagina, 0)
        Tabla.DataBind()
    End Sub

#Region "Consultores"
    Private Sub btnAgregarConsultor_Click(sender As Object, e As EventArgs) Handles btnAgregarConsultor.Click
        Try
            GestionarUsuarios(Perfil.Consultor, hdIdConsultor.Value)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

    Private Sub gvConsultores_Init(sender As Object, e As EventArgs) Handles gvConsultores.Init
        '---------------------------------------------------------------------------------------------
        'Valores iniciales de la ordenacion.
        '---------------------------------------------------------------------------------------------
        If String.IsNullOrWhiteSpace(gvConsultores_Propiedades.CampoOrdenacion) Then
            gvConsultores_Propiedades.CampoOrdenacion = "NombreCompleto"
            gvConsultores_Propiedades.DireccionOrdenacion = ComponentModel.ListSortDirection.Ascending
        End If
        '---------------------------------------------------------------------------------------------
    End Sub
    Private Sub gvConsultores_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvConsultores.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        Dim lblNombreUsuario As Label

        If Fila.DataItem IsNot Nothing Then
            lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
            lblNombreUsuario.Text = Fila.DataItem.NombreCompleto
            'Indicamos si es el registro seleccionado.
            If Fila.DataItem.ID = gvConsultores_Propiedades.IdSeleccionado Then Fila.RowState = DataControlRowState.Selected
        End If

    End Sub
    Private Sub gvConsultores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvConsultores.RowDeleting
        Dim Tabla As GridView = sender
        e.Cancel = True

        BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
    End Sub
    Private Sub gvConsultores_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvConsultores.PageIndexChanging
        Dim Tabla As GridView = sender
        Tabla.PageIndex = e.NewPageIndex
        gvConsultores_Propiedades.Pagina = Tabla.PageIndex
    End Sub
    Private Sub gvConsultores_PreRender(sender As Object, e As EventArgs) Handles gvConsultores.PreRender
        Dim Tabla As GridView = sender
        Tabla.Ordenar(If(String.IsNullOrWhiteSpace(gvConsultores_Propiedades.CampoOrdenacion), Tabla.DataKeyNames(0).ToString, gvConsultores_Propiedades.CampoOrdenacion), If(gvConsultores_Propiedades.DireccionOrdenacion Is Nothing, SortDirection.Descending, gvConsultores_Propiedades.DireccionOrdenacion.GetValueOrDefault))

        '--------------------------------------------------------------------------------------------------------
        'Al regresar al Listado calculamos la pagina que hay que mostrar respecto al Registro seleccionado.
        '--------------------------------------------------------------------------------------------------------
        If Tabla.DataSource IsNot Nothing AndAlso Tabla.DataSource.count > 0 AndAlso gvConsultores_Propiedades.IdSeleccionado IsNot Nothing _
            AndAlso Page.Request.UrlReferrer IsNot Nothing AndAlso String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault.Replace("/", ""), Page.Request.ApplicationPath.Replace("/", ""), True) <> 0 _
            AndAlso (hdIdConsultor.Value <> String.Empty Or String.Compare(Page.Request.UrlReferrer.Segments.LastOrDefault, Page.Request.Url.Segments.LastOrDefault, True) <> 0) Then

            Dim Lista As List(Of Object) = Tabla.DataSource
            Dim TipoObjeto As Type = Lista.First.GetType
            Dim Propiedades As List(Of Reflection.PropertyInfo) = TipoObjeto.GetProperties.ToList
            Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "Id", True) = 0)
            Dim PosicionReg As Integer = Lista.FindIndex(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing) = gvConsultores_Propiedades.IdSeleccionado)

            If PosicionReg >= 0 Then
                Dim PaginaActual As Integer = Math.Floor(If(PosicionReg < 0, 0, PosicionReg) / Tabla.PageSize)
                gvConsultores_Propiedades.Pagina = PaginaActual
            End If

            hdIdConsultor.Value = String.Empty
        End If
        '--------------------------------------------------------------------------------------------------------

        Tabla.PageIndex = If(gvConsultores_Propiedades.Pagina, 0)
        Tabla.DataBind()
    End Sub
#End Region
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        gvAdministradores.DataSource = (From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                        Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And Usr.IDGRUPO.Equals(Perfil.Administrador)
                                        Select Usr Order By Usr.SAB_USUARIOS.NOMBRE.Trim, Usr.SAB_USUARIOS.APELLIDO1.Trim, Usr.SAB_USUARIOS.APELLIDO2.Trim).AsEnumerable _
                                        .Select(Function(o) New With {.ID = o.ID, .NombreCompleto = If(o.SAB_USUARIOS Is Nothing, "?", String.Format("{0} {1} {2}",
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.NOMBRE), String.Empty, o.SAB_USUARIOS.NOMBRE.Trim),
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.APELLIDO1), String.Empty, o.SAB_USUARIOS.APELLIDO1.Trim),
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.APELLIDO2), String.Empty, o.SAB_USUARIOS.APELLIDO2.Trim)))}).ToList
        gvUsuarios.DataSource = (From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                 Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And Usr.IDGRUPO.Equals(Perfil.Gestor)
                                 Select Usr).AsEnumerable _
                                 .Select(Function(o) New With {.ID = o.ID, .NombreCompleto = If(o.SAB_USUARIOS Is Nothing, "?", String.Format("{0} {1} {2}",
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.NOMBRE), String.Empty, o.SAB_USUARIOS.NOMBRE.Trim),
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.APELLIDO1), String.Empty, o.SAB_USUARIOS.APELLIDO1.Trim),
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.APELLIDO2), String.Empty, o.SAB_USUARIOS.APELLIDO2.Trim)))}).ToList

        gvConsultores.DataSource = (From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                    Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And Usr.IDGRUPO.Equals(Perfil.Consultor)
                                    Select Usr).AsEnumerable _
                                 .Select(Function(o) New With {.ID = o.ID, .NombreCompleto = If(o.SAB_USUARIOS Is Nothing, "?", String.Format("{0} {1} {2}",
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.NOMBRE), String.Empty, o.SAB_USUARIOS.NOMBRE.Trim),
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.APELLIDO1), String.Empty, o.SAB_USUARIOS.APELLIDO1.Trim),
                                                                                                                                              If(String.IsNullOrWhiteSpace(o.SAB_USUARIOS.APELLIDO2), String.Empty, o.SAB_USUARIOS.APELLIDO2.Trim)))}).ToList
    End Sub
    Sub BorrarUsuario(ByVal IdUsuario As Integer)
        Dim GruposComponent As New SabLib.BLL.GruposComponent
        Dim Usr As BatzBBDD.USUARIOSGRUPOS = (From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS Where Reg.ID = IdUsuario Select Reg).SingleOrDefault
        If Usr IsNot Nothing Then
            GruposComponent.DeleteUsuario(IdGrupoRecurso, CInt(IdUsuario))
            BBDD.USUARIOSGRUPOS.DeleteObject(Usr)
            BBDD.SaveChanges()
        End If
    End Sub
    ''' <summary>
    ''' Gestion de usuarios.
    ''' Crea el usuario  o modifica del perfil si existe el usuario.
    ''' </summary>
    ''' <param name="PerfilUsr">Perfil que se quiere para el usuario.</param>
    ''' <param name="IdUsuario">Identificador de SAB del usuario.</param>
    ''' <remarks></remarks>
    Sub GestionarUsuarios(ByVal PerfilUsr As Perfil, ByVal IdUsuario As Integer)
        Try
            Dim GruposComponent As New SabLib.BLL.GruposComponent
            Dim lPerfiles As IEnumerable(Of Integer) = From p As [Enum] In [Enum].GetValues(GetType(Perfil))
                                                       Where p.GetHashCode <> PerfilUsr.GetHashCode _
                                                           And (p.GetHashCode.Equals(Perfil.Administrador) Or p.GetHashCode.Equals(Perfil.Gestor) Or p.GetHashCode.Equals(Perfil.Consultor))
                                                       Select CInt(p.GetHashCode)
            Using Transaccion As New TransactionScope
                Dim USUARIOSGRUPOS As BatzBBDD.USUARIOSGRUPOS = (From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                                                 Where lPerfiles.Contains(Reg.IDGRUPO) _
                                                                   And Reg.IDUSUARIO = IdUsuario _
                                                                   And Reg.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia
                                                                 Select Reg).SingleOrDefault
                If USUARIOSGRUPOS Is Nothing Then
                    USUARIOSGRUPOS = New BatzBBDD.USUARIOSGRUPOS
                    USUARIOSGRUPOS.IDUSUARIO = IdUsuario
                    BBDD.USUARIOSGRUPOS.AddObject(USUARIOSGRUPOS)
                End If

                USUARIOSGRUPOS.IDGRUPO = PerfilUsr.GetHashCode
                USUARIOSGRUPOS.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia

                BBDD.SaveChanges()

                Transaccion.Complete()

                If PerfilUsr = Perfil.Administrador Then
                    gvAdministradores_Propiedades.IdSeleccionado = USUARIOSGRUPOS.ID
                ElseIf PerfilUsr = Perfil.Gestor Then
                    gvUsuarios_Propiedades.IdSeleccionado = USUARIOSGRUPOS.ID
                ElseIf PerfilUsr = Perfil.Consultor Then
                    gvConsultores_Propiedades.IdSeleccionado = USUARIOSGRUPOS.ID
                End If

            End Using
            BBDD.AcceptAllChanges()

            GruposComponent.AddUsuario(IdUsuario, IdGrupoRecurso)
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region
End Class