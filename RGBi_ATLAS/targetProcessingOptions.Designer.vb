<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class targetProcessingOptions
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.iterationsComboBox = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.highIntensityTextBox = New System.Windows.Forms.TextBox()
        Me.lowIntensityTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.option3RadioButton = New System.Windows.Forms.RadioButton()
        Me.option2RadioButton = New System.Windows.Forms.RadioButton()
        Me.option1RadioButton = New System.Windows.Forms.RadioButton()
        Me.okButton = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(280, 32)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select the maximum number of iterations to perform when determining the target:"
        '
        'iterationsComboBox
        '
        Me.iterationsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.iterationsComboBox.FormattingEnabled = True
        Me.iterationsComboBox.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25"})
        Me.iterationsComboBox.Location = New System.Drawing.Point(298, 12)
        Me.iterationsComboBox.Name = "iterationsComboBox"
        Me.iterationsComboBox.Size = New System.Drawing.Size(57, 21)
        Me.iterationsComboBox.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.highIntensityTextBox)
        Me.GroupBox1.Controls.Add(Me.lowIntensityTextBox)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.option3RadioButton)
        Me.GroupBox1.Controls.Add(Me.option2RadioButton)
        Me.GroupBox1.Controls.Add(Me.option1RadioButton)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 55)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(343, 153)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Circular Targets - Boundary Edge Selection Criteria"
        '
        'highIntensityTextBox
        '
        Me.highIntensityTextBox.Enabled = False
        Me.highIntensityTextBox.Location = New System.Drawing.Point(258, 121)
        Me.highIntensityTextBox.Name = "highIntensityTextBox"
        Me.highIntensityTextBox.Size = New System.Drawing.Size(69, 20)
        Me.highIntensityTextBox.TabIndex = 4
        '
        'lowIntensityTextBox
        '
        Me.lowIntensityTextBox.Enabled = False
        Me.lowIntensityTextBox.Location = New System.Drawing.Point(104, 121)
        Me.lowIntensityTextBox.Name = "lowIntensityTextBox"
        Me.lowIntensityTextBox.Size = New System.Drawing.Size(69, 20)
        Me.lowIntensityTextBox.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Enabled = False
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(190, 124)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "High Value:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Enabled = False
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(38, 124)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Low Value:"
        '
        'option3RadioButton
        '
        Me.option3RadioButton.AutoSize = True
        Me.option3RadioButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option3RadioButton.Location = New System.Drawing.Point(14, 102)
        Me.option3RadioButton.Name = "option3RadioButton"
        Me.option3RadioButton.Size = New System.Drawing.Size(174, 17)
        Me.option3RadioButton.TabIndex = 2
        Me.option3RadioButton.TabStop = True
        Me.option3RadioButton.Text = "Fixed Range of Intenstiy Values"
        Me.option3RadioButton.UseVisualStyleBackColor = True
        '
        'option2RadioButton
        '
        Me.option2RadioButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option2RadioButton.Location = New System.Drawing.Point(14, 47)
        Me.option2RadioButton.Name = "option2RadioButton"
        Me.option2RadioButton.Size = New System.Drawing.Size(325, 49)
        Me.option2RadioButton.TabIndex = 1
        Me.option2RadioButton.Text = "Average middle Intensity values determined from max. histogram peaks for black an" & _
            "d white points"
        Me.option2RadioButton.UseVisualStyleBackColor = True
        '
        'option1RadioButton
        '
        Me.option1RadioButton.AutoSize = True
        Me.option1RadioButton.Checked = True
        Me.option1RadioButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.option1RadioButton.Location = New System.Drawing.Point(14, 24)
        Me.option1RadioButton.Name = "option1RadioButton"
        Me.option1RadioButton.Size = New System.Drawing.Size(250, 17)
        Me.option1RadioButton.TabIndex = 0
        Me.option1RadioButton.TabStop = True
        Me.option1RadioButton.Text = "Middle 1% of Intensity values for selected points"
        Me.option1RadioButton.UseVisualStyleBackColor = True
        '
        'okButton
        '
        Me.okButton.Location = New System.Drawing.Point(148, 214)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(72, 34)
        Me.okButton.TabIndex = 2
        Me.okButton.Text = "OK"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'targetProcessingOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(367, 255)
        Me.ControlBox = False
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.iterationsComboBox)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "targetProcessingOptions"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Target Processing Options"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents iterationsComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents highIntensityTextBox As System.Windows.Forms.TextBox
    Friend WithEvents lowIntensityTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents option3RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents option2RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents option1RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents okButton As System.Windows.Forms.Button
End Class
