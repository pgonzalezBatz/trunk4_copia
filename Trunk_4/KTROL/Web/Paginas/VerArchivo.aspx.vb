Imports System
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.IO

Public Class VerArchivo
    Inherits System.Web.UI.Page

#Region "Handlers"
    Protected Sub VerArchivo_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Try
            Dim hojaInstruccion As New KaPlanLib.Registro.HOJA_DE_INSTRUCCIONES_FABRICACION
            Dim ruta As String = String.Empty

            If (Context.Request.QueryString.AllKeys.Contains("codOperacion")) Then
                Dim codOperacion As String = Context.Request.QueryString("codOperacion")

                Dim conexion As New KaPlanLib.DAL.ELL

                hojaInstruccion = (From reg As KaPlanLib.Registro.HOJA_DE_INSTRUCCIONES_FABRICACION In conexion.HOJA_DE_INSTRUCCIONES_FABRICACION _
                         Where reg.CODIGO = codOperacion Select reg).SingleOrDefault()
                If (hojaInstruccion IsNot Nothing) Then
                    If Not (String.IsNullOrEmpty(hojaInstruccion.DIBUJO)) Then
                        Dim rutaHI As String = Configuration.ConfigurationManager.AppSettings.Get("RutaImagenHojaInstruccion")
                        Dim rutaDocumento = rutaHI + hojaInstruccion.DIBUJO
                        Dim path As String = rutaDocumento 'get file object as FileInfo        
                        Dim file As System.IO.FileInfo = New System.IO.FileInfo(path) '-- if the file exists on the server 
                        If file.Exists Then 'set appropriate headers            
                            HttpContext.Current.Response.Clear()
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
                            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString())
                            HttpContext.Current.Response.ContentType = "application/octet-stream"
                            HttpContext.Current.Response.WriteFile(file.FullName)
                            'HttpContext.Current.Response.End() 'if file does not exist        
                        Else
                            'Response.Write("This file does not exist.")
                        End If 'nothing in the URL as HTTP GET
                    End If
                End If
            End If
        Catch ex As Exception
            'Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")
            'log.Error("Error al descargar el fichero", ex)
            'Dim i As Integer = 1

        Finally
            HttpContext.Current.Response.End()
        End Try
    End Sub
#End Region

End Class