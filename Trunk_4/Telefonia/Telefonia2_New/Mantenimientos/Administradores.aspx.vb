Partial Public Class Administradores
    Inherits PageBase

#Region "Page_Load"

    ''' <summary>
    ''' <para>Se muestran los administradores existentes y los usuarios a elegir</para>
    ''' <para>Los que ya son administradores, se quitan de la lista de usuarios</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Administradores"
                'La primera vez que se muestran los usuarios, hay que quitar los que ya son administradores
                Dim litem As ListItem
                Dim listUsu As List(Of SabLib.ELL.Usuario) = ListarUsuarios()
                Dim listAdm As List(Of SabLib.ELL.Usuario) = ListarAdministradores()
                If (listAdm IsNot Nothing) Then
                    For Each item As SabLib.ELL.Usuario In listAdm
                        litem = ddlUsuarios.Items.FindByValue(item.Id)
                        If (litem IsNot Nothing) Then ddlUsuarios.Items.Remove(litem)
                    Next
                End If
                setPaneles()
            End If
        Catch batz As BatzException
            Master.MensajeError = batz.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelUsers) : itzultzaileWeb.Itzuli(imgAgregar)
        End If
    End Sub

    ''' <summary>
    ''' Visualiza u oculta los paneles de seleccion de usuarios
    ''' </summary>
    Private Sub setPaneles()
        Dim bUsuarios As Boolean = (ddlUsuarios.Items.Count > 0)
        pnlUsuarios.Visible = bUsuarios
        pnlSinUsuarios.Visible = Not bUsuarios
    End Sub

#End Region

#Region "Listado de usuarios"

    ''' <summary>
    ''' Lista todos los usuarios
    ''' </summary>
    ''' <returns>Lista de objetos usuario</returns>
    Private Function ListarUsuarios() As List(Of SabLib.ELL.Usuario)
        Dim userComp As New BLL.UsuariosComponent
        Dim list As List(Of SabLib.ELL.Usuario)
        Try
            list = userComp.getUsuarios(Master.Ticket.IdPlantaActual)
            If (list.Count > 0) Then
                'Los usuarios sin nombre completo de la persona, no se listaran
                For Each oUser As SabLib.ELL.Usuario In list
                    If (oUser.NombreCompleto.Trim <> String.Empty) Then
                        ddlUsuarios.Items.Add(New ListItem(oUser.NombreCompleto, oUser.Id))
                    End If
                Next
            End If
            setPaneles()
            Return list
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("errIKSmostrarUsuarios", ex)
        End Try

    End Function

    ''' <summary>
    ''' Lista los administradores
    ''' </summary>
    ''' <returns>Lista de objetos usuario con los administradores</returns>
    Private Function ListarAdministradores() As List(Of SabLib.ELL.Usuario)
        Dim userComp As New BLL.UsuariosComponent
        Dim list As List(Of SabLib.ELL.Usuario)
        Try
            list = userComp.getAdministradoresPlanta(Master.Ticket.IdPlantaActual)
            rptAdministradores.DataSource = list
            rptAdministradores.DataBind()
            Return list
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("errIKSmostrarAdministradores", ex)
        End Try
    End Function

#End Region

#Region "Agregar y quitar administrador"

    ''' <summary>
    ''' Agrega un administrador y se quita de la lista de usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AgregarAdministradorPlanta(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAgregar.Click
        Dim userComp As New BLL.UsuariosComponent
        Try
            If Not (userComp.AddAdministradorPlanta(ddlUsuarios.SelectedValue, Master.Ticket.IdPlantaActual)) Then
                Master.MensajeError = itzultzaileWeb.Itzuli("errIKSañadirAdministrador")
                log.Error("No se ha podido añadir al usuario " & ddlUsuarios.SelectedItem.Text & " como administrador de la planta " & Master.Ticket.Planta)
            Else
                log.Info("Se ha añadido al usuario " & ddlUsuarios.SelectedItem.Text & " como administrador de la planta " & Master.Ticket.Planta)
                ddlUsuarios.Items.Remove(ddlUsuarios.SelectedItem)
                setPaneles()
                ListarAdministradores()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("administradorAñadido")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Elimina un administrador y se añade a la lista de usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub DeleteAdministradorPlanta(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim userComp As New BLL.UsuariosComponent
        Try
            Dim img As ImageButton = CType(sender, ImageButton)
            Dim idUsuario As Integer = CInt(img.CommandArgument)
            If Not (userComp.DeleteAdministradorPlanta(idUsuario, Master.Ticket.IdPlantaActual)) Then
                Master.MensajeError = itzultzaileWeb.Itzuli("errIKSborrarAdministrador")
                log.Error("No se ha podido quitar al usuario " & img.CommandName & " como administrador de la planta " & Master.Ticket.Planta)
            Else
                log.Info("Se ha quitado al usuario " & img.CommandName & " como administrador de la planta " & Master.Ticket.Planta)
                ddlUsuarios.Items.Add(New ListItem(img.CommandName, idUsuario))
                setPaneles()
                ListarAdministradores()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("administradorBorrado")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class