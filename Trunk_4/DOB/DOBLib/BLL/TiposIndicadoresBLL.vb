Imports DOBLib.DAL

Namespace BLL

    Public Class TiposIndicadoresBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de tipos de indicadores
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.TipoIndicador)
            Return TiposIndicadoresDAL.loadList()
        End Function

        ''' <summary>
        ''' Comprueba si existe un tipo indicador
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExisteTipoIndicador(ByVal nombre As String) As Boolean
            Return TiposIndicadoresDAL.existsTipoIndicador(nombre)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Agrega un tipo indicador
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <param name="descripcion"></param>
        Public Shared Sub AgregarTipoIndicador(ByVal nombre As String, ByVal descripcion As String)
            TiposIndicadoresDAL.AddTipoIndicador(nombre, descripcion)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            TiposIndicadoresDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace