Namespace ELL

    Public Class TipoLinea
        Inherits TipoCultura

#Region "Variables miembro"

        Private _requiereAlveolo As Boolean = False
        Private _idTipoExtension As Integer = Integer.MinValue

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Indica si el tipo de linea, requiere seleccionar un alveolo al crear un telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RequiereAlveolo() As Boolean
            Get
                Return _requiereAlveolo
            End Get
            Set(ByVal value As Boolean)
                _requiereAlveolo = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del tipo de extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipoExtension() As Integer
            Get
                Return _idTipoExtension
            End Get
            Set(ByVal value As Integer)
                _idTipoExtension = value
            End Set
        End Property

#End Region

#Region "Estructura mantenimiento"

        ''' <summary>
        ''' Indica el tipo de accion en un mantenimiento
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows Enum Accion As Integer
            insertar = 0
            modificar = 1
        End Enum

        ''' <summary>
        ''' Estructura para almacenar el objeto y la accion a realizar
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows Structure Mantenimiento
            Dim acc As Accion
            Dim objecto As Object
        End Structure

#End Region

    End Class

End Namespace