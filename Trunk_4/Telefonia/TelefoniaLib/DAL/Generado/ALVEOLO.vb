
'===============================================================================
'BATZ, Koop. - 24/07/2015 12:13:49
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

Public MustInherit Class _ALVEOLO
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "ALVEOLO"
			Me.MappingName = "ALVEOLO"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_ALVEOLO", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_ALVEOLO.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_ALVEOLO", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property RUTA As OracleParameter
			Get
				Return New OracleParameter("p_RUTA", OracleDbType.VARCHAR2, 10)
			End Get
		End Property
		
		Public Shared ReadOnly Property ESTADO As OracleParameter
			Get
				Return New OracleParameter("p_ESTADO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TIPOALV As OracleParameter
			Get
				Return New OracleParameter("p_ID_TIPOALV", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PLANTA As OracleParameter
			Get
				Return New OracleParameter("p_ID_PLANTA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property OBSOLETO As OracleParameter
			Get
				Return New OracleParameter("p_OBSOLETO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property POS_FILA As OracleParameter
			Get
				Return New OracleParameter("p_POS_FILA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property POS_COL As OracleParameter
			Get
				Return New OracleParameter("p_POS_COL", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const RUTA As String = "RUTA"
        Public Const ESTADO As String = "ESTADO"
        Public Const ID_TIPOALV As String = "ID_TIPOALV"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const POS_FILA As String = "POS_FILA"
        Public Const POS_COL As String = "POS_COL"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _ALVEOLO.PropertyNames.ID
				ht(RUTA) = _ALVEOLO.PropertyNames.RUTA
				ht(ESTADO) = _ALVEOLO.PropertyNames.ESTADO
				ht(ID_TIPOALV) = _ALVEOLO.PropertyNames.ID_TIPOALV
				ht(ID_PLANTA) = _ALVEOLO.PropertyNames.ID_PLANTA
				ht(OBSOLETO) = _ALVEOLO.PropertyNames.OBSOLETO
				ht(POS_FILA) = _ALVEOLO.PropertyNames.POS_FILA
				ht(POS_COL) = _ALVEOLO.PropertyNames.POS_COL

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const RUTA As String = "RUTA"
        Public Const ESTADO As String = "ESTADO"
        Public Const ID_TIPOALV As String = "ID_TIPOALV"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const OBSOLETO As String = "OBSOLETO"
        Public Const POS_FILA As String = "POS_FILA"
        Public Const POS_COL As String = "POS_COL"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _ALVEOLO.ColumnNames.ID
				ht(RUTA) = _ALVEOLO.ColumnNames.RUTA
				ht(ESTADO) = _ALVEOLO.ColumnNames.ESTADO
				ht(ID_TIPOALV) = _ALVEOLO.ColumnNames.ID_TIPOALV
				ht(ID_PLANTA) = _ALVEOLO.ColumnNames.ID_PLANTA
				ht(OBSOLETO) = _ALVEOLO.ColumnNames.OBSOLETO
				ht(POS_FILA) = _ALVEOLO.ColumnNames.POS_FILA
				ht(POS_COL) = _ALVEOLO.ColumnNames.POS_COL

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const RUTA As String = "s_RUTA"
        Public Const ESTADO As String = "s_ESTADO"
        Public Const ID_TIPOALV As String = "s_ID_TIPOALV"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"
        Public Const OBSOLETO As String = "s_OBSOLETO"
        Public Const POS_FILA As String = "s_POS_FILA"
        Public Const POS_COL As String = "s_POS_COL"

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

		Public Overridable Property RUTA As String
			Get
				Return MyBase.GetString(ColumnNames.RUTA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.RUTA, Value)
			End Set
		End Property

		Public Overridable Property ESTADO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ESTADO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ESTADO, Value)
			End Set
		End Property

		Public Overridable Property ID_TIPOALV As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TIPOALV)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TIPOALV, Value)
			End Set
		End Property

		Public Overridable Property ID_PLANTA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_PLANTA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_PLANTA, Value)
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

		Public Overridable Property POS_FILA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.POS_FILA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.POS_FILA, Value)
			End Set
		End Property

		Public Overridable Property POS_COL As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.POS_COL)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.POS_COL, Value)
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

		Public Overridable Property s_RUTA As String
			Get
				If Me.IsColumnNull(ColumnNames.RUTA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.RUTA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.RUTA)
				Else
					Me.RUTA = MyBase.SetStringAsString(ColumnNames.RUTA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ESTADO As String
			Get
				If Me.IsColumnNull(ColumnNames.ESTADO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ESTADO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ESTADO)
				Else
					Me.ESTADO = MyBase.SetDecimalAsString(ColumnNames.ESTADO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TIPOALV As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TIPOALV) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TIPOALV)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TIPOALV)
				Else
					Me.ID_TIPOALV = MyBase.SetDecimalAsString(ColumnNames.ID_TIPOALV, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_PLANTA As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_PLANTA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_PLANTA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_PLANTA)
				Else
					Me.ID_PLANTA = MyBase.SetDecimalAsString(ColumnNames.ID_PLANTA, Value)
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

		Public Overridable Property s_POS_FILA As String
			Get
				If Me.IsColumnNull(ColumnNames.POS_FILA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.POS_FILA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.POS_FILA)
				Else
					Me.POS_FILA = MyBase.SetDecimalAsString(ColumnNames.POS_FILA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_POS_COL As String
			Get
				If Me.IsColumnNull(ColumnNames.POS_COL) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.POS_COL)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.POS_COL)
				Else
					Me.POS_COL = MyBase.SetDecimalAsString(ColumnNames.POS_COL, Value)
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

			Public ReadOnly Property RUTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.RUTA, Parameters.RUTA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ESTADO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ESTADO, Parameters.ESTADO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TIPOALV() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TIPOALV, Parameters.ID_TIPOALV)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_PLANTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
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

			Public ReadOnly Property POS_FILA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.POS_FILA, Parameters.POS_FILA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property POS_COL() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.POS_COL, Parameters.POS_COL)
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

		Public ReadOnly Property RUTA() As WhereParameter 
			Get
				If _RUTA_W Is Nothing Then
					_RUTA_W = TearOff.RUTA
				End If
				Return _RUTA_W
			End Get
		End Property

		Public ReadOnly Property ESTADO() As WhereParameter 
			Get
				If _ESTADO_W Is Nothing Then
					_ESTADO_W = TearOff.ESTADO
				End If
				Return _ESTADO_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOALV() As WhereParameter 
			Get
				If _ID_TIPOALV_W Is Nothing Then
					_ID_TIPOALV_W = TearOff.ID_TIPOALV
				End If
				Return _ID_TIPOALV_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As WhereParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
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

		Public ReadOnly Property POS_FILA() As WhereParameter 
			Get
				If _POS_FILA_W Is Nothing Then
					_POS_FILA_W = TearOff.POS_FILA
				End If
				Return _POS_FILA_W
			End Get
		End Property

		Public ReadOnly Property POS_COL() As WhereParameter 
			Get
				If _POS_COL_W Is Nothing Then
					_POS_COL_W = TearOff.POS_COL
				End If
				Return _POS_COL_W
			End Get
		End Property

		Private _ID_W As WhereParameter = Nothing
		Private _RUTA_W As WhereParameter = Nothing
		Private _ESTADO_W As WhereParameter = Nothing
		Private _ID_TIPOALV_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing
		Private _OBSOLETO_W As WhereParameter = Nothing
		Private _POS_FILA_W As WhereParameter = Nothing
		Private _POS_COL_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_RUTA_W = Nothing
			_ESTADO_W = Nothing
			_ID_TIPOALV_W = Nothing
			_ID_PLANTA_W = Nothing
			_OBSOLETO_W = Nothing
			_POS_FILA_W = Nothing
			_POS_COL_W = Nothing
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

		Public ReadOnly Property RUTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.RUTA, Parameters.RUTA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ESTADO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ESTADO, Parameters.ESTADO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TIPOALV() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TIPOALV, Parameters.ID_TIPOALV)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
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

		Public ReadOnly Property POS_FILA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.POS_FILA, Parameters.POS_FILA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property POS_COL() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.POS_COL, Parameters.POS_COL)
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

		Public ReadOnly Property RUTA() As AggregateParameter 
			Get
				If _RUTA_W Is Nothing Then
					_RUTA_W = TearOff.RUTA
				End If
				Return _RUTA_W
			End Get
		End Property

		Public ReadOnly Property ESTADO() As AggregateParameter 
			Get
				If _ESTADO_W Is Nothing Then
					_ESTADO_W = TearOff.ESTADO
				End If
				Return _ESTADO_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOALV() As AggregateParameter 
			Get
				If _ID_TIPOALV_W Is Nothing Then
					_ID_TIPOALV_W = TearOff.ID_TIPOALV
				End If
				Return _ID_TIPOALV_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
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

		Public ReadOnly Property POS_FILA() As AggregateParameter 
			Get
				If _POS_FILA_W Is Nothing Then
					_POS_FILA_W = TearOff.POS_FILA
				End If
				Return _POS_FILA_W
			End Get
		End Property

		Public ReadOnly Property POS_COL() As AggregateParameter 
			Get
				If _POS_COL_W Is Nothing Then
					_POS_COL_W = TearOff.POS_COL
				End If
				Return _POS_COL_W
			End Get
		End Property

		Private _ID_W As AggregateParameter = Nothing
		Private _RUTA_W As AggregateParameter = Nothing
		Private _ESTADO_W As AggregateParameter = Nothing
		Private _ID_TIPOALV_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing
		Private _OBSOLETO_W As AggregateParameter = Nothing
		Private _POS_FILA_W As AggregateParameter = Nothing
		Private _POS_COL_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_RUTA_W = Nothing
		_ESTADO_W = Nothing
		_ID_TIPOALV_W = Nothing
		_ID_PLANTA_W = Nothing
		_OBSOLETO_W = Nothing
		_POS_FILA_W = Nothing
		_POS_COL_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_ALVEOLO" 
	    
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_ALVEOLO" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_ALVEOLO" 
		
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

		p = cmd.Parameters.Add(Parameters.RUTA)
		p.SourceColumn = ColumnNames.RUTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ESTADO)
		p.SourceColumn = ColumnNames.ESTADO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TIPOALV)
		p.SourceColumn = ColumnNames.ID_TIPOALV
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.OBSOLETO)
		p.SourceColumn = ColumnNames.OBSOLETO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.POS_FILA)
		p.SourceColumn = ColumnNames.POS_FILA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.POS_COL)
		p.SourceColumn = ColumnNames.POS_COL
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

