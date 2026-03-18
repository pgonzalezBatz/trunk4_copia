Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class CostCarriersWS
    Inherits System.Web.Services.WebService

    Shared log As log4net.ILog = log4net.LogManager.GetLogger("root")

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub New()
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    <WebMethod()>
    Public Function HelloWorld() As String
        log.Info("Llamada a HelloWorld()")

        Return "Kaixo Mundua!!!!"
    End Function

    ''' <summary>
    ''' Crea un portador de coste
    ''' </summary>
    ''' <param name="ccmetadata">Metadatos del portador de coste</param>
    ''' <param name="empresasProductivas">Empresas productivas asociadas al portador de coste. ID de la tabla SAB.PLANTAS</param>
    ''' <param name="ccMetadaYear">Datos anuales asociados al portador de coste</param>
    ''' <returns></returns>
    <WebMethod()>
    Public Function CrearPortadorCoste(ByVal ccmetadata As ELL.BRAIN.CCMetadata, ByVal empresasProductivas As List(Of ELL.BRAIN.CCProductionPlant), ByVal ccMetadaYear As ELL.BRAIN.CCMetadataYear) As Boolean
        log.Info(String.Format("Llamada a CrearPortadorCoste para la empresa {0}, planta {1} y portador {2}", ccmetadata.Empresa, ccmetadata.Planta, ccmetadata.CodigoPortador))

        Dim ret As Boolean = False
        Try
            If (String.IsNullOrEmpty(ccmetadata.Lantegi)) Then
                Dim cc As ELL.BRAIN.CostCarrier = BLL.BRAIN.CostCarriersBLL.Obtener(ccmetadata.CodigoPortador, ccmetadata.Empresa)
                If (cc IsNot Nothing) Then
                    ccmetadata.Lantegi = cc.Lantegi
                End If
            End If

            If (Not String.IsNullOrEmpty(ccmetadata.BudgetCode)) Then
                Dim cp As ELL.CodigoPrespuesto = BLL.CodigosPresupuestoBLL.Obtener(ccmetadata.BudgetCode)
                If (cp IsNot Nothing) Then
                    ccmetadata.DescBudgetCode = cp.DescripcionCompleta
                End If
            End If

            Dim costCarrier As ELL.BRAIN.CostCarrier = BLL.BRAIN.CostCarriersBLL.Obtener(ccmetadata.CodigoPortador, ccmetadata.Empresa)
            If (costCarrier IsNot Nothing) Then
                ccmetadata.FechaIni = costCarrier.FechaApertura
                ccMetadaYear.Anyo = costCarrier.FechaApertura.Year
            End If

            BLL.BRAIN.CCMetadataBLL.Guardar(ccmetadata, empresasProductivas)
            BLL.BRAIN.CCMetadataYearBLL.Guardar(ccMetadaYear)
            ret = True

            log.Info(String.Format("Portador de coste creado correctamente para la empresa {0}, planta {1} y portador {2}", ccmetadata.Empresa, ccmetadata.Planta, ccmetadata.CodigoPortador))
        Catch ex As Exception
            log.Error(String.Format("Error al CrearPortadorCoste para la empresa {0}, planta {1} y portador {2}", ccmetadata.Empresa, ccmetadata.Planta, ccmetadata.CodigoPortador), ex)
        End Try

        Return ret
    End Function

    ''' <summary>
    ''' Actualiza los datos de metadata year 
    ''' </summary>
    ''' <param name="ccMetadaYear"></param>
    ''' <returns></returns>
    <WebMethod()>
    Public Function ActulizarMetadataYear(ByVal ccMetadaYear As ELL.BRAIN.CCMetadataYear) As Boolean
        log.Info(String.Format("Llamada a ActulizarMetadataYear para la empresa {0}, planta {1}, portador {2} y año {3}", ccMetadaYear.Empresa, ccMetadaYear.Planta, ccMetadaYear.CodigoPortador, ccMetadaYear.Anyo))

        Dim ret As Boolean = False
        Try
            ' Obtenemos el metadatayear porque puede ser que no todos los datos a actualizar vengan informados
            Dim ccMetadataYearAux As ELL.BRAIN.CCMetadataYear = BLL.BRAIN.CCMetadataYearBLL.Obtener(ccMetadaYear.Empresa, ccMetadaYear.Planta, ccMetadaYear.CodigoPortador, ccMetadaYear.Anyo)
            If (ccMetadataYearAux IsNot Nothing) Then
                If (ccMetadaYear.ImporteVentaCliente <> Decimal.Zero) Then
                    ccMetadataYearAux.ImporteVentaCliente = ccMetadaYear.ImporteVentaCliente
                End If

                If (ccMetadaYear.PresupBonosPersona <> Decimal.Zero) Then
                    ccMetadataYearAux.PresupBonosPersona = ccMetadaYear.PresupBonosPersona
                End If

                If (ccMetadaYear.PresupFacturas <> Decimal.Zero) Then
                    ccMetadataYearAux.PresupFacturas = ccMetadaYear.PresupFacturas
                End If

                If (ccMetadaYear.PresupViajes <> Decimal.Zero) Then
                    ccMetadataYearAux.PresupViajes = ccMetadaYear.PresupViajes
                End If

                BLL.BRAIN.CCMetadataYearBLL.Guardar(ccMetadataYearAux)
                ret = True

                log.Info(String.Format("Metadata year actualizado correctametne para para la empresa {0}, planta {1}, portador {2} y año {3}", ccMetadaYear.Empresa, ccMetadaYear.Planta, ccMetadaYear.CodigoPortador, ccMetadaYear.Anyo))
            Else
                log.Info(String.Format("No se encuentra Metadata year para para la empresa {0}, planta {1}, portador {2} y año {3}", ccMetadaYear.Empresa, ccMetadaYear.Planta, ccMetadaYear.CodigoPortador, ccMetadaYear.Anyo))
            End If
        Catch ex As Exception
            log.Error(String.Format("Error al ActulizarMetadataYear para la empresa {0}, planta {1}, portador {2} y año {3}", ccMetadaYear.Empresa, ccMetadaYear.Planta, ccMetadaYear.CodigoPortador, ccMetadaYear.Anyo), ex)
        End Try

        Return ret
    End Function

    ''' <summary>
    ''' Actualiza los datos el responsable del metadata
    ''' </summary>
    ''' <param name="idSabResponsable"></param>
    ''' <param name="portadorCoste"></param>
    ''' <returns></returns>
    <WebMethod()>
    Public Function ActualizarResponsable(ByVal idSabResponsable As Integer, ByVal portadorCoste As String) As Boolean
        log.Info(String.Format("Llamada a ActualizarResponsable para la responsable {0} y portador {1}", idSabResponsable, portadorCoste))

        Dim ret As Boolean = False
        Try
            BLL.BRAIN.CCMetadataBLL.ActualizarResponsable(idSabResponsable, portadorCoste)
            ret = True

            log.Info(String.Format("ActualizarResponsable correctamente para la responsable {0} y portador {1}", idSabResponsable, portadorCoste))
        Catch ex As Exception
            log.Error(String.Format("Error al ActualizarResponsable para la responsable {0} y portador {1}", idSabResponsable, portadorCoste), ex)
        End Try

        Return ret
    End Function

End Class