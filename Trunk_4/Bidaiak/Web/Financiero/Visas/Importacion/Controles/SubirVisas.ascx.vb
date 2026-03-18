Imports System.IO

Public Class SubirVisas
    Inherits System.Web.UI.UserControl

#Region "Eventos"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal nombreFichero As String, ByVal fichero As Byte())
    Private itzultzaileWeb As New Itzultzaile

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelInfo2)
            itzultzaileWeb.Itzuli(labelInfo3) : itzultzaileWeb.Itzuli(imgSubir)
            itzultzaileWeb.Itzuli(labelTamMax)
        End If
    End Sub

#End Region

#Region "Borrar temporal"

    ''' <summary>
    ''' Borra la tabla de temporales
    ''' </summary>
    ''' <param name="idPlanta">Id de la planta</param>
    Public Function BorrarTemporalesVisa(ByVal idPlanta As Integer) As Boolean
        Try
            PageBase.log.Info("IMPORT_VISAS: Se va a procedeer a borrar las tablas temporales")
            Dim visasBLL As New BidaiakLib.BLL.VisasBLL            
            visasBLL.DeleteMovVisasTmp(idPlanta)
            PageBase.log.Info("IMPORT_VISAS: Tabla de movimientos de visas temporales borrada")
            Dim cuentaContBLL As New BidaiakLib.BLL.DepartamentosBLL
            cuentaContBLL.DeleteAsientosContVisasTmp(idPlanta)
            PageBase.log.Info("IMPORT_VISAS: Tabla de asientos contables temporales borrada")
            Return True
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Error al borrar las tablas temporales de visa"))
            Return False
        End Try
    End Function

#End Region

#Region "Subir"

    ''' <summary>
    ''' Sube un fichero al servidor
    ''' Se podrian subir xls y xlsx pero falta por instalar el proveedor en los servidores asi que de momento solo se permite xls
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgSubir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSubir.Click
        Try
            If fuFichero.HasFile Then
                Dim extension As String = fuFichero.FileName.Substring(fuFichero.FileName.LastIndexOf(".") + 1).ToLower
                If (extension = "txt") Then
                    Dim tamañoMax As String = 10
                    PageBase.log.Info("IMPORT_VISAS:Se va a subir el fichero de visas")
                    If (fuFichero.PostedFile.ContentLength < (tamañoMax * 1000000)) Then 'Se comprueba primero que el tamaño del fichero a subir, no exceda el limite
                        PageBase.log.Info("IMPORT_VISAS:Fichero de visas subido al servidor")
                        RaiseEvent Finalizado(fuFichero.FileName, fuFichero.FileBytes)
                    Else 'Se ha pasado de tamaño      
                        Dim smsError As String = itzultzaileWeb.Itzuli("tamañoMaximoFicheroSuperado")
                        smsError &= "(" & tamañoMax & " MB)"
                        Session("TamañoFicheroMaxExcedido") = smsError
                        PageBase.log.Warn("IMPORTACION_VISAS:Tamaño del fichero de visas excedido (" & fuFichero.PostedFile.ContentLength & ")")
                        RaiseEvent Advertencia(smsError)
                    End If
                Else
                    PageBase.log.Warn("IMPORT_VISAS:Se ha intentado subir un fichero con una extension invalida (" & extension & ")")
                    RaiseEvent Advertencia(itzultzaileWeb.Itzuli("Extension invalida. Solo se pueden subir ficheros con extension .txt"))
                End If
            Else
                RaiseEvent Advertencia(itzultzaileWeb.Itzuli("Debe seleccionar el fichero a subir"))
            End If
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("errorSubirDocumento"))
        Catch ex As Exception
            PageBase.log.Error("Error al subir un documento", ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("errorSubirDocumento"))
        End Try
    End Sub

#End Region

End Class