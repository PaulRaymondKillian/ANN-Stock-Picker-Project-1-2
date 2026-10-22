Public Class StockData
    Public StockID As String
    Public StockName As String
    Public StockUnits As New Hashtable

    Public Sub New(ByVal pStockID As String)
        StockID = pStockID
    End Sub

    Public Sub Add(ByVal pDateTime As String, ByVal pOpen As String, ByVal pHigh As String, ByVal pLow As String, ByVal pClose As String, ByVal pVolume As String, ByVal pAdjClose As String)
        Dim NewStockUnit As New StockUnit(StockID, pDateTime, pOpen, pHigh, pLow, pClose, pVolume, pAdjClose)
        StockUnits.Add(NewStockUnit.DateTime, NewStockUnit)
    End Sub

    Public Function DateExists(ByVal DateTime As String) As Boolean
        Dim exists As Boolean = False

        If Not StockUnits(DateTime) Is Nothing Then
            exists = True
        End If

        Return exists

    End Function


End Class
