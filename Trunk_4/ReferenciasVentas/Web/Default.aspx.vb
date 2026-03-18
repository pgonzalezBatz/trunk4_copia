Public Class _Default
	Inherits PageBase
	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim oUsuario As New BLL.UsuariosBLL
        Dim oUsuariosRol As New BLL.BonoSisBLL
        Dim myTicket As New Sablib.ELL.Ticket
        Dim usuario As New ELL.PerfilUsuario

        Try
            
            If (Session("Ticket") IsNot Nothing) Then
                myTicket = CType(Session("Ticket"), SabLib.ELL.Ticket)
                'Dim usuario As ELL.UsuarioRol = oUsuariosRol.CargarRolUsuario(myTicket.IdUser)
                usuario = CType(Session("PerfilUsuario"), ELL.PerfilUsuario)

                If (System.Configuration.ConfigurationManager.AppSettings("Administradores").ToString.Contains(myTicket.IdUser)) Then
                    'Es usuario administrador
                    Response.Redirect(PageBase.PAG_INICIO_ADMINISTRADOR, False)
                ElseIf (usuario IsNot Nothing) Then
                    Select Case usuario.IdRol
                        Case ELL.Roles.RolUsuario.DocumentationTechnician : Response.Redirect(PageBase.PAG_INICIO_DOCUMENTATION_TECHNICIAN, False)
                        Case ELL.Roles.RolUsuario.ProductEngineer : Response.Redirect(PageBase.PAG_INICIO_PRODUCT_ENGINEER, False)
                        Case ELL.Roles.RolUsuario.ProductManager, ELL.Roles.RolUsuario.ProjectLeader, ELL.Roles.RolUsuario.ProjectLeaderManager : Response.Redirect(PageBase.PAG_INICIO_VALIDATIONS, False)
                        Case Else
                            'No tiene permisos
                            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                    End Select
                Else
                    'No tiene permisos
                    Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                End If
            Else
                'No tiene permisos
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        Catch ex As Exception
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End Try
    End Sub

End Class