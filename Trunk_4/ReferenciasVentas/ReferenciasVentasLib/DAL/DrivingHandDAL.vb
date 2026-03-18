Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace DAL

    Public Class DrivingHandDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList() As List(Of ELL.DrivingHand)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM DRIVING_HAND ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DrivingHand)(Function(r As OracleDataReader) _
            New ELL.DrivingHand With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta)
        End Function

        ''' <summary>
        ''' Obtiene un listado de Driving Hand activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarDrivingHandActivos() As List(Of ELL.DrivingHand)
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM DRIVING_HAND WHERE OBSOLETO = 0"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DrivingHand)(Function(r As OracleDataReader) _
            New ELL.DrivingHand With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta)
        End Function

        ''' <summary>
        ''' Obtiene un Driving Hand
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarDrivingHand(ByVal id As Integer) As ELL.DrivingHand
            Dim query As String = "SELECT ID, NOMBRE, DESCRIPCION, OBSOLETO FROM DRIVING_HAND WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DrivingHand)(Function(r As OracleDataReader) _
            New ELL.DrivingHand With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexionReferenciasVenta, parameter).FirstOrDefault
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
                Dim query As String = "SELECT COUNT(*) FROM DRIVING_HAND WHERE LOWER(NOMBRE)=:NOMBRE"
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
        ''' <param name="drivingHand">Objeto DrivingHand</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save(ByVal drivingHand As ELL.DrivingHand) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO DRIVING_HAND(NOMBRE, DESCRIPCION, OBSOLETO) VALUES(:NOMBRE, :DESCRIPCION, :OBSOLETO)"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, drivingHand.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, drivingHand.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, drivingHand.Obsoleto, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="drivingHand">Objeto DrivingHand</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Update(ByVal drivingHand As ELL.DrivingHand) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE DRIVING_HAND SET NOMBRE=:NOMBRE, DESCRIPCION=:DESCRIPCION, OBSOLETO=:OBSOLETO WHERE ID=:ID"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, drivingHand.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, 50, drivingHand.Descripcion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, 1, drivingHand.Obsoleto, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, drivingHand.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        ' ''' <summary>
        ' ''' Elimina un registro
        ' ''' </summary>
        ' ''' <param name="idDrivingHand">Identificador DrivingHand</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function Delete(ByVal idDrivingHand As Integer) As Boolean
        '    Dim query As String = String.Empty

        '    Try
        '        query = "DELETE FROM DRIVING_HAND WHERE ID=:ID"
        '        Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idDrivingHand, ParameterDirection.Input)
        '        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, parameter)

        '        Return True
        '    Catch ex As Exception
        '        Return False
        '    End Try
        'End Function

#End Region

    End Class

End Namespace