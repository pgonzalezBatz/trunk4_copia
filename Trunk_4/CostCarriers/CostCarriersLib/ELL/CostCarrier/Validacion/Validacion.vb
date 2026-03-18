Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class Validacion

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum Estado
            Waiting_for_approval = 10
            Approved = 20
            Rejected = 30
            Opened = 40
            Closed = 50
        End Enum

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum Accion
            Send_to_validate = 10
            Approve = 20
            Reject = 30
            Open = 40
            Close = 50
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
        ''' Fecha
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Fecha() As DateTime

        ''' <summary>
        ''' Id usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUser() As Integer

        ''' <summary>
        ''' Denominación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Denominacion() As String

        ''' <summary>
        ''' Descripción
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

        ''' <summary>
        ''' Previsto en PG
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PrevistoPG() As Boolean

        ''' <summary>
        ''' Id estado validación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdEstadoValidacion() As Integer

        ''' <summary>
        ''' Id planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String

        ''' <summary>
        ''' Cabecera del proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCabecera() As Integer

        ''' <summary>
        ''' Validaciones línea
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValidacionesLinea() As List(Of ELL.ValidacionLinea)

        ''' <summary>
        ''' Validaciones info adicional
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValidacionesInfoAdicional() As List(Of ELL.ValidacionInfoAdicional)

#End Region

    End Class

End Namespace

