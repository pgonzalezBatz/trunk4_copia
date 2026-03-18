Imports Oracle.DataAccess.Client

Namespace DAL

    Public Class PlantasNegociosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una planta/negocio
        ''' </summary>
        ''' <param name="idPlantaNegocio"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getPlantaNegocio(ByVal idPlantaNegocio As Integer) As ELL.PlantaNegocio
            Dim query As String = "SELECT * FROM VPLANTAS_NEGOCIOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idPlantaNegocio, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PlantaNegocio)(Function(r As OracleDataReader) _
            New ELL.PlantaNegocio With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")),
                                        .IdNegocio = CInt(r("ID_NEGOCIO")), .Negocio = CStr(r("NEGOCIO"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de plantas/negocios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.PlantaNegocio)
            Dim query As String = "SELECT * FROM VPLANTAS_NEGOCIOS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PlantaNegocio)(Function(r As OracleDataReader) _
            New ELL.PlantaNegocio With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")),
                                        .IdNegocio = CInt(r("ID_NEGOCIO")), .Negocio = CStr(r("NEGOCIO"))}, query, CadenaConexion)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta una planta/negocio
        ''' </summary>
        ''' <param name="oObjeto"></param> 
        Public Shared Function SavePlantaNegocio(ByVal oObjeto As ELL.PlantaNegocio) As Integer
            Dim query As String = "INSERT INTO PLANTA_NEGOCIO (ID_PLANTA, ID_NEGOCIO) " _
                                  & "VALUES (:ID_PLANTA, :ID_NEGOCIO) RETURNING ID INTO :RETURN_VALUE"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oObjeto.IdPlanta, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, oObjeto.IdNegocio, ParameterDirection.Input))

            Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
            p.DbType = DbType.Int32
            lParameters.Add(p)

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            Return lParameters.Last.Value
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta negocio y gerente
        ''' </summary>
        ''' <param name="idNegocio"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function existsPlantaNegocio(ByVal idNegocio As Integer, ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM PLANTA_NEGOCIO WHERE ID_NEGOCIO=:ID_NEGOCIO AND ID_PLANTA=:ID_PLANTA"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, idNegocio, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
            Return filas > 0
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsPlanta(ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM PLANTA_NEGOCIO WHERE ID_PLANTA=:ID_PLANTA"
            Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idNegocio"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsNegocio(ByVal idNegocio As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM PLANTA_NEGOCIO WHERE ID_NEGOCIO=:ID_NEGOCIO"
            Dim parameter As New OracleParameter("ID_NEGOCIO", OracleDbType.Int32, idNegocio, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM PLANTA_NEGOCIO WHERE ID=:ID"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        End Sub

#End Region

    End Class

End Namespace