Module DBAccessController
    Private ReadOnly strCn = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
    Public Function GetAgrupacion(ByVal id As Integer) As VMAgrupacion
        Dim ag = DBAccess.GetAgrupacion(id, strCn)
        Return New VMAgrupacion With {.Id = ag.Id, .IdParent = ag.idParent, .Peso = ag.Peso}
    End Function
End Module
