Namespace DAL

    Public Class BrainDAL

        ''' <summary>
        ''' Consulta los datos de una OF de inversion(Y)
        ''' Si mete un punto, solo nos interesa la parte de la izquierda del punto
        ''' 310118: La tabla T_44INV ya no se actualiza ni tampooco se usa la T_44POR
        ''' </summary>
        ''' <param name="stringConexion">String de conexion de la bbdd</param>
        ''' <param name="numOF">Numero de la of a validar</param>
        ''' <param name="idEmpresaBrain">Id de la empresa en Brain</param>
        ''' <returns></returns>        
        Public Function consultarOFInversion(ByVal stringConexion As String, ByVal numOF As String, ByVal idEmpresaBrain As String) As String()
            Dim lParams As New List(Of OleDb.OleDbParameter)
            Dim query As String = "SELECT VALOR,DENOM,IFNULL(LANTEGI,'OBS') FROM CUBOS.T_44N WHERE LOWER(EMPRESA)=? AND VALOR=?"
            'Dim query As String = "SELECT COD,DESC,IFNULL(STATUS,'OBS') FROM CUBOS.PROYECTOS_OFERTAS WHERE LOWER(EMPRESA)=? AND COD=?"
            numOF = numOF.Split(".")(0).Replace(".", "")
            lParams.Add(New OleDb.OleDbParameter("EMPRESA", idEmpresaBrain.ToLower))
            lParams.Add(New OleDb.OleDbParameter("VALOR", numOF))
            'If (numOF.Length <= 4) Then 'Si el parametro INV ocupa mas de 4 caracteres, falla la consulta
            '    query &= " AND (ELTO=? OR (INVE=? AND (PASO='00' OR PASO='  ')))"
            '    lParams.Add(New OleDb.OleDbParameter("ELTO", numOF))
            '    lParams.Add(New OleDb.OleDbParameter("INVE", numOF))
            'Else
            '    query &= " AND ELTO=?"
            '    lParams.Add(New OleDb.OleDbParameter("ELTO", numOF))
            'End If
            Return Memcached.OleDbDirectAccess.Seleccionar(query, stringConexion, lParams.ToArray).FirstOrDefault
        End Function

        '''' <summary>
        '''' Consulta los datos de una OF de portador(D,RP)
        '''' Se buscara con punto y sin punto para facilitar la busqueda
        '''' </summary>
        '''' <param name="stringConexion">String de conexion de la bbdd</param>
        '''' <param name="numOF">Numero de la of a validar</param>
        '''' <param name="idEmpresaBrain">Id de la empresa en Brain</param>
        '''' <returns></returns>        
        'Public Function consultarOFPortador(ByVal stringConexion As String, ByVal numOF As String, ByVal idEmpresaBrain As String) As String()
        '    Dim query As String = "SELECT ELTO,DENOM,LANTEGI FROM CUBOS.T_44POR WHERE LOWER(EMPRESA)=? AND (ELTO=? OR ELTO=?)"
        '    Dim p1 As New OleDb.OleDbParameter("EMPRESA", idEmpresaBrain.ToLower)
        '    Dim p2 As New OleDb.OleDbParameter("ELTO1", numOF)
        '    Dim p3 As New OleDb.OleDbParameter("ELTO2", numOF.Replace(".", ""))
        '    Return Memcached.OleDbDirectAccess.Seleccionar(query, stringConexion, p1, p2, p3).FirstOrDefault
        'End Function

    End Class

End Namespace