Imports System.Configuration

Public Class Constantes
#Region "LED AND BEEPER"

#Region "bytes"
    Public DC2 As Byte = &H12
    Public CR As Byte = &HD
    Public ESC As Byte = &H1B
    Public CERO As Byte = &H30
    Public UNO As Byte = &H31
    Public DOS As Byte = &H32
    Public TRES As Byte = &H33
    Public CUATRO As Byte = &H34
    Public CINCO As Byte = &H35
    Public SEIS As Byte = &H36
    Public SIETE As Byte = &H37
    Public OCHO As Byte = &H38
    Public NUEVE As Byte = &H39
    Public CORCHETE As Byte = &H5B
    Public Q As Byte = &H71
#End Region

#Region "commands"
    Public ASYNCDATA = {DC2}
    Public ENDDATA = {CR}
    Public TURN_ON_GREEN = {ESC, CORCHETE, SEIS, Q}
    Public TURN_OFF_GREEN = {ESC, CORCHETE, SIETE, Q}
    Public TURN_ON_RED = {ESC, CORCHETE, OCHO, Q}
    Public TURN_OFF_RED = {ESC, CORCHETE, NUEVE, Q}
    Public WAIT_100_MS = {ESC, CORCHETE, CINCO, Q}
    Public SHORT_HIGH_TONE = {ESC, CORCHETE, CERO, Q}
    Public SHORT_LOW_TONE = {ESC, CORCHETE, UNO, Q}
    Public LONG_LOW_TONE = {ESC, CORCHETE, DOS, Q}
    Public GOOD_READ_TONE = {ESC, CORCHETE, TRES, Q}
    Public BAD_TX_TONE = {ESC, CORCHETE, CUATRO, Q}
#End Region

#Region "packets"
    Public Function ACK() As Byte()
        Dim newlist As New List(Of Byte())
        newlist.Add(TURN_ON_GREEN)
        'newlist.Add(WAIT_100_MS)
        newlist.Add(TURN_OFF_GREEN)
        newlist.Add(ENDDATA)
        Dim result = concat(newlist)
        Return result
    End Function

    Public Function OK() As Byte()
        Dim newlist As New List(Of Byte())
        newlist.Add(ASYNCDATA)
        newlist.Add(TURN_ON_GREEN)
        If ConfigurationManager.AppSettings("Status").Equals("PRODUCTION") Then
            newlist.Add(SHORT_HIGH_TONE)
            newlist.Add(SHORT_HIGH_TONE)
            newlist.Add(SHORT_HIGH_TONE)
        End If
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(TURN_OFF_GREEN)
        newlist.Add(ENDDATA)

        Dim result = concat(newlist)
        Return result
    End Function

    Public Function NOK() As Byte()
        Dim newlist As New List(Of Byte())
        newlist.Add(ASYNCDATA)
        newlist.Add(TURN_ON_RED)
        If ConfigurationManager.AppSettings("Status").Equals("PRODUCTION") Then
            newlist.Add(LONG_LOW_TONE)
            newlist.Add(LONG_LOW_TONE)
        End If
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(WAIT_100_MS)
        newlist.Add(TURN_OFF_RED)
        newlist.Add(ENDDATA)
        Dim result = concat(newlist)
        Return result
    End Function

#End Region

    Public Function concat(ByVal bytes As List(Of Byte()))
        Dim length As Integer = 0
        For i = 0 To bytes.Count - 1
            length += bytes.ElementAt(i).Length
        Next
        Dim combined(length - 1) As Byte
        Dim position = 0
        For i = 0 To bytes.Count - 1
            bytes.ElementAt(i).CopyTo(combined, position)
            position += bytes.ElementAt(i).Length
        Next
        Return combined
    End Function

#End Region
End Class
