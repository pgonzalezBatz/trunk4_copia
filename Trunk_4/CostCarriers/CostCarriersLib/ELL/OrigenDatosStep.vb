Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class OrigenDatosStep

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum OrigenDatosStep
            Presupuesto_bonos = 1
            Presupuesto_viajes = 2
            Manual = 3
            'Nada = 4
            Planificacion = 5
        End Enum

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum TipoDistribucion
            Presupuesto = 0
            Planificacion = 1
            Viaje = 3
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

#End Region

    End Class

End Namespace

