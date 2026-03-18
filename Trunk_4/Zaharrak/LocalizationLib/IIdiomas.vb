Public Interface IIdiomas
    Function GetCacheTerminos(ByVal culture As String) As Dictionary(Of String, String)
    Function GetCacheTerminos(ByVal culture As String, ByVal path As String) As Dictionary(Of String, String)
    Function GetCacheRegex(ByVal culture As String) As Dictionary(Of String, String)
End Interface