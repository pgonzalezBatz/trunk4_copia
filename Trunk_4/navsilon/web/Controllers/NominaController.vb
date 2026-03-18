Imports System.Security.Permissions
Public Class NominaController
    Inherits Controller

    Private Const idAplicacion As Integer = 1
    Private Const idAplicacionPagasExtra As Integer = 3
    Private Const idResumen As String = "11"
    Dim strCn As String = ConfigurationManager.ConnectionStrings("navsilon").ConnectionString
    Dim strCnEpsilon As String = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString

    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    Function Index() As ActionResult
        ViewData("listofasiento") = db.GetListOfAsiento(idAplicacion, strCn).Union(db.GetListOfAsiento(idAplicacionPagasExtra, strCn))
        ViewData("tiposcuenta") = db.GetListOfTipoCuentaConColumnas(idAplicacion, idResumen, strCn, strCnEpsilon).Union(db.GetListOfTipoCuentaConColumnas(idAplicacionPagasExtra, idResumen, strCn, strCnEpsilon))
        ViewData("fechas") = Enumerable.Range(1, 12).Select(Function(i) Now.AddMonths(-i))
        Return View()
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    Function addasiento() As ActionResult
        Return View("editasiento")
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function addasiento(nombre As String) As ActionResult
        ViewData("cuentasenasiento") = db.GetListOfTipoCuentaEnAsiento(idAplicacion, idAplicacion, strCn)
        If nombre.Length = 0 Then
            ModelState.AddModelError("nombre", "El nombre del asiento es obligatorio")
        End If
        If ModelState.IsValid Then
            db.AddAsiento(idAplicacion, nombre, strCn)
            Return RedirectToAction("index")
        End If
        Return View("editasiento", nombre)
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    Function editarasiento(idAsiento As Integer) As ActionResult
        Return View(db.GetListOfAsiento(idAplicacion, strCn).Find(Function(a) a.id = idAsiento))
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function editarasiento(idAsiento As Integer, nombre As String) As ActionResult
        ViewData("cuentasenasiento") = db.GetListOfTipoCuentaEnAsiento(idAplicacion, idAplicacion, strCn)
        If nombre.Length = 0 Then
            ModelState.AddModelError("nombre", "El nombre del asiento es obligatorio")
        End If
        If ModelState.IsValid Then
            'idAsiento = db.editAsiento(idAplicacion, nombre, strCn)
            Return RedirectToAction("index")
        End If
        Return View(nombre)
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    Function definirasiento(idAsiento As Integer) As ActionResult
        ViewData("cuentasnoasiento") = db.GetListOfTipoCuentaNoEnAsiento(idAplicacion, idAsiento, strCn).Union(db.GetListOfTipoCuentaNoEnAsiento(idAplicacionPagasExtra, idAsiento, strCn)).ToList
        ViewData("cuentasenasiento") = db.GetListOfTipoCuentaEnAsiento(idAplicacion, idAsiento, strCn).Union(db.GetListOfTipoCuentaEnAsiento(idAplicacionPagasExtra, idAsiento, strCn)).ToList
        Return View(db.GetListOfAsiento(idAplicacion, strCn).Union(db.GetListOfAsiento(idAplicacionPagasExtra, strCn)).Where(Function(o) o.id = idAsiento).First)
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
    <AcceptVerbs(HttpVerbs.Post)>
    Function definirasiento(idAsiento As Integer?, idtipocuenta As Integer?, sumaresta As String, add As String, subtract As String, delete As String) As ActionResult
        Dim idaplicacion_ = db.getAplicacionFromTipoCuenta(idtipocuenta, strCn)
        If add IsNot Nothing Then
            db.AddTipoCuentaAAsiento(idaplicacion_, idAsiento, idtipocuenta, "suma", strCn)
        End If
        If subtract IsNot Nothing Then
            db.AddTipoCuentaAAsiento(idaplicacion_, idAsiento, idtipocuenta, "resta", strCn)
        End If
        If delete IsNot Nothing Then
            db.RemoveTipoCuentaAAsiento(idaplicacion_, idAsiento, idtipocuenta, strCn)
        End If
        Return RedirectToAction("definirasiento", New With {.idAsiento = idAsiento})
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    Function editcolumnas(idTipoCuenta As Integer) As ActionResult
        Dim lst As List(Of Object) = db.GetListOfTipoCuentaConColumnas(idAplicacion, idResumen, strCn, strCnEpsilon).Union(db.GetListOfTipoCuentaConColumnas(idAplicacionPagasExtra, idResumen, strCn, strCnEpsilon)).Where(Function(tc) tc.key.id = idTipoCuenta).First.listOfColumns
        ViewData("columnaspendientes") = db.GetListOfcolumnas(idResumen, strCnEpsilon).Where(Function(c) Not lst.Exists(Function(c2) c.idcolumna = c2.idcolumna))
        ViewData("columnas") = db.GetListOfcolumnas(idResumen, strCnEpsilon).Where(Function(c) lst.Exists(Function(c2) c.idcolumna = c2.idcolumna))
        Return View()
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
    <AcceptVerbs(HttpVerbs.Post)>
    Function editcolumnas(idTipoCuenta As Integer, idColumna As String, add As String, remove As String) As ActionResult
        If add IsNot Nothing Then
            db.AddTipoCuentaColumna(idTipoCuenta, idColumna, strCn)
        End If
        If remove IsNot Nothing Then
            db.RemoveTipoCuentaColumna(idTipoCuenta, idColumna, strCn)
        End If
        Return RedirectToAction("editcolumnas", New With {.idTipoCuenta = idTipoCuenta})
    End Function

    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
    Function tipoNomina(year As Integer, month As Integer, paga As Integer?) As ActionResult
        Return View(db.GetTiposNomina(year, month, strCnEpsilon))
    End Function

    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
    Function mesasiento(year As Integer, month As Integer, paga As Integer?, idNomina As List(Of String)) As ActionResult
        Dim cIdAplicacion As Integer = idAplicacion
        Dim lstNominaDeps = db.GetNominaEpsilonDeps(idResumen, year, If(paga, month), idNomina, strCnEpsilon).SelectMany(
            Function(nd)
                Dim t = db.GetCuentasTraspaso(cIdAplicacion, nd.idColumna, nd.idNivel, strCn)
                Dim l As New List(Of Object)
                For Each e In t
                    Dim lantegi = If(e.lantegiCuenta.length > 0, e.lantegiCuenta, e.lantegi)
                    l.Add(New With {nd.idNivel, nd.dNivel, nd.devengo, nd.columna, nd.idColumna, nd.idConvenio,
                                     e.nombreapunte, e.cuenta, lantegi, e.operacion, e.idAsiento, e.nombreAsiento, nd.count})
                Next
                Return l
            End Function)
        Dim lst = lstNominaDeps.Where(Function(nd) nd IsNot Nothing).OrderBy(Function(nd) nd.cuenta).GroupBy(Function(nd) New With {Key nd.nombreapunte, Key nd.cuenta, Key nd.lantegi, Key nd.idAsiento, Key nd.nombreAsiento, Key nd.idConvenio},
                                                                                                Function(k, l)

                                                                                                    Return New With {.key = k, .lst = l, .total = CDec(l.Sum(Function(s)
                                                                                                                                                                 If s.operacion = "suma" Then
                                                                                                                                                                     Return CDec(s.devengo)
                                                                                                                                                                 Else
                                                                                                                                                                     Return -CDec(s.devengo)
                                                                                                                                                                 End If
                                                                                                                                                             End Function)), .count = l.Sum(Function(s) s.count)}
                                                                                                End Function).GroupBy(Function(g) g.key.nombreAsiento, Function(k, l)
                                                                                                                                                           Dim fl = l.Where(Function(s)
                                                                                                                                                                                Return (Regex.IsMatch(k, "la", RegexOptions.IgnoreCase) And s.key.idConvenio = "001") Or (Regex.IsMatch(k, "ss", RegexOptions.IgnoreCase) And (s.key.idConvenio = "002" Or s.key.idConvenio = "006"))
                                                                                                                                                                            End Function)
                                                                                                                                                           Return New With {.key = k, .l = fl, .total = fl.Sum(Function(s) s.total), .count = fl.Sum(Function(s) s.count)}
                                                                                                                                                       End Function).ToList



        ViewData("fregistro") = New Date(year, month, Date.DaysInMonth(year, month)).ToShortDateString
        ViewData("PagosTOtal") = db.GetSumPago(year, If(paga, month), idNomina, strCnEpsilon)
        Return View(lst)
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
    <AcceptVerbs(HttpVerbs.Post)>
    Function mesasiento(year As Integer, month As Integer, ndoc As String, paga As Integer?, idNomina As List(Of String), fRegistro As Date?) As ActionResult
        Dim cIdAplicacion As Integer = idAplicacion
        Dim lstNominaDeps = db.GetNominaEpsilonDeps(idResumen, year, If(paga, month), idNomina, strCnEpsilon).SelectMany(
            Function(nd)
                Dim t = db.GetCuentasTraspaso(cIdAplicacion, nd.idColumna, nd.idNivel, strCn)
                Dim l As New List(Of Object)
                For Each e In t
                    Dim lantegi = If(e.lantegiCuenta.length > 0, e.lantegiCuenta, e.lantegi)
                    l.Add(New With {.idNivel = nd.idNivel, .dNivel = nd.dNivel, .devengo = nd.devengo, .columna = nd.columna, .idColumna = nd.idColumna, .idConvenio = nd.idConvenio,
                                    .nombreapunte = e.nombreapunte, .cuenta = e.cuenta, .lantegi = lantegi, .operacion = e.operacion, .idAsiento = e.idAsiento, .nombreAsiento = e.nombreAsiento})
                Next
                Return l
            End Function)
        Dim lst = lstNominaDeps.Where(Function(nd) nd IsNot Nothing).OrderBy(Function(nd) nd.cuenta).GroupBy(Function(nd) New With {Key .nombreapunte = nd.nombreapunte, Key .cuenta = nd.cuenta, Key .lantegi = nd.lantegi, Key .idAsiento = nd.idAsiento, Key .nombreAsiento = nd.nombreAsiento, Key .idConvenio = nd.idConvenio},
                                                                                                Function(k, l)

                                                                                                    Return New With {.key = k, .lst = l, .total = CDec(l.Sum(Function(s)
                                                                                                                                                                 If s.operacion = "suma" Then
                                                                                                                                                                     Return CDec(s.devengo)
                                                                                                                                                                 Else
                                                                                                                                                                     Return -CDec(s.devengo)
                                                                                                                                                                 End If
                                                                                                                                                             End Function))}
                                                                                                End Function).GroupBy(Function(g) g.key.nombreAsiento, Function(k, l)
                                                                                                                                                           Dim fl = l.Where(Function(s)
                                                                                                                                                                                Return (Regex.IsMatch(k, "la", RegexOptions.IgnoreCase) And s.key.idConvenio = "001") Or (Regex.IsMatch(k, "ss", RegexOptions.IgnoreCase) And (s.key.idConvenio = "002" Or s.key.idConvenio = "006"))
                                                                                                                                                                            End Function)
                                                                                                                                                           Return New With {.key = k, .l = fl, .total = fl.Sum(Function(s) s.total)}
                                                                                                                                                       End Function).ToList
        If Not fRegistro.HasValue Then
            ModelState.AddModelError("fregistro", h.traducir("Es obligatorio introducir la fecha de registro"))
        End If
        If String.IsNullOrEmpty(ndoc) Then
            ModelState.AddModelError("ndoc", h.traducir("Es obligatorio introducir el numero de documento"))
        End If
        If ModelState.IsValid Then
            db.TraspasarAsientoANavision(fRegistro.Value, ndoc, lst, ConfigurationManager.ConnectionStrings("Navision").ConnectionString)
            Return RedirectToAction("ok", New With {.year = year, .month = month})
        End If
        Return View(lst)
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
    Function MesAsientoPersona(year As Integer, month As Integer, paga As Integer?, idNomina As List(Of String)) As ActionResult
        Dim cIdAplicacion As Integer = idAplicacion
        Dim lstNominaDeps = db.GetNominaEpsilonPersona(idResumen, year, If(paga, month), idNomina, strCnEpsilon).SelectMany(
            Function(nd)
                Dim t = db.GetCuentasTraspaso(cIdAplicacion, nd.idColumna, nd.idNivel, strCn)
                Dim l As New List(Of Object)
                For Each e In t
                    Dim lantegi = If(e.lantegiCuenta.length > 0, e.lantegiCuenta, e.lantegi)
                    l.Add(New With {nd.idNivel, nd.dNivel, nd.devengo, nd.columna, nd.idColumna, nd.idConvenio,
                                     e.nombreapunte, e.cuenta, lantegi, e.operacion, e.idAsiento, e.nombreAsiento, nd.count, nd.idTrabajador})
                Next
                Return l
            End Function)
        Dim lst = lstNominaDeps.Where(Function(nd) nd IsNot Nothing).OrderBy(Function(nd) nd.cuenta).GroupBy(Function(nd) New With {Key nd.nombreapunte, Key nd.cuenta, Key nd.lantegi, Key nd.idAsiento, Key nd.nombreAsiento, Key nd.idConvenio, Key nd.idNivel, Key nd.dNivel},
                                                                                                Function(k, l)

                                                                                                    Return New With {.key = k, .lst = l, .total = CDec(l.Sum(Function(s)
                                                                                                                                                                 If s.operacion = "suma" Then
                                                                                                                                                                     Return CDec(s.devengo)
                                                                                                                                                                 Else
                                                                                                                                                                     Return -CDec(s.devengo)
                                                                                                                                                                 End If
                                                                                                                                                             End Function)), .count = l.Sum(Function(s) s.count)}
                                                                                                End Function).GroupBy(Function(g) g.key.nombreAsiento, Function(k, l)
                                                                                                                                                           Dim fl = l.Where(Function(s)
                                                                                                                                                                                Return (Regex.IsMatch(k, "la", RegexOptions.IgnoreCase) And s.key.idConvenio = "001") Or (Regex.IsMatch(k, "ss", RegexOptions.IgnoreCase) And (s.key.idConvenio = "002" Or s.key.idConvenio = "006"))
                                                                                                                                                                            End Function)
                                                                                                                                                           Return New With {.key = k, .l = fl, .total = fl.Sum(Function(s) s.total), .count = fl.Sum(Function(s) s.count)}
                                                                                                                                                       End Function).ToList



        ViewData("fregistro") = New Date(year, month, Date.DaysInMonth(year, month)).ToShortDateString
        ViewData("PagosTOtal") = db.GetSumPago(year, month, idNomina, strCnEpsilon)
        Return View(lst)
    End Function
    Function ok(year As Integer, month As Integer) As ActionResult
        Return View()
    End Function
End Class