Namespace web
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString
        Dim strCnNavsilon As String = ConfigurationManager.ConnectionStrings("navsilon").ConnectionString
        Dim strCnEpsilon As String = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        Private idRecurso As String = ConfigurationManager.AppSettings("idrecurso")

        Function Index() As ActionResult
            Return RedirectToAction("home")
            If User.Identity.IsAuthenticated Then
                Return RedirectToAction("home")
            End If
            Dim lst = db.GetUsuario(Request.LogonUserIdentity.Name.ToLower, idRecurso, strCn)
            If lst.Count = 0 Then
                Return Redirect("~/accesodenegado.html")
            End If
            Dim idSab = lst(0)(0)
            FormsAuthentication.SetAuthCookie(idSab, True)
            h.SetCulture(idSab, strCn)
            Return RedirectToAction("home")
        End Function
        Function Home(ejercicio As Nullable(Of Integer), mes As Nullable(Of Integer), nMeses As Nullable(Of Integer)) As ActionResult
            Dim lstMapeos = db.GetMapeosEpsilonAdministracion(strCnNavsilon)
            Dim lstDepartamentosRH = db.GetListOfDepartamentosEpsilon(strCnEpsilon)
            lstDepartamentosRH.RemoveAll(
                Function(d)
                    Return lstMapeos.Exists(Function(m) m.idEpsilon = d.id)
                End Function)
            ViewData("departamentossinmappear") = lstDepartamentosRH

            ViewData("nasignacionesreales") = db.GetListOfCuentasDepartamentos(strCnNavsilon).Count
            ViewData("nasignacionesteoricas") = db.GetNumeroDeAsignacionesTeoricas(strCnNavsilon)

            If Not ejercicio.HasValue Then
                ejercicio = Now.AddMonths(-3).Year
            End If
            If Not mes.HasValue Then
                mes = Now.AddMonths(-3).Month
            End If
            If Not nMeses.HasValue Then
                nMeses = 3
            End If
            'Dim lst = db.GetEvolucionPlantilla(ejercicio.Value, mes.Value, nMeses.Value, strCnEpsilon)
            'Agrupar por sexo
            'ViewData("ejercicio") = Enumerable.Range(2013, Now.Year - 2013 + 1).Select(Function(e) New Mvc.SelectListItem With {.Value = e, .Text = e, .Selected = e = ejercicio.Value})
            'ViewData("mes") = Enumerable.Range(1, 12).Select(Function(e) New Mvc.SelectListItem With {.Value = e, .Text = e, .Selected = e = mes.Value})
            'ViewData("nmeses") = Enumerable.Range(1, 6).Select(Function(e) New Mvc.SelectListItem With {.Value = e, .Text = e, .Selected = e = nMeses.Value})
            'ViewData("plantilla") = lst
            Return View()
        End Function
    End Class
End Namespace