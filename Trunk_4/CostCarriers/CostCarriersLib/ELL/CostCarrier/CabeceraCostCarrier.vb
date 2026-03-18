Imports System.Web.Script.Serialization

Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CabeceraCostCarrier

#Region "Constantes"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Shared TIPO_PROY_INDUSTRIALIZATION As String = "INDUSTRIALIZATION"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Shared TIPO_PROY_R_D As String = "R-D"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Shared TIPO_PROY_ECO_BATZ As String = "ECO BATZ"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Shared TIPO_PROY_PREDEVELOPMENT As String = "PREDEVELOPMENT"

#End Region


#Region "Miembros"

        ''' <summary>
        ''' 
        ''' </summary>
        Private _años As RangoAños = Nothing

        ''' <summary>
        ''' 
        ''' </summary>
        Private _plantas As List(Of ELL.Planta) = Nothing

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer = Integer.MinValue

        ''' <summary>
        ''' Id tipo proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipoProyecto() As Integer

        ''' <summary>
        ''' Proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Proyecto() As String

        ''' <summary>
        ''' Abreviatura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Abreviatura() As String

        ''' <summary>
        ''' Nombre proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreProyecto() As String

        ''' <summary>
        ''' Producto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Producto() As String

        ''' <summary>
        ''' Lista de plantas
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Plantas As List(Of Planta)
            Get
                If (_plantas Is Nothing) Then
                    _plantas = BLL.PlantasBLL.CargarListado(Id, Proyecto)
                End If

                Return _plantas
            End Get
        End Property

        ''' <summary>
        ''' Id oferta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdOferta As Integer = Integer.MinValue

        ''' <summary>
        ''' Código proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodigoProyecto() As String

        ''' <summary>
        ''' Responsable del proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Responsable() As String

        ''' <summary>
        ''' Co-responsable del proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CoOwner() As String

        ''' <summary>
        ''' Tipo del proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoProyecto() As String

        ''' <summary>
        ''' Tipo del proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoProyPtksis() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Años As RangoAños
            Get
                If (_años Is Nothing) Then
                    Dim jss As New JavaScriptSerializer()

                    Using cliente As New ServicioBonos.ServicioBonos
                        ' AnyoInicio:x; AnyoFin:x
                        ' En principio los años van por proyecto y estado pero vamos a suponer que todos los estados tiene el mismo rango de años
                        ' Para eso cogemos el primer estado que nos encontremos
                        Dim estados() As String

                        If (IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.R_D) Then
                            estados = {"R-D"}
                        ElseIf (IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Industrialization) Then
                            estados = {"Development", "Industrialization", "G3-Project Acceptance"}
                        ElseIf (IdTipoProyecto = ELL.TipoProyecto.TipoProyecto.Predev) Then
                            estados = {"Predevelopment"}
                        End If

                        _años = jss.Deserialize(Of RangoAños)(cliente.GetAnyosDistribucion(Proyecto, estados, ELL.OrigenDatosStep.TipoDistribucion.Planificacion))
                    End Using
                End If

                Return _años
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns>1 abierto, 0 cerrado, -1 sin estado</returns>
        Public ReadOnly Property Abierto As Integer
            Get
                Dim ret As Integer = -1

                ' Si tenemos informado el codigo del portador vemos si está a bierto o cerrado
                ' Entiendo que como el código que verifico es el troncal y solo se abren en Igorre la empresa es la 1
                If (Not String.IsNullOrEmpty(CodigoProyecto)) Then
                    Dim portador As ELL.BRAIN.CostCarrier = BLL.BRAIN.CostCarriersBLL.Obtener(CodigoProyecto, 1)

                    If (portador IsNot Nothing) Then
                        If (portador.Lantegi = "OBS" OrElse String.IsNullOrEmpty(portador.Lantegi)) Then
                            ret = 0
                        Else
                            ret = 1
                        End If
                    End If
                End If

                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Para tipos de proyectos ptksis que no son de industrialization
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property SOP() As DateTime = DateTime.MinValue

        ''' <summary>
        ''' Años serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property SeriesYears() As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property ContienePasosAbrir As Boolean = False

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Steps As List(Of ELL.Step) = New List(Of [Step])

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Cliente As String = String.Empty

#End Region

    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class RangoAños

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property AnyoInicio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property AnyoFin As Integer = Integer.MinValue

    End Class

End Namespace

