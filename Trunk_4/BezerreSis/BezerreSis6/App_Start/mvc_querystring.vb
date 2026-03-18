Partial Public Class h
    Public Shared Function ToRouteValues(col As NameValueCollection, obj As Object) As RouteValueDictionary
        Dim values As RouteValueDictionary
        If obj Is Nothing Then
            values = New RouteValueDictionary
        Else
            values = New RouteValueDictionary(obj)
        End If
        If col IsNot Nothing Then
            For Each key In col
                'values passed in object override those already in collection
                If Not values.ContainsKey(key) Then
                    values(key) = col(key)
                End If
            Next
        End If
        Return values
    End Function
    Public Shared Function ToRouteValues(col As System.Web.Routing.RouteValueDictionary, obj As Object) As System.Web.Routing.RouteValueDictionary
        Dim values = New System.Web.Routing.RouteValueDictionary(obj)
        If col IsNot Nothing Then
            For Each kv In col
                'values passed in object override those already in collection
                If Not values.ContainsKey(kv.Key) Then
                    values(kv.Key) = col(kv.Key)
                End If
            Next
        End If
        Return values
    End Function
    Public Shared Function ToRouteValuesDelete(col As NameValueCollection, ByVal ParamArray toRemove() As String) As RouteValueDictionary
        Dim values As RouteValueDictionary
        values = New RouteValueDictionary

        col.CopyTo(values)

        If toRemove IsNot Nothing Then
            For Each key In toRemove
                'values passed in object override those already in collection
                values.Remove(key)
            Next
        End If
        Return values
    End Function
    Public Shared Function ToRouteValuesDelete(col As System.Web.Routing.RouteValueDictionary, ByVal ParamArray toRemove() As String) As System.Web.Routing.RouteValueDictionary
        If toRemove IsNot Nothing Then
            For Each key In toRemove
                col.Remove(key)
            Next
        End If
        Return col
    End Function
End Class