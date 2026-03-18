Imports System.Collections

Namespace BLL

    Public Class ConsultasBLL

        Private consultasDAL As New DAL.ConsultasDAL

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codOpe"></param>
        ''' <returns></returns>
        Public Function consultarCodigoOperacion(ByVal codOpe As String) As String()
            Return consultasDAL.consultarCodigoOperacion(codOpe)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codOpe"></param>
        ''' <returns></returns>
        Public Function cargarDatosCodigosOperacionFabricacionPorRol(ByVal codOpe As String, ByVal idRol As Integer, Optional ByVal idSubRol As Integer = 0) As List(Of ELL.Caracteristicas_Plan_Fabricacion)
            Return consultasDAL.cargarDatosCodigosOperacionFabricacionPorRol(codOpe, idRol, idSubRol)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idRegistro"></param>
        ''' <returns></returns>
        Public Function cargarAyudaVisual(ByVal idRegistro As Integer) As ELL.AyudaVisual
            Return consultasDAL.cargarAyudaVisual(idRegistro)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codOpe"></param>
        ''' <returns></returns>
        Public Function cargarNivelPlanFabricacion(ByVal codOpe As String) As String
            Return consultasDAL.cargarNivelPlanFabricacion(codOpe)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codOpe"></param>
        ''' <returns></returns>
        Public Function cargarHojaInstruccion(ByVal codOpe As String) As String
            Return consultasDAL.cargarHojaInstruccion(codOpe)
        End Function

    End Class

End Namespace
