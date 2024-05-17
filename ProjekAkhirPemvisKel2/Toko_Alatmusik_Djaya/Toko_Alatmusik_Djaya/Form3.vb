Public Class Form3
    Dim x, y As Integer
    Dim newpoint As New System.Drawing.Point

    Private Sub Panel2_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel2.MouseDown
        x = Control.MousePosition.X - Me.Location.X
        y = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub Panel2_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel2.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= (x)
            newpoint.Y -= (y)
            Me.Location = newpoint
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel2.Left = Me.Width
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Panel2.Left > 12 Then
            Panel2.Left = Int(Panel2.Left - 20)
        Else
            Panel2.Left = 12
        End If
    End Sub

    Private Sub login_berhasil()
        Form4.Show()
    End Sub

    Private Sub login_berhasil2()
        Form6.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = user.Text
        Dim password As String = Me.password.Text

        If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrWhiteSpace(password) Then
            MessageBox.Show("Semua kolom harus diisi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Admin login
        If username = "admin" AndAlso password = "admin" Then
            MessageBox.Show("LOGIN BERHASIL", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            login_berhasil()
            Me.Hide()
            Return
        End If

        ' Membaca data pengguna dari file
        If My.Computer.FileSystem.FileExists("users.txt") Then
            Dim userData As String() = My.Computer.FileSystem.ReadAllText("users.txt").Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            Dim validCredentials As Boolean = False

            For Each userRecord In userData
                Dim credentials As String() = userRecord.Split(";"c)
                If credentials(0) = username AndAlso credentials(1) = password Then
                    validCredentials = True
                    Exit For
                End If
            Next

            If validCredentials Then
                MessageBox.Show("LOGIN BERHASIL", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                login_berhasil2()
                Me.Hide() ' Menyembunyikan form setelah login berhasil
            Else
                MessageBox.Show("Nama pengguna atau kata sandi salah.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("Tidak ada pengguna yang terdaftar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim registerForm As New Register()
        registerForm.Show()
        Me.Hide()
    End Sub

End Class
