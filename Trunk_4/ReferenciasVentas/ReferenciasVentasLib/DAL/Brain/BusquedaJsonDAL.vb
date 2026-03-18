Imports System.Data.OracleClient
Imports Oracle.ManagedDataAccess.Client

Namespace DAL
    Public Class BusquedaJsonDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Carga la primera OF que coincida con el filtro
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function CargarPrevBatzPN(ByVal refPieza As String) As ELL.MaestroPiezasBrainResumen
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = String.Empty

            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim datosBrain As New ELL.MaestroPiezasBrainResumen

            Try
                query = "SELECT * FROM X500PRDSD.TEIL WHERE (LOWER(TEZINR)='" & refPieza.ToLower & "' OR LOWER(TETENR)='" & refPieza.ToLower & "') "

                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                If oReader.Read Then
                    datosBrain.Planta = oReader.Item("TEWKNR")
                    datosBrain.RefPieza = oReader.Item("TETENR")
                    datosBrain.RefClientePlanoBatz = oReader.Item("TEZINR")
                    datosBrain.NivelIngenieria = oReader.Item("TEASBZ")
                    datosBrain.PlanoWeb = oReader.Item("TERHF4")
                    datosBrain.IdCustomerProject = oReader.Item("TERHF5")
                End If
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return datosBrain
        End Function

        '''' <summary>
        '''' Carga la primera OF que coincida con el filtro
        '''' </summary>        
        '''' <returns></returns>
        '''' <remarks></remarks>        
        'Public Shared Function CargarPrevBatzPN(ByVal refPieza As String, ByVal plantas As String) As ELL.MaestroPiezasBrainResumen
        '    Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        '    Dim cn As New OleDb.OleDbConnection(conexion)
        '    Dim query As String = String.Empty

        '    Dim oReader As OleDb.OleDbDataReader = Nothing
        '    Dim datosBrain As New ELL.MaestroPiezasBrainResumen

        '    Try
        '        Dim plantasSeleccionadas As String() = plantas.Split(",")
        '        If (plantasSeleccionadas.Count > 0) Then
        '            query = "SELECT * FROM X500PRDSD.TEIL WHERE (LOWER(TEZINR)='" & refPieza.ToLower & "' OR LOWER(TETENR)='" & refPieza.ToLower & "') "
        '            query &= "AND TEFIRM IN ("
        '            For Each planta In plantasSeleccionadas
        '                query &= "'" & planta & "',"
        '            Next
        '            query = query.Substring(0, query.Length - 1)
        '            query &= ")"

        '            Dim cm As New OleDb.OleDbCommand(query, cn)
        '            cm.CommandTimeout = 30
        '            cn.Open()
        '            oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        '            If oReader.Read Then
        '                datosBrain.Planta = oReader.Item("TEWKNR")
        '                datosBrain.RefPieza = oReader.Item("TETENR")
        '                datosBrain.RefClientePlanoBatz = oReader.Item("TEZINR")
        '                datosBrain.NivelIngenieria = oReader.Item("TEASBZ")
        '                datosBrain.PlanoWeb = oReader.Item("TERHF4")
        '                datosBrain.IdCustomerProject = oReader.Item("TERHF5")
        '            End If
        '        End If
        '    Catch
        '    Finally
        '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
        '    End Try

        '    Return datosBrain
        'End Function

        ''' <summary>
        ''' Carga la primera OF que coincida con el filtro
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function CargarReferenciaPieza(ByVal refPieza As String) As ELL.BusquedaJson
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT COUNT(*) as NUMERO FROM CUBOS.SOLICIPZA WHERE LOWER(SPTERN)='" & refPieza & "'"
            Dim referencia As New ELL.BusquedaJson
            referencia.Pieza = "0"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    referencia.Pieza = oReader.Item("NUMERO")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return referencia
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function CargarProyectoBrain(ByVal idProyecto As String) As ELL.Proyectos
            Dim nombre As String = String.Empty
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString

            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT ELTO, DENO_S FROM CUBOS.T_AVN WHERE ELTO='" & idProyecto & "' AND EMPRESA='1' AND PLANTA='000'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim proyecto As New ELL.Proyectos
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                If oReader.Read Then
                    proyecto.Id = oReader.Item("ELTO").ToString
                    proyecto.Nombre = oReader.Item("DENO_S").ToString
                End If
            Catch
                'Añadir al log
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try
            Return proyecto
        End Function

        ''' <summary>
        ''' Verifica si existe una referencia de cliente guardada en Brain para las plantas seleccionadas
        ''' </summary>
        ''' <param name="refCliente">Referencia de cliente</param>
        ''' <param name="plantas">Plantas Seleccionadas</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarCustomerPN(ByVal refCliente As String, ByVal plantas As String) As ELL.BusquedaJson
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim numero As Integer = 0
            Dim query As String = String.Empty
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim referencia As New ELL.BusquedaJson

            Dim plantasSeleccionadas As String() = plantas.Split(",")
            If (plantasSeleccionadas.Count > 0) Then
                query = "SELECT * FROM X500PRDSD.TEIL WHERE LOWER(TEZINR)='" & refCliente.ToLower & "' "
                query &= "AND TEFIRM IN("
                For Each planta In plantasSeleccionadas
                    query &= "'" & planta & "',"
                Next
                query = query.Substring(0, query.Length - 1)
                query &= ")"

                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                Dim oReader As OleDb.OleDbDataReader = Nothing

                Try
                    cn.Open()
                    oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                    If oReader.Read Then
                        referencia.Pieza = oReader.Item("TETENR")
                    End If
                Catch
                Finally
                    If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
                End Try
            End If

            Return referencia
        End Function

#End Region

    End Class

End Namespace