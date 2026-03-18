Public Class LanzarJob
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.Title("Ejecutar Job")
        If Not Page.IsPostBack Then
            Dim jobName As String = String.Empty
            jobNames.Items.Clear()
            jobNames.Items.Add(New ListItem("Seleciona un job...", 0))
            If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                jobName = "SIS_Costes_reales"
                jobNames.Items.Add(New ListItem("Procesar cierre", "SIS_Costes_reales"))
            Else
                jobName = "SIS_Costes_reales_DESA"
                jobNames.Items.Add(New ListItem("Procesar cierre TEST", "SIS_Costes_reales_DESA"))
            End If
            Dim lEjecuciones As List(Of Object) = ComponentBLL.ObtenerEjecucionesJob(jobName)
            If (lEjecuciones IsNot Nothing AndAlso lEjecuciones.Any) Then
                Dim ejecucion As Object = lEjecuciones.First
                If (CDate(ejecucion.FechaFin) = DateTime.MinValue) Then
                    pnlConEjecucion.Visible = True : pnlSinEjecucion.Visible = False
                    labelInfo.Text = Master.Traducir("El job todavia esta en ejecucion. Hora de inicio [FECHA]").ToString.Replace("[FECHA]", CDate(ejecucion.FechaInicio).ToString("F"))
                Else
                    pnlConEjecucion.Visible = False : pnlSinEjecucion.Visible = True
                    If (ejecucion.Resultado = 1) Then
                        lbResult.Text = Master.Traducir("El job se ejecuto por ultima vez el [FECHA] con exito").ToString.Replace("[FECHA]", CDate(ejecucion.FechaFin).ToString("F"))
                        lbResult.Style.Add("color", "green")
                    Else
                        lbResult.Text = Master.Traducir("El job se ejecuto por ultima vez el [FECHA] con error").ToString.Replace("[FECHA]", CDate(ejecucion.FechaFin).ToString("F"))
                        lbResult.Style.Add("color", "red")
                    End If
                End If
            Else
                pnlConEjecucion.Visible = False : pnlSinEjecucion.Visible = True
            End If
        End If
    End Sub

    Protected Sub btnEjecutar_Click(sender As Object, e As EventArgs) Handles btnEjecutar.Click
        If jobNames.SelectedValue.Equals("0") Then
            lbResult.Text = "No se ha ejecutado ningún job."
            Return
        End If
        Dim prJobDAL As New PruebaJobDAL
        prJobDAL.lanzarJob(jobNames.SelectedValue)
        lbResult.Text = "Job '" & jobNames.SelectedValue & "' ejecutado."
    End Sub

End Class