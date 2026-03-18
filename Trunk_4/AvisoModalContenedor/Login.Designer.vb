<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.gbDatos = New System.Windows.Forms.GroupBox()
        Me.labelAviso = New System.Windows.Forms.Label()
        Me.btnTecladoVirt = New System.Windows.Forms.Button()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.txtNumTrab = New System.Windows.Forms.TextBox()
        Me.labelNumTrab = New System.Windows.Forms.Label()
        Me.pnlContenido = New System.Windows.Forms.Panel()
        Me.pnlKeyboard = New System.Windows.Forms.Panel()
        Me.labelMensaje = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.labelTitulo = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.gbDatos.SuspendLayout()
        Me.pnlContenido.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbDatos
        '
        Me.gbDatos.Controls.Add(Me.labelAviso)
        Me.gbDatos.Controls.Add(Me.btnTecladoVirt)
        Me.gbDatos.Controls.Add(Me.btnLogin)
        Me.gbDatos.Controls.Add(Me.txtNumTrab)
        Me.gbDatos.Controls.Add(Me.labelNumTrab)
        Me.gbDatos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.gbDatos.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbDatos.Location = New System.Drawing.Point(54, 177)
        Me.gbDatos.Name = "gbDatos"
        Me.gbDatos.Size = New System.Drawing.Size(643, 260)
        Me.gbDatos.TabIndex = 0
        Me.gbDatos.TabStop = False
        Me.gbDatos.Text = "DATOS"
        '
        'labelAviso
        '
        Me.labelAviso.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelAviso.ForeColor = System.Drawing.Color.Red
        Me.labelAviso.Location = New System.Drawing.Point(6, 106)
        Me.labelAviso.Name = "labelAviso"
        Me.labelAviso.Size = New System.Drawing.Size(631, 71)
        Me.labelAviso.TabIndex = 4
        Me.labelAviso.Text = "Label1"
        Me.labelAviso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnTecladoVirt
        '
        Me.btnTecladoVirt.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTecladoVirt.Location = New System.Drawing.Point(43, 180)
        Me.btnTecladoVirt.Name = "btnTecladoVirt"
        Me.btnTecladoVirt.Size = New System.Drawing.Size(222, 59)
        Me.btnTecladoVirt.TabIndex = 3
        Me.btnTecladoVirt.Text = "Teclado Virtual"
        Me.btnTecladoVirt.UseVisualStyleBackColor = True
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(375, 180)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(223, 59)
        Me.btnLogin.TabIndex = 3
        Me.btnLogin.Text = "Hecho!"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'txtNumTrab
        '
        Me.txtNumTrab.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumTrab.Location = New System.Drawing.Point(300, 68)
        Me.txtNumTrab.Name = "txtNumTrab"
        Me.txtNumTrab.Size = New System.Drawing.Size(298, 35)
        Me.txtNumTrab.TabIndex = 1
        '
        'labelNumTrab
        '
        Me.labelNumTrab.AutoSize = True
        Me.labelNumTrab.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelNumTrab.Location = New System.Drawing.Point(37, 68)
        Me.labelNumTrab.Name = "labelNumTrab"
        Me.labelNumTrab.Size = New System.Drawing.Size(229, 31)
        Me.labelNumTrab.TabIndex = 0
        Me.labelNumTrab.Text = "Codigo trabajador"
        '
        'pnlContenido
        '
        Me.pnlContenido.AutoSize = True
        Me.pnlContenido.Controls.Add(Me.pnlKeyboard)
        Me.pnlContenido.Controls.Add(Me.labelMensaje)
        Me.pnlContenido.Controls.Add(Me.lblInfo)
        Me.pnlContenido.Controls.Add(Me.labelTitulo)
        Me.pnlContenido.Controls.Add(Me.gbDatos)
        Me.pnlContenido.Location = New System.Drawing.Point(230, 42)
        Me.pnlContenido.Name = "pnlContenido"
        Me.pnlContenido.Size = New System.Drawing.Size(765, 751)
        Me.pnlContenido.TabIndex = 1
        '
        'pnlKeyboard
        '
        Me.pnlKeyboard.BackColor = System.Drawing.SystemColors.ControlDark
        Me.pnlKeyboard.Location = New System.Drawing.Point(97, 443)
        Me.pnlKeyboard.Name = "pnlKeyboard"
        Me.pnlKeyboard.Size = New System.Drawing.Size(222, 287)
        Me.pnlKeyboard.TabIndex = 2
        '
        'labelMensaje
        '
        Me.labelMensaje.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelMensaje.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.labelMensaje.Location = New System.Drawing.Point(3, 61)
        Me.labelMensaje.Name = "labelMensaje"
        Me.labelMensaje.Size = New System.Drawing.Size(756, 113)
        Me.labelMensaje.TabIndex = 5
        Me.labelMensaje.Text = "Mensaje"
        Me.labelMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.ForeColor = System.Drawing.Color.Red
        Me.lblInfo.Location = New System.Drawing.Point(15, 409)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(0, 31)
        Me.lblInfo.TabIndex = 3
        '
        'labelTitulo
        '
        Me.labelTitulo.AutoSize = True
        Me.labelTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelTitulo.Location = New System.Drawing.Point(277, 9)
        Me.labelTitulo.Name = "labelTitulo"
        Me.labelTitulo.Size = New System.Drawing.Size(213, 39)
        Me.labelTitulo.TabIndex = 1
        Me.labelTitulo.Text = "ATENCIÓN!"
        Me.labelTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ToolTip
        '
        Me.ToolTip.IsBalloon = True
        '
        'frmLogin
        '
        Me.AcceptButton = Me.btnLogin
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1293, 805)
        Me.Controls.Add(Me.pnlContenido)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.gbDatos.ResumeLayout(False)
        Me.gbDatos.PerformLayout()
        Me.pnlContenido.ResumeLayout(False)
        Me.pnlContenido.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbDatos As System.Windows.Forms.GroupBox
    Friend WithEvents txtNumTrab As System.Windows.Forms.TextBox
    Friend WithEvents labelNumTrab As System.Windows.Forms.Label
    Friend WithEvents pnlContenido As System.Windows.Forms.Panel
    Friend WithEvents labelTitulo As System.Windows.Forms.Label
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents pnlKeyboard As System.Windows.Forms.Panel
    Friend WithEvents btnTecladoVirt As System.Windows.Forms.Button
    Friend WithEvents labelAviso As Label
    Friend WithEvents labelMensaje As Label
End Class
