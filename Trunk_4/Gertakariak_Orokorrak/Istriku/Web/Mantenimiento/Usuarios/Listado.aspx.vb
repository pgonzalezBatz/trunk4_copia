Public Class Listado
	Inherits PageBase

#Region "Propiedades"
	Public BBDD As New BatzBBDD.Entities_Gertakariak
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
    Private Sub gvAdministradores_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvAdministradores.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim lblNombreUsuario As Label

        If Fila.DataItem IsNot Nothing Then
            lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
            Dim UsrGrup As BatzBBDD.USUARIOSGRUPOS = Fila.DataItem
            Dim Usuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = UsrGrup.IDUSUARIO}, False)
            If Usuario IsNot Nothing Then lblNombreUsuario.Text = Usuario.NombreCompleto
        End If
    End Sub
    Private Sub gvAdministradores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvAdministradores.RowDeleting
        Dim Tabla As GridView = sender
        e.Cancel = True

        BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
    End Sub

    Private Sub btnAgregarAdministradorPlanta_Click(sender As Object, e As EventArgs) Handles btnAgregarAdministradorPlanta.Click
        Try
            GestionarUsuarios(Perfil.AdministradorPlanta, hdIdAdministradorPlanta.Value)
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gvAdministradoresPlanta_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvAdministradoresPlanta.RowCreated
        Dim Tabla As GridView = sender
        Dim Fila As GridViewRow = e.Row
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim lblNombreUsuario As Label

        If Fila.DataItem IsNot Nothing Then
            lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
            Dim UsrGrup As BatzBBDD.USUARIOSGRUPOS = Fila.DataItem
            Dim Usuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = UsrGrup.IDUSUARIO}, False)
            If Usuario IsNot Nothing Then lblNombreUsuario.Text = Usuario.NombreCompleto
        End If
    End Sub
    Private Sub gvAdministradoresPlanta_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvAdministradoresPlanta.RowDeleting
        Dim Tabla As GridView = sender
        e.Cancel = True

        BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
    End Sub

    Private Sub btnAgregarUsuario_Click(sender As Object, e As EventArgs) Handles btnAgregarUsuario.Click
		Try
			GestionarUsuarios(Perfil.Usuario, hfIdUsuario.Value)
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try

	End Sub
	Private Sub gvUsuarios_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvUsuarios.RowCreated
		Dim Tabla As GridView = sender
		Dim Fila As GridViewRow = e.Row
		Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
		Dim lblNombreUsuario As Label

		If Fila.DataItem IsNot Nothing Then
			lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
			Dim UsrGrup As BatzBBDD.USUARIOSGRUPOS = Fila.DataItem
			Dim Usuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = UsrGrup.IDUSUARIO}, False)
			If Usuario IsNot Nothing Then lblNombreUsuario.Text = Usuario.NombreCompleto
		End If
	End Sub
	Private Sub gvUsuarios_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvUsuarios.RowDeleting
		Dim Tabla As GridView = sender
		e.Cancel = True

		BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
	End Sub

	Private Sub btnAgregarConsultor_Click(sender As Object, e As EventArgs) Handles btnAgregarConsultor.Click
		GestionarUsuarios(Perfil.Consultor, hfIdConsultor.Value)
	End Sub
	Private Sub gvConsultores_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvConsultores.RowCreated
		Dim Tabla As GridView = sender
		Dim Fila As GridViewRow = e.Row
		Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
		Dim lblNombreUsuario As Label

		If Fila.DataItem IsNot Nothing Then
			lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
			Dim UsrGrup As BatzBBDD.USUARIOSGRUPOS = Fila.DataItem
			Dim Usuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = UsrGrup.IDUSUARIO}, False)
			If Usuario IsNot Nothing Then lblNombreUsuario.Text = Usuario.NombreCompleto
		End If
	End Sub
	Private Sub gvConsultores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvConsultores.RowDeleting
		Dim Tabla As GridView = sender
		e.Cancel = True

		BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
	End Sub

	Private Sub btnAgregarUsrAcceso_Click(sender As Object, e As EventArgs) Handles btnAgregarUsrAcceso.Click
		GestionarUsuarios(Perfil.UsuarioAcceso, hfIdUsrAcceso.Value)
	End Sub
	Private Sub gvUsrAcceso_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvUsrAcceso.RowCreated
		Dim Tabla As GridView = sender
		Dim Fila As GridViewRow = e.Row
		Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
		Dim lblNombreUsuario As Label

		If Fila.DataItem IsNot Nothing Then
			lblNombreUsuario = Fila.FindControl("lblNombreUsuario")
			Dim UsrGrup As BatzBBDD.USUARIOSGRUPOS = Fila.DataItem
			Dim Usuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = UsrGrup.IDUSUARIO}, False)
			If Usuario IsNot Nothing Then lblNombreUsuario.Text = Usuario.NombreCompleto
		End If
	End Sub
	Private Sub gvUsrAcceso_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvUsrAcceso.RowDeleting
		Dim Tabla As GridView = sender
		e.Cancel = True

		BorrarUsuario(Tabla.DataKeys(e.RowIndex).Value)
	End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Dim idPlanta As Integer = CInt(If(Session("IDPLANTA"), Ticket.IdPlanta))

        pnlAdmin.Attributes.Add("style", If(PerfilUsuario.Equals(PageBase.Perfil.Administrador), "pointer-events:auto", "pointer-events:none"))
        pnlAdmin.Attributes.Add("style", If(PerfilUsuario.Equals(PageBase.Perfil.Administrador), "display:auto", "display:none"))

        gvAdministradores.DataSource = From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                       Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And Usr.IDGRUPO = Perfil.Administrador AndAlso (Usr.SAB_USUARIOS.FECHABAJA Is Nothing OrElse Usr.SAB_USUARIOS.FECHABAJA > Date.Now)
                                       Select Usr Order By Usr.SAB_USUARIOS.NOMBRE.Trim, Usr.SAB_USUARIOS.APELLIDO1.Trim, Usr.SAB_USUARIOS.APELLIDO2.Trim
        gvAdministradores.DataBind()

        gvAdministradoresPlanta.DataSource = From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                             Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia And Usr.IDGRUPO = Perfil.AdministradorPlanta AndAlso CInt(Usr.SAB_USUARIOS.IDPLANTA) = idPlanta AndAlso (Usr.SAB_USUARIOS.FECHABAJA Is Nothing OrElse Usr.SAB_USUARIOS.FECHABAJA > Date.Now)
                                             Select Usr Order By Usr.SAB_USUARIOS.NOMBRE.Trim, Usr.SAB_USUARIOS.APELLIDO1.Trim, Usr.SAB_USUARIOS.APELLIDO2.Trim
        gvAdministradoresPlanta.DataBind()

        gvUsuarios.DataSource = From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia AndAlso Usr.IDGRUPO = Perfil.Usuario AndAlso CInt(Usr.SAB_USUARIOS.IDPLANTA) = idPlanta AndAlso (Usr.SAB_USUARIOS.FECHABAJA Is Nothing OrElse Usr.SAB_USUARIOS.FECHABAJA > Date.Now)
                                Select Usr Order By Usr.SAB_USUARIOS.NOMBRE.Trim, Usr.SAB_USUARIOS.APELLIDO1.Trim, Usr.SAB_USUARIOS.APELLIDO2.Trim
        gvUsuarios.DataBind()

        gvConsultores.DataSource = From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                   Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia AndAlso Usr.IDGRUPO = Perfil.Consultor AndAlso CInt(Usr.SAB_USUARIOS.IDPLANTA) = idPlanta AndAlso (Usr.SAB_USUARIOS.FECHABAJA Is Nothing OrElse Usr.SAB_USUARIOS.FECHABAJA > Date.Now)
                                   Select Usr Order By Usr.SAB_USUARIOS.NOMBRE.Trim, Usr.SAB_USUARIOS.APELLIDO1.Trim, Usr.SAB_USUARIOS.APELLIDO2.Trim
        gvConsultores.DataBind()

        gvUsrAcceso.DataSource = From Usr As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                 Where Usr.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia AndAlso Usr.IDGRUPO = Perfil.UsuarioAcceso AndAlso CInt(Usr.SAB_USUARIOS.IDPLANTA) = idPlanta AndAlso (Usr.SAB_USUARIOS.FECHABAJA Is Nothing OrElse Usr.SAB_USUARIOS.FECHABAJA > Date.Now)
                                 Select Usr Order By Usr.SAB_USUARIOS.NOMBRE.Trim, Usr.SAB_USUARIOS.APELLIDO1.Trim, Usr.SAB_USUARIOS.APELLIDO2.Trim
        gvUsrAcceso.DataBind()
    End Sub
    Sub BorrarUsuario(ByVal IdUsuario As Integer)
		Dim GruposComponent As New SabLib.BLL.GruposComponent
		Dim Usr As BatzBBDD.USUARIOSGRUPOS = (From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS Where Reg.ID = IdUsuario Select Reg).SingleOrDefault
		If Usr IsNot Nothing Then
			GruposComponent.DeleteUsuario(My.Settings.IdGrupoRecurso, CInt(IdUsuario))
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
                                                   And (p.GetHashCode = Perfil.Administrador Or p.GetHashCode = Perfil.Usuario Or p.GetHashCode = Perfil.Consultor Or p.GetHashCode = Perfil.AdministradorPlanta)
                                                       Select CInt(p.GetHashCode)
            Using Transaccion As New TransactionScope
				Dim USUARIOSGRUPOS As BatzBBDD.USUARIOSGRUPOS = (From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS _
																   Where lPerfiles.Contains(Reg.IDGRUPO) _
																   And Reg.IDUSUARIO = IdUsuario _
																   And Reg.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia _
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
			End Using
			BBDD.AcceptAllChanges()

			GruposComponent.AddUsuario(IdUsuario, My.Settings.IdGrupoRecurso)
		Catch ex As ApplicationException
			Throw
		Catch ex As Exception
			Throw
		End Try
	End Sub
#End Region
End Class