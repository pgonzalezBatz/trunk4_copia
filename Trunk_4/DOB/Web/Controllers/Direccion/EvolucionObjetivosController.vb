Imports System.Globalization
Imports System.Web.Mvc

Namespace Controllers.Direccion
    Public Class EvolucionObjetivosController
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
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Function Editar(ByVal idObjetivo As Integer) As ActionResult
            CargarObjetivo(idObjetivo)
            CargarEvolucionObjetivos(idObjetivo)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerEvoluciones(ByVal idObjetivo As Integer) As JsonResult
            Dim evoluciones As List(Of ELL.EvolucionObjetivo) = BLL.EvolucionObjetivosBLL.CargarListado(idObjetivo)

            Return Json(evoluciones, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        Private Sub CargarObjetivo(ByVal idObjetivo As Integer)
            ViewData("Objetivo") = BLL.ObjetivosBLL.ObtenerObjetivo(idObjetivo)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        Private Sub CargarEvolucionObjetivos(ByVal idObjetivo As Integer)
            ViewData("EvolucionesObjetivo") = BLL.EvolucionObjetivosBLL.CargarListado(idObjetivo)
        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPadre"></param>
        ''' <param name="periodicidad"></param>
        Function ListarEvolucionHijos(ByVal idPadre As Integer, ByVal periodicidad As Integer) As ActionResult
            ' Primero vamos a obtener los objetivos hijos
            Dim listaObjetivos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(idPadre)

            ' Ahora vamos a obtener la evolución para dichos objetivos hijos y esa periodicidad
            Dim evolucionObjetivos As New List(Of ELL.EvolucionObjetivo)
            Dim evolucionObjetivo As ELL.EvolucionObjetivo
            For Each objetivo In listaObjetivos
                evolucionObjetivo = BLL.EvolucionObjetivosBLL.Obtener(objetivo.Id, periodicidad)

                If (evolucionObjetivo IsNot Nothing) Then
                    evolucionObjetivos.Add(BLL.EvolucionObjetivosBLL.Obtener(objetivo.Id, periodicidad))
                Else
                    evolucionObjetivos.Add(New ELL.EvolucionObjetivo With {.Planta = BLL.PlantasBLL.ObtenerPlanta(objetivo.IdPlanta).Planta, .ValorActual = Integer.MinValue})
                End If
            Next

            ViewData("EvolucionesObjetivo") = evolucionObjetivos

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="evolucionesObjetivo"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Editar(ByVal idObjetivo As Integer, ByVal evolucionesObjetivo As List(Of ELL.EvolucionObjetivo)) As ActionResult
            Dim objetivo As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(idObjetivo)

            Dim valorActualDec As Decimal = Decimal.MinValue
            Dim esNegativo As Boolean = False

            Dim currentCulture = Globalization.CultureInfo.GetCultureInfo(Session("Ticket").Culture)
            Dim decimalCharacter = currentCulture.NumberFormat.NumberDecimalSeparator

            For Each evolucion In evolucionesObjetivo
                If (Not String.IsNullOrEmpty(evolucion.ValorActual)) Then
                    If (decimalCharacter = ",") Then
                        evolucion.ValorActual = evolucion.ValorActual.Trim().Replace(".", ",")
                    ElseIf (decimalCharacter = ".") Then
                        evolucion.ValorActual = evolucion.ValorActual.Trim().Replace(",", ".")
                    End If

                    esNegativo = evolucion.ValorActual.StartsWith("-")
                    If (esNegativo) Then
                        evolucion.ValorActual = evolucion.ValorActual.Replace("-", String.Empty)
                    End If

                    Decimal.TryParse(evolucion.ValorActual, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture(Ticket.Culture), valorActualDec)

                    If (Not String.IsNullOrEmpty(evolucion.ValorActual) AndAlso evolucion.ValorActual <> "0" AndAlso valorActualDec = Decimal.Zero) Then
                        Dim identificadorPeriodicidad As String = String.Empty
                        Select Case objetivo.Periodicidad
                            Case ELL.Objetivo.TipoPeriodicidad.Mensual
                                identificadorPeriodicidad = Utils.Traducir("Mes")
                            Case ELL.Objetivo.TipoPeriodicidad.Trimestral
                                identificadorPeriodicidad = Utils.Traducir("Trimestre")
                            Case ELL.Objetivo.TipoPeriodicidad.Cuatrimentral
                                identificadorPeriodicidad = Utils.Traducir("Cuatrimestre")
                            Case ELL.Objetivo.TipoPeriodicidad.Semestral
                                identificadorPeriodicidad = Utils.Traducir("Semestre")
                        End Select

                        MensajeAlerta(String.Format("{0} {1}. {2}", identificadorPeriodicidad, evolucionesObjetivo.IndexOf(evolucion) + 1, Utils.Traducir("Formato incorrecto")))
                        Return Editar(idObjetivo)
                    End If

                    If (esNegativo) Then
                        evolucion.ValorActual = evolucion.ValorActual.Insert(0, "-")
                    End If
                Else
                    evolucion.ValorActual = Nothing
                End If
            Next

            Try
                BLL.EvolucionObjetivosBLL.Guardar(evolucionesObjetivo)
                GuardarEvolucionesPadre(objetivo.IdObjetivoPadre, evolucionesObjetivo)

                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                'Return RedirectToAction("Editar", New With {.idObjetivo = idObjetivo})
                ' Quieren que al editar la evolución de un objetivo vuelva a mis objetivos
                Return RedirectToAction("MisObjetivos", "Objetivos")
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(idObjetivo)
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivoPadre"></param>
        ''' <param name="evolucionesObjetivo"></param>
        Private Sub GuardarEvolucionesPadre(ByVal idObjetivoPadre As Integer, ByVal evolucionesObjetivo As List(Of ELL.EvolucionObjetivo))
            ' Cargamos el objetivo padre para ver si a su vez tiene padre
            Dim objetivo As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(idObjetivoPadre)

            Dim evolucionesObjetivoPadre As List(Of ELL.EvolucionObjetivo)
            ' Quieren que la evolución se guarde el el objetivo padre si es que el objetivo padre solo se ha desplegado en una planta
            'Vamos a buscar en que objetivos aparece el objetivo padre. Si aparece solo en uno debemos grabar los valores de evolución en el padre
            If (idObjetivoPadre <> Integer.MinValue AndAlso BLL.ObjetivosBLL.CargarListadoPorPadre(idObjetivoPadre).Count = 1) Then
                evolucionesObjetivoPadre = BLL.EvolucionObjetivosBLL.CargarListado(idObjetivoPadre)
                If (evolucionesObjetivoPadre Is Nothing OrElse evolucionesObjetivoPadre.Count = 0) Then
                    evolucionesObjetivoPadre = evolucionesObjetivo
                    evolucionesObjetivoPadre.ForEach(Sub(s) s.IdObjetivo = idObjetivoPadre)
                    evolucionesObjetivoPadre.ForEach(Sub(s) s.Id = 0)
                Else
                    For Each evolucion In evolucionesObjetivo
                        If (evolucionesObjetivoPadre.Exists(Function(f) f.IdPeriodicidad = evolucion.IdPeriodicidad)) Then
                            evolucionesObjetivoPadre.First(Function(f) f.IdPeriodicidad = evolucion.IdPeriodicidad).ValorActual = evolucion.ValorActual
                        Else
                            Dim nuevaEvolucion As New ELL.EvolucionObjetivo
                            nuevaEvolucion.Id = 0
                            nuevaEvolucion.IdObjetivo = idObjetivoPadre
                            nuevaEvolucion.ValorActual = evolucion.ValorActual
                            nuevaEvolucion.IdPeriodicidad = evolucion.IdPeriodicidad
                            nuevaEvolucion.IdUsuarioAlta = evolucion.IdUsuarioAlta

                            evolucionesObjetivoPadre.Add(nuevaEvolucion)
                        End If
                    Next
                End If

                BLL.EvolucionObjetivosBLL.Guardar(evolucionesObjetivoPadre)
                EnviarMailAvisoObjetivoPadre(objetivo)

                GuardarEvolucionesPadre(objetivo.IdObjetivoPadre, evolucionesObjetivoPadre)
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objetivo"></param>
        Private Sub EnviarMailAvisoObjetivoPadre(ByVal objetivo As ELL.Objetivo)
            Try
                Dim usuarioBLL As New SabLib.BLL.UsuariosComponent
                Dim user = usuarioBLL.GetUsuario(New SabLib.ELL.Usuario() With {.Id = objetivo.IdResponsable})
                If (user IsNot Nothing) Then
                    Dim subject As String = Utils.Traducir("Actualización del indicador de un objetivo desplegado")
                    Dim body As String = String.Format(Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\AvisoObjetivoPadre.html")), Ticket.NombreCompleto,
                                                   RolActual.Planta, objetivo.Descripcion, objetivo.Id)
                    Dim mailFrom As String = """DOB"" <" & ConfigurationManager.AppSettings("mailFrom") & ">"
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                    Dim userExchange As String = ConfigurationManager.AppSettings("UserExchange")
                    Dim passExchange As String = ConfigurationManager.AppSettings("PassExchange")

#If Not DEBUG Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, user.Email, subject, body, serverEmail, Nothing, Nothing, userExchange, passExchange)
#End If
                End If
            Catch ex As Exception
                log.Error("Se ha producido un error al EnviarMailAvisoObjetivoPadre", ex)
            End Try
        End Sub

    End Class
End Namespace