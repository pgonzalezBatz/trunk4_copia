Imports Oracle.ManagedDataAccess.Client
Namespace DAL
    Public Class OperacionKaplanPrismaDAL
        Inherits DALBase

#Region "Variables"

        ''' <summary>
        ''' Obtiene la conexion de automantenimiento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property ConexionPrisma As String
            Get
                Dim status As String = "PRISMATEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "PRISMALIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

#End Region

#Region "CONSULTAS"
        ''' <summary>
        ''' Obtiene el listado de relaciones
        ''' </summary>
        ''' <returns>Listado de departamentos</returns>
        ''' <remarks></remarks>
        Public Function loadList() As List(Of ELL.OperacionKaplanPrisma)
            Dim query As String = "SELECT ID, COD_OPERACION_KAPLAN, COD_OPERACION_PRISMA FROM OPERACION_KAPLAN_PRISMA"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.OperacionKaplanPrisma)(Function(r As OracleDataReader) _
            New ELL.OperacionKaplanPrisma With {.Id = CInt(r("ID")), .CodOperacionKaplan = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_KAPLAN")), .CodOperacionPrisma = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_PRISMA"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' Obtiene una relación
        ''' </summary>
        ''' <param name="id">Id de la relación</param>        
        ''' <returns>Objeto OperacionKaplanPrisma</returns>
        ''' <remarks></remarks>
        Public Function getOperacionKaplanPrisma(ByVal id As Integer) As ELL.OperacionKaplanPrisma
            Dim query As String = "SELECT ID, COD_OPERACION_KAPLAN, COD_OPERACION_PRISMA FROM OPERACION_KAPLAN_PRISMA WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.OperacionKaplanPrisma)(Function(r As OracleDataReader) _
            New ELL.OperacionKaplanPrisma With {.Id = CInt(r("ID")), .CodOperacionKaplan = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_KAPLAN")), .CodOperacionPrisma = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_PRISMA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene una relación
        ''' </summary>
        ''' <param name="codOpe">Código de la operación</param>        
        ''' <returns>Objeto OperacionKaplanPrisma</returns>
        ''' <remarks></remarks>
        Public Function getOperacionKaplanPrismaPorCodigoKaplan(ByVal codOpe As Integer) As ELL.OperacionKaplanPrisma
            Dim query As String = "SELECT ID, COD_OPERACION_KAPLAN, COD_OPERACION_PRISMA FROM OPERACION_KAPLAN_PRISMA WHERE COD_OPERACION_KAPLAN=:COD_OPERACION_KAPLAN"
            Dim parameter As New OracleParameter("COD_OPERACION_KAPLAN", OracleDbType.Int32, codOpe, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.OperacionKaplanPrisma)(Function(r As OracleDataReader) _
            New ELL.OperacionKaplanPrisma With {.Id = CInt(r("ID")), .CodOperacionKaplan = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_KAPLAN")), .CodOperacionPrisma = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_PRISMA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene una relación
        ''' </summary>
        ''' <param name="codOpe">Código de la operación</param>        
        ''' <returns>Objeto OperacionKaplanPrisma</returns>
        ''' <remarks></remarks>
        Public Function getOperacionKaplanPrismaPorCodigoOperacion(ByVal codOpe As Integer) As List(Of ELL.OperacionKaplanPrisma)
            Dim query As String = "SELECT ID, COD_OPERACION_KAPLAN, COD_OPERACION_PRISMA, DESCRIPCION_PRISMA FROM OPERACION_KAPLAN_PRISMA WHERE COD_OPERACION_KAPLAN=:COD_OPERACION"
            Dim parameter As New OracleParameter("COD_OPERACION", OracleDbType.Int32, codOpe, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.OperacionKaplanPrisma)(Function(r As OracleDataReader) _
            New ELL.OperacionKaplanPrisma With {.Id = CInt(r("ID")), .CodOperacionKaplan = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_KAPLAN")), .CodOperacionPrisma = Sablib.BLL.Utils.integerNull(r("COD_OPERACION_PRISMA")), .DescripcionPrisma = Sablib.BLL.Utils.stringNull(r("DESCRIPCION_PRISMA"))}, query, CadenaConexion, parameter)
        End Function

#End Region

#Region "MODIFICACIONES"

        ''' <summary>
        ''' Guardar una nueva relación
        ''' </summary>
        ''' <param name="operacionKaplanPrisma">Objeto OperacionKaplanPrisma</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveOperacionKaplanPrisma(ByVal operacionKaplanPrisma As ELL.OperacionKaplanPrisma) As Boolean
            Dim id As Integer = Integer.MinValue
            Dim query As String = String.Empty
            Dim lParametros As New List(Of OracleParameter)

            Try
                query = "INSERT INTO OPERACION_KAPLAN_PRISMA(COD_OPERACION_KAPLAN, COD_OPERACION_PRISMA) VALUES(:COD_OPERACION_KAPLAN,:COD_OPERACION_PRISMA) returning ID into :RETURN_VALUE"
                lParametros.Add(New OracleParameter("COD_OPERACION_KAPLAN", OracleDbType.Int32, operacionKaplanPrisma.CodOperacionKaplan, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("COD_OPERACION_PRISMA", OracleDbType.Int32, operacionKaplanPrisma.CodOperacionPrisma, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParametros.ToArray)

                id = lParametros.Item(2).Value

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Elimina una relación
        ''' </summary>
        ''' <param name="id">Id de la relación</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeleteOperacionKaplanPrisma(ByVal id As Integer) As Integer
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters As New List(Of OracleParameter)

                query = "DELETE FROM OPERACION_KAPLAN_PRISMA WHERE ID=:ID"
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region "PRISMA"

        ''' <summary>
        ''' Guarda el identificador de prisma
        ''' </summary>
        ''' <param name="idSolicitudPrisma">Identificador de solicitud de prisma</param>
        ''' <param name="idControl">Identificador del control</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarIdSolicitudPrisma(ByVal idSolicitudPrisma As Integer, ByVal idControl As Integer) As Boolean
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE CONTROLES_ERRORES SET ID_SOLICITUD_PRISMA=:ID_SOLICITUD_PRISMA WHERE ID_CONTROL=:ID_CONTROL"
                lParameters1.Add(New OracleParameter("ID_SOLICITUD_PRISMA", OracleDbType.Int32, idSolicitudPrisma, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el numero de solicitud de la incidencia ábierta en PRISMA
        ''' </summary>
        ''' <param name="workRequestType">Tipo de Request</param>
        ''' <param name="company">Compañia</param>
        ''' <returns>Numero de trabajador encontrado</returns>        
        Public Function GetIdSolicitudPrisma(ByVal workRequestType As String, ByVal company As String) As String
            Try
                'Se hace asi la consulta porque en esta tabla hay algunos valores que no se pueden convertir a entero
                Dim query As String = "SELECT WORKREQUEST FROM " & _
                 "(SELECT WORKREQUEST FROM WORKREQUEST WHERE WORKREQUESTTYPE=:WORK_REQUEST_TPE and COMPANY=:COMPANY ORDER BY WORKREQUEST DESC) " & _
                 "WHERE ROWNUM=1"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("COMPANY", OracleDbType.Varchar2, company, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("WORK_REQUEST_TPE", OracleDbType.Varchar2, workRequestType, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, ConexionPrisma, lParametros.ToArray)
            Catch ex As Exception
                Throw New Sablib.BatzException("Error al obtener el id de la solicitud a Prisma", ex)
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el numero de trabajador de la tabla de prisma
        ''' </summary>
        ''' <param name="numTra">Numero de trabajador</param>
        ''' <param name="company">Compañia</param>
        ''' <returns>Numero de trabajador encontrado</returns>        
        Public Function GetNumTrabajador(ByVal numTra As Integer, ByVal company As String) As String
            Try
                'Se hace asi la consulta porque en esta tabla hay algunos valores que no se pueden convertir a entero
                Dim query As String = "SELECT REQUESTER FROM W_REQUESTER WHERE COMPANY=:COMPANY AND (CASE WHEN regexp_like(REQUESTER,'^[0-9]+$') THEN TO_NUMBER(REQUESTER) ELSE 0 END)=:NUM_TRAB"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("COMPANY", OracleDbType.Varchar2, company, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NUM_TRAB", OracleDbType.Int32, numTra, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, CadenaConexion, lParametros.ToArray)
            Catch ex As Exception
                Throw New Sablib.BatzException("Error al obtener el numero de trabajador", ex)
                Return String.Empty
            End Try
        End Function

#End Region

    End Class
End Namespace
