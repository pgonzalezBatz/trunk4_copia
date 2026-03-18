Imports System.Reflection

Public Class Funciones
	''' <summary>
	''' Proceso para el copiado de los valores de las propiedades de un objeto en otro.
	''' Cargamos los datos de los campos coincidentes del objeto Origen al Destino.
	''' </summary>
	''' <param name="Origen">Objeto donde 'OBTENEMOS' los valores.</param>
	''' <param name="Destino">Objeto donde 'CARGAMOS' los valores.</param>
	''' <remarks></remarks>
	Public Sub CopiarPropiedades(ByVal Origen As Object, ByVal Destino As Object)
		'------------------------------------------------------------------------------
		'El objeto "Origen" es la "clase base" (BaseType) del objeto "Destino".
		'El objeto "Destino" puede esconder propiedades del objeto "Origen".
		'Para evitar que algunos campos se queden sin datos, cogemos los valores del "Origen" y se los pasamos a la "clase base" del destino.
		'------------------------------------------------------------------------------
		For Each Propiedad As PropertyInfo In Destino.GetType.BaseType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
			If Propiedad.GetSetMethod(True) IsNot Nothing Then
				For Each rPropiedad As PropertyInfo In Origen.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
					If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
						Propiedad.SetValue(Destino, rPropiedad.GetValue(Origen, Nothing), Nothing)
					End If
				Next
			End If
		Next
		'------------------------------------------------------------------------------
	End Sub

End Class
