Imports System.Web.Mvc
Imports TarjetasVisitaLib

Namespace Controllers
    Public Class DatosAlternativosController
        Inherits BaseController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            ViewData("ContainerFluid") = True
            ViewData("DatosAlternativos") = DatosAlternativosBLL.CargarListado()
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Agregar() As ActionResult
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="descripcion"></param>
        ''' <param name="hfUsuario"></param>
        ''' <param name="nombre"></param>
        ''' <param name="puesto"></param>
        ''' <param name="movil"></param>
        ''' <param name="direccion"></param>
        ''' <param name="fijo"></param>
        ''' <param name="email"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Agregar(ByVal descripcion As String, ByVal hfUsuario As String, ByVal nombre As String, ByVal puesto As String, ByVal movil As String, ByVal direccion As String, ByVal fijo As String, ByVal email As String) As ActionResult
            If (String.IsNullOrEmpty(hfUsuario)) Then
                MensajeAlerta(Utils.Traducir(Utils.Traducir("Trabajador") & " " & Utils.Traducir("campo obligatorio")))
                Return Agregar()
            End If

            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = hfUsuario})

            Dim departamentosBLL As New SabLib.BLL.DepartamentosComponent
            Dim departamento As SabLib.ELL.Departamento = departamentosBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = usuario.IdDepartamento, .IdPlanta = usuario.IdPlanta})

            Dim datosAlternativos As New ELL.DatosAlternativos With {.Descripcion = descripcion, .Nombre = nombre, .Puesto = puesto, .Movil = movil, .Direccion = direccion, .Fijo = fijo, .Email = email, .IdSab = CInt(hfUsuario)}

            Try
                BLL.DatosAlternativosBLL.Guardar(datosAlternativos)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Agregar()
            End Try

            Return RedirectToAction("Agregar")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param> 
        ''' <returns></returns>
        Function Editar(ByVal id As Integer) As ActionResult
            ViewData("DatosAlternativos") = BLL.DatosAlternativosBLL.Obtener(id)
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="hfUsuario"></param>
        ''' <param name="nombre"></param>
        ''' <param name="puesto"></param>
        ''' <param name="movil"></param>
        ''' <param name="direccion"></param>
        ''' <param name="fijo"></param>
        ''' <param name="email"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal id As Integer, ByVal descripcion As String, ByVal hfUsuario As String, ByVal nombre As String, ByVal puesto As String, ByVal movil As String, ByVal direccion As String, ByVal fijo As String, ByVal email As String) As ActionResult
            If (String.IsNullOrEmpty(hfUsuario)) Then
                MensajeAlerta(Utils.Traducir(Utils.Traducir("Trabajador") & " " & Utils.Traducir("campo obligatorio")))
                Return Agregar()
            End If

            Dim datosAlternativos As New ELL.DatosAlternativos With {.Id = id, .Descripcion = descripcion, .Nombre = nombre, .Puesto = puesto, .Movil = movil, .Direccion = direccion, .Fijo = fijo, .Email = email, .IdSab = CInt(hfUsuario)}

            Try
                BLL.DatosAlternativosBLL.Guardar(datosAlternativos)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(id)
            End Try

            Return RedirectToAction("Editar", New With {.id = id})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal id As Integer) As ActionResult
            Try
                BLL.DatosAlternativosBLL.Eliminar(id)
                MensajeInfo(Utils.Traducir("Datos alternativos eliminados correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar datos alternativos"))
                log.Error("Error al eliminar datos alternativos", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Function CargarTelefonos(ByVal idSab As Integer, ByVal idPlanta As Integer) As JsonResult
            Dim telefonos As ServicioTelefonia.Telephone = BLL.TelefoniaBLL.CargarListado(idSab).FirstOrDefault(Function(f) f.IdSabPlanta = idPlanta)

            Return Json(telefonos, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Function CargarPuesto(ByVal idSab As Integer) As JsonResult
            Dim puesto As ELL.Puesto = BLL.PKSBLL.ObtenerPuesto(idSab)

            'Forzamos a ingles
            puesto.Nombre = Utils.Traducir(puesto.Nombre, "en-GB")

            Return Json(puesto, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <param name="cultura"></param>
        ''' <returns></returns>
        Function CargarPuestoCultura(ByVal idSab As Integer, ByVal cultura As String) As JsonResult
            Dim puesto As ELL.Puesto = BLL.PKSBLL.ObtenerPuesto(idSab)

            puesto.Nombre = Utils.Traducir(puesto.Nombre, cultura)

            Return Json(puesto, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function SeleccionarDatosAlternativos() As ActionResult
            Dim datosAlternativos As List(Of ELL.DatosAlternativos) = DatosAlternativosBLL.CargarListadoPorIdSab(Ticket.IdUser).OrderBy(Function(f) f.Descripcion).ToList()

            If (datosAlternativos IsNot Nothing AndAlso datosAlternativos.Count() > 0) Then
                ' Metemos la opción de "Nueva solicitud"
                datosAlternativos.Insert(0, New ELL.DatosAlternativos With {.Id = Integer.MinValue, .Descripcion = Utils.Traducir("Nueva solicitud")})

                ViewData("ContainerFluid") = True
                ViewData("DatosAlternativos") = datosAlternativos
                Return View()
            End If

            Return RedirectToAction("Agregar", "Solicitud")
        End Function

    End Class
End Namespace