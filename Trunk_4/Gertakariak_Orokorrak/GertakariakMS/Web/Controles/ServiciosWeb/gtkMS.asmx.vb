Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
 Public Class gtkMS
	Inherits System.Web.Services.WebService

	<WebMethod()> _
	Public Function HelloWorld() As String
		Return "Kaixo Abel"
	End Function

	''' <summary>
	''' Funcion para crear incidencias de "Mantenimiento de Sistemas"
	''' </summary>
	''' <param name="IDCreador">Identificador del usuario en SAB</param>
	''' <param name="DescProblema">Descripcion de la incidencia.</param>
	''' <param name="IdActivo">Identificador de la Instalacion (Linea).</param>
	''' <param name="IdEstructura">Identificador del tipo de incidencia (Auditoria)</param>
	''' <param name="DescAccion">Descripcion de la accion.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function CrearIncidencia(ByVal IDCreador As Integer, ByVal DescProblema As String, ByVal IdActivo As String, ByVal IdEstructura As Integer, ByVal DescAccion As String) As Integer
        Dim BBDD As New BatzBBDD.Entities_Gertakariak
        Dim GERTAKARIAK As New BatzBBDD.GERTAKARIAK

		'------------------------------------------------------------------
		'Incidencia
		'------------------------------------------------------------------
		GERTAKARIAK.IDCREADOR = IDCreador
		GERTAKARIAK.DESCRIPCIONPROBLEMA = DescProblema.Trim.Replace(vbLf, vbCrLf)
		GERTAKARIAK.IDACTIVO = IdActivo
		GERTAKARIAK.IDTIPOINCIDENCIA = 6 'Incidencias Mantenimiento de Sistemas
		GERTAKARIAK.FECHAAPERTURA = Now.Date
		'------------------------------------------------------------------

		'------------------------------------------------------------------
		'Responsable
		'------------------------------------------------------------------
        GERTAKARIAK.RESPONSABLES_GERTAKARIAK.Add(New BatzBBDD.RESPONSABLES_GERTAKARIAK With {.IDUSUARIO = IDCreador})
		'------------------------------------------------------------------

		'------------------------------------------------------------------
		'Caracteristica (Estructura)
		'------------------------------------------------------------------
        GERTAKARIAK.ESTRUCTURA.Add((From Est As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Est.ID = IdEstructura Select Est).SingleOrDefault)
		'------------------------------------------------------------------

		'------------------------------------------------------------------
		'Accion
		'------------------------------------------------------------------
        GERTAKARIAK.ACCIONES.Add(New BatzBBDD.ACCIONES With {.DESCRIPCION = DescAccion.Trim.Replace(vbLf, vbCrLf), .FECHAINICIO = Now.Date})
		'------------------------------------------------------------------

		BBDD.GERTAKARIAK.AddObject(GERTAKARIAK)
		BBDD.SaveChanges()

		Return GERTAKARIAK.ID
	End Function

End Class