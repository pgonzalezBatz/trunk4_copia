Public Class PageBase
    Inherits Page

#Region "Variables compartidas / Properties"

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")
    Public itzultzaileWeb As New itzultzaile

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private ReadOnly Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            End If
        End Get
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' <para>Si no tiene ticket, se vuelve a logear</para>    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("Ticket") Is Nothing Then
            log.Warn("Durante la navegacion, se ha perdido el ticket")
            Response.Redirect("~/PermisoDenegado.aspx?mensa=3", True)
        Else
            Try
                ComprobarAcceso()
            Catch ex As Exception
                log.Error("Se ha producido un error al consultar si el usuario tenia acceso a la pagina", ex)
                Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
            End Try
        End If
    End Sub

#End Region

#Region "Acceso y perfiles"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la pagina
    ''' </summary>
    Private Sub ComprobarAcceso()
        If (Not hasProfile(BLL.BidaiakBLL.Profiles.Administrador)) Then
            Dim bSinAcceso As Boolean = False
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            Dim numsegment As Integer
#If (DEBUG) Then
            numsegment = 2
#Else
            numsegment=3
#End If
            'En test es : / Viajes Solicitud
            'En real es : atxerre Bidaiak Viajes Solicitud
            If (segmentos.Count > numsegment) Then  'Se ha metido en una pagina de alguna carpeta (bidaiak cuenta como 1)
                Dim nameCarpeta As String = segmentos(numsegment - 1).Substring(0, segmentos(numsegment - 1).Length - 1).ToLower
                Select Case nameCarpeta
                    Case "agencia"
                        If Not (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then bSinAcceso = True
                    Case "financiero"
                        If Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then bSinAcceso = True
                    Case "administracion"
                        bSinAcceso = True
                    Case "rrhh"
                        If Not (hasProfile(BLL.BidaiakBLL.Profiles.RRHH)) Then bSinAcceso = True
                    Case "docproyectos"
                        If Not (hasProfile(BLL.BidaiakBLL.Profiles.Documentacion_Proyectos)) Then bSinAcceso = True
                End Select
                If (bSinAcceso) Then
                    log.Warn("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")")
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=3", True)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Comprueba si tiene ese perfil u otro. Es un or
    ''' </summary>    
    ''' <param name="iProfs">Perfiles a buscar</param>
    ''' <returns></returns>        
    Public Function hasProfile(ByVal ParamArray iProfs As Integer()) As Boolean
        Dim iProf As Integer = CInt(Session("Perfil"))
        For Each item As Integer In iProfs
            If ((iProf And item) = item) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Comprueba si tiene un perfil entre una lista de perfiles
    ''' </summary>   
    ''' <param name="iProf">Id del perfil</param> 
    ''' <param name="iProfs">Perfiles a buscar</param>
    ''' <returns></returns>        
    Public Function hasProfileUser(ByVal iProf As Integer, ByVal ParamArray iProfs As Integer()) As Boolean
        For Each item As Integer In iProfs
            If ((iProf And item) = item) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Comprueba si tiene ese perfil
    ''' </summary>    
    ''' <param name="iProfs">Perfiles a buscar</param>
    ''' <returns></returns>
    Public Function hasProfileAND(ByVal ParamArray iProfs As Integer()) As Boolean
        Dim iProf As Integer = CInt(Session("Perfil"))
        For Each item As Integer In iProfs
            If Not ((iProf And item) = item) Then
                Return False
            End If
        Next
        Return True
    End Function

#End Region

#Region "Cultura"

    ''' <summary>
    ''' Inicializa la cultura
    ''' </summary>	
    Public Sub inicializarCultura()
        MyBase.InitializeCulture()
    End Sub

    ''' <summary>
    ''' Inicializacion de la cultura
    ''' </summary>    
    Protected Overrides Sub InitializeCulture()
        Try
            If Ticket Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
                Culture = cultureInfo.Name
            Else
                Culture = Ticket.Culture
            End If
            MyBase.InitializeCulture()
        Catch
        End Try
    End Sub

#End Region

#Region "Decimales"

    ''' <summary>
    ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
    ''' </summary>
    ''' <param name="sDec">Numero a convertir</param>
    ''' <returns></returns>	
    Public Shared Function DecimalValue(ByVal sDec As String) As Decimal
        If (Not String.IsNullOrEmpty(sDec)) Then
            Dim myDec As String = String.Empty
            If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                myDec = sDec.Trim.Replace(".", ",")
            ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                myDec = sDec.Trim.Replace(",", ".")
            End If
            myDec = If(myDec = String.Empty, "0", myDec)
            Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Formatea el numero para que aparezca con el formato 1.000,5
    ''' Habra que tener en cuenta la cultura
    ''' </summary>
    ''' <param name="sNumber">Numero a formatear</param>
    ''' <returns></returns>    
    Public Shared Function FormatearNumero(ByVal sNumber As String, Optional ByVal numDecimals As Integer = 0) As String
        If (sNumber.StartsWith(",") Or sNumber.StartsWith(".")) Then sNumber = "0" & sNumber
        If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
            sNumber = sNumber.Replace(".", ",")
        ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
            sNumber = sNumber.Replace(",", ".")
        End If
        Dim num As Decimal = CDec(sNumber)
        If (num < 0) Then 'Si es negativo, mostramos 0
            Return "0"
        Else
            Return FormatNumber(sNumber, numDecimals, , , TriState.True)
        End If
    End Function

#End Region

#Region "Cuerpo de email"

    ''' <summary>
    ''' Prepara el cuerpo del email
    ''' </summary>
    ''' <param name="titulo">Titulo del email</param>
    ''' <param name="idViajeHoja">Id de la hoja del viaje</param>
    ''' <param name="cuerpo">cuerpo del mensaje</param>
    ''' <param name="linkUrl">Link a pintar con la url</param>
    ''' <param name="bAccesoPortal">Si es true, se accedera a traves del portal del empleado. Si no directamente</param>
    ''' <param name="https">Indica si es https o http</param>
    ''' <param name="notas">Texto que se añadira al final del email</param>
    ''' <returns>Html con el cuerpo del email</returns>    
    Public Shared Function getBodyHmtl(ByVal titulo As String, ByVal idViajeHoja As String, ByVal cuerpo As String, Optional ByVal linkUrl As String = "", Optional ByVal bAccesoPortal As Boolean = True, Optional ByVal https As Boolean = False, Optional ByVal notas As String = "") As String
        Dim html As New StringBuilder
        html.AppendLine("<html><body>")
        html.AppendLine("<table style='width:100%;border-collapse: collapse;'>")
        html.AppendLine("<tr style='background-color:#5599FF;color:#FFFFFF;font-weight:bold;'>")
        html.AppendLine("<td colspan='2'>")
        html.AppendLine(titulo.ToUpper)
        If (idViajeHoja <> String.Empty) Then html.AppendLine(" (" & idViajeHoja & ")")
        html.AppendLine("</td>")
        html.AppendLine("</tr>")
        html.AppendLine("<tr style='background-color:#E1EDFF;'>")
        html.AppendLine("<td colspan='2' style='font-weight:bold;'>")
        html.AppendLine(cuerpo & "<br /><br />")
        If (linkUrl <> String.Empty) Then
            If (bAccesoPortal) Then
                Dim urlParam As String = HttpUtility.UrlEncode("http://intranet2.batz.es/Bidaiak/" & linkUrl)
                html.AppendLine("<a href='http" & If(https, "s", "") & "://intranet2.batz.es/langileenTxokoa/Default.aspx?url=" & urlParam & "'>" & "Acceder a Bidaiak" & "</a>")
            Else
                html.AppendLine("<a href='http" & If(https, "s", "") & "://intranet2.batz.es/Bidaiak/" & linkUrl & "'>" & "Acceder a Bidaiak" & "</a>")
            End If
        End If
        html.AppendLine("</td>")
        html.AppendLine("</tr>")
        If (notas <> String.Empty) Then
            html.AppendLine("<tr style='background-color:#E1EDFF;'>")
            html.AppendLine("<td colspan='2' style='font-weight:bold;'>")
            html.AppendLine("<b><span style='color:#d61d09'>" & notas & "</span></b>")
            html.AppendLine("</td>")
            html.AppendLine("</tr>")
        End If
        html.AppendLine("</table>")
        html.AppendLine("</body></html>")
        Return html.ToString
    End Function

#End Region

End Class