Imports System.Web.Mvc

Namespace Controllers
    Public Class PlantasNegociosController
        Inherits AdministracionController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarPlantasNegocios()
            CargarPlantas()
            CargarNegocios()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal id As Integer) As ActionResult
            Try
                If (BLL.RetosBLL.ExistePlantaNegocio(id)) Then
                    MensajeAlerta(Utils.Traducir("La planta/negocio está asignada a algún reto"))
                    Return Index()
                End If

                If (BLL.ProcesosBLL.ExistePlantaNegocio(id)) Then
                    MensajeAlerta(Utils.Traducir("La planta/negocio está asignada a algún proceso"))
                    Return Index()
                End If

                If (BLL.UsuariosRolBLL.ExistePlantaNegocio(id)) Then
                    MensajeAlerta(Utils.Traducir("La planta/negocio esta asignada a algún usuario"))
                    Return Index()
                End If

                BLL.PlantasNegociosBLL.Eliminar(id)
                MensajeInfo(Utils.Traducir("Planta/negocio eliminada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar Planta/negocio"))
                log.Error("Error al eliminar Planta/negocio", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Plantas"></param>
        ''' <param name="Negocios"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Añadir(ByVal Plantas As String, ByVal Negocios As String) As ActionResult
            Dim idPlanta As Integer = CInt(Plantas)
            Dim idNegocio As Integer = CInt(Negocios)

            If (BLL.PlantasNegociosBLL.Existe(idNegocio, idPlanta)) Then
                MensajeAlerta(Utils.Traducir("La planta/negocio ya existe"))
                Return Index()
            End If

            Try
                Dim plantaNegocio As New ELL.PlantaNegocio With {.IdPlanta = idPlanta, .IdNegocio = idNegocio}
                BLL.PlantasNegociosBLL.Guardar(plantaNegocio)
                MensajeInfo(Utils.Traducir("Planta/negocio guardado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar Planta/negocio"))
                log.Error("Error al guardar Planta/negocio", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlantasNegocios()
            ViewData("PlantasNegocios") = BLL.PlantasNegociosBLL.Cargarlistado().OrderBy(Function(f) f.Planta).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarPlantas()
            ' Vamos a cargar todas las plantas
            Dim plantas As List(Of ELL.Planta) = BLL.PlantasBLL.CargarListado()
            Dim plantasLI As List(Of Mvc.SelectListItem) = plantas.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Planta, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            ViewData("Plantas") = plantasLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarNegocios()
            ' Vamos a cargar todos los negocios
            Dim negocios As List(Of ELL.Negocio) = BLL.NegociosBLL.CargarListado()
            Dim negociosLI As List(Of Mvc.SelectListItem) = negocios.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Negocio, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            ViewData("Negocios") = negociosLI
        End Sub

#End Region

    End Class
End Namespace