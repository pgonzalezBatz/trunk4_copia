Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class StepPlantilla

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum DatoRealOrigen As Integer
            EXT = 0
            INT = 1
            INT_EXT = 2
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
        ''' Descripcion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

        ''' <summary>
        ''' Id cost group plantilla
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCostGroupPlantilla() As Integer

        ''' <summary>
        ''' Offer budget cost origen datos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OBCOrigenDatos() As Integer

        ''' <summary>
        ''' Target cost formula
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TCFormula() As String

        ''' <summary>
        ''' Target cost formula customer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TCFormulaCustomer() As String

        ''' <summary>
        ''' Gastos año origen datos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property GastosAñoOrigenDatos() As Integer

        ''' <summary>
        ''' Ingresos año origen datos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IngresosAñoOrigenDatos() As Integer

        ''' <summary>
        ''' Orden
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Orden() As Integer

        ''' <summary>
        ''' Origen de dato real
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OrigenDatoReal() As Integer

        ''' <summary>
        ''' Indica si es un paso de información general
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property EsInfoGeneral() As Boolean = False

        '''' <summary>
        '''' Paid by customer origen datos
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property PBCOrigenDatos() As Integer

        ''' <summary>
        ''' Target hours formula decodificada
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property TCFormulaDecodificada() As String
            Get
                Dim variables As List(Of VariableFormula) = BLL.VariablesFormulaBLL.CargarListado()

                Dim formula As String = TCFormula
                If (Not String.IsNullOrEmpty(TCFormula)) Then
                    For Each variable In variables
                        formula = formula.Replace(String.Format("[{0}]", variable.Id), String.Format("[{0}]", variable.Nombre))
                    Next
                End If

                Return formula
            End Get
        End Property

        ''' <summary>
        ''' Target hours formula customer decodificada
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property TCFormulaCustomerDecodificada() As String
            Get
                Dim variables As List(Of VariableFormula) = BLL.VariablesFormulaBLL.CargarListado()

                Dim formula As String = TCFormulaCustomer
                If (Not String.IsNullOrEmpty(TCFormula)) Then
                    For Each variable In variables
                        formula = formula.Replace(String.Format("[{0}]", variable.Id), String.Format("[{0}]", variable.Nombre))
                    Next
                End If

                Return formula
            End Get
        End Property
#End Region

    End Class

End Namespace

