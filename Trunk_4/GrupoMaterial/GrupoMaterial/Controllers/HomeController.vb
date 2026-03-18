Imports System.Data.SqlClient
Imports Microsoft.SqlServer.Dts.Runtime
Imports Microsoft.SqlServer.Dts.Tasks.ScriptTask
Imports Microsoft.SqlServer.Dts.Tasks.ExecutePackageTask

Imports Microsoft.SqlServer.Management.IntegrationServices
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports Microsoft.SqlServer.Management.IntegrationServices.Operation

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    '<CustomAuthorize>
    Function Index() As ActionResult

        Return View()
    End Function


    '<Authorize>
    '<CustomAuthorize>
    Function Valorplanta() As ActionResult
        Dim identificicacion As String
        identificicacion = Request.QueryString("name")
        'Dim arrPlantas As String() = Nothing
        'Dim Plantasconfig As String = ConfigurationManager.AppSettings("Plantas")

        'If arrPlantas.Length > 1 Then

        'End If
        'arrPlantas = Split(Plantasconfig, ",")




        Dim DAL = New DataAccessLayer
        Dim list = DAL.Valorplanta(identificicacion)

        Dim statusDebug As String = ConfigurationManager.AppSettings("CurrentStatus")
        If statusDebug.ToLower = "live" Then
            ViewData("Servidor") = "https://intranet2.batz.es/Homeintranet"
        Else
            ViewData("Servidor") = "https://intranet-test.batz.es/Homeintranet"
        End If
        Dim lg As New SabLib.BLL.LoginComponent
        Dim myTicket As New SabLib.ELL.Ticket
        myTicket = lg.Login(HttpContext.User.Identity.Name.ToLower) 'lg.Login("batznt\inycom1") ' 

        '     ViewData("Usuario") = myTicket.NombreCompleto
        ViewData("identificador") = identificicacion
        Return View(list)
    End Function

    '<Authorize>
    '<CustomAuthorize>
    Function Valoresplanta() As ActionResult
        Dim identificicacion As String
        identificicacion = Request.QueryString("name")
        'Dim arrPlantas As String() = Nothing
        'Dim Plantasconfig As String = ConfigurationManager.AppSettings("Plantas")

        'If arrPlantas.Length > 1 Then

        'End If
        'arrPlantas = Split(Plantasconfig, ",")




        Dim DAL = New DataAccessLayer
        Dim list = DAL.Valoresplanta(identificicacion)

        Dim statusDebug As String = ConfigurationManager.AppSettings("CurrentStatus")
        If statusDebug.ToLower = "live" Then
            ViewData("Servidor") = "https://intranet2.batz.es/Homeintranet"
        Else
            ViewData("Servidor") = "https://intranet-test.batz.es/Homeintranet"
        End If
        Dim lg As New SabLib.BLL.LoginComponent
        Dim myTicket As New SabLib.ELL.Ticket
        myTicket = lg.Login(HttpContext.User.Identity.Name.ToLower) 'lg.Login("batznt\inycom1") '

        '     ViewData("Usuario") = myTicket.NombreCompleto

        Return View(list)
    End Function
    '<SimpleRoleProvider(Roles.mando)>
    Function Index2() As ActionResult
        Dim DAL = New DataAccessLayer
        Dim list = DAL.GetFullData()

        Dim statusDebug As String = ConfigurationManager.AppSettings("CurrentStatus")
        If statusDebug.ToLower = "live" Then
            ViewData("Servidor") = "https://intranet2.batz.es/Homeintranet"
        Else
            ViewData("Servidor") = "https://intranet-test.batz.es/Homeintranet"
        End If
        Dim lg As New SabLib.BLL.LoginComponent
        Dim myTicket As New SabLib.ELL.Ticket
        myTicket = lg.Login(HttpContext.User.Identity.Name.ToLower) 'lg.Login("batznt\inycom1") '

        '    ViewData("Usuario") = myTicket.NombreCompleto

        Return View(list)
    End Function

    '<CustomAuthorize>
    Function About() As ActionResult
        ViewData("Message") = "Aplicación CRUD para la administración de los Grupos Materiales."
        Return View()
    End Function

    '<CustomAuthorize>
    Function Job() As ActionResult
        'ViewData("Message") = "Ejecución del paquete SSIS (" & packageName & ")"
        ViewData("Admin") = "Admin."
        Return View()
    End Function
    Function Criticidades() As ActionResult
        ViewBag.Mensaje = ""
        'Dim valor As String
        'valor = Request.QueryString("valor")
        'Dim add As String
        'add = Request.QueryString("add")
        'If valor = "1" Then
        '    ViewBag.Mensaje = "tiene valores asignados"
        '    Return Redirect("/home/criticidades?add=" & 1)
        'End If
        'If valor = "2" Then
        '    ViewBag.Mensaje = "borrado"
        '    Return Redirect("/home/criticidades?add=" & 2)
        'End If
        'If add = "1" Then
        '    ViewBag.Mensaje = "tiene valores asignados"

        'End If
        'If add = "2" Then
        '    ViewBag.Mensaje = "borrado"

        'End If
        Dim oracleDAL As New OracleDataAccess
        Dim authorizedUsers = oracleDAL.getCriticidades()
        Dim statusDebug As String = ConfigurationManager.AppSettings("CurrentStatus")

        If statusDebug.ToLower = "live" Then
            ViewData("Servidor") = "https://intranet2.batz.es/Homeintranet"
        Else
            ViewData("Servidor") = "https://intranet-test.batz.es/Homeintranet"
        End If
        Dim lg As New SabLib.BLL.LoginComponent
        Dim myTicket As New SabLib.ELL.Ticket
        myTicket = lg.Login(HttpContext.User.Identity.Name.ToLower)

        '  ViewData("Usuario") = myTicket.NombreCompleto
        Return View(authorizedUsers)
    End Function



    Function Administrar() As ActionResult
        Dim oracleDAL As New OracleDataAccess
        Dim authorizedUsers = oracleDAL.getAuthorizedUsers()
        Dim statusDebug As String = ConfigurationManager.AppSettings("CurrentStatus")
        If statusDebug.ToLower = "live" Then
            ViewData("Servidor") = "https://intranet2.batz.es/Homeintranet"
        Else
            ViewData("Servidor") = "https://intranet-test.batz.es/Homeintranet"
        End If
        Dim lg As New SabLib.BLL.LoginComponent
        Dim myTicket As New SabLib.ELL.Ticket
        myTicket = lg.Login(HttpContext.User.Identity.Name.ToLower)

        '    ViewData("Usuario") = myTicket.NombreCompleto 'de momento jon 
        Return View(authorizedUsers)
    End Function

    Function Configure() As ActionResult
        Return View()
    End Function

    Function BorrarDatosZamudio() As ActionResult
        Dim sqlDAL As New SQLDataAccess
        sqlDAL.borrarDatosZamudio()
        TempData("msg") = "Datos borrados (Zamudio)."
        Return View("Job")
    End Function

    Function VerEstadoJob() As ActionResult
        Dim sqlDAL As New SQLDataAccess
        TempData("status") = sqlDAL.verEstadoJob()
        Return View("Job")
    End Function


    Function DeleteAdmin(data As Integer) As ActionResult
        Dim oracleDAL As New OracleDataAccess
        oracleDAL.deleteUserForResource(data)
        Dim authorizedUsers = oracleDAL.getAuthorizedUsers()
        Return View("Administrar", authorizedUsers)
    End Function
    Function DeleteCriticidad(data As Integer) As ActionResult
        Dim oracleDAL As New OracleDataAccess
        Dim valor As String = ""
        'leer si existe reg en CRITICIDAD_ELEMENTO
        'Dim tmp As List(Of Criticidad)
        'tmp = oracleDAL.ExisteCriticidad(data)
        'If tmp.Count > 0 Then
        '    valor = "1"
        'Else
        '    'mirar si existe para poner el texto
        '    valor = "2"
        oracleDAL.deleteCriticidad(data)
        'End If

        Dim Criticidades = oracleDAL.getCriticidades()
        Return Redirect("/Home/Criticidades") '?valor=" & valor
    End Function

    Function AddAdmin() As ActionResult
        Dim oracleDAL As New OracleDataAccess
        oracleDAL.AddUserForResource("test")
        Dim authorizedUsers = oracleDAL.getAuthorizedUsers()
        Return View("Administrar", authorizedUsers)
    End Function

    Function AddCriticidad() As JsonResult
        Dim resultado As String
        resultado = "prueba"

        Dim name As String
        Dim desc As String

        name = Request.Form("campo1")
        desc = Request.Form("campo2")
        Dim oracleDAL As New OracleDataAccess
        oracleDAL.AddCriticidad(name, desc)

        Return Json(resultado, JsonRequestBehavior.AllowGet) 


        'Dim Criticidades = oracleDAL.getCriticidades()
        'Return View("Criticidades", Criticidades)
    End Function


    Function AddValores() As JsonResult
        Dim textoJason As String
        Dim DAL = New DataAccessLayer
        Dim resultado As String
        resultado = "prueba"
        textoJason = DAL.JsonSerializer(resultado)

        '   Return Json(resultado, JsonRequestBehavior.AllowGet)





        Dim val1 As String = Request.Form("val1")
        Dim val2 As String = Request.Form("val2")
        Dim campo0 As String = Request.Form("campo0")

        Dim campo1 As String = Request.Form("campo1")
        Dim campo2 As String = Request.Form("campo2")
        Dim campo3 As String = Request.Form("campo3")
        Dim campo4 As String = Request.Form("campo4")
        Dim campo5 As String = Request.Form("campo5")
        Dim campo6 As String = Request.Form("campo6")
        Dim campo7 As String = Request.Form("campo7")
        Dim campo8 As String = Request.Form("campo8")
        Dim campo9 As String = Request.Form("campo9")



        Dim oracleDAL As New OracleDataAccess

        resultado = oracleDAL.AddValores(val1, val2, campo0, campo1, campo2, campo3, campo4, campo5, campo6, campo7, campo8, campo9)
        'Dim Criticidades = oracleDAL.getCriticidades()
        'Return View("Criticidades2", Criticidades)

        'Dim listaType2 As List(Of Criticidad)


        'textoJason = DAL.JsonSerializer(resultado)

        'tempòral    Return Json(textoJason, JsonRequestBehavior.AllowGet)
        Return Json(resultado, JsonRequestBehavior.AllowGet)
        'Return Nothing si quiero que vaya por error en ajax

    End Function

    Function Suggest(term As String) As ActionResult
        Dim oracleDAL As New OracleDataAccess
        Dim result = oracleDAL.getSuggestions(term)
        Return Json(result, JsonRequestBehavior.AllowGet)
    End Function

    Function SuggestRef(term As String) As ActionResult
        Dim sqlDAL As New SQLDataAccess
        Dim result = sqlDAL.getSuggestions(term)
        Return Json(result, JsonRequestBehavior.AllowGet)
    End Function

    <HttpPost()>
    Function SetUserAsAdminForResource(ByVal input As String) As ActionResult
        Dim oracleDAL As New OracleDataAccess
        oracleDAL.AddUserForResource(input)
        Dim authorizedUsers = oracleDAL.getAuthorizedUsers()
        ModelState.Clear()
        Return View("Administrar", authorizedUsers)
    End Function

    Function ActivarReferencia(ref As String) As ActionResult
        Dim sqlDAL As New SQLDataAccess
        sqlDAL.updateReferencia(ref, 0)
        TempData("refActivada") = ref
        Return View("Job")
    End Function

    <HttpPost()>
    Function DesactivarReferencia(ByVal input As String) As ActionResult
        Dim sqlDAL As New SQLDataAccess
        sqlDAL.updateReferencia(input, 1)
        Return View("Job")
    End Function

    <HttpPost()>
    Function ActualizarYLanzarJob(ByVal INFO1 As String, ByVal INFO2 As String, ByVal ANYO1 As String, ByVal MES1 As String, ByVal VIGENTE1 As String, ByVal ANYO2 As String, ByVal MES2 As String, ByVal VIGENTE2 As String, ByVal ejecucion As String) As ActionResult
        ActualizarBD(INFO1, INFO2, ANYO1, ANYO2, MES1, MES2, VIGENTE1, VIGENTE2)
        If (ejecucion.Contains("lanzar")) Then
            LanzarJob()
            TempData("msg") = "Job lanzado."
            Return View("Job")
        Else
            TempData("msg") = "Datos actualizados en BD."
            Return View("Job")
        End If
    End Function

    Private Sub ActualizarBD(info1 As String, info2 As String, anyo1 As String, anyo2 As String, mes1 As String, mes2 As String, vigente1 As String, vigente2 As String)
        Dim sqlDAL As New SQLDataAccess
        sqlDAL.actualizarItem(1, info1, anyo1, mes1, vigente1)
        sqlDAL.actualizarItem(2, info2, anyo2, mes2, vigente2)
    End Sub

    Private Sub LanzarJob()
        Dim sqlDAL As New SQLDataAccess
        sqlDAL.lanzarJob()
    End Sub

    '<HttpPost()>
    'Function LanzarJob(ByVal p As Object) As ActionResult
    '    Dim p1 = p

    '    Return View()
    'End Function
End Class
