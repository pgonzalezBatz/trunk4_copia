Namespace web
    Public Class recogidaController
        Inherits System.Web.Mvc.Controller

        Private strCnXbat As String = ConfigurationManager.ConnectionStrings("xbat").ConnectionString
        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.creacion)> _
        Function list() As ActionResult
            ViewData("idviaje") = Request("idviaje")
            ViewData("listOfRecogidas") = DBAccess.GetListOfRecogidas(strCn)
            'ViewData("puertas") = DBAccess.GetListPuerta(strCn)
            Return View("list")
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        Function viaje(ByVal idviaje As Nullable(Of Integer), ByVal recogida As List(Of Integer)) As ActionResult
            If idviaje.HasValue AndAlso recogida IsNot Nothing Then
                Dim listaRecogidas = DBAccess.GetListOfRecogidas(strCn).FindAll(Function(a) recogida.Exists(Function(i) i = a.Id))
                Dim negociosRecogidas = listaRecogidas.Select(Function(o) o.IdNegocio).Distinct()
                Dim negociosViaje = DBAccess.GetNegociosFromViaje(idviaje, strCn).Distinct()
                Dim negocios = negociosRecogidas.Union(negociosViaje).Distinct().Count()
                If negocios > 1 Then
                    ModelState.AddModelError("Negocio", "Las recogidas no se pueden añadir al viaje, pertenecen a diferentes negocios")
                    Return RedirectToAction("list", "viaje")
                End If
                ViewData("listOfRecogida") = listaRecogidas
                For Each r In recogida
                    DBAccess.AddAlbaranRecogidaToViaje(idviaje, r, "R", strCn)
                Next
                Return RedirectToAction("list", "viaje")
            End If
            Return list()
        End Function

        <SimpleRoleProvider(Role.creacion)> _
        Function create() As ActionResult
            Dim recogida As New Recogida() With {.IdEmpresaEntrega = 1}
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            ViewData("spanempresaentrega") = "Batz Igorre"
            ViewData("ofopcount") = 5
            'ViewData("puertas") = DBAccess.GetListPuerta(strCn)
            Return View(recogida)
        End Function

        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function create(ByVal r As Recogida, ByVal fm As FormCollection) As ActionResult
            r.IdSab = SimpleRoleProvider.GetId()

            'For Each m In ModelState
            '    If m.Key.Contains("ListOfOp") Then
            '        m.Value.Errors.Clear()
            '    End If
            'Next
            r.ListOfOp = r.ListOfOp.Where(Function(OfOp) OfOp.Numord.HasValue AndAlso OfOp.Numope.HasValue AndAlso OfOp.Peso.HasValue)

            If r.ListOfOp.Count = 0 Then
                ModelState.AddModelError("ofop", "Es necesario introducir una of-op y peso como minimo")
            End If
            If r.IdNegocio < 1 Then
                ModelState.AddModelError("idNegocio", "Es necesario elegir un negocio")
            End If
            Dim validOFOP = True
            If ModelState.IsValid Then
                'OF OP validas?
                For Each e In r.ListOfOp
                    If Not DBAccess.IsNumordNumOpActivo(e.Numord, e.Numope, strCn) Then
                        ModelState.AddModelError("ofop", "La of " + e.Numord.ToString + ", op " + e.Numope.ToString + " no es valida")
                    End If
                Next
            End If

            If ModelState.IsValid Then
                DBAccess.SaveRecogida(r, strCn)
                Return RedirectToAction("list")
            End If

            'ViewData("puertas") = DBAccess.GetListPuerta(strCn)
            ViewData("spanempresaentrega") = DBAccess.GetProveedorConDireccion(r.IdEmpresaEntrega, strCnXbat).Nombre
            ViewData("spanempresaRecogida") = If(r.IdEmpresaRecogida, DBAccess.GetProveedorConDireccion(r.IdEmpresaRecogida, strCnXbat).Nombre, "")
            ViewData("ofopcount") = CInt(fm("ofopcount"))

            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View(r)
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        Function edit(ByVal id As Integer) As ActionResult
            ViewData("ofopcount") = 5
            Dim r = DBAccess.GetRecogida(id, strCn)
            ViewData("spanempresarecogida") = DBAccess.GetProveedorConDireccion(r.IdEmpresaRecogida, strCnXbat).Nombre
            ViewData("spanempresaentrega") = DBAccess.GetProveedorConDireccion(r.IdEmpresaEntrega, strCnXbat).Nombre
            'ViewData("puertas") = DBAccess.GetListPuerta(strCn)
            Return View("create", r)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function edit(ByVal r As Recogida) As ActionResult
            r.IdSab = SimpleRoleProvider.GetId()
            Dim ofopActiba = True

            r.ListOfOp = r.ListOfOp.Where(Function(OfOp) OfOp.Numord.HasValue AndAlso OfOp.Numope.HasValue AndAlso OfOp.Peso.HasValue)
            For Each e In r.ListOfOp
                If Not DBAccess.IsNumordNumOpActivo(e.Numord, e.Numope, strCn) Then
                    ModelState.AddModelError("ofop", "La of " + e.Numord.ToString + ", op " + e.Numope.ToString + " no es valida")
                End If
            Next
            If r.ListOfOp.Count = 0 Then
                ModelState.AddModelError("ofop", "Es necesario introducir una of-op y peso como minimo")
            End If
            If ModelState.IsValid Then
                DBAccess.UpdateRecogida(r, strCn)
                Return RedirectToAction("list")
            End If

            'ViewData("puertas") = DBAccess.GetListPuerta(strCn)
            ViewData("spanempresarecogida") = DBAccess.GetProveedorConDireccion(r.IdEmpresaRecogida, strCnXbat).Nombre
            ViewData("spanempresaentrega") = DBAccess.GetProveedorConDireccion(r.IdEmpresaEntrega, strCnXbat).Nombre
            ViewData("ofopcount") = CInt(Request.Form("ofopcount"))
            Return View("create", r)
        End Function

        <SimpleRoleProvider(Role.creacion)> _
        Function delete(ByVal id As Integer) As ActionResult
            Return View(DBAccess.GetRecogida(id, strCn))
        End Function

        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function delete(ByVal id As Integer, ByVal fm As FormCollection) As ActionResult
            If ModelState.IsValid Then
                DBAccess.DeleteRecogida(id, strCn)
                Return RedirectToAction("list")
            End If
            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        Function divide(id As Integer) As ActionResult
            Dim r = DBAccess.GetRecogida(id, strCn)
            Return View(r)
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function divide(id As Integer, lineas As List(Of String)) As ActionResult
            Dim r = DBAccess.GetRecogida(id, strCn)
            If lineas Is Nothing OrElse lineas.Count = 0 Then
                ModelState.AddModelError("lineas", "Es necesario seleccionar lineas para llevar a otra recogida")
            ElseIf r.ListOfOp.Count = lineas.Count Then
                ModelState.AddModelError("lineas", "No tiene sentido llevar todas las lineas a otra recogida")
            End If
            If ModelState.IsValid Then
                Dim lstOfOp As New List(Of OfOp)
                For Each e In lineas
                    Dim s = Split(e, "|")
                    lstOfOp.Add(New OfOp With {.Numord = s(0), .Numope = s(1), .Peso = s(2)})
                Next
                DBAccess.CreateRecogidaFromprevious(id, lstOfOp, strCn)
                Return RedirectToAction("list")
            End If

            Return View(r)
        End Function
        Function details(id As Integer) As ActionResult
            Dim r = DBAccess.GetRecogida(id, strCn)
            ViewData("proveedorRecogida") = DBAccess.GetProveedorConDireccion(r.IdEmpresaRecogida, strCn)
            ViewData("proveedorEntrega") = DBAccess.GetProveedorConDireccion(r.IdEmpresaEntrega, strCn)
            Return View(r)
        End Function
       
    End Class
End Namespace