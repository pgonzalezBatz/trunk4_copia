Imports GertakariakMSWeb_Raiz.PageBase

Public Class AccionesPendientes
	Inherits UserControl
#Region "Propiedades"
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
	Dim FechaInicio As Date = Date.Today.AddDays(-(Date.Today.DayOfWeek - 1)).Date 'Fecha del primer dia laborable (1º Lunes)
	Dim FechaFin As Date = FechaInicio.AddDays(12) 'Fecha del ultimo dia laborable (2º Sabado para que busque antes de esta fecha sin incluirlo y evitamos problema de horas).
    Dim lAccionesTotales As IEnumerable(Of BatzBBDD.ACCIONES)
#End Region


#Region "Eventos"
    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Try
            lblNumSemana.Text = NumSemanaAño(Date.Today)
            lblNumSemana2.Text = NumSemanaAño(Date.Today.AddDays(7))

            CargarDatosTotales()
        Catch ex As Exception
            log.Error(ex)
        End Try
    End Sub
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            CargarDatosParciales()
        Catch ex As Exception
            log.Error(ex)
        End Try
    End Sub
    Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        BBDD.Connection.Close()
        BBDD.Dispose()
    End Sub
#End Region
#Region "Funciones y Procesos"
    Function NumSemanaAño(Fecha As Date) As Nullable(Of Integer)
        Dim Calendario As System.Globalization.Calendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar
        Return Calendario.GetWeekOfYear(Fecha, Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Friday)
    End Function
    ' ''' <summary>
    ' ''' Proceso para la carga del numero de acciones pendientes para esos días.
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Sub CargarDatos()
    '	'-------------------------------------------------------------------------------------------------------------------
    '	'Acciones Pendientes de revision.
    '	'-------------------------------------------------------------------------------------------------------------------
    '	'Obtenemos las acciones entre el rango de fechas de revision (Acciones.FechaPrevista) para 
    '	'las acciones de la incidencias de Mantenimiento de Sistema (Gertakariak.IdTipoIncidencia=6)
    '	'-------------------------------------------------------------------------------------------------------------------
    '	Dim gtkIncidencia As New gtkMatenimientoSist
    '	Dim gtkAccion As New gtkAcciones
    '	Dim Gertakari_Accion As New gtkGertakariak_Acciones
    '	Dim FechaInicio As Date = Date.Today.AddDays(-(Date.Today.DayOfWeek - 1)).Date 'Fecha del primer dia laborable (1º Lunes)
    '	Dim FechaFin As Date = FechaInicio.AddDays(12) 'Fecha del ultimo dia laborable (2º Sabado para que busque antes de esta fecha sin incluirlo y evitamos problema de horas).

    '	'-----------------------------------------------------------------------------------------
    '	'Usamos en el IN un IF(,) porque la funcion ".Listado" puede devolver NOTHING y la consulta falla.
    '	'-----------------------------------------------------------------------------------------
    '	Dim lAcciones = From Accion As gtkAcciones In gtkAccion.Listado _
    '	Join GerAc As gtkGertakariak_Acciones In If(Gertakari_Accion.Listado, New List(Of gtkGertakariak_Acciones)) On Accion.Id Equals GerAc.IdAccion _
    '	Join Incidencia As gtkMatenimientoSist In If(gtkIncidencia.Listado, New List(Of gtkMatenimientoSist)) On GerAc.IdIncidencia Equals Incidencia.Id _
    '	Where Accion.FechaPrevista >= FechaInicio And Accion.FechaPrevista < FechaFin _
    '	Select Accion
    '	'-----------------------------------------------------------------------------------------
    '	'-------------------------------------------------------------------------------------------------------------------

    '	'-----------------------------------------------------------------------------------------
    '	'Asignamos a cada dia su numero de Acciones.
    '	'-----------------------------------------------------------------------------------------
    '	lblAccionesLunes.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio And Accion.FechaPrevista < FechaInicio.AddDays(1))).Count
    '	lblAccionesMartes.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(1) And Accion.FechaPrevista < FechaInicio.AddDays(2))).Count
    '	lblAccionesMiercoles.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(2) And Accion.FechaPrevista < FechaInicio.AddDays(3))).Count
    '	lblAccionesJueves.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(3) And Accion.FechaPrevista < FechaInicio.AddDays(4))).Count
    '	lblAccionesViernes.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(4) And Accion.FechaPrevista < FechaInicio.AddDays(5))).Count

    '	lblAccionesLunes2.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(7) And Accion.FechaPrevista < FechaInicio.AddDays(8))).Count
    '	lblAccionesMartes2.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(8) And Accion.FechaPrevista < FechaInicio.AddDays(9))).Count
    '	lblAccionesMiercoles2.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(9) And Accion.FechaPrevista < FechaInicio.AddDays(10))).Count
    '	lblAccionesJueves2.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(10) And Accion.FechaPrevista < FechaInicio.AddDays(11))).Count
    '	lblAccionesViernes2.Text = (lAcciones.Where(Function(Accion) Accion.FechaPrevista >= FechaInicio.AddDays(11) And Accion.FechaPrevista < FechaInicio.AddDays(12))).Count
    '	'-----------------------------------------------------------------------------------------
    '	'Asignamos la abreviatura del dia de la semana
    '	'-----------------------------------------------------------------------------------------
    '	lblLunes.Text = FechaInicio.ToString("ddd")
    '	lblMartes.Text = FechaInicio.AddDays(1).ToString("ddd")
    '	lblMiercoles.Text = FechaInicio.AddDays(2).ToString("ddd")
    '	lblJueves.Text = FechaInicio.AddDays(3).ToString("ddd")
    '	lblViernes.Text = FechaInicio.AddDays(4).ToString("ddd")

    '	lblLunes2.Text = FechaInicio.AddDays(7).ToString("ddd")
    '	lblMartes2.Text = FechaInicio.AddDays(8).ToString("ddd")
    '	lblMiercoles2.Text = FechaInicio.AddDays(9).ToString("ddd")
    '	lblJueves2.Text = FechaInicio.AddDays(10).ToString("ddd")
    '	lblViernes2.Text = FechaInicio.AddDays(11).ToString("ddd")
    '	'-----------------------------------------------------------------------------------------
    'End Sub

    ''' <summary>
    ''' Proceso para la carga del numero de acciones pendientes para esos días
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarDatosTotales()
        '-------------------------------------------------------------------------------------------------------------------
        'Acciones Pendientes de revision.
        '-------------------------------------------------------------------------------------------------------------------
        'Obtenemos las acciones entre el rango de fechas de revision (Acciones.FechaPrevista) para 
        'las acciones de la incidencias de Mantenimiento de Sistema (Gertakariak.IdTipoIncidencia=6)
        '-------------------------------------------------------------------------------------------------------------------
        lAccionesTotales = (From gtkMant As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK _
                     From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES _
                     Where gtkAccion.FECHAPREVISTA >= FechaInicio And gtkAccion.FECHAPREVISTA < FechaFin _
                     And gtkMant.IDTIPOINCIDENCIA = 6 _
                     Select gtkAccion Distinct).AsEnumerable
        '-----------------------------------------------------------------------------------------
        'Asignamos a cada dia su numero de Acciones.
        '-----------------------------------------------------------------------------------------
        lblAccionesLunes.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio And Accion.FECHAPREVISTA < FechaInicio.AddDays(1))).Count
        lblAccionesMartes.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(1) And Accion.FECHAPREVISTA < FechaInicio.AddDays(2))).Count
        lblAccionesMiercoles.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(2) And Accion.FECHAPREVISTA < FechaInicio.AddDays(3))).Count
        lblAccionesJueves.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(3) And Accion.FECHAPREVISTA < FechaInicio.AddDays(4))).Count
        lblAccionesViernes.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(4) And Accion.FECHAPREVISTA < FechaInicio.AddDays(5))).Count

        lblAccionesLunes2.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(7) And Accion.FECHAPREVISTA < FechaInicio.AddDays(8))).Count
        lblAccionesMartes2.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(8) And Accion.FECHAPREVISTA < FechaInicio.AddDays(9))).Count
        lblAccionesMiercoles2.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(9) And Accion.FECHAPREVISTA < FechaInicio.AddDays(10))).Count
        lblAccionesJueves2.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(10) And Accion.FECHAPREVISTA < FechaInicio.AddDays(11))).Count
        lblAccionesViernes2.Text = (lAccionesTotales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(11) And Accion.FECHAPREVISTA < FechaInicio.AddDays(12))).Count
        '-------------------------------------------------------------------------------------------------------------------
        'FROGA:2013-07-09:
        '-------------------------------------------------------------------------------------------------------------------

        'Dim lAccSem = From gtkMant As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK _
        '			 From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES _
        '			 Where gtkAccion.FECHAPREVISTA >= FechaInicio And gtkAccion.FECHAPREVISTA < FechaFin _
        '			 And gtkMant.IDTIPOINCIDENCIA = 6 _
        '			 Select gtkAccion.ID Take 1 Distinct

        'Dim b As Boolean = lAccSem.Any
        'Dim i As Integer = lAccSem.Count

        'Asignamos a cada dia su numero de Acciones.
        '-----------------------------------------------------------------------------------------
        'lblAccionesLunes.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio And Accion.FECHAPREVISTA < FechaInicio.AddDays(1))).Count
        'lblAccionesMartes.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(1) And Accion.FECHAPREVISTA < FechaInicio.AddDays(2))).Count
        'lblAccionesMiercoles.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(2) And Accion.FECHAPREVISTA < FechaInicio.AddDays(3))).Count
        'lblAccionesJueves.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(3) And Accion.FECHAPREVISTA < FechaInicio.AddDays(4))).Count
        'lblAccionesViernes.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(4) And Accion.FECHAPREVISTA < FechaInicio.AddDays(5))).Count

        'lblAccionesLunes2.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(7) And Accion.FECHAPREVISTA < FechaInicio.AddDays(8))).Count
        'lblAccionesMartes2.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(8) And Accion.FECHAPREVISTA < FechaInicio.AddDays(9))).Count
        'lblAccionesMiercoles2.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(9) And Accion.FECHAPREVISTA < FechaInicio.AddDays(10))).Count
        'lblAccionesJueves2.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(10) And Accion.FECHAPREVISTA < FechaInicio.AddDays(11))).Count
        'lblAccionesViernes2.Text = (lAccSem.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(11) And Accion.FECHAPREVISTA < FechaInicio.AddDays(12))).Count
        '-------------------------------------------------------------------------------------------------------------------

        '-----------------------------------------------------------------------------------------
        'Asignamos la abreviatura del dia de la semana
        '-----------------------------------------------------------------------------------------
        lblLunes.Text = FechaInicio.ToString("ddd")
        lblMartes.Text = FechaInicio.AddDays(1).ToString("ddd")
        lblMiercoles.Text = FechaInicio.AddDays(2).ToString("ddd")
        lblJueves.Text = FechaInicio.AddDays(3).ToString("ddd")
        lblViernes.Text = FechaInicio.AddDays(4).ToString("ddd")

        lblLunes2.Text = FechaInicio.AddDays(7).ToString("ddd")
        lblMartes2.Text = FechaInicio.AddDays(8).ToString("ddd")
        lblMiercoles2.Text = FechaInicio.AddDays(9).ToString("ddd")
        lblJueves2.Text = FechaInicio.AddDays(10).ToString("ddd")
        lblViernes2.Text = FechaInicio.AddDays(11).ToString("ddd")
        '----------------------------------------------------------------------------------------

    End Sub

    ''' <summary>
    ''' Proceso para la carga del numero de acciones pendientes para el filtro de la pagina principal.
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarDatosParciales()
        Dim lAccionesParciales As IEnumerable(Of BatzBBDD.ACCIONES) = Nothing
        Dim PaginaFiltro As New GertakariakMSWeb_Raiz._Default1
        Dim FiltroGTK As gtkFiltro = If(Session("FiltroGTK"), New gtkFiltro)
        Dim lGTK As List(Of BatzBBDD.GERTAKARIAK) = PaginaFiltro.ListadoIncidencias(FiltroGTK)
        lAccionesParciales = From gtkMant As BatzBBDD.GERTAKARIAK In lGTK _
                            From gtkAccion As BatzBBDD.ACCIONES In gtkMant.ACCIONES _
                            Join Accion As BatzBBDD.ACCIONES In lAccionesTotales On Accion.ID Equals gtkAccion.ID _
                            Select Accion Distinct
        '-----------------------------------------------------------------------------------------
        'Asignamos a cada dia su numero de Acciones.
        '-----------------------------------------------------------------------------------------
        lblAccionesLunesP.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio And Accion.FECHAPREVISTA < FechaInicio.AddDays(1))).Count
        lblAccionesMartesP.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(1) And Accion.FECHAPREVISTA < FechaInicio.AddDays(2))).Count
        lblAccionesMiercolesP.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(2) And Accion.FECHAPREVISTA < FechaInicio.AddDays(3))).Count
        lblAccionesJuevesP.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(3) And Accion.FECHAPREVISTA < FechaInicio.AddDays(4))).Count
        lblAccionesViernesP.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(4) And Accion.FECHAPREVISTA < FechaInicio.AddDays(5))).Count

        lblAccionesLunes2P.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(7) And Accion.FECHAPREVISTA < FechaInicio.AddDays(8))).Count
        lblAccionesMartes2P.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(8) And Accion.FECHAPREVISTA < FechaInicio.AddDays(9))).Count
        lblAccionesMiercoles2P.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(9) And Accion.FECHAPREVISTA < FechaInicio.AddDays(10))).Count
        lblAccionesJueves2P.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(10) And Accion.FECHAPREVISTA < FechaInicio.AddDays(11))).Count
        lblAccionesViernes2P.Text = (lAccionesParciales.Where(Function(Accion) Accion.FECHAPREVISTA >= FechaInicio.AddDays(11) And Accion.FECHAPREVISTA < FechaInicio.AddDays(12))).Count
        '----------------------------------------------------------------------------------------
    End Sub
#End Region
End Class