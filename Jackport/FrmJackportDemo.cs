﻿using Jackport.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jackport
{
    public partial class FrmJackportDemo : Form
    {
        List<ListValueControl> products;
        List<UserInputControl> InputBox;
        ClsService clsService;
        public string agentToken;
        public string slotdId;
        public string ticketNo;
        public int qty;
        public int currentSlot = 1;

        List<Bid> bidList = new List<Bid>();
        List<PurchaseTicket> plist = new List<PurchaseTicket>();
        private int count = 360;
        int segundo = 360;
        DateTime dt = new DateTime();
        List<TimeSlot> timeSlots = new List<TimeSlot>();

        public FrmJackportDemo(Root data)
        {
            FrmLogin objLogin = new FrmLogin();
            objLogin.Hide();
            clsService = new ClsService();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            InitializeComponent();
            LoadProduct();
            LblDate.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");

            LblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            SetData(data);

            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();

            var list = data.data.TimeSlots.Where(x => x.slot_over == "0").ToList();

            timeSlots = list.Select(x => new TimeSlot
            {
                slot_id = x.slot_id
            }).ToList();

            cmbSlot.Text = "Current";

            // GetSlot(list);


        }
        //private void GetSlot(IEnumerable<TimeSlot> list)
        //{
        //    if (cmbSlot.SelectedText.Contains("Current"))
        //    {
        //        currentSlot = 1;
        //    }
        //    else if (cmbSlot.SelectedText.Contains("5"))
        //    {
        //        currentSlot = 5;
        //    }

        //    var slot = list.Select(x => new PurchaseTicket
        //    {
        //        slot_id = Convert.ToInt16(x.slot_id)
        //    }).ToList();

        //    for (int i = 0; i < currentSlot; i++)
        //    {
        //        var sl = new PurchaseTicket
        //        {
        //            slot_id = slot[i].slot_id
        //        };
        //        plist.Add(sl);
        //    }


        //}


        private void LoadProduct()
        {
            for (int i = 00; i < 100; i++)
            {
                UserInputControl p1 = new UserInputControl();
                string num = Convert.ToString(i);
                if (i == 0)
                {
                    num = Convert.ToString(00);
                }
                if (i == 1)
                {
                    num = "01";
                }
                if (i == 2)
                {
                    num = "02";
                }
                if (i == 3)
                {
                    num = "03";
                }
                if (i == 4)
                {
                    num = "04";
                }
                if (i == 5)
                {
                    num = "05";
                }
                if (i == 6)
                {
                    num = "06";
                }
                if (i == 7)
                {
                    num = "07";
                }
                if (i == 8)
                {
                    num = "08";
                }
                if (i == 9)
                {
                    num = "09";
                }


                p1.Tag = i;
                //p1.TickeName = Convert.ToString(i);
                p1.TickeName = Convert.ToString(num);
                flowLayoutPanel2.Controls.Add(p1);

            }






        }

        private void SetData(Root _root)
        {
            try
            {
                LblAgentId.Text = _root.data.AgentData.agent_code;
                LblBalance.Text = _root.data.AgentData.balance;
                LblCompanyName.Text = _root.data.ApplicationDetails.app_name;
                agentToken = _root.data.AgentData.token;

                UserAgent.AgenToken = agentToken;



                loadWinPrizes(_root.data.TimeSlots);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadWinPrizes(List<TimeSlot> timeSlotList)
        {

            var _timeSlot = timeSlotList.Select(x => new ListValueControl()
            {
                Name = x.win_number,
                Time = x.time_end,
                Color = x.slot_over.ToString().Trim() == "1" ? Color.Red : Color.Blue
            });

            foreach (ListValueControl item in _timeSlot)
            {
                flowLayoutPanel1.Controls.Add(item);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void FrmJackport_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            BuyTickets();
           
        }

        private void BuyTickets()
        {
            int flag = 0;
            GetSlots();
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {

                if (!string.IsNullOrEmpty(ctr.TickeQty) && Convert.ToInt32(ctr.TickeQty) > 0)
                {
                    flag = 1;
                    var bids = new Bid
                    {
                        quantity = Convert.ToInt16(ctr.TickeQty),
                        number = Convert.ToInt32(ctr.TickeName)

                    };
                    bidList.Add(bids);
                }
            }

            if (flag == 1)
            {
                var result = clsService.PurchaseSingleTicketAsync(agentToken, bidList, plist);
                LblBalance.Text = result.ToString();
                ClearBoard();
                Print(result);
            }
            else
            {
                MessageBox.Show("Please select ticket first");
            }
        }

        private void Print(object result)
        {
            //throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearBoard();
        }

        public void ClearBoard()
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                ctr.TickeQty = "";


            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            count--;
            if (count == 0)
                timer1.Stop();
            //LblCountDown1.Text = counter.ToString();
            LblCountDown1.Text = "00" + ":" + count / 60 + ":" + ((count % 60) >= 10 ? (count % 60).ToString() : "0" + (count % 60));
            //label1.Text = count / 60 + ":" + ((count % 60) >= 10 ? (count % 60).ToString() : "0" + (count % 60));
            int LeftTime1 = count;
            int LeftTime2 = segundo - LeftTime1;
            int LeftTime = segundo - LeftTime2;
            LblCountDown2.Text = LeftTime / 60 + ":" + ((LeftTime % 60) >= 10 ? (LeftTime % 60).ToString() : "0" + (LeftTime % 60));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<PurchasedTickets> purchasetikcet = new List<PurchasedTickets>();
            purchasetikcet = clsService.GetTodaysPurchasedTickets(agentToken);

            FrmBarcode ObjFrmBarcode = new FrmBarcode(purchasetikcet);
            ObjFrmBarcode.Show();
        }

        private void TxtE0_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE2_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE3_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE5_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE7_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE8_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE9_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtE0_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "0")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "10")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "20")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "30")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "40")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "50")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "60")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "70")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "80")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else if (Data == "90")
                {
                    ctr.TickeQty = TxtE0.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE1_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "1")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "11")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "21")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "31")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "41")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "51")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "61")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "71")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "81")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else if (Data == "91")
                {
                    ctr.TickeQty = TxtE1.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE2_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "2")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "12")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "22")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "32")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "42")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "52")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "62")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "72")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "82")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else if (Data == "92")
                {
                    ctr.TickeQty = TxtE2.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE3_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "3")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "13")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "23")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "33")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "43")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "53")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "63")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "73")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "83")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else if (Data == "93")
                {
                    ctr.TickeQty = TxtE3.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE4_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "4")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "14")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "24")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "34")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "44")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "54")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "64")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "74")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "84")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else if (Data == "94")
                {
                    ctr.TickeQty = TxtE4.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "5")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "15")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "25")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "35")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "45")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "55")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "65")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "75")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "85")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else if (Data == "95")
                {
                    ctr.TickeQty = textBox6.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE5_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "6")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "16")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "26")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "36")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "46")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "56")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "66")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "76")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "86")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else if (Data == "96")
                {
                    ctr.TickeQty = TxtE5.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE7_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "7")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "17")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "27")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "37")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "47")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "57")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "67")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "77")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "87")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "97")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE8_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "8")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "18")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "28")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "38")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "48")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "58")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "68")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "78")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else if (Data == "88")
                {
                    ctr.TickeQty = TxtE7.Text;
                }
                else if (Data == "98")
                {
                    ctr.TickeQty = TxtE8.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void TxtE9_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "9")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "19")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "29")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "39")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "49")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "59")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "69")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "79")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "89")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else if (Data == "99")
                {
                    ctr.TickeQty = TxtE9.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt0009_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "0")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "1")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "2")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "3")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "4")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "5")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "6")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "7")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "8")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else if (Data == "9")
                {
                    ctr.TickeQty = Txt0009.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt1019_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "10")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "11")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "12")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "13")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "14")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "15")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "16")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "17")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "18")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else if (Data == "19")
                {
                    ctr.TickeQty = Txt1019.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt2029_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "20")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "21")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "22")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "23")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "24")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "25")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "26")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "27")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "28")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "29")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt3031_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "30")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "31")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "32")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "33")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "34")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "35")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "36")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "37")
                {
                    ctr.TickeQty = Txt2029.Text;
                }
                else if (Data == "38")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "39")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt4049_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "40")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "41")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "42")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "43")
                {
                    ctr.TickeQty = Txt3031.Text;
                }
                else if (Data == "44")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "45")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "46")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "47")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "48")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else if (Data == "49")
                {
                    ctr.TickeQty = Txt4049.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "50")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "51")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "52")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "53")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "54")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "55")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "56")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "57")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "58")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else if (Data == "59")
                {
                    ctr.TickeQty = textBox5.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt5051_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "60")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "61")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "62")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "63")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "64")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "65")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "66")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "67")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "68")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else if (Data == "69")
                {
                    ctr.TickeQty = Txt5051.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt7079_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "70")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "71")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "72")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "73")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "74")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "75")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "76")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "77")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "78")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else if (Data == "79")
                {
                    ctr.TickeQty = Txt7079.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt8089_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "80")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "81")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "82")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "83")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "84")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "85")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "86")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "87")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "88")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else if (Data == "89")
                {
                    ctr.TickeQty = Txt8089.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void Txt9099_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (UserInputControl ctr in flowLayoutPanel2.Controls)
            {
                string Data = Convert.ToString(ctr.Tag);
                if (Data == "90")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "91")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "92")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "93")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "94")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "95")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "96")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "97")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "98")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else if (Data == "99")
                {
                    ctr.TickeQty = Txt9099.Text;
                }
                else
                {
                    ctr.TickeQty = "";
                }



            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var result = clsService.GetTodaysPurchasedTickets(agentToken);
            FrmTMLPrint objReport = new FrmTMLPrint(result);
            objReport.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void cmbSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSlots();
        }

        private void GetSlots()
        {


            if (cmbSlot.Text.Trim() == "Current")
            {
                currentSlot = 1;
            }
            else if (cmbSlot.Text.Trim() == "NEXT 5")
            {
                currentSlot = 5;
            }
            else if (cmbSlot.Text.Trim() == "NEXT 15")
            {
                currentSlot = 15;
            }
            else if (cmbSlot.Text.Trim() == "NEXT 20")
            {
                currentSlot = 20;
            }
            else if (cmbSlot.Text.Trim() == "ALL DRAW")
            {
                currentSlot = timeSlots.Count;
            }

            var slot = timeSlots.Select(x => new PurchaseTicket
            {
                slot_id = Convert.ToInt16(x.slot_id)
            }).ToList();

            for (int i = 0; i < currentSlot; i++)
            {
                var sl = new PurchaseTicket
                {
                    slot_id = slot[i].slot_id
                };
                plist.Add(sl);
            }
        }
    }
}
