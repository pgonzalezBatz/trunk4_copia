Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class SolicitudesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.Solicitud
            Dim query As String = "SELECT * FROM SOLICITUD WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitud)(Function(r As OracleDataReader) _
            New ELL.Solicitud With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Puesto = CStr(r("PUESTO")),
                                    .Movil = SabLib.BLL.Utils.stringNull(r("MOVIL")), .Direccion = CStr(r("DIRECCION")), .Fijo = CStr(r("FIJO")),
                                    .Email = CStr(r("EMAIL")), .FechaAlta = CDate(r("FECHA_ALTA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                    .Cajas = CInt(r("CAJAS")), .IdNegocio = CStr(r("ID_NEGOCIO"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function loadList() As List(Of ELL.Solicitud)
            Dim query As String = "SELECT * FROM SOLICITUD"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitud)(Function(r As OracleDataReader) _
            New ELL.Solicitud With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Puesto = CStr(r("PUESTO")),
                                    .Movil = SabLib.BLL.Utils.stringNull(r("MOVIL")), .Direccion = CStr(r("DIRECCION")), .Fijo = CStr(r("FIJO")),
                                    .Email = CStr(r("EMAIL")), .FechaAlta = CDate(r("FECHA_ALTA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                    .Cajas = CInt(r("CAJAS")), .IdNegocio = CStr(r("ID_NEGOCIO"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function loadListByIdSab(ByVal idSab As Integer) As List(Of ELL.Solicitud)
            Dim query As String = "SELECT * FROM SOLICITUD WHERE ID_USUARIO_ALTA=:ID_USUARIO_ALTA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitud)(Function(r As OracleDataReader) _
            New ELL.Solicitud With {.Id = CInt(r("ID")), .Nombre = CStr(r("NOMBRE")), .Puesto = CStr(r("PUESTO")),
                                    .Movil = SabLib.BLL.Utils.stringNull(r("MOVIL")), .Direccion = CStr(r("DIRECCION")), .Fijo = CStr(r("FIJO")),
                                    .Email = CStr(r("EMAIL")), .FechaAlta = CDate(r("FECHA_ALTA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                    .Cajas = CInt(r("CAJAS")), .IdNegocio = CStr(r("ID_NEGOCIO"))}, query, CadenaConexion, New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, idSab, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="solicitud"></param>
        Public Shared Sub Save(ByVal solicitud As ELL.Solicitud)
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, solicitud.Nombre, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PUESTO", OracleDbType.NVarchar2, solicitud.Puesto, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("MOVIL", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(solicitud.Movil), DBNull.Value, solicitud.Movil), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("DIRECCION", OracleDbType.NVarchar2, solicitud.Direccion, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("FIJO", OracleDbType.NVarchar2, solicitud.Fijo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("EMAIL", OracleDbType.NVarchar2, solicitud.Email, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, solicitud.IdUsuarioAlta, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("CAJAS", OracleDbType.Int32, solicitud.Cajas, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.NVarchar2, solicitud.IdNegocio, ParameterDirection.Input))

            Dim query As String = "INSERT INTO SOLICITUD (NOMBRE, PUESTO, MOVIL, DIRECCION, FIJO, EMAIL, ID_USUARIO_ALTA, CAJAS, ID_NEGOCIO) VALUES (:NOMBRE, :PUESTO, :MOVIL, :DIRECCION, :FIJO, :EMAIL, :ID_USUARIO_ALTA, :CAJAS, :ID_NEGOCIO)"

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

#End Region

    End Class

End Namespace