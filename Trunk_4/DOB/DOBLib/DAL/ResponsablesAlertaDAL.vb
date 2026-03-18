Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ResponsablesAlertaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de responsables sin el indicador informado para el mes actual
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.ResponsableAlerta)
            Dim query As String = "SELECT * FROM VRESPONSABLES_ALERTA_INDICADOR"

            Dim listaResponsablesAlerta As List(Of ELL.ResponsableAlerta) = Memcached.OracleDirectAccess.seleccionar(Of ELL.ResponsableAlerta)(Function(r As OracleDataReader) _
            New ELL.ResponsableAlerta With {.IdResponsable = CInt(r("ID_RESPONSABLE")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL")),
                                            .Cultura = CStr(r("IDCULTURAS")), .Responsable = CStr(r("RESPONSABLE"))}, query, CadenaConexion)

            Return listaResponsablesAlerta
        End Function

        ''' <summary>
        ''' Obtiene un listado de responsables para revisión
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function loadListRevision(ByVal ejercicio As Integer) As List(Of ELL.ResponsableAlerta)
            Dim query As String = "SELECT * FROM VRESPONSABLES_ALERTA_REVISION WHERE AÑO_OBJETIVO=:AÑO_OBJETIVO"

            Dim listaResponsablesAlerta As List(Of ELL.ResponsableAlerta) = Memcached.OracleDirectAccess.seleccionar(Of ELL.ResponsableAlerta)(Function(r As OracleDataReader) _
            New ELL.ResponsableAlerta With {.IdResponsable = CInt(r("ID_RESPONSABLE")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL")),
                                            .Cultura = CStr(r("IDCULTURAS")), .Responsable = CStr(r("RESPONSABLE")), .AñoObjetivo = CInt(r("AÑO_OBJETIVO"))}, query, CadenaConexion)

            Return listaResponsablesAlerta
        End Function

#End Region

    End Class

End Namespace