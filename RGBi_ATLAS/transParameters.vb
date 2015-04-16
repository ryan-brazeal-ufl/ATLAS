'''''ATLAS'''''
'By: Ryan Brazeal
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

Imports System.ComponentModel
Imports System.Threading

Public Class transParameters

    Dim currentDataset2Trans As Integer = 0
    Dim applyOrUnapply As Integer = 0
    Dim transDataset As New Generic.List(Of cloudPoint)
    Dim transParameters(,) As Decimal
    Friend updateMade As Boolean = False
    Dim applyAllClicked As Boolean = False

    Private Sub transParameters_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        transParameters = mainForm.transformParameters
    End Sub

    Private Sub dataset1ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset1ApplyButton.Click
        If dataset1ApplyButton.Text = "Apply" Then
            dataset1ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset1ApplyButton.Text = "Unapply" Then
            dataset1ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 1
            transDataset = mainForm.dataset1MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 1 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset2ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset2ApplyButton.Click
        If dataset2ApplyButton.Text = "Apply" Then
            dataset2ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset2ApplyButton.Text = "Unapply" Then
            dataset2ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If

        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 2
            transDataset = mainForm.dataset2MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 2 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset3ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset3ApplyButton.Click
        If dataset3ApplyButton.Text = "Apply" Then
            dataset3ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset3ApplyButton.Text = "Unapply" Then
            dataset3ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 3
            transDataset = mainForm.dataset3MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 3 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset4ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset4ApplyButton.Click
        If dataset4ApplyButton.Text = "Apply" Then
            dataset4ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset4ApplyButton.Text = "Unapply" Then
            dataset4ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 4
            transDataset = mainForm.dataset4MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 4 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset5ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset5ApplyButton.Click
        If dataset5ApplyButton.Text = "Apply" Then
            dataset5ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset5ApplyButton.Text = "Unapply" Then
            dataset5ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 5
            transDataset = mainForm.dataset5MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 5 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset6ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset6ApplyButton.Click
        If dataset6ApplyButton.Text = "Apply" Then
            dataset6ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset6ApplyButton.Text = "Unapply" Then
            dataset6ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 6
            transDataset = mainForm.dataset6MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 6 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset7ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset7ApplyButton.Click
        If dataset7ApplyButton.Text = "Apply" Then
            dataset7ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset7ApplyButton.Text = "Unapply" Then
            dataset7ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 7
            transDataset = mainForm.dataset7MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 7 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset8ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset8ApplyButton.Click
        If dataset8ApplyButton.Text = "Apply" Then
            dataset8ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset8ApplyButton.Text = "Unapply" Then
            dataset8ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 8
            transDataset = mainForm.dataset8MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 8 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset9ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset9ApplyButton.Click
        If dataset9ApplyButton.Text = "Apply" Then
            dataset9ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset9ApplyButton.Text = "Unapply" Then
            dataset9ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 9
            transDataset = mainForm.dataset9MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 9 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub dataset10ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataset10ApplyButton.Click
        If dataset10ApplyButton.Text = "Apply" Then
            dataset10ApplyButton.Text = "Unapply"
            applyOrUnapply = 1
        ElseIf dataset10ApplyButton.Text = "Unapply" Then
            dataset10ApplyButton.Text = "Apply"
            applyOrUnapply = 0
        End If


        If transBGW.IsBusy <> True Then
            ' Start the asynchronous operation.
            transProgressBar.Minimum = 0
            transProgressBar.Maximum = 100
            transProgressBar.Value = 0
            currentDataset2Trans = 10
            transDataset = mainForm.dataset10MapDataPoints
            transBGW.RunWorkerAsync()

        Else
            If applyAllClicked = False Then
                MessageBox.Show("The application is busy applying a coordinate transformation to dataset 10 so please wait until this process is complete", "APPLICATION BUSY", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            End If
        End If



    End Sub

    Private Sub transBGW_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles transBGW.DoWork

        Dim R1 As Matrix
        Dim oldXYZ, newXYZ As New Matrix(3, 1)
        Dim trans1 As New Matrix(3, 1)

        trans1.data(1, 1) = transParameters(currentDataset2Trans - 1, 4)
        trans1.data(2, 1) = transParameters(currentDataset2Trans - 1, 5)
        trans1.data(3, 1) = transParameters(currentDataset2Trans - 1, 6)

        R1 = threeDrotationMatrix(transParameters(currentDataset2Trans - 1, 1), transParameters(currentDataset2Trans - 1, 2), transParameters(currentDataset2Trans - 1, 3))

        If applyOrUnapply = 0 Then
            R1.equals(R1.Transpose)
        End If

        Dim interval As Integer = transDataset.Count \ 100

        For i As Integer = 0 To transDataset.Count - 1
            If i Mod interval = 0 Then
                transBGW.ReportProgress((i + 1) / transDataset.Count * 100)
            End If
            Dim newMapPoint As New cloudPoint

            oldXYZ.data(1, 1) = transDataset(i).X
            oldXYZ.data(2, 1) = transDataset(i).Y
            oldXYZ.data(3, 1) = transDataset(i).Z

            If applyOrUnapply = 1 Then
                newXYZ = transParameters(currentDataset2Trans - 1, 0) * R1.Transpose * oldXYZ + trans1
            Else
                oldXYZ = (oldXYZ - trans1) / transParameters(currentDataset2Trans - 1, 0)
                newXYZ = R1.Transpose * oldXYZ
            End If

            newMapPoint.X = newXYZ.data(1, 1)
            newMapPoint.Y = newXYZ.data(2, 1)
            newMapPoint.Z = newXYZ.data(3, 1)
            newMapPoint.R = transDataset(i).R
            newMapPoint.G = transDataset(i).G
            newMapPoint.B = transDataset(i).B
            newMapPoint.I = transDataset(i).I
            newMapPoint.Xm = newMapPoint.X
            newMapPoint.Ym = newMapPoint.Y
            newMapPoint.displayX = 0
            newMapPoint.displayY = 0
            newMapPoint.highlighted = transDataset(i).highlighted
            newMapPoint.deleted = transDataset(i).deleted
            newMapPoint.datasetNum = transDataset(i).datasetNum
            transDataset(i) = newMapPoint
        Next

    End Sub

    Private Function threeDrotationMatrix(ByVal xrot As Decimal, ByVal yrot As Decimal, ByVal zrot As Decimal) As Matrix
        Dim solutionMatrix As New Matrix(3, 3)

        Dim O1 As Decimal = xrot
        Dim O2 As Decimal = yrot
        Dim O3 As Decimal = zrot

        solutionMatrix.data(1, 1) = Math.Cos(O2) * Math.Cos(O3)
        solutionMatrix.data(1, 2) = Math.Sin(O1) * Math.Sin(O2) * Math.Cos(O3) + Math.Cos(O1) * Math.Sin(O3)
        solutionMatrix.data(1, 3) = Math.Sin(O1) * Math.Sin(O3) - Math.Cos(O1) * Math.Sin(O2) * Math.Cos(O3)
        solutionMatrix.data(2, 1) = (-1) * Math.Cos(O2) * Math.Sin(O3)
        solutionMatrix.data(2, 2) = Math.Cos(O1) * Math.Cos(O3) - Math.Sin(O1) * Math.Sin(O2) * Math.Sin(O3)
        solutionMatrix.data(2, 3) = Math.Cos(O1) * Math.Sin(O2) * Math.Sin(O3) + Math.Sin(O1) * Math.Cos(O3)
        solutionMatrix.data(3, 1) = Math.Sin(O2)
        solutionMatrix.data(3, 2) = -1 * Math.Sin(O1) * Math.Cos(O2)
        solutionMatrix.data(3, 3) = Math.Cos(O1) * Math.Cos(O2)

        Return solutionMatrix
    End Function

    Private Sub transBGW_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles transBGW.ProgressChanged
        transProgressBar.Value = e.ProgressPercentage
        transProgressBar.Refresh()
    End Sub

    Private Sub transBGW_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles transBGW.RunWorkerCompleted
        updateMade = True
        transProgressBar.Value = transProgressBar.Maximum
        transProgressBar.Refresh()
        transLabel.Text = "COMPLETE"
        transLabel.Refresh()
        If currentDataset2Trans = 1 Then
            mainForm.dataset1Calcd = False
            mainForm.dataset1MapDataPoints = transDataset
            If dataset2ApplyButton.Visible = True And applyAllClicked = True Then
                dataset2ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 2 Then
            mainForm.dataset2Calcd = False
            mainForm.dataset2MapDataPoints = transDataset
            If dataset3ApplyButton.Visible = True And applyAllClicked = True Then
                dataset3ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 3 Then
            mainForm.dataset3Calcd = False
            mainForm.dataset3MapDataPoints = transDataset
            If dataset4ApplyButton.Visible = True And applyAllClicked = True Then
                dataset4ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 4 Then
            mainForm.dataset4Calcd = False
            mainForm.dataset4MapDataPoints = transDataset
            If dataset5ApplyButton.Visible = True And applyAllClicked = True Then
                dataset5ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 5 Then
            mainForm.dataset5Calcd = False
            mainForm.dataset5MapDataPoints = transDataset
            If dataset6ApplyButton.Visible = True And applyAllClicked = True Then
                dataset6ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 6 Then
            mainForm.dataset6Calcd = False
            mainForm.dataset6MapDataPoints = transDataset
            If dataset7ApplyButton.Visible = True And applyAllClicked = True Then
                dataset7ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 7 Then
            mainForm.dataset7Calcd = False
            mainForm.dataset7MapDataPoints = transDataset
            If dataset8ApplyButton.Visible = True And applyAllClicked = True Then
                dataset8ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 8 Then
            mainForm.dataset8Calcd = False
            mainForm.dataset8MapDataPoints = transDataset
            If dataset9ApplyButton.Visible = True And applyAllClicked = True Then
                dataset9ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 9 Then
            mainForm.dataset9Calcd = False
            mainForm.dataset9MapDataPoints = transDataset
            If dataset10ApplyButton.Visible = True And applyAllClicked = True Then
                dataset10ApplyButton_Click(sender, e)
            End If
        ElseIf currentDataset2Trans = 10 Then
            mainForm.dataset10Calcd = False
            mainForm.dataset10MapDataPoints = transDataset
        End If
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        transLabel.Text = String.Empty
        transProgressBar.Value = transProgressBar.Minimum
        applyAllClicked = False
        Timer1.Stop()
        Me.Close()
    End Sub

    Private Sub transParametersDataGridView_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles transParametersDataGridView.CellMouseEnter
        If e.RowIndex <> -1 And e.ColumnIndex <> 0 Then
            Dim stdDevString As String
            If e.ColumnIndex = 1 Then
                stdDevString = Math.Round(mainForm.transformParametersStdDev(e.RowIndex, e.ColumnIndex - 1), 6).ToString
            ElseIf e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Then
                stdDevString = Math.Round(mainForm.transformParametersStdDev(e.RowIndex, e.ColumnIndex - 1), 4).ToString & "m"
            Else
                stdDevString = mainForm.DecToDMS(mainForm.transformParametersStdDev(e.RowIndex, e.ColumnIndex - 1) * 180D / Math.PI)
            End If
            stdDevLabel.Text = "Std. Dev. = " & Chr(177) & " " & stdDevString
        End If
    End Sub

    Private Sub transParametersDataGridView_CellMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles transParametersDataGridView.CellMouseLeave
        stdDevLabel.Text = ""
    End Sub

    Private Sub applyAllButton_Click(sender As Object, e As EventArgs) Handles applyAllButton.Click
        applyAllClicked = True
        If dataset1ApplyButton.Visible = True Then
            dataset1ApplyButton_Click(sender, e)
        End If
    End Sub
End Class