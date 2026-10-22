'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' StockDataOrganizer Object Classes
' Written by Paul Killian
' Description: Organize collected stock data and output to files
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System.IO

Public Class StockDataOrganizer
    Public Stocks() As String
    Public StockLists As New Hashtable
    Public MissingStockDataDays As String = ""
    Public HeaderFileName As String
    Public outputOpen As Boolean = False
    Public outputHigh As Boolean = False
    Public outputLow As Boolean = False
    Public outputClose As Boolean = False
    Public outputVolume As Boolean = False
    Public outputAdjClose As Boolean = False

    Public Sub New()

    End Sub

    Public Sub WriteFile(ByVal FilePath As String, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
        UpdateStockList()

        'Deconstruct FileName and create HeaderFileName
        Dim split() As String
        split = FilePath.Split("."c)
        HeaderFileName = split(0) & ".Header"

        WriteStocksToHeaderFile(HeaderFileName)
        Dim sw As New StreamWriter(FilePath)
        Dim ParmCount As Integer = 0
        If outputOpen Then
            ParmCount += 1
        End If
        If outputHigh Then
            ParmCount += 1
        End If
        If outputLow Then
            ParmCount += 1
        End If
        If outputClose Then
            ParmCount += 1
        End If
        If outputVolume Then
            ParmCount += 1
        End If
        If outputAdjClose Then
            ParmCount += 1
        End If
        sw.WriteLine(ParmCount)
        Dim currentDate As DateTime = StartDate
        Do While currentDate < EndDate.AddDays(-1)
            If IsStockTradingDay(currentDate) Then
                Console.WriteLine("Writing Data For: " & ReturnDateString(currentDate))
                AppendDateToHeaderFile(ReturnDateString(currentDate), HeaderFileName)
                For i As Integer = 0 To Stocks.Count - 1
                    If (outputOpen And (outputHigh Or outputClose Or outputLow Or outputVolume Or outputAdjClose)) Then
                        sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Open & ",")
                    ElseIf (outputOpen) Then
                        If i <> Stocks.Count - 1 Then
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Open & ",")
                        Else
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Open)
                        End If
                    End If

                    If (outputClose And (outputHigh Or outputLow Or outputVolume Or outputAdjClose)) Then
                        sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Close & ",")
                    ElseIf (outputClose) Then
                        If i <> Stocks.Count - 1 Then
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Close & ",")
                        Else
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Close)
                        End If
                    End If

                    If (outputHigh And (outputLow Or outputVolume Or outputAdjClose)) Then
                        sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).High & ",")
                    ElseIf (outputHigh) Then
                        If i <> Stocks.Count - 1 Then
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).High & ",")
                        Else
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).High)
                        End If
                    End If

                    If (outputLow And (outputVolume Or outputAdjClose)) Then
                        sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Low & ",")
                    ElseIf (outputLow) Then
                        If i <> Stocks.Count - 1 Then
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Low & ",")
                        Else
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Low)
                        End If
                    End If

                    'Volume
                    If (outputVolume And outputAdjClose) Then
                        sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Volume & ",")
                    ElseIf (outputVolume) Then
                        If i <> Stocks.Count - 1 Then
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Volume & ",")
                        Else
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).Volume)
                        End If
                    End If

                    If (outputAdjClose) Then
                        If i <> Stocks.Count - 1 Then
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).AdjClose & ",")
                        Else
                            sw.Write(StockLists(Stocks(i)).StockUnits(ReturnDateString(currentDate)).AdjClose)
                        End If
                    End If

                Next
                sw.WriteLine("")
            End If
            currentDate = currentDate.AddDays(1)
        Loop
        sw.Close()
    End Sub

    Public Sub WriteStocksToHeaderFile(ByVal FilePath As String)
        Try
            Dim sr As New StreamWriter(FilePath)
            For i As Integer = 0 To Stocks.Count - 1
                sr.Write(Stocks(i))
                If i <> Stocks.Count - 1 Then
                    sr.Write(",")
                End If
            Next
            sr.Close()
        Catch ex As Exception
            Throw ex
        Finally
        End Try
    End Sub

    Public Sub AppendDateToHeaderFile(ByVal DateString As String, ByVal FilePath As String)
        Try
            File.AppendAllText(FilePath, vbCrLf & DateString)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub UpdateStockList()
        ReDim Stocks(StockLists.Count - 1)
        Dim index As Integer = 0
        For Each stock In StockLists
            Stocks(index) = stock.key.ToString
            index += 1
        Next
    End Sub

    Public Sub Add(ByVal pStockID As String, ByVal pDateTime As String, ByVal pOpen As String, ByVal pHigh As String, ByVal pLow As String, ByVal pClose As String, ByVal pVolume As String, ByVal pAdjClose As String)
        If StockLists(pStockID) Is Nothing Then
            Dim NewStockData As StockData = New StockData(pStockID)
            StockLists.Add(pStockID, NewStockData)
        End If

        StockLists.Item(pStockID).Add(pDateTime, pOpen, pHigh, pLow, pClose, pVolume, pAdjClose)

    End Sub

    'Checks all the stocks from start date to end date to check for issues - returns true if data is good, returns false if problem with data
    Public Function VerifyStockData(ByVal StartDate As DateTime, ByVal EndDate As DateTime) As Boolean
        Dim currentDate As DateTime = StartDate
        Dim ValidData As Boolean = True

        Do While currentDate <= EndDate
            Console.WriteLine("Verifying Data On: " & ReturnDateString(currentDate))
            If Not IsStockTradingDay(currentDate) Then
                'It is a weekend or holiday (Not a Trading Day), so no stocks should have data
                ''Check no stocks have data on this date, if not, PROBLEM!
                If ReturnCountStocksNotOnDate(ReturnDateString(currentDate)) <> StockLists.Count Then
                    Console.WriteLine("   NOT TRADING DAY: " & ReturnCountStocksOnDate(ReturnDateString(currentDate)) & " has data, " & ReturnCountStocksNotOnDate(ReturnDateString(currentDate)) & " does not have data.")
                    Console.WriteLine(" WEEKEND/HOLIDAY - " & ReturnDateString(currentDate) & " - Stocks Existing: " & ReturnStocksOnDate(ReturnDateString(currentDate)))
                    ValidData = False
                Else
                    Console.WriteLine("   NOT TRADING DAY: " & ReturnCountStocksOnDate(ReturnDateString(currentDate)) & " has data, " & ReturnCountStocksNotOnDate(ReturnDateString(currentDate)) & " does not have data.")
                End If
            Else
                'It is a weekday, so all stocks should have data
                ''Check that all stocks have data on this date, if not, PROBLEM!
                If ReturnCountStocksOnDate(ReturnDateString(currentDate)) <> StockLists.Count Then
                    ValidData = False
                    MissingStockDataDays += ReturnDateString(currentDate) & ", "
                    Console.WriteLine("   TRADING DAY: " & ReturnCountStocksOnDate(ReturnDateString(currentDate)) & " has data, " & ReturnCountStocksNotOnDate(ReturnDateString(currentDate)) & " does not have data.")
                    Console.WriteLine(" WEEKDAY - " & ReturnDateString(currentDate) & " - Stocks NOT Existing: " & ReturnStocksNotOnDate(ReturnDateString(currentDate)))
                Else
                    Console.WriteLine("   TRADING DAY: " & ReturnCountStocksOnDate(ReturnDateString(currentDate)) & " has data, " & ReturnCountStocksNotOnDate(ReturnDateString(currentDate)) & " does not have data.")
                End If
            End If
            currentDate = currentDate.AddDays(1)
        Loop

        Console.WriteLine()
        Console.WriteLine("Days Missing Stock Data: " & MissingStockDataDays)
        Console.WriteLine()

        Return ValidData
    End Function

    Public Function ReturnDateString(ByVal pDateTime As DateTime) As String
        Return pDateTime.Month & "/" & pDateTime.Day & "/" & pDateTime.Year
    End Function

    Public Function IsStockTradingDay(ByVal pDateTime As DateTime) As Boolean
        Dim DateString As String = ReturnDateString(pDateTime)

        If pDateTime.DayOfWeek = 0 Or pDateTime.DayOfWeek = 6 Or IsStockMarketHoliday(pDateTime) = True Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function IsStockMarketHoliday(ByVal pDateTime As DateTime) As Boolean
        Dim isHoliday As Boolean = False
        Dim Holidays As New Hashtable
        Dim line, split() As String
        Dim sr As New StreamReader("StockMarketHolidays.txt")
        Do While Not sr.EndOfStream
            line = sr.ReadLine
            split = line.Split("/"c)
            If split.Length <> 3 Then
                Throw New Exception("Error - StockMarketHolidays.txt contains invalid data, must be formatted: 1/1/2009")
            End If
            Holidays.Add(line, line)
        Loop
        sr.Close()

        For Each item In Holidays
            If ReturnDateString(pDateTime) = item.Value.ToString Then
                isHoliday = True
            End If
        Next
        Return isHoliday
    End Function

    Public Function ReturnCountStocksOnDate(ByVal pDateTime As String) As Integer
        Dim Stocks As Integer = 0

        For Each stock In StockLists
            If stock.Value.DateExists(pDateTime) = True Then
                Stocks += 1
            End If
        Next

        Return Stocks
    End Function

    Public Function ReturnCountStocksNotOnDate(ByVal pDateTime As String) As Integer
        Dim Stocks As Integer = 0

        For Each stock In StockLists
            If stock.Value.DateExists(pDateTime) = False Then
                Stocks += 1
            End If
        Next

        Return Stocks
    End Function

    Public Function ReturnStocksOnDate(ByVal pDateTime As String) As String
        Dim Stocks = ""

        For Each stock In StockLists
            If stock.Value.DateExists(pDateTime) = True Then
                Stocks += stock.Value.StockID & ", "
            End If
        Next

        Return Stocks
    End Function

    Public Function ReturnStocksNotOnDate(ByVal pDateTime As String) As String
        Dim Stocks = ""

        For Each stock In StockLists
            If stock.Value.DateExists(pDateTime) = False Then
                Stocks += stock.Value.StockID & ", "
            End If
        Next

        Return Stocks
    End Function

    Public Function IsWeekDay(ByVal pDateTime As String) As Boolean
        Dim split() As String = pDateTime.Split("/"c)
        Dim DateTime As New Date(split(2), split(0), split(1))
        Dim DayOfWeek As Integer = DateTime.DayOfWeek
        If DayOfWeek = 0 Or DayOfWeek = 6 Then
            Return False
        Else
            Return True
        End If
    End Function

End Class
