Partial Public Class _Default
    Inherits Page

    ''' <summary>
    ''' Dependiendo el perfil, redirige a una pagina u otra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim pg As New PageBase
        Dim url As String = "~/Viaje/Viajes.aspx"
        Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
        'Primero se comprueba si tienen algun aviso de reimpresion
        Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
        Dim lHG As List(Of String()) = bidaiakBLL.loadListHGReimprimir(ticket.IdUser)
        If (lHG IsNot Nothing AndAlso lHG.Count > 0) Then
            url = "~/Publico/Alertas.aspx"
        Else
            If (pg.hasProfile(BidaiakLib.BLL.BidaiakBLL.Profiles.Consultor)) Then
                url = If(Session("Gerente") IsNot Nothing, "~/Publico/Alertas.aspx", "~/Viaje/Viajes.aspx")
            ElseIf (pg.hasProfile(BidaiakLib.BLL.BidaiakBLL.Profiles.Agencia)) Then
                url = "~/Agencia/SolicitudAgencia.aspx"
            ElseIf (pg.hasProfile(BidaiakLib.BLL.BidaiakBLL.Profiles.Financiero)) Then
                url = "~/Financiero/Anticipos/GestionAnticipos.aspx"
            ElseIf (pg.hasProfile(BidaiakLib.BLL.BidaiakBLL.Profiles.Planificador)) Then
                url = "~/Publico/Alertas.aspx"
            End If
        End If
        Response.Redirect(url, False)
    End Sub

#Region "Comentado - Generar fichero de intereses"

    '#If DEBUG Then
    '        01/07/2017: CODIGO REALIZADO DE PRUEBA PARA GENERAR EL FICHERO DE INTERESES MANDADO POR IGOR. PARA EL AÑO QUE VIENE, HABRA QUE SACARLO DE AQUI Y PONERLO EN OTRO LADO
    '        Dim hojasBLL As New BidaiakLib.BLL.HojasGastosBLL
    '        Dim fichero As String = hojasBLL.GenerarFicheroBancoIntereses(New DateTime(2017, 7, 15), 1)
    '        Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)  'El fichero se tiene que guardar en ANSI
    '        Dim file As Byte() = enc.GetBytes(fichero)
    '        Response.Clear()
    '        Response.AddHeader("Content-Disposition", "attachment; Intereses.txt")
    '        Response.BinaryWrite(file)
    '        Response.End()
    '        Response.Flush()
    '#End If

#End Region

End Class