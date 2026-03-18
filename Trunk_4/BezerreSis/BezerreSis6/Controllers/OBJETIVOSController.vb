Imports System.Data.Entity
Imports System.Net

Namespace Controllers
    Public Class OBJETIVOSController
        Inherits System.Web.Mvc.Controller

        Private db As New Entities_BezerreSis
        Dim FiltroGTK As New gtkFiltro
        Dim lObjetivos As IQueryable(Of BezerreSis.OBJETIVOS)
        Dim IdPlanta As New Nullable(Of Decimal)

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Index() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                CargarDatos_Index()
            Catch ex As ApplicationException
                Return RedirectToAction("Index", "Home")
            End Try
            Return View(lObjetivos)
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Proceso() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            'Dim objetivosProceso As New List(Of String())
            Dim objetivosProceso As New List(Of OBJETIVOPROCESO)
            Try
                objetivosProceso = CargarDatosProceso_Index()
            Catch ex As ApplicationException
                Return RedirectToAction("Index", "Home")
            End Try
            Return View(objetivosProceso)
        End Function


        <HttpPost()>
        <PlantaSeleccionadaFilter>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        Function Index(lClientes As Int32?) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                CargarDatos_Index()
            Catch ex As ApplicationException
                Return RedirectToAction("Index", "Home")
            End Try
            Return View(lObjetivos)
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Details(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim oBJETIVOS As OBJETIVOS = db.OBJETIVOS.Find(id)
            If IsNothing(oBJETIVOS) Then
                Return HttpNotFound()
            End If
            Return View(oBJETIVOS)
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Create(ByVal lClientes As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim Cliente As BezerreSis.CLIENTES = db.CLIENTES.Find(lClientes)
            ViewBag.Cliente = String.Format("{0} - ({1})", Cliente.NOMBRE, Cliente.PLANTAS.NOMBRE)
            Dim Modelo = New BezerreSis.OBJETIVOS With {.IDCLIENTE = lClientes}
            Modelo.Año = DateTime.Today.Year
            Return View(Modelo)
        End Function

        <HttpPost()>
        <PlantaSeleccionadaFilter>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        Function Create(<Bind(Include:="ID,IDCLIENTE,FECHA,PPM_ANUAL,PPM_MENSUAL,PRR_ANUAL,PRR_MENSUAL,IPB_SEMESTRAL,Año")> ByVal oBJETIVOS As OBJETIVOS) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                oBJETIVOS.FECHA = If(String.IsNullOrWhiteSpace(oBJETIVOS.Año), Nothing, New System.DateTime(oBJETIVOS.Año, 1, 1))
                If TryValidateModel(oBJETIVOS) Then
                    If ModelState.IsValid Then
                        db.OBJETIVOS.Add(oBJETIVOS)
                        db.SaveChanges()

                        MvcApplication.Loguear("Info", "OBJETIVOS . Create (after)", User.Identity.Name.ToString,
                                       "   Objetivo id: " & oBJETIVOS.ID,
                                       "   Objetivo idCliente: " & oBJETIVOS.IDCLIENTE,
                                       "   Objetivo ano: " & oBJETIVOS.Año,
                                       "   Objetivo ppmMensual: " & oBJETIVOS.PPM_MENSUAL,
                                       "   Objetivo ppmAnual: " & oBJETIVOS.PPM_ANUAL,
                                       "   Objetivo ipbSemestral: " & oBJETIVOS.IPB_SEMESTRAL)
                        Return RedirectToAction("Index", h.ToRouteValues(Request.QueryString, Nothing))
                    End If
                End If
                Dim Cliente As BezerreSis.CLIENTES = db.CLIENTES.Find(oBJETIVOS.IDCLIENTE)
                ViewBag.Cliente = String.Format("{0} - ({1})", Cliente.NOMBRE, Cliente.PLANTAS.NOMBRE)
                Return View(oBJETIVOS)
            Catch ex As Exception
                If ex.InnerException.ToString.Contains("OBJETIVOS_UK1") Then
                    ModelState.AddModelError("AÑO", "Ya existe un objetivo para este año")
                    MvcApplication.Loguear("Error", "OBJETIVOS . Create", User.Identity.Name.ToString, "Ya hay un objetivo para idcliente (" & oBJETIVOS.IDCLIENTE & ") y año (" & oBJETIVOS.Año & ")")
                    Return View(oBJETIVOS)
                Else
                    MvcApplication.log.Error(ex)
                    Return RedirectToAction("Index")
                End If
            End Try
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Edit(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim oBJETIVOS As OBJETIVOS = db.OBJETIVOS.Find(id)
            If IsNothing(oBJETIVOS) Then
                Return HttpNotFound()
            End If
            ViewBag.IDCLIENTE = New SelectList(db.CLIENTES, "ID", "NOMBRE", oBJETIVOS.IDCLIENTE)
            oBJETIVOS.Año = oBJETIVOS.FECHA.Year
            MvcApplication.Loguear("Info", "OBJETIVOS . Edit (before)", User.Identity.Name.ToString,
                                   "   Objetivo id: " & id,
                                   "   Objetivo idCliente: " & oBJETIVOS.IDCLIENTE,
                                   "   Objetivo ano: " & oBJETIVOS.Año,
                                   "   Objetivo ppmMensual: " & oBJETIVOS.PPM_MENSUAL,
                                   "   Objetivo ppmAnual: " & oBJETIVOS.PPM_ANUAL,
                                   "   Objetivo ipbSemestral: " & oBJETIVOS.IPB_SEMESTRAL)
            Return View(oBJETIVOS)
        End Function

        <PlantaSeleccionadaFilter>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        <HttpPost()>
        Function Edit(<Bind(Include:="ID,IDCLIENTE,PPM_ANUAL,PPM_MENSUAL,IPB_SEMESTRAL, Año")> ByVal oBJETIVOS As OBJETIVOS) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If ModelState.IsValid Then
                oBJETIVOS.FECHA = New Date(oBJETIVOS.Año, 1, 1)
                db.Entry(oBJETIVOS).State = EntityState.Modified
                db.SaveChanges()
                MvcApplication.Loguear("Info", "OBJETIVOS . Edit (after)", User.Identity.Name.ToString,
                                   "   Objetivo id: " & oBJETIVOS.ID,
                                   "   Objetivo idCcliente: " & oBJETIVOS.IDCLIENTE,
                                   "   Objetivo ano: " & oBJETIVOS.Año,
                                   "   Objetivo ppmMensual: " & oBJETIVOS.PPM_MENSUAL,
                                   "   Objetivo ppmAnual: " & oBJETIVOS.PPM_ANUAL,
                                   "   Objetivo ipbSemestral: " & oBJETIVOS.IPB_SEMESTRAL)
                Return RedirectToAction("Index")
            End If
            ViewBag.IDCLIENTE = New SelectList(db.CLIENTES, "ID", "NOMBRE", oBJETIVOS.IDCLIENTE)
            Return View(oBJETIVOS)
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function DeleteObjetivo(ByVal id As Decimal, ByVal lClientes As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim oBJETIVOS As OBJETIVOS = db.OBJETIVOS.Find(id)
            db.OBJETIVOS.Remove(oBJETIVOS)
            db.SaveChanges()
            MvcApplication.Loguear("Info", "OBJETIVOS . DeleteObjetivo", User.Identity.Name.ToString,
                                   "   Objetivo id: " & oBJETIVOS.ID,
                                   "   Objetivo idCliente: " & oBJETIVOS.IDCLIENTE,
                                   "   Objetivo ano: " & oBJETIVOS.FECHA.Year,
                                   "   Objetivo ppmMensual: " & oBJETIVOS.PPM_MENSUAL,
                                   "   Objetivo ppmAnual: " & oBJETIVOS.PPM_ANUAL,
                                   "   Objetivo ipbSemestral: " & oBJETIVOS.IPB_SEMESTRAL)
            Return RedirectToAction("Index", New With {.lClientes = lClientes})
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Sub CargarDatos_Index()
            Dim lClientes_Selected As New Nullable(Of Integer)
            Try
                Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
                IdPlanta = aCookie.Values("IdPlanta")
                If Request IsNot Nothing Then
                    If Not String.IsNullOrWhiteSpace(Request.Form("lClientes_obj")) Then
                        lClientes_Selected = Request.Form("lClientes_obj")
                    ElseIf Request.QueryString("lClientes_obj")?.Equals("") Then
                        lClientes_Selected = Nothing
                    ElseIf Not String.IsNullOrWhiteSpace(Request.QueryString("lClientes_obj")) Then
                        lClientes_Selected = Request.QueryString("lClientes_obj")
                    ElseIf Not String.IsNullOrWhiteSpace(Request.QueryString("lClientes_obj")) Then
                        lClientes_Selected = Request.QueryString("lClientes_obj")
                    ElseIf Not String.IsNullOrWhiteSpace(aCookie.Values("IdCliente_obj")) Then
                        lClientes_Selected = aCookie.Values("IdCliente_obj")
                    End If
                End If
                'ViewBag.lClientes_obj = db.CLIENTES.Where(Function(o) o.IDPLANTA = IdPlanta).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = (f.ID = lClientes_Selected)}).OrderBy(Function(o) o.Text).ToList.OrderBy(Function(f) f.Text)
                'ViewBag.lClientes_obj = db.CLIENTES.Where(Function(o) o.IDPLANTA = IdPlanta).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = (f.ID = lClientes_Selected)}).OrderBy(Function(o) Not o.Text.Contains("default")).ThenBy(Function(o) o.Text).ToList.OrderBy(Function(f) f.Text)
                ViewBag.lClientes_obj = db.CLIENTES.Where(Function(o) o.IDPLANTA = IdPlanta).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = (f.ID = lClientes_Selected)}).ToList.OrderBy(Function(o) Not o.Text.Contains("default")).ThenBy(Function(o) o.Text)
                ViewBag.lClientes_Selected_obj = lClientes_Selected
                lObjetivos = db.OBJETIVOS.Where(Function(o) o.IDCLIENTE = lClientes_Selected).OrderByDescending(Function(o) o.FECHA)
                ViewBag.lCliPro_obj = db.CLIPRO.Where(Function(o) o.IDCLI = lClientes_Selected).OrderBy(Function(o) o.PRODUCTOS.NOMBRE)
                ViewBag.IdPro_obj = Request("IdPro")
                aCookie.Values("IdCliente_obj") = If(lClientes_Selected Is Nothing, 0, lClientes_Selected)
                Response.Cookies.Add(aCookie)
            Catch ex As ApplicationException
                Throw
            End Try
        End Sub

        'Function CargarDatosProceso_Index() As List(Of String())
        Function CargarDatosProceso_Index() As List(Of OBJETIVOPROCESO)
            'Dim myList As List(Of String())
            Dim myList As List(Of OBJETIVOPROCESO)
            Try
                Dim oDb As New oracleDB
                myList = oDb.getObjetivosProceso()
            Catch ex As ApplicationException
                Throw
            End Try
            Return myList
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function CreateObjetivoProceso() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Return View("EditObjetivoProceso", Nothing)
        End Function


        <PlantaSeleccionadaFilter>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        <HttpPost()>
        Function CreateObjetivoProceso(<Bind(Include:="ID,ANNO,REPETITIVAS,DIAS14,DIAS56")> ByVal obj As OBJETIVOPROCESO) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            ModelState.Remove("ID")
            If ModelState.IsValid Then
                Dim cnStrBezerresis = System.Configuration.ConfigurationManager.ConnectionStrings("BEZERRESIS").ConnectionString
                Dim con As OracleConnection = Nothing
                Dim transact As OracleTransaction = Nothing
                Try
                    Dim query As String = String.Empty
                    con = New OracleConnection(cnStrBezerresis)
                    con.Open()
                    transact = con.BeginTransaction()
                    Dim oracleDB As New oracleDB

                    oracleDB.CreateObjetivoProceso(obj)
                Catch ex As Exception
                    If ex.Message.Contains("BEZERRESIS.OBJETIVOS_PROCESO_UK1") Then
                        ModelState.AddModelError("ANNO", "El año especificado ya existe en la base de datos.")
                        Return View("EditObjetivoProceso")
                    End If

                Finally
                    If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                        con.Close()
                        con.Dispose()
                    End If
                End Try
            End If
            Return RedirectToAction("Proceso")
        End Function


        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function EditObjetivoProceso(ByVal id As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim myData As OBJETIVOPROCESO
            Try
                Dim oDb As New oracleDB
                myData = oDb.getObjetivoProceso(id)
            Catch ex As ApplicationException
                Throw
            End Try

            Return View("EditObjetivoProceso", myData)
        End Function

        <PlantaSeleccionadaFilter>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        <HttpPost()>
        Function EditObjetivoProceso(ByVal obj As OBJETIVOPROCESO) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If ModelState.IsValid Then
                Dim cnStrBezerresis = System.Configuration.ConfigurationManager.ConnectionStrings("BEZERRESIS").ConnectionString
                Dim con As OracleConnection = Nothing
                Dim transact As OracleTransaction = Nothing
                Try
                    Dim query As String = String.Empty
                    con = New OracleConnection(cnStrBezerresis)
                    con.Open()
                    transact = con.BeginTransaction()
                    Dim oracleDB As New oracleDB

                    oracleDB.EditObjetivoProceso(obj)
                Catch ex As Exception

                Finally
                    If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                        con.Close()
                        con.Dispose()
                    End If
                End Try
            End If
            Return RedirectToAction("Proceso")
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function DeleteObjetivoProceso(ByVal id As Integer) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                Dim oDb As New oracleDB
                oDb.deleteObjetivoProceso(id)
            Catch ex As ApplicationException
                Throw
            End Try

            Return RedirectToAction("Proceso")
        End Function

    End Class
End Namespace
