Imports System.Configuration
Imports System.Net
Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PKSBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerPuesto(ByVal idSab As Integer) As ELL.Puesto
            Dim url As String = String.Format("{0}{1}", ConfigurationManager.AppSettings("urlDatosPKS"), idSab)
            Dim ret As ELL.Puesto
            Using wc As New WebClient()
                wc.UseDefaultCredentials = True
                Dim jss As New JavaScriptSerializer()
                wc.Encoding = System.Text.Encoding.UTF8
                ret = jss.Deserialize(Of ELL.Puesto)(wc.DownloadString(url))
                ret.IdSab = idSab
            End Using

            Return ret
        End Function

#End Region

    End Class

End Namespace