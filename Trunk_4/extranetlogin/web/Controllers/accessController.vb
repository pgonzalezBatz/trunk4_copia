Imports System.Drawing
Imports System.Security.Permissions
Imports log4net
Imports System.Linq

Namespace web
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString
        Dim log As ILog = log4net.LogManager.GetLogger("root")

        Function Index(url As String) As ActionResult
            If SimpleRoleProvider.IsUserAuthorised(roles.normal) Then
                If String.IsNullOrEmpty(url) Then
                    Return RedirectToAction("mergedResources", h.ToRouteValues(Request.QueryString, Nothing))
                Else
                    Return transfer(url)
                End If
            End If
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function Index(url As String, lu As LoginUser) As ActionResult
            Dim idSab As Integer
            If ModelState.IsValid Then
                Dim lst = db.CheckLogin(lu.usuario.Trim(" "), lu.contraseña, strCn)
                If lst.Count = 0 Then
                    ModelState.AddModelError("contraseña", h.traducir("Incorrect password"))
                    log.Error("Login ERR: Bad password for user '" & lu.usuario.Trim(" ") & "'")
                ElseIf lst.Count = 1 Then
                    idSab = lst(0)
                    log.Info("Login OK for user '" & lu.usuario.Trim(" ") & "' (" & idSab & ")")
                    If db.NumeroEmpresasNoGlobalesDiferentes(idSab, strCn) + db.NumeroEmpresasGlobalesDiferentes(idSab, strCn) > 1 Then
                        log.Warn("Tiene más de un proveedor diferente... Selección de empresa")
                        If ModelState.IsValid Then
                            h.SetCulture(idSab, strCn)
                            SimpleRoleProvider.SetAuthCookieWithRole(idSab, Function() roles.normal)
                            Dim empresas = db.GetProveedoresNombreusuario(idSab, strCn)
                            Return View("seleccionarempresa", empresas)
                        End If
                    ElseIf db.IsCorporativa(idsab, strCn) Then
                        log.Warn("Pertenece a proveedor corporativo")
                    Else
                        log.Warn("Pertenece a proveedor no corporativo")
                    End If
                ElseIf lst.Count > 1 Then
                    If db.IsCorporativa(lst(0), strCn) Then
                        Dim idSabTemp = db.getAdminCorporativa(lst(0), strCn)
                        If lst.Contains(idSabTemp) AndAlso idSabTemp > 0 Then
                            idSab = idSabTemp
                        Else
                            idSab = lst(0) '''' al azar
                        End If
                    Else
                        idSab = lst(0)
                        'Dim msg = "There is more than one user with those credentials. Please contact with helpdesk"
                        'Return View("PageNotFound", "", msg)
                    End If
                    log.Warn("Login OK for users " & String.Join(",", lst) & " WARNING More than one user with the same credentials '" & lu.usuario.Trim(" ") & "' -> final pick: " & idSab)
                End If
            End If
            If ModelState.IsValid Then
                h.SetCulture(idSab, strCn)
                SimpleRoleProvider.SetAuthCookieWithRole(idSab, Function() roles.normal) 'Solo existe el rol normal
                If String.IsNullOrEmpty(url) Then
                    Return RedirectToAction("mergedResources", h.ToRouteValues(Request.QueryString, Nothing))
                Else
                    log.Info("... transfering to url (" & url & ")")
                    Return transfer(url)
                End If
            End If
            Return View()
        End Function


        <SimpleRoleProvider(roles.normal)>
        Function SeleccionarEmpresa2(idempresa As String, nombreusuario As String, tipo As String) As ActionResult
            Dim idsab As Integer = 0
            Select Case tipo
                Case "proveedor global"
                    idsab = db.getUserIdFromProveedorGlobal(idempresa, nombreusuario, strCn)
                Case "empresa"
                    idsab = db.getUserIdFromEmpresa(idempresa, nombreusuario, strCn)
            End Select
            log.Info("After seleccionarEmpresa, IdSab del usuario de ese proveedor: " & idsab)
            SimpleRoleProvider.setAuthCookieWithRole(idsab, Function() roles.normal) 'Solo existe el rol normal
            Return RedirectToAction("mergedResources", h.ToRouteValues(Request.QueryString, Nothing))
        End Function

        Function resetpassword() As ActionResult
            Return View()
        End Function

        <SimpleRoleProvider(roles.normal)>
        Function enlaceNoValido() As ActionResult
            Return View()
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function resetpassword(usuario As String) As ActionResult
            If String.IsNullOrEmpty(usuario) OrElse Not db.existeUsuario(usuario, strCn) Then
                ModelState.AddModelError("usuario", h.traducir("El usuario introducido no existe en nuestro sistema"))
            End If
            If ModelState.IsValid Then
                Dim rngCsp As New Security.Cryptography.RNGCryptoServiceProvider()
                Dim rndB(94) As Byte
                rngCsp.GetBytes(rndB)
                Dim s2 = Convert.ToBase64String(rndB, Base64FormattingOptions.None)
                db.insertRecuperarPwd(usuario, s2, strCn)
                Dim vdd = New ViewDataDictionary()
                vdd("usuario") = usuario
                vdd("otp") = Url.Encode(s2)
                SendEmail(usuario, "Extranet de Batz", renderViewToString("~/Views/access/notificacionrecuperacion.vbhtml", Me.ControllerContext, vdd))
                log.Info("Recovery mail sent to " & usuario)
                Return RedirectToAction("confirmation")
            End If
            Return View()
        End Function

        Function uniquereset(id As String) As ActionResult
            If String.IsNullOrEmpty(id) Then
                Return Redirect("enlacenovalido")
            End If
            If String.IsNullOrEmpty(db.getRecuperarPwd(id, DateAdd(DateInterval.Day, -1, Now), strCn)) Then
                Return Redirect("enlacenovalido")
            End If
            Return View()
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function uniquereset(id As String, pwd1 As String, pwd2 As String) As ActionResult
            If String.IsNullOrEmpty(id) Then
                Return Redirect("enlacenovalido")
            End If
            Dim usuario = db.getRecuperarPwd(id, DateAdd(DateInterval.Day, -1, Now), strCn)
            If String.IsNullOrEmpty(usuario) Then
                Return Redirect("enlacenovalido")
            End If
            If String.IsNullOrEmpty(pwd1) Then
                ModelState.AddModelError("pwd1", h.traducir("Es necesario introducir una nueva clave"))
            End If
            If String.IsNullOrEmpty(pwd2) Then
                ModelState.AddModelError("pwd2", h.traducir("Es necesario introducir una nueva clave"))
            End If
            If pwd1 <> pwd2 Then
                ModelState.AddModelError("pwd2", h.traducir("Las claves no coinciden"))
            End If
            If ModelState.IsValid Then
                db.ResetPassword(usuario, pwd1, strCn)
                Return RedirectToAction("index")
            End If
            Return View()
        End Function

        Function confirmation() As ActionResult
            Return View()
        End Function

        <SimpleRoleProvider(roles.normal)>
        Function mergedResources() As ActionResult
            Dim id = SimpleRoleProvider.GetId()
            If Not String.IsNullOrEmpty(Request("IdSession")) Then
                db.DeleteSessionTicket(Request("IdSession"), strCn)
            End If
            'Dim mergedResourcesData = db.GetMergedrecursosForNombreusuarioAndPassword(id, h.GetCulture(), strCn) _
            '                            .Where(Function(e) e.area > 0) _
            '                            .OrderBy(Function(e) e.area) _
            '                            .ThenBy(Function(e) e.nombre) _
            '                            .GroupBy(Function(e) New With {Key .Name = e.areaname, Key .Id = e.area})
            Dim mergedResourcesData = db.GetMergedrecursosForNombreusuarioAndProvider(id, h.GetCulture(), strCn) _
                                        .Where(Function(e) e.area > 0) _
                                        .OrderBy(Function(e) e.area) _
                                        .ThenBy(Function(e) e.nombre) _
                                        .GroupBy(Function(e) New With {Key .Name = e.areaname, Key .Id = e.area})
            Return View("resources", mergedResourcesData)
        End Function

        <SimpleRoleProvider(roles.normal)>
        Function resources() As ActionResult
            Return mergedResources()
        End Function

        <SimpleRoleProvider(roles.normal)>
        Function transfer(url As String) As ActionResult
            Dim idSabTemp = SimpleRoleProvider.GetId()
            '''' buscar qué idsab coincidente con el nombreusuario del idsabtemp tiene acceso al recurso 'url'
            Dim idRecTemp = Request("idrec")
            Dim lst = db.getIdSabWithSameNombreusuarioProviderAndAccessToResource(idSabTemp, idRecTemp, strCn)

            If lst.Count > 1 Then
                '''' si hay más de uno, buscamos el administrador
                idSabTemp = db.getAdminFromLst(idSabTemp, lst, strCn)
                If idSabTemp < 1 Then
                    '''' si no hay administrador, cogemos el primero (RANDOM)
                    idSabTemp = lst(0)
                End If
            ElseIf lst.Count = 1 Then
                idSabTemp = lst(0)
            End If
            Return posttransfer(url, idSabTemp)
        End Function

        <SimpleRoleProvider(roles.normal)>
        Function posttransfer(url As String, idSabTemp As Integer) As ActionResult
            db.TransferSession(idSabTemp, Session.SessionID, strCn)
            log.Info("Id de sesión transferida")
            If Request("emp") Is Nothing Then
                Return Redirect(If(url.Contains("?"), url + "&IdSession=" + Session.SessionID, url + "?IdSession=" + Session.SessionID))
            Else
                Return Redirect(If(url.Contains("?"), url + "&IdSession=" + Session.SessionID, url + "?IdSession=" + Session.SessionID) + "&emp=" + Request("emp"))
            End If
        End Function

        <SimpleRoleProvider(roles.normal)>
        Function image(id As Integer) As ActionResult
            Dim cacheKey = "image" + id.ToString
            Dim b As Byte()
            If HttpRuntime.Cache(cacheKey) Is Nothing Then
                b = db.GetImage(id, strCn)
                If b IsNot Nothing Then
                    HttpRuntime.Cache.Insert(cacheKey, b, Nothing, Now.AddMonths(1), TimeSpan.Zero)
                End If
            Else
                b = HttpRuntime.Cache(cacheKey)
            End If
            Dim newBytes As Byte() = resizeImage(b)
            Return New FileStreamResult(New IO.MemoryStream(newBytes), "image/png")
        End Function

        Private Shared Function resizeImage(b() As Byte) As Byte()
            Dim startBitmap As Drawing.Bitmap = Bitmap.FromStream(New IO.MemoryStream(b))
            If startBitmap.Size.Height = 48 OrElse startBitmap.Size.Width = 48 Then
                Return b
            End If
            Dim newBitmap As Drawing.Bitmap = New Bitmap(48, 48)
            Using graphics = Drawing.Graphics.FromImage(newBitmap)
                graphics.DrawImage(startBitmap, New Rectangle(0, 0, 48, 48), New Rectangle(0, 0, startBitmap.Width, startBitmap.Height), GraphicsUnit.Pixel)
            End Using
            Dim ms2 As IO.MemoryStream = New IO.MemoryStream
            newBitmap.Save(ms2, Imaging.ImageFormat.Jpeg)
            Dim newBytes As Byte() = ms2.ToArray
            Return newBytes
        End Function

        <SimpleRoleProvider(roles.normal)>
        Function changeLanguage() As ActionResult
            Return View(db.GetListOfCulturas(strCn))
        End Function
        <SimpleRoleProvider(roles.normal)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function changeLanguage(idcultura As String) As ActionResult
            db.ChangeCultura(User.Identity.Name, idcultura, strCn)
            Response.Cookies.Add(New HttpCookie("culture", idcultura))
            Return RedirectToAction("index")
        End Function
        <SimpleRoleProvider(roles.normal)>
        Function logoff() As ActionResult
            log.Info("Loggin off (IdSab:" & SimpleRoleProvider.GetId() & ")")
            FormsAuthentication.SignOut()
            'Delete all the cookies for the domain. This will close every application's session
            For Each k In Request.Cookies.AllKeys
                Response.Cookies(k).Expires = DateTime.Now.AddDays(-1)
            Next
            Return RedirectToAction("index", h.ToRouteValues(Request.QueryString, Nothing))
        End Function
        <SimpleRoleProvider(roles.normal)>
        Function ChangePassword() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(roles.normal)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function ChangePassword(password As String, password2 As String) As ActionResult
            If password.Length = 0 Then
                ModelState.AddModelError("password", "Es necesario introducir la clave")
            End If
            If password2.Length = 0 Then
                ModelState.AddModelError("password2", "Es necesario reintroducir la clave")
            End If
            If password <> password2 Then
                ModelState.AddModelError("password", "Las claves no coinciden")
            End If
            If ModelState.IsValid Then
                db.ChangePassword(User.Identity.Name, password, strCn)
                FormsAuthentication.SignOut()
                'Delete all the cookies for the domain. This will close every application's session
                For Each k In Request.Cookies.AllKeys
                    Response.Cookies(k).Expires = DateTime.Now.AddDays(-1)
                Next
                Return RedirectToAction("index", h.ToRouteValues(Request.QueryString, New With {.Msg = "Password changed: log again"}))
            End If
            Return View()
        End Function

        Protected Shared Function renderViewToString(viewPath As String, controllerContext As Mvc.ControllerContext, vdd As ViewDataDictionary) As String
            Using writer As New IO.StringWriter()
                Dim view = New RazorView(controllerContext, viewPath, "", False, Nothing)
                Dim viewCxt = New ViewContext(controllerContext, view, vdd, New TempDataDictionary(), writer)
                view.Render(viewCxt, writer)
                Return writer.ToString()
            End Using
        End Function
        Protected Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
            Dim b = New Net.Mail.MailMessage()
            b.Subject = subject
            b.Body = body
            b.From = New Net.Mail.MailAddress("extranet@batz.es")
#If DEBUG Then
            b.To.Add("diglesias.external@batz.com")
#Else
            b.To.Add(recipients)
#End If
            b.IsBodyHtml = True
            Dim smtp = New Net.Mail.SmtpClient("posta.batz.com")
            smtp.Credentials = New System.Net.NetworkCredential("tareas", "tareas123")
            smtp.DeliveryFormat = Net.Mail.SmtpDeliveryFormat.International
            smtp.Send(b)
        End Sub
    End Class
End Namespace