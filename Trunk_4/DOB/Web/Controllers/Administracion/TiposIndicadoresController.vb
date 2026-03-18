Imports System.Web.Mvc

Namespace Controllers
    Public Class TiposIndicadoresController
        Inherits AdministracionController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarTiposIndicadores()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <param name="descripcion"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Añadir(ByVal nombre As String, ByVal descripcion As String) As ActionResult
            If (BLL.TiposIndicadoresBLL.ExisteTipoIndicador(nombre)) Then
                MensajeAlerta(Utils.Traducir("El tipo indicador ya existe"))
                Return Index()
            End If

            Try
                BLL.TiposIndicadoresBLL.AgregarTipoIndicador(nombre, descripcion)
                MensajeInfo(Utils.Traducir("Tipo indicador guardado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar tipo indicador"))
                log.Error("Error al guardar tipo indicador", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarTiposIndicadores()
            ViewData("TiposIndicadores") = BLL.TiposIndicadoresBLL.CargarListado.OrderBy(Function(f) f.Nombre).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal id As Integer) As ActionResult
            Try
                If (BLL.ObjetivosBLL.ExisteTipoIndicador(id)) Then
                    MensajeAlerta(Utils.Traducir("El tipo indicador está asignado a algún objetivo"))
                    Return Index()
                End If

                BLL.TiposIndicadoresBLL.Eliminar(id)
                MensajeInfo(Utils.Traducir("Tipo indicador eliminado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar tipo indicador"))
                log.Error("Error al eliminar tipo indicador", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

#End Region

    End Class
End Namespace