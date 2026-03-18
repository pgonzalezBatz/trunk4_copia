Imports Oracle.DataAccess.Client

Public Class colasinterop
    Const strCnColas As String = "Data Source=xbat;User Id=ilarak;Password=ilarak12;Connection LifeTime=60;"
    Private IdProcesoCatia As Integer
    Private PathCodigoCatia As String = "\\hpnas2.batz.es\catiav5cfg\codigos\CATIAV5\64bits\B22SP03HF30"

    Private Enum estadoCola
        inicio = 1
        ejecucion = 3
        preparado = 6
        finalizado = 4
    End Enum


    Public Shared Function GetElementosEjecucion() As List(Of Object)
        Dim q1 = "select p.id,RUTA_FICHERO_BASE, NOMBRE_FICHERO,descripcion  from procesos p where p.id_estado=:id_estado and id_maquina=226 and id_cola=256 and id_software=253"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_estado", OracleDbType.Int32, estadoCola.ejecucion, ParameterDirection.Input))
        Return OracleDirectAccess.Seleccionar(Of Object)(Function(r) New With {.id = r(0), .idOut = r(1), .fileName = r(2), .idIn = r(3)}, q1, strCnColas, lstp.ToArray)
    End Function
    Public Shared Function GetDatosDesdeColaYOcuparProceso() As Object
        Dim q1 = "select p.id,RUTA_FICHERO_BASE, NOMBRE_FICHERO, descripcion,nombre_usuario_corto,log_script from procesos  p where p.id_estado=:id_estado and id_maquina=226 and id_cola=256 and id_software=253"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("id_estado", OracleDbType.Int32, estadoCola.preparado, ParameterDirection.Input))
        Dim q2 = "update procesos set id_estado=3, fecha_comienzo=sysdate where id=:id"
        Dim connect As New OracleConnection(strCnColas)
        connect.Open()
        Dim trasact As OracleTransaction = connect.BeginTransaction()
        Try
            Dim lst = OracleDirectAccess.seleccionar(Of Object)(Function(r) New With {.id = r(0).ToString.Trim, .idOut = r(1).ToString.Trim, .fileName = r(2).ToString.Trim, .idIn = r(3).ToString.Trim,
                                                                .NombreUsuarioCorto = r("nombre_usuario_corto").ToString.Trim, .logScript = r("log_script").ToString.Trim}, q1, connect, lstp1.ToArray)
            If lst.Count = 0 Then
                trasact.Commit()
                connect.Close()
                Return Nothing
            End If
            OracleDirectAccess.NoQuery(q2, connect, New OracleParameter("id", OracleDbType.Int32, lst.First().id, ParameterDirection.Input))
            trasact.Commit()
            connect.Close()
            Return lst.First()
        Catch ex As Exception
            trasact.Rollback()
            connect.Close()
            Throw ex
        End Try
    End Function
    Public Shared Function GetDatosDesdeCola(id As Integer) As Object
        Dim q1 = "select p.id,RUTA_FICHERO_BASE, NOMBRE_FICHERO, descripcion,nombre_usuario_corto,log_script from procesos  p where p.id=:id and id_cola=256 and id_software=253"
        Dim lstp1 As New List(Of OracleParameter)
        lstp1.Add(New OracleParameter("id", OracleDbType.Int32, id, ParameterDirection.Input))
        Dim lst = OracleDirectAccess.seleccionar(Of Object)(Function(r) New With {.id = r(0).ToString.Trim, .idOut = r(1).ToString.Trim, .fileName = r(2).ToString.Trim, .idIn = r(3).ToString.Trim,
                                                            .NombreUsuarioCorto = r("nombre_usuario_corto").ToString.Trim, .logScript = r("log_script").ToString.Trim}, q1, strCnColas, lstp1.ToArray)
        If lst.Count = 0 Then
            Return Nothing
        End If
        Return lst.First()
    End Function
    Public Shared Sub updateProcesoSetFinalizado(idProceso As Integer, errorMesasge As String)
        Dim q1 = "update procesos set id_estado=:id_estado,  id_maquina=null, fecha_fin=sysdate, posicion_cola=null,log_script=:log_script where id=:id"
        Dim lstp As New List(Of OracleParameter)
        lstp.Add(New OracleParameter("id_estado", OracleDbType.Int32, estadoCola.finalizado, ParameterDirection.Input))
        lstp.Add(New OracleParameter("log_script", OracleDbType.Varchar2, errorMesasge, ParameterDirection.Input))
        lstp.Last.Size = 2000
        lstp.Add(New OracleParameter("id", OracleDbType.Int32, idProceso, ParameterDirection.Input))
        OracleDirectAccess.NoQuery(q1, strCnColas, lstp.ToArray)
    End Sub
End Class
