Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostGroupOT

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum Agrupacion
            Validation_tooling = 1
            Validation_costs = 2
            Serial_tooling = 3
            Prototype_tooling = 4
            Parts_for_customer = 5
        End Enum

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Nombre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

        ''' <summary>
        ''' Cambiar plantas
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CambiarPlantas() As Boolean

        ''' <summary>
        ''' Fórmula si el cliente no paga nada
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FormulaBATZ() As String

        ''' <summary>
        ''' Fórmula si el cliente paga algo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FormulaCustomer() As String

        ''' <summary>
        ''' Id de agrupación de cost groups de OT
        ''' </summary>
        ''' <returns></returns>
        Public Property IdAgrupacion As Integer = Integer.MinValue

        ''' <summary>
        ''' Indica si el step puede cambiar el porcentaje
        ''' </summary>
        ''' <returns></returns>
        Public Property CambioPorcentaje As Boolean = False

#End Region

    End Class

End Namespace

