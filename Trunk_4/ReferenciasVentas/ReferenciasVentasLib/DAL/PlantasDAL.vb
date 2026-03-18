Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class PlantasDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todas las plantas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList() As List(Of ELL.Plantas)
            Dim query As String = "SELECT ID, ID_BRAIN, NOMBRE FROM PLANTAS WHERE ID_BRAIN IS NOT NULL ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Plantas)(Function(r As OracleDataReader) _
            New ELL.Plantas With {.Id = CInt(r("ID")), .Codigo = SabLib.BLL.Utils.stringNull(r("ID_BRAIN")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, CadenaConexionSAB)
        End Function

        ''' <summary>
        ''' Obtiene una planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarPlanta(ByVal codigo As String) As ELL.Plantas
            Dim query As String = "SELECT ID, ID_BRAIN, NOMBRE FROM PLANTAS WHERE ID_BRAIN=:ID_BRAIN"

            Dim parameter As New OracleParameter("ID_BRAIN", OracleDbType.NVarchar2, 50, codigo, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Plantas)(Function(r As OracleDataReader) _
            New ELL.Plantas With {.Id = CInt(r("ID")), .Codigo = SabLib.BLL.Utils.stringNull(r("ID_BRAIN")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, CadenaConexionSAB, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene una planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarPlanta(ByVal idPlanta As Integer) As ELL.Plantas
            Dim query As String = "SELECT ID, ID_BRAIN, NOMBRE FROM PLANTAS WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Plantas)(Function(r As OracleDataReader) _
            New ELL.Plantas With {.Id = CInt(r("ID")), .Codigo = SabLib.BLL.Utils.stringNull(r("ID_BRAIN")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, CadenaConexionSAB, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="planta">Objeto Plantas</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existe(ByVal planta As ELL.Plantas) As Integer
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                Dim query As String = "SELECT COUNT(*) FROM PLANTAS WHERE LOWER(NOMBRE)=:NOMBRE AND LOWER(CODIGO)=:CODIGO"
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 50, planta.Nombre.ToLower, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("CODIGO", OracleDbType.Varchar2, 2, planta.Codigo.ToLower, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexionSAB, lParameters1.ToArray)
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ' ''' <summary>
        ' ''' Guardar un nuevo registro
        ' ''' </summary>
        ' ''' <param name="planta">Objeto Plantas</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function Save(ByVal planta As ELL.Plantas) As Boolean
        '    Dim resultado As Boolean = False
        '    Dim query As String = String.Empty
        '    Dim lParameters1 As New List(Of OracleParameter)

        '    Try
        '        query = "INSERT INTO PLANTAS(CODIGO, NOMBRE) VALUES(:CODIGO, :NOMBRE)"
        '        lParameters1.Add(New OracleParameter("CODIGO", OracleDbType.Varchar2, 2, planta.Codigo, ParameterDirection.Input))
        '        lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 50, planta.Nombre, ParameterDirection.Input))
        '        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

        '        Return True
        '    Catch ex As Exception
        '        Return False
        '    End Try
        'End Function

        ' ''' <summary>
        ' ''' Modifica los datos de un registro
        ' ''' </summary>
        ' ''' <param name="planta">Objeto Plantas</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function Update(ByVal planta As ELL.Plantas) As Boolean
        '    Dim resultado As Boolean = False
        '    Try
        '        Dim query As String = String.Empty
        '        Dim lParameters1 As New List(Of OracleParameter)

        '        query = "UPDATE PLANTAS SET CODIGO=:CODIGO, NOMBRE=:NOMBRE WHERE ID=:ID"
        '        lParameters1.Add(New OracleParameter("CODIGO", OracleDbType.Varchar2, 2, planta.Codigo, ParameterDirection.Input))
        '        lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 20, planta.Nombre, ParameterDirection.Input))
        '        lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, planta.Id, ParameterDirection.Input))

        '        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

        '        Return True
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        '    Return resultado
        'End Function

        ' ''' <summary>
        ' ''' Elimina un registro
        ' ''' </summary>
        ' ''' <param name="idPlanta">Identificador DrivingHand</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function Delete(ByVal idPlanta As Integer) As Boolean
        '    Dim query As String = String.Empty

        '    Try
        '        query = "DELETE FROM PLANTAS WHERE ID=:ID"
        '        Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
        '        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, parameter)

        '        Return True
        '    Catch ex As Exception
        '        Return False
        '    End Try
        'End Function

#End Region

    End Class

End Namespace