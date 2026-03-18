Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class TransmissionModeDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los transmission mode
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList() As List(Of ELL.TransmissionMode)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM TRANSMISSION_MODE ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TransmissionMode)(Function(r As OracleDataReader) _
            New ELL.TransmissionMode With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta)
        End Function

        ''' <summary>
        ''' Obtiene todos los transmission mode activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTransmissionModeActivos() As List(Of ELL.TransmissionMode)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM TRANSMISSION_MODE WHERE OBSOLETO = 0 ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TransmissionMode)(Function(r As OracleDataReader) _
            New ELL.TransmissionMode With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta)
        End Function

        ''' <summary>
        ''' Obtiene un Driving Hand
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTransmissionMode(ByVal id As Integer) As ELL.TransmissionMode
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM TRANSMISSION_MODE WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TransmissionMode)(Function(r As OracleDataReader) _
            New ELL.TransmissionMode With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existe(ByVal nombre As String) As Integer
            Dim parameter As New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, nombre, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM TRANSMISSION_MODE WHERE LOWER(NOMBRE)=:NOMBRE"
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
        ''' <param name="transmissionMode">Objeto TransmissionMode</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save(ByVal transmissionMode As ELL.TransmissionMode) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO TRANSMISSION_MODE(NOMBRE, DESCRIPCION, OBSOLETO) VALUES(:NOMBRE, :DESCRIPCION, :OBSOLETO)"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, transmissionMode.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, transmissionMode.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, transmissionMode.Obsoleto, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="transmissionMode">Objeto TransmissionMode</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Update(ByVal transmissionMode As ELL.TransmissionMode) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE TRANSMISSION_MODE SET NOMBRE=:NOMBRE, DESCRIPCION=:DESCRIPCION, OBSOLETO=:OBSOLETO WHERE ID=:ID"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, transmissionMode.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, transmissionMode.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, transmissionMode.Obsoleto, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, transmissionMode.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace