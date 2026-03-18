Imports System.IO

Public Class SubirFactura
    Inherits UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal nombreFichero As String, ByVal fichero As Byte())
    Private itzultzaileWeb As New Itzultzaile

#End Region

#Region "Traduccion"

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelInfo2)
            itzultzaileWeb.Itzuli(labelInfo3) : itzultzaileWeb.Itzuli(imgSubir)
        End If
    End Sub

#End Region

#Region "Borrar temporal"

    ''' <summary>
    ''' Borra la tabla de temporales
    ''' </summary>
    ''' <param name="idPlanta">Id de la planta</param>
    Public Function BorrarTmpFacturasEroski(ByVal idPlanta As Integer) As Boolean
        Try
            PageBase.log.Info("FACT_AGEN: Se va a procedeer a borrar las tablas temporales de la planta " & idPlanta)
            Dim solAgenciaBLL As New BidaiakLib.BLL.SolicAgenciasBLL            
            solAgenciaBLL.DeleteFacturaEroskiTmp(idPlanta)
            PageBase.log.Info("FACT_AGEN: Tabla de facturas de eroski temporales borrada")
            Dim cuentaContBLL As New BidaiakLib.BLL.DepartamentosBLL
            cuentaContBLL.DeleteAsientosContEroskiTmp(idPlanta)
            PageBase.log.Info("FACT_AGEN: Tabla de asientos contables temporales borrada")
            Return True
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
            Return False
        End Try
    End Function

#End Region

#Region "Subir"

    ''' <summary>
    ''' Sube un fichero al servidor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgSubir_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgSubir.Click
        Try
            If fuFichero.HasFile Then
                Dim directorio As String = ConfigurationManager.AppSettings("Documentos").ToString & "\"
                Dim tamañoMax As String = 10
                PageBase.log.Info("FACT_AGEN (PASO 1): Se va a subir el fichero de facturas/albaranes")
                If (fuFichero.PostedFile.ContentLength < (tamañoMax * 1000000)) Then 'Se comprueba primero que el tamaño del fichero a subir, no exceda el limite
                    Try 'Se comprueba que no exista el fichero
                        Dim fileInfo As New IO.FileInfo(fuFichero.FileName)
                        If (fileInfo.Extension.ToLower <> ".xlsx") Then
                            Throw New BidaiakLib.BatzException("Extension no valida. Tiene que subir un fichero xlsx", Nothing)
                        Else
                            PageBase.log.Info("FACT_AGEN: Fichero de factura de eroski subido al servidor")
                            RaiseEvent Finalizado(fuFichero.FileName, fuFichero.FileBytes)
                        End If
                    Catch batzEx As BidaiakLib.BatzException
                        Throw batzEx
                    Catch ex As Exception
                        Throw New BidaiakLib.BatzException("Error al crear el directorio. El fichero a subir tiene que tener la estructura de xx_año_mes_dia.xlsx", ex)
                    End Try
                Else 'Se ha pasado de tamaño      
                    Dim smsError As String = itzultzaileWeb.Itzuli("tamañoMaximoFicheroSuperado")
                    smsError &= "(" & tamañoMax & " MB)"
                    Session("TamañoFicheroMaxExcedido") = smsError
                    PageBase.log.Warn("FACT_AGEN: Tamaño del fichero de factura de agencia excedido (" & fuFichero.PostedFile.ContentLength & ")")
                    RaiseEvent Advertencia("smsError")
                End If
            Else
                RaiseEvent Advertencia(itzultzaileWeb.Itzuli("Debe seleccionar el fichero a subir"))
            End If
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
        Catch ex As Exception
            PageBase.log.Error("Error al subir un documento", ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("errorSubirDocumento"))
        End Try
    End Sub

#End Region

End Class