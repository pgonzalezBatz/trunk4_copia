Imports TelefoniaLib
Imports System.IO

Partial Public Class CargaFacturasMov
    Inherits PageBase

    Private SumaTotal As Decimal = 0

#Region "Page Load"

    ''' <summary>
    ''' Comprobacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If (Not Page.IsPostBack) Then
            Master.SetTitle = "Proceso de carga de facturas moviles"
            Dim ficheroTemp As New FileInfo(ConfigurationManager.AppSettings("FacturasTelefonia").ToString & "\Temp\Factura.mdb")
            If (ficheroTemp.Exists()) Then
                lblNoSubir.Visible = True
                btnSubir.Enabled = False
                btnQuitarBloqueo.Visible = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelSelFichero) : itzultzaileWeb.Itzuli(labelTamano) : itzultzaileWeb.Itzuli(btnSubir)
        itzultzaileWeb.Itzuli(labelFich) : itzultzaileWeb.Itzuli(labelSubidoOK) : itzultzaileWeb.Itzuli(labelResumen)
        itzultzaileWeb.Itzuli(btnImportar) : itzultzaileWeb.Itzuli(lblMensa) : itzultzaileWeb.Itzuli(labelFFactura)
        itzultzaileWeb.Itzuli(labelTotalFactura) : itzultzaileWeb.Itzuli(labelTotalFacturado) : itzultzaileWeb.Itzuli(labelRegInsert)
        itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnRefrescar) : itzultzaileWeb.Itzuli(labelRegInsert)
        itzultzaileWeb.Itzuli(lblNoSubir) : itzultzaileWeb.Itzuli(btnQuitarBloqueo) : itzultzaileWeb.Itzuli(lblResulRefresco)
        itzultzaileWeb.Itzuli(labelAsociarCIF)
    End Sub

    ''' <summary>
    ''' Se informa el log
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub CargaFacturasMov_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            log.Info("Se va a proceder a subir una factura")
        End If
    End Sub

#End Region

#Region "Subir Fichero"

    ''' <summary>
    ''' Sube un fichero al servidor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSubir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubir.Click
        Try            
            If fuFichero.HasFile Then                
				Dim directorio As String = ConfigurationManager.AppSettings("FacturasTelefonia").ToString & "\"
                Dim fichero As String
                Dim tamañoMax As String = 20
                pnlImportar.Visible = False : pnlNoImportar.Visible = False

                'Se comprueba primero que el tamaño del fichero a subir, no exceda el limite
                If (fuFichero.PostedFile.ContentLength < (tamañoMax * 2000000)) Then
                    Try
                        If Not (Directory.Exists(directorio)) Then
                            Directory.CreateDirectory(directorio)
                        End If
                        'Se comprueba que no exista el fichero
                        fichero = directorio & fuFichero.FileName
                        If (File.Exists(fichero)) Then
                            log.Warn("El fichero " & fichero & " ya se ha importado")
                            Master.MensajeAdvertencia = "ficheroYaImportado"
                            Exit Sub
                        End If
                    Catch ex As Exception
                        Throw New BatzException("errCrearDirectorio", ex)
                    End Try
                    Dim dirTemp As String = directorio & "\Temp\Factura.mdb"  'Se sube a un directorio temporal con un nombre para que utilice el ODBC de windows. Esto se debe a que no funcionaba con el ADO.NET
                    fuFichero.SaveAs(dirTemp)

                    Dim factComp As New BLL.FacturasComponent
                    Dim lFacturas As List(Of ELL.Factura) = factComp.LeerCabecerasFactura(dirTemp)
                    If (lFacturas IsNot Nothing) Then lFacturas.Sort(Function(o1 As ELL.Factura, o2 As ELL.Factura) o1.CifEmpresa < o2.CifEmpresa)
                    pnlImportar.Visible = True
                    gvFacturas.DataSource = lFacturas
                    gvFacturas.DataBind()
                    mvCarga.ActiveViewIndex = 1
                    lblFichero.Text = fuFichero.FileName.ToString()
                    btnSubir.CommandArgument = fichero                    
                Else 'Se ha pasado de tamaño      
                    Dim smsError As String = itzultzaileWeb.Itzuli("tamañoMaximoFicheroSuperado")
                    smsError &= "(" & tamañoMax & " MB)"
                    log.Warn("Se ha excedido el tamaño maximo de fichero (" & tamañoMax & ")")
                    Session("TamañoFicheroMaxExcedido") = smsError
                    Master.MensajeAdvertencia = smsError
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim be As New BatzException("errorSubirDocumento", ex)
            Master.MensajeError = be.Termino
        End Try
    End Sub

#End Region

#Region "Importar Datos"

    ''' <summary>
    ''' Importa los datos de las facturas moviles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnImportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Dim fichero As String = btnSubir.CommandArgument
            Dim ficheroTemp As String = ConfigurationManager.AppSettings("FacturasTelefonia").ToString & "\Temp\Factura.mdb"
			Dim factComp As New BLL.FacturasComponent
			log.Info("Se va a proceder a importar las facturas")
            Dim lFacturas As List(Of ELL.Factura)
			Dim mensaje As String = String.Empty

            Try
                lFacturas = factComp.ImportarFacturaMoviles(ficheroTemp, Master.Ticket.IdPlantaActual)
            Catch batzEx As BatzException
                log.Error("La llamada a importar factura moviles a devuelto una batzException")
                lFacturas = Nothing
            End Try

            pnlError.Visible = False
            mvCarga.ActiveViewIndex = 2
            If (lFacturas IsNot Nothing) Then
                Try
                    log.Info("Factura con datos")
                    pnlResulRefresco.Visible = False
                    btnRefrescar.Visible = True
                    getFechaUltimoRefrescoVistaMat()
                    lblFechaFactura.Text = lFacturas.First.FechaFactura
                    lblTotalFactura.Text = lFacturas.Sum(Function(o As ELL.Factura) o.Total)
                    lblTotalFacturado.Text = lFacturas.Sum(Function(o As ELL.Factura) o.TotalPagar)
                    lblRegistrosInsertados.Text = lFacturas.Sum(Function(o As ELL.Factura) o.NumLineas)
                    mensaje = itzultzaileWeb.Itzuli("importacionDatosRealizada")
                    Master.MensajeInfo = mensaje
                    btnImportar.Enabled = False
                    lblMensa.Text = "Ejecucion finalizada con exito"
                    log.Info("Importacion realizada con exito")

                    'Se va a mover el fichero
                    Try
                        log.Info("Se va a mover la factura de Temp a documentos")
                        Dim fileInfo As New FileInfo(ficheroTemp)
                        fileInfo.MoveTo(fichero)
                        log.Info("Factura movida")
                    Catch ex As Exception
                        log.Error("No se ha podido mover la factura de Temp a documentos una vez tratada. Hagalo a mano", ex)
                    End Try
                Catch ex As Exception
                    lblMensa.Text = "La importacion se ha realizado con exito pero ha ocurrido un error al pintar los datos del resultado, por tanto, no VUELVA A SUBIR EL FICHERO"
                    log.Error("La importacion se ha realizado con exito pero ha ocurrido un error al pintar los datos del resultado", ex)
                End Try
            Else 'Ha ocurrido algun error
                pnlError.Visible = True
                lblMensa.Text = "Ejecucion finalizada con error"
                Master.MensajeError = "Error en la importacion del fichero"
                log.Warn("Se le va a proponer que lo ejecute en otro servidor")

                'Al dar error, se propone ejecutarlo en el otro servidor
                Dim label As New Label
                label.Style.Add("font-size", "15px") : label.Style.Add("color", "#0000FF")
                label.Text = itzultzaileWeb.Itzuli("Se ha producido un error. Intentar de nuevo la ejecucion en el servidor") & " "
                label.Text &= If(Environment.MachineName.ToLower = "atxerre", "Elorribakar", "Atxerre") & "<br /><br />"
                Dim hlink As New HyperLink
                hlink.Style.Add("font-size", "14px") : hlink.Target = "_blank"
                hlink.Text = itzultzaileWeb.Itzuli("Ir a") & " " & Environment.MachineName
                hlink.NavigateUrl = Request.Url.Scheme & "://" & Environment.MachineName & ".batz.es/Telefonia/default.aspx"
                pnlError.Controls.Add(label)
                pnlError.Controls.Add(hlink)
            End If
        Catch ex As Exception
            log.Error("Error al terminar el proceso. Antes de continuar consulte con el administrador para saber si se han subido los datos", ex)            
            Master.MensajeError = "Error al terminar el proceso. Antes de continuar consulte con el administrador para saber si se han subido los datos"
        End Try
    End Sub

    ''' <summary>
    ''' Muestra la fecha de la ultima ejecucion de la vista materializada
    ''' </summary>
    Private Sub getFechaUltimoRefrescoVistaMat()
        Dim facturasBLL As New TelefoniaLib.BLL.FacturasComponent
        Dim fecha As DateTime = facturasBLL.getFechaUltimoRefrescoVistaMat()
        If (fecha <> DateTime.MinValue) Then
            lblUltimaEjecucion.Text = "(" & "Fecha de la ultimo refresco" & ":" & fecha.ToShortDateString & " - " & fecha.ToShortTimeString & ")"
        Else
            lblUltimaEjecucion.Text = "No se ha podido obtener la fecha del ultimo refresco"
        End If
    End Sub

    ''' <summary>
    ''' Elimina la factura que se ha quedado en temporal, debido a un error
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnQuitarBloqueo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuitarBloqueo.Click
        Try
            log.Info("Se va a proceder a borrar la factura que bloquea la ejecucion")
            Dim ficheroTemp As New FileInfo(ConfigurationManager.AppSettings("FacturasTelefonia").ToString & "\Temp\Factura.mdb")
            If (ficheroTemp.Exists()) Then
                ficheroTemp.Delete()
                log.Info("Factura del bloqueo borrada")
                Response.Redirect("CargaFacturasMov.aspx", False)
            Else
                Master.MensajeAdvertencia = "El fichero no existe"
            End If
        Catch ex As Exception
            log.Error("Error al intentar borrar la factura de bloqueo",ex)
            Master.MensajeError = "No se ha podido borrar la factura de la carpeta temporal"
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvFacturas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvFacturas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oFact As ELL.Factura = e.Row.DataItem
            SumaTotal += oFact.TotalPagar
            If (oFact.IdPlanta <= 0) Then
                pnlNoImportar.Visible = True
                pnlImportar.Visible = False
            End If            
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim lblTotal As Label = CType(e.Row.FindControl("lblTotal"), Label)
            lblTotal.Text = SumaTotal
        End If
    End Sub

#End Region

#Region "Refrescar Vista"

    ''' <summary>
    ''' Refresca la vista materializada para que los datos se puedan usar desde Cognos
    ''' </summary>    
    Private Sub btnRefrescar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefrescar.Click
        Dim myThread As New Threading.Thread(New Threading.ThreadStart(AddressOf EjecutarRefresco))
        log.Info("Se lanza el hilo para la ejecucion del procedimiento de refresco")
        myThread.Start()
        pnlResulRefresco.Visible = True
        btnRefrescar.Visible = False
        log.Info("Termina la funcion btnRefrescar_Click. Cuando finalice el hilo, se avisara por email")
    End Sub

    ''' <summary>
    ''' Proceso de refresco lanzado en otro hilo
    ''' </summary>    
    Private Sub EjecutarRefresco()
        Try
            log.Info("Hilo_EjecutarRefresco: Se va a proceder a refrescar la vista materializada de facturas")
            Dim facturasBLL As New TelefoniaLib.BLL.FacturasComponent
            facturasBLL.RefrescarVistaMaterializadaFacturas("telefonia@batz.es", ConfigurationManager.AppSettings("ProcesoFacturaAvisarA"), "Importacion de factura telefónica", "El proceso de importación de factura ha finalizado. Ya puede consultar los informes en Cognos")
            log.Info("Hilo_EjecutarRefresco: Vista materializada refrescada con exito")
        Catch batzEx As Sablib.BatzException
            log.Info("Hilo_EjecutarRefresco: No se ha podido refrescar la vista materializada de facturas")
        End Try
    End Sub

#End Region

End Class