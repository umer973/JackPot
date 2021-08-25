﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jackport
{
    public partial class FrmWinPrice : Form
    {
        public string winPrize;
        int count = 30;
        int val = 0;
        string win = "00";
        public FrmWinPrice(string _winPrize, string drawtime)
        {
            InitializeComponent();
            winPrize = _winPrize;
            lblDrawtime.Text = drawtime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FrmWinPrice_Load(object sender, EventArgs e)
        {

            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 80; // 1 second
            timer1.Start();


        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            if (winPrize != win)
            {
                win = SpinNumbers();
                lblWinNumber.Text = win;
            }
            else

            {
                lblWinNumber.Text = winPrize;
                count--;
            }


            if (count == 0)
            {

                timer1.Stop();
                this.Hide();
            }




        }

        private string SpinNumbers()
        {
            Random rnd = new Random();

            string num = rnd.Next(0, 99).ToString();

            return num;

        }

        private void lbltime_Click(object sender, EventArgs e)
        {

        }
    }
}
