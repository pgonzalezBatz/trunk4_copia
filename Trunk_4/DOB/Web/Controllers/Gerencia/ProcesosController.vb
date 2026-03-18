Imports System.Web.Mvc

Namespace Controllers
    Public Class ProcesosController
        Inherits GerenciaController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarProcesos()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <HttpPost>
        Function Index(ByVal nombre As String) As ActionResult
            CargarProcesos(nombre)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nombre"></param> 
        Private Sub CargarProcesos(Optional ByVal nombre As String = "")
            ViewData("Procesos") = BLL.ProcesosBLL.CargarListado(RolActual.IdPlanta, nombre:=nombre, procesoDeBaja:=Nothing).OrderBy(Function(f) f.Orden).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal idProceso As Integer) As ActionResult
            Try
                If (BLL.ObjetivosBLL.ExisteProceso(idProceso)) Then
                    MensajeAlerta(Utils.Traducir("El proceso está asignado a algún objetivo"))
                    Return Index()
                End If

                BLL.ProcesosBLL.Eliminar(idProceso)
                MensajeInfo(Utils.Traducir("Proceso eliminado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar proceso"))
                log.Error("Error al eliminar proceso", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlanta()
            Dim planta As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(RolActual.IdPlanta)

            ViewData("Planta") = planta
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Agregar() As ActionResult
            CargarPlanta()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Agregar(ByVal codigo As String, ByVal nombre As String) As ActionResult
            If (BLL.ProcesosBLL.ExisteCodigo(codigo, RolActual.IdPlanta)) Then
                MensajeAlerta("Ya existe ese código")
                Return Agregar()
            End If

            Dim proceso As New ELL.Proceso With {.IdPlanta = RolActual.IdPlanta, .Codigo = codigo, .Nombre = nombre, .IdUsuarioAlta = Ticket.IdUser}

            Try
                BLL.ProcesosBLL.Guardar(proceso)

                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))

                Return RedirectToAction("Index")
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Agregar()
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        Function Editar(ByVal idProceso As Integer) As ActionResult
            Dim proceso As ELL.Proceso = BLL.ProcesosBLL.ObtenerProceso(idProceso)
            ViewData("Proceso") = proceso

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProceso"></param> 
        ''' <param name="codigo"></param>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal idProceso As Integer, ByVal codigo As String, ByVal nombre As String) As ActionResult
            If (BLL.ProcesosBLL.ExisteCodigo(codigo, RolActual.IdPlanta, idProceso)) Then
                MensajeAlerta("Ya existe ese código en otro proceso")
                Return Editar(idProceso)
            End If

            Dim proceso As New ELL.Proceso With {.Id = idProceso, .Codigo = codigo, .Nombre = nombre}

            Try
                BLL.ProcesosBLL.Guardar(proceso)

                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))

                Return RedirectToAction("Index")
            Catch ex As Exception
                MensajeInfo(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(idProceso)
            End Try
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CambiarOrdenProceso(ByVal idProceso As Integer, ByVal idProcesoCambio As Integer) As ActionResult
            BLL.ProcesosBLL.CambiarOrden(idProceso, idProcesoCambio)

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        Function DarDeBaja(ByVal idProceso As Integer) As ActionResult
            Try
                BLL.ProcesosBLL.DarDeBaja(idProceso, Ticket.IdUser)
                MensajeInfo(Utils.Traducir("Proceso dado de baja correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al dar de baja el proceso"))
                log.Error("Error al dar de baja el proceso", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        Function DarDeAlta(ByVal idProceso As Integer) As ActionResult
            Try
                BLL.ProcesosBLL.DarDeAlta(idProceso)
                MensajeInfo(Utils.Traducir("Proceso dado de alta correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al dar de alta el proceso"))
                log.Error("Error al dar de alata el proceso", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

    End Class
End Namespace