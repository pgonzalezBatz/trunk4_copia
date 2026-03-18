Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ProyectosBLL

#Region "Consultas"
        ''' <summary>
        ''' Consulta el owner y coowner
        ''' </summary>
        ''' <param name="idProyecto"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerOwner(ByVal idProyecto As String) As Object
            Dim jss As New JavaScriptSerializer()

            Using cliente As New ServicioBonos.ServicioBonos
                Return jss.DeserializeObject(cliente.GetOwner(idProyecto))
            End Using
        End Function


        ''' <summary>
        ''' Consulta si para un proyecto un usuario es owner o coowner
        ''' </summary>
        ''' <param name="idProyecto"></param>
        ''' <param name="propietario"></param>
        ''' <returns></returns>
        Public Shared Function EsOwner(ByVal idProyecto As String, ByVal propietario As String) As Boolean
            Dim jss As New JavaScriptSerializer()
            Dim ret As Boolean = False
            Dim resultado = ObtenerOwner(idProyecto)

            If (resultado IsNot Nothing AndAlso (resultado("Owner").ToLower() = propietario.ToLower() OrElse resultado("Coowner").ToLower() = propietario.ToLower())) Then
                ret = True
            End If

            Return ret
        End Function

        ''' <summary>
        ''' Obtiene proyectos
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <param name="idTipoProyecto"></param>
        ''' <param name="propietario"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal producto As String, ByVal idTipoProyecto As Integer, ByVal propietario As String) As List(Of ELL.Proyecto)
            'Dim tipoProyecto As String = BLL.TiposProyectoBLL.Obtener(idTipoProyecto).Descripcion

            Dim jss As New JavaScriptSerializer()
            Dim ret As New List(Of ELL.Proyecto)
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                ret = jss.Deserialize(Of List(Of ELL.Proyecto))(cliente.GetProjects(producto, idTipoProyecto, propietario))
            End Using

            Return ret
        End Function

#End Region

    End Class

End Namespace