'''''ATLAS'''''
'By: Ryan Brazeal
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

Imports System.IO
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Threading

Public Class mainForm

#Region "Global Variables"
    Dim WithEvents lasShell As New libLAS           'wrapper object used for reading and writing LAS files
    Dim WithEvents lasShell2 As New libLAS          'wrapper object used as an intermediary for writing TXT to e57 files
    Dim WithEvents e57Shell As New libe57           'wrapper object used for writing e57 files
    Dim WithEvents cl3Shell As New libTopconCL3     'wrapper object used for reading Topcon CL3 files
    Dim WithEvents clrShell As New libTopconCLR     'wrapper object used for reading Topcon CLR files
    Dim datasetFileName As String
    Friend transformParameters(9, 6) As Decimal    '10 rows by 7 columns that contain the seven transformation parameters of each of the 10 datasets
    Friend transformParametersStdDev(9, 6) As Decimal   '10 rows by 7 columns contains the standard deviations for the seven transformation parameters for each of the 10 datasets
    Friend dataset1MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset2MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset3MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset4MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset5MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset6MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset7MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset8MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset9MapDataPoints As New Generic.List(Of cloudPoint)
    Friend dataset10MapDataPoints As New Generic.List(Of cloudPoint)
    Friend currentMapDataPoints As New Generic.List(Of cloudPoint)
    Dim totalNumPoints As Integer = 0
    Dim globalLines As New Generic.List(Of cloudLine)
    Dim currentLines As New Generic.List(Of cloudLine)
    Dim globalMaxMinValues(,) As Double = {{-100000000, 100000000}, {-100000000, 100000000}, {-100000000, 100000000}}
    Dim globalCentreValue() As Double = {0, 0, 0} 'possibly delete
    Dim currentMaxMinValues(,) As Double = {{-100000000, 100000000}, {-100000000, 100000000}, {-100000000, 100000000}} 'possibly delete
    Dim currentCentreValue() As Double = {0, 0, 0}
    Dim numDatasetsInteger As Integer = 0
    Dim datasetToUse As Integer = 0
    Dim mapDisplayed As Boolean = False
    Dim WithEvents zoomTrackBar As TrackBar
    Dim adjustmentLevel As Double
    Dim centreOfMouseMap(1) As Double
    Dim fromHerePan(1) As Double
    Dim fromHereSlide(1) As Double
    Dim fromHerePt(1) As Double
    Dim toHerePan(1) As Double
    Dim fromHereBox(1) As Integer
    Dim toHereBox(1) As Integer
    Friend dataset1Calcd As Boolean = False
    Friend dataset2Calcd As Boolean = False
    Friend dataset3Calcd As Boolean = False
    Friend dataset4Calcd As Boolean = False
    Friend dataset5Calcd As Boolean = False
    Friend dataset6Calcd As Boolean = False
    Friend dataset7Calcd As Boolean = False
    Friend dataset8Calcd As Boolean = False
    Friend dataset9Calcd As Boolean = False
    Friend dataset10Calcd As Boolean = False
    Dim currentdatasetCalcd As Boolean = False
    Dim inFormLoad As Boolean
    Dim mapBitmap As Bitmap
    Dim mapGraphics As Graphics
    Dim referenceBitmap As Bitmap
    'Dim fileBitmap As Bitmap
    'Dim fileGraphics As Graphics
    Dim isMouseButtonDown As Boolean = False
    Dim currentRightClickDataset As Integer = 0
    Dim dataset1LineWeight As Integer
    Dim dataset2LineWeight As Integer
    Dim dataset3LineWeight As Integer
    Dim dataset4LineWeight As Integer
    Dim dataset5LineWeight As Integer
    Dim dataset6LineWeight As Integer
    Dim dataset7LineWeight As Integer
    Dim dataset8LineWeight As Integer
    Dim dataset9LineWeight As Integer
    Dim dataset10LineWeight As Integer
    Dim targetsLineWeight As Integer
    Dim pickFromPt As Boolean = False
    Dim pickToPt As Boolean = False
    Dim datasetError As Boolean
    Dim currentMapCursor As Cursor
    Dim fileScale As Integer
    Dim pointsInView As Integer
    Friend intensityHistogram(0) As Integer     'global histogram for ALL points read into the application
    Friend selectedHistogram(0) As Integer      'histogram for the selected range of points on the screen from ALL points
    Friend currentViewHistogram(0) As Integer   'histogram for the selected range of points ONLY using the current render settings (i.e. affected by reduced points % option)
    Friend intensityHistMaxMin() As Integer = {0, 0}
    Friend selectedHistMaxMin() As Integer = {0, 0}
    Friend currentViewHistMaxMin() As Integer = {0, 0}
    Friend histogramCutoff As Integer = 0
    Friend histogramToShow As Integer
    Friend processingOption As Integer = 1
    Dim redrawTest As Boolean = True
    Dim insideDrawBox As Boolean = False
    Dim testForHighlights As Boolean = False
    Friend highlightedPoints As Integer = 0
    Dim camera As New Matrix(3, 1)
    Dim target As New Matrix(3, 1)
    Dim initialCamera As New Matrix(3, 1)
    Dim initialTarget As New Matrix(3, 1)
    Dim zoomScale As Decimal
    Dim activeRender As Boolean = True
    Dim dir2target As New Matrix(3, 1)
    Dim imagePlaneX As New Matrix(3, 1)
    Dim imagePlaneY As New Matrix(3, 1)
    Dim imagePlaneXYsolved As Boolean
    Dim initialScaleFactor As Double = 1
    Dim globalScaleFactor As Double = 1
    Dim currentScalefactor As Double = 1
    Dim insidePickCamera As Boolean = False
    Dim insidePickTarget As Boolean = False
    Dim insidePickRegTarget As Boolean = False
    Dim FOV As Double = 120
    Dim cutOffAngle As Double = 130
    Dim focalLength As Decimal
    Dim imageOffsets(1) As Integer
    Dim detailLevel As Integer
    Dim targetBasePos As New Matrix(3, 1)
    Dim cameraBasePos As New Matrix(3, 1)
    Dim cameraBaseX As New Matrix(3, 1)
    Dim cameraBaseY As New Matrix(3, 1)
    Dim cameraBasePos2 As New Matrix(3, 1)
    Dim cameraBaseX2 As New Matrix(3, 1)
    Dim cameraBaseY2 As New Matrix(3, 1)
    Dim initialLineWeights(9) As Integer
    Dim upDirection As New Matrix(3, 1, True)
    Dim renderParallelBoundingBox As Boolean = False
    Friend histogramDisplayed As Boolean = False
    Dim insideNavigate As Boolean = False
    Dim maxPointsToRenderWhenNavigating As Integer = 1500
    Dim navigatePointSize As Integer = 2    'Options are 1,2,4,8,16
    Dim planeNormal As New Matrix(3, 1)
    Dim pointOnPlane As New Matrix(3, 1)
    Dim targetCircles As New Generic.List(Of cloudCircle)
    Dim targetsFound As Boolean = False
    Dim panScalefactor As Decimal
    Dim targetSolution As New Matrix(3, 1)
    Dim targetRadius As Decimal
    Dim manualTargetRadius As Double = 0.1
    Dim GRcolumn1 As DataGridViewCheckBoxColumn
    Dim GRcolumn2 As DataGridViewTextBoxColumn
    Dim GRcolumn3 As DataGridViewComboBoxColumn
    Dim GRcolumn4 As DataGridViewComboBoxColumn
    Dim GRcolumn5 As DataGridViewComboBoxColumn
    Dim maxTiePoints As Integer = 0
    Dim maxControlPoints As Integer = 0
    Dim maxTargetPoints As Integer = 0
    Friend maxIterations As Integer = 10
    Friend xCol, yCol, zCol, rCol, gCol, bCol, iCol As Integer
    Friend tFFContinue As Boolean
    Friend headerRow As Boolean
    Friend delimiter As Char

    Const heightOffset As Integer = 1014
    Const widthOffset As Integer = 1280

    'delegate and invoke for safe thread calls to update the status and percentage label controls
    Private Delegate Sub updateStatusCallback(ByVal message As String)
    Private Delegate Sub updatePercentageCallback(ByVal message As String)

#End Region

    'safe thread call to update the statuslabel control message
    Private Sub updateStatus(ByVal message As String)
        Try
            If statusLabel.InvokeRequired Then
                Dim d As New updateStatusCallback(AddressOf updateStatus)
                Invoke(d, New Object() {message})
            Else
                statusLabel.Text = message
                statusLabel.Refresh()
            End If
        Catch ex As Exception
            'nothing to do here
        End Try
    End Sub

    'safe thread call to update the statuslabel control message
    Private Sub updatePercentage(ByVal message As String)
        Try
            If percentageLabel.InvokeRequired Then
                Dim d As New updateStatusCallback(AddressOf updatePercentage)
                Invoke(d, New Object() {message})
            Else
                percentageLabel.Text = message
                percentageLabel.Refresh()
            End If
        Catch ex As Exception
            'nothing to do here
        End Try
    End Sub

    'procedure that sets up initial states for certain controls
    Private Sub mainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
        inFormLoad = True
        dataset1CheckBox.Checked = True
        dataset2CheckBox.Checked = True
        dataset3CheckBox.Checked = True
        dataset4CheckBox.Checked = True
        dataset5CheckBox.Checked = True
        dataset6CheckBox.Checked = True
        dataset7CheckBox.Checked = True
        dataset8CheckBox.Checked = True
        dataset9CheckBox.Checked = True
        dataset10CheckBox.Checked = True

        zoomTrackBar = New TrackBar
        zoomTrackBar.Orientation = Orientation.Vertical
        zoomTrackBar.RightToLeftLayout = True
        zoomTrackBar.Height = 115
        zoomTrackBar.Width = 20
        Dim refpoint As System.Drawing.Point
        refpoint.X = 1212
        refpoint.Y = 30
        zoomTrackBar.Location() = refpoint
        Me.Controls.Add(zoomTrackBar)
        zoomTrackBar.BringToFront()
        zoomInButton.BringToFront()
        zoomOutButton.BringToFront()
        zoomTrackBar.Cursor = Cursors.Arrow
        zoomTrackBar.Maximum = 10
        zoomTrackBar.Minimum = 2
        zoomTrackBar.Value = 6
        zoomTrackBar.TabIndex = 13
        zoomTrackBar.BackColor = Color.FromArgb(40, 40, 40)
        adjustmentLevel = 6.0

        ToolTip1.SetToolTip(zoomTrackBar, "Current Mouse Wheel Zoom Level = 60%")

        mapPanel.Height = 768

        mapBitmap = New Bitmap(mapPanel.Width, mapPanel.Height, Imaging.PixelFormat.Format24bppRgb)
        mapBitmap.SetResolution(300, 300)
        mapGraphics = Graphics.FromImage(mapBitmap)
        mapGraphics.InterpolationMode = Drawing2D.InterpolationMode.Default
        mapGraphics.SmoothingMode = Drawing2D.SmoothingMode.Default
        mapGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        mapGraphics.Clear(Color.FromArgb(40, 40, 40))

        'fileScale = 1
        'fileBitmap = New Bitmap(mapPanel.Width * fileScale, mapPanel.Height * fileScale, Imaging.PixelFormat.Format24bppRgb)
        'fileBitmap.SetResolution(300 * Convert.ToSingle(fileScale), 300 * Convert.ToSingle(fileScale))
        'fileGraphics = Graphics.FromImage(fileBitmap)
        'fileGraphics.InterpolationMode = Drawing2D.InterpolationMode.Default
        'fileGraphics.SmoothingMode = SmoothingMode.Default
        'fileGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        'fileGraphics.Clear(Color.FromArgb(40, 40, 40))

        ToolTip1.SetToolTip(dataset1ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset2ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset3ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset4ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset5ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset6ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset7ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset8ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset9ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")
        ToolTip1.SetToolTip(dataset10ColorLabel, "Click to change this dataset's point colour" & vbNewLine & "Right-click to change this dataset's point size")

        currentMapCursor = Cursors.Cross
        currentCentreValue(0) = 0
        currentCentreValue(1) = 0
        currentCentreValue(2) = 0

        colourComboBox.SelectedIndex = 0
        reducePtsComboBox.SelectedIndex = 5
        detailLevel = reducePtsComboBox.SelectedIndex
        histogramCutOffsComboBox.SelectedIndex = 0
        pointsInView = 0
        perspectiveRadioButton.Checked = True
        statusLabel.Text = String.Empty
        zoomScale = 1

        camera.data(1, 1) = 0
        camera.data(2, 1) = 0
        camera.data(3, 1) = 0

        target.data(1, 1) = 0
        target.data(2, 1) = 0
        target.data(3, 1) = 0

        imagePlaneXYsolved = False
        focalLength = mapPanel.Width / 1000 / (Math.Tan(FOV / 2 * Math.PI / 180D))

        upDirection.data(1, 1) = 0
        upDirection.data(2, 1) = 0
        upDirection.data(3, 1) = 1

        targetsLineWeight = 2

        drawCentrePoint(mapGraphics, Color.LightGray)
        mapPanel.BackgroundImage = mapBitmap
        mapPanel.Refresh()

        GRcolumn1 = New DataGridViewCheckBoxColumn
        GRcolumn2 = New DataGridViewTextBoxColumn
        GRcolumn3 = New DataGridViewComboBoxColumn
        GRcolumn4 = New DataGridViewComboBoxColumn
        GRcolumn5 = New DataGridViewComboBoxColumn

        GRcolumn1.HeaderText = "Use"
        GRcolumn2.HeaderText = "Tie Point #"
        GRcolumn3.HeaderText = "Apply to Dataset"
        GRcolumn4.HeaderText = "Registration Target #"
        GRcolumn5.HeaderText = "Control Point ID"

        GRcolumn1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        GRcolumn2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        GRcolumn2.ReadOnly = True
        GRcolumn3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        GRcolumn3.ReadOnly = False
        GRcolumn4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        GRcolumn3.ReadOnly = False
        GRcolumn5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        GRcolumn3.ReadOnly = False

        GRcolumn1.Width = 40
        GRcolumn2.Width = 65
        GRcolumn3.Width = 150
        GRcolumn4.Width = 70
        GRcolumn5.Width = 70

        geoRefDataGridView.Columns.Add(GRcolumn1)
        geoRefDataGridView.Columns.Add(GRcolumn2)
        geoRefDataGridView.Columns.Add(GRcolumn3)
        geoRefDataGridView.Columns.Add(GRcolumn4)
        geoRefDataGridView.Columns.Add(GRcolumn5)

        ToolTip1.SetToolTip(camUpButton, "Rotate Camera by 15" & Chr(176) & " upwards")
        ToolTip1.SetToolTip(camDownButton, "Rotate Camera by 15" & Chr(176) & " downwards")
        ToolTip1.SetToolTip(camLeftButton, "Rotate Camera by 15" & Chr(176) & " left")
        ToolTip1.SetToolTip(camRightButton, "Rotate Camera by 15" & Chr(176) & " right")
        ToolTip1.SetToolTip(tarUpButton, "Rotate Aiming Target by 15" & Chr(176) & " upwards")
        ToolTip1.SetToolTip(tarDownButton, "Rotate Aiming Target by 15" & Chr(176) & " downwards")
        ToolTip1.SetToolTip(tarLeftButton, "Rotate Aiming Target by 15" & Chr(176) & " left")
        ToolTip1.SetToolTip(tarRightButton, "Rotate Aiming Target by 15" & Chr(176) & " right")

        Me.Height = Me.MinimumSize.Height
        inFormLoad = False

    End Sub

    Private Sub drawCentrePoint(ByRef selectedGraphics As Graphics, ByVal renderColor As Color)
        'plot centre point of display as cross
        Dim myBrush2 As New SolidBrush(renderColor)
        Dim ScreenBasePt(1) As Integer
        ScreenBasePt(0) = Convert.ToInt32(mapPanel.Width / 2)
        ScreenBasePt(1) = Convert.ToInt32(mapPanel.Height / 2)

        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) + 1, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) + 2, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) - 1, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) - 2, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) + 1, 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) + 2, 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) - 1, 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) - 2, 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) + 3, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) + 4, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) - 3, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0) - 4, ScreenBasePt(1), 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) + 3, 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) + 4, 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) - 3, 1, 1)
        selectedGraphics.FillRectangle(myBrush2, ScreenBasePt(0), ScreenBasePt(1) - 4, 1, 1)

        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) + 1) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) + 2) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) - 1) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) - 2) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) + 1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) + 2) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) - 1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) - 2) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) + 3) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) + 4) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) - 3) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, (ScreenBasePt(0) - 4) * fileScale, ScreenBasePt(1) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) + 3) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) + 4) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) - 3) * fileScale, fileScale, fileScale)
        'fileGraphics.FillRectangle(myBrush2, ScreenBasePt(0) * fileScale, (ScreenBasePt(1) - 4) * fileScale, fileScale, fileScale)
    End Sub
    'procedure for changing the colour of each loaded Map dataset
    Private Sub dataset1ColorLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset1ColorLabel.Click, dataset2ColorLabel.Click, dataset3ColorLabel.Click, dataset4ColorLabel.Click, dataset5ColorLabel.Click, dataset6ColorLabel.Click, dataset7ColorLabel.Click, dataset8ColorLabel.Click, dataset9ColorLabel.Click, dataset10ColorLabel.Click, processedTargetsColorLabel.Click
        Dim senderLabel As Label = CType(sender, Label)
        Dim initialColor As Color = senderLabel.BackColor
        ColorDialog1.Color = senderLabel.BackColor
        Dim colorDialogResult As DialogResult = ColorDialog1.ShowDialog

        If colorDialogResult <> Windows.Forms.DialogResult.Cancel Then
            senderLabel.BackColor = ColorDialog1.Color
            If senderLabel.BackColor <> initialColor Then
                If senderLabel.Name.ToUpper = "processedTargetsColorLabel".ToUpper Then
                    If targetsFound Then
                        drawMapButton_Click(refreshMapButton, e)
                    End If
                Else
                    drawMapButton_Click(refreshMapButton, e)
                End If
            End If
        End If
    End Sub

    'procedure to change Tooltip message and update adjustmentLevel variable
    Private Sub zoomTrackBar_Adjusted(ByVal sender As Object, ByVal e As EventArgs) Handles zoomTrackBar.ValueChanged
        Dim newValue As Integer = zoomTrackBar.Value
        adjustmentLevel = Convert.ToDouble(newValue)
        ToolTip1.SetToolTip(zoomTrackBar, "Current Mouse Wheel Zoom Level = " & (Convert.ToInt32(newValue * 10)).ToString & "%")
    End Sub

    'Procedure on the main thread that instructs the backgroundWorker control to start loading the map datasets from the file
    Private Sub openButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles openButton.Click, LoadDatasetToolStripMenuItem.Click

        If numDatasetsInteger < 10 Then

            OpenFileDialog1.FileName = ""
            Dim openFileDialogResult As DialogResult = OpenFileDialog1.ShowDialog

            If openFileDialogResult <> Windows.Forms.DialogResult.Cancel Then
                datasetFileName = OpenFileDialog1.SafeFileName
                If OpenFileDialog1.FilterIndex = 1 Then 'TXT file input
                    textFormatForm.columnsGroupBox.Enabled = True
                    textFormatForm.ShowDialog()
                    If tFFContinue Then
                        configureWorkerAndStartRead()
                    End If
                ElseIf OpenFileDialog1.FilterIndex = 2 Then    'LAS file input
                    lasShell.InputFile = ControlChars.Quote & OpenFileDialog1.FileName & ControlChars.Quote
                    lasShell.OutputFile = ControlChars.Quote & Application.StartupPath & "\pointcloud.txt" & ControlChars.Quote
                    'TXT file format will be X coord, Y coord, Z coord, Red, Green, Blue, Intensity per line for each point cloud data point, view the las2txt.exe help file for more details on the different options
                    lasShell.Options = "--parse xyzRGBi --delimiter "" """  'change the command line processing arguments to suit your needs
                    lasShell.TimerInterval = 100       'timer interval used to check on the status of the processing, 100 = every 0.1 secs a check is performed
                    'start the decoding process
                    If lasShell.StartDecoding() Then
                        statusLabel.Text = "Decoding LAS file"
                        OpenFileDialog1.FileName = Application.StartupPath & "\pointcloud.txt"
                        xCol = 1
                        yCol = 2
                        zCol = 3
                        rCol = 4
                        gCol = 5
                        bCol = 6
                        iCol = 7
                        headerRow = False
                        delimiter = " "
                    Else
                        statusLabel.Text = "LAS decoding failed to start"
                    End If
                ElseIf OpenFileDialog1.FilterIndex = 3 Then    'CL3 file input
                    cl3Shell.InputFile = ControlChars.Quote & OpenFileDialog1.FileName & ControlChars.Quote    'replace this path with the filename property from an OpenFileDialog
                    cl3Shell.OutputFile = ControlChars.Quote & Application.StartupPath & "\pointcloud.txt" & ControlChars.Quote
                    cl3Shell.TimerInterval = 100       'timer interval used to check on the status of the processing, 100 = every 0.1 secs a check is performed
                    'start the decoding process
                    If cl3Shell.StartDecoding() Then
                        statusLabel.Text = "Decoding CL3 file"
                        OpenFileDialog1.FileName = Application.StartupPath & "\pointcloud.txt"
                        xCol = 1
                        yCol = 2
                        zCol = 3
                        rCol = 4
                        gCol = 5
                        bCol = 6
                        iCol = 7
                        headerRow = False
                        delimiter = " "
                    Else
                        statusLabel.Text = "CL3 decoding failed to start"
                    End If
                ElseIf OpenFileDialog1.FilterIndex = 4 Then    'CLR file input
                    clrShell.InputFile = ControlChars.Quote & OpenFileDialog1.FileName & ControlChars.Quote    'replace this path with the filename property from an OpenFileDialog
                    clrShell.OutputFile = ControlChars.Quote & Application.StartupPath & "\pointcloud.txt" & ControlChars.Quote
                    clrShell.TimerInterval = 100       'timer interval used to check on the status of the processing, 100 = every 0.1 secs a check is performed
                    'start the decoding process
                    If clrShell.StartDecoding() Then
                        statusLabel.Text = "Decoding CLR file"
                        OpenFileDialog1.FileName = Application.StartupPath & "\pointcloud.txt"
                        xCol = 1
                        yCol = 2
                        zCol = 3
                        rCol = 4
                        gCol = 5
                        bCol = 6
                        iCol = 7
                        headerRow = False
                        delimiter = " "
                    Else
                        statusLabel.Text = "CLR decoding failed to start"
                    End If
                ElseIf OpenFileDialog1.FilterIndex = 5 Then 'PLY file input

                    Dim swapAxisResult As DialogResult = MessageBox.Show("A common issue with .PLY files is the definition of the Y and Z axes, do you want to swap the Y and Z axes (recommended)?", "POSSIBLE VISUALIZATION PROBLEM", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                    statusLabel.Text = "Decoding PLY (ASCII) file"
                    Dim ply As New StreamReader(OpenFileDialog1.FileName)  'original ply file to read from
                    Try
                        Dim type As String = ply.ReadLine
                        Dim format As String = ply.ReadLine

                        'check to ensure first header line contains PLY and second header line contains FORMAT ASCII
                        If type.ToUpper.Contains("PLY") And format.ToUpper.Contains("FORMAT ASCII") Then

                            Dim foundEndHeader As Boolean = False
                            Dim numVertex As Integer = 0
                            Dim elementCount As Integer = 0
                            Dim propertyCount As Integer
                            Dim xPos As Integer = -1
                            Dim yPos As Integer = -1
                            Dim zPos As Integer = -1
                            Dim iPos As Integer = -1
                            Dim cPos As Integer = -1
                            Dim rPos As Integer = -1
                            Dim gPos As Integer = -1
                            Dim bPos As Integer = -1
                            Dim vertexPos As Integer = -1
                            Dim elementQuantities(0) As Integer
                            Dim lastElementWasVertex As Boolean = False

                            Do Until ply.Peek = -1
                                Dim currentline As String = ply.ReadLine
                                Dim delimiter(0) As Char
                                delimiter(0) = " "
                                Dim values() As String = currentline.Split(delimiter)

                                Select Case values(0).ToUpper
                                    Case "END_HEADER"
                                        foundEndHeader = True
                                        Exit Do
                                    Case "ELEMENT"
                                        ReDim Preserve elementQuantities(elementCount)
                                        elementQuantities(elementCount) = Integer.Parse(values(2))
                                        propertyCount = 1

                                        If values(1).ToUpper = "VERTEX" Then
                                            vertexPos = elementCount
                                            lastElementWasVertex = True
                                            numVertex = Integer.Parse(values(2))
                                        Else
                                            lastElementWasVertex = False
                                        End If
                                        elementCount += 1
                                    Case "PROPERTY"
                                        If lastElementWasVertex = True Then
                                            If values(2).ToUpper = "RED" Then
                                                rPos = propertyCount
                                            ElseIf values(2).ToUpper = "GREEN" Then
                                                gPos = propertyCount
                                            ElseIf values(2).ToUpper = "BLUE" Then
                                                bPos = propertyCount
                                            ElseIf values(2).ToUpper = "INTENSITY" Then
                                                iPos = propertyCount
                                            ElseIf values(2).ToUpper = "CONFIDENCE" Then
                                                cPos = propertyCount
                                            ElseIf values(2).ToUpper = "X" Then
                                                xPos = propertyCount
                                            ElseIf values(2).ToUpper = "Y" Then
                                                yPos = propertyCount
                                            ElseIf values(2).ToUpper = "Z" Then
                                                zPos = propertyCount
                                            End If
                                            propertyCount += 1
                                        End If
                                End Select
                            Loop

                            'check to see if an end_header was read as well as if vertex exists to be read along with the vertex x,y,z positions were determined
                            If foundEndHeader = True And numVertex <> 0 And xPos <> -1 And yPos <> -1 And zPos <> -1 Then
                                If iPos = -1 And cPos <> -1 Then
                                    iPos = cPos
                                ElseIf iPos <> -1 And cPos <> -1 Then
                                    Dim colourResult As DialogResult = MessageBox.Show("Intensity values and confidence values have been detected for the vertex data." & ControlChars.NewLine & "Do you want to use intensity data (YES) or confidence data (NO) to colour the point cloud?", "COLOUR OPTIONS", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                                    If colourResult = Windows.Forms.DialogResult.No Then
                                        iPos = cPos
                                    End If
                                End If

                                Dim txtPC As New StreamWriter(Application.StartupPath & "\pointcloud.txt")   'exported txt point cloud file containing only vertice information
                                Dim currentline As String

                                For i = 0 To elementQuantities.GetLength(0) - 1
                                    For j = 1 To elementQuantities(i)
                                        currentline = ply.ReadLine

                                        If i = vertexPos Then
                                            Dim delimiter(0) As Char
                                            delimiter(0) = " "
                                            Dim values() As String = currentline.Split(delimiter)
                                            Dim newLine As String = ""

                                            newLine = values(xPos - 1) & " " & values(yPos - 1) & " " & values(zPos - 1)

                                            If swapAxisResult = Windows.Forms.DialogResult.Yes Then
                                                'newLine = values(xPos - 1) & " " & values(zPos - 1) & " " & (-1 * Double.Parse(values(yPos - 1))).ToString
                                                newLine = values(xPos - 1) & " " & (-1 * Double.Parse(values(zPos - 1))).ToString & " " & values(yPos - 1)
                                            End If

                                            If rPos <> -1 Then
                                                newLine &= (" " & values(rPos - 1))
                                            End If
                                            If gPos <> -1 Then
                                                newLine &= (" " & values(gPos - 1))
                                            End If
                                            If bPos <> -1 Then
                                                newLine &= (" " & values(bPos - 1))
                                            End If
                                            If iPos <> -1 Then
                                                newLine &= (" " & Convert.ToInt32(Math.Round((Double.Parse(values(iPos - 1)) * 1000), 0)).ToString)
                                            End If
                                            txtPC.WriteLine(newLine)
                                        End If
                                    Next
                                Next
                                txtPC.Close()
                            End If

                            xCol = 1
                            yCol = 2
                            zCol = 3
                            rCol = -1
                            gCol = -1
                            bCol = -1
                            iCol = -1

                            Dim increment As Integer = 1
                            If rPos <> -1 Then
                                rCol = 3 + increment
                                increment += 1
                            End If
                            If gPos <> -1 Then
                                gCol = 3 + increment
                                increment += 1
                            End If
                            If bPos <> -1 Then
                                bCol = 3 + increment
                                increment += 1
                            End If
                            If iPos <> -1 Then
                                iCol = 3 + increment
                                increment += 1
                            End If

                            headerRow = False
                            delimiter = " "
                            OpenFileDialog1.FileName = Application.StartupPath & "\pointcloud.txt"

                            ply.Close()
                            configureWorkerAndStartRead()
                        End If
                    Catch ex As Exception
                        ply.Close()
                        statusLabel.Text = "PLY (ASCII) decoding failed, invalid file format"
                    End Try
                End If
            End If
        Else
            MessageBox.Show("You can only load a maximum of 10 datasets!", "TOO MANY DATASETS", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub lasShell_ErrorOccurred(ByVal ErrorType As String) Handles lasShell.ErrorOccurred
        MessageBox.Show(ErrorType, "libLAS PROCESSING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    'unorthodox way of sort of handling a DONE type event of lasShell 
    'But it works at avoiding cross-threading calls to the forms controls (i.e., the label1 control in this example)
    Private Sub lasShell_Tick() Handles lasShell.Tick
        If lasShell.isRunning = False Then
            lasShell.TimerStop()
            'safe to read the LAS decoded TXT file into your application now (done in the background)
            If lasShell.decoding = True Then
                statusLabel.Text = "LAS Decoding Completed in " & lasShell.processingTime.ToString("f1") & " sec"
                statusLabel.Refresh()
                System.Threading.Thread.Sleep(3000)
                configureWorkerAndStartRead() 'starts existing background worker to read in the decoded txt file
            Else
                statusLabel.Text = "LAS Encoding Completed in " & lasShell.processingTime.ToString("f1") & " sec"
                statusLabel.Refresh()
                MessageBox.Show("Export complete", "RGBi", MessageBoxButtons.OK)
                updatePercentage("")
                If File.Exists(Application.StartupPath & "\pointcloud.txt") Then
                    File.Delete(Application.StartupPath & "\pointcloud.txt")
                End If
                statusLabel.Text = ""
            End If
        Else
            updatePercentage(lasShell.processingTime.ToString("f1") & " s")
        End If
    End Sub

    Private Sub configureWorkerAndStartRead()
        datasetToUse = 0
        For i As Integer = 1 To 10
            If dataset1MapDataPoints.Count = 0 Then
                datasetToUse = 1
                Exit For
            ElseIf dataset2MapDataPoints.Count = 0 Then
                datasetToUse = 2
                Exit For
            ElseIf dataset3MapDataPoints.Count = 0 Then
                datasetToUse = 3
                Exit For
            ElseIf dataset4MapDataPoints.Count = 0 Then
                datasetToUse = 4
                Exit For
            ElseIf dataset5MapDataPoints.Count = 0 Then
                datasetToUse = 5
                Exit For
            ElseIf dataset6MapDataPoints.Count = 0 Then
                datasetToUse = 6
                Exit For
            ElseIf dataset7MapDataPoints.Count = 0 Then
                datasetToUse = 7
                Exit For
            ElseIf dataset8MapDataPoints.Count = 0 Then
                datasetToUse = 8
                Exit For
            ElseIf dataset9MapDataPoints.Count = 0 Then
                datasetToUse = 9
                Exit For
            ElseIf dataset10MapDataPoints.Count = 0 Then
                datasetToUse = 10
                Exit For
            End If
        Next

        If datasetToUse = 1 Then
            dataset1MapDataPoints.Clear()
            dataset1Calcd = False
            dataset1LineWeight = 1
            dataset1CheckBox.Checked = True
        ElseIf datasetToUse = 2 Then
            dataset2MapDataPoints.Clear()
            dataset2Calcd = False
            dataset2LineWeight = 1
            dataset2CheckBox.Checked = True
        ElseIf datasetToUse = 3 Then
            dataset3MapDataPoints.Clear()
            dataset3Calcd = False
            dataset3LineWeight = 1
            dataset3CheckBox.Checked = True
        ElseIf datasetToUse = 4 Then
            dataset4MapDataPoints.Clear()
            dataset4Calcd = False
            dataset4LineWeight = 1
            dataset4CheckBox.Checked = True
        ElseIf datasetToUse = 5 Then
            dataset5MapDataPoints.Clear()
            dataset5Calcd = False
            dataset5LineWeight = 1
            dataset5CheckBox.Checked = True
        ElseIf datasetToUse = 6 Then
            dataset6MapDataPoints.Clear()
            dataset6Calcd = False
            dataset6LineWeight = 1
            dataset6CheckBox.Checked = True
        ElseIf datasetToUse = 7 Then
            dataset7MapDataPoints.Clear()
            dataset7Calcd = False
            dataset7LineWeight = 1
            dataset7CheckBox.Checked = True
        ElseIf datasetToUse = 8 Then
            dataset8MapDataPoints.Clear()
            dataset8Calcd = False
            dataset8LineWeight = 1
            dataset8CheckBox.Checked = True
        ElseIf datasetToUse = 9 Then
            dataset9MapDataPoints.Clear()
            dataset9Calcd = False
            dataset9LineWeight = 1
            dataset9CheckBox.Checked = True
        ElseIf datasetToUse = 10 Then
            dataset10MapDataPoints.Clear()
            dataset10Calcd = False
            dataset10LineWeight = 1
            dataset10CheckBox.Checked = True
        End If

        If backgroundWorker2.IsBusy <> True Then
            ' Start the asynchronous operation.
            Me.Cursor = Cursors.AppStarting
            If mapPanel.Cursor = Cursors.Cross Then
                currentMapCursor = Cursors.Cross
            Else
                currentMapCursor = Cursors.Hand
            End If
            mapPanel.Cursor = Cursors.AppStarting
            statusLabel.Text = "Reading in point cloud dataset"
            statusLabel.Refresh()
            backgroundWorker2.RunWorkerAsync()
        Else
            MessageBox.Show("The application is busy at the current moment so please wait until the previous process finishes", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub

    'Procedure for loading datasets into the application via the backgroundWorker control which is on a secondary thread
    Private Sub backgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker2.DoWork
        datasetError = False
        Dim pointfileStream As New StreamReader(OpenFileDialog1.FileName)

        Dim delimString(0) As Char
        delimString(0) = delimiter
        Dim header As Boolean = headerRow

        Dim pointString As String = String.Empty
        If header And pointfileStream.Peek <> -1 Then
            pointString = pointfileStream.ReadLine
        End If

        Dim dummyLine As String
        Dim fileCols As Integer
        Dim readNumPoints As Integer = 0
        While pointfileStream.Peek <> -1
            dummyLine = pointfileStream.ReadLine
            fileCols = dummyLine.Split(delimString).Length
            totalNumPoints += 1
        End While

        pointfileStream.Close()

        Dim planCol As Integer = 0
        If xCol <> -1 Then
            planCol += 1
        End If
        If yCol <> -1 Then
            planCol += 1
        End If
        If zCol <> -1 Then
            planCol += 1
        End If
        If iCol <> -1 Then
            planCol += 1
        End If
        If rCol <> -1 Then
            planCol += 1
        End If
        If gCol <> -1 Then
            planCol += 1
        End If
        If bCol <> -1 Then
            planCol += 1
        End If

        If planCol = fileCols Then
            pointfileStream = New StreamReader(OpenFileDialog1.FileName)

            Dim interval As Integer = totalNumPoints \ 100

            If header And pointfileStream.Peek <> -1 Then
                pointString = pointfileStream.ReadLine
            End If

            While pointfileStream.Peek <> -1
                If readNumPoints Mod interval = 0 AndAlso readNumPoints <> 0 Then
                    backgroundWorker2.ReportProgress((readNumPoints + 1) / totalNumPoints * 100)
                End If

                Dim elements() As String
                pointString = pointfileStream.ReadLine
                If pointString <> String.Empty Then

                    elements = pointString.Split(delimString)

                    Dim singleMapDataPoint As cloudPoint

                    Try
                        singleMapDataPoint.X = Double.Parse(elements(xCol - 1))
                    Catch ex As Exception
                        e.Cancel = True
                        Exit While
                    End Try

                    Try
                        singleMapDataPoint.Y = Double.Parse(elements(yCol - 1))
                    Catch ex As Exception
                        e.Cancel = True
                        Exit While
                    End Try

                    Try
                        singleMapDataPoint.Z = Double.Parse(elements(zCol - 1))
                    Catch ex As Exception
                        e.Cancel = True
                        Exit While
                    End Try

                    If iCol <> -1 Then
                        Try
                            singleMapDataPoint.I = Double.Parse(elements(iCol - 1))
                        Catch ex As Exception
                            e.Cancel = True
                            Exit While
                        End Try
                    End If

                    If rCol <> -1 Then
                        Try
                            singleMapDataPoint.R = Double.Parse(elements(rCol - 1))
                        Catch ex As Exception
                            e.Cancel = True
                            Exit While
                        End Try
                    End If

                    If gCol <> -1 Then
                        Try
                            singleMapDataPoint.G = Double.Parse(elements(gCol - 1))
                        Catch ex As Exception
                            e.Cancel = True
                            Exit While
                        End Try
                    End If

                    If bCol <> -1 Then
                        Try
                            singleMapDataPoint.B = Double.Parse(elements(bCol - 1))
                        Catch ex As Exception
                            e.Cancel = True
                            Exit While
                        End Try
                    End If

                    singleMapDataPoint.Xm = singleMapDataPoint.X
                    singleMapDataPoint.Ym = singleMapDataPoint.Y
                    singleMapDataPoint.deleted = False
                    singleMapDataPoint.highlighted = False

                    'intensity histogram population
                    If singleMapDataPoint.I > intensityHistogram.Length - 1 Then
                        ReDim Preserve intensityHistogram(singleMapDataPoint.I)
                    End If
                    intensityHistogram(singleMapDataPoint.I) += 1

                    If datasetToUse = 1 Then
                        singleMapDataPoint.datasetNum = 1
                        dataset1MapDataPoints.Add(singleMapDataPoint)
                        currentMapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 2 Then
                        singleMapDataPoint.datasetNum = 2
                        dataset2MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 3 Then
                        singleMapDataPoint.datasetNum = 3
                        dataset3MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 4 Then
                        singleMapDataPoint.datasetNum = 4
                        dataset4MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 5 Then
                        singleMapDataPoint.datasetNum = 5
                        dataset5MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 6 Then
                        singleMapDataPoint.datasetNum = 6
                        dataset6MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 7 Then
                        singleMapDataPoint.datasetNum = 7
                        dataset7MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 8 Then
                        singleMapDataPoint.datasetNum = 8
                        dataset8MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 9 Then
                        singleMapDataPoint.datasetNum = 9
                        dataset9MapDataPoints.Add(singleMapDataPoint)
                    ElseIf datasetToUse = 10 Then
                        singleMapDataPoint.datasetNum = 10
                        dataset10MapDataPoints.Add(singleMapDataPoint)
                    End If
                    readNumPoints += 1
                End If
            End While
            intensityHistMaxMin = calcHistogramBounds(intensityHistogram)

            pointfileStream.Close()

            If readNumPoints = 0 Then
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If

    End Sub

    Private Sub backgroundWorker2_ReportStatus(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles backgroundWorker2.ProgressChanged
        percentageLabel.Text = e.ProgressPercentage.ToString & " %"
        percentageLabel.Refresh()
    End Sub

    'Procedure to display the loaded map dataset after the secondary thread is finished
    Private Sub backgroundWorker2_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles backgroundWorker2.RunWorkerCompleted
        If File.Exists(Application.StartupPath & "\pointcloud.txt") Then
            File.Delete(Application.StartupPath & "\pointcloud.txt")
        End If

        If e.Cancelled <> True Then
            percentageLabel.Text = "100 %"
            percentageLabel.Refresh()
            Threading.Thread.Sleep(1000)
            percentageLabel.Text = ""
            percentageLabel.Refresh()

            statusLabel.Text = "Calculating dataset statistics"
            statusLabel.Refresh()

            Dim newRow(7) As Object
            newRow(0) = String.Empty
            newRow(1) = 0
            newRow(2) = 0
            newRow(3) = 0
            newRow(4) = 0
            newRow(5) = 0
            newRow(6) = 0
            newRow(7) = 0

            If datasetToUse = 1 Then
                dataset1CheckBox.Text = "1-" & datasetFileName
                GRcolumn3.Items.Add(dataset1CheckBox.Text)
                newRow(0) = dataset1CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset1ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset1CheckBox, dataset1CheckBox.Text)
                dataset1CheckBox.Visible = True
                dataset1CheckBox.Checked = True
                dataset1ColorLabel.BackColor = Color.White
                dataset1ColorLabel.Visible = True
                dataset1CheckBox.BackColor = Color.White

                'determine centroid of first imported dataset and set initial target and camera to view this point in plan (2D) orientation
                Dim maxMinTarget(,) As Double = ComputeMaxMin(currentMapDataPoints)
                updateMaxMin(maxMinTarget)

                target.data(1, 1) = maxMinTarget(0, 0) - (maxMinTarget(0, 0) - maxMinTarget(0, 1)) / 2
                target.data(2, 1) = maxMinTarget(1, 0) - (maxMinTarget(1, 0) - maxMinTarget(1, 1)) / 2
                target.data(3, 1) = maxMinTarget(2, 0) - (maxMinTarget(2, 0) - maxMinTarget(2, 1)) / 2

                'determine initial position and standoff distance of the camera
                camera.data(1, 1) = target.data(1, 1)
                camera.data(2, 1) = target.data(2, 1) - 0.1
                camera.data(3, 1) = ((maxMinTarget(0, 0) - maxMinTarget(0, 1)) / 2) / Math.Tan((FOV * Math.PI / 180) / 2)

                Dim yTest As Double = ((maxMinTarget(1, 0) - maxMinTarget(1, 1)) / 2) / Math.Tan((FOV * Math.PI / 180) / 2)
                If yTest > camera.data(3, 1) Then
                    camera.data(3, 1) = yTest
                End If

                currentScalefactor = calcScaleFactor()

                initialTarget.equals(target)
                initialCamera.equals(camera)
                initialScaleFactor = currentScalefactor

            ElseIf datasetToUse = 2 Then
                dataset2CheckBox.Text = "2-" & datasetFileName
                GRcolumn3.Items.Add(dataset2CheckBox.Text)
                newRow(0) = dataset2CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset2ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset2CheckBox, dataset2CheckBox.Text)
                dataset2CheckBox.Visible = True
                dataset2CheckBox.Checked = True
                dataset2ColorLabel.BackColor = Color.Blue
                dataset2ColorLabel.Visible = True
                dataset2CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 3 Then
                dataset3CheckBox.Text = "3-" & datasetFileName
                GRcolumn3.Items.Add(dataset3CheckBox.Text)
                newRow(0) = dataset3CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset3ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset3CheckBox, dataset3CheckBox.Text)
                dataset3CheckBox.Visible = True
                dataset3CheckBox.Checked = True
                dataset3ColorLabel.BackColor = Color.Brown
                dataset3ColorLabel.Visible = True
                dataset3CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 4 Then
                dataset4CheckBox.Text = "4-" & datasetFileName
                GRcolumn3.Items.Add(dataset4CheckBox.Text)
                newRow(0) = dataset4CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset4ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset4CheckBox, dataset4CheckBox.Text)
                dataset4CheckBox.Visible = True
                dataset4CheckBox.Checked = True
                dataset4ColorLabel.BackColor = Color.Gray
                dataset4ColorLabel.Visible = True
                dataset4CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 5 Then
                dataset5CheckBox.Text = "5-" & datasetFileName
                GRcolumn3.Items.Add(dataset5CheckBox.Text)
                newRow(0) = dataset5CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset5ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset5CheckBox, dataset5CheckBox.Text)
                dataset5CheckBox.Visible = True
                dataset5CheckBox.Checked = True
                dataset5ColorLabel.BackColor = Color.Green
                dataset5ColorLabel.Visible = True
                dataset5CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 6 Then
                dataset6CheckBox.Text = "6-" & datasetFileName
                GRcolumn3.Items.Add(dataset6CheckBox.Text)
                newRow(0) = dataset6CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset6ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset6CheckBox, dataset6CheckBox.Text)
                dataset6CheckBox.Visible = True
                dataset6CheckBox.Checked = True
                dataset6ColorLabel.BackColor = Color.Orange
                dataset6ColorLabel.Visible = True
                dataset6CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 7 Then
                dataset7CheckBox.Text = "7-" & datasetFileName
                GRcolumn3.Items.Add(dataset7CheckBox.Text)
                newRow(0) = dataset7CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset7ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset7CheckBox, dataset7CheckBox.Text)
                dataset7CheckBox.Visible = True
                dataset7CheckBox.Checked = True
                dataset7ColorLabel.BackColor = Color.Pink
                dataset7ColorLabel.Visible = True
                dataset7CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 8 Then
                dataset8CheckBox.Text = "8-" & datasetFileName
                GRcolumn3.Items.Add(dataset8CheckBox.Text)
                newRow(0) = dataset8CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset8ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset8CheckBox, dataset8CheckBox.Text)
                dataset8CheckBox.Visible = True
                dataset8CheckBox.Checked = True
                dataset8ColorLabel.BackColor = Color.Purple
                dataset8ColorLabel.Visible = True
                dataset8CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 9 Then
                dataset9CheckBox.Text = "9-" & datasetFileName
                GRcolumn3.Items.Add(dataset9CheckBox.Text)
                newRow(0) = dataset9CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset9ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset9CheckBox, dataset9CheckBox.Text)
                dataset9CheckBox.Visible = True
                dataset9CheckBox.Checked = True
                dataset9ColorLabel.BackColor = Color.Red
                dataset9ColorLabel.Visible = True
                dataset9CheckBox.BackColor = Color.White
            ElseIf datasetToUse = 10 Then
                dataset10CheckBox.Text = "10-" & datasetFileName
                GRcolumn3.Items.Add(dataset10CheckBox.Text)
                newRow(0) = dataset10CheckBox.Text
                transParameters.transParametersDataGridView.Rows.Add(newRow)
                transParameters.dataset10ApplyButton.Visible = True
                ToolTip1.SetToolTip(dataset10CheckBox, dataset10CheckBox.Text)
                dataset10CheckBox.Visible = True
                dataset10CheckBox.Checked = True
                dataset10ColorLabel.BackColor = Color.YellowGreen
                dataset10ColorLabel.Visible = True
                dataset10CheckBox.BackColor = Color.White
            End If

            numDatasetsInteger += 1
            detailLevel = reducePtsComboBox.SelectedIndex
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
            Me.Cursor = Cursors.Default
            mapPanel.Cursor = currentMapCursor

            If numDatasetsInteger > 1 Then
                Dim maxMinTarget(,) As Double = ComputeMaxMin(currentMapDataPoints)
                updateMaxMin(maxMinTarget)
                currentScalefactor = calcScaleFactor()
            End If

        Else
            Me.Cursor = Cursors.Default
            mapPanel.Cursor = currentMapCursor
            If datasetError = False Then
                MessageBox.Show("The file was not of the specified format or was unable to be read!" & vbNewLine & vbNewLine & "Hence this dataset has not been loaded into the Viewer", "ERROR LOADING FILE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
        statusLabel.Text = String.Empty
        percentageLabel.Text = String.Empty
    End Sub

    Private Sub cl3Shell_ErrorOccurred(ByVal ErrorType As String) Handles cl3Shell.ErrorOccurred
        MessageBox.Show(ErrorType, "libTopconCL3 PROCESSING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    'unorthodox way of sort of handling a DONE type event of cl3Shell 
    'But it works at avoiding cross-threading calls to the forms controls
    Private Sub cl3Shell_Tick() Handles cl3Shell.Tick
        If cl3Shell.isRunning = False Then
            cl3Shell.TimerStop()
            'safe to read the CL3 decoded TXT file into your application now (done in the background)
            statusLabel.Text = "CL3 Decoding Completed in " & cl3Shell.processingTime.ToString("f1") & " sec"
            statusLabel.Refresh()
            System.Threading.Thread.Sleep(3000)
            configureWorkerAndStartRead() 'starts existing background worker to read in the decoded txt file
        Else
            updatePercentage(cl3Shell.processingTime.ToString("f1") & " s")
        End If
    End Sub

    Private Sub clrShell_ErrorOccurred(ByVal ErrorType As String) Handles clrShell.ErrorOccurred
        MessageBox.Show(ErrorType, "libTopconCLR PROCESSING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    'unorthodox way of sort of handling a DONE type event of cl3Shell 
    'But it works at avoiding cross-threading calls to the forms controls
    Private Sub clrShell_Tick() Handles clrShell.Tick
        If clrShell.isRunning = False Then
            clrShell.TimerStop()
            'safe to read the CLR decoded TXT file into your application now (done in the background)
            statusLabel.Text = "CLR Decoding Completed in " & clrShell.processingTime.ToString("f1") & " sec"
            statusLabel.Refresh()
            System.Threading.Thread.Sleep(3000)
            configureWorkerAndStartRead() 'starts existing background worker to read in the decoded txt file
        Else
            updatePercentage(clrShell.processingTime.ToString("f1") & " s")
        End If
    End Sub

    Private Function calcScaleFactor() As Decimal
        'Dim maxX As Double = values(0, 0)
        'Dim minX As Double = values(0, 1)
        'Dim maxY As Double = values(1, 0)
        'Dim minY As Double = values(1, 1)

        'Dim PosScaleX, PosScaleY, AvgPosSF As Decimal

        'If maxX - minX = 0 Then
        '    PosScaleX = 1000000000000
        'Else
        '    PosScaleX = mapPanel.Width / (maxX - minX)
        'End If

        'If maxY - minY = 0 Then
        '    PosScaleY = 1000000000000
        'Else
        '    PosScaleY = mapPanel.Height / (maxY - minY)
        'End If

        'If PosScaleY < PosScaleX Then
        '    AvgPosSF = PosScaleY
        'Else
        '    AvgPosSF = PosScaleX
        'End If

        ''currentMapSF = AvgPosSF
        'Return AvgPosSF / 625
        'Return 1D

        Dim cam2tarDist As Decimal = Matrix.abs(camera - target)
        Dim modelDist As Decimal = 2 * cam2tarDist * Math.Tan((FOV * Math.PI / 180D) / 2)
        If modelDist <> 0 Then
            Return (mapPanel.Width / modelDist)
        Else
            Return 1
        End If

    End Function
    'compute the max and min values of a dataset with respect to its Global (unadjusted) Coordinates
    Private Function ComputeMaxMin(ByVal values As Generic.List(Of cloudPoint)) As Double(,)

        Dim MaxMin(2, 1) As Double
        Dim i As Integer

        Try
            MaxMin(0, 0) = values(0).X   'max World X
            MaxMin(0, 1) = values(0).X  'min World X
            MaxMin(1, 0) = values(0).Y    'max World Y
            MaxMin(1, 1) = values(0).Y    'min World Y
            MaxMin(2, 0) = values(0).Z  'max World Z
            MaxMin(2, 1) = values(0).Z   'min World Z

            For i = 0 To values.Count - 1
                If values(i).deleted = False Then
                    If values(i).X > MaxMin(0, 0) Then
                        MaxMin(0, 0) = values(i).X
                    End If
                    If values(i).X < MaxMin(0, 1) Then
                        MaxMin(0, 1) = values(i).X
                    End If
                    If values(i).Y > MaxMin(1, 0) Then
                        MaxMin(1, 0) = values(i).Y
                    End If
                    If values(i).Y < MaxMin(1, 1) Then
                        MaxMin(1, 1) = values(i).Y
                    End If
                    If values(i).Z > MaxMin(2, 0) Then
                        MaxMin(2, 0) = values(i).Z
                    End If
                    If values(i).Z < MaxMin(2, 1) Then
                        MaxMin(2, 1) = values(i).Z
                    End If
                End If
            Next

            Return MaxMin

        Catch ex As Exception
            MaxMin(0, 0) = -100000000
            MaxMin(0, 1) = 100000000
            MaxMin(1, 0) = -100000000
            MaxMin(1, 1) = 100000000
            MaxMin(2, 0) = -100000000
            MaxMin(2, 1) = 100000000
            Return MaxMin
        End Try
    End Function

    'compute the max and min values of a dataset with respect to its Global (unadjusted) Coordinates
    Private Function ComputeMaxMinInView() As Double(,)

        Dim MaxMin(2, 1) As Double
        Dim i As Integer

        Try
            MaxMin(0, 0) = -100000000
            MaxMin(0, 1) = 100000000
            MaxMin(1, 0) = -100000000
            MaxMin(1, 1) = 100000000
            MaxMin(2, 0) = -100000000
            MaxMin(2, 1) = 100000000

            For i = 0 To currentMapDataPoints.Count - 1
                If currentMapDataPoints(i).deleted = False And currentMapDataPoints(i).inView = True Then
                    If currentMapDataPoints(i).X > MaxMin(0, 0) Then
                        MaxMin(0, 0) = currentMapDataPoints(i).X
                    End If
                    If currentMapDataPoints(i).X < MaxMin(0, 1) Then
                        MaxMin(0, 1) = currentMapDataPoints(i).X
                    End If
                    If currentMapDataPoints(i).Y > MaxMin(1, 0) Then
                        MaxMin(1, 0) = currentMapDataPoints(i).Y
                    End If
                    If currentMapDataPoints(i).Y < MaxMin(1, 1) Then
                        MaxMin(1, 1) = currentMapDataPoints(i).Y
                    End If
                    If currentMapDataPoints(i).Z > MaxMin(2, 0) Then
                        MaxMin(2, 0) = currentMapDataPoints(i).Z
                    End If
                    If currentMapDataPoints(i).Z < MaxMin(2, 1) Then
                        MaxMin(2, 1) = currentMapDataPoints(i).Z
                    End If
                End If
            Next

            Return MaxMin

        Catch ex As Exception
            MaxMin(0, 0) = 100
            MaxMin(0, 1) = -100
            MaxMin(1, 0) = 100
            MaxMin(1, 1) = -100
            MaxMin(2, 0) = 100
            MaxMin(2, 1) = -100
            Return MaxMin
        End Try
    End Function

    'procedure for unloading a selected loaded dataset while still maintaining the correct global variable datasets
    Private Sub unloadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles unloadButton.Click
        Dim updateGlobal As Boolean = True

        If numDatasetsInteger = 0 Then
            MessageBox.Show("No datasets have been loaded so there are none to unload!", "NOTHING LOADED", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            If dataset1CheckBox.BackColor = Color.CornflowerBlue Then
                dataset1MapDataPoints.Clear()
                dataset1CheckBox.Visible = False
                dataset1ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset2CheckBox.BackColor = Color.CornflowerBlue Then
                dataset2MapDataPoints.Clear()
                dataset2CheckBox.Visible = False
                dataset2ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset3CheckBox.BackColor = Color.CornflowerBlue Then
                dataset3MapDataPoints.Clear()
                dataset3CheckBox.Visible = False
                dataset3ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset4CheckBox.BackColor = Color.CornflowerBlue Then
                dataset4MapDataPoints.Clear()
                dataset4CheckBox.Visible = False
                dataset4ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset5CheckBox.BackColor = Color.CornflowerBlue Then
                dataset5MapDataPoints.Clear()
                dataset5CheckBox.Visible = False
                dataset5ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset6CheckBox.BackColor = Color.CornflowerBlue Then
                dataset6MapDataPoints.Clear()
                dataset6CheckBox.Visible = False
                dataset6ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset7CheckBox.BackColor = Color.CornflowerBlue Then
                dataset7MapDataPoints.Clear()
                dataset7CheckBox.Visible = False
                dataset7ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset8CheckBox.BackColor = Color.CornflowerBlue Then
                dataset8MapDataPoints.Clear()
                dataset8CheckBox.Visible = False
                dataset8ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset9CheckBox.BackColor = Color.CornflowerBlue Then
                dataset9MapDataPoints.Clear()
                dataset9CheckBox.Visible = False
                dataset9ColorLabel.Visible = False
                numDatasetsInteger -= 1
            ElseIf dataset10CheckBox.BackColor = Color.CornflowerBlue Then
                dataset10MapDataPoints.Clear()
                dataset10CheckBox.Visible = False
                dataset10ColorLabel.Visible = False
                numDatasetsInteger -= 1
            Else
                MessageBox.Show("You must select a loaded dataset from the list before it can be unloaded!", "SELECT LOADED DATASET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                updateGlobal = False
            End If
        End If

        If updateGlobal = True Then
            ReDim intensityHistogram(0)
            If dataset1CheckBox.Checked = True And dataset1CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset1MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset1MapDataPoints)
            End If
            If dataset2CheckBox.Checked = True And dataset2CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset2MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset2MapDataPoints)
            End If
            If dataset3CheckBox.Checked = True And dataset3CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset3MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset3MapDataPoints)
            End If
            If dataset4CheckBox.Checked = True And dataset4CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset4MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset4MapDataPoints)
            End If
            If dataset5CheckBox.Checked = True And dataset5CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset5MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset5MapDataPoints)
            End If
            If dataset6CheckBox.Checked = True And dataset6CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset6MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset6MapDataPoints)
            End If
            If dataset7CheckBox.Checked = True And dataset7CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset7MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset7MapDataPoints)
            End If
            If dataset8CheckBox.Checked = True And dataset8CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset8MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset8MapDataPoints)
            End If
            If dataset9CheckBox.Checked = True And dataset9CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset9MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset9MapDataPoints)
            End If
            If dataset10CheckBox.Checked = True And dataset10CheckBox.Visible = True Then
                Dim maxMinNEValues(,) As Double = ComputeMaxMin(dataset1MapDataPoints)
                updateMaxMin(maxMinNEValues)
                updateHistogram(dataset10MapDataPoints)
            End If
            intensityHistMaxMin = calcHistogramBounds(intensityHistogram)
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If

    End Sub

    Private Sub updateHistogram(ByVal values As Generic.List(Of cloudPoint))
        For i As Integer = 0 To values.Count - 1
            'intensity histogram population
            If values(i).I > intensityHistogram.Length - 1 Then
                ReDim Preserve intensityHistogram(values(i).I)
            End If
            intensityHistogram(values(i).I) += 1
        Next
    End Sub

    'procedure for having the loaded map datasets mapping coordinates calculated and drawn on the Map
    Friend Sub drawMapButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drawMapButton.Click, refreshMapButton.Click
        statusLabel.Text = "Rendering View"
        statusLabel.Refresh()
        Dim delim() As Char = {","}
        Dim objectType() As String = sender.ToString.Split(delim)
        Dim senderButton As Object = sender
        If dataset1CheckBox.Visible = True Or dataset2CheckBox.Visible = True Or dataset3CheckBox.Visible = True Or dataset4CheckBox.Visible = True Or dataset5CheckBox.Visible = True Or dataset6CheckBox.Visible = True Or dataset7CheckBox.Visible = True Or dataset8CheckBox.Visible = True Or dataset9CheckBox.Visible = True Or dataset10CheckBox.Visible = True Then
            Dim oldFormCursor As Cursor = Me.Cursor
            Dim oldMapCursor As Cursor = mapPanel.Cursor
            Me.Cursor = Cursors.AppStarting
            mapPanel.Cursor = Cursors.AppStarting

            calcDisplayXYZ(currentMapDataPoints)

            If senderButton.Name = "drawMapButton" Or mapDisplayed = False Then
                currentMaxMinValues(0, 0) = globalMaxMinValues(0, 0)
                currentMaxMinValues(0, 1) = globalMaxMinValues(0, 1)
                currentMaxMinValues(1, 0) = globalMaxMinValues(1, 0)
                currentMaxMinValues(1, 1) = globalMaxMinValues(1, 1)
                currentMaxMinValues(2, 0) = globalMaxMinValues(2, 0)
                currentMaxMinValues(2, 1) = globalMaxMinValues(2, 1)
            End If

            mapGraphics.Clear(Color.FromArgb(40, 40, 40))
            'fileGraphics.Clear(Color.FromArgb(40, 40, 40))
            renderView2(currentMapDataPoints)
            mapDisplayed = True
            mapPanel.BackgroundImage = mapBitmap
            mapPanel.Refresh()
            Me.Cursor = oldFormCursor
            mapPanel.Cursor = oldMapCursor
        Else
            clearMapButton_Click(sender, e)
            mapDisplayed = False
        End If
        statusLabel.Text = ""
        statusLabel.Refresh()
    End Sub

    'calculate histogram stats
    Private Function calcHistogramBounds(ByVal histogram() As Integer) As Integer()
        Dim totalPoints As Integer = 0
        For i = 0 To histogram.Length - 1
            totalPoints += histogram(i)
        Next
        Dim appliedHistogramCutoff As Decimal
        If histogramCutoff = 50 Then
            appliedHistogramCutoff = 49.5
        Else
            appliedHistogramCutoff = histogramCutoff
        End If
        Dim minCutOff As Integer = Convert.ToInt32(Decimal.Round((appliedHistogramCutoff / 100D) * Convert.ToDouble(totalPoints)))
        Dim maxCutOff As Integer = Convert.ToInt32(Decimal.Round((1 - (appliedHistogramCutoff / 100D)) * Convert.ToDouble(totalPoints)))

        Dim foundMin As Boolean = False
        Dim foundMax As Boolean = False
        Dim runningSum As Integer = 0
        Dim calcdHistMaxMin(1) As Integer
        For i = 0 To histogram.Length - 1
            runningSum += histogram(i)
            If runningSum >= minCutOff And Not foundMin Then
                calcdHistMaxMin(1) = i
                foundMin = True
            End If
            If runningSum >= maxCutOff And Not foundMax Then
                calcdHistMaxMin(0) = i
                foundMax = True
            End If
        Next
        Return calcdHistMaxMin
    End Function

    'procedure to update Global Max Min Northing (Y) Easting (X) map coordinates
    Private Sub updateMaxMin(ByVal values(,) As Double)

        'Row 0 X values (max, min)
        'Row 1 Y values (max, min)
        'Row 2 Z values (max, min)

        If values(0, 0) > globalMaxMinValues(0, 0) Then
            globalMaxMinValues(0, 0) = values(0, 0)
        End If
        If values(0, 1) < globalMaxMinValues(0, 1) Then
            globalMaxMinValues(0, 1) = values(0, 1)
        End If
        If values(1, 0) > globalMaxMinValues(1, 0) Then
            globalMaxMinValues(1, 0) = values(1, 0)
        End If
        If values(1, 1) < globalMaxMinValues(1, 1) Then
            globalMaxMinValues(1, 1) = values(1, 1)
        End If
        If values(2, 0) > globalMaxMinValues(2, 0) Then
            globalMaxMinValues(2, 0) = values(2, 0)
        End If
        If values(2, 1) < globalMaxMinValues(2, 1) Then
            globalMaxMinValues(2, 1) = values(2, 1)
        End If

        'globalCentreValue(1) = globalMaxMinValues(0, 0) - (globalMaxMinValues(0, 0) - globalMaxMinValues(0, 1)) / 2
        'globalCentreValue(0) = globalMaxMinValues(1, 0) - (globalMaxMinValues(1, 0) - globalMaxMinValues(1, 1)) / 2
        'globalCentreValue(2) = globalMaxMinValues(2, 0) - (globalMaxMinValues(2, 0) - globalMaxMinValues(2, 1)) / 2

        globalScaleFactor = calcScaleFactor()

    End Sub

    'procedure to figure out which point the user wants to pan with and for drawing a selection box
    Private Sub mapPanel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapPanel.MouseDown
        If mapDisplayed = True And mapPanel.Cursor = Cursors.Hand Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                isMouseButtonDown = True
                fromHerePan(0) = (Convert.ToDouble(e.X + 1) - (mapPanel.Width / 2.0R))
                fromHerePan(1) = ((mapPanel.Height / 2.0R) - Convert.ToDouble(e.Y + 1))
                fromHereSlide(0) = e.X + 1
                fromHereSlide(1) = e.Y + 1
            Else
                MessageBox.Show("You cannot rotate or pan the camera while in Sliding Mode", "ROTATE OR PANNING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        ElseIf mapDisplayed = True And insideDrawBox = True Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                isMouseButtonDown = True
                fromHereBox(0) = e.Y + 1
                fromHereBox(1) = e.X + 1
            End If

            'rotations
        ElseIf mapDisplayed = True And e.Button = Windows.Forms.MouseButtons.Right And mapPanel.Cursor = Cursors.Cross Then
            isMouseButtonDown = True
            fromHerePt(0) = e.Y + 1
            fromHerePt(1) = e.X + 1
            detailLevel = 10
            updateCurrentDataset(e)
            cameraBasePos = camera
            cameraBaseX = imagePlaneX
            cameraBaseY = imagePlaneY
            cameraBasePos2 = camera
            cameraBaseX2 = imagePlaneX
            cameraBaseY2 = imagePlaneY
            If Not insideNavigate Then
                insideNavigate = True
                initialLineWeights(0) = dataset1LineWeight
                initialLineWeights(1) = dataset2LineWeight
                initialLineWeights(2) = dataset3LineWeight
                initialLineWeights(3) = dataset4LineWeight
                initialLineWeights(4) = dataset5LineWeight
                initialLineWeights(5) = dataset6LineWeight
                initialLineWeights(6) = dataset7LineWeight
                initialLineWeights(7) = dataset8LineWeight
                initialLineWeights(8) = dataset9LineWeight
                initialLineWeights(9) = dataset10LineWeight
                dataset1LineWeight = navigatePointSize
                dataset2LineWeight = navigatePointSize
                dataset3LineWeight = navigatePointSize
                dataset4LineWeight = navigatePointSize
                dataset5LineWeight = navigatePointSize
                dataset6LineWeight = navigatePointSize
                dataset7LineWeight = navigatePointSize
                dataset8LineWeight = navigatePointSize
                dataset9LineWeight = navigatePointSize
                dataset10LineWeight = navigatePointSize
            End If

            'translations (panning)
        ElseIf mapDisplayed = True And e.Button = Windows.Forms.MouseButtons.Middle And mapPanel.Cursor = Cursors.Cross Then
            isMouseButtonDown = True
            fromHerePt(0) = e.Y + 1
            fromHerePt(1) = e.X + 1
            detailLevel = 10
            updateCurrentDataset(e)
            cameraBasePos.equals(camera)
            targetBasePos.equals(target)

            Dim cam2tarDist As Decimal = Matrix.abs(camera - target)
            Dim modelDist As Decimal = 2 * cam2tarDist * Math.Tan((FOV * Math.PI / 180D) / 2)
            panScalefactor = modelDist / mapPanel.Width

            If Not insideNavigate Then
                insideNavigate = True
                initialLineWeights(0) = dataset1LineWeight
                initialLineWeights(1) = dataset2LineWeight
                initialLineWeights(2) = dataset3LineWeight
                initialLineWeights(3) = dataset4LineWeight
                initialLineWeights(4) = dataset5LineWeight
                initialLineWeights(5) = dataset6LineWeight
                initialLineWeights(6) = dataset7LineWeight
                initialLineWeights(7) = dataset8LineWeight
                initialLineWeights(8) = dataset9LineWeight
                initialLineWeights(9) = dataset10LineWeight
                dataset1LineWeight = navigatePointSize
                dataset2LineWeight = navigatePointSize
                dataset3LineWeight = navigatePointSize
                dataset4LineWeight = navigatePointSize
                dataset5LineWeight = navigatePointSize
                dataset6LineWeight = navigatePointSize
                dataset7LineWeight = navigatePointSize
                dataset8LineWeight = navigatePointSize
                dataset9LineWeight = navigatePointSize
                dataset10LineWeight = navigatePointSize
            End If
        End If
    End Sub

    'procedure to actively draw (re-render) the selection box while the user is drawing it on the screen and to slide the current view within the display
    Private Sub mapPanel_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapPanel.MouseMove
        Dim mapBasePoint2Screen As Point = Me.PointToScreen(mapPanel.Location)
        Dim mouseX As Integer = mapPanel.MousePosition.X - mapBasePoint2Screen.X
        Dim mouseY As Integer = mapPanel.MousePosition.Y - mapBasePoint2Screen.Y

        If mapDisplayed = True Then
            If isMouseButtonDown And insideDrawBox Then
                Dim finePen As New Pen(Color.FromArgb(255, 105, 180), 2)
                Dim boxBitmap As New Bitmap(mapBitmap)
                Dim boxGraphics As Graphics = Graphics.FromImage(boxBitmap)
                boxGraphics.DrawRectangle(finePen, fromHereBox(1), fromHereBox(0), (mouseX - fromHereBox(1)), (mouseY - fromHereBox(0)))
                mapPanel.BackgroundImage = boxBitmap
                mapPanel.Refresh()
            ElseIf isMouseButtonDown And mapPanel.Cursor = Cursors.Hand And e.Button = Windows.Forms.MouseButtons.Left Then
                Dim pointOrigin As New Point(-1 * (fromHereSlide(0) - mouseX), -1 * (fromHereSlide(1) - mouseY))
                Dim newRectangle As New System.Drawing.Rectangle(pointOrigin, mapPanel.Size)
                Dim newBitmap As New Bitmap(referenceBitmap)
                Dim newGraphics As Graphics = Graphics.FromImage(newBitmap)
                Dim imgAttrs As New Imaging.ImageAttributes()
                imgAttrs.SetColorKey(Color.FromArgb(38, 38, 38), Color.FromArgb(42, 42, 42))

                newGraphics.DrawImage(newBitmap, newRectangle, 0, 0, mapPanel.Width, mapPanel.Height, GraphicsUnit.Pixel, imgAttrs)
                drawCentrePoint(newGraphics, Color.Red)
                mapPanel.BackgroundImage = newBitmap
                mapPanel.Refresh()

                'translations (panning)
            ElseIf isMouseButtonDown And e.Button = Windows.Forms.MouseButtons.Middle And mapPanel.Cursor = Cursors.Cross Then
                If mouseX >= 0 And mouseX <= mapPanel.Width And mouseY >= 0 And mouseY <= mapPanel.Height Then
                    'Dim viewMaxMin(,) As Double = ComputeMaxMinInView()
                    'calcScaleFactor(viewMaxMin)

                    Dim distH As Double = panScalefactor * (mouseX - fromHerePt(1))
                    Dim distV As Double = panScalefactor * (mouseY - fromHerePt(0))

                    Dim FBdirection As New Matrix(3, 1)
                    FBdirection.data(1, 1) = imagePlaneY.data(1, 1)
                    FBdirection.data(2, 1) = imagePlaneY.data(2, 1)

                    FBdirection.data(3, 1) = imagePlaneY.data(3, 1)
                    'FBdirection.data(3, 1) = 0

                    FBdirection = FBdirection / Matrix.abs(FBdirection)   'renormalize the directions vector to ensure it is a unit vector

                    camera = cameraBasePos - distH * imagePlaneX
                    camera = camera + distV * FBdirection

                    target = targetBasePos - distH * imagePlaneX
                    target = target + distV * FBdirection

                    drawMapButton_Click(refreshMapButton, e)
                    camXLabel.Text = "X  =  " & camera.data(1, 1).ToString("f3")
                    camYLabel.Text = "Y  =  " & camera.data(2, 1).ToString("f3")
                    camZLabel.Text = "Z  =  " & camera.data(3, 1).ToString("f3")
                End If

                'rotations
            ElseIf isMouseButtonDown And e.Button = Windows.Forms.MouseButtons.Right And mapPanel.Cursor = Cursors.Cross Then
                If mouseX >= 0 And mouseX <= mapPanel.Width And mouseY >= 0 And mouseY <= mapPanel.Height Then

                    Dim anglePerPixelH As Double = 130.0R / mapPanel.Width
                    Dim anglePerPixelV As Double = 85.0R / mapPanel.Height
                    Dim currentAngle2Rotate As Double
                    Dim hUpdate As Boolean = False

                    ''horizontal movement of camera with mouse movement
                    currentAngle2Rotate = (fromHerePt(1) - mouseX) * anglePerPixelH
                    Dim adjCamera As Matrix = cameraBasePos - target
                    Dim userAdj As Double = -1
                    If ReverseHorizontalRotationsToolStripMenuItem.Checked Then
                        userAdj = 1
                    End If
                    camera = Matrix.R3(userAdj * currentAngle2Rotate) * adjCamera + target
                    hUpdate = True

                    'update camera base information if a horizontal movement of the camera was made
                    If hUpdate Then
                        Dim dir2target2, imagePlaneX2, imagePlaneY2 As New Matrix(3, 1)
                        cameraBasePos2 = camera
                        dir2target2 = (target - camera) / Matrix.abs(target - camera)
                        Try
                            imagePlaneX2 = Matrix.cross(dir2target2, upDirection)
                            imagePlaneX2 = imagePlaneX2 / Matrix.abs(imagePlaneX2) 'normalize to a unit vector
                        Catch ex As Exception
                        End Try
                        imagePlaneY2 = Matrix.cross((-1 * dir2target2), imagePlaneX2)
                        imagePlaneY2 = imagePlaneY2 / Matrix.abs(imagePlaneY2) 'normalize to a unit vector
                        cameraBaseX2 = imagePlaneX2
                        cameraBaseY2 = imagePlaneY2
                    End If

                    'vertical movement of camera with mouse movement
                    If mouseY - fromHerePt(0) < 0 Then
                        currentAngle2Rotate = (fromHerePt(0) - mouseY) * anglePerPixelV
                        Dim cam2TarDist As Decimal = Matrix.abs(cameraBasePos2 - target)
                        Dim H As Decimal = cam2TarDist / Math.Cos(currentAngle2Rotate * Math.PI / 180)
                        Dim O As Decimal = Math.Sqrt(H ^ 2 - cam2TarDist ^ 2)
                        Dim userAdj2 As Double = -1
                        If ReverseVerticalRotationsToolStripMenuItem.Checked Then
                            userAdj2 = 1
                        End If
                        camera = cameraBasePos2 + O * (userAdj2 * cameraBaseY2)
                        Dim dir2Centre As New Matrix(3, 1)
                        dir2Centre = (target - camera) / Matrix.abs(target - camera)
                        camera = camera + (H - cam2TarDist) * dir2Centre
                    ElseIf mouseY - fromHerePt(0) > 0 Then
                        currentAngle2Rotate = (fromHerePt(0) - mouseY) * anglePerPixelV
                        Dim cam2TarDist As Decimal = Matrix.abs(cameraBasePos2 - target)
                        Dim H As Decimal = cam2TarDist / Math.Cos(currentAngle2Rotate * Math.PI / 180)
                        Dim O As Decimal = Math.Sqrt(H ^ 2 - cam2TarDist ^ 2)
                        Dim userAdj2 As Double = 1
                        If ReverseVerticalRotationsToolStripMenuItem.Checked Then
                            userAdj2 = -1
                        End If
                        camera = cameraBasePos2 + O * (userAdj2 * cameraBaseY2)
                        Dim dir2Centre As New Matrix(3, 1)
                        dir2Centre = (target - camera) / Matrix.abs(target - camera)
                        camera = camera + (H - cam2TarDist) * dir2Centre
                    End If

                drawMapButton_Click(refreshMapButton, e)
                camXLabel.Text = "X  =  " & camera.data(1, 1).ToString("f3")
                camYLabel.Text = "Y  =  " & camera.data(2, 1).ToString("f3")
                camZLabel.Text = "Z  =  " & camera.data(3, 1).ToString("f3")
            End If
            End If
        End If
    End Sub

    'procedure to display Map Short-cut menu and get the mouse coordinates when the menu is displayed
    Private Sub mapPanel_MouseUp(ByVal sender As Object, ByVal e As Windows.Forms.MouseEventArgs) Handles mapPanel.MouseUp
        mapPanel.Focus()
        If e.Button = Windows.Forms.MouseButtons.Left Or e.Button = Windows.Forms.MouseButtons.Middle Or e.Button = Windows.Forms.MouseButtons.Right Then
            isMouseButtonDown = False
        End If
        If mapDisplayed = True Then
            If e.Button = Windows.Forms.MouseButtons.Right And mapPanel.Cursor = Cursors.Cross Then
                'mapPanel.ContextMenuStrip = mapPanelContextMenu
                'Dim mapBasePoint2Screen As Point = Me.PointToScreen(mapPanel.Location)
                'Dim mouseX As Integer = mapBasePoint2Screen.X + (e.X + 1)
                'Dim mouseY As Integer = mapBasePoint2Screen.Y + (e.Y + 1)
                'mapPanel.ContextMenuStrip.DropShadowEnabled = True
                'centreOfMouseMap(0) = (Convert.ToDouble(e.X + 1) - (mapPanel.Width / 2.0R))
                'centreOfMouseMap(1) = ((mapPanel.Height / 2.0R) - Convert.ToDouble(e.Y + 1))
                'mapPanel.ContextMenuStrip.Show(mouseX, mouseY)

                'imageOffsets(0) = 0
                'imageOffsets(1) = 0
                Timer1.Stop()
                Timer1.Dispose()
                Timer1.Start()

            ElseIf e.Button = Windows.Forms.MouseButtons.Left And mapPanel.Cursor = Cursors.Hand Then
                toHerePan(0) = (Convert.ToDouble(e.X + 1) - (mapPanel.Width / 2.0R))
                toHerePan(1) = ((mapPanel.Height / 2.0R) - Convert.ToDouble(e.Y + 1))

                centreOfMouseMap(0) = (fromHerePan(0) - toHerePan(0)) + 1
                centreOfMouseMap(1) = (fromHerePan(1) - toHerePan(1)) - 1

                CentreMapOnThisLocationToolStripMenuItem_Click(CentreMapOnThisLocationToolStripMenuItem, e)

            ElseIf e.Button = Windows.Forms.MouseButtons.Left And insideDrawBox Then
                toHereBox(0) = e.Y + 1
                toHereBox(1) = e.X + 1
            ElseIf e.Button = Windows.Forms.MouseButtons.Left And insidePickCamera Then
                statusLabel.Text = String.Empty
                Dim newCamera As New Matrix(3, 1)
                Dim foundPoint As Boolean = False

                newCamera = findScreenPickedPoint(currentMapDataPoints, e.X - 1, e.Y - 1, foundPoint)
                If foundPoint Then
                    camera = newCamera
                    imageOffsets(0) = 0
                    imageOffsets(1) = 0
                    drawMapButton_Click(refreshMapButton, e)
                    insidePickCamera = False
                Else
                    statusLabel.Text = "No dataset point was found at the selected screen location, select again or press ESC to quit"
                End If
            ElseIf e.Button = Windows.Forms.MouseButtons.Left And insidePickTarget Then
                statusLabel.Text = String.Empty
                Dim newTarget As New Matrix(3, 1)
                Dim foundPoint As Boolean = False

                newTarget = findScreenPickedPoint(currentMapDataPoints, e.X - 1, e.Y - 1, foundPoint)
                If foundPoint Then
                    target = newTarget
                    imageOffsets(0) = 0
                    imageOffsets(1) = 0
                    drawMapButton_Click(refreshMapButton, e)
                    insidePickTarget = False
                Else
                    statusLabel.Text = "No dataset point was found at the selected screen location, select again or press ESC to quit"
                End If
            ElseIf e.Button = Windows.Forms.MouseButtons.Left And insidePickRegTarget Then
                statusLabel.Text = String.Empty
                Dim newTarget As New Matrix(3, 1)
                Dim foundPoint As Boolean = False

                newTarget = findScreenPickedPoint(currentMapDataPoints, e.X - 1, e.Y - 1, foundPoint)
                If foundPoint Then

                    'compute points on boundary edge of target circle as computed by the LSA
                    Dim targetCircle As New cloudCircle
                    targetCircle.edgePoints = New Generic.List(Of cloudPoint)

                    For i As Integer = 0 To 360 Step 8
                        'XZ Plane Circle
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = Math.Sin(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(1, 1)
                        targetPoint.Y = newTarget.data(2, 1)
                        targetPoint.Z = Math.Cos(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    For i As Integer = 0 To 450 Step 8
                        'YZ Plane Circle (360 + 90 to get the ending point in the correct spot to start the last XY plane circle)
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = newTarget.data(1, 1)
                        targetPoint.Y = Math.Sin(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(2, 1)
                        targetPoint.Z = Math.Cos(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    For i As Integer = 0 To 360 Step 8
                        'XY Plane Circle
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = Math.Sin(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(1, 1)
                        targetPoint.Y = Math.Cos(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(2, 1)
                        targetPoint.Z = newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    For i As Integer = 90 To 0 Step -8
                        'YZ Plane Circle (-90 to get the ending point in the correct spot to close the target image)
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = newTarget.data(1, 1)
                        targetPoint.Y = Math.Sin(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(2, 1)
                        targetPoint.Z = Math.Cos(i * Math.PI / 180D) * manualTargetRadius + newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    targetCircle.centrePoint.X = newTarget.data(1, 1)
                    targetCircle.centrePoint.Y = newTarget.data(2, 1)
                    targetCircle.centrePoint.Z = newTarget.data(3, 1)

                    targetCircle.colour = processedTargetsColorLabel.BackColor  'this setting is not actually used as when the target circles are rendered the colour is as user defined in the targetColourLabel 
                    targetCircle.weight = targetsLineWeight 'this setting is not actually used as when the target circles are rendered the line weight is user defined (right-click targetColourLabel)

                    targetCircles.Add(targetCircle)
                    targetsFound = True

                    renderView2(currentMapDataPoints)
                    mapDisplayed = True
                    mapPanel.BackgroundImage = mapBitmap
                    mapPanel.Refresh()

                    maxTargetPoints += 1
                    Dim targetData() As String = {maxTargetPoints.ToString, newTarget.data(1, 1).ToString("f4"), newTarget.data(2, 1).ToString("f4"), newTarget.data(3, 1).ToString("f4"), manualTargetRadius.ToString("f4"), "Manual"}
                    targetsDataGridView.Rows.Add(targetData)
                    GRcolumn4.Items.Add(maxTargetPoints.ToString)
                    insidePickRegTarget = False
                Else
                    statusLabel.Text = "No dataset point was found at the selected screen location, select again or press ESC to quit"
                End If
            ElseIf e.Button = Windows.Forms.MouseButtons.Middle And mapPanel.Cursor = Cursors.Cross Then
                'imageOffsets(0) = 0
                'imageOffsets(1) = 0
                Timer1.Stop()
                Timer1.Dispose()
                Timer1.Start()
            End If
        End If
    End Sub

    'procedure to make sure the Map control has the Focus when the mouse has its cursor over it
    Private Sub mouseSetsPanelFocusOnEnter(ByVal sender As Object, ByVal e As EventArgs) Handles mapPanel.MouseEnter
        If histogramDisplayed = False Then
            If mapPanel.Focused = False Then
                resetButton.Focus()
                mapPanel.Focus()
            End If
        End If
    End Sub

    'procedure to make sure the datasets panel control has the Focus when the mouse has its cursor over it
    Private Sub datasetsPanel_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles datasetsPanel.MouseEnter
        If histogramDisplayed = False Then
            If datasetsPanel.Focused = False Then
                resetButton.Focus()
                datasetsPanel.Focus()
            End If
        End If
    End Sub

    'procedure to zoom in or out when the mouse wheel is rolled (uses the zoom and pan trackbar for level of zoom)
    Private Sub mapPanel_MousewheelRoll(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapPanel.MouseWheel
        If mapDisplayed = True Then
            If Not insideNavigate Then
                insideNavigate = True
                initialLineWeights(0) = dataset1LineWeight
                initialLineWeights(1) = dataset2LineWeight
                initialLineWeights(2) = dataset3LineWeight
                initialLineWeights(3) = dataset4LineWeight
                initialLineWeights(4) = dataset5LineWeight
                initialLineWeights(5) = dataset6LineWeight
                initialLineWeights(6) = dataset7LineWeight
                initialLineWeights(7) = dataset8LineWeight
                initialLineWeights(8) = dataset9LineWeight
                initialLineWeights(9) = dataset10LineWeight
                Dim rotatePointSize As Integer = 4
                dataset1LineWeight = rotatePointSize
                dataset2LineWeight = rotatePointSize
                dataset3LineWeight = rotatePointSize
                dataset4LineWeight = rotatePointSize
                dataset5LineWeight = rotatePointSize
                dataset6LineWeight = rotatePointSize
                dataset7LineWeight = rotatePointSize
                dataset8LineWeight = rotatePointSize
                dataset9LineWeight = rotatePointSize
                dataset10LineWeight = rotatePointSize
            End If
            If e.Delta < 0 Then
                If ReverseZoomToolStripMenuItem.Checked Then
                    zoomInOut(1)
                Else
                    zoomInOut(2)
                End If
            Else
                If ReverseZoomToolStripMenuItem.Checked Then
                    zoomInOut(2)
                Else
                    zoomInOut(1)
                End If
            End If
        End If
    End Sub

    Private Function findScreenPickedPoint(ByRef mapDataPoints As Generic.List(Of cloudPoint), ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef foundPoint As Boolean) As Matrix
        Dim solution As New Matrix(3, 1)
        For i As Integer = 0 To mapDataPoints.Count - 1
            If mapDataPoints(i).displayX >= ScreenX And mapDataPoints(i).displayX <= ScreenX + 3 And mapDataPoints(i).displayY >= ScreenY And mapDataPoints(i).displayY <= ScreenY + 3 Then
                foundPoint = True
                solution.data(1, 1) = mapDataPoints(i).X
                solution.data(2, 1) = mapDataPoints(i).Y
                solution.data(3, 1) = mapDataPoints(i).Z
                Exit For
            End If
        Next
        Return solution
    End Function

    'procedure to draw the dataset points on the Map
    Private Sub renderView(ByRef mapPoints As Generic.List(Of cloudPoint), ByVal plotColor As Color, ByVal lineweight As Integer, ByVal maxminNE(,) As Double, ByVal centreNE() As Double)
        Try
            pointsInView = 0
            Dim finePen As New Pen(plotColor, 0)
            finePen.Width = lineweight
            Dim rejectedPoints As Integer = 0
            Dim ScreenBasePt(1), PosBasePt(1) As Integer

            ScreenBasePt(0) = Convert.ToInt32(mapPanel.Width / 2)
            ScreenBasePt(1) = Convert.ToInt32(mapPanel.Height / 2)
            PosBasePt(0) = Convert.ToInt32(centreNE(1))
            PosBasePt(1) = Convert.ToInt32(centreNE(0))

            Dim drawingX, drawingY As Integer

            Dim myBrush As New SolidBrush(plotColor)
            Dim myBrush2 As New SolidBrush(plotColor)

            Dim includePoint As Boolean = True
            Dim modTest As Integer = 1

            If detailLevel = 1 Or detailLevel = 9 Then
                modTest = 10
            ElseIf detailLevel = 2 Or detailLevel = 8 Then
                modTest = 5
            ElseIf detailLevel = 3 Or detailLevel = 7 Then
                modTest = 4
            ElseIf detailLevel = 4 Or detailLevel = 6 Then
                modTest = 3
            ElseIf detailLevel = 5 Then
                modTest = 2
            ElseIf detailLevel = 10 Then    'only used programmatically to greatly reduce points drawn while rotating the view on the screen (98% reduction)
                modTest = 50
            End If

            If bbCheckBox.Checked Then
                renderBoundingBox()
            End If

            Dim appliedScaleFactor As Double
            If perspectiveRadioButton.Checked Then
                appliedScaleFactor = 1.0R
            Else
                appliedScaleFactor = currentScalefactor
            End If

            Dim drawPoint As Boolean    'used to test to see if points should be drawn when intensity colour is selected, otherwise ALL points are drawn for other colour selections
            For i As Integer = 0 To mapPoints.Count - 1

                If mapPoints(i).inView = True Then
                    'currentScalefactor = 1
                    PosBasePt(0) = 0
                    PosBasePt(1) = 0

                    drawingX = ScreenBasePt(0) + (mapPoints(i).Xm - centreNE(1)) * appliedScaleFactor
                    drawingY = ScreenBasePt(1) - (mapPoints(i).Ym - centreNE(0)) * appliedScaleFactor

                    If drawingX >= 0 And drawingX <= mapPanel.Width And drawingY >= 0 And drawingY <= mapPanel.Height Then

                        Dim newMapPoint As New cloudPoint
                        newMapPoint.X = mapPoints(i).X
                        newMapPoint.Y = mapPoints(i).Y
                        newMapPoint.Z = mapPoints(i).Z
                        newMapPoint.R = mapPoints(i).R
                        newMapPoint.G = mapPoints(i).G
                        newMapPoint.B = mapPoints(i).B
                        newMapPoint.I = mapPoints(i).I
                        newMapPoint.Xm = mapPoints(i).Xm
                        newMapPoint.Ym = mapPoints(i).Ym
                        'newMapPoint.Zm = mapPoints(i).Zm
                        newMapPoint.displayX = drawingX
                        newMapPoint.displayY = drawingY
                        newMapPoint.highlighted = mapPoints(i).highlighted
                        newMapPoint.deleted = mapPoints(i).deleted
                        newMapPoint.datasetNum = mapPoints(i).datasetNum
                        mapPoints(i) = newMapPoint

                        If Not mapPoints(i).deleted Then
                            drawPoint = True
                            'reduction percentage option filtering code
                            If i Mod modTest = 0 Then
                                If detailLevel >= 5 Or detailLevel = 0 Then
                                    includePoint = True
                                Else
                                    includePoint = False
                                End If
                            Else
                                If detailLevel >= 5 Then
                                    includePoint = False
                                Else
                                    includePoint = True
                                End If
                            End If

                            If includePoint = True Then
                                If mapPoints(i).highlighted Then
                                    myBrush.Color = Color.FromArgb(255, 105, 180)
                                Else
                                    If colourComboBox.SelectedIndex = 2 Then
                                        myBrush.Color = plotColor
                                    ElseIf colourComboBox.SelectedIndex = 1 Then
                                        myBrush.Color = Color.FromArgb(Convert.ToInt32(mapPoints(i).R), Convert.ToInt32(mapPoints(i).G), Convert.ToInt32(mapPoints(i).B))
                                    ElseIf colourComboBox.SelectedIndex = 0 Then
                                        If mapPoints(i).I >= intensityHistMaxMin(1) And mapPoints(i).I <= intensityHistMaxMin(0) Then
                                            Dim range As Double = intensityHistMaxMin(0) - intensityHistMaxMin(1)
                                            Dim p10 As Double = 0.1 * range + intensityHistMaxMin(1)
                                            Dim p50 As Double = 0.5 * range + intensityHistMaxMin(1)
                                            Dim p90 As Double = 0.9 * range + intensityHistMaxMin(1)
                                            Dim R, G, B As Integer

                                            'Red to White Colour Ramp (90% - 100% intensity values)
                                            If mapPoints(i).I > p90 Then
                                                R = 255
                                                G = (Convert.ToInt32((mapPoints(i).I - p90) / (range + intensityHistMaxMin(1) - p90) * 255.0R))
                                                B = G
                                                'Green to Red Colour Ramp (50% - 90% intensity values)
                                            ElseIf mapPoints(i).I > p50 Then
                                                R = (Convert.ToInt32((mapPoints(i).I - p50) / (p90 - p50) * 255.0R))
                                                G = 255 - R
                                                B = 0
                                                'Blue to Green Colour Ramp (10% - 50% intensity values)
                                            ElseIf mapPoints(i).I > p10 Then
                                                R = 0
                                                G = (Convert.ToInt32((mapPoints(i).I - p10) / (p50 - p10) * 255.0R))
                                                B = 255 - G
                                                'Black to Blue Colour Ramp (0% - 10% intensity values)
                                            Else
                                                R = 0
                                                G = 0
                                                B = (Convert.ToInt32((mapPoints(i).I - intensityHistMaxMin(1)) / (p10 - intensityHistMaxMin(1)) * 255.0R))
                                            End If
                                            myBrush.Color = Color.FromArgb(R, G, B)
                                        Else
                                            drawPoint = False
                                        End If
                                    End If
                                End If
                                If drawPoint = True Then
                                    pointsInView += 1
                                    mapGraphics.FillRectangle(myBrush, drawingX, drawingY, lineweight, lineweight)
                                    'fileGraphics.FillRectangle(myBrush, drawingX * fileScale, drawingY * fileScale, lineweight * fileScale, lineweight * fileScale)
                                    'active render code to have blocks of the point cloud(s) drawn more frequently
                                    If pointsInView Mod 10000I = 0 And activeRender = True Then
                                        mapPanel.BackgroundImage = mapBitmap
                                        mapPanel.Refresh()
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            'plot current target on the display
            Dim targetPointDataset As New Generic.List(Of cloudPoint)
            Dim singleMapDataPoint As New cloudPoint
            singleMapDataPoint.X = Convert.ToDouble(target.data(1, 1))
            singleMapDataPoint.Y = Convert.ToDouble(target.data(2, 1))
            singleMapDataPoint.Z = Convert.ToDouble(target.data(3, 1))
            targetPointDataset.Add(singleMapDataPoint)
            calcDisplayXYZ(targetPointDataset)
            drawingX = ScreenBasePt(0) + (targetPointDataset(0).Xm - centreNE(1)) * appliedScaleFactor - 2
            drawingY = ScreenBasePt(1) - (targetPointDataset(0).Ym - centreNE(0)) * appliedScaleFactor - 2
            myBrush2.Color = Color.FromArgb(153, 255, 0)
            mapGraphics.FillRectangle(myBrush2, drawingX, drawingY, 5, 5)
            'fileGraphics.FillRectangle(myBrush2, drawingX * fileScale, drawingY * fileScale, lineweight * fileScale, lineweight * fileScale)

            referenceBitmap = New Bitmap(mapBitmap)
            drawCentrePoint(mapGraphics, Color.LightGray)
            mapPanel.BackgroundImage = mapBitmap
            mapPanel.Refresh()

            pointsLabel2.Text = "Points In View: " & pointsInView.ToString
            tarXLabel.Text = "X  =  " & target.data(1, 1).ToString("f3")
            tarYLabel.Text = "Y  =  " & target.data(2, 1).ToString("f3")
            tarZLabel.Text = "Z  =  " & target.data(3, 1).ToString("f3")
            camXLabel.Text = "X  =  " & camera.data(1, 1).ToString("f3")
            camYLabel.Text = "Y  =  " & camera.data(2, 1).ToString("f3")
            camZLabel.Text = "Z  =  " & camera.data(3, 1).ToString("f3")
        Catch ex As Exception
            MessageBox.Show("Error in rendering the display, sorry for the inconvenience", "RENDER ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'procedure to draw a single dataset that has already been pre-processed for a reduced number of points
    Private Sub renderView2(ByRef mapPoints As Generic.List(Of cloudPoint))
        'Try
        pointsInView = 0
        Dim plotColor As Color
        Dim lineweight As Integer = 1
        Dim rejectedPoints As Integer = 0
        Dim ScreenBasePt(1) As Integer

        ScreenBasePt(0) = Convert.ToInt32(mapPanel.Width / 2)
        ScreenBasePt(1) = Convert.ToInt32(mapPanel.Height / 2)

        Dim drawingX, drawingY As Integer

        Dim myBrush As New SolidBrush(Color.Black)

        If bbCheckBox.Checked Then
            renderBoundingBox()
        End If

        If targetsFound And displayTargetsCheckBox.Checked Then
            renderTargets()
        End If

        Dim appliedScaleFactor As Double
        Dim drawPoint As Boolean    'used to test to see if points should be drawn when intensity colour is selected, otherwise ALL points are drawn for other colour selections
        Dim bit8 As Boolean = True
        For i As Integer = 0 To mapPoints.Count - 1
            If mapPoints(i).datasetNum = 1 Then
                plotColor = dataset1ColorLabel.BackColor
                lineweight = dataset1LineWeight
            ElseIf mapPoints(i).datasetNum = 2 Then
                plotColor = dataset2ColorLabel.BackColor
                lineweight = dataset2LineWeight
            ElseIf mapPoints(i).datasetNum = 3 Then
                plotColor = dataset3ColorLabel.BackColor
                lineweight = dataset3LineWeight
            ElseIf mapPoints(i).datasetNum = 4 Then
                plotColor = dataset4ColorLabel.BackColor
                lineweight = dataset4LineWeight
            ElseIf mapPoints(i).datasetNum = 5 Then
                plotColor = dataset5ColorLabel.BackColor
                lineweight = dataset5LineWeight
            ElseIf mapPoints(i).datasetNum = 6 Then
                plotColor = dataset6ColorLabel.BackColor
                lineweight = dataset6LineWeight
            ElseIf mapPoints(i).datasetNum = 7 Then
                plotColor = dataset7ColorLabel.BackColor
                lineweight = dataset7LineWeight
            ElseIf mapPoints(i).datasetNum = 8 Then
                plotColor = dataset8ColorLabel.BackColor
                lineweight = dataset8LineWeight
            ElseIf mapPoints(i).datasetNum = 9 Then
                plotColor = dataset9ColorLabel.BackColor
                lineweight = dataset9LineWeight
            ElseIf mapPoints(i).datasetNum = 10 Then
                plotColor = dataset10ColorLabel.BackColor
                lineweight = dataset10LineWeight
            End If

            If mapPoints(i).inView = True Then
                If perspectiveRadioButton.Checked Then
                    appliedScaleFactor = 1.0R
                Else
                    appliedScaleFactor = currentScalefactor
                End If

                drawingX = ScreenBasePt(0) + mapPoints(i).Xm * appliedScaleFactor
                drawingY = ScreenBasePt(1) - mapPoints(i).Ym * appliedScaleFactor

                If drawingX >= 0 And drawingX <= mapPanel.Width And drawingY >= 0 And drawingY <= mapPanel.Height Then

                    Dim newMapPoint As New cloudPoint
                    newMapPoint.X = mapPoints(i).X
                    newMapPoint.Y = mapPoints(i).Y
                    newMapPoint.Z = mapPoints(i).Z
                    newMapPoint.R = mapPoints(i).R
                    newMapPoint.G = mapPoints(i).G
                    newMapPoint.B = mapPoints(i).B
                    newMapPoint.I = mapPoints(i).I
                    newMapPoint.Xm = mapPoints(i).Xm
                    newMapPoint.Ym = mapPoints(i).Ym
                    'newMapPoint.Zm = mapPoints(i).Zm
                    newMapPoint.displayX = drawingX
                    newMapPoint.displayY = drawingY
                    newMapPoint.highlighted = mapPoints(i).highlighted
                    newMapPoint.deleted = mapPoints(i).deleted
                    newMapPoint.datasetNum = mapPoints(i).datasetNum
                    mapPoints(i) = newMapPoint

                    If Not mapPoints(i).deleted Then
                        drawPoint = True

                        If mapPoints(i).highlighted Then
                            myBrush.Color = Color.FromArgb(255, 105, 180)
                        Else
                            If colourComboBox.SelectedIndex = 2 Then
                                myBrush.Color = plotColor
                            ElseIf colourComboBox.SelectedIndex = 1 Then
                                If mapPoints(i).R <= 255 AndAlso mapPoints(i).G <= 255 AndAlso mapPoints(i).B <= 255 And bit8 Then
                                    myBrush.Color = Color.FromArgb(Convert.ToInt32(mapPoints(i).R), Convert.ToInt32(mapPoints(i).G), Convert.ToInt32(mapPoints(i).B))
                                Else
                                    bit8 = False
                                    Dim newR As Integer = Convert.ToInt32(Math.Truncate(mapPoints(i).R / 65536D * 255))
                                    Dim newG As Integer = Convert.ToInt32(Math.Truncate(mapPoints(i).G / 65536D * 255))
                                    Dim newB As Integer = Convert.ToInt32(Math.Truncate(mapPoints(i).B / 65536D * 255))
                                    myBrush.Color = Color.FromArgb(newR, newG, newB)
                                End If
                            ElseIf colourComboBox.SelectedIndex = 0 Then
                                If mapPoints(i).I >= intensityHistMaxMin(1) And mapPoints(i).I <= intensityHistMaxMin(0) Then
                                    'calculates a ramped intensity colour from Bk -> B -> G -> R -> W
                                    myBrush.Color = calcIntensityColour(mapPoints(i).I, intensityHistMaxMin)
                                Else
                                    drawPoint = False
                                End If
                            End If
                        End If
                        If drawPoint = True Then
                            pointsInView += 1
                            mapGraphics.FillRectangle(myBrush, drawingX, drawingY, lineweight, lineweight)
                            'fileGraphics.FillRectangle(myBrush, drawingX * fileScale, drawingY * fileScale, lineweight * fileScale, lineweight * fileScale)
                            'active render code to have blocks of the point cloud(s) drawn more frequently
                            If pointsInView Mod 10000I = 0 And activeRender = True Then
                                mapPanel.BackgroundImage = mapBitmap
                                mapPanel.Refresh()
                            End If
                        End If
                    End If
                End If
            End If
        Next

        'plot current target on the display
        Dim myBrush2 As New SolidBrush(Color.FromArgb(153, 255, 0))
        Dim targetPointDataset As New Generic.List(Of cloudPoint)
        Dim singleMapDataPoint As New cloudPoint
        singleMapDataPoint.X = Convert.ToDouble(target.data(1, 1))
        singleMapDataPoint.Y = Convert.ToDouble(target.data(2, 1))
        singleMapDataPoint.Z = Convert.ToDouble(target.data(3, 1))
        targetPointDataset.Add(singleMapDataPoint)
        calcDisplayXYZ(targetPointDataset)
        drawingX = ScreenBasePt(0) + targetPointDataset(0).Xm * appliedScaleFactor - 2
        drawingY = ScreenBasePt(1) - targetPointDataset(0).Ym * appliedScaleFactor - 2
        mapGraphics.FillRectangle(myBrush2, drawingX, drawingY, 5, 5)
        'fileGraphics.FillRectangle(myBrush2, drawingX * fileScale, drawingY * fileScale, lineweight * fileScale, lineweight * fileScale)

        referenceBitmap = New Bitmap(mapBitmap)
        drawCentrePoint(mapGraphics, Color.LightGray)
        mapPanel.BackgroundImage = mapBitmap
        mapPanel.Refresh()

        pointsLabel2.Text = "Points In View: " & pointsInView.ToString
        tarXLabel.Text = "X  =  " & target.data(1, 1).ToString("f3")
        tarYLabel.Text = "Y  =  " & target.data(2, 1).ToString("f3")
        tarZLabel.Text = "Z  =  " & target.data(3, 1).ToString("f3")
        camXLabel.Text = "X  =  " & camera.data(1, 1).ToString("f3")
        camYLabel.Text = "Y  =  " & camera.data(2, 1).ToString("f3")
        camZLabel.Text = "Z  =  " & camera.data(3, 1).ToString("f3")
        ' Catch ex As Exception
        'MessageBox.Show("Error in rendering the display, sorry for the inconvenience", "RENDER ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try
    End Sub

    Friend Function calcIntensityColour(ByVal intensity As Integer, ByVal histMaxMin() As Integer) As Color
        Dim range As Double = histMaxMin(0) - histMaxMin(1)
        Dim p10 As Double = 0.1 * range + histMaxMin(1)
        Dim p50 As Double = 0.5 * range + histMaxMin(1)
        Dim p90 As Double = 0.9 * range + histMaxMin(1)
        Dim R, G, B As Integer

        Try
            'Red to White Colour Ramp (90% - 100% intensity values)
            If intensity > p90 Then
                R = 255
                G = (Convert.ToInt32((intensity - p90) / (range + histMaxMin(1) - p90) * 255.0R))
                B = G
                'Green to Red Colour Ramp (50% - 90% intensity values)
            ElseIf intensity > p50 Then
                R = (Convert.ToInt32((intensity - p50) / (p90 - p50) * 255.0R))
                G = 255 - R
                B = 0
                'Blue to Green Colour Ramp (10% - 50% intensity values)
            ElseIf intensity > p10 Then
                R = 0
                G = (Convert.ToInt32((intensity - p10) / (p50 - p10) * 255.0R))
                B = 255 - G
                'Black to Blue Colour Ramp (0% - 10% intensity values)
            Else
                R = 0
                G = 0
                B = (Convert.ToInt32((intensity - histMaxMin(1)) / (p10 - histMaxMin(1)) * 255.0R))
            End If
        Catch ex As Exception
            R = 0
            G = 0
            B = 0
        End Try
        Return Color.FromArgb(R, G, B)
    End Function

    Private Sub renderBoundingBox()
        Dim globalPoints As New Generic.List(Of cloudPoint)
        Dim globalPoint As New cloudPoint

        'point 0
        globalPoint.X = globalMaxMinValues(0, 1)
        globalPoint.Y = globalMaxMinValues(1, 1)
        globalPoint.Z = globalMaxMinValues(2, 1)
        globalPoints.Add(globalPoint)

        'point 1
        globalPoint.X = globalMaxMinValues(0, 1)
        globalPoint.Y = globalMaxMinValues(1, 0)
        globalPoint.Z = globalMaxMinValues(2, 1)
        globalPoints.Add(globalPoint)

        'point 2
        globalPoint.X = globalMaxMinValues(0, 0)
        globalPoint.Y = globalMaxMinValues(1, 0)
        globalPoint.Z = globalMaxMinValues(2, 1)
        globalPoints.Add(globalPoint)

        'point 3
        globalPoint.X = globalMaxMinValues(0, 0)
        globalPoint.Y = globalMaxMinValues(1, 1)
        globalPoint.Z = globalMaxMinValues(2, 1)
        globalPoints.Add(globalPoint)

        'point 4
        globalPoint.X = globalMaxMinValues(0, 1)
        globalPoint.Y = globalMaxMinValues(1, 1)
        globalPoint.Z = globalMaxMinValues(2, 0)
        globalPoints.Add(globalPoint)

        'point 5
        globalPoint.X = globalMaxMinValues(0, 1)
        globalPoint.Y = globalMaxMinValues(1, 0)
        globalPoint.Z = globalMaxMinValues(2, 0)
        globalPoints.Add(globalPoint)

        'point 6
        globalPoint.X = globalMaxMinValues(0, 0)
        globalPoint.Y = globalMaxMinValues(1, 0)
        globalPoint.Z = globalMaxMinValues(2, 0)
        globalPoints.Add(globalPoint)

        'point 7
        globalPoint.X = globalMaxMinValues(0, 0)
        globalPoint.Y = globalMaxMinValues(1, 1)
        globalPoint.Z = globalMaxMinValues(2, 0)
        globalPoints.Add(globalPoint)

        If parallelRadioButton.Checked Then
            renderParallelBoundingBox = True
        End If
        calcDisplayXYZ(globalPoints)
        If parallelRadioButton.Checked Then
            renderParallelBoundingBox = False
        End If

        globalLines.Clear()
        Dim cloudLine1, cloudLine2, cloudLine3, cloudLine4, cloudLine5, cloudLine6, cloudLine7, cloudLine8, cloudLine9, cloudLine10, cloudLine11, cloudLine12 As cloudLine

        If globalPoints(0).inView And globalPoints(3).inView Then
            cloudLine1.startX = globalPoints(0).Xm
            cloudLine1.startY = globalPoints(0).Ym
            'cloudLine1.startZ = globalPoints(0).Zm
            cloudLine1.endX = globalPoints(3).Xm
            cloudLine1.endY = globalPoints(3).Ym
            'cloudLine1.endZ = globalPoints(3).Zm
            cloudLine1.lineColour = Color.FromArgb(255, 0, 0)
            cloudLine1.weight = 2
            globalLines.Add(cloudLine1)
        End If
        If globalPoints(1).inView And globalPoints(2).inView Then
            cloudLine2.startX = globalPoints(1).Xm
            cloudLine2.startY = globalPoints(1).Ym
            'cloudLine2.startZ = globalPoints(1).Zm
            cloudLine2.endX = globalPoints(2).Xm
            cloudLine2.endY = globalPoints(2).Ym
            'cloudLine2.endZ = globalPoints(2).Zm
            cloudLine2.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine2.weight = 1
            globalLines.Add(cloudLine2)
        End If
        If globalPoints(4).inView And globalPoints(7).inView Then
            cloudLine3.startX = globalPoints(4).Xm
            cloudLine3.startY = globalPoints(4).Ym
            'cloudLine3.startZ = globalPoints(4).Zm
            cloudLine3.endX = globalPoints(7).Xm
            cloudLine3.endY = globalPoints(7).Ym
            'cloudLine3.endZ = globalPoints(7).Zm
            cloudLine3.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine3.weight = 1
            globalLines.Add(cloudLine3)
        End If
        If globalPoints(6).inView And globalPoints(5).inView Then
            cloudLine4.startX = globalPoints(5).Xm
            cloudLine4.startY = globalPoints(5).Ym
            'cloudLine4.startZ = globalPoints(5).Zm
            cloudLine4.endX = globalPoints(6).Xm
            cloudLine4.endY = globalPoints(6).Ym
            'cloudLine4.endZ = globalPoints(6).Zm
            cloudLine4.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine4.weight = 1
            globalLines.Add(cloudLine4)
        End If
        If globalPoints(0).inView And globalPoints(1).inView Then
            cloudLine5.startX = globalPoints(0).Xm
            cloudLine5.startY = globalPoints(0).Ym
            'cloudLine5.startZ = globalPoints(0).Zm
            cloudLine5.endX = globalPoints(1).Xm
            cloudLine5.endY = globalPoints(1).Ym
            'cloudLine5.endZ = globalPoints(1).Zm
            cloudLine5.lineColour = Color.FromArgb(0, 255, 0)
            cloudLine5.weight = 2
            globalLines.Add(cloudLine5)
        End If
        If globalPoints(2).inView And globalPoints(3).inView Then
            cloudLine6.startX = globalPoints(2).Xm
            cloudLine6.startY = globalPoints(2).Ym
            'cloudLine6.startZ = globalPoints(2).Zm
            cloudLine6.endX = globalPoints(3).Xm
            cloudLine6.endY = globalPoints(3).Ym
            'cloudLine6.endZ = globalPoints(3).Zm
            cloudLine6.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine6.weight = 1
            globalLines.Add(cloudLine6)
        End If
        If globalPoints(4).inView And globalPoints(5).inView Then
            cloudLine7.startX = globalPoints(4).Xm
            cloudLine7.startY = globalPoints(4).Ym
            'cloudLine7.startZ = globalPoints(4).Zm
            cloudLine7.endX = globalPoints(5).Xm
            cloudLine7.endY = globalPoints(5).Ym
            'cloudLine7.endZ = globalPoints(5).Zm
            cloudLine7.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine7.weight = 1
            globalLines.Add(cloudLine7)
        End If
        If globalPoints(6).inView And globalPoints(7).inView Then
            cloudLine8.startX = globalPoints(6).Xm
            cloudLine8.startY = globalPoints(6).Ym
            'cloudLine8.startZ = globalPoints(6).Zm
            cloudLine8.endX = globalPoints(7).Xm
            cloudLine8.endY = globalPoints(7).Ym
            'cloudLine8.endZ = globalPoints(7).Zm
            cloudLine8.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine8.weight = 1
            globalLines.Add(cloudLine8)
        End If
        If globalPoints(0).inView And globalPoints(4).inView Then
            cloudLine9.startX = globalPoints(0).Xm
            cloudLine9.startY = globalPoints(0).Ym
            'cloudLine9.startZ = globalPoints(0).Zm
            cloudLine9.endX = globalPoints(4).Xm
            cloudLine9.endY = globalPoints(4).Ym
            'cloudLine9.endZ = globalPoints(4).Zm
            cloudLine9.lineColour = Color.FromArgb(0, 0, 255)
            cloudLine9.weight = 2
            globalLines.Add(cloudLine9)
        End If
        If globalPoints(1).inView And globalPoints(5).inView Then
            cloudLine10.startX = globalPoints(1).Xm
            cloudLine10.startY = globalPoints(1).Ym
            'cloudLine10.startZ = globalPoints(1).Zm
            cloudLine10.endX = globalPoints(5).Xm
            cloudLine10.endY = globalPoints(5).Ym
            'cloudLine10.endZ = globalPoints(5).Zm
            cloudLine10.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine10.weight = 1
            globalLines.Add(cloudLine10)
        End If
        If globalPoints(2).inView And globalPoints(6).inView Then
            cloudLine11.startX = globalPoints(2).Xm
            cloudLine11.startY = globalPoints(2).Ym
            'cloudLine11.startZ = globalPoints(2).Zm
            cloudLine11.endX = globalPoints(6).Xm
            cloudLine11.endY = globalPoints(6).Ym
            'cloudLine11.endZ = globalPoints(6).Zm
            cloudLine11.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine11.weight = 1
            globalLines.Add(cloudLine11)
        End If
        If globalPoints(7).inView And globalPoints(3).inView Then
            cloudLine12.startX = globalPoints(3).Xm
            cloudLine12.startY = globalPoints(3).Ym
            'cloudLine12.startZ = globalPoints(3).Zm
            cloudLine12.endX = globalPoints(7).Xm
            cloudLine12.endY = globalPoints(7).Ym
            'cloudLine12.endZ = globalPoints(7).Zm
            cloudLine12.lineColour = Color.FromArgb(255, 255, 255)
            cloudLine12.weight = 1
            globalLines.Add(cloudLine12)
        End If
        renderLines(globalLines)
    End Sub

    Private Sub renderTargets()
        For i As Integer = 0 To targetCircles.Count - 1
            calcDisplayXYZ(targetCircles(i).edgePoints)
            Dim centrePointList As New Generic.List(Of cloudPoint)
            centrePointList.Add(targetCircles(i).centrePoint)
            calcDisplayXYZ(centrePointList)


            Dim circleArcs As New Generic.List(Of cloudLine)
            Dim line As New cloudLine
            Dim j As Integer

            For j = 0 To targetCircles(i).edgePoints.Count - 2
                line.startX = targetCircles(i).edgePoints(j).Xm
                line.startY = targetCircles(i).edgePoints(j).Ym
                'line.startZ = targetCircles(i).edgePoints(j).Zm
                line.endX = targetCircles(i).edgePoints(j + 1).Xm
                line.endY = targetCircles(i).edgePoints(j + 1).Ym
                'line.endZ = targetCircles(i).edgePoints(j + 1).Zm
                line.lineColour = processedTargetsColorLabel.BackColor  'targetCircles(i).colour
                line.weight = targetsLineWeight 'targetCircles(i).weight
                circleArcs.Add(line)
            Next
            line.startX = targetCircles(i).edgePoints(j).Xm
            line.startY = targetCircles(i).edgePoints(j).Ym
            'line.startZ = targetCircles(i).edgePoints(j).Zm
            line.endX = targetCircles(i).edgePoints(0).Xm
            line.endY = targetCircles(i).edgePoints(0).Ym
            'line.endZ = targetCircles(i).edgePoints(0).Zm
            line.lineColour = processedTargetsColorLabel.BackColor  'targetCircles(i).colour
            line.weight = targetsLineWeight 'targetCircles(i).weight
            circleArcs.Add(line)

            Dim ScreenBasePt(1) As Integer

            ScreenBasePt(0) = Convert.ToInt32(mapPanel.Width / 2)
            ScreenBasePt(1) = Convert.ToInt32(mapPanel.Height / 2)

            Dim X, Y As Integer
            Dim myPen As New Pen(Color.Black)

            Dim appliedScaleFactor As Double

            If perspectiveRadioButton.Checked Then
                appliedScaleFactor = 1.0R
            Else
                appliedScaleFactor = currentScalefactor
            End If
            X = ScreenBasePt(0) + centrePointList(0).Xm * appliedScaleFactor
            Y = ScreenBasePt(1) - centrePointList(0).Ym * appliedScaleFactor

            Dim textBrush As New SolidBrush(processedTargetsColorLabel.BackColor)
            Dim sfCentre As New StringFormat()
            sfCentre.Alignment = StringAlignment.Center
            sfCentre.LineAlignment = StringAlignment.Center
            Dim axisFont As New Font("arial", 16, FontStyle.Bold, GraphicsUnit.Pixel)

            If X <= mapPanel.Width AndAlso X >= 0 AndAlso Y <= mapPanel.Height AndAlso Y >= 0 Then
                mapGraphics.DrawString((i + 1).ToString, axisFont, textBrush, X, Y, sfCentre)
                'fileGraphics.DrawString((i + 1).ToString, axisFont, textBrush, X, Y, sfCentre)
            End If

            renderLines(circleArcs)
        Next
    End Sub

    Private Sub renderLines(ByVal mapLines As Generic.List(Of cloudLine))
        Dim ScreenBasePt(1) As Integer

        ScreenBasePt(0) = Convert.ToInt32(mapPanel.Width / 2)
        ScreenBasePt(1) = Convert.ToInt32(mapPanel.Height / 2)

        Dim startX, startY, endX, endY As Integer
        Dim myPen As New Pen(Color.Black)

        Dim appliedScaleFactor As Double

        If perspectiveRadioButton.Checked Then
            appliedScaleFactor = 1.0R
        Else
            appliedScaleFactor = currentScalefactor
        End If

        For i As Integer = 0 To mapLines.Count - 1
            startX = ScreenBasePt(0) + mapLines(i).startX * appliedScaleFactor
            startY = ScreenBasePt(1) - mapLines(i).startY * appliedScaleFactor
            endX = ScreenBasePt(0) + mapLines(i).endX * appliedScaleFactor
            endY = ScreenBasePt(1) - mapLines(i).endY * appliedScaleFactor
            myPen.Color = mapLines(i).lineColour
            myPen.Width = mapLines(i).weight

            If startX <= mapPanel.Width AndAlso startX >= 0 AndAlso startY <= mapPanel.Height AndAlso startY >= 0 AndAlso endX <= mapPanel.Width AndAlso endX >= 0 AndAlso endY <= mapPanel.Height AndAlso endY >= 0 Then
                mapGraphics.DrawLine(myPen, startX, startY, endX, endY)
                'fileGraphics.DrawLine(myPen, startX, startY, endX, endY)
            End If
        Next

    End Sub

    'Procedure to Clear the Map Display
    Private Sub clearMapButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearMapButton.Click, ClearDisplayToolStripMenuItem.Click
        pointsInView = 0
        mapGraphics.Clear(Color.FromArgb(40, 40, 40))
        'fileGraphics.Clear(Color.FromArgb(40, 40, 40))
        drawCentrePoint(mapGraphics, Color.LightGray)
        mapPanel.BackgroundImage = mapBitmap
        mapPanel.Refresh()
        mapDisplayed = False
    End Sub

    'procedure that facilitates zooming and panning the map data
    Private Sub zoomInOut(ByVal InOrOut As Integer) '1 = zoomIn, 2 = zoomOut

        If mapDisplayed = True Then
            Dim oldFormCursor As Cursor = Me.Cursor
            Dim oldMapCursor As Cursor = mapPanel.Cursor
            Me.Cursor = Cursors.AppStarting
            mapPanel.Cursor = Cursors.AppStarting
            Dim cam2tarDist As Decimal = Matrix.abs(camera - target)
            Dim zoomOutAllowed As Boolean = True
            Dim zoomInAllowed As Boolean = True

            If Decimal.Round(cam2tarDist, 2) <= 0.05D Then
                statusLabel.Text = "You have reach the maximum zoom-in level, you cannot zoom-in on the current target any further"
                zoomInAllowed = False
                zoomOutAllowed = True
            ElseIf cam2tarDist > 100000D Then
                statusLabel.Text = "You have reach the maximum zoom-out level, you cannot zoom-out from the current target any further"
                zoomInAllowed = True
                zoomOutAllowed = False
            Else
                zoomInAllowed = True
                zoomOutAllowed = True
            End If

            Dim e As New System.EventArgs
            If InOrOut = 1 And zoomInAllowed Then
                'clearMapButton_Click(sender, e)
                statusLabel.Text = String.Empty
                'currentScalefactor *= (1 + adjustmentLevel / 10.0)
                camera = camera + ((adjustmentLevel - 1.0R) / 10.0) * cam2tarDist * dir2target
                cam2tarDist = Matrix.abs(camera - target)
                If cam2tarDist <= 0.05 Then
                    camera = target + 0.05 * (-1 * dir2target)
                End If
                currentScalefactor = calcScaleFactor()
                detailLevel = 10
                updateCurrentDataset(e)
                drawMapButton_Click(refreshMapButton, e)
                'reset timer object
                Timer1.Stop()
                Timer1.Dispose()
                Timer1.Start()
            ElseIf InOrOut = 2 And zoomOutAllowed Then
                'clearMapButton_Click(sender, e)
                statusLabel.Text = String.Empty
                'currentScalefactor /= (1 + adjustmentLevel / 10.0)
                'If Decimal.Round(cam2tarDist, 2) = 0D Then
                '    cam2tarDist = 1.63
                'End If
                camera += ((adjustmentLevel - 1.0R) / 10.0) * cam2tarDist * (-1 * dir2target)
                currentScalefactor = calcScaleFactor()
                detailLevel = 10
                updateCurrentDataset(e)
                drawMapButton_Click(refreshMapButton, e)
                Timer1.Stop()
                Timer1.Dispose()
                Timer1.Start()
            End If
            Me.Cursor = oldFormCursor
            mapPanel.Cursor = oldMapCursor
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Not isMouseButtonDown Then
            Timer1.Stop()
            Timer1.Dispose()
            insideNavigate = False
            dataset1LineWeight = initialLineWeights(0)
            dataset2LineWeight = initialLineWeights(1)
            dataset3LineWeight = initialLineWeights(2)
            dataset4LineWeight = initialLineWeights(3)
            dataset5LineWeight = initialLineWeights(4)
            dataset6LineWeight = initialLineWeights(5)
            dataset7LineWeight = initialLineWeights(6)
            dataset8LineWeight = initialLineWeights(7)
            dataset9LineWeight = initialLineWeights(8)
            dataset10LineWeight = initialLineWeights(9)
            detailLevel = reducePtsComboBox.SelectedIndex
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    'procedure to refresh a single selected dataset within the current view of the map
    Private Sub refreshSelectedButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles refreshSelectedButton.Click
        If mapDisplayed = True Then
            Dim oldFormCursor As Cursor = Me.Cursor
            Dim oldMapCursor As Cursor = mapPanel.Cursor
            Me.Cursor = Cursors.AppStarting
            mapPanel.Cursor = Cursors.AppStarting

            If dataset1CheckBox.BackColor = Color.CornflowerBlue And dataset1CheckBox.Visible = True And dataset1CheckBox.Checked = True Then
                If dataset1Calcd = False Then
                    dataset1Calcd = True
                    calcDisplayXYZ(dataset1MapDataPoints)
                End If
                renderView(dataset1MapDataPoints, dataset1ColorLabel.BackColor, dataset1LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset2CheckBox.BackColor = Color.CornflowerBlue And dataset2CheckBox.Visible = True And dataset2CheckBox.Checked = True Then
                If dataset2Calcd = False Then
                    dataset2Calcd = True
                    calcDisplayXYZ(dataset2MapDataPoints)
                End If
                renderView(dataset2MapDataPoints, dataset2ColorLabel.BackColor, dataset2LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset3CheckBox.BackColor = Color.CornflowerBlue And dataset3CheckBox.Visible = True And dataset3CheckBox.Checked = True Then
                If dataset3Calcd = False Then
                    dataset3Calcd = True
                    calcDisplayXYZ(dataset3MapDataPoints)
                End If
                renderView(dataset3MapDataPoints, dataset3ColorLabel.BackColor, dataset3LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset4CheckBox.BackColor = Color.CornflowerBlue And dataset4CheckBox.Visible = True And dataset4CheckBox.Checked = True Then
                If dataset4Calcd = False Then
                    dataset4Calcd = True
                    calcDisplayXYZ(dataset4MapDataPoints)
                End If
                renderView(dataset4MapDataPoints, dataset4ColorLabel.BackColor, dataset4LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset5CheckBox.BackColor = Color.CornflowerBlue And dataset5CheckBox.Visible = True And dataset5CheckBox.Checked = True Then
                If dataset5Calcd = False Then
                    dataset5Calcd = True
                    calcDisplayXYZ(dataset5MapDataPoints)
                End If
                renderView(dataset5MapDataPoints, dataset5ColorLabel.BackColor, dataset5LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset6CheckBox.BackColor = Color.CornflowerBlue And dataset6CheckBox.Visible = True And dataset6CheckBox.Checked = True Then
                If dataset6Calcd = False Then
                    dataset6Calcd = True
                    calcDisplayXYZ(dataset6MapDataPoints)
                End If
                renderView(dataset6MapDataPoints, dataset6ColorLabel.BackColor, dataset6LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset7CheckBox.BackColor = Color.CornflowerBlue And dataset7CheckBox.Visible = True And dataset7CheckBox.Checked = True Then
                If dataset7Calcd = False Then
                    dataset7Calcd = True
                    calcDisplayXYZ(dataset7MapDataPoints)
                End If
                renderView(dataset7MapDataPoints, dataset7ColorLabel.BackColor, dataset7LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset8CheckBox.BackColor = Color.CornflowerBlue And dataset8CheckBox.Visible = True And dataset8CheckBox.Checked = True Then
                If dataset8Calcd = False Then
                    dataset8Calcd = True
                    calcDisplayXYZ(dataset8MapDataPoints)
                End If
                renderView(dataset8MapDataPoints, dataset8ColorLabel.BackColor, dataset8LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset9CheckBox.BackColor = Color.CornflowerBlue And dataset9CheckBox.Visible = True And dataset9CheckBox.Checked = True Then
                If dataset9Calcd = False Then
                    dataset9Calcd = True
                    calcDisplayXYZ(dataset9MapDataPoints)
                End If
                renderView(dataset9MapDataPoints, dataset9ColorLabel.BackColor, dataset9LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            ElseIf dataset10CheckBox.BackColor = Color.CornflowerBlue And dataset10CheckBox.Visible = True And dataset10CheckBox.Checked = True Then
                If dataset10Calcd = False Then
                    dataset10Calcd = True
                    calcDisplayXYZ(dataset10MapDataPoints)
                End If
                renderView(dataset10MapDataPoints, dataset10ColorLabel.BackColor, dataset10LineWeight, currentMaxMinValues, currentCentreValue)
                mapPanel.BackgroundImage = mapBitmap
                mapPanel.Refresh()

                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            Else
                MessageBox.Show("You must select a loaded dataset that is checked for display from the list before it can be refreshed!", "SELECT LOADED DATASET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Me.Cursor = oldFormCursor
                mapPanel.Cursor = oldMapCursor
            End If
            pointsLabel2.Text = "Points In View: " & pointsInView.ToString
        End If
    End Sub

    'procedure to perform test to see which map projection math function should be used and update the List's mapPoint N,E coordinates
    Private Sub calcDisplayXYZ(ByRef mapDataPoints As Generic.List(Of cloudPoint))
        Try
            Dim pointXYZ As New Matrix(3, 1, True)
            Dim dir2Point As New Matrix(3, 1, True)

            If Matrix.abs(camera - target) <> 0D Then
                dir2target = (target - camera) / Matrix.abs(camera - target)
            Else
                dir2target.data(1, 1) = 0
                dir2target.data(2, 1) = 0
                dir2target.data(3, 1) = -1
            End If

            Dim centreOfImage As New Matrix(3, 1, True)
            centreOfImage = camera + focalLength * (-1 * dir2target)
            Dim dist2ImageXYZ As Decimal
            Dim imageXYZ As New Matrix(3, 1, True)

            Dim newImageXYZ As New Matrix(3, 1, True)

            Dim centreToImagePt As New Matrix(3, 1, True)

            If Decimal.Round(dir2target.data(3, 1), 5) = 1D Or Decimal.Round(dir2target.data(3, 1), 5) = -1D Then
                If imagePlaneXYsolved = False Then
                    imagePlaneX.data(1, 1) = 1
                    imagePlaneX.data(2, 1) = 0
                    imagePlaneX.data(3, 1) = 0
                    imagePlaneY.data(1, 1) = 0
                    imagePlaneY.data(2, 1) = 1
                    imagePlaneY.data(3, 1) = 0
                    imagePlaneXYsolved = True
                Else
                    'maintain same overall Y direction of the image plane within the global XY plane but force a 0 Z component
                    imagePlaneY.data(3, 1) = 0
                    imagePlaneY = imagePlaneY / Matrix.abs(imagePlaneY) 'normalize to a unit vector
                End If
            Else
                imagePlaneX = Matrix.cross(dir2target, upDirection)
                imagePlaneX = imagePlaneX / Matrix.abs(imagePlaneX) 'normalize to a unit vector
                imagePlaneY = Matrix.cross((-1 * dir2target), imagePlaneX)
                imagePlaneY = imagePlaneY / Matrix.abs(imagePlaneY) 'normalize to a unit vector
            End If

            Dim pointCalcd As Boolean

            Dim appliedScaleFactor As Double

            If perspectiveRadioButton.Checked Then
                appliedScaleFactor = 1.0R
            Else
                appliedScaleFactor = currentScalefactor
            End If

            Dim appliedFOV As Double
            For i As Integer = 0 To mapDataPoints.Count - 1
                pointCalcd = True
                pointXYZ.data(1, 1) = mapDataPoints(i).X
                pointXYZ.data(2, 1) = mapDataPoints(i).Y
                pointXYZ.data(3, 1) = mapDataPoints(i).Z

                If parallelRadioButton.Checked Then
                    'Parallel View Mode
                    appliedFOV = 180.0R
                    dir2Point = -1 * dir2target
                    Try
                        dist2ImageXYZ = Matrix.dot((centreOfImage - pointXYZ), (-1 * dir2target)) / Matrix.dot(dir2Point, (-1 * dir2target))
                        imageXYZ = pointXYZ + dist2ImageXYZ * dir2Point
                        'imageXYZ.data(1, 1) = -1 * imageXYZ.data(1, 1)
                    Catch
                        pointCalcd = False
                    End Try
                Else
                    'Perspective View Mode
                    appliedFOV = FOV
                    If Matrix.abs(pointXYZ - camera) <> 0 Then
                        dir2Point = (pointXYZ - camera) / Matrix.abs(pointXYZ - camera)
                        dist2ImageXYZ = Matrix.dot((centreOfImage - camera), (-1 * dir2target)) / Matrix.dot((-1 * dir2Point), (-1 * dir2target))
                        imageXYZ = (camera + dist2ImageXYZ * (-1 * dir2Point))
                    Else
                        pointCalcd = False
                    End If
                End If

                Dim newMapDataPoint As cloudPoint
                newMapDataPoint.X = mapDataPoints(i).X
                newMapDataPoint.Y = mapDataPoints(i).Y
                newMapDataPoint.Z = mapDataPoints(i).Z
                newMapDataPoint.I = mapDataPoints(i).I
                newMapDataPoint.R = mapDataPoints(i).R
                newMapDataPoint.G = mapDataPoints(i).G
                newMapDataPoint.B = mapDataPoints(i).B
                newMapDataPoint.highlighted = mapDataPoints(i).highlighted
                newMapDataPoint.deleted = mapDataPoints(i).deleted
                newMapDataPoint.datasetNum = mapDataPoints(i).datasetNum

                'Determine X and Y coordinates for the ray intersections for each individual point cloud point on the 'image' plane
                If pointCalcd And Matrix.abs(pointXYZ - camera) <> 0 Then

                    Dim dir2PointTest As New Matrix(3, 1, True)
                    dir2PointTest = (pointXYZ - camera) / Matrix.abs(pointXYZ - camera)
                    Dim viewAngle As Double = 0
                    viewAngle = Math.Acos(Matrix.dot(dir2target, dir2PointTest) / (Matrix.abs(dir2target) * Matrix.abs(dir2PointTest)))
                    viewAngle *= 180.0R / Math.PI
                    If Not renderParallelBoundingBox Then
                        If viewAngle <= appliedFOV / 2 Then
                            newMapDataPoint.inView = True
                        Else
                            newMapDataPoint.inView = False
                        End If
                    Else
                        newMapDataPoint.inView = True
                    End If
                    centreToImagePt = (imageXYZ - centreOfImage)
                    If perspectiveRadioButton.Checked Then
                        'multiply by 1000 due to 500 pixels being expressed as equal to 0.500m and we want imageXYZ back in pixel units
                        'multiply by minus(-) to invert the image to proper viewing orientation
                        newImageXYZ.data(1, 1) = -625 * Matrix.dot(centreToImagePt, imagePlaneX) - imageOffsets(0)
                        newImageXYZ.data(2, 1) = -625 * Matrix.dot(centreToImagePt, imagePlaneY) - imageOffsets(1)
                    Else    'parallel view
                        newImageXYZ.data(1, 1) = Matrix.dot(centreToImagePt, imagePlaneX) - imageOffsets(0)
                        newImageXYZ.data(2, 1) = Matrix.dot(centreToImagePt, imagePlaneY) - imageOffsets(1)
                    End If
                    newImageXYZ.data(3, 1) = 0
                    newMapDataPoint.Xm = newImageXYZ.data(1, 1)
                    newMapDataPoint.Ym = newImageXYZ.data(2, 1)
                    'newMapDataPoint.Zm = newImageXYZ.data(3, 1)
                    newMapDataPoint.displayX = mapPanel.Width / 2 + newMapDataPoint.Xm * appliedScaleFactor
                    newMapDataPoint.displayY = mapPanel.Height / 2 - newMapDataPoint.Ym * appliedScaleFactor
                Else
                    newMapDataPoint.Xm = 0
                    newMapDataPoint.Ym = 0
                    'newMapDataPoint.Zm = 0
                    newMapDataPoint.displayX = 0
                    newMapDataPoint.displayY = 0
                    newMapDataPoint.inView = False
                End If
                mapDataPoints(i) = newMapDataPoint
            Next
        Catch ex As Exception
        End Try
    End Sub

    'procedure to centre of the Map at a user selected location via a Short-cut menu
    Private Sub CentreMapOnThisLocationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CentreMapOnThisLocationToolStripMenuItem.Click
        If mapDisplayed = True Then
            Dim oldFormCursor As Cursor = Me.Cursor
            Dim oldMapCursor As Cursor = mapPanel.Cursor
            Me.Cursor = Cursors.AppStarting
            mapPanel.Cursor = Cursors.AppStarting

            clearMapButton_Click(sender, e)

            imageOffsets(0) += centreOfMouseMap(0)
            imageOffsets(1) += centreOfMouseMap(1)

            'new logic
            'Dim dLR As Double = centreOfMouseMap(0) / 1000 * Matrix.abs(target - camera) / focalLength
            'Dim dUD As Double = centreOfMouseMap(1) / 1000 * Matrix.abs(target - camera) / focalLength

            'target = target + dLR * imagePlaneX
            'target = target + dUD * imagePlaneY

            'camera = camera + dLR * imagePlaneX
            'camera = camera + dUD * imagePlaneY

            'old logic
            'currentCentreValue(0) = (centreOfMouseMap(1) + 1) / currentScalefactor + currentCentreValue(0)
            'currentCentreValue(1) = (centreOfMouseMap(0) - 1) / currentScalefactor + currentCentreValue(1)

            'If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
            '    renderView(dataset1MapDataPoints, dataset1ColorLabel.BackColor, dataset1LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
            '    renderView(dataset2MapDataPoints, dataset2ColorLabel.BackColor, dataset2LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
            '    renderView(dataset3MapDataPoints, dataset3ColorLabel.BackColor, dataset3LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
            '    renderView(dataset4MapDataPoints, dataset4ColorLabel.BackColor, dataset4LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
            '    renderView(dataset5MapDataPoints, dataset5ColorLabel.BackColor, dataset5LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
            '    renderView(dataset6MapDataPoints, dataset6ColorLabel.BackColor, dataset6LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
            '    renderView(dataset7MapDataPoints, dataset7ColorLabel.BackColor, dataset7LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
            '    renderView(dataset8MapDataPoints, dataset8ColorLabel.BackColor, dataset8LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
            '    renderView(dataset9MapDataPoints, dataset9ColorLabel.BackColor, dataset9LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If
            'If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
            '    renderView(dataset10MapDataPoints, dataset10ColorLabel.BackColor, dataset10LineWeight, currentMaxMinValues, currentCentreValue)
            '    mapDisplayed = True
            'End If

            'mapPanel.BackgroundImage = mapBitmap
            'mapPanel.Refresh()
            drawMapButton_Click(refreshMapButton, e)
            Me.Cursor = oldFormCursor
            mapPanel.Cursor = oldMapCursor
        End If

    End Sub

    'procedure to change the cursor state while on the map from panning to cross cursors (depending on the user entering pan mode)
    Private Sub panCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles panCheckBox.CheckedChanged
        If Not insideDrawBox Then
            If panCheckBox.Checked = True Then
                mapPanel.Cursor = Cursors.Hand
            Else
                mapPanel.Cursor = Cursors.Cross
            End If
        Else
            panCheckBox.Checked = False
            MessageBox.Show("You cannot enter sliding mode while drawing a selection box", "SELECTION MODE PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    'procedures to visually highlight a single loaded dataset without toggling the checkbox off or on
    Private Sub dataset1CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset1CheckBox.MouseClick
        If e.X > 12 Then
            If dataset1CheckBox.Checked = True Then
                dataset1CheckBox.Checked = False
            Else
                dataset1CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.CornflowerBlue
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset2CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset2CheckBox.MouseClick
        If e.X > 12 Then
            If dataset2CheckBox.Checked = True Then
                dataset2CheckBox.Checked = False
            Else
                dataset2CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.CornflowerBlue
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset3CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset3CheckBox.MouseClick
        If e.X > 12 Then
            If dataset3CheckBox.Checked = True Then
                dataset3CheckBox.Checked = False
            Else
                dataset3CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.CornflowerBlue
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset4CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset4CheckBox.MouseClick
        If e.X > 12 Then
            If dataset4CheckBox.Checked = True Then
                dataset4CheckBox.Checked = False
            Else
                dataset4CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.CornflowerBlue
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset5CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset5CheckBox.MouseClick
        If e.X > 12 Then
            If dataset5CheckBox.Checked = True Then
                dataset5CheckBox.Checked = False
            Else
                dataset5CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.CornflowerBlue
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset6CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset6CheckBox.MouseClick
        If e.X > 12 Then
            If dataset6CheckBox.Checked = True Then
                dataset6CheckBox.Checked = False
            Else
                dataset6CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.CornflowerBlue
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset7CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset7CheckBox.MouseClick
        If e.X > 12 Then
            If dataset7CheckBox.Checked = True Then
                dataset7CheckBox.Checked = False
            Else
                dataset7CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.CornflowerBlue
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset8CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset8CheckBox.MouseClick
        If e.X > 12 Then
            If dataset8CheckBox.Checked = True Then
                dataset8CheckBox.Checked = False
            Else
                dataset8CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.CornflowerBlue
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset9CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset9CheckBox.MouseClick
        If e.X > 12 Then
            If dataset9CheckBox.Checked = True Then
                dataset9CheckBox.Checked = False
            Else
                dataset9CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.CornflowerBlue
            dataset10CheckBox.BackColor = Color.White
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub
    Private Sub dataset10CheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dataset10CheckBox.MouseClick
        If e.X > 12 Then
            If dataset10CheckBox.Checked = True Then
                dataset10CheckBox.Checked = False
            Else
                dataset10CheckBox.Checked = True
            End If
            dataset1CheckBox.BackColor = Color.White
            dataset2CheckBox.BackColor = Color.White
            dataset3CheckBox.BackColor = Color.White
            dataset4CheckBox.BackColor = Color.White
            dataset5CheckBox.BackColor = Color.White
            dataset6CheckBox.BackColor = Color.White
            dataset7CheckBox.BackColor = Color.White
            dataset8CheckBox.BackColor = Color.White
            dataset9CheckBox.BackColor = Color.White
            dataset10CheckBox.BackColor = Color.CornflowerBlue
        Else
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    'procedure to update Tooltips for the loaded datasets
    Private Sub dataset1CheckBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dataset1CheckBox.TextChanged, dataset2CheckBox.TextChanged, dataset3CheckBox.TextChanged, dataset4CheckBox.TextChanged, dataset5CheckBox.TextChanged, dataset6CheckBox.TextChanged, dataset7CheckBox.TextChanged, dataset8CheckBox.TextChanged, dataset9CheckBox.TextChanged, dataset10CheckBox.TextChanged
        Dim checkBoxSender As CheckBox = CType(sender, CheckBox)
        If checkBoxSender.Name = "dataset1CheckBox" Then
            ToolTip1.SetToolTip(dataset1CheckBox, dataset1CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset2CheckBox" Then
            ToolTip1.SetToolTip(dataset2CheckBox, dataset2CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset3CheckBox" Then
            ToolTip1.SetToolTip(dataset3CheckBox, dataset3CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset4CheckBox" Then
            ToolTip1.SetToolTip(dataset4CheckBox, dataset4CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset5CheckBox" Then
            ToolTip1.SetToolTip(dataset5CheckBox, dataset5CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset6CheckBox" Then
            ToolTip1.SetToolTip(dataset6CheckBox, dataset6CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset7CheckBox" Then
            ToolTip1.SetToolTip(dataset7CheckBox, dataset7CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset8CheckBox" Then
            ToolTip1.SetToolTip(dataset8CheckBox, dataset8CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset9CheckBox" Then
            ToolTip1.SetToolTip(dataset9CheckBox, dataset9CheckBox.Text)
        ElseIf checkBoxSender.Name = "dataset10CheckBox" Then
            ToolTip1.SetToolTip(dataset10CheckBox, dataset10CheckBox.Text)
        End If
    End Sub

    'procedure for convert and formatting Dec.Deg into Deg-Min-Sec Lat or Long values
    Private Function Dec2DMS(ByVal decDegree As Double, ByVal LATorLONG As Integer) As String
        Dim tempDegDec As Decimal = Convert.ToDecimal(decDegree)

        Dim negTest As Boolean = False
        If tempDegDec < 0D Then
            tempDegDec *= -1
            negTest = True
        End If

        Dim NbdyD As Decimal = Math.Truncate(tempDegDec)
        Dim NbdyM As Decimal = Math.Truncate((tempDegDec - NbdyD) * 60D)
        Dim NbdyS As Decimal = ((((tempDegDec - NbdyD) * 60D) - NbdyM) * 60D)

        If NbdyS = 60D Then
            NbdyS = 0D
            NbdyM += 1D
        End If

        If NbdyM = 60D Then
            NbdyM = 0D
            NbdyD += 1D
        End If

        Dim hemisphereSuffix As String = ""

        If LATorLONG = 1 Then       'latitude to be displayed
            hemisphereSuffix = " N"
        ElseIf LATorLONG = 2 Then   'longitude to be displayed
            hemisphereSuffix = " E"
        ElseIf LATorLONG = 3 Then   'angle only to be displayed
            hemisphereSuffix = String.Empty
        End If

        If negTest = True Then
            'NbdyD *= -1
            If LATorLONG = 1 Then
                hemisphereSuffix = " S"
            ElseIf LATorLONG = 2 Then
                hemisphereSuffix = " W"
            ElseIf LATorLONG = 3 Then   'angle only to be displayed
                hemisphereSuffix = String.Empty
                NbdyD *= -1
            End If
        End If

        Return NbdyD.ToString & Chr(176) & " " & NbdyM.ToString("00") & "' " & NbdyS.ToString("00.00") & """" & hemisphereSuffix

    End Function

    'procedures to determine which dataset is to be shown at the Map Extents
    Private Sub datasetContextMenuStrip_Opened(ByVal sender As Object, ByVal e As System.EventArgs) Handles datasetContextMenuStrip.Opened
        Dim senderControl As ContextMenuStrip = CType(sender, ContextMenuStrip)
        If senderControl.SourceControl.Name = "dataset1CheckBox" Then
            currentRightClickDataset = 1
        ElseIf senderControl.SourceControl.Name = "dataset2CheckBox" Then
            currentRightClickDataset = 2
        ElseIf senderControl.SourceControl.Name = "dataset3CheckBox" Then
            currentRightClickDataset = 3
        ElseIf senderControl.SourceControl.Name = "dataset4CheckBox" Then
            currentRightClickDataset = 4
        ElseIf senderControl.SourceControl.Name = "dataset5CheckBox" Then
            currentRightClickDataset = 5
        ElseIf senderControl.SourceControl.Name = "dataset6CheckBox" Then
            currentRightClickDataset = 6
        ElseIf senderControl.SourceControl.Name = "dataset7CheckBox" Then
            currentRightClickDataset = 7
        ElseIf senderControl.SourceControl.Name = "dataset8CheckBox" Then
            currentRightClickDataset = 8
        ElseIf senderControl.SourceControl.Name = "dataset9CheckBox" Then
            currentRightClickDataset = 9
        ElseIf senderControl.SourceControl.Name = "dataset10CheckBox" Then
            currentRightClickDataset = 10
        End If
    End Sub
    Private Sub ZoomInOnThisDatasetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomInOnThisDatasetToolStripMenuItem.Click
        Dim datasetExtents(,) As Double

        If currentRightClickDataset = 1 Then
            datasetExtents = ComputeMaxMin(dataset1MapDataPoints)
        ElseIf currentRightClickDataset = 2 Then
            datasetExtents = ComputeMaxMin(dataset2MapDataPoints)
        ElseIf currentRightClickDataset = 3 Then
            datasetExtents = ComputeMaxMin(dataset3MapDataPoints)
        ElseIf currentRightClickDataset = 4 Then
            datasetExtents = ComputeMaxMin(dataset4MapDataPoints)
        ElseIf currentRightClickDataset = 5 Then
            datasetExtents = ComputeMaxMin(dataset5MapDataPoints)
        ElseIf currentRightClickDataset = 6 Then
            datasetExtents = ComputeMaxMin(dataset6MapDataPoints)
        ElseIf currentRightClickDataset = 7 Then
            datasetExtents = ComputeMaxMin(dataset7MapDataPoints)
        ElseIf currentRightClickDataset = 8 Then
            datasetExtents = ComputeMaxMin(dataset8MapDataPoints)
        ElseIf currentRightClickDataset = 9 Then
            datasetExtents = ComputeMaxMin(dataset9MapDataPoints)
        Else
            datasetExtents = ComputeMaxMin(dataset10MapDataPoints)
        End If

        If datasetExtents(0, 0) = -100000000 And datasetExtents(0, 1) = 100000000 And datasetExtents(1, 0) = -100000000 And datasetExtents(1, 1) = 100000000 Then
            MessageBox.Show("Unable to show selected dataset at Map Extents." & vbNewLine & "Map Limits might not contain this dataset!", "DATASET PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else

            target.data(1, 1) = datasetExtents(0, 0) - (datasetExtents(0, 0) - datasetExtents(0, 1)) / 2
            target.data(2, 1) = datasetExtents(1, 0) - (datasetExtents(1, 0) - datasetExtents(1, 1)) / 2
            target.data(3, 1) = datasetExtents(2, 0) - (datasetExtents(2, 0) - datasetExtents(2, 1)) / 2

            'determine position and standoff distance of the camera
            camera.data(1, 1) = target.data(1, 1)
            camera.data(2, 1) = target.data(2, 1) - 0.1
            camera.data(3, 1) = ((datasetExtents(0, 0) - datasetExtents(0, 1)) / 2) / Math.Tan((FOV * Math.PI / 180) / 2)

            Dim yTest As Double = ((datasetExtents(1, 0) - datasetExtents(1, 1)) / 2) / Math.Tan((FOV * Math.PI / 180) / 2)
            If yTest > camera.data(3, 1) Then
                camera.data(3, 1) = yTest
            End If

            currentScalefactor = calcScaleFactor()

            clearMapButton_Click(sender, e)
            If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                renderView2(dataset1MapDataPoints)
                mapDisplayed = True
            End If
            If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                renderView2(dataset2MapDataPoints)
                mapDisplayed = True
            End If
            If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                renderView2(dataset3MapDataPoints)
                mapDisplayed = True
            End If
            If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                renderView2(dataset4MapDataPoints)
                mapDisplayed = True
            End If
            If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                renderView2(dataset5MapDataPoints)
                mapDisplayed = True
            End If
            If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                renderView2(dataset6MapDataPoints)
                mapDisplayed = True
            End If
            If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                renderView2(dataset7MapDataPoints)
                mapDisplayed = True
            End If
            If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                renderView2(dataset8MapDataPoints)
                mapDisplayed = True
            End If
            If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                renderView2(dataset9MapDataPoints)
                mapDisplayed = True
            End If
            If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                renderView2(dataset10MapDataPoints)
                mapDisplayed = True
            End If

            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
            mapPanel.BackgroundImage = mapBitmap
            mapPanel.Refresh()
        End If
    End Sub

    'procedures to determine which colour label lineweight is to be changed and refresh the Map 
    Private Sub colourContextMenuStrip_Opening(ByVal sender As Object, ByVal e As System.EventArgs) Handles colourContextMenuStrip.Opening
        Dim senderControl As ContextMenuStrip = CType(sender, ContextMenuStrip)
        If senderControl.SourceControl.Name = "dataset1ColorLabel" Then
            currentRightClickDataset = 1
            If dataset1LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset1LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset1LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset1LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset1LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset2ColorLabel" Then
            currentRightClickDataset = 2
            If dataset2LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset2LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset2LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset2LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset2LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset3ColorLabel" Then
            currentRightClickDataset = 3
            If dataset3LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset3LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset3LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset3LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset3LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset4ColorLabel" Then
            currentRightClickDataset = 4
            If dataset4LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset4LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset4LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset4LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset4LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset5ColorLabel" Then
            currentRightClickDataset = 5
            If dataset5LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset5LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset5LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset5LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset5LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset6ColorLabel" Then
            currentRightClickDataset = 6
            If dataset6LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset6LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset6LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset6LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset6LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset7ColorLabel" Then
            currentRightClickDataset = 7
            If dataset7LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset7LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset7LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset7LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset7LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset8ColorLabel" Then
            currentRightClickDataset = 8
            If dataset8LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset8LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset8LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset8LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset8LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset9ColorLabel" Then
            currentRightClickDataset = 9
            If dataset9LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset9LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset9LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset9LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset9LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        ElseIf senderControl.SourceControl.Name = "dataset10ColorLabel" Then
            currentRightClickDataset = 10
            If dataset10LineWeight = 1 Then
                ThinToolStripMenuItem.Checked = True
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset10LineWeight = 2 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = True
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset10LineWeight = 4 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = True
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset10LineWeight = 8 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = True
                ObeseToolStripMenuItem.Checked = False
            ElseIf dataset10LineWeight = 16 Then
                ThinToolStripMenuItem.Checked = False
                MediumToolStripMenuItem.Checked = False
                ThickToolStripMenuItem.Checked = False
                FatToolStripMenuItem.Checked = False
                ObeseToolStripMenuItem.Checked = True
            End If
        End If
    End Sub
    Private Sub ThinToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ThinToolStripMenuItem.Click
        If currentRightClickDataset = 1 Then
            dataset1LineWeight = 1
        ElseIf currentRightClickDataset = 2 Then
            dataset2LineWeight = 1
        ElseIf currentRightClickDataset = 3 Then
            dataset3LineWeight = 1
        ElseIf currentRightClickDataset = 4 Then
            dataset4LineWeight = 1
        ElseIf currentRightClickDataset = 5 Then
            dataset5LineWeight = 1
        ElseIf currentRightClickDataset = 6 Then
            dataset6LineWeight = 1
        ElseIf currentRightClickDataset = 7 Then
            dataset7LineWeight = 1
        ElseIf currentRightClickDataset = 8 Then
            dataset8LineWeight = 1
        ElseIf currentRightClickDataset = 9 Then
            dataset9LineWeight = 1
        ElseIf currentRightClickDataset = 10 Then
            dataset10LineWeight = 1
        End If
        drawMapButton_Click(refreshMapButton, e)
    End Sub
    Private Sub MediumToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MediumToolStripMenuItem.Click
        If currentRightClickDataset = 1 Then
            dataset1LineWeight = 2
        ElseIf currentRightClickDataset = 2 Then
            dataset2LineWeight = 2
        ElseIf currentRightClickDataset = 3 Then
            dataset3LineWeight = 2
        ElseIf currentRightClickDataset = 4 Then
            dataset4LineWeight = 2
        ElseIf currentRightClickDataset = 5 Then
            dataset5LineWeight = 2
        ElseIf currentRightClickDataset = 6 Then
            dataset6LineWeight = 2
        ElseIf currentRightClickDataset = 7 Then
            dataset7LineWeight = 2
        ElseIf currentRightClickDataset = 8 Then
            dataset8LineWeight = 2
        ElseIf currentRightClickDataset = 9 Then
            dataset9LineWeight = 2
        ElseIf currentRightClickDataset = 10 Then
            dataset10LineWeight = 2
        End If
        drawMapButton_Click(refreshMapButton, e)
    End Sub
    Private Sub ThickToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ThickToolStripMenuItem.Click
        If currentRightClickDataset = 1 Then
            dataset1LineWeight = 4
        ElseIf currentRightClickDataset = 2 Then
            dataset2LineWeight = 4
        ElseIf currentRightClickDataset = 3 Then
            dataset3LineWeight = 4
        ElseIf currentRightClickDataset = 4 Then
            dataset4LineWeight = 4
        ElseIf currentRightClickDataset = 5 Then
            dataset5LineWeight = 4
        ElseIf currentRightClickDataset = 6 Then
            dataset6LineWeight = 4
        ElseIf currentRightClickDataset = 7 Then
            dataset7LineWeight = 4
        ElseIf currentRightClickDataset = 8 Then
            dataset8LineWeight = 4
        ElseIf currentRightClickDataset = 9 Then
            dataset9LineWeight = 4
        ElseIf currentRightClickDataset = 10 Then
            dataset10LineWeight = 4
        End If
        drawMapButton_Click(refreshMapButton, e)
    End Sub
    Private Sub FatToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FatToolStripMenuItem.Click
        If currentRightClickDataset = 1 Then
            dataset1LineWeight = 8
        ElseIf currentRightClickDataset = 2 Then
            dataset2LineWeight = 8
        ElseIf currentRightClickDataset = 3 Then
            dataset3LineWeight = 8
        ElseIf currentRightClickDataset = 4 Then
            dataset4LineWeight = 8
        ElseIf currentRightClickDataset = 5 Then
            dataset5LineWeight = 8
        ElseIf currentRightClickDataset = 6 Then
            dataset6LineWeight = 8
        ElseIf currentRightClickDataset = 7 Then
            dataset7LineWeight = 8
        ElseIf currentRightClickDataset = 8 Then
            dataset8LineWeight = 8
        ElseIf currentRightClickDataset = 9 Then
            dataset9LineWeight = 8
        ElseIf currentRightClickDataset = 10 Then
            dataset10LineWeight = 8
        End If
        drawMapButton_Click(refreshMapButton, e)
    End Sub
    Private Sub ObeseToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ObeseToolStripMenuItem.Click
        If currentRightClickDataset = 1 Then
            dataset1LineWeight = 16
        ElseIf currentRightClickDataset = 2 Then
            dataset2LineWeight = 16
        ElseIf currentRightClickDataset = 3 Then
            dataset3LineWeight = 16
        ElseIf currentRightClickDataset = 4 Then
            dataset4LineWeight = 16
        ElseIf currentRightClickDataset = 5 Then
            dataset5LineWeight = 16
        ElseIf currentRightClickDataset = 6 Then
            dataset6LineWeight = 16
        ElseIf currentRightClickDataset = 7 Then
            dataset7LineWeight = 16
        ElseIf currentRightClickDataset = 8 Then
            dataset8LineWeight = 16
        ElseIf currentRightClickDataset = 9 Then
            dataset9LineWeight = 16
        ElseIf currentRightClickDataset = 10 Then
            dataset10LineWeight = 16
        End If
        drawMapButton_Click(refreshMapButton, e)
    End Sub

    Private Sub targetsContextMenuStrip_Opening(ByVal sender As Object, ByVal e As System.EventArgs) Handles targetsContextMenuStrip.Opening
        If targetsLineWeight = 1 Then
            tSmallToolStripMenuItem.Checked = True
            tMediumToolStripMenuItem.Checked = False
            tLargeToolStripMenuItem.Checked = False
            tFatToolStripMenuItem.Checked = False
            tObeseToolStripMenuItem.Checked = False
        ElseIf targetsLineWeight = 2 Then
            tSmallToolStripMenuItem.Checked = False
            tMediumToolStripMenuItem.Checked = True
            tLargeToolStripMenuItem.Checked = False
            tFatToolStripMenuItem.Checked = False
            tObeseToolStripMenuItem.Checked = False
        ElseIf targetsLineWeight = 4 Then
            tSmallToolStripMenuItem.Checked = False
            tMediumToolStripMenuItem.Checked = False
            tLargeToolStripMenuItem.Checked = True
            tFatToolStripMenuItem.Checked = False
            tObeseToolStripMenuItem.Checked = False
        ElseIf targetsLineWeight = 8 Then
            tSmallToolStripMenuItem.Checked = False
            tMediumToolStripMenuItem.Checked = False
            tLargeToolStripMenuItem.Checked = False
            tFatToolStripMenuItem.Checked = True
            tObeseToolStripMenuItem.Checked = False
        ElseIf targetsLineWeight = 16 Then
            tSmallToolStripMenuItem.Checked = False
            tMediumToolStripMenuItem.Checked = False
            tLargeToolStripMenuItem.Checked = False
            tFatToolStripMenuItem.Checked = False
            tObeseToolStripMenuItem.Checked = True
        End If
    End Sub

    'procedure to have the user save the current Map (at the current view scale) to a JPEG image on their computer
    Private Sub saveMapButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveMapButton.Click, SaveDisplayImageToolStripMenuItem.Click
        If mapDisplayed = True Then
            Dim saveDialogResult As DialogResult = SaveMapToFileDialog.ShowDialog
            If saveDialogResult <> Windows.Forms.DialogResult.Cancel Then
                mapBitmap.Save(SaveMapToFileDialog.FileName, Imaging.ImageFormat.Jpeg)
                'fileBitmap.Save(SaveMapToFileDialog.FileName, Imaging.ImageFormat.Jpeg)
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub colourComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles colourComboBox.SelectedIndexChanged
        If redrawTest Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub reducePtsComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reducePtsComboBox.SelectedIndexChanged
        If Not inFormLoad Then
            If redrawTest Then
                detailLevel = reducePtsComboBox.SelectedIndex
                updateCurrentDataset(e)
                drawMapButton_Click(refreshMapButton, e)
            End If
        End If
    End Sub

    Friend Sub updateCurrentDataset(ByVal e As System.EventArgs)
        currentMapDataPoints.Clear()
        Dim countedpoints As Integer = 0
        If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
            reducePoints(dataset1MapDataPoints, countedpoints)
        End If
        If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
            reducePoints(dataset2MapDataPoints, countedpoints)
        End If
        If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
            reducePoints(dataset3MapDataPoints, countedpoints)
        End If
        If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
            reducePoints(dataset4MapDataPoints, countedpoints)
        End If
        If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
            reducePoints(dataset5MapDataPoints, countedpoints)
        End If
        If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
            reducePoints(dataset6MapDataPoints, countedpoints)
        End If
        If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
            reducePoints(dataset7MapDataPoints, countedpoints)
        End If
        If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
            reducePoints(dataset8MapDataPoints, countedpoints)
        End If
        If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
            reducePoints(dataset9MapDataPoints, countedpoints)
        End If
        If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
            reducePoints(dataset10MapDataPoints, countedpoints)
        End If
    End Sub


    'run through a single dataset and counts the number of highlighted points
    Friend Sub reducePoints(ByVal mapDataPoints As Generic.List(Of cloudPoint), ByRef counter As Integer)
        Dim modTest As Integer = 1

        If detailLevel = 1 Or detailLevel = 9 Then
            modTest = 10
        ElseIf detailLevel = 2 Or detailLevel = 8 Then
            modTest = 5
        ElseIf detailLevel = 3 Or detailLevel = 7 Then
            modTest = 4
        ElseIf detailLevel = 4 Or detailLevel = 6 Then
            modTest = 3
        ElseIf detailLevel = 5 Then
            modTest = 2
        ElseIf detailLevel = 10 Then    'only used programmatically to greatly reduce points drawn while rotating the view on the screen (to around 2000 points)
            Try
                Dim displayedTotalPoints As Integer
                If reducePtsComboBox.SelectedIndex = 1 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.1D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 2 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.2D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 3 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.25D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 4 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.333D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 5 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.5D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 6 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.667D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 7 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.75D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 8 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.8D * Convert.ToDecimal(totalNumPoints)), 0))
                ElseIf reducePtsComboBox.SelectedIndex = 9 Then
                    displayedTotalPoints = Convert.ToInt32(Decimal.Round((0.9D * Convert.ToDecimal(totalNumPoints)), 0))
                Else
                    displayedTotalPoints = totalNumPoints
                End If

                Dim ratio As Decimal = Convert.ToDecimal(maxPointsToRenderWhenNavigating) / Convert.ToDecimal(displayedTotalPoints)
                If ratio < 1D Then
                    modTest = Convert.ToInt32(Decimal.Round((1D / ratio), 0))
                Else
                    modTest = 1
                End If
            Catch ex As Exception
                modTest = 50
            End Try
        End If

        Dim includePoint As Boolean = True
        Dim stopper As Integer = mapDataPoints.Count
        Dim i As Integer = 0

        Do While i < stopper
            Try
                If Not mapDataPoints(i).deleted Then
                    'reduction percentage option filtering code
                    If modTest <> 1 Then
                        If counter Mod modTest = 0 Then
                            If detailLevel >= 5 Or detailLevel = 0 Then
                                includePoint = True
                            Else
                                includePoint = False
                            End If
                        Else
                            If detailLevel >= 5 Then
                                includePoint = False
                            Else
                                includePoint = True
                            End If
                        End If
                    Else
                        includePoint = True
                    End If

                    'point passes the test so add it to the currently viewed collective dataset
                    If includePoint = True Then
                        currentMapDataPoints.Add(mapDataPoints(i))
                    End If
                End If
                counter += 1
            Catch ex As Exception
                'possible error because of multi-threading and i >= stopper
            End Try
            i += 1
            stopper = mapDataPoints.Count
        Loop
    End Sub

    Private Sub histogramCutOffsComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles histogramCutOffsComboBox.SelectedIndexChanged
        histogramCutoff = histogramCutOffsComboBox.SelectedIndex
        intensityHistMaxMin = calcHistogramBounds(intensityHistogram)
        If histogramDisplayed = True Then
            histogramForm.renderHistogram()
        End If
        If colourComboBox.SelectedIndex = 0 Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub perspectiveRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles perspectiveRadioButton.CheckedChanged
        If perspectiveRadioButton.Checked Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub parallelRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles parallelRadioButton.CheckedChanged
        If parallelRadioButton.Checked Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub zoomExtentsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles zoomExtentsButton.Click
        If mapDisplayed Then
            globalMaxMinValues(0, 0) = -100000000
            globalMaxMinValues(0, 1) = 100000000
            globalMaxMinValues(1, 0) = -100000000
            globalMaxMinValues(1, 1) = 100000000
            globalMaxMinValues(2, 0) = -100000000
            globalMaxMinValues(2, 1) = 100000000

            'If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset1MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset2MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset3MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset4MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset5MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset6MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset7MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset8MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset9MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If
            'If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
            '    Dim maxMin(,) As Double = ComputeMaxMin(dataset10MapDataPoints)
            '    updateMaxMin(maxMin)
            'End If

            Dim maxMin(,) As Double = ComputeMaxMin(currentMapDataPoints)
            updateMaxMin(maxMin)

            target.data(1, 1) = globalMaxMinValues(0, 0) - (globalMaxMinValues(0, 0) - globalMaxMinValues(0, 1)) / 2
            target.data(2, 1) = globalMaxMinValues(1, 0) - (globalMaxMinValues(1, 0) - globalMaxMinValues(1, 1)) / 2
            target.data(3, 1) = globalMaxMinValues(2, 0) - (globalMaxMinValues(2, 0) - globalMaxMinValues(2, 1)) / 2

            camera.data(1, 1) = target.data(1, 1)
            camera.data(2, 1) = target.data(2, 1) - 0.1
            camera.data(3, 1) = ((globalMaxMinValues(0, 0) - globalMaxMinValues(0, 1)) / 2) / Math.Tan((FOV * Math.PI / 180) / 2)

            Dim yTest As Double = ((globalMaxMinValues(1, 0) - globalMaxMinValues(1, 1)) / 2) / Math.Tan((FOV * Math.PI / 180) / 2)
            If yTest > camera.data(3, 1) Then
                camera.data(3, 1) = yTest
            End If

            currentCentreValue(0) = 0
            currentCentreValue(1) = 0
            currentCentreValue(2) = 0

            currentScalefactor = globalScaleFactor
            'currentScalefactor = 1

            drawMapButton_Click(drawMapButton, e)
        End If
    End Sub

    Private Sub resetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles resetButton.Click
        redrawTest = False
        colourComboBox.SelectedIndex = 0
        histogramCutOffsComboBox.SelectedIndex = 0
        reducePtsComboBox.SelectedIndex = 5
        detailLevel = reducePtsComboBox.SelectedIndex
        redrawTest = True
        camera = initialCamera
        target = initialTarget
        currentCentreValue(0) = 0
        currentCentreValue(1) = 0
        currentCentreValue(2) = 0
        imagePlaneXYsolved = False
        currentScalefactor = initialScaleFactor
        imageOffsets(0) = 0
        imageOffsets(1) = 0
        perspectiveRadioButton.Checked = False
        perspectiveRadioButton.Checked = True
    End Sub

    Private Sub drawBoxButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drawBoxButton.Click
        If mapPanel.Cursor = Cursors.Hand Then
            MessageBox.Show("You cannot draw a selection box while in Sliding Mode", "SLIDING MODE PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If mapDisplayed Then
                statusLabel.Text = "Click and hold on the upper-left box corner then drag and release on the lower-right box corner, Press ENTER to accept or ESC to quit."
                insideDrawBox = True
            Else
                MessageBox.Show("You cannot draw a selection box on an empty display", "NO DATA PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub mainForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        'enter button pressed
        If Convert.ToInt32(e.KeyChar) = 13I Then
            If insideDrawBox Then
                If fromHereBox(0) <> toHereBox(0) And fromHereBox(1) <> toHereBox(1) Then
                    Dim oldFormCursor As Cursor = Me.Cursor
                    Dim oldMapCursor As Cursor = mapPanel.Cursor
                    Me.Cursor = Cursors.AppStarting
                    mapPanel.Cursor = Cursors.AppStarting

                    Dim pointsOnScreen As Integer = 0
                    highlightedPoints = 0

                    highlightedPoints = countTotalHighlightedInBox()
                    countHighlightedInBox(currentMapDataPoints, pointsOnScreen, False)

                    If activeRender Then
                        activeRender = False
                        drawMapButton_Click(refreshMapButton, e)
                        activeRender = True
                    Else
                        drawMapButton_Click(refreshMapButton, e)
                    End If
                    pointsSelLabel.Text = "Points Selected: " & highlightedPoints.ToString
                    statusLabel.Text = String.Empty
                    insideDrawBox = False
                    Me.Cursor = oldFormCursor
                    mapPanel.Cursor = oldMapCursor
                End If
            End If
            'esc button pressed
        ElseIf Convert.ToInt32(e.KeyChar) = 27I Then
            statusLabel.Text = String.Empty
            insideDrawBox = False
            insidePickCamera = False
            insidePickTarget = False
            insidePickRegTarget = False

            If BackgroundWorker1.IsBusy = True Then
                BackgroundWorker1.CancelAsync()
                statusLabel.Text = "CANCELLED"
                System.Threading.Thread.Sleep(2000)
                statusLabel.Text = String.Empty
                Me.Cursor = Cursors.Default
                mapPanel.Cursor = currentMapCursor
            End If

            If backgroundWorker2.IsBusy = True Then
                backgroundWorker2.CancelAsync()
                statusLabel.Text = "CANCELLED"
                System.Threading.Thread.Sleep(2000)
                statusLabel.Text = String.Empty
                Me.Cursor = Cursors.Default
                mapPanel.Cursor = currentMapCursor
            End If

            If BackgroundWorker3.IsBusy = True Then
                BackgroundWorker3.CancelAsync()
                statusLabel.Text = "CANCELLED"
                System.Threading.Thread.Sleep(2000)
                statusLabel.Text = String.Empty
                Me.Cursor = Cursors.Default
                mapPanel.Cursor = currentMapCursor
            End If

            If BackgroundWorker4.IsBusy = True Then
                BackgroundWorker4.CancelAsync()
            End If

            mapPanel.BackgroundImage = mapBitmap
            mapPanel.Refresh()
        End If
    End Sub

    'looks through all loaded and displayed datasets and counts the number of highlighted points
    Private Function countTotalHighlightedInBox() As Integer
        'reset counter and histogram data
        Dim counter As Integer = 0
        ReDim selectedHistogram(0)

        If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
            countHighlightedInBox(dataset1MapDataPoints, counter, True)
        End If
        If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
            countHighlightedInBox(dataset2MapDataPoints, counter, True)
        End If
        If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
            countHighlightedInBox(dataset3MapDataPoints, counter, True)
        End If
        If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
            countHighlightedInBox(dataset4MapDataPoints, counter, True)
        End If
        If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
            countHighlightedInBox(dataset5MapDataPoints, counter, True)
        End If
        If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
            countHighlightedInBox(dataset6MapDataPoints, counter, True)
        End If
        If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
            countHighlightedInBox(dataset7MapDataPoints, counter, True)
        End If
        If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
            countHighlightedInBox(dataset8MapDataPoints, counter, True)
        End If
        If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
            countHighlightedInBox(dataset9MapDataPoints, counter, True)
        End If
        If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
            countHighlightedInBox(dataset10MapDataPoints, counter, True)
        End If
        selectedHistMaxMin = calcHistogramBounds(selectedHistogram) 'update histogram stats
        Return counter
    End Function

    'run through a single dataset and counts the number of highlighted points
    Private Sub countHighlightedInBox(ByRef mapDataPoints As Generic.List(Of cloudPoint), ByRef counter As Integer, ByVal updateHistogram As Boolean)
        calcDisplayXYZ(mapDataPoints)
        For i As Integer = 0 To mapDataPoints.Count - 1
            Dim newMapPoint As New cloudPoint
            newMapPoint.X = mapDataPoints(i).X
            newMapPoint.Y = mapDataPoints(i).Y
            newMapPoint.Z = mapDataPoints(i).Z
            newMapPoint.R = mapDataPoints(i).R
            newMapPoint.G = mapDataPoints(i).G
            newMapPoint.B = mapDataPoints(i).B
            newMapPoint.I = mapDataPoints(i).I
            newMapPoint.Xm = mapDataPoints(i).Xm
            newMapPoint.Ym = mapDataPoints(i).Ym
            'newMapPoint.Zm = mapDataPoints(i).Zm
            newMapPoint.displayX = mapDataPoints(i).displayX
            newMapPoint.displayY = mapDataPoints(i).displayY
            newMapPoint.deleted = mapDataPoints(i).deleted
            newMapPoint.datasetNum = mapDataPoints(i).datasetNum
            If mapDataPoints(i).displayX >= fromHereBox(1) And mapDataPoints(i).displayX <= toHereBox(1) And mapDataPoints(i).displayY >= fromHereBox(0) And mapDataPoints(i).displayY <= toHereBox(0) Then
                newMapPoint.highlighted = True
                counter += 1
            Else
                If multiSelectCheckBox.Checked Then
                    newMapPoint.highlighted = mapDataPoints(i).highlighted
                Else
                    newMapPoint.highlighted = False
                End If
            End If
            mapDataPoints(i) = newMapPoint
        Next
        updateSubHistogram(mapDataPoints, updateHistogram)
    End Sub

    Private Sub updateSubHistogram(ByRef mapDataPoints As Generic.List(Of cloudPoint), ByVal updateSelectedHistogram As Boolean)
        If updateSelectedHistogram = False Then
            'reset current view's histogram data
            ReDim currentViewHistogram(0)
        End If
        For i As Integer = 0 To mapDataPoints.Count - 1
            If mapDataPoints(i).highlighted Then
                If updateSelectedHistogram Then 'update the histogram for ALL the points available within the current selection
                    If mapDataPoints(i).I > selectedHistogram.Length - 1 Then
                        ReDim Preserve selectedHistogram(mapDataPoints(i).I)
                    End If
                    selectedHistogram(mapDataPoints(i).I) += 1
                Else    'update the current view's histogram data (the histogram of ONLY the points rendered on screen)
                    If mapDataPoints(i).I > currentViewHistogram.Length - 1 Then
                        ReDim Preserve currentViewHistogram(mapDataPoints(i).I)
                    End If
                    currentViewHistogram(mapDataPoints(i).I) += 1
                End If
            End If
        Next
        If updateSelectedHistogram = False Then
            currentViewHistMaxMin = calcHistogramBounds(currentViewHistogram) 'update histogram stats
        End If
    End Sub

    Private Sub clearSelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearSelButton.Click
        If mapDisplayed = True Then
            clearHighlighted(currentMapDataPoints)
            If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                clearHighlighted(dataset1MapDataPoints)
            End If
            If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                clearHighlighted(dataset2MapDataPoints)
            End If
            If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                clearHighlighted(dataset3MapDataPoints)
            End If
            If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                clearHighlighted(dataset4MapDataPoints)
            End If
            If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                clearHighlighted(dataset5MapDataPoints)
            End If
            If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                clearHighlighted(dataset6MapDataPoints)
            End If
            If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                clearHighlighted(dataset7MapDataPoints)
            End If
            If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                clearHighlighted(dataset8MapDataPoints)
            End If
            If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                clearHighlighted(dataset9MapDataPoints)
            End If
            If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                clearHighlighted(dataset10MapDataPoints)
            End If
            highlightedPoints = 0
            If activeRender Then
                activeRender = False
                drawMapButton_Click(refreshMapButton, e)
                activeRender = True
            Else
                drawMapButton_Click(refreshMapButton, e)
            End If
            pointsSelLabel.Text = "Points Selected: 0"
            ReDim currentViewHistogram(0)
            ReDim selectedHistogram(0)
        End If
    End Sub

    Private Sub clearHighlighted(ByRef mapDataPoints As Generic.List(Of cloudPoint))
        For i As Integer = 0 To mapDataPoints.Count - 1
            Dim newMapDataPoint As cloudPoint
            newMapDataPoint.X = mapDataPoints(i).X
            newMapDataPoint.Y = mapDataPoints(i).Y
            newMapDataPoint.Z = mapDataPoints(i).Z
            newMapDataPoint.I = mapDataPoints(i).I
            newMapDataPoint.R = mapDataPoints(i).R
            newMapDataPoint.G = mapDataPoints(i).G
            newMapDataPoint.B = mapDataPoints(i).B
            newMapDataPoint.displayX = mapDataPoints(i).displayX
            newMapDataPoint.displayY = mapDataPoints(i).displayY
            newMapDataPoint.highlighted = False
            newMapDataPoint.deleted = mapDataPoints(i).deleted
            newMapDataPoint.Xm = mapDataPoints(i).Xm
            newMapDataPoint.Ym = mapDataPoints(i).Ym
            'newMapDataPoint.Zm = mapDataPoints(i).Zm
            newMapDataPoint.datasetNum = mapDataPoints(i).datasetNum
            mapDataPoints(i) = newMapDataPoint
        Next
    End Sub

    Private Sub invertButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles invertButton.Click
        If mapDisplayed = True Then
            ReDim selectedHistogram(0)
            Dim counter As Integer = 0
            Dim pointsOnScreen As Integer = 0
            invertHighlighted(currentMapDataPoints, pointsOnScreen)
            If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                invertHighlighted(dataset1MapDataPoints, counter)
                updateSubHistogram(dataset1MapDataPoints, True)
            End If
            If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                invertHighlighted(dataset2MapDataPoints, counter)
                updateSubHistogram(dataset2MapDataPoints, True)
            End If
            If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                invertHighlighted(dataset3MapDataPoints, counter)
                updateSubHistogram(dataset3MapDataPoints, True)
            End If
            If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                invertHighlighted(dataset4MapDataPoints, counter)
                updateSubHistogram(dataset4MapDataPoints, True)
            End If
            If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                invertHighlighted(dataset5MapDataPoints, counter)
                updateSubHistogram(dataset5MapDataPoints, True)
            End If
            If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                invertHighlighted(dataset6MapDataPoints, counter)
                updateSubHistogram(dataset6MapDataPoints, True)
            End If
            If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                invertHighlighted(dataset7MapDataPoints, counter)
                updateSubHistogram(dataset7MapDataPoints, True)
            End If
            If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                invertHighlighted(dataset8MapDataPoints, counter)
                updateSubHistogram(dataset8MapDataPoints, True)
            End If
            If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                invertHighlighted(dataset9MapDataPoints, counter)
                updateSubHistogram(dataset9MapDataPoints, True)
            End If
            If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                invertHighlighted(dataset10MapDataPoints, counter)
                updateSubHistogram(dataset10MapDataPoints, True)
            End If
            highlightedPoints = counter
            'highlightedPoints = countTotalHighlightedInBox()
            'countHighlightedInBox(currentMapDataPoints, pointsOnScreen, False)
            updateSubHistogram(currentMapDataPoints, False)

            pointsSelLabel.Text = "Points Selected: " & highlightedPoints.ToString
            If activeRender Then
                activeRender = False
                drawMapButton_Click(refreshMapButton, e)
                activeRender = True
            Else
                drawMapButton_Click(refreshMapButton, e)
            End If
        End If
    End Sub

    Private Sub invertHighlighted(ByRef mapDataPoints As Generic.List(Of cloudPoint), ByRef counter As Integer)
        For i As Integer = 0 To mapDataPoints.Count - 1
            Dim newMapDataPoint As cloudPoint
            newMapDataPoint.X = mapDataPoints(i).X
            newMapDataPoint.Y = mapDataPoints(i).Y
            newMapDataPoint.Z = mapDataPoints(i).Z
            newMapDataPoint.I = mapDataPoints(i).I
            newMapDataPoint.R = mapDataPoints(i).R
            newMapDataPoint.G = mapDataPoints(i).G
            newMapDataPoint.B = mapDataPoints(i).B
            newMapDataPoint.displayX = mapDataPoints(i).displayX
            newMapDataPoint.displayY = mapDataPoints(i).displayY
            newMapDataPoint.highlighted = Not mapDataPoints(i).highlighted
            If newMapDataPoint.highlighted = True Then
                counter += 1
            End If
            newMapDataPoint.deleted = mapDataPoints(i).deleted
            newMapDataPoint.Xm = mapDataPoints(i).Xm
            newMapDataPoint.Ym = mapDataPoints(i).Ym
            'newMapDataPoint.Zm = mapDataPoints(i).Zm
            newMapDataPoint.datasetNum = mapDataPoints(i).datasetNum
            mapDataPoints(i) = newMapDataPoint
        Next
    End Sub

    Private Sub deleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles deleteButton.Click
        If mapDisplayed = True Then
            deleteHighlighted(currentMapDataPoints)
            If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                deleteHighlighted(dataset1MapDataPoints)
            End If
            If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                deleteHighlighted(dataset2MapDataPoints)
            End If
            If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                deleteHighlighted(dataset3MapDataPoints)
            End If
            If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                deleteHighlighted(dataset4MapDataPoints)
            End If
            If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                deleteHighlighted(dataset5MapDataPoints)
            End If
            If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                deleteHighlighted(dataset6MapDataPoints)
            End If
            If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                deleteHighlighted(dataset7MapDataPoints)
            End If
            If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                deleteHighlighted(dataset8MapDataPoints)
            End If
            If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                deleteHighlighted(dataset9MapDataPoints)
            End If
            If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                deleteHighlighted(dataset10MapDataPoints)
            End If
            'highlightedPoints = 0
            'drawMapButton_Click(refreshMapButton, e)
            'pointsSelLabel.Text = "Points Selected: 0"
            clearSelButton_Click(sender, e)
        End If
    End Sub

    Private Sub deleteHighlighted(ByRef mapDataPoints As Generic.List(Of cloudPoint))
        For i As Integer = 0 To mapDataPoints.Count - 1
            Dim newMapDataPoint As cloudPoint
            newMapDataPoint.X = mapDataPoints(i).X
            newMapDataPoint.Y = mapDataPoints(i).Y
            newMapDataPoint.Z = mapDataPoints(i).Z
            newMapDataPoint.I = mapDataPoints(i).I
            newMapDataPoint.R = mapDataPoints(i).R
            newMapDataPoint.G = mapDataPoints(i).G
            newMapDataPoint.B = mapDataPoints(i).B
            newMapDataPoint.displayX = mapDataPoints(i).displayX
            newMapDataPoint.displayY = mapDataPoints(i).displayY
            If mapDataPoints(i).highlighted Then
                newMapDataPoint.deleted = True
            Else
                newMapDataPoint.deleted = mapDataPoints(i).deleted
            End If
            newMapDataPoint.highlighted = False
            newMapDataPoint.Xm = mapDataPoints(i).Xm
            newMapDataPoint.Ym = mapDataPoints(i).Ym
            'newMapDataPoint.Zm = mapDataPoints(i).Zm
            newMapDataPoint.datasetNum = mapDataPoints(i).datasetNum
            mapDataPoints(i) = newMapDataPoint
        Next
    End Sub

    Private Sub recoverButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles recoverButton.Click
        If mapDisplayed = True Then
            recoverDeleted(currentMapDataPoints)
            If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                recoverDeleted(dataset1MapDataPoints)
            End If
            If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                recoverDeleted(dataset2MapDataPoints)
            End If
            If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                recoverDeleted(dataset3MapDataPoints)
            End If
            If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                recoverDeleted(dataset4MapDataPoints)
            End If
            If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                recoverDeleted(dataset5MapDataPoints)
            End If
            If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                recoverDeleted(dataset6MapDataPoints)
            End If
            If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                recoverDeleted(dataset7MapDataPoints)
            End If
            If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                recoverDeleted(dataset8MapDataPoints)
            End If
            If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                recoverDeleted(dataset9MapDataPoints)
            End If
            If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                recoverDeleted(dataset10MapDataPoints)
            End If
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub recoverDeleted(ByRef mapDataPoints As Generic.List(Of cloudPoint))
        For i As Integer = 0 To mapDataPoints.Count - 1
            Dim newMapDataPoint As cloudPoint
            newMapDataPoint.X = mapDataPoints(i).X
            newMapDataPoint.Y = mapDataPoints(i).Y
            newMapDataPoint.Z = mapDataPoints(i).Z
            newMapDataPoint.I = mapDataPoints(i).I
            newMapDataPoint.R = mapDataPoints(i).R
            newMapDataPoint.G = mapDataPoints(i).G
            newMapDataPoint.B = mapDataPoints(i).B
            newMapDataPoint.displayX = mapDataPoints(i).displayX
            newMapDataPoint.displayY = mapDataPoints(i).displayY
            newMapDataPoint.highlighted = False
            newMapDataPoint.deleted = False
            newMapDataPoint.Xm = mapDataPoints(i).Xm
            newMapDataPoint.Ym = mapDataPoints(i).Ym
            'newMapDataPoint.Zm = mapDataPoints(i).Zm
            newMapDataPoint.datasetNum = mapDataPoints(i).datasetNum
            mapDataPoints(i) = newMapDataPoint
        Next
    End Sub

    Private Sub orbitCameraButtons(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles camLeftButton.Click, camRightButton.Click, camDownButton.Click, camUpButton.Click
        Dim clickedButton As Button = CType(sender, Button)
        Dim cam2TarDist As Decimal = Matrix.abs(camera - target)
        Dim H As Decimal = cam2TarDist / Math.Cos(15 * Math.PI / 180)
        Dim O As Decimal = Math.Sqrt(H ^ 2 - cam2TarDist ^ 2)
        If clickedButton.Name = "camLeftButton" Then
            camera = camera + O * (-1 * imagePlaneX)
        ElseIf clickedButton.Name = "camRightButton" Then
            camera = camera + O * imagePlaneX
        ElseIf clickedButton.Name = "camDownButton" Then
            camera = camera + O * (-1 * imagePlaneY)
        ElseIf clickedButton.Name = "camUpButton" Then
            camera = camera + O * imagePlaneY
        End If
        Dim dir2Centre As New Matrix(3, 1)
        dir2Centre = (target - camera) / Matrix.abs(target - camera)
        camera = camera + (H - cam2TarDist) * dir2Centre
        drawMapButton_Click(refreshMapButton, e)
    End Sub

    Private Sub orbitTargetButtons(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tarLeftButton.Click, tarRightButton.Click, tarDownButton.Click, tarUpButton.Click
        Dim clickedButton As Button = CType(sender, Button)
        Dim cam2TarDist As Decimal = Matrix.abs(camera - target)
        Dim H As Decimal = cam2TarDist / Math.Cos(15 * Math.PI / 180)
        Dim O As Decimal = Math.Sqrt(H ^ 2 - cam2TarDist ^ 2)
        If clickedButton.Name = "tarLeftButton" Then
            target = target + O * (-1 * imagePlaneX)
        ElseIf clickedButton.Name = "tarRightButton" Then
            target = target + O * (1 * imagePlaneX)
        ElseIf clickedButton.Name = "tarDownButton" Then
            target = target + O * (-1 * imagePlaneY)
        ElseIf clickedButton.Name = "tarUpButton" Then
            target = target + O * imagePlaneY
        End If
        Dim dir2Centre As New Matrix(3, 1)
        dir2Centre = (camera - target) / Matrix.abs(target - camera)
        target = target + (H - cam2TarDist) * dir2Centre
        drawMapButton_Click(refreshMapButton, e)
    End Sub

    Private Sub pickCameraButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pickCameraButton.Click
        If mapDisplayed Then
            statusLabel.Text = "Click a dataset point on the screen to have the camera view established at this point, press ESC to quit"
            insidePickCamera = True
        Else
            MessageBox.Show("You cannot pick a dataset point on an empty display", "NO DATA PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub pickTargetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pickTargetButton.Click
        If mapDisplayed Then
            statusLabel.Text = "Click a dataset point on the screen to have the aiming target anchored at this point, press ESC to quit"
            insidePickTarget = True
        Else
            MessageBox.Show("You cannot pick a dataset point on an empty display", "NO DATA PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub ExportDatasetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportDatasetToolStripMenuItem.Click
        If mapDisplayed = True Then
            exportDialog.FileName = ""
            Dim exportDialogResult As DialogResult = exportDialog.ShowDialog
            If exportDialogResult <> Windows.Forms.DialogResult.Cancel Then
                Dim exportSuccess As Boolean = False
                If exportDialog.FilterIndex = 1 Then    'TXT file export
                    textFormatForm.columnsGroupBox.Enabled = False
                    textFormatForm.ShowDialog()
                    If tFFContinue Then
                        If textFormatForm.XYZiSRadioButton.Checked Then
                            exportSuccess = exportTextFile(1, exportDialog.FileName)
                        ElseIf textFormatForm.XYZiCRadioButton.Checked Then
                            exportSuccess = exportTextFile(2, exportDialog.FileName)
                        ElseIf textFormatForm.XYZiRGBSRadioButton.Checked Then
                            exportSuccess = exportTextFile(3, exportDialog.FileName)
                        ElseIf textFormatForm.XYZiRGBCRadioButton.Checked Then
                            exportSuccess = exportTextFile(4, exportDialog.FileName)
                        ElseIf textFormatForm.XYZRGBiSRadioButton.Checked Then
                            exportSuccess = exportTextFile(5, exportDialog.FileName)
                        ElseIf textFormatForm.XYZRGBiCRadioButton.Checked Then
                            exportSuccess = exportTextFile(6, exportDialog.FileName)
                        End If

                        If exportSuccess Then
                            MessageBox.Show("Export complete", "RGBi", MessageBoxButtons.OK)
                            percentageLabel.Text = ""
                            statusLabel.Text = ""
                        Else
                            MessageBox.Show("An error occurred while exporting the point cloud", "RGBi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If

                    End If
                ElseIf exportDialog.FilterIndex = 2 Then    'LAS file export
                    exportSuccess = exportTextFile(5, Application.StartupPath & "\pointcloud.txt")
                    If exportSuccess Then
                        System.Threading.Thread.Sleep(2000)
                        'setting up the options for encoding a TXT file to a LAS file
                        lasShell.InputFile = ControlChars.Quote & Application.StartupPath & "\pointcloud.txt" & ControlChars.Quote
                        lasShell.OutputFile = ControlChars.Quote & exportDialog.FileName & ControlChars.Quote
                        lasShell.Options = "--parse xyzRGBi -scale 0.0001"
                        lasShell.TimerInterval = 100
                        'start the encoding process
                        If lasShell.StartEncoding() Then
                            updateStatus("Encoding LAS file")
                        Else
                            updateStatus("LAS encoding failed to start")
                        End If
                    Else
                        MessageBox.Show("An error occurred while exporting the point cloud", "RGBi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                ElseIf exportDialog.FilterIndex = 3 Then    'e57 file export
                    exportSuccess = exportTextFile(5, Application.StartupPath & "\pointcloud.txt")
                    If exportSuccess Then
                        System.Threading.Thread.Sleep(2000)
                        'setting up the options for encoding a TXT file to a LAS file
                        lasShell2.InputFile = ControlChars.Quote & Application.StartupPath & "\pointcloud.txt" & ControlChars.Quote
                        lasShell2.OutputFile = ControlChars.Quote & Application.StartupPath & "\pointcloud.las" & ControlChars.Quote
                        lasShell2.Options = "--parse xyzRGBi -scale 0.0001"
                        lasShell2.TimerInterval = 100
                        'start the encoding process
                        If lasShell2.StartEncoding() Then
                            updateStatus("Encoding e57 file")
                        Else
                            updateStatus("e57 encoding failed to start")
                        End If
                    Else
                        MessageBox.Show("An error occurred while exporting the point cloud", "RGBi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End If
            End If
        End If
    End Sub

    Private Function exportTextFile(ByVal exportOption As Integer, ByVal exportFileName As String) As Boolean
        statusLabel.Text = "Writing out point cloud dataset"
        statusLabel.Refresh()
        Me.Cursor = Cursors.AppStarting
        If mapPanel.Cursor = Cursors.Cross Then
            currentMapCursor = Cursors.Cross
        Else
            currentMapCursor = Cursors.Hand
        End If
        mapPanel.Cursor = Cursors.AppStarting

        Dim exportStream As New StreamWriter(exportFileName)
        Try

       
        'XYZI Files
        If exportOption = 1 Or exportOption = 2 Then
            If headerRow Then
                exportStream.WriteLine("X" & delimiter & "Y" & delimiter & "Z" & delimiter & "I")
            End If
                If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                    For i As Integer = 0 To dataset1MapDataPoints.Count - 1
                        If dataset1MapDataPoints(i).deleted = False Then
                            exportStream.WriteLine(dataset1MapDataPoints(i).X.ToString & delimiter & dataset1MapDataPoints(i).Y.ToString & delimiter & dataset1MapDataPoints(i).Z.ToString & delimiter & dataset1MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                    For i As Integer = 0 To dataset2MapDataPoints.Count - 1
                        If dataset2MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset2MapDataPoints(i).X.ToString & delimiter & dataset2MapDataPoints(i).Y.ToString & delimiter & dataset2MapDataPoints(i).Z.ToString & delimiter & dataset2MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                    For i As Integer = 0 To dataset3MapDataPoints.Count - 1
                        If dataset3MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset3MapDataPoints(i).X.ToString & delimiter & dataset3MapDataPoints(i).Y.ToString & delimiter & dataset3MapDataPoints(i).Z.ToString & delimiter & dataset3MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                    For i As Integer = 0 To dataset4MapDataPoints.Count - 1
                        If dataset4MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset4MapDataPoints(i).X.ToString & delimiter & dataset4MapDataPoints(i).Y.ToString & delimiter & dataset4MapDataPoints(i).Z.ToString & delimiter & dataset4MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                    For i As Integer = 0 To dataset5MapDataPoints.Count - 1
                        If dataset5MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset5MapDataPoints(i).X.ToString & delimiter & dataset5MapDataPoints(i).Y.ToString & delimiter & dataset5MapDataPoints(i).Z.ToString & delimiter & dataset5MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                    For i As Integer = 0 To dataset6MapDataPoints.Count - 1
                        If dataset6MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset6MapDataPoints(i).X.ToString & delimiter & dataset6MapDataPoints(i).Y.ToString & delimiter & dataset6MapDataPoints(i).Z.ToString & delimiter & dataset6MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                    For i As Integer = 0 To dataset7MapDataPoints.Count - 1
                        If dataset7MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset7MapDataPoints(i).X.ToString & delimiter & dataset7MapDataPoints(i).Y.ToString & delimiter & dataset7MapDataPoints(i).Z.ToString & delimiter & dataset7MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                    For i As Integer = 0 To dataset8MapDataPoints.Count - 1
                        If dataset8MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset8MapDataPoints(i).X.ToString & delimiter & dataset8MapDataPoints(i).Y.ToString & delimiter & dataset8MapDataPoints(i).Z.ToString & delimiter & dataset8MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                    For i As Integer = 0 To dataset9MapDataPoints.Count - 1
                        If dataset9MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset9MapDataPoints(i).X.ToString & delimiter & dataset9MapDataPoints(i).Y.ToString & delimiter & dataset9MapDataPoints(i).Z.ToString & delimiter & dataset9MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
                If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                    For i As Integer = 0 To dataset10MapDataPoints.Count - 1
                        If dataset10MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset10MapDataPoints(i).X.ToString & delimiter & dataset10MapDataPoints(i).Y.ToString & delimiter & dataset10MapDataPoints(i).Z.ToString & delimiter & dataset10MapDataPoints(i).I.ToString)
                        End If
                    Next
                End If
            'XYZIRGB Files
        ElseIf exportOption = 3 Or exportOption = 4 Then
            If headerRow Then
                exportStream.WriteLine("X" & delimiter & "Y" & delimiter & "Z" & delimiter & "I" & delimiter & "R" & delimiter & "G" & delimiter & "B")
            End If
            If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                For i As Integer = 0 To dataset1MapDataPoints.Count - 1
                    If dataset1MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset1MapDataPoints(i).X.ToString & delimiter & dataset1MapDataPoints(i).Y.ToString & delimiter & dataset1MapDataPoints(i).Z.ToString & delimiter & dataset1MapDataPoints(i).I.ToString & delimiter & dataset1MapDataPoints(i).R.ToString & delimiter & dataset1MapDataPoints(i).G.ToString & delimiter & dataset1MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                For i As Integer = 0 To dataset2MapDataPoints.Count - 1
                    If dataset2MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset2MapDataPoints(i).X.ToString & delimiter & dataset2MapDataPoints(i).Y.ToString & delimiter & dataset2MapDataPoints(i).Z.ToString & delimiter & dataset2MapDataPoints(i).I.ToString & delimiter & dataset2MapDataPoints(i).R.ToString & delimiter & dataset2MapDataPoints(i).G.ToString & delimiter & dataset2MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                For i As Integer = 0 To dataset3MapDataPoints.Count - 1
                    If dataset3MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset3MapDataPoints(i).X.ToString & delimiter & dataset3MapDataPoints(i).Y.ToString & delimiter & dataset3MapDataPoints(i).Z.ToString & delimiter & dataset3MapDataPoints(i).I.ToString & delimiter & dataset3MapDataPoints(i).R.ToString & delimiter & dataset3MapDataPoints(i).G.ToString & delimiter & dataset3MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                For i As Integer = 0 To dataset4MapDataPoints.Count - 1
                    If dataset4MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset4MapDataPoints(i).X.ToString & delimiter & dataset4MapDataPoints(i).Y.ToString & delimiter & dataset4MapDataPoints(i).Z.ToString & delimiter & dataset4MapDataPoints(i).I.ToString & delimiter & dataset4MapDataPoints(i).R.ToString & delimiter & dataset4MapDataPoints(i).G.ToString & delimiter & dataset4MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                For i As Integer = 0 To dataset5MapDataPoints.Count - 1
                    If dataset5MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset5MapDataPoints(i).X.ToString & delimiter & dataset5MapDataPoints(i).Y.ToString & delimiter & dataset5MapDataPoints(i).Z.ToString & delimiter & dataset5MapDataPoints(i).I.ToString & delimiter & dataset5MapDataPoints(i).R.ToString & delimiter & dataset5MapDataPoints(i).G.ToString & delimiter & dataset5MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                For i As Integer = 0 To dataset6MapDataPoints.Count - 1
                    If dataset6MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset6MapDataPoints(i).X.ToString & delimiter & dataset6MapDataPoints(i).Y.ToString & delimiter & dataset6MapDataPoints(i).Z.ToString & delimiter & dataset6MapDataPoints(i).I.ToString & delimiter & dataset6MapDataPoints(i).R.ToString & delimiter & dataset6MapDataPoints(i).G.ToString & delimiter & dataset6MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                For i As Integer = 0 To dataset7MapDataPoints.Count - 1
                    If dataset7MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset7MapDataPoints(i).X.ToString & delimiter & dataset7MapDataPoints(i).Y.ToString & delimiter & dataset7MapDataPoints(i).Z.ToString & delimiter & dataset7MapDataPoints(i).I.ToString & delimiter & dataset7MapDataPoints(i).R.ToString & delimiter & dataset7MapDataPoints(i).G.ToString & delimiter & dataset7MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                For i As Integer = 0 To dataset8MapDataPoints.Count - 1
                    If dataset8MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset8MapDataPoints(i).X.ToString & delimiter & dataset8MapDataPoints(i).Y.ToString & delimiter & dataset8MapDataPoints(i).Z.ToString & delimiter & dataset8MapDataPoints(i).I.ToString & delimiter & dataset8MapDataPoints(i).R.ToString & delimiter & dataset8MapDataPoints(i).G.ToString & delimiter & dataset8MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                For i As Integer = 0 To dataset9MapDataPoints.Count - 1
                    If dataset9MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset9MapDataPoints(i).X.ToString & delimiter & dataset9MapDataPoints(i).Y.ToString & delimiter & dataset9MapDataPoints(i).Z.ToString & delimiter & dataset9MapDataPoints(i).I.ToString & delimiter & dataset9MapDataPoints(i).R.ToString & delimiter & dataset9MapDataPoints(i).G.ToString & delimiter & dataset9MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                For i As Integer = 0 To dataset10MapDataPoints.Count - 1
                    If dataset10MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset10MapDataPoints(i).X.ToString & delimiter & dataset10MapDataPoints(i).Y.ToString & delimiter & dataset10MapDataPoints(i).Z.ToString & delimiter & dataset10MapDataPoints(i).I.ToString & delimiter & dataset10MapDataPoints(i).R.ToString & delimiter & dataset10MapDataPoints(i).G.ToString & delimiter & dataset10MapDataPoints(i).B.ToString)
                    End If
                Next
            End If
            'XYZRGBI Files
        ElseIf exportOption = 5 Or exportOption = 6 Then
            If headerRow Then
                exportStream.WriteLine("X" & delimiter & "Y" & delimiter & "Z" & delimiter & "R" & delimiter & "G" & delimiter & "B" & delimiter & "I")
            End If
            If dataset1CheckBox.Checked And dataset1CheckBox.Visible Then
                For i As Integer = 0 To dataset1MapDataPoints.Count - 1
                    If dataset1MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset1MapDataPoints(i).X.ToString & delimiter & dataset1MapDataPoints(i).Y.ToString & delimiter & dataset1MapDataPoints(i).Z.ToString & delimiter & dataset1MapDataPoints(i).R.ToString & delimiter & dataset1MapDataPoints(i).G.ToString & delimiter & dataset1MapDataPoints(i).B.ToString & delimiter & dataset1MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset2CheckBox.Checked And dataset2CheckBox.Visible Then
                For i As Integer = 0 To dataset2MapDataPoints.Count - 1
                    If dataset2MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset2MapDataPoints(i).X.ToString & delimiter & dataset2MapDataPoints(i).Y.ToString & delimiter & dataset2MapDataPoints(i).Z.ToString & delimiter & dataset2MapDataPoints(i).R.ToString & delimiter & dataset2MapDataPoints(i).G.ToString & delimiter & dataset2MapDataPoints(i).B.ToString & delimiter & dataset2MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset3CheckBox.Checked And dataset3CheckBox.Visible Then
                For i As Integer = 0 To dataset3MapDataPoints.Count - 1
                    If dataset3MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset3MapDataPoints(i).X.ToString & delimiter & dataset3MapDataPoints(i).Y.ToString & delimiter & dataset3MapDataPoints(i).Z.ToString & delimiter & dataset3MapDataPoints(i).R.ToString & delimiter & dataset3MapDataPoints(i).G.ToString & delimiter & dataset3MapDataPoints(i).B.ToString & delimiter & dataset3MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset4CheckBox.Checked And dataset4CheckBox.Visible Then
                For i As Integer = 0 To dataset4MapDataPoints.Count - 1
                    If dataset4MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset4MapDataPoints(i).X.ToString & delimiter & dataset4MapDataPoints(i).Y.ToString & delimiter & dataset4MapDataPoints(i).Z.ToString & delimiter & dataset4MapDataPoints(i).R.ToString & delimiter & dataset4MapDataPoints(i).G.ToString & delimiter & dataset4MapDataPoints(i).B.ToString & delimiter & dataset4MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset5CheckBox.Checked And dataset5CheckBox.Visible Then
                For i As Integer = 0 To dataset5MapDataPoints.Count - 1
                    If dataset5MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset5MapDataPoints(i).X.ToString & delimiter & dataset5MapDataPoints(i).Y.ToString & delimiter & dataset5MapDataPoints(i).Z.ToString & delimiter & dataset5MapDataPoints(i).R.ToString & delimiter & dataset5MapDataPoints(i).G.ToString & delimiter & dataset5MapDataPoints(i).B.ToString & delimiter & dataset5MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset6CheckBox.Checked And dataset6CheckBox.Visible Then
                For i As Integer = 0 To dataset6MapDataPoints.Count - 1
                    If dataset6MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset6MapDataPoints(i).X.ToString & delimiter & dataset6MapDataPoints(i).Y.ToString & delimiter & dataset6MapDataPoints(i).Z.ToString & delimiter & dataset6MapDataPoints(i).R.ToString & delimiter & dataset6MapDataPoints(i).G.ToString & delimiter & dataset6MapDataPoints(i).B.ToString & delimiter & dataset6MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset7CheckBox.Checked And dataset7CheckBox.Visible Then
                For i As Integer = 0 To dataset7MapDataPoints.Count - 1
                    If dataset7MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset7MapDataPoints(i).X.ToString & delimiter & dataset7MapDataPoints(i).Y.ToString & delimiter & dataset7MapDataPoints(i).Z.ToString & delimiter & dataset7MapDataPoints(i).R.ToString & delimiter & dataset7MapDataPoints(i).G.ToString & delimiter & dataset7MapDataPoints(i).B.ToString & delimiter & dataset7MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset8CheckBox.Checked And dataset8CheckBox.Visible Then
                For i As Integer = 0 To dataset8MapDataPoints.Count - 1
                    If dataset8MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset8MapDataPoints(i).X.ToString & delimiter & dataset8MapDataPoints(i).Y.ToString & delimiter & dataset8MapDataPoints(i).Z.ToString & delimiter & dataset8MapDataPoints(i).R.ToString & delimiter & dataset8MapDataPoints(i).G.ToString & delimiter & dataset8MapDataPoints(i).B.ToString & delimiter & dataset8MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset9CheckBox.Checked And dataset9CheckBox.Visible Then
                For i As Integer = 0 To dataset9MapDataPoints.Count - 1
                    If dataset9MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset9MapDataPoints(i).X.ToString & delimiter & dataset9MapDataPoints(i).Y.ToString & delimiter & dataset9MapDataPoints(i).Z.ToString & delimiter & dataset9MapDataPoints(i).R.ToString & delimiter & dataset9MapDataPoints(i).G.ToString & delimiter & dataset9MapDataPoints(i).B.ToString & delimiter & dataset9MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            If dataset10CheckBox.Checked And dataset10CheckBox.Visible Then
                For i As Integer = 0 To dataset10MapDataPoints.Count - 1
                    If dataset10MapDataPoints(i).deleted = False Then
                        exportStream.WriteLine(dataset10MapDataPoints(i).X.ToString & delimiter & dataset10MapDataPoints(i).Y.ToString & delimiter & dataset10MapDataPoints(i).Z.ToString & delimiter & dataset10MapDataPoints(i).R.ToString & delimiter & dataset10MapDataPoints(i).G.ToString & delimiter & dataset10MapDataPoints(i).B.ToString & delimiter & dataset10MapDataPoints(i).I.ToString)
                    End If
                Next
            End If
            End If
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            mapPanel.Cursor = currentMapCursor
            exportStream.Close()
            Return False
        End Try
        Me.Cursor = Cursors.Default
        mapPanel.Cursor = currentMapCursor
        exportStream.Close()
        Return True
    End Function

    Private Sub lasShell2_ErrorOccurred(ByVal ErrorType As String) Handles lasShell2.ErrorOccurred
        MessageBox.Show(ErrorType, "libLAS PROCESSING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    'unorthodox way of sort of handling a DONE type event of lasShell2 
    'But it works at avoiding cross-threading calls to the forms controls
    Private Sub lasShell2_Tick() Handles lasShell2.Tick
        If lasShell2.isRunning = False Then
            lasShell2.TimerStop()
            'safe to now encode the LAS file into an e57 file (done in the background)
            'System.Threading.Thread.Sleep(2000)
            If File.Exists(Application.StartupPath & "\pointcloud.txt") Then
                File.Delete(Application.StartupPath & "\pointcloud.txt")
            End If
            'setting up the options for encoding a TXT file to a LAS file
            e57Shell.InputFile = ControlChars.Quote & Application.StartupPath & "\pointcloud.las" & ControlChars.Quote
            e57Shell.OutputFile = ControlChars.Quote & exportDialog.FileName & ControlChars.Quote
            e57Shell.TimerInterval = 100
            'start the encoding process
            If e57Shell.StartEncoding(lasShell2.processingTime) Then
                'nothing to do here
            End If
        Else
            updatePercentage(lasShell2.processingTime.ToString("f1") & " s")
        End If
    End Sub

    Private Sub e57Shell_ErrorOccurred(ByVal ErrorType As String) Handles e57Shell.ErrorOccurred
        MessageBox.Show(ErrorType, "libE57 PROCESSING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    'unorthodox way of sort of handling a DONE type event of e57Shell 
    'But it works at avoiding cross-threading calls to the forms controls
    Private Sub e57Shell_Tick() Handles e57Shell.Tick
        If e57Shell.isRunning = False Then
            e57Shell.TimerStop()
            statusLabel.Text = "e57 Encoding Completed in " & (e57Shell.processingTime).ToString("f1") & " sec"
            statusLabel.Refresh()
            MessageBox.Show("Export complete", "RGBi", MessageBoxButtons.OK)
            updatePercentage("")
            If File.Exists(Application.StartupPath & "\pointcloud.las") Then
                File.Delete(Application.StartupPath & "\pointcloud.las")
            End If
            statusLabel.Text = ""
        Else
            updatePercentage(e57Shell.processingTime.ToString("f1") & " s")
        End If
    End Sub

    Private Sub activeCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles activeCheckBox.CheckedChanged
        activeRender = activeCheckBox.Checked
    End Sub

    Private Sub ViewIntensityHistogramToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewIntensityHistogramToolStripMenuItem.Click
        If intensityHistogram.GetLength(0) > 1 Then
            If histogramDisplayed Then
                histogramToShow = 1   'option set to display the global Histogram (for ALL datasets)
                histogramForm.renderHistogram()
            Else
                histogramDisplayed = True
                histogramToShow = 1   'option set to display the global Histogram (for ALL datasets)
                histogramForm.Show()
            End If
        Else
            MessageBox.Show("No Intensity Histogram Data is available to plot", "MISSING DATA", MessageBoxButtons.OK, MessageBoxIcon.Information)
            histogramDisplayed = False
        End If
    End Sub

    Private Sub bbCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bbCheckBox.CheckedChanged
        drawMapButton_Click(sender, e)
    End Sub

    Private Sub removeSlideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles removeSlideButton.Click
        If imageOffsets(0) <> 0 Or imageOffsets(1) <> 0 Then
            imageOffsets(0) = 0
            imageOffsets(1) = 0
            drawMapButton_Click(sender, e)
        End If
    End Sub

    Private Sub processTargetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles processCircleTargetButton.Click
        If BackgroundWorker1.IsBusy <> True Then
            ' Start the asynchronous operation.
            Me.Cursor = Cursors.AppStarting
            If mapPanel.Cursor = Cursors.Cross Then
                currentMapCursor = Cursors.Cross
            Else
                currentMapCursor = Cursors.Hand
            End If
            mapPanel.Cursor = Cursors.AppStarting
            BackgroundWorker1.RunWorkerAsync()
        Else
            MessageBox.Show("The application is busy at the current moment so please wait until the previous process finishes", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            ''for testing purposes only
            'Dim writefile As New System.IO.StreamWriter("C:/test2.txt")

            Dim countSelected As Integer = 0
            Dim targetHistogram(0) As Integer

            'determine selected points on screen as well as build the histogram for the ALL the selected points 
            For i As Integer = 0 To currentMapDataPoints.Count - 1
                If currentMapDataPoints(i).highlighted AndAlso Not currentMapDataPoints(i).deleted Then
                    countSelected += 1
                End If
            Next

            If countSelected >= 3 Then
                updateStatus("Point cloud circular target recognition has started...")
                System.Threading.Thread.Sleep(2000)
                Dim A, Data As New Matrix(countSelected, 3)
                Dim w, c, d, f, delta As New Matrix
                Dim Normal As New Matrix(3, 3)
                Dim residuals_sum As New Matrix(3, 1)
                Dim currentRow As Integer = 1

                For k As Integer = 0 To currentMapDataPoints.Count - 1
                    If currentMapDataPoints(k).highlighted AndAlso Not currentMapDataPoints(k).deleted Then
                        Data.data(currentRow, 1) = currentMapDataPoints(k).X
                        Data.data(currentRow, 2) = currentMapDataPoints(k).Y
                        Data.data(currentRow, 3) = currentMapDataPoints(k).Z
                        currentRow += 1
                    End If
                Next

                'least squares plane fitting
                For i As Integer = 1 To 3
                    A.equals(Data)
                    For j As Integer = 1 To A.nRows
                        A.data(j, i) = 1
                    Next
                    w = Data.getColumn(i)
                    d = (A.Transpose * A).Inverse * A.Transpose * w
                    c = -1 * d
                    c.data(i, 1) = 1
                    d.data(i, 1) = 1
                    f = c / Matrix.abs(d)
                    For k As Integer = 1 To 3
                        Normal.data(k, i) = f.data(k, 1)
                    Next
                    A.clear()
                Next

                Dim avgX As Decimal = 0
                Dim avgY As Decimal = 0
                Dim avgZ As Decimal = 0
                Dim samples As Integer = 0

                For i As Integer = 1 To Data.nRows
                    Try
                        avgX += Data.data(i, 1)
                        avgY += Data.data(i, 2)
                        avgZ += Data.data(i, 3)
                        samples += 1
                    Catch ex As Exception
                        Exit For
                    End Try
                Next

                avgX = avgX / samples
                avgY = avgY / samples
                avgZ = avgZ / samples

                Dim avgPoint As New Matrix(3, 1)
                avgPoint.data(1, 1) = avgX
                avgPoint.data(2, 1) = avgY
                avgPoint.data(3, 1) = avgZ

                Dim offcenter As New Matrix(Data.nRows, Data.nCols)
                For i As Integer = 1 To offcenter.nRows
                    offcenter.data(i, 1) = Data.data(i, 1) - avgX
                    offcenter.data(i, 2) = Data.data(i, 2) - avgY
                    offcenter.data(i, 3) = Data.data(i, 3) - avgZ
                Next

                Dim residuals As New Matrix
                'Dim residualsTotal As New Matrix(Data.nRows, 3)

                For i As Integer = 1 To 3
                    residuals = offcenter * Normal.getColumn(i)
                    For j As Integer = 1 To residuals.nRows
                        'residualsTotal.data(j, i) = residuals.data(j, 1)
                        residuals_sum.data(i, 1) += residuals.data(j, 1) ^ 2
                    Next
                Next

                Dim bestsolution As Integer = 1
                Dim min As Decimal = residuals_sum.data(1, 1)
                If residuals_sum.data(2, 1) < min Then
                    bestsolution = 2
                    min = residuals_sum.data(2, 1)
                End If
                If residuals_sum.data(3, 1) < min Then
                    bestsolution = 3
                End If

                'Dim v As New Matrix
                'v = residualsTotal.getColumn(bestsolution)

                planeNormal.data(1, 1) = Decimal.Round(Normal.data(1, bestsolution), 8)
                planeNormal.data(2, 1) = Decimal.Round(Normal.data(2, bestsolution), 8)
                planeNormal.data(3, 1) = Decimal.Round(Normal.data(3, bestsolution), 8)

                Dim closestDist As Decimal = 0
                samples = 0
                For i As Integer = 1 To Data.nRows
                    Try
                        closestDist += Matrix.dot(planeNormal, Data.getRow(i).Transpose)
                        samples += 1
                    Catch ex As Exception
                        Exit For
                    End Try
                Next

                closestDist = closestDist / samples
                pointOnPlane = closestDist * planeNormal

                'updatestatus("Adjusting all target selection points to now be positioned on a single plane")
                Dim mapPoint As New cloudPoint
                Dim oldPoint As New Matrix(3, 1)
                Dim newPoint As New Matrix(3, 1)
                Dim dist As Decimal
                Dim performRot As Boolean = True

                Dim traceYZ, alpha, beta As Decimal

                If planeNormal.data(1, 1) = 0 AndAlso planeNormal.data(2, 1) = 0 Then
                    'plane of adjusted points is already exactly within the World XY plane
                    performRot = False
                Else
                    traceYZ = Math.Sqrt(planeNormal.data(2, 1) ^ 2 + planeNormal.data(3, 1) ^ 2)
                    beta = Math.Acos(traceYZ / Matrix.abs(planeNormal)) * 180D / Math.PI
                    If planeNormal.data(1, 1) <= 0 Then
                        beta *= -1
                    End If

                    If traceYZ = 0 Then
                        If planeNormal.data(2, 1) > 0 Then
                            alpha = 90
                        Else
                            alpha = -90
                        End If
                    Else
                        alpha = Math.Acos(planeNormal.data(3, 1) / traceYZ) * 180D / Math.PI
                    End If

                    If planeNormal.data(2, 1) > 0 Then
                        alpha *= -1
                    End If
                End If

                For k As Integer = 0 To currentMapDataPoints.Count - 1
                    If currentMapDataPoints(k).highlighted AndAlso Not currentMapDataPoints(k).deleted Then
                        mapPoint.B = currentMapDataPoints(k).B
                        mapPoint.datasetNum = currentMapDataPoints(k).datasetNum
                        mapPoint.deleted = currentMapDataPoints(k).deleted
                        mapPoint.displayX = currentMapDataPoints(k).displayX
                        mapPoint.displayY = currentMapDataPoints(k).displayY
                        mapPoint.G = currentMapDataPoints(k).G
                        mapPoint.highlighted = currentMapDataPoints(k).highlighted
                        mapPoint.I = currentMapDataPoints(k).I
                        mapPoint.inView = currentMapDataPoints(k).inView
                        mapPoint.R = currentMapDataPoints(k).R
                        mapPoint.Xm = currentMapDataPoints(k).Xm
                        mapPoint.Ym = currentMapDataPoints(k).Ym
                        'mapPoint.Zm = currentMapDataPoints(k).Zm
                        oldPoint.data(1, 1) = currentMapDataPoints(k).X
                        oldPoint.data(2, 1) = currentMapDataPoints(k).Y
                        oldPoint.data(3, 1) = currentMapDataPoints(k).Z
                        dist = Matrix.dot((pointOnPlane - oldPoint), planeNormal) / Matrix.dot(planeNormal, planeNormal)
                        If performRot Then
                            'beta = 0
                            'alpha = 0
                            newPoint = Matrix.R2(beta) * Matrix.R1(alpha) * (oldPoint + dist * planeNormal)
                        Else
                            newPoint = (oldPoint + dist * planeNormal)
                        End If
                        mapPoint.X = newPoint.data(1, 1)
                        mapPoint.Y = newPoint.data(2, 1)
                        mapPoint.Z = newPoint.data(3, 1)
                        currentMapDataPoints(k) = mapPoint
                        'writefile.WriteLine(mapPoint.X.ToString("f5") & " " & mapPoint.Y.ToString("f5") & " " & mapPoint.Z.ToString("f5") & " " & mapPoint.R & " " & mapPoint.G & " " & mapPoint.B & " " & mapPoint.I)
                    End If
                Next

                If processingOption = 1 Then
                    'histogram cutoff to filter out all points but the middle 1% (49.5% on each end)
                    histogramCutoff = 50
                    selectedHistMaxMin = calcHistogramBounds(selectedHistogram)
                ElseIf processingOption = 2 Then
                    Dim numSelectedPoints As Integer
                    For i As Integer = 0 To selectedHistogram.GetLength(0) - 1
                        numSelectedPoints += selectedHistogram(i)
                    Next

                    Dim midPoint As Integer = Convert.ToInt32(Math.Round(numSelectedPoints / 2.0R, 0))
                    Dim midIntensity As Integer

                    'start with 1% of counted points as midIntensity point
                    histogramCutoff = 50
                    selectedHistMaxMin = calcHistogramBounds(selectedHistogram)

                    midIntensity = Convert.ToInt32(Math.Round(selectedHistMaxMin(1) + (selectedHistMaxMin(0) - selectedHistMaxMin(1)) / 2, 0))

                    Dim j As Integer = 0
                    Dim k As Integer = midIntensity
                    Dim peakCounts() As Integer = {0, 0}
                    Dim peakIntensities() As Integer = {0, 0}

                    While j < midIntensity
                        If selectedHistogram(j) > peakCounts(0) Then
                            peakCounts(0) = selectedHistogram(j)
                            peakIntensities(0) = j
                        End If
                        j += 1
                    End While

                    While k < selectedHistogram.GetLength(0) - 1
                        If selectedHistogram(k) > peakCounts(1) Then
                            peakCounts(1) = selectedHistogram(k)
                            peakIntensities(1) = k
                        End If
                        k += 1
                    End While

                    Dim halfOnePercent As Double = Convert.ToInt32(Math.Round(0.0025 * numSelectedPoints, 0))
                    Dim avgIntensity As Integer = Convert.ToInt32(Math.Round((peakIntensities(0) + peakIntensities(1)) / 2.0R, 0))

                    j = avgIntensity
                    k = avgIntensity - 1
                    Dim highCount As Integer = 0
                    Dim lowCount As Integer = 0
                    Dim highStop As Boolean = False
                    Dim lowStop As Boolean = False

                    While Not highStop
                        highCount += selectedHistogram(j)
                        If highCount >= halfOnePercent Then
                            selectedHistMaxMin(0) = j
                            highStop = True
                        End If
                        j += 1
                        If j = selectedHistogram.GetLength(0) Then
                            selectedHistMaxMin(0) = selectedHistogram.GetLength(0)
                            highStop = True
                        End If
                    End While

                    While Not lowStop
                        Try
                            lowCount += selectedHistogram(k)
                        Catch ex As Exception
                        End Try

                        If lowCount >= halfOnePercent Then
                            selectedHistMaxMin(1) = k
                            lowStop = True
                        End If
                        k -= 1
                        If k = -1 Then
                            selectedHistMaxMin(1) = 0
                            lowStop = True
                        End If
                    End While


                ElseIf processingOption = 3 Then
                    'do nothing selectedHistMaxMin is already updated from the processingOptions form
                End If

                calcDisplayXYZ(currentMapDataPoints)

                Dim circlePoints As New Generic.List(Of cloudPoint)

                'populate the circle edge points based on distribution of black and white points
                For i As Integer = 0 To currentMapDataPoints.Count - 1
                    If currentMapDataPoints(i).highlighted AndAlso Not currentMapDataPoints(i).deleted AndAlso currentMapDataPoints(i).I >= selectedHistMaxMin(1) AndAlso currentMapDataPoints(i).I <= selectedHistMaxMin(0) Then
                        circlePoints.Add(currentMapDataPoints(i))
                        'writefile.WriteLine(currentMapDataPoints(i).X.ToString("f5") & " " & currentMapDataPoints(i).Y.ToString("f5") & " " & currentMapDataPoints(i).Z.ToString("f5") & " " & currentMapDataPoints(i).R & " " & currentMapDataPoints(i).G & " " & currentMapDataPoints(i).B & " " & currentMapDataPoints(i).I)
                    End If
                Next

                If circlePoints.Count > 3 Then
                    'least squares circle fitting
                    Dim messageresult As DialogResult = Windows.Forms.DialogResult.Yes
                    If circlePoints.Count > 200 Then
                        messageresult = MessageBox.Show("You are about to process " & circlePoints.Count.ToString & " points within a circle fitting least-squares adjustment, this may take a long time to process. Are you sure you want to continue?", "NUMBER CRUNCHER", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                    End If

                    If messageresult = Windows.Forms.DialogResult.Yes Then

                        Dim A2 As New Matrix(circlePoints.Count, 3)
                        Dim Xo, D2, X2 As New Matrix(3, 1)
                        Dim BBtInv, V2 As New Matrix
                        Dim B As New Matrix(A2.nRows, (A2.nRows * 2I))
                        Dim w2 As New Matrix(A2.nRows, 1)
                        Dim Lobs, L As New Matrix(A2.nRows * 2, 1)

                        avgPoint = Matrix.R2(beta) * Matrix.R1(alpha) * avgPoint
                        Xo.data(1, 1) = avgPoint.data(1, 1)   'X coordinate for the centre of the circle
                        Xo.data(2, 1) = avgPoint.data(2, 1)   'Y coordinate for the centre of the circle
                        Xo.data(3, 1) = 0.1   'radius of the circle

                        For i As Integer = 1 To A2.nRows
                            Lobs.data(2 * i - 1, 1) = circlePoints(i - 1).X
                            Lobs.data(2 * i, 1) = circlePoints(i - 1).Y
                        Next

                        Dim iteration As Integer = 1
                        Dim stopYesNo As Boolean = False
                        Dim solutionFound As Boolean

                        'start iterative least squares
                        updateStatus("Least Squares analysis running (1)")
                        Dim planeHeight As Decimal = circlePoints(0).Z
                        While Not stopYesNo
                            For i As Integer = 1 To A2.nRows
                                A2.data(i, 1) = -2 * (circlePoints(i - 1).X - Xo.data(1, 1))
                                A2.data(i, 2) = -2 * (circlePoints(i - 1).Y - Xo.data(2, 1))
                                A2.data(i, 3) = -2 * Xo.data(3, 1)
                            Next

                            For i As Integer = 1 To B.nRows
                                B.data(i, 2 * i - 1) = 2 * (circlePoints(i - 1).X - Xo.data(1, 1))
                                B.data(i, 2 * i) = 2 * (circlePoints(i - 1).Y - Xo.data(2, 1))
                            Next

                            For i As Integer = 1 To w2.nRows
                                w2.data(i, 1) = (circlePoints(i - 1).X - Xo.data(1, 1)) ^ 2 + (circlePoints(i - 1).Y - Xo.data(2, 1)) ^ 2 - Xo.data(3, 1) ^ 2
                            Next

                            'w2 = w2 + B * (Lobs - L)

                            BBtInv = (B * B.Transpose).Inverse

                            D2 = -1 * ((A2.Transpose * BBtInv * A2).Inverse * A2.Transpose * BBtInv * w2)
                            X2 = Xo + D2
                            V2 = -1 * (B.Transpose * BBtInv * (A2 * D2 + w2))
                            L = Lobs + V2
                            If Math.Abs(X2.data(1, 1) - Xo.data(1, 1)) <= 0.001 AndAlso Math.Abs(X2.data(2, 1) - Xo.data(2, 1)) <= 0.001 AndAlso Math.Abs(X2.data(3, 1) - Xo.data(3, 1)) <= 0.001 Then
                                stopYesNo = True
                                solutionFound = True
                            Else
                                iteration += 1
                                updateStatus("Least Squares analysis running (" & iteration.ToString & ")")
                                Xo.equals(X2)
                                If iteration = maxIterations Then
                                    stopYesNo = True
                                    solutionFound = False
                                End If
                            End If
                        End While

                        If solutionFound Then
                            targetRadius = X2.data(3, 1)
                            updateStatus("Circular Target found, computing statistics")
                            Dim Cx, Cv, StdDevX, StdDevV As New Matrix
                            Cx = 10 * ((V2.Transpose * V2).toScalar / (A2.nRows - 3)) * (A2.Transpose * BBtInv * A2).Inverse
                            StdDevX = Cx.getDiagonal.Sqrt
                            Cv = B.Transpose * BBtInv * B - B.Transpose * BBtInv * A2 * (A2.Transpose * BBtInv * A2).Inverse * A2.Transpose * BBtInv * B
                            StdDevV = Cv.getDiagonal.Sqrt

                            'compute points on boundary edge of target circle as computed by the LSA
                            Dim targetCircle As New cloudCircle
                            targetCircle.edgePoints = New Generic.List(Of cloudPoint)

                            For i As Integer = 0 To 356 Step 4
                                Dim targetPoint As New cloudPoint
                                Dim targetVector As New Matrix(3, 1)
                                targetVector.data(1, 1) = Math.Sin(i * Math.PI / 180D) * X2.data(3, 1) + X2.data(1, 1)
                                targetVector.data(2, 1) = Math.Cos(i * Math.PI / 180D) * X2.data(3, 1) + X2.data(2, 1)
                                targetVector.data(3, 1) = planeHeight
                                'undo previous rotations to put the point back into the World Coordinate System
                                targetVector = Matrix.R1(-1 * alpha) * Matrix.R2(-1 * beta) * targetVector
                                targetPoint.X = targetVector.data(1, 1)
                                targetPoint.Y = targetVector.data(2, 1)
                                targetPoint.Z = targetVector.data(3, 1)
                                targetCircle.edgePoints.Add(targetPoint)
                                'writefile.WriteLine(targetPoint.X.ToString("f5") & " " & targetPoint.Y.ToString("f5") & " " & targetPoint.Z.ToString("f5") & " 150 150 150 1500")
                            Next

                            targetSolution.data(1, 1) = X2.data(1, 1)
                            targetSolution.data(2, 1) = X2.data(2, 1)
                            targetSolution.data(3, 1) = planeHeight
                            'undo previous rotations to put the point back into the World Coordinate System
                            targetSolution = Matrix.R1(-1 * alpha) * Matrix.R2(-1 * beta) * targetSolution
                            targetCircle.centrePoint.X = targetSolution.data(1, 1)
                            targetCircle.centrePoint.Y = targetSolution.data(2, 1)
                            targetCircle.centrePoint.Z = targetSolution.data(3, 1)
                            'writefile.WriteLine(targetCircle.centrePoint.X.ToString("f5") & " " & targetCircle.centrePoint.Y.ToString("f5") & " " & targetCircle.centrePoint.Z.ToString("f5") & " 150 150 150 1500")

                            targetCircle.colour = processedTargetsColorLabel.BackColor  'this setting is not actually used as when the target circles are rendered the colour is as user defined in the targetColourLabel 
                            targetCircle.weight = targetsLineWeight 'this setting is not actually used as when the target circles are rendered the line weight is user defined (right-click targetColourLabel)

                            targetCircles.Add(targetCircle)
                            targetsFound = True
                        Else
                            MessageBox.Show("The boundary edge points for the circular target are of low quality and a statistically reliable solution is not possible. If possible try increasing the point cloud density for the circular target.", "LOW QUALITY POINTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            e.Cancel = True
                        End If
                    Else
                        MessageBox.Show("Not enough boundary edge points for the circular target have been identified, no solution is possible (4 points minimum)", "NOT ENOUGH POINTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        e.Cancel = True
                    End If
                End If
            Else
                MessageBox.Show("There are not enough points currently selected on the screen to compute the best fitting plane (3 points minimum)", "NOT ENOUGH POINTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                e.Cancel = True
            End If
            'writefile.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred trying to process your selected circular target, please try re-selecting your target within the point cloud and reprocessing", "CIRCULAR TARGET PROCESSING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
        End Try
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        If e.Cancelled <> True Then
            histogramCutoff = histogramCutOffsComboBox.SelectedIndex
            intensityHistMaxMin = calcHistogramBounds(intensityHistogram)
            selectedHistMaxMin = calcHistogramBounds(selectedHistogram)
            updateCurrentDataset(e)
            clearSelButton_Click(sender, e)
            maxTargetPoints += 1
            Dim targetData() As String = {maxTargetPoints.ToString, targetSolution.data(1, 1).ToString("f4"), targetSolution.data(2, 1).ToString("f4"), targetSolution.data(3, 1).ToString("f4"), targetRadius.ToString("f4"), "Circular"}
            targetsDataGridView.Rows.Add(targetData)
            GRcolumn4.Items.Add(maxTargetPoints.ToString)
        End If

        statusLabel.Text = String.Empty
        Me.Cursor = Cursors.Default
        mapPanel.Cursor = currentMapCursor
    End Sub

    Private Sub scrubDatasetOfDuplicates(ByRef mapDataPoints As Generic.List(Of cloudPoint))
        Dim countSelected As Integer = 0
        Dim duplicateCount As Integer = 0
        Dim stopIndex As Integer = mapDataPoints.Count - 1
        Dim m As Integer = 0
        Dim n As Integer = 0

        'check for and remove duplicate points from a specific dataset (while loops used to allow for the highest search Index number to change dynamically)
        Dim percentage As Integer
        While m <= stopIndex
            n = m + 1
            While n <= stopIndex
                If mapDataPoints(m).X = mapDataPoints(n).X AndAlso mapDataPoints(m).Y = mapDataPoints(n).Y AndAlso mapDataPoints(m).Z = mapDataPoints(n).Z Then
                    mapDataPoints.RemoveAt(n)
                    stopIndex -= 1
                    duplicateCount += 1
                    If duplicateCount = 1 Then
                        statusLabel.Text = "Scrubbing dataset of identical points (" & duplicateCount.ToString & " point found)"
                    Else
                        statusLabel.Text = "Scrubbing dataset of identical points (" & duplicateCount.ToString & " points found)"
                    End If
                End If
                n += 1
            End While
            countSelected += 1
            m += 1
            percentage = Convert.ToInt32(Decimal.Round(Convert.ToDecimal(m) / Convert.ToDecimal(stopIndex) * 100D, 0))
            BackgroundWorker3.ReportProgress(percentage)
        End While
    End Sub

    Private Sub RemoveIdenticalPointsFromThisDatasetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveIdenticalPointsFromThisDatasetToolStripMenuItem.Click
        BackgroundWorker3.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker3_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker3.DoWork
        updateStatus("Scrubbing dataset of identical points")
        If currentRightClickDataset = 1 Then
            scrubDatasetOfDuplicates(dataset1MapDataPoints)
        ElseIf currentRightClickDataset = 2 Then
            scrubDatasetOfDuplicates(dataset2MapDataPoints)
        ElseIf currentRightClickDataset = 3 Then
            scrubDatasetOfDuplicates(dataset3MapDataPoints)
        ElseIf currentRightClickDataset = 4 Then
            scrubDatasetOfDuplicates(dataset4MapDataPoints)
        ElseIf currentRightClickDataset = 5 Then
            scrubDatasetOfDuplicates(dataset5MapDataPoints)
        ElseIf currentRightClickDataset = 6 Then
            scrubDatasetOfDuplicates(dataset6MapDataPoints)
        ElseIf currentRightClickDataset = 7 Then
            scrubDatasetOfDuplicates(dataset7MapDataPoints)
        ElseIf currentRightClickDataset = 8 Then
            scrubDatasetOfDuplicates(dataset8MapDataPoints)
        ElseIf currentRightClickDataset = 9 Then
            scrubDatasetOfDuplicates(dataset9MapDataPoints)
        ElseIf currentRightClickDataset = 10 Then
            scrubDatasetOfDuplicates(dataset10MapDataPoints)
        End If
        updateStatus(String.Empty)
    End Sub

    Private Sub BackgroundWorker3_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker3.ProgressChanged
        Try

            percentageLabel.Text = e.ProgressPercentage.ToString & " %"
        Catch ex As Exception
            'possible error when scrubbing data and the program is closed by the user
        End Try

    End Sub

    Private Sub BackgroundWorker3_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker3.RunWorkerCompleted
        statusLabel.Text = String.Empty
    End Sub

    Private Sub viewSelectedHistogramButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewSelectedHistogramButton.Click
        If selectedHistogram.GetLength(0) > 1 Then
            If histogramDisplayed Then
                histogramToShow = 2  'option set to display the Histogram for ALL data points within the current selection
                histogramForm.renderHistogram()
            Else
                histogramDisplayed = True
                histogramToShow = 2  'option set to display the Histogram for ALL data points within the current selection
                histogramForm.Show()
            End If
        Else
            MessageBox.Show("No Intensity Histogram Data is available to plot, you must first select a region of points on the screen", "MISSING DATA", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub displayTargetsCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles displayTargetsCheckBox.CheckedChanged
        If mapDisplayed And targetsFound Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tSmallToolStripMenuItem.Click
        targetsLineWeight = 1
        If targetsFound Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tMediumToolStripMenuItem.Click
        targetsLineWeight = 2
        If targetsFound Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tLargeToolStripMenuItem.Click
        targetsLineWeight = 4
        If targetsFound Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tFatToolStripMenuItem.Click
        targetsLineWeight = 8
        If targetsFound Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub ToolStripMenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tObeseToolStripMenuItem.Click
        targetsLineWeight = 16
        If targetsFound Then
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub optionsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optionsButton.Click
        targetProcessingOptions.ShowDialog()
    End Sub

    Private Sub addTiePointButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addTiePointButton.Click
        maxTiePoints += 1
        Dim newRowDefaults(1) As Object
        newRowDefaults(0) = True
        newRowDefaults(1) = maxTiePoints
        geoRefDataGridView.Rows.Add(newRowDefaults)

    End Sub

    Private Sub geoRefDataGridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles geoRefDataGridView.CellEndEdit
        If e.ColumnIndex = 4 Then
            If IsNothing(geoRefDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = False Then
                Dim rowIndex As Integer = -1
                For Each row As DataGridViewRow In controlPtsDataGridView.Rows
                    rowIndex += 1
                    If row.Cells(0).Value = geoRefDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Then
                        Exit For
                    End If
                Next
                If IsNothing(controlPtsDataGridView.Rows(rowIndex).Cells(2).Value) = False And IsNothing(controlPtsDataGridView.Rows(rowIndex).Cells(3).Value) = False And IsNothing(controlPtsDataGridView.Rows(rowIndex).Cells(4).Value) = False Then
                    If controlPtsDataGridView.Rows(rowIndex).Cells(2).Value.ToString <> String.Empty And controlPtsDataGridView.Rows(rowIndex).Cells(3).Value.ToString <> String.Empty And controlPtsDataGridView.Rows(rowIndex).Cells(4).Value.ToString <> String.Empty Then
                        If IsNumeric(controlPtsDataGridView.Rows(rowIndex).Cells(2).Value) = True And IsNumeric(controlPtsDataGridView.Rows(rowIndex).Cells(3).Value) = True And IsNumeric(controlPtsDataGridView.Rows(rowIndex).Cells(4).Value) = True Then
                        Else
                            MessageBox.Show("Invalid control point selected. Control point must have X,Y,Z coordinates.", "INVALID CONTROL POINT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            geoRefDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Nothing
                        End If
                    Else
                        MessageBox.Show("Invalid control point selected. Control point must have X,Y,Z coordinates.", "INVALID CONTROL POINT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        geoRefDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Nothing
                    End If
                Else
                    MessageBox.Show("Invalid control point selected. Control point must have X,Y,Z coordinates.", "INVALID CONTROL POINT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    geoRefDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub geoRefDataGridView_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles geoRefDataGridView.MouseEnter
        geoRefDataGridView.Focus()
    End Sub

    Private Sub geoRefGroupBox_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles geoRefGroupBox.MouseEnter
        geoRefGroupBox.Focus()
    End Sub

    Private Sub deleteAllTiePointsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles deleteAllTiePointsButton.Click
        Dim msgboxResult As DialogResult = MessageBox.Show("Are you sure you want to delete all Tie Points?", "DOUBLE CHECK", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If msgboxResult = Windows.Forms.DialogResult.Yes Then
            geoRefDataGridView.Rows.Clear()
            maxTiePoints = 0
        End If
    End Sub

    Private Sub clearTargetsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearTargetsButton.Click
        If maxTargetPoints <> 0 Then
            Dim msgboxResult As DialogResult = MessageBox.Show("Are you sure you want to delete all Target Points?", "DOUBLE CHECK", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If msgboxResult = Windows.Forms.DialogResult.Yes Then
                targetsDataGridView.Rows.Clear()
                targetCircles.Clear()
                maxTargetPoints = 0
                For Each GRrow As DataGridViewRow In geoRefDataGridView.Rows
                    If IsNothing(GRrow.Cells(3).Value) = False Then
                        GRrow.Cells(3).Value = Nothing
                    End If
                Next
                GRcolumn4.Items.Clear()
                drawMapButton_Click(refreshMapButton, e)
            End If
        End If
    End Sub

    Private Sub closeGeoRefButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles closeGeoRefButton.Click
        RegistrationGeoreferencingOptionsToolStripMenuItem.Checked = False
    End Sub

    Private Sub addControlPtButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addControlPtButton.Click
        maxControlPoints += 1
        Dim newRowDefaults(0) As Object
        newRowDefaults(0) = maxControlPoints
        controlPtsDataGridView.Rows.Add(newRowDefaults)
        GRcolumn5.Items.Add(maxControlPoints.ToString)
    End Sub

    Private Sub controlPtsDataGridView_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles controlPtsDataGridView.MouseEnter
        controlPtsDataGridView.Focus()
    End Sub

    Private Sub controlPtsDataGridView_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles controlPtsDataGridView.UserDeletingRow
        Dim warningOnDelete As Boolean = False
        For Each GRrow As DataGridViewRow In geoRefDataGridView.Rows
            If IsNothing(GRrow.Cells(4).Value) = False Then
                If GRrow.Cells(4).Value.ToString = e.Row.Cells(0).Value.ToString Then
                    warningOnDelete = True
                End If
            End If
        Next

        If warningOnDelete = True Then
            Dim msgboxResult As DialogResult = MessageBox.Show("This Control Point is used to define 1 or more Tie Points." & ControlChars.NewLine & "If deleted the Tie Point definitions will be changed." & ControlChars.NewLine & ControlChars.NewLine & "Are you sure you want to delete it?", "DELETE CONTROL POINT ?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If msgboxResult = Windows.Forms.DialogResult.Yes Then
                For Each GRrow As DataGridViewRow In geoRefDataGridView.Rows
                    If IsNothing(GRrow.Cells(4).Value) = False Then
                        If GRrow.Cells(4).Value.ToString = e.Row.Cells(0).Value.ToString Then
                            GRrow.Cells(4).Value = Nothing
                        End If
                    End If
                Next
                GRcolumn5.Items.Remove(e.Row.Cells(0).Value.ToString)
            Else
                e.Cancel = True
            End If
        Else
            GRcolumn5.Items.Remove(e.Row.Cells(0).Value.ToString)
        End If

    End Sub

    Private Sub deleteAllControlPtsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles deleteAllControlPtsButton.Click
        Dim msgboxResult As DialogResult = MessageBox.Show("Are you sure you want to delete all Control Points?", "DOUBLE CHECK", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If msgboxResult = Windows.Forms.DialogResult.Yes Then
            controlPtsDataGridView.Rows.Clear()
            maxControlPoints = 0
            For Each GRrow As DataGridViewRow In geoRefDataGridView.Rows
                If IsNothing(GRrow.Cells(4).Value) = False Then
                    GRrow.Cells(4).Value = Nothing
                End If
            Next
            GRcolumn5.Items.Clear()
        End If
    End Sub

    Private Sub importCPButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles importCPButton.Click
        Dim importDialog As DialogResult = importCSVFileDialog.ShowDialog
        If importDialog = Windows.Forms.DialogResult.OK Then
            Dim importStream As New StreamReader(importCSVFileDialog.FileName)
            Dim delims(0) As Char
            delims(0) = ","
            While importStream.Peek <> -1
                Dim lineRead As String = importStream.ReadLine
                Dim lineParts() As String = lineRead.Split(delims)
                Try
                    addControlPtButton_Click(sender, e)
                    controlPtsDataGridView.Rows(controlPtsDataGridView.Rows.Count - 1).Cells.Item(1).Value = lineParts(0).ToString
                    controlPtsDataGridView.Rows(controlPtsDataGridView.Rows.Count - 1).Cells.Item(2).Value = lineParts(1).ToString
                    controlPtsDataGridView.Rows(controlPtsDataGridView.Rows.Count - 1).Cells.Item(3).Value = lineParts(2).ToString
                    controlPtsDataGridView.Rows(controlPtsDataGridView.Rows.Count - 1).Cells.Item(4).Value = lineParts(3).ToString
                    controlPtsDataGridView.Rows(controlPtsDataGridView.Rows.Count - 1).Cells.Item(5).Value = lineParts(4).ToString
                Catch ex As Exception
                End Try
            End While

            importStream.Close()
        End If
    End Sub

    Private Sub exportCPButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exportCPButton.Click
        exportCSVFileDialog.Title = "Select Path and Name of Control Points File to Export"
        Dim exportDialog As DialogResult = exportCSVFileDialog.ShowDialog
        If exportDialog = Windows.Forms.DialogResult.OK Then
            Dim exportStream As New StreamWriter(exportCSVFileDialog.FileName)
            For Each cpRow As DataGridViewRow In controlPtsDataGridView.Rows
                Dim c1, c2, c3, c4, c5 As String

                If IsNothing(cpRow.Cells.Item(1).Value) = True Then
                    c1 = String.Empty
                Else
                    c1 = cpRow.Cells.Item(1).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(2).Value) = True Then
                    c2 = String.Empty
                Else
                    c2 = cpRow.Cells.Item(2).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(3).Value) = True Then
                    c3 = String.Empty
                Else
                    c3 = cpRow.Cells.Item(3).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(4).Value) = True Then
                    c4 = String.Empty
                Else
                    c4 = cpRow.Cells.Item(4).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(5).Value) = True Then
                    c5 = String.Empty
                Else
                    c5 = cpRow.Cells.Item(5).Value.ToString
                End If

                exportStream.WriteLine(c1 & "," & c2 & "," & c3 & "," & c4 & "," & c5)
            Next
            exportStream.Close()
        End If
    End Sub

    Private Sub calcGeoRefButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calcGeoRefButton.Click
        For i As Integer = 1 To numDatasetsInteger
            Dim numTiePoints As Integer = 0
            Dim controlIDs(0) As String
            Dim controlXs(0) As Decimal
            Dim controlYs(0) As Decimal
            Dim controlZs(0) As Decimal
            Dim targetNums(0) As String
            Dim targetXs(0) As Decimal
            Dim targetYs(0) As Decimal
            Dim targetZs(0) As Decimal
            For Each row As DataGridViewRow In geoRefDataGridView.Rows
                If IsNothing(row.Cells(2).Value) = False Then
                    Dim datasetNum As Integer = Integer.Parse(row.Cells(2).Value.ToString.Substring(0, 1))  'gets the dataset number infront of the dataset name
                    If datasetNum = i Then  'dataset is the current one being tested
                        If IsNothing(row.Cells(3).Value) = False And IsNothing(row.Cells(4).Value) = False Then     'check to make sure control point and target point have been specified
                            For Each cpRow As DataGridViewRow In controlPtsDataGridView.Rows
                                If cpRow.Cells(0).Value.ToString = row.Cells(4).Value.ToString Then
                                    Try
                                        Dim cpX As Decimal = Decimal.Parse(cpRow.Cells(2).Value.ToString)
                                        Dim cpY As Decimal = Decimal.Parse(cpRow.Cells(3).Value.ToString)
                                        Dim cpZ As Decimal = Decimal.Parse(cpRow.Cells(4).Value.ToString)

                                        controlIDs(controlIDs.Length - 1) = row.Cells(4).Value.ToString
                                        controlXs(controlXs.Length - 1) = cpX
                                        controlYs(controlYs.Length - 1) = cpY
                                        controlZs(controlZs.Length - 1) = cpZ
                                        ReDim Preserve controlIDs(controlIDs.Length)
                                        ReDim Preserve controlXs(controlXs.Length)
                                        ReDim Preserve controlYs(controlYs.Length)
                                        ReDim Preserve controlZs(controlZs.Length)

                                        If row.Cells(0).Value = True Then
                                            numTiePoints += 1

                                            For Each trRow As DataGridViewRow In targetsDataGridView.Rows
                                                If trRow.Cells(0).Value.ToString = row.Cells(3).Value.ToString Then
                                                    targetNums(targetNums.Length - 1) = row.Cells(3).Value.ToString
                                                    targetXs(targetXs.Length - 1) = trRow.Cells(1).Value.ToString
                                                    targetYs(targetYs.Length - 1) = trRow.Cells(2).Value.ToString
                                                    targetZs(targetZs.Length - 1) = trRow.Cells(3).Value.ToString
                                                    ReDim Preserve targetNums(targetNums.Length)
                                                    ReDim Preserve targetXs(targetXs.Length)
                                                    ReDim Preserve targetYs(targetYs.Length)
                                                    ReDim Preserve targetZs(targetZs.Length)
                                                    Exit For
                                                End If
                                            Next

                                        End If
                                        Exit For
                                    Catch ex As Exception
                                        MessageBox.Show("Invalid Coordinates for Control Point ID: " & row.Cells(4).Value.ToString & ControlChars.NewLine & "This Tie Point will be ignored.")
                                        'catches if the control point X,Y,Z values are not able to convert to decimal values
                                        Exit For
                                    End Try
                                End If
                            Next
                        End If
                    End If
                End If
            Next

            If numTiePoints >= 3 Then       'solve least-squares solution to the transformation parameters

                'Xo(Xrot,Yrot,Zrot,S)
                Dim Xo, d, TP As New Matrix(7, 1)
                Dim A As New Matrix(numTiePoints * 3, 7)
                Dim w As New Matrix(numTiePoints * 3, 1)
                Dim L As New Matrix(numTiePoints * 3, 1)
                Dim xyz As New Matrix(numTiePoints * 3, 1)
                Dim okToProcess As Boolean = True

                For k As Integer = 0 To numTiePoints - 1
                    L.data(k * 3 + 1, 1) = controlXs(k)
                    L.data(k * 3 + 2, 1) = controlYs(k)
                    L.data(k * 3 + 3, 1) = controlZs(k)

                    xyz.data(k * 3 + 1, 1) = targetXs(k)
                    xyz.data(k * 3 + 2, 1) = targetYs(k)
                    xyz.data(k * 3 + 3, 1) = targetZs(k)
                Next

                'compute initial approximates
                Dim C1, C2, C3, t1, t2, t3 As New Matrix(3, 1)

                C1.data(1, 1) = L.data(1, 1)
                C1.data(2, 1) = L.data(2, 1)
                C1.data(3, 1) = L.data(3, 1)

                C2.data(1, 1) = L.data(4, 1)
                C2.data(2, 1) = L.data(5, 1)
                C2.data(3, 1) = L.data(6, 1)

                C3.data(1, 1) = L.data(7, 1)
                C3.data(2, 1) = L.data(8, 1)
                C3.data(3, 1) = L.data(9, 1)

                t1.data(1, 1) = xyz.data(1, 1)
                t1.data(2, 1) = xyz.data(2, 1)
                t1.data(3, 1) = xyz.data(3, 1)

                t2.data(1, 1) = xyz.data(4, 1)
                t2.data(2, 1) = xyz.data(5, 1)
                t2.data(3, 1) = xyz.data(6, 1)

                t3.data(1, 1) = xyz.data(7, 1)
                t3.data(2, 1) = xyz.data(8, 1)
                t3.data(3, 1) = xyz.data(9, 1)

                'check for determining if any of the control points or raw points within the tie points are identical 
                Dim length1 As Decimal = Matrix.abs(C1 - C2)
                Dim length2 As Decimal = Matrix.abs(C1 - C3)
                Dim length3 As Decimal = Matrix.abs(C2 - C3)
                Dim length4 As Decimal = Matrix.abs(t1 - t2)
                Dim length5 As Decimal = Matrix.abs(t1 - t3)
                Dim length6 As Decimal = Matrix.abs(t2 - t3)

                If length1 <> 0 AndAlso length2 <> 0 AndAlso length3 <> 0 AndAlso length4 <> 0 AndAlso length5 <> 0 AndAlso length6 <> 0 Then
                    'solve initial approximates for the transformation parameters
                    Dim scale1, scale2, scale3 As Decimal
                    Try
                        scale1 = Math.Sqrt((C1.data(1, 1) - C2.data(1, 1)) ^ 2 + (C1.data(2, 1) - C2.data(2, 1)) ^ 2 + (C1.data(3, 1) - C2.data(3, 1)) ^ 2) / Math.Sqrt((t1.data(1, 1) - t2.data(1, 1)) ^ 2 + (t1.data(2, 1) - t2.data(2, 1)) ^ 2 + (t1.data(3, 1) - t2.data(3, 1)) ^ 2)
                    Catch ex As Exception
                        scale1 = 1D
                    End Try

                    Try
                        scale2 = Math.Sqrt((C1.data(1, 1) - C3.data(1, 1)) ^ 2 + (C1.data(2, 1) - C3.data(2, 1)) ^ 2 + (C1.data(3, 1) - C3.data(3, 1)) ^ 2) / Math.Sqrt((t1.data(1, 1) - t3.data(1, 1)) ^ 2 + (t1.data(2, 1) - t3.data(2, 1)) ^ 2 + (t1.data(3, 1) - t3.data(3, 1)) ^ 2)
                    Catch ex As Exception
                        scale2 = 1
                    End Try

                    Try
                        scale3 = Math.Sqrt((C2.data(1, 1) - C3.data(1, 1)) ^ 2 + (C2.data(2, 1) - C3.data(2, 1)) ^ 2 + (C2.data(3, 1) - C3.data(3, 1)) ^ 2) / Math.Sqrt((t2.data(1, 1) - t3.data(1, 1)) ^ 2 + (t2.data(2, 1) - t3.data(2, 1)) ^ 2 + (t2.data(3, 1) - t3.data(3, 1)) ^ 2)
                    Catch ex As Exception
                        scale3 = 1
                    End Try

                    Dim scale As Decimal = (scale1 + scale2 + scale3) / 3

                    Dim v1c As Matrix = C2 - C1
                    Dim v2c As Matrix = C3 - C1
                    Dim nc As Matrix = Matrix.cross(v1c, v2c)
                    Dim tiltc As Decimal = Math.Atan(nc.data(3, 1) / (Math.Sqrt(nc.data(1, 1) ^ 2 + nc.data(2, 1) ^ 2))) + Math.PI / 2
                    Dim azimuthc As Decimal = Math.Atan2(nc.data(1, 1), nc.data(2, 1))
                    Dim swingc As Decimal = 0D

                    Dim v1t As Matrix = t2 - t1
                    Dim v2t As Matrix = t3 - t1
                    Dim nt As Matrix = Matrix.cross(v1t, v2t)
                    Dim tiltt As Decimal = Math.Atan(nt.data(3, 1) / (Math.Sqrt(nt.data(1, 1) ^ 2 + nt.data(2, 1) ^ 2))) + Math.PI / 2
                    Dim azimutht As Decimal = Math.Atan2(nt.data(1, 1), nt.data(2, 1))
                    Dim swingt As Decimal = 0D

                    Dim ATS_Rc, ATS_Rt As New Matrix(3, 3)

                    ATS_Rc.data(1, 1) = -Math.Cos(azimuthc) * Math.Cos(swingc) - Math.Sin(azimuthc) * Math.Cos(tiltc) * Math.Sin(swingc)
                    ATS_Rc.data(1, 2) = Math.Sin(azimuthc) * Math.Cos(swingc) - Math.Cos(azimuthc) * Math.Cos(tiltc) * Math.Sin(swingc)
                    ATS_Rc.data(1, 3) = -Math.Sin(tiltc) * Math.Sin(swingc)
                    ATS_Rc.data(2, 1) = Math.Cos(azimuthc) * Math.Sin(swingc) - Math.Sin(azimuthc) * Math.Cos(tiltc) * Math.Cos(swingc)
                    ATS_Rc.data(2, 2) = -Math.Sin(azimuthc) * Math.Sin(swingc) - Math.Cos(azimuthc) * Math.Cos(tiltc) * Math.Cos(swingc)
                    ATS_Rc.data(2, 3) = -Math.Sin(tiltc) * Math.Cos(swingc)
                    ATS_Rc.data(3, 1) = -Math.Sin(azimuthc) * Math.Sin(tiltc)
                    ATS_Rc.data(3, 2) = -Math.Cos(azimuthc) * Math.Sin(tiltc)
                    ATS_Rc.data(3, 3) = Math.Cos(tiltc)

                    ATS_Rt.data(1, 1) = -Math.Cos(azimutht) * Math.Cos(swingt) - Math.Sin(azimutht) * Math.Cos(tiltt) * Math.Sin(swingt)
                    ATS_Rt.data(1, 2) = Math.Sin(azimutht) * Math.Cos(swingt) - Math.Cos(azimutht) * Math.Cos(tiltt) * Math.Sin(swingt)
                    ATS_Rt.data(1, 3) = -Math.Sin(tiltt) * Math.Sin(swingt)
                    ATS_Rt.data(2, 1) = Math.Cos(azimutht) * Math.Sin(swingt) - Math.Sin(azimutht) * Math.Cos(tiltt) * Math.Cos(swingt)
                    ATS_Rt.data(2, 2) = -Math.Sin(azimutht) * Math.Sin(swingt) - Math.Cos(azimutht) * Math.Cos(tiltt) * Math.Cos(swingt)
                    ATS_Rt.data(2, 3) = -Math.Sin(tiltt) * Math.Cos(swingt)
                    ATS_Rt.data(3, 1) = -Math.Sin(azimutht) * Math.Sin(tiltt)
                    ATS_Rt.data(3, 2) = -Math.Cos(azimutht) * Math.Sin(tiltt)
                    ATS_Rt.data(3, 3) = Math.Cos(tiltt)

                    Dim newC1 As Matrix = ATS_Rc * C2
                    Dim newC2 As Matrix = ATS_Rc * C3
                    Dim newt1 As Matrix = ATS_Rt * t2
                    Dim newt2 As Matrix = ATS_Rt * t3

                    Dim azi_c As Decimal = Math.Atan2(newC2.data(1, 1) - newC1.data(1, 1), newC2.data(2, 1) - newC1.data(2, 1)) * 180D / Math.PI
                    Dim azi_t As Decimal = Math.Atan2(newt2.data(1, 1) - newt1.data(1, 1), newt2.data(2, 1) - newt1.data(2, 1)) * 180D / Math.PI

                    swingt = (azi_c - azi_t) * Math.PI / 180D

                    ATS_Rt.data(1, 1) = -Math.Cos(azimutht) * Math.Cos(swingt) - Math.Sin(azimutht) * Math.Cos(tiltt) * Math.Sin(swingt)
                    ATS_Rt.data(1, 2) = Math.Sin(azimutht) * Math.Cos(swingt) - Math.Cos(azimutht) * Math.Cos(tiltt) * Math.Sin(swingt)
                    ATS_Rt.data(1, 3) = -Math.Sin(tiltt) * Math.Sin(swingt)
                    ATS_Rt.data(2, 1) = Math.Cos(azimutht) * Math.Sin(swingt) - Math.Sin(azimutht) * Math.Cos(tiltt) * Math.Cos(swingt)
                    ATS_Rt.data(2, 2) = -Math.Sin(azimutht) * Math.Sin(swingt) - Math.Cos(azimutht) * Math.Cos(tiltt) * Math.Cos(swingt)
                    ATS_Rt.data(2, 3) = -Math.Sin(tiltt) * Math.Cos(swingt)
                    ATS_Rt.data(3, 1) = -Math.Sin(azimutht) * Math.Sin(tiltt)
                    ATS_Rt.data(3, 2) = -Math.Cos(azimutht) * Math.Sin(tiltt)
                    ATS_Rt.data(3, 3) = Math.Cos(tiltt)

                    Dim R As Matrix = ATS_Rt.Transpose * ATS_Rc

                    Dim Xrot As Decimal = Math.Atan2(-R.data(3, 2), R.data(3, 3)) * 180D / Math.PI
                    Dim Yrot As Decimal = Math.Asin(R.data(3, 1)) * 180D / Math.PI
                    Dim Zrot As Decimal = Math.Atan2(-R.data(2, 1), R.data(1, 1)) * 180D / Math.PI

                    Dim trans As Matrix = C1 - scale * R.Transpose * t1

                    Xo.data(1, 1) = scale
                    Xo.data(2, 1) = Xrot * Math.PI / 180D
                    Xo.data(3, 1) = Yrot * Math.PI / 180D
                    Xo.data(4, 1) = Zrot * Math.PI / 180D
                    Xo.data(5, 1) = trans.data(1, 1)
                    Xo.data(6, 1) = trans.data(2, 1)
                    Xo.data(7, 1) = trans.data(3, 1)

                    'start least squares iterations
                    Dim continueLS As Boolean = True
                    Dim loopCounter As Integer = 0
                    While continueLS
                        loopCounter += 1
                        '*****************************************************************
                        Dim S As Decimal = Xo.data(1, 1)
                        Dim O1 As Decimal = Xo.data(2, 1)
                        Dim O2 As Decimal = Xo.data(3, 1)
                        Dim O3 As Decimal = Xo.data(4, 1)

                        Dim r11 As Decimal = Math.Cos(O2) * Math.Cos(O3)
                        Dim r12 As Decimal = Math.Sin(O1) * Math.Sin(O2) * Math.Cos(O3) + Math.Cos(O1) * Math.Sin(O3)
                        Dim r13 As Decimal = Math.Sin(O1) * Math.Sin(O3) - Math.Cos(O1) * Math.Sin(O2) * Math.Cos(O3)
                        Dim r21 As Decimal = (-1) * Math.Cos(O2) * Math.Sin(O3)
                        Dim r22 As Decimal = Math.Cos(O1) * Math.Cos(O3) - Math.Sin(O1) * Math.Sin(O2) * Math.Sin(O3)
                        Dim r23 As Decimal = Math.Cos(O1) * Math.Sin(O2) * Math.Sin(O3) + Math.Sin(O1) * Math.Cos(O3)
                        Dim r31 As Decimal = Math.Sin(O2)
                        Dim r32 As Decimal = -1 * Math.Sin(O1) * Math.Cos(O2)
                        Dim r33 As Decimal = Math.Cos(O1) * Math.Cos(O2)

                        For j As Integer = 0 To numTiePoints - 1
                            Dim x As Decimal = xyz.data(j * 3 + 1, 1)
                            Dim y As Decimal = xyz.data(j * 3 + 2, 1)
                            Dim z As Decimal = xyz.data(j * 3 + 3, 1)

                            'Partial Derivatives of the 3D rotation matrix and Linear Translations (Holeshot method)
                            A.data(j * 3 + 1, 1) = r11 * x + r21 * y + r31 * z
                            A.data(j * 3 + 1, 2) = 0
                            A.data(j * 3 + 1, 3) = S * (y * Math.Sin(O2) * Math.Sin(O3) + z * Math.Cos(O2) - x * Math.Sin(O2) * Math.Cos(O3))
                            A.data(j * 3 + 1, 4) = S * (r21 * x - r11 * y)
                            A.data(j * 3 + 1, 5) = 1
                            A.data(j * 3 + 1, 6) = 0
                            A.data(j * 3 + 1, 7) = 0

                            A.data(j * 3 + 2, 1) = r12 * x + r22 * y + r32 * z
                            A.data(j * 3 + 2, 2) = -1 * S * (r13 * x + r23 * y + r33 * z)
                            A.data(j * 3 + 2, 3) = S * (x * Math.Sin(O1) * Math.Cos(O2) * Math.Cos(O3) - y * Math.Sin(O1) * Math.Cos(O2) * Math.Sin(O3) + z * Math.Sin(O1) * Math.Sin(O2))
                            A.data(j * 3 + 2, 4) = S * (r22 * x - r12 * y)
                            A.data(j * 3 + 2, 5) = 0
                            A.data(j * 3 + 2, 6) = 1
                            A.data(j * 3 + 2, 7) = 0

                            A.data(j * 3 + 3, 1) = r13 * x + r23 * y + r33 * z
                            A.data(j * 3 + 3, 2) = S * (r12 * x + r22 * y + r32 * z)
                            A.data(j * 3 + 3, 3) = S * (y * Math.Cos(O1) * Math.Cos(O2) * Math.Sin(O3) - z * Math.Cos(O1) * Math.Sin(O2) - x * Math.Cos(O1) * Math.Cos(O2) * Math.Cos(O3))
                            A.data(j * 3 + 3, 4) = S * (r23 * x - r13 * y)
                            A.data(j * 3 + 3, 5) = 0
                            A.data(j * 3 + 3, 6) = 0
                            A.data(j * 3 + 3, 7) = 1
                        Next

                        For j As Integer = 0 To numTiePoints - 1
                            Dim x As Decimal = xyz.data(j * 3 + 1, 1)
                            Dim y As Decimal = xyz.data(j * 3 + 2, 1)
                            Dim z As Decimal = xyz.data(j * 3 + 3, 1)

                            w.data(j * 3 + 1, 1) = S * (r11 * x + r21 * y + r31 * z) + Xo.data(5, 1) - L.data(j * 3 + 1, 1)
                            w.data(j * 3 + 2, 1) = S * (r12 * x + r22 * y + r32 * z) + Xo.data(6, 1) - L.data(j * 3 + 2, 1)
                            w.data(j * 3 + 3, 1) = S * (r13 * x + r23 * y + r33 * z) + Xo.data(7, 1) - L.data(j * 3 + 3, 1)
                        Next

                        d = -1 * (A.Transpose * A).Inverse * (A.Transpose * w)
                        TP = Xo + d
                        '*******************************************************************
                        If Math.Abs(d.data(1, 1)) <= 0.00000001 And Math.Abs(d.data(2, 1)) <= 0.00000001 And Math.Abs(d.data(3, 1)) <= 0.00000001 And Math.Abs(d.data(4, 1)) <= 0.00000001 And Math.Abs(d.data(5, 1)) <= 0.00000001 And Math.Abs(d.data(6, 1)) <= 0.00000001 And Math.Abs(d.data(7, 1)) <= 0.00000001 Then
                            continueLS = False
                        Else
                            Xo.equals(TP)
                        End If
                        If loopCounter >= 6 Then
                            MessageBox.Show("The solution to the transformation parameters for Dataset #" & i.ToString & "did not converge in 6 iterations and as a result the processing has stopped. Check the tie point information for accuracy.", "ITERATION LIMITS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit While
                        End If
                    End While

                    If loopCounter < 10 Then
                        Dim changeTransValues As Boolean = True
                        If i = 1 Then
                            If transParameters.dataset1ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(1, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 1 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset1ApplyButton.Enabled = True
                            End If
                        ElseIf i = 2 Then
                            If transParameters.dataset2ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(2, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 2 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset2ApplyButton.Enabled = True
                            End If
                        ElseIf i = 3 Then
                            If transParameters.dataset3ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(3, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 3 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset3ApplyButton.Enabled = True
                            End If
                        ElseIf i = 4 Then
                            If transParameters.dataset4ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(4, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 4 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset4ApplyButton.Enabled = True
                            End If
                        ElseIf i = 5 Then
                            If transParameters.dataset5ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(5, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 5 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset5ApplyButton.Enabled = True
                            End If
                        ElseIf i = 6 Then
                            If transParameters.dataset6ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(6, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 6 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset6ApplyButton.Enabled = True
                            End If
                        ElseIf i = 7 Then
                            If transParameters.dataset7ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(7, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 7 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset7ApplyButton.Enabled = True
                            End If
                        ElseIf i = 8 Then
                            If transParameters.dataset8ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(8, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 8 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset8ApplyButton.Enabled = True
                            End If
                        ElseIf i = 9 Then
                            If transParameters.dataset9ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(9, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 9 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset9ApplyButton.Enabled = True
                            End If
                        ElseIf i = 10 Then
                            If transParameters.dataset10ApplyButton.Text = "Unapply" Then
                                If check4TransParametersUpdate(10, TP) Then
                                    changeTransValues = False
                                    MessageBox.Show("You must unapply the coordinate transformation to dataset 10 before updated transformation values can be solved", "RGBi Laser Scanning", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            Else
                                transParameters.dataset10ApplyButton.Enabled = True
                            End If
                        End If

                        If changeTransValues Then
                            transParameters.transParametersDataGridView.Rows(i - 1).Cells(1).Value = Math.Round((TP.data(1, 1)), 6)
                            transParameters.transParametersDataGridView.Rows(i - 1).Cells(2).Value = DecToDMS(TP.data(2, 1) * 180D / Math.PI)
                            transParameters.transParametersDataGridView.Rows(i - 1).Cells(3).Value = DecToDMS(TP.data(3, 1) * 180D / Math.PI)
                            transParameters.transParametersDataGridView.Rows(i - 1).Cells(4).Value = DecToDMS(TP.data(4, 1) * 180D / Math.PI)
                            transParameters.transParametersDataGridView.Rows(i - 1).Cells(5).Value = Math.Round(TP.data(5, 1), 3).ToString & "m"
                            transParameters.transParametersDataGridView.Rows(i - 1).Cells(6).Value = Math.Round(TP.data(6, 1), 3).ToString & "m"
                            transParameters.transParametersDataGridView.Rows(i - 1).Cells(7).Value = Math.Round(TP.data(7, 1), 3).ToString & "m"
                            transformParameters(i - 1, 0) = TP.data(1, 1)
                            transformParameters(i - 1, 1) = TP.data(2, 1)
                            transformParameters(i - 1, 2) = TP.data(3, 1)
                            transformParameters(i - 1, 3) = TP.data(4, 1)
                            transformParameters(i - 1, 4) = TP.data(5, 1)
                            transformParameters(i - 1, 5) = TP.data(6, 1)
                            transformParameters(i - 1, 6) = TP.data(7, 1)

                            Dim apost As Decimal = ((A * d + w).Transpose * (A * d + w)).toScalar / (3 * numTiePoints - 7)
                            Dim Ctp As Matrix = apost * (A.Transpose * A).Inverse
                            Dim stddev As Matrix = Ctp.getDiagonal.Sqrt

                            transformParametersStdDev(i - 1, 0) = stddev.data(1, 1)
                            transformParametersStdDev(i - 1, 1) = stddev.data(2, 1)
                            transformParametersStdDev(i - 1, 2) = stddev.data(3, 1)
                            transformParametersStdDev(i - 1, 3) = stddev.data(4, 1)
                            transformParametersStdDev(i - 1, 4) = stddev.data(5, 1)
                            transformParametersStdDev(i - 1, 5) = stddev.data(6, 1)
                            transformParametersStdDev(i - 1, 6) = stddev.data(7, 1)
                        End If
                    End If
                Else
                    resetDataset(i)
                End If
            Else
                resetDataset(i)
            End If
        Next
        transParameters.updateMade = False
        transParameters.ShowDialog()
        If transParameters.updateMade = True Then
            Me.Refresh()
            updateCurrentDataset(e)
            drawMapButton_Click(refreshMapButton, e)
        End If
    End Sub

    Private Sub resetDataset(ByVal datasetNum As Integer)
        Dim i As Integer = datasetNum
        Dim ok2Clear As Boolean = False
        If i = 1 Then
            If transParameters.dataset1ApplyButton.Text = "Apply" Then
                transParameters.dataset1ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 2 Then
            If transParameters.dataset2ApplyButton.Text = "Apply" Then
                transParameters.dataset2ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 3 Then
            If transParameters.dataset3ApplyButton.Text = "Apply" Then
                transParameters.dataset3ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 4 Then
            If transParameters.dataset4ApplyButton.Text = "Apply" Then
                transParameters.dataset4ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 5 Then
            If transParameters.dataset5ApplyButton.Text = "Apply" Then
                transParameters.dataset5ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 6 Then
            If transParameters.dataset6ApplyButton.Text = "Apply" Then
                transParameters.dataset6ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 7 Then
            If transParameters.dataset7ApplyButton.Text = "Apply" Then
                transParameters.dataset7ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 8 Then
            If transParameters.dataset8ApplyButton.Text = "Apply" Then
                transParameters.dataset8ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 9 Then
            If transParameters.dataset9ApplyButton.Text = "Apply" Then
                transParameters.dataset9ApplyButton.Enabled = False
                ok2Clear = True
            End If
        ElseIf i = 10 Then
            If transParameters.dataset10ApplyButton.Text = "Apply" Then
                transParameters.dataset10ApplyButton.Enabled = False
                ok2Clear = True
            End If
        End If

        If ok2Clear Then
            transParameters.transParametersDataGridView.Rows(i - 1).Cells(1).Value = ""
            transParameters.transParametersDataGridView.Rows(i - 1).Cells(2).Value = ""
            transParameters.transParametersDataGridView.Rows(i - 1).Cells(3).Value = ""
            transParameters.transParametersDataGridView.Rows(i - 1).Cells(4).Value = ""
            transParameters.transParametersDataGridView.Rows(i - 1).Cells(5).Value = ""
            transParameters.transParametersDataGridView.Rows(i - 1).Cells(6).Value = ""
            transParameters.transParametersDataGridView.Rows(i - 1).Cells(7).Value = ""

            transformParameters(i - 1, 0) = 0
            transformParameters(i - 1, 1) = 0
            transformParameters(i - 1, 2) = 0
            transformParameters(i - 1, 3) = 0
            transformParameters(i - 1, 4) = 0
            transformParameters(i - 1, 5) = 0
            transformParameters(i - 1, 6) = 0

            transformParametersStdDev(i - 1, 0) = 0
            transformParametersStdDev(i - 1, 1) = 0
            transformParametersStdDev(i - 1, 2) = 0
            transformParametersStdDev(i - 1, 3) = 0
            transformParametersStdDev(i - 1, 4) = 0
            transformParametersStdDev(i - 1, 5) = 0
            transformParametersStdDev(i - 1, 6) = 0
        End If
    End Sub


    Friend Function DecToDMS(ByVal arg1 As Decimal) As String
        Dim var1, var2, var3, var4, var5, var6, arg2 As Decimal

        If arg1 < 0 Then
            arg2 = Math.Abs(arg1)
        Else
            arg2 = arg1
        End If

        var1 = Decimal.Truncate(arg2)
        var2 = arg2 - var1
        var3 = var2 * 60D
        var4 = Decimal.Truncate(var3)
        var5 = var3 - var4
        var6 = var5 * 60D

        If arg1 < 0 Then
            var1 *= -1
        End If

        DecToDMS = var1.ToString & Chr(176) & " " & var4.ToString("00") & "' " & Decimal.Round(var6, 1).ToString("00.0") & Chr(34)
    End Function

    Private Function check4TransParametersUpdate(ByVal dataset As Integer, ByVal X As Matrix) As Boolean
        If X.data(1, 1) = transformParameters(dataset - 1, 0) AndAlso X.data(2, 1) = transformParameters(dataset - 1, 1) AndAlso X.data(3, 1) = transformParameters(dataset - 1, 2) AndAlso X.data(4, 1) = transformParameters(dataset - 1, 3) AndAlso X.data(5, 1) = transformParameters(dataset - 1, 4) AndAlso X.data(6, 1) = transformParameters(dataset - 1, 5) AndAlso X.data(7, 1) = transformParameters(dataset - 1, 6) Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub RegistrationGeoreferencingOptionsToolStripMenuItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RegistrationGeoreferencingOptionsToolStripMenuItem.CheckedChanged
        geoRefGroupBox.Visible = RegistrationGeoreferencingOptionsToolStripMenuItem.Checked
    End Sub

    Private Sub RegistrationTargetProcessingOptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TargetProcessingOptionsToolStripMenuItem.Click
        targetProcessingOptions.ShowDialog()
    End Sub

    Private Sub pickTargetPtButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pickTargetPtButton.Click
        If mapDisplayed Then
            statusLabel.Text = "Click a dataset point on the screen to manually add a registration target point, press ESC to quit"
            insidePickRegTarget = True
        Else
            MessageBox.Show("You cannot pick a dataset point on an empty display", "NO DATA PROBLEM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub importTargetsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles importTargetsButton.Click
        Dim importDialog As DialogResult = importCSVFileDialog.ShowDialog
        If importDialog = Windows.Forms.DialogResult.OK Then
            Dim importStream As New StreamReader(importCSVFileDialog.FileName)
            Dim delims(0) As Char
            delims(0) = ","
            While importStream.Peek <> -1
                Dim lineRead As String = importStream.ReadLine
                Dim lineParts() As String = lineRead.Split(delims)
                Dim newRadius As Decimal
                Dim radiusString As String
                Dim desc As String

                Try
                    Dim newTarget As New Matrix(3, 1)
                    newTarget.data(1, 1) = Decimal.Parse(lineParts(1))
                    newTarget.data(2, 1) = Decimal.Parse(lineParts(2))
                    newTarget.data(3, 1) = Decimal.Parse(lineParts(3))
                    desc = "Imported Point: " & lineParts(0)
                    Try
                        newRadius = Decimal.Parse(lineParts(4))
                    Catch ex As Exception
                        newRadius = manualTargetRadius
                    End Try
                    radiusString = newRadius.ToString("f4")

                    'compute points on boundary edge of target circle as computed by the LSA
                    Dim targetCircle As New cloudCircle
                    targetCircle.edgePoints = New Generic.List(Of cloudPoint)

                    For i As Integer = 0 To 360 Step 8
                        'XZ Plane Circle
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = Math.Sin(i * Math.PI / 180D) * newRadius + newTarget.data(1, 1)
                        targetPoint.Y = newTarget.data(2, 1)
                        targetPoint.Z = Math.Cos(i * Math.PI / 180D) * newRadius + newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    For i As Integer = 0 To 450 Step 8
                        'YZ Plane Circle (360 + 90 to get the ending point in the correct spot to start the last XY plane circle)
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = newTarget.data(1, 1)
                        targetPoint.Y = Math.Sin(i * Math.PI / 180D) * newRadius + newTarget.data(2, 1)
                        targetPoint.Z = Math.Cos(i * Math.PI / 180D) * newRadius + newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    For i As Integer = 0 To 360 Step 8
                        'XY Plane Circle
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = Math.Sin(i * Math.PI / 180D) * newRadius + newTarget.data(1, 1)
                        targetPoint.Y = Math.Cos(i * Math.PI / 180D) * newRadius + newTarget.data(2, 1)
                        targetPoint.Z = newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    For i As Integer = 90 To 0 Step -8
                        'YZ Plane Circle (-90 to get the ending point in the correct spot to close the target image)
                        Dim targetPoint As New cloudPoint
                        targetPoint.X = newTarget.data(1, 1)
                        targetPoint.Y = Math.Sin(i * Math.PI / 180D) * newRadius + newTarget.data(2, 1)
                        targetPoint.Z = Math.Cos(i * Math.PI / 180D) * newRadius + newTarget.data(3, 1)
                        targetCircle.edgePoints.Add(targetPoint)
                    Next

                    targetCircle.centrePoint.X = newTarget.data(1, 1)
                    targetCircle.centrePoint.Y = newTarget.data(2, 1)
                    targetCircle.centrePoint.Z = newTarget.data(3, 1)

                    targetCircle.colour = processedTargetsColorLabel.BackColor  'this setting is not actually used as when the target circles are rendered the colour is as user defined in the targetColourLabel 
                    targetCircle.weight = targetsLineWeight 'this setting is not actually used as when the target circles are rendered the line weight is user defined (right-click targetColourLabel)

                    targetCircles.Add(targetCircle)
                    targetsFound = True

                    renderView2(currentMapDataPoints)
                    mapDisplayed = True
                    mapPanel.BackgroundImage = mapBitmap
                    mapPanel.Refresh()

                    maxTargetPoints += 1
                    Dim targetData() As String = {maxTargetPoints.ToString, newTarget.data(1, 1).ToString("f4"), newTarget.data(2, 1).ToString("f4"), newTarget.data(3, 1).ToString("f4"), radiusString, desc}
                    targetsDataGridView.Rows.Add(targetData)
                    GRcolumn4.Items.Add(maxTargetPoints.ToString)
                Catch ex As Exception
                End Try
            End While

            importStream.Close()
        End If
    End Sub

    Private Sub copyRegTargetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles copyRegTargetButton.Click
        If targetsDataGridView.SelectedCells.Count = 1 Then
            Dim regPtNum As Integer
            Dim X, Y, Z As Decimal
            X = Decimal.Parse(targetsDataGridView.Item(1, targetsDataGridView.SelectedCells.Item(0).RowIndex).Value.ToString)
            Y = Decimal.Parse(targetsDataGridView.Item(2, targetsDataGridView.SelectedCells.Item(0).RowIndex).Value.ToString)
            Z = Decimal.Parse(targetsDataGridView.Item(3, targetsDataGridView.SelectedCells.Item(0).RowIndex).Value.ToString)
            regPtNum = Integer.Parse(targetsDataGridView.Item(0, targetsDataGridView.SelectedCells.Item(0).RowIndex).Value.ToString)
            'MessageBox.Show(X.ToString & " " & Y.ToString & " " & Z.ToString)
            maxControlPoints += 1
            Dim newRow(5) As Object
            newRow(0) = maxControlPoints
            newRow(1) = regPtNum
            newRow(2) = X
            newRow(3) = Y
            newRow(4) = Z
            newRow(5) = "Copied Reg. Target # " & regPtNum.ToString
            controlPtsDataGridView.Rows.Add(newRow)
            GRcolumn5.Items.Add(maxControlPoints.ToString)
        Else
            MessageBox.Show("You must select a single cell within a point's row in the Registration Targets List in order to copy that point to the Control Points List.", "NO POINT SELECTED", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub targetsDataGridView_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles targetsDataGridView.CellMouseClick
        If e.ColumnIndex >= 0 And e.RowIndex >= 0 Then
            targetsDataGridView.Item(e.ColumnIndex, e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub targetsDataGridView_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles targetsDataGridView.MouseClick
        targetsDataGridView.ClearSelection()
    End Sub

    Private Sub XYButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYButton.Click
        Dim currentStandOff As Decimal = Matrix.abs(camera - target)
        camera.data(1, 1) = target.data(1, 1)
        camera.data(2, 1) = target.data(2, 1)
        camera.data(3, 1) = target.data(3, 1) + currentStandOff
        drawMapButton_Click(refreshMapButton, e)
    End Sub

    Private Sub YZButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YZButton.Click
        Dim currentStandOff As Decimal = Matrix.abs(camera - target)
        camera.data(3, 1) = target.data(3, 1)
        camera.data(2, 1) = target.data(2, 1)
        camera.data(1, 1) = target.data(1, 1) + currentStandOff
        drawMapButton_Click(refreshMapButton, e)
    End Sub

    Private Sub XZButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XZButton.Click
        Dim currentStandOff As Decimal = Matrix.abs(camera - target)
        camera.data(1, 1) = target.data(1, 1)
        camera.data(3, 1) = target.data(3, 1)
        camera.data(2, 1) = target.data(2, 1) + currentStandOff
        drawMapButton_Click(refreshMapButton, e)
    End Sub

    Private Sub processSphericalTargetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles processSphericalTargetButton.Click
        If BackgroundWorker4.IsBusy <> True Then
            ' Start the asynchronous operation.
            Me.Cursor = Cursors.AppStarting
            If mapPanel.Cursor = Cursors.Cross Then
                currentMapCursor = Cursors.Cross
            Else
                currentMapCursor = Cursors.Hand
            End If
            mapPanel.Cursor = Cursors.AppStarting
            BackgroundWorker4.RunWorkerAsync()
        Else
            MessageBox.Show("The application is busy at the current moment so please wait until the previous process finishes", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub


    Private Sub BackgroundWorker4_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker4.DoWork
        Try
            Dim spherePoints As New Generic.List(Of cloudPoint)
            Dim selectedPoints As New Generic.List(Of cloudPoint)
            Dim targetHistogram(0) As Integer

            'determine selected points on screen
            For i As Integer = 0 To currentMapDataPoints.Count - 1
                If currentMapDataPoints(i).highlighted AndAlso Not currentMapDataPoints(i).deleted Then
                    selectedPoints.Add(currentMapDataPoints(i))
                End If
            Next

            If selectedPoints.Count > 4 Then
                updateStatus("Point cloud spherical target recognition has started...")
                System.Threading.Thread.Sleep(2000)

                If selectedPoints.Count > 100 Then
                    Dim pointsAdded(99) As Integer

                    updateStatus("Resampling selected points")
                    System.Threading.Thread.Sleep(2000)

                    'pick a random sample of 100 points to use to compute the sphere parameters
                    For i As Integer = 0 To 99
                        Dim keepSearching As Boolean = True
                        Do While keepSearching
                            Dim randNum As Integer = GetRandom(1, selectedPoints.Count)
                            Dim numExists As Boolean = False
                            For j As Integer = 0 To 99
                                If pointsAdded(j) = randNum Then
                                    numExists = True
                                    Exit For
                                End If
                            Next
                            If numExists = False Then
                                spherePoints.Add(selectedPoints(randNum))
                                keepSearching = False
                            End If
                        Loop
                    Next
                Else
                    For i As Integer = 0 To selectedPoints.Count - 1
                        spherePoints.Add(selectedPoints(i))
                    Next
                End If

                'least squares sphere fitting
                Dim messageresult As DialogResult = Windows.Forms.DialogResult.Yes
                'If spherePoints.Count > 200 Then
                '    messageresult = MessageBox.Show("You are about to process " & spherePoints.Count.ToString & " points within a sphere fitting least-squares adjustment, this may take a long time to process. Are you sure you want to continue?", "NUMBER CRUNCHER", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                'End If

                If messageresult = Windows.Forms.DialogResult.Yes Then

                    Dim A2 As New Matrix(spherePoints.Count, 4)
                    Dim Xo, D2, X2 As New Matrix(4, 1)
                    Dim BBtInv, V2 As New Matrix
                    Dim B As New Matrix(A2.nRows, (A2.nRows * 3I))
                    Dim w2 As New Matrix(A2.nRows, 1)
                    Dim Lobs, L As New Matrix(A2.nRows * 3, 1)

                    Dim p1, p2, p3, p4 As New Matrix(3, 1)
                    Dim validResult As Boolean

                    p1.data(1, 1) = spherePoints(0).X
                    p1.data(2, 1) = spherePoints(0).Y
                    p1.data(3, 1) = spherePoints(0).Z

                    p2.data(1, 1) = spherePoints(1).X
                    p2.data(2, 1) = spherePoints(1).Y
                    p2.data(3, 1) = spherePoints(1).Z

                    p3.data(1, 1) = spherePoints(2).X
                    p3.data(2, 1) = spherePoints(2).Y
                    p3.data(3, 1) = spherePoints(2).Z

                    p4.data(1, 1) = spherePoints(3).X
                    p4.data(2, 1) = spherePoints(3).Y
                    p4.data(3, 1) = spherePoints(3).Z

                    'solve initial approximates for the sphere using the first 4 random points from the selected points
                    Xo = solveInitialSphereApprox(p1, p2, p3, p4, validResult)

                    If validResult = False Then
                        Xo.data(1, 1) = 0   'X coordinate for the centre of the sphere
                        Xo.data(2, 1) = 0   'Y coordinate for the centre of the sphere
                        Xo.data(3, 1) = 0   'Z coordinate for the centre of the sphere
                        Xo.data(4, 1) = 1  'radius of the sphere
                    End If

                    For i As Integer = 0 To A2.nRows - 1
                        Lobs.data(3 * i + 1, 1) = spherePoints(i).X
                        Lobs.data(3 * i + 2, 1) = spherePoints(i).Y
                        Lobs.data(3 * i + 3, 1) = spherePoints(i).Z
                    Next

                    Dim iteration As Integer = 1
                    Dim stopYesNo As Boolean = False
                    Dim solutionFound As Boolean

                    'start iterative least squares
                    updateStatus("Least Squares analysis running (1)")

                    Dim displayMessage As Boolean = True
                    While Not stopYesNo
                        If BackgroundWorker4.CancellationPending = True Then
                            updateStatus("CANCELLED")
                            System.Threading.Thread.Sleep(2000)
                            updateStatus(String.Empty)
                            Me.Cursor = Cursors.Default
                            mapPanel.Cursor = currentMapCursor
                            solutionFound = False
                            displayMessage = False
                            Exit While
                        End If
                        For i As Integer = 1 To A2.nRows
                            A2.data(i, 1) = -2 * (spherePoints(i - 1).X - Xo.data(1, 1))
                            A2.data(i, 2) = -2 * (spherePoints(i - 1).Y - Xo.data(2, 1))
                            A2.data(i, 3) = -2 * (spherePoints(i - 1).Z - Xo.data(3, 1))
                            A2.data(i, 4) = -2 * Xo.data(4, 1)
                        Next

                        For i As Integer = 0 To B.nRows - 1
                            B.data(i + 1, 3 * i + 1) = 2 * (spherePoints(i).X - Xo.data(1, 1))
                            B.data(i + 1, 3 * i + 2) = 2 * (spherePoints(i).Y - Xo.data(2, 1))
                            B.data(i + 1, 3 * i + 3) = 2 * (spherePoints(i).Z - Xo.data(3, 1))
                        Next

                        For i As Integer = 1 To w2.nRows
                            w2.data(i, 1) = (spherePoints(i - 1).X - Xo.data(1, 1)) ^ 2 + (spherePoints(i - 1).Y - Xo.data(2, 1)) ^ 2 + (spherePoints(i - 1).Z - Xo.data(3, 1)) ^ 2 - Xo.data(4, 1) ^ 2
                        Next

                        'w2 = w2 + B * (Lobs - L)

                        BBtInv = (B * B.Transpose).Inverse

                        D2 = -1 * ((A2.Transpose * BBtInv * A2).Inverse * A2.Transpose * BBtInv * w2)
                        X2 = Xo + D2

                        V2 = -1 * (B.Transpose * BBtInv * (A2 * D2 + w2))
                        L = Lobs + V2

                        If Math.Abs(D2.data(1, 1)) <= 0.001 AndAlso Math.Abs(D2.data(2, 1)) <= 0.001 AndAlso Math.Abs(D2.data(3, 1)) <= 0.001 AndAlso Math.Abs(D2.data(4, 1)) <= 0.001 Then
                            stopYesNo = True
                            solutionFound = True
                        Else
                            iteration += 1
                            updateStatus("Least Squares analysis running (" & iteration.ToString & ")")
                            Xo.equals(X2)
                            If iteration = maxIterations Then
                                stopYesNo = True
                                solutionFound = False
                            End If
                        End If
                    End While

                    If solutionFound Then
                        targetRadius = Math.Abs(X2.data(4, 1))
                        updateStatus("Spherical Target found, computing statistics")
                        Dim Cx, Cv, StdDevX, StdDevV As New Matrix
                        Cx = ((V2.Transpose * V2).toScalar / (A2.nRows - 4)) * (A2.Transpose * BBtInv * A2).Inverse
                        StdDevX = Cx.getDiagonal.Sqrt
                        Cv = B.Transpose * BBtInv * B - B.Transpose * BBtInv * A2 * (A2.Transpose * BBtInv * A2).Inverse * A2.Transpose * BBtInv * B
                        StdDevV = Cv.getDiagonal.Sqrt

                        'compute points on boundary edge of target circle as computed by the LSA
                        Dim targetCircle As New cloudCircle
                        targetCircle.edgePoints = New Generic.List(Of cloudPoint)
                        Dim newTarget As New Matrix(3, 1)
                        newTarget.data(1, 1) = X2.data(1, 1)
                        newTarget.data(2, 1) = X2.data(2, 1)
                        newTarget.data(3, 1) = X2.data(3, 1)

                        For j As Integer = 0 To 180 Step 15
                            For i As Integer = 0 To 360 Step 5
                                'XZ Plane Circle
                                Dim targetPoint As New cloudPoint
                                Dim targetVector As New Matrix(3, 1)
                                targetVector.data(1, 1) = Math.Sin(i * Math.PI / 180D) * targetRadius + newTarget.data(1, 1)
                                targetVector.data(2, 1) = newTarget.data(2, 1)
                                targetVector.data(3, 1) = Math.Cos(i * Math.PI / 180D) * targetRadius + newTarget.data(3, 1)
                                targetVector = targetVector - newTarget
                                targetVector = Matrix.R3(j) * targetVector
                                targetVector = targetVector + newTarget
                                targetPoint.X = targetVector.data(1, 1)
                                targetPoint.Y = targetVector.data(2, 1)
                                targetPoint.Z = targetVector.data(3, 1)

                                targetCircle.edgePoints.Add(targetPoint)
                            Next
                        Next

                        For i As Integer = 0 To 360 Step 5
                            Dim targetPoint As New cloudPoint
                            Dim targetVector As New Matrix(3, 1)
                            targetVector.data(1, 1) = newTarget.data(1, 1)
                            targetVector.data(2, 1) = Math.Sin(i * Math.PI / 180D) * targetRadius + newTarget.data(2, 1)
                            targetVector.data(3, 1) = Math.Cos(i * Math.PI / 180D) * targetRadius + newTarget.data(3, 1)
                            If i Mod 15 = 0 And i <> 0 And i < 180 Then
                                Dim newRadius As Decimal = Math.Sin(i * Math.PI / 180D) * targetRadius
                                For j As Integer = 0 To 360 Step 5
                                    'XY Plane Circle
                                    Dim targetPoint2 As New cloudPoint
                                    Dim targetVector2 As New Matrix(3, 1)
                                    targetVector2.data(1, 1) = Math.Sin(j * Math.PI / 180D) * newRadius + newTarget.data(1, 1)
                                    targetVector2.data(2, 1) = Math.Cos(j * Math.PI / 180D) * newRadius + newTarget.data(2, 1)
                                    targetVector2.data(3, 1) = targetVector.data(3, 1)
                                    targetPoint2.X = targetVector2.data(1, 1)
                                    targetPoint2.Y = targetVector2.data(2, 1)
                                    targetPoint2.Z = targetVector2.data(3, 1)
                                    targetCircle.edgePoints.Add(targetPoint2)
                                Next
                            End If

                            targetPoint.X = targetVector.data(1, 1)
                            targetPoint.Y = targetVector.data(2, 1)
                            targetPoint.Z = targetVector.data(3, 1)
                            targetCircle.edgePoints.Add(targetPoint)
                        Next

                        targetCircle.centrePoint.X = newTarget.data(1, 1)
                        targetCircle.centrePoint.Y = newTarget.data(2, 1)
                        targetCircle.centrePoint.Z = newTarget.data(3, 1)

                        targetCircle.colour = processedTargetsColorLabel.BackColor  'this setting is not actually used as when the target circles are rendered the colour is as user defined in the targetColourLabel 
                        targetCircle.weight = targetsLineWeight 'this setting is not actually used as when the target circles are rendered the line weight is user defined (right-click targetColourLabel)

                        targetCircles.Add(targetCircle)
                        targetsFound = True
                        targetSolution.equals(newTarget)
                    Else
                        If displayMessage = True Then
                            MessageBox.Show("The spherical surface points for the spherical target are of low quality and a statistically reliable solution is not possible. If possible try increasing the point cloud density for the spherical target.", "LOW QUALITY POINTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            e.Cancel = True
                        Else
                            e.Cancel = True
                        End If
                    End If
                End If
            Else
                MessageBox.Show("Not enough surface surface points for the sperical target have been identified, no solution is possible (5 points minimum)", "NOT ENOUGH POINTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                e.Cancel = True
            End If
            'writefile.Close()
        Catch ex As Exception
            MessageBox.Show("An error occurred trying to process your selected spherical target, please try re-selecting your target within the point cloud and reprocessing", "SPHERICAL TARGET PROCESSING ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
        End Try
    End Sub

    Private Sub BackgroundWorker4_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker4.RunWorkerCompleted
        statusLabel.Text = String.Empty
        Me.Cursor = Cursors.Default
        mapPanel.Cursor = currentMapCursor

        renderView2(currentMapDataPoints)
        mapDisplayed = True
        mapPanel.BackgroundImage = mapBitmap
        mapPanel.Refresh()

        maxTargetPoints += 1
        Dim targetData() As String = {maxTargetPoints.ToString, targetSolution.data(1, 1).ToString("f4"), targetSolution.data(2, 1).ToString("f4"), targetSolution.data(3, 1).ToString("f4"), targetRadius.ToString("f4"), "Spherical"}
        targetsDataGridView.Rows.Add(targetData)
        GRcolumn4.Items.Add(maxTargetPoints.ToString)

    End Sub

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Private Function solveInitialSphereApprox(ByVal p1 As Matrix, ByVal p2 As Matrix, ByVal p3 As Matrix, ByVal p4 As Matrix, ByRef ValidResultFound As Boolean) As Matrix
        Dim solution As New Matrix(4, 1)
        Dim M11, M12, M13, M14, M15 As New Matrix(4, 4)
        M11.data(1, 1) = p1.data(1, 1)
        M11.data(1, 2) = p1.data(2, 1)
        M11.data(1, 3) = p1.data(3, 1)
        M11.data(1, 4) = 1
        M11.data(2, 1) = p2.data(1, 1)
        M11.data(2, 2) = p2.data(2, 1)
        M11.data(2, 3) = p2.data(3, 1)
        M11.data(2, 4) = 1
        M11.data(3, 1) = p3.data(1, 1)
        M11.data(3, 2) = p3.data(2, 1)
        M11.data(3, 3) = p3.data(3, 1)
        M11.data(3, 4) = 1
        M11.data(4, 1) = p4.data(1, 1)
        M11.data(4, 2) = p4.data(2, 1)
        M11.data(4, 3) = p4.data(3, 1)
        M11.data(4, 4) = 1

        M12.data(1, 1) = p1.data(1, 1) ^ 2 + p1.data(2, 1) ^ 2 + p1.data(3, 1) ^ 2
        M12.data(1, 2) = p1.data(2, 1)
        M12.data(1, 3) = p1.data(3, 1)
        M12.data(1, 4) = 1
        M12.data(2, 1) = p2.data(1, 1) ^ 2 + p2.data(2, 1) ^ 2 + p2.data(3, 1) ^ 2
        M12.data(2, 2) = p2.data(2, 1)
        M12.data(2, 3) = p2.data(3, 1)
        M12.data(2, 4) = 1
        M12.data(3, 1) = p3.data(1, 1) ^ 2 + p3.data(2, 1) ^ 2 + p3.data(3, 1) ^ 2
        M12.data(3, 2) = p3.data(2, 1)
        M12.data(3, 3) = p3.data(3, 1)
        M12.data(3, 4) = 1
        M12.data(4, 1) = p4.data(1, 1) ^ 2 + p4.data(2, 1) ^ 2 + p4.data(3, 1) ^ 2
        M12.data(4, 2) = p4.data(2, 1)
        M12.data(4, 3) = p4.data(3, 1)
        M12.data(4, 4) = 1

        M13.data(1, 1) = p1.data(1, 1) ^ 2 + p1.data(2, 1) ^ 2 + p1.data(3, 1) ^ 2
        M13.data(1, 2) = p1.data(1, 1)
        M13.data(1, 3) = p1.data(3, 1)
        M13.data(1, 4) = 1
        M13.data(2, 1) = p2.data(1, 1) ^ 2 + p2.data(2, 1) ^ 2 + p2.data(3, 1) ^ 2
        M13.data(2, 2) = p2.data(1, 1)
        M13.data(2, 3) = p2.data(3, 1)
        M13.data(2, 4) = 1
        M13.data(3, 1) = p3.data(1, 1) ^ 2 + p3.data(2, 1) ^ 2 + p3.data(3, 1) ^ 2
        M13.data(3, 2) = p3.data(1, 1)
        M13.data(3, 3) = p3.data(3, 1)
        M13.data(3, 4) = 1
        M13.data(4, 1) = p4.data(1, 1) ^ 2 + p4.data(2, 1) ^ 2 + p4.data(3, 1) ^ 2
        M13.data(4, 2) = p4.data(1, 1)
        M13.data(4, 3) = p4.data(3, 1)
        M13.data(4, 4) = 1

        M14.data(1, 1) = p1.data(1, 1) ^ 2 + p1.data(2, 1) ^ 2 + p1.data(3, 1) ^ 2
        M14.data(1, 2) = p1.data(1, 1)
        M14.data(1, 3) = p1.data(2, 1)
        M14.data(1, 4) = 1
        M14.data(2, 1) = p2.data(1, 1) ^ 2 + p2.data(2, 1) ^ 2 + p2.data(3, 1) ^ 2
        M14.data(2, 2) = p2.data(1, 1)
        M14.data(2, 3) = p2.data(2, 1)
        M14.data(2, 4) = 1
        M14.data(3, 1) = p3.data(1, 1) ^ 2 + p3.data(2, 1) ^ 2 + p3.data(3, 1) ^ 2
        M14.data(3, 2) = p3.data(1, 1)
        M14.data(3, 3) = p3.data(2, 1)
        M14.data(3, 4) = 1
        M14.data(4, 1) = p4.data(1, 1) ^ 2 + p4.data(2, 1) ^ 2 + p4.data(3, 1) ^ 2
        M14.data(4, 2) = p4.data(1, 1)
        M14.data(4, 3) = p4.data(2, 1)
        M14.data(4, 4) = 1

        M15.data(1, 1) = p1.data(1, 1) ^ 2 + p1.data(2, 1) ^ 2 + p1.data(3, 1) ^ 2
        M15.data(1, 2) = p1.data(1, 1)
        M15.data(1, 3) = p1.data(2, 1)
        M15.data(1, 4) = p1.data(3, 1)
        M15.data(2, 1) = p2.data(1, 1) ^ 2 + p2.data(2, 1) ^ 2 + p2.data(3, 1) ^ 2
        M15.data(2, 2) = p2.data(1, 1)
        M15.data(2, 3) = p2.data(2, 1)
        M15.data(2, 4) = p2.data(3, 1)
        M15.data(3, 1) = p3.data(1, 1) ^ 2 + p3.data(2, 1) ^ 2 + p3.data(3, 1) ^ 2
        M15.data(3, 2) = p3.data(1, 1)
        M15.data(3, 3) = p3.data(2, 1)
        M15.data(3, 4) = p3.data(3, 1)
        M15.data(4, 1) = p4.data(1, 1) ^ 2 + p4.data(2, 1) ^ 2 + p4.data(3, 1) ^ 2
        M15.data(4, 2) = p4.data(1, 1)
        M15.data(4, 3) = p4.data(2, 1)
        M15.data(4, 4) = p4.data(3, 1)

        Dim M11d As Decimal = M11.determinant4x4
        Dim M12d As Decimal = M12.determinant4x4
        Dim M13d As Decimal = M13.determinant4x4
        Dim M14d As Decimal = M14.determinant4x4
        Dim M15d As Decimal = M15.determinant4x4

        If M11d <> 0 Then
            solution.data(1, 1) = M12d / (2 * M11d)
            solution.data(2, 1) = -M13d / (2 * M11d)
            solution.data(3, 1) = M14d / (2 * M11d)
            solution.data(4, 1) = Math.Sqrt(solution.data(1, 1) ^ 2 + solution.data(2, 1) ^ 2 + solution.data(3, 1) ^ 2 - M15d / M11d)
            ValidResultFound = True
        Else
            ValidResultFound = False
        End If
        Return solution
    End Function

    Private Sub ManualCameraTargetNavigationsToolStripMenuItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ManualCameraTargetNavigationsToolStripMenuItem.CheckedChanged
        camUpButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
        camDownButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
        camLeftButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
        camRightButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
        tarUpButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
        tarDownButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
        tarLeftButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
        tarRightButton.Visible = ManualCameraTargetNavigationsToolStripMenuItem.Checked
    End Sub

    Private Sub exportTargetsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exportTargetsButton.Click
        exportCSVFileDialog.Title = "Select Path and Name of Registration Target Points File to Export"
        Dim exportDialog As DialogResult = exportCSVFileDialog.ShowDialog
        If exportDialog = Windows.Forms.DialogResult.OK Then
            Dim exportStream As New StreamWriter(exportCSVFileDialog.FileName)
            For Each cpRow As DataGridViewRow In targetsDataGridView.Rows
                Dim c1, c2, c3, c4, c5 As String

                If IsNothing(cpRow.Cells.Item(0).Value) = True Then
                    c1 = String.Empty
                Else
                    c1 = cpRow.Cells.Item(0).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(1).Value) = True Then
                    c2 = String.Empty
                Else
                    c2 = cpRow.Cells.Item(1).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(2).Value) = True Then
                    c3 = String.Empty
                Else
                    c3 = cpRow.Cells.Item(2).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(3).Value) = True Then
                    c4 = String.Empty
                Else
                    c4 = cpRow.Cells.Item(3).Value.ToString
                End If

                If IsNothing(cpRow.Cells.Item(4).Value) = True Then
                    c5 = String.Empty
                Else
                    c5 = cpRow.Cells.Item(4).Value.ToString
                End If

                exportStream.WriteLine(c1 & "," & c2 & "," & c3 & "," & c4 & "," & c5)
            Next
            exportStream.Close()
        End If
    End Sub

    Private Sub mainForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim offsetY As Integer = Me.Size.Height - heightOffset
        Dim offsetX As Integer = Me.Size.Width - widthOffset
        Dim newPoint As Point

        newPoint.X = camLeftButton.Location.X
        newPoint.Y = 861 + offsetY
        camLeftButton.Location = newPoint

        newPoint.X = camRightButton.Location.X
        newPoint.Y = 861 + offsetY
        camRightButton.Location = newPoint

        newPoint.X = camUpButton.Location.X
        newPoint.Y = 852 + offsetY
        camUpButton.Location = newPoint

        newPoint.X = camDownButton.Location.X
        newPoint.Y = 870 + offsetY
        camDownButton.Location = newPoint

        newPoint.X = Label9.Location.X
        newPoint.Y = 890 + offsetY
        Label9.Location = newPoint

        newPoint.X = camXLabel.Location.X
        newPoint.Y = 905 + offsetY
        camXLabel.Location = newPoint

        newPoint.X = camYLabel.Location.X
        newPoint.Y = 920 + offsetY
        camYLabel.Location = newPoint

        newPoint.X = camZLabel.Location.X
        newPoint.Y = 935 + offsetY
        camZLabel.Location = newPoint

        newPoint.X = 1184 + offsetX
        newPoint.Y = 861 + offsetY
        tarLeftButton.Location = newPoint

        newPoint.X = 1230 + offsetX
        newPoint.Y = 861 + offsetY
        tarRightButton.Location = newPoint

        newPoint.X = 1207 + offsetX
        newPoint.Y = 852 + offsetY
        tarUpButton.Location = newPoint

        newPoint.X = 1207 + offsetX
        newPoint.Y = 870 + offsetY
        tarDownButton.Location = newPoint

        newPoint.X = 1149 + offsetX
        newPoint.Y = 890 + offsetY
        Label16.Location = newPoint

        newPoint.X = 1183 + offsetX
        newPoint.Y = 905 + offsetY
        tarXLabel.Location = newPoint

        newPoint.X = 1183 + offsetX
        newPoint.Y = 920 + offsetY
        tarYLabel.Location = newPoint

        newPoint.X = 1183 + offsetX
        newPoint.Y = 935 + offsetY
        tarZLabel.Location = newPoint

        newPoint.X = 494 + offsetX \ 2
        newPoint.Y = 565 + offsetY
        geoRefGroupBox.Location = newPoint

        newPoint.X = statusLabel.Location.X
        newPoint.Y = 955 + offsetY
        statusLabel.Location = newPoint
        statusLabel.Width = 931 + offsetX

        newPoint.X = 1189 + offsetX
        newPoint.Y = 955 + offsetY
        percentageLabel.Location = newPoint

        newPoint.X = Label42.Location.X
        newPoint.Y = 338 + offsetY
        Label42.Location = newPoint

        newPoint.X = pickTargetPtButton.Location.X
        newPoint.Y = 313 + offsetY
        pickTargetPtButton.Location = newPoint

        newPoint.X = importTargetsButton.Location.X
        newPoint.Y = 313 + offsetY
        importTargetsButton.Location = newPoint

        newPoint.X = exportTargetsButton.Location.X
        newPoint.Y = 313 + offsetY
        exportTargetsButton.Location = newPoint

        newPoint.X = clearTargetsButton.Location.X
        newPoint.Y = 313 + offsetY
        clearTargetsButton.Location = newPoint

        mapPanel.Height = 922 + offsetY
        mapPanel.Width = 1003 + offsetX
        GroupBox5.Height = 341 + offsetY
        targetsDataGridView.Height = 233 + offsetY
        Label41.Height = 333 + offsetY
        Label40.Height = 333 + offsetY

        newPoint.X = 950 + offsetX
        newPoint.Y = panCheckBox.Location.Y
        panCheckBox.Location = newPoint

        newPoint.X = 1196 + offsetX
        newPoint.Y = pickCameraButton.Location.Y
        pickCameraButton.Location = newPoint

        newPoint.X = 1228 + offsetX
        newPoint.Y = pickTargetButton.Location.Y
        pickTargetButton.Location = newPoint

        If Not zoomTrackBar Is Nothing Then
            newPoint.X = 1212 + offsetX
            newPoint.Y = zoomTrackBar.Location.Y
            zoomTrackBar.Location = newPoint
        End If
    End Sub

    Private Sub mainForm_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        'Dim mapBitmap2 As Bitmap = mapBitmap
        mapBitmap = New Bitmap(mapPanel.Width, mapPanel.Height, Imaging.PixelFormat.Format24bppRgb)
        mapBitmap.SetResolution(300, 300)
        mapGraphics = Graphics.FromImage(mapBitmap)
    End Sub
End Class
