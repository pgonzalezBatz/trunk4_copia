
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

Public MustInherit Class _GRUPO_EXTENSIONES
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "GRUPO_EXTENSIONES"
			Me.MappingName = "GRUPO_EXTENSIONES"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GRUPO_EXTENSIONES", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_GRUPO_EXTENSIONES.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GRUPO_EXTENSIONES", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property GRUPO As OracleParameter
			Get
				Return New OracleParameter("p_GRUPO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PLANTA As OracleParameter
			Get
				Return New OracleParameter("p_ID_PLANTA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property LIBRE As OracleParameter
			Get
				Return New OracleParameter("p_LIBRE", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property OBSOLETO As OracleParameter
			Get
				Return New OracleParameter("p_OBSOLETO", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const GRUPO As String = "GRUPO"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const LIBRE As String = "LIBRE"
        Public Const OBSOLETO As String = "OBSOLETO"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _GRUPO_EXTENSIONES.PropertyNames.ID
				ht(GRUPO) = _GRUPO_EXTENSIONES.PropertyNames.GRUPO
				ht(ID_PLANTA) = _GRUPO_EXTENSIONES.PropertyNames.ID_PLANTA
				ht(LIBRE) = _GRUPO_EXTENSIONES.PropertyNames.LIBRE
				ht(OBSOLETO) = _GRUPO_EXTENSIONES.PropertyNames.OBSOLETO

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const GRUPO As String = "GRUPO"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const LIBRE As String = "LIBRE"
        Public Const OBSOLETO As String = "OBSOLETO"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _GRUPO_EXTENSIONES.ColumnNames.ID
				ht(GRUPO) = _GRUPO_EXTENSIONES.ColumnNames.GRUPO
				ht(ID_PLANTA) = _GRUPO_EXTENSIONES.ColumnNames.ID_PLANTA
				ht(LIBRE) = _GRUPO_EXTENSIONES.ColumnNames.LIBRE
				ht(OBSOLETO) = _GRUPO_EXTENSIONES.ColumnNames.OBSOLETO

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const GRUPO As String = "s_GRUPO"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"
        Public Const LIBRE As String = "s_LIBRE"
        Public Const OBSOLETO As String = "s_OBSOLETO"

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

		Public Overridable Property GRUPO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.GRUPO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.GRUPO, Value)
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

		Public Overridable Property LIBRE As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.LIBRE)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.LIBRE, Value)
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

		Public Overridable Property s_GRUPO As String
			Get
				If Me.IsColumnNull(ColumnNames.GRUPO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.GRUPO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.GRUPO)
				Else
					Me.GRUPO = MyBase.SetDecimalAsString(ColumnNames.GRUPO, Value)
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

		Public Overridable Property s_LIBRE As String
			Get
				If Me.IsColumnNull(ColumnNames.LIBRE) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.LIBRE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.LIBRE)
				Else
					Me.LIBRE = MyBase.SetDecimalAsString(ColumnNames.LIBRE, Value)
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

			Public ReadOnly Property GRUPO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.GRUPO, Parameters.GRUPO)
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

			Public ReadOnly Property LIBRE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.LIBRE, Parameters.LIBRE)
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

		Public ReadOnly Property GRUPO() As WhereParameter 
			Get
				If _GRUPO_W Is Nothing Then
					_GRUPO_W = TearOff.GRUPO
				End If
				Return _GRUPO_W
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

		Public ReadOnly Property LIBRE() As WhereParameter 
			Get
				If _LIBRE_W Is Nothing Then
					_LIBRE_W = TearOff.LIBRE
				End If
				Return _LIBRE_W
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

		Private _ID_W As WhereParameter = Nothing
		Private _GRUPO_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing
		Private _LIBRE_W As WhereParameter = Nothing
		Private _OBSOLETO_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_GRUPO_W = Nothing
			_ID_PLANTA_W = Nothing
			_LIBRE_W = Nothing
			_OBSOLETO_W = Nothing
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

		Public ReadOnly Property GRUPO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.GRUPO, Parameters.GRUPO)
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

		Public ReadOnly Property LIBRE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LIBRE, Parameters.LIBRE)
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

		Public ReadOnly Property GRUPO() As AggregateParameter 
			Get
				If _GRUPO_W Is Nothing Then
					_GRUPO_W = TearOff.GRUPO
				End If
				Return _GRUPO_W
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

		Public ReadOnly Property LIBRE() As AggregateParameter 
			Get
				If _LIBRE_W Is Nothing Then
					_LIBRE_W = TearOff.LIBRE
				End If
				Return _LIBRE_W
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

		Private _ID_W As AggregateParameter = Nothing
		Private _GRUPO_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing
		Private _LIBRE_W As AggregateParameter = Nothing
		Private _OBSOLETO_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_GRUPO_W = Nothing
		_ID_PLANTA_W = Nothing
		_LIBRE_W = Nothing
		_OBSOLETO_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_GRUPO_EXTENSIONES" 
	    
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_GRUPO_EXTENSIONES" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_GRUPO_EXTENSIONES" 
		
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

		p = cmd.Parameters.Add(Parameters.GRUPO)
		p.SourceColumn = ColumnNames.GRUPO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.LIBRE)
		p.SourceColumn = ColumnNames.LIBRE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.OBSOLETO)
		p.SourceColumn = ColumnNames.OBSOLETO
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

