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

namespace PenjualanPembelian_LIB
{
    public class NotaJualDetil
    {
        public virtual int Jumlah
        {
            get;
            set;
        }

        public virtual int Harga
        {
            get;
            set;
        }

        public virtual Barang Barang
        {
            get;
            set;
        }
        #region Constructor
        public NotaJualDetil() {
            Jumlah = 1;
            Harga = 0;
        }
        public NotaJualDetil(Barang pBarang, int pHarga, int pJumlah) {
            Barang = pBarang;
            Harga = pHarga;
            Jumlah = pJumlah;
        }
        #endregion
    }
}
