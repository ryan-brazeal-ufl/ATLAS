'''''ATLAS'''''
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

'Class: libLAS_e57
'By: Ryan Brazeal   (ryan.brazeal@rgbi.ca)
'Copyright: RGBi Engineering
'Date: Aug, 2013
'Description:   A wrapper class for using the las2e57 binary utility published by the libe57 project team (http://www.libe57.org)
'               within VB.Net for writing out point cloud data to e57 files. The class can encode the point cloud data inside a LAS file into an e57 file.
'Requirements:  txt2las.exe, and all its 50 supporting .dll files, and las2e57.exe must be located in a directory within the Windows PATH (e.g., C:\Windows directory)
'               The .exe files and supporting .dll files are included in this sample project package within the libLAS directory
'               or the required libLAS external files can be installed via OSGeo4W (see http://www.liblas.org/osgeo4w.html for more details) and the required las2e57.exe
'               external file can be installed via a download from libE57 (see http://www.libe57.org/download.html for more details) 

Option Explicit On

Imports System.Diagnostics
Imports System.Text.RegularExpressions

Public Class libe57
    Private _isRunning As Boolean = False
    Private _e57Process As Process
    Private _e57Thread As System.Threading.Thread
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
    Public Event Tick() 'used to test when the encoding process has ended

    Public Sub New()
        DefStartInfo()
        _startInfo.WorkingDirectory = Application.StartupPath
    End Sub

    Public Sub New(ByVal InFile As String, Optional ByVal OutFile As String = "", Optional ByVal Options As String = "", Optional ByVal e57Path As String = Nothing)
        DefStartInfo()
        If e57Path = Nothing Then
            _startInfo.WorkingDirectory = Application.StartupPath
        End If
        _inFile = InFile
        _outFile = OutFile
    End Sub

    Private Sub DefStartInfo()
        'hides the command line window and directs output and errors back to here
        _startInfo.FileName = "las2e57.exe"
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
    Public Property e57Path() As String
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

    Private Sub SetEncoding()
        'assumes las2e57.exe is within the Windows PATH (C:\Windows directory for example)
        _startInfo.FileName = "las2e57.exe"
        _e57Process = New Process
        _startInfo.Arguments = _inFile & " " & _outFile
        _e57Process.StartInfo = _startInfo
        _processingTime = 0D
        _errorFlag = False
        _cancelFlag = False
    End Sub

    Public Function StartEncoding(Optional ByVal processingStartTime As Double = 0D) As Boolean
        If _isRunning Or _inFile = "-" Or _outFile = "-" Then Return False
        _isRunning = True
        SetEncoding()
        _processingTime = processingStartTime

        Try
            _e57Thread = New System.Threading.Thread(AddressOf las2e57)
            _e57Thread.IsBackground = True
            _e57Thread.Name = "txt2e57_shell"
            _timer.Start()
            _e57Thread.Start()
            Return True
        Catch ex As Exception
            _timer.Stop()
            _isRunning = False
            RaiseEvent ErrorOccurred(ex.Message)
            Return False
        End Try
    End Function

    Private Sub las2e57()

        Dim oneLine As String
        Try
            _e57Process.Start()

            oneLine = _e57Process.StandardError.ReadLine()

            While Not oneLine Is Nothing
                If check4Error(oneLine) Then
                    _errorFlag = True
                    _timer.Stop()
                    Exit While
                End If
                oneLine = _e57Process.StandardError.ReadLine()
            End While

            _e57Process.Close()
            _e57Process = Nothing
            _isRunning = False
            If _errorFlag = True Then
                RaiseEvent ErrorOccurred("Encoding Error")
            End If

        Catch ex As Exception
            If Not _e57Process Is Nothing Then
                _e57Process.Close()
                _e57Process = Nothing
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
        If line.StartsWith("Usage") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Cancel()
        Try
            If Not _e57Process Is Nothing Then
                _isRunning = False
                _timer.Stop()
                _e57Process.Kill()
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
