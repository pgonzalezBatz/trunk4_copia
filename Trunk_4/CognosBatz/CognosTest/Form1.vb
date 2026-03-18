Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim cg As New CognosBatz.Cognos("http://usotegieta2.batz.es:9300/p2pd/servlet/dispatch", "ayuste", "Verdel0007", "batznt")
        Dim resul As Boolean = cg.executeReport("/content/folder[@name='BATZ']/folder[@name='98 - Monedas']/report[@name='02 - RS 1 EURO son .....']", "PDF", "abelgarcia@batz.es;ayuste@batz.es", "Asunto", "Cuerpo")
        MessageBox.Show("Email enviado")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim cg As New CognosBatz.Cognos("http://usotegieta2.batz.es:9300/p2pd/servlet/dispatch", "ayuste", "Verdel0007", "batznt")
        Dim parametros As String = cg.getParametros("/content/folder[@name='BATZ']/folder[@name='Trokelgintza - Troqueleria']/folder[@name='Kontsulta Orokorrak - Consultas Generales']/folder[@name='03 - Erosketak - Compras']/report[@name='Emision Hojas Pedidos']")
        MessageBox.Show(parametros)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        'Variable that contains the default URL for CRN Content Manager. 
        Dim endPoint As String = "http://usotegieta2.batz.es:9300/p2pd/servlet/dispatch"
        Dim searchPath As String = "/content/folder[@name='BATZ']/folder[@name='Trokelgintza - Troqueleria']/folder[@name='Kontsulta Orokorrak - Consultas Generales']/folder[@name='03 - Erosketak - Compras']/report[@name='Emision Hojas Pedidos']"
        'Dim searchPath As String = "/content/folder[@name='BATZ']/folder[@name='Sistemak - Sistemas']/folder[@name='08 - QUALITY - CALIDAD']/folder[@name='NO CONFORMIDADES']/report[@name='Informe 8D']"
        'Dim searchPath As String = "/content/folder[@name='BATZ']/folder[@name='Sistemak - Sistemas']/folder[@name='02 - SALES - VENTAS Y CARTERA']/folder[@name='01 - EVOLUTION OF SALES BY CUSTOMER']/report[@name='01. Evolution of Sales by Customer Group']"
        'Dim searchPath As String = "/content/folder[@name='BATZ']/folder[@name='Zerbitzu Orokorrak - Servicios Generales']/folder[@name='03 - Sistemas de Información y Comunicación']/folder[@name='07 - HelpDesk']/report[@name='Tareas Batz Tracker']"
        Dim nameSpaceID As String = "batznt"
        Dim userName As String = "tareas"
        Dim password As String = "tareas123"
        Dim format As String = "PDF"

        Dim obj As CognosBatz.ReportPasoParametros = New CognosBatz.ReportPasoParametros(endPoint)

        'comment this call if anonymous logon is enabled in Cognos Configuration.
        Dim logonResults As String = obj.quickLogon(nameSpaceID, userName, password)
        'System.Console.Out.WriteLine(logonResults);

        'ejemplo Informe de Pedido
        Dim fichero As System.IO.FileInfo = obj.executeReport_FI(searchPath, format, {"dped"}, {"261476"}) 'change format to (PDF, CSV, HTML, HTMLFragment, XML)

        'ejemplo Informe de Batz Tracker
        'Dim fichero As System.IO.FileInfo = obj.executeReport(searchPath, format, {"Responsable", "Proyecto", "estado"}, {"ALBERTO YUSTE", "PDF FLY", "ABIERTA"}) 'change format to (PDF, CSV, HTML, HTMLFragment, XML)

        'System.Console.Out.WriteLine("Report execution complete. File saved to d:\\temp\\reportOutput." + format);

        MessageBox.Show("Termina")
    End Sub
End Class
