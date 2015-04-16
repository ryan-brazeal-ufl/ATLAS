'''''ATLAS'''''
'By: Ryan Brazeal
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

Public Class textFormatForm

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        mainForm.tFFContinue = False
        mainForm.xCol = -1
        mainForm.yCol = -1
        mainForm.zCol = -1
        mainForm.rCol = -1
        mainForm.gCol = -1
        mainForm.bCol = -1
        mainForm.iCol = -1
        Dim duplicateCols As Boolean = False
        Dim xFound As Boolean = False
        Dim yFound As Boolean = False
        Dim zFound As Boolean = False

        For Each controlG As Control In Me.Controls
            If controlG.Name.ToLower = "columnsgroupbox" Then
                For Each controlA As Control In controlG.Controls
                    If TypeOf controlA Is ComboBox Then
                        Dim colA As ComboBox = CType(controlA, ComboBox)
                        If colA.SelectedIndex = 1 Then
                            xFound = True
                            mainForm.xCol = Integer.Parse(colA.Name.Substring(3, 1))
                        ElseIf colA.SelectedIndex = 2 Then
                            yFound = True
                            mainForm.yCol = Integer.Parse(colA.Name.Substring(3, 1))
                        ElseIf colA.SelectedIndex = 3 Then
                            zFound = True
                            mainForm.zCol = Integer.Parse(colA.Name.Substring(3, 1))
                        ElseIf colA.SelectedIndex = 4 Then
                            mainForm.rCol = Integer.Parse(colA.Name.Substring(3, 1))
                        ElseIf colA.SelectedIndex = 5 Then
                            mainForm.gCol = Integer.Parse(colA.Name.Substring(3, 1))
                        ElseIf colA.SelectedIndex = 6 Then
                            mainForm.bCol = Integer.Parse(colA.Name.Substring(3, 1))
                        ElseIf colA.SelectedIndex = 7 Then
                            mainForm.iCol = Integer.Parse(colA.Name.Substring(3, 1))
                        End If

                        For Each controlB As Control In controlG.Controls
                            If TypeOf controlB Is ComboBox Then
                                Dim colB As ComboBox = CType(controlB, ComboBox)
                                If colA.Name <> colB.Name Then
                                    If colA.SelectedIndex = colB.SelectedIndex Then
                                        If colA.SelectedIndex <> 0 Then
                                            duplicateCols = True
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        Next


        If xFound And yFound And zFound Then
            If Not duplicateCols Then
                mainForm.tFFContinue = True
                Me.Hide()
            Else
                MessageBox.Show("Duplicate column selections have been deteched, this" & ControlChars.NewLine & "is not allowed and must be corrected!", "FORMAT ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("X, Y, Z coordinates must be assigned to a column!", "FORMAT ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelItButton.Click
        mainForm.tFFContinue = False
        Me.Hide()
    End Sub

    Private Sub headerYesRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles headerYesRadioButton.CheckedChanged
        If headerYesRadioButton.Checked Then
            mainForm.headerRow = True
        End If
    End Sub

    Private Sub headerNoRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles headerNoRadioButton.CheckedChanged
        If headerNoRadioButton.Checked Then
            mainForm.headerRow = False
        End If
    End Sub

    Private Sub spaceRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles spaceRadioButton.CheckedChanged
        If spaceRadioButton.Checked Then
            mainForm.delimiter = " "
        End If
    End Sub

    Private Sub commaRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles commaRadioButton.CheckedChanged
        If commaRadioButton.Checked Then
            mainForm.delimiter = ","
        End If
    End Sub

    Private Sub XYZiSRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYZiSRadioButton.CheckedChanged
        If XYZiSRadioButton.Checked Then
            col1ComboBox.SelectedIndex = 1
            col2ComboBox.SelectedIndex = 2
            col3ComboBox.SelectedIndex = 3
            col4ComboBox.SelectedIndex = 7
            col5ComboBox.SelectedIndex = 0
            col6ComboBox.SelectedIndex = 0
            col7ComboBox.SelectedIndex = 0
            spaceRadioButton.Checked = True
        End If
    End Sub

    Private Sub XYZiCRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYZiCRadioButton.CheckedChanged
        If XYZiCRadioButton.Checked Then
            col1ComboBox.SelectedIndex = 1
            col2ComboBox.SelectedIndex = 2
            col3ComboBox.SelectedIndex = 3
            col4ComboBox.SelectedIndex = 7
            col5ComboBox.SelectedIndex = 0
            col6ComboBox.SelectedIndex = 0
            col7ComboBox.SelectedIndex = 0
            commaRadioButton.Checked = True
        End If
    End Sub

    Private Sub XYZiRGBSRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYZiRGBSRadioButton.CheckedChanged
        If XYZiRGBSRadioButton.Checked Then
            col1ComboBox.SelectedIndex = 1
            col2ComboBox.SelectedIndex = 2
            col3ComboBox.SelectedIndex = 3
            col4ComboBox.SelectedIndex = 7
            col5ComboBox.SelectedIndex = 4
            col6ComboBox.SelectedIndex = 5
            col7ComboBox.SelectedIndex = 6
            spaceRadioButton.Checked = True
        End If
    End Sub

    Private Sub XYZiRGBCRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYZiRGBCRadioButton.CheckedChanged
        If XYZiRGBCRadioButton.Checked Then
            col1ComboBox.SelectedIndex = 1
            col2ComboBox.SelectedIndex = 2
            col3ComboBox.SelectedIndex = 3
            col4ComboBox.SelectedIndex = 7
            col5ComboBox.SelectedIndex = 4
            col6ComboBox.SelectedIndex = 5
            col7ComboBox.SelectedIndex = 6
            commaRadioButton.Checked = True
        End If
    End Sub

    Private Sub XYZRGBiSRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYZRGBiSRadioButton.CheckedChanged
        If XYZRGBiSRadioButton.Checked Then
            col1ComboBox.SelectedIndex = 1
            col2ComboBox.SelectedIndex = 2
            col3ComboBox.SelectedIndex = 3
            col4ComboBox.SelectedIndex = 4
            col5ComboBox.SelectedIndex = 5
            col6ComboBox.SelectedIndex = 6
            col7ComboBox.SelectedIndex = 7
            spaceRadioButton.Checked = True
        End If
    End Sub

    Private Sub XYZRGBiCRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XYZRGBiCRadioButton.CheckedChanged
        If XYZRGBiCRadioButton.Checked Then
            col1ComboBox.SelectedIndex = 1
            col2ComboBox.SelectedIndex = 2
            col3ComboBox.SelectedIndex = 3
            col4ComboBox.SelectedIndex = 4
            col5ComboBox.SelectedIndex = 5
            col6ComboBox.SelectedIndex = 6
            col7ComboBox.SelectedIndex = 7
            commaRadioButton.Checked = True
        End If
    End Sub
End Class