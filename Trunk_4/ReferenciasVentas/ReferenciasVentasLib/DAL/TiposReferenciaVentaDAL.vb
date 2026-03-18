Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace DAL

    Public Class TiposReferenciaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList() As List(Of ELL.TiposReferenciaVenta)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION FROM REFERENCIAS_TIPOS ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TiposReferenciaVenta)(Function(r As OracleDataReader) _
            New ELL.TiposReferenciaVenta With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION"))}, query, CadenaConexionReferenciasVenta)
        End Function

        ''' <summary>
        ''' Obtiene un Driving Hand
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTipoReferenciaVenta(ByVal id As Integer) As ELL.TiposReferenciaVenta
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION FROM REFERENCIAS_TIPOS WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TiposReferenciaVenta)(Function(r As OracleDataReader) _
            New ELL.TiposReferenciaVenta With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION"))}, query, CadenaConexionReferenciasVenta, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existe(ByVal nombre As String) As Integer
            Dim parameter As New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, nombre.ToLower, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM REFERENCIAS_TIPOS WHERE LOWER(NOMBRE)=:NOMBRE"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexionReferenciasVenta, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="tipoReferenciaVenta">Objeto TiposReferenciaVenta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save(ByVal tipoReferenciaVenta As ELL.TiposReferenciaVenta) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO REFERENCIAS_TIPOS(NOMBRE, DESCRIPCION) VALUES(:NOMBRE, :DESCRIPCION)"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, tipoReferenciaVenta.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, tipoReferenciaVenta.Descripcion, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="tipoReferenciaVenta">Objeto TiposReferenciaVenta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Update(ByVal tipoReferenciaVenta As ELL.TiposReferenciaVenta) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE REFERENCIAS_TIPOS SET NOMBRE=:NOMBRE, DESCRIPCION=:DESCRIPCION WHERE ID=:ID"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, tipoReferenciaVenta.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, tipoReferenciaVenta.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, tipoReferenciaVenta.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Elimina un registro
        ''' </summary>
        ''' <param name="idTipoReferenciaVenta">Identificador de Tipo de Referencia de Venta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Delete(ByVal idTipoReferenciaVenta As Integer) As Boolean
            Dim query As String = String.Empty

            Try
                query = "DELETE FROM REFERENCIAS_TIPOS WHERE ID=:ID"
                Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idTipoReferenciaVenta, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, parameter)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

    End Class

End Namespace