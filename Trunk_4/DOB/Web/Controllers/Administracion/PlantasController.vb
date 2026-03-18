Imports System.Web.Mvc

Namespace Controllers
    Public Class PlantasController
        Inherits AdministracionController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarPlantas()
            CargarPlantasPadre()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <param name="PlantasPadre"></param>
        ''' <param name="rbHeredaRetos"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Añadir(ByVal nombre As String, ByVal PlantasPadre As Integer, ByVal rbHeredaRetos As Integer) As ActionResult
            Dim planta As New ELL.Planta With {.Planta = nombre, .IdPlantaPadre = PlantasPadre, .HeredaRetos = rbHeredaRetos}

            Try
                BLL.PlantasBLL.AgregarPlanta(planta)
                MensajeInfo(Utils.Traducir("Planta guardada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar planta"))
                log.Error("Error al guardar planta", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlantas()
            ViewData("Plantas") = BLL.PlantasBLL.CargarListado.OrderBy(Function(f) f.Planta).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlantasPadre()
            ' Vamos a cargar las empresas no obsoletas
            ' 15/05/2019 - Antes aqui se estaba cargando las plantas padre de GESTIONIKS.EMPRESAS. No tiene sentido porque no podríamos usar plantas nuevas como
            ' plantas padre
            Dim plantas As List(Of ELL.Planta) = BLL.PlantasBLL.CargarListado()
            Dim plantasLI As List(Of Mvc.SelectListItem) = plantas.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Planta, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()
            plantasLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Seleccione uno")), .Value = CStr(Integer.MinValue)})

            ViewData("PlantasPadre") = plantasLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal id As Integer) As ActionResult
            Try
                If (BLL.RetosBLL.ExistePlanta(id)) Then
                    MensajeAlerta(Utils.Traducir("La planta está asignada a algún reto"))
                    Return Index()
                End If

                If (BLL.ProcesosBLL.ExistePlanta(id)) Then
                    MensajeAlerta(Utils.Traducir("La planta está asignada a algún proceso"))
                    Return Index()
                End If

                If (BLL.UsuariosRolBLL.ExistePlanta(id)) Then
                    MensajeAlerta(Utils.Traducir("La planta está asignada a algún usuario"))
                    Return Index()
                End If

                BLL.PlantasBLL.Eliminar(id)
                MensajeInfo(Utils.Traducir("Planta eliminada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar Planta"))
                log.Error("Error al eliminar Planta", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="baja"></param> 
        ''' <returns></returns>
        Function DarBajaAlta(ByVal id As Integer, ByVal baja As Boolean) As ActionResult
            Try
                Dim planta As New ELL.Planta With {.Id = id, .FechaBaja = DateTime.Now, .IdUsuarioBaja = Ticket.IdUser}

                BLL.PlantasBLL.DarBajaAlta(planta, baja)
                If (baja) Then
                    MensajeInfo(Utils.Traducir("Planta dada de baja correctamente"))
                Else
                    MensajeInfo(Utils.Traducir("Planta dada de alta correctamente"))
                End If
            Catch ex As Exception
                If (baja) Then
                    MensajeError(Utils.Traducir("Error al dar de baja Planta"))
                Else
                    MensajeError(Utils.Traducir("Error al dar de alta Planta"))
                End If
                log.Error("Error al dar de baja/alta Planta", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

#End Region

    End Class
End Namespace