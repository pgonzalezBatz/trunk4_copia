Imports System.Security.Permissions
Namespace web
    Public Class cuentaController
        Inherits System.Web.Mvc.Controller

        Dim strCn As String = ConfigurationManager.ConnectionStrings("navsilon").ConnectionString

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listAplicacion() As ActionResult
            Return View(db.GetListOfAplicacion(strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listTipoCuenta(idAplicacion As Integer) As ActionResult
            Return View(db.GetListOfTipoCuenta(idAplicacion, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listCuenta(idAplicacion As Integer, idTipoCuenta As Integer) As ActionResult
            Return View(db.GetListOfCuenta(idTipoCuenta, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listDepartamentosCuenta(idAplicacion As Integer, idTipoCuenta As Integer) As ActionResult
            Dim lsttipoCuenta = db.GetListOfTipoCuenta(idAplicacion, strCn).Where(Function(e) e.id = idTipoCuenta)
            ViewData("lsttipocuenta") = lsttipoCuenta
            Return View("listDepartamentoscuentas", db.GetListOfNegocioAdministracion(strCn).Select(
                        Function(n)
                            Dim lstMOA = db.GetListOfManoObraAdministracion(strCn).Select(
                                    Function(mo)
                                        Dim lstCD = db.GetListOfCuentasDepartamentos(strCn)
                                        Return New With {.nombreManoObra = mo.nombre, .listOfDepartamento = db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto).Select(
                                                Function(d)
                                                    Return New With {.idDepartamento = d.id, .nombredepartamento = d.nombre, .listofcuentacuenta = lsttipoCuenta.Select(
                                                            Function(tp)
                                                                Return db.GetListOfCuenta(tp.id, strCn).Select(Function(c) New With {.id = c.id, .nombre = c.nombre, .descripcion = c.descripcion, .selected = lstCD.Exists(Function(z) z.iddepartamento = d.id AndAlso tp.id = z.idTipoCuenta AndAlso c.id = z.idCuenta)})
                                                            End Function).ToList}
                                                End Function).ToList}
                                    End Function).ToList
                            Return New With {.nombreNegocio = n.nombre, .rowspan = lstMOA.Sum(Function(mo) mo.listOfDepartamento.Count), .listOfManoObra = lstMOA}
                        End Function).ToList)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function listDepartamentosCuenta(idAplicacion As Integer, idTipoCuenta As Integer, lstCuentaDepartamento As List(Of String)) As ActionResult
            lstCuentaDepartamento = lstCuentaDepartamento.FindAll(Function(e) e <> "")
            If lstCuentaDepartamento Is Nothing Then
                ModelState.AddModelError("lstMapeo", "Es necesario seleccionar algun elemento")
            End If
            If ModelState.IsValid Then
                db.UpdateCuentaDepartamento(idAplicacion, idTipoCuenta, lstCuentaDepartamento.Select(
                                                     Function(e)
                                                         Dim s = Split(e, ",")
                                                         Return New With {.idAplicacion = idAplicacion, .idDepartamento = s(0), .idCuenta = s(1)}
                                                     End Function).ToList(), strCn)
            End If
            Return listDepartamentosCuenta(idAplicacion, idTipoCuenta)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listDepartamentosCuentas(idAplicacion As Integer) As ActionResult
            Dim lstTipoCuenta = db.GetListOfTipoCuenta(idAplicacion, strCn)
            ViewData("lsttipocuenta") = lstTipoCuenta
            Return View(db.GetListOfNegocioAdministracion(strCn).Select(
                        Function(n)
                            Dim lstMOA = db.GetListOfManoObraAdministracion(strCn).Select(
                                    Function(mo)
                                        Dim lstCD = db.GetListOfCuentasDepartamentos(strCn)
                                        Return New With {.nombreManoObra = mo.nombre, .listOfDepartamento = db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto).Select(
                                                Function(d)
                                                    Return New With {.idDepartamento = d.id, .nombredepartamento = d.nombre, .listofcuentacuenta = lstTipoCuenta.Select(
                                                            Function(tp)
                                                                Return db.GetListOfCuenta(tp.id, strCn).Select(Function(c) New With {.id = c.id, .nombre = c.nombre, .descripcion = c.descripcion, .selected = lstCD.Exists(Function(z) z.iddepartamento = d.id AndAlso tp.id = z.idTipoCuenta AndAlso c.id = z.idCuenta)})
                                                            End Function).ToList}
                                                End Function).ToList}
                                    End Function).ToList
                            Return New With {.nombreNegocio = n.nombre, .rowspan = lstMOA.Sum(Function(mo) mo.listOfDepartamento.Count), .listOfManoObra = lstMOA}
                        End Function).ToList)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function asignarCuentas(idAplicacion As Integer, idDepartamento As Integer) As ActionResult
            Dim lstCuentas = db.GetListOfCuentasDepartamentos(strCn).Where(Function(e) e.idDepartamento = idDepartamento).ToList
            ViewData("lstcuentas") = lstCuentas
            ViewData("lstnocuentas") = db.GetListOfTipoCuenta(idAplicacion, strCn).Select(
                Function(tp)
                    Return New With {.nombreTipoCuenta = tp.nombre, .listOfCuenta = db.GetListOfCuenta(tp.id, strCn).Where(
                            Function(c)
                                Return Not lstCuentas.Exists(Function(c2) c2.idCuenta = c.id)
                            End Function)}
                End Function).ToList
            Return View(db.GetDepartamentosAdministracion(idDepartamento, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function addCuenta(idAplicacion As Integer, idDepartamento As Integer, idCuenta As Integer) As ActionResult
            db.AddCuentaDepartamento(idDepartamento, idCuenta, strCn)
            Return RedirectToAction("asignarCuentas", New With {.idAplicacion = idAplicacion, .idDepartamento = idDepartamento})
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function removeCuenta(idAplicacion As Integer, idDepartamento As Integer, idCuenta As Integer) As ActionResult
            db.RemoveCuentaDepartamento(idDepartamento, idCuenta, strCn)
            Return RedirectToAction("asignarCuentas", New With {.idAplicacion = idAplicacion, .idDepartamento = idDepartamento})
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function createAplicacion() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function createAplicacion(nombre As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre de la aplicación")
            End If
            If ModelState.IsValid Then
                db.AddAplicacion(nombre, strCn)
                Return RedirectToAction("listaplicacion")
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function createTipoCuenta() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function createTipoCuenta(idAplicacion As Integer, nombre As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre del tipo cuenta")
            End If
            If ModelState.IsValid Then
                db.AddTipoCuenta(idAplicacion, nombre, strCn)
                Return RedirectToAction("listtipocuenta", New With {.idAplicacion = idAplicacion})
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function createAsiento(idAplicacion As Integer) As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
       <AcceptVerbs(HttpVerbs.Post)> _
        Function createAsiento(idAplicacion As Integer, nombre As String, sumaresta As String) As ActionResult
            If nombre.Length = 0 Then
                ModelState.AddModelError("nombre", "El nombre del asiento es obligatorio")
            End If
            If ModelState.IsValid Then
                db.AddAsiento(idAplicacion, nombre, strCn)
                Return RedirectToAction("listtipocuenta", New With {.idAplicacion = idAplicacion})
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function createCuenta() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function createCuenta(idTipoCuenta As Integer, nombre As String, descripcion As String, lantegi As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre de la cuenta")
            End If
            If ModelState.IsValid Then
                db.AddCuenta(idTipoCuenta, nombre, descripcion, lantegi, strCn)
                Return RedirectToAction("listcuenta", h.ToRouteValues(Request.QueryString, Nothing))
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function editCuenta(idTipoCuenta As Integer, idCuenta As Integer) As ActionResult
            Return View("createCuenta", db.GetCuenta(idCuenta, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function editCuenta(idTipoCuenta As Integer, idCuenta As Integer, nombre As String, descripcion As String, lantegi As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre de la cuenta")
            End If
            If ModelState.IsValid Then
                db.EditCuenta(idCuenta, nombre, descripcion, lantegi, strCn)
                Return RedirectToAction("listcuenta", h.ToRouteValuesDelete(Request.QueryString, "idcuenta"))
            End If
            Return View("createCuenta", New With {.nombre = nombre, .descripcion = descripcion})
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listComposicionAsiento(idAplicacion As Integer, idAsiento As Integer) As ActionResult
            ViewData("cuentaslibre") = db.GetListOfTipoCuentaNoEnAsiento(idAplicacion, idAsiento, strCn)
            ViewData("cuentasnolibres") = db.GetListOfTipoCuentaEnAsiento(idAplicacion, idAsiento, strCn)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function listComposicionAsiento(idAplicacion As Integer, idAsiento As Integer, idtipocuenta As Integer, add As String, subtract As String, delete As String) As ActionResult
            If add IsNot Nothing Then
                db.AddTipoCuentaAAsiento(idAplicacion, idAsiento, idtipocuenta, "suma", strCn)
            End If
            If subtract IsNot Nothing Then
                db.AddTipoCuentaAAsiento(idAplicacion, idAsiento, idtipocuenta, "resta", strCn)
            End If
            If delete IsNot Nothing Then
                db.RemoveTipoCuentaAAsiento(idAplicacion, idAsiento, idtipocuenta, strCn)
            End If
            Return RedirectToAction("listComposicionAsiento", New With {.idAplicacion = idAplicacion, .idAsiento = idAsiento})
        End Function
    End Class
End Namespace