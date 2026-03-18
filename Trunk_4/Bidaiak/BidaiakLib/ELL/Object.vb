Namespace ELL

    Public Class ConvenioCategoria

        Public Property Id As Integer
        Public Property IdConvenio As String
        Public Property Convenio As String
        Public Property IdCategoria As String
        Public Property Categoria As String
        Public Property RecibeVisasAntic As Boolean = False
        Public Property TipoLiquidacion As TipoLiq = -1
        Public Property MostrarEmpresaFacturacion As Boolean = False

        ''' <summary>
        ''' Tipo de liquidacion
        ''' </summary>        
        Public Enum TipoLiq As Integer
            Metalico = 0
            Factura = 1
        End Enum

    End Class

    Public Class SaldoCaja

        Public Property Id As Integer
        Public Property Fecha As DateTime
        Public Property Operacion As EOperacion = -1
        Public Property Cantidad As Decimal = 0
        Public Property IdMoneda As Integer = Integer.MinValue
        Public Property IdUsuario As Integer = Integer.MinValue
        Public Property IdPlanta As Integer = Integer.MinValue
        Public Property SaldoRestante As Decimal = 0
        Public Property Comentario As String

        Public Enum EOperacion As Integer
            Ingreso = 0
            Actualizacion = 1
            Extraccion = 2
            Entrego_Anticipo = 3
            Devolucion_Anticipo = 4
            Eliminar_Devolucion = 5
        End Enum

    End Class

    Public Class AsientoContableEroskiTmp
        Public Property IdSab As Integer
        Public Property numTrabajador As Integer
        Public Property IdPlanta As Integer
        Public Property FechaInsercion As DateTime
        Public Property Nombre As String
        Public Property CodigoDepart As String
        Public Property Departamento As String
        Public Property BaseExe_0 As Decimal
        Public Property BaseIR_8 As Decimal
        Public Property BaseIG_18 As Decimal
        Public Property UnidadOrganizativa As String
        Public Property Cuota_0 As Decimal
        Public Property Cuota_8 As Decimal
        Public Property Cuota_18 As Decimal
        Public Property RegEsp As Decimal
        Public Property Cuenta_0 As Integer
        Public Property Cuenta_8 As Integer
        Public Property Cuenta_18 As Integer
        Public Property Producto As String
        Public Property Factura As String
    End Class

    Public Class FakturaEroski
        Public Property Bono As String
        Public Property Factura As String
        Public Property Albaran As String
        Public Property FechaServicio As Date
        Public Property Dias As Integer
        Public Property Producto As String
        Public Property Destino As String
        Public Property Proveedor As String
        Public Property Persona As String
        Public Property BaseIG As Decimal
        Public Property CuotaG As Decimal
        Public Property BaseIR As Decimal
        Public Property CuotaR As Decimal
        Public Property BaseExe As Decimal
        Public Property RegEsp As Decimal
        Public Property CuotaRE As Decimal
        Public Property Importe As Decimal
        Public Property Nivel1 As String
        Public Property Nivel2 As String
        Public Property Nivel3 As String
        Public Property Nivel4 As String
        Public Property IdUser As Integer
        Public Property FechaInsercion As DateTime
        Public Property IdPlanta As Integer
        Public Property IdViajes As String
        Public Property IdSabOrganizador As Integer
        Public Property Tasas As Decimal
        Public Property FechaFactura As Date
        Public Property IdImportacion As Integer
    End Class

    Public Class AsientoContableCab
        Public Property Id As Integer
        Public Property FechaContabilidad As Date
        Public Property FechaEmision As Date
        Public Property FechaVencimiento As Date
        Public Property FechaFactura As Date
        Public Property FechaInsercion As DateTime
        Public Property Factura As String
        Public Property Importe As Decimal
        Public Property IVA As Decimal
        Public Property ImporteTotal As Decimal
        Public Property IdPlanta As Integer
        Public Property IdImportacion As Integer
        Public Property DocumentoBatz As String
        Public Property Lineas As List(Of AsientoContableCab)
        Public Class Linea
            Public Property Id As Integer
            Public Property IdCab As Integer
            Public Property Linea As Integer
            Public Property Cuenta As String
            Public Property TipoIVA As Integer
            Public Property Importe As Decimal
            Public Property IVA As Decimal
            Public Property CodigoDepartamento As String
        End Class
    End Class

    Public Class Parametro
        Public Property PrecioKm As Decimal
        Public Property CodProvAgencia As String
        Public Property IdPlanta As Integer
        Public Property DiasCaducidadViaje As Integer
        Public Property DiasSolicitarAnticipo As Integer
        Public Property IdConceptoKm As Integer
    End Class

    Public Class CuentaContable
        Public Property NombrePlanta As String
        Public Property IdPlantaCuenta As Integer
        Public Property IdPlantaGestion As Integer
        Public Property Cuenta18 As Integer
        Public Property Cuenta8 As Integer
        Public Property Cuenta0 As Integer

    End Class

    Public Class CuentaContrapartida
        Public Property CtaContrapartida As Integer = Integer.MinValue
        Public Property CtaCuota As Integer = Integer.MinValue
        Public Property IdPlantaCuenta As Integer
        Public Property IdPlantaGestion As Integer
        Public Property NombrePlanta As String

    End Class

End Namespace
