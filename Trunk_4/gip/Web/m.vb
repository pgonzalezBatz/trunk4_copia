Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.CompilerServices
Public Class proveedor
    Public Property id As Nullable(Of Integer)
    <Display(name:="Nombre")> <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")>
    <StringLength(50, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
    Public Property nombre As String
    <StringLength(35, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")> _
    Public Property direccion As String
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")> _
     <StringLength(15, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
    <RegularExpression("^([a-dA-DF-Zf-z\d]|[eE][a-rt-zA-RT-Z\d])[a-zA-Z\d]*", ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="RegularExpression1")>
    Public Property cif As String
    <StringLength(20, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")> _
    Public Property telefono As String
    <StringLength(20, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")> _
    Public Property fax As String
    <StringLength(6, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
    Public Property codpro As String
    <HiddenInput(DisplayValue:=False)> _
    Public Property fechaAlta As Nullable(Of DateTime)
    Public Property fechaBaja As Nullable(Of DateTime)
    <StringLength(10, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")> _
    Public Property codigoPostal As String
    <StringLength(35, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
    Public Property localidad As String
    <StringLength(35, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
    Public Property provincia As String
    <StringLength(35, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")> _
    Public Property contacto As String
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")> _
    Public Property pais As Integer
    Public Property nombrePais As String
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")> _
    Public Property fPago As Integer
    Public Property nombreFPago As String
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")> _
    Public Property idPlanta As Integer
    <DataType(DataType.EmailAddress)>
    <EmailAddress(ErrorMessage:="Email no válido")>
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")>
    Public Property email As String
    <DataType(DataType.EmailAddress)>
    <EmailAddress(ErrorMessage:="Email no válido")>
    Public Property email2 As String
    <DataType(DataType.EmailAddress)>
    <EmailAddress(ErrorMessage:="Email no válido")>
    <StringLength(100, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
    Public Property EmailFacturacion As String
    Public Property moneda As Integer
    <StringLength(1, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")> _
    Public Property porteTroq As String
    <StringLength(3, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")> _
    Public Property tipoProveedorSis As String
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")> _
    Public Property codigoIva As Integer
    Public Property numeroAbreviado As String
    Public Property comentarios As String
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")>
    <DataType(DataType.EmailAddress)>
    <EmailAddress(ErrorMessage:="Email no válido")>
    Public Property nombreUsuario As String
    Public Property nombreCreador As String
    Public Property apellido1Creador As String
    Public Property descMoneda As String
    Private razonSozial_ As String
    <StringLength(50, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
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
Public Class Moneda
    Public Property Codmon As Integer
    Public Property Desmon As String
    <StringLength(3, ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="StringLength")>
    Public Property Currency As String
End Class
Public Class FormaPago
    Public Property Codpag As Integer
    Public Property DesPag As String
    Public Property Brain As String
    Public Property Forpago As String
    Public Property Codterpago As String
    Public Property Coddiaspago As String
End Class
Public Class Pais
    Public Property Codpai As Integer
    Public Property Nompai As String
    <StringLength(2)>
    Public Property Code As String
    Public Property Pbc As Integer
    <StringLength(3)>
    Public Property Code3 As String
End Class
Public Class TelefonoDirecto
    Public Property IdEmpresa As String
    <Required>
    <MaxLength(4)>
    Public Property Numero As String
    Public Property Empresa As String
    <Required>
    Public Property NumeroProveedor As String
End Class
Public Class EmpresaCorporativa
    Public Property Id As Integer
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")>
    Public Property Cif As String
    <Required(ErrorMessageResourceType:=GetType(Resources.proba), ErrorMessageResourceName:="required")>
    Public Property Nombre As String
    Public Property Localidad As String
    Public Property Provincia As String
End Class
Public Enum roles
    normal = 1                              ' todos los usuarios
    homologaciones = 2                      ' "rolehomologaciones"
    editar = 4                              ' usuarios de igorre con recurso "recurso"
    cambiarCIF = 8                          ' "rolecambiarcif"
    AdministrarPotencialesYCapacidades = 16 ' "rolepotenciales"
    telefonosdirectos = 32                  ' "telefonosdirectos"
    editarSabPro = 64                       ' "rolesabprov"
    editarRecursos = 128                    ' "editarRecursos"    
    SinUsuarioSAB = 512
End Enum
Public Class VMRecursoEdit
    Public Property Grupo As Integer
    Public Property NombreGrupo As String
    Public Property URL As String
    Public Property Seleccionado As Boolean
    Public Property Area As String

    Public Function IsGrupoSabProveedores() As Boolean
        Return Grupo = ConfigurationManager.AppSettings("sabprovgrupo")
    End Function

End Class
Public Class VMAreaRecursoEdit
    Public Property NombreArea As String
    Public Property ListOfRecurso As IEnumerable(Of VMRecursoEdit)
End Class
Public Class VMRecursosUsuarioEmpresaYSAB
    Public Property UsuarioEmpresa As String
    Public Property usuarioSabProveedores As String
    Public Property ListOfUsuarioEmpresa As IEnumerable(Of VMAreaRecursoEdit)
    Public Property ListOfUsuarioSABProveedor As IEnumerable(Of VMAreaRecursoEdit)
End Class

Public Class Capacidad
    Public Property CapId As String
    Public Property Nombre As String
    Public Property Obsoleto As Boolean
    Public Property Orden As Integer
End Class