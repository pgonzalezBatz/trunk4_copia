

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class TIPO_LINEA 
	Inherits _TIPO_LINEA
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
        ''' Consulta los tipos de lineas existentes, dadas unas condiciones
        ''' </summary>
        ''' <param name="oTipoLin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTiposLinea(ByVal oTipoLin As ELL.TipoLinea) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim where As String = String.Empty
            sql.AppendLine("SELECT TL.ID,TLC.NOMBRE,TLC.DESCRIPCION,TLC.ID_CULTURA,TL.ID_TIPOEXT ")
            sql.AppendLine("FROM TIPO_LINEA TL INNER JOIN TIPOLINEA_CULTURA TLC ")
            sql.AppendLine("ON TL.ID=TLC.ID_TIPOLINEA ")

            If (oTipoLin.Id <> Integer.MinValue) Then
                where = "TL.ID=" & oTipoLin.Id
            End If            
            If (oTipoLin.Cultura <> String.Empty) Then
                If (where <> String.Empty) Then where &= " and "
                where &= "TLC.ID_CULTURA='" & oTipoLin.Cultura & "'"
            End If
            If (oTipoLin.IdTipoExtension <> Integer.MinValue) Then
                If (where <> String.Empty) Then where &= " and "
                where &= "TL.ID_TIPOEXT=" & oTipoLin.IdTipoExtension
            End If
            If (Not oTipoLin.Obsoleto) Then
                If (where <> String.Empty) Then where &= " and "
                where &= "TL.OBSOLETO=" & CInt(oTipoLin.Obsoleto)
            End If

            If (where <> String.Empty) Then
                where = "WHERE " & where
                sql.AppendLine(where)
            End If

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
        End Function

End Class

End NameSpace
