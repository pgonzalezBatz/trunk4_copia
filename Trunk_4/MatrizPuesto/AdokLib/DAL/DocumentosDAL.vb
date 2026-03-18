Imports Oracle.DataAccess.Client
Imports System.Configuration
Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class DocumentosDAL
        Inherits DALBase

#Region "Consultas"



        Public Function CargarCodusuario(ByVal mail As String) As List(Of ELL.CEtico)

            Dim query As String = "select codpersona from usuarios  WHERE (fechabaja is  null or fechabaja > sysdate) and email = '" & mail & "'"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                             New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("codpersona"))}, query, CadenaConexionSAB)


        End Function


        ''' <summary>
        ''' Obtiene todos los tipos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposEmpresaCIFADOK(ByVal cif As String) As List(Of ELL.Empresas)

            Dim query As String = "Select emp000 FROM adok_emp  WHERE UPPER(emp001)='" & Trim(cif).ToUpper & "'"

            Return Memcached.SQLServerDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As SqlClient.SqlDataReader) _
            New ELL.Empresas With {.Id = CInt(r("emp000"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadTrabajadores(ByVal empresa As Integer) As List(Of ELL.Trabajadores)
            'Dim param1 As SqlClient.SqlParameter
            Dim query As String = "select tra000 from adok_tra where tra004 = " & empresa

            'Return Memcached.SQLServerDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As SqlClient.SqlDataReader) _
            '      New ELL.Trabajadores With {.Id = CInt(r("tra000"))}, query, CadenaConexion)
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000"))}, query, CadenaConexion)

        End Function


        ''' <summary>
        ''' Comprueba EmpresasActivas
        ''' </summary>
        ''' <returns>Lista de las Empresas Activas</returns>    
        Public Function EmpresasActivas(ByVal plantaAdmin As Integer) As List(Of String())

            Dim sql As String = " select emp000, emp099  from adok_emp where emp018 = 0 " ' and emp099 = " & plantaAdmin
            'CadenaConexion  "Data Source=GARAPEN;User Id=adoknet;Password=adoknet12;Connection LifeTime=300;"
            ' Dim sql As String = "select tra004, tra003 from adok_tra where tra004 = 10167 and tra006 > TRUNC(SYSDATE)-60 and tra012 = 0;"
            Return Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)
        End Function
        Public Function AvisoTrabajadores(ByVal plantaAdmin As Integer) As List(Of String())

            Dim sql As String = "select tra000, tra004, tra003, tra002, tra099, tra006 from adok_tra where  tra006 < TRUNC(SYSDATE)+15 and tra006 > sysdate  and tra012 <> 1 " ' and emp099 = " & plantaAdmin

            Return Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)
        End Function



        ''' <summary>
        ''' Se cierran todas las solicitudes especificadas
        ''' </summary>
        ''' <param name="lSolicitudes"></param>    
        Public Function ActualizarEmpresaInactivas(ByVal lSolicitudes As List(Of String())) As Boolean
            '   Dim con As Oracle.DataAccess.Client.OracleConnection = Nothing
            '  Dim transact As Oracle.DataAccess.Client.OracleTransaction = Nothing
            Try



                Dim sql As String = "UPDATE adok_emp SET emp018 = 1 WHERE emp000 =:ID and emp099 =:planta"

                For Each idSol As String() In lSolicitudes

                    Dim sql1 As String = "select tra004, tra003 from adok_tra where tra004 = " & CInt(idSol(0)) & " and tra099 = " & CInt(idSol(1)) & "  and tra006 > TRUNC(SYSDATE)-60  and tra012 <> 1"  '
                    Dim resultado As List(Of String())
                    resultado = Memcached.OracleDirectAccess.Seleccionar(sql1, Conexion)

                    If resultado.Count = 0 Then
                        Dim lParameters1 As New List(Of OracleParameter)
                        lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, CInt(idSol(0)), ParameterDirection.Input))
                        lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, CInt(idSol(1)), ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(sql, Conexion, lParameters1.ToArray)
                    End If
                Next
                Return True
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        ''' <summary>
        ''' Se cierran todas las solicitudes especificadas
        ''' </summary>
        ''' <param name="lSolicitudes"></param>    
        Public Function ActualizarAvisoTrabajadores(ByVal lSolicitudes As List(Of String())) As Boolean
            'debere avisar al email de sus empresas de que van a caducar
            Try
                Return True
            Catch ex As Exception
                Throw ex
            End Try
        End Function





        'SAS
        Public Function loadProveedor(ByVal plantaAdmin As Integer, ByVal consulta As String) As List(Of ELL.Empresas)
            'Dim query As String = " select codpro,nomprov,emilio from gcprovee where nomprov like '%" & consulta & "%' or nomprov like '%" & UCase(consulta) & "%'"

            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
            '                   New ELL.Empresas With {.Id = CInt(r("codpro")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionSAS)


            'Si QUEREMOS HACER LA SELECT CON SAB
            Dim query As String = " select id,nombre, cif from empresas where nombre = '" & consulta & "' or nombre = '" & UCase(consulta) & "' and (fechabaja > sysdate or fechabaja is null) and idplanta = " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("id")), .empSAB = CInt(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Nif = SabLib.BLL.Utils.stringNull(r("cif"))}, query, CadenaConexionSAB)


            'SI QUEREMOS HACERLA CON ADOK
            'Dim query As String = " select emp000,emp002 from adok_emp where emp099 = " & plantaAdmin & " and emp018 = 0 and (emp002 like '%" & consulta & "%' or emp002 like '%" & UCase(consulta) & "%')"

            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
            '                   New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadProveedorExacto(ByVal plantaAdmin As Integer, ByVal consulta As String) As List(Of ELL.Empresas)

            'Dim query As String = " select codpro,nomprov,emilio from gcprovee where nomprov like '%" & consulta & "%' or nomprov like '%" & UCase(consulta) & "%'"

            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
            '                   New ELL.Empresas With {.Id = CInt(r("codpro")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionSAS)


            'Si QUEREMOS HACER LA SELECT CON SAB
            Dim query As String = " select id,nombre, cif from empresas where nombre like '%" & consulta & "%' or nombre like '%" & UCase(consulta) & "%' and (fechabaja > sysdate or fechabaja is null) and idplanta = " & plantaAdmin
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("id")), .empSAB = CInt(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Nif = SabLib.BLL.Utils.stringNull(r("cif"))}, query, CadenaConexionSAB)


            'SI QUEREMOS HACERLA CON ADOK
            'Dim query As String = " select emp000,emp002 from adok_emp where emp099 = " & plantaAdmin & " and emp018 = 0 and (emp002 like '%" & consulta & "%' or emp002 like '%" & UCase(consulta) & "%')"

            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
            '                   New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasSAS(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)



            Dim query As String = " select emp000,emp002 from adok_emp where emp099 = " & plantaAdmin ' pongo activos y no activos porque puede haber trabajadores no activos  & " and emp018 = 0 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)

        End Function


        'IZARO
        Public Function eliminaAsignacionTarjeta(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            'NO SE HACE LA BAJA SINO ALTA Y MODIFICACION A LA VEZ
            'Dim texto As String = ""
            'Dim fic As String
            'fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "ModDorlet.csv"

            'If Not System.IO.File.Exists(fic) Then
            '    texto = "1"
            'End If
            'Dim file As New System.IO.StreamWriter(fic, True)

            'If texto = "1" Then
            '    file.WriteLine("Codigo;Tarjeta;Trabajador;FechaFin")
            'End If

            'file.WriteLine("6;delete fcpwtar" & ";" & codigo & ";")  'no se borra la tarjeta sino la tarjeta asignada a un trabajador

            'file.Close()


            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM fcpwtar WHERE ta000=3 and ta020 = " & codigo
                'sql = "delete from fcpwtar WHERE (ta020 = " & qTrabajador & ")"

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionIZARO, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar una periodicidad", ex)
            End Try
            Return True
        End Function

        Public Function loadIzaroTrabajador(ByVal plantaAdmin As Integer, ByVal dni As String) As List(Of String())

            Dim query As String = " SELECT ti010, ti140 FROM fpertic WHERE (ti150 = '" & dni & "') and ti000=3 "

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function
        Public Function loadIzaroTrabajador2(ByVal plantaAdmin As Integer, ByVal cod As Integer) As List(Of String())

            Dim query As String = " SELECT ta010 FROM fcpwtar WHERE ta020 = " & cod

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function
        Public Function loadIzaroTrabajadorNoExiste(ByVal plantaAdmin As Integer) As List(Of String())

            Dim query As String = " SELECT ti010 FROM fpertic WHERE (ti000 = 3) ORDER BY ti010 DESC "

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function

        Public Function deptoIzaro(ByVal plantaAdmin As Integer, ByVal trabajador As String) As List(Of String())

            '   Dim query As String = " SELECT tr060 FROM fcpwtra WHERE (tr010 = " & trabajador & ") and tr000=3 "
            Dim query As String = "  Select th390 from fpertih where TH010 = " & trabajador & " And (th022 Is null or th022>sysdate)"

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function

        Public Function loadIzaroTarjeta(ByVal plantaAdmin As Integer, ByVal Tarjeta As String) As List(Of String())

            Dim query As String = " Select ta010 FROM fcpwtar WHERE  (ta010 = '" & Tarjeta & "') and ta000=3 "

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function
        Public Function loadIzaroTarjetaTra(ByVal plantaAdmin As Integer, ByVal Trabajador As Integer) As List(Of String())

            Dim query As String = " SELECT * FROM fcpwtar WHERE (ta020 = '" & Trabajador & "') and ta000=3 "

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function
        Public Function existeIzaroTra(ByVal plantaAdmin As Integer, ByVal dni As String) As List(Of String())

            Dim query As String = " SELECT ti150 FROM fpertic WHERE (ti150 = '" & dni & "') and ti000=3 "

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function


        Public Function tr230IzaroTra(ByVal plantaAdmin As Integer, ByVal dni As String) As List(Of String())

            Dim query As String = " SELECT tr230, tr260 FROM fcpwtra WHERE (tr180 = '" & dni & "') "

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function

        Public Function ta010IzaroTra(ByVal plantaAdmin As Integer, ByVal cod As Integer) As List(Of String())

            Dim query As String = " SELECT ta010 FROM fcpwtar WHERE (ta020 = " & cod & ") "

            Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

        End Function

        Public Function updateIzaroTra(ByVal tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean



            Dim nombrea As String
            Dim apellidos As String
            nombrea = Split(tipoTarjetaIzaro.Nombre, ", ")(1)
            apellidos = Split(tipoTarjetaIzaro.Nombre, ",")(0)
            Dim fic As String
            '''''''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


            '''''''''''''Dim file As New System.IO.StreamWriter(fic, True)

            '''''''''''''Dim tarjeta As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.Tarjeta.ToString, 8))
            '''''''''''''Dim trabajador As String = String.Format("{0,-6}", Left(tipoTarjetaIzaro.qTrabajador.ToString, 6))
            '''''''''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
            '''''''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
            '''''''''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
            '''''''''''''Dim DNI As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.tDNI, 8))
            '''''''''''''Dim vacio As String = String.Format("{0,-6}", "")
            '''''''''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
            '''''''''''''Dim FechaBaja As String = String.Format("{0,-8}", tipoTarjetaIzaro.FecFin.ToString("dd/MM/yy"))
            ''''''''''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
            ''''''''''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
            '''''''''''''Dim centro As String = String.Format("{0,-1}", "1")

            '''''''''''''Dim existe As List(Of String())
            '''''''''''''existe = loadIzaroTrabajador2(1, tipoTarjetaIzaro.qTrabajador) 'si ese qtrabajador tiene tarjera en fcpwtar
            '''''''''''''If existe.Count > 0 Then
            '''''''''''''    If Trim(tarjeta) = "" Then 'si se modifica fecha solo por ejemplo
            '''''''''''''        tarjeta = String.Format("{0,-8}", Left(existe(0)(0), 8))
            '''''''''''''    End If
            '''''''''''''    If Trim(tarjeta) <> "" Then
            '''''''''''''        file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
            '''''''''''''    End If
            '''''''''''''Else
            '''''''''''''    If Trim(tarjeta) <> "" Then
            '''''''''''''        file.WriteLine("A" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
            '''''''''''''    End If

            '''''''''''''End If


            '''''''''''''file.Close()




            'sql1= "update fcpwtra set tr260=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tr270=to_date('" & HastaFecha & "','dd/mm/yyyy'), tr190=" & empresa & ", tr230=" & responsable & " where upper(tr180)='" & dniUP & "' and tr000=3 and tr260= (select max (tr260) from fcpwtra where upper(tr180)='" & dniUP & "' and tr000=3)"

            '    Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty


                If tipoTarjetaIzaro.FecIni > Date.MinValue Then
                    strSql = " update fcpwtra set tr060=:departamento, tr260=:FecIni, tr270=:FecFin, tr190=:empresa, tr230=:responsable where upper(tr180)=:tdni and tr000=3 and tr260= (select max (tr260) from fcpwtra where upper(tr180)=:tdni and tr000=3)"
                Else
                    strSql = " update fcpwtra set tr060=:departamento, tr270=:FecFin, tr190=:empresa, tr230=:responsable where upper(tr180)=:tdni and tr000=3 and tr260= (select max (tr260) from fcpwtra where upper(tr180)=:tdni and tr000=3)"
                    'aqui cojo en adok ese valor de Izaro

                    '''''''''''''Dim fechaIzaro As List(Of String())
                    '''''''''''''Dim query As String = " SELECT tr260 FROM fcpwtra WHERE  where upper(tr180)=:tdni and tr000=3 and tr260= (select max (tr260) from fcpwtra where upper(tr180)=:tdni and tr000=3) "
                    '''''''''''''fechaIzaro = Memcached.OracleDirectAccess.Seleccionar(query.ToString, CadenaConexionIZARO)

                    '''''''''''''Dim lParametersFecini As New List(Of OracleParameter)
                    '''''''''''''lParametersFecini.Add(New OracleParameter("FecIni", OracleDbType.Date, CDate(fechaIzaro(0)(0)), ParameterDirection.Input))
                    '''''''''''''lParametersFecini.Add(New OracleParameter("codigo", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input)) 'poner el de adok
                    '''''''''''''lParametersFecini.Add(New OracleParameter("planta", OracleDbType.Int32, tipoTarjetaIzaro.Planta, ParameterDirection.Input))

                    '''''''''''''strSql = "UPDATE adok_tra SET  tra005=:FecIni  WHERE tra000=:codigo And tra099= :planta "
                    '''''''''''''Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParametersFecini.ToArray)
                End If

                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, tipoTarjetaIzaro.responsable, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(tipoTarjetaIzaro.tDNI).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("departamento", OracleDbType.Varchar2, 15, tipoTarjetaIzaro.departamento, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("empresa", OracleDbType.Varchar2, 50, tipoTarjetaIzaro.Empresa.ToString, ParameterDirection.Input))



                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters1.ToArray)


                strSql = " update fpertic set ti140=:responsable         where ti010=:qTrabajador and ti000=3 "
                Dim lParameters2 As New List(Of OracleParameter)
                lParameters2.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, tipoTarjetaIzaro.responsable, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters2.ToArray)


                'falta actualizar fpertif y fpertih
                'sql= "update fpertif set tf021='" & DesdeFecha & "', tf022='" & HastaFecha & "' where tf010=" & qTrabajador
                If tipoTarjetaIzaro.FecIni > Date.MinValue Then
                    strSql = " update fpertif set tf021=:FecIni, tf022=:FecFin         where tf010=:qTrabajador and tf000=3 "
                Else
                    strSql = " update fpertif set tf022=:FecFin         where tf010=:qTrabajador and tf000=3 "
                End If

                Dim lParameters3 As New List(Of OracleParameter)
                lParameters3.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters3.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))
                lParameters3.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters3.ToArray)

                'fpertih
                If tipoTarjetaIzaro.FecIni > Date.MinValue Then
                    strSql = " update fpertih set th390=:departamento, th021=:FecIni, th022=:FecFin        where th010=:qTrabajador and th000=3 "
                Else
                    strSql = " update fpertih set th390=:departamento, th022=:FecFin        where th010=:qTrabajador and th000=3 "
                End If

                Dim lParameters4 As New List(Of OracleParameter)
                lParameters4.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("departamento", OracleDbType.Varchar2, 15, tipoTarjetaIzaro.departamento, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)



                'falta meter en SAB USUARIOS el departamento, y fechas ini fin. Tambien en el alta
                Dim sabBLL As New SabLib.BLL.UsuariosComponent
                Dim oUser As SabLib.ELL.Usuario = sabBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(tipoTarjetaIzaro.responsable)})
                '      Dim deptBLL As New SabLib.BLL.DepartamentosComponent
                '      Dim oDept As SabLib.ELL.Departamento = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = oUser.IdDepartamento, .IdPlanta = oUser.IdPlanta})
                'oDept.id es el iddepartamento 
                If tipoTarjetaIzaro.FecIni > Date.MinValue Then
                    strSql = "UPDATE USUARIOS SET fechaalta=:FecIni,fechabaja=:FecFin WHERE CODPERSONA=:qTrabajador "   'iddepartamento=:departamento
                Else
                    strSql = "UPDATE USUARIOS SET fechabaja=:FecFin WHERE CODPERSONA=:qTrabajador "   'iddepartamento=:departamento
                End If

                Dim lParameters5 As New List(Of OracleParameter)
                lParameters5.Add(New OracleParameter("FecIni", OracleDbType.Date, Now, ParameterDirection.Input))
                lParameters5.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))
                lParameters5.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))
                '    lParameters5.Add(New OracleParameter("departamento", OracleDbType.Varchar2, 22, oDept.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionSAB, lParameters5.ToArray)





                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return True

        End Function


        Public Function AltaIzaroTra(ByVal tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean

            Dim nombrea As String
            Dim apellidos As String
            nombrea = Split(tipoTarjetaIzaro.Nombre, ", ")(1)
            apellidos = Split(tipoTarjetaIzaro.Nombre, ",")(0)
            '''''''''''Dim fic As String
            '''''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


            '''''''''''Dim file As New System.IO.StreamWriter(fic, True)

            '''''''''''Dim tarjeta As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.Tarjeta.ToString, 8))
            '''''''''''Dim trabajador As String = String.Format("{0,-6}", Left(tipoTarjetaIzaro.qTrabajador.ToString, 6))
            '''''''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
            '''''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
            '''''''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
            '''''''''''Dim DNI As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.tDNI, 8))
            '''''''''''Dim vacio As String = String.Format("{0,-6}", "")
            '''''''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
            '''''''''''Dim FechaBaja As String = String.Format("{0,-8}", tipoTarjetaIzaro.FecFin.ToString("dd/MM/yy"))
            '''''''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
            '''''''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
            '''''''''''Dim centro As String = String.Format("{0,-1}", "1")

            '''''''''''Dim existe As List(Of String())
            '''''''''''existe = loadIzaroTrabajador2(1, tipoTarjetaIzaro.qTrabajador) 'si ese qtrabajador tiene tarjera en fcpwtar
            '''''''''''If Trim(tarjeta) <> "" Then
            '''''''''''    If existe.Count > 0 Then
            '''''''''''        file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
            '''''''''''    Else
            '''''''''''        file.WriteLine("A" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
            '''''''''''    End If
            '''''''''''End If





            '''''''''''file.Close()


            'Sql = "insert into fcpwtra "
            'Sql = Sql & "(tr000,tr010,tr020,tr030,tr040,tr050,tr060,tr070,tr080,tr090,tr180,tr190,tr230,tr260,tr270,tr420,tr430,tr440) "
            'Sql = Sql & "values (3," & qTrabajador & ",'T','A','" & nombre & " " & apellidos & "',1,'" & departamento & "',2,0,'E','" & dni & "'," & empresa & ","
            'Sql = Sql & validador0 & ",to_date('" & DesdeFecha & "','dd/mm/yyyy'),to_date('" & HastaFecha & "','dd/mm/yyyy'),9,3," & qTrabajador & ")"

            Try

                Dim strSql As String = String.Empty
                strSql = "insert into fcpwtra "

                strSql = strSql & "(tr000,tr010,tr020,tr030,tr040,tr050,tr060,tr070,tr080,tr090,tr180,tr190,tr230,tr260,tr270,tr420,tr430,tr440) "

                strSql = strSql & " values (3,:qTrabajador,'T','A',:nombrecompleto, 1,:departamento, 2, 0,'E',:tdni,:empresa,"

                strSql = strSql & ":responsable,:FecIni, :FecFin,'9',3,:qTrabajador)"


                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombrecompleto", OracleDbType.Varchar2, 40, Trim(tipoTarjetaIzaro.Nombre).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("departamento", OracleDbType.Varchar2, 15, tipoTarjetaIzaro.departamento, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("tdni", OracleDbType.Varchar2, 12, Trim(tipoTarjetaIzaro.tDNI).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("empresa", OracleDbType.Varchar2, 50, tipoTarjetaIzaro.Empresa, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, tipoTarjetaIzaro.responsable, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))

                '      lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, tipoTarjetaIzaro.Planta, ParameterDirection.Input))



                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters1.ToArray)



                'Sql = "insert into fpertic "
                'Sql = Sql & "(ti000,ti010,ti020,ti030,ti050,ti130,ti140,ti150,ti300,ti470,ti670) "
                'Sql = Sql & "values (3," & qTrabajador & ",'T','A','" & nombre & " " & apellidos & "'"
                'Sql = Sql & ",'" & dni & "'," & validador0 & ",'" & dni & "'," & empresa & ",'E',9)"


                strSql = "insert into fpertic (ti000,ti010,ti020,ti030,ti050,ti130,ti140,ti150,ti300,ti470,ti670) "
                strSql = strSql & " values (3,:qTrabajador,'T','A',:nombrecompleto,:tdni,:responsable,:tdni,:empresa,'E',9) "
                Dim lParameters2 As New List(Of OracleParameter)
                lParameters2.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("nombrecompleto", OracleDbType.Varchar2, 40, Trim(tipoTarjetaIzaro.Nombre).ToUpper, ParameterDirection.Input))

                lParameters2.Add(New OracleParameter("tdni", OracleDbType.Varchar2, 12, Trim(tipoTarjetaIzaro.tDNI).ToUpper, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("empresa", OracleDbType.Varchar2, 50, tipoTarjetaIzaro.Empresa, ParameterDirection.Input))

                lParameters2.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, tipoTarjetaIzaro.responsable, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters2.ToArray)




                'Sql = "insert into fpertif "
                'Sql = Sql & "(tf000,tf010,tf021,tf022,tf025) "
                'Sql = Sql & "values (3," & qTrabajador & ",to_date('" & DesdeFecha & "','dd/mm/yyyy'),to_date('" & HastaFecha & "','dd/mm/yyyy'),0)"

                strSql = "insert into fpertif (tf000, tf010, tf021, tf022, tf025) "
                strSql = strSql & " values (3,:qTrabajador,:FecIni, :FecFin,0) "
                Dim lParameters3 As New List(Of OracleParameter)
                lParameters3.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))

                lParameters3.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters3.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters3.ToArray)

                'Sql = "insert into fpertih "
                'Sql = Sql & "(th000,th010,th021,th022,th390) "
                'Sql = Sql & "values (3," & qTrabajador & ",to_date('" & DesdeFecha & "','dd/mm/yyyy'),to_date('" & HastaFecha & "','dd/mm/yyyy'),'" & departamento & "')"


                strSql = "insert into fpertih (th000,th010,th021,th022,th390) "
                strSql = strSql & " values (3,:qTrabajador,:FecIni, :FecFin,:departamento) "
                Dim lParameters4 As New List(Of OracleParameter)
                lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("departamento", OracleDbType.Varchar2, 15, tipoTarjetaIzaro.departamento, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)



                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return True

        End Function

        Public Function AltaIzaroTraTarjeta(ByVal plantaAdmin As Integer, ByVal tarjetaParam As String, ByVal trabajadorParam As Integer, ByVal fecfin As Date) As Boolean

            Dim trabajador As String = String.Format("{0,-6}", trabajadorParam)
            Dim tarjeta As String = String.Format("{0,-8}", tarjetaParam)
            '''''''''''''''''''''Dim fic As String
            '''''''''''''''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


            '''''''''''''''''''''Dim file As New System.IO.StreamWriter(fic, True)


            '''''''''''''''''''''Dim Apellido1 As String = String.Format("{0,-25}", "")
            '''''''''''''''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
            '''''''''''''''''''''Dim Nombre As String = String.Format("{0,-20}", "")
            '''''''''''''''''''''Dim DNI As String = String.Format("{0,-8}", "")
            '''''''''''''''''''''Dim vacio As String = String.Format("{0,-6}", "")
            '''''''''''''''''''''Dim FechaAlta As String = String.Format("{0,-8}", "")
            '''''''''''''''''''''Dim FechaBaja As String = String.Format("{0,-8}", fecfin.ToString("dd/MM/yy"))
            '''''''''''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
            '''''''''''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
            '''''''''''''''''''''Dim centro As String = String.Format("{0,-2}", "")

            '''''''''''''''''''''file.WriteLine("A" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & Ruta & ZonaHoraria & centro)
            '''''''''''''''''''''file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & Ruta & ZonaHoraria & centro)

            ''''''''''''''''''''''file.WriteLine("1;insert fcpwtar" & tarjeta & ";" & trabajador & ";" & fecfin.ToString("dd/MM/yyyy"))   'en dorlet se da alta y modificación a la vez (pero no la baja porque seria de la tarjeta)
            ''''''''''''''''''''''file.WriteLine("2;update fcpwtar" & tarjeta & ";" & trabajador & ";" & fecfin.ToString("dd/MM/yyyy"))
            '''''''''''''''''''''file.Close()

            Try

                Dim strSql As String = String.Empty


                'Sql = "insert into fcpwtar (ta000,ta010,ta020) "
                'Sql = Sql & "values (3,'" & tarjeta & "'," & qTrabajador & ")"
                strSql = "insert into fcpwtar (ta000,ta010,ta020) "
                strSql = strSql & " values (3,:tarjeta,:qTrabajador) "

                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, trabajador, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("tarjeta", OracleDbType.Varchar2, 12, Trim(tarjeta).ToUpper, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters1.ToArray)



                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return True

        End Function

        Public Function ActualizaIzaroTra(ByVal tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean



            Dim nombrea As String
            Dim apellidos As String
            nombrea = Split(tipoTarjetaIzaro.Nombre, ", ")(1)
            apellidos = Split(tipoTarjetaIzaro.Nombre, ",")(0)
            ''''''''''''Dim fic As String
            ''''''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


            ''''''''''''Dim file As New System.IO.StreamWriter(fic, True)

            ''''''''''''Dim tarjeta As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.Tarjeta.ToString, 8))
            ''''''''''''Dim trabajador As String = String.Format("{0,-6}", Left(tipoTarjetaIzaro.qTrabajador.ToString, 6))
            ''''''''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
            ''''''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
            ''''''''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
            ''''''''''''Dim DNI As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.tDNI, 8))
            ''''''''''''Dim vacio As String = String.Format("{0,-6}", "")
            ''''''''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
            ''''''''''''Dim FechaBaja As String = String.Format("{0,-8}", tipoTarjetaIzaro.FecFin.ToString("dd/MM/yy"))
            ''''''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
            ''''''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
            ''''''''''''Dim centro As String = String.Format("{0,-1}", "1")

            ''''''''''''Dim existe As List(Of String())
            ''''''''''''existe = loadIzaroTrabajador2(1, tipoTarjetaIzaro.qTrabajador) 'si ese qtrabajador tiene tarjera en fcpwtar
            ''''''''''''If Trim(tarjeta) <> "" Then
            ''''''''''''    If existe.Count > 0 Then
            ''''''''''''        file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
            ''''''''''''    Else
            ''''''''''''        file.WriteLine("A" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
            ''''''''''''    End If
            ''''''''''''End If

            ''''''''''''file.Close()

            'sql= "update fcpwtra Set tr060='" & departamento & "',tr260=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tr270=to_date('" & HastaFecha & "','dd/mm/yyyy'), tr230='" & validador0 & "'  where tr180='" & dni & "'"
            '	sql= "update fpertic set ti140='" & validador0 & "'  where ti010='" & qTrabajador & "' and ti000=3"
            'sql= "update fpertif set tf021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tf022=to_date('" & HastaFecha & "','dd/mm/yyyy') where tf010=" & qTrabajador
            'sql= "update fpertih set th021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), th022=to_date('" & HastaFecha & "','dd/mm/yyyy'), th390='" & departamento & "' where th010=" & qTrabajador

            '        Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                ' strSql = "Update fcpwtra set tr260==:FecIni, tr270==:FecFin, tr190=:empresa, tr230=:responsable where upper(tr180)=:tdni and tr000=3 and tr260= (select max (tr260) from fcpwtra where upper(tr180)=:tdni and tr000=3)"
                strSql = " update fcpwtra set tr260=:FecIni, tr270=:FecFin, tr230=:responsable  where tr180=:tdni and tr000=3 "

                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, tipoTarjetaIzaro.responsable, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(tipoTarjetaIzaro.tDNI).ToUpper, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters1.ToArray)

                'sql=    "update fpertic set ti140='" & validador0 & "'  where ti010='" & qTrabajador & "' and ti000=3"
                strSql = " update fpertic set ti140=:responsable         where ti010=:qTrabajador and ti000=3 "
                Dim lParameters2 As New List(Of OracleParameter)
                lParameters2.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, tipoTarjetaIzaro.responsable, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters2.ToArray)

                'sql=     "update fpertif set tf021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), tf022=to_date('" & HastaFecha & "','dd/mm/yyyy') where tf010=" & qTrabajador
                strSql = " update fpertif set tf021=:FecIni, tf022=:FecFin         where tf010=:qTrabajador and tf000=3 "
                Dim lParameters3 As New List(Of OracleParameter)
                lParameters3.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters3.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))
                lParameters3.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters3.ToArray)

                'sql=    "update fpertih set th021=to_date('" & DesdeFecha & "','dd/mm/yyyy'), th022=to_date('" & HastaFecha & "','dd/mm/yyyy'), th390='" & departamento & "' where th010=" & qTrabajador
                strSql = " update fpertih set th021=:FecIni, th022=:FecFin        where th010=:qTrabajador and th000=3 "
                Dim lParameters4 As New List(Of OracleParameter)
                lParameters4.Add(New OracleParameter("FecIni", OracleDbType.Date, tipoTarjetaIzaro.FecIni, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, tipoTarjetaIzaro.FecFin, ParameterDirection.Input))
                lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, tipoTarjetaIzaro.qTrabajador, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return True

        End Function

        Public Function BorraIzaroTra(ByVal tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean
            Dim nombrea As String
            Dim apellidos As String
            nombrea = Split(tipoTarjetaIzaro.Nombre, ", ")(1)
            apellidos = Split(tipoTarjetaIzaro.Nombre, ",")(0)

            '''''''''Dim fic As String
            '''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"
            '''''''''Dim file As New System.IO.StreamWriter(fic, True)

            '''''''''Dim tarjeta As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.Tarjeta.ToString, 8))
            '''''''''Dim trabajador As String = String.Format("{0,-6}", Left(tipoTarjetaIzaro.qTrabajador.ToString, 6))
            '''''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
            '''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
            '''''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
            '''''''''Dim DNI As String = String.Format("{0,-8}", Left(tipoTarjetaIzaro.tDNI, 8))
            '''''''''Dim vacio As String = String.Format("{0,-6}", "")
            '''''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
            '''''''''Dim FechaBaja As String = String.Format("{0,-8}", tipoTarjetaIzaro.FecFin.ToString("dd/MM/yy"))
            '''''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
            '''''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
            '''''''''Dim centro As String = String.Format("{0,-1}", "1")



            '''''''''file.WriteLine("B" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
            '''''''''file.Close()


            Try



                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return True

        End Function

        ''' <summary>
        ''' Obtiene todos los tipos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarDocumentos(ByVal idDoc As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc010, doc012, DOC013, doc014, doc015, doc016, doc017  FROM adok_doc  WHERE DOC000=" & idDoc & " And doc099= " & plantaAdmin & " order by doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.area = SabLib.BLL.Utils.integerNull(r("doc017")), .ETT = SabLib.BLL.Utils.integerNull(r("doc010")), .activo = SabLib.BLL.Utils.integerNull(r("doc015")), .ubicacion = SabLib.BLL.Utils.stringNull(r("doc016")), .tipotrabajo = SabLib.BLL.Utils.integerNull(r("doc014")), .Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Trabajador = SabLib.BLL.Utils.integerNull(r("doc005")), .comentario = SabLib.BLL.Utils.stringNull(r("doc006")), .Responsable = SabLib.BLL.Utils.integerNull(r("doc007")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


        End Function

        Public Function CargarDocumentosCer(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Documentos)

            Dim query As String = "Select doc000, doc002, doc001, doc011  FROM adok_doc  WHERE doc099= " & plantaAdmin & "  and (doc011 is null or doc011 <> 1)   and UPPER(doc001) like UPPER('%" & texto & "%') Order by doc001 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                               New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc002")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc001")), .activo = SabLib.BLL.Utils.integerNull(r("doc011"))}, query, CadenaConexion)



        End Function
        Public Function CargarProfesion(ByVal texto As String) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select emp000  FROM adok_emp  WHERE UPPER(emp002) like UPPER('%" & texto & "%') Order by emp002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                               New ELL.TrabajadoresDoc With {.clave = CInt(r("emp000"))}, query, CadenaConexion)

        End Function
        Public Function CargarResponsable(ByVal texto As String) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select max(tra008) AS COD  FROM adok_tra  WHERE UPPER(tra011) like UPPER('%" & texto & "%') Order by tra000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                               New ELL.TrabajadoresDoc With {.clave = CInt(r("COD"))}, query, CadenaConexion)
        End Function
        Public Function CargarResponsables(ByVal cod As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "select apellido1, apellido2, nombre from usuarios  WHERE id =" & cod

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.Nombre = SabLib.BLL.Utils.stringNull(r("apellido1")) & " " & SabLib.BLL.Utils.stringNull(r("apellido2")) & ", " & SabLib.BLL.Utils.stringNull(r("nombre"))}, query, CadenaConexionSAB)

        End Function
        Public Function CargarDocumentosETT(ByVal idDoc As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            '    Dim param1 As OracleParameter

            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, DOC013, doc014, doc015, doc016, doc017  FROM adok_doc  WHERE doc010=1 and DOC000=" & idDoc & " And doc099= " & plantaAdmin & " order by doc012, doc000 "

            '   param1 = New OracleParameter("ID_PRODUCTO", OracleDbType.Int32, idDoc, ParameterDirection.Input)


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.area = SabLib.BLL.Utils.integerNull(r("doc017")), .activo = SabLib.BLL.Utils.integerNull(r("doc015")), .ubicacion = SabLib.BLL.Utils.stringNull(r("doc016")), .tipotrabajo = SabLib.BLL.Utils.integerNull(r("doc014")), .Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Trabajador = SabLib.BLL.Utils.integerNull(r("doc005")), .comentario = SabLib.BLL.Utils.stringNull(r("doc006")), .Responsable = SabLib.BLL.Utils.integerNull(r("doc007")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


        End Function
        Public Function CargarRol(ByVal idUser As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol001, rol099, rol002 FROM adok_roles  WHERE rol000=" & idUser & " And rol099 = " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.rol=CInt(r("rol099")), .Id = CInt(r("rol001")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, CadenaConexion)


        End Function
        Public Function CargarRolETT(ByVal idUser As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol001, rol002 FROM adok_rolesETT  WHERE rol000=" & idUser & " And rol099= " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("rol001")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, CadenaConexion)


        End Function
        Public Function CargarTiposEmpresa(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)

            Dim query As String = "Select emp000, emp001, emp002  FROM adok_emp  WHERE emp000=" & idEmpresas


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)

        End Function
        Public Function CargarEmpresas() As List(Of ELL.Empresas)

            Dim query As String = "Select emp000, emp001, emp002, FECHA  FROM adok_emp order by emp002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("FECHA"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposTrabajador(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)

            Dim query As String = "Select tra000, tra001, tra002  FROM adok_tra  WHERE tra004=" & idEmpresas


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("tra000"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposEmpresa2(ByVal doc As Integer, ByVal tra As Integer) As List(Of ELL.Empresas)

            Dim query As String = "Select trd000  FROM adok_trd  WHERE trd001=" & doc & " and trd000=" & tra


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("trd000"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposEmpresaSAB(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emp000 FROM adok_emp  WHERE emp022=" & idEmpresas & " And emp099= " & plantaAdmin


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposEmpresaS(ByVal idEmpresas As Integer, ByVal idEmpresas2 As String, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If

            Dim query As String = "Select 1 as orden, emp000, emp001, emp002, emp003, emp004, emp005, emp006, emp007, emp008, emp009, emp010, emp011, emp012, emp013, emp014, emp015, emp016, emp017, emp018, emp019, emp020, emp021,emp022, emp023  FROM adok_emp  WHERE emp000 = " & idEmpresas & " And emp099= " & plantaAdmin
            query = query & "  UNION Select 2 as orden, emp000, emp001, emp002, emp003, emp004, emp005, emp006, emp007, emp008, emp009, emp010, emp011, emp012, emp013, emp014, emp015, emp016, emp017, emp018, emp019, emp020, emp021,emp022, emp023  FROM adok_emp  WHERE emp000 in (" & idEmpresas2 & ") And emp099= " & plantaAdmin & " order by orden"



            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nif = SabLib.BLL.Utils.stringNull(r("emp001")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002")), .DocSol = SabLib.BLL.Utils.integerNull(r("emp004")), .FecSolEnv = SabLib.BLL.Utils.dateTimeNull(r("emp005")), .medio = SabLib.BLL.Utils.stringNull(r("emp006")), .DocEnv = SabLib.BLL.Utils.integerNull(r("emp007")), .FecEnv = SabLib.BLL.Utils.dateTimeNull(r("emp008")), .medio2 = SabLib.BLL.Utils.stringNull(r("emp009")), .DocRec = SabLib.BLL.Utils.integerNull(r("emp010")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emp011")), .recibi = SabLib.BLL.Utils.integerNull(r("emp012")), .Autonomo = SabLib.BLL.Utils.integerNull(r("emp021")), .preventiva = SabLib.BLL.Utils.stringNull(r("emp013")), .interlocutor = SabLib.BLL.Utils.stringNull(r("emp014")), .telefono = SabLib.BLL.Utils.stringNull(r("emp015")), .email = SabLib.BLL.Utils.stringNull(r("emp016")), .fax = SabLib.BLL.Utils.stringNull(r("emp017")), .activo = SabLib.BLL.Utils.integerNull(r("emp018")), .subcontrata = SabLib.BLL.Utils.stringNull(r("emp019")), .contacto = SabLib.BLL.Utils.stringNull(r("emp020")), .notificar = SabLib.BLL.Utils.stringNull(r("emp023")), .empSAB = SabLib.BLL.Utils.stringNull(r("emp022"))}, query, CadenaConexion)

        End Function

        Public Function CargarTiposEmpresaCIF(ByVal cif As String, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emp000, emp001, emp002, emp003, emp004, emp005, emp006, emp007, emp008, emp009, emp010, emp011, emp012, emp013, emp014, emp015, emp016, emp017, emp018, emp019, emp020, emp021, emp022, emp023  FROM adok_emp  WHERE UPPER(emp001)='" & Trim(cif).ToUpper & "' And emp099= " & plantaAdmin


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nif = SabLib.BLL.Utils.stringNull(r("emp001")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002")), .DocSol = SabLib.BLL.Utils.integerNull(r("emp004")), .FecSolEnv = SabLib.BLL.Utils.dateTimeNull(r("emp005")), .medio = SabLib.BLL.Utils.stringNull(r("emp006")), .DocEnv = SabLib.BLL.Utils.integerNull(r("emp007")), .FecEnv = SabLib.BLL.Utils.dateTimeNull(r("emp008")), .medio2 = SabLib.BLL.Utils.stringNull(r("emp009")), .DocRec = SabLib.BLL.Utils.integerNull(r("emp010")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emp011")), .recibi = SabLib.BLL.Utils.integerNull(r("emp012")), .Autonomo = SabLib.BLL.Utils.integerNull(r("emp021")), .preventiva = SabLib.BLL.Utils.stringNull(r("emp013")), .interlocutor = SabLib.BLL.Utils.stringNull(r("emp014")), .telefono = SabLib.BLL.Utils.stringNull(r("emp015")), .email = SabLib.BLL.Utils.stringNull(r("emp016")), .fax = SabLib.BLL.Utils.stringNull(r("emp017")), .activo = SabLib.BLL.Utils.integerNull(r("emp018")), .subcontrata = SabLib.BLL.Utils.stringNull(r("emp019")), .contacto = SabLib.BLL.Utils.stringNull(r("emp020")), .notificar = SabLib.BLL.Utils.stringNull(r("emp023")), .empSAB = SabLib.BLL.Utils.integerNull(r("emp022"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposTrabajadorCIF(ByVal cif As String, ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "SELECT tra008, tra000, tra004, tra014  FROM adok_tra  WHERE UPPER(tra013)='" & Trim(cif).ToUpper & "' " ' And tra099= " & plantaAdmin


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                             New ELL.Trabajadores With {.responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .Id = CInt(r("tra000")), .Empresa = CInt(r("tra004")), .solicitud = SabLib.BLL.Utils.stringNull(r("tra014"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposSolicitudClave(ByVal plantaAdmin As Integer, ByVal clave As Integer) As List(Of ELL.Solicitudes)

            Dim query As String = "SELECT sol010, sol001, sol002, sol005, sol006,sol007, sol014, sol015, sol016, sol018, sol019, sol020, sol021, sol022, sol023  FROM adok_sol  WHERE sol000 = " & clave & " And sol099= " & plantaAdmin


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                             New ELL.Solicitudes With {.responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = CInt(r("sol001")), .soldadura = CInt(r("sol005")), .altura = CInt(r("sol006")), .salas = CInt(r("sol007")), .gases = CInt(r("sol014")), .elevados = CInt(r("sol015")), .fosas = CInt(r("sol016")), .X7 = SabLib.BLL.Utils.integerNull(r("sol018")), .X8 = SabLib.BLL.Utils.integerNull(r("sol019")), .X9 = SabLib.BLL.Utils.integerNull(r("sol020")), .X10 = SabLib.BLL.Utils.integerNull(r("sol021")), .X11 = SabLib.BLL.Utils.integerNull(r("sol022")), .Subcontrata = SabLib.BLL.Utils.stringNull(r("sol023"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposSolicitudClaveETT(ByVal plantaAdmin As Integer, ByVal clave As Integer) As List(Of ELL.Solicitudes)

            Dim query As String = "SELECT  sol002, sol024, sol025, sol010, sol001, sol004, sol005, sol006,sol007, sol014, sol015, sol016, sol018, sol019, sol020, sol021, sol022, sol023  FROM adok_solETT  WHERE sol000 = " & clave & " And sol099= " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                             New ELL.Solicitudes With {.responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .FechaFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .Area = SabLib.BLL.Utils.integerNull(r("sol024")), .Numero = SabLib.BLL.Utils.integerNull(r("sol025")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = CInt(r("sol001")), .soldadura = CInt(r("sol005")), .altura = CInt(r("sol006")), .salas = CInt(r("sol007")), .gases = CInt(r("sol014")), .elevados = CInt(r("sol015")), .fosas = CInt(r("sol016")), .X7 = SabLib.BLL.Utils.integerNull(r("sol018")), .X8 = SabLib.BLL.Utils.integerNull(r("sol019")), .X9 = SabLib.BLL.Utils.integerNull(r("sol020")), .X10 = SabLib.BLL.Utils.integerNull(r("sol021")), .X11 = SabLib.BLL.Utils.integerNull(r("sol022")), .Subcontrata = SabLib.BLL.Utils.stringNull(r("sol023"))}, query, CadenaConexion)

        End Function
        Public Function CargarTiposEmpresaXBAT(ByVal idEmpresas As Integer) As List(Of ELL.Empresas)

            Dim query As String = "select nomprov, cif from gcprovee  WHERE codpro=" & idEmpresas


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionXBAT)

        End Function
        Public Function CargarTiposEmpresaXBATNombre(ByVal nombre As String) As List(Of ELL.Empresas)

            Dim query As String = "select codpro, nomprov, cif from gcprovee  WHERE nomprov like '%" & nombre & "%'"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("codpro")), .Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionXBAT)

        End Function
        Public Function CargarTiposEmpresaXBATCIF(ByVal cif As String) As List(Of ELL.Empresas)

            'Dim query As String = "select codpro, nomprov, cif from gcprovee  WHERE cif like '%" & cif & "%'"


            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
            '                 New ELL.Empresas With {.Id = CInt(r("codpro")), .Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionXBAT)

            Dim query As String = "select id, nombre, cif, contacto, telefono from empresas  WHERE cif like '%" & cif & "%'"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .contacto = SabLib.BLL.Utils.stringNull(r("contacto")), .telefono = SabLib.BLL.Utils.stringNull(r("telefono"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarTiposTrabajadorXBATCIF(ByVal dni As String) As List(Of ELL.Empresas)

            Dim query As String = "select id, codpersona from usuarios  WHERE dni like '%" & dni & "%' "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("codpersona"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarTiposTrabajadorXBATuserPlanta(ByVal planta As Integer, ByVal user As String) As List(Of ELL.Empresas)
            If planta = 230 Then
                planta = 1
            End If
            Dim lParameters1 As New List(Of OracleParameter)

            lParameters1.Add(New OracleParameter("Fecha", OracleDbType.Date, Now, ParameterDirection.Input))

            Dim query As String = "select id from usuarios  WHERE UPPER(NOMBREUSUARIO) like '%" & user & "%' and idplanta = " & planta & "  and (FECHABAJA > :Fecha or FECHABAJA is null ) "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("id"))}, query, CadenaConexionSAB, lParameters1.ToArray)


        End Function
        Public Function CargarTiposEmpresaXBATTroqueleria(ByVal id As Integer) As List(Of ELL.Empresas)


            Dim query As String = "select id, idtroqueleria from empresas  WHERE id = " & id


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("idtroqueleria"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarTiposTrabajadorXBATTroqueleria(ByVal id As Integer) As List(Of ELL.Empresas)


            Dim query As String = "select  id from usuarios  WHERE CODPERSONA = " & id


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("id"))}, query, CadenaConexionSAB)


        End Function

        Public Function CargarTiposDorletporDNI(ByVal dni As String) As List(Of ELL.Dorlet)

            Dim query As String = "select id_dorlet, tarjeta_dorlet from adok_dorlet  WHERE dni_dorlet like '%" & dni & "%'"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Dorlet)(Function(r As OracleDataReader) _
                             New ELL.Dorlet With {.Id = CInt(r("id_dorlet")), .Tarjeta = SabLib.BLL.Utils.stringNull(r("tarjeta_dorlet"))}, query, CadenaConexion)

        End Function


        Public Function loadListtodos(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select decode(doc012,0,2,1,0,2,3,3,1,4,4,5) as orden, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc011, doc012, doc017  FROM adok_doc  WHERE doc099= " & plantaAdmin & "  order by doc011 asc, orden asc, doc001 asc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.area = SabLib.BLL.Utils.integerNull(r("doc017")), .Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .activo = SabLib.BLL.Utils.integerNull(r("doc011")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function
        Public Function loadListtodosPara4(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select decode(doc012,0,2,1,0,2,3,3,1,4,4,5,5,6) as orden, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc011, doc012  FROM adok_doc  WHERE doc099= " & plantaAdmin & " and (doc012=1 or doc012=2 or doc012=6) and doc011=0  order by doc011 asc, orden asc, doc001 asc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .activo = SabLib.BLL.Utils.integerNull(r("doc011")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function

        Public Function loadListtodosPara4ETT(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select decode(doc012,0,2,1,0,2,3,3,1,4,4,5,5,6) as orden, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc011, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc017= " & tipo & " and doc099= " & plantaAdmin & "    order by doc012 asc, orden asc, doc001 asc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.area = SabLib.BLL.Utils.integerNull(r("doc017")), .Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .activo = SabLib.BLL.Utils.integerNull(r("doc011")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function
        Public Function loadListTipo4(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            'and (doc011 is null or doc011 = 0)
            Dim query As String = "Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE doc099= " & plantaAdmin & " and doc012=4 and doc014 > 0 order by doc015 asc, doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .comentario = SabLib.BLL.Utils.stringNull(r("doc016")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc014")), .activo = SabLib.BLL.Utils.integerNull(r("doc015"))}, query, CadenaConexion)
        End Function
        Public Function loadListTipo4ETT(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            'and (doc011 is null or doc011 = 0)
            Dim query As String = "Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 0 and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 1  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 2  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 3  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 4  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 5  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 6  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 7  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 8  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 9  and rownum = 1 "
            query = query & " UNION  Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc017 = 10  and rownum = 1 "
            query = query & " order by doc017 asc, doc012, doc000 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.area = SabLib.BLL.Utils.integerNull(r("doc017")), .Id = CInt(r("doc017")), .comentario = SabLib.BLL.Utils.stringNull(r("doc016")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .activo = SabLib.BLL.Utils.integerNull(r("doc015"))}, query, CadenaConexion)
        End Function
        Public Function loadListTipo4Activos(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE doc099= " & plantaAdmin & " and doc012=4 and doc015 = 0 and (doc011 is null or doc011 = 0) order by doc015 asc, doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .comentario = SabLib.BLL.Utils.stringNull(r("doc016")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc014")), .activo = SabLib.BLL.Utils.integerNull(r("doc015"))}, query, CadenaConexion)
        End Function
        Public Function loadListTipo4ActivosETT(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            'and (doc011 is null or doc011 = 0)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc010=1 and doc099= " & plantaAdmin & "  and doc014 > 0  and doc015 = 0 order by doc015 asc, doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.area = SabLib.BLL.Utils.integerNull(r("doc017")), .Id = CInt(r("doc000")), .comentario = SabLib.BLL.Utils.stringNull(r("doc016")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc014")), .activo = SabLib.BLL.Utils.integerNull(r("doc015"))}, query, CadenaConexion)
        End Function
        Public Function loadList(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc017  FROM adok_doc  WHERE doc099= " & plantaAdmin & " and (doc011 is null or doc011 = 0) order by doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.area = SabLib.BLL.Utils.integerNull(r("doc017")), .Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function

        Public Function loadListTipo(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc, adok_pla  WHERE doc000 = pla000 and pla006 = 1 and  doc099= " & plantaAdmin & " and doc014 = " & tipo & " and (doc011 is null or doc011 = 0) order by doc012, doc000 "
            'no se para que es pla006Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE  doc099= " & plantaAdmin & " and doc014 = " & tipo & " and (doc011 is null or doc011 = 0) order by doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003"))}, query, CadenaConexion)
        End Function
        Public Function loadListTipoSinPlantilla(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            'Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc, adok_pla  WHERE doc000 = pla000 and pla006 = 1 and  doc099= " & plantaAdmin & " and doc014 = " & tipo & " and (doc011 is null or doc011 = 0) order by doc012, doc000 "
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE  doc099= " & plantaAdmin & " and doc014 = " & tipo & " and (doc011 is null or doc011 = 0) order by doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003"))}, query, CadenaConexion)
        End Function
        Public Function loadListTipoDOS4(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select DOS001  FROM adok_DOS  WHERE doS099= " & plantaAdmin & " and doS000 = " & tipo & "  order by doS000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doS001"))}, query, CadenaConexion)
        End Function

        Public Function loadListEmpDoc(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE doc005 = 0 and doc099= " & plantaAdmin & " order by doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function
        Public Function loadListEmpDoc2(ByVal nombre As String) As List(Of ELL.Documentos)
            Dim nom As String
            Dim ape As String
            nom = Trim(Split(nombre, ",")(1))
            ape = Trim(Split(nombre, ",")(0))
            Dim query As String = "Select tra004 FROM adok_tra  WHERE tra002  like '%" & nom & "%' or tra003  like '%" & ape & "%'"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("tra004"))}, query, CadenaConexion)
        End Function
        Public Function loadListEmpDocETT(ByVal plantaAdmin As Integer, ByVal area As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc003, doc004  FROM adok_doc, adok_dosett  WHERE doc000 = dos001 and doc010=1 and dos000= " & area & " and  doc099= " & plantaAdmin & " order by dos001 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004"))}, query, CadenaConexion)
        End Function
        Public Function loadListEmpDocTra(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            '      Dim query As String = "Select fs099,fs000,fs001,fs002,fs003  FROM adok_fs  WHERE fs005 = 1 and (doc011 is null or doc011 = 0) and doc099= " & plantaAdmin & " order by doc012, doc000 "
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE doc005 = 1 and (doc011 is null or doc011 = 0) and doc099= " & plantaAdmin & " order by doc012, doc000 "


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function
        Public Function loadListEmpDocTra2(ByVal plantaAdmin As Integer, ByVal idempresa As Integer) As List(Of ELL.FinSemana)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select fs000, fs001, fs002, fs003 FROM adok_fs  WHERE fs001 =  " & idempresa & " and fs099= " & plantaAdmin & " order by fs003 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.FinSemana)(Function(r As OracleDataReader) _
                                 New ELL.FinSemana With {.Id = CInt(r("fs000")), .empresa = SabLib.BLL.Utils.integerNull(r("fs001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("fs003"))}, query, CadenaConexion)
        End Function

        Public Function loadListTraDoc(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE doc005 = 1 and (doc011 is null or doc011 = 0) and doc099= " & plantaAdmin & " order by doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function
        Public Function loadListTraDocETT(ByVal plantaAdmin As Integer, ByVal codsol As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emd001, doc000, doc003, doc004  FROM adok_emdett, adok_doc  WHERE doc000=emd001 and doc010=1 and emd015 = " & codsol & " and doc099= " & plantaAdmin & " order by emd001 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("emd001")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004"))}, query, CadenaConexion)
        End Function
        Public Function loadListTraDocAutonomo(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE doc005 = 1 and (doc011 is null or doc011 = 0) and doc099= " & plantaAdmin & " order by doc012, doc000 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Periodo = SabLib.BLL.Utils.integerNull(r("doc003")), .Obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)
        End Function
        Public Function loadListTra(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra   order by tra003" 'WHERE tra099= " & plantaAdmin & "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                             New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Apellidos = SabLib.BLL.Utils.stringNull(r("tra003")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)
        End Function
        Public Function loadListTraActivos(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            'Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012, decode(tra008,0,0,1) as orden  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and (tra012 is null or tra012 = 0)    order by orden, tra003"

            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
            '                 New ELL.Trabajadores With {.estado2 = SabLib.BLL.Utils.integerNull(r("orden")), .Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Apellidos = SabLib.BLL.Utils.stringNull(r("tra003")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE  (tra012 is null or tra012 <> 1)    order by tra003" ' tra099= " & plantaAdmin & " and

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                             New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Apellidos = SabLib.BLL.Utils.stringNull(r("tra003")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)
        End Function
        Public Function loadListTraActivosMail(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE  (tra012 is null or tra012 <> 1) and (tra006 < to_date('01/01/2018','dd/MM/yyyy')) and (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate)    order by tra003" 'tra099= " & plantaAdmin & " and

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                             New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Apellidos = SabLib.BLL.Utils.stringNull(r("tra003")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)
        End Function
        Public Function loadListTraActivosNoCad(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE  (tra012 is null or tra012 <> 1)  and (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate)  order by tra003" ' tra099= " & plantaAdmin & " and

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                             New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Apellidos = SabLib.BLL.Utils.stringNull(r("tra003")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)
        End Function
        Public Function loadListTraActivosNoCadResponsable(ByVal plantaAdmin As Integer, ByVal responsables As Integer()) As List(Of ELL.Trabajadores)
            Dim strResponsables As String = ""
            For i = 0 To responsables.Count - 1
                If responsables(i) > 0 Then
                    strResponsables = strResponsables & responsables(i).ToString & ","
                End If

            Next
            strResponsables = strResponsables & "999999"

            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra000 in (" & strResponsables & ")  and (tra012 is null or tra012 <> 1)  and (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate)  order by tra003"  'and tra099= " & plantaAdmin & "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                             New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Apellidos = SabLib.BLL.Utils.stringNull(r("tra003")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)
        End Function
        Public Function loadListEmpDocTot(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009  FROM adok_doc  WHERE doc099= " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009"))}, query, CadenaConexion)


        End Function

        Public Function loadListEmpDoc(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)

            Dim query As String = "Select doc013, emd003, emd006, emd005, emd008, emd007  FROM adok_emd, adok_doc  WHERE doc000= emd001 and emd099= " & plantaAdmin & " AND emd000 = " & codemp & " AND emd001 = " & coddoc

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .clave = CInt(r("emd008")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007"))}, query, CadenaConexion)


        End Function
        Public Function loadListSolDoc(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select dos008  FROM adok_dos  WHERE dos099= " & plantaAdmin & " AND dos000 = " & codemp & " AND dos001 = " & coddoc

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = CInt(r("dos008"))}, query, CadenaConexion)


        End Function
        Public Function loadListSolDocETT(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select dos008  FROM adok_dosETT  WHERE dos099= " & plantaAdmin & " AND dos000 = " & codemp & " AND dos001 = " & coddoc

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = CInt(r("dos008"))}, query, CadenaConexion)


        End Function
        Public Function loadListFS(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal codtra As Integer, ByVal fecha As Date) As List(Of ELL.FinSemana)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim lParameters1 As New List(Of OracleParameter)
            Dim query As String = "Select fs000 FROM adok_fs  WHERE fs099= " & plantaAdmin & " AND fs001 = " & codemp & " AND fs003 = to_date('" & fecha & "','DD/MM/YYYY')"
            lParameters1.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.FinSemana)(Function(r As OracleDataReader) _
                             New ELL.FinSemana With {.Id = CInt(r("fs000"))}, query, CadenaConexion, lParameters1.ToArray)


        End Function
        Public Function loadListFS3(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal fecha As String) As List(Of ELL.FinSemana)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select fs001, fs002, fs003 FROM adok_fs  WHERE fs099= " & plantaAdmin & " and fs003 > sysdate AND fs001 = " & codemp & " AND fs003 = to_date('" & fecha & "','DD/MM/YYYY') order by fs003, fs001 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.FinSemana)(Function(r As OracleDataReader) _
                             New ELL.FinSemana With {.empresa = CInt(r("fs001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("fs003"))}, query, CadenaConexion)


        End Function
        Public Function loadListFS2(ByVal plantaAdmin As Integer, ByVal fecha As String) As List(Of ELL.FinSemana)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select fs001, fs002, fs003 FROM adok_fs  WHERE fs099= " & plantaAdmin & " and fs003 > sysdate AND fs003 = to_date('" & fecha & "','DD/MM/YYYY') order by fs003, fs001 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.FinSemana)(Function(r As OracleDataReader) _
                             New ELL.FinSemana With {.empresa = CInt(r("fs001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("fs003"))}, query, CadenaConexion)


        End Function
        Public Function loadListFS1(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.FinSemana)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select fs001, fs002, fs003 FROM adok_fs  WHERE fs099= " & plantaAdmin & " and fs003 > sysdate AND fs001 = " & codemp & " order by fs003, fs001"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.FinSemana)(Function(r As OracleDataReader) _
                             New ELL.FinSemana With {.empresa = CInt(r("fs001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("fs003"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDoc(ByVal plantaAdmin As Integer, ByVal codtra As Integer, ByVal coddoc As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select trd016, trd013, trd002, trd004,  trd006,  trd007  FROM adok_trd  WHERE trd099= " & plantaAdmin & "   AND trd000 = " & codtra & " AND trd001 = " & coddoc

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.FecRev = SabLib.BLL.Utils.dateTimeNull(r("trd016")), .tiposCarne = SabLib.BLL.Utils.stringNull(r("TRD013")), .correcto = CInt(r("trd002")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .ubicacion = SabLib.BLL.Utils.stringNull(r("TRD006")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007"))}, query, CadenaConexion)


        End Function

        Public Function loadListTraSol(ByVal plantaAdmin As Integer, ByVal codtra As Integer, ByVal codsol As Integer, ByVal codemp As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select trs008  FROM adok_trs  WHERE trs099= " & plantaAdmin & " AND trs000 = " & codtra & " AND trs001 = " & codsol & " AND trs002 = " & codemp

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.clave = SabLib.BLL.Utils.integerNull(r("trs008"))}, query, CadenaConexion)


        End Function
        'Public Function loadListTraSolETT(ByVal plantaAdmin As Integer, ByVal codtra As Integer, ByVal codsol As Integer, ByVal codemp As Integer) As List(Of ELL.TrabajadoresDoc)

        '    Dim query As String = "Select trs008  FROM adok_trsETT  WHERE trs099= " & plantaAdmin & " AND trs000 = " & codtra & " AND trs001 = " & codsol & " AND trs002 = " & codemp

        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
        '                     New ELL.TrabajadoresDoc With {.clave = CInt(r("trs008"))}, query, CadenaConexion)


        'End Function
        Public Function loadListTraSol2(ByVal plantaAdmin As Integer, ByVal codsol As Integer, ByVal codemp As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select trs008  FROM adok_trs  WHERE trs099= " & plantaAdmin & " AND trs001 = " & codsol & " AND trs002 = " & codemp

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.clave = CInt(r("trs008"))}, query, CadenaConexion)


        End Function

        Public Function loadListTraSolClave(ByVal plantaAdmin As Integer, ByVal codsol As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select trs000  FROM adok_trs  WHERE trs099= " & plantaAdmin & " AND trs001 = " & codsol

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.clave = CInt(r("trs000"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpDocAsignados(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select decode(doc012,0,0,1,1,2,2,3,3,  4    ,4+3*doc014,5) as orden, adok_doc.doc014, adok_emd.emd012, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_emd.emd002, adok_emd.emd003, adok_emd.emd004, adok_emd.emd005, adok_emd.emd006, adok_emd.emd007, adok_emd.emd009, adok_emd.emd011 FROM adok_doc, adok_emd  WHERE adok_emd.emd099 = " & plantaAdmin & " and adok_emd.emd001 = adok_doc.doc000 AND adok_emd.emd000 = " & codemp & " AND adok_emd.EMD013 <> 1 and (adok_emd.emd014 is null or adok_emd.emd014 > sysdate)  and doc005=0  order by orden, adok_doc.doc000"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.vacio = "True", .Planta = SabLib.BLL.Utils.integerNull(r("doc014")), .Comentario = SabLib.BLL.Utils.stringNull(r("emd012")), .clave = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .tipodoc = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpDocAsignadosMenor4(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select decode(doc012,0,0,1,1,2,2,3,3,  4    ,4+3*doc014,5) as orden, adok_doc.doc014, adok_emd.emd012, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_emd.emd002, adok_emd.emd003, adok_emd.emd004, adok_emd.emd005, adok_emd.emd006, adok_emd.emd007, adok_emd.emd009, adok_emd.emd011 FROM adok_doc, adok_emd  WHERE adok_emd.emd001 = adok_doc.doc000 and adok_doc.doc011 = 0 and doc012 < 4 and  adok_emd.emd099 = " & plantaAdmin & " AND adok_emd.emd000 = " & codemp & " AND adok_emd.EMD013 <> 1 and (adok_emd.emd014 is null or adok_emd.emd014 > sysdate)  " & " and doc005=0  order by orden, adok_doc.doc000"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.vacio = "True", .Planta = SabLib.BLL.Utils.integerNull(r("doc014")), .Comentario = SabLib.BLL.Utils.stringNull(r("emd012")), .clave = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .tipodoc = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpDocAsignadosMenor4ETT(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal soli As Integer) As List(Of ELL.EmpresasDoc)

            'NO PONGO AND emd000 = " & codemp & " porque la empresa es siempre la ett y no a la quie se hace
            Dim query As String = "Select decode(doc012,0,0,1,1,2,2,3,3,  4    ,4+3*doc014,5) as orden, doc014, emd012, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012, doc013, emd002, emd003, emd004, emd005, emd006, emd007, emd009, emd011 FROM adok_doc, adok_emdETT  WHERE emd001 = doc000 and doc010=1 and  emd099 = " & plantaAdmin & "  AND emd015 = " & soli & " AND EMD013 <> 1 and (emd014 is null or emd014 > sysdate)  " & " and doc005=0  order by orden, doc000"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.vacio = "True", .Planta = SabLib.BLL.Utils.integerNull(r("doc014")), .Comentario = SabLib.BLL.Utils.stringNull(r("emd012")), .clave = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .tipodoc = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpDocAsignadosTipo4(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select adok_emd.emd012, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_emd.emd002, adok_emd.emd003, adok_emd.emd004, adok_emd.emd005, adok_emd.emd006, adok_emd.emd007, adok_emd.emd009, adok_emd.emd011 FROM adok_doc, adok_emd  WHERE adok_emd.emd001 = adok_doc.doc000 and adok_doc.doc011 = 0 and  adok_emd.emd099 = " & plantaAdmin & " AND adok_emd.emd000 = " & codemp & " AND adok_emd.EMD013 <> 1 and (adok_emd.emd014 is null or adok_emd.emd014 > sysdate) and doc012 = 4 " & " order by adok_doc.doc012, adok_doc.doc000"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("emd012")), .clave = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .tipodoc = SabLib.BLL.Utils.integerNull(r("doc012"))}, query, CadenaConexion)


        End Function


        Public Function loadListEmpDocAsignadosTra(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select  adok_emd.emd001, adok_emd.emd002, adok_emd.emd003, adok_emd.emd004, adok_emd.emd005, adok_emd.emd006, adok_emd.emd007, adok_emd.emd009, adok_emd.emd011 FROM adok_emd  WHERE  adok_emd.emd099 = " & plantaAdmin & " AND adok_emd.emd000 = " & codemp & " AND adok_emd.EMD013 = 1 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .coddoc = SabLib.BLL.Utils.integerNull(r("emd001")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpDocAsignadosTraCer3(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer, ByVal fecha As Date) As List(Of ELL.EmpresasDoc)
            Dim lParameters1 As New List(Of OracleParameter)
            lParameters1.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))

            Dim query As String = "Select  adok_cer.cer000, adok_cer.cer002, adok_cer.cer003 FROM adok_cer  WHERE  adok_cer.cer099 = " & plantaAdmin & " AND adok_cer.cer000 = " & coddoc & " AND adok_cer.cer001 = " & codemp & " AND adok_cer.cer003 >=:fecha "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("cer000"))}, query, CadenaConexion, lParameters1.ToArray)


        End Function
        Public Function loadListEmpDocAsignadosTraCer2(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select  adok_cer.cer000, adok_cer.cer002, adok_cer.cer003 FROM adok_cer  WHERE  adok_cer.cer099 = " & plantaAdmin & " AND adok_cer.cer000 = " & coddoc & " AND adok_cer.cer001 = " & codemp & " AND adok_cer.cer002 = 0"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("cer000")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("cer003"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpDocAsignadosTraCertipo5(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select  adok_cer.cer000, adok_cer.cer002, adok_cer.cer003 FROM adok_cer  WHERE  adok_cer.cer099 = " & plantaAdmin & " AND adok_cer.cer000 = " & coddoc & " AND adok_cer.cer001 = " & codemp & " AND adok_cer.cer002 = 0 and adok_cer.cer003 > (Select max(pla002) from adok_pla where pla000=" & coddoc & ") "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("cer000")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("cer003"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpDocAsignadosTraCer(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select  adok_cer.cer000, adok_cer.cer002, adok_cer.cer003 FROM adok_cer  WHERE  adok_cer.cer099 = " & plantaAdmin & " AND adok_cer.cer000 = " & coddoc & " AND adok_cer.cer001 = " & codemp & " AND (adok_cer.cer002 = 1 and  adok_cer.cer003 > add_months(trunc(sysdate), -12))"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("cer000"))}, query, CadenaConexion)


        End Function

        Public Function loadListEmpDocAsignados161(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_emd.emd000, adok_emd.emd002, adok_emd.emd003, adok_emd.emd004, adok_emd.emd005, adok_emd.emd006, adok_emd.emd007, adok_emd.emd009, adok_emd.emd011 FROM adok_doc, adok_emd, adok_emp  WHERE adok_emd.emd005 IS NOT NULL and (emd009 > 1 and emd009 < 4) and adok_emp.emp000 = adok_emd.emd000 and adok_emp.emp018 = 0 and  adok_emd.emd001 = adok_doc.doc000 and adok_doc.doc011 = 0 AND  adok_doc.doc000 = 161  and  adok_emd.emd099 = " & plantaAdmin & "  AND adok_emd.EMD013 <> 1 " & " order by emd002 desc"  'AND  adok_emd.emd009 = 4 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = SabLib.BLL.Utils.stringNull(r("emd000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .codemp = SabLib.BLL.Utils.stringNull(r("emd000")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


        End Function
        Public Function loadListTodosEmpDocAsignados(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_emd.emd000, adok_emd.emd002, adok_emd.emd003, adok_emd.emd004, adok_emd.emd005, adok_emd.emd006, adok_emd.emd007, adok_emd.emd009, adok_emd.emd011 FROM adok_doc, adok_emd, adok_emp  WHERE adok_emd.emd005 IS NOT NULL and (emd003 <> 0 or (emd009 <> 1 and (doc000 = 161 or doc000 = 163 ))) and adok_emp.emp000 = adok_emd.emd000 and adok_emp.emp018 = 0 and  adok_emd.emd001 = adok_doc.doc000 and adok_doc.doc011 = 0 AND adok_emd.EMD013 <> 1  and  adok_emd.emd099 = " & plantaAdmin & "  order by emd002 desc"  'AND  adok_emd.emd009 = 4 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("emd000")) & ";" & SabLib.BLL.Utils.stringNull(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .codemp = SabLib.BLL.Utils.stringNull(r("emd000")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


        End Function

        Public Function loadListEmpDocAsignados163(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_emd.emd000, adok_emd.emd002, adok_emd.emd003, adok_emd.emd004, adok_emd.emd005, adok_emd.emd006, adok_emd.emd007, adok_emd.emd009, adok_emd.emd011 FROM adok_doc, adok_emd, adok_emp  WHERE adok_emd.emd005 Is Not NULL And (emd009 > 1 And emd009 < 4) And adok_emp.emp000 = adok_emd.emd000 And adok_emp.emp018 = 0 And  adok_emd.emd001 = adok_doc.doc000 And adok_doc.doc011 = 0 And  adok_doc.doc000 = 163 And  adok_emd.emd099 = " & plantaAdmin & " And   adok_emd.EMD013 <> 1 " & " order by emd002 desc" 'AND  adok_emd.emd009 = 4 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = SabLib.BLL.Utils.stringNull(r("emd000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .codemp = SabLib.BLL.Utils.stringNull(r("emd000")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocAsignados(ByVal plantaadmin As Integer, ByVal codtra As Integer) As List(Of ELL.TrabajadoresDoc)


            '   Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd  WHERE adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0 And  adok_trd.trd099 = " & plantaAdmin & " And adok_trd.trd000 = " & codtra & " order by adok_doc.doc012, adok_doc.doc000"
            Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011, adok_trd.trd013 FROM adok_doc, adok_trd  WHERE trd099= " & plantaadmin & " and trd015 = 1 and (adok_trd.trd009 is null or adok_trd.trd009 =0 or adok_trd.trd009 > 1) and adok_trd.trd001 = adok_doc.doc000 And adok_trd.trd000 = " & codtra
            'query = query & " UNION Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011, adok_trd.trd013 FROM adok_doc, adok_trd, adok_sol  WHERE sol000 = trd010 And adok_trd.trd001 = adok_doc.doc000 And sol004 > sysdate And adok_doc.doc011 = 0 And  adok_trd.trd099 = " & plantaAdmin & " And adok_trd.trd000 = " & codtra & " And trd010 Is Not null And (trd014 Is null Or trd014 > sysdate)  "
            query = query & " order by doc012, doc000 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.clave = CInt(r("doc000")), .Planta = SabLib.BLL.Utils.integerNull(r("doc005")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .Comentario = SabLib.BLL.Utils.stringNull(r("trd013"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocAsignadosMatriz(ByVal coddoc As Integer, ByVal codtra As Integer, ByVal plantaadmin As Integer) As List(Of ELL.TrabajadoresDoc)


            '   Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd  WHERE adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0 And  adok_trd.trd099 = " & plantaAdmin & " And adok_trd.trd000 = " & codtra & " order by adok_doc.doc012, adok_doc.doc000"
            Dim query As String = "Select adok_trd.trd015, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011, adok_trd.trd013 FROM adok_doc, adok_trd  WHERE adok_trd.trd099 = " & plantaadmin & " and (adok_trd.trd009 is null or adok_trd.trd009 =0  or adok_trd.trd009 =5) and adok_trd.trd001 = adok_doc.doc000 And  adok_trd.trd000 = " & codtra & " And  adok_trd.trd001 = " & coddoc
            'query = query & " UNION Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011, adok_trd.trd013 FROM adok_doc, adok_trd, adok_sol  WHERE sol000 = trd010 And adok_trd.trd001 = adok_doc.doc000 And sol004 > sysdate And adok_doc.doc011 = 0 And  adok_trd.trd099 = " & plantaAdmin & " And adok_trd.trd000 = " & codtra & " And trd010 Is Not null And (trd014 Is null Or trd014 > sysdate)  "
            query = query & " order by doc012, doc000 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.necesario = SabLib.BLL.Utils.integerNull(r("trd015")), .clave = CInt(r("doc000")), .Planta = SabLib.BLL.Utils.integerNull(r("doc005")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .Comentario = SabLib.BLL.Utils.stringNull(r("trd013"))}, query, CadenaConexion)


        End Function
        Public Function loadListTodosTraDocAsignadosNew(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE trd002 <> 0  And  adok_tra.tra000 = adok_trd.trd000 And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_trd.trd006 Is Not NULL  And  adok_trd.trd099 = " & plantaAdmin & "  order by adok_trd.trd003 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("trd000")) & ";" & SabLib.BLL.Utils.stringNull(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .codtra = SabLib.BLL.Utils.stringNull(r("trd000")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


            'Dim query As String = "Select adok_doc.doc000  FROM adok_doc WHERE  adok_doc.doc011 = 0  "

            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
            '                 New ELL.TrabajadoresDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("doc000"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresClaveTraActivosNew(ByVal plantaAdmin As Integer, ByVal clave As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "Select tra000, tra001, tra013,tra014, tra015, tra002, tra003, tra004, tra005, tra006, tra007, tra008,  tra010,  tra011, tra012  FROM adok_tra  WHERE tra000 = " & clave & " order by tra003 asc" 'tra099= " & plantaAdmin & " and 


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .puesto = SabLib.BLL.Utils.stringNull(r("tra011")), .funcion = SabLib.BLL.Utils.stringNull(r("tra007")), .solicitud = SabLib.BLL.Utils.stringNull(r("tra014")), .Autonomo = SabLib.BLL.Utils.integerNull(r("tra015")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("tra005")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocAsignadosMatrizTodos(ByVal coddoc As Integer, ByVal codtra As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select adok_trd.trd015, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011, adok_trd.trd013 FROM adok_doc, adok_trd  WHERE adok_trd.trd099 = " & plantaAdmin & " and  adok_trd.trd001 = adok_doc.doc000 And  adok_trd.trd000 = " & codtra & " And  adok_trd.trd001 = " & coddoc

            query = query & " order by doc012, doc000 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.necesario = SabLib.BLL.Utils.integerNull(r("trd015")), .clave = CInt(r("doc000")), .Planta = SabLib.BLL.Utils.integerNull(r("doc005")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013")), .Comentario = SabLib.BLL.Utils.stringNull(r("trd013"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocObligatorio(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select emd008 FROM adok_emd  WHERE emd099 = " & plantaAdmin & " And emd000 = " & codemp & " And emd001 = " & coddoc

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = CInt(r("emd008"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocObligatorioTRD(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codtra As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select trd015 FROM adok_trd  WHERE trd099 = " & plantaAdmin & " And trd001 = " & coddoc & " And trd000 = " & codtra

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = CInt(r("trd015"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocObligatorio2(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)

            'select doc000 from adok_doc where doc012=4 and doc014=0
            Dim query As String = "Select doc000, doc003 from adok_doc where  doc014=0 And doc099 = " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = CInt(r("doc000")), .periodicidad = SabLib.BLL.Utils.integerNull(r("doc003"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocObligatorio3(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select cer000 from adok_cer where cer099 = " & plantaAdmin & " And  cer000  = " & coddoc & " And cer001  = " & codemp & " And cer002 = 0  "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.clave = CInt(r("cer000"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocObligatorio4(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codtra As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select trd003 from adok_trd where trd099 = " & plantaAdmin & " And (trd014 Is null Or trd014 > sysdate)  And trd001 = " & coddoc & "  And trd000 = " & codtra & " order by trd003 desc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocObligatorio5(ByVal plantaAdmin As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)


            Dim query As String = "Select pla002, pla003 from adok_pla where pla099 = " & plantaAdmin & "  And pla000 = " & coddoc & " order by pla002 desc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.FecRec = SabLib.BLL.Utils.dateTimeNull(r("pla002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("pla003"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraDocAsignados156(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)


            Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE ((trd009 > 1 And trd009 < 5) Or trd009=6) And adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0 And  adok_trd.trd099 = " & plantaAdmin & " And adok_trd.trd006 Is Not NULL And adok_doc.doc000 = 156  And adok_doc.doc005=1 And (trd014 Is null Or trd014 > sysdate)    order by adok_trd.trd003 desc"  'adok_doc.doc000 = 156   AND  adok_trd.trd009 = 5

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.clave = SabLib.BLL.Utils.stringNull(r("trd000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .codtra = SabLib.BLL.Utils.stringNull(r("trd000")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


        End Function
        Public Function loadListTodosTraDocAsignados(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)


            Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE (trd002 <> 0 Or (trd009 <> 1 And doc000 = 156 )) And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_trd.trd006 Is Not NULL  And  adok_trd.trd099 = " & plantaAdmin & "  order by adok_trd.trd003 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("trd000")) & ";" & SabLib.BLL.Utils.stringNull(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .codtra = SabLib.BLL.Utils.stringNull(r("trd000")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .obligatorio = SabLib.BLL.Utils.integerNull(r("doc004")), .Margen = SabLib.BLL.Utils.integerNull(r("doc008")), .plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc012")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .Listacorreos = SabLib.BLL.Utils.stringNull(r("doc013"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatriz(ByVal codpuesto As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)


            Dim query As String = "Select emd001, doc002 from adok_emd, adok_doc where emd099 = " & plantaAdmin & " and emd001 = doc000 and emd000 = " & codpuesto & " order by emd001 "
            '  Dim query As String = "Select distinct(trd010) as annos from adok_trd where trd000 = " & codpuesto & " order by trd010 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("emd001")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc002"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatriz2(ByVal codpuesto As String, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)


            'Dim query As String = "Select emd001, doc002 from adok_emd, adok_doc where emd001 = doc000 and emd000 = " & codpuesto & " order by emd001 "
            Dim query As String = "Select distinct(trd010) as annos from adok_trd_hist where trd010 is not null and trd000 in (" & codpuesto & ") and trd099 =  " & plantaAdmin & " order by annos "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("annos"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatriz3(ByVal codpuesto As String, ByVal curso As Integer, ByVal todos As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)


            'Dim query As String = "Select emd001, doc002 from adok_emd, adok_doc where emd001 = doc000 and emd000 = " & codpuesto & " order by emd001 "
            Dim query As String = "Select distinct(trd010) as annos from adok_trd_hist where trd010 is not null and trd000 in (" & codpuesto & ") and trd099 =  " & plantaAdmin & "  and trd001 = " & curso


            If todos = 1 Then
                query = query & " and trd005 < sysdate   "
            End If
            If todos = 2 Then
                Dim inicad As Date
                inicad = Now
                inicad = DateAdd(DateInterval.Month, 3, inicad)
                '       inicad = DateAdd(DateInterval.Year, 1, inicad)
                Dim lParameters3 As New List(Of OracleParameter)
                lParameters3.Add(New OracleParameter("FecCad", OracleDbType.Date, inicad, ParameterDirection.Input))

                query = query & " and trd005 < :FecCad and trd005 > sysdate  "
                query = query & " order by annos "
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("annos"))}, query, CadenaConexion, lParameters3.ToArray)

            Else
                query = query & "  order by annos "
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("annos"))}, query, CadenaConexion)

            End If



        End Function
        'Public Function loadListMatrizTrabajadores2(ByVal Intrabajador As String, ByVal profesion As Integer) As List(Of ELL.TrabajadoresDoc)

        '    '         Intrabajador = "57803, 7628"
        '    'Dim query As String = "Select emd001, doc002 from adok_emd, adok_doc where emd001 = doc000 and emd000 = " & codpuesto & " order by emd001 "
        '    Dim query As String = "Select distinct(trd000) as trabajadores, tra004 from adok_trd_hist, adok_tra where tra000 = trd000 and tra004= " & profesion & " And trd010 Is Not null And trd008 In (" & Intrabajador & ") order by trabajadores "
        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
        '                     New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("trabajadores")), .clave = SabLib.BLL.Utils.integerNull(r("tra004"))}, query, CadenaConexion)


        'End Function
        Public Function loadListMatrizTrabajadores(ByVal Intrabajador As String, ByVal profesion As Integer, ByVal responsable As Integer, ByVal todos As Integer) As List(Of ELL.TrabajadoresDoc)
            Dim query As String = "Select distinct(trd000) as trabajadores from adok_trd_hist where  trd010 Is Not null And trd008 In (" & Intrabajador & ")  "
            If profesion = 0 And responsable = 0 Then
                query = "Select distinct(trd000) as trabajadores from adok_trd_hist where  trd010 Is Not null And trd008 In (" & Intrabajador & ")   "
            End If
            If profesion <> 0 And responsable = 0 Then
                query = "Select distinct(trd000) as trabajadores from adok_trd_hist left join adok_tra on trd000 = tra000 where     tra004= " & profesion & " And trd010 Is Not null And trd008 In (" & Intrabajador & ")  "
            End If
            If profesion = 0 And responsable <> 0 Then
                query = "Select distinct(trd000) as trabajadores from adok_trd_hist left join adok_tra on trd000 = tra000 where  tra008= " & responsable & " And trd010 Is Not null And trd008 In (" & Intrabajador & ")  "
            End If
            If profesion <> 0 And responsable <> 0 Then
                query = "Select distinct(trd000) as trabajadores from adok_trd_hist left join adok_tra on trd000 = tra000 where    tra004= " & profesion & " and tra008= " & responsable & " And trd010 Is Not null And trd008 In (" & Intrabajador & ")  "
            End If

            If todos = 1 Then
                query = query & " and trd005 < sysdate   "
            End If
            If todos = 2 Then
                Dim inicad As Date
                inicad = Now
                inicad = DateAdd(DateInterval.Month, 3, inicad)
                '     inicad = DateAdd(DateInterval.Year, 1, inicad)
                Dim lParameters3 As New List(Of OracleParameter)
                lParameters3.Add(New OracleParameter("FecCad", OracleDbType.Date, inicad, ParameterDirection.Input))

                query = query & " and trd005 < :FecCad and trd005 > sysdate  "
                query = query & " order by trabajadores "
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("trabajadores"))}, query, CadenaConexion, lParameters3.ToArray)

            Else
                query = query & " order by trabajadores "
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("trabajadores"))}, query, CadenaConexion)

            End If



        End Function
        Public Function loadListMatrizCadaTra(ByVal codpuesto As Integer, ByVal codpuestos As Integer) As List(Of ELL.TrabajadoresDoc)


            'Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE (trd002 <> 0 Or (trd009 <> 1 And doc000 = 156 )) And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_trd.trd006 Is Not NULL   order by adok_trd.trd003 desc"
            ' Dim query As String = "Select tra002, tra003, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE adok_tra.tra099 = " & plantaAdmin & " And   adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)    order by tra003 asc"
            Dim query As String = "Select doc000, trd000, emd001, doc002 from adok_emd, adok_doc,adok_trd where emd001 = doc000 And   adok_tra.tra000 = adok_trd.trd000 And emd000 = " & codpuesto & " order by emd001 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("trd000")) & ";" & SabLib.BLL.Utils.stringNull(r("doc000")), .coddoc = SabLib.BLL.Utils.integerNull(r("emd001")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc002"))}, query, CadenaConexion)


        End Function
        Public Function loadListHist(ByVal coddoc As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select tra002, tra003, trd000, trd006, doc002 , trd010, trd013 from adok_trd_hist, adok_doc, adok_tra  where tra000= trd000  And trd001 = doc000 And  trd008 = " & coddoc
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.codtra = SabLib.BLL.Utils.integerNull(r("trd000")), .NIF = SabLib.BLL.Utils.stringNull(r("doc002")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .nomtra = SabLib.BLL.Utils.stringNull(r("trd013")), .clave = SabLib.BLL.Utils.integerNull(r("trd010")), .txtcorrecto = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListMat() As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select curso, valores from matriz "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.NIF = SabLib.BLL.Utils.stringNull(r("curso")), .Abrev = SabLib.BLL.Utils.stringNull(r("valores"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatClave(ByVal clave As String) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select curso, valores from matriz where curso = '" & clave & "'"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.NIF = SabLib.BLL.Utils.stringNull(r("curso")), .Abrev = SabLib.BLL.Utils.stringNull(r("valores"))}, query, CadenaConexion)


        End Function
        Public Function loadListHistFec(ByVal coddoc As String) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select trd004 from adok_trd_hist  where trd006 = '" & coddoc & "'"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatrizTra(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDocMatriz)


            'Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE (trd002 <> 0 Or (trd009 <> 1 And doc000 = 156 )) And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_trd.trd006 Is Not NULL   order by adok_trd.trd003 desc"
            'Dim query As String = "Select distinct (tra000) As tra000, tra003 from (Select tra000, tra002, tra003, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE adok_tra.tra099 = " & plantaAdmin & " And   adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_doc.doc018 > 0     ) order by tra003 asc "
            Dim query As String = "Select  tra000, tra003 from adok_tra order by tra003 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDocMatriz)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDocMatriz With {.codtra = SabLib.BLL.Utils.stringNull(r("tra000"))}, query, CadenaConexion)


        End Function

        Public Function loadListMatrizTraEmp(ByVal plantaAdmin As Integer, ByVal empresa As Integer) As List(Of ELL.TrabajadoresDocMatriz)

            'Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE (trd002 <> 0 Or (trd009 <> 1 And doc000 = 156 )) And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_trd.trd006 Is Not NULL   order by adok_trd.trd003 desc"
            '        Dim query As String = "Select distinct (tra000) As tra000, tra003 from (Select tra000, tra002, tra003, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE adok_tra.tra099 = " & plantaAdmin & " And  tra004 = " & empresa & " And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)     ) order by tra003 asc "
            Dim query As String = "Select  tra000, tra013, tra002, tra003 from adok_tra where tra004 =  " & empresa & " order by tra003 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDocMatriz)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDocMatriz With {.codtra = SabLib.BLL.Utils.stringNull(r("tra000")), .NIF = SabLib.BLL.Utils.stringNull(r("tra013")), .nomtra = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatrizTraEmpTra(ByVal plantaAdmin As Integer, ByVal empresa As Integer, ByVal puesto As Integer) As List(Of ELL.TrabajadoresDocMatriz)

            'Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE (trd002 <> 0 Or (trd009 <> 1 And doc000 = 156 )) And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_trd.trd006 Is Not NULL   order by adok_trd.trd003 desc"
            '        Dim query As String = "Select distinct (tra000) As tra000, tra003 from (Select tra000, tra002, tra003, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE adok_tra.tra099 = " & plantaAdmin & " And  tra004 = " & empresa & " And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)     ) order by tra003 asc "
            Dim query As String = "Select  tra000, tra013, tra002, tra003 from adok_tra where tra000 =  " & empresa & " and tra004 = " & puesto & " order by tra003 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDocMatriz)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDocMatriz With {.codtra = SabLib.BLL.Utils.stringNull(r("tra000")), .NIF = SabLib.BLL.Utils.stringNull(r("tra013")), .nomtra = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatrizTrabajador(ByVal plantaAdmin As Integer, ByVal empresa As Integer) As List(Of ELL.TrabajadoresDocMatriz)
            'to_number(to_char(trd004, 'YYYY'))
            'Dim query As String = "Select adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE (trd002 <> 0 Or (trd009 <> 1 And doc000 = 156 )) And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)  And adok_trd.trd006 Is Not NULL   order by adok_trd.trd003 desc"
            '        Dim query As String = "Select distinct (tra000) As tra000, tra003 from (Select tra000, tra002, tra003, adok_doc.doc000, adok_doc.doc001, adok_doc.doc002, adok_doc.doc003, adok_doc.doc004, adok_doc.doc005, adok_doc.doc006, adok_doc.doc007, adok_doc.doc008, adok_doc.doc009, adok_doc.doc012, adok_doc.doc013, adok_trd.trd000, adok_trd.trd002, adok_trd.trd003, adok_trd.trd004, adok_trd.trd005, adok_trd.trd006, adok_trd.trd007, adok_trd.trd009, adok_trd.trd011 FROM adok_doc, adok_trd, adok_tra  WHERE adok_tra.tra099 = " & plantaAdmin & " And  tra004 = " & empresa & " And  adok_tra.tra000 = adok_trd.trd000 And (adok_tra.tra012 Is null Or adok_tra.tra012 <> 1) And  adok_trd.trd001 = adok_doc.doc000 And adok_doc.doc011 = 0  And (trd014 Is null Or trd014 > sysdate)     ) order by tra003 asc "
            Dim query As String = "Select doc002, trd000, trd013, trd002, trd003, trd010 As anno   from adok_trd, adok_doc where trd001 = doc000 And trd006 Is Not null And trd000 =  " & empresa & " order by trd000 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDocMatriz)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDocMatriz With {.doc1 = SabLib.BLL.Utils.integerNull(r("anno")), .NIF = SabLib.BLL.Utils.stringNull(r("doc002")), .nomtra = SabLib.BLL.Utils.stringNull(r("trd013")), .codtra = SabLib.BLL.Utils.stringNull(r("trd000"))}, query, CadenaConexion)


        End Function
        Public Function loadListMatrizDoc(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)

            Dim query As String = "Select doc000, doc001 from adok_doc where doc099 = " & plantaAdmin & " and doc011 = 0  order by doc001 asc "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = SabLib.BLL.Utils.stringNull(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001"))}, query, CadenaConexion)


        End Function

        Public Function loadListEmpresas(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emp000, emp001, emp002, emp003, emp004, emp005, emp006, emp007, emp008, emp009, emp018  FROM adok_emp  WHERE emp099= " & plantaAdmin & " Order by emp018 ASC, emp002 asc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nif = SabLib.BLL.Utils.stringNull(r("emp001")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002")), .activo = SabLib.BLL.Utils.integerNull(r("emp018"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasActivas(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emp000, emp001  FROM adok_emp  Order by emp002 ASC "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp001"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasActivasConTra(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            'Dim query As String = "Select distinct(emp000),  emp001, emp002, emp003, emp004, emp005, emp006, emp007, emp008, emp009, emp018 from "
            'query = query & " (Select tra000, emp000, emp001, emp002, emp003, emp004, emp005, emp006, emp007, emp008, emp009, emp018  FROM adok_emp2, adok_tra  WHERE (tra012 Is null Or tra012 <> 1)  And emp099= " & plantaAdmin & "  And (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate)  and tra004=emp000 And emp018 = 0 And emp099= 1 Order by emp018 ASC, emp002 asc ) order by emp002 asc "

            Dim query As String = "Select emp000, emp002  FROM adok_emp order by emp002 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasActivasConTraResponsable(ByVal plantaAdmin As Integer, ByVal responsables As Integer()) As List(Of ELL.Empresas)

            Dim strResponsables As String = ""
            For i = 0 To responsables.Count - 1
                If responsables(i) > 0 Then
                    strResponsables = strResponsables & responsables(i).ToString & ","
                End If

            Next
            strResponsables = strResponsables & "0"

            'strResponsables = "193,78,2,193,193,2,193,2,0"

            Dim query As String = "Select emp000, emp002  FROM adok_emp where emp000 in (" & strResponsables & ") order by emp002 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadContaTrabajadores(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "Select count(tra000) as contador  from adok_tra   WHERE tra012 = 0 " ' and tra099= " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                             New ELL.Trabajadores With {.Id = CInt(r("contador"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emp000, emp001, emp002 FROM adok_emp  WHERE UPPER(emp002) like UPPER('%" & texto & "%') Order by emp002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadListDocumentosTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc002, doc011  FROM adok_doc  WHERE doc099= " & plantaAdmin & " and UPPER(doc002) like UPPER('%" & texto & "%') Order by doc002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                               New ELL.Documentos With {.Id = CInt(r("doc000")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc002")), .activo = SabLib.BLL.Utils.integerNull(r("doc011"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasTextoActivas(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emp000, emp001, emp002  FROM adok_emp  WHERE UPPER(emp002) like UPPER('%" & texto & "%') Order by emp002 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasTextoActivasResponsables(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal responsables As Integer()) As List(Of ELL.Empresas)
            Dim strResponsables As String = ""
            For i = 0 To responsables.Count - 1
                If responsables(i) > 0 Then
                    strResponsables = strResponsables & responsables(i).ToString & ","
                End If

            Next
            strResponsables = strResponsables & "0"

            Dim query As String = "Select emp000, emp001, emp002  FROM adok_emp  WHERE  emp000 in (" & strResponsables & ") and UPPER(emp002) like UPPER('%" & texto & "%') Order by emp002 "
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("emp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadListEmpresasTextoActivasExacto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select emp000, emp001, emp002, emp018  FROM adok_emp  WHERE emp018 = 0 and emp099= " & plantaAdmin & " and UPPER(emp002) = UPPER('" & texto & "') Order by emp002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("emp000")), .Nif = SabLib.BLL.Utils.stringNull(r("emp001")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002")), .activo = SabLib.BLL.Utils.integerNull(r("emp018"))}, query, CadenaConexion)


        End Function
        Public Function loadListCursosTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)

            Dim query As String = "Select doc000, doc001, doc002  FROM adok_doc  WHERE doc099 = " & plantaAdmin & " and UPPER(doc001) like UPPER('%" & texto & "%') Order by doc002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("doc000")), .nDNI = SabLib.BLL.Utils.stringNull(r("doc001")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001"))}, query, CadenaConexion)


        End Function
        Public Function loadListProfesionTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)

            Dim query As String = "Select emp000, emp002  FROM adok_emp  WHERE  UPPER(emp002) like UPPER('%" & texto & "%') Order by emp002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("emp000")), .nDNI = SabLib.BLL.Utils.stringNull(r("emp002")), .Nombre = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)


        End Function
        Public Function loadListResponsablesTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)

            Dim query As String = "Select distinct tra011  FROM adok_tra  WHERE  UPPER(tra011) like UPPER('%" & texto & "%') Order by tra011 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Nombre = SabLib.BLL.Utils.stringNull(r("tra011"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)

            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE (tra012 is null or tra012 <> 1) and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like ('%" & texto.ToUpper & "%')" & " ) Order by tra003 "   'tra099= " & plantaAdmin & " and 
            ' Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & "  and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresTextoPuesto(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal Puesto As Integer) As List(Of ELL.Trabajadores)

            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra004 = " & Puesto & " and (tra012 is null or tra012 <> 1) and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like ('%" & texto.ToUpper & "%')" & " ) Order by tra003 " '" and tra099= " & plantaAdmin &

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresTextoResponsables(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal responsables As Integer()) As List(Of ELL.Trabajadores)

            Dim strResponsables As String = ""
            For i = 0 To responsables.Count - 1
                If responsables(i) > 0 Then
                    strResponsables = strResponsables & responsables(i).ToString & ","
                End If

            Next
            strResponsables = strResponsables & "0"

            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra000 in (" & strResponsables & ") and (tra012 is null or tra012 <> 1) and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like ('%" & texto.ToUpper & "%')" & " ) Order by tra003 " ' and tra099= " & plantaAdmin & " 
            ' Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & "  and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresTextoResponsables2(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal responsables As Integer(), ByVal puestos As Integer()) As List(Of ELL.Trabajadores)

            Dim strResponsables As String = ""
            For i = 0 To responsables.Count - 1
                If responsables(i) > 0 Then
                    strResponsables = strResponsables & responsables(i).ToString & ","
                End If

            Next
            strResponsables = strResponsables & "0"

            Dim strPuestos As String = ""
            For i = 0 To puestos.Count - 1
                If puestos(i) > 0 Then
                    strPuestos = strPuestos & puestos(i).ToString & ","
                End If

            Next
            strPuestos = strPuestos & "0"
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra000 in (" & strResponsables & ") and tra004 in (" & strPuestos & ") and tra099= " & plantaAdmin & " and (tra012 is null or tra012 <> 1) and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like ('%" & texto.ToUpper & "%')" & " ) Order by tra003 "
            ' Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & "  and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function



        Public Function CargarListaDoctexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.TrabajadoresDoc)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select doc000, doc005 FROM adok_doc  WHERE doc099= " & plantaAdmin & "  and (doc011 is null or doc011 <> 1) and ( UPPER(doc001) like UPPER('%" & texto & "%') or UPPER(doc002) like UPPER('%" & texto & "%') ) Order by doc000"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                               New ELL.TrabajadoresDoc With {.clave = CInt(r("doc000")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc005"))}, query, CadenaConexion)


        End Function
        Public Function CargarListaDocTraTipo(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select trd000, tra004, tra003, tra002, emp002, trd006, trd002,trd003,trd009,trd011, trd004, trd005, trd007 from adok_trd, adok_tra, adok_emp where tra000 = trd000 And tra004=emp000 And trd099 = " & plantaAdmin & "  And trd001 = " & tipo & "  And trd006 Is Not null  order by emp002 asc, tra003 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Comentario = SabLib.BLL.Utils.stringNull(r("trd000")) & ";" & tipo.ToString, .codtra = SabLib.BLL.Utils.stringNull(r("trd000")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)

        End Function

        Public Function CargarListaDocEmpTipo(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.TrabajadoresDoc)

            Dim query As String = "Select emd000 as trd000, emp000 as tra004, ' ' as tra003, ' ' as tra002, emp002, emd004 as trd006, emd003 as trd002,emd002 as trd003,emd009 as trd009,emd011 as trd011, emd006 as trd004, emd004 as trd005, emd007 as trd007 from adok_emd,  adok_emp where emp000 = emd000 And emd099 = " & plantaAdmin & "  And emd001 = " & tipo & "  And emd005 Is Not null  order by emp002"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                             New ELL.TrabajadoresDoc With {.Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Comentario = SabLib.BLL.Utils.stringNull(r("trd000")) & ";" & tipo.ToString, .codtra = SabLib.BLL.Utils.stringNull(r("trd000")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emp002"))}, query, CadenaConexion)

        End Function

        Public Function loadListTrabajadoresTextoNoCad(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If


            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE  (tra012 is null or tra012 <> 1) and (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate)  and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like ('%" & texto.ToUpper & "%')" & " ) Order by tra003 "  'tra099= " & plantaAdmin & " and
            ' Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & "  and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If

            'este si solo activos Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and (tra012 is null or tra012 = 0) and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE  (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "   'tra099= " & plantaAdmin & "  and

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraTextoEmp(ByVal Emp As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)

            'este si solo activos Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and (tra012 is null or tra012 = 0) and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra004= " & Emp & "  and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " )    and (tra012 is null or tra012 <> 1)               Order by tra003 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListTraTextoDNI(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)

            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            'este si solo activos Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and (tra012 is null or tra012 = 0) and (   UPPER(tra003) like UPPER('%" & texto & "%')" & " or UPPER(tra002) like UPPER('%" & texto & "%')" & " ) Order by tra003 "
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003  FROM adok_tra  WHERE tra099= " & plantaAdmin & "  and (  UPPER(tra013) like UPPER('%" & texto & "%')" & " ) Order by tra013 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        Public Function loadListResponsableTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Responsables)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            'Dim nombre As String()
            'Dim apellidos As String()

            'nombre = Split(Trim(texto), ", ")
            'apellidos = Split(nombre(0), " ")
            Dim query As String
            Dim codpersona As Integer
            If IsNumeric(texto) Then
                codpersona = CInt(texto)
            Else
                codpersona = 0
            End If
            'If nombre.Count > 1 Then
            '    query = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE idplanta = " & plantaAdmin & "    and (   UPPER(apellido1) like UPPER('" & apellidos(0) & "')" & " and    UPPER(nombre) like UPPER('" & nombre(1) & "')" & " and UPPER(apellido2) like UPPER('" & apellidos(1) & "')" & " )     and fechabaja is null and apellido1 is not null order by apellido1, apellido2"
            'Else
            query = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE (idplanta = " & plantaAdmin & " or idplanta = 230)   and ( dni like ('%" & texto & "%') or codpersona = (" & codpersona & ") or          UPPER(apellido1) like UPPER('%" & texto & "%')" & " or UPPER(nombre) like UPPER('%" & texto & "%')" & " or UPPER(apellido2) like UPPER('%" & texto & "%')" & " )  and fechabaja is null  and  apellido1 is not null order by apellido1, apellido2" 'fechabaja is null and
            '  query = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE idplanta = " & plantaAdmin & "    and ( UPPER(apellido1) like UPPER('%" & texto & "%')" & " or UPPER(nombre) like UPPER('%" & texto & "%')" & " or UPPER(apellido2) like UPPER('%" & texto & "%')" & " )     and fechabaja is null and apellido1 is not null order by apellido1, apellido2"
            'End If


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Id = CInt(r("res000")), .Abrev = SabLib.BLL.Utils.stringNull(r("res001")), .Nombre = SabLib.BLL.Utils.stringNull(r("res002"))}, query, CadenaConexionSAB)


        End Function
        Public Function loadListEmpresasTextoXBATCod(ByVal plantaAdmin As Integer, ByVal texto As Integer) As List(Of ELL.Empresas)
            Dim conexExterna As String
            Dim query As String = "select codpro, nomprov, cif from gcprovee  WHERE codpro=" & texto
            conexExterna = CadenaConexionXBAT
            Select Case plantaAdmin
                Case 1
                    conexExterna = CadenaConexionXBAT
                Case 2
                    conexExterna = CadenaConexionXBAT

            End Select

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = CInt(r("codpro")), .Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, conexExterna)

        End Function
        Public Function loadListEmpresasTextoXBATNombre(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)

            '     Dim query As String = "Select emp000, emp001, emp002  FROM adok_emp  WHERE emp099= " & plantaAdmin & " and UPPER(emp002) like UPPER('%" & texto & "%') Order by emp002 "
            'Dim query As String = "select nomprov, cif from gcprovee  WHERE codpro=" & idEmpresas
            '''''''''lo cambio       Dim query As String = "select nomprov, cif from gcprovee  WHERE UPPER(nomprov) like UPPER('%" & texto & "%') Order by nomprov "
            Dim query As String = "select nombre as nomprov, cif from empresas where nombre like '%" & texto & "%' or nombre like '%" & UCase(texto) & "%' and (fechabaja > sysdate or fechabaja is null) and idplanta = " & plantaAdmin & " order by nomprov "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionSAB) 'CadenaConexionXBAT

        End Function
        Public Function loadListEmpresasTextoXBATCIF(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)

            '     Dim query As String = "Select emp000, emp001, emp002  FROM adok_emp  WHERE emp099= " & plantaAdmin & " and UPPER(emp002) like UPPER('%" & texto & "%') Order by emp002 "
            'Dim query As String = "select nomprov, cif from gcprovee  WHERE codpro=" & idEmpresas
            Dim query As String = "select nomprov, cif from gcprovee  WHERE UPPER(cif) like UPPER('%" & texto & "%') Order by nomprov "


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionXBAT)

        End Function
        Public Function loadListTrabajadoresTexto(ByVal plantaAdmin As Integer, ByVal nombre As String, ByVal apellidos As String) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "Select tra000, tra001, tra013,tra004, tra006, tra008, tra002, tra003, tra012  FROM adok_tra  WHERE   UPPER(tra002) like UPPER('%" & Trim(nombre) & "%') And UPPER(tra003) Like UPPER('" & Trim(apellidos) & "') Order by tra003 "  'tra099= " & plantaAdmin & " and

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        End Function
        'Public Function loadListCursosTexto(ByVal plantaAdmin As Integer, ByVal nombre As String) As List(Of ELL.Trabajadores)
        '    'If plantaAdmin = 230 Then
        '    '    plantaAdmin = 1
        '    'End If
        '    Dim query As String = "Select trd000, trd001   FROM adok_trd_hist  WHERE tra099= " & plantaAdmin & " and UPPER(tra002) like UPPER('%" & Trim(nombre) & "%') And UPPER(tra003) Like UPPER('" & Trim(apellidos) & "') Order by tra003 "

        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
        '                       New ELL.Trabajadores With {.Id = CInt(r("tra000")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        'End Function
        Public Function loadListTrabajadoresTextoActivos(ByVal plantaAdmin As Integer, ByVal nombre As String, ByVal apellidos As String) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra012  FROM adok_tra  WHERE (tra012 is null or tra012 <> 1) and UPPER(tra002) like UPPER('%" & Trim(nombre) & "%') And UPPER(tra003) Like UPPER('%" & Trim(apellidos) & "%') Order by tra003 " 'tra099= " & plantaAdmin & " and 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresClaveEmp(ByVal plantaAdmin As Integer, ByVal empresa As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String
            'de momento cojo todos  Dim query As String = "Select tra000, tra001, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and tra004 = " & clave
            'If empresa = 0 Then
            '    query = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra099= " & plantaAdmin & " order by tra012 asc, tra003 asc"  'and (tra012 is null or tra012 = 1) 
            'Else
            query = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE  (tra012 is null or tra012 <> 1)  and TRA004 = " & empresa & " order by  tra003 asc" 'tra099= " & plantaAdmin & " and     'and (tra012 is null or tra012 = 1) 
            'End If

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresClaveEmpSub(ByVal plantaAdmin As Integer, ByVal empresa As String) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String
            query = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE  TRA004 in (" & empresa & ") order by tra004 asc, tra012 asc, tra003 asc"   'tra099= " & plantaAdmin & " and   'and (tra012 is null or tra012 = 1) 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)

        End Function

        Public Function loadListTrabajadoresClaveEmpTODOS(ByVal plantaAdmin As Integer, ByVal empresa As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String
            'de momento cojo todos  Dim query As String = "Select tra000, tra001, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and tra004 = " & clave
            'If empresa = 0 Then
            '    query = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra099= " & plantaAdmin & " order by tra012 asc, tra003 asc"  'and (tra012 is null or tra012 = 1) 
            'Else
            query = "Select decode(tra012,0,0,1,3,0) as orden, tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE  TRA004 = " & empresa & " order by orden asc, tra003 asc" 'tra099= " & plantaAdmin & "   and  'and (tra012 is null or tra012 = 1) 
            'End If

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        End Function
        Public Function loadListSolicitudesClaveEmp(ByVal plantaAdmin As Integer, ByVal proveedor As String) As List(Of ELL.Solicitudes)
            Dim query As String
            If proveedor > 0 Then
                query = "Select 1 as orden, sol023, sol024,  sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol011, sol012  FROM adok_sol  WHERE (sol001 = " & proveedor & " or sol023 like '%" & proveedor & "%' ) and sol099= " & plantaAdmin & " and sol012 = 0  and sol004 >= sysdate "    ' sol012 asc, sol003 asc   and (tra012 is null or tra012 = 1) 
                query = query & " UNION Select 2 as orden, sol023, sol024, sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol011, sol012  FROM adok_sol  WHERE (sol001 = " & proveedor & " or sol023 like '%" & proveedor & "%' ) and sol099= " & plantaAdmin & " and sol012 = 0 and sol004 < sysdate  order by orden asc, sol003 desc "  ' sol012 asc, sol003 asc   and (tra012 is null or tra012 = 1) 

            Else
                query = "Select 1 as orden, sol023, sol024, sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol011, sol012  FROM adok_sol  WHERE sol099= " & plantaAdmin & " and sol012 = 0  and sol004 >= sysdate "    ' sol012 asc, sol003 asc   and (tra012 is null or tra012 = 1) 
                query = query & " UNION Select 2 as orden, sol023, sol024,  sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol011, sol012  FROM adok_sol  WHERE sol099= " & plantaAdmin & " and sol012 = 0 and sol004 < sysdate  order by orden asc, sol003 desc "  ' sol012 asc, sol003 asc   and (tra012 is null or tra012 = 1) 

            End If

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Subcontrata = SabLib.BLL.Utils.stringNull(r("sol023")), .plantaSeleccionada = SabLib.BLL.Utils.integerNull(r("sol024")), .Id = CInt(r("sol000")), .Empresa = CInt(r("sol001")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .EmpresaTroquelaje = SabLib.BLL.Utils.stringNull(r("sol011")), .activo = SabLib.BLL.Utils.integerNull(r("sol012"))}, query, CadenaConexion)

        End Function
        Public Function loadListSolicitudesClaveEmpETT(ByVal plantaAdmin As Integer, ByVal proveedor As String) As List(Of ELL.Solicitudes)
            Dim query As String
            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol011, sol012  FROM adok_solETT  WHERE sol099= " & plantaAdmin & " and sol012 = 0  order by sol003 desc "  ' sol012 asc, sol003 asc   and (tra012 is null or tra012 = 1) 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Id = CInt(r("sol000")), .Empresa = CInt(r("sol001")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .EmpresaTroquelaje = SabLib.BLL.Utils.stringNull(r("sol011")), .activo = SabLib.BLL.Utils.integerNull(r("sol012"))}, query, CadenaConexion)

        End Function
        Public Function CargarListaCertificados(ByVal plantaAdmin As Integer) As List(Of ELL.Certificados)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String
            query = "select 1 as orden,  rownum as auto,  doc000, emp000,  decode(doc012,3,'Certificado',4,'Instrucción Norma') as tipo,  doc002, emp002, cer003, decode(cer002, 1, 'No', 0, 'Si') as estado from adok_emp, adok_cer, adok_doc where cer099= " & plantaAdmin & "  and cer001 = emp000  and cer000 = doc000  and doc012 in (3,4) and cer002 = 0 "
            query = query & " UNION select 2 as orden,  rownum as auto,  doc000, emp000,  decode(doc012,3,'Certificado',4,'Instrucción Norma') as tipo,  doc002, emp002, cer003, decode(cer002, 1, 'No', 0, 'Si') as estado from adok_emp, adok_cer, adok_doc where cer099= " & plantaAdmin & "  and cer001 = emp000  and cer000 = doc000  and doc012 in (3,4) and cer002 = 1 order by orden asc, cer003 desc"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Certificados)(Function(r As OracleDataReader) _
                               New ELL.Certificados With {.Clave = SabLib.BLL.Utils.stringNull(r("doc000")) & ";" & SabLib.BLL.Utils.stringNull(r("emp000")), .Id = CInt(r("auto")), .Tipo = SabLib.BLL.Utils.stringNull(r("tipo")), .Empresa = SabLib.BLL.Utils.stringNull(r("emp002")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("cer003")), .descripcion = SabLib.BLL.Utils.stringNull(r("doc002")), .Estado = SabLib.BLL.Utils.stringNull(r("estado"))}, query, CadenaConexion)

        End Function
        Public Function CargarListaCertificadosEmp(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Certificados)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String
            query = "select rownum as auto,  doc000, emp000,  decode(doc012,3,'Certificado',4,'Instrucción Norma') as tipo,  doc002, emp002, cer003, decode(cer002, 1, 'No', 0, 'Si') as estado from adok_emp, adok_cer, adok_doc where cer099= " & plantaAdmin & " and emp000=" & proveedor & "  and cer001 = emp000  and cer000 = doc000  and doc012 in (3,4) order by cer003 desc"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Certificados)(Function(r As OracleDataReader) _
                               New ELL.Certificados With {.Clave = SabLib.BLL.Utils.stringNull(r("doc000")) & ";" & SabLib.BLL.Utils.stringNull(r("emp000")), .Id = CInt(r("auto")), .Tipo = SabLib.BLL.Utils.stringNull(r("tipo")), .Empresa = SabLib.BLL.Utils.stringNull(r("emp002")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("cer003")), .descripcion = SabLib.BLL.Utils.stringNull(r("doc002")), .Estado = SabLib.BLL.Utils.stringNull(r("estado"))}, query, CadenaConexion)

        End Function
        Public Function CargarListaCertificadosDoc(ByVal plantaAdmin As Integer, ByVal doc As Integer) As List(Of ELL.Certificados)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String
            query = "select 1 as orden,  rownum as auto,  doc000, emp000,  decode(doc012,3,'Certificado',4,'Instrucción Norma') as tipo,  doc002, emp002, cer003, decode(cer002, 1, 'No', 0, 'Si') as estado from adok_emp, adok_cer, adok_doc where cer099= " & plantaAdmin & " and doc000=" & doc & "  and cer001 = emp000  and cer000 = doc000  and doc012 in (3,4) and cer002 = 0 "
            query = query & " UNION select 2 as orden,  rownum as auto, doc000, emp000,  decode(doc012,3,'Certificado',4,'Instrucción Norma') as tipo,  doc002, emp002, cer003, decode(cer002, 1, 'No', 0, 'Si') as estado from adok_emp, adok_cer, adok_doc where cer099= " & plantaAdmin & " and doc000=" & doc & "  and cer001 = emp000  and cer000 = doc000  and doc012 in (3,4) and cer002 = 1 order by orden asc, cer003 desc"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Certificados)(Function(r As OracleDataReader) _
                               New ELL.Certificados With {.Clave = SabLib.BLL.Utils.stringNull(r("doc000")) & ";" & SabLib.BLL.Utils.stringNull(r("emp000")), .Id = CInt(r("auto")), .Tipo = SabLib.BLL.Utils.stringNull(r("tipo")), .Empresa = SabLib.BLL.Utils.stringNull(r("emp002")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("cer003")), .descripcion = SabLib.BLL.Utils.stringNull(r("doc002")), .Estado = SabLib.BLL.Utils.stringNull(r("estado"))}, query, CadenaConexion)

        End Function
        Public Function CargarListaCertificadosEmpDoc(ByVal plantaAdmin As Integer, ByVal proveedor As Integer, ByVal doc As Integer) As List(Of ELL.Certificados)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String
            query = "select 1 as orden, rownum as auto,  doc000, emp000,  decode(doc012,3,'Certificado',4,'Instrucción Norma') as tipo,  doc002, emp002, cer003, decode(cer002, 1, 'No', 0, 'Si') as estado from adok_emp, adok_cer, adok_doc where cer099= " & plantaAdmin & " and emp000=" & proveedor & " and doc000=" & doc & "  and cer001 = emp000  and cer000 = doc000  and doc012 in (3,4) and cer002 = 0 "
            query = query & " UNION select 2 as orden, rownum as auto, doc000, emp000, decode(doc012,3,'Certificado',4,'Instrucción Norma') as tipo,  doc002, emp002, cer003, decode(cer002, 1, 'No', 0, 'Si') as estado from adok_emp, adok_cer, adok_doc where cer099= " & plantaAdmin & " and emp000=" & proveedor & " and doc000=" & doc & "  and cer001 = emp000  and cer000 = doc000  and doc012 in (3,4) and cer002 = 1 order by orden asc, cer003 desc"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Certificados)(Function(r As OracleDataReader) _
                               New ELL.Certificados With {.Clave = SabLib.BLL.Utils.stringNull(r("doc000")) & ";" & SabLib.BLL.Utils.stringNull(r("emp000")), .Id = CInt(r("auto")), .Tipo = SabLib.BLL.Utils.stringNull(r("tipo")), .Empresa = SabLib.BLL.Utils.stringNull(r("emp002")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("cer003")), .descripcion = SabLib.BLL.Utils.stringNull(r("doc002")), .Estado = SabLib.BLL.Utils.stringNull(r("estado"))}, query, CadenaConexion)

        End Function

        Public Function loadListSolicitudesClaveEmpRest(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Solicitudes)
            Dim query As String
            'Dim lParameters1 As New List(Of OracleParameter)

            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012  FROM adok_sol  WHERE (sol001 = " & proveedor & " or sol023 like '%" & proveedor & "%')" & " and sol099= " & plantaAdmin & "  and sol012 = 0  order by sol012 asc, sol003 desc "  'and (tra012 is null or tra012 = 1) 
            'lParameters1.Add(New OracleParameter("Fecha", OracleDbType.Date, Now, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Id = CInt(r("sol000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .activo = SabLib.BLL.Utils.integerNull(r("sol012"))}, query, CadenaConexion)


        End Function

        Public Function loadListSolicitudesClaveEmpRestETT(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Solicitudes)
            Dim query As String
            'Dim lParameters1 As New List(Of OracleParameter)

            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012  FROM adok_solETT  WHERE (sol001 = " & proveedor & " or sol023 like '%" & proveedor & "%')" & " and sol099= " & plantaAdmin & "  and sol012 = 0  order by sol012 asc, sol003 desc "  'and (tra012 is null or tra012 = 1) 
            'lParameters1.Add(New OracleParameter("Fecha", OracleDbType.Date, Now, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Id = CInt(r("sol000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .activo = SabLib.BLL.Utils.integerNull(r("sol012"))}, query, CadenaConexion)


        End Function
        Public Function loadListSolicitudesClaveEmpRestAct(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Solicitudes)
            Dim query As String
            Dim lParameters1 As New List(Of OracleParameter)

            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012  FROM adok_sol  WHERE (sol001 = " & proveedor & " or sol023 like '%" & proveedor & "%')" & " and sol099= " & plantaAdmin & "  and sol012 = 0 and sol004 > :Fecha  order by sol003 desc "  'and (tra012 is null or tra012 = 1) 
            lParameters1.Add(New OracleParameter("Fecha", OracleDbType.Date, Now, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Id = CInt(r("sol000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .activo = SabLib.BLL.Utils.integerNull(r("sol012"))}, query, CadenaConexion, lParameters1.ToArray)


        End Function
        Public Function loadListSolicitudesClaveEmpRest2(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Solicitudes)
            Dim query As String

            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012  FROM adok_sol  WHERE (sol001 = " & proveedor & ")" & " and sol099= " & plantaAdmin & "  and sol012 = 0  order by sol003 desc "  'and (tra012 is null or tra012 = 1) 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Id = CInt(r("sol000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .activo = SabLib.BLL.Utils.integerNull(r("sol012"))}, query, CadenaConexion)


        End Function
        Public Function CargarListaSolicitudesClaveUser(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Solicitudes)
            Dim query As String

            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012, sol024  FROM adok_sol  WHERE (sol017 = " & proveedor & " or sol002 = " & proveedor & ") and sol099= " & plantaAdmin & "  and sol012 = 0  order by sol003 desc "  'and (tra012 is null or tra012 = 1) 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.plantaSeleccionada = SabLib.BLL.Utils.integerNull(r("sol024")), .Id = CInt(r("sol000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .activo = SabLib.BLL.Utils.integerNull(r("sol012"))}, query, CadenaConexion)


        End Function
        Public Function CargarListaSolicitudesClaveUserETT(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Solicitudes)
            Dim query As String

            query = "Select sol024, sol025, sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012  FROM adok_solETT  WHERE (sol017 = " & proveedor & " or sol002 = " & proveedor & ") and sol099= " & plantaAdmin & "  and sol012 = 0  order by sol003 desc "  'and (tra012 is null or tra012 = 1) 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Id = CInt(r("sol000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .activo = SabLib.BLL.Utils.integerNull(r("sol012")), .altura = SabLib.BLL.Utils.integerNull(r("sol024")), .activo2 = SabLib.BLL.Utils.integerNull(r("sol025"))}, query, CadenaConexion)


        End Function
        Public Function CargarListaSolicitudesClaveUserETTRRHH(ByVal plantaAdmin As Integer, ByVal proveedor As Integer) As List(Of ELL.Solicitudes)
            Dim query As String

            query = "Select sol024, sol025, sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012  FROM adok_solETT  WHERE (sol017 = " & proveedor & " or sol002 = " & proveedor & ") and sol099= " & plantaAdmin & "   order by sol003 desc "  'and (tra012 is null or tra012 = 1) 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Id = CInt(r("sol000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .activo = SabLib.BLL.Utils.integerNull(r("sol012")), .altura = SabLib.BLL.Utils.integerNull(r("sol024")), .activo2 = SabLib.BLL.Utils.integerNull(r("sol025"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresClaveEmpEstado(ByVal plantaAdmin As Integer, ByVal empresa As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If

            ' Dim query As String = "Select tra000, tra001, tra002, tra013, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and tra004 = " & empresa & " and (tra012 is null or tra012 <> 1) order by  tra003 asc"

            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE  (tra012 is null or tra012 <> 1)  and (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate)  and TRA004 = " & empresa & " order by  tra003 asc"  ' 'tra099= " & plantaAdmin & " and


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("tra005")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresClaveEmpEstadoResponsables(ByVal plantaAdmin As Integer, ByVal empresa As Integer, ByVal responsables As Integer()) As List(Of ELL.Trabajadores)
            Dim strResponsables As String = ""
            For i = 0 To responsables.Count - 1
                If responsables(i) > 0 Then
                    strResponsables = strResponsables & responsables(i).ToString & ","
                End If

            Next
            strResponsables = strResponsables & "0"

            Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra000 in (" & strResponsables & ") and  (tra012 is null or tra012 <> 1)  and (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate)  and TRA004 = " & empresa & " order by  tra003 asc"  ' 'tra099= " & plantaAdmin & " and 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("tra005")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        End Function

        Public Function loadListTrabajadoresClaveEmpEstado2(ByVal plantaAdmin As Integer, ByVal empresa As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If

            'de momento,  Dim query As String = "Select tra000, tra001, tra002, tra013, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and tra004 = " & empresa & " order by  tra003 asc"

            'de momento, pare deshacerlo         Dim query As String = "Select tra000, tra001, tra013, tra002, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra099= " & plantaAdmin & " and (tra012 is null or tra012 <> 1)  and (tra006 < to_date('02/01/1900','dd/MM/yyyy') or tra006 is null or tra006 > sysdate) and TRA004 = " & empresa & " order by  tra003 asc"  ' 

            Dim query As String = "Select tra000, tra001, tra002, tra013, tra003, tra004, tra005, tra006, tra007, tra008, tra012  FROM adok_tra  WHERE tra004 = " & empresa & " and (tra012 is null or tra012 <> 1)     order by  tra003 asc"  'tra099= " & plantaAdmin & " and 

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("tra005")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012"))}, query, CadenaConexion)


        End Function
        Public Function loadListTrabajadoresClaveEmpSolicitud(ByVal plantaAdmin As Integer, ByVal solicitud As Integer, ByVal empresa As Integer) As List(Of ELL.Trabajadores)

            Dim query As String = "Select trs000, tra002, trs002, tra003, tra006, trs008  FROM adok_trs, adok_tra  WHERE trs000=tra000 and trs099= " & plantaAdmin & " and TRs001 = " & solicitud & " and TRs002 = " & empresa & " and tra012<>1 order by trs000 asc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.FechaFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006")), .Id = CInt(r("trs000")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        End Function
        'Public Function loadListTrabajadoresClaveEmpSolicitud2(ByVal plantaAdmin As Integer, ByVal solicitud As Integer) As List(Of ELL.Trabajadores)

        '    Dim query As String = "Select trs000, tra002, trs002, tra003, trs008  FROM adok_trs, adok_tra  WHERE trs000=tra000 and trs099= " & plantaAdmin & " and TRs001 = " & solicitud & " order by trs000 asc "

        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
        '                       New ELL.Trabajadores With {.Id = CInt(r("trs000")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002"))}, query, CadenaConexion)


        'End Function
        Public Function loadListTrabajadoresClaveTra(ByVal plantaAdmin As Integer, ByVal clave As Integer) As List(Of ELL.Trabajadores)

            Dim query As String = "Select tra000, tra001, tra013, tra013,tra014, tra015, tra002, tra003, tra004, tra005, tra006, tra007, tra008,  tra010,  tra011, tra012  FROM adok_tra  WHERE  tra000 = " & clave    ' tra099= " & plantaAdmin & " and


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .DescResponsable = SabLib.BLL.Utils.stringNull(r("tra011")), .funcion = SabLib.BLL.Utils.stringNull(r("tra007")), .solicitud = SabLib.BLL.Utils.stringNull(r("tra014")), .Autonomo = SabLib.BLL.Utils.integerNull(r("tra015")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("tra005")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, query, CadenaConexion)


        End Function
        Public Function loadListSolicitudesClaveTra(ByVal plantaAdmin As Integer, ByVal clave As Integer) As List(Of ELL.Solicitudes)


            Dim query As String = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012, sol013, sol014, sol015, sol016, sol018, sol019, sol020, sol021, sol022, sol023, sol024  FROM adok_sol  WHERE  sol099= " & plantaAdmin & " and sol000 = " & clave & "   order by sol012 asc, sol000 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.plantaSeleccionada = SabLib.BLL.Utils.integerNull(r("sol024")), .Id = CInt(r("sol000")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .Subcontrata = SabLib.BLL.Utils.stringNull(r("sol023")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .soldadura = SabLib.BLL.Utils.integerNull(r("sol005")), .altura = SabLib.BLL.Utils.integerNull(r("sol006")), .salas = SabLib.BLL.Utils.integerNull(r("sol007")), .gases = SabLib.BLL.Utils.integerNull(r("sol014")), .elevados = SabLib.BLL.Utils.integerNull(r("sol015")), .fosas = SabLib.BLL.Utils.integerNull(r("sol016")), .X7 = SabLib.BLL.Utils.integerNull(r("sol018")), .X8 = SabLib.BLL.Utils.integerNull(r("sol019")), .X9 = SabLib.BLL.Utils.integerNull(r("sol020")), .X10 = SabLib.BLL.Utils.integerNull(r("sol021")), .X11 = SabLib.BLL.Utils.integerNull(r("sol022")), .email = SabLib.BLL.Utils.stringNull(r("sol008")), .otros = SabLib.BLL.Utils.stringNull(r("sol009")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .activo = SabLib.BLL.Utils.integerNull(r("sol012")), .DescSolicitante = SabLib.BLL.Utils.stringNull(r("sol013"))}, query, CadenaConexion)
            '.EmpresaTroquelaje = SabLib.BLL.Utils.stringNull(r("sol011")),

            '  Dim query As String = "Select tra000, tra001, tra013, tra015, tra002, tra003, tra004, tra005, tra006, tra007, tra008,  tra010,  tra011, tra012, emp021  FROM adok_tra, adok_emp  WHERE adok_tra.tra004 = adok_emp.emp000  and tra099= " & plantaAdmin & " and tra000 = " & clave


            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
            '                   New ELL.Solicitudes With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .puesto = SabLib.BLL.Utils.stringNull(r("tra011")), .funcion = SabLib.BLL.Utils.stringNull(r("tra007")), .Autonomo = SabLib.BLL.Utils.integerNull(r("tra015")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("tra005")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, query, CadenaConexion)


        End Function

        Public Function loadListSolicitudesClaveTraETT(ByVal plantaAdmin As Integer, ByVal clave As Integer) As List(Of ELL.Solicitudes)


            Dim query As String = "Select sol024, sol025, sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol009, sol010, sol012, sol013, sol014, sol015, sol016, sol018, sol019, sol020, sol021, sol022, sol023  FROM adok_solETT  WHERE  sol099= " & plantaAdmin & " and sol000 = " & clave & "   order by sol012 asc, sol000 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                               New ELL.Solicitudes With {.Area = SabLib.BLL.Utils.integerNull(r("sol024")), .Numero = SabLib.BLL.Utils.integerNull(r("sol025")), .Id = CInt(r("sol000")), .Empresa = SabLib.BLL.Utils.integerNull(r("sol001")), .Subcontrata = SabLib.BLL.Utils.stringNull(r("sol023")), .responsable = SabLib.BLL.Utils.integerNull(r("sol002")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("sol003")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("sol004")), .soldadura = SabLib.BLL.Utils.integerNull(r("sol005")), .altura = SabLib.BLL.Utils.integerNull(r("sol006")), .salas = SabLib.BLL.Utils.integerNull(r("sol007")), .gases = SabLib.BLL.Utils.integerNull(r("sol014")), .elevados = SabLib.BLL.Utils.integerNull(r("sol015")), .fosas = SabLib.BLL.Utils.integerNull(r("sol016")), .X7 = SabLib.BLL.Utils.integerNull(r("sol018")), .X8 = SabLib.BLL.Utils.integerNull(r("sol019")), .X9 = SabLib.BLL.Utils.integerNull(r("sol020")), .X10 = SabLib.BLL.Utils.integerNull(r("sol021")), .X11 = SabLib.BLL.Utils.integerNull(r("sol022")), .email = SabLib.BLL.Utils.stringNull(r("sol008")), .otros = SabLib.BLL.Utils.stringNull(r("sol009")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010")), .activo = SabLib.BLL.Utils.integerNull(r("sol012")), .DescSolicitante = SabLib.BLL.Utils.stringNull(r("sol013"))}, query, CadenaConexion)

        End Function
        Public Function loadListTrabajadoresClaveTraActivos(ByVal plantaAdmin As Integer, ByVal clave As Integer) As List(Of ELL.Trabajadores)
            'If plantaAdmin = 230 Then
            '    plantaAdmin = 1
            'End If
            Dim query As String = "Select tra000, tra001, tra013,tra014, tra015, tra002, tra003, tra004, tra005, tra006, tra007, tra008,  tra010,  tra011, tra012  FROM adok_tra, adok_emp  WHERE adok_tra.tra004 = adok_emp.emp000 and (tra012 is null or tra012 <> 1)  and  tra000 = " & clave & " order by tra003 asc"  'tra099= " & plantaAdmin & " and


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Trabajadores)(Function(r As OracleDataReader) _
                               New ELL.Trabajadores With {.Id = CInt(r("tra000")), .nDNI = SabLib.BLL.Utils.stringNull(r("tra001")), .tDNI = SabLib.BLL.Utils.stringNull(r("tra013")), .Nombre = SabLib.BLL.Utils.stringNull(r("tra003")) & ", " & SabLib.BLL.Utils.stringNull(r("tra002")), .Empresa = SabLib.BLL.Utils.integerNull(r("tra004")), .activo = SabLib.BLL.Utils.integerNull(r("tra012")), .responsable = SabLib.BLL.Utils.integerNull(r("tra008")), .puesto = SabLib.BLL.Utils.stringNull(r("tra011")), .funcion = SabLib.BLL.Utils.stringNull(r("tra007")), .solicitud = SabLib.BLL.Utils.stringNull(r("tra014")), .Autonomo = SabLib.BLL.Utils.integerNull(r("tra015")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("tra005")), .FecFin = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, query, CadenaConexion)


        End Function
        Public Function loadListRes(ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            '      Dim query As String = "Select res000, res001, res002  FROM adok_res  WHERE res099 = " & plantaAdmin & " order by res000"
            Dim query As String = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE idplanta = " & plantaAdmin & " and fechabaja is null and apellido1 is not null order by apellido1, apellido2"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Id = CInt(r("res000")), .Nombre = SabLib.BLL.Utils.stringNull(r("res001")), .Abrev = SabLib.BLL.Utils.stringNull(r("res002"))}, query, CadenaConexionSAB)


        End Function
        Public Function loadListSol(ByVal plantaAdmin As Integer) As List(Of ELL.Solicitudes)

            Dim query As String = "SELECT sol000, sol010  FROM adok_sol  WHERE sol099= " & plantaAdmin & " order by sol000"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                                 New ELL.Solicitudes With {.Id = CInt(r("sol000")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010"))}, query, CadenaConexion)

        End Function
        Public Function loadListSolETT(ByVal plantaAdmin As Integer) As List(Of ELL.Solicitudes)

            Dim query As String = "SELECT sol000, sol010  FROM adok_solETT  WHERE sol099= " & plantaAdmin & " order by sol000"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Solicitudes)(Function(r As OracleDataReader) _
                                 New ELL.Solicitudes With {.Id = CInt(r("sol000")), .descripcion = SabLib.BLL.Utils.stringNull(r("sol010"))}, query, CadenaConexion)

        End Function


        Public Function loadResponsables(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            '   Dim query As String = "Select res000, res001, res002  FROM adok_res  WHERE res000 = " & cod & " and res099 = " & plantaAdmin & " order by res000"
            Dim query As String = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE  id = " & cod & " and idplanta = " & plantaAdmin & " and fechabaja is null and apellido1 is not null order by apellido1, apellido2"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Id = CInt(r("res000")), .Abrev = SabLib.BLL.Utils.stringNull(r("res001")), .Nombre = SabLib.BLL.Utils.stringNull(r("res002"))}, query, CadenaConexionSAB)


        End Function

        Public Function loadResponsablesMail(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)

            'Dim query As String = "  Select  email as mail FROM usuarios  WHERE  idempresas = " & cod & " and (fechabaja is null or fechabaja > sysdate) and usuario_empresa = 1 order by email " 'esto hace que coja el de extranet
            Dim query As String = "  Select  distinct(email) as mail FROM usuarios  WHERE  idempresas = " & cod & " and (fechabaja is null or fechabaja > sysdate) and iddirectorioactivo is null and codpersona is null order by email " 'esto hace que coja el de extranet

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Mail = SabLib.BLL.Utils.stringNull(r("mail"))}, query, CadenaConexionSAB)


        End Function

        Public Function loadResponsablesNombreMail(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)

            'Dim query As String = "  Select  email as mail FROM usuarios  WHERE  idempresas = " & cod & " and (fechabaja is null or fechabaja > sysdate) and usuario_empresa = 1 order by email " 'esto hace que coja el de extranet
            Dim query As String = "  Select apellido1, nombre, email as mail, CODPERSONA  FROM usuarios  WHERE  id = " & cod & "  order by email " 'esto hace que coja el de extranet

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Mail = SabLib.BLL.Utils.stringNull(r("mail")), .Id = SabLib.BLL.Utils.integerNull(r("CODPERSONA")), .Nombre = SabLib.BLL.Utils.stringNull(r("apellido1")) & ", " & SabLib.BLL.Utils.stringNull(r("nombre"))}, query, CadenaConexionSAB)


        End Function

        Public Function loadMailSAB(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)

            Dim query As String = "  Select email as mail, id FROM usuarios  WHERE  idempresas = " & cod & " and USUARIO_EMPRESA = 1  order by email "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Mail = SabLib.BLL.Utils.stringNull(r("mail")), .Id = SabLib.BLL.Utils.integerNull(r("id"))}, query, CadenaConexionSAB)


        End Function

        Public Function loadGlobalesNombreMail(ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "  Select dos002 FROM adok_dos  WHERE  dos099= " & plantaAdmin & " and dos000 = 1  order by dos002 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Mail = SabLib.BLL.Utils.stringNull(r("dos002"))}, query, CadenaConexion)


        End Function

        Public Function loadListtodosResp(ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            '   Dim query As String = "Select res000, res001, res002 FROM adok_res  WHERE res099= " & plantaAdmin & "  order by res000 asc "
            Dim query As String = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  ) as   res002  FROM usuarios  WHERE idplanta = " & plantaAdmin & " and fechabaja is null and apellido1 is not null order by apellido1, apellido2"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Id = CInt(r("res000")), .Nombre = SabLib.BLL.Utils.stringNull(r("res001")), .Abrev = SabLib.BLL.Utils.stringNull(r("res002"))}, query, CadenaConexionSAB)
        End Function
        Public Function loadListPla(ByVal plantaAdmin As Integer, ByVal idDocumento As Integer) As List(Of ELL.Plantillas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select pla000, pla002, pla005  FROM adok_pla  WHERE   pla099 = " & plantaAdmin & " and pla000 = " & idDocumento & " order by pla002 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Plantillas)(Function(r As OracleDataReader) _
                             New ELL.Plantillas With {.documento = CInt(r("pla000")), .nombre = SabLib.BLL.Utils.stringNull(r("pla005")), .Fecha = SabLib.BLL.Utils.DateNull(r("pla002"))}, query, CadenaConexion)


        End Function
        Public Function loadListPlaDoc(ByVal plantaAdmin As Integer, ByVal DocNombre As String) As List(Of ELL.Plantillas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select pla000, pla002, pla005  FROM adok_pla  WHERE (pla006 is  null or pla006 < 1) and pla099 = " & plantaAdmin & " and pla005 = '" & DocNombre & "' order by pla002 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Plantillas)(Function(r As OracleDataReader) _
                             New ELL.Plantillas With {.documento = CInt(r("pla000")), .nombre = SabLib.BLL.Utils.stringNull(r("pla005")), .Fecha = SabLib.BLL.Utils.DateNull(r("pla002"))}, query, CadenaConexion)


        End Function
        Public Function loadListPla2(ByVal plantaAdmin As Integer, ByVal idDocumento As Integer) As List(Of ELL.Plantillas)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select pla000, pla002, pla005  FROM adok_pla  WHERE pla006 = 1 and pla099 = " & plantaAdmin & " and pla000 = " & idDocumento & " order by pla002 desc, pla004 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Plantillas)(Function(r As OracleDataReader) _
                             New ELL.Plantillas With {.documento = CInt(r("pla000")), .nombre = SabLib.BLL.Utils.stringNull(r("pla005")), .Fecha = SabLib.BLL.Utils.DateNull(r("pla002"))}, query, CadenaConexion)


        End Function

        Public Function loadListPre(ByVal plantaAdmin As Integer) As List(Of ELL.Preventiva)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select mp000, mp001  FROM adok_mp  WHERE mp099 = " & plantaAdmin & " order by mp000"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Preventiva)(Function(r As OracleDataReader) _
                             New ELL.Preventiva With {.Id = CInt(r("mp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("mp001"))}, query, CadenaConexion)


        End Function

        Public Function loadPre(ByVal plantaAdmin As Integer, ByVal codpre As Integer) As List(Of ELL.Preventiva)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select mp000, mp001  FROM adok_mp  WHERE mp099 = " & plantaAdmin & " and mp000 = " & codpre

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Preventiva)(Function(r As OracleDataReader) _
                             New ELL.Preventiva With {.Id = CInt(r("mp000")), .Nombre = SabLib.BLL.Utils.stringNull(r("mp001"))}, query, CadenaConexion)


        End Function

        Public Function loadListCad(ByVal plantaAdmin As Integer) As List(Of ELL.Caducidades)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select per000, per001, per002, per003, decode(per002, 1, 'Día', 6, 'Mes', 3, 'Trimestre', 4, 'Año', '')  as nombreint  FROM adok_per  WHERE per099 = " & plantaAdmin & " order by per000 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Caducidades)(Function(r As OracleDataReader) _
                             New ELL.Caducidades With {.id = CInt(r("per000")), .nombre = SabLib.BLL.Utils.stringNull(r("per001")), .intervalo = SabLib.BLL.Utils.integerNull(r("per002")), .cantidad = SabLib.BLL.Utils.integerNull(r("per003")), .nombreInt = SabLib.BLL.Utils.stringNull(r("nombreInt"))}, query, CadenaConexion)


        End Function
        Public Function loadListDocTipo4(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            'and (doc011 is null or doc011 = 0)
            Dim query As String = "Select doc014, doc015, doc016, doc000, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc012  FROM adok_doc  WHERE doc099= " & plantaAdmin & " and doc012=4   order by doc014 asc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                             New ELL.Documentos With {.Id = CInt(r("doc014")), .comentario = SabLib.BLL.Utils.stringNull(r("doc016")), .Nombre = SabLib.BLL.Utils.stringNull(r("doc001")), .Abrev = SabLib.BLL.Utils.stringNull(r("doc002")), .Plantilla = SabLib.BLL.Utils.integerNull(r("doc009")), .EsDocumento = SabLib.BLL.Utils.integerNull(r("doc014")), .activo = SabLib.BLL.Utils.integerNull(r("doc015"))}, query, CadenaConexion)

            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Caducidades)(Function(r As OracleDataReader) _
            '                 New ELL.Caducidades With {.id = CInt(r("per000")), .nombre = SabLib.BLL.Utils.stringNull(r("per001")), .intervalo = SabLib.BLL.Utils.integerNull(r("per002")), .cantidad = SabLib.BLL.Utils.integerNull(r("per003")), .nombreInt = SabLib.BLL.Utils.stringNull(r("nombreInt"))}, query, CadenaConexion)


        End Function
        Public Function loadListRol(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol099, rol001, rol000,  decode(rol099, 1, 'PREVISIÓN', 9, 'MEDIO AMBIENTE', 3, '')  as NombreDepto, decode(rol001, 2, 'Administrador', 9, 'Consulta', 3, 'Control de Acceso', 4, 'Financiero', 5, 'RRHH', 22,'Servicio Médico',21, 'Prevención', '')  as nombrerol, rol002  FROM adok_roles  order by rol000 desc"   ' WHERE rol099 = " & plantaAdmin & "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Planta = CInt(r("rol099")), .Id = CInt(r("rol000")), .rol = SabLib.BLL.Utils.stringNull(r("rol001")), .NombreDepto = SabLib.BLL.Utils.stringNull(r("NombreDepto")), .NombreRol = SabLib.BLL.Utils.stringNull(r("nombreRol")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, CadenaConexion)


        End Function

        Public Function loadListRolETT(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol000, rol001, decode(rol001, 2, 'Administrador', 11, 'Usuario', 5, 'RR.HH.',  'Consultor')  as nombrerol, rol002  FROM adok_rolesETT  WHERE rol099 = " & plantaAdmin & " order by rol000 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("rol000")), .rol = SabLib.BLL.Utils.stringNull(r("rol001")), .NombreRol = SabLib.BLL.Utils.stringNull(r("nombreRol")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, CadenaConexion)


        End Function


        Public Function loadListDos(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select dos001, dos002  FROM adok_dos  WHERE dos099 = " & plantaAdmin & " and dos000 = 1 order by DOS002 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("dos001")), .NombreRol = SabLib.BLL.Utils.stringNull(r("dos002"))}, query, CadenaConexion)


        End Function
        Public Function loadListDosETT(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select dos001, dos002  FROM adok_dosETT  WHERE dos099 = " & plantaAdmin & " and dos000 = 1 order by DOS002 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("dos001")), .NombreRol = SabLib.BLL.Utils.stringNull(r("dos002"))}, query, CadenaConexion)


        End Function

        Public Function loadCad(ByVal plantaAdmin As Integer, ByVal codigo As Integer) As List(Of ELL.Caducidades)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select per000, per001, per002, per003  FROM adok_per WHERE per000 = " & codigo & "and per099 = " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Caducidades)(Function(r As OracleDataReader) _
                             New ELL.Caducidades With {.id = CInt(r("per000")), .nombre = SabLib.BLL.Utils.stringNull(r("per001")), .intervalo = SabLib.BLL.Utils.integerNull(r("per002")), .cantidad = SabLib.BLL.Utils.integerNull(r("per003"))}, query, CadenaConexion)


        End Function
        Public Function loadInt(ByVal plantaAdmin As Integer, ByVal codigo As Integer) As List(Of ELL.Caducidades)
            If plantaAdmin = 230 Then
                plantaAdmin = 1
            End If
            Dim query As String = "Select int001  FROM adok_int WHERE int000 = " & codigo

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Caducidades)(Function(r As OracleDataReader) _
                             New ELL.Caducidades With {.nombre = SabLib.BLL.Utils.stringNull(r("int001"))}, query, CadenaConexion)


        End Function

        Public Function loadListPlanta() As List(Of ELL.Documentos)

            Dim query As String = "SELECT ID, ID_BRAIN, NOMBRE FROM PLANTAS WHERE ID_BRAIN IS NOT NULL ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                     New ELL.Documentos With {.Id = CInt(r("ID")), .Abrev = SabLib.BLL.Utils.stringNull(r("ID_BRAIN")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, CadenaConexionSAB)

        End Function
#End Region


#Region "Modificaciones"
        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="docu">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Save(ByVal docu As ELL.Documentos) As Boolean
            If docu.Planta = 230 Then
                docu.Planta = 1
            End If
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_doc (doc099, doc001, doc002, doc003, doc004, doc005, doc006, doc007, doc008, doc009, doc011, doc012, doc013, doc014, doc016, doc010, doc017) VALUES(:planta, :nombre,
:Abrev,  :Periodo, :Obligatorio, :Trabajador, :comentario, :Responsable, :Margen , :Plantilla, 0, :EsDocumento, :listacorreos, :tipotrabajo, :textoSolicitud, :ETT, :area)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 150, docu.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Abrev", OracleDbType.Varchar2, 10, docu.Abrev, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Periodo", OracleDbType.Int32, 18, docu.Periodo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Obligatorio", OracleDbType.Int32, 1, docu.Obligatorio, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Trabajador", OracleDbType.Int32, 1, docu.Trabajador, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("comentario", OracleDbType.Varchar2, 250, docu.comentario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Responsable", OracleDbType.Int32, 18, docu.Responsable, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Margen", OracleDbType.Int32, 18, docu.Margen, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Plantilla", OracleDbType.Int32, 1, docu.Plantilla, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("EsDocumento", OracleDbType.Int32, 1, docu.EsDocumento, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("listacorreos", OracleDbType.Varchar2, 150, docu.listacorreos, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("tipotrabajo", OracleDbType.Int32, 1, docu.tipotrabajo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ETT", OracleDbType.Int32, 1, docu.ETT, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("area", OracleDbType.Int32, docu.area, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("textoSolicitud", OracleDbType.Varchar2, docu.textoSolicitud, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function

        Public Function DeleteMatriz() As Boolean
            Dim query As String = String.Empty

            query = "DELETE FROM MATRIZ "
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion)

            Return True
        End Function
        Public Function SaveMatriz(ByVal curso As String, ByVal valores As String) As Boolean
            Dim query As String = String.Empty
            query = "UPDATE MATRIZ SET VALORES = '" & valores & "' where curso='" & curso & "'"

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion)
            Return True
        End Function

        Public Function SaveMatrizInsert(ByVal curso As String, ByVal valores As String) As Boolean
            Dim query As String = String.Empty

            query = "INSERT INTO MATRIZ (curso, valores) VALUES('" & curso & "','" & valores & "')"

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion)
            Return True
        End Function
        Public Function SaveResponsables(ByVal Responsables As ELL.Responsables) As Boolean
            If Responsables.Planta = 230 Then
                Responsables.Planta = 1
            End If
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_res (res099, res001, res002) VALUES(:planta, :nombre,:Abrev)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Responsables.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 150, Responsables.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Abrev", OracleDbType.Varchar2, 10, Responsables.Abrev, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un Responsable", ex)
            End Try
        End Function
        Public Function SaveCaducidades(ByVal Responsables As ELL.Caducidades) As Boolean
            If Responsables.Planta = 230 Then
                Responsables.Planta = 1
            End If
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_per (per099, per001, per002, per003) VALUES(:planta, :nombre, :intervalo, :cantidad)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Responsables.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 150, Responsables.nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("cantidad", OracleDbType.Int32, Responsables.cantidad, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("intervalo", OracleDbType.Int32, 1, Responsables.intervalo, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar una Periodicidad", ex)
            End Try
        End Function
        Public Function SaveRol(ByVal rol As ELL.Rol) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_roles (rol099, rol000, rol001, rol002) VALUES(:planta, :codigo, :rol,:nombreuser)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, rol.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, rol.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("rol", OracleDbType.Int32, rol.rol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombreuser", OracleDbType.Varchar2, rol.NombreRol, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un Rol", ex)
            End Try
        End Function

        Public Function SaveRolETT(ByVal rol As ELL.Rol) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_rolesETT (rol099, rol000, rol001, rol002) VALUES(:planta, :codigo, :rol,:nombreuser)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, rol.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, rol.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("rol", OracleDbType.Int32, rol.rol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombreuser", OracleDbType.Varchar2, rol.NombreRol, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un Rol", ex)
            End Try
        End Function

        Public Function SaveEmail(ByVal rol As ELL.Rol) As Boolean
            If rol.Planta = 230 Then
                rol.Planta = 1
            End If
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_dos (dos099, dos000, dos001, dos002) VALUES(:planta,1,  :codigo, :nombreuser)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, rol.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, rol.Id, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("nombreuser", OracleDbType.Varchar2, rol.NombreRol, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un Rol", ex)
            End Try
        End Function
        Public Function SaveEmailETT(ByVal rol As ELL.Rol) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_dosETT (dos099, dos000, dos001, dos002) VALUES(:planta,1,  :codigo, :nombreuser)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, rol.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, rol.Id, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("nombreuser", OracleDbType.Varchar2, rol.NombreRol, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un Rol", ex)
            End Try
        End Function
        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="docu">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 

        Public Function SaveEmp(ByVal docu As ELL.Empresas) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_emp (emp000,  emp001, emp002) VALUES(:planta,  :nombre, :traduccion)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, Trim(docu.Nombre), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("traduccion", OracleDbType.Varchar2, 100, Trim(docu.notificar), ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True
            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una empresa con ese CIF ".ToUpper & docu.Nif, ex)
                End If
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function

        Public Function SaveEmpSAB(ByVal docu As ELL.Empresas) As List(Of String())

            '       Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO empresas (idplanta,  cif, nombre ,fechaalta) VALUES(:planta,  :cif,  :nombre, sysdate)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("CIF", OracleDbType.Varchar2, 12, Trim(docu.Nif).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, Trim(docu.Nombre).ToUpper, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionSAB, lParameters1.ToArray)
                'se mira el cod empresa que se ha asignado

                Dim query2 As String = " Select EmpresasSequence.nextval   From DUAL "
                'Dim query2 As String = " Select id From empresas where id = 1141 "  'esto lo quitare
                Return Memcached.OracleDirectAccess.Seleccionar(query2, CadenaConexionSAB)

            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una empresa con ese CIF ".ToUpper & docu.Nif, ex)
                End If

                Throw New SabLib.BatzException("Error al grabar una empresa", ex)
            End Try
        End Function


        Public Function SaveEmpADOK(ByVal docu As ELL.Empresas) As List(Of String())
            If docu.Planta = 230 Then
                docu.Planta = 1
            End If
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_emp (emp099,  emp001, emp002, emp018, emp022) VALUES(:planta,  :cif,  :nombre, 0, :codigo)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                'es autogenerado el de adok el de SAB va a emp011:
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, docu.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("CIF", OracleDbType.Varchar2, 12, Trim(docu.Nif).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, Trim(docu.Nombre).ToUpper, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                'se mira los documentos obligatorios y se añaden en adok_emd 


                'se mira el cod empresa que se ha asignado 

                Dim query2 As String = " Select SEQ_EMP.nextval   From DUAL "

                Return Memcached.OracleDirectAccess.Seleccionar(query2, CadenaConexion)

            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una empresa con ese CIF ".ToUpper & docu.Nif, ex)
                End If

                Throw New SabLib.BatzException("Error al grabar una empresa", ex)
            End Try
        End Function


        Public Function SaveUserSAB(ByVal docu As ELL.Empresas) As List(Of String())

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO USUARIOS (fechaalta, fechabaja, dni, idplanta, idempresas,  PWD, idculturas, email, NOMBREUSUARIO, usuario_empresa) VALUES(:FecIni, :FecFin, :DNI, :planta, :id, '70504C06',  'es-ES', :email, :email, 0)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, docu.Id, ParameterDirection.Input))
                '  lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, Trim(docu.Nombre).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 100, Trim(docu.email), ParameterDirection.Input))


                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, Now, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecRec, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DNI", OracleDbType.Varchar2, docu.Nif, ParameterDirection.Input))



                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionSAB, lParameters1.ToArray)

                'se mira el cod trabajador que se ha asignado

                Dim query2 As String = " Select USUARIOSSEQUENCE.nextval   From DUAL "

                Return Memcached.OracleDirectAccess.Seleccionar(query2, CadenaConexionSAB)

            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una empresa con ese CIF ".ToUpper & docu.Nif, ex)
                End If

                Throw New SabLib.BatzException("Error al grabar una empresa", ex)
            End Try
        End Function


        Public Function SaveUserSABExiste(ByVal docu As ELL.Empresas) As List(Of String())
            'se diferencia del anterior en que el usuario_empresa = 0 (solo debe haber un 1) (ahora codpersona)
            Dim resultado As Boolean = False
            Dim arrNombre As String() = Nothing

            Dim query As String = String.Empty
            Dim NOMBREUSUARIO As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                arrNombre = Split(Trim(docu.email), ",")
                NOMBREUSUARIO = arrNombre(1) & arrNombre(0)


                '      query = "INSERT INTO USUARIOS (fechaalta, fechabaja, dni, idplanta, idempresas,  PWD, idculturas, email, NOMBREUSUARIO, usuario_empresa) VALUES(:FecIni, :FecFin, :DNI, :planta, :id, '70504C06',  'es-ES', :email, :email, 0)"
                query = "INSERT INTO USUARIOS (codpersona, fechaalta, fechabaja, dni, idplanta, idempresas,  PWD, idculturas,  NOMBREUSUARIO, usuario_empresa, NOMBRE, APELLIDO1,IDDEPARTAMENTO) VALUES(:codpersona, :FecIni, :FecFin, :DNI, :planta, :id, '70504C06',  'es-ES', :NOMBREUSUARIO,  0,:NOMBRE,:APELLIDO1, :IDDEPARTAMENTO)"

                lParameters1.Add(New OracleParameter("codpersona", OracleDbType.Int32, docu.empSAB, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, docu.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("IDDEPARTAMENTO", OracleDbType.Int32, docu.activo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, arrNombre(1), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("APELLIDO1", OracleDbType.Varchar2, 100, arrNombre(0), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBREUSUARIO", OracleDbType.Varchar2, 100, NOMBREUSUARIO, ParameterDirection.Input))
                'lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 100, Trim(docu.email), ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, Now, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, CDate(docu.FecRec.ToShortDateString), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DNI", OracleDbType.Varchar2, docu.Nif, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionSAB, lParameters1.ToArray)

                'se mira el cod trabajador que se ha asignado

                Dim query2 As String = " Select USUARIOSSEQUENCE.nextval   From DUAL "

                Return Memcached.OracleDirectAccess.Seleccionar(query2, CadenaConexionSAB)

            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe un trabajador con ese CIF ".ToUpper & docu.Nif, ex)
                End If

                Throw New SabLib.BatzException("Error al grabar una empresa", ex)
            End Try
        End Function

        Public Function SaveUserSABExisteModificar(ByVal docu As ELL.Empresas) As List(Of String())
            'se diferencia del anterior en que el usuario_empresa = 0 (solo debe haber un 1) (ahora codpersona)
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                '      query = "INSERT INTO USUARIOS (fechaalta, fechabaja, dni, idplanta, idempresas,  PWD, idculturas, email, NOMBREUSUARIO, usuario_empresa) VALUES(:FecIni, :FecFin, :DNI, :planta, :id, '70504C06',  'es-ES', :email, :email, 0)"
                query = "UPDATE USUARIOS SET codpersona=:codpersona, fechaalta=:FecIni, fechabaja=:FecFin,   idempresas=:id,  PWD='70504C06', idculturas='es-ES', email=:email, NOMBREUSUARIO=:email  WHERE idplanta=:planta and dni=:dni "

                lParameters1.Add(New OracleParameter("codpersona", OracleDbType.Int32, docu.empSAB, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, docu.Id, ParameterDirection.Input))
                '  lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, Trim(docu.Nombre).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 100, Trim(docu.email), ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, Now, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, CDate(docu.FecRec.ToShortDateString), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DNI", OracleDbType.Varchar2, docu.Nif, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionSAB, lParameters1.ToArray)

                'se mira el cod trabajador que se ha asignado

                Dim query2 As String = " Select USUARIOSSEQUENCE.nextval   From DUAL "

                Return Memcached.OracleDirectAccess.Seleccionar(query2, CadenaConexionSAB)

            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe un trabajador con ese CIF ".ToUpper & docu.Nif, ex)
                End If

                Throw New SabLib.BatzException("Error al grabar una empresa", ex)
            End Try
        End Function


        Public Function SavePlantaSAB(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean

            Dim query As String = "INSERT INTO USUARIOS_PLANTAS(ID_USUARIO,ID_PLANTA) VALUES(:ID_USUARIO,:ID_PLANTA)"
            Dim parameters(1) As OracleParameter
            parameters(0) = New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUsuario, ParameterDirection.Input)
            parameters(1) = New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)


            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionSAB, parameters)
            Return True
        End Function

        Public Function SaveTra(ByVal docu As ELL.Trabajadores) As Boolean
            'If docu.Planta = 230 Then
            '    docu.Planta = 1
            'End If
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_tra (tra000, tra099,  tra001, tra013, tra002, tra003, tra004,  tra012, tra008, tra014, tra005, tra006, tra015, tra011, tra007) VALUES(:id, :planta,  :nDNI, :tDNI, :nombre, :apellidos, :Empresa, 0, :responsable, :Solicitud, :FecIni, :FecFin, :Autonomo, :puesto, :funcion)"

                'If docu.nDNI = "" Then
                '    docu.nDNI = "No DNI"
                'End If
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 6, docu.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, 1, ParameterDirection.Input))
                'es autogenerado       lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, docu.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nDNI", OracleDbType.Varchar2, 12, Trim(docu.tDNI).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(docu.tDNI).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 50, Trim(docu.Nombre).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("apellidos", OracleDbType.Varchar2, 150, Trim(docu.Apellidos).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Int32, 6, docu.Empresa, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Int32, docu.responsable, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Solicitud", OracleDbType.Varchar2, 150, "0", ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Autonomo", OracleDbType.Int32, 1, 0, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("puesto", OracleDbType.Varchar2, 150, docu.DescResponsable, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("funcion", OracleDbType.Varchar2, 150, "0", ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                'se mira los documentos obligatorios y se añaden en adok_emd 



                Return True
            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una trabajador con ese DNI ".ToUpper & docu.nDNI, ex)
                End If
                Throw New SabLib.BatzException("Error al grabar un trabajador", ex)
            End Try
        End Function



        Public Function SaveCer(ByVal planta As Integer, ByVal coddoc As Integer, ByVal codemp As Integer, ByVal aceptado As Integer, ByVal nombre As String) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_cer (cer099,  cer000, cer001, cer002, cer003, cer004) VALUES(:planta,  :coddoc, :codemp, :aceptado, sysdate, :nombre)"
                'EmpresaTroquelaje seria sol011
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, coddoc, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, codemp, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("aceptado", OracleDbType.Int32, 1, aceptado, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombre", OracleDbType.Varchar2, 50, Trim(nombre), ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                'se mira los documentos obligatorios y se añaden en adok_emd 



                Return True
            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una solicitud con ese codigo ".ToUpper, ex)
                End If
                Throw New SabLib.BatzException("Error al grabar una solicitud", ex)
            End Try
        End Function



        Public Function SaveDorlet(ByVal docu As ELL.Dorlet) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_dorlet (DNI_DORLET,  NOMBRE_DORLET, APELLIDOS_DORLET, EMPRESA_DORLET, MATRICULA_DORLET, TARJETA_DORLET, FECHAA_DORLET, FECHAB_DORLET, CONTRATA_DORLET, RUTAS_DORLET, PLANTA  ) VALUES (:ndni,  :NOMBRE, :apellidos, :Empresa, :matricula, :Tarjeta, :FecIni, :FecFin, :contrata, :rutas, :planta)"



                lParameters1.Add(New OracleParameter("nDNI", OracleDbType.Varchar2, 12, docu.DNI.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 50, docu.Nombre.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("apellidos", OracleDbType.Varchar2, 150, docu.Apellidos.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Varchar2, 40, docu.Empresa.ToString.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Tarjeta", OracleDbType.Varchar2, 20, docu.Tarjeta.ToUpper, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("contrata", OracleDbType.Int32, 1, docu.contrata, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("matricula", OracleDbType.Varchar2, 150, docu.matricula.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("rutas", OracleDbType.Varchar2, 150, docu.rutas.ToUpper, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True
            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una trabajador con ese DNI ".ToUpper & docu.DNI, ex)
                End If
                Throw New SabLib.BatzException("Error al grabar un trabajador", ex)
            End Try
        End Function


        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="plant">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SavePlant(ByVal plant As ELL.Plantillas) As Boolean
            If plant.Planta = 230 Then
                plant.Planta = 1
            End If
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim lParameters3 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_pla (pla099, pla000, pla002, pla005) VALUES(:planta, :codigo, :fecha,  :nombre)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, plant.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, plant.documento, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("fecha", OracleDbType.Date, plant.Fecha, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombre", OracleDbType.Varchar2, 200, plant.nombre.ToUpper, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                'caducar  docs si son de plantilla y subo plantilla

                query = "UPDATE adok_emd SET emd004=:fecha, emd007=13 WHERE emd099= :planta and emd001=:codigo "

                '          lParameters2.Add(New OracleParameter("FecRec", OracleDbType.Date, Now, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("planta", OracleDbType.Int32, plant.Planta, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, plant.documento, ParameterDirection.Input))
                '   lParameters2.Add(New OracleParameter("fecha", OracleDbType.Date, plant.Fecha.Date, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("fecha", OracleDbType.Date, CDate("01/01/1900"), ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters2.ToArray)


                query = "UPDATE adok_trd SET trd005=:fecha, trd007 = 13 WHERE trd099= :planta and trd001=:codigo "

                lParameters3.Add(New OracleParameter("planta", OracleDbType.Int32, plant.Planta, ParameterDirection.Input))
                lParameters3.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, plant.documento, ParameterDirection.Input))
                '   lParameters2.Add(New OracleParameter("fecha", OracleDbType.Date, plant.Fecha.Date, ParameterDirection.Input))
                lParameters3.Add(New OracleParameter("fecha", OracleDbType.Date, CDate("01/01/1900"), ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function


        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="plant">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SavePlant2(ByVal plant As ELL.Plantillas) As Boolean
            If plant.Planta = 230 Then
                plant.Planta = 1
            End If
            'son las plantillas de lectura obligatoria pla006 = 1
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim lParameters3 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO adok_pla (pla099, pla000, pla002, pla005, pla006) VALUES(:planta, :codigo, :fecha,  :nombre, 1)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, plant.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, plant.documento, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("fecha", OracleDbType.Date, plant.Fecha, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombre", OracleDbType.Varchar2, 200, plant.nombre.ToUpper, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function
        Public Function Update(ByVal docu As ELL.Documentos, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                If docu.Planta = 230 Then
                    docu.Planta = 1
                End If
                Dim strSql As String = String.Empty
                'no se actualiza , doc014=:tipotrabajo
                strSql = "UPDATE adok_doc SET doc017=:area, doc010=:ETT, doc016=:textoSolicitud, DOC001=:Nombre, doc002=:Abrev, doc003=:Periodo, doc004=:Obligatorio, doc005=:Trabajador, doc006=:comentario, doc007=:Responsable, doc008=:Margen, doc009=:Plantilla, doc012=:EsDocumento, doc013=:listacorreos WHERE DOC000=:codigo And doc099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 150, docu.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Abrev", OracleDbType.Varchar2, 10, docu.Abrev, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Periodo", OracleDbType.Int32, 18, docu.Periodo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Obligatorio", OracleDbType.Int32, 1, docu.Obligatorio, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Trabajador", OracleDbType.Int32, 1, docu.Trabajador, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("comentario", OracleDbType.Varchar2, 250, docu.comentario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Responsable", OracleDbType.Int32, 18, docu.Responsable, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Margen", OracleDbType.Int32, 18, docu.Margen, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Plantilla", OracleDbType.Int32, 1, docu.Plantilla, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("EsDocumento", OracleDbType.Int32, 1, docu.EsDocumento, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("listacorreos", OracleDbType.Varchar2, 150, docu.listacorreos, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 1, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ETT", OracleDbType.Int32, 1, docu.ETT, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("area", OracleDbType.Int32, docu.area, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                'lParameters1.Add(New OracleParameter("tipotrabajo", OracleDbType.Int32, 1, docu.tipotrabajo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("textoSolicitud", OracleDbType.Varchar2, docu.textoSolicitud, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateResponsables(ByVal Responsables As ELL.Responsables, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_res SET res001=:Nombre, res002=:Abrev WHERE res000=:codigo And res099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 150, Responsables.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Abrev", OracleDbType.Varchar2, 10, Responsables.Abrev, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 1, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Responsables.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateCaducidades(ByVal Caducidades As ELL.Caducidades, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_per SET per001=:Nombre, per002=:intervalo, per003=:cantidad WHERE per000=:codigo And per099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 150, Caducidades.nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("intervalo", OracleDbType.Int32, Caducidades.intervalo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("cantidad", OracleDbType.Int32, Caducidades.cantidad, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 1, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Caducidades.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Function UpdateRol(ByVal Rol As ELL.Rol, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_roles SET  rol001=:intervalo WHERE rol000=:codigo And rol099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)

                lParameters1.Add(New OracleParameter("intervalo", OracleDbType.Int32, Rol.rol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Rol.Planta, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Function UpdateRolETT(ByVal Rol As ELL.Rol, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_rolesETT SET  rol001=:intervalo WHERE rol000=:codigo And rol099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)

                lParameters1.Add(New OracleParameter("intervalo", OracleDbType.Int32, Rol.rol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Rol.Planta, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmp(ByVal docu As ELL.Empresas, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                If docu.Planta = 230 Then
                    docu.Planta = 1
                End If
                Dim strSql As String = String.Empty

                ' strSql = "UPDATE adok_emp SET emp001=:NIf, emp002=:Nombre, emp004=:DocSol, emp005=:FecSolEnv, emp006=:medio, emp007=:DocEnv, emp008=:FecEnv, emp009=:medio2, emp010=:DocRec,emp011=:FecRec,emp012=:recibi, emp021=:Autonomo, emp013=:preventiva,emp014=:interlocutor,emp015=:telefono,emp016=:email,emp017=:fax,emp019=:subcontrata,emp020=:contacto WHERE emp000=:codigo And emp099= :planta "
                strSql = "UPDATE adok_emp SET emp001=:NIf, emp002=:Nombre, emp013=:preventiva,emp014=:interlocutor,emp015=:telefono,emp016=:email,emp017=:fax,emp019=:subcontrata,emp020=:contacto,emp023=:notificar WHERE emp000=:codigo And emp099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("NIF", OracleDbType.Varchar2, 12, docu.Nif.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, docu.Nombre.ToUpper, ParameterDirection.Input))

                'lParameters1.Add(New OracleParameter("Autonomo", OracleDbType.Int32, 1, docu.Autonomo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("preventiva", OracleDbType.Varchar2, 50, docu.preventiva.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("interlocutor", OracleDbType.Varchar2, 50, docu.interlocutor.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("telefono", OracleDbType.Varchar2, 250, docu.telefono, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 250, docu.email, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("fax", OracleDbType.Varchar2, 250, docu.fax, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("subcontrata", OracleDbType.Varchar2, 100, docu.subcontrata.ToUpper, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("contacto", OracleDbType.Varchar2, 50, docu.contacto.ToLower, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("notificar", OracleDbType.Varchar2, 50, docu.notificar.ToLower, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function




        Public Function UpdateEmailSol(ByVal docu As ELL.Empresas, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_sol SET sol008=:email WHERE sol001=:codigo And sol099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)

                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 250, docu.email, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function



        Public Function UpdateTra(ByVal docu As ELL.Trabajadores, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                'If docu.Planta = 230 Then
                '    docu.Planta = 1
                'End If
                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_tra SET tra001=:ndni,  tra013=:tdni, tra002=:Nombre, tra003=:Apellidos, tra004=:Empresa, tra005=:FecIni, tra006=:FecFin, tra015=:Autonomo, tra011=:puesto, tra007=:funcion, tra008=:responsable, tra014=:solicitud  WHERE tra000=:codigo And tra099= :planta "
                '   strSql = "UPDATE adok_tra SET tra001=:dni, tra002=:Nombre, tra003=:Apellidos, tra004=:Empresa, tra015=:Autonomo  WHERE tra000=:codigo And tra099= :planta "


                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("nDNI", OracleDbType.Varchar2, 12, docu.nDNI.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, docu.tDNI.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 50, docu.Nombre.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("apellidos", OracleDbType.Varchar2, 150, docu.Apellidos.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Int32, 6, docu.Empresa, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Autonomo", OracleDbType.Int32, 1, docu.Autonomo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Solicitud", OracleDbType.Varchar2, docu.solicitud, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Int32, 1, docu.responsable, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("puesto", OracleDbType.Varchar2, 150, docu.puesto.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("funcion", OracleDbType.Varchar2, 150, docu.funcion.ToUpper, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function SaveSol(ByVal docu As ELL.Solicitudes) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            docu.FecFin = docu.FecFin.AddHours(23)
            Try
                query = "INSERT INTO adok_sol (sol017, sol099, sol024,  sol001, sol002, sol003, sol004, sol005,sol006,sol007,sol008,sol009, sol010, sol012, sol013, sol014, sol015, sol016, sol018, sol019, sol020, sol021, sol022) VALUES(:iduser, :planta, :plantaseleccionada,  :Empresa, :responsable, :FecIni, :FecFin,:soldadura, :altura, :salas, :email, :otros, :descripcion, 0, :DescSolicitante, :gases, :elevados, :fosas, :X7, :X8, :X9, :X10, :X11)"
                'EmpresaTroquelaje seria sol011
                lParameters1.Add(New OracleParameter("iduser", OracleDbType.Int32, docu.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Varchar2, 12, docu.Empresa, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Int32, docu.responsable, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("soldadura", OracleDbType.Int32, 1, docu.soldadura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("altura", OracleDbType.Int32, 1, docu.altura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("salas", OracleDbType.Int32, 1, docu.salas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("gases", OracleDbType.Int32, 1, docu.gases, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("elevados", OracleDbType.Int32, 1, docu.elevados, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("fosas", OracleDbType.Int32, 1, docu.fosas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X7", OracleDbType.Int32, 1, docu.X7, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X8", OracleDbType.Int32, 1, docu.X8, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X9", OracleDbType.Int32, 1, docu.X9, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X10", OracleDbType.Int32, 1, docu.X10, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X11", OracleDbType.Int32, 1, docu.X11, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 50, Trim(docu.email), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("otros", OracleDbType.Varchar2, 150, Trim(docu.otros), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, 150, Trim(docu.descripcion), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DescSolicitante", OracleDbType.Varchar2, 50, Trim(docu.DescSolicitante), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("plantaseleccionada", OracleDbType.Int32, docu.plantaSeleccionada, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                'los 6 tipos en EMD para ese WHERE sol000=:codigo (es EMD015) And sol099= :planta e insertas los que tenga a 1 mas fecha de caducidad EMD014



                Return True
            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una solicitud con ese codigo ".ToUpper, ex)
                End If
                Throw New SabLib.BatzException("Error al grabar una solicitud", ex)
            End Try
        End Function




        Public Function SaveSolETT(ByVal docu As ELL.Solicitudes) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            docu.FecFin = docu.FecFin.AddHours(23)
            Try
                query = "INSERT INTO adok_solETT (sol024, sol025, sol017, sol099,  sol001, sol002, sol003, sol004, sol005,sol006,sol007,sol008,sol009, sol010, sol012, sol013, sol014, sol015, sol016, sol018, sol019, sol020, sol021, sol022) VALUES(:area, :numero, :iduser, :planta,  :Empresa, :responsable, :FecIni, :FecFin,:soldadura, :altura, :salas, :email, :otros, :descripcion, 0, :DescSolicitante, :gases, :elevados, :fosas, :X7, :X8, :X9, :X10, :X11)"
                'EmpresaTroquelaje seria sol011
                lParameters1.Add(New OracleParameter("area", OracleDbType.Int32, docu.Area, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("numero", OracleDbType.Int32, docu.Numero, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("iduser", OracleDbType.Int32, docu.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Varchar2, 12, docu.Empresa, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Int32, docu.responsable, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("soldadura", OracleDbType.Int32, 1, docu.soldadura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("altura", OracleDbType.Int32, 1, docu.altura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("salas", OracleDbType.Int32, 1, docu.salas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("gases", OracleDbType.Int32, 1, docu.gases, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("elevados", OracleDbType.Int32, 1, docu.elevados, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("fosas", OracleDbType.Int32, 1, docu.fosas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X7", OracleDbType.Int32, 1, docu.X7, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X8", OracleDbType.Int32, 1, docu.X8, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X9", OracleDbType.Int32, 1, docu.X9, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X10", OracleDbType.Int32, 1, docu.X10, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X11", OracleDbType.Int32, 1, docu.X11, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 50, Trim(docu.email), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("otros", OracleDbType.Varchar2, 150, Trim(docu.otros), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, 150, Trim(docu.descripcion), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DescSolicitante", OracleDbType.Varchar2, 50, Trim(docu.DescSolicitante), ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                'los 6 tipos en EMD para ese WHERE sol000=:codigo (es EMD015) And sol099= :planta e insertas los que tenga a 1 mas fecha de caducidad EMD014



                Return True
            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una solicitud con ese codigo ".ToUpper, ex)
                End If
                Throw New SabLib.BatzException("Error al grabar una solicitud", ex)
            End Try
        End Function




        Public Function UpdateSolSubcontrata(ByVal docu As ELL.Solicitudes, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_sol SET  sol023=:Subcontrata  WHERE sol000=:codigo And sol099= :planta "

                'EmpresaTroquelaje seria sol011
                Dim lParameters1 As New List(Of OracleParameter)

                lParameters1.Add(New OracleParameter("Subcontrata", OracleDbType.Varchar2, 100, docu.Subcontrata, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)



                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateSol(ByVal docu As ELL.Solicitudes, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                docu.FecFin = docu.FecFin.AddHours(23)
                Dim strSql As String = String.Empty

                'strSql = "UPDATE adok_tra SET tra001=:ndni,  tra013=:tdni, tra002=:Nombre, tra003=:Apellidos, tra004=:Empresa, tra005=:FecIni, tra006=:FecFin, tra015=:Autonomo, tra011=:puesto, tra007=:funcion, tra008=:responsable  WHERE tra000=:codigo And tra099= :planta "
                strSql = "UPDATE adok_sol SET sol024=:plantaseleccionada, sol001=:Empresa, sol023=:Subcontrata,  sol002=:responsable, sol003=:FecIni, sol004=:FecFin, sol005=:soldadura,sol006=:altura,sol007=:salas, sol008=:email, sol009=:otros, sol010=:descripcion, sol013=:DescSolicitante, sol014=:gases, sol015=:elevados , sol016=:fosas, sol018=:X7, sol019=:X8, sol020=:X9, sol021=:X10, sol022=:X11  WHERE sol000=:codigo And sol099= :planta "

                'EmpresaTroquelaje seria sol011
                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Varchar2, 12, docu.Empresa, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Subcontrata", OracleDbType.Varchar2, 100, docu.Subcontrata, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Int32, docu.responsable, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("soldadura", OracleDbType.Int32, 1, docu.soldadura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("altura", OracleDbType.Int32, 1, docu.altura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("salas", OracleDbType.Int32, 1, docu.salas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("gases", OracleDbType.Int32, 1, docu.gases, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("elevados", OracleDbType.Int32, 1, docu.elevados, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("fosas", OracleDbType.Int32, 1, docu.fosas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X7", OracleDbType.Int32, 1, docu.X7, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X8", OracleDbType.Int32, 1, docu.X8, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X9", OracleDbType.Int32, 1, docu.X9, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X10", OracleDbType.Int32, 1, docu.X10, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X11", OracleDbType.Int32, 1, docu.X11, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 50, Trim(docu.email), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("otros", OracleDbType.Varchar2, 150, Trim(docu.otros), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, 150, Trim(docu.descripcion), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DescSolicitante", OracleDbType.Varchar2, 150, Trim(docu.DescSolicitante), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("plantaseleccionada", OracleDbType.Int32, docu.plantaSeleccionada, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)




                'borrar los 6 tipos en EMD para ese WHERE sol000=:codigo (es EMD015) And sol099= :planta e insertas los que tenga a 1 mas fecha de caducidad EMD014
                strSql = "DELETE FROM adok_emd WHERE emd099 = " & docu.Planta & " and emd015 = " & codigo

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion)

                'si se ha seleccionado empresa
                Dim sql As String
                Dim periodicidad As String
                Dim numreg As Integer
                Dim coddoc As String

                Dim tipoDocumento As Integer

                Select Case tipoDocumento
                    Case docu.soldadura = 1
                        tipoDocumento = 1
                    Case docu.altura = 1
                        tipoDocumento = 2
                    Case docu.salas = 1
                        tipoDocumento = 3
                    Case docu.gases = 1
                        tipoDocumento = 4
                    Case docu.elevados = 1
                        tipoDocumento = 5
                    Case docu.fosas = 1
                        tipoDocumento = 6
                    Case docu.X7 = 1
                        tipoDocumento = 7
                    Case docu.X8 = 1
                        tipoDocumento = 8
                    Case docu.X9 = 1
                        tipoDocumento = 9
                    Case docu.X10 = 1
                        tipoDocumento = 10
                    Case docu.X11 = 1
                        tipoDocumento = 11
                End Select



                If docu.Empresa > 0 Then
                    'los 6
                    '   If docu.soldadura = 1 Then
                    'busco en adoc_doc el que tenga doc014 = 1
                    sql = " select doc000, doc003  from adok_doc where doc014 = " & tipoDocumento & " order by doc000 desc "
                    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    If numreg > 0 Then
                        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                        'mirar si ya existe y si no insertar
                        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                        If numreg = 0 Then
                            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                            Dim lParameters11 As New List(Of OracleParameter)
                            lParameters11.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters11.ToArray)
                        End If
                    End If
                    '   End If
                    ''''''''    If docu.altura = 1 Then
                    ''''''''    'busco en adoc_doc el que tenga doc014 = 2
                    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 2 order by doc000 desc "
                    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''    If numreg > 0 Then
                    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                    ''''''''        'mirar si ya existe y si no insertar
                    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''        If numreg = 0 Then

                    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                    ''''''''        End If
                    ''''''''    End If
                    ''''''''End If





                    ''''''''If docu.salas = 1 Then
                    ''''''''    'busco en adoc_doc el que tenga doc014 = 3
                    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 3 order by doc000 desc "
                    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''    If numreg > 0 Then
                    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                    ''''''''        'mirar si ya existe y si no insertar
                    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''        If numreg = 0 Then

                    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                    ''''''''        End If
                    ''''''''    End If
                    ''''''''End If
                    ''''''''If docu.gases = 1 Then
                    ''''''''    'busco en adoc_doc el que tenga doc014 = 4
                    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 4 order by doc000 desc "
                    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''    If numreg > 0 Then
                    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                    ''''''''        'mirar si ya existe y si no insertar
                    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''        If numreg = 0 Then

                    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                    ''''''''        End If
                    ''''''''    End If
                    ''''''''End If
                    ''''''''If docu.elevados = 1 Then
                    ''''''''    'busco en adoc_doc el que tenga doc014 = 5
                    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 5 order by doc000 desc "
                    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''    If numreg > 0 Then
                    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                    ''''''''        'mirar si ya existe y si no insertar
                    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''        If numreg = 0 Then

                    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                    ''''''''        End If
                    ''''''''    End If
                    ''''''''End If
                    ''''''''If docu.fosas = 1 Then
                    ''''''''    'busco en adoc_doc el que tenga doc014 = 6
                    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 6 order by doc000 desc "
                    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''    If numreg > 0 Then
                    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                    ''''''''        'mirar si ya existe y si no insertar
                    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                    ''''''''        If numreg = 0 Then

                    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                    ''''''''        End If
                    ''''''''    End If
                    ''''''''End If



                End If



                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function





        Public Function UpdateSolETT(ByVal docu As ELL.Solicitudes, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                docu.FecFin = docu.FecFin.AddHours(23)
                Dim strSql As String = String.Empty

                'strSql = "UPDATE adok_tra SET tra001=:ndni,  tra013=:tdni, tra002=:Nombre, tra003=:Apellidos, tra004=:Empresa, tra005=:FecIni, tra006=:FecFin, tra015=:Autonomo, tra011=:puesto, tra007=:funcion, tra008=:responsable  WHERE tra000=:codigo And tra099= :planta "
                strSql = "UPDATE adok_solETT SET sol024=:area, sol025=:numero, sol001=:Empresa, sol023=:Subcontrata,  sol002=:responsable, sol003=:FecIni, sol004=:FecFin, sol005=:soldadura,sol006=:altura,sol007=:salas, sol008=:email, sol009=:otros, sol010=:descripcion, sol013=:DescSolicitante, sol014=:gases, sol015=:elevados , sol016=:fosas, sol018=:X7, sol019=:X8, sol020=:X9, sol021=:X10, sol022=:X11  WHERE sol000=:codigo And sol099= :planta "

                'EmpresaTroquelaje seria sol011
                Dim lParameters1 As New List(Of OracleParameter)
                lParameters1.Add(New OracleParameter("area", OracleDbType.Int32, docu.Area, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("numero", OracleDbType.Int32, docu.Numero, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Varchar2, 12, docu.Empresa, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Subcontrata", OracleDbType.Varchar2, 100, docu.Subcontrata, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("responsable", OracleDbType.Int32, docu.responsable, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("soldadura", OracleDbType.Int32, 1, docu.soldadura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("altura", OracleDbType.Int32, 1, docu.altura, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("salas", OracleDbType.Int32, 1, docu.salas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("gases", OracleDbType.Int32, 1, docu.gases, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("elevados", OracleDbType.Int32, 1, docu.elevados, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("fosas", OracleDbType.Int32, 1, docu.fosas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X7", OracleDbType.Int32, 1, docu.X7, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X8", OracleDbType.Int32, 1, docu.X8, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X9", OracleDbType.Int32, 1, docu.X9, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X10", OracleDbType.Int32, 1, docu.X10, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("X11", OracleDbType.Int32, 1, docu.X11, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("email", OracleDbType.Varchar2, 50, Trim(docu.email), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("otros", OracleDbType.Varchar2, 150, Trim(docu.otros), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("descripcion", OracleDbType.Varchar2, 150, Trim(docu.descripcion), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DescSolicitante", OracleDbType.Varchar2, 150, Trim(docu.DescSolicitante), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)


                'jon no se que hacer aqui

                '''''''''''''''borrar los 6 tipos en EMD para ese WHERE sol000=:codigo (es EMD015) And sol099= :planta e insertas los que tenga a 1 mas fecha de caducidad EMD014
                ''''''''''''''strSql = "DELETE FROM adok_emd WHERE emd099 = " & docu.Planta & " and emd015 = " & codigo

                ''''''''''''''Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion)

                '''''''''''''''si se ha seleccionado empresa
                ''''''''''''''Dim sql As String
                ''''''''''''''Dim periodicidad As String
                ''''''''''''''Dim numreg As Integer
                ''''''''''''''Dim coddoc As String

                ''''''''''''''Dim tipoDocumento As Integer

                ''''''''''''''Select Case tipoDocumento
                ''''''''''''''    Case docu.soldadura = 1
                ''''''''''''''        tipoDocumento = 1
                ''''''''''''''    Case docu.altura = 1
                ''''''''''''''        tipoDocumento = 2
                ''''''''''''''    Case docu.salas = 1
                ''''''''''''''        tipoDocumento = 3
                ''''''''''''''    Case docu.gases = 1
                ''''''''''''''        tipoDocumento = 4
                ''''''''''''''    Case docu.elevados = 1
                ''''''''''''''        tipoDocumento = 5
                ''''''''''''''    Case docu.fosas = 1
                ''''''''''''''        tipoDocumento = 6
                ''''''''''''''    Case docu.X7 = 1
                ''''''''''''''        tipoDocumento = 7
                ''''''''''''''    Case docu.X8 = 1
                ''''''''''''''        tipoDocumento = 8
                ''''''''''''''    Case docu.X9 = 1
                ''''''''''''''        tipoDocumento = 9
                ''''''''''''''    Case docu.X10 = 1
                ''''''''''''''        tipoDocumento = 10
                ''''''''''''''    Case docu.X11 = 1
                ''''''''''''''        tipoDocumento = 11
                ''''''''''''''End Select



                ''''''''''''''If docu.Empresa > 0 Then
                ''''''''''''''    'los 6
                ''''''''''''''    '   If docu.soldadura = 1 Then
                ''''''''''''''    'busco en adoc_doc el que tenga doc014 = 1
                ''''''''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = " & tipoDocumento & " order by doc000 desc "
                ''''''''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    If numreg > 0 Then
                ''''''''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                ''''''''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                ''''''''''''''        'mirar si ya existe y si no insertar
                ''''''''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                ''''''''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''        If numreg = 0 Then
                ''''''''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                ''''''''''''''            Dim lParameters11 As New List(Of OracleParameter)
                ''''''''''''''            lParameters11.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                ''''''''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters11.ToArray)
                ''''''''''''''        End If
                ''''''''''''''    End If
                ''''''''''''''    '   End If
                ''''''''''''''    ''''''''    If docu.altura = 1 Then
                ''''''''''''''    ''''''''    'busco en adoc_doc el que tenga doc014 = 2
                ''''''''''''''    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 2 order by doc000 desc "
                ''''''''''''''    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''    If numreg > 0 Then
                ''''''''''''''    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                ''''''''''''''    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                ''''''''''''''    ''''''''        'mirar si ya existe y si no insertar
                ''''''''''''''    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                ''''''''''''''    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''        If numreg = 0 Then

                ''''''''''''''    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                ''''''''''''''    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                ''''''''''''''    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                ''''''''''''''    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                ''''''''''''''    ''''''''        End If
                ''''''''''''''    ''''''''    End If
                ''''''''''''''    ''''''''End If





                ''''''''''''''    ''''''''If docu.salas = 1 Then
                ''''''''''''''    ''''''''    'busco en adoc_doc el que tenga doc014 = 3
                ''''''''''''''    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 3 order by doc000 desc "
                ''''''''''''''    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''    If numreg > 0 Then
                ''''''''''''''    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                ''''''''''''''    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                ''''''''''''''    ''''''''        'mirar si ya existe y si no insertar
                ''''''''''''''    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                ''''''''''''''    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''        If numreg = 0 Then

                ''''''''''''''    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                ''''''''''''''    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                ''''''''''''''    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                ''''''''''''''    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                ''''''''''''''    ''''''''        End If
                ''''''''''''''    ''''''''    End If
                ''''''''''''''    ''''''''End If
                ''''''''''''''    ''''''''If docu.gases = 1 Then
                ''''''''''''''    ''''''''    'busco en adoc_doc el que tenga doc014 = 4
                ''''''''''''''    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 4 order by doc000 desc "
                ''''''''''''''    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''    If numreg > 0 Then
                ''''''''''''''    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                ''''''''''''''    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                ''''''''''''''    ''''''''        'mirar si ya existe y si no insertar
                ''''''''''''''    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                ''''''''''''''    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''        If numreg = 0 Then

                ''''''''''''''    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                ''''''''''''''    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                ''''''''''''''    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                ''''''''''''''    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                ''''''''''''''    ''''''''        End If
                ''''''''''''''    ''''''''    End If
                ''''''''''''''    ''''''''End If
                ''''''''''''''    ''''''''If docu.elevados = 1 Then
                ''''''''''''''    ''''''''    'busco en adoc_doc el que tenga doc014 = 5
                ''''''''''''''    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 5 order by doc000 desc "
                ''''''''''''''    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''    If numreg > 0 Then
                ''''''''''''''    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                ''''''''''''''    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                ''''''''''''''    ''''''''        'mirar si ya existe y si no insertar
                ''''''''''''''    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                ''''''''''''''    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''        If numreg = 0 Then

                ''''''''''''''    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                ''''''''''''''    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                ''''''''''''''    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                ''''''''''''''    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                ''''''''''''''    ''''''''        End If
                ''''''''''''''    ''''''''    End If
                ''''''''''''''    ''''''''End If
                ''''''''''''''    ''''''''If docu.fosas = 1 Then
                ''''''''''''''    ''''''''    'busco en adoc_doc el que tenga doc014 = 6
                ''''''''''''''    ''''''''    sql = " select doc000, doc003  from adok_doc where doc014 = 6 order by doc000 desc "
                ''''''''''''''    ''''''''    numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''    If numreg > 0 Then
                ''''''''''''''    ''''''''        coddoc = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(0)
                ''''''''''''''    ''''''''        periodicidad = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion)(0)(1)

                ''''''''''''''    ''''''''        'mirar si ya existe y si no insertar
                ''''''''''''''    ''''''''        sql = " select emd000 from adok_emd where emd099 = " & docu.Planta & " and emd000 = " & docu.Empresa & " and emd001 = " & coddoc
                ''''''''''''''    ''''''''        numreg = Memcached.OracleDirectAccess.Seleccionar(sql, Conexion).Count
                ''''''''''''''    ''''''''        If numreg = 0 Then

                ''''''''''''''    ''''''''            strSql = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013, emd014, emd015) VALUES(" & docu.Planta & "," & docu.Empresa & "," & coddoc & ", 2, " & periodicidad & ", 0, :FecFin" & " , " & codigo & ")"
                ''''''''''''''    ''''''''            Dim lParameters2 As New List(Of OracleParameter)
                ''''''''''''''    ''''''''            lParameters2.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                ''''''''''''''    ''''''''            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters2.ToArray)
                ''''''''''''''    ''''''''        End If
                ''''''''''''''    ''''''''    End If
                ''''''''''''''    ''''''''End If



                ''''''''''''''End If



                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function





        Public Function UpdateSolSub(ByVal docu As ELL.Solicitudes, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_sol SET sol023=:Subcontrata  WHERE sol000=:codigo And sol099= :planta "

                Dim lParameters1 As New List(Of OracleParameter)

                lParameters1.Add(New OracleParameter("Subcontrata", OracleDbType.Varchar2, 100, docu.Subcontrata, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 6, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)


                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function



        Public Function UpdateDorlet(ByVal docu As ELL.Dorlet, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE adok_dorlet SET DNI_DORLET=:ndni,   NOMBRE_DORLET=:Nombre, APELLIDOS_DORLET=:Apellidos, EMPRESA_DORLET=:Empresa, MATRICULA_DORLET=:matricula, TARJETA_DORLET=:tarjeta, FECHAA_DORLET=:FecIni, FECHAB_DORLET=:FecFin, CONTRATA_DORLET=:contrata, RUTAS_DORLET=:rutas  WHERE ID_DORLET=:codigo And PLANTA= :planta "




                Dim lParameters1 As New List(Of OracleParameter)

                lParameters1.Add(New OracleParameter("nDNI", OracleDbType.Varchar2, 12, docu.DNI.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 50, docu.Nombre.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("apellidos", OracleDbType.Varchar2, 150, docu.Apellidos.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Empresa", OracleDbType.Varchar2, 40, docu.Empresa.ToString.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Tarjeta", OracleDbType.Varchar2, 20, docu.Tarjeta.ToUpper, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("contrata", OracleDbType.Int32, 1, docu.contrata, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("matricula", OracleDbType.Varchar2, 150, docu.matricula.ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("rutas", OracleDbType.Varchar2, 150, docu.rutas.ToUpper, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, 18, codigo, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function



        Public Function BorrarPuestos() As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "DELETE adok_emp  "



                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Function BorrarTrabajadores() As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "DELETE adok_tra  "



                Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexion)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function DeleteSol(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM adok_trs WHERE trs099 = " & planta & " and trs000 = " & codigo


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar la solicitud", ex)
            End Try
            Return resultado
        End Function

        Public Function DeletePer(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM adok_per WHERE per099 = " & planta & " and per000 = " & codigo


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar una periodicidad", ex)
            End Try
            Return resultado
        End Function

        Public Function DeleteRes(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM adok_res WHERE res099 = " & planta & " and res000 = " & codigo


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar un responsable", ex)
            End Try
            Return resultado
        End Function


        Public Function DeleteRol(ByVal planta As Int32, ByVal codigo As Int32, ByVal rol As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM adok_roles WHERE rol099 = " & planta & " and rol000 = " & codigo & " and rol001 = " & rol


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar un rol", ex)
            End Try
            Return resultado
        End Function


        Public Function DeleteRolETT(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM adok_rolesETT WHERE rol099 = " & planta & " and rol000 = " & codigo


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar un rol", ex)
            End Try
            Return resultado
        End Function

        Public Function DeleteEmail(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM adok_dos WHERE dos099 = " & planta & " and dos000=1 and dos001 = " & codigo


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar un mail", ex)
            End Try
            Return resultado
        End Function
        Public Function DeleteEmailETT(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM adok_dosETT WHERE dos099 = " & planta & " and dos000=1 and dos001 = " & codigo


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar un mail", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateTraDoc2(ByVal docu As ELL.TrabajadoresDoc, ByVal chequeado As Int32, ByVal nombre As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            query = "UPDATE adok_trd SET trd009=0 WHERE trd000=:codtra And trd001=:coddoc "

            lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
            lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))


            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

            Return True
        End Function

        Public Function UpdateTraDoc3(ByVal docu As ELL.TrabajadoresDoc, ByVal chequeado As Int32, ByVal nombre As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            query = "UPDATE adok_trd SET trd015=:chequeado WHERE trd000=:codtra And trd001=:coddoc "

            lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
            lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
            lParameters1.Add(New OracleParameter("chequeado", OracleDbType.Int32, 8, chequeado, ParameterDirection.Input))



            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

            Return True
        End Function

        Public Function DesactivarTraDoc(ByVal doc As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            query = "UPDATE adok_trd SET trd015=0 WHERE trd000=:codtra  "

            lParameters1.Add(New OracleParameter("codtra", OracleDbType.Varchar2, 15, doc, ParameterDirection.Input))




            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

            Return True
        End Function

        Public Function UpdateTraDoc(ByVal docu As ELL.TrabajadoresDoc, ByVal chequeado As Int32, ByVal nombre As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim anno As Integer = 0
            Try


                If IsDate(docu.FecIni) Then
                    anno = docu.FecIni.Year
                End If

                'lo primero borrar del historico los doc docu.coddoc y docu.codtra que sean de ese mismo año
                'pongo trd099 = 2
                lParameters2.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("anno", OracleDbType.Int32, anno, ParameterDirection.Input))
                '02/10/2020 cambio por peticion jonhatan jon
                query = "UPDATE adok_trd_hist SET trd099=1 WHERE trd000=:codtra And trd001=:coddoc and trd010 = :anno"
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters2.ToArray)

                If chequeado = 1 Then
                    query = "DELETE FROM adok_trd WHERE trd099 = " & docu.Planta & " and trd001 = " & docu.coddoc & " and trd000 = " & docu.codtra



                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 0 Then
                    query = "DELETE FROM adok_trd WHERE trd000 = " & docu.codtra & " and trd001 = " & docu.coddoc
                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                    ' query = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd011) VALUES(:planta, :codtra, :coddoc,  1, :autor)"
                    query = "INSERT INTO adok_trd (trd099, trd000, trd001, trd002, trd007, trd014) VALUES(:planta, :codtra, :coddoc,  2, :periodicidad, :cadsolicitud)"

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, 8, docu.periodicidad, ParameterDirection.Input))
                    If IsDate(docu.FecCad) Then
                        lParameters1.Add(New OracleParameter("cadsolicitud", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    Else
                        lParameters1.Add(New OracleParameter("cadsolicitud", OracleDbType.Date, Date.MaxValue, ParameterDirection.Input))
                    End If


                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                End If
                If chequeado = 2 Then 'al subir

                    query = "UPDATE adok_trd SET trd010=:anno, trd013=:tiposcarne, trd012='', trd007=:periodo, trd011=:autor, TRD006=:ubicacion, TRD003 =:FecRec, trd002 =:estado, TRD009 = 5, TRD005 =:FecCad, TRD004 =:FecIni WHERE trd099= :planta and trd000=:codtra And trd001=:coddoc "

                    lParameters1.Add(New OracleParameter("tiposcarne", OracleDbType.Varchar2, docu.tiposCarne, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 300, docu.ubicacion, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, Now, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("anno", OracleDbType.Int32, anno, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, docu.estado, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodo", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 3 Then 'actualizamos resto de campos
                    query = "UPDATE adok_trd SET trd010=:anno, trd013=:tiposCarne, trd011=:autor,   trd003 =:FecRec , trd009 =:Aptitud , trd002 =:estado , trd007 =:periodicidad, trd005 =:FecCad, trd004 =:FecIni, trd012 =:Comentario   WHERE trd099= :planta and trd000=:codtra And trd001=:coddoc "

                    ' emd005=:ubicacion,    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 150, docu.ubicacion, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, docu.FecRec, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Aptitud", OracleDbType.Int32, docu.Aptitud, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, docu.estado, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("anno", OracleDbType.Int32, anno, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Comentario", OracleDbType.Varchar2, 250, docu.Comentario, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("tiposCarne", OracleDbType.Varchar2, 250, docu.tiposCarne, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 4 Then 'actualizamos desde medico
                    query = "UPDATE adok_trd SET trd016=:FecRev, trd010=:anno, trd013=:tiposCarne, trd011=:autor,    trd009 =:Aptitud , trd002 =:estado , trd007 =:periodicidad, trd012 =:Comentario   WHERE trd099= :planta and trd000=:codtra And trd001=:coddoc "

                    ' emd005=:ubicacion,    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 150, docu.ubicacion, ParameterDirection.Input))
                    '       lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, docu.FecRec, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Aptitud", OracleDbType.Int32, docu.Aptitud, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, docu.estado, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("anno", OracleDbType.Int32, anno, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    '      lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    '       lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRev", OracleDbType.Date, docu.FecRev, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Comentario", OracleDbType.Varchar2, 250, docu.Comentario, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("tiposCarne", OracleDbType.Varchar2, 250, docu.tiposCarne, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 6 Then 'al subir historico

                    'query = "UPDATE adok_trd SET trd013=:tiposcarne, trd012='', trd007=:periodo, trd011=:autor, TRD006=:ubicacion, TRD003 =:FecRec, trd002 =:estado, TRD009 = 5, TRD005 =:FecCad, TRD004 =:FecIni WHERE trd099= :planta and trd000=:codtra And trd001=:coddoc "
                    query = "INSERT INTO adok_trd_hist (trd099,trd010,trd013,trd012,trd007, trd011, TRD006, TRD003,trd002,TRD009, TRD005, TRD004, trd000, trd001 ) VALUES(:planta,:anno,:tiposcarne,'', :periodo, :autor,   :ubicacion, :FecRec, 2, 5, :FecCad, :FecIni, :codtra, :coddoc)"


                    lParameters1.Add(New OracleParameter("tiposcarne", OracleDbType.Varchar2, docu.tiposCarne, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 300, docu.ubicacion, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, Now, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("anno", OracleDbType.Int32, anno, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))
                    '      lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, 2, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodo", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el documento de trabajador", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateTraSol(ByVal docu As ELL.TrabajadoresDoc, ByVal chequeado As Int32, ByVal responsable As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim fechaCad As Date
            Dim fechaCadPed As Date
            Dim query As String = String.Empty
            Dim sql As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim lParameters3 As New List(Of OracleParameter)
            Dim resultados As List(Of ELL.EmpresasDoc)
            Dim solicitudes As String = String.Empty
            Dim arrSolicitudes As String() = Nothing
            Dim no_grabar As Integer
            Try
                If chequeado = 1 Then
                    query = "DELETE FROM adok_trs WHERE trs099 = " & docu.Planta & " and trs001 = " & docu.coddoc & " and trs000 = " & docu.codtra



                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)



                    'habra que insertar fecha caducidad al trabajador. Si ya tiene mayor por otro pedido no se pone.

                    sql = "select tra006, tra014 from adok_tra where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("tra014")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, sql, CadenaConexion)

                    fechaCad = resultados(0).FecCad
                    solicitudes = resultados(0).Comentario
                    sql = "Select sol004  FROM adok_sol  WHERE sol099= " & docu.Planta & " AND sol000 = " & docu.coddoc

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.FecCad = SabLib.BLL.Utils.dateTimeNull(r("sol004"))}, sql, CadenaConexion)

                    If resultados.Count > 0 Then
                        fechaCadPed = resultados(0).FecCad
                    Else
                        fechaCadPed = Date.MinValue
                    End If

                    'no lo hago al quitar si al poner
                    'If fechaCad < fechaCadPed Then
                    '    query = "UPDATE adok_tra SET tra006=:fecha where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    '    lParameters2.Add(New OracleParameter("fecha", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))

                    '    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters2.ToArray)
                    'End If

                    '    no_grabar = 0

                    ' quitaremos en  tra014 la solicitud

                    arrSolicitudes = Split(solicitudes, ";")
                    If arrSolicitudes.Length > 1 Then

                        solicitudes = ""
                        For i = 0 To arrSolicitudes.Length - 1
                            If arrSolicitudes(i) = docu.coddoc.ToString Then
                            Else
                                solicitudes = solicitudes & arrSolicitudes(i) & ";"

                            End If
                        Next
                    End If

                    '      If no_grabar = 0 Then
                    '      solicitudes = solicitudes & ";" & docu.coddoc.ToString & ";" ' & docu.Comentario & ";" & fechaCadPed
                    query = "UPDATE adok_tra SET tra014=:solicitudes where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    lParameters3.Add(New OracleParameter("solicitudes", OracleDbType.Varchar2, solicitudes, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)
                    '     End If




                End If
                If chequeado = 0 Then
                    ' query = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd011) VALUES(:planta, :codtra, :coddoc,  1, :autor)"
                    query = "INSERT INTO adok_trs (trs099, trs000, trs001, trs002) VALUES(:planta, :codtra, :codsol, :codemp)"

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codsol", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 8, docu.clave, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                    'habra que insertar fecha caducidad al trabajador. Si ya tiene mayor por otro pedido no se pone.

                    sql = "select tra006, tra014 from adok_tra where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("tra014")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, sql, CadenaConexion)

                    fechaCad = resultados(0).FecCad
                    solicitudes = resultados(0).Comentario
                    sql = "Select sol004  FROM adok_sol  WHERE sol099= " & docu.Planta & " AND sol000 = " & docu.coddoc

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.FecCad = SabLib.BLL.Utils.dateTimeNull(r("sol004"))}, sql, CadenaConexion)
                    If resultados.Count > 0 Then
                        fechaCadPed = resultados(0).FecCad
                    Else
                        fechaCadPed = Date.MinValue
                    End If


                    If fechaCad < fechaCadPed Then
                        query = "UPDATE adok_tra SET tra006=:fecha, tra008=:responsable where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                        lParameters2.Add(New OracleParameter("fecha", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("responsable", OracleDbType.Int32, responsable, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters2.ToArray)


                        'actualizar dorlet


                        Dim existe As List(Of String())
                        existe = loadIzaroTrabajador(docu.Planta, Trim(docu.NIF).ToUpper)
                        If existe.Count > 0 Then


                            'Con docu.codtra sacar nombre , 
                            Dim nombrea As String
                            Dim apellidos As String
                            Dim tarjeta As String


                            Dim lista As List(Of ELL.Trabajadores)
                            lista = loadListTrabajadoresClaveTra(docu.Planta, docu.codtra)
                            If lista.Count > 0 Then
                                nombrea = Split(lista(0).Nombre, ", ")(1)
                                apellidos = Split(lista(0).Nombre, ",")(0)
                            Else
                                nombrea = ""
                                apellidos = ""
                            End If
                            ' con CInt(existe(0)(0)) sacar tarjeta
                            Dim etarjeta As List(Of String())
                            etarjeta = loadIzaroTrabajador2(1, CInt(existe(0)(0))) 'si ese qtrabajador tiene tarjera en fcpwtar
                            If existe.Count > 0 Then
                                tarjeta = String.Format("{0,-8}", Left(etarjeta(0)(0), 8))
                            Else
                                tarjeta = ""
                            End If




                            '''''''Dim fic As String
                            '''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


                            '''''''Dim file As New System.IO.StreamWriter(fic, True)


                            '''''''tarjeta = String.Format("{0,-8}", Left(tarjeta.ToString, 8))
                            '''''''Dim trabajador As String = String.Format("{0,-6}", Left(CInt(existe(0)(0)).ToString, 6))
                            '''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
                            '''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
                            '''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
                            '''''''Dim DNI As String = String.Format("{0,-8}", Left(docu.NIF, 8))
                            '''''''Dim vacio As String = String.Format("{0,-6}", "")
                            '''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
                            '''''''Dim FechaBaja As String = String.Format("{0,-8}", fechaCadPed.ToString("dd/MM/yy"))
                            ''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
                            ''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
                            '''''''Dim centro As String = String.Format("{0,-1}", "1")




                            '''''''If Trim(tarjeta) <> "" Then
                            '''''''    file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
                            '''''''End If


                            ''''''''file.WriteLine("2;update fcpwtra" & "" & ";" & CInt(existe(0)(0)) & ";" & fechaCadPed.ToString("dd/MM/yyyy"))
                            '''''''file.Close()



                            'para el responsable pongo codpersona el resp (izaro)
                            Dim Rlista4 As List(Of ELL.Responsables)

                            Rlista4 = loadResponsablesNombreMail(responsable, 1)
                            If Rlista4.Count > 0 Then
                                If Rlista4(0).Id > 0 Then
                                    responsable = Rlista4(0).Id
                                End If
                            End If



                            'si activo = 1 entonces pongo fecha de hoy si 0 el que tuviera en registro
                            Dim lParameters5 As New List(Of OracleParameter)
                            Dim lParameters6 As New List(Of OracleParameter)
                            Dim lParameters4 As New List(Of OracleParameter)

                            Dim strSql As String = String.Empty


                            strSql = " update fcpwtra set tr230=:responsable, tr270=:FecFin  where tr180=:tdni "

                            lParameters5.Add(New OracleParameter("FecFin", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                            lParameters5.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(docu.NIF).ToUpper, ParameterDirection.Input))
                            lParameters5.Add(New OracleParameter("responsable", OracleDbType.Int32, responsable, ParameterDirection.Input))

                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters5.ToArray)


                            strSql = " update fpertic set ti140=:responsable         where ti010=:qTrabajador and ti000=3 "
                            Dim lParameters7 As New List(Of OracleParameter)
                            lParameters7.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, responsable, ParameterDirection.Input))
                            lParameters7.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))
                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters7.ToArray)


                            strSql = " update fpertif set  tf022=:FecFin         where tf010=:qTrabajador "

                            lParameters6.Add(New OracleParameter("FecFin", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                            lParameters6.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters6.ToArray)

                            strSql = " update fpertih set  th022=:FecFin        where th010=:qTrabajador " ' no hay por que acualizarlo , th390=:departamento

                            lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                            lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)

                        End If
                    End If

                    no_grabar = 0
                    ' insertaremos en  tra014 la solicitud

                    arrSolicitudes = Split(solicitudes, ";")
                    If arrSolicitudes.Length > 1 Then

                        For i = 0 To arrSolicitudes.Length - 1
                            If arrSolicitudes(i) = docu.coddoc.ToString Then
                                no_grabar = 1

                            End If
                        Next
                    End If

                    If no_grabar = 0 Then
                        solicitudes = solicitudes & ";" & docu.coddoc.ToString & ";" ' & docu.Comentario & ";" & fechaCadPed
                        query = "UPDATE adok_tra SET tra014=:solicitudes where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                        lParameters3.Add(New OracleParameter("solicitudes", OracleDbType.Varchar2, solicitudes, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)
                    End If


                End If



                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el trabajador", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateTraSolETT(ByVal docu As ELL.TrabajadoresDoc, ByVal chequeado As Int32, ByVal responsable As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim fechaCad As Date
            Dim fechaCadPed As Date
            Dim query As String = String.Empty
            Dim sql As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim lParameters3 As New List(Of OracleParameter)
            Dim resultados As List(Of ELL.EmpresasDoc)
            Dim solicitudes As String = String.Empty
            Dim arrSolicitudes As String() = Nothing
            Dim no_grabar As Integer
            Try
                If chequeado = 1 Then
                    query = "DELETE FROM adok_trs WHERE trs099 = " & docu.Planta & " and trs001 = " & docu.coddoc & " and trs000 = " & docu.codtra



                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)



                    'habra que insertar fecha caducidad al trabajador. Si ya tiene mayor por otro pedido no se pone.

                    sql = "select tra006, tra014 from adok_tra where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("tra014")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, sql, CadenaConexion)

                    fechaCad = resultados(0).FecCad
                    solicitudes = resultados(0).Comentario
                    sql = "Select sol004  FROM adok_solETT  WHERE sol099= " & docu.Planta & " AND sol000 = " & docu.coddoc

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.FecCad = SabLib.BLL.Utils.dateTimeNull(r("sol004"))}, sql, CadenaConexion)

                    If resultados.Count > 0 Then
                        fechaCadPed = resultados(0).FecCad
                    Else
                        fechaCadPed = Date.MinValue
                    End If

                    'no lo hago al quitar si al poner
                    'If fechaCad < fechaCadPed Then
                    '    query = "UPDATE adok_tra SET tra006=:fecha where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    '    lParameters2.Add(New OracleParameter("fecha", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))

                    '    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters2.ToArray)
                    'End If

                    '    no_grabar = 0

                    ' quitaremos en  tra014 la solicitud

                    arrSolicitudes = Split(solicitudes, ";")
                    If arrSolicitudes.Length > 1 Then

                        solicitudes = ""
                        For i = 0 To arrSolicitudes.Length - 1
                            If arrSolicitudes(i) = docu.coddoc.ToString Then
                            Else
                                solicitudes = solicitudes & arrSolicitudes(i) & ";"

                            End If
                        Next
                    End If

                    '      If no_grabar = 0 Then
                    '      solicitudes = solicitudes & ";" & docu.coddoc.ToString & ";" ' & docu.Comentario & ";" & fechaCadPed
                    query = "UPDATE adok_tra SET tra014=:solicitudes where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    lParameters3.Add(New OracleParameter("solicitudes", OracleDbType.Varchar2, solicitudes, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)
                    '     End If




                End If
                If chequeado = 0 Then
                    ' query = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd011) VALUES(:planta, :codtra, :coddoc,  1, :autor)"
                    query = "INSERT INTO adok_trs (trs099, trs000, trs001, trs002) VALUES(:planta, :codtra, :codsol, :codemp)"

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codsol", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 8, docu.clave, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                    'habra que insertar fecha caducidad al trabajador. Si ya tiene mayor por otro pedido no se pone.

                    sql = "select tra006, tra014 from adok_tra where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.Comentario = SabLib.BLL.Utils.stringNull(r("tra014")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("tra006"))}, sql, CadenaConexion)

                    fechaCad = resultados(0).FecCad
                    solicitudes = resultados(0).Comentario
                    sql = "Select sol004  FROM adok_solETT  WHERE sol099= " & docu.Planta & " AND sol000 = " & docu.coddoc

                    resultados = Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                             New ELL.EmpresasDoc With {.FecCad = SabLib.BLL.Utils.dateTimeNull(r("sol004"))}, sql, CadenaConexion)
                    If resultados.Count > 0 Then
                        fechaCadPed = resultados(0).FecCad
                    Else
                        fechaCadPed = Date.MinValue
                    End If


                    If fechaCad < fechaCadPed Then
                        query = "UPDATE adok_tra SET tra006=:fecha, tra008=:responsable where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                        lParameters2.Add(New OracleParameter("fecha", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("responsable", OracleDbType.Int32, responsable, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters2.ToArray)


                        'actualizar dorlet


                        Dim existe As List(Of String())
                        existe = loadIzaroTrabajador(docu.Planta, Trim(docu.NIF).ToUpper)
                        If existe.Count > 0 Then


                            'Con docu.codtra sacar nombre , 
                            Dim nombrea As String
                            Dim apellidos As String
                            Dim tarjeta As String


                            Dim lista As List(Of ELL.Trabajadores)
                            lista = loadListTrabajadoresClaveTra(docu.Planta, docu.codtra)
                            If lista.Count > 0 Then
                                nombrea = Split(lista(0).Nombre, ", ")(1)
                                apellidos = Split(lista(0).Nombre, ",")(0)
                            Else
                                nombrea = ""
                                apellidos = ""
                            End If
                            ' con CInt(existe(0)(0)) sacar tarjeta
                            Dim etarjeta As List(Of String())
                            etarjeta = loadIzaroTrabajador2(1, CInt(existe(0)(0))) 'si ese qtrabajador tiene tarjera en fcpwtar
                            If existe.Count > 0 Then
                                tarjeta = String.Format("{0,-8}", Left(etarjeta(0)(0), 8))
                            Else
                                tarjeta = ""
                            End If




                            ''''''''''''Dim fic As String
                            ''''''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


                            ''''''''''''Dim file As New System.IO.StreamWriter(fic, True)


                            ''''''''''''tarjeta = String.Format("{0,-8}", Left(tarjeta.ToString, 8))
                            ''''''''''''Dim trabajador As String = String.Format("{0,-6}", Left(CInt(existe(0)(0)).ToString, 6))
                            ''''''''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
                            ''''''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
                            ''''''''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
                            ''''''''''''Dim DNI As String = String.Format("{0,-8}", Left(docu.NIF, 8))
                            ''''''''''''Dim vacio As String = String.Format("{0,-6}", "")
                            ''''''''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
                            ''''''''''''Dim FechaBaja As String = String.Format("{0,-8}", fechaCadPed.ToString("dd/MM/yy"))
                            '''''''''''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
                            '''''''''''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
                            ''''''''''''Dim centro As String = String.Format("{0,-1}", "1")




                            ''''''''''''If Trim(tarjeta) <> "" Then
                            ''''''''''''    file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
                            ''''''''''''End If


                            ''''''''''''file.Close()



                            'para el responsable pongo codpersona el resp (izaro)
                            Dim Rlista4 As List(Of ELL.Responsables)

                            Rlista4 = loadResponsablesNombreMail(responsable, 1)
                            If Rlista4.Count > 0 Then
                                If Rlista4(0).Id > 0 Then
                                    responsable = Rlista4(0).Id
                                End If
                            End If
                            'si activo = 1 entonces pongo fecha de hoy si 0 el que tuviera en registro
                            Dim lParameters5 As New List(Of OracleParameter)
                            Dim lParameters6 As New List(Of OracleParameter)
                            Dim lParameters4 As New List(Of OracleParameter)

                            Dim strSql As String = String.Empty


                            strSql = " update fcpwtra set tr230=:responsable, tr270=:FecFin  where tr180=:tdni "

                            lParameters5.Add(New OracleParameter("FecFin", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                            lParameters5.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(docu.NIF).ToUpper, ParameterDirection.Input))
                            lParameters5.Add(New OracleParameter("responsable", OracleDbType.Int32, responsable, ParameterDirection.Input))

                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters5.ToArray)


                            strSql = " update fpertic set ti140=:responsable         where ti010=:qTrabajador and ti000=3 "
                            Dim lParameters7 As New List(Of OracleParameter)
                            lParameters7.Add(New OracleParameter("responsable", OracleDbType.Varchar2, 22, responsable, ParameterDirection.Input))
                            lParameters7.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))
                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters7.ToArray)


                            strSql = " update fpertif set  tf022=:FecFin         where tf010=:qTrabajador "

                            lParameters6.Add(New OracleParameter("FecFin", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                            lParameters6.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters6.ToArray)

                            strSql = " update fpertih set  th022=:FecFin        where th010=:qTrabajador " ' no hay por que acualizarlo , th390=:departamento

                            lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, fechaCadPed, ParameterDirection.Input))
                            lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                            Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)

                        End If
                    End If

                    no_grabar = 0
                    ' insertaremos en  tra014 la solicitud

                    arrSolicitudes = Split(solicitudes, ";")
                    If arrSolicitudes.Length > 1 Then

                        For i = 0 To arrSolicitudes.Length - 1
                            If arrSolicitudes(i) = docu.coddoc.ToString Then
                                no_grabar = 1

                            End If
                        Next
                    End If

                    If no_grabar = 0 Then
                        solicitudes = solicitudes & ";" & docu.coddoc.ToString & ";" ' & docu.Comentario & ";" & fechaCadPed
                        query = "UPDATE adok_tra SET tra014=:solicitudes where tra099= " & docu.Planta & " AND tra000 = " & docu.codtra

                        lParameters3.Add(New OracleParameter("solicitudes", OracleDbType.Varchar2, solicitudes, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)
                    End If


                End If



                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el trabajador", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateTraDocSol(ByVal docu As ELL.TrabajadoresDoc, ByVal chequeado As Int32, ByVal tipo As Int32) As Boolean
            Dim resultado As Boolean = False
            'Dim fechaCad As Date
            'Dim fechaCadPed As Date
            Dim query As String = String.Empty
            Dim sql As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim lParameters3 As New List(Of OracleParameter)
            'Dim resultados As List(Of ELL.EmpresasDoc)
            Dim solicitudes As String = String.Empty
            Dim arrSolicitudes As String() = Nothing
            'Dim no_grabar As Integer
            Try
                'docu.coddoc=codigo solicitud, tipo = coddoc
                If chequeado = 1 Then 'delete en adok_trd si concide con la solicitud trd010

                    query = "DELETE FROM adok_trd WHERE trd099 = " & docu.Planta & " and trd001 = " & tipo & " and trd000 = " & docu.codtra & " and trd010 = " & docu.coddoc



                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 0 Then  'insert en adok_trd, si no existe
                    query = "INSERT INTO adok_trd (trd099, trd000, trd001, trd002, trd007, trd010) VALUES(:planta, :codtra, :coddoc,  2, :periodicidad, :solicitud)"
                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 6, docu.codtra, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, tipo, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, 8, docu.periodicidad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("solicitud", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                End If



                Return True

            Catch ex As Exception
                Return True 'ya existe   Throw New SabLib.BatzException("Error al actualizar el trabajador", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateFS(ByVal planta As Integer, ByVal empresa As Integer, ByVal trabajador As Integer, ByVal fecha As Date, ByVal codigo As Integer) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                If codigo = 1 Then
                    query = "DELETE FROM adok_fs WHERE fs099 = " & planta & " and fs001 = " & empresa & " and fs003 =:fecha "
                    lParameters1.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If codigo = 0 Then

                    query = "INSERT INTO adok_fs (fs099, fs001, fs003) VALUES( " & planta & "," & empresa & ",:fecha" & ")"
                    lParameters1.Add(New OracleParameter("fecha", OracleDbType.Date, fecha, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                End If


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar los trabajadores en Festivos", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmpDoc(ByVal docu As ELL.EmpresasDoc, ByVal chequeado As Int32, ByVal nombre As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim lParameters3 As New List(Of OracleParameter)

            Try
                If chequeado = 1 Then
                    query = "DELETE FROM adok_emd WHERE emd099 = " & docu.Planta & " and emd001 = " & docu.coddoc & " and emd000 = " & docu.codemp



                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)




                    'quitar los trabajadores con ese id doc por ser certificado
                    lParameters3.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters3.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters3.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    query = "DELETE FROM adok_trd WHERE trd099 = " & docu.Planta & " and trd001 = " & docu.coddoc & " And trd000 in (select tra000 from adok_tra where tra004 = " & docu.codemp & ")"

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)

                End If
                If chequeado = 0 Then
                    query = "DELETE FROM adok_emd WHERE emd000 = " & docu.codemp & " and emd001 = " & docu.coddoc
                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)

                    ' query = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd011) VALUES(:planta, :codemp, :coddoc,  1, :autor)"

                    query = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd007, emd013) VALUES(:planta, :codemp, :coddoc,  2, :periodicidad, :tipodoc)"

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, 8, docu.periodicidad, ParameterDirection.Input))
                    If docu.tipodoc = 1 Then
                        lParameters1.Add(New OracleParameter("tipodoc", OracleDbType.Int32, 8, docu.tipodoc, ParameterDirection.Input))
                    Else
                        lParameters1.Add(New OracleParameter("tipodoc", OracleDbType.Int32, 8, 0, ParameterDirection.Input))
                    End If



                    'lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                End If
                If chequeado = 2 Then 'al subir

                    query = "UPDATE adok_emd SET  emd012='', emd011=:autor, emd005=:ubicacion, emd002 =:FecRec, emd003 = :estado,  emD009 = 4, emd004 =:FecCad, emd006 =:FecIni, emd007 =:periodo WHERE emd099= :planta and emd000=:codemp And emd001=:coddoc "


                    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 150, docu.ubicacion, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, Now, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodo", OracleDbType.Int32, 8, docu.periodicidad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, docu.estado, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 3 Then 'actualizamos resto de campos
                    query = "UPDATE adok_emd SET emd011=:autor,   emd002 =:FecRec , emd009 =:Impuestos , emd003 =:estado , emd007 =:periodicidad, emd004 =:FecCad, emd006 =:FecIni, emd012 =:Comentario   WHERE emd099= :planta and emd000=:codemp And emd001=:coddoc "


                    ' emd005=:ubicacion,    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 150, docu.ubicacion, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, docu.FecRec, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Impuestos", OracleDbType.Int32, docu.Impuestos, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, docu.estado, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Comentario", OracleDbType.Varchar2, 250, docu.Comentario, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el documento de empresa", ex)
            End Try
            Return resultado
        End Function
        Public Function UpdateEmpDocETT(ByVal docu As ELL.EmpresasDoc, ByVal chequeado As Int32, ByVal nombre As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim lParameters3 As New List(Of OracleParameter)

            Try
                If chequeado = 1 Then
                    query = "DELETE FROM adok_emdETT WHERE emd099 = " & docu.Planta & " and emd001 = " & docu.coddoc & " and emd000 = " & docu.codemp



                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                    'quitar certificado si lo tuviera
                    lParameters2.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters2.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters2.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    query = "DELETE FROM adok_cer WHERE cer099 = " & docu.Planta & " and cer000 = " & docu.coddoc & " and cer001 = " & docu.codemp

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters2.ToArray)


                    'quitar los trabajadores con ese id doc por ser certificado
                    lParameters3.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters3.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters3.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    query = "DELETE FROM adok_trd WHERE trd099 = " & docu.Planta & " and trd001 = " & docu.coddoc & " And trd000 in (select tra000 from adok_tra where tra004 = " & docu.codemp & ")"

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters3.ToArray)

                End If
                If chequeado = 0 Then
                    ' query = "INSERT INTO adok_emd (emd099, emd000, emd001, emd003, emd011) VALUES(:planta, :codemp, :coddoc,  1, :autor)"
                    query = "INSERT INTO adok_emdETT (emd099, emd000, emd001, emd003, emd007, emd013, emd015) VALUES(:planta, :codemp, :coddoc,  2, :periodicidad, :tipodoc, :solicitud)"

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, 8, docu.periodicidad, ParameterDirection.Input))
                    If docu.tipodoc = 1 Then
                        lParameters1.Add(New OracleParameter("tipodoc", OracleDbType.Int32, 8, 1, ParameterDirection.Input)) 'docu.tipodoc
                    Else
                        lParameters1.Add(New OracleParameter("tipodoc", OracleDbType.Int32, 8, 0, ParameterDirection.Input))
                    End If
                    lParameters1.Add(New OracleParameter("solicitud", OracleDbType.Int32, 8, docu.tipodoc, ParameterDirection.Input))


                    'lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                End If
                If chequeado = 2 Then 'al subir

                    query = "UPDATE adok_emdETT SET  emd012='', emd011=:autor, emd005=:ubicacion, emd002 =:FecRec, emd003 = :estado,  emD009 = 4, emd004 =:FecCad, emd006 =:FecIni, emd007 =:periodo WHERE emd099= :planta and emd000=:codemp And emd001=:coddoc "


                    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 150, docu.ubicacion, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, Now, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodo", OracleDbType.Int32, 8, docu.periodicidad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, docu.estado, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 3 Then 'actualizamos resto de campos
                    query = "UPDATE adok_emdETT SET emd011=:autor,   emd002 =:FecRec , emd009 =:Impuestos , emd003 =:estado , emd007 =:periodicidad, emd004 =:FecCad, emd006 =:FecIni, emd012 =:Comentario   WHERE emd099= :planta and emd000=:codemp And emd001=:coddoc "


                    ' emd005=:ubicacion,    lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, 150, docu.ubicacion, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecRec", OracleDbType.Date, docu.FecRec, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Impuestos", OracleDbType.Int32, docu.Impuestos, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("estado", OracleDbType.Int32, docu.estado, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecCad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("FecIni", OracleDbType.Date, docu.FecIni, ParameterDirection.Input))

                    lParameters1.Add(New OracleParameter("autor", OracleDbType.Varchar2, 150, nombre, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("Comentario", OracleDbType.Varchar2, 250, docu.Comentario, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el documento de empresa", ex)
            End Try
            Return resultado
        End Function
        Public Function UpdateSolDoc(ByVal docu As ELL.EmpresasDoc, ByVal chequeado As Int32, ByVal nombre As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                If chequeado = 1 Then
                    query = "DELETE FROM adok_dos WHERE dos099 = " & docu.Planta & " and dos001 = " & docu.coddoc & " and dos000 = " & docu.codemp


                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 0 Then

                    query = "INSERT INTO adok_dos (dos099, dos000, dos001) VALUES(:planta, :codemp, :coddoc)"

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                End If

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el documento de solución", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateSolDocETT(ByVal docu As ELL.EmpresasDoc, ByVal chequeado As Int32, ByVal nombre As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                If chequeado = 1 Then
                    query = "DELETE FROM adok_dosETT WHERE dos099 = " & docu.Planta & " and dos001 = " & docu.coddoc & " and dos000 = " & docu.codemp


                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                End If
                If chequeado = 0 Then

                    query = "INSERT INTO adok_dosETT (dos099, dos000, dos001) VALUES(:planta, :codemp, :coddoc)"

                    lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 6, docu.codemp, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)
                End If

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el documento de solución", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmd(ByVal docu As ELL.EmpresasDoc) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim query2 As String = String.Empty
            Dim lParameters2 As New List(Of OracleParameter)

            Try

                'actualizamos campo emd007 por haber cambiado el adok_doc
                query = "UPDATE adok_emd SET  emd007 =:periodicidad   WHERE emd099= :planta and emd001=:coddoc "

                lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                'actualizamos campo TRD007 por haber cambiado el adok_doc
                query2 = "UPDATE adok_trd SET  trd007 =:periodicidad   WHERE trd099= :planta and trd001=:coddoc "

                lParameters2.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))

                lParameters2.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query2, CadenaConexion, lParameters2.ToArray)



                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar el documento", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateEmdEmp(ByVal docu As ELL.EmpresasDoc) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim query2 As String = String.Empty
            Dim lParameters2 As New List(Of OracleParameter)

            Try

                'actualizamos campo emd007 por caducidad de empresa particular
                query = "UPDATE adok_emd SET  emd007 =:periodicidad, EMD004=:feccad   WHERE emd099= :planta and emd001=:coddoc and emd000=:codemp"

                lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("feccad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codemp", OracleDbType.Int32, 8, docu.codemp, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la caducidad el documento", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateTrdTra(ByVal docu As ELL.TrabajadoresDoc) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo emd007 por caducidad de empresa particular
                query = "UPDATE adok_trd SET  trd007 =:periodicidad, trd005=:feccad   WHERE trd099= :planta and trd001=:coddoc and trd000=:codtra"

                lParameters1.Add(New OracleParameter("periodicidad", OracleDbType.Int32, docu.periodicidad, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("feccad", OracleDbType.Date, docu.FecCad, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("coddoc", OracleDbType.Int32, 8, docu.coddoc, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, 8, docu.codtra, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la caducidad el documento", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateTrd(ByVal codtra As Integer) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            query = "UPDATE adok_trd SET trd009 = 1 WHERE trd000= :codtra "

            lParameters1.Add(New OracleParameter("codtra", OracleDbType.Int32, codtra, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

        End Function

        Public Function UpdateTra(ByVal docu As ELL.Trabajadores) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_tra SET tra012 =:activo WHERE tra099= :planta and tra000=:id"

                lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)



                'izaro y SAB

                Dim existe As List(Of String())
                existe = loadIzaroTrabajador(docu.Planta, Trim(docu.tDNI).ToUpper)
                If existe.Count > 0 Then
                    'actualizar Dorlet
                    'si activo = 1 entonces pongo fecha de hoy si 0 el que tuviera en registro
                    Dim lParameters5 As New List(Of OracleParameter)
                    Dim lParameters2 As New List(Of OracleParameter)
                    Dim lParameters3 As New List(Of OracleParameter)
                    Dim lParameters4 As New List(Of OracleParameter)

                    Dim strSql As String = String.Empty

                    If docu.activo = 0 Then 'activo, metemos la fecha fin, si inactivo sysdate



                        'Con docu.id sacar nombre , 
                        Dim nombrea As String
                        Dim apellidos As String
                        Dim tarjeta As String


                        Dim lista As List(Of ELL.Trabajadores)
                        lista = loadListTrabajadoresClaveTra(docu.Planta, docu.Id)
                        If lista.Count > 0 Then
                            nombrea = Split(lista(0).Nombre, ", ")(1)
                            apellidos = Split(lista(0).Nombre, ",")(0)
                        Else
                            nombrea = ""
                            apellidos = ""
                        End If
                        ' con CInt(existe(0)(0)) sacar tarjeta
                        tarjeta = ""
                        If existe.Count > 0 Then

                            Dim etarjeta As List(Of String())
                            etarjeta = loadIzaroTrabajador2(1, CInt(existe(0)(0))) 'si ese qtrabajador tiene tarjera en fcpwtar
                            If etarjeta.Count > 0 Then
                                tarjeta = String.Format("{0,-8}", Left(etarjeta(0)(0), 8))

                            End If
                        End If




                        '''''''Dim fic As String
                        '''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


                        '''''''Dim file As New System.IO.StreamWriter(fic, True)


                        '''''''tarjeta = String.Format("{0,-8}", Left(tarjeta.ToString, 8))
                        '''''''Dim trabajador As String = String.Format("{0,-6}", Left(CInt(existe(0)(0)).ToString, 6))
                        '''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
                        '''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
                        '''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
                        '''''''Dim DNI As String = String.Format("{0,-8}", Left(docu.tDNI, 8))
                        '''''''Dim vacio As String = String.Format("{0,-6}", "")
                        '''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
                        '''''''Dim FechaBaja As String = String.Format("{0,-8}", docu.FecFin.ToString("dd/MM/yy"))
                        ''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
                        ''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
                        '''''''Dim centro As String = String.Format("{0,-1}", "1")



                        '''''''If Trim(tarjeta) <> "" Then
                        '''''''    file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
                        '''''''End If
                        '''''''file.Close()





                        strSql = " update fcpwtra set tr270=:FecFin  where tr180=:tdni "

                        lParameters5.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                        lParameters5.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(docu.tDNI).ToUpper, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters5.ToArray)





                        strSql = " update fpertif set  tf022=:FecFin         where tf010=:qTrabajador "

                        lParameters3.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                        lParameters3.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                        Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters3.ToArray)

                        strSql = " update fpertih set  th022=:FecFin        where th010=:qTrabajador " ' no hay por que acualizarlo , th390=:departamento

                        lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                        lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                        Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)


                    Else

                        '!!!!!!!!!!!!!!!!!!!!!activar no se hace con el boton sino modificando fechas

                        '''''''''Con docu.codtra sacar nombre , 
                        ''''''''Dim nombrea As String
                        ''''''''Dim apellidos As String
                        ''''''''Dim tarjeta As String


                        ''''''''Dim lista As List(Of ELL.Trabajadores)
                        ''''''''lista = loadListTrabajadoresClaveTra(docu.Planta, docu.Id)
                        ''''''''If lista.Count > 0 Then
                        ''''''''    nombrea = Split(lista(0).Nombre, ", ")(1)
                        ''''''''    apellidos = Split(lista(0).Nombre, ",")(0)
                        ''''''''Else
                        ''''''''    nombrea = ""
                        ''''''''    apellidos = ""
                        ''''''''End If
                        ''''''''' con CInt(existe(0)(0)) sacar tarjeta
                        ''''''''tarjeta = ""
                        ''''''''If existe.Count > 0 Then

                        ''''''''    Dim etarjeta As List(Of String())
                        ''''''''    etarjeta = loadIzaroTrabajador2(1, CInt(existe(0)(0))) 'si ese qtrabajador tiene tarjera en fcpwtar
                        ''''''''    If etarjeta.Count > 0 Then
                        ''''''''        tarjeta = String.Format("{0,-8}", Left(etarjeta(0)(0), 8))

                        ''''''''    End If
                        ''''''''End If



                        ''''''''Dim fic As String
                        ''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


                        ''''''''Dim file As New System.IO.StreamWriter(fic, True)


                        ''''''''tarjeta = String.Format("{0,-8}", Left(tarjeta.ToString, 8))
                        ''''''''Dim trabajador As String = String.Format("{0,-6}", Left(CInt(existe(0)(0)).ToString, 6))
                        ''''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
                        ''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
                        ''''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
                        ''''''''Dim DNI As String = String.Format("{0,-8}", Left(docu.tDNI, 8))
                        ''''''''Dim vacio As String = String.Format("{0,-6}", "")
                        ''''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
                        ''''''''Dim FechaBaja As String = String.Format("{0,-8}", docu.FecFin.ToString("dd/MM/yy"))
                        '''''''''Dim Ruta As String = String.Format("{0,-2}", "")
                        '''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
                        ''''''''Dim centro As String = String.Format("{0,-1}", "1")



                        ''''''''If Trim(tarjeta) <> "" Then
                        ''''''''    file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
                        ''''''''End If

                        ''''''''file.Close()




                        ''''''''strSql = " update fcpwtra set tr270=:FecFin  where tr180=:tdni "

                        ''''''''lParameters5.Add(New OracleParameter("FecFin", OracleDbType.Date, Now, ParameterDirection.Input))
                        ''''''''lParameters5.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(docu.tDNI).ToUpper, ParameterDirection.Input))

                        ''''''''Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters5.ToArray)





                        ''''''''strSql = " update fpertif set  tf022=:FecFin         where tf010=:qTrabajador "

                        ''''''''lParameters3.Add(New OracleParameter("FecFin", OracleDbType.Date, Now, ParameterDirection.Input))
                        ''''''''lParameters3.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                        ''''''''Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters3.ToArray)

                        ''''''''strSql = " update fpertih set  th022=:FecFin        where th010=:qTrabajador " ' no hay por que acualizarlo , th390=:departamento

                        ''''''''lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, Now, ParameterDirection.Input))
                        ''''''''lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                        ''''''''Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)



                        '''''''''nuevo, hay que quitar el reg de fcpwtar

                        ''''''''query = "DELETE FROM fcpwtar WHERE ta020 = " & CInt(existe(0)(0))


                        ''''''''Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionIZARO)






                        '''''''''hay que modificar en SAB ademas de en Izaro
                        '''''''''falta meter en SAB USUARIOS fechas fin. 

                        ''''''''query = "UPDATE USUARIOS SET fechabaja=:FecFin WHERE CODPERSONA=:qTrabajador "
                        ''''''''Dim lParameters6 As New List(Of OracleParameter)

                        ''''''''lParameters6.Add(New OracleParameter("FecFin", OracleDbType.Date, Now, ParameterDirection.Input))
                        ''''''''lParameters6.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))

                        ''''''''Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionSAB, lParameters6.ToArray)


                        '''''''''tambien fecha fin en adok
                        ''''''''query = "UPDATE adok_tra SET tra006 =:FecFin WHERE tra099= :planta and tra000=:id"
                        ''''''''Dim lParameters7 As New List(Of OracleParameter)
                        ''''''''lParameters7.Add(New OracleParameter("FecFin", OracleDbType.Date, Now, ParameterDirection.Input))

                        ''''''''lParameters7.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                        ''''''''lParameters7.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                        ''''''''Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters7.ToArray)




                    End If

                End If


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la actividad del trabajador", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateTraFecha(ByVal docu As ELL.Trabajadores) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_tra SET tra006 =:feccad WHERE tra099= :planta and tra000=:id"

                lParameters1.Add(New OracleParameter("feccad", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)


                'actualizar dorlet
                Dim existe As List(Of String())
                existe = loadIzaroTrabajador(docu.Planta, Trim(docu.tDNI).ToUpper)
                If existe.Count > 0 Then



                    'Con docu.id sacar nombre , 
                    Dim nombrea As String
                    Dim apellidos As String
                    Dim tarjeta As String


                    Dim lista As List(Of ELL.Trabajadores)
                    lista = loadListTrabajadoresClaveTra(docu.Planta, docu.Id)
                    If lista.Count > 0 Then
                        nombrea = Split(lista(0).Nombre, ", ")(1)
                        apellidos = Split(lista(0).Nombre, ",")(0)
                    Else
                        nombrea = ""
                        apellidos = ""
                    End If
                    ' con CInt(existe(0)(0)) sacar tarjeta
                    Dim etarjeta As List(Of String())
                    etarjeta = loadIzaroTrabajador2(1, CInt(existe(0)(0))) 'si ese qtrabajador tiene tarjera en fcpwtar
                    If existe.Count > 0 Then
                        tarjeta = String.Format("{0,-8}", Left(etarjeta(0)(0), 8))
                    Else
                        tarjeta = ""
                    End If




                    ''''''''''''Dim fic As String
                    ''''''''''''fic = System.Configuration.ConfigurationManager.AppSettings("PathFicherosTarjeta").ToString() & "adoknet02.dat"


                    ''''''''''''Dim file As New System.IO.StreamWriter(fic, True)


                    ''''''''''''tarjeta = String.Format("{0,-8}", Left(tarjeta.ToString, 8))
                    ''''''''''''Dim trabajador As String = String.Format("{0,-6}", Left(CInt(existe(0)(0)).ToString, 6))
                    ''''''''''''Dim Apellido1 As String = String.Format("{0,-25}", Left(apellidos, 25))
                    ''''''''''''Dim Apellido2 As String = String.Format("{0,-25}", "")
                    ''''''''''''Dim Nombre As String = String.Format("{0,-20}", Left(nombrea, 20))
                    ''''''''''''Dim DNI As String = String.Format("{0,-8}", Left(docu.tDNI, 8))
                    ''''''''''''Dim vacio As String = String.Format("{0,-6}", "")
                    ''''''''''''Dim FechaAlta As String = String.Format("{0,-8}", "01/01/17")
                    ''''''''''''Dim FechaBaja As String = String.Format("{0,-8}", docu.FecFin.ToString("dd/MM/yy"))
                    ''''''''''''''''''''''Dim Ruta As String = String.Format("{0,-2}", "")
                    ''''''''''''''''''''''Dim ZonaHoraria As String = String.Format("{0,-2}", "")
                    ''''''''''''Dim centro As String = String.Format("{0,-1}", "1")
                    ''''''''''''If Trim(tarjeta) <> "" Then
                    ''''''''''''    file.WriteLine("M" & tarjeta & trabajador & Apellido1 & Apellido2 & Nombre & DNI & vacio & FechaAlta & FechaBaja & centro)
                    ''''''''''''End If


                    ''''''''''''file.Close()





                    'actualizar Dorlet
                    'si activo = 1 entonces pongo fecha de hoy si 0 el que tuviera en registro
                    Dim lParameters5 As New List(Of OracleParameter)
                    Dim lParameters6 As New List(Of OracleParameter)
                    Dim lParameters4 As New List(Of OracleParameter)

                    Dim strSql As String = String.Empty


                    strSql = " update fcpwtra set tr270=:FecFin  where tr180=:tdni "

                    lParameters5.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    lParameters5.Add(New OracleParameter("tDNI", OracleDbType.Varchar2, 12, Trim(docu.tDNI).ToUpper, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters5.ToArray)





                    strSql = " update fpertif set  tf022=:FecFin         where tf010=:qTrabajador "

                    lParameters6.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    lParameters6.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                    Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters6.ToArray)

                    strSql = " update fpertih set  th022=:FecFin        where th010=:qTrabajador " ' no hay por que acualizarlo , th390=:departamento

                    lParameters4.Add(New OracleParameter("FecFin", OracleDbType.Date, docu.FecFin, ParameterDirection.Input))
                    lParameters4.Add(New OracleParameter("qTrabajador", OracleDbType.Int32, CInt(existe(0)(0)), ParameterDirection.Input))


                    Memcached.OracleDirectAccess.NoQuery(strSql, CadenaConexionIZARO, lParameters4.ToArray)

                End If
                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la fecha del trabajador", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateTraCaducados() As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "UPDATE adok_tra SET tra012 = 1 WHERE tra012 = 0 and tra006 < :feccad "

                lParameters1.Add(New OracleParameter("feccad", OracleDbType.Date, Now, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la fecha del trabajador", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmp(ByVal docu As ELL.Trabajadores) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_emp SET emp018 =:activo WHERE emp099= :planta and emp000=:id"

                lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la actividad de la empresa", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateSolDoc(ByVal docu As ELL.Trabajadores) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_doc SET doc015 =:activo WHERE doc099= :planta and doc000=:id"

                lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la actividad de la empresa", ex)
            End Try
            Return resultado
        End Function
        'Public Function UpdateSolDocETT(ByVal docu As ELL.Trabajadores) As Boolean
        '    Dim resultado As Boolean = False
        '    Dim query As String = String.Empty
        '    Dim lParameters1 As New List(Of OracleParameter)

        '    Try

        '        'actualizamos campo tra012 por desactivar = 1
        '        query = "UPDATE adok_docETT SET doc015 =:activo WHERE doc099= :planta and doc000=:id"

        '        lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

        '        lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
        '        lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

        '        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

        '        Return True

        '    Catch ex As Exception
        '        Throw New SabLib.BatzException("Error al actualizar la actividad de la empresa", ex)
        '    End Try
        '    Return resultado
        'End Function
        Public Function UpdateDOS(ByVal docu As ELL.Trabajadores) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_emd_hist SET emd005 = '' WHERE emd099= :planta and emd008=:id"

                'lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar el fichero", ex)
            End Try
            Return resultado
        End Function
        Public Function UpdateDOSTra(ByVal docu As ELL.Trabajadores) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_trd_hist SET trd006 = '' WHERE trd006=:ubicacion"

                'lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("ubicacion", OracleDbType.Varchar2, docu.Nombre, ParameterDirection.Input))
                'lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar el fichero", ex)
            End Try
            Return resultado
        End Function
        Public Function UpdateSol(ByVal docu As ELL.Solicitudes) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_sol SET sol012 =:activo WHERE sol099= :planta and sol000=:id"

                lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la actividad de la solicitud", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateSolETT(ByVal docu As ELL.Solicitudes) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE adok_solETT SET sol012 =:activo WHERE sol099= :planta and sol000=:id"

                lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la actividad de la solicitud", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateDocAct(ByVal docu As ELL.Documentos) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo doc011 por desactivar = 1
                query = "UPDATE adok_doc SET doc011 =:activo WHERE doc099= :planta and doc000=:id"

                lParameters1.Add(New OracleParameter("activo", OracleDbType.Int32, docu.activo, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, 8, docu.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al actualizar la actividad del documento", ex)
            End Try
            Return resultado
        End Function

        Public Function LeerTraDoc(ByVal docu As ELL.TrabajadoresDoc) As List(Of ELL.TrabajadoresDoc)

            Try

                Dim query As String = "SELECT trd001, trd002, trd003, trd004, trd005, trd006, trd007, trd009, trd011, trd012 FROM adok_trd  WHERE trd001=" & docu.coddoc & "   and trd000=" & docu.codtra


                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("trd001")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007")), .Aptitud = SabLib.BLL.Utils.integerNull(r("trd009")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .Comentario = SabLib.BLL.Utils.stringNull(r("trd012"))}, query, CadenaConexion)



            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de trabajador", ex)
            End Try
            '   Return resultado
        End Function

        Public Function LeerEmpDoc(ByVal docu As ELL.EmpresasDoc) As List(Of ELL.EmpresasDoc)

            Try

                Dim query As String = "SELECT emd001, emd002, emd003, emd004, emd005, emd006, emd007, emd009, emd011, emd012 FROM adok_emd  WHERE emd001=" & docu.coddoc & " and emd000=" & docu.codemp & " And emd099= " & docu.Planta


                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                                 New ELL.EmpresasDoc With {.coddoc = SabLib.BLL.Utils.integerNull(r("emd001")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007")), .Impuestos = SabLib.BLL.Utils.integerNull(r("emd009")), .Comentario = SabLib.BLL.Utils.stringNull(r("emd012"))}, query, CadenaConexion)



            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de empresa", ex)
            End Try
            '   Return resultado
        End Function


        Public Function LeerEmpDocHis(ByVal docu As ELL.EmpresasDoc) As List(Of ELL.EmpresasDoc)

            Try

                'Dim query As String = "SELECT distinct  emd005, emd001, emd002, emd003, emd004, emd006, emd007, EMD008, emd011 FROM adok_emd_hist  WHERE emd001=" & docu.coddoc & " and emd000=" & docu.codemp & " And emd099= " & docu.Planta & " and emd005 IS NOT NULL ORDER BY EMD008 DESC"

                'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                '                 New ELL.EmpresasDoc With {.clave = CInt(r("EMD008")), .coddoc = SabLib.BLL.Utils.integerNull(r("emd001")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007"))}, query, CadenaConexion)

                'si necesito mas descomentar lo de arriba y mirar lo de disctint
                '   Dim query As String = "SELECT distinct  emd005 FROM adok_emd_hist  WHERE emd001=" & docu.coddoc & " and emd000=" & docu.codemp & " And emd099= " & docu.Planta & " and emd005 IS NOT NULL ORDER BY EMD008 DESC"


                Dim query As String = " Select  emd005, max(EMD008) as maxEMD008 FROM adok_emd_hist  WHERE emd001=" & docu.coddoc & " and emd000=" & docu.codemp & " And emd099= " & docu.Planta & " and emd005 IS NOT NULL GROUP BY emd005 order by emd005 desc"
                'Dim query As String = " Select  emd005, EMD008 as maxEMD008 FROM adok_emd_hist  WHERE emd001=" & docu.coddoc & " and emd000=" & docu.codemp & " And emd099= " & docu.Planta & " and emd005 IS NOT NULL order by emd005 desc"
                '.FecRec = SabLib.BLL.Utils.dateTimeNull(r("EMD002")), Dim query As String = " Select  EMD002, emd005, EMD008 FROM adok_emd_hist  WHERE emd001=" & docu.coddoc & " and emd000=" & docu.codemp & " And emd099= " & docu.Planta & " and emd005 IS NOT NULL order by emd002"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                                 New ELL.EmpresasDoc With {.clave = CInt(r("maxEMD008")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005"))}, query, CadenaConexion)


            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de empresa", ex)
            End Try
            '   Return resultado
        End Function

        Public Function LeerTraDocHisTodos(ByVal docu As ELL.TrabajadoresDoc, ByVal todos As Integer) As List(Of ELL.TrabajadoresDoc)

            Try
                '    Dim query As String = " Select trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE trd002 = 2 and trd001=" & docu.Id & " And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"
                'trd002 <> 2 and 
                Dim query As String = " Select  trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE trd001=" & docu.coddoc & "  and trd000=" & docu.codtra & " And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"
                '    query = "Select  trd006, trD008 as maxtrD008 FROM adok_trd_hist  WHERE trd001=" & docu.coddoc & "  and trd000=" & docu.codtra & " And trd099= " & docu.Planta & " and trd006 IS NOT NULL"
                If todos = 2 Then
                    Dim inicad As Date
                    ' Dim fincad As Date
                    inicad = Now
                    inicad = DateAdd(DateInterval.Month, 3, inicad)
                    '        inicad = DateAdd(DateInterval.Year, 1, inicad)

                    Dim lParameters3 As New List(Of OracleParameter)
                    lParameters3.Add(New OracleParameter("FecCad", OracleDbType.Date, inicad, ParameterDirection.Input))
                    'lParameters3.Add(New OracleParameter("FecCad", OracleDbType.Date, annocad, ParameterDirection.Input))

                    '     query = " Select trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE trd002 = 2 and trd001=" & docu.Id & " and trd005 < :FecCad and trd005 > sysdate  And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"
                    'trd002 <> 2 and 
                    query = " Select  trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE trd001=" & docu.coddoc & " and trd005 < :FecCad and trd005 > sysdate  and trd000=" & docu.codtra & " And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"
                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDoc With {.clave = CInt(r("maxtrD008")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006"))}, query, CadenaConexion, lParameters3.ToArray)


                Else
                    If todos = 1 Then
                        '           query = " Select trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE trd002 = 2 and trd001=" & docu.Id & " and trd005 < sysdate  And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"
                        'trd002 <> 2 and
                        query = " Select  trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE  trd001=" & docu.coddoc & " and trd005 < sysdate  and trd000=" & docu.codtra & " And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"
                    End If

                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDoc With {.clave = CInt(r("maxtrD008")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006"))}, query, CadenaConexion)

                End If



            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de trabajador", ex)
            End Try
            '   Return resultado
        End Function


        Public Function LeerTraDocHis(ByVal docu As ELL.TrabajadoresDoc) As List(Of ELL.TrabajadoresDoc)

            Try
                'trd002 <> 2 and
                Dim query As String = " Select  trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE  trd001=" & docu.coddoc & "  and trd000=" & docu.codtra & " And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDoc With {.clave = CInt(r("maxtrD008")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006"))}, query, CadenaConexion)




            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de trabajador", ex)
            End Try
            '   Return resultado
        End Function

        Public Function LeerTraDocHisCurso(ByVal docu As ELL.Trabajadores, ByVal todos As Integer) As List(Of ELL.TrabajadoresDocMatriz)

            Try

                'seleccionar caducados o no
                '  " Select trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE  trd001=" & docu.Id & " And trd099= " & docu.Planta & " And trd006 Is Not NULL  GROUP BY trd006  order by trd006 desc"
                Dim query As String = " Select trd006, trD008 as maxtrD008 FROM adok_trd_hist  WHERE  trd001=" & docu.Id & " And trd099= " & docu.Planta & " And trd006 Is Not NULL"
                query = " Select trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE  trd001=" & docu.Id & " And trd099= " & docu.Planta & " And trd006 Is Not NULL  GROUP BY trd006  order by trd006 desc"


                If todos = 2 Then
                    Dim inicad As Date
                    ' Dim fincad As Date
                    inicad = Now
                    inicad = DateAdd(DateInterval.Month, 3, inicad)
                    '        inicad = DateAdd(DateInterval.Year, 1, inicad)

                    Dim lParameters3 As New List(Of OracleParameter)
                    lParameters3.Add(New OracleParameter("FecCad", OracleDbType.Date, inicad, ParameterDirection.Input))
                    'lParameters3.Add(New OracleParameter("FecCad", OracleDbType.Date, annocad, ParameterDirection.Input))
                    'trd002 <> 2 and
                    query = " Select trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE  trd001=" & docu.Id & " And trd005 < :FecCad And trd005 > sysdate  And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"

                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDocMatriz)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDocMatriz With {.clave = CInt(r("maxtrD008")), .Nombre = SabLib.BLL.Utils.stringNull(r("trd006")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006"))}, query, CadenaConexion, lParameters3.ToArray)

                Else
                    If todos = 1 Then
                        'trd002 <> 2 and 
                        query = " Select trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE trd001=" & docu.Id & " and trd005 < sysdate  And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"
                    End If

                    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDocMatriz)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDocMatriz With {.clave = CInt(r("maxtrD008")), .Nombre = SabLib.BLL.Utils.stringNull(r("trd006")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006"))}, query, CadenaConexion)

                End If





            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de trabajador", ex)
            End Try
            '   Return resultado
        End Function
        Public Function LeerTraDocHis2(ByVal docu As ELL.TrabajadoresDocMatriz) As List(Of ELL.TrabajadoresDocMatriz)

            Try


                Dim query As String = " Select  trd006, MAX(trD008) as maxtrD008 FROM adok_trd_hist  WHERE  trd000=" & docu.codtra & " And trd099= " & docu.Planta & " and trd006 IS NOT NULL  GROUP BY trd006  order by trd006 desc"


                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDocMatriz)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDocMatriz With {.codtra = CInt(r("maxtrD008")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006"))}, query, CadenaConexion)


            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de trabajador", ex)
            End Try
            '   Return resultado
        End Function

        Public Function LeerEmpDocHisClave(ByVal docu As ELL.EmpresasDoc) As List(Of ELL.EmpresasDoc)

            Try

                Dim query As String = "SELECT distinct  emd005, emd001, emd002, emd003, emd004, emd006, emd007, EMD008, emd011 FROM adok_emd_hist  WHERE emd008=" & docu.clave & " and emd005 IS NOT NULL ORDER BY EMD008 DESC"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EmpresasDoc)(Function(r As OracleDataReader) _
                                 New ELL.EmpresasDoc With {.clave = CInt(r("EMD008")), .coddoc = SabLib.BLL.Utils.integerNull(r("emd001")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("emd002")), .correcto = SabLib.BLL.Utils.integerNull(r("emd003")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("emd004")), .ubicacion = SabLib.BLL.Utils.stringNull(r("emd005")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("emd011")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("emd006")), .periodicidad = SabLib.BLL.Utils.integerNull(r("emd007"))}, query, CadenaConexion)



            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de empresa", ex)
            End Try
            '   Return resultado
        End Function

        Public Function LeerTraDocHisClave(ByVal docu As ELL.TrabajadoresDoc) As List(Of ELL.TrabajadoresDoc)

            Try
                'mirarlo
                Dim query As String = "SELECT distinct  trd005, trd001, trd002, trd003, trd004, trd006, trd007, trD008, trd011 FROM adok_trd_hist  WHERE trd008=" & docu.clave & "   and trd006 IS NOT NULL ORDER BY trD008 DESC"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.TrabajadoresDoc)(Function(r As OracleDataReader) _
                                 New ELL.TrabajadoresDoc With {.clave = CInt(r("trD008")), .coddoc = SabLib.BLL.Utils.integerNull(r("trd001")), .FecRec = SabLib.BLL.Utils.dateTimeNull(r("trd003")), .correcto = SabLib.BLL.Utils.integerNull(r("trd002")), .FecCad = SabLib.BLL.Utils.dateTimeNull(r("trd005")), .ubicacion = SabLib.BLL.Utils.stringNull(r("trd006")), .ubicacionfisica = SabLib.BLL.Utils.stringNull(r("trd011")), .FecIni = SabLib.BLL.Utils.dateTimeNull(r("trd004")), .periodicidad = SabLib.BLL.Utils.integerNull(r("trd007"))}, query, CadenaConexion)



            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento de trabajador", ex)
            End Try
            '   Return resultado
        End Function


        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existe(ByVal ID As String, ByVal rol As Integer, ByVal planta As Integer) As Integer

            Dim parameter As New OracleParameter("id", OracleDbType.Int32, ID, ParameterDirection.Input)
            'Dim parameter As New OracleParameter("id", OracleDbType.Int32, ID, ParameterDirection.Input)
            'Dim parameter As New OracleParameter("id", OracleDbType.Int32, ID, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM ADOK_ROLES WHERE rol000=:id and rol001 = " & rol & " and rol099" & planta
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Public Function existeETT(ByVal ID As String) As Integer

            Dim parameter As New OracleParameter("id", OracleDbType.Int32, ID, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM ADOK_ROLESETT WHERE rol000=:id"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function




#End Region

    End Class

End Namespace