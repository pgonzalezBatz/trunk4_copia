Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class MiembrosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene estados
        ''' </summary>
        ''' <param name="idProyecto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarLocalProjectLeads(ByVal idProyecto As String) As List(Of ELL.Miembro)
            Dim jss As New JavaScriptSerializer()
            Dim miembros As New List(Of ELL.Miembro)

            Using cliente As New ServicioBonos.ServicioBonos
                miembros = jss.Deserialize(Of List(Of ELL.Miembro))(cliente.GetProjectMembersByRol(idProyecto, "Local Project Lead"))
            End Using

            Return miembros
        End Function

#End Region

    End Class

End Namespace