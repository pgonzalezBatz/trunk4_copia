Namespace DAL
    Public Class CULTURAS
        Inherits _CULTURAS

        Public Sub New()
            'Decide connection string depending on state
            If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
            Else
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
            End If
        End Sub
    End Class
End Namespace