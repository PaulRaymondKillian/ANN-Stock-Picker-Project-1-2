Imports System.IO
Imports System.Text.StringBuilder

Public Class Form1
    Dim inputDir As String = ""
    Dim inputFile As String = ""
    Dim inputExtenstion As String = ""
    Dim outputFileSuffix As String = ""

    Dim sr As StreamReader
    Dim sw_Output As StreamWriter
    Dim sw_PercentChange As StreamWriter
    Dim sw_Results As StreamWriter
    Dim sw_BookendsReport As StreamWriter

    Dim CountInputRows As Integer
    Dim CountParms As Integer
    Dim CountParmsPerStock
    Dim CountStocks As Integer
    Dim line As String
    Dim InputStrings() As String
    Dim InputRow() As String
    Dim StockList(,) As Stock
    Dim OutputList() As Output
    Dim AdjustVolume As Boolean = False

    Dim PercentThreshold As Decimal = 10
    Dim ExponentPower As Decimal = 3
    Dim OutputType As Output_Type

    Dim countInputTimeUnits As Integer = 1
    Dim countPredictionTimeUnits As Integer = 1

    Dim StaggerInputRows As Boolean = True
    Dim CountBookendLength As Integer = 1

    Public Enum Output_Type As Integer
        Binary_Threshold = 0
        Linear = 1
        LinearTimes10 = 2
        Exponential = 3
        AdjustedExponential = 4
    End Enum

    Private Sub btnInputFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputFile.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                txtInputFile.Text = OpenFileDialog1.FileName
                DeconstructInputFileName(txtInputFile.Text)
                SetInputFileInformation(inputDir & inputFile & "." & inputExtenstion)
                CreateStockDataArray(inputDir & inputFile & "." & inputExtenstion)
                PopulateListBox(InputStrings)
                btnGo.Enabled = True
            Catch ex As Exception
                btnGo.Enabled = False
                MsgBox("Error Opening Input File and Creating Stock Data Array - " & ex.Message)
            End Try
        End If
    End Sub

    Public Sub PopulateListBox(ByVal StringArray() As String)
        ListBox1.Items.Clear()
        For i As Integer = 0 To StringArray.Length - 1
            ListBox1.Items.Add(StringArray(i))
        Next
    End Sub

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
                    .high = Convert.ToDecimal(InputRow(index + 1))
                    .low = Convert.ToDecimal(InputRow(index + 2))
                    .close = Convert.ToDecimal(InputRow(index + 3))
                    .volume = Convert.ToInt32(InputRow(index + 4))
                    .adjClose = Convert.ToDecimal(InputRow(index + 5))
                    If AdjustVolume Then
                        .AdjustVolume()
                    End If
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
                txtCountInputRows.Text = "N/A"
                txtCountParms.Text = "N/A"
                txtCountParmsPerStock.Text = "N/A"
                txtCountStocks.Text = "N/A"
                Throw New Exception("Invalid File: Number of parameters per input row must be the same. Input parameters on row " & CountInputRows & " is " & split.Length & ", previous row had " & CountParms)
            End If

            CountInputRows += 1
        Loop

        CountStocks = CountParms / CountParmsPerStock
        txtCountInputRows.Text = CountInputRows
        txtCountParms.Text = CountParms
        txtCountParmsPerStock.Text = CountParmsPerStock
        txtCountStocks.Text = CountStocks


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
                If cboBinThreshInverse.Checked = False Then
                    'If Inverse is NOT checked do normal function
                    output = txtBinThreshOutput.Text
                Else
                    'If Inverse IS CHECKED then set to 0 for a buy
                    output = 0
                End If
            Else
                'Percent Threshold NOT Met
                If cboBinThreshInverse.Checked = False Then
                    'If Inverse is NOT checked do normal function
                    output = 0
                Else
                    'If Inverse IS CHECKED then set to 0 for a buy
                    output = txtBinThreshOutput.Text
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

    Private Sub txtCountInputUnits_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCountInputUnits.LostFocus
        If Not IsNumeric(txtCountInputUnits.Text) Then
            MsgBox("Invalid Value for Count Input Units")
            txtCountInputUnits.Text = "1"
            countInputTimeUnits = Convert.ToInt32(txtCountInputUnits.Text)
        End If
    End Sub

    Private Sub txtCountInputUnits_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCountInputUnits.TextChanged
        If IsNumeric(txtCountInputUnits.Text) Then
            countInputTimeUnits = Convert.ToInt32(txtCountInputUnits.Text)

            If Convert.ToInt32(txtCountInputUnits.Text) > 1 Then
                rdoTimeSpanIso.Enabled = True
            Else
                rdoTimeSpanIso.Enabled = False
                rdoTimeSpanStaggered.Checked = True
            End If
        End If
    End Sub

    Private Sub rdoOTypeBinary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoOTypeBinary.CheckedChanged
        If rdoOTypeBinary.Checked = True Then
            OutputType = Output_Type.Binary_Threshold
            txtOTypeBinThreshold.Enabled = True
            txtBinThreshOutput.Enabled = True
            cboBinThreshInverse.Enabled = True
        Else
            txtOTypeBinThreshold.Enabled = False
            txtBinThreshOutput.Enabled = False
            cboBinThreshInverse.Enabled = False
            cboBinThreshInverse.Checked = False
        End If
    End Sub

    Private Sub rdoOTypeExponential_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoOTypeExponential.CheckedChanged
        If rdoOTypeExponential.Checked Then
            OutputType = Output_Type.AdjustedExponential
            txtOTypeExponent.Enabled = True
        Else
            txtOTypeExponent.Enabled = False
        End If
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Try
            PopulateOutputList()
            WriteOutputFile()
            MsgBox("Output File Created Successfully!")
            If chkPercentChangeMetrics.Checked = True Then
                WritePercentChangeMetricsFile()
            End If
            If chkResultsMetrics.Checked = True Then
                WriteResultsMetricsFile()
            End If
            If chkBookendsMetrics.Checked = True Then
                WriteBookendsMetricsReportFile()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub WriteBookendsMetricsReportFile()
        'Use this method to output data over time about 2 stocks on the front of the stock list, and 2 stocks on the end of the stock list (bookends)
        'This will be used to further verify the data is being processed correctly.
        Try
            sw_BookendsReport = File.CreateText(inputDir & inputFile & outputFileSuffix & "_BookendsReport" & ".DAT")
            sw_BookendsReport.WriteLine("Input Data Time Units: " & countInputTimeUnits)
            sw_BookendsReport.WriteLine("Prediction Time Units: " & countPredictionTimeUnits)
            sw_BookendsReport.WriteLine()
            For OutputRowIndex As Integer = 0 To OutputList.Length - 1
                sw_BookendsReport.WriteLine("####################################################")
                For i As Integer = 0 To CountBookendLength - 1
                    WriteBookendsMetricsStockInfo(OutputRowIndex, i)
                    sw_BookendsReport.WriteLine()
                Next
                sw_BookendsReport.WriteLine("###" & vbCrLf)
                For i As Integer = CountBookendLength To 1 Step -1
                    WriteBookendsMetricsStockInfo(OutputRowIndex, CountStocks - i)
                    If i <> 1 Then
                        sw_BookendsReport.WriteLine()
                    End If
                Next
                sw_BookendsReport.WriteLine("####################################################")
                sw_BookendsReport.WriteLine()
            Next
        Catch ex As Exception
            Throw ex
        Finally
            sw_BookendsReport.Close()
        End Try
    End Sub

    Public Sub WriteBookendsMetricsStockInfo(ByVal OutputRowIndex As Integer, ByVal StockIndex As Integer)
        Dim open, close As Decimal

        sw_BookendsReport.WriteLine("Row #" & OutputRowIndex & " - Stock #" & StockIndex & ":")

        'Write Input Data Units for this Moment in Time for Stock
        sw_BookendsReport.Write("Inputs Data: ")
        Dim StockListBaseRow As Integer
        If StaggerInputRows Then
            StockListBaseRow = OutputRowIndex
        Else
            StockListBaseRow = OutputRowIndex * countInputTimeUnits
        End If
        For i As Integer = 0 To countInputTimeUnits - 1
            With StockList(StockListBaseRow + i, StockIndex)
                sw_BookendsReport.Write(.open & "," & .high & "," & .low & "," & .close & "," & .volume & "," & .adjClose)
            End With
            If i <> countInputTimeUnits - 1 Then
                sw_BookendsReport.Write(" | ")
            Else
                sw_BookendsReport.Write(vbCrLf)
            End If
        Next

        'Write Prediction Data Units for this Moment in Time for Stock
        open = -1   'Default value
        close = -1  'Default value
        sw_BookendsReport.Write("Prediction Data: ")
        For i As Integer = 0 To countPredictionTimeUnits - 1
            With StockList(StockListBaseRow + countInputTimeUnits + i, StockIndex)
                If i = 0 And countPredictionTimeUnits = 1 Then
                    open = .open
                    close = .close
                    sw_BookendsReport.Write("<" & .open & ">," & .high & "," & .low & ",<" & .close & ">," & .volume & "," & .adjClose)
                ElseIf i = 0 Then
                    open = .open
                    sw_BookendsReport.Write("<" & .open & ">," & .high & "," & .low & "," & .close & "," & .volume & "," & .adjClose)
                ElseIf i = countPredictionTimeUnits - 1 Then
                    close = .close
                    sw_BookendsReport.Write(.open & "," & .high & "," & .low & ",<" & .close & ">," & .volume & "," & .adjClose)
                Else
                    sw_BookendsReport.Write(.open & "," & .high & "," & .low & "," & .close & "," & .volume & "," & .adjClose)
                End If
            End With
            If i <> countPredictionTimeUnits - 1 Then
                sw_BookendsReport.Write(" | ")
            Else
                sw_BookendsReport.Write(vbCrLf)
            End If
        Next

        sw_BookendsReport.WriteLine("Calculations: " & close & "-" & open & "=" & close - open & " | " & close - open & "/" & open & "=<" & 100 * (close - open) / open & "%> - Result: " & OutputList(OutputRowIndex).Result(StockIndex))
    End Sub

    Public Sub WriteResultsMetricsFile()
        Try
            sw_Results = File.CreateText(inputDir & inputFile & outputFileSuffix & "_Results" & ".DAT")
            sw_Results.WriteLine("Output File Information:")
            sw_Results.WriteLine("Input File contained " & CountParms & " total parameters per row, " & CountParmsPerStock & " parameters per stock per unit of time for " & CountStocks & " stocks.")
            sw_Results.WriteLine("Each row of output file contains data for " & countInputTimeUnits & " units of time per stock.")
            sw_Results.WriteLine("Each row of output file contains results for " & countPredictionTimeUnits & " units of time in the future per stock.")
            sw_Results.WriteLine("")
            For OutputRowIndex As Integer = 0 To OutputList.Length - 1
                Dim ResultsString As New System.Text.StringBuilder("Data Row " & OutputRowIndex & ": ")
                If OutputType = Output_Type.Binary_Threshold Then
                    ResultsString.Append(CountBuys(OutputRowIndex) & " Buys = ")
                End If
                For StockIndex As Integer = 0 To CountStocks - 1
                    ResultsString.Append(Math.Round(OutputList(OutputRowIndex).Result(StockIndex), 4))
                    If StockIndex <> CountStocks - 1 Then
                        ResultsString.Append(", ")
                    End If
                Next
                sw_Results.WriteLine(ResultsString.ToString)
                ResultsString = Nothing
            Next
        Catch ex As Exception
            Throw ex
        Finally
            sw_Results.Close()
        End Try
    End Sub

    Public Sub WritePercentChangeMetricsFile()
        Try
            sw_PercentChange = File.CreateText(inputDir & inputFile & outputFileSuffix & "_PercentChange" & ".DAT")
            For InputRowIndex As Integer = 0 To OutputList.Length - 1
                sw_PercentChange.WriteLine("Data Row " & InputRowIndex & " Percent Changes:")
                Dim PercentChangeString As New System.Text.StringBuilder("")
                For StockIndex As Integer = 0 To CountStocks - 1
                    PercentChangeString.Append(Math.Round(OutputList(InputRowIndex).PercentChange(StockIndex), 4))
                    If StockIndex <> CountStocks - 1 Then
                        PercentChangeString.Append(",")
                    End If
                Next
                sw_PercentChange.WriteLine(PercentChangeString.ToString)
                sw_PercentChange.WriteLine()
                PercentChangeString = Nothing
            Next
        Catch ex As Exception
            Throw ex
        Finally
            sw_PercentChange.Close()
        End Try
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

    Public Function CountBuys(ByVal OutputRowIndex) As Integer
        'This IF should be redundant because the logic should be handled elsewhere, but I will keep it to ensure this does not run when OType <> BinThreshold
        If OutputType = Output_Type.Binary_Threshold Then
            Dim count As Integer = 0
            For i As Integer = 0 To CountStocks - 1
                If cboBinThreshInverse.Checked = False Then
                    'If Inverse is NOT checked
                    If OutputList(OutputRowIndex).Result(i) = Convert.ToDecimal(txtBinThreshOutput.Text) Then
                        count += 1
                    End If
                Else
                    'If Inverse IS CHECKED
                    If OutputList(OutputRowIndex).Result(i) = 0 Then
                        count += 1
                    End If
                End If
            Next
            CountBuys = count
        Else
            CountBuys = -1
        End If
    End Function

    Private Sub txtCountPredictionUnits_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCountPredictionUnits.LostFocus
        If Not IsNumeric(txtCountPredictionUnits.Text) Then
            MsgBox("Invalid Value for Count Prediction Units")
            txtCountPredictionUnits.Text = "1"
            countPredictionTimeUnits = Convert.ToInt32(txtCountPredictionUnits.Text)
        End If
    End Sub

    Private Sub txtCountPredictionUnits_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCountPredictionUnits.TextChanged
        If IsNumeric(txtCountPredictionUnits.Text) Then
            countPredictionTimeUnits = Convert.ToInt32(txtCountPredictionUnits.Text)
        End If
    End Sub

    Private Sub rdoOTypeLinear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoOTypeLinear.CheckedChanged
        If rdoOTypeLinear.Checked = True Then
            OutputType = Output_Type.Linear
        End If
    End Sub

    Private Sub rdoOTypeSquared_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoOTypeSquared.CheckedChanged
        If rdoOTypeSquared.Checked = True Then
            OutputType = Output_Type.Exponential
        End If
    End Sub

    Private Sub txtOTypeBinThreshold_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOTypeBinThreshold.TextChanged
        If IsNumeric(txtOTypeBinThreshold.Text) Then
            PercentThreshold = Convert.ToDecimal(txtOTypeBinThreshold.Text)
        End If
    End Sub

    Private Sub txtOTypeExponent_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOTypeExponent.TextChanged
        If IsNumeric(txtOTypeExponent.Text) Then
            ExponentPower = Convert.ToDecimal(txtOTypeExponent.Text)
            If Convert.ToDecimal(txtOTypeExponent.Text) > 5 Then
                MsgBox("Whoa buddy, are you sure? That value may cause overflow and degrade the ability for the network to converge correctly, accurately, or ever.")
            End If
        End If

    End Sub

    Private Sub rdoTimeSpanIso_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoTimeSpanIso.CheckedChanged
        If rdoTimeSpanIso.Checked = True Then
            StaggerInputRows = False
        End If
    End Sub

    Private Sub rdoTimeSpanStaggered_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoTimeSpanStaggered.CheckedChanged
        If rdoTimeSpanStaggered.Checked = True Then
            StaggerInputRows = True
        End If
    End Sub

    Private Sub chkBookendsMetrics_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBookendsMetrics.CheckedChanged
        If chkBookendsMetrics.Checked = True Then
            If Not IsNumeric(txtCountStocks.Text) Then
                MsgBox("Please set input file first to determine if Bookends Report is capable of being run")
                chkBookendsMetrics.Checked = False
            ElseIf Convert.ToInt32(txtCountStocks.Text) < CountBookendLength * 2 Then
                MsgBox("Input file must contain data on " & CountBookendLength * 2 & " or more stocks for Bookends Report to be run.")
                chkBookendsMetrics.Checked = False
            Else
                txtBookendLength.Enabled = True
            End If
        Else
            txtBookendLength.Enabled = False
        End If
    End Sub

    Private Sub txtBookendLength_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBookendLength.TextChanged
        If IsNumeric(txtBookendLength.Text) And txtBookendLength.Enabled = True Then
            If Convert.ToInt32(txtBookendLength.Text) > CountStocks / 2 Then
                MsgBox("Bookend report length is too high, cannot be over " & CountStocks / 2 & " for this particular input file - Resetting back to default value (2)")
                txtBookendLength.Text = 2
            Else
                CountBookendLength = Convert.ToInt32(txtBookendLength.Text)
            End If
        End If
    End Sub

    Private Sub txtBinThreshMultiplier_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBinThreshOutput.LostFocus
        If Not IsNumeric(txtBinThreshOutput.Text) Then
            MsgBox("Error - Invalid Value for Binary Threshold Output Value, changing back to default.")
            txtBinThreshOutput.Text = "1"
        ElseIf Convert.ToDecimal(txtBinThreshOutput.Text) < 1 Then
            If Convert.ToDecimal(txtBinThreshOutput.Text) = 0 Then
                MsgBox("You are an idiot, you cannot use zero as your output value. I am changing the value to 999999999 to punish you.")
                txtBinThreshOutput.Text = "999999999"
            Else
                MsgBox("Warning - You may use that value for the Output Value, but it may hinder training and also may prevent reports from outputting helpful/accurate data.")
            End If
        End If
    End Sub

    Private Sub txtOutputFileSuffix_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOutputFileSuffix.LostFocus
        If InStr(txtOutputFileSuffix.Text, "\") _
        Or InStr(txtOutputFileSuffix.Text, "/") _
        Or InStr(txtOutputFileSuffix.Text, ":") _
        Or InStr(txtOutputFileSuffix.Text, "*") _
        Or InStr(txtOutputFileSuffix.Text, "?") _
        Or InStr(txtOutputFileSuffix.Text, "<") _
        Or InStr(txtOutputFileSuffix.Text, ">") _
        Or InStr(txtOutputFileSuffix.Text, "|") Then
            MsgBox("Invalid Character Entered - \/:*?<>| Not Allowed, please change it")
        Else
            outputFileSuffix = txtOutputFileSuffix.Text
        End If
    End Sub

End Class
