Imports System.IO

Module Module1
    Dim ScrubberFileLocation As String

    Sub Main(ByVal args() As String)
        If args.Count = 1 Then
            ScrubberFileLocation = args(0)
            Console.WriteLine("Creating Scrubber Object...")
            Dim Scrubber As New SP_InputScrubber.InputScrubber(ScrubberFileLocation)
            Console.WriteLine("Scrubbing Raw Data File...")
            Scrubber.ScrubFile()
            Console.WriteLine("File Scrubbed Successfully!")
        Else
            Console.WriteLine("Invalid number of parameters entered. Parameter must be Scrubber File Loction")
        End If

        'Console.WriteLine("Press Enter to Exit...")
        'Dim z As String = Console.ReadLine

    End Sub

End Module
