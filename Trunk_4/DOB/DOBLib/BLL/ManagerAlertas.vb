Imports System.Configuration
Imports System.IO
Imports System.Reflection

Public Class ManagerAlertas

#Region "Miembros"

    Private Shared m_Log As log4net.ILog = log4net.LogManager.GetLogger("root")

#End Region

    Public Shared Event ProcesoFinalizado()

#Region "Propiedades"

    ''' <summary>
    ''' Log
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared ReadOnly Property Log As log4net.ILog
        Get
            If (m_Log Is Nothing) Then
                m_Log = log4net.LogManager.GetLogger("root")
            End If
            Return m_Log
        End Get
    End Property

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Gestiona las alertas
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub GestionarAlertas()
        Log.Info("///////////// INICIO - GESTIONARALERTAS()")

        Try

#Region "Alertas evolución objetivo"

            ' Tenemos que comprobar si estamos en el ultimo día del mes
            Dim fechaUltimoDiaMes As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))

            ' Vemos cual es la fecha del ultimo envio de alertas
            Dim applicationName As String = Environment.GetCommandLineArgs()(0)
            Dim exePath As String = System.IO.Path.Combine(Environment.CurrentDirectory, applicationName)
            Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(exePath)
            Dim dato As String = config.AppSettings.Settings("fechaUltimoEnvioAlertas").Value
            Dim fecha As DateTime = DateTime.MinValue
            If (Not String.IsNullOrEmpty(dato)) Then
                fecha = CDate(dato)
                Log.Info("La fechaUltimoEnvioAlertas es " & dato)
            End If

            If (fechaUltimoDiaMes = DateTime.Today AndAlso fecha <> DateTime.Today) Then
                Log.Info("Estamos en el último día del mes así que enviamos las alertas")
                GestionarAlertasEvolucionObjetivo()

                ' Guardamos la ultima fecha en la que hemos realizado el envio de alertas
                config.AppSettings.Settings("fechaUltimoEnvioAlertas").Value = DateTime.Today.ToShortDateString()
                config.Save(ConfigurationSaveMode.Modified, False)
                Log.Info("Escribimos la fechaUltimoEnvioAlertas en el config: " & DateTime.Today.ToShortDateString())
            Else
                Log.Info("No estamos en el último día del mes así que no enviamos las alertas")
            End If

#End Region

#Region "Alertas revisiones"

            Dim fechaActual As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
            Dim fechaEnero As DateTime = New DateTime(DateTime.Today.Year, 1, 1)
            Dim fechaMarzo As DateTime = New DateTime(DateTime.Today.Year, 3, 1)
            Dim fechaJunio As DateTime = New DateTime(DateTime.Today.Year, 6, 1)
            Dim fechaOctubre As DateTime = New DateTime(DateTime.Today.Year, 10, 1)
            Dim revision As Integer = Integer.MinValue

            ' Tenemos que comprobar si es 1 de Marzo o 1 de Junio o 1 de Octubre o 1 de Enero
            If (fechaActual = fechaEnero OrElse fechaActual = fechaMarzo OrElse fechaActual = fechaJunio OrElse fechaActual = fechaOctubre) Then
                Log.Info("Estamos en el 1 de Enero o 1 de Marzo o 1 de Junio o 1 de Octubre")

                Select Case fechaActual
                    Case fechaEnero
                        revision = ELL.Revision.Tipo.Enero
                    Case fechaMarzo
                        revision = ELL.Revision.Tipo.Marzo
                    Case fechaJunio
                        revision = ELL.Revision.Tipo.Junio
                    Case fechaOctubre
                        revision = ELL.Revision.Tipo.Octubre
                End Select

                'GestionarAlertasRevisiones(revision)
            Else
                Log.Info("No es 1 de Enero ni 1 de Marzo ni 1 de Junio ni 1 de Octubre así que no enviamos las alertas de revisiones")
            End If

#End Region

            GC.Collect()
        Catch ex As Exception
            Log.Error("ManagerAlertas - Error en GestionarAlertas", ex)
        Finally
            RaiseEvent ProcesoFinalizado()
        End Try

        Log.Info("\\\\\\\\\\\\\ FIN - GESTIONARALERTAS()")
        Log.Info("***********************************************************")
    End Sub

    ''' <summary>
    ''' Gestiona los envios pendiente
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub GestionarAlertasEvolucionObjetivo()
        Log.Info("Inicio - GestionarAlertasEvolucionObjetivo()")

        Try
            ' Cargamos los responsables que tienen pendiente a fecha actual meter el indicador del objetivo en el mes actual
            Dim listaResponsablesAlerta As List(Of ELL.ResponsableAlerta) = BLL.ResponsablesAlertaBLL.CargarListado()
            listaResponsablesAlerta.Add(New ELL.ResponsableAlerta With {.IdResponsable = 60205, .Email = "illanos@batz.es", .Cultura = "es-ES", .Responsable = "IKER LLANOS ZUBZIARRETA"})

            ' Por cada uno de ellos les enviamos un mail
            For Each responsable In listaResponsablesAlerta
                If (String.IsNullOrEmpty(responsable.Email)) Then
                    Log.Info(String.Format("El responsable {0}({1}) no tiene email", responsable.Responsable, responsable.IdResponsable))
                Else
                    EnviarMailSinIndicadoresMesObjetivo(responsable.Email, responsable.Cultura)
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error en GestionarAlertasEvolucionObjetivo()", ex)
        End Try

        Log.Info("Fin - GestionarAlertasEvolucionObjetivo()")
    End Sub

    ''' <summary>
    ''' Gestiona las revisiones de los objetivos
    ''' </summary>
    ''' <param name="revision"></param>
    ''' <remarks></remarks>
    Private Shared Sub GestionarAlertasRevisiones(ByVal revision As Integer)
        Log.Info("Inicio - GestionarAlertasRevisiones()")

        Try
            ' Si la revisión es la de enero habría que remontarse un año atrás
            Dim ejercicio As Integer = DateTime.Today.Year
            If (revision = ELL.Revision.Tipo.Enero) Then
                ejercicio -= 1
            End If

            ' Cargamos los responsables que tienen pendiente la revisión 
            Dim listaResponsablesAlerta As List(Of ELL.ResponsableAlerta) = BLL.ResponsablesAlertaBLL.CargarListadoRevision(ejercicio)
            listaResponsablesAlerta.Add(New ELL.ResponsableAlerta With {.IdResponsable = 60205, .Email = "illanos@batz.es", .Cultura = "es-ES", .Responsable = "IKER LLANOS ZUBZIARRETA"})

            ' Por cada uno de ellos les enviamos un mail
            For Each responsable In listaResponsablesAlerta
                If (String.IsNullOrEmpty(responsable.Email)) Then
                    Log.Info(String.Format("El responsable {0}({1}) no tiene email", responsable.Responsable, responsable.IdResponsable))
                Else
                    EnviarMailRevisiones(responsable.Email, responsable.Cultura, revision)
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error en GestionarAlertasRevisiones()", ex)
        End Try

        Log.Info("Fin - GestionarAlertasRevisiones()")
    End Sub

    ''' <summary>
    ''' Envía el mail
    ''' </summary>
    ''' <param name="mailto"></param>
    ''' <param name="cultura"></param>
    ''' <remarks></remarks>
    Private Shared Sub EnviarMailSinIndicadoresMesObjetivo(ByVal mailTo As String, ByVal cultura As String)
        Dim plantilla As String = "PlantillasMail\SinIndicadoresMesObjetivo.en-GB.html"
        Dim subject As String = U.Traducir("Seguimiento de objetivos", "en-GB")

        Select Case cultura.ToLower()
            Case "es-es"
                plantilla = "PlantillasMail\SinIndicadoresMesObjetivo.es-ES.html"
                subject = U.Traducir("Seguimiento de objetivos", "es-ES")
            Case "eu-es"
                plantilla = "PlantillasMail\SinIndicadoresMesObjetivo.eu-ES.html"
                subject = U.Traducir("Seguimiento de objetivos", "eu-ES")
        End Select

        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
        Dim body As String = U.LeerFicheroTexto(Path.Combine(Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName, plantilla))
        Dim mailFrom As String = """DOB"" <" & ConfigurationManager.AppSettings("mailFrom") & ">"

        Try
            SabLib.BLL.Utils.EnviarEmail(mailFrom, mailTo, subject, body, serverEmail)
            Log.Info("Mail enviado a " & mailTo)
        Catch ex As Exception
            Log.Error("Error en EnviarMailSinIndicadoresMesObjetivo", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Envía el mail
    ''' </summary>
    ''' <param name="mailto"></param>
    ''' <param name="revision"></param>
    ''' <param name="cultura"></param>
    ''' <remarks></remarks>
    Private Shared Sub EnviarMailRevisiones(ByVal mailTo As String, ByVal cultura As String, ByVal revision As Integer)
        Dim plantilla As String = "PlantillasMail\RevisionObjetivos.en-GB.html"
        Dim subject As String = U.Traducir("Seguimiento de objetivos", "en-GB")

        Select Case cultura.ToLower()
            Case "es-es"
                plantilla = "PlantillasMail\RevisionObjetivos.es-ES.html"
                subject = U.Traducir("Seguimiento de objetivos", "es-ES")
            Case "eu-es"
                plantilla = "PlantillasMail\RevisionObjetivos.eu-ES.html"
                subject = U.Traducir("Seguimiento de objetivos", "eu-ES")
        End Select

        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
        Dim body As String = U.LeerFicheroTexto(Path.Combine(Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName, plantilla))
        body = String.Format(body, U.Traducir(System.Enum.GetName(GetType(ELL.Revision.Tipo), revision), cultura.ToLower()))
        Dim mailFrom As String = """DOB"" <" & ConfigurationManager.AppSettings("mailFrom") & ">"

        Try
            SabLib.BLL.Utils.EnviarEmail(mailFrom, mailTo, subject, body, serverEmail)
            Log.Info("Mail enviado a " & mailTo)
        Catch ex As Exception
            Log.Error("Error en EnviarMailRevisiones", ex)
        End Try
    End Sub

#End Region

End Class
