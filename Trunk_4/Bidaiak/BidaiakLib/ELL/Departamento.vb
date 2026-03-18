Namespace ELL

    Public Class Departamento

#Region "Variables miembro"

        Private _codDepart As String = String.Empty
        Private _departamento As String = String.Empty
        Private _cuenta18 As Integer = 0
        Private _cuenta8 As Integer = 0
        Private _cuenta0 As Integer = 0
        Private _ofImproductiva As Integer = 0
        Private _obsoleta As Boolean = False
        Private _idPlanta As Integer = Integer.MinValue
        Private _actividades As List(Of Actividad) = Nothing

#End Region

#Region "Properties"

        ''' <summary>
        ''' Codigo del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property CodigoDepartamento() As String
            Get
                Return _codDepart
            End Get
            Set(ByVal value As String)
                _codDepart = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Departamento() As String
            Get
                Return _departamento
            End Get
            Set(ByVal value As String)
                _departamento = value
            End Set
        End Property

        ''' <summary>
        ''' Cuenta contable al 18%
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Cuenta18() As Integer
            Get
                Return _cuenta18
            End Get
            Set(ByVal value As Integer)
                _cuenta18 = value
            End Set
        End Property

        ''' <summary>
        ''' Cuenta contable al 8%
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Cuenta8() As Integer
            Get
                Return _cuenta8
            End Get
            Set(ByVal value As Integer)
                _cuenta8 = value
            End Set
        End Property

        ''' <summary>
        ''' Cuenta contable al 0%
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Cuenta0() As Integer
            Get
                Return _cuenta0
            End Get
            Set(ByVal value As Integer)
                _cuenta0 = value
            End Set
        End Property

        ''' <summary>
        ''' Of improductiva asociada a un departamento. Cuando se realice un viaje sin of, esta of sera la que se registre en los costos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property OFImproductiva() As Integer
            Get
                Return _ofImproductiva
            End Get
            Set(ByVal value As Integer)
                _ofImproductiva = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si esta obsoleta o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Obsoleta() As Boolean
            Get
                Return _obsoleta
            End Get
            Set(ByVal value As Boolean)
                _obsoleta = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

        ''' <summary>
        ''' Actividades relacionadas con el departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Actividades() As List(Of Actividad)
            Get
                Return _actividades
            End Get
            Set(ByVal value As List(Of Actividad))
                _actividades = value
            End Set
        End Property

#End Region

    End Class

End Namespace