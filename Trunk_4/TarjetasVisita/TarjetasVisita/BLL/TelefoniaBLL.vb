Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class TelefoniaBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idSab As Integer) As List(Of ServicioTelefonia.Telephone)
            Dim telefonos As List(Of ServicioTelefonia.Telephone) = Nothing
            Using servicio As New ServicioTelefonia.ServiceTelefonia1
                telefonos = servicio.getTelefonos(idSab).ToList()
            End Using

            Return telefonos
        End Function

#End Region

    End Class

End Namespace