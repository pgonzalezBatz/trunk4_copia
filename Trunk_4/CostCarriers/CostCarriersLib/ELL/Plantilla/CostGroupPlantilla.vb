Imports System.Web.Script.Serialization

Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostGroupPlantilla

#Region "Miembros"

        Private _desc As String = String.Empty

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
            Get
                Dim ret As String = String.Empty

                If (IdBonos <> Integer.MinValue) Then
                    If (IdCostGroupOT <> Integer.MinValue AndAlso IdCostGroupOT <> -1) Then
                        ret = CostGroupOT
                    ElseIf (IdBonos = -1) Then
                        ret = "No group"
                    Else
                        Dim jss As New JavaScriptSerializer()

                        Using cliente As New ServicioBonos.ServicioBonos
                            ret = jss.Deserialize(Of Object)(cliente.GetGrupoDistribucion(IdBonos))("Nombre")
                        End Using
                    End If
                ElseIf (String.IsNullOrEmpty(_desc) AndAlso IdCostGroupOT <> Integer.MinValue AndAlso IdCostGroupOT <> -1) Then
                    ret = CostGroupOT
                ElseIf (Not String.IsNullOrEmpty(_desc) AndAlso (IdCostGroupOT = Integer.MinValue OrElse IdCostGroupOT = -1)) Then
                    ret = _desc
                End If

                Return ret
            End Get
            Set(value As String)
                _desc = value
            End Set
        End Property

        ''' <summary>
        ''' Descripcion guardada
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DescripcionGuardada() As String

        ''' <summary>
        ''' Id estado plantilla
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdEstadoPlantilla() As Integer

        ''' <summary>
        ''' Id cost group de bonos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdBonos() As Integer

        ''' <summary>
        ''' Id cost group OT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCostGroupOT() As Integer

        ''' <summary>
        ''' Cost group de bonos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CostGroupOT() As String

#End Region

    End Class

End Namespace

