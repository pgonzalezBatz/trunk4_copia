Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PermisoBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Consultar(ByVal id As Integer) As ELL.Permiso
            Return DAL.PermisoDAL.getObject(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function ConsultarUltimo(ByVal idSab As Integer) As ELL.Permiso
            Return DAL.PermisoDAL.getObjectLast(idSab)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSabResponsable"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoAutorizar(ByVal idSabResponsable As Integer) As List(Of ELL.Permiso)
            Return DAL.PermisoDAL.loadListToAuthorize(idSabResponsable)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.Permiso)
            Return DAL.PermisoDAL.loadList()
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="permiso"></param>
        Public Shared Sub Guardar(ByVal permiso As ELL.Permiso)
            DAL.PermisoDAL.Save(permiso)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="permiso"></param>
        Public Shared Sub Autorizar(ByVal permiso As ELL.Permiso)
            DAL.PermisoDAL.Authorize(permiso)
        End Sub

#End Region

    End Class

End Namespace