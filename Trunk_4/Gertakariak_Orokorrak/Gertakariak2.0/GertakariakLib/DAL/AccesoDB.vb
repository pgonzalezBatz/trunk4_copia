Imports GertakariakLib.ELL
Imports Oracle.ManagedDataAccess.Client

Public Class AccesoDB


    Protected Shared ReadOnly Property CadenaConexion As String
        Get
            Dim status As String = "ConexionWeb_TEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "ConexionWeb_LIVE"
            Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Get
    End Property
    Public Shared Function GetGertakaria(idGtk As Integer) As gtkTroqueleria
        Dim query As String = "SELECT PROCEDENCIANC,TOTALACORDADO FROM GERTAKARIAK WHERE ID = :ID"
        Dim result As gtkTroqueleria = Memcached.OracleDirectAccess.Seleccionar(Of gtkTroqueleria)(Function(r As OracleDataReader) New gtkTroqueleria With {.Id = idGtk,
                                                                                                                                          .ProcedenciaNC = CInt(r("PROCEDENCIANC")),
                                                                                                                                          .TotalAcordado = CDec(r("TOTALACORDADO"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, idGtk, ParameterDirection.Input)).FirstOrDefault()
        Return result
    End Function
End Class
