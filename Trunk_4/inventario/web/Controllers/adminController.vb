Imports Neodynamic.SDK.Printing
Imports System.Security.Permissions
Namespace web
    Public Class adminController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("inventario").ConnectionString

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listtipo() As ActionResult
            Return View(db.GetListOfTipo(strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function addtipo() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function addtipo(nombre As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", h.Traducir("Es obligatorio introducir el nombre"))
            End If
            If ModelState.IsValid Then
                db.AddTipo(nombre, strCn)
                Return RedirectToAction("listtipo")
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function edittipo(idtipo As Integer) As ActionResult
            ViewData("nombre") = db.GetTipo(idtipo, strCn).Value
            Return View("addtipo")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function edittipo(idtipo As Integer, nombre As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", h.Traducir("Es obligatorio introducir el nombre"))
            End If
            If ModelState.IsValid Then
                db.EditTipo(idtipo, nombre, strCn)
                Return RedirectToAction("listtipo")
            End If
            ViewData("nombre") = nombre
            Return View("addtipo")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listmarca(idtipo As Integer) As ActionResult
            Return View(db.GetListOfMarca(idtipo, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function addmarca(idtipo As Integer) As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function addmarca(idtipo As Integer, nombre As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", h.Traducir("Es obligatorio introducir el nombre"))
            End If
            If ModelState.IsValid Then
                db.Addmarca(idtipo, nombre, strCn)
                Return RedirectToAction("listmarca", New With {.idtipo = idtipo})
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function editmarca(idtipo As Integer, idmarca As Integer) As ActionResult
            ViewData("nombre") = db.Getmarca(idmarca, strCn).Value
            Return View("addmarca")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function editmarca(idtipo As Integer, idmarca As Integer, nombre As String) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", h.Traducir("Es obligatorio introducir el nombre"))
            End If
            If ModelState.IsValid Then
                db.EditMarca(idmarca, nombre, strCn)
                Return RedirectToAction("listmarca", New With {.idtipo = idtipo})
            End If
            ViewData("nombre") = nombre
            Return View("addatributo")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function listmodelo(idtipo As Integer, idmarca As Integer) As ActionResult
            Return View(db.GetListOfModelo(idmarca, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function listAsignaciones(idtipo As Integer, idmarca As Integer, idmodelo As Integer) As ActionResult
            Return View(db.GetListOfUsuariosModelo(idmodelo, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function addmodelo(idtipo As Integer, idmarca As Integer) As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function addmodelo(idtipo As Integer, idmarca As Integer, nombre As String, precio As Nullable(Of Decimal)) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", h.Traducir("Es obligatorio introducir el nombre"))
            End If
            If Not precio.HasValue Then
                ModelState.AddModelError("precio", h.Traducir("Es obligatorio introducir el precio"))
            End If
            If ModelState.IsValid Then
                db.Addmodelo(idmarca, nombre, precio.Value, strCn)
                Return RedirectToAction("listmodelo", New With {.idtipo = idtipo, .idmarca = idmarca})
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function editmodelo(idtipo As Integer, idmarca As Integer, idmodelo As Integer) As ActionResult
            Dim modelo = db.GetModelo(idmodelo, strCn)
            ViewData("nombre") = modelo.nombre
            ViewData("precio") = modelo.precio
            Return View("addmodelo")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function editmodelo(idtipo As Integer, idmarca As Integer, idmodelo As Integer, nombre As String, precio As Nullable(Of Decimal)) As ActionResult
            If String.IsNullOrEmpty(nombre) Then
                ModelState.AddModelError("nombre", h.Traducir("Es obligatorio introducir el nombre"))
            End If
            If Not precio.HasValue Then
                ModelState.AddModelError("precio", h.Traducir("Es obligatorio introducir el precio"))
            End If
            If ModelState.IsValid Then
                db.EditModelo(idmodelo, nombre, precio.Value, strCn)
                Return RedirectToAction("listmodelo", New With {.idtipo = idtipo, .idmarca = idmarca, .idmodelo = idmodelo})
            End If
            ViewData("nombre") = nombre
            ViewData("precio") = precio.Value
            Return View("listmodelo", New With {.idtipo = idtipo, .idmarca = idmarca, .idmodelo = idmodelo})
        End Function
    End Class
End Namespace