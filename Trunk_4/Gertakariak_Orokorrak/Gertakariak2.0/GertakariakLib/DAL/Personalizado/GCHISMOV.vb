

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class GCHISMOV 
	Inherits _GCHISMOV
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
            Dim bdGCHISMOV As New GCHISMOV
            Dim NumSeq As Integer
            bdGCHISMOV.LoadFromRawSql("Select SEQ_NUMMOV.nextval From dual")
            If Not (bdGCHISMOV.EOF) Then
                NumSeq = bdGCHISMOV.DataRow.Item(0)
            End If
            Return NumSeq
        End Function

End Class

End NameSpace
