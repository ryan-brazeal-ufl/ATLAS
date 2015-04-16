'''''ATLAS'''''
'By: Ryan Brazeal
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

Public Class histogramForm

    Dim histogram() As Integer
    Dim histMaxMin() As Integer
    Dim beenLoaded As Boolean = False
    Dim renderBlank As Boolean = False
    Dim previousHistogram As Integer = 0

    Private Sub histogramForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        mainForm.histogramDisplayed = False
    End Sub

    Private Sub histogramForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        renderHistogram()
        beenLoaded = True
    End Sub

    'compute the max and min values of a dataset with respect to its Global (unadjusted) Coordinates
    Private Function ComputeMaxMin(ByVal values() As Integer) As Integer(,)

        Dim MaxMin(1, 1) As Integer
        Dim i As Integer

        Try
            MaxMin(0, 0) = values.GetLength(0) - 1
            MaxMin(0, 1) = 0
            MaxMin(1, 0) = -100000000
            MaxMin(1, 1) = 100000000

            For i = 0 To values.Count - 1
                If values(i) <> 0 Then
                    If values(i) > MaxMin(1, 0) Then
                        MaxMin(1, 0) = values(i)
                    End If
                    If values(i) < MaxMin(1, 1) Then
                        MaxMin(1, 1) = values(i)
                    End If
                End If
            Next

        Catch ex As Exception
            MaxMin(0, 0) = 0
            MaxMin(0, 1) = 0
            MaxMin(1, 0) = -100000000
            MaxMin(1, 1) = 100000000
        End Try
        Return MaxMin
    End Function

    Friend Sub renderHistogram()
        If mainForm.histogramToShow = 1 Then
            Me.Text = "Histogram Plot of Insensity Values (ALL DATASETS)"
            histogram = mainForm.intensityHistogram
            histMaxMin = mainForm.intensityHistMaxMin
            renderBlank = False
        ElseIf mainForm.histogramToShow = 2 Then
            Me.Text = "Histogram Plot of Insensity Values (ALL DATASETS WITHIN CURRENT SELECTION)"
            histogram = mainForm.selectedHistogram
            histMaxMin = mainForm.selectedHistMaxMin
            renderBlank = False
        ElseIf mainForm.histogramToShow = 3 Then
            Me.Text = "Histogram Plot of Insensity Values (DATA POINTS ON SCREEN WITHIN CURRENT SELECTION)"
            histogram = mainForm.currentViewHistogram
            histMaxMin = mainForm.currentViewHistMaxMin
            renderBlank = False
        Else
            renderBlank = True
        End If

        Dim renderBitmap As New Bitmap(renderPanel.Width, renderPanel.Height, Imaging.PixelFormat.Format24bppRgb)
        Dim renderGraphics As Graphics

        renderBitmap.SetResolution(300, 300)
        renderGraphics = Graphics.FromImage(renderBitmap)
        renderGraphics.InterpolationMode = Drawing2D.InterpolationMode.Default
        renderGraphics.SmoothingMode = Drawing2D.SmoothingMode.Default
        renderGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        renderGraphics.Clear(Color.FromArgb(60, 60, 60))

        'rendering options
        Dim axisPen As New Pen(Color.White)
        axisPen.Width = 2
        Dim textBrush As New SolidBrush(Color.White)
        Dim colourBrush As New SolidBrush(Color.Yellow)
        Dim colourPen As New Pen(Color.Yellow)
        colourPen.Width = 2
        Dim Toffset As Integer = 40
        Dim Boffset As Integer = 80
        Dim Loffset As Integer = 80
        Dim Roffset As Integer = 40
        Dim numInc As Integer = 10
        Dim xTickLength As Integer = 5
        Dim yTickLength As Integer = 5

        Dim xOrigin As Integer = Loffset
        Dim yorigin As Integer = renderPanel.Height - Boffset
        Dim xAxisLength As Integer = renderPanel.Width - Loffset - Roffset
        Dim yAxisLength As Integer = renderPanel.Height - Toffset - Boffset
        Dim axisFont As New Font("arial", 14, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim sfCentre, sfRight As New StringFormat()
        sfCentre.Alignment = StringAlignment.Center
        sfCentre.LineAlignment = StringAlignment.Center
        sfRight.Alignment = StringAlignment.Far
        sfRight.LineAlignment = StringAlignment.Far

        Dim totalPoints As Integer = 0
        For i = 0 To histogram.Length - 1
            totalPoints += histogram(i)
        Next

        'plotting constants
        Dim histogramMaxMin(,) As Integer = ComputeMaxMin(histogram)
        Dim xRange As Integer = histogramMaxMin(0, 0) - histogramMaxMin(0, 1)
        Dim yRange As Integer = histogramMaxMin(1, 0) - histogramMaxMin(1, 1)

        Dim xInc As Integer = xAxisLength / numInc
        Dim yInc As Integer = yAxisLength / numInc
        Dim iInc As Integer = xRange / numInc
        Dim cInc As Integer = yRange / numInc
        Dim xScale, yScale As Decimal
        If xRange <> 0 Then
            xScale = Convert.ToDecimal(xAxisLength) / Convert.ToDecimal(xRange)
        Else
            xScale = 1
        End If
        If yRange <> 0 Then
            yScale = Convert.ToDecimal(yAxisLength) / Convert.ToDecimal(yRange)
        Else
            yScale = 1
        End If

        'width of cutoff rectangles to draw on screen
        Dim minCutOffLength As Integer = Convert.ToInt32(Decimal.Round(Convert.ToDecimal(histMaxMin(1)) * xScale, 0))
        Dim maxCutOffLength As Integer = Convert.ToInt32(Decimal.Round(Convert.ToDecimal(histogram.GetLength(0) - histMaxMin(0)) * xScale, 0))

        Dim cutoffText As String
        If mainForm.histogramCutoff = 50 Then
            cutoffText = "49.5"
        Else
            cutoffText = mainForm.histogramCutoff.ToString
        End If
        'render cutoff rectangles
        Dim fillBrush As New SolidBrush(Color.FromArgb(55, 55, 55))
        renderGraphics.FillRectangle(fillBrush, Loffset, Toffset - 1, minCutOffLength, yAxisLength)
        renderGraphics.FillRectangle(fillBrush, xOrigin + xAxisLength - maxCutOffLength, Toffset - 1, maxCutOffLength - 1, yAxisLength)

        If mainForm.histogramCutoff <> 0 And mainForm.histogramToShow = 1 Then
            renderGraphics.DrawString(cutoffText & "% end cut-offs applied", axisFont, textBrush, renderPanel.Width - 10, Toffset / 2 + axisFont.Height / 2, sfRight)
        End If

        'render plot axis and labels
        renderGraphics.DrawLine(axisPen, Loffset, Toffset - 1, Loffset, renderPanel.Height - Boffset)
        renderGraphics.DrawLine(axisPen, Loffset, renderPanel.Height - Boffset, renderPanel.Width - Roffset + 1, renderPanel.Height - Boffset)
        renderGraphics.DrawString("Intensity Digital Numbers (Cumulative %)", axisFont, textBrush, xAxisLength / 2 + Loffset, renderPanel.Height - Boffset / 4, sfCentre)
        renderGraphics.DrawString("Count", axisFont, textBrush, Loffset / 2, Toffset / 2, sfCentre)
        renderGraphics.DrawString("0", axisFont, textBrush, xOrigin - 10, yorigin + 10, sfCentre)

        'renderGraphics.RotateTransform(-90)
        'renderGraphics.TranslateTransform(-1 * (yAxisLength / 2 + Toffset), Loffset / 4)
        'renderGraphics.DrawString("Count", axisFont, Brushes.White, 0, 0, string_format1)
        'renderGraphics.ResetTransform()
        If Not renderBlank Then


            Dim samplePointsX() As Integer = {0}
            Dim samplePointsY() As Integer = {0}
            Dim percentages(histogram.GetLength(0) - 1) As Single
            Dim cumulativeTotal As Integer = 0

            'get actual sample data from the intensity measurements reported in the dataset(s)
            For i As Integer = 0 To histogram.Count - 1
                cumulativeTotal += histogram(i)
                percentages(i) = Convert.ToSingle(cumulativeTotal) / Convert.ToSingle(totalPoints) * Convert.ToSingle(100I)
                If histogram(i) <> 0 Then
                    ReDim Preserve samplePointsX(samplePointsX.GetLength(0))
                    ReDim Preserve samplePointsY(samplePointsY.GetLength(0))
                    samplePointsX(samplePointsX.GetLength(0) - 1) = i
                    samplePointsY(samplePointsY.GetLength(0) - 1) = histogram(i)
                End If
            Next

            'fill in the points (at single intensity value increments) between the sample data points via linear interpolation (this is used for creating a smooth colour ramp for the lines drawn next) 
            Dim interpolPointsX() As Integer = {0}
            Dim interpolPointsY() As Integer = {0}
            For i As Integer = 0 To samplePointsX.GetLength(0) - 2
                Dim incXRange As Integer = samplePointsX(i + 1) - samplePointsX(i)
                Dim incYRange As Integer = samplePointsY(i + 1) - samplePointsY(i)
                For j As Integer = 0 To incXRange - 2
                    Dim interpolX As Integer = samplePointsX(i) + j
                    Dim interpolY As Integer = samplePointsY(i) + Convert.ToInt32(Decimal.Round((Convert.ToDecimal(incYRange) / Convert.ToDecimal(incXRange) * Convert.ToDecimal(j)), 0))
                    ReDim Preserve interpolPointsX(interpolPointsX.GetLength(0))
                    ReDim Preserve interpolPointsY(interpolPointsY.GetLength(0))
                    interpolPointsX(interpolPointsX.GetLength(0) - 1) = interpolX
                    interpolPointsY(interpolPointsY.GetLength(0) - 1) = interpolY

                    If interpolX <= histMaxMin(1) Or interpolX >= histMaxMin(0) Then
                        colourBrush.Color = Color.FromArgb(60, 60, 60)
                    Else
                        colourBrush.Color = mainForm.calcIntensityColour(interpolX, histMaxMin)
                    End If
                    renderGraphics.FillRectangle(colourBrush, xOrigin + Convert.ToInt32(Decimal.Round(interpolX * xScale, 0)) - 1, yorigin - Convert.ToInt32(Decimal.Round(interpolY * yScale, 0)) - 2, 2, 2)
                Next
            Next

            'drawn the smoothly changing colour ramped lines that will correspond to the colours seen in the main display
            For i As Integer = 0 To interpolPointsX.GetLength(0) - 2
                If interpolPointsX(i) <= histMaxMin(1) Or interpolPointsX(i) >= histMaxMin(0) Then
                    colourPen.Color = Color.FromArgb(60, 60, 60)
                Else
                    colourPen.Color = mainForm.calcIntensityColour(interpolPointsX(i), histMaxMin)
                End If
                renderGraphics.DrawLine(colourPen, xOrigin + Convert.ToInt32(Decimal.Round(interpolPointsX(i) * xScale, 0)), yorigin - Convert.ToInt32(Decimal.Round(interpolPointsY(i) * yScale, 0)) - 2, xOrigin + Convert.ToInt32(Decimal.Round(interpolPointsX(i + 1) * xScale, 0)), yorigin - Convert.ToInt32(Decimal.Round(interpolPointsY(i + 1) * yScale, 0)) - 2)
            Next

            Dim percentFont As New Font("arial", 11, FontStyle.Regular, GraphicsUnit.Pixel)
            For i As Integer = 1 To numInc
                Try
                    Dim index As Integer = i * iInc - 3
                    Dim percent As Single = Math.Round(percentages(index), 3)
                    renderGraphics.DrawLine(axisPen, xOrigin + i * xInc, yorigin, xOrigin + i * xInc, yorigin + xTickLength)
                    renderGraphics.DrawLine(axisPen, xOrigin - yTickLength, yorigin - i * yInc, xOrigin, yorigin - i * yInc)
                    renderGraphics.DrawString((i * iInc - 3).ToString, axisFont, textBrush, xOrigin + i * xInc, yorigin + xTickLength + axisFont.Height / 2 + 2, sfCentre)
                    renderGraphics.DrawString("(" & percent.ToString & "%)", percentFont, textBrush, xOrigin + i * xInc, yorigin + xTickLength + axisFont.Height + 8, sfCentre)
                    renderGraphics.DrawString((i * cInc).ToString, axisFont, textBrush, xOrigin - yTickLength - axisFont.Height / 2 - 2, yorigin - i * yInc + axisFont.Height / 2, sfRight)
                Catch ex As Exception
                End Try
            Next
        End If

        renderPanel.BackgroundImage = renderBitmap
        renderPanel.Refresh()
    End Sub

    Private Sub histogramForm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        renderPanel.Width = Me.Width - 14
        renderPanel.Height = Me.Height - 33
        Dim buttonPoint As New Point(refreshButton.Location.X, renderPanel.Height - 36)
        refreshButton.Location = buttonPoint
        If beenLoaded Then
            renderHistogram()
        End If
    End Sub

    Private Sub refreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles refreshButton.Click
        If mainForm.histogramToShow <> 1 And mainForm.highlightedPoints = 0 Then
            mainForm.histogramToShow = 1
        End If
        renderHistogram()
    End Sub
End Class