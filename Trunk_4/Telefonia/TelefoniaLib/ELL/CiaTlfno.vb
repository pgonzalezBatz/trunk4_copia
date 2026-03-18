Namespace ELL

    Public Class CiaTlfno
        Inherits Tipo

#Region "Variables miembro"

        Private _idPlanta As Integer = Integer.MinValue
        Private _prefijo As String = String.Empty

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Identificador de la planta a la que pertenece el alveolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property


        ''' <summary>
        ''' Prefijo del pais
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prefijo() As String
            Get
                Return _prefijo
            End Get
            Set(ByVal value As String)
                _prefijo = value
            End Set
        End Property

#End Region

#Region "Property Names"

        ''' <summary>
        ''' Clase para definir los nombres de las propiedades de la clase
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows Class PropertyNames
            Private Const _ID As String = "ID"
            Private Const _NOMBRE As String = "NOMBRE"

            Public Shared ReadOnly Property ID() As String
                Get
                    Return _ID
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRE() As String
                Get
                    Return _NOMBRE
                End Get
            End Property

        End Class

#End Region

    End Class

End Namespace