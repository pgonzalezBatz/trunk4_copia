Module Module1
    Dim sleepTime = 1000 * 60 * 90 ' 1 hour 30 minutes
    Sub Main()
        Try
            Dim args() As String = System.Environment.GetCommandLineArgs()
            Dim idProceso = CInt(args(1))
            Dim intentos = CInt(args(2))
            Dim diferido = args(3)
            If diferido = "normal" Then
                Console.Write("Going to sleep for " + sleepTime.ToString + " miliseconds")
                Threading.Thread.Sleep(sleepTime)
                Console.WriteLine("Ready to work!")
                colasinterop.ReencolarProceso(idProceso, intentos + 1, colasinterop.Diferido.normal)
            Else
                Console.WriteLine("Setting process to run with deferred execution")
                colasinterop.ReencolarProceso(idProceso, intentos + 1, colasinterop.Diferido.diferido)
            End If
            Console.WriteLine("requeuer done")
        Catch ex As Exception

        End Try
    End Sub

End Module
