Imports System.Web.Mvc

Namespace Controllers
    Public Class RevisionesController
        Inherits BaseController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function MisRevisiones() As ActionResult
            CargarEjercicios(ejercicio:=Ejercicio)
            CargarRevisiones(ejercicio:=Ejercicio)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Ejercicios"></param>
        ''' <returns></returns>
        <HttpPost>
        Function MisRevisiones(ByVal Ejercicios As Integer) As ActionResult
            ' Guardamos el ejercicio en la cookies
            Ejercicio = Ejercicios

            CargarEjercicios(ejercicio:=Ejercicio)
            CargarRevisiones()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Agregar() As ActionResult
            CargarMisObjetivos()
            CargarRevisionesTipo()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="revision"></param>
        ''' <returns></returns>
        Function Editar(ByVal idObjetivo As Integer, ByVal revision As Integer) As ActionResult
            CargarRevision(idObjetivo, revision)
            CargarMisObjetivos(idObjetivo)
            CargarRevisionesTipo(revision)
            CargarDocumentos(idObjetivo, revision)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Objetivos"></param>
        ''' <param name="RevisionesTipo"></param>
        ''' <param name="comentario"></param>
        ''' <param name="paAñoSiguiente"></param>
        ''' <param name="paComentario"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Agregar(ByVal Objetivos As Integer, ByVal RevisionesTipo As Integer, ByVal comentario As String, ByVal paAñoSiguiente As Boolean, paComentario As String) As ActionResult
            Dim revision As New ELL.Revision With {.IdObjetivo = Objetivos, .Revision = RevisionesTipo, .Comentario = comentario, .PAAñoSiguiente = paAñoSiguiente, .PAComentario = paComentario}

            If (BLL.RevisionesBLL.ObtenerRevision(Objetivos, RevisionesTipo) IsNot Nothing) Then
                MensajeAlerta(Utils.Traducir("Ya existe esa revisión para ese objetivo"))
                Return Agregar()
            End If

            Try
                BLL.RevisionesBLL.Guardar(revision)
                MensajeInfo(Utils.Traducir("Revisión guardada correctamente"))
                Return RedirectToAction("Editar", New With {.idObjetivo = Objetivos, .revision = RevisionesTipo})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar revisión"))
                log.Error("Error al guardar revisión", ex)
                Return Agregar()
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="revisionTipo"></param>
        ''' <param name="comentario"></param>
        ''' <param name="paAñoSiguiente"></param>
        ''' <param name="paComentario"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal idObjetivo As Integer, ByVal revisionTipo As Integer, ByVal comentario As String, ByVal paAñoSiguiente As Boolean, paComentario As String) As ActionResult
            Dim revision As New ELL.Revision With {.IdObjetivo = idObjetivo, .Revision = revisionTipo, .Comentario = comentario, .PAAñoSiguiente = paAñoSiguiente, .PAComentario = paComentario}

            Try
                BLL.RevisionesBLL.Guardar(revision)
                MensajeInfo(Utils.Traducir("Revisión guardada correctamente"))
                Return RedirectToAction("Editar", New With {.idObjetivo = idObjetivo, .revision = revisionTipo})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al guardar revisión"))
                log.Error("Error al guardar revisión", ex)
                Return Editar(idObjetivo, revisionTipo)
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ejercicio"></param>
        Private Sub CargarRevisiones(Optional ByVal ejercicio As Integer? = Nothing)
            Dim revisiones As List(Of ELL.Revision) = BLL.RevisionesBLL.CargarListadoPorResponsable(Ticket.IdUser)

            If (ejercicio IsNot Nothing AndAlso ejercicio <> Integer.MinValue) Then
                revisiones = revisiones.Where(Function(f) f.AñoObjetivo = ejercicio).ToList()
            End If

            ViewData("Revisiones") = revisiones
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="revision"></param>
        Private Sub CargarRevision(ByVal idObjetivo As Integer, ByVal revision As Integer)
            ViewData("Revision") = BLL.RevisionesBLL.ObtenerRevision(idObjetivo, revision)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="revision"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal idObjetivo As Integer, ByVal revision As Integer) As ActionResult
            Try
                BLL.RevisionesBLL.Eliminar(idObjetivo, revision)
                MensajeInfo(Utils.Traducir("Revisión eliminada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar revisión"))
                log.Error("Error al eliminar revisión", ex)
            End Try

            Return RedirectToAction("MisRevisiones")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        Private Sub CargarMisObjetivos(Optional idObjetivo As Integer? = Nothing)
            Dim objetivos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListado(RolActual.IdPlanta, idResponsable:=Ticket.IdUser).OrderByDescending(Function(f) f.AñoObjetivo).ThenBy(Function(f) f.Descripcion).ToList()
            Dim objetivosLI As List(Of Mvc.SelectListItem) = objetivos.Select(Function(f) New Mvc.SelectListItem With {.Text = String.Format("{0} ({1})", f.Descripcion, f.AñoObjetivo), .Value = f.Id}).OrderBy(Function(f) f.Text).ToList()

            If (idObjetivo IsNot Nothing AndAlso objetivosLI.Exists(Function(f) f.Value = idObjetivo)) Then
                objetivosLI.First(Function(f) f.Value = idObjetivo).Selected = True
            End If

            ViewData("Objetivos") = objetivosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="revision"></param>
        Private Sub CargarRevisionesTipo(Optional revision As Integer? = Nothing)
            Dim revisionesTipoLI As New List(Of Mvc.SelectListItem)

            Dim revisionesTipo = System.Enum.GetValues(GetType(ELL.Revision.Tipo))
            For Each item In revisionesTipo
                revisionesTipoLI.Add(New Mvc.SelectListItem With {.Text = System.Enum.GetName(GetType(ELL.Revision.Tipo), item), .Value = item})
            Next

            If (revision IsNot Nothing AndAlso revisionesTipoLI.Exists(Function(f) f.Value = revision)) Then
                revisionesTipoLI.First(Function(f) f.Value = revision).Selected = True
            End If

            ViewData("RevisionesTipo") = revisionesTipoLI.OrderBy(Function(f) f.Value).ToList()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ejercicio"></param>
        Private Sub CargarEjercicios(Optional ByVal ejercicio As Integer? = Nothing)
            ' Tenemos que recuperar los distintos ejercicios para los cuales hay objetivos
            Dim ejercicios As List(Of Integer) = BLL.ObjetivosBLL.ObtenerEjercicios(RolActual.IdPlanta)

            Dim ejerciciosLI As New List(Of Mvc.SelectListItem)
            ejercicios.ForEach(Sub(s) ejerciciosLI.Add(New SelectListItem With {.Value = s, .Text = s}))

            ejerciciosLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Todos")), .Value = CStr(Integer.MinValue)})

            If (ejercicio IsNot Nothing AndAlso ejerciciosLI.Exists(Function(f) f.Value = ejercicio)) Then
                ejerciciosLI.First(Function(f) f.Value = ejercicio).Selected = True
            End If

            ViewData("Ejercicios") = ejerciciosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="revision"></param>
        Private Sub CargarDocumentos(ByVal idObjetivo As Integer, ByVal revision As Integer)
            ViewData("Documentos") = BLL.DocumentosBLL.CargarListado(idObjetivo, ELL.TipoDocumento.Tipo.Revision_cierre, revision:=revision)
        End Sub

    End Class
End Namespace