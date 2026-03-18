Namespace ELL.BRAIN

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CCMetadataYear

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
        Public Property Planta As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Anyo As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property CodigoPortador As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property CodigoMoneda As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Moneda As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property PresupBonosPersona As Decimal = Decimal.Zero

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property PresupFacturas As Decimal = Decimal.Zero

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property PresupViajes As Decimal = Decimal.Zero

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property ImporteVentaCliente As Decimal = Decimal.Zero

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

#End Region

    End Class

End Namespace

