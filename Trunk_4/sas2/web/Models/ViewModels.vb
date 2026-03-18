Imports ExpressiveAnnotations.Attributes


Public Class VMPuntoViaje
    <RequiredIf("IdHelbide == null")>
    Public Property IdEmpresa As Integer?
    Public Property TxtIdEmpresa As String
    <RequiredIf("IdEmpresa == null")>
    Public Property IdHelbide As Integer?
    Public Property txtIdHelbide As String
    Public Property NoEmpresa As String
End Class
Public Class VMVectorViaje
    Public Property PuntoOrigen As VMPuntoViaje
    Public Property PuntoDestino As VMPuntoViaje
End Class
Public Class VMRecogidaCabecera
    Public Property id As Integer?
    <Required()>
    Public Property Fecha As DateTime
    <Required()>
    Public Property VectorRecogida As VMVectorViaje
    Public Property Observaciones As String
    Public Property idSab As Integer
End Class
Public Class VMRecogidaLinea
    <Required()>
    Public Property Numord As Integer?
    <Required()>
    Public Property Numope As Integer?
    <Required()>
    Public Property Peso As Decimal?
    <Required()>
    Public Property ZonaEntrega As String
    Public Property ListOfNumope As IEnumerable(Of Mvc.SelectListItem)
    Public Property ListOfZonaEntrega As IEnumerable(Of Mvc.SelectListItem)
End Class
Public Class VMRecogidaFinal
    Inherits VMRecogidaCabecera

    Public Property Seleccionado As Boolean?
    Public Property Linea As VMRecogidaLinea
    Public Property LstLinea As List(Of VMRecogidaLinea)
End Class
Public Class VMMovimientoStep1
    <Required()>
    Public Property Fecha As DateTime
    <Required()>
    Public Property VectorMovimiento As VMVectorViaje
    <Required()>
    Public Property Numord As Integer?
    <Required()>
    Public Property Numope As Integer?

    Public Property ListOfNumope As IEnumerable(Of Mvc.SelectListItem)

    Public Property IdNegocio As Integer

    Public Property Negocio As String
End Class
Public Class VMMovimientoStep2
    Inherits VMMovimientoStep1
    Public Property ListOfMarca As ICollection(Of Movimiento)
End Class
Public Class VMMovimientoFinal
    Inherits VMMovimientoStep1

    Public Property SelectedForGrouping As Boolean?
    Public Property Creador As String

    Public Sub New(step2 As VMMovimientoStep1)
        Me.Fecha = step2.Fecha
        Me.Numord = step2.Numord
        Me.Numope = step2.Numope
        Me.VectorMovimiento = step2.VectorMovimiento
        Me.ListOfNumope = step2.ListOfNumope
    End Sub
    Public Sub New()

    End Sub

    Public Function HasOneSelectedElement(ListOfMarca As IEnumerable(Of VMMarca)) As Boolean
        Return ListOfMarca.Count > 0 AndAlso ListOfMarca.Any(Function(o) o.Seleccionado)
    End Function
    Public Function CalculatePeso() As Decimal
        Return ListOfMarca.Sum(Function(o) o.Peso)
    End Function
    Public Function CalculateCantidad() As Decimal
        Return ListOfMarca.Sum(Function(o) o.Cantidad)
    End Function

    <AssertThat("HasOneSelectedElement(ListOfMarca)", ErrorMessage:="Necesario seleccionar algun elemento")>
    Public Property ListOfMarca As ICollection(Of VMMarca)
End Class
Public Class VMMarca

    <Required>
    Public Property Seleccionado As Boolean?

    Public Property Id As Integer
    <RequiredIf("Seleccionado != null && Seleccionado == true ", ErrorMessage:="Necesario seleccionar marca")>
    Public Property Marca As String
    <RequiredIf("Seleccionado != null && Seleccionado == true ", ErrorMessage:="Necesario introducir cantidad")>
    Public Property Cantidad As Nullable(Of Integer)
    <RequiredIf("Seleccionado != null && Seleccionado == true ", ErrorMessage:="Necesario introducir peso")>
    Public Property Peso As Nullable(Of Decimal)
    Public Property Diametro As Nullable(Of Decimal)
    Public Property Largo As Nullable(Of Decimal)
    Public Property Ancho As Nullable(Of Decimal)
    Public Property Alto As Nullable(Of Decimal)
    Public Property Observacion As String
    Public Property Articulo As String
    Public Property Material As String
    Public Property salida As String
End Class

    <Obsolete>
    Public Class VMMovimiento
    Public Property Id As Integer?
    Public Property FechaEntrega As Date
    Public Property Origen As String
    Public Property Destino As String
    <Required()>
    Public Property Numord As Integer
    <Required()>
    Public Property Numope As Integer
    <Required()>
    Public Property Marca As String
    <Required()>
    Public Property Peso As Nullable(Of Decimal)
    Public Property Diametro As Nullable(Of Decimal)
    Public Property Largo As Nullable(Of Decimal)
    Public Property Ancho As Nullable(Of Decimal)
    Public Property Alto As Nullable(Of Decimal)
    <Required()>
    Public Property Cantidad As Nullable(Of Integer)
    Public Property Observacion As String
    Public Property Articulo As String
    Public Property Material As String
    Public Property NombreSab As String
End Class
Public Class VMMovimientoProveedorFecha
    Public Property FechaEntrega As Date
    Public Property Destino As String
    Public Property Origen As String
    Public Property LstMovimiento As IEnumerable(Of VMMovimiento)
    Public Function CalculatePeso() As Decimal
        Return LstMovimiento.Sum(Function(o) o.Peso)
    End Function
    Public Function CalculateCantidad() As Decimal
        Return LstMovimiento.Sum(Function(o) o.Cantidad)
    End Function
End Class
Public Class VMMovimientoSinAsignar
    Public Property LstMovimientoProveedorFecha As IEnumerable(Of VMMovimientoFinal)
End Class
<Obsolete>
Public Class VMMovimientoRecogida
    Public Property LstRecogida As IEnumerable(Of VMRecogidaFinal)
    Public Property LstMovimientoProveedorFecha As IEnumerable(Of VMMovimientoFinal)
End Class
Public Class VMAgruparMovimiento
    Public Property LstMovimientoProveedorFecha As IEnumerable(Of VMMovimientoFinal)
    <Required()>
    Public Property Peso As Decimal?
End Class
Public Class VMRecogidaViaje
    Public Property LstRecogida As IEnumerable(Of VMRecogidaFinal)
    <Required()>
    Public Property Transportista As Integer?
    Public Property LstTransportista As IEnumerable(Of Mvc.SelectListItem)
    Public Property Matricula1 As String
    Public Property Matricula2 As String
End Class
Public Class VMAgrupacion
    Public Property Id As Integer
    Public Property Peso As Decimal
    Public Property IdParent As Integer?
    Public Property ListOfMovimiento As IEnumerable(Of VMMovimientoFinal)
    Public Property Children As IEnumerable(Of VMAgrupacion)
    Public Property Seleccionado As Boolean?
End Class
Public Class VMAgrupacionSinAsignar
    Public Property ListOfAgrupacion As IEnumerable(Of VMAgrupacion)
End Class
Public Class VMCreateAlbaran
    Public Property VectorViaje As VMVectorViaje
    Public Property Observaciones As String
    Public Property IdAlbaranExistente As Integer?
    Public Property LstBulto As VMAgrupacionSinAsignar
End Class