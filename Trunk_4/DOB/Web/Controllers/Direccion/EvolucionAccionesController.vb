Imports System.Globalization
Imports System.Web.Mvc

Namespace Controllers.Direccion
    Public Class EvolucionAccionesController
        Inherits BaseController

        '#Region "Propiedades"

        '        ''' <summary>
        '        ''' 
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Protected Overrides ReadOnly Property RolesAcceso As List(Of ELL.Rol.RolUsuario)
        '            Get
        '                Dim roles As New List(Of ELL.Rol.RolUsuario)
        '                roles.Add(ELL.Rol.RolUsuario.Director)
        '                roles.Add(ELL.Rol.RolUsuario.Gerente)
        '                Return roles
        '            End Get
        '        End Property

        '#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        ''' <returns></returns>
        Function Editar(ByVal idAccion As Integer) As ActionResult
            CargarAccion(idAccion)
            CargarEvolucionAccion(idAccion)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerEvoluciones(ByVal idAccion As Integer) As JsonResult
            Dim evoluciones As List(Of ELL.EvolucionAccion) = BLL.EvolucionAccionesBLL.CargarListado(idAccion)

            Return Json(evoluciones, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        Private Sub CargarAccion(ByVal idAccion As Integer)
            ViewData("Accion") = BLL.AccionesBLL.ObtenerAccion(idAccion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        Private Sub CargarEvolucionAccion(ByVal idAccion As Integer)
            ViewData("EvolucionesAccion") = BLL.EvolucionAccionesBLL.CargarListado(idAccion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idAccion"></param>
        ''' <param name="evolucionesAccion"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal idAccion As Integer, ByVal evolucionesAccion As List(Of ELL.EvolucionAccion)) As ActionResult
            Dim accion As ELL.Accion = BLL.AccionesBLL.ObtenerAccion(idAccion)

            Dim porcentajeDec As Decimal = Decimal.MinValue
            Dim esNegativo As Boolean = False

            For Each evolucion In evolucionesAccion
                If (Not String.IsNullOrEmpty(evolucion.Porcentaje)) Then
                    evolucion.Porcentaje = evolucion.Porcentaje.Trim()
                    esNegativo = evolucion.Porcentaje.StartsWith("-")
                    If (esNegativo) Then
                        evolucion.Porcentaje = evolucion.Porcentaje.Replace("-", String.Empty)
                    End If
                    Decimal.TryParse(evolucion.Porcentaje, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), porcentajeDec)

                    If (Not String.IsNullOrEmpty(evolucion.Porcentaje) AndAlso evolucion.Porcentaje <> "0" AndAlso porcentajeDec = Decimal.Zero) Then
                        Dim identificadorPeriodicidad As String = String.Empty
                        Select Case accion.Periodicidad
                            Case ELL.Objetivo.TipoPeriodicidad.Mensual
                                identificadorPeriodicidad = Utils.Traducir("Mes")
                            Case ELL.Objetivo.TipoPeriodicidad.Trimestral
                                identificadorPeriodicidad = Utils.Traducir("Trimestre")
                            Case ELL.Objetivo.TipoPeriodicidad.Cuatrimentral
                                identificadorPeriodicidad = Utils.Traducir("Cuatrimestre")
                            Case ELL.Objetivo.TipoPeriodicidad.Semestral
                                identificadorPeriodicidad = Utils.Traducir("Semestre")
                        End Select

                        MensajeAlerta(String.Format("{0} {1}. {2}", identificadorPeriodicidad, evolucionesAccion.IndexOf(evolucion) + 1, Utils.Traducir("Formato incorrecto")))
                        Return Editar(idAccion)
                    End If

                    If (esNegativo) Then
                        evolucion.Porcentaje = evolucion.Porcentaje.Insert(0, "-")
                    End If
                Else
                    evolucion.Porcentaje = Nothing
                End If
            Next

            Try
                BLL.EvolucionAccionesBLL.Guardar(evolucionesAccion)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                'Return RedirectToAction("Editar", New With {.idAccion = idAccion})
                Return RedirectToAction("MisObjetivos", "Objetivos", New With {.idObjetivo = accion.IdObjetivo})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(idAccion)
            End Try

        End Function

    End Class
End Namespace