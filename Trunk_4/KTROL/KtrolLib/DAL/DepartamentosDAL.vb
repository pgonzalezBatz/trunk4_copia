Imports Oracle.ManagedDataAccess.Client
Namespace DAL
    Public Class DepartamentosDAL
        Inherits DALBase

#Region "CONSULTAS"

        ''' <summary>
        ''' Obtiene el listado de departamentos
        ''' </summary>
        ''' <returns>Listado de departamentos</returns>
        ''' <remarks></remarks>
        Public Function loadList(ByVal idPlanta As Integer) As List(Of ELL.Departamentos)
            Dim query As String = "SELECT ID, NOMBRE, COD_DEPARTAMENTO, RUTA_ACCESO, CALIDAD, OPERARIO, ID_PLANTA FROM DEPARTAMENTOS WHERE ID_PLANTA=:ID_PLANTA"
            Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Departamentos)(Function(r As OracleDataReader) _
            New ELL.Departamentos With {.Id = CInt(r("ID")), .Nombre = Sablib.BLL.Utils.stringNull(r("NOMBRE")), .CodigoDpto = Sablib.BLL.Utils.stringNull(r("COD_DEPARTAMENTO")), .RutaAcceso = Sablib.BLL.Utils.stringNull(r("RUTA_ACCESO")), .DptoEsCalidad = Sablib.BLL.Utils.booleanNull(r("CALIDAD")), .DptoEsOperario = Sablib.BLL.Utils.booleanNull(r("OPERARIO")), .IdPlanta = Sablib.BLL.Utils.integerNull(r("ID_PLANTA"))}, query, CadenaConexion, parameter)

        End Function

        ''' <summary>
        ''' Obtiene un departamento
        ''' </summary>
        ''' <param name="idDepartamento">Id del departamento</param>        
        ''' <returns>Datos de una máquina</returns>
        ''' <remarks></remarks>
        Public Function getDepartamento(ByVal idDepartamento As Integer) As ELL.Departamentos
            Dim query As String = "SELECT ID, NOMBRE, COD_DEPARTAMENTO, RUTA_ACCESO, CALIDAD, OPERARIO, ID_PLANTA FROM DEPARTAMENTOS WHERE ID=:ID_DEPARTAMENTO"
            Dim parameter As New OracleParameter("ID_DEPARTAMENTO", OracleDbType.Int32, idDepartamento, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Departamentos)(Function(r As OracleDataReader) _
            New ELL.Departamentos With {.Id = CInt(r("ID")), .Nombre = Sablib.BLL.Utils.stringNull(r("NOMBRE")), .CodigoDpto = Sablib.BLL.Utils.stringNull(r("COD_DEPARTAMENTO")), .RutaAcceso = Sablib.BLL.Utils.stringNull(r("RUTA_ACCESO")), .DptoEsCalidad = Sablib.BLL.Utils.booleanNull(r("CALIDAD")), .DptoEsOperario = Sablib.BLL.Utils.booleanNull(r("OPERARIO")), .IdPlanta = Sablib.BLL.Utils.integerNull(r("ID_PLANTA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Verifica si existe un departamento
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existDepartamento(ByVal codDepartamento As String) As Integer
            Try
                Dim query As String = "SELECT COUNT(*) FROM DEPARTAMENTOS WHERE COD_DEPARTAMENTO=:COD_DEPARTAMENTO"
                Dim parameter As New OracleParameter("COD_DEPARTAMENTO", OracleDbType.NVarchar2, 20, codDepartamento, ParameterDirection.Input)
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region

#Region "MODIFICACIONES"

        ''' <summary>
        ''' Guardar un departamento
        ''' </summary>
        ''' <param name="departamento">Objeto Departamentos</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveDepartamento(ByVal departamento As ELL.Departamentos) As Integer
            Dim id As Integer = Integer.MinValue
            Dim query As String = String.Empty
            Dim lParametros As New List(Of OracleParameter)

            Try
                query = "INSERT INTO DEPARTAMENTOS(NOMBRE, COD_DEPARTAMENTO, RUTA_ACCESO, CALIDAD, OPERARIO, ID_PLANTA) VALUES(:NOMBRE,:COD_DEPARTAMENTO,:RUTA_ACCESO,:CALIDAD,:OPERARIO,:ID_PLANTA) returning ID into :RETURN_VALUE"
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, 1000, departamento.Nombre, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("COD_DEPARTAMENTO", OracleDbType.NVarchar2, 20, departamento.CodigoDpto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("RUTA_ACCESO", OracleDbType.NVarchar2, 200, departamento.RutaAcceso, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CALIDAD", OracleDbType.Int16, 1, departamento.DptoEsCalidad, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OPERARIO", OracleDbType.Int16, 1, departamento.DptoEsOperario, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, departamento.IdPlanta, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParametros.ToArray)

                id = lParametros.Item(6).Value
            Catch ex As Exception
                Throw ex
            End Try
            Return id
        End Function

        ''' <summary>
        ''' Modificar los datos de un departamento
        ''' </summary>
        ''' <param name="departamento">Objeto Departamentos</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateDepartamento(ByVal departamento As ELL.Departamentos) As Boolean
            Try
                Dim query As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_DEPARTAMENTO", OracleDbType.Int32, departamento.Id, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("RUTA_ACCESO", OracleDbType.NVarchar2, 200, departamento.RutaAcceso, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CALIDAD", OracleDbType.Int16, 1, departamento.DptoEsCalidad, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OPERARIO", OracleDbType.Int16, 1, departamento.DptoEsOperario, ParameterDirection.Input))

                query = "UPDATE DEPARTAMENTOS SET RUTA_ACCESO=:RUTA_ACCESO, CALIDAD=:CALIDAD, OPERARIO=:OPERARIO WHERE ID=:ID_DEPARTAMENTO"

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParametros.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Elimina un departamento
        ''' </summary>
        ''' <param name="idDepartamento">Id del departamento</param>
        Public Function DeleteDepartamento(ByVal idDepartamento As Integer) As Integer
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "DELETE FROM DEPARTAMENTOS WHERE ID=:ID_DEPARTAMENTO"
                lParameters1.Add(New OracleParameter("ID_DEPARTAMENTO", OracleDbType.Int32, idDepartamento, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception               
                Return False
            End Try
        End Function

#End Region

    End Class
End Namespace
