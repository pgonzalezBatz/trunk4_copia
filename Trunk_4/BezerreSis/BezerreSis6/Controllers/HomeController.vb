Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private db As New Entities_BezerreSis

    <Authorize(Roles:="Usuario")>
    Function Index() As ActionResult

        '#If DEBUG Then
        '        Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
        '        aCookie.Values("IdPlanta") = 1
        '        aCookie.Values("Planta_Nombre") = "Batz IGORRE"
        '        Response.Cookies.Add(aCookie)
        '        MvcApplication.Loguear("Info", "ifdebug, SeleccionPlanta: " & aCookie.Values("Planta_Nombre"), User.Identity.Name.ToString)
        '        'Return RedirectToAction("Proceso", "INDICADORES2")
        '        Return RedirectToAction("Index", "RECLAMACIONES")
        '#End If

        Dim cookie = Request.Cookies(FormsAuthentication.FormsCookieName)
        Dim ticket = FormsAuthentication.Decrypt(cookie.Value)

        ViewBag.ddlPlanta = New SelectList(db.PLANTAS.Where(Function(o) o.OBSOLETO = False AndAlso o.ID_BRAIN IsNot Nothing), "ID", "NOMBRE")
        Return View()
    End Function

    <HttpPost()>
    <ActionName("SeleccionPLanta")>
    <ValidateAntiForgeryToken()>
    <Authorize(Roles:="Usuario")>
    Function SeleccionPLanta(ByVal ddlPlanta As Decimal) As ActionResult
        If IsNothing(ddlPlanta) Then
            Return RedirectToAction("Index")
        Else
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            aCookie.Values("IdPlanta") = ddlPlanta
            aCookie.Values("Planta_Nombre") = db.PLANTAS.Where(Function(o) o.ID = ddlPlanta).FirstOrDefault.NOMBRE
            Response.Cookies.Add(aCookie)
            MvcApplication.Loguear("Info", "SeleccionPlanta: " & aCookie.Values("Planta_Nombre"), User.Identity.Name.ToString)
            Return RedirectToAction("Index", "RECLAMACIONES")
        End If
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing) Then
            db.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    <AllowAnonymous>
    Function Mensaje() As ActionResult
        ViewBag.Titulo = Server.HtmlDecode(Request("Titulo"))
        ViewBag.Mensaje = Server.HtmlDecode(Request("msg"))
        Return View()
    End Function

    '<HandleError()>
    Function MyError() As ActionResult
        Return View()
    End Function

    Function UnAuthorized() As ActionResult
        Return View()
    End Function

End Class
