Imports System.Collections.Specialized
Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

    Public Class EXTENSION_OTROS
        Inherits _EXTENSION_OTROS
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
        ''' Obtiene las extensiones de un item otro
        ''' </summary>
		''' <param name="idOtro">Identificador del otro</param>
		''' <param name="bVisible">Indicara si se quieren obtener los visibles, no visibles o todos</param>   
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Function getExtensionesOtro(ByVal idOtro As Integer, ByVal bVisible As Nullable(Of Boolean)) As DataTable
			Dim sql As New System.Text.StringBuilder
			Dim dt As New DataTable
			Dim where As String = String.Empty
			Dim reader As IDataReader
			Dim dataAdapter As New BLL.DataReaderAdapterBatz
			Dim parameters As New ListDictionary

			Dim p As OracleParameter = New OracleParameter(":ID_OTRO", OracleDbType.Int32, ParameterDirection.Input)
			parameters.Add(p, idOtro)
			If (bVisible.HasValue) Then
				p = New OracleParameter(":VISIBLE", OracleDbType.Int32, ParameterDirection.Input)
				parameters.Add(p, CInt(bVisible.Value))
			End If

            sql.AppendLine("SELECT E.NOMBRE,EI.EXTENSION AS ExtensionInterna,T2.NUMERO AS TlfnoDirecto,E.EXTENSION AS ExtensionMovil,T.NUMERO AS TlfnoMovil,E.ID_TIPOLINEA,E.ID_PLANTA")
			sql.AppendLine(" FROM EXTENSION_OTROS EO INNER JOIN EXTENSION E ON EO.ID_EXTENSION=E.ID")
			sql.AppendLine(" LEFT JOIN EXTENSION EI ON E.ID_EXT_INTERNA=EI.ID")
			sql.AppendLine(" LEFT JOIN TELEFONO T ON E.ID_TELEFONO=T.ID")
			sql.AppendLine(" LEFT JOIN TELEFONO T2 ON EI.ID_TELEFONO=T2.ID")
            sql.AppendLine("WHERE EO.ID_OTRO=:ID_OTRO AND EO.F_HASTA IS NULL ")
            If (bVisible.HasValue) Then
				sql.AppendLine(" AND E.VISIBLE=:VISIBLE")
			End If
			sql.AppendLine("ORDER BY EI.EXTENSION ")

			reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
			Return dataAdapter.FillFromReader(dt, reader)
		End Function

    End Class

End NameSpace
