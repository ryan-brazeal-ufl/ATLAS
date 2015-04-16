<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class histogramForm
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
        Me.renderPanel = New System.Windows.Forms.Panel()
        Me.refreshButton = New System.Windows.Forms.Button()
        Me.renderPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'renderPanel
        '
        Me.renderPanel.BackColor = System.Drawing.Color.Transparent
        Me.renderPanel.Controls.Add(Me.refreshButton)
        Me.renderPanel.Location = New System.Drawing.Point(-1, 0)
        Me.renderPanel.Name = "renderPanel"
        Me.renderPanel.Size = New System.Drawing.Size(1026, 625)
        Me.renderPanel.TabIndex = 0
        '
        'refreshButton
        '
        Me.refreshButton.Location = New System.Drawing.Point(13, 589)
        Me.refreshButton.Name = "refreshButton"
        Me.refreshButton.Size = New System.Drawing.Size(75, 23)
        Me.refreshButton.TabIndex = 0
        Me.refreshButton.Text = "Refresh"
        Me.refreshButton.UseVisualStyleBackColor = True
        '
        'histogramForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1024, 624)
        Me.Controls.Add(Me.renderPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(700, 350)
        Me.Name = "histogramForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "    Intensity Histogram Plot"
        Me.renderPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents renderPanel As System.Windows.Forms.Panel
    Friend WithEvents refreshButton As System.Windows.Forms.Button
End Class
