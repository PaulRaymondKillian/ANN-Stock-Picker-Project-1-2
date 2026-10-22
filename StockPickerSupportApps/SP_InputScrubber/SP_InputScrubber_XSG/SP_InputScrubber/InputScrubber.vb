Imports System.IO

Public Class InputScrubber
    Public inputDir As String = ""
    Public inputFile As String = ""
    Public inputExtenstion As String = ""
    Public outputFileSuffix As String = ""

    Public sr As StreamReader
    Public sw_Output As StreamWriter
    Public sw_PercentChange As StreamWriter
    Public sw_Results As StreamWriter
    Public sw_BookendsReport As StreamWriter

    Public CountInputRows As Integer
    Public CountParms As Integer
    Public CountParmsPerStock
    Public CountStocks As Integer
    Public line As String
    Public InputStrings() As String
    Public InputRow() As String
    Public StockList(,) As Stock
    Public OutputList() As Output
    Public AdjustVolume As Boolean = False

    Public PercentThreshold As Decimal = 10
    Public ExponentPower As Decimal = 3
    Public OutputType As Output_Type

    Public countInputTimeUnits As Integer = 1
    Public countPredictionTimeUnits As Integer = 1

    Public StaggerInputRows As Boolean = True
    Public CountBookendLength As Integer = 1

    Public BinThreshInverse As Boolean = False
    Public BinThreshOutput As String = 1

    Public Enum Output_Type As Integer
        Binary_Threshold = 0
        Linear = 1
        LinearTimes10 = 2
        Exponential = 3
        AdjustedExponential = 4
    End Enum

    Public Sub New(ByVal FilePath As String)
        ReadScrubberFile(FilePath)
        SetInputFileInformation(inputDir & inputFile & "." & inputExtenstion)
        CreateStockDataArray(inputDir & inputFile & "." & inputExtenstion)
    End Sub

    Public Sub ScrubFile()
        Try
            PopulateOutputList()
            WriteOutputFile()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub ReadScrubberFile(ByVal FilePath As String)
        Dim sr As StreamReader
        Dim split(), line As String
        Try
            sr = File.OpenText(FilePath)
            line = sr.ReadLine
            line = sr.ReadLine
            split = line.Split("="c)
            DeconstructInputFileName(split(1))

            sr.ReadLine()
            sr.ReadLine()

            line = sr.ReadLine
            split = line.Split("="c)
            AdjustVolume = Convert.ToBoolean(split(1))

            line = sr.ReadLine
            split = line.Split("="c)
            OutputType = Convert.ToInt32(split(1))

            line = sr.ReadLine
            split = line.Split("="c)
            If split.Count > 1 Or split(1) <> "" Then
                PercentThreshold = Convert.ToInt32(split(1))
            End If

            line = sr.ReadLine
            split = line.Split("="c)
            If split.Count > 1 AndAlso split(1) <> "" Then
                ExponentPower = Convert.ToInt32(split(1))
            End If

            line = sr.ReadLine
            split = line.Split("="c)
            If split.Count > 1 AndAlso split(1) <> "" Then
                BinThreshInverse = Convert.ToBoolean(split(1))
            End If

            line = sr.ReadLine
            split = line.Split("="c)
            If split.Count > 1 AndAlso split(1) <> "" Then
                BinThreshOutput = Convert.ToInt32(split(1))
            End If

            line = sr.ReadLine
            split = line.Split("="c)
            outputFileSuffix = split(1)

            sr.ReadLine()
            sr.ReadLine()

            line = sr.ReadLine
            split = line.Split("="c)
            countInputTimeUnits = Convert.ToInt32(split(1))

            line = sr.ReadLine
            split = line.Split("="c)
            countPredictionTimeUnits = Convert.ToInt32(split(1))

            line = sr.ReadLine
            split = line.Split("="c)
            StaggerInputRows = Convert.ToBoolean(split(1))

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub SetInputParms(ByVal InputParms() As String)
        inputDir = InputParms(0)
        inputFile = InputParms(1)
        inputExtenstion = InputParms(2)
        outputFileSuffix = InputParms(3)

        AdjustVolume = InputParms(4)

        OutputType = InputParms(5)
        PercentThreshold = InputParms(6)
        ExponentPower = InputParms(7)
        BinThreshInverse = InputParms(8)
        BinThreshOutput = InputParms(9)

        countInputTimeUnits = InputParms(10)
        countPredictionTimeUnits = InputParms(11)

        StaggerInputRows = InputParms(12)
    End Sub

    Public Sub WriteOutputFile()
        sw_Output = File.CreateText(inputDir & inputFile & outputFileSuffix & ".DATA")
        Try
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
        Catch ex As Exception
            Throw New Exception("Error writing to output file: " & ex.Message)
        Finally
            sw_Output.Close()
        End Try
    End Sub

    Public Sub PopulateOutputList()
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
    End Sub

    Public Sub DeconstructInputFileName(ByVal FilePath As String)
        Dim split() As String
        Dim FileSplit() As String

        'Extract the Directory Path and the  File Name
        split = FilePath.Split("\"c)
        For i As Integer = 0 To split.Length - 1
            If i = 0 Then
                inputDir = split(i)
            ElseIf i = split.Length - 1 Then
                inputFile = split(i)
                inputDir += "\"
            Else
                inputDir += "\" & split(i)
            End If
        Next

        'Remove the extension from the file, this logic can handle extra periods in the file name
        FileSplit = inputFile.Split("."c)
        inputFile = ""  'Reset Value for further deconstruction
        inputExtenstion = FileSplit(FileSplit.Length - 1)
        For i As Integer = 0 To FileSplit.Length - 2
            inputFile += FileSplit(i)
            If i <> FileSplit.Length - 2 Then
                inputFile += "."
            End If
        Next

    End Sub

    Public Function CalculateOutput(ByVal type As Output_Type, ByVal percent As Decimal) As Decimal
        Dim output As Decimal
        percent *= 100

        If type = Output_Type.Binary_Threshold Then
            If percent >= PercentThreshold Then
                'Percent Threshold Met
                If BinThreshInverse = False Then
                    'If Inverse is NOT checked do normal function
                    output = BinThreshOutput
                Else
                    'If Inverse IS CHECKED then set to 0 for a buy
                    output = 0
                End If
            Else
                'Percent Threshold NOT Met
                If BinThreshInverse = False Then
                    'If Inverse is NOT checked do normal function
                    output = 0
                Else
                    'If Inverse IS CHECKED then set to 0 for a buy
                    output = BinThreshOutput
                End If
            End If
        ElseIf type = Output_Type.Linear Then
            output = percent
        ElseIf type = Output_Type.LinearTimes10 Then
            output = 100 * percent
        ElseIf type = Output_Type.Exponential Then
            output = Math.Abs(percent) * percent    'This setup is necessary for a negative value to remain negative
        ElseIf type = Output_Type.AdjustedExponential Then
            output = Math.Abs(percent) * Math.Pow(percent, ExponentPower - 1)   'This setup is necessar for a negative value to remain negative
        End If

        Return output
    End Function

    Public Sub CreateStockDataArray(ByVal FilePath As String)
        ReDim StockList(CountInputRows - 1, CountStocks - 1)
        ReDim InputStrings(CountInputRows - 1)

        Dim sr As StreamReader
        Dim line As String
        sr = File.OpenText(FilePath)
        sr.ReadLine() 'Skipping past the Parm Per Stock data line
        For inputRowIndex As Integer = 0 To CountInputRows - 1
            line = sr.ReadLine
            InputRow = line.Split(","c)
            InputStrings(inputRowIndex) = line
            Dim index As Integer = 0
            'Create array of stock data
            For i As Integer = 0 To CountStocks - 1
                StockList(inputRowIndex, i) = New Stock(i)
                With StockList(inputRowIndex, i)
                    .open = Convert.ToDecimal(InputRow(index))
                    .close = Convert.ToDecimal(InputRow(index + 1))
                    .high = Convert.ToDecimal(InputRow(index + 2))
                    .low = Convert.ToDecimal(InputRow(index + 3))
                    .volume = Convert.ToDecimal(InputRow(index + 4))
                    .adjClose = Convert.ToDecimal(InputRow(index + 5))
                End With
                index += 6
            Next
        Next
    End Sub

    Public Sub SetInputFileInformation(ByVal FilePath As String)
        CountInputRows = 0
        CountParms = -1
        Dim sr As StreamReader
        Dim line As String
        Dim split() As String
        sr = File.OpenText(FilePath)
        CountParmsPerStock = Convert.ToInt32(sr.ReadLine)
        Do Until sr.EndOfStream
            line = sr.ReadLine
            split = line.Split(","c)
            If CountParms = -1 Then
                CountParms = split.Length
            ElseIf CountParms <> split.Length Then
                Throw New Exception("Invalid File: Number of parameters per input row must be the same. Input parameters on row " & CountInputRows & " is " & split.Length & ", previous row had " & CountParms)
            End If

            CountInputRows += 1
        Loop

        CountStocks = CountParms / CountParmsPerStock
    End Sub

End Class
