'''''ATLAS'''''
'Date: April 2015
'GNU GPL V2 License
'www.rgbi.ca
'www.jrpi.ca

'VB.NET 2010 MATRIX CLASS
'VERSION 1.4

'Programmed by: Ryan Brazeal, P.Eng., P.Surv., PMP
'Contact: ryan.brazeal@rgbi.ca
'Date Started: February, 2008
'Date of last update: July, 2013

'Created to help students learn and explore linear algebra and also to help create least-squares applications

'UPDATE LOG:

'   - added method to convert a 1x1 matrix to a scalar value of decimal data type   [Feb.12, 2008]
'   - added method to return the diagonal elements of a square matrix as a vector matrix (ie. nRows, 1) [Feb.12, 2008]
'   - added method to return the square root of all the elements within a matrix  [Feb.12, 2008]
'   - removed Matrix Inversion by Cholesky Decomposition due to debugging problem and Gaussian Inversion method works perfect! [Feb.12, 2008]
'   - added redimensioning method to either preserve existing data and simply rezise the matrix or resize and reset all elements = 0 [Feb.26, 2008]
'   - added methods to return an entire row or column from a matrix [Mar.2, 2008]

'   VERSION 1.3 CREATED
'   - added methods to solve dot product and cross product between 3 dimensional vectors [Mar.28, 2013]
'   - added method to solve the absolute value (length) of a 3 dimensional vector [Mar.28, 2013]
'   - added the suppressErrorMessage switch in order to help with debugging problems by allowing the host application to catch the exceptions thrown by the matrix class [Apr.5, 2013]
'   - added the overload equals function which allows you to set a matrix equal to another matrix object (kind of weird behaviour but it works, if you use the = operator it treats the
'       matrix objects as having a ByRef relationship and a change in one will cause a change in the others elements as well. [Apr.15, 2013]
'   - added x,y,z direction vectors and R1,R2,R3 three dimensional rotation matrix definitions [Apr.17, 2013]

'   VERSION 1.4 CREATED
'   - re-wrote the printAll method to use a DataGridView control on a new form, allows for much easier Matrix element viewing. The form, rows and columns can be resized [Jul.21, 2013]
'   - added the 4x4, 3x3, 2x2 determinants methods [Jul.27, 2013]

'Please use an APPROX. ZERO VALUE = 0.000000000001 instead of the values 0.0, especially for VC matrices

Imports Microsoft.VisualBasic
Imports Microsoft

Public Class Matrix
    
    Private nRowsV As Integer                   'number of rows in the matrix
    Private nColsV As Integer                   'number of columns in the matrix
    Private dataV(0, 0) As Decimal              'array to store individual matrix elements
    Private suppressErrorMessagesV As Boolean   'boolean switch to turn off the Matrix object's Error Messages
    Private WithEvents newForm As Form
    Private elementsDGV As DataGridView

    'default constructor, creates 1 by 1 array
    Sub New()
        nRowsV = 1
        nColsV = 1
        data(1, 1) = 0
        suppressErrorMessagesV = False
        'suppressErrorMessagesV = True
    End Sub

    'construtor with specific # of rows and # of columns (base1 NOT base 0)
    Sub New(ByVal rows As Integer, ByVal cols As Integer)
        nRowsV = rows
        nColsV = cols
        suppressErrorMessagesV = False
        'suppressErrorMessagesV = True

        Dim newMatrix(,) As Decimal = My2DReDim(rows, cols)
        dataV = newMatrix
    End Sub

    'construtor with specific # of rows and # of columns (base 1 NOT base 0) as well as suppression option for Error Messages
    Sub New(ByVal rows As Integer, ByVal cols As Integer, ByVal suppressErrorMessages As Boolean)
        nRowsV = rows
        nColsV = cols
        suppressErrorMessagesV = suppressErrorMessages
        'suppressErrorMessagesV = True

        Dim newMatrix(,) As Decimal = My2DReDim(rows, cols)
        dataV = newMatrix
    End Sub

    'used with the New(rows,cols) constructor to create the empty array of the correct size
    Private Function My2DReDim(ByVal rows As Integer, ByVal cols As Integer) As Decimal(,)
        Dim newMatrix(rows - 1, cols - 1) As Decimal
        Dim i, j As Integer

        For i = 0 To rows - 1
            For j = 0 To cols - 1
                newMatrix(i, j) = 0D
            Next
        Next
        Return newMatrix
    End Function

    'gets the number of rows in the matrix (base 1 NOT base 0)
    ReadOnly Property nRows() As Integer
        Get
            Return nRowsV
        End Get
    End Property

    'gets the number of columns in the matrix (base 1 NOT base 0)
    ReadOnly Property nCols() As Integer
        Get
            Return nColsV
        End Get
    End Property

    'get or set an individual matrix element (row and column are base 1 on input NOT base 0)
    Property data(ByVal row As Integer, ByVal col As Integer) As Decimal
        Get
            row -= 1
            col -= 1
            If row < nRowsV And col < nColsV Then
                If row >= 0 And col >= 0 Then
                    Return dataV(row, col)
                Else
                    If Not suppressErrorMessagesV Then
                        MessageBox.Show("Attempted to read Matrix data at an index less than zero (ie. Index to Low)", "Index Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        ErrorSoKill()
                    End If
                End If
            Else
                If Not suppressErrorMessagesV Then
                    MessageBox.Show("Attempted to read Matrix data on a row or column that does not exist (ie. Index to High)", "Index Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    ErrorSoKill()
                End If
            End If
        End Get
        Set(ByVal value As Decimal)
            row -= 1
            col -= 1
            If row < nRowsV And col < nColsV Then
                If row >= 0 And col >= 0 Then
                    dataV(row, col) = value
                Else
                    If Not suppressErrorMessagesV Then
                        MessageBox.Show("Attempted to set Matrix data at an index less than zero (ie. Index to Low)", "Index Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        ErrorSoKill()
                    End If
                End If
            Else
                If Not suppressErrorMessagesV Then
                    MessageBox.Show("Attempted to set Matrix data on a row or column that does not exist (ie. Index to High)", "Index Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    ErrorSoKill()
                End If
            End If
        End Set
    End Property

    'get or set the suppress Error Messages switch within the class object
    Property suppressErrorMessages() As Boolean
        Get
            Return suppressErrorMessagesV
        End Get
        Set(ByVal value As Boolean)
            suppressErrorMessagesV = value
        End Set
    End Property

    'matrix multiplication
    Shared Operator *(ByVal matrix1 As Matrix, ByVal matrix2 As Matrix) As Matrix
        Dim solutionMatrix As New Matrix(matrix1.nRows, matrix2.nCols)
        If matrix1.nCols = matrix2.nRows Then
            Dim i, j, k As Integer

            For i = 1 To matrix1.nRows
                For j = 1 To matrix2.nCols
                    For k = 1 To matrix1.nCols
                        solutionMatrix.data(i, j) += matrix1.data(i, k) * matrix2.data(k, j)
                    Next
                Next
            Next
        Else
            If Not matrix1.suppressErrorMessages Or Not matrix2.suppressErrorMessages Then
                MessageBox.Show("Attempted to multiply matrices of incorrect size (ie. Columns1 not = Rows2)", "Incorrect Matrix dimensions for Multiplication", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                solutionMatrix.ErrorSoKill()
            End If
        End If
        Return solutionMatrix
    End Operator

    'matrix addition
    Shared Operator +(ByVal matrix1 As Matrix, ByVal matrix2 As Matrix) As Matrix
        Dim solutionMatrix As New Matrix(matrix1.nRows, matrix1.nCols)
        If matrix1.nRows = matrix2.nRows And matrix1.nCols = matrix2.nCols Then
            Dim i, j As Integer
            For i = 1 To matrix1.nRows
                For j = 1 To matrix1.nCols
                    solutionMatrix.data(i, j) = matrix1.data(i, j) + matrix2.data(i, j)
                Next
            Next
        Else
            If Not matrix1.suppressErrorMessages Or Not matrix2.suppressErrorMessages Then
                MessageBox.Show("Attempted to add matrices of incorrect size", "Incorrect Matrix dimensions for Addition", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                solutionMatrix.ErrorSoKill()
            End If
        End If
        Return solutionMatrix
    End Operator

    'matrix subtraction
    Shared Operator -(ByVal matrix1 As Matrix, ByVal matrix2 As Matrix) As Matrix
        Dim solutionMatrix As New Matrix(matrix1.nRows, matrix1.nCols)
        If matrix1.nRows = matrix2.nRows And matrix1.nCols = matrix2.nCols Then
            Dim i, j As Integer
            For i = 1 To matrix1.nRows
                For j = 1 To matrix1.nCols
                    solutionMatrix.data(i, j) = matrix1.data(i, j) - matrix2.data(i, j)
                Next
            Next
        Else
            If Not matrix1.suppressErrorMessages Or Not matrix2.suppressErrorMessages Then
                MessageBox.Show("Attempted to subtract matrices of incorrect size", "Incorrect Matrix dimensions for Subtraction", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                solutionMatrix.ErrorSoKill()
            End If
        End If
        Return solutionMatrix
    End Operator

    'scalar multiplication
    Shared Operator *(ByVal scalar As Decimal, ByVal matrix1 As Matrix) As Matrix
        Dim solutionMatrix As New Matrix(matrix1.nRows, matrix1.nCols)
        Dim i, j As Integer

        For i = 1 To solutionMatrix.nRows
            For j = 1 To solutionMatrix.nCols
                solutionMatrix.data(i, j) = scalar * matrix1.data(i, j)
            Next
        Next
        Return solutionMatrix
    End Operator

    'scalar division
    Shared Operator /(ByVal matrix1 As Matrix, ByVal scalar As Decimal) As Matrix
        Dim solutionMatrix As New Matrix(matrix1.nRows, matrix1.nCols)
        Dim i, j As Integer

        If scalar <> 0 Then
            For i = 1 To solutionMatrix.nRows
                For j = 1 To solutionMatrix.nCols
                    solutionMatrix.data(i, j) = matrix1.data(i, j) / scalar
                Next
            Next
        Else
            If Not matrix1.suppressErrorMessages Then
                MessageBox.Show("Attempted to divide a matrix by scalar zero", "Divide by Zero Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                'solutionMatrix.ErrorSoKill()
            End If
        End If
        Return solutionMatrix
    End Operator

    '3 dimensional vector dot product
    Shared Function dot(ByVal matrix1 As Matrix, ByVal matrix2 As Matrix) As Decimal
        Dim solution As Decimal = 0
        If matrix1.nRows = 3 And matrix2.nRows = 3 And matrix1.nCols = 1 And matrix2.nCols = 1 Then
            Dim i As Integer
            For i = 1 To matrix1.nRows
                solution += matrix1.data(i, 1) * matrix2.data(i, 1)
            Next
        Else
            If Not matrix1.suppressErrorMessages Or Not matrix2.suppressErrorMessages Then
                MessageBox.Show("Attempted to take dot product of matrices with a size other than 3 x 1", "Incorrect Matrix dimensions for Dot Product", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                matrix1.ErrorSoKill()
            End If
        End If
        Return solution
    End Function

    '3 dimensional vector cross product
    Shared Function cross(ByVal matrix1 As Matrix, ByVal matrix2 As Matrix) As Matrix
        Dim solutionMatrix As New Matrix(3, 1)
        If matrix1.nRows = 3 And matrix2.nRows = 3 And matrix1.nCols = 1 And matrix2.nCols = 1 Then
            solutionMatrix.data(1, 1) = matrix1.data(2, 1) * matrix2.data(3, 1) - matrix1.data(3, 1) * matrix2.data(2, 1)
            solutionMatrix.data(2, 1) = matrix1.data(3, 1) * matrix2.data(1, 1) - matrix1.data(1, 1) * matrix2.data(3, 1)
            solutionMatrix.data(3, 1) = matrix1.data(1, 1) * matrix2.data(2, 1) - matrix1.data(2, 1) * matrix2.data(1, 1)
        Else
            If Not matrix1.suppressErrorMessages Or Not matrix2.suppressErrorMessages Then
                MessageBox.Show("Attempted to take cross product of matrices with a size other than 3 x 1", "Incorrect Matrix dimensions for Cross Product", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                matrix1.ErrorSoKill()
            End If
        End If
        Return solutionMatrix
    End Function

    '3 dimensional vector absolute value (length)
    Shared Function abs(ByVal matrix1 As Matrix) As Decimal
        Dim solution As Decimal = 0
        If matrix1.nRows = 3 And matrix1.nCols = 1 Then
            Dim i As Integer
            For i = 1 To matrix1.nRows
                solution += matrix1.data(i, 1) * matrix1.data(i, 1)
            Next
            solution = Math.Sqrt(solution)
        Else
            If Not matrix1.suppressErrorMessages Then
                MessageBox.Show("Attempted to solve absolute value of matrix with a size other than 3 x 1", "Incorrect Matrix dimension for Absolute Value", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                matrix1.ErrorSoKill()
            End If
        End If
        Return solution
    End Function

    'creates the X direction vector [1 0 0]T
    Shared Function dirX() As Matrix
        Dim solutionMatrix As New Matrix(3, 1)
        solutionMatrix.data(1, 1) = 1
        solutionMatrix.data(2, 1) = 0
        solutionMatrix.data(3, 1) = 0
        Return solutionMatrix
    End Function

    'creates the Y direction vector [0 1 0]T
    Shared Function dirY() As Matrix
        Dim solutionMatrix As New Matrix(3, 1)
        solutionMatrix.data(1, 1) = 0
        solutionMatrix.data(2, 1) = 1
        solutionMatrix.data(3, 1) = 0
        Return solutionMatrix
    End Function

    'creates the Z direction vector [0 0 1]T
    Shared Function dirZ() As Matrix
        Dim solutionMatrix As New Matrix(3, 1)
        solutionMatrix.data(1, 1) = 0
        solutionMatrix.data(2, 1) = 0
        solutionMatrix.data(3, 1) = 1
        Return solutionMatrix
    End Function

    'creates 3D rotation matrix around the X axis (angle follow right hand rule for positive rotation angle definition)
    Shared Function R1(ByVal DegAngle As Decimal) As Matrix
        Dim solutionMatrix As New Matrix(3, 3)
        solutionMatrix.data(1, 1) = 1D
        solutionMatrix.data(1, 2) = 0D
        solutionMatrix.data(1, 3) = 0D
        solutionMatrix.data(2, 1) = 0D
        solutionMatrix.data(2, 2) = Math.Cos(DegAngle * Math.PI / 180D)
        solutionMatrix.data(2, 3) = Math.Sin(DegAngle * Math.PI / 180D)
        solutionMatrix.data(3, 1) = 0D
        solutionMatrix.data(3, 2) = -1 * Math.Sin(DegAngle * Math.PI / 180D)
        solutionMatrix.data(3, 3) = Math.Cos(DegAngle * Math.PI / 180D)
        Return solutionMatrix
    End Function

    'creates 3D rotation matrix around the Y axis (angle follow right hand rule for positive rotation angle definition)
    Shared Function R2(ByVal DegAngle As Decimal) As Matrix
        Dim solutionMatrix As New Matrix(3, 3)
        solutionMatrix.data(1, 1) = Math.Cos(DegAngle * Math.PI / 180D)
        solutionMatrix.data(1, 2) = 0D
        solutionMatrix.data(1, 3) = -1 * Math.Sin(DegAngle * Math.PI / 180D)
        solutionMatrix.data(2, 1) = 0D
        solutionMatrix.data(2, 2) = 1D
        solutionMatrix.data(2, 3) = 0D
        solutionMatrix.data(3, 1) = Math.Sin(DegAngle * Math.PI / 180D)
        solutionMatrix.data(3, 2) = 0D
        solutionMatrix.data(3, 3) = Math.Cos(DegAngle * Math.PI / 180D)
        Return solutionMatrix
    End Function

    'creates 3D rotation matrix around the Z axis (angle follow right hand rule for positive rotation angle definition)
    Shared Function R3(ByVal DegAngle As Decimal) As Matrix
        Dim solutionMatrix As New Matrix(3, 3)
        solutionMatrix.data(1, 1) = Math.Cos(DegAngle * Math.PI / 180D)
        solutionMatrix.data(1, 2) = Math.Sin(DegAngle * Math.PI / 180D)
        solutionMatrix.data(1, 3) = 0D
        solutionMatrix.data(2, 1) = -1 * Math.Sin(DegAngle * Math.PI / 180D)
        solutionMatrix.data(2, 2) = Math.Cos(DegAngle * Math.PI / 180D)
        solutionMatrix.data(2, 3) = 0D
        solutionMatrix.data(3, 1) = 0D
        solutionMatrix.data(3, 2) = 0D
        solutionMatrix.data(3, 3) = 1D
        Return solutionMatrix
    End Function

    'set a matrix to Identity matrix
    Public Function makeIdentity() As Matrix
        Dim solutionMatrix As New Matrix(Me.nRows, Me.nCols)
        Dim i, j As Integer
        For i = 1 To solutionMatrix.nRows
            For j = 1 To solutionMatrix.nCols
                If i = j Then
                    solutionMatrix.data(i, j) = 1
                Else
                    solutionMatrix.data(i, j) = 0
                End If
            Next
        Next
        Return solutionMatrix
    End Function

    'resets (clears) all the elements of a matrix
    Public Function clear() As Matrix
        Dim solutionmatrix As New Matrix(Me.nRows, Me.nCols)
        Return solutionmatrix
    End Function

    'matrix inversion by Gaussian elimination
    Public Function Inverse() As Matrix
        Dim solutionMatrix As New Matrix(Me.nRows, (Me.nCols * 2I))
        Dim inverseMatrix As New Matrix(Me.nRows, Me.nCols)
        Dim boolTest As Boolean = True

        If Me.nRows <> Me.nCols Then
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to Inverse a matrix which is NOT square", "Matrix Inverse Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
            boolTest = False
        End If

        If boolTest Then
            Dim i, j As Integer
            For i = 0 To Me.nRows - 1
                For j = 0 To Me.nCols - 1
                    solutionMatrix.data(i + 1, j + 1) = Me.data(i + 1, j + 1)
                Next
            Next

            j = 0
            For i = Me.nCols To solutionMatrix.nCols - 1
                solutionMatrix.data(j + 1, i + 1) = 1D
                j += 1
            Next
            Dim t As Integer
            For i = 0 To Me.nRows - 1
                If solutionMatrix.data(i + 1, i + 1) = 0 Then
                    t = i + 1
                    While (t < Me.nRows AndAlso solutionMatrix.data(t + 1, i + 1) = 0)
                        t += 1
                    End While

                    If t = Me.nRows Then
                        If Not Me.suppressErrorMessages Then
                            MessageBox.Show("Attempted to Inverse a matrix which is singular", "Matrix Inverse Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            ErrorSoKill()
                        End If
                        boolTest = False
                    End If

                    If boolTest Then
                        For j = i To solutionMatrix.nCols - 1
                            solutionMatrix.data(i + 1, j + 1) += solutionMatrix.data(t + 1, j + 1) / solutionMatrix.data(t + 1, i + 1)
                        Next
                    End If
                End If
                Dim temp As Decimal = solutionMatrix.data(i + 1, i + 1)
                For j = i To solutionMatrix.nCols - 1
                    solutionMatrix.data(i + 1, j + 1) /= temp
                Next

                For t = 0 To Me.nRows - 1
                    If t <> i Then
                        temp = solutionMatrix.data(t + 1, i + 1)
                        For j = i To solutionMatrix.nCols - 1
                            solutionMatrix.data(t + 1, j + 1) -= temp / solutionMatrix.data(i + 1, i + 1) * solutionMatrix.data(i + 1, j + 1)
                        Next
                    End If
                Next
            Next

            Dim q As Integer
            For i = 0 To inverseMatrix.nRows - 1
                q = 0
                For j = inverseMatrix.nCols To solutionMatrix.nCols - 1
                    inverseMatrix.data(i + 1, q + 1) = solutionMatrix.data(i + 1, j + 1)
                    q += 1
                Next
            Next
        End If
        Return inverseMatrix
    End Function

    'matrix transpose
    Public Function Transpose() As Matrix
        Dim i, j As Integer
        Dim solutionMatrix As New Matrix(Me.nCols, Me.nRows)
        For i = 1 To Me.nRows
            For j = 1 To Me.nCols
                solutionMatrix.data(j, i) = Me.data(i, j)
            Next
        Next
        Return solutionMatrix
    End Function

    '1x1 matrix to scalar decimal
    Public Function toScalar() As Decimal
        Dim result As Decimal
        If Me.nRows = 1 And Me.nCols = 1 Then
            result = Me.data(1, 1)
        Else
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to convert a non 1x1 matrix to a scalar", "Matrix --> Scalar Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
        End If
        Return result
    End Function

    'get the diagonal elements of a square matrix (handy for returning variance values from a V-C matrix in least squares)
    Public Function getDiagonal() As Matrix
        Dim solutionMatrix As New Matrix(Me.nRows, 1)
        If Me.nRows = Me.nCols Then
            Dim i As Integer
            For i = 1 To Me.nRows
                solutionMatrix.data(i, 1) = Me.data(i, i)
            Next
        Else
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to get the diagonal elements of a non square matrix", "Get Diagonal Elements Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
        End If
        Return solutionMatrix
    End Function

    'takes the square root of the absolute value (positive always) of each element within a matrix 
    Public Function Sqrt() As Matrix
        Dim solutionMatrix As New Matrix(Me.nRows, Me.nCols)
        Dim i, j As Integer
        For i = 1 To Me.nRows
            For j = 1 To Me.nCols
                solutionMatrix.data(i, j) = Math.Sqrt(Math.Abs(Me.data(i, j)))
            Next
        Next
        Return solutionMatrix
    End Function

    'matrix re-dimensioning function, option to preserve the data already inside a Matrix and just make it a different size or reset all the data = 0 
    Public Function matrixReDim(ByVal rows As Integer, ByVal cols As Integer, Optional ByVal Preserve As Boolean = False) As Matrix
        Dim solutionMatrix As New Matrix(rows, cols)
        Dim i, j As Integer
        If Preserve = True Then
            For i = 1 To rows
                For j = 1 To cols
                    If i <= Me.nRows And j <= Me.nCols Then
                        solutionMatrix.data(i, j) = Me.data(i, j)
                    End If
                Next
            Next
        End If
        Return solutionMatrix
    End Function

    'get an entire row of a matrix
    Public Function getRow(ByVal row As Integer) As Matrix
        Dim solutionMatrix As New Matrix(1, Me.nCols)
        Dim i As Integer
        If row > 0 And row <= Me.nRows Then
            For i = 1 To Me.nCols
                solutionMatrix.data(1, i) = Me.data(row, i)
            Next
        Else
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to get a row that does not exist", "Get Row Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
        End If
        Return solutionMatrix
    End Function

    'get an entire column of a matrix
    Public Function getColumn(ByVal column As Integer) As Matrix
        Dim solutionMatrix As New Matrix(Me.nRows, 1)
        Dim i As Integer
        If column > 0 And column <= Me.nCols Then
            For i = 1 To Me.nRows
                solutionMatrix.data(i, 1) = Me.data(i, column)
            Next
        Else
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to get a column that does not exist", "Get Column Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
    End If
        Return solutionMatrix
    End Function

    'function used to set one matrix's elements to the same as another matrix
    Public Overloads Function equals(ByVal matrix1 As Matrix) As Matrix
        For i As Integer = 1 To matrix1.nRows
            For j As Integer = 1 To matrix1.nCols
                Me.data(i, j) = matrix1.data(i, j)
            Next
        Next
        Return Me
    End Function

    'error handling function, kills the execution of the program
    Private Sub ErrorSoKill()
        End
    End Sub

    'matrix printing
    Public Sub printAll(Optional ByVal matrixName As String = "", Optional ByVal formatOutput As Boolean = False, Optional ByVal decimals As Integer = 0)
        newForm = New Form
        elementsDGV = New DataGridView

        For k = 1 To nColsV
            elementsDGV.Columns.Add("c" & k.ToString, k.ToString)
        Next
        Dim anchorPt As New Point(0, 0)
        Dim maxSize As New Size(1600, 1200)
        Dim minSize As New Size(200, 200)
        elementsDGV.Width = 645
        elementsDGV.Height = 280
        elementsDGV.Location = anchorPt
        elementsDGV.ScrollBars = ScrollBars.Both
        elementsDGV.ColumnHeadersVisible = False
        elementsDGV.RowHeadersVisible = False
        elementsDGV.AllowUserToAddRows = False
        elementsDGV.AllowUserToDeleteRows = False
        elementsDGV.AllowUserToOrderColumns = False

        newForm.Controls.Add(elementsDGV)
        newForm.Width = 665
        newForm.Height = 330
        newForm.Text = matrixName
        newForm.MaximumSize = maxSize
        newForm.MinimumSize = minSize

        Dim rowElements(nColsV - 1) As Object

        For i As Integer = 1 To nRowsV
            For j As Integer = 1 To nColsV
                If formatOutput = True Then
                    rowElements(j - 1) = (Decimal.Round(data(i, j), decimals)).ToString
                Else
                    rowElements(j - 1) = data(i, j).ToString
                End If
            Next
            elementsDGV.Rows.Add(rowElements)
        Next
        newForm.StartPosition = FormStartPosition.CenterParent
        newForm.Opacity = 2.5
        newForm.ShowDialog()
    End Sub

    Private Sub newForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles newForm.Resize
        elementsDGV.Width = newForm.Width - 20
        elementsDGV.Height = newForm.Height - 50
    End Sub

    'solves for a 4x4 matrix determinant
    Public Function determinant4x4() As Decimal
        Dim solution As Decimal
        If Me.nRows = Me.nCols And Me.nRows = 4 Then
            Dim const1 As Decimal
            For i As Integer = 1 To 4
                const1 = Me.data(1, i)
                If i Mod 2 = 0 Then
                    const1 *= -1
                End If
                Dim matrix3x3 As New Matrix(3, 3)
                For j As Integer = 1 To 3
                    Dim m As Integer = 1
                    For k As Integer = 1 To 4
                        If k <> i Then
                            matrix3x3.data(j, m) = Me.data(j + 1, k)
                            m += 1
                        End If
                    Next
                Next
                solution += matrix3x3.determinant3x3() * const1
            Next
        Else
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to get the 4x4 determinant of a non-square matrix", "Determinant Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
        End If
        Return solution
    End Function

    Public Function determinant3x3() As Decimal
        Dim solution As Decimal
        If Me.nRows = Me.nCols And Me.nRows = 3 Then
            Dim const1 As Decimal
            For i As Integer = 1 To 3
                const1 = Me.data(1, i)
                If i Mod 2 = 0 Then
                    const1 *= -1
                End If
                Dim matrix2x2 As New Matrix(2, 2)
                For j As Integer = 1 To 2
                    Dim m As Integer = 1
                    For k As Integer = 1 To 3
                        If k <> i Then
                            matrix2x2.data(j, m) = Me.data(j + 1, k)
                            m += 1
                        End If
                    Next
                Next
                solution += matrix2x2.determinant2x2 * const1
            Next
        Else
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to get the 3x3 determinant of a non-square matrix", "Determinant Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
        End If
        Return solution
    End Function

    Public Function determinant2x2() As Decimal
        Dim solution As Decimal
        If Me.nRows = Me.nCols And Me.nRows = 2 Then
            solution = Me.data(1, 1) * Me.data(2, 2) - Me.data(1, 2) * Me.data(2, 1)
        Else
            If Not Me.suppressErrorMessages Then
                MessageBox.Show("Attempted to get the 2x2 determinant of a non-square matrix", "Determinant Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                ErrorSoKill()
            End If
        End If
        Return solution
    End Function

End Class
