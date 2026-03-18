Imports System
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.IO
Imports Ionic.Zip

Public Class DescargarAyudaVisual
    Inherits System.Web.UI.Page

    Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")

#Region "Handlers"

    Protected Sub DescargarArchivo_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Try
            Dim registro As ELL.AyudaVisual

            If (Context.Request.QueryString.AllKeys.Contains("idRegistro")) Then
                Dim idRegistro As String = Context.Request.QueryString("idRegistro")

                If (Integer.TryParse(Context.Request.QueryString("idRegistro"), idRegistro)) Then
                    registro = GetAyudaVisual(idRegistro)
                    If (registro IsNot Nothing) Then
                        If (registro.ARCHIVO IsNot Nothing) Then
                            HttpContext.Current.Response.Clear()
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & registro.NOMBRE)
                            HttpContext.Current.Response.ContentType = "application/octet-stream"
                            HttpContext.Current.Response.BinaryWrite(registro.ARCHIVO.ToArray)
                        End If
                    End If
                End If


            End If
        Catch ex As Exception
        Finally
            HttpContext.Current.Response.End()
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene la imagen de la característica
    ''' </summary>
    ''' <param name="idRegistro"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAyudaVisual(ByVal idRegistro As Integer) As ELL.AyudaVisual
        Dim consultasBLL As New BLL.ConsultasBLL
        Return consultasBLL.cargarAyudaVisual(idRegistro)
    End Function

    'Protected Sub DescargarArchivo_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    '    Try

    '        'Dim rutaFicheros As String = String.Empty
    '        'Dim numFicheros As Integer = 0
    '        Dim registro As List(Of KaPlanLib.Registro.Archivos)

    '        If (Context.Request.QueryString.AllKeys.Contains("idRegistro")) Then
    '            Dim idRegistro As String = Context.Request.QueryString("idRegistro")

    '            If (Integer.TryParse(Context.Request.QueryString("idRegistro"), idRegistro)) Then
    '                registro = GetAyudaVisual(idRegistro)
    '                If (registro.Count <> 0) Then
    '                    If (registro(0).Archivo IsNot Nothing) Then
    '                        HttpContext.Current.Response.Clear()
    '                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & registro(0).Nombre)
    '                        HttpContext.Current.Response.ContentType = "application/octet-stream"
    '                        HttpContext.Current.Response.BinaryWrite(registro(0).Archivo.ToArray)
    '                    End If
    '                End If
    '            End If


    '        End If
    '    Catch ex As Exception
    '    Finally
    '        HttpContext.Current.Response.End()
    '    End Try
    'End Sub

    '''' <summary>
    '''' Obtiene la imagen de la caracerística
    '''' </summary>
    '''' <param name="idRegistro"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function GetAyudaVisual(ByVal idRegistro As Integer) As List(Of KaPlanLib.Registro.Archivos)
    '    Dim registro As List(Of KaPlanLib.Registro.Archivos)
    '    Try
    '        Dim BBDD As New KaPlanLib.DAL.ELL

    '        registro = (From reg As KaPlanLib.Registro.Archivos In BBDD.Archivos _
    '                    Join ArchivosFab In BBDD.Archivos_Caracteristicas_Plan_FAB On reg.ID Equals ArchivosFab.Id_Archivo _
    '                    Join caracPlan In BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION On ArchivosFab.Id_Carac_Plan Equals caracPlan.ID_REGISTRO
    '                    Where caracPlan.ID_REGISTRO = idRegistro Select reg).ToList()
    '    Catch ex As Exception
    '        Throw New BatzException("Error al cargar el registro con Id " + idRegistro.ToString() + " ", ex)
    '    End Try
    '    Return registro
    'End Function

#End Region

End Class