Imports System.IO
Imports System.Text.StringBuilder

Module Module1
    Dim inputDir As String
    Dim inputFile As String
    Dim countParms As Integer
    Dim percentThreshold As Integer
    Dim sr As StreamReader
    Dim sw_Output As StreamWriter
    Dim sw_PercentChange As StreamWriter
    Dim sw_Results As StreamWriter
    Dim sw_Validation As StreamWriter
    Dim countInputRows As Integer
    Dim line As String
    Dim line2 As String
    Dim inputRow() As String
    Dim inputRow2() As String
    Dim countStocks As Integer
    Dim countStocks2 As Integer
    Dim StockListStart() As Stock
    Dim StockListEnd() As Stock
    Dim PercentChange() As Double
    Dim Results() As Integer

    Sub Main()
        Console.WriteLine("Stock Picker - Data Scrubber v1.0")
        inputDir = "D:\StockPicker\SP_InputScrubber\"   'Hardcoded for now
        inputFile = "10-27-08 to 10-30-09_day.txt"  'Hardcoded for now
        countParms = 6  'Hardcoded for now
        percentThreshold = 10
        countInputRows = CalcCountInputRows(inputDir & inputFile)

        sr = File.OpenText(inputDir & inputFile)
        sw_Output = File.CreateText(inputDir & "Output_" & inputFile)
        sw_PercentChange = File.CreateText(inputDir & "PercentChange_" & inputFile)
        sw_Results = File.CreateText(inputDir & "Results_" & inputFile)
        sw_Validation = File.CreateText(inputDir & "Validation_" & inputFile)


        line = sr.ReadLine
        inputRow = line.Split(","c)
        countStocks = (inputRow.Length - 1) / countParms

            ReDim StockListStart(countStocks - 1)
            ReDim PercentChange(countStocks - 1)
            ReDim Results(countStocks - 1)

            Dim index As Integer = 0
            'Create first list of stocks
            For i As Integer = 0 To countStocks - 1
                StockListStart(i) = New Stock(i)
                StockListStart(i).open = Convert.ToDouble(inputRow(index))
                StockListStart(i).high = Convert.ToDouble(inputRow(index + 1))
                StockListStart(i).low = Convert.ToDouble(inputRow(index + 2))
                StockListStart(i).close = Convert.ToDouble(inputRow(index + 3))
                StockListStart(i).volume = Convert.ToInt32(inputRow(index + 4))
                StockListStart(i).adjClose = Convert.ToDouble(inputRow(index + 5))
                index += 6
            Next

        For FileIndexRow As Integer = 0 To countInputRows - 2

            line2 = sr.ReadLine
            inputRow2 = line2.Split(","c)
            countStocks2 = (inputRow.Length - 1) / countParms
            ReDim StockListEnd(countStocks - 1)

            'Check if both stock counts are the same; if they differ we have a problem
            If countStocks <> countStocks2 Then
                Throw New Exception("Invalid input parameters: All rows of data must have the same number of parms")
            End If

            index = 0
            'Create second list of stocks
            For i As Integer = 0 To countStocks - 1
                StockListEnd(i) = New Stock(i)
                StockListEnd(i).open = Convert.ToDouble(inputRow(index))
                StockListEnd(i).high = Convert.ToDouble(inputRow(index + 1))
                StockListEnd(i).low = Convert.ToDouble(inputRow(index + 2))
                StockListEnd(i).close = Convert.ToDouble(inputRow(index + 3))
                StockListEnd(i).volume = Convert.ToInt32(inputRow(index + 4))
                StockListEnd(i).adjClose = Convert.ToDouble(inputRow(index + 5))
                index += 6
            Next

            'Set values for PercentChange and Results arrays
            For i As Integer = 0 To countStocks - 1
                PercentChange(i) = (StockListStart(i).CompareClose(StockListEnd(i))) / StockListStart(i).close
                If PercentChange(i) * 100 >= percentThreshold Then
                    Results(i) = 1
                Else
                    Results(i) = 0
                End If
            Next

            'Build Output String
            Dim outputString As New Text.StringBuilder(line)
            For i As Integer = 0 To countStocks - 1
                outputString.Append(Results(i))
                If i <> countStocks - 1 Then
                    outputString.Append(",")
                End If
            Next

            'Build PercentChange String
            Dim percentChangeString As New Text.StringBuilder("")
            For i As Integer = 0 To countStocks - 1
                percentChangeString.Append(PercentChange(i))
                If i <> countStocks - 1 Then
                    percentChangeString.Append(",")
                End If
            Next

            'Build Results String
            Dim ResultsString As New Text.StringBuilder("")
            For i As Integer = 0 To countStocks - 1
                ResultsString.Append(Results(i))
                If i <> countStocks - 1 Then
                    ResultsString.Append(",")
                End If
            Next

            'Write Output String to Output File
            sw_Output.WriteLine(outputString.ToString)
            sw_PercentChange.WriteLine(percentChangeString.ToString)
            sw_Results.WriteLine(ResultsString.ToString)

            'Output Validation File Information
            sw_Validation.WriteLine("Start  End     Diff    Percent      Result")
            For i As Integer = 0 To countStocks - 1
                sw_Validation.WriteLine(StockListStart(i).close & vbTab & StockListEnd(i).close & vbTab & Math.Round(StockListStart(i).CompareClose(StockListEnd(i)), 5) & vbTab & Math.Round((100 * PercentChange(i)), 2) & "%" & vbTab & "=" & vbTab & Results(i))
            Next
            sw_Validation.WriteLine("##############################################")
            sw_Validation.WriteLine()

            StockListStart = StockListEnd
            line = line2
            inputRow = inputRow2
            countStocks = countStocks2
            StockListEnd = Nothing

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
        CalcCountInputRows = count
    End Function

End Module
