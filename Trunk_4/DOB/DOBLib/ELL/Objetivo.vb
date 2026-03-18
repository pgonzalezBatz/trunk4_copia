Namespace ELL

    Public Class Objetivo

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum TipoPeriodicidad
            Mensual = 1
            Trimestral = 3
            Cuatrimentral = 4
            Semestral = 6
        End Enum

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum TipoAgrupacion
            Reto = 1
            Responsable = 2
            Proceso = 3
        End Enum

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum Semaforo
            Rojo = 1
            Amarillo = 2
            Verde = 3
        End Enum

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum TipoTendencia
            Ascendente = 1
            Plana = 2
            Descendente = 3
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Descripción
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

        ''' <summary>
        ''' Id reto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdReto() As Integer

        ''' <summary>
        ''' Id proceso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdProceso() As Integer

        ''' <summary>
        ''' Id responsable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdResponsable() As Integer

        ''' <summary>
        ''' Fecha objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaObjetivo() As DateTime

        ''' <summary>
        ''' Nombre indicador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreIndicador() As String

        ''' <summary>
        ''' Descripción indicador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DescripcionIndicador() As String

        ''' <summary>
        ''' Id tipo indicador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipoIndicador() As Integer

        ''' <summary>
        ''' Valor inicial
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValorInicial() As Decimal

        ''' <summary>
        ''' Valor objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValorObjetivo() As Decimal

        ''' <summary>
        ''' Valor objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValorActual() As Decimal

        ''' <summary>
        ''' Titulo reto 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TituloReto() As String

        ''' <summary>
        ''' Código proceso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodigoProceso() As String

        ''' <summary>
        ''' Tipo indicador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoIndicador() As String

        ''' <summary>
        ''' Mes objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property MesObjetivo() As Integer

        ''' <summary>
        ''' Año objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property AñoObjetivo() As Integer

        ''' <summary>
        ''' Responsable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Responsable() As String

        ''' <summary>
        ''' Fecha alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaAlta() As DateTime

        ''' <summary>
        ''' Fecha baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaBaja() As DateTime

        ''' <summary>
        ''' Id usuario alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioAlta() As Integer

        ''' <summary>
        ''' Id usuario baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioBaja() As Integer

        ''' <summary>
        ''' Cumplimiento acciónes
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CumplimientoAcciones() As Decimal

        ''' <summary>
        ''' Id planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Periodicidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Periodicidad() As TipoPeriodicidad

        ''' <summary>
        ''' Reto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Reto() As String

        ''' <summary>
        ''' ValorAnterior
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValorAnterior() As Decimal

        ''' <summary>
        ''' Tiene documentos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TieneDocumentos() As Boolean

        ''' <summary>
        ''' Tiene documentos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Sentido() As Boolean

        ''' <summary>
        ''' Tiene acciones hijas
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TieneAcciones() As Boolean = False

        ''' <summary>
        ''' Id objetivo padre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdObjetivoPadre() As Integer = Integer.MinValue


        ''' <summary>
        ''' Nivel en el árbol. Sirve para mostrar los objetivos en CuadroMandoHijos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NivelArbol() As Integer = 0

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Planta() As String
            Get
                Return BLL.PlantasBLL.ObtenerPlanta(IdPlanta).Planta
            End Get
        End Property

        ''' <summary>
        ''' Color actual
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property ColorActual() As Semaforo
            Get
                Dim color As Semaforo = Semaforo.Rojo
                If (ValorActual <> Decimal.MinValue AndAlso ValorObjetivo <> Decimal.MinValue) Then

                    If (Sentido) Then
                        ' Ascendente
                        Select Case ValorActual
                            Case >= ValorObjetivo
                                color = Semaforo.Verde
                            Case < ValorActual
                                color = Semaforo.Rojo
                        End Select
                    Else
                        ' Descendente
                        Select Case ValorActual
                            Case <= ValorObjetivo
                                color = Semaforo.Verde
                            Case > ValorObjetivo
                                color = Semaforo.Rojo
                        End Select
                    End If
                End If

                Return color
            End Get
        End Property

        ''' <summary>
        ''' Tendencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Tendencia() As TipoTendencia
            Get
                Dim tend As TipoTendencia = TipoTendencia.Plana
                If (ValorActual <> Decimal.MinValue AndAlso ValorObjetivo <> Decimal.MinValue) Then
                    If ((ValorActual > ValorAnterior AndAlso Sentido) OrElse (ValorActual < ValorAnterior AndAlso Not Sentido)) Then
                        tend = TipoTendencia.Ascendente
                    ElseIf ((ValorActual < ValorAnterior AndAlso Sentido) OrElse (ValorActual > ValorAnterior AndAlso Not Sentido)) Then
                        tend = TipoTendencia.Descendente
                    End If
                End If

                Return tend
            End Get
        End Property

#End Region

    End Class

End Namespace
