Imports System.Data
Namespace BLL
    Public Class DataReaderAdapterBatz
        Inherits Common.DataAdapter
        Function FillFromReader(ByVal DataTable As DataTable, ByVal DataReader As IDataReader) As DataTable
            Me.Fill(DataTable, DataReader)
            DataReader.Close()
            Return DataTable
        End Function
    End Class
End Namespace

