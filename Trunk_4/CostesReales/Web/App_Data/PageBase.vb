Public Class PageBase
    Inherits Page

#Region "Variables compartidas"

    Public Shared STICKET As String = "Ticket"
    Public Shared SCULTURA As String = "micultura"
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    Public Shared PAG_OBRAS As String = "~/Obras.html"
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("CostesReales")
    Public Shared PAG_PERIODOCIERRE As String = "~/PeriodoCierre.aspx"

#End Region

#Region "Ticket"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property Ticket() As SabLib.ELL.Ticket
        Get
            Return If(Session(STICKET) Is Nothing, Nothing, CType(Session(STICKET), SabLib.ELL.Ticket))
        End Get
        Set(ByVal value As SabLib.ELL.Ticket)
            Session(STICKET) = value
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
        Try
            If (Session(STICKET) Is Nothing) Then
                If (Request.QueryString("id") Is Nothing) Then 'Si viene la clave id en el queryString, significara que ya ha sido enviada a la pagina de permiso denegado
                    Dim lg As New SabLib.BLL.LoginComponent
                    Ticket = lg.Login(User.Identity.Name.ToLower)
#If DEBUG Then
                    'Ticket = lg.Login("batznt\jsagarna")
                    'Ticket = lg.Login("mblusitana\opresno")
                    Ticket = lg.Login("batznt\inycom2")
#End If

                    Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.CreateSpecificCulture(Ticket.Culture)
                    If (Ticket Is Nothing) Then
                        log.Warn(User.Identity.Name & " no tiene acceso a la intranet")
                        Response.Redirect(PAG_PERMISODENEGADO)
                        'Else
                        '    Response.Redirect("default.aspx", False)
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Redirect(PAG_PERMISODENEGADO)
        End Try
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
    ''' Inicializa la cultura
    ''' </summary>
    Protected Overrides Sub InitializeCulture()
        Try
            If Session(SCULTURA) Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Session(SCULTURA) = cultureInfo.Name
                Culture = cultureInfo.Name
            Else
                Culture = Session(SCULTURA)
            End If
            MyBase.InitializeCulture()
        Catch ex As Exception
            log.Error("Error en el InitializeCulture de Pagebase", ex)
        End Try
    End Sub

#End Region

End Class

