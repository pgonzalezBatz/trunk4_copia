

Imports AccesoAutomaticoBD
Imports log4net
Imports System.Collections.Specialized

Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class CLASIFICACIONREPETITIVA
        Inherits _CLASIFICACIONREPETITIVA
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

        ''' <summary>
        ''' Contador de Registros que existen relacionados con alguna Incidencia.
        ''' </summary>
        ''' <param name="Id">Identificador de la Clasificacion Repetitiva.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Contador(ByVal Id As Integer) As Integer
            Dim parameters As New ListDictionary

            Dim p As OracleParameter = New OracleParameter("p_ID", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, Id)

            p = New OracleParameter("outCursor", OracleDbType.RefCursor, ParameterDirection.Output)
            parameters.Add(p, Nothing)

            Return CInt(MyBase.LoadFromSqlScalar("GTK_S_CLASREP_CONTADOR", parameters, CommandType.StoredProcedure))

        End Function

    End Class

End Namespace
