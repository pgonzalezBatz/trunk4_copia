
Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la aplicación
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

    ''' <summary>
    ''' Inicio de session
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)        
        Try                        
            Dim lg As New SabLib.BLL.LoginComponent
            Dim myTicket As New SabLib.ELL.Ticket
            Dim Recurso As String = ConfigurationManager.AppSettings.Get("RecursoWeb")
            myTicket = lg.Login(User.Identity.Name.ToLower)
            'TEST:myTicket = lg.Login("mexicana\alvalganon")
            If Not myTicket Is Nothing Then
                If lg.AccesoRecursoValido(myTicket, Recurso) Then
                    Dim administradores As String() = ConfigurationManager.AppSettings("Administradores").Split(",")
                    For Each admin As String In administradores
                        If (CInt(admin) = myTicket.IdUser) Then
                            Session("Admin") = "1"
                            Exit For
                        End If
                    Next
                    Session("Ticket") = myTicket
                    Dim compBLL As New BLL.Component
                    Dim lPlantasGerente As List(Of Object) = compBLL.loadGerentesPlantas(myTicket.IdUser)
                    Dim lPlantasAux As List(Of Integer) = Nothing
                    If (lPlantasGerente IsNot Nothing AndAlso lPlantasGerente.Count > 0) Then
                        lPlantasAux = New List(Of Integer)
                        Dim plantBLL As New BLL.PlantasComponent
                        Dim plantaKPI As ELL.Planta
                        For Each plant In lPlantasGerente
                            If (plant.IdPlanta = 4 OrElse plant.IdPlanta = 107 OrElse plant.IdPlanta = 147) Then 'Si es gerente de Kunshan, Chengdu o Batz Guanzhou, se le asigna China consolidado
                                plantaKPI = New ELL.Planta With {.Id = 7}
                            Else
                                plantaKPI = plantBLL.loadListPlantas(New ELL.Planta With {.IdPlantaSAB = plant.IdPlanta, .Avisar = True}).FirstOrDefault  'Solo se obtienen las plantas de las que puede avisar                                                            
                            End If
                            If (plantaKPI IsNot Nothing AndAlso Not lPlantasAux.Exists(Function(o) o = plantaKPI.Id)) Then
                                lPlantasAux.Add(plantaKPI.Id)
                            End If
                        Next
                    End If
                    Session("Gerente") = lPlantasAux
                    PageBase.log.Info(myTicket.NombreCompleto & " ha entrado")
                    Response.Redirect(Request.Url.AbsoluteUri)  'Para que acceda a una pagina directament       
                Else
                    PageBase.log.Error("No tiene acceso al recurso")
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=2")
                End If
            Else
                PageBase.log.Error("No tiene ticket")
                Response.Redirect("~/PermisoDenegado.aspx?mensa=1")
            End If            
        Catch ex As Exception
            PageBase.log.Error("Excepcion en el login", ex)
            Response.Redirect("~/PermisoDenegado.aspx?mensa=3")
        End Try
    End Sub

End Class