

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

    Public Class OTROS
        Inherits _OTROS
        Private Log As ILog = LogManager.GetLogger("root.Telefonia")
        Public Sub New()
            'Decide connection string depending on state
            Try
                If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIALIVE").ConnectionString
                Else
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIATEST").ConnectionString
                End If
            Catch ex As Exception
                Log.Error("Error al inicializar el connection string Telefonia.", ex)
            End Try
        End Sub

    End Class

End NameSpace
