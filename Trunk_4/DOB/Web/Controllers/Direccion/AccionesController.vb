Imports System.Globalization
Imports System.Web.Mvc

Namespace Controllers.Administracion

    Public Class AccionesController
        Inherits DireccionController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Function Agregar(ByVal idObjetivo As Integer) As ActionResult
            'Obtenemos las acciones del objetivo para ver que la suma de todos sus grados de importancia ni supere el 100%
            'Le sugeriremos al usuario el resto hasta 100%

            ViewData("GradoImportancia") = ObtenerGradoImportanciaResto(idObjetivo)
            CargarObjetivo(idObjetivo)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param> 
        ''' <returns></returns>
        Function Editar(ByVal idAccion As Integer) As ActionResult
            CargarAccion(idAccion)
            CargarDocumentos(idAccion)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal idAccion As Integer) As ActionResult
            Dim accion As ELL.Accion = BLL.AccionesBLL.ObtenerAccion(idAccion)
            Try
                BLL.AccionesBLL.Eliminar(idAccion)
                MensajeInfo(Utils.Traducir("Acción eliminada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar acción"))
                log.Error("Error al eliminar acción", ex)
            End Try

            ' He puesto esto así porque al eliminar una acción desde Mis objetivos tenemos que volver ahí no nos vale que se quede en el
            ' listado de acciones
            'Return Redirect(Request.UrlReferrer.OriginalString)
            Return RedirectToAction("MisObjetivos", "Objetivos", New With {.idObjetivo = accion.IdObjetivo})
            'Return RedirectToAction("MisAcciones")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Function MisAccionesPorObjetivo(ByVal idObjetivo As Integer) As ActionResult
            CargarAccionesPorObjetivo(idObjetivo)
            ViewData("SinLayout") = 1

            Return View("MisAcciones")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function MisAcciones() As ActionResult
            ViewData("SoloAcciones") = 1
            CargarObjetivos(Ticket.IdUser)
            CargarAcciones(Ticket.IdUser)

            Return View("MisAcciones")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Objetivos"></param>
        ''' <param name="plazoDesde"></param>
        ''' <param name="plazoHasta"></param>
        ''' <returns></returns>
        <HttpPost>
        Function MisAcciones(ByVal Objetivos As Integer, ByVal plazoDesde As DateTime?, ByVal plazoHasta As DateTime?) As ActionResult
            CargarObjetivos(Ticket.IdUser)
            CargarAcciones(Ticket.IdUser, Objetivos, plazoDesde, plazoHasta)

            Return View("MisAcciones")
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        ''' <param name="descripcion"></param>
        ''' <returns></returns>
        Function EvolucionAccion(ByVal idAccion As Integer, Optional ByVal descripcion As String = Nothing) As ActionResult
            ViewData("IdAccion") = idAccion
            ViewData("Descripcion") = descripcion
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Listar(ByVal idObjetivo As Integer) As ActionResult
            CargarAccionesPorObjetivo(idObjetivo)

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        Private Function ObtenerGradoImportanciaResto(ByVal idObjetivo As Integer) As Decimal
            Dim listaAcciones As List(Of ELL.Accion) = BLL.AccionesBLL.CargarListadoPorObjetivo(idObjetivo)
            Dim gradoImportancia As Decimal = listaAcciones.Sum(Function(f) f.GradoImportancia)

            Return 100 - gradoImportancia
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="idAccion"></param> 
        Private Function ObtenerGradoImportanciaResto(ByVal idObjetivo As Integer, ByVal idAccion As Integer) As Decimal
            Dim listaAcciones As List(Of ELL.Accion) = BLL.AccionesBLL.CargarListadoPorObjetivo(idObjetivo)
            Dim gradoImportancia As Decimal = listaAcciones.Sum(Function(f) f.GradoImportancia) - listaAcciones.First(Function(f) f.Id = idAccion).GradoImportancia

            Return 100 - gradoImportancia
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idUser"></param>
        ''' <param name="idObjetivo"></param> 
        ''' <param name="plazoDesde"></param>
        ''' <param name="plazoHasta"></param>
        Private Sub CargarAcciones(ByVal idUser As Integer, Optional idObjetivo As Integer? = Nothing, Optional plazoDesde As DateTime? = Nothing, Optional plazoHasta As DateTime? = Nothing)
            ViewData("Acciones") = BLL.AccionesBLL.CargarListado(idUser, idObjetivo:=idObjetivo, plazoDesde:=plazoDesde, plazoHasta:=plazoHasta)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        Private Sub CargarAccionesPorObjetivo(ByVal idObjetivo As Integer)
            ' Solicita Asier Ortuzar que se ordene por descripción 4 enero 2019
            ' Solicita Jokin Laspiur que se ordenen por fecha el 14 de enero de 2020
            ViewData("Acciones") = BLL.AccionesBLL.CargarListadoPorObjetivo(idObjetivo).OrderBy(Function(f) f.FechaObjetivo).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        Private Sub CargarAccion(idAccion As Integer)
            ViewData("Accion") = BLL.AccionesBLL.ObtenerAccion(idAccion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Private Sub CargarDocumentos(ByVal id As Integer)
            ViewData("Documentos") = BLL.DocumentosBLL.CargarListado(id, ELL.TipoDocumento.Tipo.Accion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idResponsable"></param>
        Private Sub CargarObjetivos(ByVal idResponsable As Integer)
            Dim objetivos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListado(RolActual.IdPlanta, idResponsable).ToList()
            Dim objetivosLI As List(Of Mvc.SelectListItem) = objetivos.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Descripcion, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            objetivosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})

            ViewData("Objetivos") = objetivosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="fechaObjetivo"></param>
        ''' <param name="gradoImportancia"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Agregar(ByVal idObjetivo As Integer, ByVal descripcion As String, ByVal fechaObjetivo As DateTime, ByVal gradoImportancia As String) As ActionResult
            Dim gradoImportanciaDec As Decimal = Decimal.MinValue

            Decimal.TryParse(gradoImportancia, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), gradoImportanciaDec)

            If (Not String.IsNullOrEmpty(gradoImportancia) AndAlso gradoImportancia <> "0" AndAlso gradoImportanciaDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Grado importancia"), Utils.Traducir("Formato incorrecto")))
                Return Agregar(idObjetivo)
            End If

            Dim gradoImportanciaResto As Decimal = ObtenerGradoImportanciaResto(idObjetivo)

            If (gradoImportanciaDec > gradoImportanciaResto) Then
                MensajeAlerta(Utils.Traducir("El grado de importancia no puede ser superior a ") & gradoImportanciaResto)
                Return Agregar(idObjetivo)
            End If

            Dim accion As New ELL.Accion With {.IdObjetivo = idObjetivo, .Descripcion = descripcion, .FechaObjetivo = fechaObjetivo,
                                               .GradoImportancia = gradoImportanciaDec, .IdUsuarioAlta = Ticket.IdUser}

            Try
                BLL.AccionesBLL.Guardar(accion)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Return RedirectToAction("MisObjetivos", "Objetivos", New With {.idObjetivo = idObjetivo})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Agregar(idObjetivo)
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        ''' <param name="idObjetivo"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="fechaObjetivo"></param>
        ''' <param name="gradoImportancia"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal idAccion As Integer, ByVal idObjetivo As Integer, ByVal descripcion As String, ByVal fechaObjetivo As DateTime, ByVal gradoImportancia As String) As ActionResult
            Dim gradoImportanciaDec As Decimal = Decimal.MinValue

            Decimal.TryParse(gradoImportancia, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), gradoImportanciaDec)

            If (Not String.IsNullOrEmpty(gradoImportancia) AndAlso gradoImportancia <> "0" AndAlso gradoImportanciaDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Grado importancia"), Utils.Traducir("Formato incorrecto")))
                Return Editar(idAccion)
            End If

            ' Hay que comprobar que quitando la propia acción no se supere el 100 sumando el resto de las acciones
            Dim gradoImportanciaResto As Decimal = ObtenerGradoImportanciaResto(idObjetivo, idAccion)
            If (gradoImportanciaDec > gradoImportanciaResto) Then
                MensajeAlerta(Utils.Traducir("El grado de importancia no puede ser superior a ") & gradoImportanciaResto)
                Return Editar(idAccion)
            End If

            Dim accion As ELL.Accion = BLL.AccionesBLL.ObtenerAccion(idAccion)
            accion.Id = idAccion
            accion.IdObjetivo = idObjetivo
            accion.Descripcion = descripcion
            accion.FechaObjetivo = fechaObjetivo
            accion.GradoImportancia = gradoImportanciaDec

            Try
                BLL.AccionesBLL.Guardar(accion)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Return RedirectToAction("MisObjetivos", "Objetivos", New With {.idObjetivo = idObjetivo})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(idAccion)
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        Private Sub CargarObjetivo(idObjetivo As Integer)
            ViewData("Objetivo") = BLL.ObjetivosBLL.ObtenerObjetivo(idObjetivo)
        End Sub

    End Class

End Namespace