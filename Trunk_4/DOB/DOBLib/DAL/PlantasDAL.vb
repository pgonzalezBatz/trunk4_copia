Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class PlantasDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getPlanta(ByVal idPlanta As Integer) As ELL.Planta
            Dim query As String = "SELECT * FROM VPLANTAS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Planta)(Function(r As OracleDataReader) _
            New ELL.Planta With {.Id = CInt(r("ID")), .Planta = CStr(r("PLANTA")), .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")),
                                 .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .HeredaRetos = CBool(r("HEREDA_RETOS")),
                                 .PlantaPadre = SabLib.BLL.Utils.stringNull(r("PLANTA_PADRE")), .IdPlantaPadre = SabLib.BLL.Utils.integerNull(r("ID_PLANTA_PADRE"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de plantas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.Planta)
            Dim query As String = "SELECT * FROM VPLANTAS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Planta)(Function(r As OracleDataReader) _
            New ELL.Planta With {.Id = CInt(r("ID")), .Planta = CStr(r("PLANTA")), .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")),
                                 .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .HeredaRetos = CBool(r("HEREDA_RETOS")),
                                 .PlantaPadre = SabLib.BLL.Utils.stringNull(r("PLANTA_PADRE")), .IdPlantaPadre = SabLib.BLL.Utils.integerNull(r("ID_PLANTA_PADRE"))}, query, CadenaConexion, Nothing)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Agrega una planta
        ''' </summary>
        ''' <param name="planta"></param> 
        Public Shared Sub AddPlanta(ByVal planta As ELL.Planta)
            Dim query As String = "INSERT INTO PLANTA (NOMBRE, ID_PLANTA_PADRE, HEREDA_RETOS) VALUES (:NOMBRE, :ID_PLANTA_PADRE, :HEREDA_RETOS)"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, planta.Planta, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA_PADRE", OracleDbType.Int32, If(planta.IdPlantaPadre = Integer.MinValue, DBNull.Value, planta.IdPlantaPadre), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("HEREDA_RETOS", OracleDbType.Int32, planta.HeredaRetos, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM PLANTA WHERE ID=:ID"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        End Sub

        ''' <summary>
        ''' Marca como eliminado un objetivo
        ''' </summary>
        ''' <param name="planta"></param>
        ''' <param name="baja"></param>
        Public Shared Sub MarkDeleted(ByVal planta As ELL.Planta, ByVal baja As Boolean)
            Dim query As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, planta.Id, ParameterDirection.Input))

            If (baja) Then
                lParameters.Add(New OracleParameter("FECHA_BAJA", OracleDbType.Date, planta.FechaBaja, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_USUARIO_BAJA", OracleDbType.Int32, planta.IdUsuarioBaja, ParameterDirection.Input))
                query = "UPDATE PLANTA SET FECHA_BAJA=:FECHA_BAJA, ID_USUARIO_BAJA=:ID_USUARIO_BAJA WHERE ID=:ID"
            Else
                query = "UPDATE PLANTA SET FECHA_BAJA=NULL, ID_USUARIO_BAJA=NULL WHERE ID=:ID"
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace