

Namespace BLL

    Public Class SolicitudesBLL

        Private solicitudesDAL As New DAL.SolicitudesDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de solicitudes pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarSolicitud(ByVal idSolicitud As Integer) As ELL.Solicitudes
            Return solicitudesDAL.CargarSolicitud(idSolicitud)
        End Function

        ''' <summary>
        ''' Obtiene un listado de solicitudes pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarSolicitudesPendientes(ByVal idTipoSolicitud As Integer) As List(Of ELL.Solicitudes)
            Return solicitudesDAL.CargarSolicitudesPendientes(idTipoSolicitud)
        End Function

        ''' <summary>
        ''' Obtiene un listado de solicutdes pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarSolicitudesPendientesUsuario(ByVal idTipoSolicitud As Integer, ByVal idUsuario As Integer) As List(Of ELL.Solicitudes)
            Return solicitudesDAL.CargarSolicitudesPendientesUsuario(idTipoSolicitud, idUsuario)
        End Function

        ''' <summary>
        ''' Obtiene todo el listado de solicitudes tramitadas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarSolicitudesTramitadas(ByVal filtradoHistorial As ELL.FiltradoHistorial, ByVal tramitador As Boolean, ByVal idUsuario As Integer) As List(Of ELL.Solicitudes)
            Return solicitudesDAL.CargarSolicitudesTramitadas(filtradoHistorial, tramitador, idUsuario)
        End Function

        ''' <summary>
        ''' La solicitud tiene referencias pendientes por integrar en Brain
        ''' </summary>
        ''' <param name="idSolicitud"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TieneReferenciasPendientesBrain_old(ByVal idSolicitud As Integer) As Boolean
            If (solicitudesDAL.TieneReferenciasPendientesBrain_old(idSolicitud) > 0) Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' La solicitud tiene referencias pendientes por integrar en Brain
        ''' </summary>
        ''' <param name="idSolicitud"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TieneReferenciasPendientesBrain(ByVal idSolicitud As Integer) As Boolean
            If (solicitudesDAL.TieneReferenciasPendientesBrain(idSolicitud) > 0) Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Comprobar el número de plantas afectadas en total para una solicitud (la suma de todas las plantas por cada referencia de una solicitud)
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPlantasAfectadasSolicitud(ByVal idSolicitud As Integer) As Integer           
            Return solicitudesDAL.CargarPlantasAfectadasSolicitud(idSolicitud)
        End Function

        ''' <summary>
        ''' Cargar las solicitudes pendientes de validar por parte del project leader(si el usuario tiene otro tipo de rol mostrar todas las validaciones pendientes)
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="idRol">Identificador del rol del usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarValidacionesPendientesReferenciasVenta(ByVal idUsuario As Integer, ByVal idRol As Integer) As List(Of ELL.Solicitudes)
            Return solicitudesDAL.CargarValidacionesPendientesReferenciasVenta(idUsuario, idRol)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Validar la solicitud por parte del project leader(si el usuario tiene otro tipo de rol y tiene acceso a validar entonces lo validará un rango superior)
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidarSolicitud(ByVal idSolicitud As Integer, ByVal idUsuario As Integer) As Boolean
            Return solicitudesDAL.ValidarSolicitud(idSolicitud, idUsuario)
        End Function

        ''' <summary>
        ''' Rechazar la solicitud por parte del project leader(si el usuario tiene otro tipo de rol y tiene acceso a validar entonces lo validará un rango superior)
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="comentarioValidador">Comentario de rechazo del validador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RechazarSolicitud(ByVal idSolicitud As Integer, ByVal idUsuario As Integer, ByVal comentarioValidador As String) As Boolean
            Return solicitudesDAL.RechazarSolicitud(idSolicitud, idUsuario, comentarioValidador)
        End Function

#End Region

    End Class

End Namespace
