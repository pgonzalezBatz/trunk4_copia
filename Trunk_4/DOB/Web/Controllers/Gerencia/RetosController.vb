Imports System.Web.Mvc

Namespace Controllers
    Public Class RetosController
        Inherits GerenciaController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarRetos()

            Return View("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <HttpPost>
        Function Index(ByVal nombre As String, ByVal descripcion As String) As ActionResult
            CargarRetos(nombre, descripcion)

            Return View()
        End Function

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
        ''' <param name="titulo"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="fuDocumento"></param>
        ''' <returns></returns>
        <ValidateInput(False)>
        <HttpPost>
        Function Agregar(ByVal codigo As String, ByVal titulo As String, ByVal descripcion As String, ByVal fuDocumento As HttpPostedFileBase) As ActionResult
            'If (String.IsNullOrEmpty(descripcion)) Then
            '    Return Agregar()
            'End If

            If (BLL.RetosBLL.ExisteCodigo(codigo, RolActual.IdPlanta)) Then
                MensajeAlerta("Ya existe ese código")
                Return Agregar()
            End If

            ' Recogemos los datos del usuario
            Dim buffer() As Byte = Nothing
            Dim nombreFichero As String = Nothing
            If (fuDocumento IsNot Nothing AndAlso fuDocumento.InputStream IsNot Nothing) Then
                Using binaryReader As New IO.BinaryReader(fuDocumento.InputStream)
                    buffer = binaryReader.ReadBytes(fuDocumento.ContentLength)
                End Using
                nombreFichero = IO.Path.GetFileName(fuDocumento.FileName)
            End If

            Dim reto As New ELL.Reto With {.IdPlanta = RolActual.IdPlanta, .Codigo = codigo, .Titulo = titulo, .Descripcion = descripcion, .NombreFichero = nombreFichero, .IdUsuarioAlta = Ticket.IdUser, .IdTipoDocumento = ELL.TipoDocumento.Tipo.Reto}

            Try
                BLL.RetosBLL.Guardar(reto, buffer)

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
        Private Sub CargarPlanta()
            Dim planta As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(RolActual.IdPlanta)

            ViewData("Planta") = planta
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <param name="descripcion"></param>
        Private Sub CargarRetos(Optional ByVal nombre As String = "", Optional ByVal descripcion As String = "")
            ViewData("Retos") = BLL.RetosBLL.CargarListado(idPlanta:=RolActual.IdPlanta, nombre:=nombre, descripcion:=descripcion, retoDeBaja:=Nothing).OrderBy(Function(f) f.Codigo).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal idReto As Integer) As ActionResult
            Try
                If (BLL.ObjetivosBLL.ExisteReto(idReto)) Then
                    MensajeAlerta(Utils.Traducir("El reto está asignado a algún objetivo"))
                    Return Index()
                End If

                BLL.RetosBLL.Eliminar(idReto)
                MensajeInfo(Utils.Traducir("Reto eliminado correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar reto"))
                log.Error("Error al eliminar reto", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        Function DarDeBaja(ByVal idReto As Integer) As ActionResult
            Try
                BLL.RetosBLL.DarDeBaja(idReto, Ticket.IdUser)
                MensajeInfo(Utils.Traducir("Reto dado de baja correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al dar de baja el reto"))
                log.Error("Error al dar de baja el reto", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        Function DarDeAlta(ByVal idReto As Integer) As ActionResult
            Try
                BLL.RetosBLL.DarDeAlta(idReto)
                MensajeInfo(Utils.Traducir("Reto dado de alta correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al dar de alta el reto"))
                log.Error("Error al dar de alata el reto", ex)
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        Function Editar(ByVal idReto As Integer) As ActionResult
            ViewData("Reto") = BLL.RetosBLL.ObtenerReto(idReto)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idReto"></param> 
        ''' <param name="idDocumento"></param> 
        ''' <param name="codigo"></param>
        ''' <param name="titulo"></param>
        ''' <param name="descripcion"></param>
        ''' <param name="fuDocumento"></param>
        ''' <returns></returns>
        <ValidateInput(False)>
        <HttpPost>
        Function Editar(ByVal idReto As String, ByVal idDocumento As String, ByVal codigo As String, ByVal titulo As String, ByVal descripcion As String, ByVal fuDocumento As HttpPostedFileBase) As ActionResult
            If (BLL.RetosBLL.ExisteCodigo(codigo, RolActual.IdPlanta, idReto)) Then
                MensajeAlerta("Ya existe ese código en otro reto")
                Return Editar(idReto)
            End If

            ' Recogemos los datos del usuario
            Dim buffer() As Byte = Nothing
            Dim nombreFichero As String = Nothing
            If (fuDocumento IsNot Nothing AndAlso fuDocumento.InputStream IsNot Nothing) Then
                Using binaryReader As New IO.BinaryReader(fuDocumento.InputStream)
                    buffer = binaryReader.ReadBytes(fuDocumento.ContentLength)
                End Using
                nombreFichero = IO.Path.GetFileName(fuDocumento.FileName)
            End If

            Dim reto As ELL.Reto = BLL.RetosBLL.ObtenerReto(idReto)
            reto.Id = idReto
            reto.IdDocumento = idDocumento
            reto.Codigo = codigo
            reto.Titulo = titulo
            reto.Descripcion = descripcion
            reto.NombreFichero = nombreFichero
            reto.IdTipoDocumento = ELL.TipoDocumento.Tipo.Reto

            Try
                BLL.RetosBLL.Guardar(reto, buffer)

                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))

                Return RedirectToAction("Index")
            Catch ex As Exception
                MensajeInfo(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(idReto)
            End Try
        End Function

    End Class
End Namespace