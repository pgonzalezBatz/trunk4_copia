Imports System.Globalization.CultureInfo

Public Class PageBase2
    Inherits Page

#Region "Variables compartidas"
    Public Shared PAG_INICIO As String = "~/Index.aspx"   '"~/CrearDocumento.aspx" 
    Public Shared PAG_INICIO_ADMINISTRADOR As String = "~/Paginas/Solicitudes/TramitarSolicitudes.aspx"
    Public Shared PAG_INICIO_PRODUCT_ENGINEER As String = "~/Paginas/Solicitudes/ReferenciaFinalVenta.aspx"
    Public Shared PAG_INICIO_DOCUMENTATION_TECHNICIAN As String = "~/Paginas/Solicitudes/TramitarSolicitudes.aspx"
    Public Shared PAG_INICIO_VALIDATIONS As String = "~/Paginas/Solicitudes/Validaciones.aspx"
    Public Shared PAG_PERMISODENEGADO As String = "~/PermisoDenegado.aspx"
    '  Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.SolicitudCR")
    Public Shared plantaAdmin As Int32 = 999
    Public Shared rolUsuario As Integer = 0
    Public Shared codigoTra As String = ""
    Public Shared categoria As String = ""
    Public Shared comentario As String = ""
    'Public Shared empresaBusqueda As Int32 = 999
    'Public Shared plantaAdminNombre As String = ""
    'Public Shared DirFicherosBajar As String = ""
    'Public Shared DirFicherosSubir As String = ""
    ' Public ItzultzaileWeb As New LocalizationLib2.Itzultzaile
#End Region

    ''' <summary>
    ''' Constructor vacio
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Inicializa la cultura
    ''' </summary>
    Public Sub inicializarCultura()
        MyBase.InitializeCulture()
    End Sub

    ''' <summary>
    ''' Se inicializa la cultura
    ''' </summary>
    Protected Overrides Sub InitializeCulture()
        Try
            If Session("miCultura") Is Nothing Then
                Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                Session("miCultura") = cultureInfo.Name
                Culture = cultureInfo.Name
            Else
                Culture = Session("miCultura")
            End If
            Dim myTicket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
            myTicket.Culture = CurrentCulture.Name
            MyBase.InitializeCulture()
        Catch
        End Try
    End Sub


#Region "Traducciones"
    ''' <summary>
    ''' En este evento se encuentran cargados todos los controles de la página y podemos usar el traductor "Itzultzaile".
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    'Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
    '    'ItzultzaileWeb.TraducirObjetos(Page.Controls)
    '    ItzultzaileWeb.TraducirWebControls(Page.Controls)
    'End Sub
#End Region

End Class
