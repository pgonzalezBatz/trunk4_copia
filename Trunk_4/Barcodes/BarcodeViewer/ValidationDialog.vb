Public Class ValidationDialog


    Dim registers As IEnumerable
    Dim myParentForm As Form1
    Private nOKoption As Boolean

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub
    Public Sub New(v As String)
        InitializeComponent()
        Label1.Text = v
    End Sub

    'Public Sub New(v As String, nOKoption As Boolean)
    '    Me.New(v)
    '    Me.nOKoption = nOKoption
    'End Sub

    'Public Sub New(v As String, s As Boolean, list As IEnumerable, form1 As Form1)
    '    InitializeComponent()
    '    myParentForm = form1
    '    registers = list
    '    Button2.Visible = s
    '    If s Then
    '        Button1.Top = 40
    '        Button2.Top = 80
    '    End If
    '    Label1.Text = v
    'End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'myParentForm.restart()
        Me.Close()
    End Sub

    'Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    '    'DBConnection.setProvisional(registers)
    '    'myParentForm.restart()
    '    'Me.Close()
    'End Sub
End Class