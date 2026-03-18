Imports System.Web.Mvc

Namespace Controllers
    Public Class PlantasDespliegueController
        Inherits AdministracionController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarPlantasDespliegue()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaPadre"></param>
        ''' <returns></returns>
        Function Editar(ByVal idPlantaPadre As Integer) As ActionResult
            CargarComboPlantas(sinPlantasHijas:=False, idPlantaSeleccionada:=idPlantaPadre)
            CargarPlantas()
            CargarPlantasHijas(idPlantaPadre)

            ViewData("IdPlantaPadre") = idPlantaPadre

            Return View("Editar")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Agregar() As ActionResult
            CargarComboPlantas()
            CargarPlantas()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaPadre"></param>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Function ListarHijas(ByVal idPlantaPadre As Integer, ByVal idObjetivo As Integer) As ActionResult
            CargarPlantasHijas(idPlantaPadre, idObjetivo)

            Return View("ListarHijas")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Plantas"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Agregar(ByVal Plantas As Integer) As ActionResult
            Dim plantasHijas As New List(Of Integer)
            Dim split As String()

            For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("chkBox-"))
                If (Request.Params(key).Contains("true")) Then
                    split = key.Split("-")
                    plantasHijas.Add(split(1))
                End If
            Next

            If (plantasHijas.Count = 0) Then
                MensajeAlerta(Utils.Traducir("Debe seleccionar alguna planta hija"))
                Return Agregar()
            End If

            Try
                BLL.PlantasDespliegueBLL.Guardar(Plantas, plantasHijas)
                MensajeInfo(Utils.Traducir("Plantas desplegadas guardadas correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar plantas desplegadas"))
                log.Error("Error al guardar plantas desplegadas", ex)
            End Try

            Return RedirectToAction("Editar", New With {.idPlantaPadre = Plantas})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdPlantaPadre"></param>
        ''' <returns></returns>
        <HttpPost>
        Function EditarPlantas(ByVal hIdPlantaPadre As String) As ActionResult
            Dim plantasHijas As New List(Of Integer)
            Dim split As String()

            For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("chkBox-"))
                If (Request.Params(key).Contains("true")) Then
                    split = key.Split("-")
                    plantasHijas.Add(split(1))
                End If
            Next

            If (plantasHijas.Count = 0) Then
                MensajeAlerta(Utils.Traducir("Debe seleccionar alguna planta hija"))
                Return Editar(hIdPlantaPadre)
            End If

            Try
                BLL.PlantasDespliegueBLL.Guardar(hIdPlantaPadre, plantasHijas)
                MensajeInfo(Utils.Traducir("Plantas desplegadas guardadas correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar plantas desplegadas"))
                log.Error("Error al guardar plantas desplegadas", ex)
            End Try

            Return RedirectToAction("Editar", New With {.idPlantaPadre = hIdPlantaPadre})
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlantasDespliegue()
            ViewData("PlantasDespliegue") = BLL.PlantasDespliegueBLL.CargarListado()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sinPlantasHijas"></param>
        ''' <param name="idPlantaSeleccionada"></param>
        Private Sub CargarComboPlantas(Optional ByVal sinPlantasHijas As Boolean = True, Optional idPlantaSeleccionada As Integer? = Nothing)
            ' Cogemos sólo las plantas de alta
            Dim plantas As List(Of ELL.Planta) = BLL.PlantasBLL.CargarListado().Where(Function(F) F.FechaBaja = DateTime.MinValue).ToList()

            If (sinPlantasHijas) Then
                ' Cargarmos las plantas que tiene hijos
                Dim idsPlantaConHijos As List(Of Integer) = BLL.PlantasDespliegueBLL.CargarListado.Select(Function(f) f.IdPlantaPadre).Distinct().ToList()

                ' A las plantas quitamos aquellas plantas que tiene hijos
                For Each idPlanta In idsPlantaConHijos
                    If (plantas.Exists(Function(f) f.Id = idPlanta)) Then
                        plantas.Remove(plantas.FirstOrDefault(Function(f) f.Id = idPlanta))
                    End If
                Next
            End If

            Dim plantasLI As List(Of Mvc.SelectListItem) = plantas.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Planta, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            If (idPlantaSeleccionada IsNot Nothing) Then
                If (plantasLI.Exists(Function(f) f.Value = idPlantaSeleccionada)) Then
                    plantasLI.FirstOrDefault(Function(f) f.Value = idPlantaSeleccionada).Selected = True
                End If
            End If

            ViewData("Plantas") = plantasLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlantas()
            ViewData("PlantasSeleccionar") = BLL.PlantasBLL.CargarListado()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaPadre"></param>
        ''' <param name="idObjetivo"></param>
        Private Sub CargarPlantasHijas(ByVal idPlantaPadre As Integer, Optional idObjetivo As Integer? = Nothing)
            Dim plantasHijas As List(Of ELL.PlantasDespliegue) = BLL.PlantasDespliegueBLL.CargarListadoPorPlantaPadre(idPlantaPadre)

            If (idObjetivo IsNot Nothing) Then
                ' Tenemos que quitar aquellas planta donde ya este desplegado el objetivo
                Dim listaObjetivosPadre As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(idObjetivo)

                For Each idPlanta In listaObjetivosPadre.Select(Function(f) f.IdPlanta).Distinct()
                    If (plantasHijas.Exists(Function(f) f.IdPlantaHija = idPlanta)) Then
                        plantasHijas.Remove(plantasHijas.FirstOrDefault(Function(f) f.IdPlantaHija = idPlanta))
                    End If
                Next
            End If

            ViewData("PlantasHijas") = plantasHijas
        End Sub

#End Region

    End Class
End Namespace