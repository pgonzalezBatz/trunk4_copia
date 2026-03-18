
'===============================================================================
'BATZ, Koop. - 08/09/2010 8:02:05
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_BusinessEntity.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD


NameSpace DAL

Public MustInherit Class _TICKET_MOV
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "TICKET_MOV"
			Me.MappingName = "TICKET_MOV"
		End Sub

	'=================================================================
	'  Public Overrides Sub AddNew()
	'=================================================================
	'
	'=================================================================
	Public Overrides Sub AddNew()
		MyBase.AddNew()
				
	End Sub
	
	Public Overrides Sub FlushData()
		Me._whereClause = nothing
		Me._aggregateClause = nothing		
		MyBase.FlushData()
	End Sub
	
		
	'=================================================================
	'  	Public Function LoadAll() As Boolean
	'=================================================================
	'  Loads all of the records in the database, and sets the currentRow to the first row
	'=================================================================
	Public Function LoadAll() As Boolean
	
		Dim parameters As ListDictionary = Nothing
		
		parameters = New ListDictionary		
		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_TICKET_MOV", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey() As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		
		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_TICKET_MOV", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property DET_CALIDAD_SUMINISTRADA As OracleParameter
			Get
				Return New OracleParameter("p_DET_CALIDAD_SUMINISTRADA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_CANTIDAD_MEDIDA_ORIGINADA As OracleParameter
			Get
				Return New OracleParameter("p_DET_CANTIDAD_MEDIDA_ORIGINADA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_CANTIDAD_MEDIDA_RECIBIDA As OracleParameter
			Get
				Return New OracleParameter("p_DET_CANTIDAD_MEDIDA_RECIBIDA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_COD_ABREVIATURA_LLAMADA As OracleParameter
			Get
				Return New OracleParameter("p_DET_COD_ABREVIATURA_LLAMADA", OracleDbType.NVarchar2, 3)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_COD_DESTINO As OracleParameter
			Get
				Return New OracleParameter("p_DET_COD_DESTINO", OracleDbType.NVarchar2, 3)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_COD_HORARIO As OracleParameter
			Get
				Return New OracleParameter("p_DET_COD_HORARIO", OracleDbType.NVarchar2, 2)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_COD_TRAFICO As OracleParameter
			Get
				Return New OracleParameter("p_DET_COD_TRAFICO", OracleDbType.NVarchar2, 3)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_DESCRIP_TIPO_LLAMADA As OracleParameter
			Get
				Return New OracleParameter("p_DET_DESCRIP_TIPO_LLAMADA", OracleDbType.NVarchar2, 30)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_FECHA As OracleParameter
			Get
				Return New OracleParameter("p_DET_FECHA", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_HORA_INICIO As OracleParameter
			Get
				Return New OracleParameter("p_DET_HORA_INICIO", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_IMPORTE As OracleParameter
			Get
				Return New OracleParameter("p_DET_IMPORTE", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_NU_EXTENSION As OracleParameter
			Get
				Return New OracleParameter("p_DET_NU_EXTENSION", OracleDbType.NVarchar2, 9)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_NU_TELEFONO As OracleParameter
			Get
				Return New OracleParameter("p_DET_NU_TELEFONO", OracleDbType.NVarchar2, 9)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_NUM_FACTURA As OracleParameter
			Get
				Return New OracleParameter("p_DET_NUM_FACTURA", OracleDbType.NVarchar2, 14)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_NUM_PREFACTURA As OracleParameter
			Get
				Return New OracleParameter("p_DET_NUM_PREFACTURA", OracleDbType.NVarchar2, 14)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_NUMERO_LLAMADO As OracleParameter
			Get
				Return New OracleParameter("p_DET_NUMERO_LLAMADO", OracleDbType.NVarchar2, 25)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_OPERADOR As OracleParameter
			Get
				Return New OracleParameter("p_DET_OPERADOR", OracleDbType.NVarchar2, 8)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_TIPO_DESTINO As OracleParameter
			Get
				Return New OracleParameter("p_DET_TIPO_DESTINO", OracleDbType.NVarchar2, 14)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_TIPO_TRAFICO As OracleParameter
			Get
				Return New OracleParameter("p_DET_TIPO_TRAFICO", OracleDbType.NVarchar2, 30)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_UND_CALIDAD_TRANSMISION As OracleParameter
			Get
				Return New OracleParameter("p_DET_UND_CALIDAD_TRANSMISION", OracleDbType.NVarchar2, 10)
			End Get
		End Property
		
		Public Shared ReadOnly Property DET_UNIDAD_MEDIDA As OracleParameter
			Get
				Return New OracleParameter("p_DET_UNIDAD_MEDIDA", OracleDbType.NVarchar2, 10)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const DET_CALIDAD_SUMINISTRADA As String = "DET_CALIDAD_SUMINISTRADA"
        Public Const DET_CANTIDAD_MEDIDA_ORIGINADA As String = "DET_CANTIDAD_MEDIDA_ORIGINADA"
        Public Const DET_CANTIDAD_MEDIDA_RECIBIDA As String = "DET_CANTIDAD_MEDIDA_RECIBIDA"
        Public Const DET_COD_ABREVIATURA_LLAMADA As String = "DET_COD_ABREVIATURA_LLAMADA"
        Public Const DET_COD_DESTINO As String = "DET_COD_DESTINO"
        Public Const DET_COD_HORARIO As String = "DET_COD_HORARIO"
        Public Const DET_COD_TRAFICO As String = "DET_COD_TRAFICO"
        Public Const DET_DESCRIP_TIPO_LLAMADA As String = "DET_DESCRIP_TIPO_LLAMADA"
        Public Const DET_FECHA As String = "DET_FECHA"
        Public Const DET_HORA_INICIO As String = "DET_HORA_INICIO"
        Public Const DET_IMPORTE As String = "DET_IMPORTE"
        Public Const DET_NU_EXTENSION As String = "DET_NU_EXTENSION"
        Public Const DET_NU_TELEFONO As String = "DET_NU_TELEFONO"
        Public Const DET_NUM_FACTURA As String = "DET_NUM_FACTURA"
        Public Const DET_NUM_PREFACTURA As String = "DET_NUM_PREFACTURA"
        Public Const DET_NUMERO_LLAMADO As String = "DET_NUMERO_LLAMADO"
        Public Const DET_OPERADOR As String = "DET_OPERADOR"
        Public Const DET_TIPO_DESTINO As String = "DET_TIPO_DESTINO"
        Public Const DET_TIPO_TRAFICO As String = "DET_TIPO_TRAFICO"
        Public Const DET_UND_CALIDAD_TRANSMISION As String = "DET_UND_CALIDAD_TRANSMISION"
        Public Const DET_UNIDAD_MEDIDA As String = "DET_UNIDAD_MEDIDA"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(DET_CALIDAD_SUMINISTRADA) = _TICKET_MOV.PropertyNames.DET_CALIDAD_SUMINISTRADA
				ht(DET_CANTIDAD_MEDIDA_ORIGINADA) = _TICKET_MOV.PropertyNames.DET_CANTIDAD_MEDIDA_ORIGINADA
				ht(DET_CANTIDAD_MEDIDA_RECIBIDA) = _TICKET_MOV.PropertyNames.DET_CANTIDAD_MEDIDA_RECIBIDA
				ht(DET_COD_ABREVIATURA_LLAMADA) = _TICKET_MOV.PropertyNames.DET_COD_ABREVIATURA_LLAMADA
				ht(DET_COD_DESTINO) = _TICKET_MOV.PropertyNames.DET_COD_DESTINO
				ht(DET_COD_HORARIO) = _TICKET_MOV.PropertyNames.DET_COD_HORARIO
				ht(DET_COD_TRAFICO) = _TICKET_MOV.PropertyNames.DET_COD_TRAFICO
				ht(DET_DESCRIP_TIPO_LLAMADA) = _TICKET_MOV.PropertyNames.DET_DESCRIP_TIPO_LLAMADA
				ht(DET_FECHA) = _TICKET_MOV.PropertyNames.DET_FECHA
				ht(DET_HORA_INICIO) = _TICKET_MOV.PropertyNames.DET_HORA_INICIO
				ht(DET_IMPORTE) = _TICKET_MOV.PropertyNames.DET_IMPORTE
				ht(DET_NU_EXTENSION) = _TICKET_MOV.PropertyNames.DET_NU_EXTENSION
				ht(DET_NU_TELEFONO) = _TICKET_MOV.PropertyNames.DET_NU_TELEFONO
				ht(DET_NUM_FACTURA) = _TICKET_MOV.PropertyNames.DET_NUM_FACTURA
				ht(DET_NUM_PREFACTURA) = _TICKET_MOV.PropertyNames.DET_NUM_PREFACTURA
				ht(DET_NUMERO_LLAMADO) = _TICKET_MOV.PropertyNames.DET_NUMERO_LLAMADO
				ht(DET_OPERADOR) = _TICKET_MOV.PropertyNames.DET_OPERADOR
				ht(DET_TIPO_DESTINO) = _TICKET_MOV.PropertyNames.DET_TIPO_DESTINO
				ht(DET_TIPO_TRAFICO) = _TICKET_MOV.PropertyNames.DET_TIPO_TRAFICO
				ht(DET_UND_CALIDAD_TRANSMISION) = _TICKET_MOV.PropertyNames.DET_UND_CALIDAD_TRANSMISION
				ht(DET_UNIDAD_MEDIDA) = _TICKET_MOV.PropertyNames.DET_UNIDAD_MEDIDA

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const DET_CALIDAD_SUMINISTRADA As String = "DET_CALIDAD_SUMINISTRADA"
        Public Const DET_CANTIDAD_MEDIDA_ORIGINADA As String = "DET_CANTIDAD_MEDIDA_ORIGINADA"
        Public Const DET_CANTIDAD_MEDIDA_RECIBIDA As String = "DET_CANTIDAD_MEDIDA_RECIBIDA"
        Public Const DET_COD_ABREVIATURA_LLAMADA As String = "DET_COD_ABREVIATURA_LLAMADA"
        Public Const DET_COD_DESTINO As String = "DET_COD_DESTINO"
        Public Const DET_COD_HORARIO As String = "DET_COD_HORARIO"
        Public Const DET_COD_TRAFICO As String = "DET_COD_TRAFICO"
        Public Const DET_DESCRIP_TIPO_LLAMADA As String = "DET_DESCRIP_TIPO_LLAMADA"
        Public Const DET_FECHA As String = "DET_FECHA"
        Public Const DET_HORA_INICIO As String = "DET_HORA_INICIO"
        Public Const DET_IMPORTE As String = "DET_IMPORTE"
        Public Const DET_NU_EXTENSION As String = "DET_NU_EXTENSION"
        Public Const DET_NU_TELEFONO As String = "DET_NU_TELEFONO"
        Public Const DET_NUM_FACTURA As String = "DET_NUM_FACTURA"
        Public Const DET_NUM_PREFACTURA As String = "DET_NUM_PREFACTURA"
        Public Const DET_NUMERO_LLAMADO As String = "DET_NUMERO_LLAMADO"
        Public Const DET_OPERADOR As String = "DET_OPERADOR"
        Public Const DET_TIPO_DESTINO As String = "DET_TIPO_DESTINO"
        Public Const DET_TIPO_TRAFICO As String = "DET_TIPO_TRAFICO"
        Public Const DET_UND_CALIDAD_TRANSMISION As String = "DET_UND_CALIDAD_TRANSMISION"
        Public Const DET_UNIDAD_MEDIDA As String = "DET_UNIDAD_MEDIDA"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(DET_CALIDAD_SUMINISTRADA) = _TICKET_MOV.ColumnNames.DET_CALIDAD_SUMINISTRADA
				ht(DET_CANTIDAD_MEDIDA_ORIGINADA) = _TICKET_MOV.ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA
				ht(DET_CANTIDAD_MEDIDA_RECIBIDA) = _TICKET_MOV.ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA
				ht(DET_COD_ABREVIATURA_LLAMADA) = _TICKET_MOV.ColumnNames.DET_COD_ABREVIATURA_LLAMADA
				ht(DET_COD_DESTINO) = _TICKET_MOV.ColumnNames.DET_COD_DESTINO
				ht(DET_COD_HORARIO) = _TICKET_MOV.ColumnNames.DET_COD_HORARIO
				ht(DET_COD_TRAFICO) = _TICKET_MOV.ColumnNames.DET_COD_TRAFICO
				ht(DET_DESCRIP_TIPO_LLAMADA) = _TICKET_MOV.ColumnNames.DET_DESCRIP_TIPO_LLAMADA
				ht(DET_FECHA) = _TICKET_MOV.ColumnNames.DET_FECHA
				ht(DET_HORA_INICIO) = _TICKET_MOV.ColumnNames.DET_HORA_INICIO
				ht(DET_IMPORTE) = _TICKET_MOV.ColumnNames.DET_IMPORTE
				ht(DET_NU_EXTENSION) = _TICKET_MOV.ColumnNames.DET_NU_EXTENSION
				ht(DET_NU_TELEFONO) = _TICKET_MOV.ColumnNames.DET_NU_TELEFONO
				ht(DET_NUM_FACTURA) = _TICKET_MOV.ColumnNames.DET_NUM_FACTURA
				ht(DET_NUM_PREFACTURA) = _TICKET_MOV.ColumnNames.DET_NUM_PREFACTURA
				ht(DET_NUMERO_LLAMADO) = _TICKET_MOV.ColumnNames.DET_NUMERO_LLAMADO
				ht(DET_OPERADOR) = _TICKET_MOV.ColumnNames.DET_OPERADOR
				ht(DET_TIPO_DESTINO) = _TICKET_MOV.ColumnNames.DET_TIPO_DESTINO
				ht(DET_TIPO_TRAFICO) = _TICKET_MOV.ColumnNames.DET_TIPO_TRAFICO
				ht(DET_UND_CALIDAD_TRANSMISION) = _TICKET_MOV.ColumnNames.DET_UND_CALIDAD_TRANSMISION
				ht(DET_UNIDAD_MEDIDA) = _TICKET_MOV.ColumnNames.DET_UNIDAD_MEDIDA

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const DET_CALIDAD_SUMINISTRADA As String = "s_DET_CALIDAD_SUMINISTRADA"
        Public Const DET_CANTIDAD_MEDIDA_ORIGINADA As String = "s_DET_CANTIDAD_MEDIDA_ORIGINADA"
        Public Const DET_CANTIDAD_MEDIDA_RECIBIDA As String = "s_DET_CANTIDAD_MEDIDA_RECIBIDA"
        Public Const DET_COD_ABREVIATURA_LLAMADA As String = "s_DET_COD_ABREVIATURA_LLAMADA"
        Public Const DET_COD_DESTINO As String = "s_DET_COD_DESTINO"
        Public Const DET_COD_HORARIO As String = "s_DET_COD_HORARIO"
        Public Const DET_COD_TRAFICO As String = "s_DET_COD_TRAFICO"
        Public Const DET_DESCRIP_TIPO_LLAMADA As String = "s_DET_DESCRIP_TIPO_LLAMADA"
        Public Const DET_FECHA As String = "s_DET_FECHA"
        Public Const DET_HORA_INICIO As String = "s_DET_HORA_INICIO"
        Public Const DET_IMPORTE As String = "s_DET_IMPORTE"
        Public Const DET_NU_EXTENSION As String = "s_DET_NU_EXTENSION"
        Public Const DET_NU_TELEFONO As String = "s_DET_NU_TELEFONO"
        Public Const DET_NUM_FACTURA As String = "s_DET_NUM_FACTURA"
        Public Const DET_NUM_PREFACTURA As String = "s_DET_NUM_PREFACTURA"
        Public Const DET_NUMERO_LLAMADO As String = "s_DET_NUMERO_LLAMADO"
        Public Const DET_OPERADOR As String = "s_DET_OPERADOR"
        Public Const DET_TIPO_DESTINO As String = "s_DET_TIPO_DESTINO"
        Public Const DET_TIPO_TRAFICO As String = "s_DET_TIPO_TRAFICO"
        Public Const DET_UND_CALIDAD_TRANSMISION As String = "s_DET_UND_CALIDAD_TRANSMISION"
        Public Const DET_UNIDAD_MEDIDA As String = "s_DET_UNIDAD_MEDIDA"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property DET_CALIDAD_SUMINISTRADA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.DET_CALIDAD_SUMINISTRADA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.DET_CALIDAD_SUMINISTRADA, Value)
			End Set
		End Property

		Public Overridable Property DET_CANTIDAD_MEDIDA_ORIGINADA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA, Value)
			End Set
		End Property

		Public Overridable Property DET_CANTIDAD_MEDIDA_RECIBIDA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA, Value)
			End Set
		End Property

		Public Overridable Property DET_COD_ABREVIATURA_LLAMADA As String
			Get
				Return MyBase.GetString(ColumnNames.DET_COD_ABREVIATURA_LLAMADA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_COD_ABREVIATURA_LLAMADA, Value)
			End Set
		End Property

		Public Overridable Property DET_COD_DESTINO As String
			Get
				Return MyBase.GetString(ColumnNames.DET_COD_DESTINO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_COD_DESTINO, Value)
			End Set
		End Property

		Public Overridable Property DET_COD_HORARIO As String
			Get
				Return MyBase.GetString(ColumnNames.DET_COD_HORARIO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_COD_HORARIO, Value)
			End Set
		End Property

		Public Overridable Property DET_COD_TRAFICO As String
			Get
				Return MyBase.GetString(ColumnNames.DET_COD_TRAFICO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_COD_TRAFICO, Value)
			End Set
		End Property

		Public Overridable Property DET_DESCRIP_TIPO_LLAMADA As String
			Get
				Return MyBase.GetString(ColumnNames.DET_DESCRIP_TIPO_LLAMADA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_DESCRIP_TIPO_LLAMADA, Value)
			End Set
		End Property

		Public Overridable Property DET_FECHA As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.DET_FECHA)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.DET_FECHA, Value)
			End Set
		End Property

		Public Overridable Property DET_HORA_INICIO As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.DET_HORA_INICIO)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.DET_HORA_INICIO, Value)
			End Set
		End Property

		Public Overridable Property DET_IMPORTE As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.DET_IMPORTE)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.DET_IMPORTE, Value)
			End Set
		End Property

		Public Overridable Property DET_NU_EXTENSION As String
			Get
				Return MyBase.GetString(ColumnNames.DET_NU_EXTENSION)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_NU_EXTENSION, Value)
			End Set
		End Property

		Public Overridable Property DET_NU_TELEFONO As String
			Get
				Return MyBase.GetString(ColumnNames.DET_NU_TELEFONO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_NU_TELEFONO, Value)
			End Set
		End Property

		Public Overridable Property DET_NUM_FACTURA As String
			Get
				Return MyBase.GetString(ColumnNames.DET_NUM_FACTURA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_NUM_FACTURA, Value)
			End Set
		End Property

		Public Overridable Property DET_NUM_PREFACTURA As String
			Get
				Return MyBase.GetString(ColumnNames.DET_NUM_PREFACTURA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_NUM_PREFACTURA, Value)
			End Set
		End Property

		Public Overridable Property DET_NUMERO_LLAMADO As String
			Get
				Return MyBase.GetString(ColumnNames.DET_NUMERO_LLAMADO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_NUMERO_LLAMADO, Value)
			End Set
		End Property

		Public Overridable Property DET_OPERADOR As String
			Get
				Return MyBase.GetString(ColumnNames.DET_OPERADOR)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_OPERADOR, Value)
			End Set
		End Property

		Public Overridable Property DET_TIPO_DESTINO As String
			Get
				Return MyBase.GetString(ColumnNames.DET_TIPO_DESTINO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_TIPO_DESTINO, Value)
			End Set
		End Property

		Public Overridable Property DET_TIPO_TRAFICO As String
			Get
				Return MyBase.GetString(ColumnNames.DET_TIPO_TRAFICO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_TIPO_TRAFICO, Value)
			End Set
		End Property

		Public Overridable Property DET_UND_CALIDAD_TRANSMISION As String
			Get
				Return MyBase.GetString(ColumnNames.DET_UND_CALIDAD_TRANSMISION)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_UND_CALIDAD_TRANSMISION, Value)
			End Set
		End Property

		Public Overridable Property DET_UNIDAD_MEDIDA As String
			Get
				Return MyBase.GetString(ColumnNames.DET_UNIDAD_MEDIDA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DET_UNIDAD_MEDIDA, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_DET_CALIDAD_SUMINISTRADA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_CALIDAD_SUMINISTRADA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.DET_CALIDAD_SUMINISTRADA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_CALIDAD_SUMINISTRADA)
				Else
					Me.DET_CALIDAD_SUMINISTRADA = MyBase.SetDecimalAsString(ColumnNames.DET_CALIDAD_SUMINISTRADA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_CANTIDAD_MEDIDA_ORIGINADA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA)
				Else
					Me.DET_CANTIDAD_MEDIDA_ORIGINADA = MyBase.SetDecimalAsString(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_CANTIDAD_MEDIDA_RECIBIDA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA)
				Else
					Me.DET_CANTIDAD_MEDIDA_RECIBIDA = MyBase.SetDecimalAsString(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_COD_ABREVIATURA_LLAMADA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_COD_ABREVIATURA_LLAMADA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_COD_ABREVIATURA_LLAMADA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_COD_ABREVIATURA_LLAMADA)
				Else
					Me.DET_COD_ABREVIATURA_LLAMADA = MyBase.SetStringAsString(ColumnNames.DET_COD_ABREVIATURA_LLAMADA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_COD_DESTINO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_COD_DESTINO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_COD_DESTINO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_COD_DESTINO)
				Else
					Me.DET_COD_DESTINO = MyBase.SetStringAsString(ColumnNames.DET_COD_DESTINO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_COD_HORARIO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_COD_HORARIO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_COD_HORARIO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_COD_HORARIO)
				Else
					Me.DET_COD_HORARIO = MyBase.SetStringAsString(ColumnNames.DET_COD_HORARIO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_COD_TRAFICO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_COD_TRAFICO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_COD_TRAFICO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_COD_TRAFICO)
				Else
					Me.DET_COD_TRAFICO = MyBase.SetStringAsString(ColumnNames.DET_COD_TRAFICO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_DESCRIP_TIPO_LLAMADA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_DESCRIP_TIPO_LLAMADA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_DESCRIP_TIPO_LLAMADA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_DESCRIP_TIPO_LLAMADA)
				Else
					Me.DET_DESCRIP_TIPO_LLAMADA = MyBase.SetStringAsString(ColumnNames.DET_DESCRIP_TIPO_LLAMADA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_FECHA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_FECHA) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.DET_FECHA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_FECHA)
				Else
					Me.DET_FECHA = MyBase.SetDateTimeAsString(ColumnNames.DET_FECHA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_HORA_INICIO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_HORA_INICIO) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.DET_HORA_INICIO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_HORA_INICIO)
				Else
					Me.DET_HORA_INICIO = MyBase.SetDateTimeAsString(ColumnNames.DET_HORA_INICIO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_IMPORTE As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_IMPORTE) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.DET_IMPORTE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_IMPORTE)
				Else
					Me.DET_IMPORTE = MyBase.SetDecimalAsString(ColumnNames.DET_IMPORTE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_NU_EXTENSION As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_NU_EXTENSION) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_NU_EXTENSION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_NU_EXTENSION)
				Else
					Me.DET_NU_EXTENSION = MyBase.SetStringAsString(ColumnNames.DET_NU_EXTENSION, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_NU_TELEFONO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_NU_TELEFONO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_NU_TELEFONO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_NU_TELEFONO)
				Else
					Me.DET_NU_TELEFONO = MyBase.SetStringAsString(ColumnNames.DET_NU_TELEFONO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_NUM_FACTURA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_NUM_FACTURA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_NUM_FACTURA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_NUM_FACTURA)
				Else
					Me.DET_NUM_FACTURA = MyBase.SetStringAsString(ColumnNames.DET_NUM_FACTURA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_NUM_PREFACTURA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_NUM_PREFACTURA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_NUM_PREFACTURA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_NUM_PREFACTURA)
				Else
					Me.DET_NUM_PREFACTURA = MyBase.SetStringAsString(ColumnNames.DET_NUM_PREFACTURA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_NUMERO_LLAMADO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_NUMERO_LLAMADO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_NUMERO_LLAMADO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_NUMERO_LLAMADO)
				Else
					Me.DET_NUMERO_LLAMADO = MyBase.SetStringAsString(ColumnNames.DET_NUMERO_LLAMADO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_OPERADOR As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_OPERADOR) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_OPERADOR)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_OPERADOR)
				Else
					Me.DET_OPERADOR = MyBase.SetStringAsString(ColumnNames.DET_OPERADOR, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_TIPO_DESTINO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_TIPO_DESTINO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_TIPO_DESTINO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_TIPO_DESTINO)
				Else
					Me.DET_TIPO_DESTINO = MyBase.SetStringAsString(ColumnNames.DET_TIPO_DESTINO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_TIPO_TRAFICO As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_TIPO_TRAFICO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_TIPO_TRAFICO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_TIPO_TRAFICO)
				Else
					Me.DET_TIPO_TRAFICO = MyBase.SetStringAsString(ColumnNames.DET_TIPO_TRAFICO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_UND_CALIDAD_TRANSMISION As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_UND_CALIDAD_TRANSMISION) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_UND_CALIDAD_TRANSMISION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_UND_CALIDAD_TRANSMISION)
				Else
					Me.DET_UND_CALIDAD_TRANSMISION = MyBase.SetStringAsString(ColumnNames.DET_UND_CALIDAD_TRANSMISION, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DET_UNIDAD_MEDIDA As String
			Get
				If Me.IsColumnNull(ColumnNames.DET_UNIDAD_MEDIDA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DET_UNIDAD_MEDIDA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DET_UNIDAD_MEDIDA)
				Else
					Me.DET_UNIDAD_MEDIDA = MyBase.SetStringAsString(ColumnNames.DET_UNIDAD_MEDIDA, Value)
				End If
			End Set
		End Property


	#End Region  	

	#Region "Where Clause"
    Public Class WhereClause

        Public Sub New(ByVal entity As BusinessEntity)
            Me._entity = entity
        End Sub
		
		Public ReadOnly Property TearOff As TearOffWhereParameter
			Get
				If _tearOff Is Nothing Then
					_tearOff = new TearOffWhereParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "TearOff's"
		Public class TearOffWhereParameter

			Public Sub New(ByVal clause As WhereClause)
				Me._clause = clause
			End Sub
		
	
			Public ReadOnly Property DET_CALIDAD_SUMINISTRADA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_CALIDAD_SUMINISTRADA, Parameters.DET_CALIDAD_SUMINISTRADA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_CANTIDAD_MEDIDA_ORIGINADA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA, Parameters.DET_CANTIDAD_MEDIDA_ORIGINADA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_CANTIDAD_MEDIDA_RECIBIDA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA, Parameters.DET_CANTIDAD_MEDIDA_RECIBIDA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_COD_ABREVIATURA_LLAMADA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_COD_ABREVIATURA_LLAMADA, Parameters.DET_COD_ABREVIATURA_LLAMADA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_COD_DESTINO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_COD_DESTINO, Parameters.DET_COD_DESTINO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_COD_HORARIO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_COD_HORARIO, Parameters.DET_COD_HORARIO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_COD_TRAFICO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_COD_TRAFICO, Parameters.DET_COD_TRAFICO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_DESCRIP_TIPO_LLAMADA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_DESCRIP_TIPO_LLAMADA, Parameters.DET_DESCRIP_TIPO_LLAMADA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_FECHA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_FECHA, Parameters.DET_FECHA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_HORA_INICIO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_HORA_INICIO, Parameters.DET_HORA_INICIO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_IMPORTE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_IMPORTE, Parameters.DET_IMPORTE)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_NU_EXTENSION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_NU_EXTENSION, Parameters.DET_NU_EXTENSION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_NU_TELEFONO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_NU_TELEFONO, Parameters.DET_NU_TELEFONO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_NUM_FACTURA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_NUM_FACTURA, Parameters.DET_NUM_FACTURA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_NUM_PREFACTURA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_NUM_PREFACTURA, Parameters.DET_NUM_PREFACTURA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_NUMERO_LLAMADO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_NUMERO_LLAMADO, Parameters.DET_NUMERO_LLAMADO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_OPERADOR() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_OPERADOR, Parameters.DET_OPERADOR)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_TIPO_DESTINO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_TIPO_DESTINO, Parameters.DET_TIPO_DESTINO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_TIPO_TRAFICO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_TIPO_TRAFICO, Parameters.DET_TIPO_TRAFICO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_UND_CALIDAD_TRANSMISION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_UND_CALIDAD_TRANSMISION, Parameters.DET_UND_CALIDAD_TRANSMISION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DET_UNIDAD_MEDIDA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DET_UNIDAD_MEDIDA, Parameters.DET_UNIDAD_MEDIDA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property DET_CALIDAD_SUMINISTRADA() As WhereParameter 
			Get
				If _DET_CALIDAD_SUMINISTRADA_W Is Nothing Then
					_DET_CALIDAD_SUMINISTRADA_W = TearOff.DET_CALIDAD_SUMINISTRADA
				End If
				Return _DET_CALIDAD_SUMINISTRADA_W
			End Get
		End Property

		Public ReadOnly Property DET_CANTIDAD_MEDIDA_ORIGINADA() As WhereParameter 
			Get
				If _DET_CANTIDAD_MEDIDA_ORIGINADA_W Is Nothing Then
					_DET_CANTIDAD_MEDIDA_ORIGINADA_W = TearOff.DET_CANTIDAD_MEDIDA_ORIGINADA
				End If
				Return _DET_CANTIDAD_MEDIDA_ORIGINADA_W
			End Get
		End Property

		Public ReadOnly Property DET_CANTIDAD_MEDIDA_RECIBIDA() As WhereParameter 
			Get
				If _DET_CANTIDAD_MEDIDA_RECIBIDA_W Is Nothing Then
					_DET_CANTIDAD_MEDIDA_RECIBIDA_W = TearOff.DET_CANTIDAD_MEDIDA_RECIBIDA
				End If
				Return _DET_CANTIDAD_MEDIDA_RECIBIDA_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_ABREVIATURA_LLAMADA() As WhereParameter 
			Get
				If _DET_COD_ABREVIATURA_LLAMADA_W Is Nothing Then
					_DET_COD_ABREVIATURA_LLAMADA_W = TearOff.DET_COD_ABREVIATURA_LLAMADA
				End If
				Return _DET_COD_ABREVIATURA_LLAMADA_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_DESTINO() As WhereParameter 
			Get
				If _DET_COD_DESTINO_W Is Nothing Then
					_DET_COD_DESTINO_W = TearOff.DET_COD_DESTINO
				End If
				Return _DET_COD_DESTINO_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_HORARIO() As WhereParameter 
			Get
				If _DET_COD_HORARIO_W Is Nothing Then
					_DET_COD_HORARIO_W = TearOff.DET_COD_HORARIO
				End If
				Return _DET_COD_HORARIO_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_TRAFICO() As WhereParameter 
			Get
				If _DET_COD_TRAFICO_W Is Nothing Then
					_DET_COD_TRAFICO_W = TearOff.DET_COD_TRAFICO
				End If
				Return _DET_COD_TRAFICO_W
			End Get
		End Property

		Public ReadOnly Property DET_DESCRIP_TIPO_LLAMADA() As WhereParameter 
			Get
				If _DET_DESCRIP_TIPO_LLAMADA_W Is Nothing Then
					_DET_DESCRIP_TIPO_LLAMADA_W = TearOff.DET_DESCRIP_TIPO_LLAMADA
				End If
				Return _DET_DESCRIP_TIPO_LLAMADA_W
			End Get
		End Property

		Public ReadOnly Property DET_FECHA() As WhereParameter 
			Get
				If _DET_FECHA_W Is Nothing Then
					_DET_FECHA_W = TearOff.DET_FECHA
				End If
				Return _DET_FECHA_W
			End Get
		End Property

		Public ReadOnly Property DET_HORA_INICIO() As WhereParameter 
			Get
				If _DET_HORA_INICIO_W Is Nothing Then
					_DET_HORA_INICIO_W = TearOff.DET_HORA_INICIO
				End If
				Return _DET_HORA_INICIO_W
			End Get
		End Property

		Public ReadOnly Property DET_IMPORTE() As WhereParameter 
			Get
				If _DET_IMPORTE_W Is Nothing Then
					_DET_IMPORTE_W = TearOff.DET_IMPORTE
				End If
				Return _DET_IMPORTE_W
			End Get
		End Property

		Public ReadOnly Property DET_NU_EXTENSION() As WhereParameter 
			Get
				If _DET_NU_EXTENSION_W Is Nothing Then
					_DET_NU_EXTENSION_W = TearOff.DET_NU_EXTENSION
				End If
				Return _DET_NU_EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property DET_NU_TELEFONO() As WhereParameter 
			Get
				If _DET_NU_TELEFONO_W Is Nothing Then
					_DET_NU_TELEFONO_W = TearOff.DET_NU_TELEFONO
				End If
				Return _DET_NU_TELEFONO_W
			End Get
		End Property

		Public ReadOnly Property DET_NUM_FACTURA() As WhereParameter 
			Get
				If _DET_NUM_FACTURA_W Is Nothing Then
					_DET_NUM_FACTURA_W = TearOff.DET_NUM_FACTURA
				End If
				Return _DET_NUM_FACTURA_W
			End Get
		End Property

		Public ReadOnly Property DET_NUM_PREFACTURA() As WhereParameter 
			Get
				If _DET_NUM_PREFACTURA_W Is Nothing Then
					_DET_NUM_PREFACTURA_W = TearOff.DET_NUM_PREFACTURA
				End If
				Return _DET_NUM_PREFACTURA_W
			End Get
		End Property

		Public ReadOnly Property DET_NUMERO_LLAMADO() As WhereParameter 
			Get
				If _DET_NUMERO_LLAMADO_W Is Nothing Then
					_DET_NUMERO_LLAMADO_W = TearOff.DET_NUMERO_LLAMADO
				End If
				Return _DET_NUMERO_LLAMADO_W
			End Get
		End Property

		Public ReadOnly Property DET_OPERADOR() As WhereParameter 
			Get
				If _DET_OPERADOR_W Is Nothing Then
					_DET_OPERADOR_W = TearOff.DET_OPERADOR
				End If
				Return _DET_OPERADOR_W
			End Get
		End Property

		Public ReadOnly Property DET_TIPO_DESTINO() As WhereParameter 
			Get
				If _DET_TIPO_DESTINO_W Is Nothing Then
					_DET_TIPO_DESTINO_W = TearOff.DET_TIPO_DESTINO
				End If
				Return _DET_TIPO_DESTINO_W
			End Get
		End Property

		Public ReadOnly Property DET_TIPO_TRAFICO() As WhereParameter 
			Get
				If _DET_TIPO_TRAFICO_W Is Nothing Then
					_DET_TIPO_TRAFICO_W = TearOff.DET_TIPO_TRAFICO
				End If
				Return _DET_TIPO_TRAFICO_W
			End Get
		End Property

		Public ReadOnly Property DET_UND_CALIDAD_TRANSMISION() As WhereParameter 
			Get
				If _DET_UND_CALIDAD_TRANSMISION_W Is Nothing Then
					_DET_UND_CALIDAD_TRANSMISION_W = TearOff.DET_UND_CALIDAD_TRANSMISION
				End If
				Return _DET_UND_CALIDAD_TRANSMISION_W
			End Get
		End Property

		Public ReadOnly Property DET_UNIDAD_MEDIDA() As WhereParameter 
			Get
				If _DET_UNIDAD_MEDIDA_W Is Nothing Then
					_DET_UNIDAD_MEDIDA_W = TearOff.DET_UNIDAD_MEDIDA
				End If
				Return _DET_UNIDAD_MEDIDA_W
			End Get
		End Property

		Private _DET_CALIDAD_SUMINISTRADA_W As WhereParameter = Nothing
		Private _DET_CANTIDAD_MEDIDA_ORIGINADA_W As WhereParameter = Nothing
		Private _DET_CANTIDAD_MEDIDA_RECIBIDA_W As WhereParameter = Nothing
		Private _DET_COD_ABREVIATURA_LLAMADA_W As WhereParameter = Nothing
		Private _DET_COD_DESTINO_W As WhereParameter = Nothing
		Private _DET_COD_HORARIO_W As WhereParameter = Nothing
		Private _DET_COD_TRAFICO_W As WhereParameter = Nothing
		Private _DET_DESCRIP_TIPO_LLAMADA_W As WhereParameter = Nothing
		Private _DET_FECHA_W As WhereParameter = Nothing
		Private _DET_HORA_INICIO_W As WhereParameter = Nothing
		Private _DET_IMPORTE_W As WhereParameter = Nothing
		Private _DET_NU_EXTENSION_W As WhereParameter = Nothing
		Private _DET_NU_TELEFONO_W As WhereParameter = Nothing
		Private _DET_NUM_FACTURA_W As WhereParameter = Nothing
		Private _DET_NUM_PREFACTURA_W As WhereParameter = Nothing
		Private _DET_NUMERO_LLAMADO_W As WhereParameter = Nothing
		Private _DET_OPERADOR_W As WhereParameter = Nothing
		Private _DET_TIPO_DESTINO_W As WhereParameter = Nothing
		Private _DET_TIPO_TRAFICO_W As WhereParameter = Nothing
		Private _DET_UND_CALIDAD_TRANSMISION_W As WhereParameter = Nothing
		Private _DET_UNIDAD_MEDIDA_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_DET_CALIDAD_SUMINISTRADA_W = Nothing
			_DET_CANTIDAD_MEDIDA_ORIGINADA_W = Nothing
			_DET_CANTIDAD_MEDIDA_RECIBIDA_W = Nothing
			_DET_COD_ABREVIATURA_LLAMADA_W = Nothing
			_DET_COD_DESTINO_W = Nothing
			_DET_COD_HORARIO_W = Nothing
			_DET_COD_TRAFICO_W = Nothing
			_DET_DESCRIP_TIPO_LLAMADA_W = Nothing
			_DET_FECHA_W = Nothing
			_DET_HORA_INICIO_W = Nothing
			_DET_IMPORTE_W = Nothing
			_DET_NU_EXTENSION_W = Nothing
			_DET_NU_TELEFONO_W = Nothing
			_DET_NUM_FACTURA_W = Nothing
			_DET_NUM_PREFACTURA_W = Nothing
			_DET_NUMERO_LLAMADO_W = Nothing
			_DET_OPERADOR_W = Nothing
			_DET_TIPO_DESTINO_W = Nothing
			_DET_TIPO_TRAFICO_W = Nothing
			_DET_UND_CALIDAD_TRANSMISION_W = Nothing
			_DET_UNIDAD_MEDIDA_W = Nothing
				Me._entity.Query.FlushWhereParameters()

			End Sub
	
		Private _entity As BusinessEntity
		Private _tearOff As TearOffWhereParameter
    End Class	

	Public ReadOnly Property Where() As WhereClause
		Get
			If _whereClause Is Nothing Then
				_whereClause = New WhereClause(Me)
			End If
	
			Return _whereClause
		End Get
	End Property
	
	Private _whereClause As WhereClause = Nothing	
#End Region	

#Region "Aggregate Clause"
    Public Class AggregateClause

        Public Sub New(ByVal entity As BusinessEntity)
            Me._entity = entity
        End Sub
		
		Public ReadOnly Property TearOff As TearOffAggregateParameter
			Get
				If _tearOff Is Nothing Then
					_tearOff = new TearOffAggregateParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "AggregateParameter TearOff's"
		Public class TearOffAggregateParameter

			Public Sub New(ByVal clause As AggregateClause)
				Me._clause = clause
			End Sub
		
	
		Public ReadOnly Property DET_CALIDAD_SUMINISTRADA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_CALIDAD_SUMINISTRADA, Parameters.DET_CALIDAD_SUMINISTRADA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_CANTIDAD_MEDIDA_ORIGINADA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA, Parameters.DET_CANTIDAD_MEDIDA_ORIGINADA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_CANTIDAD_MEDIDA_RECIBIDA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA, Parameters.DET_CANTIDAD_MEDIDA_RECIBIDA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_COD_ABREVIATURA_LLAMADA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_COD_ABREVIATURA_LLAMADA, Parameters.DET_COD_ABREVIATURA_LLAMADA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_COD_DESTINO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_COD_DESTINO, Parameters.DET_COD_DESTINO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_COD_HORARIO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_COD_HORARIO, Parameters.DET_COD_HORARIO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_COD_TRAFICO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_COD_TRAFICO, Parameters.DET_COD_TRAFICO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_DESCRIP_TIPO_LLAMADA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_DESCRIP_TIPO_LLAMADA, Parameters.DET_DESCRIP_TIPO_LLAMADA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_FECHA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_FECHA, Parameters.DET_FECHA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_HORA_INICIO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_HORA_INICIO, Parameters.DET_HORA_INICIO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_IMPORTE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_IMPORTE, Parameters.DET_IMPORTE)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_NU_EXTENSION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_NU_EXTENSION, Parameters.DET_NU_EXTENSION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_NU_TELEFONO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_NU_TELEFONO, Parameters.DET_NU_TELEFONO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_NUM_FACTURA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_NUM_FACTURA, Parameters.DET_NUM_FACTURA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_NUM_PREFACTURA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_NUM_PREFACTURA, Parameters.DET_NUM_PREFACTURA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_NUMERO_LLAMADO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_NUMERO_LLAMADO, Parameters.DET_NUMERO_LLAMADO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_OPERADOR() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_OPERADOR, Parameters.DET_OPERADOR)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_TIPO_DESTINO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_TIPO_DESTINO, Parameters.DET_TIPO_DESTINO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_TIPO_TRAFICO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_TIPO_TRAFICO, Parameters.DET_TIPO_TRAFICO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_UND_CALIDAD_TRANSMISION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_UND_CALIDAD_TRANSMISION, Parameters.DET_UND_CALIDAD_TRANSMISION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DET_UNIDAD_MEDIDA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DET_UNIDAD_MEDIDA, Parameters.DET_UNIDAD_MEDIDA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property DET_CALIDAD_SUMINISTRADA() As AggregateParameter 
			Get
				If _DET_CALIDAD_SUMINISTRADA_W Is Nothing Then
					_DET_CALIDAD_SUMINISTRADA_W = TearOff.DET_CALIDAD_SUMINISTRADA
				End If
				Return _DET_CALIDAD_SUMINISTRADA_W
			End Get
		End Property

		Public ReadOnly Property DET_CANTIDAD_MEDIDA_ORIGINADA() As AggregateParameter 
			Get
				If _DET_CANTIDAD_MEDIDA_ORIGINADA_W Is Nothing Then
					_DET_CANTIDAD_MEDIDA_ORIGINADA_W = TearOff.DET_CANTIDAD_MEDIDA_ORIGINADA
				End If
				Return _DET_CANTIDAD_MEDIDA_ORIGINADA_W
			End Get
		End Property

		Public ReadOnly Property DET_CANTIDAD_MEDIDA_RECIBIDA() As AggregateParameter 
			Get
				If _DET_CANTIDAD_MEDIDA_RECIBIDA_W Is Nothing Then
					_DET_CANTIDAD_MEDIDA_RECIBIDA_W = TearOff.DET_CANTIDAD_MEDIDA_RECIBIDA
				End If
				Return _DET_CANTIDAD_MEDIDA_RECIBIDA_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_ABREVIATURA_LLAMADA() As AggregateParameter 
			Get
				If _DET_COD_ABREVIATURA_LLAMADA_W Is Nothing Then
					_DET_COD_ABREVIATURA_LLAMADA_W = TearOff.DET_COD_ABREVIATURA_LLAMADA
				End If
				Return _DET_COD_ABREVIATURA_LLAMADA_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_DESTINO() As AggregateParameter 
			Get
				If _DET_COD_DESTINO_W Is Nothing Then
					_DET_COD_DESTINO_W = TearOff.DET_COD_DESTINO
				End If
				Return _DET_COD_DESTINO_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_HORARIO() As AggregateParameter 
			Get
				If _DET_COD_HORARIO_W Is Nothing Then
					_DET_COD_HORARIO_W = TearOff.DET_COD_HORARIO
				End If
				Return _DET_COD_HORARIO_W
			End Get
		End Property

		Public ReadOnly Property DET_COD_TRAFICO() As AggregateParameter 
			Get
				If _DET_COD_TRAFICO_W Is Nothing Then
					_DET_COD_TRAFICO_W = TearOff.DET_COD_TRAFICO
				End If
				Return _DET_COD_TRAFICO_W
			End Get
		End Property

		Public ReadOnly Property DET_DESCRIP_TIPO_LLAMADA() As AggregateParameter 
			Get
				If _DET_DESCRIP_TIPO_LLAMADA_W Is Nothing Then
					_DET_DESCRIP_TIPO_LLAMADA_W = TearOff.DET_DESCRIP_TIPO_LLAMADA
				End If
				Return _DET_DESCRIP_TIPO_LLAMADA_W
			End Get
		End Property

		Public ReadOnly Property DET_FECHA() As AggregateParameter 
			Get
				If _DET_FECHA_W Is Nothing Then
					_DET_FECHA_W = TearOff.DET_FECHA
				End If
				Return _DET_FECHA_W
			End Get
		End Property

		Public ReadOnly Property DET_HORA_INICIO() As AggregateParameter 
			Get
				If _DET_HORA_INICIO_W Is Nothing Then
					_DET_HORA_INICIO_W = TearOff.DET_HORA_INICIO
				End If
				Return _DET_HORA_INICIO_W
			End Get
		End Property

		Public ReadOnly Property DET_IMPORTE() As AggregateParameter 
			Get
				If _DET_IMPORTE_W Is Nothing Then
					_DET_IMPORTE_W = TearOff.DET_IMPORTE
				End If
				Return _DET_IMPORTE_W
			End Get
		End Property

		Public ReadOnly Property DET_NU_EXTENSION() As AggregateParameter 
			Get
				If _DET_NU_EXTENSION_W Is Nothing Then
					_DET_NU_EXTENSION_W = TearOff.DET_NU_EXTENSION
				End If
				Return _DET_NU_EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property DET_NU_TELEFONO() As AggregateParameter 
			Get
				If _DET_NU_TELEFONO_W Is Nothing Then
					_DET_NU_TELEFONO_W = TearOff.DET_NU_TELEFONO
				End If
				Return _DET_NU_TELEFONO_W
			End Get
		End Property

		Public ReadOnly Property DET_NUM_FACTURA() As AggregateParameter 
			Get
				If _DET_NUM_FACTURA_W Is Nothing Then
					_DET_NUM_FACTURA_W = TearOff.DET_NUM_FACTURA
				End If
				Return _DET_NUM_FACTURA_W
			End Get
		End Property

		Public ReadOnly Property DET_NUM_PREFACTURA() As AggregateParameter 
			Get
				If _DET_NUM_PREFACTURA_W Is Nothing Then
					_DET_NUM_PREFACTURA_W = TearOff.DET_NUM_PREFACTURA
				End If
				Return _DET_NUM_PREFACTURA_W
			End Get
		End Property

		Public ReadOnly Property DET_NUMERO_LLAMADO() As AggregateParameter 
			Get
				If _DET_NUMERO_LLAMADO_W Is Nothing Then
					_DET_NUMERO_LLAMADO_W = TearOff.DET_NUMERO_LLAMADO
				End If
				Return _DET_NUMERO_LLAMADO_W
			End Get
		End Property

		Public ReadOnly Property DET_OPERADOR() As AggregateParameter 
			Get
				If _DET_OPERADOR_W Is Nothing Then
					_DET_OPERADOR_W = TearOff.DET_OPERADOR
				End If
				Return _DET_OPERADOR_W
			End Get
		End Property

		Public ReadOnly Property DET_TIPO_DESTINO() As AggregateParameter 
			Get
				If _DET_TIPO_DESTINO_W Is Nothing Then
					_DET_TIPO_DESTINO_W = TearOff.DET_TIPO_DESTINO
				End If
				Return _DET_TIPO_DESTINO_W
			End Get
		End Property

		Public ReadOnly Property DET_TIPO_TRAFICO() As AggregateParameter 
			Get
				If _DET_TIPO_TRAFICO_W Is Nothing Then
					_DET_TIPO_TRAFICO_W = TearOff.DET_TIPO_TRAFICO
				End If
				Return _DET_TIPO_TRAFICO_W
			End Get
		End Property

		Public ReadOnly Property DET_UND_CALIDAD_TRANSMISION() As AggregateParameter 
			Get
				If _DET_UND_CALIDAD_TRANSMISION_W Is Nothing Then
					_DET_UND_CALIDAD_TRANSMISION_W = TearOff.DET_UND_CALIDAD_TRANSMISION
				End If
				Return _DET_UND_CALIDAD_TRANSMISION_W
			End Get
		End Property

		Public ReadOnly Property DET_UNIDAD_MEDIDA() As AggregateParameter 
			Get
				If _DET_UNIDAD_MEDIDA_W Is Nothing Then
					_DET_UNIDAD_MEDIDA_W = TearOff.DET_UNIDAD_MEDIDA
				End If
				Return _DET_UNIDAD_MEDIDA_W
			End Get
		End Property

		Private _DET_CALIDAD_SUMINISTRADA_W As AggregateParameter = Nothing
		Private _DET_CANTIDAD_MEDIDA_ORIGINADA_W As AggregateParameter = Nothing
		Private _DET_CANTIDAD_MEDIDA_RECIBIDA_W As AggregateParameter = Nothing
		Private _DET_COD_ABREVIATURA_LLAMADA_W As AggregateParameter = Nothing
		Private _DET_COD_DESTINO_W As AggregateParameter = Nothing
		Private _DET_COD_HORARIO_W As AggregateParameter = Nothing
		Private _DET_COD_TRAFICO_W As AggregateParameter = Nothing
		Private _DET_DESCRIP_TIPO_LLAMADA_W As AggregateParameter = Nothing
		Private _DET_FECHA_W As AggregateParameter = Nothing
		Private _DET_HORA_INICIO_W As AggregateParameter = Nothing
		Private _DET_IMPORTE_W As AggregateParameter = Nothing
		Private _DET_NU_EXTENSION_W As AggregateParameter = Nothing
		Private _DET_NU_TELEFONO_W As AggregateParameter = Nothing
		Private _DET_NUM_FACTURA_W As AggregateParameter = Nothing
		Private _DET_NUM_PREFACTURA_W As AggregateParameter = Nothing
		Private _DET_NUMERO_LLAMADO_W As AggregateParameter = Nothing
		Private _DET_OPERADOR_W As AggregateParameter = Nothing
		Private _DET_TIPO_DESTINO_W As AggregateParameter = Nothing
		Private _DET_TIPO_TRAFICO_W As AggregateParameter = Nothing
		Private _DET_UND_CALIDAD_TRANSMISION_W As AggregateParameter = Nothing
		Private _DET_UNIDAD_MEDIDA_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_DET_CALIDAD_SUMINISTRADA_W = Nothing
		_DET_CANTIDAD_MEDIDA_ORIGINADA_W = Nothing
		_DET_CANTIDAD_MEDIDA_RECIBIDA_W = Nothing
		_DET_COD_ABREVIATURA_LLAMADA_W = Nothing
		_DET_COD_DESTINO_W = Nothing
		_DET_COD_HORARIO_W = Nothing
		_DET_COD_TRAFICO_W = Nothing
		_DET_DESCRIP_TIPO_LLAMADA_W = Nothing
		_DET_FECHA_W = Nothing
		_DET_HORA_INICIO_W = Nothing
		_DET_IMPORTE_W = Nothing
		_DET_NU_EXTENSION_W = Nothing
		_DET_NU_TELEFONO_W = Nothing
		_DET_NUM_FACTURA_W = Nothing
		_DET_NUM_PREFACTURA_W = Nothing
		_DET_NUMERO_LLAMADO_W = Nothing
		_DET_OPERADOR_W = Nothing
		_DET_TIPO_DESTINO_W = Nothing
		_DET_TIPO_TRAFICO_W = Nothing
		_DET_UND_CALIDAD_TRANSMISION_W = Nothing
		_DET_UNIDAD_MEDIDA_W = Nothing
			Me._entity.Query.FlushAggregateParameters()

		End Sub
	
		Private _entity As BusinessEntity
		Private _tearOff As TearOffAggregateParameter
    End Class	

	Public ReadOnly Property Aggregate() As AggregateClause
		Get
			If _aggregateClause Is Nothing Then
				_aggregateClause = New AggregateClause(Me)
			End If
	
			Return _aggregateClause
		End Get
	End Property
	
	Private _aggregateClause As AggregateClause = Nothing	
#End Region	

	Protected Overrides Function GetInsertCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_TICKET_MOV" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_TICKET_MOV" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_TICKET_MOV" 
		
            Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.DET_CALIDAD_SUMINISTRADA)
		p.SourceColumn = ColumnNames.DET_CALIDAD_SUMINISTRADA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_CANTIDAD_MEDIDA_ORIGINADA)
		p.SourceColumn = ColumnNames.DET_CANTIDAD_MEDIDA_ORIGINADA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_CANTIDAD_MEDIDA_RECIBIDA)
		p.SourceColumn = ColumnNames.DET_CANTIDAD_MEDIDA_RECIBIDA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_COD_ABREVIATURA_LLAMADA)
		p.SourceColumn = ColumnNames.DET_COD_ABREVIATURA_LLAMADA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_COD_DESTINO)
		p.SourceColumn = ColumnNames.DET_COD_DESTINO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_COD_HORARIO)
		p.SourceColumn = ColumnNames.DET_COD_HORARIO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_COD_TRAFICO)
		p.SourceColumn = ColumnNames.DET_COD_TRAFICO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_DESCRIP_TIPO_LLAMADA)
		p.SourceColumn = ColumnNames.DET_DESCRIP_TIPO_LLAMADA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_FECHA)
		p.SourceColumn = ColumnNames.DET_FECHA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_HORA_INICIO)
		p.SourceColumn = ColumnNames.DET_HORA_INICIO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_IMPORTE)
		p.SourceColumn = ColumnNames.DET_IMPORTE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_NU_EXTENSION)
		p.SourceColumn = ColumnNames.DET_NU_EXTENSION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_NU_TELEFONO)
		p.SourceColumn = ColumnNames.DET_NU_TELEFONO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_NUM_FACTURA)
		p.SourceColumn = ColumnNames.DET_NUM_FACTURA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_NUM_PREFACTURA)
		p.SourceColumn = ColumnNames.DET_NUM_PREFACTURA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_NUMERO_LLAMADO)
		p.SourceColumn = ColumnNames.DET_NUMERO_LLAMADO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_OPERADOR)
		p.SourceColumn = ColumnNames.DET_OPERADOR
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_TIPO_DESTINO)
		p.SourceColumn = ColumnNames.DET_TIPO_DESTINO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_TIPO_TRAFICO)
		p.SourceColumn = ColumnNames.DET_TIPO_TRAFICO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_UND_CALIDAD_TRANSMISION)
		p.SourceColumn = ColumnNames.DET_UND_CALIDAD_TRANSMISION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DET_UNIDAD_MEDIDA)
		p.SourceColumn = ColumnNames.DET_UNIDAD_MEDIDA
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

