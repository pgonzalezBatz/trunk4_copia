Imports System.Collections

Namespace BLL
    Public Class OperacionKaplanPrismaBLL

        Private operacionKaplanPrismaDAL As New DAL.OperacionKaplanPrismaDAL

#Region "Consultas"
        ''' <summary>
        ''' Carga todas las relaciones
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarOperacionesKaplanPrisma() As List(Of ELL.OperacionKaplanPrisma)
            Return operacionKaplanPrismaDAL.loadList()
        End Function

        ''' <summary>
        ''' Carga una relación
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarOperacionKaplanPrisma(ByVal id As Integer) As ELL.OperacionKaplanPrisma
            Return operacionKaplanPrismaDAL.getOperacionKaplanPrisma(id)
        End Function

        ''' <summary>
        ''' Carga una relación
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarOperacionKaplanPrismaPorCodigoKaplan(ByVal codOperacion As Integer) As ELL.OperacionKaplanPrisma
            Return operacionKaplanPrismaDAL.getOperacionKaplanPrismaPorCodigoKaplan(codOperacion)
        End Function

        ''' <summary>
        ''' Carga una relación
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarOperacionKaplanPrismaPorCodigoOperacion(ByVal codOperacion As Integer) As List(Of ELL.OperacionKaplanPrisma)
            Return operacionKaplanPrismaDAL.getOperacionKaplanPrismaPorCodigoOperacion(codOperacion)
        End Function
#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar una relación nueva
        ''' </summary>
        ''' <param name="operacionKaplanPrisma">Objeto OperacionKaplanPrisma</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarOperacionKaplanPrisma(ByRef operacionKaplanPrisma As ELL.OperacionKaplanPrisma) As Boolean
            Return operacionKaplanPrismaDAL.SaveOperacionKaplanPrisma(operacionKaplanPrisma)
        End Function

        ''' <summary>
        ''' Eliminar una relación
        ''' </summary>
        ''' <param name="id">Identificador de la relación</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarOperacionKaplanPrisma(ByVal id As Integer) As Boolean
            Return operacionKaplanPrismaDAL.DeleteOperacionKaplanPrisma(id)
        End Function
#End Region

#Region "PRISMA"

        ''' <summary>
        ''' Obtiene el numero de solicitud de la incidencia ábierta en PRISMA
        ''' </summary>
        ''' <param name="workRequestType">Tipo de Request</param>
        ''' <param name="company">Compañia</param>
        ''' <returns>Numero de trabajador encontrado</returns>       
        Public Function GetIdSolicitudPrisma(ByVal workRequestType As String, ByVal company As String) As String
            Return operacionKaplanPrismaDAL.GetIdSolicitudPrisma(workRequestType, company)
        End Function

        ''' <summary>
        ''' Guarda el identificador de prisma
        ''' </summary>
        ''' <param name="idSolicitudPrisma">Identificador de solicitud de prisma</param>
        ''' <param name="idControl">Identificador del control</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarIdSolicitudPrisma(ByVal idSolicitudPrisma As Integer, ByVal idControl As Integer) As Integer
            Return operacionKaplanPrismaDAL.GuardarIdSolicitudPrisma(idSolicitudPrisma, idControl)
        End Function

        ''' <summary>
        ''' Obtiene el numero de trabajador de la tabla de prisma
        ''' </summary>
        ''' <param name="numTra">Numero de trabajador</param>
        ''' <param name="company">Compañia</param>
        ''' <returns>Numero de trabajador encontrado</returns>        
        Public Function GetNumTrabajador(ByVal numTra As Integer, ByVal company As String) As String
            Return operacionKaplanPrismaDAL.GetNumTrabajador(numTra, company)
        End Function

#End Region

    End Class

End Namespace

