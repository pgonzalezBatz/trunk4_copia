Partial Public Class Perfiles
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Gestion de perfiles"
            inicializar()
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(chbAccesoDir) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(chbAdmin) : itzultzaileWeb.Itzuli(lnkEliminar) : itzultzaileWeb.Itzuli(labelConfirmTitle)
            itzultzaileWeb.Itzuli(labelConfirmMessage) : itzultzaileWeb.Itzuli(btnConfirmDelete) : itzultzaileWeb.Itzuli(labelConfirmCerrar)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>	
    Private Sub inicializar()
        mvUsuarios.ActiveViewIndex = 0
        searchUser.Limpiar()
        searchUser.PlaceHolder = itzultzaileWeb.Itzuli("IdSab, nombre, apellidos, nombre de usuario o numero de trabajador")
    End Sub

#End Region

#Region "Vista Listado"

    ''' <summary>
    ''' Se selecciona un usuario
    ''' </summary>
    ''' <param name="id"></param>
    Private Sub searchUser_ItemSeleccionado(id As Integer) Handles searchUser.ItemSeleccionado
        Try
            mostrarDetalle(id)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Vista Detalle"

    ''' <summary>
    ''' Muestra el detalle del usuario
    ''' </summary>
    ''' <param name="idUsuario">Identificador del usuario a cargar</param>   
    Private Sub mostrarDetalle(ByVal idUsuario As Integer)
        Try
            mvUsuarios.ActiveViewIndex = 1
            lnkEliminar.Visible = False
            Dim oUser As New SabLib.ELL.Usuario With {.Id = idUsuario}
            Dim userComp As New SabLib.BLL.UsuariosComponent
            CargarPerfiles()
            oUser = userComp.GetUsuario(oUser, False)
            lblPersona.Text = oUser.NombreCompleto
            hfIdUsuario.Value = oUser.Id
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim myPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, oUser.Id, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
            Dim idPerfil As Integer = CInt(myPerfil(0))
            chbAccesoDir.Checked = (myPerfil(1) = "1")
            chbAdmin.Checked = False
            If (hasProfileUser(idPerfil, BLL.BidaiakBLL.Profiles.Administrador)) Then
                chbAdmin.Checked = True
                lnkEliminar.Visible = True
                lnkEliminar.Attributes.Add("href", "#confirmDelete")
            Else
                If Not (hasProfileUser(idPerfil, BLL.BidaiakBLL.Profiles.Consultor)) Then
                    lnkEliminar.Visible = True
                    lnkEliminar.Attributes.Add("href", "#confirmDelete")
                    chbAdmin.Checked = False
                    For Each item As Integer In ExtraerPerfiles(idPerfil)
                        chblPerfiles.Items.FindByValue(item).Selected = True
                    Next
                End If
            End If
            For Each check As ListItem In chblPerfiles.Items
                check.Enabled = Not (chbAdmin.Checked)
            Next
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle del usuario/perfil", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se habilitan o no los checks
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub chbAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles chbAdmin.CheckedChanged
        For Each check As ListItem In chblPerfiles.Items
            check.Enabled = Not (chbAdmin.Checked)
            If (chbAdmin.Checked) Then check.Selected = False
        Next
    End Sub

    ''' <summary>
    ''' Extrae los perfiles
    ''' </summary>
    ''' <returns></returns>    
    Private Function ExtraerPerfiles(ByVal idPerfil As Integer) As List(Of BLL.BidaiakBLL.Profiles)
        Dim lProf As New List(Of BLL.BidaiakBLL.Profiles)
        For Each iProf As Integer In [Enum].GetValues(GetType(BLL.BidaiakBLL.Profiles))
            If ((idPerfil And iProf) = iProf) Then lProf.Add(iProf)
        Next
        Return lProf
    End Function

    ''' <summary>
    ''' Carga los perfiles
    ''' En este checkboxlist, no se carga el del administrador
    ''' </summary>
    Private Sub CargarPerfiles()
        If (chblPerfiles.Items.Count = 0) Then
            For Each item As Integer In [Enum].GetValues(GetType(BLL.BidaiakBLL.Profiles))
                If (item <> BLL.BidaiakBLL.Profiles.Administrador AndAlso item <> BLL.BidaiakBLL.Profiles.Consultor) Then chblPerfiles.Items.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(BLL.BidaiakBLL.Profiles), item).Replace("_", " ")), item))
            Next
        End If
        chblPerfiles.ClearSelection()
    End Sub

    ''' <summary>
    ''' Asocia el perfil al usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Dim bidaiBLL As New BLL.BidaiakBLL
            Dim perfil As Integer = 0
            If (chbAdmin.Checked) Then
                perfil = BLL.BidaiakBLL.Profiles.Administrador
            Else
                For Each check As ListItem In chblPerfiles.Items
                    If (check.Selected) Then perfil += check.Value
                Next
            End If
            If (perfil = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar un perfil")
            Else
                bidaiBLL.setProfile(Master.IdPlantaGestion, CInt(hfIdUsuario.Value), perfil, chbAccesoDir.Checked, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")), CInt(ConfigurationManager.AppSettings("Grupo_Admon")))
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Perfil asociado")
                log.Info("Se ha cambiado el perfil del usuario (" & hfIdUsuario.Value & ")")
                Volver()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los perfiles", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Elimina el perfil del usuario y se convierte en consultor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnConfirmDelete_Click(sender As Object, e As EventArgs) Handles btnConfirmDelete.Click
        Try
            Dim bidaiBLL As New BLL.BidaiakBLL
            bidaiBLL.setProfile(Master.IdPlantaGestion, CInt(hfIdUsuario.Value), BLL.BidaiakBLL.Profiles.Consultor, False, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")), CInt(ConfigurationManager.AppSettings("Grupo_Admon")))
            log.Info("Se ha borrado el perfil del usuario (" & hfIdUsuario.Value & ")")
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Perfil borrado")
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>	
    Private Sub Volver()
        mvUsuarios.ActiveViewIndex = 0
        inicializar()
    End Sub

#End Region

End Class