Namespace ELL

    Public Class Documento

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Tipo documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipoDocumento() As Integer

        ''' <summary>
        ''' Nombre fichero
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreFichero() As String

        ''' <summary>
        ''' Id padre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPadre() As Integer

        ''' <summary>
        ''' Fecha alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaAlta() As DateTime

        ''' <summary>
        ''' Id usuario alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioAlta() As Integer

        ''' <summary>
        ''' Nombre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

        ''' <summary>
        ''' Apellido 1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Apellido1() As String

        ''' <summary>
        ''' Apellido 2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Apellido2() As String

        ''' <summary>
        ''' Ruta fichero completa
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property RutaFicheroCompleta As String
            Get
                Dim rootDocumentos As String = Configuration.ConfigurationManager.AppSettings("rootDocumentos")
                Dim extensionDocumento As String = System.IO.Path.GetExtension(NombreFichero)

                Return IO.Path.Combine(rootDocumentos, Id & extensionDocumento)
            End Get
        End Property

        ''' <summary>
        ''' Usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property NombreUsuario() As String
            Get
                Return String.Format("{0} {1} {2}", Nombre, Apellido1, Apellido2).ToUpper()
            End Get
        End Property

        ''' <summary>
        ''' Revisión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Revision() As Integer = Integer.MinValue

#End Region

    End Class

End Namespace
