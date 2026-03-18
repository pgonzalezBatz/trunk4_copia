Imports System.Security.Cryptography
Public Class personalController
    Inherits System.Web.Mvc.Controller
    ReadOnly strCn As String = ConfigurationManager.ConnectionStrings("microsoft").ConnectionString

    <SimpleRoleProvider(Role.rrhh, Role.bajasSinEvolucion)>
    Public Function BusquedaCompleta(s As String) As ActionResult
        If String.IsNullOrEmpty(s) Then
            Return View()
        End If
        Return View(db.lookupListOfPersonal(s.Replace(" ", ""), strCn))
    End Function
    <SimpleRoleProvider(Role.rrhh, Role.eki)>
    Public Function BusquedaCompletaAltas(s As String) As ActionResult
        Return View("busquedaCompleta", db.LookupListOfPersonal(If(s, "").Replace(" ", ""), False, True, strCn))
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    Public Function Evolucion(nif As String) As ActionResult
        ViewData("secuenciasOrganigrama") = db.cambiosSecuenciayOrganigrama(nif, strCn).Select(Function(o)
                                                                                                   Dim dif
                                                                                                   If IsDate(o.ffinsec) Then
                                                                                                       dif = InterpretarTimeSpan(o.ffinsec - o.finiciosec)
                                                                                                   Else
                                                                                                       dif = InterpretarTimeSpan(CType(Now, Object) - o.finiciosec)
                                                                                                   End If
                                                                                                   Return addPropertyToAnonymous(o, New KeyValuePair(Of String, Object)("diff", dif))
                                                                                               End Function).ToList
        ViewData("secuencias") = db.cambiosSecuencia(nif, strCn).Select(Function(o)
                                                                            Dim dif
                                                                            If IsDate(o.ffinsec) Then
                                                                                dif = InterpretarTimeSpan(o.ffinsec - o.finiciosec)
                                                                            Else
                                                                                dif = InterpretarTimeSpan(CType(Now, Object) - o.finiciosec)
                                                                            End If
                                                                            Return addPropertyToAnonymous(o, New KeyValuePair(Of String, Object)("diff", dif))
                                                                        End Function).ToList
        ViewData("indices") = db.CambiosIndice(nif, strCn)
        ViewData("indecesActuales") = db.indiceActual(nif, strCn)
        ViewData("vencimientos") = db.vencimientos(nif, strCn)
        ViewData("prorrogas") = db.ListOfProrrogas(nif, strCn)
        Return View(db.lookupListOfPersonal(nif, strCn).First())
    End Function
    <SimpleRoleProvider(Role.rrhh, Role.eki + Role.departamento)>
    Public Function ListadoDepartamentos(s As String) As ActionResult
        Dim l = db.listadoDepartamental(strCn)
        Dim f = Function(g1) New With {Key .n2 = g1.n2}
        'Dim lstGroups = New List(Of String) From {"n2"}
        'Dim tGroups = dynamicGrouping(lstGroups)
        'Dim p2 = l.GroupBy(tGroups.Item1, tGroups.Item2).ToList
        If String.IsNullOrEmpty(s) Then
            Return View(l)
        End If

        Return View(l.Where(Function(p) p.idTrabajador.contains(s) Or (p.nombre.ToString.Trim(" ") + p.apellido1.ToString.Trim(" ")).ToLower().Contains(s.ToLower.Replace(" ", "")) Or p.apellido2.ToString.Trim(" ").ToLower().Contains(s.ToLower)))
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    Public Function arbolDepartamentos() As ActionResult
        Return View()
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    Public Function listadoDepartamentos2() As JsonResult
        Dim l = db.listadoDepartamental(strCn)
        Return Json(New With {.name = "BATZ", .children = NestedGrouping(l, Function(o) o.n2.Trim(" "),
                               Function(o) o.n3.Trim(" "),
                               Function(o) o.n4.Trim(" ")
                               )}, JsonRequestBehavior.AllowGet)
    End Function
    <SimpleRoleProvider(Role.rrhh, Role.eki + Role.excedencia)>
    Public Function listadoExcedencias()
        Return View(db.GetListOfExcedencias(strCn))
    End Function
    <SimpleRoleProvider(Role.rrhh, Role.altasBajas)>
    Public Function AltasBajasYCambios(fromDate As Date?, toDate As Date?) As ActionResult
        If fromDate.HasValue And toDate.HasValue Then
            ViewData("bajas") = db.GetListOfBajas(fromDate, toDate, strCn)
            ViewData("altas") = db.GetListOfAltas(fromDate, toDate, strCn)
            ViewData("cambios") = db.GetListOfCambiosPuesto(fromDate, toDate, strCn)
        End If
        Return View()
    End Function
    Function InterpretarTimeSpan(t As TimeSpan) As String
        If t.Days < 31 Then
            Return t.Days.ToString + " dias"
        ElseIf t.Days < 366 Then
            Return Math.Round(t.Days / 30.25, 2).ToString + " meses"
        Else
            Return Math.Round(t.Days / 365.25, 2).ToString + " años "
        End If
    End Function
    Function addPropertyToAnonymous(o As Object, ByVal ParamArray d() As KeyValuePair(Of String, Object)) As Object
        Dim dic As IDictionary(Of String, Object) = New Dynamic.ExpandoObject()
        For Each p In o.GetType().GetProperties()
            dic(p.Name) = p.GetValue(o, Nothing)
        Next

        For Each prp In d
            dic(prp.Key) = prp.Value
        Next

        Return dic
    End Function
    Public Shared Function NestedGrouping(l As IEnumerable(Of Object), ParamArray f() As Func(Of Object, Object)) As IEnumerable(Of Object)
        If f.Count > 0 Then
            Return l.GroupBy(f.First, resultSelector:=Function(key As Object, l2 As IEnumerable(Of Object))
                                                          If key Is Nothing Then
                                                              Return Nothing
                                                          End If
                                                          Return New With {.name = key, .children = NestedGrouping(l2, f.Skip(1).ToArray)}
                                                      End Function)
        Else
            Return l.Select(Function(o) New With {.name = o.nombre + o.apellido1 + If(o.apellido2, ""), .convenio = o.convenio, .categoria = o.categoria})
        End If
    End Function
    Private Function dynamicGrouping(lst As List(Of String)) As Tuple(Of Func(Of Object, Object), Func(Of Object, Object, Object))


        Dim f = Function(g)
                    Dim ex As Object = New Dynamic.ExpandoObject()
                    lst.ForEach(Sub(l)
                                    Select Case l
                                        Case "n2"
                                            ex.n2 = g.n2
                                    End Select
                                End Sub)
                    Return ex
                End Function
        Dim f2 = Function(g2, l2) New With {.n2 = g2.n2, .n3 = "", .count = 2}
        Return New Tuple(Of Func(Of Object, Object), Func(Of Object, Object, Object))(f, f2)
    End Function

End Class
