Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim lg As New SabLib.BLL.LoginComponent
            Dim myTicket As New SabLib.ELL.Ticket
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.Kem")
            Session(PageBase.STICKET) = Nothing
            myTicket = lg.Login(User.Identity.Name.ToLower)
            If Not myTicket Is Nothing Then
                If AccesoRecurso(myTicket) Then
                    Session(PageBase.SCULTURA) = myTicket.Culture
                    Session(PageBase.STICKET) = myTicket
                    If (myTicket.IdPlanta = 1) Then 'Se redirige a la pagina para la eleccion de la planta
                        Session(PageBase.PLANTA_IGORRE) = True
                        Response.Redirect("SeleccionPlanta.aspx")
                    Else
                        Dim plantComp As New SabLib.BLL.PlantasComponent
                        Dim myPlant As SabLib.ELL.Planta = plantComp.GetPlanta(myTicket.IdPlanta)
                        If (myPlant.IdEpsilon <> String.Empty OrElse myPlant.IdIzaro > 0) Then
                            log.Warn("No tiene acceso a KEM porque su planta esta en Epsilon o Izaro. Planta " & myPlant.Nombre & "(" & myPlant.Id & ")")
                            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                        Else
                            log.Info("Acceso directo a la planta de " & myPlant.Nombre)
                            Session(PageBase.PLANTA) = myPlant
                            Response.Redirect(PageBase.PAG_INICIO)
                        End If
                    End If
                    'Gestion antigua que se hacia por las plantas que tuviera asignado el usuario
                    'Dim plantComp As New SabLib.BLL.PlantasComponent
                    'Dim lPlantas As List(Of SabLib.ELL.Planta) = plantComp.GetPlantasUsuario(myTicket.IdUser)
                    'If (lPlantas.Count = 0) Then
                    '    Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                    'Else
                    '    Session(PageBase.SCULTURA) = myTicket.Culture
                    '    Session(PageBase.STICKET) = myTicket
                    '    'Nos quedamos solo con la planta de Igorre y con las que no tengan informado id_Izaro
                    '    lPlantas = lPlantas.FindAll(Function(o As SabLib.ELL.Planta) o.Id = 1 Or o.IdIzaro < 0)
                    '    If (lPlantas.Count = 0) Then 'La persona no puede acceder a KEM   
                    '        log.Warn("No tiene acceso a KEM porque sus plantas estan en Izaro")
                    '        Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                    '    Else
                    '        'Si tiene la planta 1, se le redirigira a una pagina de seleccion de plantas
                    '        If (lPlantas.Find(Function(o1 As SabLib.ELL.Planta) o1.Id = 1) IsNot Nothing) Then
                    '            Session(PageBase.PLANTA_IGORRE) = True
                    '            Response.Redirect("SeleccionPlanta.aspx")
                    '        Else
                    '            log.Info("Acceso directo a la planta de " & lPlantas.First.Nombre)
                    '            Session(PageBase.PLANTA) = lPlantas.First
                    '            Response.Redirect(PageBase.PAG_INICIO)
                    '        End If
                    '    End If
                    'End If                   
                Else
                    Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                End If
            Else
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errLogin", ex)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End Try
    End Sub

    ''' <summary>
    ''' Comprueba si tiene acceso a algun recurso valido
    ''' </summary>
    ''' <param name="mTicket"></param>
    ''' <returns></returns>
    Private Function AccesoRecurso(ByVal mTicket As SabLib.ELL.Ticket) As Boolean
        Dim bConAcceso As Boolean = False
        Dim lg As New SabLib.BLL.LoginComponent
        Dim Recursos As String() = ConfigurationManager.AppSettings.Get(PageBase.RECURSO_KEM).Split(",")
        For Each rec As String In Recursos
            If (lg.AccesoRecursoValido(mTicket, rec)) Then
                bConAcceso = True
                Exit For
            End If
        Next
        Return bConAcceso
    End Function

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

End Class