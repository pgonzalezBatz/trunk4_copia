Imports System.Web.Mvc

Namespace Controllers
    Public Class NegociosController
        Inherits AdministracionController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarNegocios()
            CargarDivisiones()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Divisiones"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Añadir(ByVal Divisiones As String) As ActionResult
            Dim idDivision As Integer = CInt(Divisiones)

            If (BLL.NegociosBLL.Existe(idDivision)) Then
                MensajeAlerta(Utils.Traducir("El negocio ya existe"))
                Return Index()
            End If

            Try
                BLL.NegociosBLL.AgregarNegocio(idDivision)
                MensajeInfo(Utils.Traducir("Negocio guardado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar negocio"))
                log.Error("Error al guardar negocio", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarNegocios()
            ViewData("Negocios") = BLL.NegociosBLL.CargarListado.OrderBy(Function(f) f.Negocio).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarDivisiones()
            ' Vamos a cargar las divisiones no obsoletas
            Dim divisiones As List(Of ELL.Division) = BLL.DivisionesBLL.CargarListado()
            Dim divisionesLI As List(Of Mvc.SelectListItem) = divisiones.Select(Function(f) New Mvc.SelectListItem With {.Text = f.Nombre, .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            ViewData("Divisiones") = divisionesLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal id As Integer) As ActionResult
            Try
                If (BLL.PlantasNegociosBLL.ExisteNegocio(id)) Then
                    MensajeAlerta(Utils.Traducir("El negocio existe en una Planta/negocio"))
                    Return Index()
                End If

                BLL.NegociosBLL.Eliminar(id)
                MensajeInfo(Utils.Traducir("Negocio eliminado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar Negocio"))
                log.Error("Error al eliminar Negocio", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

#End Region

    End Class
End Namespace