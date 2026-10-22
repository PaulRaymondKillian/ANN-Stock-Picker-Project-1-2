<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtCountInputUnits = New System.Windows.Forms.TextBox
        Me.txtCountPredictionUnits = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.rdoTimeSpanStaggered = New System.Windows.Forms.RadioButton
        Me.rdoTimeSpanIso = New System.Windows.Forms.RadioButton
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtCountStocks = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtCountParmsPerStock = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtCountParms = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtCountInputRows = New System.Windows.Forms.TextBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.txtBookendLength = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkBookendsMetrics = New System.Windows.Forms.CheckBox
        Me.chkPercentChangeMetrics = New System.Windows.Forms.CheckBox
        Me.chkResultsMetrics = New System.Windows.Forms.CheckBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtOTypeExponent = New System.Windows.Forms.TextBox
        Me.txtOTypeBinThreshold = New System.Windows.Forms.TextBox
        Me.rdoOTypeExponential = New System.Windows.Forms.RadioButton
        Me.rdoOTypeLinear = New System.Windows.Forms.RadioButton
        Me.rdoOTypeBinary = New System.Windows.Forms.RadioButton
        Me.rdoOTypeSquared = New System.Windows.Forms.RadioButton
        Me.btnGo = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtInputFile = New System.Windows.Forms.TextBox
        Me.btnInputFile = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtCountInputUnits)
        Me.GroupBox2.Controls.Add(Me.txtCountPredictionUnits)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Location = New System.Drawing.Point(455, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(194, 73)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Prediction Settings"
        '
        'txtCountInputUnits
        '
        Me.txtCountInputUnits.Location = New System.Drawing.Point(122, 19)
        Me.txtCountInputUnits.Name = "txtCountInputUnits"
        Me.txtCountInputUnits.Size = New System.Drawing.Size(37, 20)
        Me.txtCountInputUnits.TabIndex = 10
        Me.txtCountInputUnits.Text = "1"
        '
        'txtCountPredictionUnits
        '
        Me.txtCountPredictionUnits.Location = New System.Drawing.Point(122, 43)
        Me.txtCountPredictionUnits.Name = "txtCountPredictionUnits"
        Me.txtCountPredictionUnits.Size = New System.Drawing.Size(37, 20)
        Me.txtCountPredictionUnits.TabIndex = 11
        Me.txtCountPredictionUnits.Text = "1"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Input Time Units:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 46)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(110, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "Prediction Time Units:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rdoTimeSpanStaggered)
        Me.GroupBox3.Controls.Add(Me.rdoTimeSpanIso)
        Me.GroupBox3.Location = New System.Drawing.Point(455, 82)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(194, 66)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Time Span Progression Settings"
        '
        'rdoTimeSpanStaggered
        '
        Me.rdoTimeSpanStaggered.AutoSize = True
        Me.rdoTimeSpanStaggered.Checked = True
        Me.rdoTimeSpanStaggered.Location = New System.Drawing.Point(9, 19)
        Me.rdoTimeSpanStaggered.Name = "rdoTimeSpanStaggered"
        Me.rdoTimeSpanStaggered.Size = New System.Drawing.Size(74, 17)
        Me.rdoTimeSpanStaggered.TabIndex = 10
        Me.rdoTimeSpanStaggered.TabStop = True
        Me.rdoTimeSpanStaggered.Text = "Staggered"
        Me.rdoTimeSpanStaggered.UseVisualStyleBackColor = True
        '
        'rdoTimeSpanIso
        '
        Me.rdoTimeSpanIso.AutoSize = True
        Me.rdoTimeSpanIso.Enabled = False
        Me.rdoTimeSpanIso.Location = New System.Drawing.Point(9, 42)
        Me.rdoTimeSpanIso.Name = "rdoTimeSpanIso"
        Me.rdoTimeSpanIso.Size = New System.Drawing.Size(154, 17)
        Me.rdoTimeSpanIso.TabIndex = 11
        Me.rdoTimeSpanIso.Text = "Isolated Multi-Day Handling"
        Me.rdoTimeSpanIso.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.txtCountStocks)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.txtCountParmsPerStock)
        Me.GroupBox4.Controls.Add(Me.Label4)
        Me.GroupBox4.Controls.Add(Me.txtCountParms)
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Controls.Add(Me.txtCountInputRows)
        Me.GroupBox4.Location = New System.Drawing.Point(8, 33)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(441, 45)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Input File Information"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(351, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Stocks:"
        '
        'txtCountStocks
        '
        Me.txtCountStocks.Enabled = False
        Me.txtCountStocks.Location = New System.Drawing.Point(400, 17)
        Me.txtCountStocks.Name = "txtCountStocks"
        Me.txtCountStocks.Size = New System.Drawing.Size(34, 20)
        Me.txtCountStocks.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(221, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Parms/Stock:"
        '
        'txtCountParmsPerStock
        '
        Me.txtCountParmsPerStock.Enabled = False
        Me.txtCountParmsPerStock.Location = New System.Drawing.Point(299, 17)
        Me.txtCountParmsPerStock.Name = "txtCountParmsPerStock"
        Me.txtCountParmsPerStock.Size = New System.Drawing.Size(34, 20)
        Me.txtCountParmsPerStock.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(125, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Parms:"
        '
        'txtCountParms
        '
        Me.txtCountParms.Enabled = False
        Me.txtCountParms.Location = New System.Drawing.Point(170, 17)
        Me.txtCountParms.Name = "txtCountParms"
        Me.txtCountParms.Size = New System.Drawing.Size(34, 20)
        Me.txtCountParms.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Input Rows:"
        '
        'txtCountInputRows
        '
        Me.txtCountInputRows.Enabled = False
        Me.txtCountInputRows.Location = New System.Drawing.Point(76, 17)
        Me.txtCountInputRows.Name = "txtCountInputRows"
        Me.txtCountInputRows.Size = New System.Drawing.Size(34, 20)
        Me.txtCountInputRows.TabIndex = 10
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtBookendLength)
        Me.GroupBox5.Controls.Add(Me.Label1)
        Me.GroupBox5.Controls.Add(Me.chkBookendsMetrics)
        Me.GroupBox5.Controls.Add(Me.chkPercentChangeMetrics)
        Me.GroupBox5.Controls.Add(Me.chkResultsMetrics)
        Me.GroupBox5.Location = New System.Drawing.Point(455, 280)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(194, 113)
        Me.GroupBox5.TabIndex = 1
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Output Metrics Files"
        '
        'txtBookendLength
        '
        Me.txtBookendLength.Enabled = False
        Me.txtBookendLength.Location = New System.Drawing.Point(127, 82)
        Me.txtBookendLength.Name = "txtBookendLength"
        Me.txtBookendLength.Size = New System.Drawing.Size(32, 20)
        Me.txtBookendLength.TabIndex = 15
        Me.txtBookendLength.Text = "1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 85)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Bookend Length:"
        '
        'chkBookendsMetrics
        '
        Me.chkBookendsMetrics.AutoSize = True
        Me.chkBookendsMetrics.Location = New System.Drawing.Point(9, 65)
        Me.chkBookendsMetrics.Name = "chkBookendsMetrics"
        Me.chkBookendsMetrics.Size = New System.Drawing.Size(165, 17)
        Me.chkBookendsMetrics.TabIndex = 13
        Me.chkBookendsMetrics.Text = "Bookends Metrics Report File"
        Me.chkBookendsMetrics.UseVisualStyleBackColor = True
        '
        'chkPercentChangeMetrics
        '
        Me.chkPercentChangeMetrics.AutoSize = True
        Me.chkPercentChangeMetrics.Location = New System.Drawing.Point(9, 42)
        Me.chkPercentChangeMetrics.Name = "chkPercentChangeMetrics"
        Me.chkPercentChangeMetrics.Size = New System.Drawing.Size(165, 17)
        Me.chkPercentChangeMetrics.TabIndex = 12
        Me.chkPercentChangeMetrics.Text = "Percent Changed Metrics File"
        Me.chkPercentChangeMetrics.UseVisualStyleBackColor = True
        '
        'chkResultsMetrics
        '
        Me.chkResultsMetrics.AutoSize = True
        Me.chkResultsMetrics.Checked = True
        Me.chkResultsMetrics.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkResultsMetrics.Location = New System.Drawing.Point(9, 19)
        Me.chkResultsMetrics.Name = "chkResultsMetrics"
        Me.chkResultsMetrics.Size = New System.Drawing.Size(117, 17)
        Me.chkResultsMetrics.TabIndex = 8
        Me.chkResultsMetrics.Text = "Results Metrics File"
        Me.chkResultsMetrics.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label11)
        Me.GroupBox6.Controls.Add(Me.Label10)
        Me.GroupBox6.Controls.Add(Me.Label9)
        Me.GroupBox6.Controls.Add(Me.txtOTypeExponent)
        Me.GroupBox6.Controls.Add(Me.txtOTypeBinThreshold)
        Me.GroupBox6.Controls.Add(Me.rdoOTypeExponential)
        Me.GroupBox6.Controls.Add(Me.rdoOTypeLinear)
        Me.GroupBox6.Controls.Add(Me.rdoOTypeBinary)
        Me.GroupBox6.Controls.Add(Me.rdoOTypeSquared)
        Me.GroupBox6.Location = New System.Drawing.Point(455, 154)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(194, 120)
        Me.GroupBox6.TabIndex = 1
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Output Type"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(163, 90)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(29, 13)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "(x^y)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(98, 90)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(18, 13)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "y="
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(165, 21)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(15, 13)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "%"
        '
        'txtOTypeExponent
        '
        Me.txtOTypeExponent.Enabled = False
        Me.txtOTypeExponent.Location = New System.Drawing.Point(122, 87)
        Me.txtOTypeExponent.Name = "txtOTypeExponent"
        Me.txtOTypeExponent.Size = New System.Drawing.Size(37, 20)
        Me.txtOTypeExponent.TabIndex = 14
        Me.txtOTypeExponent.Text = "3"
        '
        'txtOTypeBinThreshold
        '
        Me.txtOTypeBinThreshold.Location = New System.Drawing.Point(122, 16)
        Me.txtOTypeBinThreshold.Name = "txtOTypeBinThreshold"
        Me.txtOTypeBinThreshold.Size = New System.Drawing.Size(37, 20)
        Me.txtOTypeBinThreshold.TabIndex = 11
        Me.txtOTypeBinThreshold.Text = "10"
        '
        'rdoOTypeExponential
        '
        Me.rdoOTypeExponential.AutoSize = True
        Me.rdoOTypeExponential.Location = New System.Drawing.Point(9, 88)
        Me.rdoOTypeExponential.Name = "rdoOTypeExponential"
        Me.rdoOTypeExponential.Size = New System.Drawing.Size(80, 17)
        Me.rdoOTypeExponential.TabIndex = 11
        Me.rdoOTypeExponential.Text = "Exponential"
        Me.rdoOTypeExponential.UseVisualStyleBackColor = True
        '
        'rdoOTypeLinear
        '
        Me.rdoOTypeLinear.AutoSize = True
        Me.rdoOTypeLinear.Location = New System.Drawing.Point(9, 42)
        Me.rdoOTypeLinear.Name = "rdoOTypeLinear"
        Me.rdoOTypeLinear.Size = New System.Drawing.Size(54, 17)
        Me.rdoOTypeLinear.TabIndex = 12
        Me.rdoOTypeLinear.TabStop = True
        Me.rdoOTypeLinear.Text = "Linear"
        Me.rdoOTypeLinear.UseVisualStyleBackColor = True
        '
        'rdoOTypeBinary
        '
        Me.rdoOTypeBinary.AutoSize = True
        Me.rdoOTypeBinary.Checked = True
        Me.rdoOTypeBinary.Location = New System.Drawing.Point(9, 19)
        Me.rdoOTypeBinary.Name = "rdoOTypeBinary"
        Me.rdoOTypeBinary.Size = New System.Drawing.Size(107, 17)
        Me.rdoOTypeBinary.TabIndex = 13
        Me.rdoOTypeBinary.TabStop = True
        Me.rdoOTypeBinary.Text = "Binary Threshold:"
        Me.rdoOTypeBinary.UseVisualStyleBackColor = True
        '
        'rdoOTypeSquared
        '
        Me.rdoOTypeSquared.AutoSize = True
        Me.rdoOTypeSquared.Location = New System.Drawing.Point(9, 65)
        Me.rdoOTypeSquared.Name = "rdoOTypeSquared"
        Me.rdoOTypeSquared.Size = New System.Drawing.Size(65, 17)
        Me.rdoOTypeSquared.TabIndex = 9
        Me.rdoOTypeSquared.TabStop = True
        Me.rdoOTypeSquared.Text = "Squared"
        Me.rdoOTypeSquared.UseVisualStyleBackColor = True
        '
        'btnGo
        '
        Me.btnGo.Enabled = False
        Me.btnGo.Location = New System.Drawing.Point(455, 399)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(194, 27)
        Me.btnGo.TabIndex = 4
        Me.btnGo.Text = "Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Input File:"
        '
        'txtInputFile
        '
        Me.txtInputFile.Location = New System.Drawing.Point(64, 6)
        Me.txtInputFile.Name = "txtInputFile"
        Me.txtInputFile.Size = New System.Drawing.Size(347, 20)
        Me.txtInputFile.TabIndex = 6
        '
        'btnInputFile
        '
        Me.btnInputFile.Location = New System.Drawing.Point(417, 4)
        Me.btnInputFile.Name = "btnInputFile"
        Me.btnInputFile.Size = New System.Drawing.Size(32, 23)
        Me.btnInputFile.TabIndex = 7
        Me.btnInputFile.Text = "..."
        Me.btnInputFile.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.HorizontalScrollbar = True
        Me.ListBox1.Location = New System.Drawing.Point(8, 84)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(441, 342)
        Me.ListBox1.TabIndex = 8
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(659, 435)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.btnInputFile)
        Me.Controls.Add(Me.txtInputFile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox4)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(667, 469)
        Me.MinimumSize = New System.Drawing.Size(667, 469)
        Me.Name = "Form1"
        Me.Text = "SP: Input Scrubber - Extended Storage GUI"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCountInputRows As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtInputFile As System.Windows.Forms.TextBox
    Friend WithEvents btnInputFile As System.Windows.Forms.Button
    Friend WithEvents chkResultsMetrics As System.Windows.Forms.CheckBox
    Friend WithEvents rdoOTypeSquared As System.Windows.Forms.RadioButton
    Friend WithEvents txtCountInputUnits As System.Windows.Forms.TextBox
    Friend WithEvents txtCountPredictionUnits As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents rdoTimeSpanStaggered As System.Windows.Forms.RadioButton
    Friend WithEvents rdoTimeSpanIso As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtCountStocks As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtCountParmsPerStock As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCountParms As System.Windows.Forms.TextBox
    Friend WithEvents chkPercentChangeMetrics As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtOTypeExponent As System.Windows.Forms.TextBox
    Friend WithEvents txtOTypeBinThreshold As System.Windows.Forms.TextBox
    Friend WithEvents rdoOTypeExponential As System.Windows.Forms.RadioButton
    Friend WithEvents rdoOTypeLinear As System.Windows.Forms.RadioButton
    Friend WithEvents rdoOTypeBinary As System.Windows.Forms.RadioButton
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents chkBookendsMetrics As System.Windows.Forms.CheckBox
    Friend WithEvents txtBookendLength As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
