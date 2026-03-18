
'===============================================================================
'BATZ, Koop. - 25/05/2010 10:24:05
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

Public MustInherit Class _RECURSOS
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "RECURSOS"
			Me.MappingName = "RECURSOS"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_RECURSOS", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_RECURSOS.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_RECURSOS", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDPARENT As OracleParameter
			Get
				Return New OracleParameter("p_IDPARENT", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property URL As OracleParameter
			Get
				Return New OracleParameter("p_URL", OracleDbType.VARCHAR2, 150)
			End Get
		End Property
		
		Public Shared ReadOnly Property OBSOLETO As OracleParameter
			Get
				Return New OracleParameter("p_OBSOLETO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property TIPO As OracleParameter
			Get
				Return New OracleParameter("p_TIPO", OracleDbType.VARCHAR2, 1)
			End Get
		End Property
		
		Public Shared ReadOnly Property IMAGEN As OracleParameter
			Get
				Return New OracleParameter("p_IMAGEN", OracleDbType.Blob, 2147483647)
			End Get
		End Property
		
		Public Shared ReadOnly Property VISIBLE As OracleParameter
			Get
				Return New OracleParameter("p_VISIBLE", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property FICHERO As OracleParameter
			Get
				Return New OracleParameter("p_FICHERO", OracleDbType.Blob, 2147483647)
			End Get
		End Property
		
		Public Shared ReadOnly Property NOMBRE_FICHERO As OracleParameter
			Get
				Return New OracleParameter("p_NOMBRE_FICHERO", OracleDbType.VARCHAR2, 25)
			End Get
		End Property
		
		Public Shared ReadOnly Property TARGET As OracleParameter
			Get
				Return New OracleParameter("p_TARGET", OracleDbType.VARCHAR2, 10)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const IDPARENT As String = "IDPARENT"
        Public Const URL As String = "URL"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const TIPO As String = "TIPO"
        Public Const IMAGEN As String = "IMAGEN"
        Public Const VISIBLE As String = "VISIBLE"
        Public Const FICHERO As String = "FICHERO"
        Public Const NOMBRE_FICHERO As String = "NOMBRE_FICHERO"
        Public Const TARGET As String = "TARGET"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _RECURSOS.PropertyNames.ID
				ht(IDPARENT) = _RECURSOS.PropertyNames.IDPARENT
				ht(URL) = _RECURSOS.PropertyNames.URL
				ht(OBSOLETO) = _RECURSOS.PropertyNames.OBSOLETO
				ht(TIPO) = _RECURSOS.PropertyNames.TIPO
				ht(IMAGEN) = _RECURSOS.PropertyNames.IMAGEN
				ht(VISIBLE) = _RECURSOS.PropertyNames.VISIBLE
				ht(FICHERO) = _RECURSOS.PropertyNames.FICHERO
				ht(NOMBRE_FICHERO) = _RECURSOS.PropertyNames.NOMBRE_FICHERO
				ht(TARGET) = _RECURSOS.PropertyNames.TARGET

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const IDPARENT As String = "IDPARENT"
        Public Const URL As String = "URL"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const TIPO As String = "TIPO"
        Public Const IMAGEN As String = "IMAGEN"
        Public Const VISIBLE As String = "VISIBLE"
        Public Const FICHERO As String = "FICHERO"
        Public Const NOMBRE_FICHERO As String = "NOMBRE_FICHERO"
        Public Const TARGET As String = "TARGET"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _RECURSOS.ColumnNames.ID
				ht(IDPARENT) = _RECURSOS.ColumnNames.IDPARENT
				ht(URL) = _RECURSOS.ColumnNames.URL
				ht(OBSOLETO) = _RECURSOS.ColumnNames.OBSOLETO
				ht(TIPO) = _RECURSOS.ColumnNames.TIPO
				ht(IMAGEN) = _RECURSOS.ColumnNames.IMAGEN
				ht(VISIBLE) = _RECURSOS.ColumnNames.VISIBLE
				ht(FICHERO) = _RECURSOS.ColumnNames.FICHERO
				ht(NOMBRE_FICHERO) = _RECURSOS.ColumnNames.NOMBRE_FICHERO
				ht(TARGET) = _RECURSOS.ColumnNames.TARGET

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const IDPARENT As String = "s_IDPARENT"
        Public Const URL As String = "s_URL"
        Public Const OBSOLETO As String = "s_OBSOLETO"
        Public Const TIPO As String = "s_TIPO"
        Public Const IMAGEN As String = "s_IMAGEN"
        Public Const VISIBLE As String = "s_VISIBLE"
        Public Const FICHERO As String = "s_FICHERO"
        Public Const NOMBRE_FICHERO As String = "s_NOMBRE_FICHERO"
        Public Const TARGET As String = "s_TARGET"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property ID As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID, Value)
			End Set
		End Property

		Public Overridable Property IDPARENT As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDPARENT)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDPARENT, Value)
			End Set
		End Property

		Public Overridable Property URL As String
			Get
				Return MyBase.GetString(ColumnNames.URL)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.URL, Value)
			End Set
		End Property

		Public Overridable Property OBSOLETO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.OBSOLETO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.OBSOLETO, Value)
			End Set
		End Property

		Public Overridable Property TIPO As String
			Get
				Return MyBase.GetString(ColumnNames.TIPO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.TIPO, Value)
			End Set
		End Property

		Public Overridable Property IMAGEN As Byte()
			Get
				Return MyBase.GetByteArray(ColumnNames.IMAGEN)
			End Get
			Set(ByVal Value As Byte())
				MyBase.SetByteArray(ColumnNames.IMAGEN, Value)
			End Set
		End Property

		Public Overridable Property VISIBLE As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.VISIBLE)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.VISIBLE, Value)
			End Set
		End Property

		Public Overridable Property FICHERO As Byte()
			Get
				Return MyBase.GetByteArray(ColumnNames.FICHERO)
			End Get
			Set(ByVal Value As Byte())
				MyBase.SetByteArray(ColumnNames.FICHERO, Value)
			End Set
		End Property

		Public Overridable Property NOMBRE_FICHERO As String
			Get
				Return MyBase.GetString(ColumnNames.NOMBRE_FICHERO)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.NOMBRE_FICHERO, Value)
			End Set
		End Property

		Public Overridable Property TARGET As String
			Get
				Return MyBase.GetString(ColumnNames.TARGET)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.TARGET, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

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

		Public Overridable Property s_IDPARENT As String
			Get
				If Me.IsColumnNull(ColumnNames.IDPARENT) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDPARENT)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDPARENT)
				Else
					Me.IDPARENT = MyBase.SetDecimalAsString(ColumnNames.IDPARENT, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_URL As String
			Get
				If Me.IsColumnNull(ColumnNames.URL) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.URL)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.URL)
				Else
					Me.URL = MyBase.SetStringAsString(ColumnNames.URL, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_OBSOLETO As String
			Get
				If Me.IsColumnNull(ColumnNames.OBSOLETO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.OBSOLETO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.OBSOLETO)
				Else
					Me.OBSOLETO = MyBase.SetDecimalAsString(ColumnNames.OBSOLETO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_TIPO As String
			Get
				If Me.IsColumnNull(ColumnNames.TIPO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.TIPO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TIPO)
				Else
					Me.TIPO = MyBase.SetStringAsString(ColumnNames.TIPO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_VISIBLE As String
			Get
				If Me.IsColumnNull(ColumnNames.VISIBLE) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.VISIBLE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.VISIBLE)
				Else
					Me.VISIBLE = MyBase.SetDecimalAsString(ColumnNames.VISIBLE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_NOMBRE_FICHERO As String
			Get
				If Me.IsColumnNull(ColumnNames.NOMBRE_FICHERO) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.NOMBRE_FICHERO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NOMBRE_FICHERO)
				Else
					Me.NOMBRE_FICHERO = MyBase.SetStringAsString(ColumnNames.NOMBRE_FICHERO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_TARGET As String
			Get
				If Me.IsColumnNull(ColumnNames.TARGET) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.TARGET)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.TARGET)
				Else
					Me.TARGET = MyBase.SetStringAsString(ColumnNames.TARGET, Value)
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
		
	
			Public ReadOnly Property ID() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID, Parameters.ID)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IDPARENT() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDPARENT, Parameters.IDPARENT)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property URL() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.URL, Parameters.URL)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property OBSOLETO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property TIPO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPO, Parameters.TIPO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IMAGEN() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IMAGEN, Parameters.IMAGEN)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property VISIBLE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.VISIBLE, Parameters.VISIBLE)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property FICHERO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.FICHERO, Parameters.FICHERO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property NOMBRE_FICHERO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBRE_FICHERO, Parameters.NOMBRE_FICHERO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property TARGET() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.TARGET, Parameters.TARGET)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property ID() As WhereParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
			End Get
		End Property

		Public ReadOnly Property IDPARENT() As WhereParameter 
			Get
				If _IDPARENT_W Is Nothing Then
					_IDPARENT_W = TearOff.IDPARENT
				End If
				Return _IDPARENT_W
			End Get
		End Property

		Public ReadOnly Property URL() As WhereParameter 
			Get
				If _URL_W Is Nothing Then
					_URL_W = TearOff.URL
				End If
				Return _URL_W
			End Get
		End Property

		Public ReadOnly Property OBSOLETO() As WhereParameter 
			Get
				If _OBSOLETO_W Is Nothing Then
					_OBSOLETO_W = TearOff.OBSOLETO
				End If
				Return _OBSOLETO_W
			End Get
		End Property

		Public ReadOnly Property TIPO() As WhereParameter 
			Get
				If _TIPO_W Is Nothing Then
					_TIPO_W = TearOff.TIPO
				End If
				Return _TIPO_W
			End Get
		End Property

		Public ReadOnly Property IMAGEN() As WhereParameter 
			Get
				If _IMAGEN_W Is Nothing Then
					_IMAGEN_W = TearOff.IMAGEN
				End If
				Return _IMAGEN_W
			End Get
		End Property

		Public ReadOnly Property VISIBLE() As WhereParameter 
			Get
				If _VISIBLE_W Is Nothing Then
					_VISIBLE_W = TearOff.VISIBLE
				End If
				Return _VISIBLE_W
			End Get
		End Property

		Public ReadOnly Property FICHERO() As WhereParameter 
			Get
				If _FICHERO_W Is Nothing Then
					_FICHERO_W = TearOff.FICHERO
				End If
				Return _FICHERO_W
			End Get
		End Property

		Public ReadOnly Property NOMBRE_FICHERO() As WhereParameter 
			Get
				If _NOMBRE_FICHERO_W Is Nothing Then
					_NOMBRE_FICHERO_W = TearOff.NOMBRE_FICHERO
				End If
				Return _NOMBRE_FICHERO_W
			End Get
		End Property

		Public ReadOnly Property TARGET() As WhereParameter 
			Get
				If _TARGET_W Is Nothing Then
					_TARGET_W = TearOff.TARGET
				End If
				Return _TARGET_W
			End Get
		End Property

		Private _ID_W As WhereParameter = Nothing
		Private _IDPARENT_W As WhereParameter = Nothing
		Private _URL_W As WhereParameter = Nothing
		Private _OBSOLETO_W As WhereParameter = Nothing
		Private _TIPO_W As WhereParameter = Nothing
		Private _IMAGEN_W As WhereParameter = Nothing
		Private _VISIBLE_W As WhereParameter = Nothing
		Private _FICHERO_W As WhereParameter = Nothing
		Private _NOMBRE_FICHERO_W As WhereParameter = Nothing
		Private _TARGET_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_IDPARENT_W = Nothing
			_URL_W = Nothing
			_OBSOLETO_W = Nothing
			_TIPO_W = Nothing
			_IMAGEN_W = Nothing
			_VISIBLE_W = Nothing
			_FICHERO_W = Nothing
			_NOMBRE_FICHERO_W = Nothing
			_TARGET_W = Nothing
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
		
	
		Public ReadOnly Property ID() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID, Parameters.ID)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IDPARENT() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDPARENT, Parameters.IDPARENT)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property URL() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.URL, Parameters.URL)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property OBSOLETO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property TIPO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPO, Parameters.TIPO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IMAGEN() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IMAGEN, Parameters.IMAGEN)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property VISIBLE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.VISIBLE, Parameters.VISIBLE)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property FICHERO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FICHERO, Parameters.FICHERO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property NOMBRE_FICHERO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBRE_FICHERO, Parameters.NOMBRE_FICHERO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property TARGET() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TARGET, Parameters.TARGET)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property ID() As AggregateParameter 
			Get
				If _ID_W Is Nothing Then
					_ID_W = TearOff.ID
				End If
				Return _ID_W
			End Get
		End Property

		Public ReadOnly Property IDPARENT() As AggregateParameter 
			Get
				If _IDPARENT_W Is Nothing Then
					_IDPARENT_W = TearOff.IDPARENT
				End If
				Return _IDPARENT_W
			End Get
		End Property

		Public ReadOnly Property URL() As AggregateParameter 
			Get
				If _URL_W Is Nothing Then
					_URL_W = TearOff.URL
				End If
				Return _URL_W
			End Get
		End Property

		Public ReadOnly Property OBSOLETO() As AggregateParameter 
			Get
				If _OBSOLETO_W Is Nothing Then
					_OBSOLETO_W = TearOff.OBSOLETO
				End If
				Return _OBSOLETO_W
			End Get
		End Property

		Public ReadOnly Property TIPO() As AggregateParameter 
			Get
				If _TIPO_W Is Nothing Then
					_TIPO_W = TearOff.TIPO
				End If
				Return _TIPO_W
			End Get
		End Property

		Public ReadOnly Property IMAGEN() As AggregateParameter 
			Get
				If _IMAGEN_W Is Nothing Then
					_IMAGEN_W = TearOff.IMAGEN
				End If
				Return _IMAGEN_W
			End Get
		End Property

		Public ReadOnly Property VISIBLE() As AggregateParameter 
			Get
				If _VISIBLE_W Is Nothing Then
					_VISIBLE_W = TearOff.VISIBLE
				End If
				Return _VISIBLE_W
			End Get
		End Property

		Public ReadOnly Property FICHERO() As AggregateParameter 
			Get
				If _FICHERO_W Is Nothing Then
					_FICHERO_W = TearOff.FICHERO
				End If
				Return _FICHERO_W
			End Get
		End Property

		Public ReadOnly Property NOMBRE_FICHERO() As AggregateParameter 
			Get
				If _NOMBRE_FICHERO_W Is Nothing Then
					_NOMBRE_FICHERO_W = TearOff.NOMBRE_FICHERO
				End If
				Return _NOMBRE_FICHERO_W
			End Get
		End Property

		Public ReadOnly Property TARGET() As AggregateParameter 
			Get
				If _TARGET_W Is Nothing Then
					_TARGET_W = TearOff.TARGET
				End If
				Return _TARGET_W
			End Get
		End Property

		Private _ID_W As AggregateParameter = Nothing
		Private _IDPARENT_W As AggregateParameter = Nothing
		Private _URL_W As AggregateParameter = Nothing
		Private _OBSOLETO_W As AggregateParameter = Nothing
		Private _TIPO_W As AggregateParameter = Nothing
		Private _IMAGEN_W As AggregateParameter = Nothing
		Private _VISIBLE_W As AggregateParameter = Nothing
		Private _FICHERO_W As AggregateParameter = Nothing
		Private _NOMBRE_FICHERO_W As AggregateParameter = Nothing
		Private _TARGET_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_IDPARENT_W = Nothing
		_URL_W = Nothing
		_OBSOLETO_W = Nothing
		_TIPO_W = Nothing
		_IMAGEN_W = Nothing
		_VISIBLE_W = Nothing
		_FICHERO_W = Nothing
		_NOMBRE_FICHERO_W = Nothing
		_TARGET_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_RECURSOS" 
	    
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_RECURSOS" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_RECURSOS" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID)
		p.SourceColumn = ColumnNames.ID
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDPARENT)
		p.SourceColumn = ColumnNames.IDPARENT
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.URL)
		p.SourceColumn = ColumnNames.URL
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.OBSOLETO)
		p.SourceColumn = ColumnNames.OBSOLETO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.TIPO)
		p.SourceColumn = ColumnNames.TIPO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IMAGEN)
		p.SourceColumn = ColumnNames.IMAGEN
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.VISIBLE)
		p.SourceColumn = ColumnNames.VISIBLE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.FICHERO)
		p.SourceColumn = ColumnNames.FICHERO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.NOMBRE_FICHERO)
		p.SourceColumn = ColumnNames.NOMBRE_FICHERO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.TARGET)
		p.SourceColumn = ColumnNames.TARGET
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

