'===============================================================================
'BATZ, Koop. - 27/03/2008 11:54:54
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase  esta en el directorio Architecture  en "dOOdads".
'===============================================================================
Imports AccesoAutomaticoBD
Imports log4net
Imports System.Collections.Specialized

Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class GERTAKARIAK
        Inherits _GERTAKARIAK
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
        ''' Realiza un filtrado o busqueda de las incidencias dependiendo de los parametros introducidos
        ''' </summary>
        ''' <param name="filtrado"> Objeto que contiene todos los elementos para realizar la busqueda
        ''' <para>Filtrado.Texto:Texto a buscar en todos los campos descripcion(Ver siguientes):</para>
        ''' <para>GERTAKARIAK               -->DENOMINACIONPRODUCTO,DESCRIPCIONPROBLEMA,TITULO,MARCA,PEDIDO,CANTIDAD,CAUSASPROBLEMA,CAPACIDADPROV</para>
        ''' <para>ACCIONES                  -->DESCRIPCION,EFICACIA</para>
        ''' <para>AREAORIGENKULTURA         -->DESCRIPCION</para>
        ''' <para>TIPOREPETITIVAKULTURA     -->DESCRIPCION</para>
        ''' <para>FAMILIAREPETITIVAKULTURA  -->DESCRIPCION</para>
        ''' <para>ORDENFABRICACION          -->DESCRIPCION</para>
        ''' <para>GERTAKARIAKIRUDIAK        -->DESCRIPCION</para>
        ''' </param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filtrar(ByVal filtrado As ELL.Filtrado) As IDataReader
            Try
                Dim parameters As New ListDictionary

                Dim p As OracleParameter = New OracleParameter("p_TEXTO", OracleDbType.Varchar2, ParameterDirection.Input)
                parameters.Add(p, filtrado.Texto)

                'p = New OracleParameter("p_OF", OracleDbType.Int32, ParameterDirection.Input)
                'parameters.Add(p, BLL.Utils.sqlIntegerNull(filtrado.OFa))

                'p = New OracleParameter("p_OP", OracleDbType.Int32, ParameterDirection.Input)
                'parameters.Add(p, BLL.Utils.sqlIntegerNull(filtrado.OP))

                'p = New OracleParameter("p_FECHA_INICIO", OracleDbType.Date, ParameterDirection.Input)
                'parameters.Add(p, BLL.Utils.sqlDateTimeNull(filtrado.FechaInicio))

                'p = New OracleParameter("p_FECHA_FIN", OracleDbType.Date, ParameterDirection.Input)
                'parameters.Add(p, BLL.Utils.sqlDateTimeNull(filtrado.FechaFin))

                'p = New OracleParameter("p_AREAS_ORIGEN", OracleDbType.Varchar2, ParameterDirection.Input)
                'parameters.Add(p, BLL.Utils.sqlStringNull(filtrado.AreasOrigenSQL))

                'p = New OracleParameter("p_Estado", OracleDbType.Int32, ParameterDirection.Input)
                'parameters.Add(p, filtrado.Estado)

                p = New OracleParameter("outCursor", OracleDbType.RefCursor, ParameterDirection.Output)
                parameters.Add(p, Nothing)

                Return MyBase.LoadFromSqlReader("GTK_S_FILTRO", parameters, CommandType.StoredProcedure)
            Catch ex As Exception
                Throw (ex)
            End Try
        End Function
        ''' <summary>
        ''' Realiza un filtrado o busqueda para la consulta indicada.
        ''' </summary>
        ''' <param name="SQL">Consulta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        '''Public Function FiltrarPor(ByVal Filtro As ELL.Filtrado) As IDataReader
        Public Function FiltrarPor(ByVal SQL As String) As IDataReader
            Try
                'Return MyBase.LoadFromSqlReader(Filtro.SelectFromWhere("GERTAKARIAK"), Nothing, CommandType.Text)
                Return MyBase.LoadFromSqlReader(SQL, Nothing, CommandType.Text)
            Catch ex As Exception
                Throw (ex)
            End Try
        End Function
    End Class
End Namespace
