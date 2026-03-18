Public Class MPCR
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private itzultzaileWeb As New TraduccionesLib.itzultzaile

    Protected ReadOnly Property Servidor As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "intranet-test", "intranet2")
        End Get
    End Property

    Public ReadOnly Property Cx As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", ConfigurationManager.ConnectionStrings("BI_CR_Igorre_DESA").ConnectionString, ConfigurationManager.ConnectionStrings("BI_CR_Igorre").ConnectionString)
        End Get
    End Property

    Public ReadOnly Property Cx_SAB As String
        Get
            Return If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", ConfigurationManager.ConnectionStrings("SABTEST").ConnectionString, ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString)
        End Get
    End Property

    Public Function Traducir(ByVal termino As String)
        Return itzultzaileWeb.Itzuli(termino)
    End Function

    Public WriteOnly Property SetTitle As String
        Set(ByVal value As String)
            lblTitulo.Text = itzultzaileWeb.Itzuli(value)
        End Set
    End Property

    Public Sub Title(ByVal titulo As String)
        lblTitulo.Text = titulo
    End Sub
End Class