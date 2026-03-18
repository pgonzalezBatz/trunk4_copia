Imports System.ComponentModel
Imports ExpressiveAnnotations.Attributes
Public Class MovimientoBase
    '<Range(1, 3, ErrorMessage:="Negocio obligatorio")>
    <Required()>
    <DisplayName("Negocio")>
    Public Property IdNegocio As Integer
    Public Property Negocio As String
    <Required()>
    Public Property Numord As Integer
    <Required()>
    Public Property Numope As Integer
    <Required()>
    <RegularExpression("\d{1,5}")>
    Public Property CodPro As String
    <Required()>
    Public Property FechaEntrega As Nullable(Of DateTime)
    Public Property IdSab As Integer
    Public Property NombreSab As String
    Public Property NombreProveedor As String
    Public Property cifProveedor As String
    <Required()>
    Public Property EmpresaSalida As Integer
    Public Property NombreEmpresaSalida As String
End Class
Public Class Movimiento
    Inherits MovimientoBase
    Public Property Id As Integer
    <Required()>
    Public Property Marca As String
    Public Property CantidadXbat As Integer
    <Required()>
    Public Property Cantidad As Nullable(Of Integer)
    <Required()>
    Public Property Peso As Nullable(Of Decimal)
    Public Property Diametro As Nullable(Of Decimal)
    Public Property Largo As Nullable(Of Decimal)
    Public Property Ancho As Nullable(Of Decimal)
    Public Property Alto As Nullable(Of Decimal)
    Public Property Observacion As String
    Public Property Articulo As String
    Public Property Material As String
    Public Property Otros As Object
    Public Property PoblacionOrigen As String
    Public Property PoblacionDestino As String
End Class
<Obsolete>
Public Class PreMovimiento
    Public Property Id As Integer
    Public Property Marca As String
    Public Property Numord As Integer
    Public Property Numope As Integer
    Public Property Peso As Nullable(Of Decimal)
    Public Property Observacion As String
    Public Property IdEmpresa As Nullable(Of Integer)
    Public Property NombreEmpresa As String
    Public Property Cantidad As Integer
    Public Property Canrec As Integer
    Public Property Canped As Integer
    Public Property Otros As String
End Class
<Obsolete>
Public Class Agrupacion
    Public Property Id As Integer
    Public Property Peso As Decimal
    Public Property idParent As Integer?
    Public Property ListOfMovimiento As IEnumerable(Of Movimiento)
    Public Property children As IEnumerable(Of Agrupacion)
    Public Property IdNegocio As Integer
    Public Property Negocio As String
End Class
Public Class Albaran
    Public Property Id As Integer
    Public Property ListOfAgrupacion As IEnumerable(Of Agrupacion)
    Public Property Observaciones As String
    Public Property IdHelbide As Nullable(Of Integer)
    Public Property IdNegocio As Integer
    Public Property Negocio As String
End Class
Public Class Recogida
    Public Property Id As Integer
    <Required()> _
    <Range(1, 100000, ErrorMessage:="Es necesario seleccionar empresa de recogida")> _
    Public Property IdEmpresaRecogida As Integer
    Public Property nombreEmpresaRecogida As String
    <Required()> _
    Public Property IdEmpresaEntrega As Integer
    Public Property nombreEmpresaEntrega As String
    Public Property puerta As Integer?
    <Required()>
    Public Property Fecha As DateTime?
    <Required()> _
    Public Property IdSab As Integer
    Public Property nombreSab As String
    Public Property Observacion As String
    Public Property ListOfOp As IEnumerable(Of OfOp)
    Public Property observacionesdireccion As String
    <Required()>
    Public Property IdNegocio As Integer
    Public Property Negocio As String
End Class
Public Class OfOp
    <RequiredIf("Numope != null || Peso != null", ErrorMessage:="Es necesario indicar la OF")>
    Public Property Numord As Integer?
    <RequiredIf("Numord != null || Peso != null", ErrorMessage:="Es necesario indicar la OP")>
    Public Property Numope As Integer?
    <RequiredIf("Numord != null || Numope != null", ErrorMessage:="Es necesario indicar el peso")>
    Public Property Peso As Decimal?
    '<RequiredIf("Numord != null || Numope != null || Peso != null", ErrorMessage:="Es necesario indicar el lugar de entrega")>
    Public Property puerta As Integer?
End Class
Public Class Viaje
    Public Property Id As Integer
    Public Property ListOfAlbaran As List(Of Albaran)
    Public Property IdTransportista As String
    Public Property Salida As Nullable(Of DateTime)
    Public Property Matricula1 As String
    Public Property Matricula2 As String
    Public Property cifTransportista As String
    Public Property NombreTransportista As String
    Public Property ListOfRecogida As List(Of Recogida)
    Public Property Precio As Decimal
    Public Property comentarioAlmacen As String
    Public Property comentarioProveedor As String
    Public Property deCamino As Nullable(Of Date)
    Public Property kilometros As Nullable(Of Decimal)
    Public Property nPuntosEspera As Nullable(Of Integer)
    Public Property esperaSuperiorHora As Nullable(Of Decimal)
    Public Property festivos As Nullable(Of Decimal)
    Public Property fechaCreacion As DateTime
    Public Property distancia As Decimal?
End Class
Public Enum Role
    creacion = 1
    envio = 2
    compras = 4
    premovimientos = 8
    taxi = 16
    busquedas = 32
    revision = 64
End Enum
