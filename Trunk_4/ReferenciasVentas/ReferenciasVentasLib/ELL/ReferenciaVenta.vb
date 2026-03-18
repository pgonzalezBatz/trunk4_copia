Namespace ELL

    <Serializable()>
    Public Class ReferenciaVenta

#Region "Variables miembro"

        Private _Id As Integer = Integer.MinValue
        Private _IdSolicitud As Integer = Integer.MinValue
        Private _TipoReferencia As String = String.Empty
        Private _TipoReferenciaNombre As String = String.Empty
        Private _CustomerPartNumber As String = String.Empty
        Private _DrawingNumber As String = String.Empty
        Private _TipoNumero As NumberType
        Private _TipoNumeroNombre As String = String.Empty
        Private _PlanoWeb As String = String.Empty
        Private _NivelIngenieria As String = String.Empty
        Private _PreviousBatzPN As String = String.Empty
        Private _EvolutionChanges As String = String.Empty
        Private _IdProduct As Integer = Integer.MinValue
        Private _NameProduct As String = String.Empty
        Private _IdType As Integer = Integer.MinValue
        Private _IdTransmissionMode As Integer = Integer.MinValue
        Private _IdCustomerProjectName As String = String.Empty
        Private _NameCustomerProjectName As String = String.Empty
        Private _Specification As String = String.Empty
        Private _FinalNameBrain As String = String.Empty
        Private _PlantsToCharge As List(Of String) = Nothing
        Private _Comentario As String = String.Empty
        Private _BatzPartNumber As String = String.Empty
        Private _InsercionBrain As Boolean = False
        Private _EnvioEmail As Boolean = False
        Private _integrado As Boolean = False

#End Region

#Region "Enumeraciones"
        Public Enum NumberType As Integer
            Customer = 1
            Drawing = 2
            Ambos = 3
            Development = 4
        End Enum

#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

        ', ByVal BatzPartNumber As Integer
        Public Sub New(ByVal id As Integer, ByVal TipoReferencia As String, ByVal CustomerPartNumber As String, ByVal DrawingNumber As String, ByVal TipoNumero As NumberType, ByVal PlanoWeb As String, ByVal NivelIngenieria As String,
                       ByVal PreviousBatzPN As String, ByVal EvolutionChanges As String, IdProduct As Integer, ByVal IdType As Integer, ByVal IdTransmissionMode As Integer, ByVal IdCustomerProjectName As String,
                       ByVal NameCustomerProjectName As String, ByVal Specification As String, ByVal FinalNameBrain As String, ByVal PlantsToCharge As List(Of String), ByVal Comentario As String)
            Me._Id = id
            Me._TipoReferencia = TipoReferencia
            Me._CustomerPartNumber = CustomerPartNumber
            Me._DrawingNumber = DrawingNumber
            Me._TipoNumero = TipoNumero
            Me._PlanoWeb = PlanoWeb
            Me._NivelIngenieria = NivelIngenieria
            Me._PreviousBatzPN = PreviousBatzPN
            Me._EvolutionChanges = EvolutionChanges
            Me._IdProduct = IdProduct
            Me._IdType = IdType
            Me._IdTransmissionMode = IdTransmissionMode
            Me._IdCustomerProjectName = IdCustomerProjectName
            Me._NameCustomerProjectName = NameCustomerProjectName
            Me._Specification = Specification
            Me._FinalNameBrain = FinalNameBrain
            Me._PlantsToCharge = PlantsToCharge
            Me._Comentario = Comentario
        End Sub
#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal value As Integer)
                _Id = value
            End Set
        End Property

        ''' <summary>
        ''' Id de la solicitud
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSolicitud() As Integer
            Get
                Return _IdSolicitud
            End Get
            Set(ByVal value As Integer)
                _IdSolicitud = value
            End Set
        End Property

        ''' <summary>
        ''' Tipo de referencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipoReferencia() As Integer
            Get
                Return _TipoReferencia
            End Get
            Set(ByVal value As Integer)
                _TipoReferencia = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre del tipo de referencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoReferenciaNombre() As String
            Get
                Dim oTipoReferenciaVentaBLL As New BLL.TiposReferenciaVentaBLL
                Return oTipoReferenciaVentaBLL.CargarTipoReferenciaVenta(Me._TipoReferencia).Nombre
            End Get
            Set(ByVal value As String)
                _TipoReferenciaNombre = value
            End Set
        End Property

        ''' <summary>
        ''' CustomerPartNumber
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CustomerPartNumber() As String
            Get
                Return _CustomerPartNumber
            End Get
            Set(ByVal value As String)
                _CustomerPartNumber = value
            End Set
        End Property

        ''' <summary>
        ''' CustomerPartNumber
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DrawingNumber() As String
            Get
                Return _DrawingNumber
            End Get
            Set(ByVal value As String)
                _DrawingNumber = value
            End Set
        End Property

        ''' <summary>
        ''' TipoNumero
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoNumero() As NumberType
            Get
                Return _TipoNumero
            End Get
            Set(ByVal value As NumberType)
                _TipoNumero = value
            End Set
        End Property

        ''' <summary>
        ''' TipoNumeroNombre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoNumeroNombre() As String
            Get
                If (_TipoNumero = NumberType.Ambos) Then
                    Return "Customer & drawing"
                Else
                    Return _TipoNumero.ToString
                End If
            End Get
            Set(ByVal value As String)
                _TipoNumeroNombre = value
            End Set
        End Property

        ''' <summary>
        ''' PlanoWeb
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PlanoWeb() As String
            Get
                Return _PlanoWeb
            End Get
            Set(ByVal value As String)
                _PlanoWeb = value
            End Set
        End Property

        ''' <summary>
        ''' NivelIngenieria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NivelIngenieria() As String
            Get
                Return _NivelIngenieria
            End Get
            Set(ByVal value As String)
                _NivelIngenieria = value
            End Set
        End Property

        ''' <summary>
        ''' PreviousBatzPartNumber
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PreviousBatzPartNumber() As String
            Get
                Return _PreviousBatzPN
            End Get
            Set(ByVal value As String)
                _PreviousBatzPN = value
            End Set
        End Property

        ''' <summary>
        ''' EvolutionChanges
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property EvolutionChanges() As String
            Get
                Return _EvolutionChanges
            End Get
            Set(ByVal value As String)
                _EvolutionChanges = value
            End Set
        End Property

        ''' <summary>
        ''' IdProduct
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdProduct() As Integer
            Get
                Return _IdProduct
            End Get
            Set(ByVal value As Integer)
                _IdProduct = value
            End Set
        End Property

        ''' <summary>
        ''' NameProduct
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NameProduct() As String
            Get
                Dim oProducto As New BLL.ProductoBLL
                Return oProducto.CargarProducto(Me._IdProduct).Nombre
            End Get
            Set(ByVal value As String)
                _NameProduct = value
            End Set
        End Property

        ''' <summary>
        ''' IdType
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdType() As Integer
            Get
                Return _IdType
            End Get
            Set(ByVal value As Integer)
                _IdType = value
            End Set
        End Property

        ''' <summary>
        ''' NameType
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NameType() As String
            Get
                Dim oTypeBLL As New BLL.TypeBLL
                If (Me._IdType <> Integer.MinValue) Then
                    Return oTypeBLL.CargarTipo(Me._IdType).Nombre
                Else
                    Return "-"
                End If
            End Get
            Set(ByVal value As String)
                _IdProduct = value
            End Set
        End Property

        ''' <summary>
        ''' IdTransmissionMode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTransmissionMode() As Integer
            Get
                Return _IdTransmissionMode
            End Get
            Set(ByVal value As Integer)
                _IdTransmissionMode = value
            End Set
        End Property

        ''' <summary>
        ''' NameProduct
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NameTransmissionMode() As String
            Get
                Dim oTransmissionMode As New BLL.TransmissionModeBLL
                If (Me._IdTransmissionMode <> Integer.MinValue) Then
                    Return oTransmissionMode.CargarTransmissionMode(Me._IdTransmissionMode).Nombre
                Else
                    Return "-"
                End If
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        ''' <summary>
        ''' IdCustomerProjectName
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCustomerProjectName() As String
            Get
                Return _IdCustomerProjectName
            End Get
            Set(ByVal value As String)
                _IdCustomerProjectName = value
            End Set
        End Property

        ''' <summary>
        ''' NameCustomerProjectName
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NameCustomerProjectName() As String
            Get
                Try
                    'Dim oBrainBLL As New BLL.BrainBLL
                    'Return oBrainBLL.CargarProyectoPorId(_IdCustomerProjectName)

                    Dim ret As String = String.Empty
                    Dim ptksisBLL As New BLL.ProyectosPTKSisBLL
                    Dim proyecto As ELL.Proyectos = ptksisBLL.CargarProyectoPorId(_IdCustomerProjectName)
                    If (proyecto IsNot Nothing) Then
                        ret = proyecto.Nombre
                    End If
                    Return ret
                Catch ex As Exception
                    Return String.Empty
                End Try
                'Return _NameCustomerProjectName
            End Get
            Set(ByVal value As String)
                _NameCustomerProjectName = value
            End Set
        End Property

        ''' <summary>
        ''' Specification
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Specification() As String
            Get
                Return _Specification
            End Get
            Set(ByVal value As String)
                _Specification = value
            End Set
        End Property

        ''' <summary>
        ''' FinalNameBrain
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FinalNameBrain() As String
            Get
                Return _FinalNameBrain
            End Get
            Set(ByVal value As String)
                _FinalNameBrain = value
            End Set
        End Property


        ''' <summary>
        ''' IdPlantToCharge
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PlantsToCharge() As List(Of String)
            Get
                Return _PlantsToCharge
            End Get
            Set(ByVal value As List(Of String))
                _PlantsToCharge = value
            End Set
        End Property

        ''' <summary>
        ''' Comentario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Comentario() As String
            Get
                Return _Comentario
            End Get
            Set(ByVal value As String)
                _Comentario = value
            End Set
        End Property

        ''' <summary>
        ''' BatzPartNumber
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property BatzPartNumber() As String
            Get
                Return _BatzPartNumber
            End Get
            Set(ByVal value As String)
                _BatzPartNumber = value
            End Set
        End Property

        ''' <summary>
        ''' InsercionBrain
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InsercionBrain() As Boolean
            Get
                Return _InsercionBrain
            End Get
            Set(ByVal value As Boolean)
                _InsercionBrain = value
            End Set
        End Property

        ''' <summary>
        ''' EnvioEmail
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EnvioEmail() As Boolean
            Get
                Return _EnvioEmail
            End Get
            Set(ByVal value As Boolean)
                _EnvioEmail = value
            End Set
        End Property

        ''' <summary>
        ''' Integrado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Integrado() As Boolean
            Get
                Return _integrado
            End Get
            Set(ByVal value As Boolean)
                _integrado = value
            End Set
        End Property

#End Region

    End Class

    Public Class FiltradoHistorial

#Region "Variables miembro"

        Private _IdSolicitud As Integer = Integer.MinValue
        Private _IdUsuario As Integer = Integer.MinValue
        Private _FechaCreacionDesde As String = String.Empty
        Private _FechaCreacionHasta As String = String.Empty
        Private _FechaResolucionDesde As String = String.Empty
        Private _FechaResolucionHasta As String = String.Empty
        Private _Aprobado As Integer = 2
        Private _IdTipoSolicitud As Integer = Integer.MinValue

#End Region

#Region "Properties"

        Public Property IdSolicitud() As Integer
            Get
                Return _IdSolicitud
            End Get
            Set(ByVal value As Integer)
                _IdSolicitud = value
            End Set
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _IdUsuario
            End Get
            Set(ByVal value As Integer)
                _IdUsuario = value
            End Set
        End Property

        Public Property FechaCreacionDesde() As String
            Get
                Return _FechaCreacionDesde
            End Get
            Set(ByVal value As String)
                _FechaCreacionDesde = value
            End Set
        End Property

        Public Property FechaCreacionHasta() As String
            Get
                Return _FechaCreacionHasta
            End Get
            Set(ByVal value As String)
                _FechaCreacionHasta = value
            End Set
        End Property

        Public Property FechaResolucionDesde() As String
            Get
                Return _FechaResolucionDesde
            End Get
            Set(ByVal value As String)
                _FechaResolucionDesde = value
            End Set
        End Property

        Public Property FechaResolucionHasta() As String
            Get
                Return _FechaResolucionHasta
            End Get
            Set(ByVal value As String)
                _FechaResolucionHasta = value
            End Set
        End Property

        Public Property Aprobado() As Integer
            Get
                Return _Aprobado
            End Get
            Set(ByVal value As Integer)
                _Aprobado = value
            End Set
        End Property

        Public Property IdTipoSolicitud() As Integer
            Get
                Return _IdTipoSolicitud
            End Get
            Set(ByVal value As Integer)
                _IdTipoSolicitud = value
            End Set
        End Property

#End Region

    End Class

End Namespace

