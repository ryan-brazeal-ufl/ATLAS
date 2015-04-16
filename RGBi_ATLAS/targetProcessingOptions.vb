'''''ATLAS'''''
'By: Ryan Brazeal
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

Public Class targetProcessingOptions

    Private Sub option3RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles option3RadioButton.CheckedChanged
        If option3RadioButton.Checked Then
            mainForm.processingOption = 3
        End If
        Label2.Enabled = option3RadioButton.Checked
        Label3.Enabled = option3RadioButton.Checked
        lowIntensityTextBox.Enabled = option3RadioButton.Checked
        highIntensityTextBox.Enabled = option3RadioButton.Checked
    End Sub

    Private Sub targetProcessingOptions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        iterationsComboBox.SelectedIndex = mainForm.maxIterations - 1
    End Sub

    Friend Sub iterationsComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iterationsComboBox.SelectedIndexChanged
        mainForm.maxIterations = iterationsComboBox.SelectedIndex + 1
    End Sub

    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles okButton.Click
        If option3RadioButton.Checked Then
            If IsNumeric(lowIntensityTextBox.Text) AndAlso IsNumeric(highIntensityTextBox.Text) AndAlso lowIntensityTextBox.Text <> String.Empty AndAlso highIntensityTextBox.Text <> String.Empty Then
                Dim lowValue As Integer
                Dim highValue As Integer
                Try
                    lowValue = Integer.Parse(lowIntensityTextBox.Text)
                    Try
                        highValue = Integer.Parse(highIntensityTextBox.Text)
                        If highValue >= lowValue Then
                            mainForm.selectedHistMaxMin(0) = highValue
                            mainForm.selectedHistMaxMin(1) = lowValue
                            Me.Close()
                        Else
                            MessageBox.Show("The high intensity value must be greater than or equal to the low intensity value", "INCORRECT INTENSITY LIMITS", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                            option1RadioButton.Checked = True
                        End If
                    Catch ex As Exception
                        MessageBox.Show("The high intensity value you entered MUST be an integer", "HIGH INTENSITY NOT AN INTEGER", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                        option1RadioButton.Checked = True
                    End Try
                Catch ex As Exception
                    MessageBox.Show("The low intensity value you entered MUST be an integer", "LOW INTENSITY NOT AN INTEGER", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                    option1RadioButton.Checked = True
                End Try
            Else
                MessageBox.Show("The intensity values must be integer numeric values", "NON-NUMERIC INTENSITY LIMITS", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                option1RadioButton.Checked = True
            End If
        Else
            Me.Close()
        End If
    End Sub

    Private Sub option1RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles option1RadioButton.CheckedChanged
        If option1RadioButton.Checked Then
            mainForm.processingOption = 1
        End If
    End Sub

    Private Sub option2RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles option2RadioButton.CheckedChanged
        If option2RadioButton.Checked Then
            mainForm.processingOption = 2
        End If
    End Sub
End Class