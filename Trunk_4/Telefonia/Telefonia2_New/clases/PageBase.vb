Imports System.Globalization.CultureInfo

Public Class PageBase
    Inherits Page

#Region "Variables compartidas"

    Public Shared STICKET As String = "Ticket"
    Public Shared SCULTURA As String = "MiCultura"
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared PAG_REFRESCARSESSION As String = "~/RefrescarSession.aspx"
    Public Shared PAG_ADMINISTRACION As String = "~/Mantenimientos/DefaultAdm.aspx"
    Public Shared itzultzaileWeb As New itzultzaile
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Telefonia")

#End Region

#Region "Ticket"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Private Property Ticket() As ELL.TicketTlfno
        Get
            If (Session(STICKET) Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session(STICKET), ELL.TicketTlfno)
            End If
        End Get
        Set(ByVal value As ELL.TicketTlfno)
            Session(STICKET) = value
        End Set
    End Property

    ''' <summary>
    ''' Configura el nuevo objeto del ticket y comprueba si es administrador
    ''' </summary>
    ''' <param name="myTicket">Ticket</param>
    ''' <returns></returns>    
    Public Function configurarTicket(ByVal myTicket As SabLib.ELL.Ticket) As Boolean
        Try
            Dim userComp As New BLL.UsuariosComponent
            Dim myTicketT As New ELL.TicketTlfno With {.Culture = myTicket.Culture, .IdDepartamento = myTicket.IdDepartamento, .IdEmpresa = myTicket.IdEmpresa,
                .IdSession = myTicket.IdSession, .NombrePersona = myTicket.NombrePersona, .Apellido1 = myTicket.Apellido1, .Apellido2 = myTicket.Apellido2,
                .IdTrabajador = myTicket.IdTrabajador, .IdUser = myTicket.IdUser, .NombreUsuario = myTicket.NombreUsuario, .Plantas = myTicket.Plantas,
                .email = myTicket.email, .IdPlantaActual = myTicket.IdPlanta}
            '1º Se comprueba si es administrador de la aplicacion
            Dim lPlantas As List(Of SabLib.ELL.Planta) = userComp.getPlantasAdministrador(myTicket.IdUser, False)
            If (lPlantas IsNot Nothing AndAlso lPlantas.Count > 0) Then myTicketT.EsAdministradorPlanta = True
            Session(PageBase.SCULTURA) = myTicketT.Culture
            Session(PageBase.STICKET) = myTicketT
            Return True
        Catch
            Return False
        End Try
    End Function

#End Region

#Region "Page Load"

    ''' <summary>
    ''' <para>Si no tiene ticket, se redirecciona a una pagina para refrescar la session</para>
    ''' <para>Comprueba que esa pagina es accesible por el perfil</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Session(STICKET) Is Nothing) Then
            Response.Redirect(PAG_REFRESCARSESSION)
        Else
            'hay que comprobar que siempre que se cargue una pagina que este en la carpeta mantenimiento, solo puedan acceder administradores y adm de plantas
            'y luego entre estos, capar algunas paginas            
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            'Si proviene del portal del empleado, solo podra acceder a la pagina de FacturacionPersona.aspx
            If (Ticket.ProvienePortalEmpleado) Then
                If (Not pag = "FacturacionPersona.aspx") Then
                    Response.Redirect(PAG_PERMISODENEGADO)
                End If
            Else
                If (segmentos.Count > 2) Then
                    If (segmentos(1) = "Mantenimientos/") Then
                        If Not (Ticket.EsAdministradorPlanta) Then
                            Response.Redirect(PAG_PERMISODENEGADO)
                        End If
                    End If
                End If
            End If
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


    Protected Overrides Sub InitializeCulture()
        Try
            If Session(SCULTURA) Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
                Session(SCULTURA) = cultureInfo.Name
                Culture = cultureInfo.Name
            Else
                Culture = Session(SCULTURA)
            End If
            Ticket.Culture = CurrentCulture.Name
            MyBase.InitializeCulture()
        Catch
            Response.Redirect(PAG_PERMISODENEGADO, False)
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

End Class
