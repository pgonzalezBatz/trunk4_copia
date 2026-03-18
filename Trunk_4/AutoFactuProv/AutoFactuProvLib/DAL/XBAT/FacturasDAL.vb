Imports Oracle.ManagedDataAccess.Client

Namespace XBAT.DAL

    Public Class Facturas
        Inherits AutoFactuProvLib.DAL.DALBase

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getNextDOCBATZ() As String
            Dim query As String = "UPDATE DOC_BATZ SET ID = ID + 1 RETURNING ID INTO :ID"

            Dim p As New OracleParameter("ID", OracleDbType.Int32, ParameterDirection.ReturnValue)
            p.DbType = DbType.Int32

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionIntegraFactu, p)

            Return p.Value
        End Function

#End Region

    End Class

End Namespace