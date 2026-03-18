Imports System.Configuration
Imports System.Net
Imports System.Web.Script.Serialization

Namespace BLL

    Public Class ProveedoresBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProveedor"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal idProveedor As Integer) As ELL.proveedor
            ' La url para obtener el json es https://intranet2.batz.es/gip/proveedor/jsonobtenerdatosproveedor?id=5
            System.Net.ServicePointManager.Expect100Continue = False
            System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType)
            Dim url As String = ConfigurationManager.AppSettings("urlDatosProveedor") & idProveedor
            Dim json As String = String.Empty
            Using wc As New WebClient()
                wc.UseDefaultCredentials = True
                json = wc.DownloadString(url)
            End Using

            Dim jss As New JavaScriptSerializer()

            Dim proveedor As ELL.proveedor = jss.Deserialize(Of ELL.proveedor)(json)
            Return proveedor
        End Function

#End Region

    End Class

End Namespace