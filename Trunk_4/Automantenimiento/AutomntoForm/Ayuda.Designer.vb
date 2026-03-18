<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAyuda
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
        Me.pcbImagen = New System.Windows.Forms.PictureBox()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.pnlTexto = New System.Windows.Forms.Panel()
        Me.lblTexto = New System.Windows.Forms.Label()
        CType(Me.pcbImagen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.pnlTexto.SuspendLayout()
        Me.SuspendLayout()
        '
        'pcbImagen
        '
        Me.pcbImagen.Location = New System.Drawing.Point(12, 13)
        Me.pcbImagen.Name = "pcbImagen"
        Me.pcbImagen.Size = New System.Drawing.Size(100, 50)
        Me.pcbImagen.TabIndex = 0
        Me.pcbImagen.TabStop = False
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.Location = New System.Drawing.Point(3, 10)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(94, 31)
        Me.lblInfo.TabIndex = 5
        Me.lblInfo.Text = "lblInfo"
        '
        'Panel1
        '
        Me.Panel1.AutoSize = True
        Me.Panel1.Controls.Add(Me.pcbImagen)
        Me.Panel1.Location = New System.Drawing.Point(58, 92)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 100)
        Me.Panel1.TabIndex = 6
        '
        'pnlTexto
        '
        Me.pnlTexto.AutoSize = True
        Me.pnlTexto.Controls.Add(Me.lblTexto)
        Me.pnlTexto.Controls.Add(Me.lblInfo)
        Me.pnlTexto.Location = New System.Drawing.Point(58, 12)
        Me.pnlTexto.Name = "pnlTexto"
        Me.pnlTexto.Size = New System.Drawing.Size(148, 74)
        Me.pnlTexto.TabIndex = 7
        '
        'lblTexto
        '
        Me.lblTexto.AutoSize = True
        Me.lblTexto.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTexto.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblTexto.Location = New System.Drawing.Point(6, 41)
        Me.lblTexto.Name = "lblTexto"
        Me.lblTexto.Size = New System.Drawing.Size(89, 25)
        Me.lblTexto.TabIndex = 6
        Me.lblTexto.Text = "lblTexto"
        '
        'frmAyuda
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(571, 322)
        Me.Controls.Add(Me.pnlTexto)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmAyuda"
        Me.Text = "Imagen"
        CType(Me.pcbImagen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.pnlTexto.ResumeLayout(False)
        Me.pnlTexto.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pcbImagen As System.Windows.Forms.PictureBox
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlTexto As System.Windows.Forms.Panel
    Friend WithEvents lblTexto As System.Windows.Forms.Label
End Class
