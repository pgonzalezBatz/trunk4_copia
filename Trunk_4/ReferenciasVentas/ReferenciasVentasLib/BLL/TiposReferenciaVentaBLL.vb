Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class TiposReferenciaVentaBLL

        Private drivingHandDAL As New DAL.TiposReferenciaDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista() As List(Of ELL.TiposReferenciaVenta)
            Return drivingHandDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene un DrivingHand
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTipoReferenciaVenta(ByVal id As Integer) As ELL.TiposReferenciaVenta
            Return drivingHandDAL.CargarTipoReferenciaVenta(id)
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Existe(ByVal nombre As String) As Boolean
            If (drivingHandDAL.existe(nombre) > 0) Then
                Return True
            Else : Return False
            End If
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un nuevo registro
        ''' </summary>
        ''' <param name="tipoReferenciaVenta">Objeto TiposReferenciaVenta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarTipoReferenciaVenta(ByVal tipoReferenciaVenta As ELL.TiposReferenciaVenta) As Boolean
            Return drivingHandDAL.Save(tipoReferenciaVenta)
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="tipoReferenciaVenta">Objeto TiposReferenciaVenta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarTipoReferenciaVenta(ByVal tipoReferenciaVenta As ELL.TiposReferenciaVenta) As Boolean
            Return drivingHandDAL.Update(tipoReferenciaVenta)
        End Function

        ''' <summary>
        ''' Elimina un registro
        ''' </summary>
        ''' <param name="idTipoReferenciaVenta">Identificador del Tipo de Referencia de Venta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarTipoReferenciaVenta(ByVal idTipoReferenciaVenta As Integer) As Boolean
            Return drivingHandDAL.Delete(idTipoReferenciaVenta)
        End Function

#End Region

    End Class

End Namespace
