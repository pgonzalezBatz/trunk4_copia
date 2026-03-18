Namespace ELL

    Public Class Reto

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Id planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Código
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Codigo() As String

        ''' <summary>
        ''' Título
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Titulo() As String

        ''' <summary>
        ''' Descripcion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

        ''' <summary>
        ''' Id documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdDocumento() As Integer

        ''' <summary>
        ''' Fecha alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaAlta() As DateTime

        ''' <summary>
        ''' Fecha baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaBaja() As DateTime

        ''' <summary>
        ''' Id usuario alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioAlta() As Integer

        ''' <summary>
        ''' Id usuario baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioBaja() As Integer

        ''' <summary>
        ''' Id usuario baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipoDocumento() As Integer

        ''' <summary>
        ''' Id usuario baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreFichero() As String

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String

        ''' <summary>
        ''' Ruta fichero completa
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property RutaFicheroCompleta As String
            Get
                Dim rootDocumentos As String = Configuration.ConfigurationManager.AppSettings("rootDocumentos")
                Dim extensionDocumento As String = System.IO.Path.GetExtension(NombreFichero)

                Return IO.Path.Combine(rootDocumentos, IdDocumento & extensionDocumento)
            End Get
        End Property

#End Region

    End Class

End Namespace
