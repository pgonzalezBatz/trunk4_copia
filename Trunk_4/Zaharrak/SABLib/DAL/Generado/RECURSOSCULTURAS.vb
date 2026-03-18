
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

Public MustInherit Class _RECURSOSCULTURAS
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "RECURSOSCULTURAS"
			Me.MappingName = "RECURSOSCULTURAS"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_RECURSOSCULTURAS", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal IDCULTURAS As String, ByVal IDRECURSOS As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_RECURSOSCULTURAS.Parameters.IDCULTURAS, IDCULTURAS)

parameters.Add(_RECURSOSCULTURAS.Parameters.IDRECURSOS, IDRECURSOS)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_RECURSOSCULTURAS", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property IDRECURSOS As OracleParameter
			Get
				Return New OracleParameter("p_IDRECURSOS", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDCULTURAS As OracleParameter
			Get
				Return New OracleParameter("p_IDCULTURAS", OracleDbType.VARCHAR2, 5)
			End Get
		End Property
		
		Public Shared ReadOnly Property NOMBRE As OracleParameter
			Get
				Return New OracleParameter("p_NOMBRE", OracleDbType.VARCHAR2, 150)
			End Get
		End Property
		
		Public Shared ReadOnly Property DESCRIPCION As OracleParameter
			Get
				Return New OracleParameter("p_DESCRIPCION", OracleDbType.VARCHAR2, 200)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const IDRECURSOS As String = "IDRECURSOS"
        Public Const IDCULTURAS As String = "IDCULTURAS"
        Public Const NOMBRE As String = "NOMBRE"
        Public Const DESCRIPCION As String = "DESCRIPCION"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDRECURSOS) = _RECURSOSCULTURAS.PropertyNames.IDRECURSOS
				ht(IDCULTURAS) = _RECURSOSCULTURAS.PropertyNames.IDCULTURAS
				ht(NOMBRE) = _RECURSOSCULTURAS.PropertyNames.NOMBRE
				ht(DESCRIPCION) = _RECURSOSCULTURAS.PropertyNames.DESCRIPCION

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const IDRECURSOS As String = "IDRECURSOS"
        Public Const IDCULTURAS As String = "IDCULTURAS"
        Public Const NOMBRE As String = "NOMBRE"
        Public Const DESCRIPCION As String = "DESCRIPCION"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDRECURSOS) = _RECURSOSCULTURAS.ColumnNames.IDRECURSOS
				ht(IDCULTURAS) = _RECURSOSCULTURAS.ColumnNames.IDCULTURAS
				ht(NOMBRE) = _RECURSOSCULTURAS.ColumnNames.NOMBRE
				ht(DESCRIPCION) = _RECURSOSCULTURAS.ColumnNames.DESCRIPCION

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const IDRECURSOS As String = "s_IDRECURSOS"
        Public Const IDCULTURAS As String = "s_IDCULTURAS"
        Public Const NOMBRE As String = "s_NOMBRE"
        Public Const DESCRIPCION As String = "s_DESCRIPCION"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property IDRECURSOS As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDRECURSOS)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDRECURSOS, Value)
			End Set
		End Property

		Public Overridable Property IDCULTURAS As String
			Get
				Return MyBase.GetString(ColumnNames.IDCULTURAS)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.IDCULTURAS, Value)
			End Set
		End Property

		Public Overridable Property NOMBRE As String
			Get
				Return MyBase.GetString(ColumnNames.NOMBRE)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.NOMBRE, Value)
			End Set
		End Property

		Public Overridable Property DESCRIPCION As String
			Get
				Return MyBase.GetString(ColumnNames.DESCRIPCION)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.DESCRIPCION, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_IDRECURSOS As String
			Get
				If Me.IsColumnNull(ColumnNames.IDRECURSOS) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDRECURSOS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDRECURSOS)
				Else
					Me.IDRECURSOS = MyBase.SetDecimalAsString(ColumnNames.IDRECURSOS, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDCULTURAS As String
			Get
				If Me.IsColumnNull(ColumnNames.IDCULTURAS) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.IDCULTURAS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDCULTURAS)
				Else
					Me.IDCULTURAS = MyBase.SetStringAsString(ColumnNames.IDCULTURAS, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_NOMBRE As String
			Get
				If Me.IsColumnNull(ColumnNames.NOMBRE) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.NOMBRE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NOMBRE)
				Else
					Me.NOMBRE = MyBase.SetStringAsString(ColumnNames.NOMBRE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_DESCRIPCION As String
			Get
				If Me.IsColumnNull(ColumnNames.DESCRIPCION) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.DESCRIPCION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DESCRIPCION)
				Else
					Me.DESCRIPCION = MyBase.SetStringAsString(ColumnNames.DESCRIPCION, Value)
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
		
	
			Public ReadOnly Property IDRECURSOS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDRECURSOS, Parameters.IDRECURSOS)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IDCULTURAS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property NOMBRE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property DESCRIPCION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property IDRECURSOS() As WhereParameter 
			Get
				If _IDRECURSOS_W Is Nothing Then
					_IDRECURSOS_W = TearOff.IDRECURSOS
				End If
				Return _IDRECURSOS_W
			End Get
		End Property

		Public ReadOnly Property IDCULTURAS() As WhereParameter 
			Get
				If _IDCULTURAS_W Is Nothing Then
					_IDCULTURAS_W = TearOff.IDCULTURAS
				End If
				Return _IDCULTURAS_W
			End Get
		End Property

		Public ReadOnly Property NOMBRE() As WhereParameter 
			Get
				If _NOMBRE_W Is Nothing Then
					_NOMBRE_W = TearOff.NOMBRE
				End If
				Return _NOMBRE_W
			End Get
		End Property

		Public ReadOnly Property DESCRIPCION() As WhereParameter 
			Get
				If _DESCRIPCION_W Is Nothing Then
					_DESCRIPCION_W = TearOff.DESCRIPCION
				End If
				Return _DESCRIPCION_W
			End Get
		End Property

		Private _IDRECURSOS_W As WhereParameter = Nothing
		Private _IDCULTURAS_W As WhereParameter = Nothing
		Private _NOMBRE_W As WhereParameter = Nothing
		Private _DESCRIPCION_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_IDRECURSOS_W = Nothing
			_IDCULTURAS_W = Nothing
			_NOMBRE_W = Nothing
			_DESCRIPCION_W = Nothing
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
		
	
		Public ReadOnly Property IDRECURSOS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDRECURSOS, Parameters.IDRECURSOS)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IDCULTURAS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property NOMBRE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property DESCRIPCION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property IDRECURSOS() As AggregateParameter 
			Get
				If _IDRECURSOS_W Is Nothing Then
					_IDRECURSOS_W = TearOff.IDRECURSOS
				End If
				Return _IDRECURSOS_W
			End Get
		End Property

		Public ReadOnly Property IDCULTURAS() As AggregateParameter 
			Get
				If _IDCULTURAS_W Is Nothing Then
					_IDCULTURAS_W = TearOff.IDCULTURAS
				End If
				Return _IDCULTURAS_W
			End Get
		End Property

		Public ReadOnly Property NOMBRE() As AggregateParameter 
			Get
				If _NOMBRE_W Is Nothing Then
					_NOMBRE_W = TearOff.NOMBRE
				End If
				Return _NOMBRE_W
			End Get
		End Property

		Public ReadOnly Property DESCRIPCION() As AggregateParameter 
			Get
				If _DESCRIPCION_W Is Nothing Then
					_DESCRIPCION_W = TearOff.DESCRIPCION
				End If
				Return _DESCRIPCION_W
			End Get
		End Property

		Private _IDRECURSOS_W As AggregateParameter = Nothing
		Private _IDCULTURAS_W As AggregateParameter = Nothing
		Private _NOMBRE_W As AggregateParameter = Nothing
		Private _DESCRIPCION_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_IDRECURSOS_W = Nothing
		_IDCULTURAS_W = Nothing
		_NOMBRE_W = Nothing
		_DESCRIPCION_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_RECURSOSCULTURAS" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_RECURSOSCULTURAS" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_RECURSOSCULTURAS" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDCULTURAS)
		p.SourceColumn = ColumnNames.IDCULTURAS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDRECURSOS)
		p.SourceColumn = ColumnNames.IDRECURSOS
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDRECURSOS)
		p.SourceColumn = ColumnNames.IDRECURSOS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDCULTURAS)
		p.SourceColumn = ColumnNames.IDCULTURAS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.NOMBRE)
		p.SourceColumn = ColumnNames.NOMBRE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DESCRIPCION)
		p.SourceColumn = ColumnNames.DESCRIPCION
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

