
Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ProjectsAffectedBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProyecto"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerProyectosAfectados(ByVal idProyecto As String) As List(Of ELL.ProjectAffected)

            Dim jss As New JavaScriptSerializer()
            Dim ret As New List(Of ELL.ProjectAffected)
            Using cliente As New ServicioBonos.ServicioBonos
                ret = jss.Deserialize(Of List(Of ELL.ProjectAffected))(cliente.GetProjectsAffected(idProyecto))
            End Using

            Return ret
        End Function

#End Region

    End Class

End Namespace