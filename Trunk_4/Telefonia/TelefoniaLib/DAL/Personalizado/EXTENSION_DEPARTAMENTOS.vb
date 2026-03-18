Imports System.Collections.Specialized
Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class EXTENSION_DEPARTAMENTOS 
	Inherits _EXTENSION_DEPARTAMENTOS
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
        ''' Obtiene las extensiones de un departamento
        ''' </summary>
        ''' <param name="idDep">Identificador del departamento</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getExtensionesDepartamento(ByVal idDep As Integer) As DataTable
            Dim sql As New System.Text.StringBuilder
            Dim dt As New DataTable
            Dim where As String = String.Empty
            Dim reader As IDataReader
            Dim dataAdapter As New BLL.DataReaderAdapterBatz
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter(":ID_DEP", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, idDep)

			'070410: Añadido el control de la fecha de EP
            sql.AppendLine("SELECT E.NOMBRE,E.EXTENSION AS ExtensionInterna,T.NUMERO AS TlfnoDirecto,EI.EXTENSION AS ExtensionMovil,T2.NUMERO AS TlfnoMovil,E.ID_PLANTA,P.NOMBRE AS PLANTA,E.ID_TIPOLINEA") 'T2.NUMERO AS TlfnoDirecto,T.NUMERO AS TlfnoMovil
            sql.AppendLine(" FROM EXTENSION_DEPARTAMENTOS EP INNER JOIN EXTENSION E ON EP.ID_EXTENSION=E.ID")
            sql.AppendLine(" LEFT JOIN EXTENSION EI ON EI.ID_EXT_INTERNA=E.ID")   'ON E.ID_EXT_INTERNA=EI.ID
            sql.AppendLine(" LEFT JOIN TELEFONO T ON E.ID_TELEFONO=T.ID")
            sql.AppendLine(" LEFT JOIN TELEFONO T2 ON EI.ID_TELEFONO=T2.ID")
            sql.AppendLine(" LEFT JOIN SAB.PLANTAS P ON E.ID_PLANTA=P.ID")
            sql.AppendLine("WHERE EP.ID_DEPARTAMENTO=:ID_DEP AND (EP.F_HASTA IS NULL OR (EP.F_HASTA IS NOT NULL AND EP.F_HASTA>=SYSDATE)) AND (E.VISIBLE IS NOT NULL AND E.VISIBLE=-1)") 'Se ha cambiado EI.Visible por E.Visible porque habia una extension de un depto que no se veia
            sql.AppendLine("ORDER BY EI.EXTENSION ")

            reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
            Return dataAdapter.FillFromReader(dt, reader)
        End Function

End Class

End NameSpace
