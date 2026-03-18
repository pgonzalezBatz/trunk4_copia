Imports System.Collections.Generic

Namespace BLL.Interface
    Public Interface IEstadisticasComponent

        ''' <summary>
        ''' Guarda un acceso a una aplicacion
        ''' </summary>
        ''' <param name="nombreUsuario">Nombre del usuario del acceso</param>
        ''' <param name="numeroTrabajador">Numero del trabajador</param>
        ''' <param name="aplicacion">Aplicacion a la que ha entrado</param>
        ''' <param name="ip">Direccion IP</param>
        Function GuardarAcceso(ByVal nombreUsuario As String, ByVal numeroTrabajador As Integer, ByVal aplicacion As String, ByVal ip As String) As Boolean

        Function AccesosARecurso(ByVal nombreRecurso As String, ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime) As Integer
        Function AccesosARecursoPorUsuario(ByVal nombreRecurso As String, ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime) As DataTable
        Function AccesosARecursoPorUsuarioSQL(ByVal nombreRecurso As String, ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime) As List(Of String())
        Function EstadisticasMesuales(ByVal year As Integer, ByVal nombreRecurso As String) As List(Of Integer)

    End Interface
End Namespace