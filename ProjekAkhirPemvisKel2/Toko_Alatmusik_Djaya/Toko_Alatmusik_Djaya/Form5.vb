Imports MySql.Data.MySqlClient

Public Class Form5

    ' Sub ini untuk mengosongkan input
    Sub Kosong()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox1.Focus()
    End Sub

    ' Sub untuk mengisi data kategori (tidak digunakan dalam contoh ini)
    Sub isi()
        TextBox2.Clear()
        TextBox2.Focus()
    End Sub

    ' Sub untuk menampilkan data kategori dari database
    Sub tampilgerbong()
        DA = New MySqlDataAdapter("select * from tbkategori", CONN)
        DS = New DataSet
        DS.Clear()
        DA.Fill(DS, "kategori")
        DataGridView1.DataSource = DS.Tables("kategori")
        DataGridView1.Refresh()
    End Sub

    ' Sub untuk mengatur tampilan DataGridView
    Sub aturGrid()
        DataGridView1.Columns(0).Width = 100
        DataGridView1.Columns(1).Width = 200
        DataGridView1.Columns(0).HeaderText = "Kode Kategori"
        DataGridView1.Columns(1).HeaderText = "Nama Kategori"
    End Sub

    ' Event untuk form load
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksi()
        tampilgerbong()
        Kosong()
        aturGrid()
    End Sub

    ' Event untuk tombol simpan
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox2.Text = Nothing Or TextBox1.Text = Nothing Then
            MsgBox("Data Belum Lengkap")
            TextBox1.Focus()
        Else
            CMD = New MySqlCommand("Select * from tbkategori where kodekategori = '" & TextBox1.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If Not RD.HasRows Then
                RD.Close()
                CMD = New MySqlCommand("insert into tbkategori values('" & TextBox1.Text & "', '" & TextBox2.Text & "')", CONN)
                CMD.ExecuteNonQuery()
                tampilgerbong()
                Kosong()
                MsgBox("Simpan Data Sukses!")
                TextBox1.Focus()
            Else
                RD.Close()
                MsgBox("Data Tersebut Sudah Ada")
            End If
        End If
    End Sub

    ' Event untuk DataGridView CellClick
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            If row.Index < DataGridView1.RowCount - 1 And row.Index >= 0 Then
                TextBox1.Text = row.Cells(0).Value
                TextBox2.Text = row.Cells(1).Value
            End If
        End If
    End Sub

    ' Event untuk tombol ubah
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox2.Text = Nothing Then
            MsgBox("Kode Gerbong belum diisi")
            TextBox1.Focus()
        Else
            Dim ubah As String = "update tbkategori set kategori = '" &
            TextBox2.Text & "' where kodekategori = '" & TextBox1.Text & "'"
            CMD = New MySqlCommand(ubah, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("Data berhasil diubah")
            tampilgerbong()
            Kosong()
        End If
    End Sub

    ' Event untuk tombol hapus
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox1.Text = Nothing Then
            MsgBox("Kode Jenis belum diisi")
            TextBox1.Focus()
        Else
            Dim ubah As String = "delete from tbkategori where kodekategori = '" &
            TextBox1.Text & "'"
            CMD = New MySqlCommand(ubah, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("Data berhasil dihapus")
            tampilgerbong()
            Kosong()
        End If
    End Sub

    ' Event untuk tombol batal
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Kosong()
        tampilgerbong()
    End Sub

    ' Validasi input hanya angka untuk TextBox1
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        TextBox1.MaxLength = 2
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Validasi input hanya huruf untuk TextBox2
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        TextBox2.MaxLength = 50
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
        If e.KeyChar = Chr(13) Then
            TextBox2.Text = UCase(TextBox2.Text)
        End If
    End Sub

    ' Event untuk pencarian data
    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            CMD = New MySqlCommand("Select * from tbkategori where kodekategori like '%" & TextBox3.Text & "%'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()

            If RD.HasRows Then
                RD.Close()
                DA = New MySqlDataAdapter("select * from tbkategori where kodekategori like '%" & TextBox3.Text & "%'", CONN)
                DS = New DataSet
                DA.Fill(DS, "dapat")
                DataGridView1.DataSource = DS.Tables("dapat")
                DataGridView1.ReadOnly = True
            Else
                RD.Close()
                MsgBox("data tidak ditemukan")
            End If
        End If
    End Sub

    ' Event untuk tombol tutup
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()
    End Sub
End Class
