Imports System.Collections.Generic

Namespace BLL.Interface
    Public Interface IDepartamentosComponent

        Enum EDepartamentos As Integer
            Todos = 0
            Activos = 1
            Inactivos = 2
        End Enum


        ''' <summary>
        ''' Devuelve la informacion del departamento
        ''' </summary>
        ''' <param name="oDepto">Departamento</param>
        ''' <returns>Objeto departamento</returns>
        ''' <remarks></remarks>
        Function GetDepartamento(ByVal oDepto As ELL.Departamento) As ELL.Departamento

        ''' <summary>
        ''' Devuelve los departamentos que cumplan las caracteristicas
        ''' </summary>
        ''' <param name="oDepartamento">Departamento a consultar</param>
        ''' <param name="sortField">Parametro opcional para ordenar por un campo</param>
        ''' <returns>Lista de departamentos</returns>        
        ''' <remarks></remarks>
        Function GetDepartamentos(ByVal oDepartamento As ELL.Departamento, Optional ByVal sortField As String = DAL.DEPARTAMENTOS.ColumnNames.ID) As System.Collections.Generic.List(Of ELL.Departamento)


        ''' <summary>
        ''' Obtiene todos los departamentos de epsilon o de sab
        ''' </summary>
        ''' <param name="eDepart">Se indicara que departamentos se obtendran</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getDepartamentos(ByVal eDepart As EDepartamentos, ByVal idPlanta As Integer) As List(Of ELL.Departamento)

        ''' <summary>
        ''' Devuelve los departamentos existentes en una planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns>Lista de departamentos</returns>        
        ''' <remarks></remarks>
        Function GetDepartamentosPlanta(ByVal idPlanta As Integer) As System.Collections.Generic.List(Of ELL.Departamento)

        ''' <summary>
        ''' Elimina un departamento
        ''' </summary>
        ''' <param name="oDepto">Objeto de departamento</param>
        Function Save(ByVal oDepto As ELL.Departamento, ByVal bNuevo As Boolean) As Boolean

        ''' <summary>
        ''' Elimina un departamento
        ''' </summary>
        ''' <param name="idDepto">Identificador del departamento</param>
        Function Delete(ByVal idDepto As Integer) As Boolean

        ''' <summary>
        ''' Obtiene el maximo id de la tabla de departamentos para esa planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        Function GenerarIdDepto(ByVal idPlanta As Integer) As Integer


    End Interface
End Namespace