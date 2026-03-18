Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class BrainDAL
        Inherits DALBase

#Region "UNIDAD MEDIDA"

        ''' <summary>
        ''' Devuelve la unidad de medida por su identificador y planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreUnidadMedida(ByVal idUnidad As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_02N WHERE ELTO='" & idUnidad & "' AND EMPRESA='" & empresa & "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim unidadMedida As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    unidadMedida.ELTO = oReader.Item("ELTO")
                    unidadMedida.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return unidadMedida
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todas las unidades de medida
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarUnidadesMedida(ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_02N WHERE EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaUnidadesMedida As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaUnidadesMedida.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaUnidadesMedida
        End Function

        ''' <summary>
        ''' Comprueba si existe la unidad de medida por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarUnidadMedida(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_02N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim unidadMedida As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    unidadMedida.ELTO = oReader.Item("ELTO")
                    unidadMedida.DENO_S = oReader.Item("DENO_S")
                    Exit While
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return unidadMedida
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene las unidades de medida que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarUnidadesMedida(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_02N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaUnidadesMedida As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaUnidadesMedida.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaUnidadesMedida
        End Function

#End Region

#Region "CATEGORIAS PRODUCTO"

        ''' <summary>
        ''' Devuelve la categoría de producto por su identificador y planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreCategoriaProducto(ByVal idCategoriaProducto As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TAN WHERE ELTO='" & idCategoriaProducto & "' AND EMPRESA='" & empresa & "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim categoria As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    categoria.ELTO = oReader.Item("ELTO")
                    categoria.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return categoria
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todas las categorias de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarCategoriasProducto(ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TAN WHERE EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaCategoriasProducto As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaCategoriasProducto.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaCategoriasProducto
        End Function

        ''' <summary>
        ''' Comprueba si existe la unidad de medida por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarCategoriaProducto(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TAN WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim categoriaProducto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    categoriaProducto.ELTO = oReader.Item("ELTO")
                    categoriaProducto.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return categoriaProducto
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene las unidades de medida que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarCategoriasProducto(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TAN WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaCategoriasProducto As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaCategoriasProducto.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaCategoriasProducto
        End Function

#End Region

#Region "GRUPOS MATERIAL"

        ''' <summary>
        ''' Comprueba si existe el grupo de material por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreGrupoMaterial(ByVal idGrupoMaterial As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_40N WHERE ELTO='" & idGrupoMaterial & "' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim grupoMaterial As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    grupoMaterial.ELTO = oReader.Item("ELTO")
                    grupoMaterial.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return grupoMaterial
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todos los grupos de material
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarGruposMaterial() As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_40N WHERE EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaGruposMaterial As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaGruposMaterial.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaGruposMaterial
        End Function

        ''' <summary>
        ''' Comprueba si existe el grupo de material por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarGrupoMaterial(ByVal texto As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_40N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim grupoMaterial As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    grupoMaterial.ELTO = oReader.Item("ELTO")
                    grupoMaterial.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return grupoMaterial
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene los grupos de material que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarGruposMaterial(ByVal texto As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_40N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaGruposMaterial As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaGruposMaterial.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaGruposMaterial
        End Function

#End Region

#Region "GRUPOS PRODUCTO"

        ''' <summary>
        ''' Comprueba si existe el grupo de material por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreGrupoProducto(ByVal idGrupoProducto As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_61N WHERE ELTO='" & idGrupoProducto & "' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim grupoProducto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    grupoProducto.ELTO = oReader.Item("ELTO")
                    grupoProducto.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return grupoProducto
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todos los grupos de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarGruposProducto() As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_61N WHERE EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaGruposProducto As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaGruposProducto.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Dim listaNuevos As New List(Of ELL.BrainBase)
            For Each gp In listaGruposProducto
                Dim grupoProductoBLL As New BLL.GrupoProductoBLL()
                Dim grupos As List(Of ELL.GrupoProducto) = grupoProductoBLL.CargarListado(gp.ELTO)

                'Por cada grupo nos puede devolver más de un código de grupo
                Dim cont As Integer = 1
                For Each g In grupos
                    If (cont = 1) Then
                        gp.CodigoProducto = g.CodigoProducto
                        gp.Producto = g.Descripcion
                    Else
                        listaNuevos.Add(New ELL.BrainBase With {.ELTO = gp.ELTO, .DENO_S = gp.DENO_S, .CodigoProducto = g.CodigoProducto, .Producto = g.Descripcion})
                    End If
                    cont += 1
                Next
            Next
            listaGruposProducto.AddRange(listaNuevos)

            Return listaGruposProducto
        End Function

        ''' <summary>
        ''' Comprueba si existe el grupo de producto por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarGrupoProducto(ByVal texto As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_61N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim grupoProducto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    grupoProducto.ELTO = oReader.Item("ELTO")
                    grupoProducto.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return grupoProducto
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene los grupos de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarGruposProducto(ByVal texto As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_61N WHERE (LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' OR LOWER(ELTO) LIKE '%' || '" + texto.ToLower + "' || '%') AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaGruposProducto As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaGruposProducto.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Dim listaNuevos As New List(Of ELL.BrainBase)
            For Each gp In listaGruposProducto
                Dim grupoProductoBLL As New BLL.GrupoProductoBLL()
                Dim grupos As List(Of ELL.GrupoProducto) = grupoProductoBLL.CargarListado(gp.ELTO)

                'Por cada grupo nos puede devolver más de un código de grupo
                Dim cont As Integer = 1
                For Each g In grupos
                    If (cont = 1) Then
                        gp.CodigoProducto = g.CodigoProducto
                        gp.Producto = g.Descripcion
                    Else
                        listaNuevos.Add(New ELL.BrainBase With {.ELTO = gp.ELTO, .DENO_S = gp.DENO_S, .CodigoProducto = g.CodigoProducto, .Producto = g.Descripcion})
                    End If
                    cont += 1
                Next
            Next
            listaGruposProducto.AddRange(listaNuevos)

            Return listaGruposProducto
        End Function

#End Region

#Region "TIPOS PRODUCTO"

        ''' <summary>
        ''' Comprueba si existe el grupo de material por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreTipoProducto(ByVal idTipoProducto As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TVN WHERE ELTO='" & idTipoProducto & "' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim tipoProducto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    tipoProducto.ELTO = oReader.Item("ELTO")
                    tipoProducto.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return tipoProducto
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todos los tipos de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTiposProducto() As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TVN WHERE EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaTiposProducto As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaTiposProducto.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaTiposProducto
        End Function

        ''' <summary>
        ''' Comprueba si existe el grupo de producto por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTipoProducto(ByVal texto As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TVN WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim tipoProducto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    tipoProducto.ELTO = oReader.Item("ELTO")
                    tipoProducto.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return tipoProducto
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene los grupos de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTiposProducto(ByVal texto As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_TVN WHERE (LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' OR LOWER(ELTO) LIKE '%' || '" + texto.ToLower + "' || '%') AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaTiposProducto As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaTiposProducto.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaTiposProducto
        End Function

#End Region

#Region "TIPOS PIEZA"

        ''' <summary>
        ''' Comprueba si existe el grupo de material por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreTipoPieza(ByVal idTipoPieza As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_01N WHERE ELTO='" & idTipoPieza & "' AND EMPRESA='1' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim tipoPieza As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    tipoPieza.ELTO = oReader.Item("ELTO")
                    tipoPieza.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return tipoPieza
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todos los tipos de pieza
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTiposPieza() As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_01N WHERE PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaTiposPieza As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaTiposPieza.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaTiposPieza
        End Function

        ''' <summary>
        ''' Comprueba si existe el tipo de pieza por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTipoPieza(ByVal texto As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_01N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim tipoPieza As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    tipoPieza.ELTO = oReader.Item("ELTO")
                    tipoPieza.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return tipoPieza
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene los tipos de pieza que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarTiposPieza(ByVal texto As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_01N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaTiposPieza As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaTiposPieza.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaTiposPieza
        End Function

        ' ''' <summary>
        ' ''' Obtiene todos los tipos de pieza
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>        
        'Public Function CargarTiposPieza(ByVal empresa As String) As List(Of ELL.BrainBase)
        '    Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        '    Dim cn As New OleDb.OleDbConnection(conexion)
        '    Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_01N WHERE EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

        '    Dim cm As New OleDb.OleDbCommand(query, cn)
        '    cm.CommandTimeout = 30
        '    Dim oReader As OleDb.OleDbDataReader = Nothing
        '    Dim listaTiposPieza As New List(Of ELL.BrainBase)
        '    Try
        '        cn.Open()
        '        oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        '        While oReader.Read
        '            listaTiposPieza.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
        '        End While
        '    Catch
        '    Finally
        '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
        '    End Try

        '    Return listaTiposPieza
        'End Function

        ' ''' <summary>
        ' ''' Comprueba si existe el tipo de pieza por el texto introducido
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>        
        'Public Function CargarTipoPieza(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
        '    Dim contador As Integer = 0

        '    Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        '    Dim cn As New OleDb.OleDbConnection(conexion)
        '    Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_01N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

        '    Dim cm As New OleDb.OleDbCommand(query, cn)
        '    cm.CommandTimeout = 30
        '    Dim oReader As OleDb.OleDbDataReader = Nothing
        '    Dim tipoPieza As New ELL.BrainBase
        '    Try
        '        cn.Open()
        '        oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        '        While oReader.Read
        '            contador += 1
        '            tipoPieza.ELTO = oReader.Item("ELTO")
        '            tipoPieza.DENO_S = oReader.Item("DENO_S")
        '        End While
        '    Catch
        '    Finally
        '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
        '    End Try

        '    If (contador = 1) Then
        '        Return tipoPieza
        '    Else
        '        Return Nothing
        '    End If
        'End Function

        ' ''' <summary>
        ' ''' Obtiene los tipos de pieza que cumplan los requisitos del texto introducido
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>        
        'Public Function CargarTiposPieza(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
        '    Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        '    Dim cn As New OleDb.OleDbConnection(conexion)
        '    Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_01N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

        '    Dim cm As New OleDb.OleDbCommand(query, cn)
        '    cm.CommandTimeout = 30
        '    Dim oReader As OleDb.OleDbDataReader = Nothing
        '    Dim listaTiposPieza As New List(Of ELL.BrainBase)
        '    Try
        '        cn.Open()
        '        oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        '        While oReader.Read
        '            listaTiposPieza.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
        '        End While
        '    Catch
        '    Finally
        '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
        '    End Try

        '    Return listaTiposPieza
        'End Function

#End Region

#Region "DISPONENTES"

        ''' <summary>
        ''' Devuelve la categoría de producto por su identificador y planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreDisponente(ByVal idDisponente As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_11N WHERE ELTO='" & idDisponente & "' AND EMPRESA='" & empresa & "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim disponente As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    disponente.ELTO = oReader.Item("ELTO")
                    disponente.DENO_S = oReader.Item("DENO_S")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return disponente
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todos los disponentes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarDisponentes(ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_11N WHERE EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaDisponentes As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaDisponentes.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaDisponentes
        End Function

        ''' <summary>
        ''' Comprueba si existe el disponente por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarDisponente(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_11N WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim disponente As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    disponente.ELTO = oReader.Item("ELTO")
                    disponente.DENO_S = oReader.Item("DENO_S")
                    Exit While
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return disponente
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene los disponentes que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarDisponentes(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_11N WHERE (LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' OR LOWER(ELTO) LIKE '%' || '" + texto.ToLower + "' || '%') AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY ELTO ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaDisponentes As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaDisponentes.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaDisponentes
        End Function

#End Region

#Region "ALMACENES"

        ''' <summary>
        ''' Devuelve el almacen por su identificador y planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreAlmacen(ByVal idAlmacen As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT LCLANR, LCNAM1 FROM X500PRDSD.LAGR WHERE LCLANR='" & idAlmacen & "' AND LCFIRM='" & empresa & "' AND LCWKNR='000' AND LCSTAP='1' ORDER BY LCLANR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim almacen As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    almacen.ELTO = oReader.Item("LCLANR")
                    almacen.DENO_S = oReader.Item("LCNAM1")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return almacen
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todos los almacenes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarAlmacenes(ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT LCLANR, LCNAM1 FROM X500PRDSD.LAGR WHERE LCFIRM='" + empresa + "' AND LCWKNR='000' AND LCSTAP='1' ORDER BY LCLANR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaAlmacenes As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaAlmacenes.Add(New ELL.BrainBase With {.ELTO = oReader.Item("LCLANR"), .DENO_S = oReader.Item("LCNAM1")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaAlmacenes
        End Function

        ''' <summary>
        ''' Comprueba si existe el almacen por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarAlmacen(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT LCLANR, LCNAM1 FROM X500PRDSD.LAGR WHERE LOWER(LCNAM1) LIKE '%' || '" + texto.ToLower + "' || '%' AND LCFIRM='" + empresa + "' AND AND LCWKNR='000' AND LCSTAP='1' ORDER BY LCLANR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim almacen As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    almacen.ELTO = oReader.Item("LCLANR")
                    almacen.DENO_S = oReader.Item("LCNAM1")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return almacen
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene los almacenes que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarAlmacenes(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT LCLANR, LCNAM1 FROM X500PRDSD.LAGR WHERE (LOWER(LCNAM1) LIKE '%' || '" + texto.ToLower + "' || '%' OR LOWER(LCLANR) LIKE '%' || '" + texto.ToLower + "' || '%') AND LCFIRM='" + empresa + "' AND LCWKNR='000' AND LCSTAP='1' ORDER BY LCLANR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaAlmacenes As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaAlmacenes.Add(New ELL.BrainBase With {.ELTO = oReader.Item("LCLANR"), .DENO_S = oReader.Item("LCNAM1")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaAlmacenes
        End Function

#End Region

#Region "PROYECTOS"

        ''' <summary>
        ''' Obtiene todos los proyectos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList(ByVal texto As String) As List(Of ELL.Proyectos)
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            Dim query As String = "SELECT ID, NOMBRE FROM PROYECTOS WHERE LOWER(NOMBRE) LIKE '%' || :TEXTO || '%' AND EMPRESA='1' AND PLANTA='000' ORDER BY NOMBRE ASC"
            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 20, texto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, conexion, parameter)
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectoPorId(ByVal idProyecto As String) As String
            Dim nombre As String = String.Empty
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString

            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT ELTO, DENO_S FROM CUBOS.T_AVN WHERE ELTO='" + idProyecto + "' AND EMPRESA='1' AND PLANTA='000'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim proyecto As New ELL.Proyectos
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    nombre = oReader.Item("DENO_S").ToString
                End While
                Return nombre
            Catch
                Return Nothing
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectosBrain() As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_AVN WHERE EMPRESA='1' AND PLANTA='000' ORDER BY DENO_S ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim proyectos As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    proyectos.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return proyectos
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectoBrain(ByVal texto As String) As ELL.BrainBase
            Dim encontrado As Boolean = False
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString

            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT ELTO, DENO_S FROM CUBOS.T_AVN WHERE LOWER(DENO_S) = '" + texto.ToLower + "' AND EMPRESA='1' AND PLANTA='000'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim proyecto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    encontrado = True
                    proyecto.ELTO = oReader.Item("ELTO")
                    proyecto.DENO_S = oReader.Item("DENO_S")
                End While
                If (encontrado) Then
                    Return proyecto
                Else
                    Return Nothing
                End If
            Catch
                Return Nothing
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectosBrain(ByVal texto As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            'Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_AVN WHERE LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='1' AND PLANTA='000' ORDER BY DENO_S ASC"
            Dim query As String = "SELECT DISTINCT ELTO, DENO_S FROM CUBOS.T_AVN WHERE (LOWER(DENO_S) LIKE '%' || '" + texto.ToLower + "' || '%' OR LOWER(ELTO) LIKE '%' || '" + texto.ToLower + "' || '%') AND EMPRESA='1' AND PLANTA='000' ORDER BY DENO_S ASC"


            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim proyectos As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    proyectos.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENO_S")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return proyectos
        End Function

#End Region

#Region "SUBPROYECTOS"

        ''' <summary>
        ''' Devuelve el subproyecto por su identificador y planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarNombreSubproyecto(ByVal idSubproyecto As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            'Dim query As String = "SELECT DISTINCT ELTO, DENOM FROM CUBOS.T_44POR WHERE ELTO='" & idSubproyecto & "' AND EMPRESA='" & empresa & "' AND TBWKNR='000' ORDER BY ELTO ASC"
            Dim query As String = "SELECT DISTINCT VALOR, DENOM FROM CUBOS.T_44N WHERE VALOR='" & idSubproyecto & "' AND EMPRESA='" & empresa & "' AND PLANTA='000' ORDER BY VALOR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim subproyecto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    subproyecto.ELTO = oReader.Item("ELTO")
                    subproyecto.DENO_S = oReader.Item("DENOM")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return subproyecto
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene todos los subproyectos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarSubproyectos(ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            'Dim query As String = "SELECT DISTINCT ELTO, DENOM FROM CUBOS.T_44POR WHERE EMPRESA='" + empresa + "' AND TBWKNR='000' ORDER BY ELTO ASC"
            Dim query As String = "SELECT VALOR, DENOM FROM CUBOS.T_44N WHERE EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY VALOR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaSubproyectos As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaSubproyectos.Add(New ELL.BrainBase With {.ELTO = oReader.Item("VALOR"), .DENO_S = oReader.Item("DENOM")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaSubproyectos
        End Function

        ''' <summary>
        ''' Comprueba si existe el subproyecto por el texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarSubproyecto(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            'Dim query As String = "SELECT DISTINCT ELTO, DENOM FROM CUBOS.T_44POR WHERE LOWER(DENOM) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND TBWKNR='000' ORDER BY ELTO ASC"
            Dim query As String = "SELECT DISTINCT VALOR, DENOM FROM CUBOS.T_44N WHERE LOWER(DENOM) LIKE '%' || '" + texto.ToLower + "' || '%' AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY VALOR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim subproyecto As New ELL.BrainBase
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                    subproyecto.ELTO = oReader.Item("ELTO")
                    subproyecto.DENO_S = oReader.Item("DENOM")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            If (contador = 1) Then
                Return subproyecto
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Obtiene los subproyectos que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarSubproyectos(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            'Dim query As String = "SELECT DISTINCT ELTO, DENOM FROM CUBOS.T_44POR WHERE (LOWER(DENOM) LIKE '%' || '" + texto.ToLower + "' || '%' OR LOWER(ELTO) LIKE '%' || '" + texto.ToLower + "' || '%') AND EMPRESA='" + empresa + "' AND TBWKNR='000' ORDER BY ELTO ASC"
            Dim query As String = "SELECT DISTINCT VALOR, DENOM FROM CUBOS.T_44N WHERE (LOWER(DENOM) LIKE '%' || '" + texto.ToLower + "' || '%' OR LOWER(VALOR) LIKE '%' || '" + texto.ToLower + "' || '%') AND EMPRESA='" + empresa + "' AND PLANTA='000' ORDER BY VALOR ASC"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaSubproyectos As New List(Of ELL.BrainBase)
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaSubproyectos.Add(New ELL.BrainBase With {.ELTO = oReader.Item("ELTO"), .DENO_S = oReader.Item("DENOM")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaSubproyectos
        End Function

#End Region

#Region "PORTADORES DE COSTE"

        ''' <summary>
        ''' Devuelve el siguiente portador RP de coste para un producto
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <param name="anteriorBatzPN"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function GetSiguienteBatzPN(ByVal producto As String, ByRef anteriorBatzPN As String) As String
            Dim contador As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT REPLACE(UPPER(TRIM(VALOR)),'RP{0}','') NUMPROYECTO " _
                                  & "FROM CUBOS.T_44N " _
                                  & "WHERE UPPER(VALOR) LIKE 'RP{0}%' AND LANTEGI <> 'OBS' AND LANTEGI <>'' " _
                                  & "AND LENGTH(TRIM(TRANSLATE(REPLACE(UPPER(TRIM(VALOR)),'RP{0}',''), '*', ' 0123456789'))) = 0 " _
                                  & "ORDER BY LENGTH(NUMPROYECTO) DESC, NUMPROYECTO DESC"

            query = String.Format(query, producto)

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim valor As String = String.Empty
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

                'Sólo necesitamos leer el primer valor
                oReader.Read()
                valor = oReader.Item("NUMPROYECTO")
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            anteriorBatzPN = String.Format("8100{0}{1}", producto.PadLeft(2, "0"), valor.PadLeft(3, "0"))

            ' El valor puede tener 0 por delante. Lo convertimos a número y le sumamos uno
            Dim valorEntero As Integer = CInt(valor)

            ' Le sumamos uno a dicho valor entero
            Dim siguientePartNumber As String = valorEntero + 1

            'Rellenamos con 0 por delante hasta la longitud de la cadena devuelta por XPERT
            siguientePartNumber.PadLeft(valor.Length, "0")

            'Componemos el número
            siguientePartNumber = String.Format("8100{0}{1}", producto.PadLeft(2, "0"), siguientePartNumber.PadLeft(3, "0"))

            Return siguientePartNumber
        End Function

#End Region

#Region "PREVIOUS BATZ PART NUMBER"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="texto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPreviousBatzPN(ByVal texto As String) As List(Of ELL.BrainBase)
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "Select SPTENR FROM CUBOS.SOLICIPZA WHERE LOWER(SPTENR) Like '%' || '" + texto.ToLower + "' || '%'"
            Dim existe As Boolean = False

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim listaPreviousBatzPN As New List(Of ELL.BrainBase)

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    listaPreviousBatzPN.Add(New ELL.BrainBase With {.ELTO = oReader.Item("SPTENR"), .DENO_S = oReader.Item("SPTENR")})
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return listaPreviousBatzPN
        End Function

#End Region

#Region "SELECT, INSERT, UPDATE, DELETE REFERENCIAS EN BRAIN"

#Region "SELECT"

        ''' <summary>
        ''' Cargar los datos de una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferenciaBrain(ByVal referencia As String) As ELL.DatosBrain
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT * FROM CUBOS.SOLICIPZA WHERE LOWER(SPTENR)='" & referencia.ToLower & "'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim datosBrain As New ELL.DatosBrain
            Dim listaBrainPlanta As New List(Of ELL.DatosBrain.InformacionPlanta)

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    datosBrain.Planta = oReader.Item("SPWKNR")
                    datosBrain.RefPieza = oReader.Item("SPTENR")
                    datosBrain.Estado = oReader.Item("SPSTAP")
                    datosBrain.Descripcion1 = oReader.Item("SPBEZ1")
                    datosBrain.Descripcion2 = oReader.Item("SPBEZ2")
                    datosBrain.MatchCode = oReader.Item("SPMATC")
                    datosBrain.NumDin = oReader.Item("SPDINR")
                    datosBrain.TipoPieza = oReader.Item("SPTART")
                    datosBrain.PasarDespieceWeb = oReader.Item("SPKDF4")
                    datosBrain.PseudoSubconjunto = oReader.Item("SPAUCD")
                    datosBrain.GrupoMaterial = oReader.Item("SPMAGR")
                    datosBrain.GrupoProducto = oReader.Item("SPPRGR")
                    datosBrain.RefClientePlanoBatz = oReader.Item("SPZINR")
                    datosBrain.NivelIngenieria = oReader.Item("SPASBZ")
                    datosBrain.PlanoWeb = oReader.Item("SPRHF4")
                    datosBrain.PesoNeto = oReader.Item("SPGWNE")
                    datosBrain.PiezaCompraDirigida = oReader.Item("SPIRVT")
                    datosBrain.Proyecto = oReader.Item("SPRHF5")
                    datosBrain.TipoProducto = oReader.Item("SPKLNR")
                    datosBrain.Observaciones = oReader.Item("SPRHF6")

                    Dim datosBrainPlanta As New ELL.DatosBrain.InformacionPlanta
                    datosBrainPlanta.Empresa = oReader.Item("SPFIRM")
                    datosBrainPlanta.UnidadMedidaCantidad = oReader.Item("SPMEIN")
                    datosBrainPlanta.UnidadMedidaPrecio = oReader.Item("SPMEPR")
                    datosBrainPlanta.CategoriaProducto = oReader.Item("SPPRKA")
                    datosBrainPlanta.Disponente = oReader.Item("SPDISP")
                    datosBrainPlanta.NumAlmacen = oReader.Item("SPLANR")
                    datosBrainPlanta.ControlCalidad = oReader.Item("SPWEPR")
                    datosBrainPlanta.Subproyecto = oReader.Item("SPKLAS")
                    datosBrainPlanta.FlagCompleto = oReader.Item("SPCOMP")
                    datosBrainPlanta.FechaIntegracion = oReader.Item("SPDATE")
                    datosBrainPlanta.FlagCorrecto = oReader.Item("SPFLAG")
                    datosBrainPlanta.CausaError = oReader.Item("SPCAUS")
                    listaBrainPlanta.Add(datosBrainPlanta)
                End While

                datosBrain.InfoPlanta = listaBrainPlanta
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return datosBrain
        End Function

        ' ''' <summary>
        ' ''' Verifica si existe una referencia guardada en Brain
        ' ''' </summary>
        ' ''' <param name="referencia"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function ExisteReferenciaBrain(ByVal referencia As String) As Boolean
        '    Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        '    Dim cn As New OleDb.OleDbConnection(conexion)
        '    Dim query As String = "SELECT * FROM CUBOS.SOLICIPZA WHERE SPTENR='" & referencia & "'"
        '    Dim existe As Boolean = False

        '    Dim cm As New OleDb.OleDbCommand(query, cn)
        '    cm.CommandTimeout = 30
        '    Dim oReader As OleDb.OleDbDataReader = Nothing
        '    Dim datosBrain As New ELL.DatosBrain
        '    Dim listaBrainPlanta As New List(Of ELL.DatosBrain.InformacionPlanta)

        '    Try
        '        cn.Open()
        '        oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        '        If oReader.Read Then
        '            existe = True
        '        End If
        '    Catch
        '    Finally
        '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
        '    End Try

        '    Return existe
        'End Function

        ''' <summary>
        ''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarIntegracionReferenciasBrain(ByVal idSolicitud As Integer, ByVal numPlantas As Integer) As Boolean
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT * FROM CUBOS.SOLICIPZA WHERE SPSOLI='" & idSolicitud.ToString & "' AND SPFLAG='S' AND SPCAUS=''"
            Dim contador As Integer = 0

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador += 1
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return If(contador = numPlantas, True, False)
        End Function

        ''' <summary>
        ''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarReferenciasBrainSolicipza(ByVal idSolicitud As Integer) As Integer
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT COUNT(*) as NUMERO FROM CUBOS.SOLICIPZA WHERE SPSOLI='" & idSolicitud.ToString & "' AND SPCOMP='N'"
            Dim contador As String = "0"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador = oReader.Item("NUMERO")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return contador
        End Function

        ''' <summary>
        ''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarInegracionReferenciasBrain(ByVal idSolicitud As Integer) As Integer
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT COUNT(*) as NUMERO FROM CUBOS.SOLICIPZA WHERE SPSOLI='" & idSolicitud.ToString & "' AND SPDATE<>'0' AND SPFLAG='S'"
            Dim contador As String = "0"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador = oReader.Item("NUMERO")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return contador
        End Function

        ''' <summary>
        ''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarInegracionReferenciasBrainSolicipza(ByVal idSolicitud As Integer, Optional ByVal idRef As String = "") As Integer
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim contador As String = "0"

            Dim query As String = "SELECT COUNT(*) as NUMERO FROM CUBOS.SOLICIPZA WHERE SPSOLI='" & idSolicitud.ToString & "' AND SPDATE<>'0' AND SPFLAG='S'"
            If Not (String.IsNullOrEmpty(idRef)) Then
                query &= " AND SPZINR='" & idRef & "'"
            End If
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing

            Try

                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    contador = oReader.Item("NUMERO")
                End While
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return contador
        End Function

        ''' <summary>
        ''' Comprobar el número de plantas afectadas en total para una referencia de venta (la suma de todas las plantas por de la referencia)
        ''' </summary>
        ''' <param name="idRef">Identificador de la referencia</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPlantasAfectadasReferencia(ByVal idRef As String, ByVal idSol As Integer) As Integer
            Try
                Dim lParameters As New List(Of OracleParameter)
                Dim query As String = "SELECT COUNT(*) " &
                                      "FROM REFERENCIAS_PLANTAS " &
                                      "INNER JOIN REFERENCIAS_VENTA on REFERENCIAS_VENTA.ID=REFERENCIAS_PLANTAS.ID_REFERENCIA " &
                                      "WHERE REFERENCIAS_VENTA.REFERENCIA_CLIENTE=:ID_REFERENCIA AND REFERENCIAS_VENTA.ID_SOLICITUD=:ID_SOLICITUD "
                lParameters.Add(New OracleParameter("ID_REFERENCIA", OracleDbType.NVarchar2, 19, idRef, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, idSol, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexionReferenciasVenta, lParameters.ToArray)
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region

#Region "INSERT"

        ''' <summary>
        ''' Guarda en Brain los datos de una referencia
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function GuardarReferenciaBrain_old(ByVal datosBrain As ELL.DatosBrain, ByVal procesadoListo As Boolean) As Boolean
            Dim query As String = String.Empty
            Dim trans As OleDb.OleDbTransaction = Nothing
            Dim cn As OleDb.OleDbConnection = Nothing
            Dim resultado As Boolean = False

            Try
                Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
                cn = New OleDb.OleDbConnection(conexion)

                cn.Open()

                trans = cn.BeginTransaction()

                For Each planta In datosBrain.InfoPlanta
                    query = "INSERT INTO CUBOS.SOLICIPZA(SPFIRM, SPWKNR, SPSOLI, SPTENR, SPSTAP, SPBEZ1, "
                    If Not (String.IsNullOrEmpty(datosBrain.Descripcion2)) Then
                        query &= "SPBEZ2, "
                    End If
                    query &= "SPMATC, "
                    If Not (String.IsNullOrEmpty(datosBrain.NumDin)) Then
                        query &= "SPDINR, "
                    End If
                    query &= "SPTART, SPKDF4, SPAUCD, SPMEIN, SPMEPR, SPPRKA, SPMAGR, "
                    If Not (String.IsNullOrEmpty(datosBrain.GrupoProducto)) Then
                        query &= "SPPRGR, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.RefClientePlanoBatz)) Then
                        query &= "SPZINR, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.NivelIngenieria)) Then
                        query &= "SPASBZ, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PlanoWeb)) Then
                        query &= "SPRHF4, "
                    End If
                    If Not (String.IsNullOrEmpty(planta.Disponente)) Then
                        query &= "SPDISP, "
                    End If
                    If Not (String.IsNullOrEmpty(planta.NumAlmacen)) Then
                        query &= "SPLANR, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PesoNeto)) Then
                        query &= "SPGWNE, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Observaciones)) Then
                        query &= "SPRHF6, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Comentario)) Then
                        query &= "SPTEXT, "
                    End If
                    query &= "SPWEPR, SPIRVT, SPRHF5, SPKLAS, SPKLNR, SPCOMP) "
                    query += "VALUES('" & planta.Empresa & "' , '" & datosBrain.Planta & "' , '" & datosBrain.IdSolicitud & "' , '" & datosBrain.RefPieza & "' , '" & datosBrain.Estado & "' , '" & datosBrain.Descripcion1 & "' , '"
                    If Not (String.IsNullOrEmpty(datosBrain.Descripcion2)) Then
                        query &= datosBrain.Descripcion2 & "' , '"
                    End If
                    query &= datosBrain.MatchCode & "' , '"
                    If Not (String.IsNullOrEmpty(datosBrain.NumDin)) Then
                        query &= datosBrain.NumDin & "' , '"
                    End If
                    query &= datosBrain.TipoPieza & "' , '" & datosBrain.PasarDespieceWeb & "' , '" & datosBrain.PseudoSubconjunto & "' , '" & planta.UnidadMedidaCantidad & "' , '" & planta.UnidadMedidaPrecio & "' , '" & planta.CategoriaProducto & "' , '" & datosBrain.GrupoMaterial & "' , '"
                    If Not (String.IsNullOrEmpty(datosBrain.GrupoProducto)) Then
                        query &= datosBrain.GrupoProducto & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.RefClientePlanoBatz)) Then
                        query &= datosBrain.RefClientePlanoBatz & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.NivelIngenieria)) Then
                        query &= datosBrain.NivelIngenieria & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PlanoWeb)) Then
                        query &= datosBrain.PlanoWeb & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(planta.Disponente)) Then
                        query &= planta.Disponente & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(planta.NumAlmacen)) Then
                        query &= planta.NumAlmacen & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PesoNeto)) Then
                        query = query.Substring(0, query.Length - 1)
                        query &= datosBrain.PesoNeto & " , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Observaciones)) Then
                        query &= datosBrain.Observaciones & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Comentario)) Then
                        query &= datosBrain.Comentario & "' , '"
                    End If
                    query &= planta.ControlCalidad & "' , '" & datosBrain.PiezaCompraDirigida & "' , '" & datosBrain.Proyecto & "' , '" & planta.Subproyecto & "' , '" & datosBrain.TipoProducto & "' , '" & planta.FlagCompleto & "')"

                    Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
                    cmd.CommandTimeout = 30
                    cmd.ExecuteNonQuery()
                Next

                If (procesadoListo) Then
                    For Each planta In datosBrain.InfoPlanta
                        query = "UPDATE CUBOS.SOLICIPZA SET SPCOMP='S' " &
                                "WHERE SPSOLI='" & datosBrain.IdSolicitud & "'"

                        Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
                        cmd.CommandTimeout = 30
                        cmd.ExecuteNonQuery()
                    Next
                End If

                resultado = True

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                resultado = False
            Finally
                If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
                    cn.Close()
                    cn.Dispose()
                End If
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Guarda en Brain los datos de una referencia
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function GuardarReferenciaBrain(ByVal datosBrain As ELL.DatosBrain, ByVal procesadoListo As Boolean) As Boolean
            Dim query As String = String.Empty
            Dim trans As OleDb.OleDbTransaction = Nothing
            Dim cn As OleDb.OleDbConnection = Nothing
            Dim resultado As Boolean = False

            Try
                Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
                cn = New OleDb.OleDbConnection(conexion)

                cn.Open()

                trans = cn.BeginTransaction()

                query = "DELETE FROM CUBOS.SOLICIPZA WHERE SPSOLI='" & datosBrain.IdSolicitud & "' AND SPTENR='" & datosBrain.RefPieza & "' AND SPZINR='" & datosBrain.RefClientePlanoBatz & "'"

                Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
                cmd.CommandTimeout = 30
                cmd.ExecuteNonQuery()

                For Each planta In datosBrain.InfoPlanta
                    query = "INSERT INTO CUBOS.SOLICIPZA(SPFIRM, SPWKNR, SPSOLI, SPTENR, SPSTAP, SPBEZ1, "
                    If Not (String.IsNullOrEmpty(datosBrain.Descripcion2)) Then
                        query &= "SPBEZ2, "
                    End If
                    query &= "SPMATC, "
                    If Not (String.IsNullOrEmpty(datosBrain.NumDin)) Then
                        query &= "SPDINR, "
                    End If
                    query &= "SPTART, SPKDF4, SPAUCD, SPMEIN, SPMEPR, SPPRKA, SPMAGR, "
                    If Not (String.IsNullOrEmpty(datosBrain.GrupoProducto)) Then
                        query &= "SPPRGR, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.RefClientePlanoBatz)) Then
                        query &= "SPZINR, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.NivelIngenieria)) Then
                        query &= "SPASBZ, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PlanoWeb)) Then
                        query &= "SPRHF4, "
                    End If
                    If Not (String.IsNullOrEmpty(planta.Disponente)) Then
                        query &= "SPDISP, "
                    End If
                    If Not (String.IsNullOrEmpty(planta.NumAlmacen)) Then
                        query &= "SPLANR, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PesoNeto)) Then
                        query &= "SPGWNE, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Observaciones)) Then
                        query &= "SPRHF6, "
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Comentario)) Then
                        query &= "SPTEXT, "
                    End If
                    query &= "SPWEPR, SPIRVT, SPRHF5, SPKLAS, SPKLNR, SPCOMP) "
                    query += "VALUES('" & planta.Empresa & "' , '" & datosBrain.Planta & "' , '" & datosBrain.IdSolicitud & "' , '" & datosBrain.RefPieza & "' , '" & datosBrain.Estado & "' , '" & datosBrain.Descripcion1 & "' , '"
                    If Not (String.IsNullOrEmpty(datosBrain.Descripcion2)) Then
                        query &= datosBrain.Descripcion2 & "' , '"
                    End If
                    query &= datosBrain.MatchCode & "' , '"
                    If Not (String.IsNullOrEmpty(datosBrain.NumDin)) Then
                        query &= datosBrain.NumDin & "' , '"
                    End If
                    query &= datosBrain.TipoPieza & "' , '" & datosBrain.PasarDespieceWeb & "' , '" & datosBrain.PseudoSubconjunto & "' , '" & planta.UnidadMedidaCantidad & "' , '" & planta.UnidadMedidaPrecio & "' , '" & planta.CategoriaProducto & "' , '" & datosBrain.GrupoMaterial & "' , '"
                    If Not (String.IsNullOrEmpty(datosBrain.GrupoProducto)) Then
                        query &= datosBrain.GrupoProducto & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.RefClientePlanoBatz)) Then
                        query &= datosBrain.RefClientePlanoBatz & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.NivelIngenieria)) Then
                        query &= datosBrain.NivelIngenieria & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PlanoWeb)) Then
                        query &= datosBrain.PlanoWeb & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(planta.Disponente)) Then
                        query &= planta.Disponente & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(planta.NumAlmacen)) Then
                        query &= planta.NumAlmacen & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.PesoNeto)) Then
                        query = query.Substring(0, query.Length - 1)
                        query &= datosBrain.PesoNeto & " , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Observaciones)) Then
                        query &= datosBrain.Observaciones.Replace("'", "") & "' , '"
                    End If
                    If Not (String.IsNullOrEmpty(datosBrain.Comentario)) Then
                        query &= datosBrain.Comentario.Replace("'", "") & "' , '"
                    End If
                    query &= planta.ControlCalidad & "' , '" & datosBrain.PiezaCompraDirigida & "' , '" & datosBrain.Proyecto & "' , '" & planta.Subproyecto & "' , '" & datosBrain.TipoProducto & "' , 'N')"

                    cmd = New OleDb.OleDbCommand(query, cn, trans)
                    cmd.CommandTimeout = 30
                    cmd.ExecuteNonQuery()
                Next

                If (procesadoListo) Then
                    For Each planta In datosBrain.InfoPlanta
                        query = "UPDATE CUBOS.SOLICIPZA SET SPCOMP='S' " &
                                "WHERE SPSOLI='" & datosBrain.IdSolicitud & "'"

                        cmd = New OleDb.OleDbCommand(query, cn, trans)
                        cmd.CommandTimeout = 30
                        cmd.ExecuteNonQuery()
                    Next
                End If

                resultado = True

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                resultado = False
            Finally
                If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
                    cn.Close()
                    cn.Dispose()
                End If
            End Try
            Return resultado
        End Function

#End Region

#Region "UPDATE"

        '''' <summary>
        '''' Modifica en Brain los datos de una referencia
        '''' </summary>
        '''' <returns></returns>
        '''' <remarks></remarks>        
        'Public Function ModificarReferenciaBrain_old(ByVal datosBrain As ELL.DatosBrain) As Boolean
        '    Dim query As String = String.Empty
        '    Dim trans As OleDb.OleDbTransaction = Nothing
        '    Dim cn As OleDb.OleDbConnection = Nothing
        '    Dim resultado As Boolean = False

        '    Try
        '        Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        '        cn = New OleDb.OleDbConnection(conexion)

        '        cn.Open()

        '        trans = cn.BeginTransaction()

        '        For Each planta In datosBrain.InfoPlanta
        '            query = "UPDATE CUBOS.SOLICIPZA SET SPSTAP='" & datosBrain.Estado & "' , SPBEZ1='" & datosBrain.Descripcion1 & "' , SPBEZ2='" & datosBrain.Descripcion2 & "' , SPMATC='" & datosBrain.MatchCode & "' , " &
        '            "SPDINR='" & datosBrain.NumDin & "' , SPTART='" & datosBrain.TipoPieza & "' , SPKDF4='" & datosBrain.PasarDespieceWeb & "' , SPAUCD='" & datosBrain.PseudoSubconjunto & "' , SPMEIN='" & planta.UnidadMedidaCantidad & "' , " &
        '            "SPMEPR='" & planta.UnidadMedidaPrecio & "' , SPPRKA='" & planta.CategoriaProducto & "' , SPMAGR='" & datosBrain.GrupoMaterial & "' , SPPRGR='" & datosBrain.GrupoProducto & "' , SPZINR='" & datosBrain.RefClientePlanoBatz & "' , " &
        '            "SPASBZ='" & datosBrain.NivelIngenieria & "' , SPRHF4='" & datosBrain.PlanoWeb & "' , SPDISP='" & planta.Disponente & "' , " &
        '            "SPLANR='" & planta.NumAlmacen & "' , "
        '            If Not (String.IsNullOrEmpty(datosBrain.PesoNeto)) Then
        '                query &= "SPGWNE=" & datosBrain.PesoNeto & " , "
        '            End If
        '            query &= "SPWEPR='" & planta.ControlCalidad & "' , SPIRVT='" & datosBrain.PiezaCompraDirigida & "' , SPRHF5='" & datosBrain.Proyecto & "' , SPKLAS='" & planta.Subproyecto & "' , " &
        '            "SPKLNR='" & datosBrain.TipoProducto & "' , SPRHF6='" & datosBrain.Observaciones & "' , SPDATE=0 , SPFLAG='', SPCAUS='' " &
        '            "WHERE SPFIRM='" & planta.Empresa & "' AND SPWKNR='" & datosBrain.Planta & "' AND SPTENR='" & datosBrain.RefPieza & "'"

        '            'Make a Command for this connection
        '            'and this transaction.
        '            Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
        '            cmd.CommandTimeout = 30
        '            cmd.ExecuteNonQuery()
        '        Next

        '        resultado = True

        '        trans.Commit()

        '    Catch ex As Exception
        '        trans.Rollback()
        '        resultado = False
        '    Finally
        '        If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
        '            cn.Close()
        '            cn.Dispose()
        '        End If
        '    End Try
        '    Return resultado
        'End Function

        ''' <summary>
        ''' Modifica en Brain los datos de una referencia
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ModificarReferenciaBrain(ByVal datosBrain As ELL.DatosBrain) As Boolean
            Dim query As String = String.Empty
            Dim trans As OleDb.OleDbTransaction = Nothing
            Dim cn As OleDb.OleDbConnection = Nothing
            Dim resultado As Boolean = False

            Try
                Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
                cn = New OleDb.OleDbConnection(conexion)

                cn.Open()

                trans = cn.BeginTransaction()

                For Each planta In datosBrain.InfoPlanta
                    query = "UPDATE CUBOS.SOLICIPZA SET SPSTAP='" & datosBrain.Estado & "' , SPBEZ1='" & datosBrain.Descripcion1 & "' , SPBEZ2='" & datosBrain.Descripcion2 & "' , SPMATC='" & datosBrain.MatchCode & "' , " &
                    "SPDINR='" & datosBrain.NumDin & "' , SPTART='" & datosBrain.TipoPieza & "' , SPKDF4='" & datosBrain.PasarDespieceWeb & "' , SPAUCD='" & datosBrain.PseudoSubconjunto & "' , SPMEIN='" & planta.UnidadMedidaCantidad & "' , " &
                    "SPMEPR='" & planta.UnidadMedidaPrecio & "' , SPPRKA='" & planta.CategoriaProducto & "' , SPMAGR='" & datosBrain.GrupoMaterial & "' , SPPRGR='" & datosBrain.GrupoProducto & "' , SPZINR='" & datosBrain.RefClientePlanoBatz & "' , " &
                    "SPASBZ='" & datosBrain.NivelIngenieria & "' , SPRHF4='" & datosBrain.PlanoWeb & "' , SPDISP='" & planta.Disponente & "' , " & "SPCOMP='S' , " &
                    "SPLANR='" & planta.NumAlmacen & "' , "
                    If Not (String.IsNullOrEmpty(datosBrain.PesoNeto)) Then
                        query &= "SPGWNE=" & datosBrain.PesoNeto & " , "
                    End If
                    query &= "SPWEPR='" & planta.ControlCalidad & "' , SPIRVT='" & datosBrain.PiezaCompraDirigida & "' , SPRHF5='" & datosBrain.Proyecto & "' , SPKLAS='" & planta.Subproyecto & "' , " &
                    "SPKLNR='" & datosBrain.TipoProducto & "' , SPRHF6='" & datosBrain.Observaciones & "' , SPDATE=0 , SPFLAG='', SPCAUS='' " &
                    "WHERE SPFIRM='" & planta.Empresa & "' AND SPWKNR='" & datosBrain.Planta & "' AND SPTENR='" & datosBrain.RefPieza & "'"

                    'Make a Command for this connection
                    'and this transaction.
                    Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
                    cmd.CommandTimeout = 30
                    cmd.ExecuteNonQuery()
                Next

                resultado = True

                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                resultado = False
            Finally
                If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
                    cn.Close()
                    cn.Dispose()
                End If
            End Try
            Return resultado
        End Function

#End Region

#Region "DELETE"

        ' ''' <summary>
        ' ''' Eliminar una referencia de venta en Brain
        ' ''' </summary>
        ' ''' <param name="referenciaVenta">Objeto ReferenciaVenta</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function EliminacionBrainReferenciaVenta(ByVal referenciaVenta As ELL.ReferenciaVenta) As Boolean
        '    Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        '    Dim cn As New OleDb.OleDbConnection(conexion)
        '    Dim query As String = String.Empty
        '    Dim resultado As Boolean = False

        '    Try
        '        cn.Open()
        '        query = "DELETE FROM CUBOS.SOLICIPZA WHERE SPWKNR='000' AND SPTENR='" & referenciaVenta.BatzPartNumber & "'"
        '        Dim cmd As New OleDb.OleDbCommand(query, cn)
        '        cmd.CommandTimeout = 30
        '        cmd.ExecuteNonQuery()


        '        resultado = True
        '    Catch
        '        resultado = False
        '    Finally
        '        If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
        '            cn.Close()
        '            cn.Dispose()
        '        End If
        '    End Try
        '    Return resultado
        'End Function

        ''' <summary>
        ''' Eliminar una referencia de venta en Brain
        ''' </summary>
        ''' <param name="referenciaVenta">Objeto ReferenciaVenta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminacionBrainReferenciaVenta(ByVal referenciaVenta As ELL.ReferenciaVenta) As Boolean
            Dim query As String = String.Empty
            Dim trans As OleDb.OleDbTransaction = Nothing
            Dim cn As OleDb.OleDbConnection = Nothing
            Dim resultado As Boolean = False

            Try
                Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
                cn = New OleDb.OleDbConnection(conexion)

                cn.Open()

                trans = cn.BeginTransaction()

                query = "DELETE FROM CUBOS.SOLICIPZA WHERE SPWKNR='000' AND SPTENR='" & referenciaVenta.BatzPartNumber & "' AND SPZINR='" & referenciaVenta.CustomerPartNumber & "'"
                Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
                cmd.CommandTimeout = 30
                cmd.ExecuteNonQuery()

                'query = "UPDATE CUBOS.SOLICIPZA SET SPCOMP='N' WHERE SPSOLI='" & referenciaVenta.IdSolicitud & "'"
                'cmd = New OleDb.OleDbCommand(query, cn, trans)
                'cmd.CommandTimeout = 30
                'cmd.ExecuteNonQuery()

                resultado = True

                trans.Commit()
            Catch
                resultado = False
                trans.Rollback()
            Finally
                If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
                    cn.Close()
                    cn.Dispose()
                End If
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Elimina todas las referencias de una solicitud que hayan podido ser generadas en Brain (la solicitud de las referencias aún no ha sido tramitada)
        ''' </summary>
        ''' <param name="referencias">Listado de referencias a eliminar en Brain</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarReferenciasVentaSolicitudBrain(ByVal referencias As List(Of ELL.ReferenciaVenta)) As Boolean
            Dim query As String = String.Empty
            Dim trans As OleDb.OleDbTransaction = Nothing
            Dim cn As OleDb.OleDbConnection = Nothing
            Dim resultado As Boolean = False

            Try
                Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
                cn = New OleDb.OleDbConnection(conexion)

                cn.Open()

                trans = cn.BeginTransaction()

                For Each referencia In referencias
                    If Not (String.IsNullOrEmpty(referencia.BatzPartNumber)) Then
                        query = "DELETE FROM CUBOS.SOLICIPZA WHERE SPWKNR='000' AND SPTENR='" & referencia.BatzPartNumber & "'"
                        Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
                        cmd.CommandTimeout = 30
                        cmd.ExecuteNonQuery()
                    End If
                Next

                trans.Commit()

                resultado = True
            Catch ex As Exception
                trans.Rollback()
                resultado = False
            Finally
                If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then
                    cn.Close()
                    cn.Dispose()
                End If
            End Try
            Return resultado
        End Function

#End Region

#End Region

#Region "SELECT, INSERT, UPDATE, DELETE REFERENCIAS EN MAESTRO DE PIEZAS X33SD.TEIL"

        ''' <summary>
        ''' Cargar los datos de una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDatosAltaReferenciaPiezaMaestroBrain(ByVal referencia As String) As ELL.MaestroPiezasBrainResumen
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT * FROM X500PRDSD.TEIL WHERE LOWER(TEZINR)='" & referencia.ToLower & "' AND TEFIRM='1'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim datosBrain As New ELL.MaestroPiezasBrainResumen

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                If oReader.Read Then
                    datosBrain.Planta = oReader.Item("TEWKNR")
                    datosBrain.RefPieza = oReader.Item("TETENR")
                    datosBrain.RefClientePlanoBatz = oReader.Item("TEZINR")
                    datosBrain.NivelIngenieria = oReader.Item("TEASBZ")
                    datosBrain.PlanoWeb = oReader.Item("TERHF4")
                End If
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return datosBrain
        End Function

        ''' <summary>
        ''' Cargar los datos de una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDatosReferenciaClienteBatzMaestroBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String)) As ELL.MaestroPiezasBrainResumen
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = String.Empty

            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim datosBrain As New ELL.MaestroPiezasBrainResumen

            Try
                If (plantasSeleccionadas.Count > 0) Then
                    query = "SELECT * FROM X500PRDSD.TEIL WHERE (LOWER(TEZINR)='" & referencia.ToLower & "' OR LOWER(TETENR)='" & referencia.ToLower & "') "
                    query &= "AND TEFIRM IN ("
                    For Each planta In plantasSeleccionadas
                        query &= "'" & planta & "',"
                    Next
                    query = query.Substring(0, query.Length - 1)
                    query &= ")"

                    Dim cm As New OleDb.OleDbCommand(query, cn)
                    cm.CommandTimeout = 30
                    cn.Open()
                    oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                    If oReader.Read Then
                        datosBrain.Planta = oReader.Item("TEWKNR")
                        datosBrain.RefPieza = oReader.Item("TETENR")
                        datosBrain.RefClientePlanoBatz = oReader.Item("TEZINR")
                        datosBrain.NivelIngenieria = oReader.Item("TEASBZ")
                        datosBrain.PlanoWeb = oReader.Item("TERHF4")
                        datosBrain.IdCustomerProject = oReader.Item("TERHF5")
                    End If
                End If
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return datosBrain
        End Function

        ''' <summary>
        ''' Cargar los datos de un drawing number guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDrawingMaestroBrain(ByVal referencia As String) As ELL.MaestroPiezasBrainResumen
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT * FROM X500PRDSD.TEIL WHERE LOWER(TEZINR)='" & referencia.ToLower & "' OR LOWER(TETENR)='" & referencia.ToLower & "' AND TEFIRM='1'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim datosBrain As New ELL.MaestroPiezasBrainResumen

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                If oReader.Read Then
                    datosBrain.Planta = oReader.Item("TEWKNR")
                    datosBrain.RefPieza = oReader.Item("TETENR")
                    datosBrain.RefClientePlanoBatz = oReader.Item("TEZINR")
                    datosBrain.NivelIngenieria = oReader.Item("TEASBZ")
                    datosBrain.PlanoWeb = oReader.Item("TERHF4")
                End If
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return datosBrain
        End Function


        ''' <summary>
        ''' Cargar los datos de una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferenciaPiezaMaestroBrain(ByVal referencia As String) As ELL.DatosBrain
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT * FROM X500PRDSD.TEIL WHERE LOWER(TETENR)='" & referencia.ToLower & "'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim datosBrain As New ELL.DatosBrain

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                If oReader.Read Then
                    datosBrain.Planta = oReader.Item("TEWKNR")
                    datosBrain.RefPieza = oReader.Item("TETENR")
                    datosBrain.Estado = oReader.Item("TESTAP")
                    datosBrain.Descripcion1 = oReader.Item("TEBEZ1")
                    datosBrain.Descripcion2 = oReader.Item("TEBEZ2")
                    datosBrain.MatchCode = oReader.Item("TEMATC")
                    datosBrain.NumDin = oReader.Item("TEDINR")
                    datosBrain.TipoPieza = oReader.Item("TETART")
                    datosBrain.PasarDespieceWeb = oReader.Item("TEKDF4")
                    datosBrain.PseudoSubconjunto = oReader.Item("TEAUCD")
                    datosBrain.GrupoMaterial = oReader.Item("TEMAGR")
                    datosBrain.GrupoProducto = oReader.Item("TEPRGR")
                    datosBrain.RefClientePlanoBatz = oReader.Item("TEZINR")
                    datosBrain.NivelIngenieria = oReader.Item("TEASBZ")
                    datosBrain.PlanoWeb = oReader.Item("TERHF4")
                    datosBrain.PesoNeto = oReader.Item("TEGWNE")
                    datosBrain.PiezaCompraDirigida = oReader.Item("TEIRVT")
                    datosBrain.Proyecto = oReader.Item("TERHF5")
                    datosBrain.TipoProducto = oReader.Item("TEKLNR")
                    datosBrain.Observaciones = oReader.Item("TERHF6")
                End If
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return datosBrain
        End Function

        ''' <summary>
        ''' Verifica si existe una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteReferenciaBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String), Optional ByVal verificar As Boolean = False) As Boolean
            Dim numero As Integer = 0

            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim query As String = "SELECT COUNT(*) as NUMERO FROM X500PRDSD.TEIL WHERE TEWKNR='000' AND LOWER(TETENR)='" & referencia.ToLower & "' "
            If (verificar) Then
                If (plantasSeleccionadas.Count > 0) Then
                    query &= "AND TEFIRM NOT IN("
                    For Each planta In plantasSeleccionadas
                        query &= "'" & planta & "',"
                    Next
                    query = query.Substring(0, query.Length - 1)
                    query &= ")"
                End If
            Else
                If (plantasSeleccionadas.Count > 0) Then
                    query &= "AND TEFIRM IN("
                    For Each planta In plantasSeleccionadas
                        query &= "'" & planta & "',"
                    Next
                    query = query.Substring(0, query.Length - 1)
                    query &= ")"
                End If
            End If
            
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim oReader As OleDb.OleDbDataReader = Nothing

            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                If oReader.Read Then
                    numero = oReader.Item("NUMERO")
                End If
            Catch
            Finally
                If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            End Try

            Return If(numero = 0, False, True)
            Return True
        End Function

        ''' <summary>
        ''' Verifica si existe una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteReferenciaClienteBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String), Optional ByVal verificar As Boolean = False) As Boolean
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim numero As Integer = 0
            Dim query As String = String.Empty
            Dim cn As New OleDb.OleDbConnection(conexion)


            If (plantasSeleccionadas.Count = 0) Then
                Return False
            Else
                query = "SELECT COUNT(*) as NUMERO FROM X500PRDSD.TEIL WHERE LOWER(TEZINR)='" & referencia.ToLower & "' "
                If (verificar) Then
                    query &= "AND TEFIRM NOT IN("
                Else
                    query &= "AND TEFIRM IN("

                End If
                For Each planta In plantasSeleccionadas
                    query &= "'" & planta & "',"
                Next
                query = query.Substring(0, query.Length - 1)
                query &= ")"

                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                Dim oReader As OleDb.OleDbDataReader = Nothing

                Try
                    cn.Open()
                    oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                    If oReader.Read Then
                        numero = oReader.Item("NUMERO")
                    End If
                Catch
                Finally
                    If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
                End Try

                Return If(numero = 0, False, True)
            End If
        End Function

        ''' <summary>
        ''' Verifica si existe una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteDrawingBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String)) As Boolean
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            Dim cn As New OleDb.OleDbConnection(conexion)
            Dim numero As Integer = 0
            Dim query As String = String.Empty

            If (plantasSeleccionadas.Count = 0) Then
                Return False
            Else
                query = "SELECT COUNT(*) as NUMERO FROM X500PRDSD.TEIL WHERE (LOWER(TEZINR)='" & referencia.ToLower & "' OR LOWER(TETENR)='" & referencia.ToLower & "') "

                query &= "AND TEFIRM IN("
                For Each planta In plantasSeleccionadas
                    query &= "'" & planta & "',"
                Next
                query = query.Substring(0, query.Length - 1)
                query &= ")"

                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                Dim oReader As OleDb.OleDbDataReader = Nothing

                Try
                    cn.Open()
                    oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                    If oReader.Read Then
                        numero = oReader.Item("NUMERO")
                    End If
                Catch
                Finally
                    If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
                End Try

                Return If(numero = 0, False, True)
            End If
        End Function

#End Region

    End Class

    ''query = "INSERT INTO CUBOS.SOLICIPZA(SPFIRM, SPWKNR, SPTENR, SPSTAP, SPBEZ1, SPBEZ2, SPMATC, SPDINR, SPTART, SPKDF4, SPAUCD, SPMEIN, SPMEPR, SPPRKA, SPMAGR, SPPRGR, " & _
    '"SPZINR, SPASBZ, SPRHF4, SPWKSF, SPWKFE, SPDISP, SPLANR, SPGWNE, SPWEPR, SPIRVT, SPRHF5, SPKLAS, SPKLNR, SPRHF6, SPESNR, SPKDF1, SPBLKS " & _
    '") VALUES(@SPFIRM, @SPWKNR, @SPTENR, @SPSTAP, @SPBEZ1, @SPBEZ2, @SPMATC, @SPDINR, @SPTART, @SPKDF4, @SPAUCD, @SPMEIN, @SPMEPR, @SPPRKA, @SPMAGR, @SPPRGR, " & _
    '"@SPZINR, @SPASBZ, @SPRHF4, @SPWKSF, @SPWKFE, @SPDISP, @SPLANR, @SPGWNE, @SPWEPR, @SPIRVT, @SPRHF5, @SPKLAS, @SPKLNR, @SPRHF6, @SPESNR, @SPKDF1, @SPBLKS)"

    ''Make a Command for this connection
    '' and this transaction.
    'Dim cmd As New OleDb.OleDbCommand(query, cn, trans)
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPFIRM", planta.Empresa))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPWKNR", datosBrain.Planta))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPTENR", datosBrain.RefPieza))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPSTAP", datosBrain.Estado))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPBEZ1", datosBrain.Descripcion1))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPBEZ2", datosBrain.Descripcion2))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPMATC", datosBrain.MatchCode))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPDINR", datosBrain.NumDin))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPTART", planta.TipoPieza))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPKDF4", datosBrain.PasarDespieceWeb))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPAUCD", datosBrain.PseudoSubconjunto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPMEIN", planta.UnidadMedidaCantidad))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPMEPR", planta.UnidadMedidaPrecio))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPPRKA", planta.CategoriaProducto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPMAGR", datosBrain.GrupoMaterial))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPPRGR", datosBrain.GrupoProducto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPZINR", datosBrain.RefClientePlanoBatz))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPASBZ", datosBrain.NivelIngenieria))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPRHF4", datosBrain.PlanoWeb))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPWKSF", datosBrain.Material))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPWKFE", datosBrain.Dimensiones))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPDISP", planta.Disponente))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPLANR", planta.NumAlmacen))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPGWNE", datosBrain.PesoNeto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPWEPR", planta.ControlCalidad))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPIRVT", datosBrain.PiezaCompraDirigida))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPRHF5", datosBrain.Proyecto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPKLAS", planta.Subproyecto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPKLNR", datosBrain.TipoProducto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPRHF6", datosBrain.Observaciones))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPESNR", datosBrain.ArticuloRepuesto))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPKDF1", datosBrain.NumPiezasGolpe))
    'cmd.Parameters.Add(New OleDb.OleDbParameter("@SPBLKS", datosBrain.MedioFabricacion))

End Namespace