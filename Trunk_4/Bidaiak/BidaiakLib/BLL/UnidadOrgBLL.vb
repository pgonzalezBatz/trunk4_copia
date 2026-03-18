Namespace BLL

    Public Class UnidadOrgBLL

        Private unidadOrg As New DAL.UnidadOrgDAL

        ''' <summary>
        ''' Obtiene la informacion de una unidad organizativa
        ''' </summary>
        ''' <param name="id">Id de la unidad</param>
        ''' <returns></returns>		
        Public Function load(ByVal id As Integer) As ELL.UnidadOrg
            Dim oUnidad As ELL.UnidadOrg = unidadOrg.load(id)
            If (oUnidad IsNot Nothing) Then
                Dim sablibComp As New Sablib.BLL.UsuariosComponent
                oUnidad.Responsable = sablibComp.GetUsuario(New Sablib.ELL.Usuario With {.Id = oUnidad.Responsable.Id}, False)
            End If
            Return oUnidad
        End Function

        ''' <summary>
        ''' Obtiene una lista de unidades de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>		
        Public Function loadList(ByVal idPlanta As Integer) As System.Collections.Generic.List(Of ELL.UnidadOrg)
            Return unidadOrg.loadList(idPlanta)
        End Function
    End Class

End Namespace
