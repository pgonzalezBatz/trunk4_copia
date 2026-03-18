Imports System.Collections.Specialized
Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class EXTENSION_PERSONAS 
	Inherits _EXTENSION_PERSONAS
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


#Region "Sincronizacion con Geminix"

        ''' <summary>
        ''' Sincroniza SAB con telefonos, para saber que usuarios se han dado de baja, y tienen asociada una extension o un telefono
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta a sincronizar</param>
        ''' <returns></returns>        
        Public Function SincronizacionGeminix(ByVal idPlanta As Integer) As DataTable
            Dim sql As New System.Text.StringBuilder
            Dim dt As New DataTable
            Dim reader As IDataReader
            Dim dataAdapter As New BLL.DataReaderAdapterBatz
            sql.AppendLine(" SELECT DISTINCT E.EXTENSION,T.NUMERO,U.ID ,RTRIM(LTRIM((nvl(U.nombre,'') || ' ' || nvl(U.apellido1,'') || ' ' || nvl(U.apellido2,'')))) as Nombre")
            sql.AppendLine(" FROM SAB.USUARIOS U inner join EXTENSION_PERSONAS EP ON U.ID=EP.ID_USUARIO")
            sql.AppendLine(" LEFT JOIN TELEFONO_PERSONAS TP ON U.ID=TP.ID_USUARIO")
            sql.AppendLine(" LEFT JOIN EXTENSION E ON (EP.ID_EXTENSION=E.ID AND E.ID_PLANTA=:ID_PLANTA)")
            sql.AppendLine(" LEFT JOIN TELEFONO T ON (TP.ID_TLFNO=T.ID AND T.ID_PLANTA=:ID_PLANTA)")
            sql.AppendLine(" WHERE(U.FechaBaja Is Not null And U.fechaBaja < sysdate) AND U.IDPLANTA=:ID_PLANTA")
            sql.AppendLine(" AND (EP.F_HASTA IS NULL OR (EP.F_HASTA IS NOT NULL AND EP.F_HASTA>sysdate))")
            sql.AppendLine(" AND (TP.F_HASTA IS NULL OR (TP.F_HASTA IS NOT NULL AND TP.F_HASTA>sysdate))")
            sql.AppendLine(" AND extension is not null or numero is not null")
            sql.AppendLine(" ORDER BY U.ID")
            Dim parameters As New ListDictionary
            Dim p As OracleParameter = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, idPlanta)
            reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
            Return dataAdapter.FillFromReader(dt, reader)
        End Function

#End Region

    End Class

End NameSpace
