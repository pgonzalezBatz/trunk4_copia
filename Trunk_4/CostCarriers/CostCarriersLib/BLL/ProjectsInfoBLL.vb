Imports System.Web.Script.Serialization

Namespace BLL
    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ProjectsInfoBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene las plantas a cargar con el IdBrain
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerProjectInfo(ByVal idProyecto As String) As ELL.ProjectInfo
            Dim jss As New JavaScriptSerializer()
            Dim proyecto As ELL.ProjectInfo
            Using cliente As New ServicioBonos.ServicioBonos
                proyecto = jss.Deserialize(Of ELL.ProjectInfo)(cliente.GetProjectInfo(idProyecto))
            End Using

            Return proyecto
        End Function

#End Region

    End Class

End Namespace
