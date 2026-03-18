<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmObservacion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmObservacion))
        Me.txtObservacion = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.pnlKeyboard = New System.Windows.Forms.Panel()
        Me.lblPunto = New System.Windows.Forms.Label()
        Me.pnlContenido = New System.Windows.Forms.Panel()
        Me.labelTitulo = New System.Windows.Forms.Label()
        Me.pnlContenido.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtObservacion
        '
        Me.txtObservacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtObservacion.Location = New System.Drawing.Point(23, 90)
        Me.txtObservacion.Multiline = True
        Me.txtObservacion.Name = "txtObservacion"
        Me.txtObservacion.Size = New System.Drawing.Size(670, 159)
        Me.txtObservacion.TabIndex = 0
        '
        'btnGuardar
        '
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGuardar.Location = New System.Drawing.Point(206, 260)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(107, 56)
        Me.btnGuardar.TabIndex = 1
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCerrar.Location = New System.Drawing.Point(388, 258)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(89, 56)
        Me.btnCerrar.TabIndex = 2
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'pnlKeyboard
        '
        Me.pnlKeyboard.AutoSize = True
        Me.pnlKeyboard.Location = New System.Drawing.Point(17, 333)
        Me.pnlKeyboard.Name = "pnlKeyboard"
        Me.pnlKeyboard.Size = New System.Drawing.Size(756, 100)
        Me.pnlKeyboard.TabIndex = 8
        '
        'lblPunto
        '
        Me.lblPunto.AutoSize = True
        Me.lblPunto.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPunto.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lblPunto.Location = New System.Drawing.Point(21, 62)
        Me.lblPunto.Name = "lblPunto"
        Me.lblPunto.Size = New System.Drawing.Size(82, 25)
        Me.lblPunto.TabIndex = 9
        Me.lblPunto.Text = "lblPunto"
        '
        'pnlContenido
        '
        Me.pnlContenido.Controls.Add(Me.labelTitulo)
        Me.pnlContenido.Controls.Add(Me.lblPunto)
        Me.pnlContenido.Controls.Add(Me.pnlKeyboard)
        Me.pnlContenido.Controls.Add(Me.txtObservacion)
        Me.pnlContenido.Controls.Add(Me.btnCerrar)
        Me.pnlContenido.Controls.Add(Me.btnGuardar)
        Me.pnlContenido.Location = New System.Drawing.Point(25, 12)
        Me.pnlContenido.Name = "pnlContenido"
        Me.pnlContenido.Size = New System.Drawing.Size(806, 623)
        Me.pnlContenido.TabIndex = 10
        '
        'labelTitulo
        '
        Me.labelTitulo.AutoSize = True
        Me.labelTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelTitulo.Location = New System.Drawing.Point(15, 6)
        Me.labelTitulo.Name = "labelTitulo"
        Me.labelTitulo.Size = New System.Drawing.Size(315, 39)
        Me.labelTitulo.TabIndex = 10
        Me.labelTitulo.Text = "OBSERVACIONES"
        '
        'frmObservacion
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(914, 726)
        Me.Controls.Add(Me.pnlContenido)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmObservacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Observaciones"
        Me.pnlContenido.ResumeLayout(False)
        Me.pnlContenido.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtObservacion As System.Windows.Forms.TextBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
    Friend WithEvents pnlKeyboard As System.Windows.Forms.Panel
    Friend WithEvents lblPunto As System.Windows.Forms.Label
    Friend WithEvents pnlContenido As System.Windows.Forms.Panel
    Friend WithEvents labelTitulo As System.Windows.Forms.Label
End Class
