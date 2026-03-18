Imports System
Imports System.Configuration
Imports System.Data.Entity
Imports System.Net
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.IO
Imports System.Collections.Generic



Namespace Controllers
    Public Class ReclamacionesController
        Inherits Controller
        Private db As New Entities_BezerreSis
        Dim brainDatabase As New brainDB
        Dim oracleDatabase As New oracleDB
        Dim myDatabase As New myDb
        Public cnStrBezerresis As String = ConfigurationManager.ConnectionStrings("BEZERRESIS").ConnectionString

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function IndexTest() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim reclamacionesList As List(Of ReclamacionViewModel)
            reclamacionesList = CargaDatos(False)
            ViewBag.Oficial = False
            TempData("ReturnUrl") = "IndexTest" ''''TODO:CHANGE (Esto hará que se se de la opción o no de crear una reclamacion oficial, y en caso de que no, dará opción o no de crear su homóloga en gtk)
            ViewBag.MyTitle = "* Testing *"
            Return View("Index", reclamacionesList.ToList)
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Index() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim reclamacionesList As List(Of ReclamacionViewModel)
            reclamacionesList = CargaDatos(True)
            ViewBag.Oficial = True
            TempData("ReturnUrl") = "Index"
            'ViewBag.MyTitle = "* Testing3 *"
            Return View(reclamacionesList.ToList)
        End Function

        Private Function CargaDatos(ByVal oficial As Boolean) As List(Of ReclamacionViewModel)
            Dim cookies = Request.Cookies("BezerreSis_Filtro")
            Dim idPlanta = cookies.Values("IdPlanta")
            Dim oracleDb As New oracleDB
            Dim lClientes_Selected As New List(Of Integer)
            Dim lProductos_Selected As New List(Of Integer)
            Dim lCreadores_Selected As New List(Of Integer)
            Dim lEstados_Selected As New List(Of Integer)
            Dim fecha_desde As New Nullable(Of DateTime)
            Dim fecha_hasta As New Nullable(Of Date)

            ' Leer cookies y asignar valores a las listas y fechas
            If Request IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdCliente")) Then
                    lClientes_Selected = cookies.Values("IdCliente").Split(",").Select(Function(f) CInt(f)).ToList()
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdProducto")) Then
                    lProductos_Selected = cookies.Values("IdProducto").Split(",").Select(Function(f) CInt(f)).ToList()
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_DESDE")) Then
                    fecha_desde = DateTime.ParseExact(cookies.Values("FECHA_DESDE"), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture)
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_HASTA")) Then
                    fecha_hasta = DateTime.ParseExact(cookies.Values("FECHA_HASTA"), "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture)
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdCreador")) Then
                    lCreadores_Selected = cookies.Values("IdCreador").Split(",").Select(Function(f) CInt(f)).ToList()
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdEstado")) Then
                    lEstados_Selected = cookies.Values("IdEstado").Split(",").Select(Function(f) CInt(f)).ToList()
                End If
            End If

            Dim lEstados As New List(Of SelectListItem)
            lEstados.Add(New SelectListItem With {.Text = "ABIERTA", .Value = 1, .Selected = If(lEstados_Selected.Count = 0, False, (lEstados_Selected.Contains(1)))})
            lEstados.Add(New SelectListItem With {.Text = "CERRADA", .Value = 2, .Selected = If(lEstados_Selected.Count = 0, False, (lEstados_Selected.Contains(2)))})
            ViewBag.lEstados = lEstados
            ViewBag.FECHA_DESDE = If(fecha_desde Is Nothing, Nothing, "" & If(fecha_desde.Value.Day < 10, "0", "") & fecha_desde.Value.Day & "/" & If(fecha_desde.Value.Month < 10, "0", "") & fecha_desde.Value.Month & "/" & fecha_desde.Value.Year)
            ViewBag.FECHA_HASTA = If(fecha_hasta Is Nothing, Nothing, "" & If(fecha_hasta.Value.Day < 10, "0", "") & fecha_hasta.Value.Day & "/" & If(fecha_hasta.Value.Month < 10, "0", "") & fecha_hasta.Value.Month & "/" & fecha_hasta.Value.Year)
            ViewBag.lClientes = db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso Not o.NOMBRE.Contains("default")).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lClientes_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)
            ViewBag.lProductos = db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lProductos_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)
            ViewBag.lCreadores = oracleDatabase.getUsuariosCreadores().Select(Function(f) New SelectListItem With {.Text = f.Text, .Value = f.Value.Substring(2), .Selected = If(lCreadores_Selected.Count = 0, 0, lCreadores_Selected.Contains(f.Value.Substring(2)))})

            Dim myReclamaciones As New List(Of ReclamacionViewModel)
            Dim makeCalculations = lClientes_Selected.Count > 0 AndAlso lProductos_Selected.Count > 0 AndAlso lCreadores_Selected.Count > 0 AndAlso lEstados_Selected.Count > 0

            If makeCalculations Then
                Dim oraDB As New oracleDB
                myReclamaciones = oraDB.getReclamacionesFromView(oficial) _
                            .Where(Function(o) lClientes_Selected.Contains(o.CLIENTE_ID) _
                                        AndAlso lProductos_Selected.Contains(o.PRODUCTO_ID) _
                                        AndAlso lCreadores_Selected.Contains(o.CREADOR_ID) _
                                        AndAlso If(lEstados_Selected.Count = 0 OrElse lEstados_Selected.Count = 2, 1 = 1, If(lEstados_Selected.Contains(2), o.ESTADO.Equals("Cerrada"), o.ESTADO.Equals("Abierta"))) _
                                        AndAlso (fecha_desde Is Nothing OrElse o.FECHACREACION >= fecha_desde.Value) _
                                        AndAlso (fecha_hasta Is Nothing OrElse o.FECHACREACION <= fecha_hasta.Value)).ToList()

                ViewBag.clientesResumen = String.Join(", ", db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lClientes_Selected.Count > 0, lClientes_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))
                ViewBag.productosResumen = String.Join(", ", db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lProductos_Selected.Count > 0, lProductos_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))
                ViewBag.estadosResumen = String.Join(", ", lEstados.Where(Function(o) If(lEstados_Selected.Count > 0, lEstados_Selected.Contains(o.Value), True)).Select(Function(f) f.Text).OrderBy(Function(f) f))
                ViewBag.creadoresResumen = String.Join(", ", oracleDatabase.getUsuariosCreadores().Where(Function(o) If(lCreadores_Selected.Count > 0, lCreadores_Selected.Contains(o.Value.Substring(2)), True)).Select(Function(f) f.Text).OrderBy(Function(f) f))
            End If

            ViewBag.ShowTable = makeCalculations
            ViewBag.MuestraMsg = cookies.Values.AllKeys.Contains("IdCliente") AndAlso Not makeCalculations 'significa que ya se ha pulsado el botón de buscar
            Return myReclamaciones.OrderByDescending(Function(o) o.FECHACREACION).ToList()
        End Function


        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Create() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Return View()
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        <HttpPost()>
        Function Create(ByVal rec As RECLAMACIONES) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            ' Forzar "Reclamación oficial" a 1 si está en cero
            If rec.RECLAMACIONOFICIAL = 0 Then
                rec.RECLAMACIONOFICIAL = 1
                ModelState("RECLAMACIONOFICIAL").Errors.Clear()
            End If

            ' Verificar si el usuario tiene permiso para crear la reclamación
            If Not myDatabase.hasPermission(rec.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . Create", User.Identity.Name.ToString, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If

            If rec.RECLAMACIONOFICIAL = 1 And rec.NUMPIEZASNOK < 1 Then
                ModelState.AddModelError("NUMPIEZASNOK", "Reclamaciones oficiales debe ser mayor que 0.")
            End If
            ' Guardar la reclamación si el modelo es válido
            If ModelState.IsValid Then
                Dim con As OracleConnection = Nothing
                Dim transact As OracleTransaction = Nothing
                Try
                    Dim query As String = String.Empty
                    con = New OracleConnection(cnStrBezerresis)
                    con.Open()
                    transact = con.BeginTransaction()
                    Dim oracleDB As New oracleDB

                    db.RECLAMACIONES.Add(rec)
                    db.SaveChanges()

                    Dim idGtk = 0
                    'No crear NC en GTK si la clasificación es "Garantias)"
                    If Not rec.CLASIFICACION = 3 AndAlso Request.Form("CREARENGTK").ToLower.Equals("true") Then
                        If rec.CLASIFICACION <> 2 Then ' 1 = "Garantias"
                            idGtk = oracleDB.crearEnGtk(rec, rec.ID, con)
                        End If
                    End If

                    ' Registrar la reclamación en el log
                    MvcApplication.Loguear("Info", "RECLAMACIONES . Create (after)", User.Identity.Name.ToString,
                "   Reclamation id: " & rec.ID,
                "   Reclamation descripcion: " & rec.DESCRIPCION,
                "   Reclamation refinternapieza: " & rec.REFINTERNAPIEZA,
                "   Reclamation idCliente: " & rec.CLIENTE,
                "   Reclamation idProducto: " & rec.PRODUCTO,
                "   Reclamation codcliente: " & rec.CODXCLIENTE,
                "   Reclamation numpiezasnok: " & rec.NUMPIEZASNOK,
                "   Reclamation procedencia: " & rec.PROCEDENCIA,
                "   Reclamation clasificacion: " & rec.CLASIFICACION,
                "   Reclamation nivelimportancia: " & rec.NIVELIMPORTANCIA,
                "   Reclamation repetitiva: " & rec.REPETITIVA,
                "   Reclamation oficial: " & rec.RECLAMACIONOFICIAL,
                "   Assigned NC id: " & If(idGtk = 0, " - ", idGtk))

                    transact.Commit()
                Catch e As System.Data.Entity.Validation.DbEntityValidationException
                    ' Manejo de errores y rollback en caso de error
                    For Each miniE In e.EntityValidationErrors
                        If Not miniE.IsValid AndAlso miniE.ValidationErrors.Count > 0 Then
                            For Each vE In miniE.ValidationErrors
                                MvcApplication.Loguear("Error", "RECLAMACIONES . Create", User.Identity.Name.ToString, vE.ErrorMessage)

                            Next
                        End If
                    Next
                    Return RedirectToAction("MyError", "Home")
                Catch ex As Exception
                    transact.Rollback()
                    MvcApplication.Loguear("Error", "RECLAMACIONES . Create", User.Identity.Name.ToString, ex.StackTrace)
                    Return RedirectToAction("MyError", "Home")
                Finally
                    If con IsNot Nothing AndAlso con.State <> ConnectionState.Closed Then
                        con.Close()
                        con.Dispose()
                    End If
                End Try
                Return RedirectToAction(TempData.Peek("ReturnUrl"))
            Else
                Return View(rec)
            End If
        End Function


        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Edit(ByVal id As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            'Dim rec As RECLAMACIONES = db.RECLAMACIONES.Where(Function(o) o.ID = id).FirstOrDefault
            Dim rec As RECLAMACIONES = db.RECLAMACIONES.Find(id)

            If IsNothing(rec) Then
                Return HttpNotFound()
            End If
            'If Not oracleDatabase.hasPermission(User.Identity.Name, rec.CREADOR, Session("idUser")) Then
            If Not myDatabase.hasPermission(rec.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . Edit (before)", User.Identity.Name.ToString, "   Reclamation id: " & id, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If
            MvcApplication.Loguear("Info", "RECLAMACIONES . Edit (before)", User.Identity.Name.ToString,
                                   "   Reclamation id: " & id,
                                   "   Reclamation idCreador: " & rec.CREADOR,
                                   "   Reclamation descripcion: " & rec.DESCRIPCION,
                                   "   Reclamation refinternapieza: " & rec.REFINTERNAPIEZA,
                                   "   Reclamation cliente: " & rec.CLIENTES.NOMBRE,
                                   "   Reclamation producto: " & rec.PRODUCTOS.NOMBRE,
                                   "   Reclamation codcliente: " & rec.CODXCLIENTE,
                                   "   Reclamation numpiezasnok: " & rec.NUMPIEZASNOK,
                                   "   Reclamation procedencia: " & rec.PROCEDENCIA,
                                   "   Reclamation clasificacion: " & rec.CLASIFICACION,
                                   "   Reclamation nivelimportancia: " & rec.NIVELIMPORTANCIA,
                                   "   Reclamation repetitiva: " & rec.REPETITIVA,
                                   "   Reclamation oficial: " & rec.RECLAMACIONOFICIAL)
            Return View("Create", rec)
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        <HttpPost()>
        Function Edit(ByVal rec As RECLAMACIONES) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If rec.RECLAMACIONOFICIAL = 1 And rec.NUMPIEZASNOK < 1 Then
                ModelState.AddModelError("NUMPIEZASNOK", "Reclamaciones oficiales debe ser mayor que 0.")
            End If
            If ModelState.IsValid Then
                Dim con As OracleConnection = Nothing
                Dim transact As OracleTransaction = Nothing
                Try
                    Dim query As String = String.Empty
                    con = New OracleConnection(cnStrBezerresis)
                    con.Open()
                    transact = con.BeginTransaction()
                    Dim oracleDB As New oracleDB

                    db.Entry(rec).State = EntityState.Modified
                    db.SaveChanges()

                    Dim idGtk = " - "
                    ''''TODO:  HAY QUE OTORGAR PERMISOS AL USUARIO 'BEZERRESIS' PARA LAS TABLAS GERTAKARIAK,G8D,CARACTERISTICAS (select,update,insert) Y ESTRUCTURA,RESPONSABLES_GERTAKARIAK,EQUIPORESOLUCION,G8D_E56 (select)
                    idGtk = oracleDB.updateInGTK(rec, con)
                    transact.Commit()
                    MvcApplication.Loguear("Info", "RECLAMACIONES . Edit (after)", User.Identity.Name.ToString,
                                           "   Reclamation id: " & rec.ID,
                                           "   Reclamation idCreador: " & rec.CREADOR,
                                           "   Reclamation descripcion: " & rec.DESCRIPCION,
                                           "   Reclamation refinternapieza: " & rec.REFINTERNAPIEZA,
                                           "   Reclamation idCliente: " & rec.CLIENTE,
                                           "   Reclamation idProducto: " & rec.PRODUCTO,
                                           "   Reclamation codcliente: " & rec.CODXCLIENTE,
                                           "   Reclamation numpiezasnok: " & rec.NUMPIEZASNOK,
                                           "   Reclamation procedencia: " & rec.PROCEDENCIA,
                                           "   Reclamation clasificacion: " & rec.CLASIFICACION,
                                           "   Reclamation nivelimportancia: " & rec.NIVELIMPORTANCIA,
                                           "   Reclamation repetitiva: " & rec.REPETITIVA,
                                           "   Reclamation oficial: " & rec.RECLAMACIONOFICIAL,
                                           "   Assigned NC id: " & idGtk)
                Catch ex As Exception
                    transact.Rollback()
                    MvcApplication.Loguear("Error", "RECLAMACIONES . Edit", User.Identity.Name.ToString, ex.StackTrace)
                    Return RedirectToAction("MyError", "Home")
                Finally
                    If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                        con.Close()
                        con.Dispose()
                    End If
                End Try
                Return RedirectToAction(TempData.Peek("ReturnUrl"))
            Else
                Return RedirectToAction("Edit", rec.ID)
            End If
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Details(ByVal id As Integer?) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            'Dim rec As RECLAMACIONES = db.RECLAMACIONES.Where(Function(o) o.ID = id).FirstOrDefault
            Dim rec As RECLAMACIONES = db.RECLAMACIONES.Find(id)

            If IsNothing(rec) Then
                Return HttpNotFound()
            End If
            Return View(rec)
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            'Dim RECLAMACION As RECLAMACIONES = db.RECLAMACIONES.Where(Function(o) o.ID = id).FirstOrDefault
            Dim RECLAMACION As RECLAMACIONES = db.RECLAMACIONES.Find(id)

            If IsNothing(RECLAMACION) Then
                Return HttpNotFound()
            End If
            If Not myDatabase.hasPermission(RECLAMACION.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . Delete", User.Identity.Name.ToString, "   Reclamation id: " & id, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim query As String = String.Empty
                con = New OracleConnection(cnStrBezerresis)
                con.Open()
                transact = con.BeginTransaction()
                Dim oracleDB As New oracleDB
                Dim idGtk = ""
                idGtk = oracleDB.getNumeroGtk(id)
                db.RECLAMACIONES.Remove(RECLAMACION)

                'Dim RECLAMACION_CIERRE As RECLAMACIONES_CIERRE = db.RECLAMACIONES_CIERRE.Where(Function(o) o.ID = id).FirstOrDefault
                Dim RECLAMACION_CIERRE As RECLAMACIONES_CIERRE = db.RECLAMACIONES_CIERRE.Find(id)

                If Not IsNothing(RECLAMACION_CIERRE) Then
                    db.RECLAMACIONES_CIERRE.Remove(RECLAMACION_CIERRE)
                End If
                db.SaveChanges()

                MvcApplication.Loguear("Info", "RECLAMACIONES . Delete", User.Identity.Name.ToString,
                                       "   Reclamation id: " & id,
                                       "   Reclamation idCreador: " & RECLAMACION.CREADOR,
                                       "   Reclamation descripcion: " & RECLAMACION.DESCRIPCION,
                                       "   Reclamation refinternapieza: " & RECLAMACION.REFINTERNAPIEZA,
                                       "   Reclamation idCliente: " & RECLAMACION.CLIENTE,
                                       "   Reclamation idProducto: " & RECLAMACION.PRODUCTO,
                                       "   Reclamation codcliente: " & RECLAMACION.CODXCLIENTE,
                                       "   Reclamation numpiezasnok: " & RECLAMACION.NUMPIEZASNOK,
                                       "   Reclamation procedencia: " & RECLAMACION.PROCEDENCIA,
                                       "   Reclamation clasificacion: " & RECLAMACION.CLASIFICACION,
                                       "   Reclamation nivelimportancia: " & RECLAMACION.NIVELIMPORTANCIA,
                                       "   Reclamation repetitiva: " & RECLAMACION.REPETITIVA,
                                       "   Reclamation oficial: " & RECLAMACION.RECLAMACIONOFICIAL,
                                       "   Assigned NC id (not deleted): " & idGtk,
                                       "   Reclamation fechaCierrecliente: " & RECLAMACION_CIERRE?.FECHA_CIERRECLIENTE,
                                       "   Reclamation fechaRespuestaContencion: " & RECLAMACION_CIERRE?.FECHA_RESP_CONTENCION,
                                       "   Reclamation fechaRespuestaCorrectiva: " & RECLAMACION_CIERRE?.FECHA_RESP_CORRECTIVAS,
                                       "   Reclamation costeRevisionCliente: " & RECLAMACION_CIERRE?.COSTE_REVISIONCLIENTE,
                                       "   Reclamation costeRevisionInterna: " & RECLAMACION_CIERRE?.COSTE_REVISIONINTERNA,
                                       "   Reclamation costeMaterialesChatarra: " & RECLAMACION_CIERRE?.COSTE_MATERIALESCHATARRA,
                                       "   Reclamation costeCargosCliente: " & RECLAMACION_CIERRE?.COSTE_CARGOSCLIENTE,
                                       "   Reclamation costeOtros: " & RECLAMACION_CIERRE?.COSTE_OTROS,
                                       "   Reclamation costeOtrosDescripcion: " & RECLAMACION_CIERRE?.COSTE_OTROS_DESCRIPCION)
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                MvcApplication.Loguear("Error", "RECLAMACIONES . Delete", User.Identity.Name.ToString, ex.StackTrace)
                Return RedirectToAction("MyError", "Home")
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return RedirectToAction(TempData.Peek("ReturnUrl"))
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Close(ByVal id As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            ' Recuperar la reclamación principal
            Dim rec2 As RECLAMACIONES = db.RECLAMACIONES.Find(id)
            If rec2 Is Nothing Then
                Return HttpNotFound()
            End If

            ' Asignar la clasificación al ViewBag
            ViewBag.Classification = rec2.CLASIFICACION

            ' Recuperar la reclamación de cierre asociada, si existe
            Dim rec As RECLAMACIONES_CIERRE = rec2.RECLAMACIONES_CIERRE

            ' Si no existe una reclamación de cierre, crear una nueva
            If IsNothing(rec) Then
                rec = New RECLAMACIONES_CIERRE With {.ID = id}
            End If

            ' Verificar permisos
            If Not myDatabase.hasPermission(rec2.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . Close (before)", User.Identity.Name.ToString, "   Reclamation id: " & id, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If

            ' Registrar en el log
            MvcApplication.Loguear("Info", "RECLAMACIONES . Close (before)", User.Identity.Name.ToString,
                           "   Reclamation id: " & id,
                           "   Reclamation fechaCierrecliente: " & rec?.FECHA_CIERRECLIENTE,
                           "   Reclamation fechaRespuestaContencion: " & rec?.FECHA_RESP_CONTENCION,
                           "   Reclamation fechaRespuestaCorrectiva: " & rec?.FECHA_RESP_CORRECTIVAS,
                           "   Reclamation costeRevisionCliente: " & rec?.COSTE_REVISIONCLIENTE,
                           "   Reclamation costeRevisionInterna: " & rec?.COSTE_REVISIONINTERNA,
                           "   Reclamation costeMaterialesChatarra: " & rec?.COSTE_MATERIALESCHATARRA,
                           "   Reclamation costeCargosCliente: " & rec?.COSTE_CARGOSCLIENTE,
                           "   Reclamation costeOtros: " & rec?.COSTE_OTROS,
                           "   Reclamation costeOtrosDescripcion: " & rec?.COSTE_OTROS_DESCRIPCION)

            ' Obtener la lista de archivos desde el sistema de archivos
            Dim rutaBase As String = ConfigurationManager.AppSettings("RutaArchivosReclamaciones")
            Dim rutaCarpetaReclamacion As String = Path.Combine(rutaBase, rec.ID.ToString())
            Dim archivos As List(Of String) = New List(Of String)()

            If Directory.Exists(rutaCarpetaReclamacion) Then
                archivos = Directory.GetFiles(rutaCarpetaReclamacion).Select(Function(f) Path.GetFileName(f)).ToList()
            End If

            ' Pasar la lista de archivos al ViewBag
            ViewBag.Archivos = archivos

            ' Pasar el modelo a la vista
            Return View("Close", rec)
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        <HttpPost()>
        Function Close(ByVal rec As RECLAMACIONES_CIERRE, ByVal uploadedFiles As IEnumerable(Of HttpPostedFileBase)) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            If ModelState.IsValid Then
                Dim con As OracleConnection = Nothing
                Dim transact As OracleTransaction = Nothing
                Dim oraDB As New oracleDB
                Try
                    con = New OracleConnection(cnStrBezerresis)
                    con.Open()
                    transact = con.BeginTransaction()
                    Dim oracleDB As New oracleDB

                    ' Actualizar o agregar la reclamación de cierre
                    Dim temp = db.RECLAMACIONES_CIERRE.Find(rec.ID)
                    If temp Is Nothing Then
                        db.RECLAMACIONES_CIERRE.Add(rec)
                    Else
                        db.RECLAMACIONES_CIERRE.Remove(temp)
                        db.SaveChanges()
                        db.RECLAMACIONES_CIERRE.Add(rec)
                    End If

                    ' Manejar la lógica de GTK
                    If rec.FECHA_CIERRECLIENTE IsNot Nothing AndAlso rec.FECHA_CIERRECLIENTE > Date.MinValue Then
                        oraDB.cerrarEnGTK(rec.ID, rec.FECHA_CIERRECLIENTE)
                    Else
                        oraDB.abrirEnGTK(rec.ID)
                    End If

                    db.SaveChanges()

                    ' Nueva funcionalidad: Guardar archivos subidos
                    Dim rutaBase As String = ConfigurationManager.AppSettings("RutaArchivosReclamaciones")
                    Dim rutaCarpetaReclamacion As String = System.IO.Path.Combine(rutaBase, rec.ID.ToString())
                    If Not System.IO.Directory.Exists(rutaCarpetaReclamacion) Then
                        System.IO.Directory.CreateDirectory(rutaCarpetaReclamacion)
                    End If

                    For Each uploadedFile In uploadedFiles
                        If uploadedFile IsNot Nothing AndAlso uploadedFile.ContentLength > 0 Then
                            Dim filePath As String = System.IO.Path.Combine(rutaCarpetaReclamacion, System.IO.Path.GetFileName(uploadedFile.FileName))
                            ' Usar System.IO para guardar el archivo
                            Using fileStream As System.IO.FileStream = System.IO.File.Create(filePath)
                                uploadedFile.InputStream.CopyTo(fileStream)
                            End Using
                        End If
                    Next


                    transact.Commit()

                    ' Registrar en el log
                    MvcApplication.Loguear("Info", "RECLAMACIONES . Close (after)", User.Identity.Name.ToString,
                                   "   Reclamation id: " & rec.ID,
                                   "   Reclamation fechaCierrecliente: " & rec.FECHA_CIERRECLIENTE,
                                   "   Reclamation fechaRespuestaContencion: " & rec.FECHA_RESP_CONTENCION,
                                   "   Reclamation fechaRespuestaCorrectiva: " & rec.FECHA_RESP_CORRECTIVAS,
                                   "   Reclamation costeRevisionCliente: " & rec.COSTE_REVISIONCLIENTE,
                                   "   Reclamation costeRevisionInterna: " & rec.COSTE_REVISIONINTERNA,
                                   "   Reclamation costeMaterialesChatarra: " & rec.COSTE_MATERIALESCHATARRA,
                                   "   Reclamation costeCargosCliente: " & rec.COSTE_CARGOSCLIENTE,
                                   "   Reclamation costeOtros: " & rec.COSTE_OTROS,
                                   "   Reclamation costeOtrosDescripcion: " & rec.COSTE_OTROS_DESCRIPCION)
                Catch ex As Exception
                    transact.Rollback()
                    MvcApplication.Loguear("Error", "RECLAMACIONES . Close", User.Identity.Name.ToString, ex.StackTrace)
                    Return RedirectToAction("MyError", "Home")
                Finally
                    If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                        con.Close()
                        con.Dispose()
                    End If
                End Try

                Return RedirectToAction(TempData.Peek("ReturnUrl"))
            Else
                ' Si hay errores de validación, reasignar ViewBag.Classification antes de volver a mostrar la vista
                ViewBag.Classification = db.RECLAMACIONES.Find(rec.ID).CLASIFICACION

                ' Recuperar la lista de archivos nuevamente
                Dim rutaBase As String = ConfigurationManager.AppSettings("RutaArchivosReclamaciones")
                Dim rutaCarpetaReclamacion As String = Path.Combine(rutaBase, rec.ID.ToString())
                Dim archivos As List(Of String) = New List(Of String)()

                If Directory.Exists(rutaCarpetaReclamacion) Then
                    archivos = Directory.GetFiles(rutaCarpetaReclamacion).Select(Function(f) Path.GetFileName(f)).ToList()
                End If

                ViewBag.Archivos = archivos

                Return View("Close", rec)
            End If
        End Function


        'Private Sub cerrarEnGTK(id As Decimal, fechaCierre As Date?)
        '    cambiarFechaCierreEnGTK(id, fechaCierre)
        'End Sub

        'Private Sub abrirEnGTK(id As Decimal)
        '    cambiarFechaCierreEnGTK(id, Nothing)
        'End Sub

        'Private Sub cambiarFechaCierreEnGTK(id As Decimal, fechaCierre As Date?)
        '    Dim query As String = "UPDATE GERTAKARIAK SET FECHA_CIERRE=:FECHACIERRE WHERE ID_BEZERRESIS=:ID"
        '    Dim lParam As New List(Of OracleParameter)
        '    lParam.Add(New OracleParameter("FECHACIERRE", OracleDbType.Date, If(fechaCierre, DBNull.Value), ParameterDirection.Input))
        '    lParam.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        '    Memcached.OracleDirectAccess.NoQuery(query, ConfigurationManager.ConnectionStrings("BEZERRESIS").ConnectionString, lParam.ToArray)
        'End Sub

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Reassign2(ByVal id As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            'Dim rec As RECLAMACIONES = db.RECLAMACIONES.Where(Function(o) o.ID = id).FirstOrDefault
            Dim rec As RECLAMACIONES = db.RECLAMACIONES.Find(id)

            If IsNothing(rec) Then
                Return HttpNotFound()
            End If
            If Not myDatabase.hasPermission(rec.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . Reassign (before)", User.Identity.Name.ToString, "   Reclamation id: " & id, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If
            MvcApplication.Loguear("Info", "RECLAMACIONES . Reassign (before)", User.Identity.Name.ToString, "   Reclamation id: " & id)
            Return View(rec)
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Reassign3(ByVal idGtk As Integer, ByVal idBezerresis As Integer) 'As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim gtkId = idGtk
            Dim bezerresisId = idBezerresis

            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim query As String = String.Empty
                con = New OracleConnection(cnStrBezerresis)
                con.Open()
                transact = con.BeginTransaction()
                Dim oracleDB As New oracleDB
                oracleDB.vincularIncidencia(gtkId, bezerresisId, con)
                oracleDB.copiarDatosVinculacion(CInt(gtkId), CInt(bezerresisId), con)
                MvcApplication.Loguear("Info", "RECLAMACIONES . Reassign (after)", User.Identity.Name.ToString,
                                   "   Reclamation id: " & bezerresisId,
                                   "   Assigned NC id: " & gtkId)
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                MvcApplication.Loguear("Error", "RECLAMACIONES . Reassign", User.Identity.Name.ToString, ex.StackTrace)
                Return RedirectToAction("MyError", "Home")
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return RedirectToAction(TempData.Peek("ReturnUrl"))
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Unassign(ByVal id As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            'Dim rec As RECLAMACIONES = db.RECLAMACIONES.Where(Function(o) o.ID = id).FirstOrDefault
            Dim rec As RECLAMACIONES = db.RECLAMACIONES.Find(id)

            If Not myDatabase.hasPermission(rec.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . Unassign", User.Identity.Name.ToString, "   Reclamation id: " & id, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If
            oracleDatabase.desvincularIncidencia(id)
            Return RedirectToAction(TempData.Peek("ReturnUrl"))
        End Function

        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function Index_Filtro() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            aCookie.Values("IdCliente") = Request("lClientes")
            aCookie.Values("IdProducto") = Request("lProductos")
            aCookie.Values("productosAll") = Request("productosAll")?.Contains("active")
            aCookie.Values("IdCreador") = Request("lCreadores")
            aCookie.Values("IdEstado") = Request("lEstados")
            'aCookie.Values("FECHA_DESDE") = Request("FECHA_DESDE")
            'aCookie.Values("FECHA_HASTA") = Request("FECHA_HASTA")
            aCookie.Values("FECHA_DESDE") = DateTime.Parse(Request("FECHA_DESDE")).ToString("yyyy/MM/dd")
            aCookie.Values("FECHA_HASTA") = DateTime.Parse(Request("FECHA_HASTA")).ToString("yyyy/MM/dd")
            Response.Cookies.Add(aCookie)
            Return RedirectToAction(TempData.Peek("ReturnUrl"))
        End Function

        ' Acción para descargar archivos
        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        Function DownloadArchivo(ByVal reclamacionId As Integer, ByVal nombreArchivo As String) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            ' Validar los parámetros de entrada
            If String.IsNullOrEmpty(nombreArchivo) OrElse reclamacionId <= 0 Then
                Return HttpNotFound()
            End If

            ' Sanitizar el nombre del archivo para evitar vulnerabilidades de ruta
            Dim sanitizedFileName As String = Path.GetFileName(nombreArchivo)
            Dim rutaBase As String = ConfigurationManager.AppSettings("RutaArchivosReclamaciones")
            Dim rutaCarpetaReclamacion As String = Path.Combine(rutaBase, reclamacionId.ToString())
            Dim rutaArchivo As String = Path.Combine(rutaCarpetaReclamacion, sanitizedFileName)

            ' Verificar si el archivo existe
            If Not System.IO.File.Exists(rutaArchivo) Then
                Return HttpNotFound()
            End If

            ' Verificar permisos del usuario
            Dim reclamacion As RECLAMACIONES = db.RECLAMACIONES.Find(reclamacionId)
            If reclamacion Is Nothing OrElse Not myDatabase.hasPermission(reclamacion.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . DownloadArchivo", User.Identity.Name.ToString, "   Reclamation id: " & reclamacionId, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If

            ' Leer el archivo y devolverlo al cliente
            Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(rutaArchivo)
            Dim mimeType As String = MimeMapping.GetMimeMapping(sanitizedFileName)
            Return File(fileBytes, mimeType, sanitizedFileName)
        End Function

        ' Acción para eliminar archivos
        <PlantaSeleccionadaFilterAttribute>
        <Authorize(Roles:="Usuario")>
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function DeleteArchivo(ByVal reclamacionId As Integer, ByVal nombreArchivo As String) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")

            ' Validar los parámetros de entrada
            If String.IsNullOrEmpty(nombreArchivo) OrElse reclamacionId <= 0 Then
                Return HttpNotFound()
            End If

            ' Sanitizar el nombre del archivo para evitar vulnerabilidades de ruta
            Dim sanitizedFileName As String = Path.GetFileName(nombreArchivo)
            Dim rutaBase As String = ConfigurationManager.AppSettings("RutaArchivosReclamaciones")
            Dim rutaCarpetaReclamacion As String = Path.Combine(rutaBase, reclamacionId.ToString())
            Dim rutaArchivo As String = Path.Combine(rutaCarpetaReclamacion, sanitizedFileName)

            ' Verificar si el archivo existe
            If Not System.IO.File.Exists(rutaArchivo) Then
                Return HttpNotFound()
            End If

            ' Verificar permisos del usuario
            Dim reclamacion As RECLAMACIONES = db.RECLAMACIONES.Find(reclamacionId)
            If reclamacion Is Nothing OrElse Not myDatabase.hasPermission(reclamacion.CREADOR, Session("idUser")) Then
                MvcApplication.Loguear("Warn", "RECLAMACIONES . DeleteArchivo", User.Identity.Name.ToString, "   Reclamation id: " & reclamacionId, "   UNAUTHORIZED")
                Return RedirectToAction("Unauthorized", "Home")
            End If

            Try
                ' Eliminar el archivo del sistema de archivos
                System.IO.File.Delete(rutaArchivo)
                MvcApplication.Loguear("Info", "RECLAMACIONES . DeleteArchivo", User.Identity.Name.ToString,
                           "   Reclamation id: " & reclamacionId,
                           "   Nombre del archivo eliminado: " & sanitizedFileName)
                TempData("SuccessMessage") = "Archivo eliminado correctamente."
            Catch ex As Exception
                ' Manejar errores y registrar en el log
                MvcApplication.Loguear("Error", "RECLAMACIONES . DeleteArchivo", User.Identity.Name.ToString, ex.Message)
                TempData("ErrorMessage") = "Error al eliminar el archivo. Por favor, inténtalo de nuevo."
            End Try

            ' Redirigir de vuelta a la vista Close
            Return RedirectToAction("Close", New With {.id = reclamacionId})
        End Function


#Region "Consultas BRAIN"

        <Authorize(Roles:="Usuario")>
        Function Suggest(term As String, emp As String) As ActionResult
            Dim result = brainDatabase.getSuggestions(term, emp)
            Return Json(result, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        <Authorize(Roles:="Usuario")>
        Function GetDataFromPieza(ByVal input As String, ByVal emp As String) As ActionResult
            Dim result = brainDatabase.getDataFromPieza(input, emp)
            If result Is Nothing Then
                result = {"N/A", "N/A", "N/A", Nothing}
            Else
                Dim oracleDatabase As New oracleDB
                result(3) = oracleDatabase.getProductoIdFromName(result(3), emp)
            End If
            Return Json(result, JsonRequestBehavior.AllowGet)
        End Function

#End Region

#Region "Consultas Oracle"

        <HttpPost()>
        <Authorize(Roles:="Usuario")>
        Function GetProductos(ByVal input1 As String, ByVal input2 As String) As ActionResult
            Dim result As List(Of SelectListItem)
            If input2.ToUpper.Contains("BATZ ") OrElse input2.ToUpper.Contains("FPK BRASIL") Then
                Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
                Dim idPlanta = aCookie.Values("IdPlanta")
                result = oracleDatabase.getProductosForPlantaLogueada(idPlanta)
            Else
                result = oracleDatabase.getProductosForCliente(input1, input2)
            End If
            Return Json(result, JsonRequestBehavior.AllowGet)
        End Function

#End Region

    End Class
End Namespace
