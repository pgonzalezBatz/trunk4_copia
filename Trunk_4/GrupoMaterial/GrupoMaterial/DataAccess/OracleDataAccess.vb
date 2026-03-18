
Imports Oracle.ManagedDataAccess.Client
Imports SabLib
Imports SabLib.BLL

Public Class OracleDataAccess
    Public cnString As String = ConfigurationManager.ConnectionStrings("ORACLECONNECTION").ConnectionString
    Public cnString2 As String = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
    Public cnString3 As String = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
    Public cnString4 As String = ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString
    Public cnString5 As String = ConfigurationManager.ConnectionStrings("SABTEST").ConnectionString

    Public Function getAuthorizedUsers() As List(Of SabLib.ELL.Usuario)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))



        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select
        query.Append("SELECT distinct U.ID,U.IDEMPRESAS,U.IDCULTURAS, U.NOMBREUSUARIO,U.IDDIRECTORIOACTIVO,U.CODPERSONA,U.PWD,U.FECHAALTA,U.FECHABAJA,U.APELLIDO1,U.APELLIDO2,U.IDMATRIX,U.IDFTP,U.EMAIL,U.IDDEPARTAMENTO,U.NOMBRE,NULL AS FOTO,U.DNI,U.NIKEUSKARAZ,U.IDPLANTA ")
        query.Append("FROM W_RECURSOS_USUARIO RP INNER JOIN USUARIOS U ON RP.IDUSUARIO=U.ID ")
        query.Append("WHERE RP.IDRECURSO=:IDRECURSO ")
        query.Append(" AND ((U.FECHABAJA IS NULL) OR (U.FECHABAJA IS NOT NULL AND U.FECHABAJA>=TRUNC(SYSDATE))) ")
        query.Append(" ORDER BY U.NOMBRE,APELLIDO1,APELLIDO2")

        ''''working!!!!!!!!!
        ''''Dim parameters As New List(Of OracleParameter)
        ''''parameters.Add(New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input))
        ''''Dim lUser = Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuario)(Function(r As OracleDataReader) New ELL.Usuario With {.Id = CInt(r("ID")), .IdDirectorioActivo = r("IDDIRECTORIOACTIVO"), .Nombre = r("NOMBRE"), .Apellido1 = r("APELLIDO1"), .Apellido2 = r("APELLIDO2")}, query.ToString, cnString, parameters.ToArray)
        ''''Return lUser

        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString, parameters1)
        Dim listUsuarios As List(Of SabLib.ELL.Usuario) = Nothing
        If (lUser1 IsNot Nothing) Then
            listUsuarios = New List(Of SabLib.ELL.Usuario)
            For Each sUser As String() In lUser1
                listUsuarios.Add(getObject(sUser, False, False))
            Next
        End If
        Return listUsuarios

    End Function

    Public Function Completo(ByVal id As String) As List(Of String())
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        Dim result As String = ""
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString4 = ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString
        Else
            cnString4 = ConfigurationManager.ConnectionStrings("SABTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select
        query.Append("select distinct ID_PLANTA from ELEMENTO_PLANTA_PROV_HOMOL where ID_ELEMENTO=" & id)


        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString2, parameters1)



        Return lUser1
    End Function


    Public Function Completo2(ByVal id As String) As List(Of String())
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        Dim result As String = ""
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString4 = ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString
        Else
            cnString4 = ConfigurationManager.ConnectionStrings("SABTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select
        query.Append("select distinct ID_criticidad from criticidad_elemento where ID_ELEMENTO='" & id & "'")


        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString2, parameters1)



        Return lUser1
    End Function


    Public Function Valoresplantatxt(ByVal id As String) As String
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        Dim result As String = ""
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString4 = ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString
        Else
            cnString4 = ConfigurationManager.ConnectionStrings("SABTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select
        query.Append("select nombre from plantas where id=" & id)


        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString4, parameters1)



        Return lUser1(0)(0)
    End Function


    Public Function Seleccionado(ByVal id As String, ByVal id2 As Integer) As List(Of Criticidad)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select

        query.Append("select id_criticidad  from criticidad_elemento where  id_criticidad=" & id2 & " and id_elemento='" & id & "'")



        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString2, parameters1)

        Dim i As Integer
        Dim listUsuarios As New List(Of Criticidad)
        If (lUser1 IsNot Nothing) Then

            listUsuarios = New List(Of Criticidad)
            For i = 0 To lUser1.Count - 1
                listUsuarios.Add(New Criticidad With {.Code = lUser1(i)(0)})
            Next

        End If


        Return listUsuarios
    End Function


    Public Function ExisteCriticidad(ByVal id As Integer) As List(Of Criticidad)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select

        query.Append("select id_criticidad  from criticidad_elemento where  id_criticidad=" & id)



        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString2, parameters1)

        Dim i As Integer
        Dim listUsuarios As New List(Of Criticidad)
        If (lUser1 IsNot Nothing) Then

            listUsuarios = New List(Of Criticidad)
            For i = 0 To lUser1.Count - 1
                listUsuarios.Add(New Criticidad With {.Code = lUser1(i)(0)})
            Next

        End If


        Return listUsuarios
    End Function

    Public Function Valoresplantaint(ByVal id As String, ByVal id2 As Integer) As List(Of Criticidad)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select

        query.Append("select NUM_PROM_HOML from ELEMENTO_PLANTA_PROV_HOMOL where id_elemento='" & id & "' and id_planta=" & id2)



        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString2, parameters1)

        Dim i As Integer
        Dim listUsuarios As New List(Of Criticidad)
        If (lUser1 IsNot Nothing) Then

            listUsuarios = New List(Of Criticidad)
            For i = 0 To lUser1.Count - 1
                listUsuarios.Add(New Criticidad With {.Code = lUser1(i)(0)})
            Next

        End If


        Return listUsuarios
    End Function


    Public Function PlantasBBDD() As List(Of Criticidad)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select

        query.Append("select distinct id_planta from ELEMENTO_PLANTA_PROV_HOMOL")



        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString2, parameters1)

        Dim i As Integer
        Dim listUsuarios As New List(Of Criticidad)
        If (lUser1 IsNot Nothing) Then

            listUsuarios = New List(Of Criticidad)
            For i = 0 To lUser1.Count - 1
                listUsuarios.Add(New Criticidad With {.Code = lUser1(i)(0)})
            Next

        End If


        Return listUsuarios
    End Function


    Public Function deletePlantas(ByVal id As String)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If


        'Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        'Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select
        'query.Append("select id, criticidad, desc_criticidad from criticidad order by criticidad asc ")


        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, id, ParameterDirection.Input)



        Dim query2 As String
        query2 = "delete from ELEMENTO_PLANTA_PROV_HOMOL where id_planta not in(" & id & ") "
        'sql = "delete from fcpwtar WHERE (ta020 = " & qTrabajador & ")"

        Memcached.OracleDirectAccess.NoQuery(query2, cnString2, parameters1)


    End Function


    Public Function getCriticidades() As List(Of Criticidad)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If


        Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select
        query.Append("select id, criticidad, desc_criticidad from criticidad order by criticidad asc ")


        Dim parameters1(index) As OracleParameter
        parameters1(0) = New OracleParameter("IDRECURSO", OracleDbType.Int32, idRecurso, ParameterDirection.Input)
        Dim lUser1 = Memcached.OracleDirectAccess.Seleccionar(query.ToString, cnString2, parameters1)

        Dim i As Integer
        Dim listUsuarios As New List(Of Criticidad)
        If (lUser1 IsNot Nothing) Then

            listUsuarios = New List(Of Criticidad)
            For i = 0 To lUser1.Count - 1
                listUsuarios.Add(New Criticidad With {.Code = lUser1(i)(0), .desc = lUser1(i)(2), .Name = lUser1(i)(1)})
            Next
 
        End If

        'Dim listUsuarios As List(Of SabLib.ELL.Usuario) = Nothing
        'If (lUser1 IsNot Nothing) Then
        '    listUsuarios = New List(Of SabLib.ELL.Usuario)
        '    For Each sUser As String() In lUser1
        '        listUsuarios.Add(getObject(sUser, False, False))
        '    Next
        'End If
        Return listUsuarios

    End Function


    Private Function getObject(ByVal sUser As String(), ByVal obtenerFoto As Boolean, ByVal obtenerResponsable As Boolean) As ELL.Usuario
        Dim oUser As New ELL.Usuario

        oUser = New ELL.Usuario
        oUser.Id = CInt(sUser(0))
        oUser.IdEmpresa = CInt(sUser(1))
        oUser.Cultura = sUser(2)
        oUser.NombreUsuario = Utils.stringNull(sUser(3))
        oUser.IdDirectorioActivo = Utils.stringNull(sUser(4))
        oUser.CodPersona = Utils.integerNull(sUser(5))
        oUser.PWD = Utils.stringNull(sUser(6))
        oUser.FechaAlta = Utils.dateTimeNull(sUser(7))
        oUser.FechaBaja = Utils.dateTimeNull(sUser(8))
        oUser.Apellido1 = Utils.stringNull(sUser(9))
        oUser.Apellido2 = Utils.stringNull(sUser(10))
        oUser.IdMatrix = Utils.stringNull(sUser(11))
        oUser.IdFTP = Utils.stringNull(sUser(12))
        oUser.Email = Utils.stringNull(sUser(13))
        oUser.IdDepartamento = Utils.stringNull(sUser(14))
        oUser.Nombre = Utils.stringNull(sUser(15))
        If (obtenerFoto) Then
            If (Not String.IsNullOrEmpty(sUser(16))) Then
                oUser.Foto = Utils.StringToByteArray(sUser(16))
            End If
        End If
        oUser.Dni = Utils.stringNull(sUser(17))
        If (sUser.Length > 18) Then oUser.NikEuskaraz = Utils.booleanNull(sUser(18))
        If (sUser.Length > 19) Then oUser.IdPlanta = Utils.integerNull(sUser(19))
        If (sUser.Length > 20) Then oUser.UsuarioEmpresa = Utils.booleanNull(sUser(20))
        Return oUser
    End Function

    Friend Sub deleteUserForResource(ByVal id As Integer)
        Dim recComp As New SabLib.BLL.RecursosComponent
        recComp.DeleteUsuario(id, ConfigurationManager.AppSettings("resourceId"))
    End Sub

    Friend Sub deleteCriticidad(ByVal id As Integer)
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        'Dim users = sabComp.GetUsuariosConRecurso(ConfigurationManager.AppSettings("resourceId"))
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If


        'Dim idRecurso = ConfigurationManager.AppSettings("resourceId")
        'Dim query As New System.Text.StringBuilder
        Dim index As Integer = 0

        'Como es un distinct, hay que poner todos los campos menos los blobs. Para que mantenga el orden, le asignamos null en la select
        'query.Append("select id, criticidad, desc_criticidad from criticidad order by criticidad asc ")





        Dim query2 As String
        query2 = "DELETE FROM criticidad_elemento WHERE id_criticidad = " & id

        Memcached.OracleDirectAccess.NoQuery(query2, cnString2)

        query2 = "DELETE FROM criticidad WHERE id = " & id

        Memcached.OracleDirectAccess.NoQuery(query2, cnString2)



    End Sub


    Public Function AddValores(ByVal val1 As String, ByVal val2 As String, ByVal val3 As String, ByVal val4 As String, ByVal val5 As String, ByVal val6 As String, ByVal val7 As String, ByVal val8 As String, ByVal val9 As String, ByVal val10 As String, ByVal val11 As String, ByVal val12 As String) As String
        Dim sabComp = New SabLib.BLL.UsuariosComponent
        Dim retorno As String = "Not saved"
        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If

        Dim index As Integer = 0


        Dim query2 As String
        If IsNumeric(val1) And IsNumeric(val2) Then

            query2 = "DELETE FROM criticidad_elemento where id_elemento= '" & val1 & "'"
            Memcached.OracleDirectAccess.NoQuery(query2, cnString2)

            query2 = "INSERT INTO criticidad_elemento (id_elemento, id_criticidad) VALUES('" & val1 & "'," & val2 & ")"
            Memcached.OracleDirectAccess.NoQuery(query2, cnString2)

            query2 = "DELETE FROM ELEMENTO_PLANTA_PROV_HOMOL where id_elemento='" & val1 & "'"
            Memcached.OracleDirectAccess.NoQuery(query2, cnString2)



            ' aqui varios
            retorno = "Saved"

            Dim arrPlantas As String() = Nothing
            Dim Plantasconfig As String = ConfigurationManager.AppSettings("Plantas")
            arrPlantas = Split(Plantasconfig, ",")

            If IsNumeric(val3) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(0) & ",'" & val1 & "'," & val3 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val4) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(1) & ",'" & val1 & "'," & val4 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val5) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(2) & ",'" & val1 & "'," & val5 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val6) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(3) & ",'" & val1 & "'," & val6 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val7) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(4) & ",'" & val1 & "'," & val7 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val8) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(5) & ",'" & val1 & "'," & val8 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val9) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(6) & ",'" & val1 & "'," & val9 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val10) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(7) & ",'" & val1 & "'," & val10 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val11) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(8) & ",'" & val1 & "'," & val11 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
            If IsNumeric(val12) Then
                query2 = "INSERT INTO ELEMENTO_PLANTA_PROV_HOMOL (id_planta, id_elemento, NUM_PROM_HOML) VALUES(" & arrPlantas(9) & ",'" & val1 & "'," & val12 & ")"
                Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
            End If
        End If



        Return retorno


    End Function

    Friend Sub AddCriticidad(ByVal val1 As String, ByVal val2 As String)
        Dim sabComp = New SabLib.BLL.UsuariosComponent

        If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower = "live" Then
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString
        Else
            cnString2 = ConfigurationManager.ConnectionStrings("GRUPOTEST").ConnectionString
        End If

        Dim query2 As String
        If val1 <> "" And val2 <> "" Then

            query2 = "insert into criticidad (criticidad, desc_criticidad)  VALUES('" & val1 & "','" & val2 & "') "
            Memcached.OracleDirectAccess.NoQuery(query2, cnString2)
        End If

    End Sub

    Friend Sub AddUserForResource(ByVal data As String)
        Dim recComp As New SabLib.BLL.RecursosComponent
        recComp.AddUsuario(CInt(data), ConfigurationManager.AppSettings("resourceId"))
    End Sub

    'Friend Function getSuggestions(term As String) As List(Of SabLib.ELL.Usuario)
    '    Dim userComp As New SabLib.BLL.UsuariosComponent
    '    Dim result = userComp.GetUsuarios(New SabLib.ELL.Usuario With {.NombreUsuario = term})
    '    Dim result = userComp.GetUsuariosBusquedaSAB_Optimizado(term)
    '    Return result
    'End Function

    'Friend Function getSuggestions(term As String) As String
    '    Dim userComp As New SabLib.BLL.UsuariosComponent
    '    Dim usuarios = userComp.GetUsuariosBusquedaSAB_Optimizado(term)

    '    Dim resultado As String = "["
    '    usuarios.Sort(Function(o1 As SabLib.ELL.Usuario, o2 As SabLib.ELL.Usuario) o1.NombreCompleto.ToLower < o2.NombreCompleto.ToLower)
    '    For Each usuario As SabLib.ELL.Usuario In usuarios
    '        If (usuario.DadoBaja) Then
    '            Continue For
    '        Else
    '            resultado &= "{""id"":""" & usuario.Id & """,""no"":""" & usuario.NombreCompleto & """,""nt"":""" & usuario.CodPersona & """,""fa"":""" & usuario.FechaAlta.ToShortDateString & """,""fb"":""" & If(usuario.DadoBaja, 1, 0) & """},"
    '        End If
    '    Next
    '    resultado &= "]"
    '    resultado = resultado.Replace("},]", "}]")

    '    Return resultado
    'End Function

    Friend Function getSuggestions(term As String) As List(Of Object)
        Dim userComp As New SabLib.BLL.UsuariosComponent
        Dim usuarios = userComp.GetUsuariosBusquedaSAB_Optimizado(term)
        Dim result As New List(Of Object)
        usuarios.Sort(Function(o1 As SabLib.ELL.Usuario, o2 As SabLib.ELL.Usuario) o1.NombreCompleto.ToLower < o2.NombreCompleto.ToLower)
        For Each usuario As SabLib.ELL.Usuario In usuarios
            If (usuario.DadoBaja) Then
                Continue For
            Else
                result.Add(New With {.id = usuario.Id, .label = usuario.NombreCompleto, .value = usuario.NombreCompleto})
                'resultado &= "{""id"":""" & usuario.Id & """,""no"":""" & usuario.NombreCompleto & """,""nt"":""" & usuario.CodPersona & """,""fa"":""" & usuario.FechaAlta.ToShortDateString & """,""fb"":""" & If(usuario.DadoBaja, 1, 0) & """},"
            End If
        Next
        Return result
    End Function


End Class
