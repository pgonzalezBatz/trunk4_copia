Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class RevisionesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una revisión
        ''' </summary>
        ''' <param name="idObjetivo"></param>        
        ''' <param name="revision"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getRevision(ByVal idObjetivo As Integer, ByVal revision As Integer) As ELL.Revision
            Dim query As String = "SELECT * FROM VREVISIONES WHERE ID_OBJETIVO=:ID_OBJETIVO AND REVISION=:REVISION"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("REVISION", OracleDbType.Int32, revision, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Revision)(Function(r As OracleDataReader) _
            New ELL.Revision With {.IdObjetivo = CInt(r("ID_OBJETIVO")), .Revision = CInt(r("REVISION")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")),
                               .PAAñoSiguiente = CBool(r("PA_AÑO_SIGUIENTE")), .PAComentario = SabLib.BLL.Utils.stringNull(r("PA_COMENTARIO")),
                               .FechaAlta = CDate(r("FECHA_ALTA")), .DescripcionObjetivo = CStr(r("DESCRIPCION")), .IdResponsable = CInt(r("ID_RESPONSABLE")),
                               .AñoObjetivo = CInt(r("AÑO_OBJETIVO"))}, query, CadenaConexion, lParameters.ToArray).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de revisiones
        ''' </summary>
        ''' <param name="idResponsable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListByResponsable(ByVal idResponsable As Integer) As List(Of ELL.Revision)
            Dim query As String = "SELECT * FROM VREVISIONES WHERE ID_RESPONSABLE=:ID_RESPONSABLE"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, idResponsable, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Revision)(Function(r As OracleDataReader) _
            New ELL.Revision With {.IdObjetivo = CInt(r("ID_OBJETIVO")), .Revision = CInt(r("REVISION")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")),
                               .PAAñoSiguiente = CBool(r("PA_AÑO_SIGUIENTE")), .PAComentario = SabLib.BLL.Utils.stringNull(r("PA_COMENTARIO")),
                               .FechaAlta = CDate(r("FECHA_ALTA")), .DescripcionObjetivo = CStr(r("DESCRIPCION")), .IdResponsable = CInt(r("ID_RESPONSABLE")),
                               .AñoObjetivo = CInt(r("AÑO_OBJETIVO"))}, query, CadenaConexion, lParameters.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene un listado de revisiones
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListByObjetivo(ByVal idObjetivo As Integer) As List(Of ELL.Revision)
            Dim query As String = "SELECT * FROM VREVISIONES WHERE ID_OBJETIVO=:ID_OBJETIVO"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Revision)(Function(r As OracleDataReader) _
            New ELL.Revision With {.IdObjetivo = CInt(r("ID_OBJETIVO")), .Revision = CInt(r("REVISION")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")),
                               .PAAñoSiguiente = CBool(r("PA_AÑO_SIGUIENTE")), .PAComentario = SabLib.BLL.Utils.stringNull(r("PA_COMENTARIO")),
                               .FechaAlta = CDate(r("FECHA_ALTA")), .DescripcionObjetivo = CStr(r("DESCRIPCION")), .IdResponsable = CInt(r("ID_RESPONSABLE")),
                               .AñoObjetivo = CInt(r("AÑO_OBJETIVO"))}, query, CadenaConexion, lParameters.ToArray)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una revision
        ''' </summary>
        ''' <param name="revision">Reto</param>
        Public Shared Sub Save(ByVal revision As ELL.Revision)
            Dim bNuevo As Boolean = (getRevision(revision.IdObjetivo, revision.Revision) Is Nothing)
            Dim query As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, revision.IdObjetivo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("REVISION", OracleDbType.Int32, revision.Revision, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(revision.Comentario), DBNull.Value, revision.Comentario), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PA_AÑO_SIGUIENTE", OracleDbType.Int32, revision.PAAñoSiguiente, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PA_COMENTARIO", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(revision.PAComentario), DBNull.Value, revision.PAComentario), ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO REVISION (ID_OBJETIVO, REVISION, COMENTARIO, PA_AÑO_SIGUIENTE, PA_COMENTARIO) " _
                        & "VALUES (:ID_OBJETIVO, :REVISION, :COMENTARIO, :PA_AÑO_SIGUIENTE, :PA_COMENTARIO)"
            Else
                query = "UPDATE REVISION SET COMENTARIO=:COMENTARIO, PA_AÑO_SIGUIENTE=:PA_AÑO_SIGUIENTE, PA_COMENTARIO=:PA_COMENTARIO WHERE ID_OBJETIVO=:ID_OBJETIVO AND REVISION=:REVISION"
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una revision
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="revision"></param>
        Public Shared Sub Delete(ByVal idObjetivo As Integer, ByVal revision As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = "DELETE FROM REVISION WHERE ID_OBJETIVO=:ID_OBJETIVO AND REVISION=:REVISION"
            Dim listaDocumentos As List(Of ELL.Documento) = DAL.DocumentosDAL.loadList(idObjetivo, ELL.TipoDocumento.Tipo.Revision_cierre).Where(Function(f) f.Revision = revision).ToList()
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' Borramos la revision
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("REVISION", OracleDbType.Int32, revision, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                ' Borramos los documentos
                DAL.DocumentosDAL.DeleteDocumentos(idObjetivo, ELL.TipoDocumento.Tipo.Revision_cierre, con, revision:=revision)

                transact.Commit()

                ' Borramos fisicamente los ficheros
                Dim pathFichero As String = Configuration.ConfigurationManager.AppSettings("rootDocumentos")
                For Each documento In listaDocumentos
                    IO.Directory.GetFiles(pathFichero, documento.Id & ".*").ToList().ForEach(Sub(s) IO.File.Delete(s))
                Next
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

#End Region

    End Class

End Namespace