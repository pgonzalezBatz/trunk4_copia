Namespace ELL

    ''' <summary>
    ''' Esta clase es una copia de la del GIP pero por motivos de dependencias no es factible
    ''' </summary>
    Public Class proveedor
        Public Property id As Integer?
        Public Property nombre As String
        Public Property direccion As String
        Public Property cif As String
        Public Property telefono As String
        Public Property fax As String
        Public Property codpro As String
        Public Property fechaAlta As DateTime?
        Public Property fechaBaja As DateTime?
        Public Property codigoPostal As String
        Public Property localidad As String
        Public Property provincia As String
        Public Property contacto As String
        Public Property pais As Integer
        Public Property nombrePais As String
        Public Property fPago As Integer
        Public Property nombreFPago As String
        Public Property idPlanta As Integer
        Public Property email As String
        Public Property email2 As String
        Public Property EmailFacturacion As String
        Public Property moneda As Integer
        Public Property porteTroq As String
        Public Property tipoProveedorSis As String
        Public Property codigoIva As Integer
        Public Property numeroAbreviado As String
        Public Property comentarios As String
        Public Property nombreUsuario As String
        Public Property nombreCreador As String
        Public Property apellido1Creador As String
        Public Property descMoneda As String

        Private razonSozial_ As String

        Public Property RazonSocial As String
            Get
                If String.IsNullOrEmpty(razonSozial_) Then
                    Return nombre
                Else
                    Return razonSozial_
                End If
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then
                    razonSozial_ = nombre
                Else
                    razonSozial_ = value
                End If
            End Set
        End Property

        Public Property Homologado As String

        Public Property Clasificacion As String

        Public Property Notificado As Boolean

    End Class

End Namespace