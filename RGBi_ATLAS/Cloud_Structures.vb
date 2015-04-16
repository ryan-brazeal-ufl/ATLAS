'''''ATLAS'''''
'By: Ryan Brazeal
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

Module Cloud_Structures

    Friend Structure cloudPoint
        Dim X As Double
        Dim Y As Double
        Dim Z As Double
        Dim I As Integer
        Dim R As Integer
        Dim G As Integer
        Dim B As Integer
        Dim Xm As Double
        Dim Ym As Double
        'Dim Zm As Double
        Dim datasetNum As Integer
        Dim displayX As Integer
        Dim displayY As Integer
        Dim deleted As Boolean
        Dim highlighted As Boolean
        Dim inView As Boolean
    End Structure

    Friend Structure cloudLine
        Dim startX As Double
        Dim startY As Double
        Dim startZ As Double
        Dim endX As Double
        Dim endY As Double
        Dim endZ As Double
        Dim lineColour As Color
        Dim weight As Integer
    End Structure

    Friend Structure cloudCircle
        Dim edgePoints As Generic.List(Of cloudPoint)
        Dim centrePoint As cloudPoint
        Dim colour As Color
        Dim weight As Integer
    End Structure

End Module
