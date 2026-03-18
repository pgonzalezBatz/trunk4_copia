

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class TIPO_ALVEOLO 
	Inherits _TIPO_ALVEOLO
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
        ''' Consulta los tipos de alveolos existentes, dadas unas condiciones
        ''' </summary>
        ''' <param name="oTipoAlv"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTiposAlv(ByVal oTipoAlv As ELL.TipoCultura) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim where As String = String.Empty
            sql.AppendLine("SELECT TA.ID,TAC.NOMBRE,TAC.DESCRIPCION,TAC.ID_CULTURA ")
            sql.AppendLine("FROM TIPO_ALVEOLO TA INNER JOIN TIPOALV_CULTURA TAC ")
            sql.AppendLine("ON TA.ID=TAC.ID_TIPOALV ")

            If (oTipoAlv.Id <> Integer.MinValue) Then
                where = "TA.ID=" & oTipoAlv.Id
            End If
            If (oTipoAlv.Cultura <> String.Empty) Then
                If (where <> String.Empty) Then where &= " and "
                where &= "TAC.ID_CULTURA='" & oTipoAlv.Cultura & "'"
            End If
            If (Not oTipoAlv.Obsoleto) Then
                If (where <> String.Empty) Then where &= " and "
                where &= "TA.OBSOLETO=" & CInt(oTipoAlv.Obsoleto)
            End If

            If (where <> String.Empty) Then
                where = "WHERE " & where
                sql.AppendLine(where)
            End If

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
        End Function


End Class

End NameSpace
