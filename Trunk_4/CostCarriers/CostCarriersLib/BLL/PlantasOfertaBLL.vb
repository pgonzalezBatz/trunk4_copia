Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantasOfertaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene proyectos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerPlantasOferta(ByVal idProyecto As String, ByVal idOferta As Integer) As List(Of ELL.PlantaOferta)
            Dim jss As New JavaScriptSerializer()
            Dim ret As New List(Of ELL.PlantaOferta)
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                ret = jss.Deserialize(Of List(Of ELL.PlantaOferta))(cliente.GetProjectFactories(idProyecto, idOferta))
            End Using

            Return ret
        End Function

        ''' <summary>
        ''' Obtiene la moneda de la planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerMonedaPlanta(ByVal idPlanta) As Integer
            Dim idMoneda As Integer = Integer.MinValue
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                idMoneda = cliente.GetFactoryCurrencyOffer(idPlanta)
            End Using

            Return idMoneda
        End Function

#End Region

    End Class

End Namespace