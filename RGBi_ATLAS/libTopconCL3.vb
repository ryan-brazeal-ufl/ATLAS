'''''ATLAS'''''
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

'Class: libTopconCL3
'By: Ryan Brazeal   (ryan.brazeal@rgbi.ca)
'Copyright: RGBi Engineering
'Date: Aug, 2013
'Description:   A wrapper class for using the cl32txt binary utility - that I created using the Topcon GLS SDK codec (TopconCodec.dll) written within 
'               VS C++ 10 - within VB.Net for reading in point cloud data from Topcon CL3 files. The class can decode CL3 files into TXT files
'               that can then be read into a VB.Net application.
'Requirements:  cl32txt.exe and the TopconCodec.dll file must be located in a directory within the Windows PATH (e.g., C:\Windows directory)
'               The .exe file and supporting .dll file are included in this sample project package within the References directory.

Option Explicit On

Imports System.Diagnostics
Imports System.Text.RegularExpressions

Public Class libTopconCL3
    Private _isRunning As Boolean = False
    Private _cl3Process As Process
    Private _cl3Thread As System.Threading.Thread
    Private _startInfo As New ProcessStartInfo
    Private WithEvents _timer As New Timer
    Private _timerInterval As Integer
    Private _processingTime As Double
    Private _errorFlag As Boolean
    Private _cancelFlag As Boolean

    'command line arguments
    Private _inFile As String
    Private _outFile As String
   
    'Events
    Public Event Cancelled()
    Public Event ErrorOccurred(ByVal ErrorType As String)
    Public Event Tick() 'used to test when the decoding process has ended

    Public Sub New()
        DefStartInfo()
        _startInfo.WorkingDirectory = Application.StartupPath
    End Sub

    Public Sub New(ByVal InFile As String, Optional ByVal OutFile As String = "", Optional ByVal Options As String = "", Optional ByVal lasPath As String = Nothing)
        DefStartInfo()
        If lasPath = Nothing Then
            _startInfo.WorkingDirectory = Application.StartupPath
        End If
        _inFile = InFile
        _outFile = OutFile
    End Sub

    Private Sub DefStartInfo()
        'hides the command line window and directs output and errors back to here
        _startInfo.FileName = "cl32txt.exe"
        _startInfo.UseShellExecute = False
        _startInfo.RedirectStandardOutput = True
        _startInfo.RedirectStandardError = True
        _startInfo.CreateNoWindow = True
        _timerInterval = 100
        _timer.Interval = _timerInterval
        _processingTime = 0D
        _errorFlag = False
        _cancelFlag = False
    End Sub
    Public Property InputFile() As String
        Get
            Return _inFile
        End Get
        Set(ByVal Value As String)
            _inFile = Value
        End Set
    End Property
    Public Property OutputFile() As String
        Get
            Return _outFile
        End Get
        Set(ByVal Value As String)
            _outFile = Value
        End Set
    End Property
    Public Property lasPath() As String
        Get
            Return _startInfo.WorkingDirectory
        End Get
        Set(ByVal Value As String)
            _startInfo.WorkingDirectory = Value
        End Set
    End Property
    Public Property TimerInterval() As Integer
        Get
            Return _timerInterval
        End Get
        Set(ByVal value As Integer)
            _timerInterval = value
            _timer.Interval = _timerInterval
        End Set
    End Property
    Public ReadOnly Property isRunning() As Boolean
        Get
            Return _isRunning
        End Get
    End Property
    Public ReadOnly Property processingTime() As Double
        Get
            Return _processingTime
        End Get
    End Property

    Private Sub SetDecoding()
        'assumes cl32txt.exe is within the Windows PATH (C:\Windows directory for example)
        _startInfo.FileName = "cl32txt.exe"
        _cl3Process = New Process
        _startInfo.Arguments = _inFile & " " & _outFile
        _cl3Process.StartInfo = _startInfo
        _processingTime = 0D
        _errorFlag = False
        _cancelFlag = False
    End Sub

    Public Function StartDecoding() As Boolean

        If _isRunning Or _inFile = "-" Or _outFile = "-" Then Return False
        _isRunning = True
        SetDecoding()

        Try
            _cl3Thread = New System.Threading.Thread(AddressOf cl32txt)
            _cl3Thread.IsBackground = True
            _cl3Thread.Name = "cl32txt_shell"
            _timer.Start()
            _cl3Thread.Start()
            Return True
        Catch ex As Exception
            _timer.Stop()
            _isRunning = False
            RaiseEvent ErrorOccurred(ex.Message)
            Return False
        End Try

    End Function

    Private Sub cl32txt()

        Dim oneLine As String
        Try
            _cl3Process.Start()

            oneLine = _cl3Process.StandardError.ReadLine()
            While Not oneLine Is Nothing
                If check4Error(oneLine) Then
                    _errorFlag = True
                    _timer.Stop()
                    Exit While
                End If
                oneLine = _cl3Process.StandardError.ReadLine()
            End While

            _cl3Process.Close()
            _cl3Process = Nothing
            _isRunning = False
            If _errorFlag = True Then
                RaiseEvent ErrorOccurred("Decoding Error")
            End If

        Catch ex As Exception
            If Not _cl3Process Is Nothing Then
                _cl3Process.Close()
                _cl3Process = Nothing
            End If
            _isRunning = False
            _timer.Stop()
            RaiseEvent ErrorOccurred(ex.Message)
        End Try

    End Sub

    Public Sub TimerStop()
        _timer.Stop()
        _processingTime = Math.Round(_processingTime, 3)
    End Sub

    Private Function check4Error(ByVal line As String) As Boolean
        If line.StartsWith("Error") Or line.StartsWith("Usage") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Cancel()
        Try
            If Not _cl3Process Is Nothing Then
                _isRunning = False
                _timer.Stop()
                _cl3Process.Kill()
                _cancelFlag = True
                RaiseEvent Cancelled()
            End If
        Catch ex As Exception
            _isRunning = False
            _timer.Stop()
            RaiseEvent ErrorOccurred(ex.Message)
            'blanket catch
        End Try
    End Sub

    Private Sub _timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _timer.Tick
        _processingTime += _timerInterval / 1000D
        RaiseEvent Tick()
    End Sub
End Class
