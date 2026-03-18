Namespace BLL.UO

    Public Class OF_NAVISION
        Implements IOF

        ''' <summary>
        ''' Obtiene una OF de brain
        ''' </summary>
        ''' <param name="conexion">String de conexion de la bbdd o direccion del webservice</param>
        ''' <param name="numOF">Numero de la of a validar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>NumOF,Denominacion,lantegi</returns>    
        Public Function consultarOF(conexion As String, numOF As String, idPlanta As Integer) As String() Implements IOF.consultarOF
            Try
                Dim navBLL As New BLL.NavisionBLL
                Return navBLL.consultarOF(conexion, numOF)
                'Puede que haya que hacer algo parecido
                'Dim plantBLL As New SabLib.BLL.PlantasComponent
                'Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(idPlanta)
                'If (oPlant.IdBrain <> String.Empty) Then
                '    numOF = numOF.ToLower
                '    If (numOF.StartsWith("y") Or numOF.StartsWith("n") Or numOF.StartsWith("p")) Then
                '        Return bidaiakDAL.consultarOFInversion(conexion, numOF.ToUpper, oPlant.IdBrain)
                '    Else
                '        Return bidaiakDAL.consultarOFPortador(conexion, numOF.ToUpper, oPlant.IdBrain)
                '    End If
                'Else
                '    Throw New BidaiakLib.BatzException("No tiene el campo de IdBrain informado", Nothing)
                'End If
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            End Try
        End Function
    End Class

End Namespace