Imports BarcodeViewer

Public Class ErrorDialog
    Private msg As String
    Public reset As Boolean = False
    Public myParentForm As Form1

    Public Sub New(msg As String, reset As Boolean, form1 As Form1)
        InitializeComponent()
        Me.msg = msg
        Me.reset = reset
        lblError.Text = msg
        Me.myParentForm = form1
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        If reset Then
            myParentForm.restart()
        End If
    End Sub
End Class