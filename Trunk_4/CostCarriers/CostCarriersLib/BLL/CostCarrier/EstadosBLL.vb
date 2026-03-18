Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene estado
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.Estado
            Return DAL.EstadosDAL.getObject(id)
        End Function

        ''' <summary>
        ''' Obtiene estados
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="datosPresupuesto"></param>
        ''' <param name="datosPresupViajes"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idPlanta As Integer, ByVal datosPresupuesto As List(Of ELL.DatosDistribucion), ByVal datosPresupViajes As List(Of ELL.DatosDistribucion), ByVal datosPresupAnyos As List(Of ELL.DatosDistribucionAnyos)) As List(Of ELL.Estado)
            Dim estados As List(Of ELL.Estado) = DAL.EstadosDAL.loadList(idPlanta)

            For Each estado In estados
                estado._datosPresupuesto = datosPresupuesto.Where(Function(f) f.Estado = estado.Estado).ToList()
                estado._datosPresupViajes = datosPresupViajes.Where(Function(f) f.Estado = estado.Estado).ToList()
                estado._datosPresupAnyos = datosPresupAnyos.Where(Function(f) f.Estado = estado.Estado).ToList()
            Next

            Return estados
        End Function

#End Region

    End Class

End Namespace