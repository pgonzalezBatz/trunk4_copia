Namespace web
    Public Class adminController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("XBAT").ConnectionString

        Function Crear() As ActionResult
            If Request.UrlReferrer Is Nothing Then
                Session("direccionRetorno") = "#"
            Else
                Session("direccionRetorno") = Request.UrlReferrer.AbsoluteUri
            End If
            ViewData("listOfPais") = DBAccess.GetListOfPais(strCn)
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function Crear(ByVal h As Helbide) As ActionResult
            If ModelState.IsValid Then
                Dim dir = Session("direccionRetorno").ToString
                If Regex.IsMatch(dir, "\?") Then
                    dir = dir + "&id=" + DBAccess.CreateHelbide(h, strCn).ToString
                Else
                    dir = dir + "?id=" + DBAccess.CreateHelbide(h, strCn).ToString
                End If
                Return Redirect(dir)
            End If
            ViewData("listOfPais") = DBAccess.GetListOfPais(strCn)
            Return View(h)
        End Function
        Function CrearPartial() As ActionResult
            ViewData("listOfPais") = DBAccess.GetListOfPais(strCn)
            Return PartialView()
        End Function

        Function Buscar() As ActionResult
            If Request.QueryString("q") IsNot Nothing AndAlso Request.QueryString("q").Length > 0 Then
                ViewData("listOfHelbide") = DBAccess.Buscar(Request.QueryString("q"), strCn)
            End If
            If Request.QueryString("id") IsNot Nothing AndAlso Request.QueryString("id").Length > 0 Then
                ViewData("listOfHelbide") = DBAccess.GetHelbide(Request.QueryString("id"), strCn)
            End If
            Return View()
        End Function

        Function Editar(ByVal id As Integer) As ActionResult
            If Request.UrlReferrer Is Nothing Then
                Session("direccionRetorno") = "#"
            Else
                Session("direccionRetorno") = Request.UrlReferrer.AbsoluteUri
            End If
            ViewData("listOfPais") = DBAccess.GetListOfPais(strCn)
            Dim h = DBAccess.GetHelbide(id, strCn).First()
            Return View(h)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function Editar(ByVal h As Helbide) As ActionResult
            If ModelState.IsValid Then
                DBAccess.UpdateHelbide(h, strCn)
                Return Redirect(Session("direccionRetorno"))
            End If
            ViewData("listOfPais") = DBAccess.GetListOfPais(strCn)
            Return View(h)
        End Function

    End Class
End Namespace