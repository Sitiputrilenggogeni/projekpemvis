Public Class Register
    Private Sub Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the form
        Label1.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Perform registration logic
        Dim username As String = TextBox1.Text
        Dim password As String = TextBox2.Text

        ' Simple validation
        If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrWhiteSpace(password) Then
            Label1.Text = "Semua kolom harus diisi."
            Label1.ForeColor = Color.Red
        Else
            ' Save user data (for simplicity, saving to a file; consider using a database)
            Dim userData As String = username & ";" & password
            My.Computer.FileSystem.WriteAllText("users.txt", userData & Environment.NewLine, True)

            ' Notify success
            Label1.Text = "Registrasi berhasil."
            Label1.ForeColor = Color.Green

            ' Close registration form and show login form
            Dim loginForm As New Form3()
            loginForm.Show()
            Me.Close()
        End If
    End Sub
End Class
