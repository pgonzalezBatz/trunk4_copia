Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ProyectosPTKSisDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList(ByVal texto As String) As List(Of ELL.Proyectos)
            Dim query As String = "SELECT ID, NOMBRE, PROGRAMA FROM PROYECTOS WHERE LOWER(NOMBRE) LIKE '%' || :TEXTO || '%'  ORDER BY NOMBRE ASC"
            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 20, texto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Programa = SabLib.BLL.Utils.stringNull(r("PROGRAMA"))}, query, CadenaConexionBonoSis, parameter)
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectoPorId(ByVal idProyecto As String) As ELL.Proyectos
            Dim query As String = "SELECT ID, NOMBRE, PROGRAMA FROM PROYECTOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.NVarchar2, 30, idProyecto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Programa = SabLib.BLL.Utils.stringNull(r("PROGRAMA"))}, query, CadenaConexionBonoSis, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectoPorPrograma(ByVal programa As String) As ELL.Proyectos
            Dim query As String = "SELECT ID, NOMBRE, PROGRAMA FROM PROYECTOS WHERE PROGRAMA=:PROGRAMA"
            Dim parameter As New OracleParameter("PROGRAMA", OracleDbType.NVarchar2, programa, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Programa = SabLib.BLL.Utils.stringNull(r("PROGRAMA"))}, query, CadenaConexionBonoSis, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyecto(ByVal texto As String) As ELL.Proyectos
            Dim query As String = "SELECT ID, NOMBRE, PROGRAMA FROM PROYECTOS WHERE LOWER(NOMBRE)=:TEXTO"
            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 50, texto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Programa = SabLib.BLL.Utils.stringNull(r("PROGRAMA"))}, query, CadenaConexionBonoSis, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectosPTKSis(ByVal texto As String) As List(Of ELL.Objeto)
            Dim query As String = "SELECT ID, PROGRAMA, NOMBRE FROM PROYECTOS WHERE LOWER(NOMBRE) LIKE '%' || :TEXTO || '%' AND ESTADO IS NOT NULL AND ACTIVO=1 AND LOWER(ESTADO) NOT LIKE '%archived%' AND LOWER(ESTADO) NOT LIKE '%rfq%' ORDER BY FECHA_REGISTRO DESC"
            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 20, texto.ToLower, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Objeto)(Function(r As OracleDataReader) _
            New ELL.Objeto With {.Id = SabLib.BLL.Utils.stringNull(r("PROGRAMA")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdPtksis = SabLib.BLL.Utils.stringNull(r("ID"))}, query, CadenaConexionBonoSis, parameter)
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectosPTKSis() As List(Of ELL.Objeto)
            Dim query As String = "SELECT ID, PROGRAMA, NOMBRE FROM PROYECTOS WHERE ESTADO IS NOT NULL AND ACTIVO=1 AND LOWER(ESTADO) NOT LIKE '%archived%' " &
                                  "AND LOWER(ESTADO) NOT LIKE '%rfq%' ORDER BY FECHA_REGISTRO DESC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Objeto)(Function(r As OracleDataReader) _
            New ELL.Objeto With {.Id = SabLib.BLL.Utils.stringNull(r("PROGRAMA")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdPtksis = SabLib.BLL.Utils.stringNull(r("ID"))}, query, CadenaConexionBonoSis)
        End Function

#End Region

    End Class

End Namespace