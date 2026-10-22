Imports System.IO
Imports System.Text.StringBuilder

Module Module1
    Dim inputDir As String
    Dim inputFile As String
    Dim countParms As Integer
    Dim sr As StreamReader
    Dim sw_Output As StreamWriter
    Dim sw_PercentChange As StreamWriter
    Dim sw_Results As StreamWriter
    Dim sw_Validation As StreamWriter
    Dim CountInputRows As Integer
    Dim line As String
    Dim InputStrings() As String
    Dim InputRow() As String
    Dim CountStocks As Integer
    Dim StockList(,) As Stock
    Dim PercentChange(,) As Double
    Dim Results(,) As Integer

    Dim PercentThreshold As Integer
    Dim ExponentPower As Integer
    Dim OutputType As Output_Type

    Public Enum Output_Type As Integer
        Binary_Threshold = 0
        Linear = 1
        LinearTimes10 = 2
        Exponential = 3
        AdjustedExponential = 4
    End Enum


    Sub Main()
        Console.WriteLine("Stock Picker - Data Scrubber - Extended Storage v1.0")
        inputDir = "D:\StockPicker\SP_InputScrubber\"   'Hardcoded for now
        inputFile = "10-27-08 to 10-30-09_day.txt"  'Hardcoded for now
        Console.WriteLine("Output Types: ")
        Console.WriteLine("Binary Threshold = 0")
        Console.WriteLine("Linear = 1")
        Console.WriteLine("Linear Times 10 = 2")
        Console.WriteLine("Exponential = 3")
        Console.WriteLine("Adjusted Exponential = 4")
        Console.Write("Please Input Scrubber Output Type: ")
        OutputType = Console.ReadLine
        If OutputType = Output_Type.Binary_Threshold Then
            Console.Write("Stock Increase Percent Threshold: ")
            PercentThreshold = Convert.ToInt32(Console.ReadLine)
        ElseIf OutputType = Output_Type.AdjustedExponential Then
            Console.Write("Output Adjusted Exponent value (%^X): ")
            ExponentPower = Convert.ToInt32(Console.ReadLine)
        End If

        sr = File.OpenText(inputDir & inputFile)
        sw_Output = File.CreateText(inputDir & "Output_" & inputFile)
        sw_PercentChange = File.CreateText(inputDir & "PercentChange_" & inputFile)
        sw_Results = File.CreateText(inputDir & "Results_" & inputFile)
        sw_Validation = File.CreateText(inputDir & "Validation_" & inputFile)
        countParms = Convert.ToInt32(sr.ReadLine)


        line = sr.ReadLine
        inputRow = line.Split(","c)
        CountStocks = (InputRow.Length - 1) / countParms
        CountInputRows = CalcCountInputRows(inputDir & inputFile)

        ReDim StockList(CountInputRows - 1, CountStocks - 1)
        ReDim PercentChange(CountInputRows - 2, CountStocks - 1)    '1 less row in this array
        ReDim Results(CountInputRows - 2, CountStocks - 1)          '1 less row in this array
        ReDim InputStrings(CountInputRows - 1)

        For inputRowIndex As Integer = 0 To CountInputRows - 1
            InputStrings(inputRowIndex) = line
            Dim index As Integer = 0
            'Create array of stock data
            For i As Integer = 0 To CountStocks - 1
                StockList(inputRowIndex, i) = New Stock(i)
                StockList(inputRowIndex, i).open = Convert.ToDouble(InputRow(index))
                StockList(inputRowIndex, i).high = Convert.ToDouble(InputRow(index + 1))
                StockList(inputRowIndex, i).low = Convert.ToDouble(InputRow(index + 2))
                StockList(inputRowIndex, i).close = Convert.ToDouble(InputRow(index + 3))
                StockList(inputRowIndex, i).volume = Convert.ToInt32(InputRow(index + 4))
                StockList(inputRowIndex, i).adjClose = Convert.ToDouble(InputRow(index + 5))
                index += 6
            Next

            If inputRowIndex <> CountInputRows - 1 Then
                line = sr.ReadLine
                InputRow = line.Split(","c)
                If ((InputRow.Length - 1) / countParms) <> CountStocks Then
                    Throw New Exception("Number of Parms for Stock Line Invalid - must remain consistant # of parms throughout entire data file")
                End If
            End If
        Next

        'Build Results and Percent Change Arrays
        For inputRowIndex As Integer = 0 To CountInputRows - 2
            For stockIndex As Integer = 0 To CountStocks - 1
                'Put in the Percent Change Array the Change in the Stock for the NEXT DAY! - We are trying to predict how it does the next day starting at the opening bell of that day, not closing bell of previous day
                PercentChange(inputRowIndex, stockIndex) = StockList(inputRowIndex + 1, stockIndex).PercentChangeThatDay
                Results(inputRowIndex, stockIndex) = CalculateOutput(OutputType, PercentChange(inputRowIndex, stockIndex))
            Next
        Next

        'Build Output Strings and Write to File
        For inputRowIndex As Integer = 0 To CountInputRows - 2
            Dim outputString As New Text.StringBuilder(InputStrings(inputRowIndex))
            For stockIndex As Integer = 0 To CountStocks - 1
                outputString.Append(Results(inputRowIndex, stockIndex))
                If stockIndex <> CountStocks - 1 Then
                    outputString.Append(",")
                End If
            Next
            sw_Output.WriteLine(outputString.ToString)
            outputString = Nothing
        Next


        ''Build PercentChange String and Write to File
        For inputRowIndex As Integer = 0 To CountInputRows - 2
            sw_PercentChange.WriteLine("PercentChange Day " & inputRowIndex & "-" & inputRowIndex + 1)
            Dim percentChangeString As New Text.StringBuilder("")
            For stockIndex As Integer = 0 To CountStocks - 1
                percentChangeString.Append(Math.Round(PercentChange(inputRowIndex, stockIndex), 4))
                If stockIndex <> CountStocks - 1 Then
                    percentChangeString.Append(",")
                End If
            Next
            sw_PercentChange.WriteLine(percentChangeString.ToString)
            percentChangeString = Nothing
        Next


        ''Build Results String
        For inputRowIndex As Integer = 0 To CountInputRows - 2
            Dim ResultsString As New Text.StringBuilder(CountBuys(inputRowIndex) & " Buys = ")
            For stockIndex As Integer = 0 To CountStocks - 1
                ResultsString.Append(Results(inputRowIndex, stockIndex))
                If stockIndex <> CountStocks - 1 Then
                    ResultsString.Append(",")
                End If
            Next
            sw_Results.WriteLine(ResultsString.ToString)
            ResultsString = Nothing
        Next

        For inputRowIndex As Integer = 0 To CountInputRows - 2
            sw_Validation.WriteLine("Day #" & inputRowIndex + 1)
            sw_Validation.WriteLine("Open  Close     Diff    Percent      Result")
            For stockIndex As Integer = 0 To CountStocks - 1
                With StockList(inputRowIndex + 1, stockIndex)
                    sw_Validation.WriteLine(.open & vbTab & .close & vbTab & Math.Round(.close - .open, 5) & vbTab & Math.Round((100 * PercentChange(inputRowIndex, stockIndex)), 2) & "%" & vbTab & "=" & vbTab & Results(inputRowIndex, stockIndex))
                End With
            Next
            sw_Validation.WriteLine("##############################################")
            sw_Validation.WriteLine()

        Next

        sw_Output.Close()
        sw_PercentChange.Close()
        sw_Results.Close()
        sw_Validation.Close()

        Console.WriteLine("Press Enter to exit...")
        Dim z As String = Console.ReadLine
    End Sub

    Public Function CalcCountInputRows(ByVal FilePath As String)
        Dim count As Integer
        Dim sr As StreamReader
        sr = File.OpenText(FilePath)
        Do Until sr.EndOfStream
            sr.ReadLine()
            count += 1
        Loop
        CalcCountInputRows = count - 1
    End Function

    Public Function CountBuys(ByVal inputRowIndex) As Integer
        Dim count As Integer = 0
        For i As Integer = 0 To CountStocks - 1
            If Results(inputRowIndex, i) = 1 Then
                count += 1
            End If
        Next
        CountBuys = count
    End Function

    Public Function CalculateOutput(ByVal type As Output_Type, ByVal percent As Double) As Double
        Dim output As Double
        percent *= 100

        If type = Output_Type.Binary_Threshold Then
            If percent * 100 >= PercentThreshold Then
                output = 1
            Else
                output = 0
            End If
        ElseIf type = Output_Type.Linear Then
            output = percent
        ElseIf type = Output_Type.LinearTimes10 Then
            output = 100 * percent
        ElseIf type = Output_Type.Exponential Then
            output = Math.Abs(percent) * Math.Pow(percent, 2)
        ElseIf type = Output_Type.AdjustedExponential Then
            output = Math.Abs(percent) * Math.Pow(percent, ExponentPower)
        End If

        Return output
    End Function

End Module
