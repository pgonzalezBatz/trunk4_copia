Public Class ProveedorController
    Inherits System.Web.Mvc.Controller

    Public Enum Searchtype
        standard = 1
        direccion = 2
        codigoPostal = 3
        localidad = 4
        provincia = 5
        pais = 6
    End Enum

    ReadOnly strCnOracle As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString
    ReadOnly strCnGM As String = ConfigurationManager.ConnectionStrings("grupomaterial").ConnectionString
    ReadOnly strCnMicrosoft1 As String = ConfigurationManager.ConnectionStrings("microsoft1").ConnectionString
    ReadOnly strCnMicrosoft2 As String = ConfigurationManager.ConnectionStrings("microsoft2").ConnectionString
    ReadOnly strCnMicrosoftEpsilon As String = ConfigurationManager.ConnectionStrings("microsoftEpsilon").ConnectionString
    ReadOnly strCnRH As String = ConfigurationManager.ConnectionStrings("rh").ConnectionString
    ReadOnly recursoSabPro As Integer = ConfigurationManager.AppSettings("sabprov")
    ReadOnly grupoSabPro As Integer = ConfigurationManager.AppSettings("sabprovgrupo")
    Public Const TakeLimit As Integer = 20
    Public Const skipMin As Integer = 0


    <SimpleRoleProvider(roles.normal)>
    Function Search(idplanta As String, q As String, st As String) As ActionResult
        idplanta = If(idplanta, h.GetidPlanta())
        ViewData("plantaSeleccionada") = idplanta
        Dim plantasActivas = System.Configuration.ConfigurationManager.AppSettings("plantasActivas").Split(";")
        ViewData("idplanta") = Db.GetListOfPlanta(strCnOracle).Select(Function(e)
                                                                          If idplanta = e.Value Then
                                                                              e.Selected = True
                                                                          End If
                                                                          Return e
                                                                      End Function).
                                                                      Where(Function(o) plantasActivas.Contains(o.Value)).OrderBy(Function(o) CInt(o.Value))
        If String.IsNullOrEmpty(q) Then
            Return View()
        End If
        q = q.ToLower
        Dim lst = Db.GetListOfproveedor(idplanta, recursoSabPro, strCnOracle)
        Select Case st
            Case Searchtype.codigoPostal
                lst = lst.Where(Function(p) p.codigoPostal.ToLower.Contains(q))
            Case Searchtype.direccion
                lst = lst.Where(Function(p) p.direccion.ToLower.Contains(q))
            Case Searchtype.localidad
                lst = lst.Where(Function(p) p.localidad.ToLower.Contains(q))
            Case Searchtype.provincia
                lst = lst.Where(Function(p) p.provincia.ToLower.Contains(q))
            Case Searchtype.pais
                lst = lst.Where(Function(p) p.nombrePais.ToLower.Contains(q))
            Case Else
                lst = lst.Where(Function(p) p.nombre.ToLower.Contains(q) Or p.codpro.ToLower.Contains(q) Or p.cif.ToLower.Contains(q) Or p.RazonSocial.ToLower.Contains(q))
        End Select

        ViewBag.count = lst.Count
        lst = lst.Skip(If(Request("skip"), skipMin))
        lst = lst.Take(If(Request("take"), TakeLimit))
        ViewData("user") = Db.GetDatosTrabajador(SimpleRoleProvider.GetId(), strCnOracle, strCnMicrosoftEpsilon)

        If Request.AcceptTypes.Any(Function(a) a.Contains("json")) Then
            Return Json(lst, JsonRequestBehavior.AllowGet)
        End If
        Return View(lst)
    End Function


    <SimpleRoleProvider(roles.normal)>
    Function SearchHomologacion(idplanta As String, q As String, st As String) As ActionResult
        idplanta = If(idplanta, h.GetidPlanta())
        ViewData("plantaSeleccionada") = idplanta
        Dim plantasActivas = System.Configuration.ConfigurationManager.AppSettings("plantasActivas").Split(";")
        ViewData("idplanta") = Db.GetListOfPlanta(strCnOracle).Select(Function(e)
                                                                          If idplanta = e.Value Then
                                                                              e.Selected = True
                                                                          End If
                                                                          Return e
                                                                      End Function).
                                                                      Where(Function(o) plantasActivas.Contains(o.Value)).OrderBy(Function(o) CInt(o.Value))
        If String.IsNullOrEmpty(q) Then
            Return View()
        End If
        q = q.ToLower
        Dim lst = Db.GetListOfproveedorHomologacion(idplanta, recursoSabPro, strCnOracle)
        lst = lst.Where(Function(p) p(0).ToLower.Contains(q) Or p(1).ToLower.Contains(q) Or p(2).ToLower.Contains(q) Or p(3).ToLower.Contains(q) Or p(4).ToLower.Contains(q))

        ViewBag.count = lst.Count
        lst = lst.Skip(If(Request("skip"), skipMin))
        lst = lst.Take(If(Request("take"), TakeLimit))
        ViewData("user") = Db.GetDatosTrabajador(SimpleRoleProvider.GetId(), strCnOracle, strCnMicrosoftEpsilon)

        If Request.AcceptTypes.Any(Function(a) a.Contains("json")) Then
            Return Json(lst, JsonRequestBehavior.AllowGet)
        End If
        Return View(lst)
    End Function

    Function Homologar(ByVal id As Integer, ByVal id2 As String) As ActionResult
        Dim myId As Integer
        Dim isGlobal As Boolean
#If DEBUG Then
        id = 5435
        id2 = "-"
#End If
        isGlobal = Not (String.IsNullOrEmpty(id2) OrElse id2.Equals("-"))
        myId = If(isGlobal, CInt(id2), id)
        ViewBag.isGlobal = isGlobal
        Return View(myId)
    End Function

    Function HomologarItem(ByVal elementId As String, ByVal providerId As Integer, ByVal isGlobal As Boolean, ByVal setChecked As Boolean)
        Db.HomologarItem(elementId, providerId, isGlobal, setChecked, strCnGM)
        ViewBag.isGlobal = isGlobal
        Return View("Homologar", providerId)
    End Function

    Function SetPertenencia(ByVal elementId As String, ByVal providerId As Integer, ByVal isGlobal As Boolean, ByVal setChecked As Boolean)
        Db.SetPertenencia(elementId, providerId, isGlobal, setChecked, strCnGM)
        ViewBag.isGlobal = isGlobal
        Return View("Homologar", providerId)
    End Function

    Function Details(idPlanta As Integer, ByVal id As Integer) As ActionResult
        Dim p = Db.GetProveedor(id, strCnOracle)
        p.tipoProveedorSis = Db.getTipoProveedorSistemas(p.codpro, p.idPlanta, strCnRH, strCnOracle)
        ViewData("adjuntos") = Db.GetAdjuntosProveedor(id, strCnOracle)
        ViewData("ExistsNavision") = Db.ExistProveedorNavision(p.codpro, strCnMicrosoft1)
        Return View(p)
    End Function

    <SimpleRoleProvider(roles.editar)>
    Function Create(p As proveedor) As ActionResult
        'Dim rm As New Resources.ResourceManager(GetType())
        ModelState.Clear()
        If p.moneda = 0 Then : p.moneda = 90 : End If
        If p.fPago = 0 Then : p.fPago = 29 : End If
        ViewBag.monedas = Db.GetListOfMoneda(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codmon, .Text = e.Desmon})
        ViewBag.formaPago = Db.GetListOfFormasPago(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpag, .Text = e.DesPag})
        ViewBag.tipoproveedorsis = Db.GetListOfTipoProveedorSis(1, strCnRH)
        ViewBag.porteTroq = Db.GetListOfPortesTroq()
        'ViewData("pais") = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai, .Selected = e.Codpai = 34})
        ViewBag.pais = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai, .Selected = e.Codpai = 34})
        Return View(p)
    End Function
    <SimpleRoleProvider(roles.editar)>
    <HttpPost()>
    Function Create(nombre As String, ByVal o As proveedor) As ActionResult
        o.idPlanta = h.GetidPlanta()
        If ConfigurationManager.AppSettings("transferencias").Split(";").Contains(o.fPago.ToString()) Then
            ModelState.AddModelError("fPago", "No se puede asignar pago de transferencia hasta que administración tenga el Nº de cuenta introducido en Navision.")
        End If
        If Not String.IsNullOrEmpty(o.cif) AndAlso Db.ExisteCif(o.cif.Trim(" "), o.idPlanta, strCnOracle) Then
            'TODO: Si el cif que existe, es un subcontrata (proveedor de nuestro proveedor) esta relacionado con el tema ADOk
            '      dar la posibilidad de "importar"
            Dim idEmpresa = Db.GetProveedorAdok(o.cif, o.idPlanta, strCnOracle)
            If idEmpresa.HasValue Then
                Return RedirectToAction("EditAdok", New With {.id = idEmpresa, o.idPlanta})
            End If
            ModelState.AddModelError("cif", h.Traducir("El cif proporcionado ya existe en la base de datos"))
        End If
        If Db.ExistUsuarioExtranetParaMismaPlanta(o.nombreUsuario, o.idPlanta, strCnOracle) Then
            ModelState.AddModelError("nombreUsuario", h.Traducir("El nombre de usuario ya existe en la base de datos para esta planta"))
        End If
        If Db.ExisteProv(o.nombre, o.idPlanta, strCnOracle) Then
            ModelState.AddModelError("nombre", h.Traducir("El nombre del proveedor ya existe en la base de datos para esta planta"))
        End If
        If ModelState.IsValid Then
            Dim trab = Db.GetDatosTrabajador(SimpleRoleProvider.GetId(), strCnOracle, strCnMicrosoftEpsilon)
            Dim porSis = 0
            Dim porTroq = 100
            If Not IsNothing(trab) AndAlso trab.n2.contains("sis") Then
                porSis = 100
                porTroq = 0
            ElseIf Not IsNothing(trab) AndAlso trab.n2.contains("Orokorrak") Then
                porSis = 50
                porTroq = 50
            End If
            o.codigoIva = If(o.pais = 34, 5, 3)
            Dim CodigoTrabajadorCreadorParaXBAT = trab.idTrabajador
            Db.Insertproveedor(o, porTroq, porSis, CodigoTrabajadorCreadorParaXBAT, grupoSabPro, strCnOracle, strCnMicrosoft2, strCnMicrosoft1, strCnRH)
            Return RedirectToAction("search", h.ToRouteValues(Request.QueryString, New With {o.idPlanta, .st = 1, .q = o.nombre}))
        End If
        ViewBag.moneda = Db.GetListOfMoneda(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codmon, .Text = e.Desmon})
        ViewBag.formaPago = Db.GetListOfFormasPago(strCnOracle).Where(Function(e) Not e.DesPag.ToLower.Contains("transf")).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpag, .Text = e.DesPag})
        ViewBag.tipoproveedorsis = Db.GetListOfTipoProveedorSis(1, strCnRH)
        ViewBag.porteTroq = Db.GetListOfPortesTroq()
        'ViewData("pais") = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai})
        ViewBag.pais = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai})
        Return View()
    End Function
    <SimpleRoleProvider(roles.editar)>
    Function Edit(id As Integer, ByVal p As proveedor) As ActionResult
        p = Db.GetProveedor(p.id, strCnOracle)
        p.tipoProveedorSis = Db.getTipoProveedorSistemas(p.codpro, p.idPlanta, strCnRH, strCnOracle)
        ModelState.Clear()
        ViewBag.monedas = Db.GetListOfMoneda(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codmon, .Text = e.Desmon, .Selected = p.moneda = e.Codmon})
        ViewBag.formaPago = Db.GetListOfFormasPago(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpag, .Text = e.DesPag, .Selected = e.Codpag = p.fPago})
        ViewBag.tipoproveedorsisLst = Db.GetListOfTipoProveedorSis(1, strCnRH).Select(Function(o)
                                                                                          o.Selected = o.Value = p.tipoProveedorSis
                                                                                          Return o
                                                                                      End Function)
        ViewBag.porteTroq = Db.GetListOfPortesTroq()
        ViewBag.pais = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai, .Selected = e.Codpai = p.pais})
        ViewData("existeBrain") = Db.ExisteProveedorEnBrain(p.codpro, strCnRH)
        Return View(p)
    End Function
    <SimpleRoleProvider(roles.editar)>
    <HttpPost()>
    Function Edit(ByVal p As proveedor) As ActionResult
        If Db.ExistUsuarioExtranetParaMismaPlanta(p.id, p.nombreUsuario, p.idPlanta, strCnOracle) Then
            ModelState.AddModelError("nombreUsuario", h.Traducir("El nombre de usuario ya existe en la base de datos"))
        End If
        If ModelState.IsValid Then
            Dim u = Db.GetDatosTrabajador(SimpleRoleProvider.GetId(), strCnOracle, strCnMicrosoftEpsilon)
            Db.Updateproveedor(p, u.nombre.ToString.Trim(" ") + " " + u.apellido1.ToString.Trim(" ") + " " + u.apellido2.ToString.Trim(" "), recursoSabPro, strCnOracle, strCnRH, strCnMicrosoft1, strCnMicrosoft2)
            Return RedirectToAction("search", h.ToRouteValues(Request.QueryString, Nothing))
        End If
        ViewBag.monedas = Db.GetListOfMoneda(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codmon, .Text = e.Desmon, .Selected = p.moneda = e.Codmon})
        ViewBag.formaPago = Db.GetListOfFormasPago(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpag, .Text = e.DesPag, .Selected = e.Codpag = p.fPago})
        ViewBag.tipoproveedorsis = Db.GetListOfTipoProveedorSis(1, strCnRH)
        ViewBag.porteTroq = Db.GetListOfPortesTroq()
        ViewBag.pais = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai})
        ViewData("existeBrain") = Db.ExisteProveedorEnBrain(p.codpro, strCnRH)
        Return View(p)
    End Function
    <SimpleRoleProvider(roles.editar)>
    Function EditAdok(id As Integer, ByVal p As proveedor) As ActionResult
        p = Db.GetProveedor(id, strCnOracle)
        ModelState.Clear()
        ViewBag.monedas = Db.GetListOfMoneda(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codmon, .Text = e.Desmon, .Selected = p.moneda = e.Codmon})
        ViewBag.formaPago = Db.GetListOfFormasPago(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpag, .Text = e.DesPag, .Selected = e.Codpag = p.fPago})
        ViewBag.tipoproveedorsis = Db.GetListOfTipoProveedorSis(1, strCnRH)
        ViewBag.porteTroq = Db.GetListOfPortesTroq()
        ViewBag.pais = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai})
        ModelState.AddModelError("cif", h.Traducir("El CIF introducido ya existe en el sistema como proveedor del ADOK. Rellena los datos correspondientes para convertirlo a proveedor habitual."))
        Return View("edit", p)
    End Function
    <SimpleRoleProvider(roles.editar)>
    <HttpPost()>
    Function EditAdok(ByVal p As proveedor) As ActionResult
        If ModelState.IsValid Then
            Dim u = Db.GetDatosTrabajador(SimpleRoleProvider.GetId(), strCnOracle, strCnMicrosoftEpsilon)
            Dim trab = Db.GetDatosTrabajador(SimpleRoleProvider.GetId(), strCnOracle, strCnMicrosoftEpsilon)
            Dim porSis = 0
            Dim porTroq = 100
            If Not IsNothing(trab) AndAlso trab.n2.contains("sis") Then
                porSis = 100
                porTroq = 0
            ElseIf Not IsNothing(trab) AndAlso trab.n2.contains("Orokorrak") Then
                porSis = 50
                porTroq = 50
            End If
            p.codigoIva = If(p.pais = 34, 5, 3)
            Db.UpdateAdokToproveedor(p, porTroq, porSis, trab.idTrabajador, u.nombre.ToString.Trim(" ") + " " + u.apellido1.ToString.Trim(" ") + " " + u.apellido2.ToString.Trim(" "), recursoSabPro, strCnOracle, strCnRH, strCnMicrosoft1, strCnMicrosoft2)
            Return RedirectToAction("search", h.ToRouteValues(Request.QueryString, Nothing))
        End If
        ViewBag.monedas = Db.GetListOfMoneda(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codmon, .Text = e.Desmon, .Selected = p.moneda = e.Codmon})
        ViewBag.formaPago = Db.GetListOfFormasPago(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpag, .Text = e.DesPag, .Selected = e.Codpag = p.fPago})
        ViewBag.tipoproveedorsis = Db.GetListOfTipoProveedorSis(1, strCnRH)
        ViewBag.porteTroq = Db.GetListOfPortesTroq()
        ViewBag.pais = Db.GetListOfPais(strCnOracle).Select(Of SelectListItem)(Function(e) New SelectListItem With {.Value = e.Codpai, .Text = e.Nompai})
        If p.moneda = 0 Then : p.moneda = 90 : End If
        If p.fPago = 0 Then : p.fPago = 29 : End If
        Return View("create", p)
    End Function
    '<SimpleRoleProvider(roles.cambiarCIF)>
    'Function ImportNavision(p As proveedor) As ActionResult
    '    p = db.GetProveedor(p.idPlanta, p.id, strCnOracle)
    '    Dim trab = db.GetDatosTrabajador(SimpleRoleProvider.GetId(), strCnOracle, strCnMicrosoftEpsilon)
    '    Dim porSis = 0
    '    Dim porTroq = 100
    '    If Not IsNothing(trab) AndAlso trab.n2.contains("sis") Then
    '        porSis = 100
    '        porTroq = 0
    '    ElseIf Not IsNothing(trab) AndAlso trab.n2.contains("Orokorrak") Then
    '        porSis = 50
    '        porTroq = 50
    '    End If
    '    db.importarNavision(p, porTroq, porSis, trab.nombre + " " + trab.apellido1 + " " + trab.apellido2, strCnMicrosoft1)
    '    Return RedirectToAction("details", h.ToRouteValues(Request.QueryString, Nothing))
    'End Function
    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    Function SearchPotenciales(q As String)
        If String.IsNullOrEmpty(q) Then
            Return View()
        End If
        q = q.ToLower
        Return View(Db.GetListOfProveedorPotencialXbat(strCnOracle).Where(Function(p) p.nombre.ToLower.Contains(q)))
    End Function
    <SimpleRoleProvider(roles.editar)>
    Function CreateProveedorBrain(p As proveedor) As ActionResult
        p = Db.GetProveedor(p.id, strCnOracle)
        Db.CreateProveedorBrain(p, strCnOracle, strCnRH)
        Return RedirectToAction("edit", h.ToRouteValues(Request.QueryString, Nothing))
    End Function
    <SimpleRoleProvider(roles.normal)>
    Function Exportsistemas(p As proveedor) As ActionResult
        ModelState.Values.ToList.ForEach(Sub(e) e.Errors.Clear())
        p = Db.GetProveedor(p.id, strCnOracle)
        Return View(Db.GetEmpresasRH(p.cif, strCnRH))
    End Function
    <SimpleRoleProvider(roles.normal)>
    <HttpPost()>
    Function Exportsistemas(p As proveedor, idPlanta As String, idPlantaDestino As String) As ActionResult
        p = Db.GetProveedor(p.id, strCnOracle)
        ModelState.Values.ToList.ForEach(Sub(e) e.Errors.Clear())
        If String.IsNullOrEmpty(idPlantaDestino) Then
            ModelState.AddModelError("idPlantaDestino", h.Traducir("Es obligatorio introducir planta"))
        End If
        If ModelState.IsValid Then
            Db.ExportarProveedorAPlantasSistemas(p, idPlantaDestino, strCnRH)
            Return RedirectToAction("exportsistemas", h.ToRouteValues(Request.QueryString, Nothing))
        End If
        Return View(Db.GetEmpresasRH(p.cif, strCnRH))
    End Function
    <SimpleRoleProvider(roles.editar, roles.editarRecursos)>
    Function Listrecursos(id As Integer?, seleccionPlanta As String) As ActionResult
        Dim l = New VMRecursosUsuarioEmpresaYSAB With {
            .UsuarioEmpresa = Db.GetNombreUsuarioEmpresa(id, strCnOracle),
            .usuarioSabProveedores = Db.GetNombreUsuarioSabProveedor(id, grupoSabPro, strCnOracle),
            .ListOfUsuarioEmpresa = Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).GroupBy(Function(r) r.Area, Function(k, l2)
                                                                                                                                        Return New VMAreaRecursoEdit With {.NombreArea = k,
                                                                                                                                          .ListOfRecurso = l2}
                                                                                                                                    End Function),
            .ListOfUsuarioSABProveedor = Db.GetListOfRecursosUsuarioAdministrador(grupoSabPro, id, "es-ES", strCnOracle).GroupBy(Function(r) r.Area, Function(k, l2)
                                                                                                                                                         Return New VMAreaRecursoEdit With {.NombreArea = k,
                                                                                                                                          .ListOfRecurso = l2}
                                                                                                                                                     End Function)
            }
        Return View(l)
    End Function
    <SimpleRoleProvider(roles.editar, roles.editarRecursos)>
    <HttpPost()>
    Function UpdaterecursosUsuarioEmpresa(id As Integer?, seleccionPlanta As String, ListOfUsuarioEmpresa As IEnumerable(Of VMAreaRecursoEdit)) As ActionResult
        Dim lstrecurso = ListOfUsuarioEmpresa.SelectMany(Function(e) e.ListOfRecurso).Where(Function(e) e.Seleccionado).Select(Function(e) e.Grupo)
        Dim hsOldRecursos = New HashSet(Of Integer)(Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado And o.Grupo <> grupoSabPro).Select(Function(o) o.Grupo))
        Dim hsNewRecursos = New HashSet(Of Integer)(lstrecurso)
        Db.UpdateRecursosEmpresa(id, grupoSabPro, lstrecurso, strCnOracle)
        Dim pro = Db.GetProveedor(id, strCnOracle)
        If lstrecurso IsNot Nothing AndAlso lstrecurso.Count > 0 AndAlso Not pro.Notificado Then
            Dim newPwd = Membership.GeneratePassword(10, 0)
            Db.UpdatePassword(pro.id, newPwd, strCnOracle)
            Dim strBody As New StringBuilder()
            strBody.Append("Hornitzaile agurgarria,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
            strBody.Append("Honen bitartez zera jakinarazten dizuegu,  hornitzaileen portal berria erabilgarri duzuela ondorengo helbidean: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
            strBody.Append("Zure sarbide-gakoak hauexek dira:") : strBody.Append(Environment.NewLine)
            strBody.Append("     Erabiltzailea: ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
            strBody.Append("     Pasahitza:     ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
            strBody.Append("Segurtasuna dela eta, Batzek lehenetsitako pasahitza aldatzea gomendatzen dizue. Pasahitzak ez dira iraungitzen.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
            strBody.Append("Adeitasunez, Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

            strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

            strBody.Append("Estimado proveedor,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
            strBody.Append("Con la presente se le informa de que la nueva Portal de Proveedores de BATZ esta disponible para su uso en la dirección: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
            strBody.Append("Sus credenciales de acceso son las siguientes:") : strBody.Append(Environment.NewLine)
            strBody.Append("     Usuario:   ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
            strBody.Append("     Clave:     ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
            strBody.Append("Por criterios de seguridad, Batz recomienda cambiar la clave que se le ofrece por defecto. Dicha clave no caduca.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
            strBody.Append("Atentamente Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

            strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

            strBody.Append("Dear Supplier,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
            strBody.Append("It is our pleasure to inform you that with a view to facilitating the work of its suppliers, Batz has implemented its BATZ Supplier Portal, which is accesible on the following address: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
            strBody.Append("Your access details are as follows:") : strBody.Append(Environment.NewLine)
            strBody.Append("     User:       ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
            strBody.Append("     Password:   ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
            strBody.Append("For security purposes, Batz suggests you to change the password upon the first login. The password doesn´t expire.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
            strBody.Append("King regards Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
            SendEmail(pro.email, "Batz S.Coop. Extranet", strBody.ToString)
            Db.updateNotificado(pro, strCnOracle)
        Else
            hsNewRecursos.SymmetricExceptWith(hsOldRecursos)
            If hsNewRecursos.Count > 0 Then
                'Hay alguna diferencia entre los recursos. Notificar
                Dim strBody As New StringBuilder()
                strBody.Append("Hornitzaile agurgarria,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("E-mail honen bitartez jakinarazten dizugu, BATZ-eko extranetaren barruan zenuen errekutso zerrenda aldatu dela. Gaur egunean, ondorengo errekurtsoetara sartzeko baimena daukazu:") : strBody.Append(Environment.NewLine)
                For Each r In Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado)
                    strBody.Append("   -  ") : strBody.Append(r.NombreGrupo) : strBody.Append(Environment.NewLine)
                Next
                strBody.Append(Environment.NewLine)
                strBody.Append("Sartu BATZ-eko extranet-ean ondorengo linka erabilita eta konproba ezazu. https://extranet.batz.es") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                strBody.Append("Estimado proveedor,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("Mediante este e-mail le comunico que ha sido modificado el listado de recursos a los que usted tiene acceso dentro de la extranet de BATZ. Actualmente dispone de acceso para los siguientes recursos:") : strBody.Append(Environment.NewLine)
                For Each r In Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado)
                    strBody.Append("   -  ") : strBody.Append(r.NombreGrupo) : strBody.Append(Environment.NewLine)
                Next
                strBody.Append(Environment.NewLine)
                strBody.Append("Acceda a la extranet de BATZ mediante el siguiente enlace y compruébelo. https://extranet.batz.es") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                strBody.Append("Dear Supplier,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("Hereby, we inform you that the list of resources you have access to has been modified. You currently have access to the following resources:") : strBody.Append(Environment.NewLine)
                For Each r In Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado)
                    strBody.Append("   -  ") : strBody.Append(r.NombreGrupo) : strBody.Append(Environment.NewLine)
                Next
                strBody.Append(Environment.NewLine)
                strBody.Append("Enter to the Batz extranet using the following link and check it out. https://extranet.batz.es") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                SendEmail(pro.email, "Batz S.Coop. Extranet", strBody.ToString)
            End If
        End If

        Return RedirectToAction("listrecursos", h.ToRouteValues(Request.QueryString, Nothing))
    End Function
    <SimpleRoleProvider(roles.editar, roles.editarRecursos)>
    <HttpPost()>
    Function UpdaterecursosSabProveedor(id As Integer?, seleccionPlanta As String, ListOfUsuarioSABProveedor As IEnumerable(Of VMAreaRecursoEdit)) As ActionResult
        If ModelState.IsValid Then
            Dim lstrecurso = ListOfUsuarioSABProveedor.SelectMany(Function(e) e.ListOfRecurso).Where(Function(e) e.Seleccionado).Select(Function(e) e.Grupo)
            Db.UpdateRecursosSabProveedor(id, grupoSabPro, lstrecurso, strCnOracle)
            Dim pro = Db.GetProveedor(id, strCnOracle)
            If lstrecurso IsNot Nothing AndAlso lstrecurso.Count > 0 AndAlso Not pro.Notificado Then
                Dim newPwd = Membership.GeneratePassword(10, 0)
                Db.UpdatePassword(pro.id, newPwd, strCnOracle)
                Dim strBody As New StringBuilder()
                strBody.Append("Hornitzaile agurgarria,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("Honen bitartez zera jakinarazten dizuegu,  hornitzaileen portal berria erabilgarri duzuela ondorengo helbidean: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
                strBody.Append("Zure sarbide-gakoak hauexek dira:") : strBody.Append(Environment.NewLine)
                strBody.Append("     Erabiltzailea: ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
                strBody.Append("     Pasahitza:     ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
                strBody.Append("Segurtasuna dela eta, Batzek lehenetsitako pasahitza aldatzea gomendatzen dizue. Pasahitzak ez dira iraungitzen.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("Adeitasunez, Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                strBody.Append("Estimado proveedor,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("Con la presente se le informa de que la nueva Portal de Proveedores de BATZ esta disponible para su uso en la dirección: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
                strBody.Append("Sus credenciales de acceso son las siguientes:") : strBody.Append(Environment.NewLine)
                strBody.Append("     Usuario:   ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
                strBody.Append("     Clave:     ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
                strBody.Append("Por criterios de seguridad, Batz recomienda cambiar la clave que se le ofrece por defecto. Dicha clave no caduca.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("Atentamente Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                strBody.Append("Dear Supplier,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("It is our pleasure to inform you that with a view to facilitating the work of its suppliers, Batz has implemented its BATZ Supplier Portal, which is accesible on the following address: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
                strBody.Append("Your access details are as follows:") : strBody.Append(Environment.NewLine)
                strBody.Append("     User:       ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
                strBody.Append("     Password:   ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
                strBody.Append("For security purposes, Batz suggests you to change the password upon the first login. The password doesn´t expire.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                strBody.Append("King regards Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                SendEmail(pro.email, "Batz S.Coop. Extranet", strBody.ToString)
                Db.updateNotificado(pro, strCnOracle)
            Else
                Dim hsOldRecursos = New HashSet(Of Integer)(Db.GetListOfRecursosUsuarioAdministrador(grupoSabPro, id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado And o.Grupo <> CInt(ConfigurationManager.AppSettings("sabprovgrupo"))).Select(Function(o) o.Grupo))
                Dim hsNewRecursos = New HashSet(Of Integer)(lstrecurso)
                hsNewRecursos.SymmetricExceptWith(hsOldRecursos)
                If hsNewRecursos.Count > 0 Then
                    'Hay alguna diferencia entre los recursos. Notificar
                    Dim strBody As New StringBuilder()
                    strBody.Append("Hornitzaile agurgarria,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                    strBody.Append("E-mail honen bitartez jakinarazten dizugu, BATZ-eko extranetaren barruan zenuen errekutso zerrenda aldatu dela. Gaur egunean, ondorengo errekurtsoetara sartzeko baimena daukazu:") : strBody.Append(Environment.NewLine)
                    For Each r In Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado)
                        strBody.Append("   -  ") : strBody.Append(r.NombreGrupo) : strBody.Append(Environment.NewLine)
                    Next
                    strBody.Append(Environment.NewLine)
                    strBody.Append("Sartu BATZ-eko extranet-ean ondorengo linka erabilita eta konproba ezazu. https://extranet.batz.es") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                    strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                    strBody.Append("Estimado proveedor,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                    strBody.Append("Mediante este e-mail le comunico que ha sido modificado el listado de recursos a los que usted tiene acceso dentro de la extranet de BATZ. Actualmente dispone de acceso para los siguientes recursos:") : strBody.Append(Environment.NewLine)
                    For Each r In Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado)
                        strBody.Append("   -  ") : strBody.Append(r.NombreGrupo) : strBody.Append(Environment.NewLine)
                    Next
                    strBody.Append(Environment.NewLine)
                    strBody.Append("Acceda a la extranet de BATZ mediante el siguiente enlace y compruébelo. https://extranet.batz.es") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                    strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                    strBody.Append("Dear Supplier,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
                    strBody.Append("Hereby, we inform you that the list of resources you have access to has been modified. You currently have access to the following resources:") : strBody.Append(Environment.NewLine)
                    For Each r In Db.getListOfRecursosUsuarioEmpresa(id, h.GetCulture(), strCnOracle).Where(Function(o) o.Seleccionado)
                        strBody.Append("   -  ") : strBody.Append(r.NombreGrupo) : strBody.Append(Environment.NewLine)
                    Next
                    strBody.Append(Environment.NewLine)
                    strBody.Append("Enter to the Batz extranet using the following link and check it out. https://extranet.batz.es") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

                    SendEmail(pro.email, "Batz S.Coop. Extranet", strBody.ToString)
                End If
            End If
        End If

        Return RedirectToAction("listrecursos", h.ToRouteValues(Request.QueryString, Nothing))
    End Function
    <SimpleRoleProvider(roles.editar, roles.editarRecursos)>
    Function Renotificar(id As Integer)
        Return View()
    End Function
    <SimpleRoleProvider(roles.editar, roles.editarRecursos)>
    <HttpPost()>
    Function RenotificarPost(id As Integer)
        Dim pro = Db.GetProveedor(id, strCnOracle)
        Dim newPwd = Membership.GeneratePassword(10, 0)
        Db.UpdatePassword(id, newPwd, strCnOracle)
        Dim strBody As New StringBuilder()
        strBody.Append("Hornitzaile agurgarria,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
        strBody.Append("Honen bitartez zera jakinarazten dizuegu,  hornitzaileen portal berria erabilgarri duzuela ondorengo helbidean: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
        strBody.Append("Zure sarbide-gakoak hauexek dira:") : strBody.Append(Environment.NewLine)
        strBody.Append("     Erabiltzailea: ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
        strBody.Append("     Pasahitza:     ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
        strBody.Append("Segurtasuna dela eta, Batzek lehenetsitako pasahitza aldatzea gomendatzen dizue. Pasahitzak ez dira iraungitzen.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
        strBody.Append("Adeitasunez, Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

        strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

        strBody.Append("Estimado proveedor,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
        strBody.Append("Con la presente se le informa de que la nueva Portal de Proveedores de BATZ esta disponible para su uso en la dirección: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
        strBody.Append("Sus credenciales de acceso son las siguientes:") : strBody.Append(Environment.NewLine)
        strBody.Append("     Usuario:   ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
        strBody.Append("     Clave:     ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
        strBody.Append("Por criterios de seguridad, Batz recomienda cambiar la clave que se le ofrece por defecto. Dicha clave no caduca.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
        strBody.Append("Atentamente Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

        strBody.Append("==================================================") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)

        strBody.Append("Dear Supplier,") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
        strBody.Append("It is our pleasure to inform you that with a view to facilitating the work of its suppliers, Batz has implemented its BATZ Supplier Portal, which is accesible on the following address: https://extranet.batz.es") : strBody.Append(Environment.NewLine)
        strBody.Append("Your access details are as follows:") : strBody.Append(Environment.NewLine)
        strBody.Append("     User:       ") : strBody.Append(pro.nombreUsuario) : strBody.Append(Environment.NewLine)
        strBody.Append("     Password:   ") : strBody.Append(newPwd) : strBody.Append(Environment.NewLine)
        strBody.Append("For security purposes, Batz suggests you to change the password upon the first login. The password doesn´t expire.") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
        strBody.Append("King regards Batz Group") : strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine)
        SendEmail(pro.email, "Batz S.Coop. Extranet", strBody.ToString)
        Return RedirectToAction("listrecursos", h.ToRouteValues(Request.QueryString, Nothing))
    End Function
    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    Function Listcapacidades(p As proveedor) As ActionResult
        Return View("listcapacidades", Db.GetCapacidades(p, strCnOracle))
    End Function

    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    <HttpPost()>
    Function UpdateCapacidades(p As proveedor, capacidades As List(Of String))
        Db.updateCapacidades(p, capacidades, strCnOracle)
        Return RedirectToAction("listcapacidades", h.ToRouteValues(Request.QueryString, Nothing))
    End Function

    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    <HttpPost()>
    Function nuevacapacidad(nombre As String) As ActionResult
        Dim cap = Db.GetCapacidad(nombre, strCnOracle)
        If cap IsNot Nothing Then
            ModelState.AddModelError("nombre", h.Traducir("Esta capacidad ya está creada"))
        Else
            Db.CreateCapacidad(nombre, strCnOracle)
            Return RedirectToAction("listcapacidades", h.ToRouteValues(Request.QueryString, Nothing))
        End If
        Return View()
    End Function

    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    Function nuevacapacidad() As ActionResult
        Return View("nuevacapacidad")
    End Function

    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    Function editarcapacidad(ByVal idCapacidad As String)
        Dim cap = Db.GetCapacidadFromId(idCapacidad, strCnOracle)
        Return View(cap)
    End Function

    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    <HttpPost()>
    Function editarcapacidad(c As Capacidad)
        Db.EditarCapacidad(c, strCnOracle)
        Return RedirectToAction("search", h.ToRouteValues(Request.QueryString, Nothing))
    End Function

    <SimpleRoleProvider(roles.AdministrarPotencialesYCapacidades)>
    Function borrarcapacidad(idCapacidad As String)
        Db.EliminarCapacidad(idCapacidad, strCnOracle)
        Return RedirectToAction("search", h.ToRouteValues(Request.QueryString, Nothing))
    End Function


    <SimpleRoleProvider(roles.normal)>
    Function Listadjunto(p As proveedor) As ActionResult
        Return View(Db.GetAdjuntosProveedor(p.id, strCnOracle))
    End Function
    <SimpleRoleProvider(roles.normal)>
    Function Adjunto(idEmpresa As Integer, ByVal idAdjunto As Integer) As FileResult
        Dim a = Db.GetAdjuntosProveedor(idEmpresa, strCnOracle).Where(Function(o) o.id = idAdjunto).First
        Return New FileStreamResult(New IO.MemoryStream(a.adjunto, False), a.mime) With {.FileDownloadName = a.nombre}
    End Function
    <HttpPost()>
    <SimpleRoleProvider(roles.homologaciones)>
    Function Adjuntar(id As Integer, ByVal adjunto As HttpPostedFileBase) As ActionResult
        If adjunto.FileName.Length = 0 Then
            ModelState.AddModelError("error", h.Traducir("El archivo que se esta intentando adjuntar no se encuentra"))
        End If
        If ModelState.IsValid Then
            Dim fileData(adjunto.ContentLength) As Byte
            Dim fileLength = adjunto.ContentLength
            adjunto.InputStream.Read(fileData, 0, fileLength)
            Db.InsertAdjunto(id, fileData, IO.Path.GetFileName(adjunto.FileName), adjunto.ContentType, strCnOracle)
        End If
        Return RedirectToAction("listadjunto", h.ToRouteValues(Request.QueryString, Nothing))
    End Function
    <HttpPost()>
    <SimpleRoleProvider(roles.homologaciones)>
    Function Eliminaradjunto(idadjunto As Integer)
        Db.deleteAdjunto(idadjunto, strCnOracle)
        Return RedirectToAction("listadjunto", h.ToRouteValuesDelete(Request.QueryString, "idaadjunto"))
    End Function
    <SimpleRoleProvider(roles.editar, roles.editarRecursos)>
    Public Function ListUsuarios(id As Integer) As ActionResult
        'ViewData("IdEmpresa") = id
        Return View(Db.GetUsuariosProveedor(id, strCnOracle))
    End Function

    Function ClearCache() As ActionResult
        Runtime.Caching.MemoryCache.Default.Remove("lstproveedor" + h.GetidPlanta().ToString)
        Return RedirectToAction("search")
    End Function
    Function ListLogs(p As proveedor) As ActionResult
        Return View(Db.GetListOfProveedorLogs(p.id, strCnOracle))
    End Function
    <SimpleRoleProvider(roles.SinUsuarioSAB, roles.normal, roles.homologaciones, roles.editar, roles.cambiarCIF, roles.AdministrarPotencialesYCapacidades, roles.telefonosdirectos, roles.editarSabPro, roles.editarRecursos)>
    Function JSONObtenerdatosProveedor(id As Integer?) As JsonResult
        If id.HasValue Then
            Return Json(Db.GetProveedor(id, strCnOracle), JsonRequestBehavior.AllowGet)
        End If
        Return Json(Nothing, JsonRequestBehavior.AllowGet)
    End Function

    Protected Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage With {
            .Subject = subject,
            .Body = body,
            .From = New Net.Mail.MailAddress("noreply@batz.es")
        }
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient("posta.batz.com")
        smtp.Send(b)
    End Sub


End Class