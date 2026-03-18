

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class GCCABPED 
	Inherits _GCCABPED
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
                Log.Error("Error al inicializar el connection string de XBAT.", ex)
		End Try
        End Sub

        Public Function Secuencia()
            Dim bdGCCABPED As New GCCABPED
            Dim NumPedCab As Integer
            bdGCCABPED.LoadFromRawSql("Select SEQ_NUMPEDC.nextval From dual")
            If Not (bdGCCABPED.EOF) Then
                NumPedCab = bdGCCABPED.DataRow.Item(0)
            End If
            Return NumPedCab
        End Function

    End Class
End Namespace
