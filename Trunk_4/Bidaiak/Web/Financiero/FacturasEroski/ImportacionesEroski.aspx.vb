Public Class ImportacionesEroski
    Inherits PageBase

#Region "Carga de datos"

    ''' <summary>
    ''' Se muestran los resumen de las importaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Importaciones de Facturas Eroski"
            pnlPendienteUsuario.Visible = False : pnlPendienteOtroUsuario.Visible = False : pnlNoPendiente.Visible = False
            ComprobarEjecuciones()
        End If
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(btnContinuar) : itzultzaileWeb.Itzuli(lblPendienteOtro)
        End If
    End Sub

    ''' <summary>
    ''' Se comprueba si ya existe alguna ejecucion en curso y si es del usuario actual. Sino, se muestra un resumen
    ''' </summary>    
    Private Sub ComprobarEjecuciones()
        Try
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim oEjec As BLL.BidaiakBLL.Ejecucion = bidaiakBLL.loadEjecucion(BLL.BidaiakBLL.TipoEjecucion.Factura_Eroski, Master.IdPlantaGestion)
            If (oEjec Is Nothing) Then  'No hay ninguna ejecucion incompleta
                pnlNoPendiente.Visible = True
                resumen.iniciar()
            Else
                If (oEjec.IdUser = Master.Ticket.IdUser) Then  'El usuario tiene iniciada una ejecucion
                    pnlPendienteUsuario.Visible = True
                    lblPendiente.Text = itzultzaileWeb.Itzuli("Tiene iniciada una ejecucion de importacion de facturas de Eroski") & " -> Año: " & oEjec.Anno & " y mes:" & TraducirMes(oEjec.Mes)
                    btnContinuar.CommandArgument = oEjec.Anno & "_" & oEjec.Mes
                Else 'Existe otro usuario propietario de la ejecucion 
                    pnlPendienteOtroUsuario.Visible = True
                End If
            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Traduce el mes
    ''' </summary>
    ''' <param name="mes">Numero del mes</param>
    ''' <returns></returns>    
    Private Function TraducirMes(mes As Integer) As String
        Select Case mes
            Case 1 : Return itzultzaileWeb.Itzuli("ene")
            Case 2 : Return itzultzaileWeb.Itzuli("feb")
            Case 3 : Return itzultzaileWeb.Itzuli("mar")
            Case 4 : Return itzultzaileWeb.Itzuli("abr")
            Case 5 : Return itzultzaileWeb.Itzuli("may")
            Case 6 : Return itzultzaileWeb.Itzuli("jun")
            Case 7 : Return itzultzaileWeb.Itzuli("jul")
            Case 8 : Return itzultzaileWeb.Itzuli("ago")
            Case 9 : Return itzultzaileWeb.Itzuli("sep")
            Case 10 : Return itzultzaileWeb.Itzuli("oct")
            Case 11 : Return itzultzaileWeb.Itzuli("nov")
            Case 12 : Return itzultzaileWeb.Itzuli("dic")
            Case Else : Return String.Empty
        End Select
    End Function

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se ha generado un error en el control
    ''' </summary>
    ''' <param name="mensaje">Mensaje del error</param>    
    Private Sub resumen_ErrorGenerado(mensaje As String) Handles resumen.ErrorGenerado
        Master.MensajeError = mensaje
    End Sub

    ''' <summary>
    ''' Se ha generado un evento para subir un fichero
    ''' </summary>
    ''' <param name="año">Año</param>
    ''' <param name="mes">Mes</param>    
    Private Sub resumen_SubirFichero(año As Integer, mes As Integer) Handles resumen.SubirFichero
        Response.Redirect("ImpFacturas.aspx?anno=" & año & "&mes=" & mes)
    End Sub

    ''' <summary>
    ''' Continua con el proceso de subida
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Dim año, mes As Integer
        Dim info As String() = btnContinuar.CommandArgument.Split("_")
        año = CInt(info(0)) : mes = CInt(info(1))
        Response.Redirect("ImpFacturas.aspx?anno=" & año & "&mes=" & mes)
    End Sub

#End Region

End Class