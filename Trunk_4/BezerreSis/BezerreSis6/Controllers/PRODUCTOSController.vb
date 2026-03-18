Imports System.Data.Entity
Imports System.Net

Namespace Controllers
    Public Class PRODUCTOSController
        Inherits System.Web.Mvc.Controller

        Private db As New Entities_BezerreSis
        Dim IdPlanta As New Nullable(Of Decimal)

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Index() As ActionResult
            Try
                If Session("Home") Then Return RedirectToAction("Index", "Home")
                Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
                IdPlanta = aCookie.Values("IdPlanta")
                Return View(db.PRODUCTOS.Where(Function(o) o.IDPLANTA = IdPlanta).OrderBy(Function(o) o.NOMBRE).ToList())
            Catch ex As ApplicationException
                MvcApplication.log.Debug(ex)
                Return RedirectToAction("Index", "Home")
            Catch ex As Exception
                MvcApplication.log.Error(ex)
                Return RedirectToAction("Index", "Home")
            End Try
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Details(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim pRODUCTOS As PRODUCTOS = db.PRODUCTOS.Find(id)
            If IsNothing(pRODUCTOS) Then
                Return HttpNotFound()
            End If
            Return View(pRODUCTOS)
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Create() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Return View()
        End Function

        <HttpPost()>
        <PlantaSeleccionadaFilter>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Usuario")>
        Function Create(<Bind(Include:="ID,CODIGO,NOMBRE,IDPLANTA")> ByVal pRODUCTOS As PRODUCTOS) As ActionResult
            'Function Create(<Bind(Include:="ID,CODIGO,NOMBRE,IDPLANTA,NOMBREBRAIN")> ByVal pRODUCTOS As PRODUCTOS) As ActionResult
            Try
                If Session("Home") Then Return RedirectToAction("Index", "Home")
                If ModelState.IsValid Then
                    Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
                    IdPlanta = aCookie.Values("IdPlanta")
                    pRODUCTOS.IDPLANTA = IdPlanta
                    db.PRODUCTOS.Add(pRODUCTOS)
                    db.SaveChanges()
                    MvcApplication.Loguear("Info", "PRODUCTOS . Create (after)", User.Identity.Name.ToString,
                                           "   Product id: " & pRODUCTOS.ID,
                                           "   Product nombre: " & pRODUCTOS.NOMBRE,
                                           "   Product idPlanta: " & pRODUCTOS.IDPLANTA)
                    '"   Product nombreBrain: " & pRODUCTOS.NOMBREBRAIN,
                    Return RedirectToAction("Index")
                End If
                Return View(pRODUCTOS)
            Catch ex As ApplicationException
                MvcApplication.log.Error(ex)
                Return RedirectToAction("Index", "Home")
            Catch ex As Exception
                MvcApplication.log.Error(ex)
                Return RedirectToAction("Index")
            End Try
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Edit(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim pRODUCTOS As PRODUCTOS = db.PRODUCTOS.Find(id)
            MvcApplication.Loguear("Info", "PRODUCTOS . Edit (before)", User.Identity.Name.ToString,
                                   "   Product id: " & id,
                                   "   Product nombre: " & pRODUCTOS.NOMBRE,
                                   "   Product idPlanta: " & pRODUCTOS.IDPLANTA)
            '"   Product nombreBrain: " & pRODUCTOS.NOMBREBRAIN,
            If IsNothing(pRODUCTOS) Then
                Return HttpNotFound()
            End If
            Return View(pRODUCTOS)
        End Function

        <HttpPost()>
        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Edit(<Bind(Include:="ID,CODIGO,NOMBRE,IDPLANTA")> ByVal pRODUCTOS As PRODUCTOS) As ActionResult
            'Function Edit(<Bind(Include:="ID,CODIGO,NOMBRE,IDPLANTA,NOMBREBRAIN")> ByVal pRODUCTOS As PRODUCTOS) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Try
                If ModelState.IsValid Then
                    db.Entry(pRODUCTOS).State = EntityState.Modified
                    db.SaveChanges()
                    MvcApplication.Loguear("Info", "PRODUCTOS . Edit (after)", User.Identity.Name.ToString,
                                           "   Product id: " & pRODUCTOS.ID,
                                           "   Product nombre: " & pRODUCTOS.NOMBRE,
                                           "   Product idPlanta: " & pRODUCTOS.IDPLANTA)
                    '"   Product nombreBrain: " & pRODUCTOS.NOMBREBRAIN,
                    Return RedirectToAction("Index")
                End If
                Return View(pRODUCTOS)
            Catch ex As ApplicationException
                MvcApplication.log.Debug(ex)
                Return RedirectToAction("Index", "Home")
            Catch ex As Exception
                MvcApplication.log.Error(ex)
                Return RedirectToAction("Index")
            End Try
        End Function

        <PlantaSeleccionadaFilter>
        <Authorize(Roles:="Usuario")>
        Function Delete(ByVal id As Decimal) As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim pRODUCTOS As PRODUCTOS = db.PRODUCTOS.Find(id)
            If IsNothing(pRODUCTOS) Then
                Return HttpNotFound()
            End If
            db.PRODUCTOS.Remove(pRODUCTOS)
            db.SaveChanges()
            MvcApplication.Loguear("Info", "PRODUCTOS . Delete", User.Identity.Name.ToString,
                                   "   Product id: " & id,
                                   "   Product nombre: " & pRODUCTOS.NOMBRE,
                                   "   Product idPlanta: " & pRODUCTOS.IDPLANTA)
            '"   Product nombreBrain: " & pRODUCTOS.NOMBREBRAIN,
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
