Namespace ELL


    ''' <summary>
    ''' Clase definida para representar a todos los tipos que tengan esta estructura
    ''' </summary>    
    Public Class TipoCultura

#Region "Enumeracion"
        Public Enum Idioma As Integer            
            Euskera = 0
            Español = 1
            Ingles = 2
            Checo = 3            
        End Enum
#End Region

#Region "Variables shared"

        Public Shared CULTURA_ESPAÑOL As String = "es-ES"
        Public Shared CULTURA_EUSKARA As String = "eu-ES"
        Public Shared CULTURA_INGLES As String = "en-GB"
        Public Shared CULTURA_CHECO As String = "ch-CH"

#End Region

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _descripcion As String = String.Empty
        Private _obsoleto As Boolean = False
        Private _cultura As String = String.Empty

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Identificador unico del tipo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property


        ''' <summary>
        ''' Nombre del tipo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property


        ''' <summary>
        ''' Descripcion del mismo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property


        ''' <summary>
        ''' Indica si el tipo esta obsoleto o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Obsoleto() As Boolean
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Boolean)
                _obsoleto = value
            End Set
        End Property



        ''' <summary>
        ''' Cultura del termino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cultura() As String
            Get
                Return _cultura
            End Get
            Set(ByVal value As String)
                _cultura = value
            End Set
        End Property


        Public Shared Function ContactMatch(ByVal item As TipoCultura, ByVal argument As TipoCultura) As Boolean
            Return item.Cultura = argument.Cultura
        End Function

#End Region

#Region "Property Names"

        ''' <summary>
        ''' Clase para definir los nombres de las propiedades de la clase
        ''' </summary>
        ''' <remarks></remarks>
        Public Class PropertyNames
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

#Region "Estructura mantenimiento"

        ''' <summary>
        ''' Indica el tipo de accion en un mantenimiento
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum Accion As Integer
            insertar = 0
            modificar = 1
        End Enum

        ''' <summary>
        ''' Estructura para almacenar el objeto y la accion a realizar
        ''' </summary>
        ''' <remarks></remarks>
        Public Structure Mantenimiento
            Dim acc As Accion
            Dim objecto As Object
        End Structure

#End Region

    End Class

End Namespace