
'===============================================================================
'BATZ, Koop. - 08/09/2010 8:02:04
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

Public MustInherit Class _FACTURAS_MOVILES
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "FACTURAS_MOVILES"
			Me.MappingName = "FACTURAS_MOVILES"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_FACTURAS_MOVILES", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_FACTURAS_MOVILES.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_FACTURAS_MOVILES", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property TELEFONO As OracleParameter
			Get
				Return New OracleParameter("p_TELEFONO", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property EXTENSION As OracleParameter
			Get
				Return New OracleParameter("p_EXTENSION", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property TRAFICO As OracleParameter
			Get
				Return New OracleParameter("p_TRAFICO", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property TIPO_LLAMADA As OracleParameter
			Get
				Return New OracleParameter("p_TIPO_LLAMADA", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property TIPO_DESTINO As OracleParameter
			Get
				Return New OracleParameter("p_TIPO_DESTINO", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property NUMERO_LLAMADO As OracleParameter
			Get
				Return New OracleParameter("p_NUMERO_LLAMADO", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property FECHA As OracleParameter
			Get
				Return New OracleParameter("p_FECHA", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property HORA As OracleParameter
			Get
				Return New OracleParameter("p_HORA", OracleDbType.VARCHAR2, 50)
			End Get
		End Property
		
		Public Shared ReadOnly Property TIEMPO As OracleParameter
			Get
				Return New OracleParameter("p_TIEMPO", OracleDBType.Decimal, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property IMPORTE As OracleParameter
			Get
				Return New OracleParameter("p_IMPORTE", OracleDBType.Decimal, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TLFNO As OracleParameter
			Get
				Return New OracleParameter("p_ID_TLFNO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_EXTENSION As OracleParameter
			Get
				Return New OracleParameter("p_ID_EXTENSION", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property FECHA_INSERCION As OracleParameter
			Get
				Return New OracleParameter("p_FECHA_INSERCION", OracleDbType.Date, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const TELEFONO As String = "TELEFONO"
        Public Const EXTENSION As String = "EXTENSION"
        Public Const TRAFICO As String = "TRAFICO"
        Public Const TIPO_LLAMADA As String = "TIPO_LLAMADA"
        Public Const TIPO_DESTINO As String = "TIPO_DESTINO"
        Public Const NUMERO_LLAMADO As String = "NUMERO_LLAMADO"
        Public Const FECHA As String = "FECHA"
        Public Const HORA As String = "HORA"
        Public Const TIEMPO As String = "TIEMPO"
        Public Const IMPORTE As String = "IMPORTE"
        Public Const ID_TLFNO As String = "ID_TLFNO"
        Public Const ID_EXTENSION As String = "ID_EXTENSION"
        Public Const ID As String = "ID"
        Public Const FECHA_INSERCION As String = "FECHA_INSERCION"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(TELEFONO) = _FACTURAS_MOVILES.PropertyNames.TELEFONO
				ht(EXTENSION) = _FACTURAS_MOVILES.PropertyNames.EXTENSION
				ht(TRAFICO) = _FACTURAS_MOVILES.PropertyNames.TRAFICO
				ht(TIPO_LLAMADA) = _FACTURAS_MOVILES.PropertyNames.TIPO_LLAMADA
				ht(TIPO_DESTINO) = _FACTURAS_MOVILES.PropertyNames.TIPO_DESTINO
				ht(NUMERO_LLAMADO) = _FACTURAS_MOVILES.PropertyNames.NUMERO_LLAMADO
				ht(FECHA) = _FACTURAS_MOVILES.PropertyNames.FECHA
				ht(HORA) = _FACTURAS_MOVILES.PropertyNames.HORA
				ht(TIEMPO) = _FACTURAS_MOVILES.PropertyNames.TIEMPO
				ht(IMPORTE) = _FACTURAS_MOVILES.PropertyNames.IMPORTE
				ht(ID_TLFNO) = _FACTURAS_MOVILES.PropertyNames.ID_TLFNO
				ht(ID_EXTENSION) = _FACTURAS_MOVILES.PropertyNames.ID_EXTENSION
				ht(ID) = _FACTURAS_MOVILES.PropertyNames.ID
				ht(FECHA_INSERCION) = _FACTURAS_MOVILES.PropertyNames.FECHA_INSERCION

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const TELEFONO As String = "TELEFONO"
        Public Const EXTENSION As String = "EXTENSION"
        Public Const TRAFICO As String = "TRAFICO"
        Public Const TIPO_LLAMADA As String = "TIPO_LLAMADA"
        Public Const TIPO_DESTINO As String = "TIPO_DESTINO"
        Public Const NUMERO_LLAMADO As String = "NUMERO_LLAMADO"
        Public Const FECHA As String = "FECHA"
        Public Const HORA As String = "HORA"
        Public Const TIEMPO As String = "TIEMPO"
        Public Const IMPORTE As String = "IMPORTE"
        Public Const ID_TLFNO As String = "ID_TLFNO"
        Public Const ID_EXTENSION As String = "ID_EXTENSION"
        Public Const ID As String = "ID"
        Public Const FECHA_INSERCION As String = "FECHA_INSERCION"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(TELEFONO) = _FACTURAS_MOVILES.ColumnNames.TELEFONO
				ht(EXTENSION) = _FACTURAS_MOVILES.ColumnNames.EXTENSION
				ht(TRAFICO) = _FACTURAS_MOVILES.ColumnNames.TRAFICO
				ht(TIPO_LLAMADA) = _FACTURAS_MOVILES.ColumnNames.TIPO_LLAMADA
				ht(TIPO_DESTINO) = _FACTURAS_MOVILES.ColumnNames.TIPO_DESTINO
				ht(NUMERO_LLAMADO) = _FACTURAS_MOVILES.ColumnNames.NUMERO_LLAMADO
				ht(FECHA) = _FACTURAS_MOVILES.ColumnNames.FECHA
				ht(HORA) = _FACTURAS_MOVILES.ColumnNames.HORA
				ht(TIEMPO) = _FACTURAS_MOVILES.ColumnNames.TIEMPO
				ht(IMPORTE) = _FACTURAS_MOVILES.ColumnNames.IMPORTE
				ht(ID_TLFNO) = _FACTURAS_MOVILES.ColumnNames.ID_TLFNO
				ht(ID_EXTENSION) = _FACTURAS_MOVILES.ColumnNames.ID_EXTENSION
				ht(ID) = _FACTURAS_MOVILES.ColumnNames.ID
				ht(FECHA_INSERCION) = _FACTURAS_MOVILES.ColumnNames.FECHA_INSERCION

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const TELEFONO As String = "s_TELEFONO"
        Public Const EXTENSION As String = "s_EXTENSION"
        Public Const TRAFICO As String = "s_TRAFICO"
        Public Const TIPO_LLAMADA As String = "s_TIPO_LLAMADA"
        Public Const TIPO_DESTINO As String = "s_TIPO_DESTINO"
        Public Const NUMERO_LLAMADO As String = "s_NUMERO_LLAMADO"
        Public Const FECHA As String = "s_FECHA"
        Public Const HORA As String = "s_HORA"
        Public Const TIEMPO As String = "s_TIEMPO"
        Public Const IMPORTE As String = "s_IMPORTE"
        Public Const ID_TLFNO As String = "s_ID_TLFNO"
        Public Const ID_EXTENSION As String = "s_ID_EXTENSION"
        Public Const ID As String = "s_ID"
        Public Const FECHA_INSERCION As String = "s_FECHA_INSERCION"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property TELEFONO As String
			Get
				Return MyBase.GetString(ColumnNames.TELEFONO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.TELEFONO, Value)
			End Set
		End Property

		Public Overridable Property EXTENSION As String
			Get
				Return MyBase.GetString(ColumnNames.EXTENSION)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.EXTENSION, Value)
			End Set
		End Property

		Public Overridable Property TRAFICO As String
			Get
				Return MyBase.GetString(ColumnNames.TRAFICO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.TRAFICO, Value)
			End Set
		End Property

		Public Overridable Property TIPO_LLAMADA As String
			Get
				Return MyBase.GetString(ColumnNames.TIPO_LLAMADA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.TIPO_LLAMADA, Value)
			End Set
		End Property

		Public Overridable Property TIPO_DESTINO As String
			Get
				Return MyBase.GetString(ColumnNames.TIPO_DESTINO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.TIPO_DESTINO, Value)
			End Set
		End Property

		Public Overridable Property NUMERO_LLAMADO As String
			Get
				Return MyBase.GetString(ColumnNames.NUMERO_LLAMADO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.NUMERO_LLAMADO, Value)
			End Set
		End Property

		Public Overridable Property FECHA As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.FECHA)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.FECHA, Value)
			End Set
		End Property

		Public Overridable Property HORA As String
			Get
				Return MyBase.GetString(ColumnNames.HORA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.HORA, Value)
			End Set
		End Property

		Public Overridable Property TIEMPO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.TIEMPO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.TIEMPO, Value)
			End Set
		End Property

		Public Overridable Property IMPORTE As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IMPORTE)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IMPORTE, Value)
			End Set
		End Property

		Public Overridable Property ID_TLFNO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TLFNO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TLFNO, Value)
			End Set
		End Property

		Public Overridable Property ID_EXTENSION As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_EXTENSION)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_EXTENSION, Value)
			End Set
		End Property

		Public Overridable Property ID As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID, Value)
			End Set
		End Property

		Public Overridable Property FECHA_INSERCION As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.FECHA_INSERCION)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.FECHA_INSERCION, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_TELEFONO As String
			Get
				If Me.IsColumnNull(ColumnNames.TELEFONO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.TELEFONO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TELEFONO)
				Else
					Me.TELEFONO = MyBase.SetStringAsString(ColumnNames.TELEFONO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_EXTENSION As String
			Get
				If Me.IsColumnNull(ColumnNames.EXTENSION) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.EXTENSION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.EXTENSION)
				Else
					Me.EXTENSION = MyBase.SetStringAsString(ColumnNames.EXTENSION, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_TRAFICO As String
			Get
				If Me.IsColumnNull(ColumnNames.TRAFICO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.TRAFICO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TRAFICO)
				Else
					Me.TRAFICO = MyBase.SetStringAsString(ColumnNames.TRAFICO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_TIPO_LLAMADA As String
			Get
				If Me.IsColumnNull(ColumnNames.TIPO_LLAMADA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.TIPO_LLAMADA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TIPO_LLAMADA)
				Else
					Me.TIPO_LLAMADA = MyBase.SetStringAsString(ColumnNames.TIPO_LLAMADA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_TIPO_DESTINO As String
			Get
				If Me.IsColumnNull(ColumnNames.TIPO_DESTINO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.TIPO_DESTINO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TIPO_DESTINO)
				Else
					Me.TIPO_DESTINO = MyBase.SetStringAsString(ColumnNames.TIPO_DESTINO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_NUMERO_LLAMADO As String
			Get
				If Me.IsColumnNull(ColumnNames.NUMERO_LLAMADO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.NUMERO_LLAMADO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NUMERO_LLAMADO)
				Else
					Me.NUMERO_LLAMADO = MyBase.SetStringAsString(ColumnNames.NUMERO_LLAMADO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_FECHA As String
			Get
				If Me.IsColumnNull(ColumnNames.FECHA) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.FECHA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.FECHA)
				Else
					Me.FECHA = MyBase.SetDateTimeAsString(ColumnNames.FECHA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_HORA As String
			Get
				If Me.IsColumnNull(ColumnNames.HORA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.HORA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.HORA)
				Else
					Me.HORA = MyBase.SetStringAsString(ColumnNames.HORA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_TIEMPO As String
			Get
				If Me.IsColumnNull(ColumnNames.TIEMPO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.TIEMPO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TIEMPO)
				Else
					Me.TIEMPO = MyBase.SetDecimalAsString(ColumnNames.TIEMPO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IMPORTE As String
			Get
				If Me.IsColumnNull(ColumnNames.IMPORTE) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IMPORTE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IMPORTE)
				Else
					Me.IMPORTE = MyBase.SetDecimalAsString(ColumnNames.IMPORTE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TLFNO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TLFNO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TLFNO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TLFNO)
				Else
					Me.ID_TLFNO = MyBase.SetDecimalAsString(ColumnNames.ID_TLFNO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_EXTENSION As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_EXTENSION) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_EXTENSION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_EXTENSION)
				Else
					Me.ID_EXTENSION = MyBase.SetDecimalAsString(ColumnNames.ID_EXTENSION, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID As String
			Get
				If Me.IsColumnNull(ColumnNames.ID) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID)
				Else
					Me.ID = MyBase.SetDecimalAsString(ColumnNames.ID, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_FECHA_INSERCION As String
			Get
				If Me.IsColumnNull(ColumnNames.FECHA_INSERCION) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.FECHA_INSERCION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.FECHA_INSERCION)
				Else
					Me.FECHA_INSERCION = MyBase.SetDateTimeAsString(ColumnNames.FECHA_INSERCION, Value)
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
		
	
			Public ReadOnly Property TELEFONO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TELEFONO, Parameters.TELEFONO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property EXTENSION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.EXTENSION, Parameters.EXTENSION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property TRAFICO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TRAFICO, Parameters.TRAFICO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property TIPO_LLAMADA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPO_LLAMADA, Parameters.TIPO_LLAMADA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property TIPO_DESTINO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPO_DESTINO, Parameters.TIPO_DESTINO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property NUMERO_LLAMADO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMERO_LLAMADO, Parameters.NUMERO_LLAMADO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property FECHA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA, Parameters.FECHA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property HORA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.HORA, Parameters.HORA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property TIEMPO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TIEMPO, Parameters.TIEMPO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IMPORTE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IMPORTE, Parameters.IMPORTE)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TLFNO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TLFNO, Parameters.ID_TLFNO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_EXTENSION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_EXTENSION, Parameters.ID_EXTENSION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID, Parameters.ID)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property FECHA_INSERCION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA_INSERCION, Parameters.FECHA_INSERCION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property TELEFONO() As WhereParameter 
			Get
				If _TELEFONO_W Is Nothing Then
					_TELEFONO_W = TearOff.TELEFONO
				End If
				Return _TELEFONO_W
			End Get
		End Property

		Public ReadOnly Property EXTENSION() As WhereParameter 
			Get
				If _EXTENSION_W Is Nothing Then
					_EXTENSION_W = TearOff.EXTENSION
				End If
				Return _EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property TRAFICO() As WhereParameter 
			Get
				If _TRAFICO_W Is Nothing Then
					_TRAFICO_W = TearOff.TRAFICO
				End If
				Return _TRAFICO_W
			End Get
		End Property

		Public ReadOnly Property TIPO_LLAMADA() As WhereParameter 
			Get
				If _TIPO_LLAMADA_W Is Nothing Then
					_TIPO_LLAMADA_W = TearOff.TIPO_LLAMADA
				End If
				Return _TIPO_LLAMADA_W
			End Get
		End Property

		Public ReadOnly Property TIPO_DESTINO() As WhereParameter 
			Get
				If _TIPO_DESTINO_W Is Nothing Then
					_TIPO_DESTINO_W = TearOff.TIPO_DESTINO
				End If
				Return _TIPO_DESTINO_W
			End Get
		End Property

		Public ReadOnly Property NUMERO_LLAMADO() As WhereParameter 
			Get
				If _NUMERO_LLAMADO_W Is Nothing Then
					_NUMERO_LLAMADO_W = TearOff.NUMERO_LLAMADO
				End If
				Return _NUMERO_LLAMADO_W
			End Get
		End Property

		Public ReadOnly Property FECHA() As WhereParameter 
			Get
				If _FECHA_W Is Nothing Then
					_FECHA_W = TearOff.FECHA
				End If
				Return _FECHA_W
			End Get
		End Property

		Public ReadOnly Property HORA() As WhereParameter 
			Get
				If _HORA_W Is Nothing Then
					_HORA_W = TearOff.HORA
				End If
				Return _HORA_W
			End Get
		End Property

		Public ReadOnly Property TIEMPO() As WhereParameter 
			Get
				If _TIEMPO_W Is Nothing Then
					_TIEMPO_W = TearOff.TIEMPO
				End If
				Return _TIEMPO_W
			End Get
		End Property

		Public ReadOnly Property IMPORTE() As WhereParameter 
			Get
				If _IMPORTE_W Is Nothing Then
					_IMPORTE_W = TearOff.IMPORTE
				End If
				Return _IMPORTE_W
			End Get
		End Property

		Public ReadOnly Property ID_TLFNO() As WhereParameter 
			Get
				If _ID_TLFNO_W Is Nothing Then
					_ID_TLFNO_W = TearOff.ID_TLFNO
				End If
				Return _ID_TLFNO_W
			End Get
		End Property

		Public ReadOnly Property ID_EXTENSION() As WhereParameter 
			Get
				If _ID_EXTENSION_W Is Nothing Then
					_ID_EXTENSION_W = TearOff.ID_EXTENSION
				End If
				Return _ID_EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property ID() As WhereParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
			End Get
		End Property

		Public ReadOnly Property FECHA_INSERCION() As WhereParameter 
			Get
				If _FECHA_INSERCION_W Is Nothing Then
					_FECHA_INSERCION_W = TearOff.FECHA_INSERCION
				End If
				Return _FECHA_INSERCION_W
			End Get
		End Property

		Private _TELEFONO_W As WhereParameter = Nothing
		Private _EXTENSION_W As WhereParameter = Nothing
		Private _TRAFICO_W As WhereParameter = Nothing
		Private _TIPO_LLAMADA_W As WhereParameter = Nothing
		Private _TIPO_DESTINO_W As WhereParameter = Nothing
		Private _NUMERO_LLAMADO_W As WhereParameter = Nothing
		Private _FECHA_W As WhereParameter = Nothing
		Private _HORA_W As WhereParameter = Nothing
		Private _TIEMPO_W As WhereParameter = Nothing
		Private _IMPORTE_W As WhereParameter = Nothing
		Private _ID_TLFNO_W As WhereParameter = Nothing
		Private _ID_EXTENSION_W As WhereParameter = Nothing
		Private _ID_W As WhereParameter = Nothing
		Private _FECHA_INSERCION_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_TELEFONO_W = Nothing
			_EXTENSION_W = Nothing
			_TRAFICO_W = Nothing
			_TIPO_LLAMADA_W = Nothing
			_TIPO_DESTINO_W = Nothing
			_NUMERO_LLAMADO_W = Nothing
			_FECHA_W = Nothing
			_HORA_W = Nothing
			_TIEMPO_W = Nothing
			_IMPORTE_W = Nothing
			_ID_TLFNO_W = Nothing
			_ID_EXTENSION_W = Nothing
			_ID_W = Nothing
			_FECHA_INSERCION_W = Nothing
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
		
	
		Public ReadOnly Property TELEFONO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TELEFONO, Parameters.TELEFONO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property EXTENSION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EXTENSION, Parameters.EXTENSION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property TRAFICO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TRAFICO, Parameters.TRAFICO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property TIPO_LLAMADA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPO_LLAMADA, Parameters.TIPO_LLAMADA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property TIPO_DESTINO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPO_DESTINO, Parameters.TIPO_DESTINO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property NUMERO_LLAMADO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMERO_LLAMADO, Parameters.NUMERO_LLAMADO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property FECHA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA, Parameters.FECHA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property HORA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.HORA, Parameters.HORA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property TIEMPO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIEMPO, Parameters.TIEMPO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IMPORTE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IMPORTE, Parameters.IMPORTE)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TLFNO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TLFNO, Parameters.ID_TLFNO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_EXTENSION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_EXTENSION, Parameters.ID_EXTENSION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID, Parameters.ID)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property FECHA_INSERCION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA_INSERCION, Parameters.FECHA_INSERCION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property TELEFONO() As AggregateParameter 
			Get
				If _TELEFONO_W Is Nothing Then
					_TELEFONO_W = TearOff.TELEFONO
				End If
				Return _TELEFONO_W
			End Get
		End Property

		Public ReadOnly Property EXTENSION() As AggregateParameter 
			Get
				If _EXTENSION_W Is Nothing Then
					_EXTENSION_W = TearOff.EXTENSION
				End If
				Return _EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property TRAFICO() As AggregateParameter 
			Get
				If _TRAFICO_W Is Nothing Then
					_TRAFICO_W = TearOff.TRAFICO
				End If
				Return _TRAFICO_W
			End Get
		End Property

		Public ReadOnly Property TIPO_LLAMADA() As AggregateParameter 
			Get
				If _TIPO_LLAMADA_W Is Nothing Then
					_TIPO_LLAMADA_W = TearOff.TIPO_LLAMADA
				End If
				Return _TIPO_LLAMADA_W
			End Get
		End Property

		Public ReadOnly Property TIPO_DESTINO() As AggregateParameter 
			Get
				If _TIPO_DESTINO_W Is Nothing Then
					_TIPO_DESTINO_W = TearOff.TIPO_DESTINO
				End If
				Return _TIPO_DESTINO_W
			End Get
		End Property

		Public ReadOnly Property NUMERO_LLAMADO() As AggregateParameter 
			Get
				If _NUMERO_LLAMADO_W Is Nothing Then
					_NUMERO_LLAMADO_W = TearOff.NUMERO_LLAMADO
				End If
				Return _NUMERO_LLAMADO_W
			End Get
		End Property

		Public ReadOnly Property FECHA() As AggregateParameter 
			Get
				If _FECHA_W Is Nothing Then
					_FECHA_W = TearOff.FECHA
				End If
				Return _FECHA_W
			End Get
		End Property

		Public ReadOnly Property HORA() As AggregateParameter 
			Get
				If _HORA_W Is Nothing Then
					_HORA_W = TearOff.HORA
				End If
				Return _HORA_W
			End Get
		End Property

		Public ReadOnly Property TIEMPO() As AggregateParameter 
			Get
				If _TIEMPO_W Is Nothing Then
					_TIEMPO_W = TearOff.TIEMPO
				End If
				Return _TIEMPO_W
			End Get
		End Property

		Public ReadOnly Property IMPORTE() As AggregateParameter 
			Get
				If _IMPORTE_W Is Nothing Then
					_IMPORTE_W = TearOff.IMPORTE
				End If
				Return _IMPORTE_W
			End Get
		End Property

		Public ReadOnly Property ID_TLFNO() As AggregateParameter 
			Get
				If _ID_TLFNO_W Is Nothing Then
					_ID_TLFNO_W = TearOff.ID_TLFNO
				End If
				Return _ID_TLFNO_W
			End Get
		End Property

		Public ReadOnly Property ID_EXTENSION() As AggregateParameter 
			Get
				If _ID_EXTENSION_W Is Nothing Then
					_ID_EXTENSION_W = TearOff.ID_EXTENSION
				End If
				Return _ID_EXTENSION_W
			End Get
		End Property

		Public ReadOnly Property ID() As AggregateParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
			End Get
		End Property

		Public ReadOnly Property FECHA_INSERCION() As AggregateParameter 
			Get
				If _FECHA_INSERCION_W Is Nothing Then
					_FECHA_INSERCION_W = TearOff.FECHA_INSERCION
				End If
				Return _FECHA_INSERCION_W
			End Get
		End Property

		Private _TELEFONO_W As AggregateParameter = Nothing
		Private _EXTENSION_W As AggregateParameter = Nothing
		Private _TRAFICO_W As AggregateParameter = Nothing
		Private _TIPO_LLAMADA_W As AggregateParameter = Nothing
		Private _TIPO_DESTINO_W As AggregateParameter = Nothing
		Private _NUMERO_LLAMADO_W As AggregateParameter = Nothing
		Private _FECHA_W As AggregateParameter = Nothing
		Private _HORA_W As AggregateParameter = Nothing
		Private _TIEMPO_W As AggregateParameter = Nothing
		Private _IMPORTE_W As AggregateParameter = Nothing
		Private _ID_TLFNO_W As AggregateParameter = Nothing
		Private _ID_EXTENSION_W As AggregateParameter = Nothing
		Private _ID_W As AggregateParameter = Nothing
		Private _FECHA_INSERCION_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_TELEFONO_W = Nothing
		_EXTENSION_W = Nothing
		_TRAFICO_W = Nothing
		_TIPO_LLAMADA_W = Nothing
		_TIPO_DESTINO_W = Nothing
		_NUMERO_LLAMADO_W = Nothing
		_FECHA_W = Nothing
		_HORA_W = Nothing
		_TIEMPO_W = Nothing
		_IMPORTE_W = Nothing
		_ID_TLFNO_W = Nothing
		_ID_EXTENSION_W = Nothing
		_ID_W = Nothing
		_FECHA_INSERCION_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_FACTURAS_MOVILES" 
	    
		CreateParameters(cmd)
		
		
		Dim p As OracleParameter
		p = cmd.Parameters(Parameters.ID.ParameterName)
		p.Direction = ParameterDirection.Output
		p.DbType = DbType.Decimal
    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_FACTURAS_MOVILES" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_FACTURAS_MOVILES" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.TELEFONO)
		p.SourceColumn = ColumnNames.TELEFONO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.EXTENSION)
		p.SourceColumn = ColumnNames.EXTENSION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.TRAFICO)
		p.SourceColumn = ColumnNames.TRAFICO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.TIPO_LLAMADA)
		p.SourceColumn = ColumnNames.TIPO_LLAMADA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.TIPO_DESTINO)
		p.SourceColumn = ColumnNames.TIPO_DESTINO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.NUMERO_LLAMADO)
		p.SourceColumn = ColumnNames.NUMERO_LLAMADO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.FECHA)
		p.SourceColumn = ColumnNames.FECHA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.HORA)
		p.SourceColumn = ColumnNames.HORA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.TIEMPO)
		p.SourceColumn = ColumnNames.TIEMPO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IMPORTE)
		p.SourceColumn = ColumnNames.IMPORTE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TLFNO)
		p.SourceColumn = ColumnNames.ID_TLFNO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_EXTENSION)
		p.SourceColumn = ColumnNames.ID_EXTENSION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.FECHA_INSERCION)
		p.SourceColumn = ColumnNames.FECHA_INSERCION
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

