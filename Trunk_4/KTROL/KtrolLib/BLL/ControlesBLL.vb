Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class ControlesBLL

        Private controlesDAL As New DAL.ControlesDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene los datos de un control
        ''' </summary>
        ''' <param name="idControl">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerControl(ByVal idControl As Integer) As ELL.Controles
            Return controlesDAL.ObtenerControl(idControl)
        End Function

        ''' <summary>
        ''' Obtiene los valores de los controles por usuario
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerControlValores(ByVal idControl As Integer) As List(Of ELL.ControlesValoresResumen)
            Return controlesDAL.ObtenerControlValores(idControl)
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerControlErrores(ByVal idControl As Integer) As ELL.ControlesErrores
            Return controlesDAL.ObtenerControlErrores(idControl)
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ControlSinErrores(ByVal idControl As Integer) As Boolean
            Return controlesDAL.ControlSinErrores(idControl)
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CaracteristicasSinErrores(ByVal idControl As Integer) As Boolean
            Return controlesDAL.CaracteristicasSinErrores(idControl)
        End Function

        ''' <summary>
        ''' Obtiene el último control realizado para un código de operación
        ''' </summary>
        ''' <param name="codOperacion">Código de operación</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function UltimoControlConErrores(ByVal codOperacion As String) As Boolean
            Dim ultimoControl As Integer = controlesDAL.ObtenerUltimoControlCodigoOperacion(codOperacion)
            Dim ultimoControlError As ELL.ControlesErrores = controlesDAL.ObtenerUltimoControlCodigoOperacionErrores(codOperacion)

            If (ultimoControl > 0 AndAlso ultimoControlError IsNot Nothing) Then
                If (ultimoControlError.Reparado) Then
                    If (ultimoControl = ultimoControlError.IdControl) Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda control nuevo con los valores
        ''' </summary>
        ''' <param name="control"></param>
        ''' <param name="controlValores"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarControlYValores(ByVal control As ELL.Controles, ByVal controlValores As List(Of ELL.ControlesValores), ByVal controlErrores As ELL.ControlesErrores) As Integer
            Return controlesDAL.GuardarControlYValores(control, controlValores, controlErrores)
        End Function

        ''' <summary>
        ''' Modificar el valor de un registro de un control
        ''' </summary>
        ''' <param name="control">Objeto ControlesValoresResumen</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarValorControl(ByVal control As ELL.ControlesValoresResumen) As Boolean
            Return controlesDAL.ModificarValorControl(control)
        End Function

        ''' <summary>
        ''' Modificar el valor de un registro de un control y guardar el error de este control
        ''' </summary>
        ''' <param name="controlError">Objeto ControlesErrores</param>
        ''' <param name="controlValor">Objeto ControlesValoresResumen</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarErrorYValorControl(ByVal controlError As ELL.ControlesErrores, ByVal controlValor As ELL.ControlesValoresResumen) As Boolean
            Return controlesDAL.ModificarErrorYValorControl(controlError, controlValor)
        End Function

        ''' <summary>
        ''' Modificar los datos de error de un control
        ''' </summary>
        ''' <param name="controlError">Objeto ControlesErrores</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarError(ByVal controlError As ELL.ControlesErrores) As Boolean
            Return controlesDAL.ModificarError(controlError)
        End Function

        ''' <summary>
        ''' Elimina el error de de un control
        ''' </summary>
        ''' <param name="idControl">Identificador del control</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarControlError(ByVal idControl As Integer) As Boolean
            Return controlesDAL.EliminarControlError(idControl)
        End Function

        ''' <summary>
        ''' Eliminar todos los datos de un control
        ''' </summary>
        ''' <param name="idControl">Identificador del control</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarControl(ByVal idControl As Integer) As Boolean
            Return controlesDAL.EliminarControl(idControl)
        End Function

#End Region
	End Class

End Namespace
