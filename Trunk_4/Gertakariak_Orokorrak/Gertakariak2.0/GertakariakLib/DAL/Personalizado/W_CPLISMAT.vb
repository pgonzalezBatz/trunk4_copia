Imports AccesoAutomaticoBD
Imports log4net

Namespace DAL
    Public Class W_CPLISMAT
        Inherits _W_CPLISMAT
        Private Log As ILog = LogManager.GetLogger("root.GertakariakLib")
        Public Sub New()
            'Decide connection string depending on state
            Try
                If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
					Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("ConexionWeb_LIVE").ConnectionString
                Else
					Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("ConexionWeb_TEST").ConnectionString
                End If
            Catch ex As Exception
                Log.Error("Error al inicializar el connection string the Gertakariak.", ex)
            End Try
        End Sub
    End Class
End Namespace

