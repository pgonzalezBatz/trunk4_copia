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
        Me.pnlPassword = New System.Windows.Forms.Panel()
        Me.labelPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.btnTecladoVirt = New System.Windows.Forms.Button()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.txtNumTrab = New System.Windows.Forms.TextBox()
        Me.labelNumTrab = New System.Windows.Forms.Label()
        Me.pnlContenido = New System.Windows.Forms.Panel()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lblMaquina = New System.Windows.Forms.Label()
        Me.labelTitulo = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlKeyboard = New System.Windows.Forms.Panel()
        Me.gbDatos.SuspendLayout()
        Me.pnlPassword.SuspendLayout()
        Me.pnlContenido.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbDatos
        '
        Me.gbDatos.Controls.Add(Me.pnlPassword)
        Me.gbDatos.Controls.Add(Me.btnTecladoVirt)
        Me.gbDatos.Controls.Add(Me.btnLogin)
        Me.gbDatos.Controls.Add(Me.txtNumTrab)
        Me.gbDatos.Controls.Add(Me.labelNumTrab)
        Me.gbDatos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.gbDatos.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbDatos.Location = New System.Drawing.Point(20, 107)
        Me.gbDatos.Name = "gbDatos"
        Me.gbDatos.Size = New System.Drawing.Size(617, 244)
        Me.gbDatos.TabIndex = 0
        Me.gbDatos.TabStop = False
        Me.gbDatos.Text = "Datos"
        '
        'pnlPassword
        '
        Me.pnlPassword.Controls.Add(Me.labelPassword)
        Me.pnlPassword.Controls.Add(Me.txtPassword)
        Me.pnlPassword.Location = New System.Drawing.Point(37, 89)
        Me.pnlPassword.Name = "pnlPassword"
        Me.pnlPassword.Size = New System.Drawing.Size(506, 56)
        Me.pnlPassword.TabIndex = 2
        '
        'labelPassword
        '
        Me.labelPassword.AutoSize = True
        Me.labelPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPassword.Location = New System.Drawing.Point(0, 18)
        Me.labelPassword.Name = "labelPassword"
        Me.labelPassword.Size = New System.Drawing.Size(155, 31)
        Me.labelPassword.TabIndex = 1
        Me.labelPassword.Text = "Contraseña"
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(215, 14)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(290, 35)
        Me.txtPassword.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.txtPassword, "Contraseña del portal del empleado")
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'btnTecladoVirt
        '
        Me.btnTecladoVirt.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTecladoVirt.Location = New System.Drawing.Point(6, 179)
        Me.btnTecladoVirt.Name = "btnTecladoVirt"
        Me.btnTecladoVirt.Size = New System.Drawing.Size(222, 59)
        Me.btnTecladoVirt.TabIndex = 3
        Me.btnTecladoVirt.Text = "Teclado Virtual"
        Me.btnTecladoVirt.UseVisualStyleBackColor = True
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(292, 179)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(223, 59)
        Me.btnLogin.TabIndex = 3
        Me.btnLogin.Text = "Entrar"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'txtNumTrab
        '
        Me.txtNumTrab.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumTrab.Location = New System.Drawing.Point(253, 56)
        Me.txtNumTrab.Name = "txtNumTrab"
        Me.txtNumTrab.Size = New System.Drawing.Size(290, 35)
        Me.txtNumTrab.TabIndex = 1
        '
        'labelNumTrab
        '
        Me.labelNumTrab.AutoSize = True
        Me.labelNumTrab.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelNumTrab.Location = New System.Drawing.Point(37, 56)
        Me.labelNumTrab.Name = "labelNumTrab"
        Me.labelNumTrab.Size = New System.Drawing.Size(200, 31)
        Me.labelNumTrab.TabIndex = 0
        Me.labelNumTrab.Text = "Num trabajador"
        '
        'pnlContenido
        '
        Me.pnlContenido.AutoSize = True
        Me.pnlContenido.Controls.Add(Me.lblInfo)
        Me.pnlContenido.Controls.Add(Me.lblMaquina)
        Me.pnlContenido.Controls.Add(Me.labelTitulo)
        Me.pnlContenido.Controls.Add(Me.gbDatos)
        Me.pnlContenido.Location = New System.Drawing.Point(126, 37)
        Me.pnlContenido.Name = "pnlContenido"
        Me.pnlContenido.Size = New System.Drawing.Size(756, 440)
        Me.pnlContenido.TabIndex = 1
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
        'lblMaquina
        '
        Me.lblMaquina.AutoSize = True
        Me.lblMaquina.Font = New System.Drawing.Font("Microsoft Sans Serif", 23.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaquina.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lblMaquina.Location = New System.Drawing.Point(15, 62)
        Me.lblMaquina.Name = "lblMaquina"
        Me.lblMaquina.Size = New System.Drawing.Size(164, 35)
        Me.lblMaquina.TabIndex = 2
        Me.lblMaquina.Text = "lblMaquina"
        '
        'labelTitulo
        '
        Me.labelTitulo.AutoSize = True
        Me.labelTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelTitulo.Location = New System.Drawing.Point(15, 9)
        Me.labelTitulo.Name = "labelTitulo"
        Me.labelTitulo.Size = New System.Drawing.Size(677, 39)
        Me.labelTitulo.TabIndex = 1
        Me.labelTitulo.Text = "AUTOMANTENIMIENTO DE LA MAQUINA"
        '
        'ToolTip
        '
        Me.ToolTip.IsBalloon = True
        '
        'pnlKeyboard
        '
        Me.pnlKeyboard.Location = New System.Drawing.Point(126, 446)
        Me.pnlKeyboard.Name = "pnlKeyboard"
        Me.pnlKeyboard.Size = New System.Drawing.Size(756, 100)
        Me.pnlKeyboard.TabIndex = 2
        '
        'frmLogin
        '
        Me.AcceptButton = Me.btnLogin
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1293, 804)
        Me.Controls.Add(Me.pnlKeyboard)
        Me.Controls.Add(Me.pnlContenido)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.gbDatos.ResumeLayout(False)
        Me.gbDatos.PerformLayout()
        Me.pnlPassword.ResumeLayout(False)
        Me.pnlPassword.PerformLayout()
        Me.pnlContenido.ResumeLayout(False)
        Me.pnlContenido.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbDatos As System.Windows.Forms.GroupBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtNumTrab As System.Windows.Forms.TextBox
    Friend WithEvents labelPassword As System.Windows.Forms.Label
    Friend WithEvents labelNumTrab As System.Windows.Forms.Label
    Friend WithEvents pnlContenido As System.Windows.Forms.Panel
    Friend WithEvents lblMaquina As System.Windows.Forms.Label
    Friend WithEvents labelTitulo As System.Windows.Forms.Label
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents pnlKeyboard As System.Windows.Forms.Panel
    Friend WithEvents btnTecladoVirt As System.Windows.Forms.Button
    Friend WithEvents pnlPassword As System.Windows.Forms.Panel

End Class
