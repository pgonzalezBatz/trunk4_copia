Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ProductosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene productos
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <param name="propietario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idTipoProyecto As Integer, ByVal propietario As String) As List(Of ELL.Producto)
            Dim tipoProyecto As String = BLL.TiposProyectoBLL.Obtener(idTipoProyecto).Descripcion

            Dim jss As New JavaScriptSerializer()
            Dim ret As New List(Of ELL.Producto)
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                ret = jss.Deserialize(Of List(Of ELL.Producto))(cliente.GetProducts(tipoProyecto, propietario))
            End Using

            Return ret
        End Function

#End Region

    End Class

End Namespace