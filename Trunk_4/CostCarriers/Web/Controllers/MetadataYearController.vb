Imports System.Globalization
Imports System.Web.Mvc

Namespace Controllers
    Public Class MetadataYearController
        Inherits BaseController

#Region "Propiedades"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides ReadOnly Property RolesAcceso As List(Of ELL.Rol.TipoRol)
            Get
                Dim roles As New List(Of ELL.Rol.TipoRol)
                roles.Add(ELL.Rol.TipoRol.Financiero)
                Return roles
            End Get
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CostCarriersMetadata"></param>
        ''' <returns></returns>
        Function Index(ByVal CostCarriersMetadata As String) As ActionResult
            ' El valor del combo de la búsqueda son tres valores separados por guiones: empresa, planta y código portador
            Dim valores() As String
            Dim empresa As String = Nothing
            Dim planta As String = Nothing
            Dim codigoPortador As String = Nothing

            If (Not String.IsNullOrEmpty(CostCarriersMetadata)) Then
                valores = CostCarriersMetadata.Split("-")
                empresa = valores(0)
                planta = valores(1)
                codigoPortador = valores(2)
            End If

            CargarComboCostCarriers(CostCarriersMetadata)
            CargarCostCarriersYears(empresa, planta, codigoPortador)

            ViewData("CostCarrierMetadata") = CostCarriersMetadata

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="anyo"></param>
        ''' <param name="codigoPortador"></param>
        ''' <returns></returns>
        Function Editar(ByVal empresa As String, ByVal planta As String, ByVal anyo As Integer, ByVal codigoPortador As String) As ActionResult
            ViewData("CostCarrier") = String.Format("{0}-{1}-{2}", empresa, planta, codigoPortador)

            Dim metadata As ELL.BRAIN.CCMetadataYear = BLL.BRAIN.CCMetadataYearBLL.Obtener(empresa, planta, codigoPortador, anyo)

            CargarComboMonedas(metadata.CodigoMoneda)
            CargarCostCarriersYear(empresa, planta, anyo, codigoPortador)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CostCarriersMetadata"></param>
        Private Sub CargarComboCostCarriers(Optional ByVal CostCarriersMetadata As String = Nothing)
            Dim costCarriers As List(Of ELL.BRAIN.CCMetadata) = BLL.BRAIN.CCMetadataBLL.CargarListado()
            Dim costCarriersLI As List(Of Mvc.SelectListItem) = costCarriers.Select(Function(f) New Mvc.SelectListItem With {.Text = String.Format("{0} - ({1})", f.CodigoPortador, f.NombreEmpresa), .Value = String.Format("{0}-{1}-{2}", f.Empresa, f.Planta, f.CodigoPortador)}).OrderBy(Function(f) f.Text).ToList()

            costCarriersLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Seleccione uno")), .Value = String.Empty})

            If (Not String.IsNullOrEmpty(CostCarriersMetadata) AndAlso costCarriersLI.Exists(Function(f) f.Value = CostCarriersMetadata)) Then
                costCarriersLI.First(Function(f) f.Value = CostCarriersMetadata).Selected = True
            End If

            ViewData("CostCarriersMetadata") = costCarriersLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function CargarMonedas() As JsonResult
            Dim comons As List(Of ELL.XBAT.Comon) = BLL.XBAT.ComonBLL.CargarListado()
            Return Json(comons, JsonRequestBehavior.AllowGet)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigoMoneda"></param>
        Private Sub CargarComboMonedas(Optional ByVal codigoMoneda As String = Nothing)
            Dim comons As List(Of ELL.XBAT.Comon) = BLL.XBAT.ComonBLL.CargarListado()
            Dim comonsLI As List(Of Mvc.SelectListItem) = comons.Select(Function(f) New Mvc.SelectListItem With {.Text = f.CodmonBRAIN, .Value = f.Codmon}).OrderBy(Function(f) f.Text).ToList()

            comonsLI.Insert(0, New SelectListItem With {.Text = String.Format("<{0}>", Utils.Traducir("Seleccione uno")), .Value = String.Empty})

            If (Not String.IsNullOrEmpty(codigoMoneda) AndAlso comonsLI.Exists(Function(f) f.Value = codigoMoneda)) Then
                comonsLI.First(Function(f) f.Value = codigoMoneda).Selected = True
            End If

            ViewData("Monedas") = comonsLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaBrain"></param>
        Private Function CargarMonedaDefecto(ByVal idPlantaBrain As String) As Integer
            Dim codigoMoneda As Integer = 90
            Dim moneda As ELL.XBAT.Comon = BLL.XBAT.ComonBLL.ObtenerPorIdPlantaBrain(idPlantaBrain)

            If (moneda IsNot Nothing) Then
                codigoMoneda = moneda.Codmon
            End If

            Return codigoMoneda
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        Private Sub CargarCostCarriersYears(Optional ByVal empresa As String = Nothing, Optional ByVal planta As String = Nothing, Optional ByVal codigoPortador As String = Nothing)
            Dim costCarriersYears As List(Of ELL.BRAIN.CCMetadataYear) = BLL.BRAIN.CCMetadataYearBLL.CargarListado(empresa, planta, codigoPortador)

            ViewData("CostCarriersYears") = costCarriersYears
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="anyo"></param>
        ''' <param name="codigoPortador"></param>
        Private Sub CargarCostCarriersYear(ByVal empresa As String, ByVal planta As String, ByVal anyo As Integer, ByVal codigoPortador As String)
            Dim costCarriersYear As ELL.BRAIN.CCMetadataYear = BLL.BRAIN.CCMetadataYearBLL.Obtener(empresa, planta, codigoPortador, anyo)

            ViewData("CCMetadataYear") = costCarriersYear
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarComboAnyos()
            Dim anyosLI As New List(Of Mvc.SelectListItem)

            For anyo = DateTime.Today.Year To DateTime.Today.Year - 20 Step -1
                anyosLI.Add(New SelectListItem With {.Text = anyo, .Value = anyo})
            Next

            ViewData("Anyos") = anyosLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CostCarrierMetadata"></param>
        ''' <returns></returns>
        Function Agregar(ByVal CostCarrierMetadata As String) As ActionResult
            ViewData("CostCarrier") = CostCarrierMetadata

            CargarComboCostCarriers(CostCarrierMetadata)
            CargarComboAnyos()
            CargarComboMonedas(CargarMonedaDefecto(CostCarrierMetadata.Split("-")(0)))

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        <HttpPost>
        Function Agregar(ByVal CostCarriersMetadata As String, ByVal Anyos As Integer, ByVal Monedas As Integer, ByVal hfMonedas As String, ByVal presupBonosPersona As String,
                         ByVal presupFacturas As String, ByVal presupViajes As String, ByVal importeVentaCliente As String) As ActionResult
            Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")

            ' Comprobamos si ya existe ese registro
            Dim valores() As String = CostCarriersMetadata.Split("-")
            If (BLL.BRAIN.CCMetadataYearBLL.Obtener(valores(0), valores(1), valores(2), Anyos) IsNot Nothing) Then
                MensajeInfo(Utils.Traducir("Ya existen datos para ese año"))
                Return Agregar(CostCarriersMetadata)
            End If

            Dim presupBonosPersonaDec As Decimal = Decimal.MinValue
            Dim presupFacturasDec As Decimal = Decimal.MinValue
            Dim presupViajesDec As Decimal = Decimal.MinValue
            Dim importeVentaClienteDec As Decimal = Decimal.MinValue
            Dim esNegativo As Boolean = False

#Region "Valores decimales"

            esNegativo = presupBonosPersona.StartsWith("-")
            If (esNegativo) Then
                presupBonosPersona = presupBonosPersona.Replace("-", String.Empty)
            End If
            presupBonosPersona = presupBonosPersona.Replace(".", String.Empty)

            Decimal.TryParse(presupBonosPersona, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, presupBonosPersonaDec)

            If (Not String.IsNullOrEmpty(presupBonosPersona) AndAlso presupBonosPersona <> "0" AndAlso presupBonosPersona <> "0,00" AndAlso presupBonosPersonaDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Presup. bonos persona"), Utils.Traducir("Formato incorrecto")))
                Return Agregar(CostCarriersMetadata)
            End If

            If (esNegativo) Then
                presupBonosPersonaDec = presupBonosPersonaDec * -1
            End If

            '**************

            esNegativo = presupFacturas.StartsWith("-")
            If (esNegativo) Then
                presupFacturas = presupFacturas.Replace("-", String.Empty)
            End If
            presupFacturas = presupFacturas.Replace(".", String.Empty)

            Decimal.TryParse(presupFacturas, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, presupFacturasDec)

            If (Not String.IsNullOrEmpty(presupFacturas) AndAlso presupFacturas <> "0" AndAlso presupFacturas <> "0,00" AndAlso presupFacturasDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Presup. facturas"), Utils.Traducir("Formato incorrecto")))
                Return Agregar(CostCarriersMetadata)
            End If

            If (esNegativo) Then
                presupFacturasDec = presupFacturasDec * -1
            End If

            '**************

            esNegativo = presupViajes.StartsWith("-")
            If (esNegativo) Then
                presupViajes = presupViajes.Replace("-", String.Empty)
            End If
            presupViajes = presupViajes.Replace(".", String.Empty)

            Decimal.TryParse(presupViajes, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, presupViajesDec)

            If (Not String.IsNullOrEmpty(presupViajes) AndAlso presupViajes <> "0" AndAlso presupViajes <> "0,00" AndAlso presupViajesDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Presup. viajes"), Utils.Traducir("Formato incorrecto")))
                Return Agregar(CostCarriersMetadata)
            End If

            If (esNegativo) Then
                presupViajesDec = presupViajesDec * -1
            End If

            '**************

            If (Not String.IsNullOrEmpty(importeVentaCliente)) Then
                esNegativo = importeVentaCliente.StartsWith("-")
                If (esNegativo) Then
                    importeVentaCliente = importeVentaCliente.Replace("-", String.Empty)
                End If
                importeVentaCliente = importeVentaCliente.Replace(".", String.Empty)

                Decimal.TryParse(importeVentaCliente, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, importeVentaClienteDec)

                If (Not String.IsNullOrEmpty(importeVentaCliente) AndAlso importeVentaCliente <> "0" AndAlso importeVentaCliente <> "0,00" AndAlso importeVentaClienteDec = Decimal.Zero) Then
                    MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Importe de venta al cliente"), Utils.Traducir("Formato incorrecto")))
                    Return Agregar(CostCarriersMetadata)
                End If

                If (esNegativo) Then
                    importeVentaClienteDec = importeVentaClienteDec * -1
                End If
            End If

#End Region

            Dim ccMetadataYear As New ELL.BRAIN.CCMetadataYear()
            ccMetadataYear.Empresa = valores(0)
            ccMetadataYear.Planta = valores(1)
            ccMetadataYear.Anyo = Anyos
            ccMetadataYear.CodigoPortador = valores(2)
            ccMetadataYear.CodigoMoneda = Monedas
            ccMetadataYear.Moneda = hfMonedas
            ccMetadataYear.PresupBonosPersona = presupBonosPersonaDec
            ccMetadataYear.PresupFacturas = presupFacturasDec
            ccMetadataYear.PresupViajes = presupViajesDec
            ccMetadataYear.ImporteVentaCliente = importeVentaClienteDec

            Try
                BLL.BRAIN.CCMetadataYearBLL.Guardar(ccMetadataYear)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Return RedirectToAction("Editar", New With {.empresa = valores(0), .planta = valores(1), .anyo = Anyos, .codigoPortador = valores(2)})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Agregar(CostCarriersMetadata)
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        <HttpPost>
        Function Editar(ByVal hfCodigoPortador As String, ByVal hfAnyo As String, ByVal Monedas As Integer, ByVal hfMonedas As String, ByVal presupBonosPersona As String,
                         ByVal presupFacturas As String, ByVal presupViajes As String, ByVal importeVentaCliente As String) As ActionResult
            Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")

            ' Comprobamos si ya existe ese registro
            Dim valores() As String = hfCodigoPortador.Split("-")
            Dim ccMetadataYear As ELL.BRAIN.CCMetadataYear = BLL.BRAIN.CCMetadataYearBLL.Obtener(valores(0), valores(1), valores(2), hfAnyo)

            Dim presupBonosPersonaDec As Decimal = Decimal.MinValue
            Dim presupFacturasDec As Decimal = Decimal.MinValue
            Dim presupViajesDec As Decimal = Decimal.MinValue
            Dim importeVentaClienteDec As Decimal = Decimal.MinValue
            Dim esNegativo As Boolean = False

#Region "Valores decimales"
            esNegativo = presupBonosPersona.StartsWith("-")
            If (esNegativo) Then
                presupBonosPersona = presupBonosPersona.Replace("-", String.Empty)
            End If
            presupBonosPersona = presupBonosPersona.Replace(".", String.Empty)

            Decimal.TryParse(presupBonosPersona, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, presupBonosPersonaDec)

            If (Not String.IsNullOrEmpty(presupBonosPersona) AndAlso presupBonosPersona <> "0" AndAlso presupBonosPersona <> "0,00" AndAlso presupBonosPersonaDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Presup. bonos persona"), Utils.Traducir("Formato incorrecto")))
                Return Editar(valores(0), valores(1), hfAnyo, valores(2))
            End If

            If (esNegativo) Then
                presupBonosPersonaDec = presupBonosPersonaDec * -1
            End If

            '**************

            esNegativo = presupFacturas.StartsWith("-")
            If (esNegativo) Then
                presupFacturas = presupFacturas.Replace("-", String.Empty)
            End If
            presupFacturas = presupFacturas.Replace(".", String.Empty)

            Decimal.TryParse(presupFacturas, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, presupFacturasDec)

            If (Not String.IsNullOrEmpty(presupFacturas) AndAlso presupFacturas <> "0" AndAlso presupFacturas <> "0,00" AndAlso presupFacturasDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Presup. facturas"), Utils.Traducir("Formato incorrecto")))
                Return Editar(valores(0), valores(1), hfAnyo, valores(2))
            End If

            If (esNegativo) Then
                presupFacturasDec = presupFacturasDec * -1
            End If

            '**************

            esNegativo = presupViajes.StartsWith("-")
            If (esNegativo) Then
                presupViajes = presupViajes.Replace("-", String.Empty)
            End If
            presupViajes = presupViajes.Replace(".", String.Empty)

            Decimal.TryParse(presupViajes, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, presupViajesDec)

            If (Not String.IsNullOrEmpty(presupViajes) AndAlso presupViajes <> "0" AndAlso presupViajes <> "0,00" AndAlso presupViajesDec = Decimal.Zero) Then
                MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Presup. viajes"), Utils.Traducir("Formato incorrecto")))
                Return Editar(valores(0), valores(1), hfAnyo, valores(2))
            End If

            If (esNegativo) Then
                presupViajesDec = presupViajesDec * -1
            End If

            '**************

            If (Not String.IsNullOrEmpty(importeVentaCliente)) Then
                esNegativo = importeVentaCliente.StartsWith("-")
                If (esNegativo) Then
                    importeVentaCliente = importeVentaCliente.Replace("-", String.Empty)
                End If
                importeVentaCliente = importeVentaCliente.Replace(".", String.Empty)

                Decimal.TryParse(importeVentaCliente, NumberStyles.AllowDecimalPoint + NumberStyles.AllowThousands, culturaEsES, importeVentaClienteDec)

                If (Not String.IsNullOrEmpty(importeVentaCliente) AndAlso importeVentaCliente <> "0" AndAlso importeVentaCliente <> "0,00" AndAlso importeVentaClienteDec = Decimal.Zero) Then
                    MensajeAlerta(String.Format("{0}. {1}", Utils.Traducir("Importe de venta al cliente"), Utils.Traducir("Formato incorrecto")))
                    Return Editar(valores(0), valores(1), hfAnyo, valores(2))
                End If

                If (esNegativo) Then
                    importeVentaClienteDec = importeVentaClienteDec * -1
                End If
            End If

#End Region

            ccMetadataYear.Empresa = valores(0)
            ccMetadataYear.Planta = valores(1)
            ccMetadataYear.Anyo = hfAnyo
            ccMetadataYear.CodigoPortador = valores(2)
            ccMetadataYear.CodigoMoneda = Monedas
            ccMetadataYear.Moneda = hfMonedas
            ccMetadataYear.PresupBonosPersona = presupBonosPersonaDec
            ccMetadataYear.PresupFacturas = presupFacturasDec
            ccMetadataYear.PresupViajes = presupViajesDec
            ccMetadataYear.ImporteVentaCliente = importeVentaClienteDec

            Try
                BLL.BRAIN.CCMetadataYearBLL.Guardar(ccMetadataYear)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
                Return RedirectToAction("Editar", New With {.empresa = valores(0), .planta = valores(1), .anyo = hfAnyo, .codigoPortador = valores(2)})
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Editar(valores(0), valores(1), hfAnyo, valores(2))
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigo"></param>
        ''' <param name="anyo"></param>
        ''' <returns></returns>
        Function Eliminar(ByVal empresa As String, ByVal planta As String, ByVal codigo As String, ByVal anyo As Integer) As ActionResult
            Try
                BLL.BRAIN.CCMetadataYearBLL.Eliminar(empresa, planta, codigo, anyo)
                MensajeInfo(Utils.Traducir("Metadatos por año eliminada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Error al eliminar metadatos por año"))
                log.Error("Error al eliminar metadatos por año", ex)
            End Try

            Return RedirectToAction("Index", New With {.CostCarriersMetadata = String.Format("{0}-{1}-{2}", empresa, planta, codigo)})
        End Function

#End Region

    End Class
End Namespace