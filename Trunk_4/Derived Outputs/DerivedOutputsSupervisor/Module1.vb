Module Module1
    Dim waitingTime = 1000 * 60 * 5
    Sub Main()
        While True
            launchDerivedOutputs()
            System.Threading.Thread.Sleep(1000 * 10)
        End While
    End Sub
    Private Sub launchDerivedOutputs()
        ensureNoDerivedOutputsRunning()
        Dim p = Process.Start("c:/Users/aazkuenaga.BATZNT/Documents/dotnet_code/Trunk_4/SBatz/Derived Outputs/DerivedOutputs/bin/Debug/DerivedOutputs.exe", "proba")
        p.WaitForExit(20000 * 10)
        If Not p.HasExited Then
            p.Kill()
        End If
        If Console.KeyAvailable Then
            If Console.ReadKey().Key = ConsoleKey.Escape Then
                Exit Sub
            End If
        End If
    End Sub

    Private Sub ensureNoDerivedOutputsRunning()
        Dim lstProcesos = System.Diagnostics.Process.GetProcessesByName("DerivedOutputs")
        If lstProcesos.Count > 0 Then
            For Each p In lstProcesos
                p.WaitForExit(waitingTime)
                If Not p.HasExited() Then
                    p.Kill()
                End If
            Next
        End If
    End Sub

End Module
