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
    Dim CountStocks As Integer
    Dim line As String
    Dim InputStrings() As String
    Dim InputRow() As String
    Dim StockList(,) As Stock
    Dim OutputList() As Output

    Dim PercentThreshold As Integer
    Dim ExponentPower As Integer
    Dim OutputType As Output_Type

    Dim countInputTimeUnits As Integer
    Dim countPredictionTimeUnits As Integer

    Dim StaggerInputRows As Boolean = False

    Public Enum Output_Type As Integer
        Binary_Threshold = 0
        Linear = 1
        LinearTimes10 = 2
        Exponential = 3
        AdjustedExponential = 4
    End Enum


    Sub Main()
        Console.WriteLine("Stock Picker - Data Scrubber - Extended Storage v1.0")
        inputDir = "C:\Source\_ANN\StockPicker\SP_InputScrubber\"   'Hardcoded for now
        inputFile = "10-27-08 to 10-30-09_day.txt"  'Hardcoded for now
        Console.WriteLine("Output Types: ")
        Console.WriteLine("Binary Threshold = 0")
        Console.WriteLine("Linear = 1")
        Console.WriteLine("Linear Times 100 = 2")
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
        ElseIf OutputType < 0 Or OutputType > 4 Then
            Throw New Exception("You caused an error, idiot.")
        End If

        Console.WriteLine()
        Console.WriteLine("Setup Data File - How many days of information and how far prediction?")
        Console.Write("Please Input the Number of Units of Time for Input (Days/etc.): ")
        countInputTimeUnits = Convert.ToInt32(Console.ReadLine)
        Console.Write("How many days in the future to predict?: ")
        countPredictionTimeUnits = Convert.ToInt32(Console.ReadLine)
        Console.WriteLine("This input file will contain input data of " & countInputTimeUnits & " units of time and predict " & countPredictionTimeUnits & " units of time in the future.")

        If countPredictionTimeUnits > 1 Then
            Console.Write("Would you like to stagger the input rows? Choosing 'N' will utilize isolated multi-day handling instead. (Y/N):")
            If Console.ReadLine.ToUpper = "Y" Then
                StaggerInputRows = True
            Else
                StaggerInputRows = False
            End If
        End If

        sr = File.OpenText(inputDir & inputFile)
        sw_Output = File.CreateText(inputDir & "Output_" & inputFile)
        sw_PercentChange = File.CreateText(inputDir & "PercentChange_" & inputFile)
        sw_Results = File.CreateText(inputDir & "Results_" & inputFile)
        sw_Validation = File.CreateText(inputDir & "Validation_" & inputFile)
        sw_Results.WriteLine("Input File contains " & countParms & " parameters per stock per unit of time.")
        sw_Results.WriteLine("Each input line contains " & countInputTimeUnits & " units of time per stock.")
        sw_Results.WriteLine("Each input line contains results for " & countPredictionTimeUnits & " units of time in the future.")

        countParms = Convert.ToInt32(sr.ReadLine)   'Read Parm Value from File

        line = sr.ReadLine
        InputRow = line.Split(","c)
        CountStocks = InputRow.Length / countParms
        CountInputRows = CalcCountInputRows(inputDir & inputFile)

        ReDim StockList(CountInputRows - 1, CountStocks - 1)
        ReDim InputStrings(CountInputRows - 1)

        For inputRowIndex As Integer = 0 To CountInputRows - 1
            InputStrings(inputRowIndex) = line
            Dim index As Integer = 0
            'Create array of stock data
            For i As Integer = 0 To CountStocks - 1
                StockList(inputRowIndex, i) = New Stock(i)
                With StockList(inputRowIndex, i)
                    .open = Convert.ToDouble(InputRow(index))
                    .high = Convert.ToDouble(InputRow(index + 1))
                    .low = Convert.ToDouble(InputRow(index + 2))
                    .close = Convert.ToDouble(InputRow(index + 3))
                    .volume = Convert.ToInt32(InputRow(index + 4))
                    .adjClose = Convert.ToDouble(InputRow(index + 5))
                End With
                index += 6
            Next

            If inputRowIndex <> CountInputRows - 1 Then
                line = sr.ReadLine
                InputRow = line.Split(","c)
                If (InputRow.Length / countParms) <> CountStocks Then
                    Throw New Exception("Number of Parms for Stock Line Invalid - must remain consistant # of parms throughout entire data file")
                End If
            End If
        Next

        'Populate OutputList
        If StaggerInputRows Then
            'This is the normal method where we advance using every time unit in a staggared fashion
            ReDim OutputList(CountInputRows - (countInputTimeUnits + countPredictionTimeUnits))
            For OutputRowIndex As Integer = 0 To OutputList.Length - 1
                OutputList(OutputRowIndex) = New Output(countInputTimeUnits, CountStocks)

                'Set Input Strings for this output row
                For i As Integer = 0 To countInputTimeUnits - 1
                    OutputList(OutputRowIndex).InputStrings(i) = InputStrings(OutputRowIndex + i)
                Next

                For StockIndex As Integer = 0 To CountStocks - 1
                    OutputList(OutputRowIndex).PercentChange(StockIndex) = StockList(OutputRowIndex + countInputTimeUnits, StockIndex).PercentChange(StockList(OutputRowIndex + countInputTimeUnits + countPredictionTimeUnits - 1, StockIndex))
                    OutputList(OutputRowIndex).Result(StockIndex) = CalculateOutput(OutputType, OutputList(OutputRowIndex).PercentChange(StockIndex))
                Next
            Next
        Else
            'This is the method for weeks, and such, so that we wanted M-F&F to always be the inputs/outputs in a static manner
            ReDim OutputList(Convert.ToInt32((CountInputRows - countInputTimeUnits) / countPredictionTimeUnits) - 1)
            For OutputRowIndex As Integer = 0 To OutputList.Length - 1
                OutputList(OutputRowIndex) = New Output(countInputTimeUnits, CountStocks)

                'Set Input Strings for this output row
                For i As Integer = 0 To countInputTimeUnits - 1
                    OutputList(OutputRowIndex).InputStrings(i) = InputStrings((OutputRowIndex * countInputTimeUnits) + i)
                Next

                For StockIndex As Integer = 0 To CountStocks - 1
                    OutputList(OutputRowIndex).PercentChange(StockIndex) = StockList(((OutputRowIndex + 1) * countInputTimeUnits), StockIndex).PercentChange(StockList(((OutputRowIndex + 1) * countInputTimeUnits) + countPredictionTimeUnits - 1, StockIndex))
                    OutputList(OutputRowIndex).Result(StockIndex) = CalculateOutput(OutputType, OutputList(OutputRowIndex).PercentChange(StockIndex))
                Next
            Next
        End If


        'Write the Output File
        For i As Integer = 0 To OutputList.Length - 1
            For j As Integer = 0 To countInputTimeUnits - 1
                sw_Output.Write(OutputList(i).InputStrings(j))
                If j <> countInputTimeUnits - 1 Then
                    sw_Output.Write(",")
                Else
                    sw_Output.Write("=")
                End If
            Next

            For j As Integer = 0 To CountStocks - 1
                sw_Output.Write(OutputList(i).Result(j))
                If j <> CountStocks - 1 Then
                    sw_Output.Write(",")
                Else
                    sw_Output.Write(vbCrLf)
                End If
            Next

        Next

        ''Build Results and Percent Change Arrays
        'For TimeUnitIndex As Integer = 0 To CountInputRows - (countPredictionTimeUnits + 1)
        '    For stockIndex As Integer = 0 To CountStocks - 1
        '        'Put in the Percent Change Array the Change in the Stock for the NEXT DAY! - We are trying to predict how it does the next day starting at the opening bell of that day, not closing bell of previous day
        '        'PercentChange(inputRowIndex, stockIndex) = StockList(inputRowIndex + 1, stockIndex).PercentChangeThatDay
        '        PercentChange(TimeUnitIndex, stockIndex) = StockList(TimeUnitIndex + 1, stockIndex).PercentChange(StockList(inputRowIndex + countPredictionTimeUnits - 1, stockIndex))
        '        Results(inputRowIndex, stockIndex) = CalculateOutput(OutputType, PercentChange(inputRowIndex, stockIndex))
        '    Next
        'Next

        ''Build Output Strings and Write to File

        'For TimeUnitIndex As Integer = 0 To CountInputRows - (countPredictionTimeUnits + 1)
        '    Dim outputString As New Text.StringBuilder("")
        '    For i As Integer = 0 To countInputTimeUnits - 1
        '        outputString.Append(InputStrings(TimeUnitIndex + i))
        '    Next

        '    outputString.Append("=")

        '    For stockIndex As Integer = 0 To CountStocks - 1
        '        outputString.Append(Results(TimeUnitIndex, stockIndex))
        '        If stockIndex <> CountStocks - 1 Then
        '            outputString.Append(",")
        '        End If
        '    Next
        '    sw_Output.WriteLine(outputString.ToString)
        '    outputString = Nothing
        'Next


        ' ''Build PercentChange String and Write to File
        'For inputRowIndex As Integer = 0 To CountInputRows - 2
        '    sw_PercentChange.WriteLine("PercentChange Day " & inputRowIndex & "-" & inputRowIndex + 1)
        '    Dim percentChangeString As New Text.StringBuilder("")
        '    For stockIndex As Integer = 0 To CountStocks - 1
        '        percentChangeString.Append(Math.Round(PercentChange(inputRowIndex, stockIndex), 4))
        '        If stockIndex <> CountStocks - 1 Then
        '            percentChangeString.Append(",")
        '        End If
        '    Next
        '    sw_PercentChange.WriteLine(percentChangeString.ToString)
        '    percentChangeString = Nothing
        'Next


        ' ''Build Results String
        'For inputRowIndex As Integer = 0 To CountInputRows - 2
        '    Dim ResultsString As New Text.StringBuilder(CountBuys(inputRowIndex) & " Buys = ")
        '    For stockIndex As Integer = 0 To CountStocks - 1
        '        ResultsString.Append(Results(inputRowIndex, stockIndex))
        '        If stockIndex <> CountStocks - 1 Then
        '            ResultsString.Append(",")
        '        End If
        '    Next
        '    sw_Results.WriteLine(ResultsString.ToString)
        '    ResultsString = Nothing
        'Next

        'For inputRowIndex As Integer = 0 To CountInputRows - 2
        '    sw_Validation.WriteLine("Day #" & inputRowIndex + 1)
        '    sw_Validation.WriteLine("Open  Close     Diff    Percent      Result")
        '    For stockIndex As Integer = 0 To CountStocks - 1
        '        With StockList(inputRowIndex + 1, stockIndex)
        '            sw_Validation.WriteLine(.open & vbTab & .close & vbTab & Math.Round(.close - .open, 5) & vbTab & Math.Round((100 * PercentChange(inputRowIndex, stockIndex)), 2) & "%" & vbTab & "=" & vbTab & Results(inputRowIndex, stockIndex))
        '        End With
        '    Next
        '    sw_Validation.WriteLine("##############################################")
        '    sw_Validation.WriteLine()

        'Next

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
        Do Until sr.ReadLine() Is Nothing
            count += 1
        Loop
        CalcCountInputRows = count - 1
    End Function

    'Public Function CountBuys(ByVal inputRowIndex) As Integer
    '    Dim count As Integer = 0
    '    For i As Integer = 0 To CountStocks - 1
    '        If Results(inputRowIndex, i) = 1 Then
    '            count += 1
    '        End If
    '    Next
    '    CountBuys = count
    'End Function

    Public Function CalculateOutput(ByVal type As Output_Type, ByVal percent As Double) As Double
        Dim output As Double
        percent *= 100

        If type = Output_Type.Binary_Threshold Then
            If percent >= PercentThreshold Then
                output = 1
            Else
                output = 0
            End If
        ElseIf type = Output_Type.Linear Then
            output = percent
        ElseIf type = Output_Type.LinearTimes10 Then
            output = 100 * percent
        ElseIf type = Output_Type.Exponential Then
            output = Math.Abs(percent) * percent
        ElseIf type = Output_Type.AdjustedExponential Then
            output = Math.Abs(percent) * Math.Pow(percent, ExponentPower - 1)
        End If

        Return output
    End Function

End Module
