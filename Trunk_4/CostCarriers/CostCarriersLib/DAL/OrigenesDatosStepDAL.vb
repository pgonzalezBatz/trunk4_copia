Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class OrigenesDatosStepDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un origen de datos para el step
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getOrigenDatosStep(ByVal id As Integer) As ELL.OrigenDatosStep
            Dim query As String = "SELECT * FROM ORIGEN_DATOS_STEP WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.OrigenDatosStep)(Function(r As OracleDataReader) _
            New ELL.OrigenDatosStep With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION"))},
                                          query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene todas los origenes de datos del step
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.OrigenDatosStep)
            Dim query As String = "SELECT * FROM ORIGEN_DATOS_STEP"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.OrigenDatosStep)(Function(r As OracleDataReader) _
            New ELL.OrigenDatosStep With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION"))},
                                          query, CadenaConexion)
        End Function

#End Region

    End Class

End Namespace