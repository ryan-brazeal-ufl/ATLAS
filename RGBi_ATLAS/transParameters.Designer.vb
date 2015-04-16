<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class transParameters
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.transParametersDataGridView = New System.Windows.Forms.DataGridView()
        Me.TPcolumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPcolumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPcolumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPcolumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPcolumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPcolumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPcolumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TPcolumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataset1ApplyButton = New System.Windows.Forms.Button()
        Me.dataset2ApplyButton = New System.Windows.Forms.Button()
        Me.dataset3ApplyButton = New System.Windows.Forms.Button()
        Me.dataset4ApplyButton = New System.Windows.Forms.Button()
        Me.dataset5ApplyButton = New System.Windows.Forms.Button()
        Me.dataset6ApplyButton = New System.Windows.Forms.Button()
        Me.dataset7ApplyButton = New System.Windows.Forms.Button()
        Me.dataset8ApplyButton = New System.Windows.Forms.Button()
        Me.dataset9ApplyButton = New System.Windows.Forms.Button()
        Me.dataset10ApplyButton = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.transProgressBar = New System.Windows.Forms.ProgressBar()
        Me.transLabel = New System.Windows.Forms.Label()
        Me.transBGW = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.stdDevLabel = New System.Windows.Forms.Label()
        Me.applyAllButton = New System.Windows.Forms.Button()
        CType(Me.transParametersDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'transParametersDataGridView
        '
        Me.transParametersDataGridView.AllowUserToAddRows = False
        Me.transParametersDataGridView.AllowUserToDeleteRows = False
        Me.transParametersDataGridView.AllowUserToResizeColumns = False
        Me.transParametersDataGridView.AllowUserToResizeRows = False
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.transParametersDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.transParametersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.transParametersDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TPcolumn1, Me.TPcolumn2, Me.TPcolumn3, Me.TPcolumn4, Me.TPcolumn5, Me.TPcolumn6, Me.TPcolumn7, Me.TPcolumn8})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.transParametersDataGridView.DefaultCellStyle = DataGridViewCellStyle4
        Me.transParametersDataGridView.Location = New System.Drawing.Point(12, 19)
        Me.transParametersDataGridView.Name = "transParametersDataGridView"
        Me.transParametersDataGridView.ReadOnly = True
        Me.transParametersDataGridView.RowHeadersVisible = False
        Me.transParametersDataGridView.Size = New System.Drawing.Size(1023, 243)
        Me.transParametersDataGridView.TabIndex = 0
        '
        'TPcolumn1
        '
        Me.TPcolumn1.HeaderText = "Dataset Name"
        Me.TPcolumn1.Name = "TPcolumn1"
        Me.TPcolumn1.ReadOnly = True
        Me.TPcolumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn1.Width = 150
        '
        'TPcolumn2
        '
        Me.TPcolumn2.HeaderText = "Scale "
        Me.TPcolumn2.Name = "TPcolumn2"
        Me.TPcolumn2.ReadOnly = True
        Me.TPcolumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn2.Width = 90
        '
        'TPcolumn3
        '
        Me.TPcolumn3.HeaderText = "X Rotation "
        Me.TPcolumn3.Name = "TPcolumn3"
        Me.TPcolumn3.ReadOnly = True
        Me.TPcolumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn3.Width = 150
        '
        'TPcolumn4
        '
        Me.TPcolumn4.HeaderText = "Y Rotation "
        Me.TPcolumn4.Name = "TPcolumn4"
        Me.TPcolumn4.ReadOnly = True
        Me.TPcolumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn4.Width = 150
        '
        'TPcolumn5
        '
        Me.TPcolumn5.HeaderText = "Z Rotation "
        Me.TPcolumn5.Name = "TPcolumn5"
        Me.TPcolumn5.ReadOnly = True
        Me.TPcolumn5.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn5.Width = 150
        '
        'TPcolumn6
        '
        Me.TPcolumn6.HeaderText = "X Translation"
        Me.TPcolumn6.Name = "TPcolumn6"
        Me.TPcolumn6.ReadOnly = True
        Me.TPcolumn6.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn6.Width = 109
        '
        'TPcolumn7
        '
        Me.TPcolumn7.HeaderText = "Y Translation"
        Me.TPcolumn7.Name = "TPcolumn7"
        Me.TPcolumn7.ReadOnly = True
        Me.TPcolumn7.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn7.Width = 110
        '
        'TPcolumn8
        '
        Me.TPcolumn8.HeaderText = "Z Translation"
        Me.TPcolumn8.Name = "TPcolumn8"
        Me.TPcolumn8.ReadOnly = True
        Me.TPcolumn8.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TPcolumn8.Width = 110
        '
        'dataset1ApplyButton
        '
        Me.dataset1ApplyButton.Enabled = False
        Me.dataset1ApplyButton.Location = New System.Drawing.Point(1041, 40)
        Me.dataset1ApplyButton.Name = "dataset1ApplyButton"
        Me.dataset1ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset1ApplyButton.TabIndex = 1
        Me.dataset1ApplyButton.Text = "Apply"
        Me.dataset1ApplyButton.UseVisualStyleBackColor = True
        Me.dataset1ApplyButton.Visible = False
        '
        'dataset2ApplyButton
        '
        Me.dataset2ApplyButton.Enabled = False
        Me.dataset2ApplyButton.Location = New System.Drawing.Point(1041, 62)
        Me.dataset2ApplyButton.Name = "dataset2ApplyButton"
        Me.dataset2ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset2ApplyButton.TabIndex = 2
        Me.dataset2ApplyButton.Text = "Apply"
        Me.dataset2ApplyButton.UseVisualStyleBackColor = True
        Me.dataset2ApplyButton.Visible = False
        '
        'dataset3ApplyButton
        '
        Me.dataset3ApplyButton.Enabled = False
        Me.dataset3ApplyButton.Location = New System.Drawing.Point(1041, 84)
        Me.dataset3ApplyButton.Name = "dataset3ApplyButton"
        Me.dataset3ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset3ApplyButton.TabIndex = 3
        Me.dataset3ApplyButton.Text = "Apply"
        Me.dataset3ApplyButton.UseVisualStyleBackColor = True
        Me.dataset3ApplyButton.Visible = False
        '
        'dataset4ApplyButton
        '
        Me.dataset4ApplyButton.Enabled = False
        Me.dataset4ApplyButton.Location = New System.Drawing.Point(1041, 106)
        Me.dataset4ApplyButton.Name = "dataset4ApplyButton"
        Me.dataset4ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset4ApplyButton.TabIndex = 4
        Me.dataset4ApplyButton.Text = "Apply"
        Me.dataset4ApplyButton.UseVisualStyleBackColor = True
        Me.dataset4ApplyButton.Visible = False
        '
        'dataset5ApplyButton
        '
        Me.dataset5ApplyButton.Enabled = False
        Me.dataset5ApplyButton.Location = New System.Drawing.Point(1041, 128)
        Me.dataset5ApplyButton.Name = "dataset5ApplyButton"
        Me.dataset5ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset5ApplyButton.TabIndex = 5
        Me.dataset5ApplyButton.Text = "Apply"
        Me.dataset5ApplyButton.UseVisualStyleBackColor = True
        Me.dataset5ApplyButton.Visible = False
        '
        'dataset6ApplyButton
        '
        Me.dataset6ApplyButton.Enabled = False
        Me.dataset6ApplyButton.Location = New System.Drawing.Point(1041, 150)
        Me.dataset6ApplyButton.Name = "dataset6ApplyButton"
        Me.dataset6ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset6ApplyButton.TabIndex = 6
        Me.dataset6ApplyButton.Text = "Apply"
        Me.dataset6ApplyButton.UseVisualStyleBackColor = True
        Me.dataset6ApplyButton.Visible = False
        '
        'dataset7ApplyButton
        '
        Me.dataset7ApplyButton.Enabled = False
        Me.dataset7ApplyButton.Location = New System.Drawing.Point(1041, 172)
        Me.dataset7ApplyButton.Name = "dataset7ApplyButton"
        Me.dataset7ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset7ApplyButton.TabIndex = 7
        Me.dataset7ApplyButton.Text = "Apply"
        Me.dataset7ApplyButton.UseVisualStyleBackColor = True
        Me.dataset7ApplyButton.Visible = False
        '
        'dataset8ApplyButton
        '
        Me.dataset8ApplyButton.Enabled = False
        Me.dataset8ApplyButton.Location = New System.Drawing.Point(1041, 194)
        Me.dataset8ApplyButton.Name = "dataset8ApplyButton"
        Me.dataset8ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset8ApplyButton.TabIndex = 8
        Me.dataset8ApplyButton.Text = "Apply"
        Me.dataset8ApplyButton.UseVisualStyleBackColor = True
        Me.dataset8ApplyButton.Visible = False
        '
        'dataset9ApplyButton
        '
        Me.dataset9ApplyButton.Enabled = False
        Me.dataset9ApplyButton.Location = New System.Drawing.Point(1041, 216)
        Me.dataset9ApplyButton.Name = "dataset9ApplyButton"
        Me.dataset9ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset9ApplyButton.TabIndex = 9
        Me.dataset9ApplyButton.Text = "Apply"
        Me.dataset9ApplyButton.UseVisualStyleBackColor = True
        Me.dataset9ApplyButton.Visible = False
        '
        'dataset10ApplyButton
        '
        Me.dataset10ApplyButton.Enabled = False
        Me.dataset10ApplyButton.Location = New System.Drawing.Point(1041, 238)
        Me.dataset10ApplyButton.Name = "dataset10ApplyButton"
        Me.dataset10ApplyButton.Size = New System.Drawing.Size(88, 23)
        Me.dataset10ApplyButton.TabIndex = 10
        Me.dataset10ApplyButton.Text = "Apply"
        Me.dataset10ApplyButton.UseVisualStyleBackColor = True
        Me.dataset10ApplyButton.Visible = False
        '
        'transProgressBar
        '
        Me.transProgressBar.Location = New System.Drawing.Point(165, 266)
        Me.transProgressBar.Name = "transProgressBar"
        Me.transProgressBar.Size = New System.Drawing.Size(870, 23)
        Me.transProgressBar.TabIndex = 12
        '
        'transLabel
        '
        Me.transLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.transLabel.Location = New System.Drawing.Point(1041, 267)
        Me.transLabel.Name = "transLabel"
        Me.transLabel.Size = New System.Drawing.Size(75, 23)
        Me.transLabel.TabIndex = 13
        Me.transLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'transBGW
        '
        Me.transBGW.WorkerReportsProgress = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 3000
        '
        'stdDevLabel
        '
        Me.stdDevLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stdDevLabel.Location = New System.Drawing.Point(12, 267)
        Me.stdDevLabel.Name = "stdDevLabel"
        Me.stdDevLabel.Size = New System.Drawing.Size(147, 23)
        Me.stdDevLabel.TabIndex = 14
        Me.stdDevLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'applyAllButton
        '
        Me.applyAllButton.Location = New System.Drawing.Point(1041, 4)
        Me.applyAllButton.Name = "applyAllButton"
        Me.applyAllButton.Size = New System.Drawing.Size(88, 37)
        Me.applyAllButton.TabIndex = 15
        Me.applyAllButton.Text = "Apply/Unapply ALL"
        Me.applyAllButton.UseVisualStyleBackColor = True
        '
        'transParameters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1136, 295)
        Me.Controls.Add(Me.applyAllButton)
        Me.Controls.Add(Me.stdDevLabel)
        Me.Controls.Add(Me.transLabel)
        Me.Controls.Add(Me.transProgressBar)
        Me.Controls.Add(Me.dataset10ApplyButton)
        Me.Controls.Add(Me.dataset9ApplyButton)
        Me.Controls.Add(Me.dataset8ApplyButton)
        Me.Controls.Add(Me.dataset7ApplyButton)
        Me.Controls.Add(Me.dataset6ApplyButton)
        Me.Controls.Add(Me.dataset5ApplyButton)
        Me.Controls.Add(Me.dataset4ApplyButton)
        Me.Controls.Add(Me.dataset3ApplyButton)
        Me.Controls.Add(Me.dataset2ApplyButton)
        Me.Controls.Add(Me.dataset1ApplyButton)
        Me.Controls.Add(Me.transParametersDataGridView)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "transParameters"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Conformal Transformation Parameters"
        CType(Me.transParametersDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents transParametersDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents dataset1ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset2ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset3ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset4ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset5ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset6ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset7ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset8ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset9ApplyButton As System.Windows.Forms.Button
    Friend WithEvents dataset10ApplyButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents transProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents transLabel As System.Windows.Forms.Label
    Friend WithEvents transBGW As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents stdDevLabel As System.Windows.Forms.Label
    Friend WithEvents TPcolumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TPcolumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TPcolumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TPcolumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TPcolumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TPcolumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TPcolumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TPcolumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents applyAllButton As System.Windows.Forms.Button
End Class
