Imports AutomntoLib

Public Class frmAyuda

#Region "Constructor"

    Private Id As Integer
    Private Tipo As String

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="_id">Id</param>    
    ''' <param name="_tipo">M:Imagen de ayuda asociada a la maquina;P:Imagen de ayuda del punto</param>
    Public Sub New(ByVal _id As Integer, ByVal _tipo As String)
        InitializeComponent()
        Id = _id
        Tipo = _tipo
    End Sub

#End Region

#Region "Carga del formulario"

    ''' <summary>
    ''' Se muestra la imagen parametrizada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Imagen_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            ConfigurarVentana()
            lblInfo.Text = itzultzaileWeb.Itzuli("Hacer click en la imagen para volver").ToUpper
            lblInfo.MaximumSize = New Drawing.Size(My.Computer.Screen.Bounds.Width - 30, 0)  'Para que si el texto es mas largo que la pantalla, lo parta en varias lineas
            MostrarAyuda()
        Catch ex As Exception
            log.Error("Ha ocurrido un error al cargar la imagen", ex)
            MessageBox.Show(itzultzaileWeb.Itzuli("Error al cargar la imagen").ToUpper, "Carga", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Configura los parametros de la ventana
    ''' </summary>    
    Private Sub ConfigurarVentana()
        Me.TopMost = True  'Para que sea modal
        Me.TopLevel = True
        Me.FormBorderStyle = FormBorderStyle.None  'Para que no tenga botones de minimizar y cerrar
        'Establece como dimensiones de la ventana las de la pantalla
        Me.Width = My.Computer.Screen.Bounds.Width
        Me.Height = My.Computer.Screen.Bounds.Height
        'Situa la ventana en el punto 0,0
        Me.Location = New Drawing.Point(0, 0)
    End Sub

    ''' <summary>
    ''' Muestra la informacion de la ayuda
    ''' </summary>    
    Private Sub MostrarAyuda()
        Dim tooltip As New ToolTip
        If (Tipo = "M") Then
            Dim maqBLL As New BLL.MaquinaBLL
            Dim ayuda As ELL.Clases.Maquina.Ayuda = maqBLL.consultarAyuda(Id)
            Dim mstreamImage As New IO.MemoryStream(ayuda.Imagen)
            pcbImagen.Image = Image.FromStream(mstreamImage)
            mstreamImage.Close()
            lblTexto.Text = String.Empty : lblTexto.Visible = False
        ElseIf (Tipo = "P") Then
            Dim puntBLL As New BLL.PuntoBLL
            Dim punto As ELL.Clases.Punto = puntBLL.consultar(Id, True)
            If (punto.NombreImgAyuda <> String.Empty) Then
                Dim mstreamImage As New IO.MemoryStream(punto.ImgAyuda)
                pcbImagen.Image = Image.FromStream(mstreamImage)
                mstreamImage.Close()
            Else
                pcbImagen.Image = Image.FromFile(Modulo.ImagenesConfig & "sinImagen.png")
            End If
            Dim textoDescr As String = punto.RangoAyudaKultura(Ticket.Culture)
            If (textoDescr <> String.Empty) Then             
                lblTexto.Text = textoDescr                
                lblTexto.MaximumSize = New Drawing.Size(My.Computer.Screen.Bounds.Width - 60, 0)  'Para que si el texto es mas largo que la pantalla, lo parta en varias lineas                                
                pnlTexto.Controls.Add(lblTexto)
            End If
            lblTexto.Visible = True
        End If                
        pcbImagen.Cursor = Cursors.Hand
        pcbImagen.Location = New Drawing.Size(25, 15)
        Dim limitWidth, limitHeight As Integer
        'Para que la imagen nunca se salga del tamaño de la ventana
        limitWidth = Me.Width - 100 : limitHeight = Me.Height - 130        
        If (pcbImagen.Image.Width > limitWidth And pcbImagen.Image.Height > limitHeight) Then  'Si es mas alto y mas ancho
            pcbImagen.Size = New Drawing.Size(limitWidth, limitHeight)
            pcbImagen.SizeMode = Windows.Forms.PictureBoxSizeMode.StretchImage
        ElseIf (pcbImagen.Image.Width > limitWidth And pcbImagen.Image.Height <= limitHeight) Then  'Si es mas ancho pero menos bajo
            pcbImagen.Size = New Drawing.Size(limitWidth, pcbImagen.Image.Height)
            pcbImagen.SizeMode = Windows.Forms.PictureBoxSizeMode.Normal
        ElseIf (pcbImagen.Image.Width <= limitWidth And pcbImagen.Image.Height > limitHeight) Then  'Si es mas alto pero menos ancho
            pcbImagen.Size = New Drawing.Size(pcbImagen.Image.Width, limitHeight)
            pcbImagen.SizeMode = Windows.Forms.PictureBoxSizeMode.Normal
        Else  'Si es menos ancha y menos alta
            pcbImagen.Size = New Drawing.Size(pcbImagen.Image.Width, pcbImagen.Image.Height)
            pcbImagen.SizeMode = Windows.Forms.PictureBoxSizeMode.Normal
        End If
        AddHandler pcbImagen.Click, AddressOf CerrarVentana
        tooltip.SetToolTip(pcbImagen, itzultzaileWeb.Itzuli("Cerrar"))
    End Sub

    ''' <summary>
    ''' Cierra el formulario que muestra la imagen
    ''' </summary>    
    Protected Sub CerrarVentana()
        Me.Hide()
    End Sub

#End Region

End Class