'===============================================================================
'BATZ, Koop. - 28/02/2008 9:41:44
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase  esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports AccesoAutomaticoBD
Imports log4net
Imports System.Collections.Specialized

Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class FAMILIAREPETITIVA
        Inherits _FAMILIAREPETITIVA
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


        Public Function fgFamiliasRepetitivasContador(ByVal Id As Integer) As IDataReader
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter("p_ID", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, Id)

            p = New OracleParameter("outCursor", OracleDbType.RefCursor, ParameterDirection.Output)
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSqlReader("GTK_S_FAMILIAS_CONTADOR", parameters, CommandType.StoredProcedure)
        End Function

        ''' <summary>
        ''' Contador de Registros que existen relacionados con alguna Incidencia.
        ''' </summary>
        ''' <param name="Id">Identificador de la Familia Repetitiva.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Contador(ByVal Id As Integer) As Integer
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter("p_ID", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, Id)

            p = New OracleParameter("outCursor", OracleDbType.RefCursor, ParameterDirection.Output)
            parameters.Add(p, Nothing)

            Return CInt(MyBase.LoadFromSqlScalar("GTK_S_FAMREP_CONTADOR", parameters, CommandType.StoredProcedure))

        End Function

        Private Function OutCursor() As ListDictionary
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor, ParameterDirection.Output)
            parameters.Add(p, Nothing)

            Return parameters
        End Function

        ''' <summary>
        ''' Realiza un filtrado o busqueda de las incidencias dependiendo de los parametros introducidos
        ''' </summary>
        ''' <param name="SQL">Consulta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FiltrarPor(ByVal SQL As String) As IDataReader
            Try
                Return MyBase.LoadFromSqlReader(SQL, Nothing, CommandType.Text)
            Catch ex As Exception
                Throw (ex)
            End Try
        End Function
    End Class

End Namespace
