

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class ALVEOLO 
	Inherits _ALVEOLO
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

        ''' <summary>
        ''' Obtiene los alveolos libres de una planta y que su estado sea ok
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAlveolosLibres(ByVal idPlanta As Integer) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim where As String = String.Empty
            sql.AppendLine("SELECT A.ID,A.RUTA,A.ESTADO,A.ID_PLANTA,A.OBSOLETO,A.ID_TIPOALV,A.POS_FILA,A.POS_COL ")
            sql.AppendLine("FROM ALVEOLO A ")
            sql.AppendLine("WHERE A.ID_PLANTA=" & idPlanta & " and A.OBSOLETO=0 and A.ESTADO=-1 AND ")
            sql.AppendLine("NOT EXISTS(")
            sql.AppendLine("Select E.ID_ALVEOLO FROM EXTENSION E WHERE E.ID_ALVEOLO=A.ID AND E.OBSOLETO=0) ")
            sql.AppendLine("ORDER BY A.RUTA")

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
        End Function


End Class

End NameSpace
