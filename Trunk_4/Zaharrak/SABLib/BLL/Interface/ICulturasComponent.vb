Imports System.Collections.Generic

Namespace BLL.Interface
    Public Interface ICulturasComponent

        ''' <summary>
        ''' Obtiene todas las culturas
        ''' </summary>
        Function GetCulturas() As List(Of ELL.cultura)

    End Interface
End Namespace