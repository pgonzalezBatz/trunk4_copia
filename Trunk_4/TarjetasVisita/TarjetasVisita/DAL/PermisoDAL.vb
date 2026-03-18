Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PermisoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.Permiso
            Dim query As String = "SELECT * FROM VPERMISO_ACTUAL WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Permiso)(Function(r As OracleDataReader) _
            New ELL.Permiso With {.Id = CInt(r("ID")), .IdSab = CInt(r("ID_SAB")), .FechaSolicitud = CDate(r("FECHA_SOLICITUD")), .IdSabResponsable = SabLib.BLL.Utils.integerNull(r("ID_SAB_RESPONSABLE")),
                                  .FechaRespuesta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_RESPUESTA")), .Autorizado = CBool(r("AUTORIZADO")), .Nombre = CStr(r("NOMBRE")),
                                  .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function getObjectLast(ByVal idSab As Integer) As ELL.Permiso
            Dim query As String = "SELECT * FROM VPERMISO_ACTUAL WHERE ID_SAB=:ID_SAB"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Permiso)(Function(r As OracleDataReader) _
            New ELL.Permiso With {.Id = CInt(r("ID")), .IdSab = CInt(r("ID_SAB")), .FechaSolicitud = CDate(r("FECHA_SOLICITUD")), .IdSabResponsable = SabLib.BLL.Utils.integerNull(r("ID_SAB_RESPONSABLE")),
                                  .FechaRespuesta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_RESPUESTA")), .Autorizado = CBool(r("AUTORIZADO")), .Nombre = CStr(r("NOMBRE")),
                                  .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL"))}, query, CadenaConexion, New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function loadListToAuthorize(ByVal idSab As Integer) As List(Of ELL.Permiso)
            Dim query As String = "SELECT * FROM VPERMISOS WHERE ID_SAB_RESPONSABLE=:ID_SAB_RESPONSABLE"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Permiso)(Function(r As OracleDataReader) _
            New ELL.Permiso With {.Id = CInt(r("ID")), .IdSab = CInt(r("ID_SAB")), .FechaSolicitud = CDate(r("FECHA_SOLICITUD")), .IdSabResponsable = SabLib.BLL.Utils.integerNull(r("ID_SAB_RESPONSABLE")),
                                  .FechaRespuesta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_RESPUESTA")), .Autorizado = CBool(r("AUTORIZADO")), .Nombre = CStr(r("NOMBRE")),
                                  .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL"))}, query, CadenaConexion, New OracleParameter("ID_SAB_RESPONSABLE", OracleDbType.Int32, idSab, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function loadList() As List(Of ELL.Permiso)
            Dim query As String = "SELECT * FROM VPERMISOS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Permiso)(Function(r As OracleDataReader) _
            New ELL.Permiso With {.Id = CInt(r("ID")), .IdSab = CInt(r("ID_SAB")), .FechaSolicitud = CDate(r("FECHA_SOLICITUD")), .IdSabResponsable = SabLib.BLL.Utils.integerNull(r("ID_SAB_RESPONSABLE")),
                                  .FechaRespuesta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_RESPUESTA")), .Autorizado = CBool(r("AUTORIZADO")), .Nombre = CStr(r("NOMBRE")),
                                  .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL"))}, query, CadenaConexion)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="permiso"></param>
        Public Shared Sub Save(ByVal permiso As ELL.Permiso)
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, permiso.IdSab, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_SAB_RESPONSABLE", OracleDbType.Int32, permiso.IdSabResponsable, ParameterDirection.Input))

            Dim query As String = "INSERT INTO PERMISO (ID_SAB, ID_SAB_RESPONSABLE) VALUES (:ID_SAB, :ID_SAB_RESPONSABLE)"

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="permiso"></param>
        Public Shared Sub Authorize(ByVal permiso As ELL.Permiso)
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, permiso.Id, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("FECHA_RESPUESTA", OracleDbType.Date, permiso.FechaRespuesta, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("AUTORIZADO", OracleDbType.Int32, permiso.Autorizado, ParameterDirection.Input))

            Dim query As String = "UPDATE PERMISO SET FECHA_RESPUESTA=:FECHA_RESPUESTA, AUTORIZADO=:AUTORIZADO WHERE ID=:ID"

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

#End Region

    End Class

End Namespace