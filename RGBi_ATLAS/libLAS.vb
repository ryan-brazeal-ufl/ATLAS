'''''ATLAS'''''
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

'Class: libLAS_TXT
'By: Ryan Brazeal   (ryan.brazeal@rgbi.ca)
'Copyright: RGBi Engineering
'Date: Aug, 2013
'Description:   A wrapper class for using the las2txt and txt2las binary utilities published by the libLAS project team (http://www.libLAS.org)
'               within VB.Net for reading in and writing out point cloud data from/to LAS files. The class can either decode LAS files into TXT files
'               that can then be read into a VB.Net application or the class and encode the point cloud data inside a TXT file into a LAS file.
'Requirements:  las2txt.exe and txt2las.exe and all 50 supporting .dll files must be located in a directory within the Windows PATH (e.g., C:\Windows directory)
'               The .exe files and supporting .dll files are included in this smaple project package within the libLAS directory
'               or these required external files can be installed via OSGeo4W (see http://www.liblas.org/osgeo4w.html for more details)

'Notes: Only basic support for reading in and writing out LAS files is provided here. libLas provides a much wider set of tools and further LAS file processing
'       tools and utilities and are available using the full APIs (however they are not .NET libraries and therefore must be used with C/C++/Python or one can
'       attempt to Invoke the functions within VB.Net using dllImport, but that is more than I care to do or need for LAS file support)

Option Explicit On

Imports System.Diagnostics
Imports System.Text.RegularExpressions

Public Class libLAS
    Private _isRunning As Boolean = False
    Private _lasProcess As Process
    Private _lasThread As System.Threading.Thread
    Private _startInfo As New ProcessStartInfo
    Private WithEvents _timer As New Timer
    Private _timerInterval As Integer
    Private _processingTime As Double
    Private _decoding As Boolean
    Private _errorFlag As Boolean
    Private _cancelFlag As Boolean

    'command line arguments
    Private _inFile As String
    Private _outFile As String
    Private _options As String

    'Events
    Public Event Cancelled()
    Public Event ErrorOccurred(ByVal ErrorType As String)
    Public Event Tick() 'used to test when the decoding/encoding process has ended

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
        _options = Options
    End Sub

    Private Sub DefStartInfo()
        'hides the command line window and directs output and errors back to here
        _startInfo.FileName = "las2txt.exe"
        _startInfo.UseShellExecute = False
        _startInfo.RedirectStandardOutput = True
        _startInfo.RedirectStandardError = True
        _startInfo.CreateNoWindow = True
        _timerInterval = 100
        _timer.Interval = _timerInterval
        _processingTime = 0D
        _decoding = True
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
    Public Property Options() As String
        Get
            Return _options
        End Get
        Set(ByVal Value As String)
            _options = Value
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
    Public ReadOnly Property decoding() As Boolean
        Get
            Return _decoding
        End Get
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
        _decoding = True
        'assumes las2txt.exe is within the Windows PATH (C:\Windows directory for example)
        _startInfo.FileName = "las2txt.exe"
        _lasProcess = New Process
        _startInfo.Arguments = "-i " & _inFile & " -o " & _outFile & " " & _options
        _lasProcess.StartInfo = _startInfo
        _processingTime = 0D
        _errorFlag = False
        _cancelFlag = False
    End Sub

    Public Function StartDecoding() As Boolean

        If _isRunning Or _inFile = "-" Or _outFile = "-" Then Return False
        _isRunning = True
        SetDecoding()

        Try
            _lasThread = New System.Threading.Thread(AddressOf las2txt)
            _lasThread.IsBackground = True
            _lasThread.Name = "las2txt_shell"
            _timer.Start()
            _lasThread.Start()
            Return True
        Catch ex As Exception
            _timer.Stop()
            _isRunning = False
            RaiseEvent ErrorOccurred(ex.Message)
            Return False
        End Try

    End Function

    Private Sub las2txt()

        Dim oneLine As String
        Try
            _lasProcess.Start()

            oneLine = _lasProcess.StandardError.ReadLine()
            While Not oneLine Is Nothing
                If check4Error(oneLine) Then
                    _errorFlag = True
                    _timer.Stop()
                    Exit While
                End If
                oneLine = _lasProcess.StandardError.ReadLine()
            End While

            _lasProcess.Close()
            _lasProcess = Nothing
            _isRunning = False
            If _errorFlag = True Then
                RaiseEvent ErrorOccurred("Decoding Error")
            End If

        Catch ex As Exception
            If Not _lasProcess Is Nothing Then
                _lasProcess.Close()
                _lasProcess = Nothing
            End If
            _isRunning = False
            _timer.Stop()
            RaiseEvent ErrorOccurred(ex.Message)
        End Try

    End Sub

    Private Sub SetEncoding()
        _decoding = False
        'assumes txt2las.exe is within the Windows PATH (C:\Windows directory for example)
        _startInfo.FileName = "txt2las.exe"
        _lasProcess = New Process
        _startInfo.Arguments = "-i " & _inFile & " -o " & _outFile & " " & _options
        _lasProcess.StartInfo = _startInfo
        _processingTime = 0D
        _errorFlag = False
        _cancelFlag = False
    End Sub

    Public Function StartEncoding() As Boolean
        If _isRunning Or _inFile = "-" Or _outFile = "-" Then Return False
        _isRunning = True
        SetEncoding()

        Try
            _lasThread = New System.Threading.Thread(AddressOf txt2las)
            _lasThread.IsBackground = True
            _lasThread.Name = "txt2las_shell"
            _timer.Start()
            _lasThread.Start()
            Return True
        Catch ex As Exception
            _timer.Stop()
            _isRunning = False
            RaiseEvent ErrorOccurred(ex.Message)
            Return False
        End Try
    End Function

    Private Sub txt2las()

        Dim oneLine As String
        Try
            _lasProcess.Start()

            oneLine = _lasProcess.StandardError.ReadLine()

            While Not oneLine Is Nothing
                If check4Error(oneLine) Then
                    _errorFlag = True
                    _timer.Stop()
                    Exit While
                End If
                oneLine = _lasProcess.StandardError.ReadLine()
            End While

            _lasProcess.Close()
            _lasProcess = Nothing
            _isRunning = False
            If _errorFlag = True Then
                RaiseEvent ErrorOccurred("Encoding Error")
            End If

        Catch ex As Exception
            If Not _lasProcess Is Nothing Then
                _lasProcess.Close()
                _lasProcess = Nothing
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
        If line.StartsWith("error") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Cancel()
        Try
            If Not _lasProcess Is Nothing Then
                _isRunning = False
                _timer.Stop()
                _lasProcess.Kill()
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
