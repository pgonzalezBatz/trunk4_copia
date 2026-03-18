Imports System.Security.Permissions
Namespace efa
    Public Class adminController
        Inherits System.Web.Mvc.Controller

        Private strCnSab As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
        Private strCnEfa As String = ConfigurationManager.ConnectionStrings("BaliabideF").ConnectionString
        Private IdGrupo As Integer = ConfigurationManager.AppSettings.Get("GrupoAdmin")
        Private strCnTelef As String = ConfigurationManager.ConnectionStrings("Telefonia").ConnectionString

        <SimpleRoleProvider(EfaRole.admin)>
        Function Index() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function Grupo() As ActionResult
            ViewData("listofgroup") = DB.GetListOfGrupo(strCnEfa)
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Grupo(ByVal g As Grupo, ByVal ImageUpload As HttpPostedFileBase) As ActionResult
            If ImageUpload IsNot Nothing Then
                Dim fileData(ImageUpload.ContentLength) As Byte
                Dim fileLength = ImageUpload.ContentLength
                ImageUpload.InputStream.Read(fileData, 0, fileLength)
                g.Image = fileData
            End If
            If ModelState.IsValid Then
                DB.AddGrupo(g, strCnEfa)
                Return RedirectToAction("grupo")
            End If
            ViewData("listofgroup") = DB.GetListOfGrupo(strCnEfa)
            Return View(g)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function EditarGrupo(ByVal grupo As String) As ActionResult
            Return View(DB.GetGrupoNormal(grupo, strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function EditarGrupo(ByVal g As Grupo, ByVal ImageUpload As HttpPostedFileBase) As ActionResult
            If ImageUpload IsNot Nothing Then
                Dim fileData(ImageUpload.ContentLength) As Byte
                Dim fileLength = ImageUpload.ContentLength
                ImageUpload.InputStream.Read(fileData, 0, fileLength)
                g.Image = fileData
            End If
            If ModelState.IsValid Then
                DB.UpdateGrupo(g, strCnEfa)
                Return RedirectToAction("grupo")
            End If
            Return View(g)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function EliminarGrupo(ByVal grupo As String) As ActionResult
            Return View(DB.GetGrupoNormal(grupo, strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function EliminarGrupo(ByVal g As Grupo) As ActionResult
            If ModelState.IsValid Then
                If DB.HasGroupAnyRegistros(g.Nombre, strCnEfa) Then
                    ModelState.AddModelError("error", "El grupo ya ha sido asignado a recursos por lo que no se puede borrar")
                    Return View(g)
                End If
                DB.DeleteGrupo(g, strCnEfa)
                Return RedirectToAction("grupo")
            End If
            Return View(g)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function Recurso(ByVal grupo As String) As ActionResult
            Dim r As New Recurso With {.NombreGrupo = grupo}
            ViewData("listofplanta") = DB.GetListOfPlanta(strCnSab)
            ViewData("listOfRecurso") = DB.GetListOfRecursoNormalTodo(grupo, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnEfa)
            Return View(r)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Recurso(ByVal r As Recurso) As ActionResult
            If ModelState.IsValid Then
                DB.AddRecurso(r, strCnEfa)
                Return RedirectToAction("recurso", New With {.grupo = r.NombreGrupo})
            End If
            ViewData("listofplanta") = DB.GetListOfPlanta(strCnSab)
            ViewData("listOfRecurso") = DB.GetListOfRecursoNormalTodo(r.nombreGrupo, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnEfa)
            Return View(r)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function EditarRecurso(ByVal grupo As String, ByVal idRecurso As String) As ActionResult
            Return View(DB.GetRecurso(grupo, idRecurso, strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function EditarRecurso(ByVal r As Recurso) As ActionResult
            If ModelState.IsValid Then
                DB.UpdateRecurso(r, strCnEfa)
                Return (RedirectToAction("recurso", New With {.grupo = r.NombreGrupo}))
            End If
            Return View(r)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function EliminarRecurso(ByVal grupo As String, ByVal idRecurso As String) As ActionResult
            Return View(DB.GetRecurso(grupo, idRecurso, strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function EliminarRecurso(ByVal r As Recurso) As ActionResult
            If ModelState.IsValid Then
                DB.DeleteRecurso(r, strCnEfa)
                Return (RedirectToAction("recurso", New With {.grupo = r.NombreGrupo}))
            End If
            Return View(r)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function listadorecursosnormales() As ActionResult
            ViewData("registros") = DB.GetListOfRegistroCogidoNormal(strCnEfa)
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function listadotelefonos() As ActionResult
            ViewData("registros") = DB.GetListOfRegistroCogidoTelefono(strCnTelef)
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function SeleccionarTrabajador() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function SeleccionarTrabajador(ByVal idSab As String) As ActionResult
            If IsNumeric(idSab) Then
                Return RedirectToAction("ElegirAccion", New With {.idSab = idSab})
            End If
            ModelState.AddModelError("box", "Es obligatorio seleccionar un trabajador")
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function ElegirAccion(ByVal idSab As Integer) As ActionResult
            Return View(DB.GetTrabajador(idSab, strCnSab))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function SeleccionarGrupo(ByVal idSab As Integer) As ActionResult
            Dim l = DB.GetListOfGrupo(DB.GetIdPlanta(idSab, strCnSab), strCnEfa)
            l.Add(New Grupo With {.Nombre = "Telefono"})
            Return View(l)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function SeleccionarRecurso(ByVal idsab As Integer, ByVal grupo As String) As ActionResult
            If String.IsNullOrEmpty(grupo) Then
                'Dejar
                Dim l As List(Of Registro) = DB.GetListOfRegistroDeUsuarioNormal(idsab, strCnEfa)
                l.AddRange(DB.GetListOfRegistroDeUsuarioTelefono(idsab, strCnTelef))
                Return View(l)
            Else
                'Coger
                If grupo.ToLower = "telefono" Then
                    Return View(DB.GetListOfRecursoTelefonoLibre(grupo, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnTelef))
                Else
                    Return View(DB.GetListOfRecursoNormalLibre(grupo, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnEfa))
                End If
            End If
            'Return View(DB.GetListOf(strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function SeleccionarRecurso(ByVal r As Registro) As ActionResult
            If String.IsNullOrEmpty(Request("grupo")) Then
                'Dejar
                r.Dejar = CDate(Now.ToShortDateString)
                If r.NombreGrupo.ToLower = "telefono" Then
                    DB.DejarRecursoTelefono(r, strCnTelef)
                Else
                    DB.DejarRecursoNormal(r, strCnEfa)
                End If
            Else
                r.Coger = CDate(Now.ToShortDateString)
                If r.NombreGrupo.ToLower = "telefono" Then
                    DB.CogerRecursoTelefono(r, strCnTelef)
                Else
                    DB.CogerRecursoNormal(r, strCnEfa)
                End If
            End If
            Return RedirectToAction("Confirm", New With {.idSab = r.IdSab})
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function alarmas() As ActionResult
            ViewData("listofgroup") = DB.GetListOfGrupoAlarmas(strCnEfa)
            Return View("alarmas")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Cambiaralarmas(ByVal g As Grupo) As ActionResult
            If g.dias.HasValue Then
                DB.UpdateMaxDias(g.Nombre, g.dias, strCnEfa)
                Return RedirectToAction("alarmas")
            End If
            ModelState.AddModelError("dias", "Es necesario introducir un valor")
            ViewData("listofgroup") = DB.GetListOfGrupoAlarmas(strCnEfa)

            Return alarmas()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function excepciones(ByVal nombre As String) As ActionResult
            If nombre = "Telefono" Then
                Return excepcionesTelefono()
            Else
                Return excepcionesNormales(nombre)
            End If
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function Confirm(ByVal idSab As Integer) As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function UltimasPersonasEnCogerRecurso0() As ActionResult
            Return View(DB.GetListOfGrupo(strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function UltimasPersonasEnCogerRecurso1(grupo As String) As ActionResult
            Return View(DB.GetListOfRecursoNormalTodo(grupo, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function UltimasPersonasEnCogerRecurso2(grupo As String, recurso As String) As ActionResult
            Return View(DB.GetListOfUltimasPersonasEnCogerRecurso(grupo, recurso, strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Private Function excepcionesTelefono() As ActionResult
            ViewData("listofrecursos") = DB.GetListOfRecursoTelefonoTodo(DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnTelef)
            Return View("excepciones")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Private Function excepcionesNormales(ByVal nombre As String) As ActionResult
            ViewData("listofrecursos") = DB.GetListOfRecursoNormalTodo(nombre, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnEfa)
            Return View("excepciones")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function CambiarExcepcion(ByVal Nombre As String, ByVal idRecurso As String, ByVal fecha As Nullable(Of DateTime)) As ActionResult
            DB.UpdateExcepcion(Nombre, idRecurso, fecha, strCnEfa)
            Return RedirectToAction("excepciones", New With {.nombre = Nombre})
            Return alarmas()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function listTonerImpresora() As ActionResult
            Return View(DB.GetListOfTonerImpresora(strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function listTonercolor(idImpresora As Integer) As ActionResult
            ViewData("tonerimpresora") = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(ti) ti.id = idImpresora)
            Return View(DB.GetListOfTonerColor(idImpresora, strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function listComponente() As ActionResult
            Return View(DB.GetListOfComponente(strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function listComponenteModelo(idComponente As Integer) As ActionResult
            ViewData("componente") = DB.GetListOfComponente(strCnEfa).Find(Function(c) c.id = idComponente)
            Return View(DB.GetListOfComponenteModelo(idComponente, strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function addtonerImpresora() As ActionResult
            Return View("edittonerImpresora")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function addtonerImpresora(nombre As String, serie As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", "Es obligatorio introducir el nombre")
            End If
            If String.IsNullOrEmpty(serie) Then
                ModelState.AddModelError("serie", "Es obligatorio introducir la serie")
            End If
            If ModelState.IsValid Then
                DB.AddTonerImpresora(nombre, serie, strCnEfa)
                Return RedirectToAction("listtonerimpresora")
            End If
            Return View("edittonerImpresora")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function addComponente() As ActionResult
            Return View("editComponente")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function addComponente(nombre As String, serie As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", "Es obligatorio introducir el nombre")
            End If
            If String.IsNullOrEmpty(serie) Then
                ModelState.AddModelError("serie", "Es obligatorio introducir la serie")
            End If
            If ModelState.IsValid Then
                DB.AddComponente(nombre, serie, strCnEfa)
                Return RedirectToAction("listcomponente")
            End If
            Return View("editcomponente")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function addComponenteModelo(idcomponente As Integer) As ActionResult
            ViewData("componente") = DB.GetListOfComponente(strCnEfa).Find(Function(c) c.id = idcomponente)
            Return View("editComponentemodelo")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function addComponenteModelo(idComponente As Integer, pn As String, descripcion As String, nElementos As Integer, networkPath As String) As ActionResult
            If String.IsNullOrEmpty(pn) Then
                ModelState.AddModelError("nombre", "Es obligatorio introducir el nombre")
            End If
            If ModelState.IsValid Then
                DB.AddComponenteModelo(idComponente, pn, descripcion, nElementos, networkPath, strCnEfa)
                Return RedirectToAction("listcomponentemodelo", h.ToRouteValues(Request.QueryString, Nothing))
            End If
            Return View("editcomponentemodelo")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function addtonerColor(idImpresora As Integer) As ActionResult
            ViewData("tonerimpresora") = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(ti) ti.id = idImpresora)
            Return View("edittonerColor")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function addtonerColor(idImpresora As String, idcolor As String, color As String, stock As Nullable(Of Integer), stockMinimo As Nullable(Of Integer)) As ActionResult
            If String.IsNullOrEmpty(idcolor) Then
                ModelState.AddModelError("idcolor", "Es obligatorio introducir el nombre")
            End If
            If String.IsNullOrEmpty(color) Then
                ModelState.AddModelError("color", "Es obligatorio introducir la serie")
            End If
            If Not stock.HasValue Then
                ModelState.AddModelError("stock", "Es obligatorio introducir el stock actual")
            End If
            If Not stockMinimo.HasValue Then
                ModelState.AddModelError("stockMinimo", "Es obligatorio introducir el stock minimo permitido antes de notificar")
            End If
            If ModelState.IsValid Then
                DB.AddTonerColor(idImpresora, idcolor, color, stock, stockMinimo, strCnEfa)
                Return RedirectToAction("listtonercolor", New With {.idImpresora = idImpresora})
            End If
            Return View("edittonercolor")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function edittonerImpresora(idImpresora As Integer) As ActionResult
            Dim TonerImpresora = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(ti) ti.id = idImpresora)
            Return View(TonerImpresora)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function edittonerImpresora(idImpresora As Integer, nombre As String, serie As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", "Es obligatorio introducir el nombre")
            End If
            If String.IsNullOrEmpty(serie) Then
                ModelState.AddModelError("serie", "Es obligatorio introducir la serie")
            End If
            If ModelState.IsValid Then
                DB.updateTonerImpresora(idImpresora, nombre, serie, Nothing, strCnEfa)
                Return RedirectToAction("listtonerimpresora")
            End If
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function editComponente(idComponente As Integer) As ActionResult
            Dim Componente = DB.GetListOfComponente(strCnEfa).Find(Function(ti) ti.id = idComponente)
            Return View(Componente)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function editComponente(idComponente As Integer, nombre As String, serie As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", "Es obligatorio introducir el nombre")
            End If
            If String.IsNullOrEmpty(serie) Then
                ModelState.AddModelError("serie", "Es obligatorio introducir la serie")
            End If
            If ModelState.IsValid Then
                DB.updateComponente(idComponente, nombre, serie, Nothing, strCnEfa)
                Return RedirectToAction("listcomponente")
            End If
            Return View(New With {.id = idComponente, .nombre = nombre, .serie = serie})
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function editComponenteModelo(idcomponente As Integer, idcomponentemodelo As Integer) As ActionResult
            ViewData("componente") = DB.GetListOfComponente(strCnEfa).Find(Function(c) c.id = idcomponente)
            Return View("editComponentemodelo", DB.GetListOfComponenteModelo(idcomponente, strCnEfa).Find(Function(cm) cm.id = idcomponentemodelo))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function editComponenteModelo(idComponente As Integer, idcomponentemodelo As Integer, pn As String, descripcion As String, nElementos As Integer, networkPath As String) As ActionResult
            If String.IsNullOrEmpty(pn) Then
                ModelState.AddModelError("pn", "Es obligatorio introducir el nombre")
            End If
            If Not IsNumeric(nElementos) Then
                ModelState.AddModelError("nelementos", "Es obligatorio introducir stock actual")
            End If
            If ModelState.IsValid Then
                DB.UpdateComponenteModelo(idComponente, idcomponentemodelo, pn, descripcion, nElementos, networkPath, Nothing, strCnEfa)
                Return RedirectToAction("listcomponentemodelo", h.ToRouteValuesDelete(Request.QueryString, "idcomponentemodelo"))
            End If
            Return View("editcomponentemodelo", New With {.id = idcomponentemodelo, .pn = pn, .nElementos = nElementos, .descripcion = descripcion})
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function edittonerColor(idImpresora As Integer, idColor As String) As ActionResult
            ViewData("tonerimpresora") = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(ti) ti.id = idImpresora)
            Return View(DB.GetListOfTonerColor(idImpresora, strCnEfa).Find(Function(tc) tc.idcolor = idColor))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function edittonercolor(idImpresora As Integer, idcolor As String, color As String, stock As Nullable(Of Integer), stockMinimo As Nullable(Of Integer)) As ActionResult
            If String.IsNullOrEmpty(idcolor) Then
                ModelState.AddModelError("idcolor", "Es obligatorio introducir el identificador de color")
            End If
            If String.IsNullOrEmpty(color) Then
                ModelState.AddModelError("color", "Es obligatorio introducir el color")
            End If
            If Not stock.HasValue Then
                ModelState.AddModelError("stock", "Es obligatorio introducir el stock actual")
            End If
            If Not stockMinimo.HasValue Then
                ModelState.AddModelError("stockminimo", "Es obligatorio introducir el stock minimo")
            End If
            If ModelState.IsValid Then
                DB.updateTonerColor(idImpresora, idcolor, color, stock.Value, stockMinimo.Value, Nothing, strCnEfa)
                Return RedirectToAction("listtonercolor", New With {.idimpresora = idImpresora})
            End If
            ViewData("tonerimpresora") = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(ti) ti.id = idImpresora)
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function anulartonerImpresora(idImpresora As Integer) As ActionResult
            Dim TonerImpresora = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(ti) ti.id = idImpresora)
            Return View(TonerImpresora)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function anulartonerImpresora(idImpresora As Integer, nombre As String, serie As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                Return RedirectToAction("listtonerimpresora")
            End If
            If String.IsNullOrEmpty(serie) Then
                Return RedirectToAction("listtonerimpresora")
            End If
            DB.updateTonerImpresora(idImpresora, nombre, serie, Now, strCnEfa)
            Return RedirectToAction("listtonerimpresora")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function anularComponente(idComponente As Integer) As ActionResult
            Dim componente = DB.GetListOfComponente(strCnEfa).Find(Function(ti) ti.id = idComponente)
            Return View(componente)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function anularComponente(idComponente As Integer, nombre As String, serie As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                Return RedirectToAction("liscomponente")
            End If
            If String.IsNullOrEmpty(serie) Then
                Return RedirectToAction("liscomponente")
            End If
            DB.updateComponente(idComponente, nombre, serie, Now, strCnEfa)
            Return RedirectToAction("liscomponente")
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function anularComponenteModelo(idComponente As Integer, idComponenteModelo As Integer) As ActionResult
            ViewData("componente") = DB.GetListOfComponente(strCnEfa).Find(Function(ti) ti.id = idComponente)
            Dim componenteModelo = DB.GetListOfComponenteModelo(idComponente, strCnEfa).Find(Function(cm) cm.id = idComponenteModelo)
            Return View(componenteModelo)
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function anularComponenteModelo(idComponente As Integer, idComponenteModelo As Integer, pn As String, descripcion As String, nElementos As Integer, networkPath As String) As ActionResult
            If String.IsNullOrEmpty(pn) Then
                Return RedirectToAction("listcomponentemodelo", h.ToRouteValuesDelete(Request.QueryString, "idcomponentemodelo"))
            End If
            If String.IsNullOrEmpty(nElementos) Then
                Return RedirectToAction("listcomponentemodelo", h.ToRouteValuesDelete(Request.QueryString, "idcomponentemodelo"))
            End If
            DB.UpdateComponenteModelo(idComponente, idComponenteModelo, pn, descripcion, nElementos, networkPath, Now, strCnEfa)
            Return RedirectToAction("listcomponentemodelo", h.ToRouteValuesDelete(Request.QueryString, "idcomponentemodelo"))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        Function anulartonerColor(idImpresora As Integer, idColor As String) As ActionResult
            ViewData("tonerimpresora") = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(ti) ti.id = idImpresora)
            Return View(DB.GetListOfTonerColor(idImpresora, strCnEfa).Find(Function(ic) ic.idcolor = idColor))
        End Function
        <SimpleRoleProvider(EfaRole.admin)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function anulartonerColor(idImpresora As Integer, idColor As String, color As String, stock As Integer, stockMinimo As Integer) As ActionResult
            If String.IsNullOrEmpty(idColor) Then
                Return RedirectToAction("listtonercolor", New With {.idimpresora = idImpresora})
            End If
            If String.IsNullOrEmpty(color) Then
                Return RedirectToAction("listtonercolor", New With {.idimpresora = idImpresora})
            End If
            DB.updateTonerColor(idImpresora, idColor, color, stock, stockMinimo, Now, strCnEfa)
            Return RedirectToAction("listtonercolor", New With {.idimpresora = idImpresora})
        End Function
    End Class
End Namespace