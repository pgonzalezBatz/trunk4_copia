Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.Net
Imports Oracle.ManagedDataAccess.Client

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class ServiciosWeb
    Inherits System.Web.Services.WebService

    Public log As log4net.ILog = Global_asax.log
    Public BBDD As New BatzBBDD.Entities_Gertakariak
    Public ItzultzaileWeb As New TraduccionesLib.itzultzaile

    <WebMethod()>
    Public Function HelloWorld() As String
        Return "Hola a todos"
    End Function

    <WebMethod()>
    Public Function get_Usuarios_Aplicacion(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            Dim Func As New SabLib.BLL.UsuariosComponent
            get_Usuarios_Aplicacion = Nothing

            get_Usuarios_Aplicacion = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText).Where(Function(Reg) Reg.FechaBaja = Nothing Or Reg.FechaBaja >= Date.Now).OrderBy(Function(o) o.NombreCompleto).OrderBy(Function(o) o.NombreCompleto) _
                    .Select(Function(o) _
                                AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                    If(String.IsNullOrWhiteSpace(o.NombreCompleto), String.Empty, o.NombreCompleto.Trim), o.Id)).Distinct.ToArray
        Catch ex As Exception
            log.Error("WebMethod()_ get_Usuarios_Aplicacion", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_Usuarios_Aplicacion
    End Function
    <WebMethod()>
    Public Function get_Usuarios(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            Dim Func As New SabLib.BLL.UsuariosComponent
            get_Usuarios = Nothing

            Dim aContextKey As Array = Split(contextKey, ";")
            Dim Procedencia As Integer = If(String.IsNullOrWhiteSpace(aContextKey(0)), Nothing, aContextKey(0))


            'Proveedores = 2
            If Procedencia = 2 Then
                If prefixText IsNot Nothing Then
                    Dim IdEmpresa As Nullable(Of Integer) = If(String.IsNullOrWhiteSpace(aContextKey(1)), Nothing, CInt(aContextKey(1)))
                    Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                    Dim FechaActual As Nullable(Of Date) = Now.Date
                    Dim lUsr =
                        (From Usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Join Emp As BatzBBDD.EMPRESAS In BBDD.EMPRESAS On Usr.IDEMPRESAS Equals Emp.ID
                         Where (Emp.FECHABAJA Is Nothing Or Emp.FECHABAJA >= FechaActual) And (Usr.FECHABAJA Is Nothing Or Usr.FECHABAJA >= FechaActual) And Usr.CODPERSONA Is Nothing _
                             And Emp.IDTROQUELERIA = IdEmpresa
                         Select New With {.ID = Usr.ID, .NOMBRE_Emp = Usr.EMPRESAS.NOMBRE, .CONTACTO_Emp = Usr.EMPRESAS.CONTACTO, .CIF = Usr.EMPRESAS.CIF,
                             .NOMBREUSUARIO = Usr.NOMBREUSUARIO, .EMAIL = Usr.EMAIL, .NOMBRE = Usr.NOMBRE, .APELLIDO1 = Usr.APELLIDO1, .APELLIDO2 = Usr.APELLIDO2} Distinct).ToList

                    'Dim sql As String = CType(lUsr.AsEnumerable, System.Data.Objects.ObjectQuery).ToTraceString
                    For Each Texto As String In aPrefixText
                        'Transformamos el texto en una expresion regular.
                        Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                        lUsr = (From Usr In lUsr
                                Where If(String.IsNullOrWhiteSpace(Usr.CIF), Nothing, ExpReg.IsMatch(Usr.CIF)) _
                                    Or If(String.IsNullOrWhiteSpace(Usr.NOMBRE_Emp), Nothing, ExpReg.IsMatch(Usr.NOMBRE_Emp)) _
                                    Or If(String.IsNullOrWhiteSpace(Usr.CONTACTO_Emp), Nothing, ExpReg.IsMatch(Usr.CONTACTO_Emp)) _
                                    Or If(String.IsNullOrWhiteSpace(Usr.NOMBREUSUARIO), Nothing, ExpReg.IsMatch(Usr.NOMBREUSUARIO)) _
                                    Or If(String.IsNullOrWhiteSpace(Usr.EMAIL), Nothing, ExpReg.IsMatch(Usr.EMAIL)) _
                                    Or If(String.IsNullOrWhiteSpace(Usr.NOMBRE), Nothing, ExpReg.IsMatch(Usr.NOMBRE)) _
                                    Or If(String.IsNullOrWhiteSpace(Usr.APELLIDO1), Nothing, ExpReg.IsMatch(Usr.APELLIDO1)) _
                                    Or If(String.IsNullOrWhiteSpace(Usr.APELLIDO2), Nothing, ExpReg.IsMatch(Usr.APELLIDO2))
                                Select New With {.ID = Usr.ID, .NOMBRE_Emp = Usr.NOMBRE_Emp, .CONTACTO_Emp = Usr.CONTACTO_Emp, .CIF = Usr.CIF,
                                    .NOMBREUSUARIO = Usr.NOMBREUSUARIO, .EMAIL = Usr.EMAIL, .NOMBRE = Usr.NOMBRE, .APELLIDO1 = Usr.APELLIDO1, .APELLIDO2 = Usr.APELLIDO2} Distinct).ToList
                    Next
                    Dim get_Usuarios_Proveedor = (From Reg In lUsr
                                                  Select Nombre = String.Format("{0} {1} {2} ({3})", Reg.NOMBRE, Reg.APELLIDO1, Reg.APELLIDO2, Reg.EMAIL) _
                                        , Reg.ID Distinct).OrderBy(Function(o) o.Nombre) _
                                .Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                    If(String.IsNullOrWhiteSpace(o.Nombre), String.Empty, o.Nombre.Trim), o.ID)).Distinct.ToArray

                    '--------------------------------------------------------------------------------------------------------------------------------
                    'Incluimos los trabajadores de batz para que puede seleccionarse cuando es una NC de proveedor.
                    'Puede que no haya usuario para el proveedor y alguien de batz gestione la NC como "Responsable" en nombre del proveedor.
                    '--------------------------------------------------------------------------------------------------------------------------------
                    Dim get_Usuarios_SAB = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText, System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")).Where(Function(Reg) Reg.FechaBaja = Nothing Or Reg.FechaBaja >= Date.Now) _
                    .Select(Function(o) _
                                AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                    If(String.IsNullOrWhiteSpace(o.NombreCompleto), String.Empty, o.NombreCompleto.Trim), o.Id)).Distinct.ToArray
                    '--------------------------------------------------------------------------------------------------------------------------------

                    get_Usuarios = get_Usuarios_Proveedor.Union(get_Usuarios_SAB).Distinct.ToArray
                End If
            Else
                get_Usuarios = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText, System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")).Where(Function(Reg) Reg.FechaBaja = Nothing Or Reg.FechaBaja >= Date.Now) _
                    .Select(Function(o) _
                                AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                    If(String.IsNullOrWhiteSpace(o.NombreCompleto), String.Empty, o.NombreCompleto.Trim), o.Id)).Distinct.ToArray
            End If

        Catch ex As Exception
            log.Error("WebMethod()_ get_Usuarios", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_Usuarios
    End Function
    ''' <summary>
    ''' Obtenemos los usuarios que tiene algun perfil en la aplicacion (Administrador, Gestor) para la planta seleccionada
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <param name="count"></param>
    ''' <param name="contextKey">Identificador de la planta</param>
    ''' <returns></returns>
    <WebMethod()>
    Public Function get_Usuarios_Perfil(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            Dim Func As New SabLib.BLL.UsuariosComponent
            get_Usuarios_Perfil = Nothing

            Dim lReg As IQueryable(Of BatzBBDD.SAB_USUARIOS) = From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS Where Reg.IDTIPOINCIDENCIA = CInt(contextKey) Select Reg.SAB_USUARIOS
            get_Usuarios_Perfil = Func.GetUsuariosBusquedaSAB_Optimizado(prefixText, System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")).Where(Function(o) lReg.Select(Function(reg) reg.ID).Contains(o.Id)).OrderBy(Function(o) o.NombreCompleto) _
                    .Select(Function(o) _
                                AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                    If(String.IsNullOrWhiteSpace(o.NombreCompleto), String.Empty, o.NombreCompleto.Trim), o.Id)).Distinct.ToArray
        Catch ex As Exception
            log.Error("WebMethod()_ get_Usuarios_Perfil", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_Usuarios_Perfil
    End Function
    <WebMethod()>
    Public Function get_Usuarios_NC(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Try
            get_Usuarios_NC = Nothing

            If Not String.IsNullOrWhiteSpace(prefixText) Then
                Dim TipoIncidencia As Integer = CInt(contextKey)
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)

                '--------------------------------------------------------
                'Obtenemos el listado de usuario de las NC
                '--------------------------------------------------------
                Dim lPerfiles As IEnumerable(Of Integer) = From p As [Enum] In [Enum].GetValues(GetType(GTK_Troqueleria.PageBase.AsistenteReunionPreliminar))
                                                           Select CInt(p.GetHashCode)
                Dim lCreador = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                                Where gtk.IDTIPOINCIDENCIA = TipoIncidencia And gtk.SAB_USUARIOS IsNot Nothing
                                Select gtk.SAB_USUARIOS).ToList.Distinct
                Dim lPerseguidor = (From Resp As BatzBBDD.RESPONSABLES_GERTAKARIAK In BBDD.RESPONSABLES_GERTAKARIAK
                                    Where Resp.GERTAKARIAK.IDTIPOINCIDENCIA = TipoIncidencia And Resp.SAB_USUARIOS IsNot Nothing
                                    Select Resp.SAB_USUARIOS).ToList.Distinct
                Dim lResponsable = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                                    From Equ In gtk.EQUIPORESOLUCION
                                    Where gtk.IDTIPOINCIDENCIA = TipoIncidencia
                                    Select Equ).ToList.Distinct
                Dim lInformar = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                                 From Det As BatzBBDD.DETECCION In gtk.DETECCION
                                 Where gtk.IDTIPOINCIDENCIA = TipoIncidencia And lPerfiles.Contains(Det.IDDEPARTAMENTO)
                                 Select Det.SAB_USUARIOS).ToList.Distinct
                Dim lUsuarios = lCreador.Union(lPerseguidor).Union(lResponsable).Union(lInformar).Distinct.ToList
                '--------------------------------------------------------

                '--------------------------------------------------------
                'Buscamos para el texto indicado
                '--------------------------------------------------------
                For Each Texto As String In aPrefixText
                    'Transformamos el texto en una expresion regular.
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)

                    lUsuarios = (From Reg As BatzBBDD.SAB_USUARIOS In lUsuarios
                                 Where If(String.IsNullOrWhiteSpace(Reg.NOMBRE), Nothing, ExpReg.IsMatch(Reg.NOMBRE)) _
                                     Or If(String.IsNullOrWhiteSpace(Reg.APELLIDO1), Nothing, ExpReg.IsMatch(Reg.APELLIDO1)) _
                                     Or If(String.IsNullOrWhiteSpace(Reg.APELLIDO2), Nothing, ExpReg.IsMatch(Reg.APELLIDO2)) _
                                     Or If(String.IsNullOrWhiteSpace(Reg.EMAIL), Nothing, ExpReg.IsMatch(Reg.EMAIL)) _
                                     Or If(String.IsNullOrWhiteSpace(Reg.NOMBREUSUARIO), Nothing, ExpReg.IsMatch(Reg.NOMBREUSUARIO))
                                 Select Reg Distinct).ToList
                Next
                If lUsuarios.Any Then
                    get_Usuarios_NC = lUsuarios.Select(Function(o) _
                                AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                    If(String.IsNullOrWhiteSpace(String.Format("{0} {1} {2}", o.NOMBRE, o.APELLIDO1, o.APELLIDO2)), String.Empty, String.Format("{0} {1} {2}", o.NOMBRE, o.APELLIDO1, o.APELLIDO2)), o.ID)).Distinct.ToArray
                End If
                '--------------------------------------------------------
            End If
        Catch ex As Exception
            log.Error("WebMethod()_ get_Usuarios_NC", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_Usuarios_NC
    End Function

    <WebMethod()>
    Public Function get_Proyecto(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        get_Proyecto = Nothing
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            '---------------------------------------------------------------------------------------------------------
            'Texto a buscar
            '---------------------------------------------------------------------------------------------------------
            If prefixText IsNot Nothing Then
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                Dim lPro As List(Of BatzBBDD.BONOSIS_PROYECTOS) = (From Pro As BatzBBDD.BONOSIS_PROYECTOS In BBDD.BONOSIS_PROYECTOS Select Pro Distinct).ToList
                For Each Texto As String In aPrefixText
                    'Transformamos el texto en una expresion regular.
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)

                    lPro = (From Pro As BatzBBDD.BONOSIS_PROYECTOS In lPro
                            Where If(String.IsNullOrWhiteSpace(Pro.NOMBRE), Nothing, ExpReg.IsMatch(Pro.NOMBRE)) _
                            Or If(String.IsNullOrWhiteSpace(Pro.PRODUCTO), Nothing, ExpReg.IsMatch(Pro.PRODUCTO)) _
                            Or If(String.IsNullOrWhiteSpace(Pro.ESTADO), Nothing, ExpReg.IsMatch(Pro.ESTADO)) _
                            Or If(String.IsNullOrWhiteSpace(Pro.NUMOF), Nothing, ExpReg.IsMatch(Pro.NUMOF)) _
                            Or If(String.IsNullOrWhiteSpace(Pro.PLANTTOCHARGE), Nothing, ExpReg.IsMatch(Pro.PLANTTOCHARGE)) _
                            Or If(String.IsNullOrWhiteSpace(Pro.RESPONSABLE), Nothing, ExpReg.IsMatch(Pro.RESPONSABLE))
                            Select Pro Distinct).ToList
                Next
                If lPro.Any Then
                    get_Proyecto = lPro.Select(Function(o) New With {.Id = o.ID,
                                                                     .Descripcion = If(String.IsNullOrWhiteSpace(o.NOMBRE), String.Empty, o.NOMBRE.Trim) & " - " & o.PRODUCTO & " (" & o.ESTADO & ")"}).OrderBy(Function(o) o.Descripcion) _
                                                                                    .Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                                                                            If(String.IsNullOrWhiteSpace(o.Descripcion), String.Empty, o.Descripcion.Trim), o.Id)).Distinct.ToArray
                End If
            End If
            '---------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            log.Error("WebMethod()_ get_Proyecto", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_Proyecto
    End Function


    <WebMethod()>
    Public Function get_DescAcc(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        get_DescAcc = Nothing
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            If prefixText IsNot Nothing Then
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                'INNER JOIN
                'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas
                Dim lAcciones As List(Of BatzBBDD.ACCIONES) = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                                                               Where Reg.IDTIPOACCION = count And gtk.IDTIPOINCIDENCIA = contextKey
                                                               Select Reg Distinct).ToList
                For Each Texto As String In aPrefixText
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                    lAcciones = (From Reg As BatzBBDD.ACCIONES In lAcciones
                                 Where If(String.IsNullOrWhiteSpace(Reg.DESCRIPCION), Nothing, ExpReg.IsMatch(Reg.DESCRIPCION))
                                 Select Reg Distinct).ToList
                Next
                If lAcciones.Any Then
                    get_DescAcc = lAcciones.Select(Function(o) New With {.Id = o.ID, .Descripcion = If(String.IsNullOrWhiteSpace(o.DESCRIPCION), String.Empty, o.DESCRIPCION.Trim)}).OrderBy(Function(o) o.Descripcion) _
                                                .Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(If(String.IsNullOrWhiteSpace(o.Descripcion), String.Empty, o.Descripcion.Trim), o.Id)).Distinct.Take(200).ToArray()
                End If
            End If

        Catch ex As Exception
            log.Error("WebMethod()_ get_DescAcc", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_DescAcc
    End Function

    <WebMethod()>
    Public Function get_EficAcc(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        get_EficAcc = Nothing
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            If prefixText IsNot Nothing Then
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                'INNER JOIN
                'IDTIPOACCION: 1-Contenedora (Inmediata), 2-Provisionales, 3-Definitivas, 4-Preventivas
                Dim lAcciones As List(Of BatzBBDD.ACCIONES) = (From Reg As BatzBBDD.ACCIONES In BBDD.ACCIONES, gtk As BatzBBDD.GERTAKARIAK In Reg.GERTAKARIAK
                                                               Where Reg.IDTIPOACCION = count And gtk.IDTIPOINCIDENCIA = contextKey
                                                               Select Reg Distinct).ToList
                For Each Texto As String In aPrefixText
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                    lAcciones = (From Reg As BatzBBDD.ACCIONES In lAcciones
                                 Where If(String.IsNullOrWhiteSpace(Reg.EFICACIA), Nothing, ExpReg.IsMatch(Reg.EFICACIA))
                                 Select Reg Distinct).ToList
                Next
                If lAcciones.Any Then
                    'get_EficAcc = lAcciones.Select(Function(o) New With {.Id = o.ID, .Descripcion = If(String.IsNullOrWhiteSpace(o.EFICACIA), String.Empty, o.EFICACIA.Trim)}).OrderBy(Function(o) o.Descripcion) _
                    '                            .Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(If(String.IsNullOrWhiteSpace(o.Descripcion), String.Empty, o.Descripcion.Trim), o.Id)).Distinct.Take(200).ToArray()
                    get_EficAcc = lAcciones.Select(Function(o) New String(If(String.IsNullOrWhiteSpace(o.EFICACIA), String.Empty, o.EFICACIA.Trim))).Distinct.Take(200).OrderBy(Function(o) o).ToArray
                End If
            End If

        Catch ex As Exception
            log.Error("WebMethod()_ get_EficAcc", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_EficAcc
    End Function

    <WebMethod()>
    Public Function get_CausaRaizPF(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        get_CausaRaizPF = Nothing
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            If prefixText IsNot Nothing Then
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                'INNER JOIN
                Dim lRegistros As List(Of BatzBBDD.G8D_E56) = (From E56 As BatzBBDD.G8D_E56 In BBDD.G8D_E56
                                                               Join G8D As BatzBBDD.G8D In BBDD.G8D On E56.ID Equals G8D.ID_E56
                                                               Join gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK On G8D.IDGTK Equals gtk.ID
                                                               Where gtk.IDTIPOINCIDENCIA = contextKey Select E56 Distinct).ToList
                For Each Texto As String In aPrefixText
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                    lRegistros = (From Reg As BatzBBDD.G8D_E56 In lRegistros
                                  Where If(String.IsNullOrWhiteSpace(Reg.CAUSARAIZ_PF), Nothing, ExpReg.IsMatch(Reg.CAUSARAIZ_PF))
                                  Select Reg Distinct).ToList
                Next
                If lRegistros.Any Then
                    'get_CausaRaizPF = lRegistros.Select(Function(o) New With {.Id = o.ID, .Descripcion = If(String.IsNullOrWhiteSpace(o.CAUSARAIZ_PF), String.Empty, o.CAUSARAIZ_PF.Trim)}).OrderBy(Function(o) o.Descripcion) _
                    '							.Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(If(String.IsNullOrWhiteSpace(o.Descripcion), String.Empty, o.Descripcion.Trim), o.Id)).Distinct.Take(200).ToArray()
                    get_CausaRaizPF = lRegistros.Select(Function(o) New String(If(String.IsNullOrWhiteSpace(o.CAUSARAIZ_PF), String.Empty, o.CAUSARAIZ_PF.Trim))).Distinct.Take(200).OrderBy(Function(o) o).ToArray
                End If
            End If

        Catch ex As Exception
            log.Error("WebMethod()_ get_CausaRaizPF", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_CausaRaizPF
    End Function
    <WebMethod()>
    Public Function get_CausaRaizPC(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        get_CausaRaizPC = Nothing
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            If prefixText IsNot Nothing Then
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                'INNER JOIN
                Dim lRegistros As List(Of BatzBBDD.G8D_E56) = (From E56 As BatzBBDD.G8D_E56 In BBDD.G8D_E56
                                                               Join G8D As BatzBBDD.G8D In BBDD.G8D On E56.ID Equals G8D.ID_E56
                                                               Join gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK On G8D.IDGTK Equals gtk.ID
                                                               Where gtk.IDTIPOINCIDENCIA = contextKey Select E56 Distinct).ToList
                For Each Texto As String In aPrefixText
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                    lRegistros = (From Reg As BatzBBDD.G8D_E56 In lRegistros
                                  Where If(String.IsNullOrWhiteSpace(Reg.CAUSARAIZ_PC), Nothing, ExpReg.IsMatch(Reg.CAUSARAIZ_PC))
                                  Select Reg Distinct).ToList
                Next
                If lRegistros.Any Then
                    'get_CausaRaizPC = lRegistros.Select(Function(o) New With {.Id = o.ID, .Descripcion = If(String.IsNullOrWhiteSpace(o.CAUSARAIZ_PC), String.Empty, o.CAUSARAIZ_PC.Trim)}).OrderBy(Function(o) o.Descripcion) _
                    '							.Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(If(String.IsNullOrWhiteSpace(o.Descripcion), String.Empty, o.Descripcion.Trim), o.Id)).Distinct.Take(200).ToArray()
                    get_CausaRaizPC = lRegistros.Select(Function(o) New String(If(String.IsNullOrWhiteSpace(o.CAUSARAIZ_PC), String.Empty, o.CAUSARAIZ_PC.Trim))).Distinct.Take(200).OrderBy(Function(o) o).ToArray
                End If
            End If

        Catch ex As Exception
            log.Error("WebMethod()_ get_CausaRaizPC", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_CausaRaizPC
    End Function

    <WebMethod()>
    Public Function get_OFOPM(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Lista As New List(Of String)
        Try
            '---------------------------------------------------------------------------------------------------------
            'Texto a buscar
            '---------------------------------------------------------------------------------------------------------
            If prefixText IsNot Nothing Then
                Dim lReg = (From W_CPLISMAT In BBDD.W_CPLISMAT Join W_CPCABEC In BBDD.W_CPCABEC
                                                          On W_CPLISMAT.NUMORD Equals W_CPCABEC.NUMORD And W_CPLISMAT.NUMOPE Equals W_CPCABEC.NUMOPE
                            Where W_CPCABEC.TIPORD = 0
                            Select New With {.Orden = W_CPLISMAT.NUMORD, .Operacion = W_CPLISMAT.NUMOPE, .Marca = W_CPLISMAT.NUMMAR.Trim, .Material = W_CPLISMAT.MATERIAL.Trim} Distinct).ToList

                '--------------------------------------------------------------------------------------------
                'Identificamos en el texto el formato OF-OP para hacer una busqueda explicita.
                '--------------------------------------------------------------------------------------------
                Dim aPrefixText_OFOP As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                Dim ExpReg_OFOP As Regex = New Regex("[\d][\-][\d]", RegexOptions.IgnoreCase)
                For Each Texto As String In aPrefixText_OFOP
                    If ExpReg_OFOP.IsMatch(Texto) Then
                        Dim aOFOP As String() = Texto.Split("-")
                        lReg = (From Reg In lReg Where If(IsNumeric(aOFOP(0)), Reg.Orden = aOFOP(0), Nothing) And If(IsNumeric(aOFOP(1)), Reg.Operacion = aOFOP(1), Nothing) Select Reg).ToList
                    End If
                Next
                '--------------------------------------------------------------------------------------------

                Dim aPrefixText As String() = prefixText.Split(New String() {" ", "-", "/", "\", "(", ")"}, StringSplitOptions.RemoveEmptyEntries)
                For Each Texto As String In aPrefixText
                    'Transformamos el texto en una expresion regular.
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                    lReg = (From Reg In lReg
                            Where ExpReg.IsMatch(Reg.Orden) Or ExpReg.IsMatch(Reg.Operacion) _
                                Or If(String.IsNullOrWhiteSpace(Reg.Marca), Nothing, ExpReg.IsMatch(Reg.Marca)) _
                                Or If(String.IsNullOrWhiteSpace(Reg.Material), Nothing, ExpReg.IsMatch(Reg.Material))
                            Select Reg Distinct Order By Reg.Orden).ToList
                Next
                If lReg.Any Then
                    '---------------------------------------------------------------------------------------------
                    'Agrupamos por OF y OP por si la NC afecta a todas las marcas
                    '---------------------------------------------------------------------------------------------
                    Dim lRegOfOP = (From Reg In lReg Group Reg By Reg.Orden, Reg.Operacion Into myGroup = Group
                                    Let RegGroup = myGroup.FirstOrDefault
                                    Select New With {.Orden = RegGroup.Orden, .Operacion = RegGroup.Operacion, .Marca = String.Empty, .Material = String.Empty}).ToList
                    lReg.AddRange(lRegOfOP)
                    '---------------------------------------------------------------------------------------------
                    For Each reg In lReg.OrderBy(Function(o) o.Orden).ThenBy(Function(o) o.Operacion).ThenBy(Function(o) o.Marca).Take(count)
                        Lista.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                              If(String.IsNullOrWhiteSpace(reg.Orden), String.Empty, reg.Orden) _
                              & "-" & If(String.IsNullOrWhiteSpace(reg.Operacion), String.Empty, reg.Operacion) _
                              & " (" & If(String.IsNullOrWhiteSpace(reg.Marca), String.Empty, reg.Marca.Trim) _
                              & "-" & If(String.IsNullOrWhiteSpace(reg.Material), String.Empty, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(reg.Material.Trim.ToLower)) & ")",
                             String.Format("{0}:{1}:{2}", reg.Orden, reg.Operacion, reg.Marca.ToUpper)))

                    Next
                End If
            End If
            '---------------------------------------------------------------------------------------------------------

            Return Lista.ToArray
        Catch ex As ApplicationException
            Global_asax.log.Info("<WebMethod()> " & New StackFrame(0).GetMethod().Name, ex)
            Return New String() {ex.Message}
        Catch ex As Exception
            Global_asax.log.Error("<WebMethod()> " & New StackFrame(0).GetMethod().Name, ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
    End Function
    <WebMethod()>
    Public Function get_Marcas(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        get_Marcas = Nothing
        Try
            Dim gtk As BatzBBDD.GERTAKARIAK = BBDD.GERTAKARIAK.Where(Function(o) o.ID = CInt(category)).SingleOrDefault
            Dim kv As StringDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            Dim OFM As Array = Split(kv.Values(0), "-")

            Dim NUMORD As Integer = OFM(0)
            Dim NUMOPE As Integer = OFM(1)
            Dim lReg = From W_CPLISMAT In BBDD.W_CPLISMAT.Where(Function(o) o.NUMORD = NUMORD And NUMOPE = NUMOPE).Select(Function(o) New With {Key o.NUMORD, Key o.NUMOPE, Key o.NUMMAR, Key o.MATERIAL}).Distinct.AsEnumerable
                       Join Reg_OFM As BatzBBDD.OFMARCA In gtk.OFMARCA
                           On Reg_OFM.NUMOF Equals W_CPLISMAT.NUMORD And Reg_OFM.OP Equals W_CPLISMAT.NUMOPE And Reg_OFM.MARCA Equals W_CPLISMAT.NUMMAR
                       Where Reg_OFM.NUMOF = NUMORD And Reg_OFM.OP = NUMOPE
                       Select New AjaxControlToolkit.CascadingDropDownNameValue(W_CPLISMAT.NUMMAR.Trim & "-" & W_CPLISMAT.MATERIAL.Trim, W_CPLISMAT.NUMMAR.Trim) Distinct

            get_Marcas = lReg.ToArray
        Catch ex As Exception
            log.Error(ex)
        End Try
        Return get_Marcas
    End Function

    <WebMethod()> Public Function get_CapacidadesProv(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        get_CapacidadesProv = Nothing
        Dim Capacidad_NC As AjaxControlToolkit.CascadingDropDownNameValue = Nothing
        Dim Lista_cdd As List(Of AjaxControlToolkit.CascadingDropDownNameValue) = Nothing

        Try
            If Not String.IsNullOrWhiteSpace(contextKey) Then
                If Not String.IsNullOrWhiteSpace(category) AndAlso category <> "Capacidades" Then
                    Capacidad_NC = (From Reg As BatzBBDD.CAPACIDADES In BBDD.CAPACIDADES.AsEnumerable Where Reg.CAPID = category Select New AjaxControlToolkit.CascadingDropDownNameValue(Reg.NOMBRE, Reg.CAPID, True)).SingleOrDefault
                End If

                Dim lReg As IEnumerable(Of AjaxControlToolkit.CascadingDropDownNameValue)
                Dim lCapacidades = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS Where Reg.IDTROQUELERIA = contextKey Select Reg.CAPACIDADES).ToList.FirstOrDefault
                lReg = lCapacidades.Where(Function(o) o.CAPID <> category And o.OBSOLETO = 0).Select(Function(o) New AjaxControlToolkit.CascadingDropDownNameValue(o.NOMBRE, o.CAPID)).Distinct
                Lista_cdd = lReg.ToList

                If Capacidad_NC IsNot Nothing Then Lista_cdd.Add(Capacidad_NC)

                get_CapacidadesProv = Lista_cdd.OrderBy(Function(o) o.name).ToArray
            End If
        Catch ex As Exception
            log.Error(ex)
        End Try

        Return get_CapacidadesProv
    End Function

    <WebMethod()> Sub NotificacionUG(ByVal ID_GTK As Integer, ByVal sFrom As String)
        Dim Incidencia As New BatzBBDD.GERTAKARIAK
        Try
            Incidencia = (From Reg As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where Reg.ID = ID_GTK Select Reg).SingleOrDefault
            If Incidencia IsNot Nothing AndAlso Incidencia.OFMARCA.Any Then
                Dim lUG = From CPCABEC As BatzBBDD.CPCABEC In BBDD.CPCABEC.AsEnumerable
                          Join OFM As BatzBBDD.OFMARCA In Incidencia.OFMARCA On CPCABEC.NUMORD Equals OFM.NUMOF
                          Where CPCABEC.NUMMOD = 0
                          Select CPCABEC.LANTEGI_AC Distinct
                If lUG.Any Then
                    Dim lUsr As New List(Of BatzBBDD.ESTRUCTURA)
                    Dim Nodo_UG = From UG As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA
                                  Where lUG.Contains(UG.ORDEN) And UG.ESTRUCTURA_Origen.ID = My.Settings.IdNotificacionesUG
                                  Select UG
                    If Nodo_UG.Any Then
                        For Each UG In Nodo_UG
                            If UG.ESTRUCTURA1.Any Then
                                UG.ESTRUCTURA1.ToList.ForEach(Sub(Nodo_Usr) lUsr.Add(Nodo_Usr))
                            End If
                        Next
                    End If
                    Dim lPara = From UsrSab As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS.AsEnumerable
                                Join UsrUG As BatzBBDD.ESTRUCTURA In lUsr.AsEnumerable On UsrUG.ORDEN Equals UsrSab.ID
                                Select UsrSab
                    Dim list As New List(Of String)
                    For Each lp In lPara
                        list.Add(lp.EMAIL)
                    Next
                    log.Info("lPara:" & String.Join(",", list))
                    log.Info(sFrom)
                    log.Info(Incidencia.ID)
                    If lPara.Any Then
                        'Dim UsrFrom As BatzBBDD.SAB_USUARIOS = (From Reg As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Where Reg.ID = IdFrom Select Reg).SingleOrDefault
                        'Dim emailFrom As String = If(UsrFrom Is Nothing OrElse String.IsNullOrWhiteSpace(UsrFrom.EMAIL), "NuevaNC@batz.es", UsrFrom.EMAIL)
                        AvisoCorre(sFrom, lPara, Nothing,
                                   String.Format(ItzultzaileWeb.Itzuli("Notificacion de NC: {0} / OF-(OP): {1}"), Incidencia.ID, String.Join(", ", (From OFM As BatzBBDD.OFMARCA In Incidencia.OFMARCA Group By gOFM = OFM.NUMOF Into OFM_G = Group Select New With {gOFM, .gOP = String.Join(",", OFM_G.OrderBy(Function(o) o.OP).Select(Function(o) o.OP).Distinct)}).Select(Function(o) o.gOFM & "-(" & o.gOP & ")"))),
                                   ItzultzaileWeb.Itzuli("Notificacion de la creacion de la NC."),
                                   Incidencia)
                    End If
                End If
            Else
                log.Info("Unknown NC with Id:" & ID_GTK)
            End If
        Catch ex As ApplicationException
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID, ex)
        Catch ex As Exception
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID, ex)
        End Try
    End Sub


    <WebMethod()> Sub NotificacionTPC(ByVal ID_GTK As Integer, ByVal sFrom As String, ByVal urgente As Boolean)
        log.Info("Enviando Notificación TPC/PE...")
        Dim Incidencia As New BatzBBDD.GERTAKARIAK
        Try
            Incidencia = (From Reg As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where Reg.ID = ID_GTK Select Reg).SingleOrDefault
            Dim list As New List(Of String)
            Dim myList = Incidencia.OFMARCA.Select(Function(f) f.NUMOF).Distinct
            log.Info("Lista de OFs: " & String.Join(";", myList))
            For Each itemOF In myList
                Dim mailUsr = (From regOF As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA
                               Join regUsr As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA On regOF.ID Equals regUsr.IDITURRIA
                               Join regSab As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS On regUsr.ORDEN Equals regSab.ID
                               Where regOF.IDITURRIA = My.Settings.IdPEs AndAlso regOF.DESCRIPCION = itemOF
                               Select regSab.EMAIL).FirstOrDefault
                If mailUsr IsNot Nothing Then '''' si existe en el maestro de excepciones PE/TPC (para proyectos Matrix)
                    list.Add(mailUsr)
                    log.Info(" - added mailUsr for IDITURRIA " & My.Settings.IdPEs & " and OF " & itemOF & " -> " & mailUsr)
                Else
                    Dim wc As WebClient
                    wc = New WebClient
                    Try
                        Dim res As String
                        'res = wc.DownloadString("http://prodinternal.batz.com:8480/internal/restservices/batzservices/getPersonsAttributeWithRoleFromOF?OFName=" & itemOF.ToString & "&AttributeName=Email Address&RoleName=TPC&OnlyActive=true")
                        'If Not res.Trim.Equals("") Then '''' si existe rol TPC asignado en Enovia
                        '    list.AddRange(res.Split(";"))
                        '    log.Info(" - added TPCs " & res)
                        'Else
                        res = wc.DownloadString("http://prodinternal.batz.com:8480/internal/restservices/batzservices/getPersonsAttributeWithRoleFromOF?OFName=" & itemOF.ToString & "&AttributeName=Email Address&RoleName=Project Engineer&OnlyActive=true")
                        If Not res.Trim.Equals("") Then '''' si existe rol PE asignado en Enovia
                            list.AddRange(res.Split(";"))
                            log.Info(" - added PEs " & res)
                        End If
                        'End If
                    Catch ex As Exception
                        '...handle error...
                        log.Error("Error al conectarse a enovia")
                    End Try
                End If
            Next
            list = list.Select(Function(f) f.ToLower).ToList
            list = list.Distinct().ToList

            Dim lParaDefinitivo = BBDD.SAB_USUARIOS.AsEnumerable.Where(Function(f) list.Contains(f.EMAIL)).GroupBy(Function(o) o.EMAIL).Select(Function(o) o.FirstOrDefault())
            Dim lUsers = From reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA
                         Join usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS On reg.ORDEN Equals usr.ID
                         Where reg.IDITURRIA = My.Settings.AdminNotificaciones Select usr
            lParaDefinitivo = lParaDefinitivo.Union(lUsers)

            If Not ConfigurationManager.AppSettings("CurrentStatus").ToLower().Equals("live") Then
                log.Info("lParaTeorico:" & String.Join(",", lParaDefinitivo.Select(Function(f) f.EMAIL)))
                list = New List(Of String)
                list.Add("diglesias.external@batz.com")
                lParaDefinitivo = BBDD.SAB_USUARIOS.AsEnumerable.Where(Function(f) list.Contains(f.EMAIL)).GroupBy(Function(o) o.EMAIL).Select(Function(o) o.FirstOrDefault())
            End If

            log.Info("lParaDefinitivo:" & String.Join(",", lParaDefinitivo.Select(Function(f) f.EMAIL)))
            log.Info("ID: " & Incidencia.ID)
            If lParaDefinitivo.Any Then
                log.Info("Se va a avisar por correo")
                Dim t1 As String = ItzultzaileWeb.Itzuli(If(urgente, "URGENTE! ", "") & "Notificacion de NC: {0} / OF-(OP): {1}")
                log.Info("t1 traducido")
                AvisoCorre(sFrom, lParaDefinitivo, Nothing,
                                   String.Format(t1, Incidencia.ID, String.Join(", ", (From OFM As BatzBBDD.OFMARCA In Incidencia.OFMARCA Group By gOFM = OFM.NUMOF Into OFM_G = Group Select New With {gOFM, .gOP = String.Join(",", OFM_G.OrderBy(Function(o) o.OP).Select(Function(o) o.OP).Distinct)}).Select(Function(o) o.gOFM & "-(" & o.gOP & ")"))),
                                   ItzultzaileWeb.Itzuli("Notificacion de la creacion de la NC."),
                                   Incidencia)
            Else
                log.Warn("Notificación no enviada, no hay destinatarios válidos.")
            End If

        Catch ex As ApplicationException
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID, ex)
        Catch ex As Exception
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID, ex)
        End Try
    End Sub

    <WebMethod()> Sub NotificacionCompras(ByVal ID_GTK As Integer, ByVal sFrom As String, ByVal tipo As String)
        log.Info("Notificación a compras de NC " & tipo)
        Dim Incidencia As New BatzBBDD.GERTAKARIAK
        Dim mailCompras = System.Configuration.ConfigurationManager.AppSettings.Get("mailCompras")
        Dim lUsers = From reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA
                     Join usr As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS On reg.ORDEN Equals usr.ID
                     Where reg.IDITURRIA = My.Settings.AdminCompras Select usr
        Dim lParaDefinitivo = mailCompras & ";" & String.Join(";", lUsers.Select(Function(f) f.EMAIL))
        If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
            log.Info("lParaTeorico: " & lParaDefinitivo)
            lParaDefinitivo = "diglesias@batz.es"
        End If
        log.Info("lParaDefinitivo: " & lParaDefinitivo)
        Try
            Incidencia = (From Reg As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where Reg.ID = ID_GTK Select Reg).SingleOrDefault
            If Incidencia IsNot Nothing AndAlso Incidencia.OFMARCA.Any Then
                log.Info(sFrom)
                log.Info(Incidencia.ID)
                AvisoCorreoCompras(sFrom, lParaDefinitivo,
                               String.Format(ItzultzaileWeb.Itzuli("Notificacion de NC de " & tipo & ": {0} / OF-(OP): {1}"), Incidencia.ID, String.Join(", ", (From OFM As BatzBBDD.OFMARCA In Incidencia.OFMARCA Group By gOFM = OFM.NUMOF Into OFM_G = Group Select New With {gOFM, .gOP = String.Join(",", OFM_G.OrderBy(Function(o) o.OP).Select(Function(o) o.OP).Distinct)}).Select(Function(o) o.gOFM & "-(" & o.gOP & ")"))),
                               ItzultzaileWeb.Itzuli("Notificacion de la creacion de la NC de " & tipo & "."),
                               Incidencia)
            Else
                log.Info("Unknown NC with Id:" & ID_GTK)
            End If
        Catch ex As ApplicationException
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID, ex)
        Catch ex As Exception
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID, ex)
        End Try
    End Sub


    Sub AvisoCorre(from As String, lPara As IEnumerable(Of BatzBBDD.SAB_USUARIOS), lCopiaOculta As IEnumerable(Of BatzBBDD.SAB_USUARIOS), Subject As String, PiePagina As String, ByRef Incidencia As BatzBBDD.GERTAKARIAK)
        Try
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor

            log.Info("Enviando correo")
            Dim Body As String = Nothing
            Dim CCO As String = Nothing
            Dim url_Pagina As String = Context.Request.Url.Scheme & Uri.SchemeDelimiter & Context.Request.Url.Authority & Context.Request.ApplicationPath & "/Incidencia/Correos/PlantillaAviso.aspx?CodCultura={0}"
            Dim hrefNC As String = String.Format(Me.Context.Request.Url.Scheme & Uri.SchemeDelimiter & Context.Request.Url.Authority & Context.Request.ApplicationPath & "/Index.aspx?IdIncidencia={0}", Incidencia.ID)
            log.Info("phase 1")
            '------------------------------------------------------------------------------------------------------
            'Enviamos un aviso por correo
            '------------------------------------------------------------------------------------------------------

            If lPara Is Nothing OrElse Not lPara.Where(Function(Reg) Not String.IsNullOrWhiteSpace(Reg.EMAIL)).Any Then Throw New ApplicationException("Falta definir el destinario")
            If lCopiaOculta IsNot Nothing AndAlso lCopiaOculta.Any Then CCO = String.Join(";", From Reg In lCopiaOculta.AsEnumerable Where Not String.IsNullOrWhiteSpace(Reg.EMAIL) Select Reg.EMAIL)
            log.Info("phase 2")
            Dim sOFOPM As String = String.Join(" / ", (From Reg In Incidencia.OFMARCA Group By NUMOF = Reg.NUMOF, OP = Reg.OP Into gReg = Group Select New With {.NUMOF = NUMOF, .OP = OP, .MARCA = String.Join("-", gReg.Where(Function(o) Not String.IsNullOrWhiteSpace(o.MARCA)).Select(Function(o) o.MARCA.Trim).ToList)}).Select(Function(Reg) Reg.NUMOF & "-" & Reg.OP & If(String.IsNullOrWhiteSpace(Reg.MARCA), String.Empty, "-(" & Reg.MARCA & ")")))
            'Dim sFuncion As String = String.Join(", ", From Fun As BatzBBDD.ESTRUCTURA In ObtenerEstructuras(BBDD.ESTRUCTURA.Where(Function(o) o.ID = My.Settings.IdFunciones_TV).SingleOrDefault)
            '                                           Join EstGtk As BatzBBDD.ESTRUCTURA In Incidencia.ESTRUCTURA On Fun.ID Equals EstGtk.ID
            '                                           Select Fun.DESCRIPCION)
            Dim sSubProceso As String = String.Join(", ", From Fun As BatzBBDD.ESTRUCTURA In ObtenerEstructuras(BBDD.ESTRUCTURA.Where(Function(o) o.ID = My.Settings.IdSubprocesos_TV).SingleOrDefault)
                                                          Join EstGtk As BatzBBDD.ESTRUCTURA In Incidencia.ESTRUCTURA On Fun.ID Equals EstGtk.ID
                                                          Select Fun.DESCRIPCION)
            log.Info("phase 3")
            'Dim sAcciones As String = String.Empty
            'If Incidencia.G8D.Any Then
            '    Dim Etapa As BatzBBDD.G8D_E14 = Incidencia.G8D.SingleOrDefault.G8D_E14
            '    If Etapa IsNot Nothing Then
            '        sAcciones = Etapa.E4_DESCRIPCION_5
            '    End If
            'End If

            Dim sAcciones As String = Incidencia.DETALLEACCION
            ''''log.Info("phase 4")
            '''''-----------------------------------------------------------------------
            '''''Documentos Adjuntos
            '''''-----------------------------------------------------------------------

            ''''Dim lAdjuntos As New List(Of Net.Mail.Attachment)
            ''''If Incidencia.DOCUMENTOS.Any Then
            ''''    For Each Reg As BatzBBDD.DOCUMENTOS In Incidencia.DOCUMENTOS
            ''''        Dim fileName As String = Reg.NOMBRE & "." & Reg.EXTENSION
            ''''        Dim myAttachment As New Net.Mail.Attachment(New IO.MemoryStream(Reg.DOCUMENTO), fileName)
            ''''        lAdjuntos.Add(myAttachment)
            ''''    Next
            ''''End If
            log.Info("phase 5")
            '-----------------------------------------------------------------------

            For Each UsrSab As BatzBBDD.SAB_USUARIOS In lPara.Where(Function(Reg) Not String.IsNullOrWhiteSpace(Reg.EMAIL)).Distinct
                Subject = ItzultzaileWeb.Itzuli(Subject)
                'log.Info("phase 6")
                Dim a = String.Format(url_Pagina, UsrSab.IDCULTURAS)
                'log.Info(a)
                Dim a2 As ICredentials = New NetworkCredential("batznt\tareas", "tareas123")
                'log.Info(a2.ToString)
                Dim a3 As New WebProxy(a, True, Nothing, a2)
                'log.Info(a3.ToString)
                'ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
                Dim b = New HtmlAgilityPack.HtmlWeb().Load(a, "GET", a3, a2).DocumentNode.SelectSingleNode("//table").OuterHtml
                'log.Info(b.ToString)
                Dim c = Subject
                'log.Info(c)
                Dim d = If(Incidencia.SAB_USUARIOS Is Nothing, "?", String.Format("{0} - {1} {2} {3}", Incidencia.SAB_USUARIOS.CODPERSONA, Incidencia.SAB_USUARIOS.NOMBRE, Incidencia.SAB_USUARIOS.APELLIDO1, Incidencia.SAB_USUARIOS.APELLIDO2))
                'log.Info(d)
                Dim e = sOFOPM
                'log.Info(e)
                Dim f = If(String.IsNullOrWhiteSpace(sSubProceso), String.Empty, sSubProceso)
                'log.Info(f)
                Dim g = If(Incidencia.ESTRUCTURA(1) IsNot Nothing, Incidencia.ESTRUCTURA(1).DESCRIPCION, " ? ")
                'log.Info(g)
                Dim h = If(Incidencia.ESTRUCTURA(2) IsNot Nothing, Incidencia.ESTRUCTURA(2).DESCRIPCION, " ? ")
                'log.Info(h)
                Dim i = Incidencia.DESCRIPCIONPROBLEMA
                'log.Info(i)
                Dim j = If(String.IsNullOrWhiteSpace(sAcciones), String.Empty, sAcciones)
                'log.Info(j)
                Dim k = hrefNC
                'log.Info(k)
                Dim l = Incidencia.ID
                'log.Info(l)
                Dim m = PiePagina
                'log.Info(m)
                Body = String.Format(b, c, d, e, f, g, h, i, j, k, l, m)

                log.Info("phase 4b")
                '-----------------------------------------------------------------------
                'Documentos Adjuntos
                '-----------------------------------------------------------------------

                Dim lAdjuntos As New List(Of Net.Mail.Attachment)
                If Incidencia.DOCUMENTOS.Any Then
                    For Each Reg As BatzBBDD.DOCUMENTOS In Incidencia.DOCUMENTOS
                        Dim fileName As String = Reg.NOMBRE & "." & Reg.EXTENSION
                        Dim myAttachment As New Net.Mail.Attachment(New IO.MemoryStream(Reg.DOCUMENTO), fileName)
                        lAdjuntos.Add(myAttachment)
                    Next
                End If

                log.Info("phase 7")
                SabLib.BLL.Utils.EnviarEmail(from, UsrSab.EMAIL, Subject, Body, serverEmail, If(String.IsNullOrWhiteSpace(CCO), Nothing, CCO), lAdjuntos)
            Next
            '---------------------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID & vbCrLf & "Subject:" & Subject, ex)
            Throw
        End Try
        log.Info("mail enviado")
    End Sub


    Sub AvisoCorreoCompras(from As String, emailCompras As String, Subject As String, PiePagina As String, ByRef Incidencia As BatzBBDD.GERTAKARIAK)
        Try
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor

            Dim Body As String = Nothing
            Dim CCO As String = System.Configuration.ConfigurationManager.AppSettings.Get("mailCCO")
            Dim url_Pagina As String = Context.Request.Url.Scheme & Uri.SchemeDelimiter & Context.Request.Url.Authority & Context.Request.ApplicationPath & "/Incidencia/Correos/PlantillaAviso.aspx?CodCultura={0}"
            Dim hrefNC As String = String.Format(Me.Context.Request.Url.Scheme & Uri.SchemeDelimiter & Context.Request.Url.Authority & Context.Request.ApplicationPath & "/Index.aspx?IdIncidencia={0}", Incidencia.ID)
            log.Info("phase 1")
            '------------------------------------------------------------------------------------------------------
            'Enviamos un aviso por correo

            Dim sOFOPM As String = String.Join(" / ", (From Reg In Incidencia.OFMARCA Group By NUMOF = Reg.NUMOF, OP = Reg.OP Into gReg = Group Select New With {.NUMOF = NUMOF, .OP = OP, .MARCA = String.Join("-", gReg.Where(Function(o) Not String.IsNullOrWhiteSpace(o.MARCA)).Select(Function(o) o.MARCA.Trim).ToList)}).Select(Function(Reg) Reg.NUMOF & "-" & Reg.OP & If(String.IsNullOrWhiteSpace(Reg.MARCA), String.Empty, "-(" & Reg.MARCA & ")")))
            Dim sSubProceso As String = String.Join(", ", From Fun As BatzBBDD.ESTRUCTURA In ObtenerEstructuras(BBDD.ESTRUCTURA.Where(Function(o) o.ID = My.Settings.IdSubprocesos_TV).SingleOrDefault)
                                                          Join EstGtk As BatzBBDD.ESTRUCTURA In Incidencia.ESTRUCTURA On Fun.ID Equals EstGtk.ID
                                                          Select Fun.DESCRIPCION)
            log.Info("phase 3")
            Dim sAcciones As String = String.Empty
            If Incidencia.G8D.Any Then
                Dim Etapa As BatzBBDD.G8D_E14 = Incidencia.G8D.SingleOrDefault.G8D_E14
                If Etapa IsNot Nothing Then
                    sAcciones = Etapa.E4_DESCRIPCION_5
                End If
            End If
            log.Info("phase 4")
            '-----------------------------------------------------------------------
            'Documentos Adjuntos
            '-----------------------------------------------------------------------

            Dim lAdjuntos As New List(Of Net.Mail.Attachment)
            If Incidencia.DOCUMENTOS.Any Then
                For Each Reg As BatzBBDD.DOCUMENTOS In Incidencia.DOCUMENTOS
                    Dim fileName As String = Reg.NOMBRE & "." & Reg.EXTENSION
                    Dim myAttachment As New Net.Mail.Attachment(New IO.MemoryStream(Reg.DOCUMENTO), fileName)
                    lAdjuntos.Add(myAttachment)
                Next
            End If
            log.Info("phase 5")
            '-----------------------------------------------------------------------

            Subject = ItzultzaileWeb.Itzuli(Subject)
            log.Info("phase 6")
            Dim a = String.Format(url_Pagina, "es-ES")
            Dim a2 As ICredentials = New NetworkCredential("batznt\tareas", "tareas123")
            Dim a3 As New WebProxy(a, True, Nothing, a2)
            Dim b = New HtmlAgilityPack.HtmlWeb().Load(a, "GET", a3, a2).DocumentNode.SelectSingleNode("//table").OuterHtml
            Dim c = Subject
            Dim d = If(Incidencia.SAB_USUARIOS Is Nothing, "?", String.Format("{0} - {1} {2} {3}", Incidencia.SAB_USUARIOS.CODPERSONA, Incidencia.SAB_USUARIOS.NOMBRE, Incidencia.SAB_USUARIOS.APELLIDO1, Incidencia.SAB_USUARIOS.APELLIDO2))
            Dim e = sOFOPM
            Dim f = If(String.IsNullOrWhiteSpace(sSubProceso), String.Empty, sSubProceso)
            Dim g = If(Incidencia.ESTRUCTURA(1) IsNot Nothing, Incidencia.ESTRUCTURA(1).DESCRIPCION, " ? ")
            Dim h = If(Incidencia.ESTRUCTURA(2) IsNot Nothing, Incidencia.ESTRUCTURA(2).DESCRIPCION, " ? ")
            Dim i = Incidencia.DESCRIPCIONPROBLEMA
            Dim j = If(String.IsNullOrWhiteSpace(sAcciones), String.Empty, sAcciones)
            Dim k = hrefNC
            Dim l = Incidencia.ID
            Dim m = PiePagina

            Body = String.Format(b, c, d, e, f, g, h, i, j, k, l, m)
            log.Info("phase 7")
            SabLib.BLL.Utils.EnviarEmail(from, emailCompras, Subject, Body, serverEmail, If(String.IsNullOrWhiteSpace(CCO), Nothing, CCO), lAdjuntos)
            '---------------------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            log.Error(vbCrLf & "Incidencia.ID: " & Incidencia.ID & vbCrLf & "Subject:" & Subject, ex)
            Throw
        End Try
        log.Info("mail enviado a compras")
    End Sub

    ''' <summary>
    ''' Listado de estructuras que componen el nodo dado.
    ''' </summary>
    ''' <param name="EstructuraInicial"></param>
    ''' <param name="lEst"></param>
    ''' <returns></returns>
    Function ObtenerEstructuras(ByRef EstructuraInicial As BatzBBDD.ESTRUCTURA, Optional ByRef lEst As List(Of BatzBBDD.ESTRUCTURA) = Nothing) As List(Of BatzBBDD.ESTRUCTURA)
        If EstructuraInicial IsNot Nothing Then
            If lEst Is Nothing Then lEst = New List(Of BatzBBDD.ESTRUCTURA)
            For Each Reg As BatzBBDD.ESTRUCTURA In EstructuraInicial.ESTRUCTURA1
                lEst.Add(Reg)
                lEst = ObtenerEstructuras(Reg, lEst)
            Next
        End If
        Return lEst
    End Function

    <WebMethod()> Public Function get_Capacidades_OF(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        get_Capacidades_OF = Nothing
        Dim Lista_cdd As New List(Of AjaxControlToolkit.CascadingDropDownNameValue)
        Dim Capacidad_NC As AjaxControlToolkit.CascadingDropDownNameValue = Nothing
        Dim JavaScriptSerializer As New Script.Serialization.JavaScriptSerializer()
        Try
            If Not String.IsNullOrWhiteSpace(category) Then
                Capacidad_NC = (From Reg As BatzBBDD.CAPACIDADES In BBDD.CAPACIDADES.AsEnumerable Where Reg.CAPID = category Select New AjaxControlToolkit.CascadingDropDownNameValue(Reg.NOMBRE, Reg.CAPID, True)).SingleOrDefault
            End If

            If Not String.IsNullOrWhiteSpace(contextKey) Then
                Dim contextKey_json = JavaScriptSerializer.Deserialize(Of Object)(contextKey)

                Dim lOFM As List(Of String) = If(String.IsNullOrWhiteSpace(contextKey_json("hd_OFOPM")), Nothing, Split(contextKey_json("hd_OFOPM"), ";").ToList)
                Dim IdProv As Nullable(Of Integer) = If(contextKey_json("IdProv") Is Nothing OrElse String.IsNullOrWhiteSpace(contextKey_json("IdProv")), New Nullable(Of Integer), CInt(contextKey_json("IdProv")))

                '----------------------------------------------------------------------------------------------------------------------------
                'Filtramos por "Vigentes"
                'HABPOT (O -> Obsoleto, H -> Habitual, P -> Potencial)
                '----------------------------------------------------------------------------------------------------------------------------
                Dim lProSubID As List(Of String) = (From Prov As BatzBBDD.GCPROVEE In BBDD.GCPROVEE Where String.Compare(Prov.HABPOT, "O", True) <> 0
                                                    From Cab As BatzBBDD.SCPEDCAB In Prov.SCPEDCAB Where Cab IsNot Nothing
                                                    From Lin As BatzBBDD.SCPEDLIN In Cab.SCPEDLIN Where Lin IsNot Nothing
                                                    Select Prov.CODPRO Distinct).ToList
                Dim lCodProID As List(Of String) = (From Prov As BatzBBDD.GCPROVEE In BBDD.GCPROVEE Where String.Compare(Prov.HABPOT, "O", True) <> 0
                                                    From Cab As BatzBBDD.GCCABPED In Prov.GCCABPED Where Cab IsNot Nothing
                                                    From Lin As BatzBBDD.GCLINPED In Cab.GCLINPED Where Lin IsNot Nothing
                                                    Select Prov.CODPRO Distinct).ToList
                '----------------------------------------------------------------------------------------------------------------------------

                If lOFM IsNot Nothing AndAlso lOFM.Any Then
                    For Each OFM In lOFM
                        Dim aOFM = Split(OFM, ":")
                        Dim OrdenF As Integer = aOFM(0)
                        Dim Operacion As Integer = aOFM(1)
                        Dim Marca As String = aOFM(2)

                        '-----------------------------------------------------------------------------------------
                        'Proveedores de SubContratacion.
                        'Los pedidos de SubContratacion no llevan marca.
                        '-----------------------------------------------------------------------------------------
                        lProSubID = (From Cab As BatzBBDD.SCPEDCAB In BBDD.SCPEDCAB
                                     From Lin As BatzBBDD.SCPEDLIN In Cab.SCPEDLIN
                                     Where lProSubID.Contains(Cab.CODPROEXT) And Lin.NUMORDLIN = OrdenF And Lin.NUMOPELIN = Operacion
                                     Select Cab.CODPROEXT Distinct).ToList
                        '-----------------------------------------------------------------------------------------
                        '-----------------------------------------------------------------------------------------
                        'Proveedores de Troqueleria.
                        '-----------------------------------------------------------------------------------------
                        lCodProID = (From Lin As BatzBBDD.GCLINPED In BBDD.GCLINPED
                                     Where lCodProID.Contains(Lin.CODPROLIN) _
                                        And Lin.NUMORDF = OrdenF And Lin.NUMOPE = Operacion _
                                        And If(Marca Is Nothing OrElse Marca.Trim = String.Empty, True = True, Lin.NUMMAR.Trim = Marca)
                                     Select Lin.CODPROLIN Distinct).ToList
                        '-----------------------------------------------------------------------------------------
                    Next
                    'Proveedores relacionados con las OFs
                    Dim lProveedores_OF = From Prov As BatzBBDD.GCPROVEE In BBDD.GCPROVEE Where lCodProID.Contains(Prov.CODPRO) Or lProSubID.Contains(Prov.CODPRO) Select Prov Distinct
                    If IdProv IsNot Nothing Then lProveedores_OF = lProveedores_OF.Where(Function(o) o.CODPRO = IdProv)
                    '-----------------------------------------

                    Dim lCAPACIDADES As New List(Of BatzBBDD.CAPACIDADES)
                    If lProveedores_OF.Any Then
                        lCAPACIDADES = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                        Join Prov As BatzBBDD.GCPROVEE In lProveedores_OF On Prov.CODPRO.Trim Equals Reg.IDTROQUELERIA.Trim
                                        From Cap As BatzBBDD.CAPACIDADES In Reg.CAPACIDADES
                                        Select Cap Distinct).ToList
                    Else
                        lCAPACIDADES = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                        From Cap As BatzBBDD.CAPACIDADES In Reg.CAPACIDADES
                                        Select Cap Distinct).ToList
                    End If

                    '-----------------------------------------

                    Dim lReg = lCAPACIDADES.Where(Function(o) o.CAPID <> category And o.OBSOLETO = 0).Select(Function(o) New AjaxControlToolkit.CascadingDropDownNameValue(o.NOMBRE, o.CAPID)).Distinct
                    If lReg IsNot Nothing AndAlso lReg.Any Then
                        Lista_cdd.AddRange(lReg)
                    End If
                    '-----------------------------------------
                End If
            End If

            'If Capacidad_NC IsNot Nothing Then Lista_cdd.Add(Capacidad_NC)
            If Capacidad_NC Is Nothing And Lista_cdd.Any AndAlso Lista_cdd.Count = 1 Then
                Lista_cdd.FirstOrDefault.isDefaultValue = True
            ElseIf Capacidad_NC IsNot Nothing Then
                Lista_cdd.Add(Capacidad_NC)
            End If
            get_Capacidades_OF = Lista_cdd.OrderBy(Function(o) o.name).ToArray

        Catch ex As Exception
            log.Error(ex)
        End Try

        Return get_Capacidades_OF
    End Function

    <WebMethod()>
    Public Function get_Proveedor(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        get_Proveedor = Nothing
        Try
            'Dim BBDD As New BatzBBDD.Entities_Gertakariak
            If prefixText IsNot Nothing Then
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                Dim lProveedores As List(Of BatzBBDD.EMPRESAS) = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                                                  Join Plt As BatzBBDD.PLANTAS In BBDD.PLANTAS On Reg.IDPLANTA Equals Plt.ID
                                                                  Where (Reg.FECHABAJA Is Nothing Or Reg.FECHABAJA <= Now) And Plt.ID = contextKey Select Reg Distinct).ToList
                For Each Texto As String In aPrefixText
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                    lProveedores = (From Reg As BatzBBDD.EMPRESAS In lProveedores
                                    Where If(String.IsNullOrWhiteSpace(Reg.CIF), Nothing, ExpReg.IsMatch(Reg.CIF)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.CONTACTO), Nothing, ExpReg.IsMatch(Reg.CONTACTO)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.CPOSTAL), Nothing, ExpReg.IsMatch(Reg.CPOSTAL)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.DIRECCION), Nothing, ExpReg.IsMatch(Reg.DIRECCION)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.FAX), Nothing, ExpReg.IsMatch(Reg.FAX)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.IDTROQUELERIA), Nothing, ExpReg.IsMatch(Reg.IDTROQUELERIA)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.LOCALIDAD), Nothing, ExpReg.IsMatch(Reg.LOCALIDAD)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.NOMBRE), Nothing, ExpReg.IsMatch(Reg.NOMBRE)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.PROVINCIA), Nothing, ExpReg.IsMatch(Reg.PROVINCIA)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.TELEFONO), Nothing, ExpReg.IsMatch(Reg.TELEFONO))
                                    Select Reg Distinct).ToList
                Next
                If lProveedores.Any Then
                    get_Proveedor = lProveedores.Select(Function(o) New With {.Id = o.IDTROQUELERIA,
                                                            .Descripcion = If(String.IsNullOrWhiteSpace(o.NOMBRE), String.Empty, o.NOMBRE.Trim) & "  (" & o.LOCALIDAD & " - " & o.PROVINCIA & ")"}).OrderBy(Function(o) o.Descripcion) _
                                                            .Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                                            If(String.IsNullOrWhiteSpace(o.Descripcion), String.Empty, o.Descripcion.Trim), o.Id)).Distinct.Take(20).ToArray
                End If

            End If

        Catch ex As Exception
            log.Error("WebMethod()_ get_Proveedor", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_Proveedor
    End Function
    <WebMethod()> Public Function get_Proveedor_OF(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        get_Proveedor_OF = Nothing
        Dim Lista_cdd As New List(Of AjaxControlToolkit.CascadingDropDownNameValue)
        'Dim Proveedor_NC As AjaxControlToolkit.CascadingDropDownNameValue = Nothing
        Dim JavaScriptSerializer As New Script.Serialization.JavaScriptSerializer()
        Try
            'If Not String.IsNullOrWhiteSpace(category) Then
            '    log.Debug(" *category: " & category)
            '    Proveedor_NC = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS.AsEnumerable Where Reg.IDTROQUELERIA = category Select New AjaxControlToolkit.CascadingDropDownNameValue(Reg.NOMBRE, Reg.IDTROQUELERIA, True)).SingleOrDefault
            '    If Proveedor_NC IsNot Nothing Then Lista_cdd.Add(Proveedor_NC)
            'End If

            If Not String.IsNullOrWhiteSpace(contextKey) Then
                Dim contextKey_json = JavaScriptSerializer.Deserialize(Of Object)(contextKey)

                Dim lOFM As List(Of String) = If(String.IsNullOrWhiteSpace(contextKey_json("hd_OFOPM")), Nothing, Split(contextKey_json("hd_OFOPM"), ";").ToList)
                Dim IdCap As String = If(contextKey_json("IdCap") Is Nothing OrElse String.IsNullOrWhiteSpace(contextKey_json("IdCap")), Nothing, contextKey_json("IdCap"))
                'log.Debug("lOFM:" & String.Join("&",lOFM) & " idCap:" & IdCap)
                '----------------------------------------------------------------------------------------------------------------------------
                'Filtramos por "Vigentes"
                'HABPOT (O -> Obsoleto, H -> Habitual, P -> Potencial)
                '----------------------------------------------------------------------------------------------------------------------------
                Dim lProSubID As List(Of String) = (From Prov As BatzBBDD.GCPROVEE In BBDD.GCPROVEE
                                                    From Cab As BatzBBDD.SCPEDCAB In Prov.SCPEDCAB Where Cab IsNot Nothing
                                                    From Lin As BatzBBDD.SCPEDLIN In Cab.SCPEDLIN Where Lin IsNot Nothing
                                                    Select Prov.CODPRO.Trim Distinct).ToList
                Dim lCodProID As List(Of String) = (From Prov As BatzBBDD.GCPROVEE In BBDD.GCPROVEE
                                                    From Cab As BatzBBDD.GCCABPED In Prov.GCCABPED Where Cab IsNot Nothing
                                                    From Lin As BatzBBDD.GCLINPED In Cab.GCLINPED Where Lin IsNot Nothing
                                                    Select Prov.CODPRO.Trim Distinct).ToList
                'Dim lProSubIDFinal As New List(Of String)
                'Dim lCodProIDFinal As New List(Of String)
                '----------------------------------------------------------------------------------------------------------------------------
                'log.Debug("Getting providers:")
                If lOFM IsNot Nothing AndAlso lOFM.Any Then
                    For Each OFM In lOFM
                        Dim aOFM = Split(OFM, ":")
                        Dim OrdenF As Integer = aOFM(0)
                        Dim Operacion As Integer = aOFM(1)
                        Dim Marca As String = aOFM(2)
                        'log.Debug("  OrdenF:" & OrdenF & ", Operacion:" & Operacion & ", Marca:" & Marca & ", category:" & category & ", IdCap:" & IdCap)
                        '-----------------------------------------------------------------------------------------
                        'Proveedores de SubContratacion.
                        'Los pedidos de SubContratacion no llevan marca.
                        '-----------------------------------------------------------------------------------------
                        lProSubID = (From Cab As BatzBBDD.SCPEDCAB In BBDD.SCPEDCAB
                                     From Lin As BatzBBDD.SCPEDLIN In Cab.SCPEDLIN
                                     Where lProSubID.Contains(Cab.CODPROEXT.Trim) And Lin.NUMORDLIN = OrdenF And Lin.NUMOPELIN = Operacion
                                     Select Cab.CODPROEXT.Trim Distinct).ToList
                        'log.Debug("    lproSubIDTemp count: " & lProSubID.Count & "(" & String.Join(",", lProSubID) & ")")
                        'lProSubIDFinal.AddRange(lProSubIDTemp)

                        '-----------------------------------------------------------------------------------------
                        '-----------------------------------------------------------------------------------------
                        'Proveedores de Troqueleria.
                        '-----------------------------------------------------------------------------------------
                        lCodProID = (From Lin As BatzBBDD.GCLINPED In BBDD.GCLINPED
                                     Where lCodProID.Contains(Lin.CODPROLIN.Trim) _
                                        And Lin.NUMORDF = OrdenF And Lin.NUMOPE = Operacion _
                                         And If(Marca Is Nothing OrElse Marca.Trim = String.Empty, True = True, Lin.NUMMAR.Trim.ToUpper = Marca.Trim.ToUpper)
                                     Select Lin.CODPROLIN.Trim Distinct).ToList
                        'log.Debug("    lcodProIDTemp count: " & lCodProID.Count & "(" & String.Join(",", lCodProID) & ")")

                        'lCodProIDFinal.AddRange(lCodProIDTemp)

                        '-----------------------------------------------------------------------------------------
                    Next

                    'Proveedores relacionados con las OFs
                    Dim lProveedores_OF = From Prov As BatzBBDD.GCPROVEE In BBDD.GCPROVEE Where lCodProID.Contains(Prov.CODPRO.Trim) Or lProSubID.Contains(Prov.CODPRO.Trim) Select Prov Distinct
                    '-----------------------------------------
                    'log.Debug("  lproveedores count: " & lProveedores_OF.Count)
                    Dim lEmpresas = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                     Join Prov As BatzBBDD.GCPROVEE In lProveedores_OF On Prov.CODPRO.Trim Equals Reg.IDTROQUELERIA.Trim
                                     From Cap As BatzBBDD.CAPACIDADES In Reg.CAPACIDADES
                                     Where String.Compare(Prov.HABPOT, "O", True) <> 0
                                     Select Reg, Prov.HABPOT Distinct Order By Reg.NOMBRE).ToList
                    'Identificamos a los proveedores "Obsoletos" (HABPOT=O)
                    Dim lEmpresas_O = (From Reg As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                       Join Prov As BatzBBDD.GCPROVEE In lProveedores_OF On Prov.CODPRO.Trim Equals Reg.IDTROQUELERIA.Trim
                                       From Cap As BatzBBDD.CAPACIDADES In Reg.CAPACIDADES
                                       Where String.Compare(Prov.HABPOT, "O", True) = 0 Order By Reg.NOMBRE
                                       Select Reg, Prov.HABPOT Distinct).ToList
                    lEmpresas = lEmpresas.Union(lEmpresas_O).ToList
                    'log.Debug("  empresa count1: " & lEmpresas.Count)
                    For Each emp In lEmpresas
                        'log.Debug("    emp:" & emp.Reg.ID)
                        For Each cap In emp.Reg.CAPACIDADES
                            'log.Debug("     cap:'" & cap.CAPID & "'")
                        Next
                    Next
                    'If IdCap IsNot Nothing Then lEmpresas = lEmpresas.Where(Function(o) o.Reg.CAPACIDADES IsNot Nothing AndAlso o.Reg.CAPACIDADES.Any AndAlso o.Reg.CAPACIDADES.FirstOrDefault.CAPID = IdCap).ToList
                    If IdCap IsNot Nothing Then lEmpresas = lEmpresas.Where(Function(o) o.Reg.CAPACIDADES IsNot Nothing AndAlso o.Reg.CAPACIDADES.Any AndAlso o.Reg.CAPACIDADES.Where(Function(c) c.CAPID = IdCap).Any).ToList
                    'log.Debug("  empresa count2: " & lEmpresas.Count)
                    '-----------------------------------------
                    Dim lReg As New List(Of AjaxControlToolkit.CascadingDropDownNameValue)
                    If Not String.IsNullOrWhiteSpace(category) Then
                        lReg = lEmpresas.Where(Function(o) o.Reg.IDTROQUELERIA <> category).Select(Function(o) New AjaxControlToolkit.CascadingDropDownNameValue(If(String.Compare(o.HABPOT, "O", True) <> 0, String.Empty, "(*)") & o.Reg.NOMBRE, o.Reg.IDTROQUELERIA)).Distinct.ToList()
                    Else
                        lReg = lEmpresas.Select(Function(o) New AjaxControlToolkit.CascadingDropDownNameValue(If(String.Compare(o.HABPOT, "O", True) <> 0, String.Empty, "(*)") & o.Reg.NOMBRE, o.Reg.IDTROQUELERIA)).Distinct.ToList()
                    End If
                    If lReg IsNot Nothing AndAlso lReg.Any Then Lista_cdd.AddRange(lReg)
                    '-----------------------------------------
                    If lReg IsNot Nothing AndAlso lReg.Any Then
                        'log.Debug("  lreg count: " & lReg.Count)
                    End If
                End If

            End If

            'If Proveedor_NC Is Nothing And Lista_cdd.Any AndAlso Lista_cdd.Count = 1 Then
            '    Lista_cdd.FirstOrDefault.isDefaultValue = True
            'ElseIf Proveedor_NC IsNot Nothing Then
            '    Lista_cdd.Add(Proveedor_NC)
            'End If
            'get_Proveedor_OF = Lista_cdd.OrderBy(Function(o) o.name).ToArray

            'If Proveedor_NC Is Nothing And Lista_cdd.Any AndAlso Lista_cdd.Count = 1 Then Lista_cdd.FirstOrDefault.isDefaultValue = True
            get_Proveedor_OF = Lista_cdd.ToArray

        Catch ex As Exception
            log.Error(ex)
        End Try

        Return get_Proveedor_OF
    End Function
    <WebMethod()>
    Public Function get_Proveedor_Filtro(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        get_Proveedor_Filtro = Nothing
        Try
            If prefixText IsNot Nothing Then
                Dim aPrefixText As String() = prefixText.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                Dim lProveedores As List(Of BatzBBDD.EMPRESAS) = (From Emp As BatzBBDD.EMPRESAS In BBDD.EMPRESAS
                                                                  Join GTK As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK On GTK.IDPROVEEDOR Equals Emp.IDTROQUELERIA
                                                                  Where GTK.IDTIPOINCIDENCIA = contextKey And Emp.IDTROQUELERIA IsNot Nothing Select Emp Distinct).ToList
                For Each Texto As String In aPrefixText
                    Dim ExpReg As Regex = New Regex(SabLib.BLL.Utils.TextoLike(Texto), RegexOptions.IgnoreCase)
                    lProveedores = (From Reg As BatzBBDD.EMPRESAS In lProveedores
                                    Where If(String.IsNullOrWhiteSpace(Reg.CIF), Nothing, ExpReg.IsMatch(Reg.CIF)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.CONTACTO), Nothing, ExpReg.IsMatch(Reg.CONTACTO)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.CPOSTAL), Nothing, ExpReg.IsMatch(Reg.CPOSTAL)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.DIRECCION), Nothing, ExpReg.IsMatch(Reg.DIRECCION)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.FAX), Nothing, ExpReg.IsMatch(Reg.FAX)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.IDTROQUELERIA), Nothing, ExpReg.IsMatch(Reg.IDTROQUELERIA)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.LOCALIDAD), Nothing, ExpReg.IsMatch(Reg.LOCALIDAD)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.NOMBRE), Nothing, ExpReg.IsMatch(Reg.NOMBRE)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.PROVINCIA), Nothing, ExpReg.IsMatch(Reg.PROVINCIA)) _
                                    Or If(String.IsNullOrWhiteSpace(Reg.TELEFONO), Nothing, ExpReg.IsMatch(Reg.TELEFONO))
                                    Select Reg Distinct).ToList
                Next
                If lProveedores.Any Then
                    get_Proveedor_Filtro = lProveedores.Select(Function(o) New With {.Id = o.IDTROQUELERIA,
                                                            .Descripcion = If(String.IsNullOrWhiteSpace(o.NOMBRE), String.Empty, o.NOMBRE.Trim) & "  (" & o.LOCALIDAD & " - " & o.PROVINCIA & ")"}).OrderBy(Function(o) o.Descripcion) _
                                                            .Select(Function(o) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                                                            If(String.IsNullOrWhiteSpace(o.Descripcion), String.Empty, o.Descripcion.Trim), o.Id)).Distinct.Take(20).ToArray
                End If

            End If

        Catch ex As Exception
            log.Error("WebMethod()_ get_Proveedor_Filtro", ex)
            Return New String() {ex.Message & " " & ex.StackTrace.ToString}
        End Try
        Return get_Proveedor_Filtro
    End Function

    <WebMethod()>
    Public Function get_Proyecto_Cliente(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        get_Proyecto_Cliente = Nothing
        Try
            Dim kv As StringDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            Dim IdCliente As Integer = CInt(kv.Values(0))
            Dim lReg = (From Reg As BatzBBDD.W_PROYECTO_CLIENTE_OF_TODAS In BBDD.W_PROYECTO_CLIENTE_OF_TODAS
                        Join OFMARCA As BatzBBDD.OFMARCA In BBDD.OFMARCA On Reg.NUMORD Equals OFMARCA.NUMOF
                        Where Reg.ID_CLIENTE = IdCliente
                        Select New With {Key Reg.DESCRI, Key Reg.ID} Distinct).OrderBy(Function(o) o.DESCRI).ToList.Select(Function(o) New AjaxControlToolkit.CascadingDropDownNameValue(o.DESCRI, o.ID))

            get_Proyecto_Cliente = lReg.ToArray
        Catch ex As Exception
            log.Error(ex)
        End Try
        Return get_Proyecto_Cliente
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False, XmlSerializeString:=False)>
    Public Function get_Relaciones_Mant(ByVal Id_Origen As String) As String
        Dim l_ID As New List(Of Decimal)
        If IsNumeric(Id_Origen.Replace(".", "_")) Then
            l_ID = (From Reg As BatzBBDD.ESTRUCTURA_TROQUELERIA In BBDD.ESTRUCTURA_TROQUELERIA Where Reg.IDEST_ORIGEN = CDec(Id_Origen) Select Reg.IDESTRUCTURA Distinct).ToList
        Else
            l_ID = (From Reg As BatzBBDD.ESTRUCTURA_TROQUELERIA In BBDD.ESTRUCTURA_TROQUELERIA Where Reg.IDCAP_ORIGEN = Id_Origen Select Reg.IDESTRUCTURA Distinct).ToList
        End If
        get_Relaciones_Mant = "[" & String.Join(", ", l_ID) & "]"

        Return "[" & String.Join(", ", l_ID) & "]"
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=False, XmlSerializeString:=False)>
    Public Function get_Relaciones(ByVal Id_Origen As String, ByVal Id_Relacion As String) As String
        Dim json_Data As String = String.Empty
        Dim lEstructura As New List(Of BatzBBDD.ESTRUCTURA)
        Dim Est_ID As String = get_Relaciones_Mant(Id_Origen)
        Dim lEst_ID As List(Of Integer) = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of Integer))(Est_ID)

        lEstructura = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA.AsEnumerable Where lEst_ID.Contains(Reg.ID)).OrderBy(Function(o) o.ORDEN).ThenBy(Function(o) o.DESCRIPCION).ToList

        If lEstructura.Any Then
            Dim Nodo1 As Boolean = True
            For Each Reg As BatzBBDD.ESTRUCTURA In lEstructura
                If Nodo1 = False Then json_Data &= ", "
                'Dim extra = getExtraInfoForCaracteristica(Reg.ID)
                'json_Data &= String.Format("{{""text"":""{0}"", ""idEstructura"":""{1}"", ""extra"":""{2}""", Reg.DESCRIPCION, Reg.ID, extra)
                json_Data &= String.Format("{{""text"":""{0}"", ""idEstructura"":""{1}""", Reg.DESCRIPCION, Reg.ID)
                json_Data &= "}"
                Nodo1 = False
            Next
            json_Data = "[" & json_Data & "]"
        Else
            'json_Data = New Captura2().Treeview_data(BuscarOrigen(BBDD.ESTRUCTURA.Find(CDec(Id_Relacion))).ESTRUCTURA1.AsQueryable.OrderBy(Function(o) o.ORDEN).ThenBy(Function(o) o.DESCRIPCION))
        End If

        Return json_Data
    End Function


    Private Function getExtraInfoForCaracteristica(iD As Decimal) As String
        Dim cx As String
        If ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
            cx = ConfigurationManager.ConnectionStrings.Item("ConexionWeb_LIVE").ConnectionString
        Else
            cx = ConfigurationManager.ConnectionStrings.Item("ConexionWeb_TEST").ConnectionString
        End If
        'Dim query As String = "SELECT ORDEN,DESCRIPCION,EXTRA FROM ESTRUCTURA WHERE ID =:ID"
        Dim query As String = "SELECT EXTRA FROM ESTRUCTURA WHERE ID =:ID"
        Dim lParametros As New List(Of OracleParameter)
        lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, iD, ParameterDirection.Input))
        Dim result = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, cx, lParametros.ToArray)
        Return result
    End Function


    Function BuscarOrigen(ByVal Estructura As BatzBBDD.ESTRUCTURA) As BatzBBDD.ESTRUCTURA
        Return If(Estructura.ESTRUCTURA_Origen Is Nothing, Estructura, BuscarOrigen(Estructura.ESTRUCTURA_Origen))
    End Function
End Class