Public Class StockUnit
    Public StockID As String
    Public DateTime As String
    Public Open As String
    Public High As String
    Public Low As String
    Public Close As String
    Public Volume As String
    Public AdjClose As String
    Public DayOfWeek As String
    Public Year As String
    Public DayOfYear As String

    Public Sub New(ByVal pStockID As String, ByVal pDateTime As String, ByVal pOpen As String, ByVal pHigh As String, ByVal pLow As String, ByVal pClose As String, ByVal pVolume As String, ByVal pAdjClose As String)
        StockID = pStockID
        DateTime = SanitizeDateTime(pDateTime)
        Open = pOpen
        High = pHigh
        Low = pLow
        Close = pClose
        Volume = pVolume
        AdjClose = pAdjClose
        SetDateData(DateTime)
    End Sub

    Public Function SanitizeDateTime(ByVal pDateTime As String) As String
        Dim split() As String
        split = pDateTime.Split(" "c)
        Return split(0)
    End Function

    Public Sub SetDateData(ByVal pDateTime As String)
        Dim split() As String = pDateTime.Split("/"c)
        Dim DateTime As New Date(split(2), split(0), split(1))
        DayOfWeek = DateTime.DayOfWeek
        DayOfYear = DateTime.DayOfYear
        Year = DateTime.Year
    End Sub

End Class
