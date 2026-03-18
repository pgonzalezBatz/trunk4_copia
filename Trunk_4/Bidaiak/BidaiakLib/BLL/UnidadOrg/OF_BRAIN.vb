Namespace BLL.UO

    Public Class OF_BRAIN
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
                Dim brainDAL As New DAL.BrainDAL
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(idPlanta)
                If (oPlant.IdBrain <> String.Empty) Then
                    numOF = numOF.ToLower
                    Dim ofBrain As String() = brainDAL.consultarOFInversion(conexion, numOF.ToUpper, oPlant.IdBrain)
                    If (ofBrain IsNot Nothing) Then
                        If (String.IsNullOrWhiteSpace(ofBrain(2))) Then
                            ofBrain(2) = "OBS"
                        End If
                        If (ofBrain(1).ToLower.IndexOf("obs") <> -1) Then
                            ofBrain(1) = ofBrain(1).Substring(0, ofBrain(1).Length - 4)
                            ofBrain(2) = "OBS"
                        End If
                        ofBrain(1) = ofBrain(1).Trim
                    End If
                    Return ofBrain
                    'If (numOF.StartsWith("y") Or numOF.StartsWith("n") Or numOF.StartsWith("p")) Then
                    '    Return brainDAL.consultarOFInversion(conexion, numOF.ToUpper, oPlant.IdBrain)
                    'Else
                    '    Return brainDAL.consultarOFPortador(conexion, numOF.ToUpper, oPlant.IdBrain)
                    'End If
                Else
                        Throw New BatzException("No tiene el campo de IdBrain informado", Nothing)
                End If
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al consultar los datos de la OF de Brain", ex)
            End Try
        End Function
    End Class

End Namespace