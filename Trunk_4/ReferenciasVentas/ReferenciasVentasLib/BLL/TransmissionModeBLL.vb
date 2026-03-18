'Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class TransmissionModeBLL

        Private transmissionModeDAL As New DAL.TransmissionModeDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista() As List(Of ELL.TransmissionMode)
            Return transmissionModeDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene el listado de TransmissionMode activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTransmissionModeActivos() As List(Of ELL.TransmissionMode)
            Return transmissionModeDAL.CargarTransmissionModeActivos()
        End Function

        ''' <summary>
        ''' Obtiene un TransmissionMode
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTransmissionMode(ByVal id As Integer) As ELL.TransmissionMode
            Return transmissionModeDAL.CargarTransmissionMode(id)
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Existe(ByVal nombre As String) As Boolean
            If (transmissionModeDAL.existe(nombre) > 0) Then
                Return True
            Else : Return False
            End If
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un nuevo registro
        ''' </summary>
        ''' <param name="transmissionMode">Objeto TransmissionMode</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarTransmissionMode(ByVal transmissionMode As ELL.TransmissionMode) As Boolean
            Return transmissionModeDAL.Save(transmissionMode)
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="transmissionMode">Objeto TransmissionMode</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarTransmissionMode(ByVal transmissionMode As ELL.TransmissionMode) As Boolean
            Return transmissionModeDAL.Update(transmissionMode)
        End Function

        ' ''' <summary>
        ' ''' Elimina un registro
        ' ''' </summary>
        ' ''' <param name="idTransmissionMode">Identificador del TransmissionMode</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function EliminarTransmissionMode(ByVal idTransmissionMode As Integer) As Boolean
        '    Return transmissionModeDAL.Delete(idTransmissionMode)
        'End Function

#End Region

    End Class

End Namespace
