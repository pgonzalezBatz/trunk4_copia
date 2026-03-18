Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class HistoricosBLL

        Private historicosDAL As New DAL.HistoricosDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene los controles de un día
        ''' </summary>
        ''' <param name="codOp">Código de operación</param>
        ''' <param name="carac">Característica</param>
        ''' <param name="fechaDesde">Fecha de inicio del control</param>
        ''' <param name="fechaHasta">Fecha fin del control</param>
        ''' <param name="verificadores">Lista de verificadores</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerControles(ByVal codOp As String, ByVal carac As String, ByVal fechaDesde As Date, ByVal fechaHasta As Date, ByVal verificadores As List(Of Integer)) As List(Of ELL.Historicos)
            Return historicosDAL.ObtenerControles(codOp, carac, fechaDesde, fechaHasta, verificadores)
        End Function

        ''' <summary>
        ''' Obtiene los controles de un día
        ''' </summary>
        ''' <param name="codOp">Código de operación</param>
        ''' <param name="carac">Característica</param>
        ''' <param name="fechaDesde">Fecha de inicio del control</param>
        ''' <param name="fechaHasta">Fecha fin del control</param>
        ''' <param name="verificadores">Lista de verificadores</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerControlesPantalla(ByVal codOp As String, ByVal carac As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal verificadores As List(Of Integer), ByVal idControl As String) As List(Of ELL.Historicos)
            Return historicosDAL.ObtenerControlesPantalla(codOp, carac, fechaDesde, fechaHasta, verificadores, idControl)
        End Function

#End Region

    End Class

End Namespace
