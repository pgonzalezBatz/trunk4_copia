<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHoja
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
        Me.labelPersona = New System.Windows.Forms.Label()
        Me.lblNombrePersona = New System.Windows.Forms.Label()
        Me.labelMaquina = New System.Windows.Forms.Label()
        Me.lblMaquina = New System.Windows.Forms.Label()
        Me.pnlKeyboard = New System.Windows.Forms.Panel()
        Me.myTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'labelPersona
        '
        Me.labelPersona.AutoSize = True
        Me.labelPersona.Font = New System.Drawing.Font("Microsoft Sans Serif", 17.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPersona.Location = New System.Drawing.Point(28, 18)
        Me.labelPersona.Name = "labelPersona"
        Me.labelPersona.Size = New System.Drawing.Size(0, 29)
        Me.labelPersona.TabIndex = 3
        '
        'lblNombrePersona
        '
        Me.lblNombrePersona.AutoSize = True
        Me.lblNombrePersona.Font = New System.Drawing.Font("Microsoft Sans Serif", 17.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombrePersona.Location = New System.Drawing.Point(137, 18)
        Me.lblNombrePersona.Name = "lblNombrePersona"
        Me.lblNombrePersona.Size = New System.Drawing.Size(136, 29)
        Me.lblNombrePersona.TabIndex = 4
        Me.lblNombrePersona.Text = "lblNombre"
        '
        'labelMaquina
        '
        Me.labelMaquina.AutoSize = True
        Me.labelMaquina.Font = New System.Drawing.Font("Microsoft Sans Serif", 17.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelMaquina.Location = New System.Drawing.Point(28, 49)
        Me.labelMaquina.Name = "labelMaquina"
        Me.labelMaquina.Size = New System.Drawing.Size(0, 29)
        Me.labelMaquina.TabIndex = 5
        '
        'lblMaquina
        '
        Me.lblMaquina.AutoSize = True
        Me.lblMaquina.Font = New System.Drawing.Font("Microsoft Sans Serif", 17.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaquina.Location = New System.Drawing.Point(138, 49)
        Me.lblMaquina.Name = "lblMaquina"
        Me.lblMaquina.Size = New System.Drawing.Size(141, 29)
        Me.lblMaquina.TabIndex = 6
        Me.lblMaquina.Text = "lblMaquina"
        '
        'pnlKeyboard
        '
        Me.pnlKeyboard.AutoSize = True
        Me.pnlKeyboard.Location = New System.Drawing.Point(127, 522)
        Me.pnlKeyboard.Name = "pnlKeyboard"
        Me.pnlKeyboard.Size = New System.Drawing.Size(756, 100)
        Me.pnlKeyboard.TabIndex = 7
        '
        'myTooltip
        '
        Me.myTooltip.IsBalloon = True
        '
        'btnSalir
        '
        Me.btnSalir.Image = Global.AutomntoForm.My.Resources.Resources.LogOff
        Me.btnSalir.Location = New System.Drawing.Point(1007, 12)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(75, 54)
        Me.btnSalir.TabIndex = 8
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'frmHoja
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1104, 657)
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.pnlKeyboard)
        Me.Controls.Add(Me.lblMaquina)
        Me.Controls.Add(Me.labelMaquina)
        Me.Controls.Add(Me.lblNombrePersona)
        Me.Controls.Add(Me.labelPersona)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmHoja"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hoja"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents labelPersona As System.Windows.Forms.Label
    Friend WithEvents lblNombrePersona As System.Windows.Forms.Label
    Friend WithEvents labelMaquina As System.Windows.Forms.Label
    Friend WithEvents lblMaquina As System.Windows.Forms.Label
    Friend WithEvents pnlKeyboard As System.Windows.Forms.Panel
    Friend WithEvents myTooltip As System.Windows.Forms.ToolTip
    Friend WithEvents btnSalir As System.Windows.Forms.Button
End Class
