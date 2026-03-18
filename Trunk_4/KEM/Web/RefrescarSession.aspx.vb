Public Partial Class RefrescarSession
    Inherits Page

    ''' <summary>
    ''' Esta pagina, refrescara la session por haber caducado y redirigira a la pagina de inicio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            Dim lg As New SabLib.BLL.LoginComponent
            Dim myTicket As New SabLib.ELL.Ticket
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.Kem")
            Session(PageBase.STICKET) = Nothing
            myTicket = lg.Login(User.Identity.Name.ToLower)
            If Not myTicket Is Nothing Then
                If AccesoRecurso(myTicket) Then
                    Dim plantComp As New SabLib.BLL.PlantasComponent
                    Dim lPlantas As List(Of SabLib.ELL.Planta) = plantComp.GetPlantasUsuario(myTicket.IdUser)
                    If (lPlantas.Count = 0) Then
                        Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                    Else
                        Session(PageBase.SCULTURA) = myTicket.Culture
                        Session(PageBase.STICKET) = myTicket
                        'Si tiene la planta 1, se le redirigira a una pagina de seleccion de plantas
                        If (lPlantas.Find(Function(o1 As SabLib.ELL.Planta) o1.Id = 1) IsNot Nothing) Then
                            Session(PageBase.PLANTA_IGORRE) = True
                            Response.Redirect("SeleccionPlanta.aspx")
                        Else
                            Session(PageBase.PLANTA) = lPlantas.Item(0)
                            Response.Redirect(PageBase.PAG_INICIO)
                        End If
                    End If
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
    Private Function AccesoRecurso(ByVal mTicket As SABLib.ELL.Ticket) As Boolean
		Dim bConAcceso As Boolean = False
		Dim lg As New SABLib.BLL.LoginComponent
        Dim Recursos As String() = ConfigurationManager.AppSettings.Get(PageBase.RECURSO_KEM).Split(",")
        For Each rec As String In Recursos
			If (lg.AccesoRecursoValido(mTicket, rec)) Then
				bConAcceso = True
				Exit For
			End If
		Next
		Return bConAcceso
	End Function

End Class