Public Class asientoController
    Inherits System.Web.Mvc.Controller

    Dim strCnEpsilon As String = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
    Dim strCn As String = ConfigurationManager.ConnectionStrings("navsilon").ConnectionString

    Function listEjercicioFecha() As ActionResult
        Dim lst = Enumerable.Range(0, 13).Select(Function(i As Integer)
                                                     Dim d As New Date(Now.Year, Now.Month, 1)
                                                     Return d.AddMonths(-i)
                                                 End Function)

        Return View(lst)
    End Function
    Function listaplicacion(year As Integer, month As Integer) As ActionResult
        Return View(db.GetListOfAplicacion(strCn))
    End Function
    Function listasiento(year As Integer, month As Integer, idaplicacion As Integer) As ActionResult
        Return View(db.GetListOfAsiento(idaplicacion, strCn))
    End Function
    Function volcado(year As Integer, month As Integer, idaplicacion As Integer, idasiento As Integer) As ActionResult
        Dim lst As New List(Of Object)
        For Each a In db.GetListOfTipoCuentaEnAsiento(idaplicacion, idasiento, strCn)
            Dim p = GetResultadoResumen(year, month, m.GetResumenConveno(a.id), a.id)
            lst.Add(p)
        Next
        Return View(lst)
    End Function
    Function asientoNomina(year As Integer, month As Integer, tipoCuenta As Integer) As ActionResult
        Dim asiento = db.GetAsiento11(year, month, "001", strCnEpsilon)
        Dim lst = db.GetListOfNegocioAdministracion(strCn).Select(
            Function(n As Object)
                Dim lstmo = db.GetListOfManoObraAdministracion(strCn).Select(
                                     Function(mo As Object)
                                         Return New With {.idManoObra = mo.id, .nombreManoObra = mo.nombre,
                                                          .listOfDepartamento = db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto).Select(
                                                              Function(d)
                                                                  Dim cuenta = db.GetListOfCuentasDepartamentos(strCn).Find(Function(c) c.idDepartamento = d.id And c.idTipoCuenta = tipoCuenta)
                                                                  Dim lstDepEpsilon = db.GetMapeosEpsilonAdministracion(strCn).Where(Function(e) e.idAdministracion = d.id).ToList()
                                                                  Dim total = asiento.Where(Function(e) lstDepEpsilon.Exists(Function(e2) e.idNivel = e2.idEpsilon)).Sum(
                                                                       Function(e3)
                                                                           Select Case e3.operacion
                                                                               Case 1
                                                                                   Return e3.devengo
                                                                               Case 2
                                                                                   Return -e3.devengo
                                                                               Case 0
                                                                                   Return 0
                                                                               Case Else
                                                                                   Throw New Exception
                                                                           End Select
                                                                       End Function)
                                                                  If cuenta Is Nothing Then
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = Nothing,
                                                                                   .descripcionCuenta = Nothing, .devengo = total}
                                                                  Else
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = cuenta.idCuenta,
                                                                                   .descripcionCuenta = cuenta.descripcionCuenta, .devengo = total}
                                                                  End If

                                                              End Function).ToList()}
                                     End Function).tolist()
                Return New With {.idNegocio = n.id, .nombreNegocio = n.nombre, .listOfManoObra = lstmo, .elementCount = lstmo.Sum(Function(mo) mo.listOfDepartamento.Count)
                                 }
            End Function).ToList()
        Return View("asiento", lst)
    End Function
    Function asientoCuotaLagunAro(year As Integer, month As Integer, tipoCuenta As Integer, idConvenio As String) As ActionResult
        Dim asiento = db.GetAsiento11(year, month, idConvenio, strCnEpsilon)
        Dim lst = db.GetListOfNegocioAdministracion(strCn).Select(
            Function(n As Object)
                Dim lstmo = db.GetListOfManoObraAdministracion(strCn).Select(
                                     Function(mo As Object)
                                         Return New With {.idManoObra = mo.id, .nombreManoObra = mo.nombre,
                                                          .listOfDepartamento = db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto).Select(
                                                              Function(d)
                                                                  Dim cuenta = db.GetListOfCuentasDepartamentos(strCn).Find(Function(c) c.idDepartamento = d.id And c.idTipoCuenta = tipoCuenta)
                                                                  Dim lstDepEpsilon = db.GetMapeosEpsilonAdministracion(strCn).Where(Function(e) e.idAdministracion = d.id).ToList()
                                                                  Dim total = asiento.Where(Function(e) lstDepEpsilon.Exists(Function(e2) e.idNivel = e2.idEpsilon)).Sum(
                                                                       Function(e3)
                                                                           Return e3.devengo
                                                                       End Function)
                                                                  If cuenta Is Nothing Then
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = Nothing,
                                                                                   .descripcionCuenta = Nothing, .devengo = total}
                                                                  Else
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = cuenta.idCuenta,
                                                                                   .descripcionCuenta = cuenta.descripcionCuenta, .devengo = total}
                                                                  End If

                                                              End Function).ToList()}
                                     End Function).tolist()
                Return New With {.idNegocio = n.id, .nombreNegocio = n.nombre, .listOfManoObra = lstmo, .elementCount = lstmo.Sum(Function(mo) mo.listOfDepartamento.Count)
                                 }
            End Function).ToList()
        Return View("asiento", lst)
    End Function
    Function asientoCuotaSS(year As Integer, month As Integer, tipoCuenta As Integer, idConvenio As String) As ActionResult
        Dim asiento = db.GetAsiento13(year, month, idConvenio, strCnEpsilon)
        Dim lst = db.GetListOfNegocioAdministracion(strCn).Select(
            Function(n As Object)
                Dim lstmo = db.GetListOfManoObraAdministracion(strCn).Select(
                                     Function(mo As Object)
                                         Return New With {.idManoObra = mo.id, .nombreManoObra = mo.nombre,
                                                          .listOfDepartamento = db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto).Select(
                                                              Function(d)
                                                                  Dim cuenta = db.GetListOfCuentasDepartamentos(strCn).Find(Function(c) c.idDepartamento = d.id And c.idTipoCuenta = tipoCuenta)
                                                                  Dim lstDepEpsilon = db.GetMapeosEpsilonAdministracion(strCn).Where(Function(e) e.idAdministracion = d.id).ToList()
                                                                  Dim total = asiento.Where(Function(e) lstDepEpsilon.Exists(Function(e2) e.idNivel = e2.idEpsilon)).Sum(
                                                                       Function(e3)
                                                                           Return e3.devengo
                                                                       End Function)
                                                                  If cuenta Is Nothing Then
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = Nothing,
                                                                                   .descripcionCuenta = Nothing, .devengo = total}
                                                                  Else
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = cuenta.idCuenta,
                                                                                   .descripcionCuenta = cuenta.descripcionCuenta, .devengo = total}
                                                                  End If

                                                              End Function).ToList()}
                                     End Function).tolist()
                Return New With {.idNegocio = n.id, .nombreNegocio = n.nombre, .listOfManoObra = lstmo, .elementCount = lstmo.Sum(Function(mo) mo.listOfDepartamento.Count)
                                 }
            End Function).ToList()
        Return View("asiento", lst)
    End Function

    Private Function GetResultadoResumen(year As Integer, month As Integer, resumen As ResumenConvenio, tipoCuenta As Integer)
        Dim asiento = db.GetAsiento(year, month, resumen, strCnEpsilon)
        Return db.GetListOfNegocioAdministracion(strCn).SelectMany(
            Function(n As Object)
                Dim lstmo = db.GetListOfManoObraAdministracion(strCn).SelectMany(
                                     Function(mo As Object)
                                         Return db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto).Select(
                                                              Function(d)
                                                                  Dim cuenta = db.GetListOfCuentasDepartamentos(strCn).Find(Function(c) c.idDepartamento = d.id And c.idTipoCuenta = tipoCuenta)
                                                                  Dim lstDepEpsilon = db.GetMapeosEpsilonAdministracion(strCn).Where(Function(e) e.idAdministracion = d.id).ToList()
                                                                  Dim total = asiento.Where(Function(e) lstDepEpsilon.Exists(Function(e2) e.idNivel = e2.idEpsilon)).Sum(
                                                                       Function(e3)
                                                                           Select Case e3.operacion
                                                                               Case 1
                                                                                   Return e3.devengo
                                                                               Case 2
                                                                                   Return -e3.devengo
                                                                               Case 0
                                                                                   Return 0
                                                                               Case Else
                                                                                   Throw New Exception
                                                                           End Select
                                                                       End Function)
                                                                  If cuenta Is Nothing Then
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = Nothing,
                                                                                   .descripcionCuenta = Nothing, .devengo = total}
                                                                  Else
                                                                      Return New With {.idDepartamento = d.id, .nombreDepartamento = d.nombre, .idcuenta = cuenta.idCuenta,
                                                                                   .descripcionCuenta = cuenta.descripcionCuenta, .devengo = total}
                                                                  End If

                                                              End Function).ToList()
                                     End Function).ToList()
                Return lstmo
            End Function).GroupBy(Function(g) g.idcuenta)
    End Function
End Class