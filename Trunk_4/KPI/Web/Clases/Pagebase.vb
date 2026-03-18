Imports System.Globalization.CultureInfo

Public Class PageBase
    Inherits Page

#Region "Variables compartidas"

    Public itzultzaileWeb As New itzultzaile
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.KPI")

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
    ''' <para>Si no tiene ticket, se redirecciona a una pagina de permiso denegado</para>
    ''' <para>Comprueba que esa pagina es accesible por el perfil</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Ticket Is Nothing) Then
            Response.Redirect("~/PermisoDenegado.aspx?mensa=1")
        Else
            'hay que comprobar que siempre que se cargue una pagina que este en la carpeta Mantenimientos, solo puedan acceder administradores
            Dim segmentos As String() = Page.Request.Url.Segments
            If (segmentos.Count > 2 AndAlso Session("Admin") Is Nothing AndAlso segmentos(1) = "Mantenimientos/") Then
                Response.Redirect("~/PermisoDenegado.aspx?mensa=3")
            End If
        End If
    End Sub

#End Region

#Region "Decimal"

    ''' <summary>
    ''' Convierte a decimal un string
    ''' </summary>
    ''' <param name="sDec">Decimal</param>
    ''' <returns></returns>    
    Public Function DecimalValue(sDec As String) As Decimal
        If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
            sDec = sDec.Replace(".", ",")
        ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
            sDec = sDec.Replace(",", ".")
        End If
        Return Convert.ToDecimal(sDec)
    End Function

#End Region

#Region "Cultura"

    ''' <summary>
    ''' Inicializa la cultura
    ''' </summary>    
    Public Sub InicializarCultura()
        MyBase.InitializeCulture()
    End Sub

    ''' <summary>
    ''' Inicializa la cultura del hilo
    ''' </summary>    
    Protected Overrides Sub InitializeCulture()
        Try
            If Ticket Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = cultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Ticket.Culture = cultureInfo.Name
                Culture = cultureInfo.Name
                Ticket.Culture = CurrentCulture.Name
            Else
                Culture = Ticket.Culture
            End If
            MyBase.InitializeCulture()
        Catch
            Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
        End Try
    End Sub

#End Region

End Class
