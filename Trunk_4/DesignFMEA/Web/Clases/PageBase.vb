Imports System.Globalization.CultureInfo

Public Class PageBase
    Inherits Page

#Region "Variables compartidas"

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.DesignFMEA")
    'Public itzultzaileWeb As New LocalizationLib.Itzultzaile
    Public itzultzaileWeb As New TraduccionesLib.itzultzaile

#End Region

#Region "Ticket"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            End If

        End Get
        Set(ByVal value As SabLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
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
            WriteLog("Durante la navegacion, se ha perdido el ticket", TipoLog.Warn, Nothing)
            Response.Redirect("~/PermisoDenegado.aspx?mensa=3")
        End If
    End Sub

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
                Ticket.Culture = cultureInfo.Name
                Culture = cultureInfo.Name
            Else
                Culture = Ticket.Culture
            End If
            Ticket.Culture = CurrentCulture.Name
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
        Dim myDec As String = String.Empty
        If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
            myDec = sDec.Trim.Replace(".", ",")
        ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
            myDec = sDec.Trim.Replace(",", ".")
        End If
        Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
    End Function

#End Region

#Region "log4net"

    ''' <summary>
    ''' Enumeracion con los valores posibles del log
    ''' </summary>
    Public Enum TipoLog As Integer
        Info = 0
        Err = 1
        Warn = 2
    End Enum

    ''' <summary>
    ''' Escribe en el log un texto
    ''' </summary>
    ''' <param name="texto">Texto a escribir</param>
    ''' <param name="tipo">Tipo de mensaje (Informacion, advertencia o error)</param>
    ''' <param name="ex">Parametro opcional con la excepcion lanzada en su caso</param>	
    Public Shared Sub WriteLog(ByVal texto As String, ByVal tipo As TipoLog, Optional ByVal ex As Exception = Nothing)
        Try
            Dim myTicket As SabLib.ELL.Ticket = Nothing
            If (HttpContext.Current.Session("Ticket") IsNot Nothing) Then
                myTicket = CType(HttpContext.Current.Session("Ticket"), SabLib.ELL.Ticket)
            End If
            If (myTicket IsNot Nothing) Then
                texto &= " [user]:" & myTicket.NombreUsuario
            End If
            If (tipo = TipoLog.Info) Then
                log.Info(texto)
            ElseIf (tipo = TipoLog.Err) Then
                log.Error(texto, ex)
            ElseIf (tipo = TipoLog.Warn) Then
                log.Warn(texto, ex)
            End If
        Catch
        End Try
    End Sub

#End Region

End Class