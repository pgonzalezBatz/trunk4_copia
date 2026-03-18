Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class UsuariosRolBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idSab As Integer) As List(Of ELL.UsuarioRol)
            Return DAL.UsuariosRolDAL.loadList(idSab)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="isSab"></param>
        ''' <param name="idRecurso"></param>
        Public Shared Sub GuardarSolicitante(ByVal isSab As Integer, ByVal idRecurso As Integer)
            Dim recursosBLL As New SabLib.BLL.RecursosComponent()
            recursosBLL.AddUsuario(isSab, idRecurso)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="idRecurso"></param>
        Public Shared Sub EliminarSolicitante(ByVal id As Integer, ByVal idRecurso As Integer)
            ' Obtenemos el usuario/rol
            Dim listaUsuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(id)

            ' Si tiene algun rol quiere decir que además de solicitante es otra cosa con lo cual no se puede borrar
            ' Si no tiene ningún rol se le puede borrar el recurso
            If (listaUsuariosRol.Count = 0) Then
                Dim recursosBLL As New SabLib.BLL.RecursosComponent()
                recursosBLL.DeleteUsuario(id, idRecurso)
            End If
        End Sub

#End Region

    End Class

End Namespace