Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ProcesosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getProceso(ByVal idProceso As Integer) As ELL.Proceso
            Dim query As String = "SELECT * FROM VPROCESOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idProceso, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Proceso)(Function(r As OracleDataReader) _
            New ELL.Proceso With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .Codigo = CStr(r("CODIGO")), .Nombre = CStr(r("NOMBRE")),
                               .FechaAlta = CDate(r("FECHA_ALTA")), .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")),
                               .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")),
                               .Planta = CStr(r("PLANTA")), .Orden = CInt(r("ORDEN"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de procesos
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="baja"></param>
        ''' <param name="nombre"></param>
        ''' <param name="procesoDeBaja"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(Optional ByVal idPlanta As Integer? = Nothing, Optional ByVal baja As Boolean? = False, Optional ByVal nombre As String = "", Optional ByVal procesoDeBaja? As Boolean = Nothing) As List(Of ELL.Proceso)
            Dim query As String = "SELECT * FROM VPROCESOS{0}"
            Dim where As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            If (idPlanta IsNot Nothing) Then
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                where = " WHERE ID_PLANTA=:ID_PLANTA"
            End If

            If (baja IsNot Nothing) Then
                lParameters.Add(New OracleParameter("BAJA", OracleDbType.Int32, baja, ParameterDirection.Input))
                If (String.IsNullOrEmpty(where)) Then
                    where = " WHERE BAJA=:BAJA"
                Else
                    where &= " AND BAJA=:BAJA"
                End If
            End If

            If (Not String.IsNullOrEmpty(nombre)) Then
                lParameters.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, nombre.ToUpper(), ParameterDirection.Input))
                If (String.IsNullOrEmpty(where)) Then
                    where = " WHERE UPPER(NOMBRE) LIKE '%' || :NOMBRE || '%'"
                Else
                    where &= " AND UPPER(NOMBRE) LIKE '%' || :NOMBRE || '%'"
                End If
            End If

            If (procesoDeBaja IsNot Nothing) Then
                If (String.IsNullOrEmpty(where)) Then
                    If (procesoDeBaja) Then
                        where = " WHERE FECHA_BAJA IS NOT NULL"
                    Else
                        where = " WHERE FECHA_BAJA IS NULL"
                    End If
                Else
                    If (procesoDeBaja) Then
                        where &= " AND FECHA_BAJA IS NOT NULL"
                    Else
                        where &= " AND FECHA_BAJA IS NULL"
                    End If
                End If
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Proceso)(Function(r As OracleDataReader) _
            New ELL.Proceso With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .Codigo = CStr(r("CODIGO")), .Nombre = CStr(r("NOMBRE")),
                               .FechaAlta = CDate(r("FECHA_ALTA")), .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")),
                               .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")),
                               .Planta = CStr(r("PLANTA")), .Orden = CInt(r("ORDEN"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsPlanta(ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM PROCESO WHERE ID_PLANTA=:ID_PLANTA"
            Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

        ''' <summary>
        ''' Comprueba si existe el codigo
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsCodigo(ByVal codigo As String, ByVal idPlanta As Integer, Optional ByVal idProceso As Integer? = Nothing) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM PROCESO WHERE LOWER(CODIGO)=LOWER(:CODIGO) AND ID_PLANTA=:ID_PLANTA{0}"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("CODIGO", OracleDbType.NVarchar2, codigo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            If (idProceso IsNot Nothing) Then
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idProceso, ParameterDirection.Input))
                query = String.Format(query, " AND ID <> :ID")
            Else
                query = String.Format(query, String.Empty)
            End If

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
            Return filas > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un proceso
        ''' </summary>
        ''' <param name="proceso">Proceso</param>  
        Public Shared Function Save(ByVal proceso As ELL.Proceso) As Integer
            Dim bNuevo As Boolean = (proceso.Id = 0)
            Dim query As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("CODIGO", OracleDbType.NVarchar2, proceso.Codigo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, proceso.Nombre, ParameterDirection.Input))

            If (bNuevo) Then
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, proceso.IdPlanta, ParameterDirection.Input))

                query = "INSERT INTO PROCESO (ID_PLANTA, CODIGO, NOMBRE, ID_USUARIO_ALTA, ORDEN) " _
                        & "VALUES (:ID_PLANTA, UPPER(:CODIGO), :NOMBRE, :ID_USUARIO_ALTA, NVL((SELECT MAX(ORDEN) + 1 FROM PROCESO WHERE ID_PLANTA=52), 1)) RETURNING ID INTO :RETURN_VALUE"

                lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, proceso.IdUsuarioAlta, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE PROCESO SET CODIGO=UPPER(:CODIGO), NOMBRE=:NOMBRE WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, proceso.Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                proceso.Id = lParameters.Last.Value
            End If

            Return proceso.Id
        End Function

        ''' <summary>
        ''' Cambia el orden
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <param name="idProcesoCambio"></param>
        ''' <remarks></remarks>
        Public Shared Sub ChangeOrder(ByVal idProceso As Integer, ByVal idProcesoCambio As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idProceso, ParameterDirection.Input))

                ' Obtenemos el orden original
                Dim query As String = "SELECT ORDEN FROM PROCESO WHERE ID=:ID"
                Dim orden As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())

                lParameters = New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idProceso, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_CAMBIO", OracleDbType.Int32, idProcesoCambio, ParameterDirection.Input))

                'Actualizamos un elemento con el orden del otro
                query = "UPDATE PROCESO SET ORDEN=(SELECT ORDEN FROM PROCESO WHERE ID=:ID_CAMBIO) WHERE ID=:ID"
                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray())

                lParameters = New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ORDEN", OracleDbType.Int32, orden, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_CAMBIO", OracleDbType.Int32, idProcesoCambio, ParameterDirection.Input))

                'Actualizamos otr elemento con el orden del ono
                query = "UPDATE PROCESO SET ORDEN=:ORDEN WHERE ID=:ID_CAMBIO"
                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray())

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Da de baja un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param> 
        ''' <param name="idUsuario">Id del ususario</param>
        ''' <remarks></remarks>
        Public Shared Sub Unsubscribe(ByVal idProceso As Integer, ByVal idUsuario As Integer)
            Dim query As String = "UPDATE PROCESO SET FECHA_BAJA=SYSDATE, ID_USUARIO_BAJA=:ID_USUARIO_BAJA WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idProceso, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_USUARIO_BAJA", OracleDbType.Int32, idUsuario, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

        ''' <summary>
        ''' Da de baja un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param> 
        ''' <remarks></remarks>
        Public Shared Sub Subscribe(ByVal idProceso As Integer)
            Dim query As String = "UPDATE PROCESO SET FECHA_BAJA=NULL, ID_USUARIO_BAJA=NULL WHERE ID=:ID"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, idProceso, ParameterDirection.Input))
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param>
        Public Shared Sub DeleteProceso(ByVal idProceso As Integer)
            Dim query As String = "DELETE FROM PROCESO WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idProceso, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace