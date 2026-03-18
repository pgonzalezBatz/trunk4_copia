
Namespace BLL

    Public Class GrupoProductoBLL

        Private grupoProducto As New DAL.GrupoProductoDAL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigoBrain"></param>
        ''' <returns></returns>
        Public Function CargarListado(ByVal codigoBrain As String) As List(Of ELL.GrupoProducto)
            Return grupoProducto.loadList(codigoBrain)
        End Function

#End Region

    End Class

End Namespace
