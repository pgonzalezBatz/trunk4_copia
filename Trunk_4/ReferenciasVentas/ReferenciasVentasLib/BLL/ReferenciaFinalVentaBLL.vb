

Namespace BLL

    Public Class ReferenciaFinalVentaBLL

        Private referenciaFinalVentaDAL As New DAL.ReferenciaFinalVentaDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene los datos de una referencia
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferencia(ByVal idRef As Integer) As ELL.ReferenciaVenta
            Return referenciaFinalVentaDAL.CargarReferencia(idRef)
        End Function

        ''' <summary>
        ''' Obtiene los datos de una referencia por la referencia de cliente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferenciaPorCPN(ByVal idCPN As Integer) As ELL.ReferenciaVenta
            Return referenciaFinalVentaDAL.CargarReferenciaPorCPN(idCPN)
        End Function

        ''' <summary>
        ''' Obtiene un listado de referencias pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferenciasCreadasSolicitud(ByVal idSolicitud As Integer) As List(Of ELL.ReferenciaVenta)
            Return referenciaFinalVentaDAL.CargarReferenciasCreadasSolicitud(idSolicitud)
        End Function

        ''' <summary>
        ''' Obtiene un listado de referencias pendientes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferenciasSolicitud(ByVal idSolicitud As Integer) As List(Of ELL.ReferenciaVenta)
            Return referenciaFinalVentaDAL.CargarReferenciasSolicitud(idSolicitud)
        End Function

        ''' <summary>
        ''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HayReferenciasCreadasSolicitud(ByVal idSolicitud As Integer) As Boolean
            Dim numReferenciasCreadasSolicitud As Integer = CargarReferenciasCreadasSolicitud(idSolicitud).Count
            If (numReferenciasCreadasSolicitud > 0) Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Obtiene un listado de plantas de una referencia
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPlantasReferencia(ByVal idRef As Integer) As List(Of ELL.ReferenciaPlantas)
            Return referenciaFinalVentaDAL.CargarPlantasReferencia(idRef)
        End Function

        ''' <summary>
        ''' Obtiene el listado de Batz Part Number antiguas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPreviousBatzPN(ByVal texto As String) As List(Of String())
            Return referenciaFinalVentaDAL.CargarPreviousBatzPN(texto)
        End Function

        ''' <summary>
        ''' Verificar que una referencia de Batz existe en la base de datos (no necesariamente tiene que haber sido integrado en Brain)
        ''' </summary>
        ''' <param name="referenciaBatz">Referencia de venta en batz</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteReferenciaBatz(ByVal referenciaBatz As String) As Boolean
            Return referenciaFinalVentaDAL.ExisteReferenciaBatz(referenciaBatz)
        End Function

        ''' <summary>
        ''' Cargar la última referencia de Batz generado para el grupo de producto seleccionado
        ''' </summary>
        ''' <param name="grupo">Grupo de producto</param>
        ''' <returns></returns>
        Public Function CargarUltimaReferenciaBatzProducto(ByVal grupo As String) As String
            Return referenciaFinalVentaDAL.CargarUltimaReferenciaBatzProducto(grupo)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un nuevo registro
        ''' </summary>
        ''' <param name="articulos">Listado de objetos CreacionArticulos</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarSolicitud(ByRef articulos As List(Of ELL.ReferenciaVenta), ByVal perfilUsuario As ELL.PerfilUsuario, ByVal idTipoSolicitud As Integer, ByVal idValidador As Integer) As Integer
            Return referenciaFinalVentaDAL.GuardarSolicitud(articulos, perfilUsuario, idTipoSolicitud, idValidador)
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="valores">Batz Part Number</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarBatzPartNumber(ByVal idSolicitud As Integer, ByVal valores As Dictionary(Of Integer, String), ByVal aprobado As Boolean, ByVal idTramitador As Integer, ByVal comentario As String) As Boolean
            Return referenciaFinalVentaDAL.GuardarBatzPartNumber(idSolicitud, valores, aprobado, idTramitador, comentario)
        End Function

        ''' <summary>
        ''' Guardar los datos tras la tramitación
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <param name="aprobado">Flag de aprobado</param>
        ''' <param name="idTramitador">Identificador del tramitador</param>
        ''' <param name="comentarioDT">Comentario sobre la tramitación</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarTramitacion(ByVal idSolicitud As Integer, ByVal aprobado As Boolean, ByVal idTramitador As Integer, ByVal comentarioDT As String) As Boolean
            Return referenciaFinalVentaDAL.GuardarTramitacion(idSolicitud, aprobado, idTramitador, comentarioDT)
        End Function

        ''' <summary>
        ''' Modifica el flag de inserción en Brain
        ''' </summary>
        ''' <param name="idReferencia">Identificador de la referencia</param>
        ''' <param name="insercionBrain">Flag de inserción o eliminación</param>
        ''' <param name="BatzPartNumber">Referencia de Pieza de Batz</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsercionBrainReferenciaVenta(ByVal idReferencia As Integer, ByVal insercionBrain As Boolean, ByVal BatzPartNumber As String, ByVal envioEmail As Boolean, ByVal tipoInsercion As Integer, Optional ByVal producto As String = "", Optional ByVal integrado As Boolean = False) As Boolean
            Return referenciaFinalVentaDAL.InsercionBrainReferenciaVenta(idReferencia, insercionBrain, BatzPartNumber, envioEmail, tipoInsercion, producto, integrado)
        End Function

#End Region

    End Class

End Namespace
