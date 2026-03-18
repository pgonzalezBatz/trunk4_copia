Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionLinea

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Id validación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdValidacion() As Integer

        ''' <summary>
        ''' Id step
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdStep() As Integer

        ''' <summary>
        ''' Paid by customer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OfferBudget() As Integer

        ''' <summary>
        ''' Paid by customer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PaidByCustomer() As Integer

        ''' <summary>
        ''' Budget approved
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property BudgetApproved() As Integer

        ''' <summary>
        ''' Real data interno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property RealDataInt() As Integer
            Get
                Dim valor As Integer = 0
                If (IdStep <> Integer.MinValue AndAlso IdStep <> 0) Then
                    Dim paso As ELL.Step = BLL.StepsBLL.Obtener(IdStep)

                    If (paso IsNot Nothing) Then
                        valor = paso.DatoRealInterno
                    End If
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' Real data externo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property RealDataExt() As Integer
            Get
                Dim valor As Integer = 0
                If (IdStep <> Integer.MinValue AndAlso IdStep <> 0) Then
                    Dim paso As ELL.Step = BLL.StepsBLL.Obtener(IdStep)

                    If (paso IsNot Nothing) Then
                        valor = paso.DatoRealExterno
                    End If
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' Real data externo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property RealDataInt_Ext() As Integer
            Get
                Dim valor As Integer = 0
                If (IdStep <> Integer.MinValue AndAlso IdStep <> 0) Then
                    Dim paso As ELL.Step = BLL.StepsBLL.Obtener(IdStep)

                    If (paso IsNot Nothing) Then
                        valor = paso.DatoRealInterno + paso.DatoRealExterno
                    End If
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' Horas
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Hours() As Integer

        ''' <summary>
        ''' Nombre en XPERT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

        ''' <summary>
        ''' Id cost group
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCostGroup() As Integer

        ''' <summary>
        ''' Id estado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdEstadoValidacion() As Integer

        ''' <summary>
        ''' Id usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUser() As Integer

        ''' <summary>
        ''' Id planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Validaciones año
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValidacionesAño() As List(Of ELL.ValidacionAño)

#End Region

    End Class

End Namespace

