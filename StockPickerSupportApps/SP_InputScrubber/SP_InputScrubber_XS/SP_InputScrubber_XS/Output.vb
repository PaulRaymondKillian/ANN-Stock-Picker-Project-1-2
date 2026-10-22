Public Class Output
    Public InputStrings() As String
    Public PercentChange() As Double
    Public Result() As Double

    Public Sub New(ByVal countInputDays As Integer, ByVal countStocks As Integer)
        ReDim InputStrings(countInputDays - 1)
        ReDim PercentChange(countStocks - 1)
        ReDim Result(countStocks - 1)
    End Sub
End Class
