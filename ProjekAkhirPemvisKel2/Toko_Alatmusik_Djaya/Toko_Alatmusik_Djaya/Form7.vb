Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing

Public Class Form7
    Dim pageWidth As Integer = 827
    Dim pageHeight As Integer = 1169
    Dim listpesanan As New List(Of List(Of String))()
    Dim currentPage, totalPage, totalItem, marginPixels, y, x, marginRight
    Dim marginInch As Single

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        koneksi()
        tampildata()
        FillComboBoxKategori() ' Panggil FillComboBoxKategori saat Form dibuka
        AddHandler DataGridView1.SelectionChanged, AddressOf DataGridView1_SelectionChanged
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Sub tampildata()
        Try
            ' Query untuk mengambil data dari tbpesan
            Dim query As String = "SELECT nama, nohp, kategori, alat, harga FROM tbpesan"
            DA = New MySqlDataAdapter(query, CONN)
            DS = New DataSet()
            DA.Fill(DS, "tbpesan")
            ' Menampilkan data di DataGridView
            DataGridView1.DataSource = DS.Tables("tbpesan")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CONN.Close()
        End Try
    End Sub

    Private Sub cbkategori_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbkategori.SelectedIndexChanged
        cbalat.Items.Clear()
        FillComboAlat()
    End Sub

    Private Sub FillComboBoxKategori()
        ' Panggil fungsi koneksi dari modul
        koneksi()
        Try
            Dim query As String = "SELECT DISTINCT kategori FROM tbalmusik"
            CMD = New MySqlCommand(query, CONN)
            RD = CMD.ExecuteReader()

            cbkategori.Items.Clear()

            ' Isi ComboBox dengan data dari database
            While RD.Read()
                cbkategori.Items.Add(RD("kategori").ToString())
            End While

            ' Tutup koneksi dan tutup DataReader
            RD.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CONN.Close()
        End Try
    End Sub

    Private Sub FillComboAlat()
        ' Panggil fungsi koneksi dari modul
        koneksi()
        Try
            If cbkategori.SelectedItem IsNot Nothing Then ' Pastikan kategori dipilih sebelum mengisi alat
                Dim selectedkategori As String = cbkategori.SelectedItem.ToString()
                Dim query As String = "SELECT nama FROM tbalmusik WHERE kategori = @kategori"
                CMD = New MySqlCommand(query, CONN)
                CMD.Parameters.AddWithValue("@kategori", selectedkategori)
                RD = CMD.ExecuteReader()

                cbalat.Items.Clear() ' Bersihkan item sebelum menambahkan yang baru

                While RD.Read()
                    cbalat.Items.Add(RD("nama").ToString())
                End While

                ' Tutup koneksi dan tutup DataReader
                RD.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CONN.Close()
        End Try
    End Sub

    Private Sub DisplayHarga()
        ' Panggil fungsi koneksi dari modul
        koneksi()
        Try
            Dim selectedKategori As String = cbkategori.SelectedItem.ToString()
            Dim selectedAlat As String = cbalat.SelectedItem.ToString()
            Dim query As String = "SELECT harga FROM tbalmusik WHERE kategori = @kategori AND nama = @nama"
            CMD = New MySqlCommand(query, CONN)
            CMD.Parameters.AddWithValue("@kategori", selectedKategori)
            CMD.Parameters.AddWithValue("@nama", selectedAlat)
            RD = CMD.ExecuteReader()

            ' Kosongkan cbharga sebelum menambah item baru
            cbharga.Items.Clear()

            ' Tampilkan harga di ComboBox
            If RD.Read() Then
                cbharga.Items.Add(RD("harga").ToString())
                cbharga.SelectedIndex = 0 ' Pilih item pertama secara otomatis
            End If

            ' Tutup koneksi dan tutup DataReader
            RD.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CONN.Close()
        End Try
    End Sub

    Private Sub cbalat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbalat.SelectedIndexChanged
        DisplayHarga()
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        koneksi()
        Try
            ' Cek apakah data sudah ada di tbpesan
            CMD = New MySqlCommand("SELECT * FROM tbpesan WHERE nama = @nama", CONN)
            CMD.Parameters.AddWithValue("@nama", txtnama.Text)
            RD = CMD.ExecuteReader()
            RD.Read()

            If Not RD.HasRows Then
                RD.Close()
                CMD = New MySqlCommand("INSERT INTO tbpesan (nama, nohp, kategori, alat, harga) VALUES (@nama, @nohp, @kategori, @alat, @harga)", CONN)
                CMD.Parameters.AddWithValue("@nama", txtnama.Text)
                CMD.Parameters.AddWithValue("@nohp", txtHP.Text)
                CMD.Parameters.AddWithValue("@kategori", cbkategori.Text)
                CMD.Parameters.AddWithValue("@alat", cbalat.Text)
                CMD.Parameters.AddWithValue("@harga", cbharga.Text)
                CMD.ExecuteNonQuery()
                MsgBox("Simpan Data Sukses!")
                tampildata() ' Perbarui DataGridView setelah menyimpan data
                txtnama.Focus()
            Else
                RD.Close()
                MsgBox("Data Tersebut Sudah Ada")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CONN.Close()
        End Try
    End Sub

    Private Sub cbharga_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbharga.SelectedIndexChanged
        ' Event handler kosong; hapus jika tidak digunakan
    End Sub

    Private Sub btnUbah_Click(sender As Object, e As EventArgs) Handles btnUbah.Click
        koneksi()
        Try
            ' Pastikan ada baris yang dipilih di DataGridView
            If DataGridView1.SelectedRows.Count > 0 Then
                ' Dapatkan data dari baris yang dipilih
                Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
                Dim namaAwal As String = selectedRow.Cells("nama").Value.ToString() ' Menggunakan nama sebagai acuan
                Dim nama As String = txtnama.Text
                Dim nohp As String = txtHP.Text
                Dim kategori As String = cbkategori.Text
                Dim alat As String = cbalat.Text
                Dim harga As String = cbharga.Text

                ' Query untuk update data
                Dim query As String = "UPDATE tbpesan SET nama = @nama, nohp = @nohp, kategori = @kategori, alat = @alat, harga = @harga WHERE nama = @namaAwal"
                CMD = New MySqlCommand(query, CONN)
                CMD.Parameters.AddWithValue("@nama", nama)
                CMD.Parameters.AddWithValue("@nohp", nohp)
                CMD.Parameters.AddWithValue("@kategori", kategori)
                CMD.Parameters.AddWithValue("@alat", alat)
                CMD.Parameters.AddWithValue("@harga", harga)
                CMD.Parameters.AddWithValue("@namaAwal", namaAwal)
                CMD.ExecuteNonQuery()

                ' Tampilkan pesan sukses
                MessageBox.Show("Data berhasil diubah!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Refresh data di DataGridView
                tampildata()
            Else
                MessageBox.Show("Pilih baris data yang ingin diubah.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CONN.Close()
        End Try
    End Sub

    Private Sub btnHapus_Click(sender As Object, e As EventArgs) Handles btnHapus.Click
        koneksi()
        Try
            ' Pastikan ada baris yang dipilih di DataGridView
            If DataGridView1.SelectedRows.Count > 0 Then
                ' Dapatkan data dari baris yang dipilih
                Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
                Dim nama As String = selectedRow.Cells("nama").Value.ToString() ' Menggunakan nama sebagai acuan

                ' Konfirmasi penghapusan
                Dim dialogResult As DialogResult = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If dialogResult = dialogResult.Yes Then
                    ' Query untuk menghapus data
                    Dim query As String = "DELETE FROM tbpesan WHERE nama = @nama"
                    CMD = New MySqlCommand(query, CONN)
                    CMD.Parameters.AddWithValue("@nama", nama)
                    CMD.ExecuteNonQuery()

                    ' Tampilkan pesan sukses
                    MessageBox.Show("Data berhasil dihapus!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' Refresh data di DataGridView
                    tampildata()
                End If
            Else
                MessageBox.Show("Pilih baris data yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            CONN.Close()
        End Try
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs)
        ' Pastikan ada baris yang dipilih di DataGridView
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Dapatkan data dari baris yang dipilih
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            txtnama.Text = selectedRow.Cells("nama").Value.ToString()
            txtHP.Text = selectedRow.Cells("nohp").Value.ToString()
            cbkategori.Text = selectedRow.Cells("kategori").Value.ToString()
            cbalat.Text = selectedRow.Cells("alat").Value.ToString()
            cbharga.Text = selectedRow.Cells("harga").Value.ToString()
        End If
    End Sub
    Private Sub readDatapesanan()
        koneksi()
        CMD = New MySqlCommand("select * from tbpesan",
        CONN)
        RD = CMD.ExecuteReader
        totalItem = 0
        Do While RD.Read
            Dim datapesanan As New List(Of String)()
            datapesanan.Add(RD("nama").ToString)
            datapesanan.Add(RD("nohp").ToString)
            datapesanan.Add(RD("kategori").ToString)
            datapesanan.Add(RD("alat").ToString)
            datapesanan.Add(RD("harga").ToString)
            listpesanan.Add(datapesanan)
            totalItem += 1
        Loop
        totalPage = Math.Ceiling(totalItem / 3)
        RD.Close()
    End Sub

    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        readDatapesanan()
        currentPage = 1
        PrintDocument1.Print()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim Fheader As New Font("Times New Roman", 24, FontStyle.Bold)
        Dim FBodyB As New Font("Times New Roman", 14, FontStyle.Bold)
        Dim FBody As New Font("Times New Roman", 14, FontStyle.Regular)

        Dim black As SolidBrush = New SolidBrush(Color.Black)
        Dim center As New StringFormat()
        center.Alignment = StringAlignment.Center
        Dim posY, i As Integer
        Dim hitung As Integer = 0
        If currentPage <= 1 Then
            marginInch = 2.54F
            marginPixels = CInt(e.PageSettings.PrinterResolution.X * marginInch) '
            e.PageSettings.Margins = New Margins(marginPixels, marginPixels, marginPixels, marginPixels)
            x = e.MarginBounds.Left
            y = e.MarginBounds.Top
            marginRight = e.MarginBounds.Right
            'judul
            e.Graphics.DrawString("Data Pesanan", Fheader, black, pageWidth /
            2, y, center)
            posY = y + 70
        Else
            posY = y
        End If
        e.Graphics.DrawLine(Pens.Black, x, posY, marginRight, posY)
        For i = (currentPage - 1) * 3 To totalItem - 1
            e.Graphics.DrawString("Nama", FBody, black, x + 20, posY +
            30)
            e.Graphics.DrawString(": " & listpesanan(i)(0).ToString, FBody, black, x + 200, posY + 30)
            e.Graphics.DrawString("No HP", FBody, black, x + 20, posY + 60)
            e.Graphics.DrawString(": " & listpesanan(i)(1).ToString, FBody, black, x + 200, posY + 60)
            e.Graphics.DrawString("Kategori", FBody, black, x + 20, posY + 90)
            e.Graphics.DrawString(": " & listpesanan(i)(2).ToString, FBody, black, x + 200, posY + 90)
            e.Graphics.DrawString("Alat", FBody, black, x + 20, posY + 120)
            e.Graphics.DrawString(": " & listpesanan(i)(3).ToString, FBody, black, x + 200, posY + 120)
            e.Graphics.DrawString("Harga", FBody, black, x + 20, posY + 150)
            e.Graphics.DrawString(": " & listpesanan(i)(4).ToString, FBody, black, x + 200, posY + 150)
            e.Graphics.DrawLine(Pens.Black, x, posY + 280, marginRight, posY + 300)
            posY += 270
            hitung += 1
            If hitung >= 3 Then
                Exit For
            End If
        Next
        If currentPage <= 1 Then
            e.Graphics.DrawLine(Pens.Black, x, y + 70, x, posY + 10)
            e.Graphics.DrawLine(Pens.Black, marginRight, y + 70,
            marginRight, posY + 10)
        Else
            e.Graphics.DrawLine(Pens.Black, x, y, x, posY + 10)
            e.Graphics.DrawLine(Pens.Black, marginRight, y, marginRight,
            posY + 10)
        End If
        currentPage += 1
        e.HasMorePages = currentPage <= totalPage
    End Sub
End Class
