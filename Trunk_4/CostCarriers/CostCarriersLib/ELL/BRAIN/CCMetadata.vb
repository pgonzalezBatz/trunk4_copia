Namespace ELL.BRAIN

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CCMetadata

#Region "Constantes"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Shared ORIGEN_MANUAL = "M"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Shared ORIGEN_AUTO = "A"

#End Region

#Region "Miembros"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Empresa As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Planta As String = "000"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property CodigoPortador As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoPlanta As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property DenomAmpliada As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property FechaIni As DateTime

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property FechaFin As DateTime = DateTime.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Negocio As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Responsable As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IdResponsableSAB As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property FechaEstimIni As DateTime = DateTime.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property NumAnyosSerie As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Producto As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IdProyecto As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Proyecto As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property EstadoProyecto As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoActivo As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IdTipoActivo As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Propiedad As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Lantegi As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property BudgetCode As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property DescBudgetCode As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property CantidadSolicitada As Decimal = Decimal.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property EmpresaProductiva As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Origen As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Denominacion As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NombreEmpresa As String
            Get
                Dim nombre As String = String.Empty
                If (Not String.IsNullOrEmpty(Empresa)) Then
                    Dim plantasBLL As New SabLib.BLL.PlantasComponent()
                    nombre = plantasBLL.GetPlantaByIdBRAIN(Empresa).Nombre
                End If

                Return nombre
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DescripcionCompleta As String
            Get
                Return String.Format("{0} - {1}", CodigoPortador, Denominacion)
            End Get
        End Property

#End Region

    End Class

End Namespace

