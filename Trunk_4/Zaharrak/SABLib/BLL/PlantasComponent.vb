Namespace BLL

    Public Class PlantasComponent
        Implements [Interface].IPlantasComponent

#Region "Consultas"

        ''' <summary>
        ''' Devuelve la informacion de la planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta </param>
        ''' <returns>Objeto planta</returns>
        ''' <remarks></remarks>
        Public Function GetPlanta(ByVal idPlanta As Integer) As ELL.Planta Implements [Interface].IPlantasComponent.GetPlanta
            Dim oPlanta As New ELL.Planta
            Dim plantasDAL As New DAL.PLANTAS

            plantasDAL.Where.ID.Value = idPlanta
            plantasDAL.Where.OBSOLETO.Value = 0
            plantasDAL.Query.Load()

            If plantasDAL.RowCount = 1 Then
                oPlanta = getObject(plantasDAL)
            End If
            Return oPlanta
        End Function


        ''' <summary>
        ''' Devuelve las plantas existentes
        ''' </summary>
        ''' <returns>Lista de plantas</returns>
        ''' <remarks></remarks>
        Public Function GetPlantas() As System.Collections.Generic.List(Of ELL.Planta) Implements [Interface].IPlantasComponent.GetPlantas
            Dim plantas As New List(Of ELL.Planta)
            Dim plantasDAL As New DAL.PLANTAS

            plantasDAL.Where.OBSOLETO.Value = 0
            plantasDAL.Query.Load()

            If plantasDAL.RowCount > 0 Then
                Do
                    plantas.Add(getObject(plantasDAL))
                Loop While plantasDAL.MoveNext()
            End If
            Return plantas
        End Function


        ''' <summary>
        ''' Devuelve las plantas en las que esta asociado un usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns>Lista de plantas</returns>
        ''' <remarks></remarks>
        Public Function GetPlantas(ByVal idUsuario As Integer) As System.Collections.Generic.List(Of ELL.Planta) Implements [Interface].IPlantasComponent.GetPlantasUsuario
            Dim plantas As New List(Of ELL.Planta)
            Try
                Dim oPlanta As ELL.Planta
                Dim usuPlantasDAL As New DAL.USUARIOS_PLANTAS

                usuPlantasDAL.Where.ID_USUARIO.Value = idUsuario
                usuPlantasDAL.Query.Load()

                If usuPlantasDAL.RowCount > 0 Then
                    Do
                        oPlanta = GetPlanta(usuPlantasDAL.ID_PLANTA)
                        plantas.Add(oPlanta)
                    Loop While usuPlantasDAL.MoveNext()
                End If
            Catch

            End Try
            Return plantas
        End Function


        ''' <summary>
        ''' A partir de un objeto mygeneration, devuelve un objeto planta
        ''' </summary>
        ''' <param name="plantaDAL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getObject(ByVal plantaDAL As DAL.PLANTAS) As ELL.Planta
            Dim oPlanta As New ELL.Planta
            oPlanta.Id = plantaDAL.ID
            oPlanta.Nombre = plantaDAL.NOMBRE
            oPlanta.Descripcion = plantaDAL.DESCRIP
            oPlanta.Dominio = plantaDAL.DOMINIO
            oPlanta.Obsoleto = plantaDAL.OBSOLETO
            Return oPlanta
        End Function

#End Region

#Region "Save"

        ''' <summary>
        ''' Inserta o modifica la planta
        ''' </summary>
        ''' <param name="oPlanta">Planta a guardar o modificar</param>        
        ''' <returns>Booleano</returns>
        Public Function Save(ByVal oPlanta As ELL.Planta) As Boolean Implements [Interface].IPlantasComponent.Save
            Try
                Dim plantaDAL As New DAL.PLANTAS

                If (oPlanta.Id = Integer.MinValue) Then
                    plantaDAL.AddNew()
                Else
                    plantaDAL.LoadByPrimaryKey(oPlanta.Id)
                End If

                If (plantaDAL.RowCount = 1) Then
                    plantaDAL.NOMBRE = oPlanta.Nombre
                    plantaDAL.DESCRIP = oPlanta.Descripcion
                    plantaDAL.DOMINIO = oPlanta.Dominio
                    plantaDAL.OBSOLETO = oPlanta.Obsoleto
                    plantaDAL.Save()
                    Return True
                End If
                Return False
            Catch
                Return False
            End Try
        End Function

#End Region

#Region "Delete"

        ''' <summary>
        ''' Marca como obsoleto la planta
        ''' </summary>
        ''' <param name="idPlant">Identificador de la planta</param>
        ''' <returns>Booleano que indica si se ha eliminado o no</returns>        
        Public Function Delete(ByVal idPlant As Integer) As Boolean Implements [Interface].IPlantasComponent.Delete
            Dim plantaDAL As New DAL.PLANTAS
            plantaDAL.LoadByPrimaryKey(idPlant)
            If plantaDAL.RowCount = 1 Then
                plantaDAL.OBSOLETO = 1
                Try
                    plantaDAL.Save()
                Catch
                    Return False
                End Try
                Return True
            End If
            Return False
        End Function

#End Region

    End Class

End Namespace
