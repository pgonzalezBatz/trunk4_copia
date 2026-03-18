Imports DOBLib.DAL

Namespace BLL

    Public Class NegociosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un negocio
        ''' </summary>
        ''' <param name="idNegocio">Id del negocio</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerNegocio(ByVal idNegocio As Integer) As ELL.Negocio
            Return NegociosDAL.getNegocio(idNegocio)
        End Function

        ''' <summary>
        ''' Obtiene un listado de negocios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.Negocio)
            Return NegociosDAL.loadList()
        End Function

        ''' <summary>
        ''' Comprueba si existe un negocio
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Existe(ByVal id As Integer) As Boolean
            Return NegociosDAL.existsNegocio(id)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Agregar un negocio
        ''' </summary>
        ''' <param name="id"></param> 
        Public Shared Sub AgregarNegocio(ByVal id As Integer)
            NegociosDAL.AddNegocio(id)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            NegociosDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace