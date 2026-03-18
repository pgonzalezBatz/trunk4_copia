Imports System.Data.Entity
Imports System.Net

Namespace Controllers
    Public Class CLIENTESController
        Inherits System.Web.Mvc.Controller

        Private db As New Entities_BezerreSis
        Dim FiltroGTK As New gtkFiltro
        Dim IdPlanta As New Nullable(Of Decimal)

        <PlantaSeleccionadaFilter()>
        <Authorize(Roles:="Usuario")>
        Function Index() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            IdPlanta = aCookie.Values("IdPlanta")
            Return View(db.CLIENTES.Where(Function(o) o.IDPLANTA = IdPlanta).OrderBy(Function(o) Not o.NOMBRE.Contains("default")).ThenBy(Function(o) o.NOMBRE).ToList())
        End Function

        <PlantaSeleccionadaFilter()>
        <Authorize(Roles:="Usuario")>
        Function Details(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim cLIENTES As CLIENTES = db.CLIENTES.Find(id)
            If IsNothing(cLIENTES) Then
                Return HttpNotFound()
            End If
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            aCookie.Values("IdCliente") = id
            Response.Cookies.Add(aCookie)
            Return View(cLIENTES)
        End Function

        <PlantaSeleccionadaFilter()>
        <Authorize(Roles:="Usuario")>
        Function Create() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Return View()
        End Function

        <PlantaSeleccionadaFilter()>
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        Function Create(<Bind(Include:="ID,NOMBRE")> ByVal cLIENTES As CLIENTES) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                If ModelState.IsValid Then
                    Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
                    IdPlanta = aCookie.Values("IdPlanta")
                    cLIENTES.IDPLANTA = IdPlanta
                    db.CLIENTES.Add(cLIENTES)
                    db.SaveChanges()
                    aCookie.Values("IdCliente") = cLIENTES.ID
                    Response.Cookies.Add(aCookie)
                    MvcApplication.Loguear("Info", "CLIENTES . Create (after)", User.Identity.Name.ToString,
                                           "   Client id: " & cLIENTES.ID,
                                           "   Client nombre: " & cLIENTES.NOMBRE,
                                           "   Client idPlanta: " & cLIENTES.IDPLANTA)
                    Return RedirectToAction("Index")
                End If
                Return View(cLIENTES)
            Catch ex As ApplicationException
                MvcApplication.log.Debug(ex)
                Return RedirectToAction("Index", "Home")
            Catch ex As Exception
                If ex.InnerException.ToString.Contains("CLIENTES_UK1") Then
                    ModelState.AddModelError("NOMBREBRAIN", "El valor 'NOMBREBRAIN' ya existe para esta planta, elige otro")
                    Return View(cLIENTES)
                Else
                    MvcApplication.log.Error(ex)
                    Return RedirectToAction("Index")
                End If
            End Try
        End Function

        <PlantaSeleccionadaFilter()>
        <Authorize(Roles:="Usuario")>
        Function Edit(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim cLIENTES As CLIENTES = db.CLIENTES.Find(id)
            If IsNothing(cLIENTES) Then
                Return HttpNotFound()
            End If
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            aCookie.Values("IdCliente") = id
            Response.Cookies.Add(aCookie)
            MvcApplication.Loguear("Info", "CLIENTES . Edit (before)", User.Identity.Name.ToString,
                                           "   Client id: " & id,
                                           "   Client nombre: " & cLIENTES.NOMBRE,
                                           "   Client idPlanta: " & cLIENTES.IDPLANTA)
            Return View(cLIENTES)
        End Function

        <PlantaSeleccionadaFilter()>
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        Function Edit(<Bind(Include:="ID,NOMBRE,IDPLANTA")> ByVal cLIENTES As CLIENTES) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                If ModelState.IsValid Then
                    db.Entry(cLIENTES).State = EntityState.Modified
                    db.SaveChanges()
                    MvcApplication.Loguear("Info", "CLIENTES . Edit (after)", User.Identity.Name.ToString,
                                           "   Client id: " & cLIENTES.ID,
                                           "   Client nombre: " & cLIENTES.NOMBRE,
                                           "   Client idPlanta: " & cLIENTES.IDPLANTA)
                    Return RedirectToAction("Index")
                End If
                Return View(cLIENTES)
            Catch ex As ApplicationException
                MvcApplication.log.Debug(ex)
                Return RedirectToAction("Index", "Home")
            Catch ex As Exception
                If ex.InnerException.ToString.Contains("CLIENTES_UK1") Then
                    ModelState.AddModelError("NOMBREBRAIN", "El valor 'NOMBREBRAIN' ya existe para esta planta, elige otro")
                    Return View(cLIENTES)
                Else
                    MvcApplication.log.Error(ex)
                    Return RedirectToAction("Index")
                End If
            End Try
        End Function

        <PlantaSeleccionadaFilter()>
        <Authorize(Roles:="Usuario")>
        Function Delete(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim cLIENTES As CLIENTES = db.CLIENTES.Find(id)
            If IsNothing(cLIENTES) Then
                Return HttpNotFound()
            End If
            db.CLIENTES.Remove(cLIENTES)
            db.SaveChanges()
            MvcApplication.Loguear("Info", "CLIENTES . Delete", User.Identity.Name.ToString,
                                           "   Client id: " & id,
                                           "   Client nombre: " & cLIENTES.NOMBRE,
                                           "   Client idPlanta: " & cLIENTES.IDPLANTA)
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            aCookie.Values("IdCliente") = String.Empty
            Response.Cookies.Add(aCookie)
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        <PlantaSeleccionadaFilter()>
        <Authorize(Roles:="Usuario")>
        Function Cli_Pro(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
                IdPlanta = aCookie.Values("IdPlanta")
                Dim lPro = db.PRODUCTOS.Where(Function(o) o.IDPLANTA = IdPlanta).ToList.Where(Function(Producto) Not db.CLIENTES.Find(id).CLIPRO.Select(Function(CliPro) CliPro.PRODUCTOS).Contains(Producto))
                ViewData("lProductos") = lPro.Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID}).OrderBy(Function(o) o.Text).ToList
                Return Details(id)
            Catch ex As ApplicationException
                MvcApplication.log.Debug(ex)
                Return RedirectToAction("Index", "Home")
            Catch ex As Exception
                MvcApplication.log.Error(ex)
                Return RedirectToAction("Index", "Home")
            End Try
        End Function

        <PlantaSeleccionadaFilter()>
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        Function Guardar(ByVal id As Decimal, lProductos As Nullable(Of Integer)) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim Cli As CLIENTES = db.CLIENTES.Find(id)
            Dim Proc As PRODUCTOS = db.PRODUCTOS.Find(lProductos)
            If Cli IsNot Nothing And lProductos IsNot Nothing Then
                Cli.CLIPRO.Add(New CLIPRO With {.IDCLI = Cli.ID, .IDPRO = Proc.ID})
                db.SaveChanges()
                MvcApplication.Loguear("Info", "CLIENTES . Guardar (added product to client)", User.Identity.Name.ToString,
                                           "   Client id: " & Cli.ID,
                                           "   Client nombre: " & Cli.NOMBRE,
                                           "   Client idPlanta: " & Cli.IDPLANTA,
                                           "   Product id: " & Proc.ID,
                                           "   Product nombre: " & Proc.NOMBRE,
                                           "   Product idPlanta: " & Proc.IDPLANTA)
            End If
            Return RedirectToAction(String.Format("Details/{0}", id))
        End Function

    End Class
End Namespace
