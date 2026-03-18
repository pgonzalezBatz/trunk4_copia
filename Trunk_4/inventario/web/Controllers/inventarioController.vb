Imports System.Security.Permissions
Namespace web
    Public Class InventarioController
        Inherits System.Web.Mvc.Controller

        Private ReadOnly strCn As String = ConfigurationManager.ConnectionStrings("inventario").ConnectionString
        Private ReadOnly strCnOcsWeb As String = ConfigurationManager.ConnectionStrings("ocsweb").ConnectionString
        Private ReadOnly strCnEpsilon As String = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Index() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function AsignToUser() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function AsignToUser(idsab As Nullable(Of Integer)) As ActionResult
            If Not idsab.HasValue Then
                ModelState.AddModelError("idsab", "Es necesario seleccior usuario")
            End If
            If ModelState.IsValid Then
                If IsNumeric(Request("idEtiqueta")) Then
                    DB.EditEtiquetaReasignarUsuario(Request("idEtiqueta"), idsab, Request("departamento") IsNot Nothing, strCn)
                    Return RedirectToAction("getlabel", New With {.idEtiqueta = Request("idEtiqueta")})
                End If
                Return RedirectToAction("asigntouser2", New With {.idsab = idsab.Value})
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function AsignToUser2(idSab As Integer) As ActionResult
            ViewData("activosasignados") = DB.GetListOfEtiquetasUsuario(idSab, strCn)
            Return View(DB.GetUsuario(idSab, strCn, strCnEpsilon))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function AsignToUser2(idsab As Integer, idEtiqueta As Nullable(Of Integer)) As ActionResult
            If Not idEtiqueta.HasValue Then
                ModelState.AddModelError("id", "Es necesario leer la etiqueta")
            End If
            If ModelState.IsValid Then
                If DB.IsEtiquetaAsignada(idEtiqueta, strCn) Then
                    Return RedirectToAction("GetLabel", New With {idEtiqueta})
                End If
                If DB.GetEtiqueta(idEtiqueta, strCn).Count > 0 Then
                    'Simple asignacion
                    DB.AddUsuarioEtiqueta(idsab, idEtiqueta, strCn)
                    Return RedirectToAction("ok", New With {idsab})
                Else
                    'Asignacion e introduccion de datos
                    Return RedirectToAction("asigntouserpicktipo", New With {idsab, idEtiqueta})
                End If
            End If
            ViewData("activosasignados") = DB.GetListOfEtiquetasUsuario(idsab, strCn)
            Return View(DB.GetUsuario(idsab, strCn, strCnEpsilon))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function AsignToUserPicktipo(idSab As Integer, idEtiqueta As Integer) As ActionResult
            ViewData("listoftipo") = DB.GetListOfTipo(strCn)
            ViewData("usuario") = DB.GetUsuario(idSab, strCn, strCnEpsilon)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function AsignToUserPickmarcaAndModelo(idSab As Integer, idEtiqueta As Integer, idTipo As Integer) As ActionResult
            ViewData("usuario") = DB.GetUsuario(idSab, strCn, strCnEpsilon)
            ViewData("tipo") = DB.GetTipo(idTipo, strCn)
            ViewData("listofmarcamodelo") = DB.GetListOfMarcaModelo(idTipo, strCn)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function AsignToUserPickmarca(idSab As Integer, idEtiqueta As Integer, idTipo As Integer) As ActionResult
            ViewData("listofmarca") = DB.GetListOfMarca(idTipo, strCn)
            ViewData("usuario") = DB.GetUsuario(idSab, strCn, strCnEpsilon)
            ViewData("tipo") = DB.GetTipo(idTipo, strCn)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function AsignToUserPickmodelo(idSab As Integer, idEtiqueta As Integer, idTipo As Integer, idMarca As Integer) As ActionResult
            ViewData("listofmodelo") = DB.GetListOfModelo(idMarca, strCn)
            ViewData("usuario") = DB.GetUsuario(idSab, strCn, strCnEpsilon)
            ViewData("tipo") = DB.GetTipo(idTipo, strCn)
            ViewData("marca") = DB.Getmarca(idTipo, strCn)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Asignmodel(idSab As Integer, idEtiqueta As Integer, idTipo As Integer, idMarca As Integer, idModelo As Integer, numeroserie As String, departamento As String, descripcion As String) As ActionResult
            DB.AddUsuarioEtiqueta(idSab, idEtiqueta, idModelo, numeroserie, departamento IsNot Nothing, descripcion, strCn)
            Return RedirectToAction("asigntouser2", New With {idSab})
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Ok() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Addlabelok() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Addlabel() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Addlabel(idetiqueta As Integer) As ActionResult
            If DB.GetEtiqueta(idetiqueta, strCn).Count > 0 Then
                Return RedirectToAction("GetLabel", New With {idetiqueta})
            End If
            If ModelState.IsValid Then
                Return RedirectToAction("addlabelpicktipo", New With {idetiqueta})
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Addlabelpicktipo(idetiqueta As Integer) As ActionResult
            If DB.IsEtiquetaBaja(idetiqueta, strCn) Then
                ModelState.AddModelError("idetiqueta", "La etiqueta esta dada de baja")
                Return View("addlabel")
            End If
            ViewData("listoftipo") = DB.GetListOfTipo(strCn)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function AddlabelPickmarcaAndModelo(idEtiqueta As Integer, idTipo As Integer) As ActionResult
            ViewData("tipo") = DB.GetTipo(idTipo, strCn)
            ViewData("listofmarcamodelo") = DB.GetListOfMarcaModelo(idTipo, strCn)
            Return View("addlabelPickmarcaAndModelo")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Addlabelpickmarca(idetiqueta As Integer, idtipo As Integer) As ActionResult
            ViewData("listofmarca") = DB.GetListOfMarca(idtipo, strCn)
            ViewData("tipo") = DB.GetTipo(idtipo, strCn)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Addlabelpickmodelo(idetiqueta As Integer, idtipo As Integer, idmarca As Integer) As ActionResult
            ViewData("listofmodelo") = DB.GetListOfModelo(idmarca, strCn)
            ViewData("tipo") = DB.GetTipo(idtipo, strCn)
            ViewData("marca") = DB.Getmarca(idtipo, strCn)
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Addlabelpickmodelo(idetiqueta As Integer, idtipo As Integer, idmarca As Integer, idmodelo As Integer, numeroserie As String, descripcion As String) As ActionResult
            If ConfigurationManager.AppSettings("tipoPortatil").Split(";").Contains(idtipo) AndAlso String.IsNullOrEmpty(numeroserie) Then
                ModelState.AddModelError("numeroserie", h.Traducir("Obligatorio introducir Microsoft OM en portatiles y sobremesa"))
            End If
            If ModelState.IsValid Then
                DB.AddEtiqueta(idetiqueta, idmodelo, numeroserie, descripcion, strCn)
                Return RedirectToAction("addlabelok")
            End If
            Return AddlabelPickmarcaAndModelo(idetiqueta, idtipo)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function GetLabel(idEtiqueta As Integer) As ActionResult
            Return View(DB.GetEtiquetaActiva(idEtiqueta, strCn, strCnEpsilon))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function DeleteEtiqueta(id As Integer) As ActionResult
            DB.DeleteEtiqueta(id, strCn)
            Return RedirectToAction("index", "buscar")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function BajaEtiqueta(idEtiqueta As Integer)
            DB.EditEtiquetaBaja(idEtiqueta, strCn)
            Return RedirectToAction("GetLabel", New With {idEtiqueta})
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function DesasignarEtiqueta(idEtiqueta As Integer)
            DB.EditEtiquetaDesasignar(idEtiqueta, strCn)
            Return RedirectToAction("GetLabel", New With {idEtiqueta})
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function ReasignarContenidoUsuario1() As ActionResult
            Return View("reasign1")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function ReasignarContenidoUsuario1(idsab As Integer?, idsabBaja As Integer?, etiquetas As List(Of Integer), liberar As String) As ActionResult
            If idsabBaja.HasValue AndAlso etiquetas IsNot Nothing AndAlso etiquetas.Count > 0 Then
                ViewData("usuariobaja") = DB.GetUsuario(idsabBaja.Value, strCn, strCnEpsilon)
                If String.IsNullOrEmpty(liberar) Then
                    ViewData("usuario") = DB.GetUsuario(idsab.Value, strCn, strCnEpsilon)
                Else
                    ViewData("usuario") = New With {.nombre = "Usuario destino no elegido. Se procedera a liberar los recursos"}
                    ViewData("liberar") = liberar
                End If
                ViewData("etiquetas") = DB.GetListOfEtiquetasUsuario(idsabBaja, strCn).Where(Function(e) etiquetas.Contains(e.idetiqueta))
                Return View("Reasign2")
            End If
            Return View("reasign1")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function ReasignarContenidoUsuario2(idsab As Integer?, idsabBaja As Integer?, etiquetas As List(Of Integer), liberar As String) As ActionResult
            If idsabBaja.HasValue Then
                For Each e In etiquetas
                    If String.IsNullOrEmpty(liberar) Then
                        DB.EditEtiquetaReasignarUsuario(e, idsab, False, strCn)
                    Else
                        DB.EditEtiquetaDesasignar(e, strCn)
                    End If
                Next
                Return RedirectToAction("index")
            End If
            Return View("ReasignarContenidoUsuario1")
        End Function
        Function HistoricoUsuario(idSab As Integer) As ActionResult
            ViewData("usuario") = DB.GetUsuario(idSab, strCn, strCnEpsilon)
            Return View(DB.GetListOfEtiquetasUsuarioHistorico(idSab, strCn))
        End Function
        Function HistoricoEtiqueta(idEtiqueta As Integer) As ActionResult
            Return View(DB.GetListOfUsuarioEtiquetaHistorico(idEtiqueta, strCn))
        End Function

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Editlabel(idEtiqueta As Integer) As ActionResult
            Return View(DB.GetEtiquetaActiva(idEtiqueta, strCn, strCnEpsilon))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Editlabel(idetiqueta As Integer, numeroserie As String, descripcion As String) As ActionResult
            If ModelState.IsValid Then
                DB.EditEtiqueta(idetiqueta, numeroserie, descripcion, strCn)
                Return RedirectToAction("getlabel", New With {idetiqueta})
            End If
            Return View()
        End Function
    End Class
End Namespace