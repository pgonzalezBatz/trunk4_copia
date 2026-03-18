Partial Public Class SubirFotos
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se le dice los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelSubirFoto) : itzultzaileWeb.Itzuli(imgSubir)
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelInfo2)
            itzultzaileWeb.Itzuli(btnEliminar)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.VisualizarCabecera = False
                If (Request("idUser") IsNot Nothing) Then
                    Dim idUser As Integer = CInt(Request("idUser"))
                    Dim userComp As New Sablib.BLL.UsuariosComponent
                    Dim oUser As New Sablib.ELL.Usuario With {.Id = idUser}
                    oUser = userComp.GetUsuario(oUser, False, True)
                    pnlFotoExistente.Visible = (oUser.Foto IsNot Nothing)
                    pnlMostrarEliminar.Visible = (oUser.Foto IsNot Nothing)
                    btnEliminar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');return false;"
                    btnEliminar.CommandArgument = idUser
                Else
                    Master.MensajeErrorText = "Acceso denegado"
                End If
            End If
        Catch ex As Exception
            Master.MensajeErrorText = "errorCargar"
        End Try
    End Sub

#End Region

#Region "Acciones"

    ''' <summary>
    ''' Sube la foto seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub imgSubir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSubir.Click
        Try
            If fuFoto.HasFile Then
                Dim tamañoMax As Integer = CInt(ConfigurationManager.AppSettings("tamañoFichMaxFotoKB").ToString)                
                If (fuFoto.PostedFile.ContentLength < (tamañoMax * 1000)) Then
                    Dim userComp As New Sablib.BLL.UsuariosComponent
                    Dim oUser As New Sablib.ELL.Usuario With {.Id = CInt(btnEliminar.CommandArgument)}
                    oUser = userComp.GetUsuario(oUser, False, True)
                    oUser.Foto = fuFoto.FileBytes
                    If (userComp.Save(oUser)) Then
                        Master.MensajeInfoText = "elementoSubido"
                        RecargarPaginaPadre()
                    Else
                        Master.MensajeErrorText = "errSubir"
                    End If
                Else 'Se ha pasado de tamaño      
                    Dim smsError As String = itzultzaileWeb.Itzuli("tamañoMaximoFicheroSuperado")
                    smsError &= "(" & tamañoMax & " KB)"
                    Master.MensajeError = smsError
                End If
            Else
                Master.MensajeAdvertenciaText = "seleccioneAlgunFichero"
            End If
        Catch ex As Exception
            Master.MensajeErrorText = "errSubir"
        End Try
    End Sub

    ''' <summary>
    ''' Elimina la foto actual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            Dim userComp As New Sablib.BLL.UsuariosComponent
            Dim oUser As New Sablib.ELL.Usuario With {.Id = CInt(btnEliminar.CommandArgument)}
            Dim sinFoto As Byte() = {}
            oUser = userComp.GetUsuario(oUser, False, True)
            oUser.Foto = sinFoto
            If (userComp.Save(oUser)) Then
                Master.MensajeInfoText = "elementoBorrado"
                RecargarPaginaPadre()
            End If
        Catch ex As Exception
            Master.MensajeErrorText = "errBorrar"
        End Try
    End Sub

#End Region

#Region "Pagina padre"

    ''' <summary>
    ''' Recarga la pagina padre
    ''' </summary>    
    Private Sub RecargarPaginaPadre()
        Dim script As New Text.StringBuilder
        script.Append("if (window.opener){opener = window.opener;}")
        script.Append("else{opener = window.dialogArguments;}")
        script.Append("opener.ReloadPage(" & CInt(Request("idUser")) & ");")
        script.Append("self.close();")
        Page.ClientScript.RegisterStartupScript(GetType(String), "Success", script.ToString(), True)
    End Sub

#End Region

End Class