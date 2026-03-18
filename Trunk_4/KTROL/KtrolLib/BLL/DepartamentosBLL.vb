Imports System.Collections

Namespace BLL
    Public Class DepartamentosBLL

        Private departamentosDAL As New DAL.DepartamentosDAL

#Region "Consultas"
        ''' <summary>
        ''' Carga todas las colas de simulacion
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDepartamentos(ByVal idPlanta As Integer) As List(Of ELL.Departamentos)
            Return departamentosDAL.loadList(idPlanta)
        End Function

        ''' <summary>
        ''' Carga una cola
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDepartamento(ByVal idDepartamento As Integer) As ELL.Departamentos
            Return departamentosDAL.getDepartamento(idDepartamento)
        End Function

        ''' <summary>
        ''' Comprueba si existe un departamento
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existeDepartamento(ByVal codDepartamento As String) As Boolean
            If (departamentosDAL.existDepartamento(codDepartamento) > 0) Then
                Return True
            Else : Return False
            End If
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar un departamento nuevo
        ''' </summary>
        ''' <param name="departamento">Objeto Departamentos</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarDepartamento(ByRef departamento As ELL.Departamentos) As Boolean
            Dim resultado = departamentosDAL.SaveDepartamento(departamento)
            If (resultado <> Integer.MinValue) Then
                ' Todo ha ido OK
                Return True
            Else
                'Error en el insert
                Return False
            End If
        End Function

        ''' <summary>
        ''' Modificar los datos de un departamento
        ''' </summary>
        ''' <param name="departamento">Objeto Departamentos</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarDepartamento(ByVal departamento As ELL.Departamentos) As Boolean
            Return departamentosDAL.UpdateDepartamento(departamento)
        End Function

        ''' <summary>
        ''' Eliminar un departamento
        ''' </summary>
        ''' <param name="idDepartamento">Identificador de un departamento</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarDepartamento(ByVal idDepartamento As Integer) As Boolean
            Return departamentosDAL.DeleteDepartamento(idDepartamento)
        End Function
#End Region

    End Class

End Namespace

