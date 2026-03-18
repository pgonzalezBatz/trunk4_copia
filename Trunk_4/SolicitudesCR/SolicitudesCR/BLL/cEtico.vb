Imports System.Configuration
Imports Oracle.DataAccess.Client
Imports Oracle.ManagedDataAccess.Client

Namespace BLL
    Public Class cEtico


        Private ReadOnly Property ConnectionCEtico As String
            Get
                Dim con As String = If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "solicitudescrTEST", "solicitudescrLIVE")
                Return ConfigurationManager.ConnectionStrings(con).ConnectionString
            End Get
        End Property
        Private ReadOnly Property CadenaConexionSAB As String
            Get
                Dim con As String = If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "debug", "SABTEST", "SABLIVE")
                Return ConfigurationManager.ConnectionStrings(con).ConnectionString
            End Get
        End Property

        Public Function GuardarSolicitud(ByVal Solicitud As ELL.CEtico) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)
            'Solicitud.codCategoria es la url fichero
            Try
                query = "INSERT INTO CE_SOL (sol001,sol005,sol007,sol008, sol002, sol003, sol004, sol098, sol099) VALUES(:IdEmpleado,0,99,99, :Fecha,:codCategoria, :comentario, :plantaDesc,:planta)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Solicitud.planta, ParameterDirection.Input))
                'lParameters1.Add(New OracleParameter("tramite", OracleDbType.Int32, 0, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("comentario", OracleDbType.Varchar2, Solicitud.comentario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("IdEmpleado", OracleDbType.Varchar2, Solicitud.Idtra, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("plantaDesc", OracleDbType.Varchar2, 1500, Solicitud.plantaDesc, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codCategoria", OracleDbType.Varchar2, 1500, Solicitud.codCategoria, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Fecha", OracleDbType.Date, Solicitud.Fecha, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico(), lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar una Solicitud", ex)
            End Try
        End Function



        Public Function ModificarSolicitudHist(ByVal Solicitud As ELL.CEtico) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO CE_SOL_HIST (sol006, sol000, sol001,sol005,sol007,sol008, sol002, sol003, sol004, sol098, sol099) VALUES(:traduccion, :id, :IdEmpleado,:cierre,:accion,:bajas, :Fecha,:codCategoria, :comentario, :plantaDesc,:planta)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, Solicitud.planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("id", OracleDbType.Int32, Solicitud.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("cierre", OracleDbType.Int32, Solicitud.cierre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("accion", OracleDbType.Int32, Solicitud.Accion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("bajas", OracleDbType.Int32, Solicitud.bajas, ParameterDirection.Input))
                'lParameters1.Add(New OracleParameter("tramite", OracleDbType.Int32, 0, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("comentario", OracleDbType.Varchar2, Solicitud.comentario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("traduccion", OracleDbType.Varchar2, Solicitud.traduccion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("IdEmpleado", OracleDbType.Varchar2, Solicitud.Idtra, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("plantaDesc", OracleDbType.Varchar2, 1500, Solicitud.plantaDesc, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codCategoria", OracleDbType.Varchar2, 1500, Solicitud.codCategoria, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("Fecha", OracleDbType.Date, Solicitud.Fecha, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico(), lParameters1.ToArray)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar una Solicitud", ex)
            End Try
        End Function


        Public Function ModificarSolicitud(ByVal Solicitud As ELL.CEtico) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "update CE_SOL set sol005 =:codCategoria, sol007 =:traduccion,  sol008 = :bajas where sol000 = :Id"  'en realidad codcategoria viene accion

                lParameters1.Add(New OracleParameter("traduccion", OracleDbType.Int16, 1500, Solicitud.Accion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("bajas", OracleDbType.Int16, Solicitud.bajas, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codCategoria", OracleDbType.Int16, Solicitud.cierre, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("Id", OracleDbType.Varchar2, Solicitud.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico(), lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar una Solicitud", ex)
            End Try
        End Function

        Public Function ModificarSolicitudComment(ByVal Solicitud As ELL.CEtico) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)

            Try

                query = "update CE_SOL set  sol006 =:traduccion where sol000 = :Id"

                lParameters1.Add(New OracleParameter("traduccion", OracleDbType.Varchar2, Solicitud.traduccion, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("Id", OracleDbType.Varchar2, Solicitud.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico(), lParameters1.ToArray)



                lParameters2.Add(New OracleParameter("traduccion", OracleDbType.Varchar2, Solicitud.traduccion, ParameterDirection.Input))

                lParameters2.Add(New OracleParameter("Id", OracleDbType.Varchar2, Solicitud.Id, ParameterDirection.Input))

                query = "update CE_SOL_HIST set  sol006 =:traduccion where sol000 = :Id and fecha_mod = (select max(fecha_mod) from CE_SOL_HIST)"

                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico(), lParameters2.ToArray)
                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar una Solicitud", ex)
            End Try
        End Function

        Public Function ModificarSolicitudcerrada(ByVal Solicitud As ELL.CEtico) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "update CE_SOL set sol005 =:codCategoria, sol007 =:cerrada where sol000 = :Id"  'en realidad motivo de cerrada

                lParameters1.Add(New OracleParameter("codCategoria", OracleDbType.Varchar2, 1500, 1, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("cerrada", OracleDbType.Varchar2, Solicitud.traduccion, ParameterDirection.Input))

                lParameters1.Add(New OracleParameter("Id", OracleDbType.Varchar2, Solicitud.Id, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico(), lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar una Solicitud", ex)
            End Try
        End Function

        Public Function CargarListaSolicitudes(rolUsuario, tra) As List(Of ELL.CEtico)
            Dim query As String

            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol098, FECHA_MOD  FROM CE_SOL where ( sol001 = " & tra & " or  sol099 = " & tra & " ) and (sol007 = 0 or sol007 = 99)   Order by sol000 desc "



            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                               New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("sol000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("sol098")), .Idtra = SabLib.BLL.Utils.stringNull(r("sol001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("sol002")), .FechaMod = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MOD")), .comentario = SabLib.BLL.Utils.stringNull(r("sol004")), .codCategoria = SabLib.BLL.Utils.stringNull(r("sol003")), .Accion = SabLib.BLL.Utils.integerNull(r("sol005")), .traduccion = SabLib.BLL.Utils.stringNull(r("sol006")), .cierre = SabLib.BLL.Utils.integerNull(r("sol007")), .bajas = SabLib.BLL.Utils.integerNull(r("sol008"))}, query, ConnectionCEtico())

        End Function
        Public Function CargarListaSolicitudesComite() As List(Of ELL.CEtico)
            Dim query As String
            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol098, FECHA_MOD  FROM CE_SOL where sol005 ='xxxxxxxxx' )    Order by sol000  desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                               New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("sol000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("sol098")), .Idtra = SabLib.BLL.Utils.stringNull(r("sol001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("sol002")), .FechaMod = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MOD")), .comentario = SabLib.BLL.Utils.stringNull(r("sol004")), .codCategoria = SabLib.BLL.Utils.stringNull(r("sol003")), .Accion = SabLib.BLL.Utils.integerNull(r("sol005")), .traduccion = SabLib.BLL.Utils.stringNull(r("sol006")), .cierre = SabLib.BLL.Utils.integerNull(r("sol007"))}, query, ConnectionCEtico())

        End Function

        Public Function GetUsuario(ByVal idSAB As Integer, plantaAdmin As Integer) As List(Of ELL.CEtico)
            'Dim u = GetMemCacheInstance().Get(Keys.Usuario + idSAB.ToString())
            'If u Is Nothing Then
            Dim query As String
            query = "SELECT nombre, APELLIDO1, APELLIDO2 FROM USUARIOS WHERE idplanta= " & plantaAdmin & " and CODPERSONA=" & idSAB
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                            New ELL.CEtico With {.Idtra = SabLib.BLL.Utils.stringNull(r("NOMBRE")) & ", " & SabLib.BLL.Utils.stringNull(r("APELLIDO1")) & " " & SabLib.BLL.Utils.stringNull(r("APELLIDO2"))}, query, CadenaConexionSAB())
            'New OracleParameter(":ID", OracleDbType.Int32, idSAB, ParameterDirection.Input)).First()
            'End If
            'Return u
        End Function
        Public Function GetSolicitud(ByVal idSol As Integer) As List(Of ELL.CEtico)
            'Dim u = GetMemCacheInstance().Get(Keys.Usuario + idSAB.ToString())
            'If u Is Nothing Then
            Dim query As String
            query = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol098, sol099, FECHA_MOD  FROM CE_SOL WHERE sol000=" & idSol
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                            New ELL.CEtico With {.planta = SabLib.BLL.Utils.stringNull(r("sol099")), .Id = SabLib.BLL.Utils.integerNull(r("sol000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("sol098")), .Idtra = SabLib.BLL.Utils.stringNull(r("sol001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("sol002")), .FechaMod = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MOD")), .comentario = SabLib.BLL.Utils.stringNull(r("sol004")), .codCategoria = SabLib.BLL.Utils.stringNull(r("sol003")), .Accion = SabLib.BLL.Utils.integerNull(r("sol005")), .traduccion = SabLib.BLL.Utils.stringNull(r("sol006")), .cierre = SabLib.BLL.Utils.integerNull(r("sol007"))}, query, ConnectionCEtico())
            'New OracleParameter(":ID", OracleDbType.Int32, idSAB, ParameterDirection.Input)).First()
            'End If
            'Return u
        End Function
        Public Function GetPlantas() As List(Of ELL.CEtico)
            'Dim u = GetMemCacheInstance().Get(Keys.Usuario + idSAB.ToString())
            'If u Is Nothing Then
            Dim query As String
            query = "Select pl000, pl001 FROM CE_PLANTAS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                            New ELL.CEtico With {.planta = SabLib.BLL.Utils.integerNull(r("pl000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("pl001"))}, query, ConnectionCEtico())
            'New OracleParameter(":ID", OracleDbType.Int32, idSAB, ParameterDirection.Input)).First()
            'End If
            'Return u
        End Function
        Public Function CargarListaSolicitudesTodos() As List(Of ELL.CEtico)

            Dim query As String = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol098, FECHA_MOD  FROM CE_SOL    Order by sol000  desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                               New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("sol000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("sol098")), .Idtra = SabLib.BLL.Utils.stringNull(r("sol001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("sol002")), .FechaMod = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MOD")), .comentario = SabLib.BLL.Utils.stringNull(r("sol004")), .codCategoria = SabLib.BLL.Utils.stringNull(r("sol003")), .Accion = SabLib.BLL.Utils.stringNull(r("sol005")), .traduccion = SabLib.BLL.Utils.stringNull(r("sol006")), .cierre = SabLib.BLL.Utils.stringNull(r("sol007")), .bajas = SabLib.BLL.Utils.stringNull(r("sol008"))}, query, ConnectionCEtico())

        End Function

        Public Function LeerEmpDoc(ByVal idDocumento As Integer) As List(Of ELL.CEtico)

            Try
                Dim query As String = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol098, FECHA_MOD  FROM CE_SOL WHERE SOL000= " & idDocumento & " Order by sol000  desc"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                               New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("sol000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("sol098")), .Idtra = SabLib.BLL.Utils.stringNull(r("sol001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("sol002")), .FechaMod = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MOD")), .comentario = SabLib.BLL.Utils.stringNull(r("sol004")), .codCategoria = SabLib.BLL.Utils.stringNull(r("sol003")), .Accion = SabLib.BLL.Utils.integerNull(r("sol005")), .traduccion = SabLib.BLL.Utils.stringNull(r("sol006")), .cierre = SabLib.BLL.Utils.integerNull(r("sol007"))}, query, ConnectionCEtico())


            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento", ex)
            End Try
            '   Return resultado
        End Function

        Public Function LeerEmpDocHist(ByVal idDocumento As Integer) As List(Of ELL.CEtico)

            Try
                Dim query As String = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol098, FECHA_MOD  FROM CE_SOL_HIST WHERE SOL000= " & idDocumento & " Order by fecha_mod  desc"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                               New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("sol000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("sol098")), .Idtra = SabLib.BLL.Utils.stringNull(r("sol001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("sol002")), .FechaMod = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MOD")), .comentario = SabLib.BLL.Utils.stringNull(r("sol004")), .codCategoria = SabLib.BLL.Utils.stringNull(r("sol003")), .Accion = SabLib.BLL.Utils.integerNull(r("sol005")), .traduccion = SabLib.BLL.Utils.stringNull(r("sol006")), .cierre = SabLib.BLL.Utils.integerNull(r("sol007")), .bajas = SabLib.BLL.Utils.integerNull(r("sol008"))}, query, ConnectionCEtico())


            Catch ex As Exception
                Throw New SabLib.BatzException("Error al leer el documento", ex)
            End Try
            '   Return resultado
        End Function

        Public Function CargarListaSolicitudesTodosPlanta(tra) As List(Of ELL.CEtico)

            Dim query As String = "Select sol000, sol001, sol002, sol003, sol004, sol005, sol006, sol007, sol008, sol098, FECHA_MOD  FROM CE_SOL WHERE sol001 = " & tra & " or  sol099 = " & tra & "  Order by sol000  desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                               New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("sol000")), .plantaDesc = SabLib.BLL.Utils.stringNull(r("sol098")), .Idtra = SabLib.BLL.Utils.stringNull(r("sol001")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("sol002")), .FechaMod = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MOD")), .comentario = SabLib.BLL.Utils.stringNull(r("sol004")), .codCategoria = SabLib.BLL.Utils.stringNull(r("sol003")), .Accion = SabLib.BLL.Utils.integerNull(r("sol005")), .traduccion = SabLib.BLL.Utils.stringNull(r("sol006")), .cierre = SabLib.BLL.Utils.integerNull(r("sol007")), .bajas = SabLib.BLL.Utils.integerNull(r("sol008"))}, query, ConnectionCEtico())

        End Function

        Public Function GuardarRol(ByVal rol As ELL.Rol) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO CE_roles (rol099, rol000, rol001, rol002, rol003) VALUES(:planta, :codigo, :rol,:nombreuser,:nombrerol)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, 1, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, rol.Id, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("rol", OracleDbType.Int32, rol.rol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombreuser", OracleDbType.Varchar2, rol.NombreUser, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("nombrerol", OracleDbType.Varchar2, rol.NombreRol, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un Rol", ex)
            End Try
        End Function

        Public Function loadListCad() As List(Of SabLib.ELL.Planta)

            Dim query As String = "Select pl000, pl001  FROM CE_PLANTAS  order by pl000 asc"

            Return Memcached.OracleDirectAccess.seleccionar(Of SabLib.ELL.Planta)(Function(r As OracleDataReader) _
                             New SabLib.ELL.Planta With {.Id = CInt(r("pl000")), .Nombre = SabLib.BLL.Utils.stringNull(r("pl001"))}, query, ConnectionCEtico)


        End Function


        Public Function DeleteRol(ByVal planta As Int32, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Try

                query = "DELETE FROM CE_roles WHERE  rol000 = " & codigo


                Memcached.OracleDirectAccess.NoQuery(query, ConnectionCEtico, lParameters1.ToArray)


                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al borrar un rol", ex)
            End Try
            Return resultado
        End Function

        Public Function CargarRol(ByVal idUser As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol001, rol002 FROM CE_roles  WHERE rol000=" & idUser

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("rol001")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, ConnectionCEtico)


        End Function

        Public Function CargarRolMail(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol000 FROM CE_roles  WHERE rol001=2 "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("rol000"))}, query, ConnectionCEtico)


        End Function


        Public Function CargarEmail(ByVal id As Integer) As List(Of ELL.CEtico)

            'Dim query As String = "select codpro, nomprov, cif from gcprovee  WHERE cif like '%" & cif & "%'"


            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
            '                 New ELL.Empresas With {.Id = CInt(r("codpro")), .Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionXBAT)

            Dim query As String = "select id, email from usuarios  WHERE codpersona = " & id


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                             New ELL.CEtico With {.Id = CInt(r("id")), .email = SabLib.BLL.Utils.stringNull(r("email"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarCultura(ByVal email As String) As List(Of ELL.CEtico)


            Dim query As String = "select idculturas from usuarios  WHERE email = '" & email & "'"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                             New ELL.CEtico With {.campo1 = SabLib.BLL.Utils.stringNull(r("idculturas"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarEmailTo(ByVal id As Integer) As List(Of ELL.CEtico)

            'Dim query As String = "select codpro, nomprov, cif from gcprovee  WHERE cif like '%" & cif & "%'"


            'Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
            '                 New ELL.Empresas With {.Id = CInt(r("codpro")), .Nif = SabLib.BLL.Utils.stringNull(r("cif")), .Nombre = SabLib.BLL.Utils.stringNull(r("nomprov"))}, query, CadenaConexionXBAT)

            Dim query As String = "select id, email from usuarios  WHERE id = " & id


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                             New ELL.CEtico With {.Id = CInt(r("id")), .email = SabLib.BLL.Utils.stringNull(r("email"))}, query, CadenaConexionSAB)


        End Function

        Public Function CargarCodusuario(ByVal mail As String) As List(Of ELL.CEtico)

            Dim query As String = "select codpersona from usuarios  WHERE (fechabaja is  null or fechabaja > sysdate) and email = '" & mail & "'"


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CEtico)(Function(r As OracleDataReader) _
                             New ELL.CEtico With {.Id = SabLib.BLL.Utils.integerNull(r("codpersona"))}, query, CadenaConexionSAB)


        End Function


        Public Function ModificarRol(ByVal Rol As ELL.Rol, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE CE_roles SET  rol001=:intervalo WHERE rol000=:codigo "


                Dim lParameters1 As New List(Of OracleParameter)

                lParameters1.Add(New OracleParameter("intervalo", OracleDbType.Int32, Rol.rol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("codigo", OracleDbType.Int32, codigo, ParameterDirection.Input))


                Memcached.OracleDirectAccess.NoQuery(strSql, ConnectionCEtico, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Function CargarListaRol(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol000, rol001, rol003  as nombrerol, rol002  FROM CE_roles   order by rol000 desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("rol000")), .rol = SabLib.BLL.Utils.stringNull(r("rol001")), .NombreRol = SabLib.BLL.Utils.stringNull(r("nombreRol")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, ConnectionCEtico)


        End Function
        Public Function CargarListaResponsabletexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Responsables)
            Dim nombre As String()
            Dim apellidos As String()

            nombre = Split(Trim(texto), " ")
            apellidos = Split(nombre(0), " ")
            Dim query As String
            Dim codpersona As Integer
            If IsNumeric(texto) Then
                codpersona = CInt(texto)
            Else
                codpersona = 0
            End If
            If nombre.Count > 1 Then

                query = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE idplanta = " & plantaAdmin & " And   ( UPPER(apellido1) like UPPER('%" & nombre(1) & "%')" & " and UPPER(nombre) like UPPER('%" & nombre(0) & "%')" & " ) or  ( UPPER(apellido2) like UPPER('%" & nombre(1) & "%')" & " and UPPER(apellido1) like UPPER('%" & nombre(0) & "%')" & "  )                       " & "  and fechabaja is null  and  apellido1 is not null order by apellido1, apellido2"
                '     query = query & "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE   UPPER(apellido1) like UPPER('%" & nombre(1) & "%')" & " and UPPER(nombre) like UPPER('%" & nombre(0) & "%')" & " " & "  and fechabaja is null  and  apellido1 is not null order by apellido1, apellido2"
            Else
                'quito planta (idplanta = " & plantaAdmin & " or idplanta = 230)   and
                query = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE idplanta = " & plantaAdmin & " And (dni Like ('%" & texto & "%') or codpersona = (" & codpersona & ") or          UPPER(apellido1) like UPPER('%" & Trim(texto) & "%')" & " or UPPER(nombre) like UPPER('%" & Trim(texto) & "%')" & " or UPPER(apellido2) like UPPER('%" & Trim(texto) & "%')" & " )  and fechabaja is null  and  apellido1 is not null order by apellido1, apellido2" 'fechabaja is null and
                '  query = "  Select  id As  res000, nombreusuario As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  FROM usuarios  WHERE idplanta = " & plantaAdmin & "    and ( UPPER(apellido1) like UPPER('%" & texto & "%')" & " or UPPER(nombre) like UPPER('%" & texto & "%')" & " or UPPER(apellido2) like UPPER('%" & texto & "%')" & " )     and fechabaja is null and apellido1 is not null order by apellido1, apellido2"
            End If


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Id = CInt(r("res000")), .Abrev = SabLib.BLL.Utils.stringNull(r("res001")), .Nombre = SabLib.BLL.Utils.stringNull(r("res002"))}, query, CadenaConexionSAB)


        End Function


        Public Function CargarTrabajador(ByVal id As Integer) As List(Of ELL.Responsables)


            Dim query As String = "select id,  codpersona As res001,  (apellido1 || ' ' || apellido2 || ', ' || nombre  )  as   res002  from usuarios  WHERE id = " & id


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Responsables)(Function(r As OracleDataReader) _
                             New ELL.Responsables With {.Id = SabLib.BLL.Utils.integerNull(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("res002")), .Planta = SabLib.BLL.Utils.integerNull(r("res001"))}, query, CadenaConexionSAB)


        End Function


        Public Function existe(ByVal ID As String) As Integer

            Dim parameter As New OracleParameter("id", OracleDbType.Int32, ID, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM CE_ROLES WHERE rol000=:id"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), ConnectionCEtico, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function


        Public Function CargarRolCualquierPlanta(ByVal idUser As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol001, rol002, rol099 FROM CE_roles  WHERE rol000=" & idUser & " order by rol001 desc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("rol001")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002")), .rol = SabLib.BLL.Utils.integerNull(r("rol099"))}, query, ConnectionCEtico)


        End Function

        Public Function CargarRolCualquierPlantaMayor2(ByVal idUser As Integer) As List(Of ELL.Rol)

            Dim query As String = "Select rol001 as rol001, rol002 FROM CE_roles  WHERE rol001 > 1 and rol000=" & idUser & " order by rol001 desc "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Rol)(Function(r As OracleDataReader) _
                             New ELL.Rol With {.Id = CInt(r("rol001")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, ConnectionCEtico)


        End Function

    End Class
End Namespace