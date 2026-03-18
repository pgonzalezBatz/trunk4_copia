Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class TypeBLL

        Private typeDAL As New DAL.TypeDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de tipos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista() As List(Of ELL.Type)
            Return typeDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene el listado de tipos activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposActivos() As List(Of ELL.Type)
            Return typeDAL.CargarTiposActivos()
        End Function

        ''' <summary>
        ''' Obtiene el listado de tipos activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposProducto(ByVal idProducto As Integer) As List(Of ELL.Type)
            Return typeDAL.CargarTiposProducto(idProducto)
        End Function

        ''' <summary>
        ''' Obtiene un tipo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTipo(ByVal id As Integer) As ELL.Type
            Return typeDAL.CargarTipo(id)
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Existe(ByVal nombre As String) As Boolean
            If (typeDAL.existe(nombre) > 0) Then
                Return True
            Else : Return False
            End If
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un nuevo registro
        ''' </summary>
        ''' <param name="tipo">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarTipo(ByVal tipo As ELL.Type) As Boolean
            Return typeDAL.Save(tipo)
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="tipo">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarTipo(ByVal tipo As ELL.Type) As Boolean
            Return typeDAL.Update(tipo)
        End Function

#End Region

    End Class

End Namespace
