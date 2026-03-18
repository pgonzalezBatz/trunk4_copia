Imports System.IO.Ports
Imports System.Timers
Imports System.Configuration
Imports System.Text.RegularExpressions

Public Class Form1

#Region "Shared variables"
    Dim serialPortName As String = ConfigurationManager.AppSettings("PortName")
    Dim sp As SerialPort
    Dim disconnectionTimer = ConfigurationManager.AppSettings("DisconnectionTimer")
    Dim timerStarted As Boolean = False
    Dim cte As New Constantes
    Public lectura1, lectura2 As String
    Dim WithEvents myDisconnectionTimer As Timers.Timer = New Timers.Timer()
    Dim CONNECTED As Boolean = False

    Public Enum Column
        _COD_ALBARAN = 1
        _NUM_PEDIDO = 2
        _LIN_PEDIDO = 3
        _COD_REFERENCIA = 4
        _MATERIAL = 5
        _OF = 6
        _OP = 7
        _MARCA = 8
        _CANTIDAD = 9
        _PRECIO_UNITARIO = 10
        _IMPORTE = 11
    End Enum
#End Region

#Region "Initialization"
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        variableInitialization()
        'If cbProviders.SelectedItem.Code.Equals("0") Then
        '    showErrorDialog("ATENCIÓN: Elige un proveedor")
        '    Panel1.Enabled = True
        '    btnChangeDefaults.Visible = False
        '    btnApply.Visible = True
        '    btnCancel.Visible = True
        'Else
        connectToScanner()
        'End If
        'checkConnection()
    End Sub

    Private Sub variableInitialization()
        Dim year = DBConnection.getYear()
        TextBox3.Text = ConfigurationManager.AppSettings("Year")
        Dim ports = SerialPort.GetPortNames()
        cbPorts.DataSource = ports
        cbPorts.SelectedIndex = ports.Count - 1
        Dim providers As New List(Of Provider)
        providers.Add(New Provider With {.Code = "7014", .Name = "ROYME"})

        cbProviders.DataSource = providers
        cbProviders.ValueMember = "Code"
        cbProviders.DisplayMember = "Name"

        Dim schemas As New List(Of String)
        schemas.Add("XBAT")
        ComboBox2.DataSource = schemas
        ComboBox2.SelectedIndex = 0
        setValueInAppConfig("Year", year)
        setValueInAppConfig("Portname", cbPorts.SelectedValue)

        Button1.Enabled = False
        Button1.Visible = False
    End Sub

#End Region

#Region "invoke UI changes"

    Private Delegate Sub AppendTextBoxDelegate(ByVal tb As TextBox, ByVal txt As String)

    Private Sub AppendTextBox(ByVal tb As TextBox, ByVal txt As String)
        If tb.InvokeRequired Then
            tb.Invoke(New AppendTextBoxDelegate(AddressOf AppendTextBox), New Object() {tb, txt})
        Else
            tb.AppendText(txt)
        End If
    End Sub

    Private Delegate Sub ChangeDataSourceDelegate(ByVal gv As DataGridView, ByVal data As List(Of Data))

    Private Sub ChangeDataSource(ByVal gv As DataGridView, ByVal data As List(Of Data))
        If gv.InvokeRequired Then
            gv.Invoke(New ChangeDataSourceDelegate(AddressOf ChangeDataSource), New Object() {gv, data})
        Else
            gv.DataSource = data
        End If
    End Sub

    Private Delegate Sub ReloadDataSourceDelegate(ByVal gv As DataGridView, ByVal data As List(Of Data))

    Private Sub ReloadDataSource(ByVal gv As DataGridView, ByVal data As List(Of Data))
        If gv.InvokeRequired Then
            gv.Invoke(New ReloadDataSourceDelegate(AddressOf ReloadDataSource), New Object() {gv, data})
        Else
            Dim bindingSource1 = New System.Windows.Forms.BindingSource With {.DataSource = gv.DataSource}
            gv.DataSource = bindingSource1
            Dim newData = createQRDataFromData(data)
            bindingSource1.Add(newData)
        End If
    End Sub

    Private Delegate Sub EnableDataGridViewDelegate(ByVal gv As DataGridView, ByVal enable As Boolean)

    Private Sub enableDataGrid(gv As DataGridView, enable As Boolean)
        If gv.InvokeRequired Then
            gv.Invoke(New EnableDataGridViewDelegate(AddressOf enableDataGrid), New Object() {gv, enable})
        Else
            gv.Enabled = enable
            gv.Visible = enable
        End If
    End Sub

    Private Delegate Sub setLabelDelegate(ByVal lbl As Label, ByVal txt As String)

    Private Sub setLabel(ByVal lbl As Label, ByVal txt As String)
        If lbl.InvokeRequired Then
            lbl.Invoke(New setLabelDelegate(AddressOf setLabel), New Object() {lbl, txt})
        Else
            lbl.Text = txt
        End If
    End Sub

    Private Delegate Sub setPictureBoxDelegate(ByVal pb As PictureBox, ByVal imagePath As String)

    Private Sub setPictureBox(ByVal pb As PictureBox, ByVal imagePath As String)
        If pb.InvokeRequired Then
            pb.Invoke(New setPictureBoxDelegate(AddressOf setPictureBox), New Object() {pb, imagePath})
        Else
            pb.Image = Image.FromFile(imagePath)
        End If
    End Sub

#End Region

#Region "Scanner communication"
    Private Sub connectToScanner()
        serialPortName = ConfigurationManager.AppSettings("PortName")
        sp = New SerialPort(serialPortName)
        sp.BaudRate = 9600
        sp.Parity = Parity.None
        sp.StopBits = StopBits.One
        sp.DataBits = 8
        sp.Handshake = Handshake.None
        sp.ReadTimeout = 5000
        sp.DtrEnable = True
        sp.RtsEnable = True
        sp.WriteTimeout = 5000
        AddHandler sp.DataReceived, AddressOf DataReceivedHandler
        Try
            sp.Open()
            CONNECTED = True
            myDisconnectionTimer.Interval = disconnectionTimer
            AddHandler myDisconnectionTimer.Elapsed, AddressOf disconnectiontimer_elapsed
            myDisconnectionTimer.Start()
        Catch ex As Exception
            CONNECTED = False
            Exit Sub
        End Try
        Me.ActiveControl = btnValidarAlbaran
    End Sub

    'Private Sub checkConnection()
    '    If CONNECTED Then
    '        setPictureBox(pbConnection, "../../Images/connected.png")
    '    Else
    '        setPictureBox(pbConnection, "../../Images/disconnected.png")
    '    End If
    'End Sub

    Public Sub DataReceivedHandler(sender As Object, e As SerialDataReceivedEventArgs) '''' para la linea MQBA0
        myDisconnectionTimer.Stop()
        sp = CType(sender, SerialPort)
        Dim indata As String = sp.ReadExisting()
        EnviarACK()

        '''' GUARDAR INDATA EN TABLA ORACLE - para comprobar que se reciben bien los datos
        Try
            Dim data = indata.Split("$")
            Dim data1 = data(0) '*** código albarán --- que no exista en 'gccabalb'
            Dim data2 = data(1) '*** número pedido --- que sí exista - en 'gclinped'
            Dim data3 = data(2) '*** línea pedido --- que sí exista - en 'gclinped'
            Dim data4 = data(3) ' código referencia - ignorar
            Dim data5 = Regex.Replace(data(4), "[^a-zA-Z0-9]", "")
            Dim data6 = data(5)  '*** of
            Dim data7 = data(6) '*** op
            'Dim data7 = "" '*** op
            Dim data8 = data(7) '*** marca
            Dim data9 = data(8) ' cantidad
            Dim data10 = data(9) + " " ' precio unitarioº
            Dim data11 = data(10) + " " ' importe
            Dim data13 = indata ''  texto completo de la lectura QR
            Dim data14 = Nothing
            Dim data15 = Nothing

            Dim numPed = data2
            Dim linPed = data3

            Dim myDbData As DbData = DBConnection.getDbData(numPed, linPed)
            If myDbData IsNot Nothing Then
                Dim status = If(Not DBConnection.checkAlbaran(data1), "OK", "ERR")
                Dim status2 = If(DBConnection.checkNumPedido(numPed), "OK", "ERR")
                Dim status3 = If(DBConnection.checkLinPedido(linPed), "OK", "ERR")
                'Dim status4 = If(Regex.Replace(data5, "[^a-zA-Z0-9]", "").ToUpper.Equals(Regex.Replace(myDbData.Material, "[^a-zA-Z0-9]", "").ToUpper), "OK", "ERR")
                Dim status6 = If(data6.Trim.Equals(myDbData.Numordf), "OK", "ERR")
                Dim status7 = If(data7.Trim.Equals(myDbData.Numope), "OK", "ERR")
                Dim status8 = If(data8.Trim.Equals(myDbData.Nummar.Trim), "OK", "ERR")
                Dim status9 = If(CInt(data9) + CInt(myDbData.CanRec) <= CInt(myDbData.CanPed), "OK", "ERR")
                Dim statusNew = If((data6.Equals("") AndAlso data7.Equals("") AndAlso data8.Equals("")) OrElse (Not data6.Equals("") AndAlso Not data7.Equals("") AndAlso Not data8.Equals("")), "OK", "ERR")
                Dim dbValue = If(status.Equals("OK"), " ", "Exists")
                Dim dbValue2 = If(status2.Equals("OK"), "Exists", " ")
                Dim dbValue3 = If(status3.Equals("OK"), "Exists", " ")
                Dim newData As New List(Of Data)
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO ALBARÁN", .ReaderValue = data1, .DbValue = dbValue, .Status = status})
                newData.Add(New BarcodeViewer.Data With {.Key = "NÚMERO PEDIDO", .ReaderValue = data2, .DbValue = dbValue2, .Status = status2})
                newData.Add(New BarcodeViewer.Data With {.Key = "LÍNEA PEDIDO", .ReaderValue = data3, .DbValue = dbValue3, .Status = status3})
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO REFERENCIA", .ReaderValue = data4, .DbValue = "", .Status = Nothing})
                'newData.Add(New BarcodeViewer.Data With {.Key = "MATERIAL", .ReaderValue = data5, .DbValue = myDbData.Material, .Status = status4})
                newData.Add(New BarcodeViewer.Data With {.Key = "MATERIAL", .ReaderValue = data5, .DbValue = myDbData.Material, .Status = Nothing})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF", .ReaderValue = data6, .DbValue = myDbData.Numordf, .Status = status6})
                newData.Add(New BarcodeViewer.Data With {.Key = "OP", .ReaderValue = data7, .DbValue = myDbData.Numope, .Status = status7})
                newData.Add(New BarcodeViewer.Data With {.Key = "MARCA", .ReaderValue = data8, .DbValue = myDbData.Nummar, .Status = status8})
                newData.Add(New BarcodeViewer.Data With {.Key = "CANTIDAD", .ReaderValue = data9, .DbValue = myDbData.CanRec & " / " & myDbData.CanPed, .Status = status9})
                newData.Add(New BarcodeViewer.Data With {.Key = "PRECIO UNITARIO", .ReaderValue = data10})
                newData.Add(New BarcodeViewer.Data With {.Key = "IMPORTE", .ReaderValue = data11})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF/OP/MARCA", .ReaderValue = "", .DbValue = "", .Status = statusNew})

                Dim mismoAlbaran = checkIfMismoAlbaran(data1, dgvAlbaran.DataSource?.List(0))
                If Not mismoAlbaran Then
                    showErrorDialog("Has escaneado un albarán distinto al del comienzo. Para cambiar de albaran primero tienes que validar el ya escaneado")
                Else
                    Dim repe = DBConnection.checkEscaneadoRepetido(newData)
                    enableDataGrid(dgvQR, Not repe)
                    If Not repe Then
                        DBConnection.saveQR(indata, newData)
                        ChangeDataSource(dgvQR, newData)
                        ReloadDataSource(dgvAlbaran, newData)
                        dgvQR.Columns(0).DefaultCellStyle.Font = New Font(DataGridView.DefaultFont, FontStyle.Bold)
                        setLabel(lblAlbaran, data1)

                        If dgvAlbaran.DataSource?.Count > 0 Then
                            btnValidarAlbaran.BackColor = Color.ForestGreen
                            btnValidarAlbaran.ForeColor = Color.White
                            btnValidarAlbaran.FlatStyle = FlatStyle.Popup
                        End If
                    Else
                        status = "repe"
                        showErrorDialog("Acabas de escanear un código QR repetido (ya existe en la BD)... asegúrate de escanear el código correspondiente")
                    End If

                End If
            Else
                Dim newData As New List(Of Data)
                Dim newStatus = "Nothing"
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO ALBARÁN", .ReaderValue = data1, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "NÚMERO PEDIDO", .ReaderValue = data2, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "LÍNEA PEDIDO", .ReaderValue = data3, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO REFERENCIA", .ReaderValue = data4, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "MATERIAL", .ReaderValue = data5, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF", .ReaderValue = data6, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "OP", .ReaderValue = data7, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "MARCA", .ReaderValue = data8, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "CANTIDAD", .ReaderValue = data9, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "PRECIO UNITARIO", .ReaderValue = data10})
                newData.Add(New BarcodeViewer.Data With {.Key = "IMPORTE", .ReaderValue = data11})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF/OP/MARCA", .ReaderValue = "", .DbValue = "", .Status = newStatus})
                ReloadDataSource(dgvAlbaran, newData)

                DBConnection.saveQR(indata, newData)

                showErrorDialog("No existe en la BD")
            End If
        Catch ex As Exception
            If ex.Message.Equals("Índice fuera de los límites de la matriz.") Then
                showErrorDialog("FATAL ERROR: El código QR no está bien formado")
            Else
                showErrorDialog("FATAL ERROR: " & ex.Message)
            End If
        End Try
        'status = String.Empty
        myDisconnectionTimer.Start()
    End Sub

    Private Sub disconnectiontimer_elapsed(sender As Object, e As ElapsedEventArgs)
        Try
            myDisconnectionTimer.Stop()
            If Not sp.IsOpen Then
                If CONNECTED Then
                End If
                sp.Open()
                CONNECTED = True
            End If
        Catch ex As Exception
            CONNECTED = False
        Finally
            myDisconnectionTimer.Start()
        End Try
    End Sub

    Private Sub EnviarACK()
        sp.Write(cte.ACK, 0, cte.ACK.Length)
    End Sub
#End Region

#Region "Data manipulation"
    Private Function checkIfMismoAlbaran(data1 As String, v As QRData) As Boolean
        Return v Is Nothing OrElse data1.Equals(v.CodAlbaran)
    End Function

    Private Function createQRDataFromData(data As List(Of Data)) As QRData
        Dim qrData As New QRData
        qrData.CodAlbaran = data(0).ReaderValue
        qrData.NumPedido = data(1).ReaderValue
        qrData.LinPedido = data(2).ReaderValue
        qrData.CodReferencia = data(3).ReaderValue
        qrData.Material = data(4).ReaderValue
        qrData.OfData = data(5).ReaderValue
        qrData.OpData = data(6).ReaderValue
        qrData.Marca = data(7).ReaderValue
        qrData.Cantidad = data(8).ReaderValue
        qrData.PrecioUnitario = data(9).ReaderValue
        qrData.Importe = data(10).ReaderValue

        Dim status = "OK"
        For Each item In data
            If item.Status IsNot Nothing AndAlso item.Status.Equals("WARN") Then
                status = "WARN"
                Exit For
            ElseIf item.Status IsNot Nothing AndAlso item.Status.Equals("ERR") Then
                status = "ERR"
                Exit For
            ElseIf item.Status IsNot Nothing AndAlso item.Status.Equals("Nothing") Then
                status = "Nothing"
                Exit For
            End If
        Next
        qrData.Status = status
        Return qrData
    End Function

    Private Function comprobarAlbaran() As Boolean
        Dim registers As IEnumerable = dgvAlbaran.DataSource.List
        Dim status = "OK"
        For Each item In registers
            If item.Status.Equals("ERR") OrElse item.status.Equals("Nothing") Then
                status = "ERR"
                Exit For
            End If
        Next
        If status.Equals("ERR") Then
            For Each item In registers
                DBConnection.removeFromQR(item)
            Next
            Return False
        End If
        Return True
    End Function

    Private Sub setValueInAppConfig(ByVal key As String, ByVal val As String)
        Dim configuration = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath)
        configuration.AppSettings.Settings(key).Value = val
        configuration.Save(ConfigurationSaveMode.Modified)
        ConfigurationManager.RefreshSection("appSettings")
    End Sub
#End Region

#Region "show Dialog"
    Private Sub showValidationDialog(ByVal msg As String, ByVal NOKoption As Boolean, ByVal Optional list As IEnumerable = Nothing)
        Dim myDialog = New ValidationDialog(msg)
        myDialog.Location = New Point(
                                (Me.Location.X + Me.Width / 2) - myDialog.Width / 2,
                                (Me.Location.Y + Me.Height / 2) - myDialog.Height / 2
                            )
        myDialog.StartPosition = FormStartPosition.Manual
        myDialog.ShowDialog()
    End Sub

    Private Sub showErrorDialog(ByVal msg As String, ByVal Optional reset As Boolean = False)
        Dim myDialog = New ErrorDialog(msg, reset, Me)
        myDialog.Location = New Point(
                                (Me.Location.X + Me.Width / 2) - myDialog.Width / 2,
                                (Me.Location.Y + Me.Height / 2) - myDialog.Height / 2
                            )
        myDialog.StartPosition = FormStartPosition.Manual
        myDialog.ShowDialog()
    End Sub
#End Region

#Region "User actions"
    Private Sub cbPorts_SelectedIndexChanged(sender As ComboBox, e As EventArgs) Handles cbPorts.SelectedIndexChanged
        If sp IsNot Nothing AndAlso Not sender.SelectedValue.Equals("") Then
            If sp IsNot Nothing AndAlso sp.IsOpen Then
                sp.Close()
            End If
        End If
        'setPortNameInAppConfig()
        setValueInAppConfig("Portname", cbPorts.SelectedValue)
        connectToScanner()
        'checkConnection()
    End Sub

    Private Sub cbProviders_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbProviders.SelectedIndexChanged
        If sp IsNot Nothing AndAlso Not sender.SelectedValue.Equals("") Then
            If sp IsNot Nothing AndAlso sp.IsOpen Then
                sp.Close()
            End If
        End If
        setValueInAppConfig("Provider", cbProviders.SelectedItem.Code)
        connectToScanner()
    End Sub

    Private Sub btnChangeDefaults_Click(sender As Object, e As EventArgs) Handles btnChangeDefaults.Click
        Panel1.Enabled = True
        btnChangeDefaults.Visible = False
        btnApply.Visible = True
        btnCancel.Visible = True
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim year = TextBox3.Text
        Dim codprov As Provider = cbProviders.SelectedItem
        Dim schema = ComboBox2.SelectedValue
        Dim configuration = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath)
        configuration.AppSettings.Settings("Year").Value = year
        configuration.AppSettings.Settings("Provider").Value = codprov.Code
        configuration.AppSettings.Settings("Schema").Value = schema
        DBConnection.cnStr = ConfigurationManager.ConnectionStrings(schema).ConnectionString
        configuration.Save(ConfigurationSaveMode.Modified)
        ConfigurationManager.RefreshSection("appSettings")
        Panel1.Enabled = False
        btnApply.Visible = False
        btnCancel.Visible = False
        btnChangeDefaults.Visible = True
        btnChangeDefaults.Enabled = False
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        btnChangeDefaults.Visible = True
        Panel1.Enabled = False
        btnApply.Visible = False
        btnCancel.Visible = False
    End Sub

    Private Sub btnValidarAlbaran_Click(sender As Object, e As EventArgs) Handles btnValidarAlbaran.Click
        If dgvAlbaran.DataSource Is Nothing Then
            showErrorDialog("Debes escanear al menos un código QR para poder validar.", False)
            Exit Sub
        End If
        Dim dataOK = comprobarAlbaran()
        If dataOK Then
            Dim precioOK = True
            For Each linea As QRData In dgvAlbaran.DataSource.List
                If Not DBConnection.checkPrecioLinea(linea) Then
                    precioOK = False
                    Exit For
                End If
            Next
            If precioOK Then
                showValidationDialog("Albarán nº " & dgvAlbaran.DataSource.List(0).CodAlbaran & " correcto", False)
            Else
                showErrorDialog("Alguno de los precios no coincide con la BD. Se marca como albarán provisional", False)
                For Each linea As QRData In dgvAlbaran.DataSource.List
                    'DBConnection.setProvisional(linea)
                    DBConnection.actualizaGclinped(linea)
                    DBConnection.actualizaGcalbara(linea)
                    DBConnection.borraGcalbara(linea)
                Next
            End If
        Else
            showErrorDialog("Albarán no válido: alguna de las lecturas/líneas es errónea", False)
        End If
        restart()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        restart()
    End Sub

    Friend Sub restart()
        dgvAlbaran.DataSource = Nothing
        dgvQR.DataSource = Nothing
        lblAlbaran.Text = ""

        btnValidarAlbaran.BackColor = SystemColors.ControlLight
        btnValidarAlbaran.ForeColor = SystemColors.ControlText
        btnValidarAlbaran.FlatStyle = FlatStyle.Standard
    End Sub

#End Region

#Region "GridView format"
    Private Sub dgvQR_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvQR.CellFormatting
        If e.ColumnIndex = 3 Then
            Dim val = e.Value
            If val Is Nothing Then
                e.CellStyle.BackColor = Color.White
            ElseIf val.Equals("OK") Then
                e.CellStyle.BackColor = Color.Green
                e.CellStyle.ForeColor = Color.White
            Else
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub dgvAlbaran_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvAlbaran.RowPostPaint
        Dim dgvRow As DataGridViewRow = Me.dgvAlbaran.Rows(e.RowIndex)
        Dim status = dgvRow.Cells(11).Value
        If status IsNot Nothing Then
            If status.Equals("ERR") Then
                dgvRow.DefaultCellStyle.BackColor = Color.Red
            ElseIf status.Equals("WARN") Then
                dgvRow.DefaultCellStyle.BackColor = Color.Yellow
            ElseIf status.Equals("Nothing") Then
                dgvRow.DefaultCellStyle.BackColor = Color.Gray
            Else
                dgvRow.DefaultCellStyle.BackColor = Color.Green
            End If
        End If
    End Sub

    Private Sub dgvAlbaran_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvAlbaran.DataBindingComplete
        If dgvAlbaran.Columns.Count > 0 Then
            dgvAlbaran.Columns("CodAlbaran").Visible = False
            dgvAlbaran.Columns("Texto").Visible = False
            dgvAlbaran.Columns("Descripcion").Visible = False
        End If
    End Sub

#End Region

#Region "Easter Egg"
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If (e.Control AndAlso e.Shift AndAlso e.KeyCode = Keys.H) Then
            Form1_HelpButtonClicked(sender, Nothing)
        ElseIf (e.Control AndAlso e.Shift AndAlso e.KeyCode = Keys.R) Then
            restart()
        ElseIf (e.Control AndAlso e.Shift AndAlso e.KeyCode = Keys.C) Then
            btnChangeDefaults.Enabled = Not btnChangeDefaults.Enabled
        End If
    End Sub

    Private Sub Test(sender As Object, e As EventArgs) Handles Button1.Click
        Dim indata = "223614$333865$1$39D847.10$39D847/10$18012$40$51.152$4$19,4000$77,60"
        'indata = "226150$333293$1$MATRICERIA01083$S/SOLIDO$20502$10$01.01.271$4$119,3500$477,40"
        Try
            'Dim indata = "2236140"

            Dim data = indata.Split("$")
            Dim data1 = data(0) '*** código albarán --- que no exista en 'gccabalb'
            Dim data2 = data(1) '*** número pedido --- que sí exista - en 'gclinped'
            Dim data3 = data(2) '*** línea pedido --- que sí exista - en 'gclinped'
            Dim data4 = data(3) ' código referencia - ignorar
            Dim data5 = Regex.Replace(data(4), "[^a-zA-Z0-9]", "")
            Dim data6 = data(5)  '*** of
            Dim data7 = data(6) '*** op
            'Dim data7 = "" '*** op
            Dim data8 = data(7) '*** marca
            Dim data9 = data(8) ' cantidad
            Dim data10 = data(9) + " " ' precio unitarioº
            Dim data11 = data(10) + " " ' importe
            Dim data13 = indata ''  texto completo de la lectura QR
            Dim data14 = Nothing
            Dim data15 = Nothing

            Dim numPed = data2
            Dim linPed = data3

            Dim myDbData As DbData = DBConnection.getDbData(numPed, linPed)
            If myDbData IsNot Nothing Then
                Dim status = If(Not DBConnection.checkAlbaran(data1), "OK", "ERR")
                Dim status2 = If(DBConnection.checkNumPedido(numPed), "OK", "ERR")
                Dim status3 = If(DBConnection.checkLinPedido(linPed), "OK", "ERR")
                'Dim status4 = If(Regex.Replace(data5, "[^a-zA-Z0-9]", "").ToUpper.Equals(Regex.Replace(myDbData.Material, "[^a-zA-Z0-9]", "").ToUpper), "OK", "ERR")
                Dim status6 = If(data6.Trim.Equals(myDbData.Numordf), "OK", "ERR")
                Dim status7 = If(data7.Trim.Equals(myDbData.Numope), "OK", "ERR")
                Dim status8 = If(data8.Trim.Equals(myDbData.Nummar.Trim), "OK", "ERR")
                Dim status9 = If(CInt(data9) + CInt(myDbData.CanRec) <= CInt(myDbData.CanPed), "OK", "ERR")
                Dim statusNew = If((data6.Equals("") AndAlso data7.Equals("") AndAlso data8.Equals("")) OrElse (Not data6.Equals("") AndAlso Not data7.Equals("") AndAlso Not data8.Equals("")), "OK", "ERR")
                Dim dbValue = If(status.Equals("OK"), " ", "Exists")
                Dim dbValue2 = If(status2.Equals("OK"), "Exists", " ")
                Dim dbValue3 = If(status3.Equals("OK"), "Exists", " ")
                Dim newData As New List(Of Data)
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO ALBARÁN", .ReaderValue = data1, .DbValue = dbValue, .Status = status})
                newData.Add(New BarcodeViewer.Data With {.Key = "NÚMERO PEDIDO", .ReaderValue = data2, .DbValue = dbValue2, .Status = status2})
                newData.Add(New BarcodeViewer.Data With {.Key = "LÍNEA PEDIDO", .ReaderValue = data3, .DbValue = dbValue3, .Status = status3})
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO REFERENCIA", .ReaderValue = data4, .DbValue = "", .Status = Nothing})
                'newData.Add(New BarcodeViewer.Data With {.Key = "MATERIAL", .ReaderValue = data5, .DbValue = myDbData.Material, .Status = status4})
                newData.Add(New BarcodeViewer.Data With {.Key = "MATERIAL", .ReaderValue = data5, .DbValue = myDbData.Material, .Status = Nothing})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF", .ReaderValue = data6, .DbValue = myDbData.Numordf, .Status = status6})
                newData.Add(New BarcodeViewer.Data With {.Key = "OP", .ReaderValue = data7, .DbValue = myDbData.Numope, .Status = status7})
                newData.Add(New BarcodeViewer.Data With {.Key = "MARCA", .ReaderValue = data8, .DbValue = myDbData.Nummar, .Status = status8})
                newData.Add(New BarcodeViewer.Data With {.Key = "CANTIDAD", .ReaderValue = data9, .DbValue = myDbData.CanRec & " / " & myDbData.CanPed, .Status = status9})
                newData.Add(New BarcodeViewer.Data With {.Key = "PRECIO UNITARIO", .ReaderValue = data10})
                newData.Add(New BarcodeViewer.Data With {.Key = "IMPORTE", .ReaderValue = data11})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF/OP/MARCA", .ReaderValue = "", .DbValue = "", .Status = statusNew})

                Dim mismoAlbaran = checkIfMismoAlbaran(data1, dgvAlbaran.DataSource?.List(0))
                If Not mismoAlbaran Then
                    showErrorDialog("Has escaneado un albarán distinto al del comienzo. Para cambiar de albaran primero tienes que validar el ya escaneado")
                Else
                    Dim repe = DBConnection.checkEscaneadoRepetido(newData)
                    enableDataGrid(dgvQR, Not repe)
                    If Not repe Then
                        DBConnection.saveQR(indata, newData)
                        ChangeDataSource(dgvQR, newData)
                        ReloadDataSource(dgvAlbaran, newData)
                        dgvQR.Columns(0).DefaultCellStyle.Font = New Font(DataGridView.DefaultFont, FontStyle.Bold)
                        setLabel(lblAlbaran, data1)


                        If dgvAlbaran.DataSource?.Count > 0 Then
                            btnValidarAlbaran.BackColor = Color.ForestGreen
                            btnValidarAlbaran.ForeColor = Color.White
                            btnValidarAlbaran.FlatStyle = FlatStyle.Popup
                        End If
                    Else
                        status = "repe"
                        showErrorDialog("Acabas de escanear un código QR repetido (ya existe en la BD)... asegúrate de escanear el código correspondiente")
                    End If

                End If
            Else
                Dim newData As New List(Of Data)
                Dim newStatus = "Nothing"
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO ALBARÁN", .ReaderValue = data1, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "NÚMERO PEDIDO", .ReaderValue = data2, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "LÍNEA PEDIDO", .ReaderValue = data3, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "CÓDIGO REFERENCIA", .ReaderValue = data4, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "MATERIAL", .ReaderValue = data5, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF", .ReaderValue = data6, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "OP", .ReaderValue = data7, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "MARCA", .ReaderValue = data8, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "CANTIDAD", .ReaderValue = data9, .DbValue = "", .Status = newStatus})
                newData.Add(New BarcodeViewer.Data With {.Key = "PRECIO UNITARIO", .ReaderValue = data10})
                newData.Add(New BarcodeViewer.Data With {.Key = "IMPORTE", .ReaderValue = data11})
                newData.Add(New BarcodeViewer.Data With {.Key = "OF/OP/MARCA", .ReaderValue = "", .DbValue = "", .Status = newStatus})
                ReloadDataSource(dgvAlbaran, newData)

                DBConnection.saveQR(indata, newData)

                showErrorDialog("No existe en la BD")
            End If
        Catch ex As Exception
            If ex.Message.Equals("Índice fuera de los límites de la matriz.") Then
                showErrorDialog("FATAL ERROR: El código QR no está bien formado")
            Else
                showErrorDialog("FATAL ERROR: " & ex.Message)
            End If
        End Try

    End Sub

    Private Sub Form1_HelpButtonClicked(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.HelpButtonClicked
        MessageBox.Show("CONFIGURATION:" & Environment.NewLine & Environment.NewLine &
                        " - TNS_ADMIN:
                                    \\svm_cifs.batz.es\AreaWeb\CONFIG\Oracle" & Environment.NewLine & Environment.NewLine &
                        " - TSNAMES.ORA:
                                    XBAT=
                                      (DESCRIPTION=
                                        (LOAD_BALANCE=on)
                                        (ADDRESS=
                                          (PROTOCOL=TCP)
                                          (HOST=oracle-cluster)
                                          (PORT=1523)
                                        )
                                        (CONNECT_DATA=
                                          (FAILOVER_MODE=
                                            (TYPE=select)
                                            (METHOD=basic)
                                            (RETRIES=10)
                                            (DELAY=1)
                                          )
                                          (SERVER=dedicated)
                                          (SERVICE_NAME=xbat)
                                        )
                                      )

                                    GARAPEN=
                                      (DESCRIPTION=
                                        (ADDRESS=
                                          (PROTOCOL=TCP)
                                          (HOST=igalirrintza.batz.es)
                                          (PORT=1521)
                                        )
                                        (CONNECT_DATA=
                                          (SERVER=dedicated)
                                          (SERVICE_NAME=garapen)
                                        )
                                      )")
    End Sub
#End Region

End Class
