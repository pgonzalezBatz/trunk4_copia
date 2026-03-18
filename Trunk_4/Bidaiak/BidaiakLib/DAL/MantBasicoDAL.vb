Imports Oracle.DataAccess.Client

Namespace DAL

    Public Class MantBasicoDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter
        Private mTipo As BLL.IMantBasico.eTipo = BLL.IMantBasico.eTipo.SERV_AGENCIA
        Private mIdPlanta As Integer = 0

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        '''<param name="tipo">Indica la clase que representa para saber contra que tabla tiene que operar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        Sub New(ByVal tipo As BLL.IMantBasico.eTipo, ByVal idPlanta As Integer)
            Dim status As String = "BIDAIAKTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            mTipo = tipo
            mIdPlanta = idPlanta
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un objeto de mantenimiento basico
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.MantBasico
            Try
                Dim query As String = "SELECT ID,NOMBRE,DESCRIPCION,OBSOLETO,ID_PLANTA FROM " & [Enum].GetName(GetType(BLL.IMantBasico.eTipo), mTipo)
                query &= " WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Dim lMant As List(Of ELL.MantBasico) = Memcached.OracleDirectAccess.Seleccionar(Of ELL.MantBasico)(Function(r As OracleDataReader) _
                 New ELL.MantBasico With {.Id = CInt(r(0)), .Nombre = r(1), .Descripcion = Sablib.BLL.Utils.stringNull(r(2)), .Obsoleto = CInt(r(3)), .IdPlanta = CInt(r(4))}, query, cn, parameter)

                Dim oMantBasico As ELL.MantBasico = Nothing
                If (lMant IsNot Nothing AndAlso lMant.Count > 0) Then oMantBasico = lMant.Item(0)
                Return oMantBasico
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de  objetos mantenimiento
        ''' </summary>
        ''' <param name="bVigentes">Parametro opcional que indica si se obtendran todos o solo los vigentes</param>
        ''' <returns></returns>        
        Public Function loadList(Optional ByVal bVigentes As Boolean = False) As List(Of ELL.MantBasico)
            Try
                Dim query As String = "SELECT ID,NOMBRE,DESCRIPCION,OBSOLETO,ID_PLANTA FROM " & [Enum].GetName(GetType(BLL.IMantBasico.eTipo), mTipo) _
                                    & "WHERE ID_PLANTA=:ID_PLANTA"
                If (bVigentes) Then query &= " WHERE OBSOLETO=0"
                parameter = New OracleParameter("ID_PLANTA", OracleDbType.Int32, mIdPlanta, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.MantBasico)(Function(r As OracleDataReader) _
                 New ELL.MantBasico With {.Id = CInt(r(0)), .Nombre = r(1), .Descripcion = Sablib.BLL.Utils.stringNull(r(2)), .Obsoleto = CInt(r(3)), .IdPlanta = CInt(r(4))}, query, cn, Nothing)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el objeto mantBasico
        ''' </summary>
        ''' <param name="oMantB">Objeto con la informacion</param>        
        Public Sub Save(ByVal oMantB As ELL.MantBasico)
            Try
                Dim nombreTabla As String = [Enum].GetName(GetType(BLL.IMantBasico.eTipo), mTipo)
                Dim query As String = String.Empty
                Dim index As Integer = 3
                If (oMantB.Id <> Integer.MinValue) Then index = 4
                Dim parameters(index) As OracleParameter
                parameters(0) = New OracleParameter(":NOMBRE", OracleDbType.Varchar2, oMantB.Nombre, ParameterDirection.Input)
                parameters(1) = New OracleParameter(":DESCRIPCION", OracleDbType.Varchar2, oMantB.Descripcion, ParameterDirection.Input)
                parameters(2) = New OracleParameter(":OBSOLETO", OracleDbType.Int32, oMantB.Obsoleto, ParameterDirection.Input)
                parameters(3) = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, oMantB.IdPlanta, ParameterDirection.Input)

                If (oMantB.Id = Integer.MinValue) Then 'Insert
                    query = "INSERT INTO " & nombreTabla & " (NOMBRE,DESCRIPCION,OBSOLETO,ID_PLANTA) VALUES(:NOMBRE,:DESCRIPCION,:OBSOLETO,:ID_PLANTA)"
                Else 'update
                    query = "UPDATE " & nombreTabla & " SET NOMBRE=:NOMBRE,DESCRIPCION=:DESCRIPCION,OBSOLETO=:OBSOLETO,ID_PLANTA=:ID_PLANTA WHERE ID=:ID"
                    parameters(4) = New OracleParameter(":ID", OracleDbType.Int32, oMantB.Id, ParameterDirection.Input)
                End If
                Memcached.OracleDirectAccess.NoQuery(query, cn, parameters)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la informacion", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Marca como obsoleto un objeto
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            Try
                Dim query As String = "UPDATE " & [Enum].GetName(GetType(BLL.IMantBasico.eTipo), mTipo) & " SET OBSOLETO=1 WHERE ID=:ID"
                parameter = New OracleParameter(":ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la informacion", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace