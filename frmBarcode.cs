using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OnBarcode.Barcode.WinForms;
using OnBarcode.Barcode;
using System.IO;
using System.Drawing.Printing;

namespace barCodeTest
{
    public partial class frmBarcode : Form
    {
        public frmBarcode()
        {
            InitializeComponent();
        }

        string input = "";
        string path = "E:/barcode.bmp";
        public const int maxrow = 8, maxcol = 4, maxpage = 10, xdif = 200, ydif = 150, xmargin = 25 , ymargin = 25;
        public int currow = 0, curcol = 0, curpage = 0;
        public string data = "";
        public int tot = 0, count = 0;
        public Image img;

        private void frmBarcode_Load(object sender, EventArgs e)
        {
            OnBarcode.Barcode.WinForms.LinearWinForm win = new LinearWinForm();
            win.Show();
        }

        public Image GenerateBacode(string _str)
        {
            Linear barcode = new Linear();
            barcode.Type = BarcodeType.UPCA;
            barcode.BarcodeHeight = 16;
            barcode.UOM = UnitOfMeasure.PIXEL;
            barcode.Data = _str;
            barcode.Resolution = 100;
            barcode.Rotate = Rotate.Rotate0;
            
            Image tmp = barcode.drawBarcode();
            return tmp;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            input = txtInput.Text;
            if (string.IsNullOrEmpty(input)) return;
            lbBarcode.Items.Add(txtInput.Text);
        }

        private void printDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.HasMorePages = true;
            
            int i = 0;
            for (i = curpage * 32; i < ((curpage + 1) * 32); i++)
            {
                if (tot == i) { e.HasMorePages = false; break; }
                data = lbBarcode.Items[i].ToString();
                currow = i % maxrow;
                curcol = (int) i/maxrow;
                curcol -= curpage * 4;
                img = GenerateBacode(data);
           //     e.Graphics.DrawString("Al-Amin Stationers and Traders", new Font("Serif", 10), Brushes.Black, (xdif * curcol) + xmargin, (ydif * currow) + ymargin);
                e.Graphics.DrawImage(img, new Point((xdif * curcol) + xmargin , (ydif * currow)+ ymargin));
            }
            curpage++;
            if (e.HasMorePages) e.HasMorePages = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            tot = lbBarcode.Items.Count;
        //    pntPreDia.Document = printDoc;
        //    pntPreDia.ShowDialog();
            printDoc.Print();
        }
    }
}