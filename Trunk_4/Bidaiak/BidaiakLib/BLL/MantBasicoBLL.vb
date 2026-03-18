Namespace BLL

    Public Class MantBasicoBLL

        Private mantBasicoDAL As DAL.MantBasicoDAL

#Region "Constructor"

        Private _tipo As IMantBasico.eTipo = IMantBasico.eTipo.Ninguno

        ''' <summary>
        ''' Indica la clase que representa para saber contra que tabla tiene que operar
        ''' </summary>        
        Public Sub New(ByVal tipo As IMantBasico.eTipo, ByVal idPlanta As Integer)
            _tipo = tipo
            mantBasicoDAL = New DAL.MantBasicoDAL(tipo, idPlanta)
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un objeto de mantenimiento basico
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.MantBasico
            Return mantBasicoDAL.loadInfo(id)
        End Function

        ''' <summary>
        ''' Obtiene el listado de  objetos mantenimiento
        ''' </summary>
        ''' <param name="bVigentes">Parametro opcional que indica si se obtendran todos o solo los vigentes</param>
        ''' <returns></returns>        
        Public Function loadList(Optional ByVal bVigentes As Boolean = False) As List(Of ELL.MantBasico)
            Return mantBasicoDAL.loadList(bVigentes)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el objeto mantBasico
        ''' </summary>
        ''' <param name="oMantB">Objeto con la informacion</param>        
        Public Sub Save(ByVal oMantB As ELL.MantBasico)
            mantBasicoDAL.Save(oMantB)
        End Sub

        ''' <summary>
        ''' Marca como obsoleto un objeto
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            mantBasicoDAL.Delete(id)
        End Sub

#End Region


    End Class

End Namespace