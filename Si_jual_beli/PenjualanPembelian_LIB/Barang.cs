﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.IO;
namespace PenjualanPembelian_LIB
{
    public class Barang
    {
        private string kodeBarang;
        private string barcode;
        private string nama;
        private int hargaJual;
        private int stok;
        private Kategori kategori;

        #region Properties
        public Kategori Kategori
        {
            get { return kategori; }
            set { kategori = value; }
        }
        public int Stok
        {
            get { return stok; }
            set { stok = value; }
        }
        public int HargaJual
        {
            get { return hargaJual; }
            set { hargaJual = value; }
        }
        public string Nama
        {
            get { return nama; }
            set { nama = value; }
        }
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
        public string KodeBarang
        {
            get { return kodeBarang; }
            set { kodeBarang = value; }
        }
        #endregion

        #region Constructor
        public Barang()
        {
            KodeBarang = "";
            Barcode = "";
            Nama = "";
            HargaJual = 0;
            Stok = 0;
            //kategori barang merupakan aggregation ,jadi harus diciptakan(di-new) diluar class
        }
        public Barang(string pKode, string pBarcode, string pNama, int pHargaJual, int pStok, Kategori pKategori) {
            KodeBarang = pKode;
            Barcode = pBarcode;
            Nama = pNama;
            HargaJual = pHargaJual;
            Stok = pStok;
            Kategori = pKategori;
        }
        #endregion

        #region Method
        public static string TambahData(Barang pBarang) {
            string sql = "INSERT INTO barang(KodeBarang, Barcode, Nama, HargaJual, Stok, KodeKategori) VALUES ('" + pBarang.KodeBarang + "','" 
                + pBarang.Barcode.Replace("'", "\\'")+ "','" + pBarang.Nama.Replace("'", "\\'") + "'," + pBarang.HargaJual
                + "," + pBarang.Stok + ",'" + pBarang.Kategori.KodeKategori + "')";
            try {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch(MySqlException ex){
                return ex.Message + ". Perintah SQL : " + sql;
            }
        }

        public static string UbahData(Barang pBarang)
        {
            string sql = "UPDATE barang SET Barcode = '" + pBarang.Barcode + "', Nama = '" + pBarang.Nama.Replace("'", "\\'") + "', HargaJual = '" + pBarang.HargaJual + "' WHERE KodeBarang = '" + pBarang.KodeBarang + "'";

            try
            {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch (MySqlException ex)
            {
                return ex.Message + ". Perintah sql : " + sql;
            }
        }

        public static string HapusData(Barang pBarang)
        {
            string sql = "DELETE FROM barang WHERE KodeBarang = '" + pBarang.KodeBarang + "'";

            try
            {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch (MySqlException ex)
            {
                return ex.Message + ". Perintah sql : " + sql;
            }
        }

        public static string GenerateKode (string pKodeKategori, out string pHasilKode) {
            //format kode : xxyyy (xx : kode kategori, yyy : nomor urut)
            //untuk membuat no urut maka dapatkan kode barang terakhir (terbesar) dari kode kategori tertentu
            string sql = "SELECT MAX(RIGHT(KodeBarang,3)) FROM barang WHERE KodeKategori = '" + pKodeKategori + "'";
            pHasilKode = "";
            try
            {
                MySqlDataReader hasildata = Koneksi.JalankanPerintahQuery(sql);

                if (hasildata.Read() == true)
                { //jika berhasil membaca data (ditemukan data di tabel barang)
                    if (hasildata.GetValue(0).ToString() != "")
                    {
                        int kodeTerbaru = int.Parse(hasildata.GetValue(0).ToString()) + 1;

                        //gunakan PadLeft untuk menambahkan nol didepan kode.
                        //contoh : kode 1 --> 001, 11 --> 011, 111 --> 111
                        pHasilKode = pKodeKategori + kodeTerbaru.ToString().PadLeft(3, '0');
                    }
                    else
                    {
                        //jika ditemukan data dengan kategori tertentu maka kode terbaru = "001"
                        pHasilKode = pKodeKategori + "001";
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string BacaData(string kriteria, string nilaiKriteria, List<Barang> listHasilData)
        {
            string sql = "";

            //jika tidak ada kriteria yang diisikan 
            if (kriteria == "")
            {
                sql = "SELECT B.KodeBarang, B.Barcode, B.Nama, B.HargaJual, B.Stok, B.KodeKategori, K.Nama AS NamaKategori FROM barang B INNER JOIN kategori K ON B.KodeKategori = K.KodeKategori";
            }
            else
            {
                sql = "SELECT B.KodeBarang, B.Barcode, B.Nama, B.HargaJual, B.Stok, B.KodeKategori, K.Nama AS NamaKategori FROM barang B INNER JOIN kategori K ON B.KodeKategori = K.KodeKategori WHERE " + kriteria + " LIKE '%" + nilaiKriteria + "%'";
            }
            try
            {
                MySqlDataReader hasilData = Koneksi.JalankanPerintahQuery(sql);
                listHasilData.Clear();//kosongi isi list dulu
                while (hasilData.Read() == true)//selama masih ada data
                {
                    Barang brg = new Barang();
                    brg.KodeBarang = hasilData.GetValue(0).ToString();
                    brg.Barcode = hasilData.GetValue(1).ToString();
                    brg.Nama = hasilData.GetValue(2).ToString();
                    brg.HargaJual = int.Parse(hasilData.GetValue(3).ToString());
                    brg.Stok = int.Parse(hasilData.GetValue(4).ToString());
                    Kategori kat = new Kategori(hasilData.GetValue(5).ToString(), hasilData.GetValue(6).ToString());
                    brg.Kategori = kat;
                    listHasilData.Add(brg);
                }
                return "1";
            }
            catch (MySqlException ex)
            {
                return ex.Message + ". Perintah sql : " + sql;
            }
        }
        public static string UbahStokTerjual(String pKodeBarang, int pJumlahTerjual) {
            //tuliskan perintah sql yang akan dijalankan - mengupdate/mengurangi stok barang yg terjual
            string sql = "UPDATE barang SET Stok = Stok - " + pJumlahTerjual + " WHERE KodeBarang = '" + pKodeBarang + "'";

            try
            {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch (Exception ex){
                return ex.Message;

            }
        }
        public static string UbahStokTerbeli(String pKodeBarang, int pJumlahTerbeli)
        {
            //tuliskan perintah sql yang akan dijalankan - mengupdate/mengurangi stok barang yg terjual
            string sql = "UPDATE barang SET Stok = Stok + " + pJumlahTerbeli + " WHERE KodeBarang = '" + pKodeBarang + "'";

            try
            {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        public static string CetakNota(string pKriteria, string pNilaiKriteria, string pNamaFile)
        {
            try
            {
                List<Barang> listBarang = new List<Barang>();

                //baca data nota tertentu yang akan dicetak
                string hasilBaca = Barang.BacaData(pKriteria, pNilaiKriteria, listBarang);

                //simpan dulu isi nota yang akan ditampilkan ke objek file (streamwriter)
                StreamWriter file = new StreamWriter(pNamaFile);
                for (int i = 0; i < listBarang.Count; i++)
                {
                    file.WriteLine("|{0,5} | {1,5} | {2,0} | {3,5} | {4,3} | {5,5}|", listBarang[i].kodeBarang, listBarang[i].barcode, listBarang[i].Nama, listBarang[i].HargaJual, listBarang[i].Stok, listBarang[i].Kategori.KodeKategori);
                }
                file.Close();
                //cetak ke printer
                Cetak c = new Cetak(pNamaFile, "Courier New", 9, 10, 10, 10, 10);
                c.CetakKePrinter("tulisan");
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}

