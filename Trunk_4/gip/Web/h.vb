Partial Class h
    Public Shared Function GetidPlanta() As Integer
        Return Db.GetPlanta(SimpleRoleProvider.GetId(), ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
    End Function
    Public Shared Function GetListOfidPlanta() As IEnumerable(Of Integer)
        Return Db.GetListOfPlanta(SimpleRoleProvider.GetId(), ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
    End Function

End Class
