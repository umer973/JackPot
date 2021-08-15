﻿using Jackport.DataModel;
using Jackport.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jackport.Helper
{
    public static class PrintJobHelper
    {
        private static TimeSlotList _ticket;

        private static CancelledTicket cticket;
        private static ReportSummary _report;
        private static PrintDocument printDocument;

        private static Graphics graphics;

        private static PrintPreviewDialog p = new PrintPreviewDialog();


        private static int InitialHeight = 360;

        static PrintJobHelper()
        {
            CommonHelper.ReadXMlData();
            AdjustHeight();
            printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = PrintJobSettings.PrinterName;


        }


        #region :: Print Formating

        public static void DrawAtStart(string text, int Offset)
        {
            int startX = 10;
            int startY = 5;
            System.Drawing.Font minifont = new Font("Arial", 5);

            graphics.DrawString(text, minifont,
                     new SolidBrush(Color.Black), startX + 5, startY + Offset);
        }
        public static void InsertItem(string key, string value, int Offset)
        {
            Font minifont = new Font("Arial", 5);
            int startX = 10;
            int startY = 5;

            graphics.DrawString(key, minifont,
                         new SolidBrush(Color.Black), startX + 5, startY + Offset);

            graphics.DrawString(value, minifont,
                     new SolidBrush(Color.Black), startX + 130, startY + Offset);
        }
        public static void InsertHeaderStyleItem(string key, string value, int Offset)
        {
            int startX = 10;
            int startY = 5;
            Font itemfont = new Font("Arial", 6, FontStyle.Bold);

            graphics.DrawString(key, itemfont,
                         new SolidBrush(Color.Black), startX + 5, startY + Offset);

            graphics.DrawString(value, itemfont,
                     new SolidBrush(Color.Black), startX + 130, startY + Offset);
        }
        public static void DrawLine(string text, Font font, int Offset, int xOffset)
        {
            int startX = 10;
            int startY = 5;
            graphics.DrawString(text, font,
                     new SolidBrush(Color.Black), startX + xOffset, startY + Offset);
        }
        public static void DrawSimpleString(string text, Font font, int Offset, int xOffset)
        {
            int startX = 10;
            int startY = 5;
            graphics.DrawString(text, font,
                     new SolidBrush(Color.Black), startX + xOffset, startY + Offset);
        }

        #endregion

        private static void AdjustHeight()
        {
            //var capacity = 5 * order.ItemTransactions.Capacity;
            //InitialHeight += capacity;

            //capacity = 5 * order.DealTransactions.Capacity;
            //InitialHeight += capacity;
        }

        public static void Print(TimeSlotList ticket)
        {

            _ticket = ticket;
            printDocument.PrintPage += new PrintPageEventHandler(PrintTicket);
            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(PrintJobSettings.PaperSize, PrintJobSettings.Width, PrintJobSettings.Height);
            p.Document = printDocument;

            if (PrintJobSettings.IsDirectPrint)
            {
                printDocument.Print();
            }
            else

            {
                p.ShowDialog();
            }

        }

        public static void PrintCancelledTicket(CancelledTicket ticket)
        {


            cticket = ticket;
            printDocument.PrintPage += new PrintPageEventHandler(PrintCancelledTicket);
            PrintPreviewDialog p = new PrintPreviewDialog();
            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(PrintJobSettings.PaperSize, PrintJobSettings.Width, PrintJobSettings.Height);

            p.Document = printDocument;


            if (PrintJobSettings.IsDirectPrint)
            {
                printDocument.Print();
            }
            else

            {
                p.ShowDialog();
            }




        }


        public static void PrintReportSummary(ReportSummary report)
        {

            _report = report;

            printDocument.PrintPage += new PrintPageEventHandler(PrintReportSummary);
            PrintPreviewDialog p = new PrintPreviewDialog();
            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(PrintJobSettings.PaperSize, PrintJobSettings.Width, PrintJobSettings.Height);

            p.Document = printDocument;


            if (PrintJobSettings.IsDirectPrint)
            {
                printDocument.Print();
            }
            else

            {
                p.ShowDialog();
            }




        }

        private static void PrintTicket(object sender, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            Font minifont = new Font("Arial", 5);
            Font itemfont = new Font("Arial", 6);
            Font smallfont = new Font("Arial", 8);
            Font mediumfont = new Font("Arial", 10);
            Font largefont = new Font("Arial", 12);
            int Offset = 10;
            int smallinc = 10, mediuminc = 12, largeinc = 15;



            Offset = Offset + largeinc + 10;

            String underLine = "-------------------------------------";
            //DrawLine(underLine, largefont, Offset, 0);

            //Offset = Offset + mediuminc;
            InsertHeaderStyleItem(_ticket.agent_code + "   " + _ticket.ticket_barcode, "", Offset);

            Offset = Offset + mediuminc;
            //InsertItem("BARCODE     :  " + _ticket.ticket_barcode, "", Offset);

            Offset = Offset + mediuminc;

            //  Image image = Resources._33_337047_sell_icon_png;
            // e.Graphics.DrawImage(image, 10 + 50, 5 + Offset, 100, 30);
            // InsertItem(" ", "", Offset);

            // Offset = Offset + mediuminc;

            //DrawAtStart("STARTDIGIT: " + "2", Offset);
            //Offset = Offset + mediuminc;

            Offset = Offset + largeinc;
            //InsertItem("JACKPOT", "", Offset);
            InsertHeaderStyleItem("JACKPOT STARTDIGIT", "", Offset);


            // DrawSimpleString("JACKPOT", minifont, Offset, 15);

            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Date : " + _ticket.date_slot + "  Time :  " + CommonHelper.GetdateFormat(_ticket.time_end).ToString(), "", Offset);



            //InsertHeaderStyleItem("No - Qty", "", Offset);


            int total = 0;
            string bids = string.Empty;
            int count = _ticket.bids.Count;

            for (int i = 0; i < _ticket.bids.Count;)
            {
                if (count >= 3)
                {
                    bids = "";
                    for (int j = 0; j <= 2; j++)
                    {


                        bids = bids + _ticket.bids[i].number.ToString() + "-  " + _ticket.bids[i].quantity.ToString() + "  ";

                        total = total + Convert.ToInt16(_ticket.bids[i].quantity);
                        i++;
                        count--;
                    }

                }
                else if (count >= 2)
                {
                    bids = "";
                    for (int j = 0; j <= 1; j++)
                    {


                        bids = bids + _ticket.bids[i].number.ToString() + "-  " + _ticket.bids[i].quantity.ToString() + "  ";

                        total = total + Convert.ToInt16(_ticket.bids[i].quantity);
                        i++;
                        count--;
                    }

                }
                else if (count >= 1)
                {
                    bids = "";
                    bids = _ticket.bids[i].number.ToString() + "-  " + _ticket.bids[i].quantity.ToString() + "  ";

                    total = total + Convert.ToInt16(_ticket.bids[i].quantity);
                    i++;
                    count--;

                }


                Offset = Offset + mediuminc;
                InsertHeaderStyleItem(bids, "", Offset);




            }

            Offset = Offset + mediuminc;

            InsertHeaderStyleItem("Qty  :" + total + " RS. " + total * 2 + "  " + CommonHelper.GetdateFormat(_ticket.time_end).ToString(), "", Offset);



            //graphics.DrawString("Welcome to HOT AND CRISPY", smallfont,
            //new SolidBrush(Color.Black), startX + 22, startY + Offset);


            //  Offset = Offset + mediuminc;



            //  DrawAtStart("Date: " + "11-09-21", Offset);

            // DrawAtStart("Time: " + "9:00 PM", Offset);

            /// Offset = Offset + mediuminc;

            //DrawAtStart("Qty : " + "10", Offset);

            // DrawAtStart("Rs: " + "2", Offset);


            //  DrawAtStart("Time: " + "9:00 PM", Offset);


            Offset = Offset + mediuminc;
            // underLine = "-------------------------";
            //  DrawLine(underLine, largefont, Offset, 30);

            // Offset = Offset + largeinc;

            // InsertHeaderStyleItem("Name. ", "Price. ", Offset);

            //Offset = Offset + largeinc;
            //foreach (var itran in order.ItemTransactions)
            //{
            //    InsertItem(itran.Item.Name + " x " + itran.Quantity, itran.Total.CValue, Offset);
            //    Offset = Offset + smallinc;
            //}
            //foreach (var dtran in order.DealTransactions)
            //{
            //    InsertItem(dtran.Deal.Name, dtran.Total.CValue, Offset);
            //    Offset = Offset + smallinc;

            //    foreach (var di in dtran.Deal.DealItems)
            //    {
            //        InsertItem(di.Item.Name + " x " + (dtran.Quantity * di.Quantity), "", Offset);
            //        Offset = Offset + smallinc;
            //    }
            //}

            //underLine = "-------------------------";
            //DrawLine(underLine, largefont, Offset, 30);

            //Offset = Offset + largeinc;
            //InsertItem(" Net. Total: ", order.Total.CValue, Offset);

            //if (!order.Cash.Discount.IsZero())
            //{
            //    Offset = Offset + smallinc;
            //    InsertItem(" Discount: ", order.Cash.Discount.CValue, Offset);
            //}

            //Offset = Offset + smallinc;
            //InsertHeaderStyleItem(" Amount Payable: ", order.GrossTotal.CValue, Offset);

            //Offset = Offset + largeinc;
            //String address = shop.Address;
            //DrawSimpleString("Address: " + address, minifont, Offset, 15);

            //Offset = Offset + smallinc;
            //String number = "Tel: " + shop.Phone1 + " - OR - " + shop.Phone2;
            //DrawSimpleString(number, minifont, Offset, 35);

            //Offset = Offset + 7;
            //underLine = "-------------------------------------";
            //DrawLine(underLine, largefont, Offset, 0);

            //Offset = Offset + mediuminc;
            //String greetings = "Thanks for visiting us.";
            //DrawSimpleString(greetings, mediumfont, Offset, 28);

            //Offset = Offset + mediuminc;
            //underLine = "-------------------------------------";
            //DrawLine(underLine, largefont, Offset, 0);

            //Offset += (2 * mediuminc);
            //string tip = "TIP: -----------------------------";
            //InsertItem(tip, "", Offset);

            //Offset = Offset + largeinc;
            //string DrawnBy = "Meganos Softwares: 0312-0459491 - OR - 0321-6228321";
            //DrawSimpleString(DrawnBy, minifont, Offset, 15);
        }

        private static void PrintCancelledTicket(object sender, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            Font minifont = new Font("Arial", 5);
            Font itemfont = new Font("Arial", 6);
            Font smallfont = new Font("Arial", 8);
            Font mediumfont = new Font("Arial", 10);
            Font largefont = new Font("Arial", 12);
            int Offset = 10;
            int smallinc = 10, mediuminc = 12, largeinc = 15;





            String underLine = "-------------------------------";
            DrawLine(underLine, mediumfont, Offset, 0);

            Offset = Offset + largeinc;

            InsertHeaderStyleItem("Terminal ID     :" + cticket.agent_code, "", Offset);

            Offset = Offset + mediuminc;

            InsertHeaderStyleItem("Barcode No     :" + cticket.ticket_barcode, "", Offset);

            Offset = Offset + mediuminc;

            InsertHeaderStyleItem("Cancelled Amt   :" + cticket.ticket_total_amount, "", Offset);


            Offset = Offset + mediuminc;

            InsertHeaderStyleItem("Cancelled At    :" + cticket.ticket_cancel_time, "", Offset);


            //graphics.DrawString("Welcome to HOT AND CRISPY", smallfont,
            //new SolidBrush(Color.Black), startX + 22, startY + Offset);


            //  Offset = Offset + mediuminc;

            //  DrawAtStart("Date: " + "11-09-21", Offset);

            // DrawAtStart("Time: " + "9:00 PM", Offset);

            /// Offset = Offset + mediuminc;

            //DrawAtStart("Qty : " + "10", Offset);

            // DrawAtStart("Rs: " + "2", Offset);


            //  DrawAtStart("Time: " + "9:00 PM", Offset);


            Offset = Offset + mediuminc;
            // underLine = "-------------------------";
            //  DrawLine(underLine, largefont, Offset, 30);

            // Offset = Offset + largeinc;

            // InsertHeaderStyleItem("Name. ", "Price. ", Offset);

            //Offset = Offset + largeinc;
            //foreach (var itran in order.ItemTransactions)
            //{
            //    InsertItem(itran.Item.Name + " x " + itran.Quantity, itran.Total.CValue, Offset);
            //    Offset = Offset + smallinc;
            //}
            //foreach (var dtran in order.DealTransactions)
            //{
            //    InsertItem(dtran.Deal.Name, dtran.Total.CValue, Offset);
            //    Offset = Offset + smallinc;

            //    foreach (var di in dtran.Deal.DealItems)
            //    {
            //        InsertItem(di.Item.Name + " x " + (dtran.Quantity * di.Quantity), "", Offset);
            //        Offset = Offset + smallinc;
            //    }
            //}

            //underLine = "-------------------------";
            //DrawLine(underLine, largefont, Offset, 30);

            //Offset = Offset + largeinc;
            //InsertItem(" Net. Total: ", order.Total.CValue, Offset);

            //if (!order.Cash.Discount.IsZero())
            //{
            //    Offset = Offset + smallinc;
            //    InsertItem(" Discount: ", order.Cash.Discount.CValue, Offset);
            //}

            //Offset = Offset + smallinc;
            //InsertHeaderStyleItem(" Amount Payable: ", order.GrossTotal.CValue, Offset);

            //Offset = Offset + largeinc;
            //String address = shop.Address;
            //DrawSimpleString("Address: " + address, minifont, Offset, 15);

            //Offset = Offset + smallinc;
            //String number = "Tel: " + shop.Phone1 + " - OR - " + shop.Phone2;
            //DrawSimpleString(number, minifont, Offset, 35);

            //Offset = Offset + 7;
            //underLine = "-------------------------------------";
            //DrawLine(underLine, largefont, Offset, 0);

            //Offset = Offset + mediuminc;
            //String greetings = "Thanks for visiting us.";
            //DrawSimpleString(greetings, mediumfont, Offset, 28);

            //Offset = Offset + mediuminc;
            //underLine = "-------------------------------------";
            //DrawLine(underLine, largefont, Offset, 0);

            //Offset += (2 * mediuminc);
            //string tip = "TIP: -----------------------------";
            //InsertItem(tip, "", Offset);

            //Offset = Offset + largeinc;
            //string DrawnBy = "Meganos Softwares: 0312-0459491 - OR - 0321-6228321";
            //DrawSimpleString(DrawnBy, minifont, Offset, 15);
        }

        private static void PrintReportSummary(object sender, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            Font minifont = new Font("Arial", 5);
            Font itemfont = new Font("Arial", 6);
            Font smallfont = new Font("Arial", 8);
            Font mediumfont = new Font("Arial", 10);
            Font largefont = new Font("Arial", 12);
            int Offset = 10;
            int smallinc = 10, mediuminc = 12, largeinc = 15;

            String underLine = "-------------------------------------------------------";

            Offset = Offset + largeinc;

            InsertHeaderStyleItem(UserAgent.AppName, "", Offset);

            Offset = Offset + largeinc;
            InsertHeaderStyleItem("Agent   " + UserAgent.AgentCode, "", Offset);

          
            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Date    " + DateTime.Now, "", Offset);

            Offset = Offset + largeinc;
            InsertHeaderStyleItem("Report   ", "", Offset);


            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("From :- " + _report.start_date + " To :-  " + _report.end_date, "", Offset);

            Offset = Offset + mediuminc;
            DrawLine(underLine, mediumfont, Offset, 0);


            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Gross Sales Amount      ", _report.gross_sales_amount, Offset);
        

            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Cancelled Amount        ", _report.cancelled_sales_amount, Offset);

            Offset = Offset + mediuminc;
            DrawLine(underLine, mediumfont, Offset, 0);

            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Net Sales Amount       " , _report.net_sales_amount, Offset);

            Offset = Offset + mediuminc;

            InsertHeaderStyleItem("Payout Amount           " , _report.payout_amount, Offset);

            Offset = Offset + mediuminc;
            DrawLine(underLine, mediumfont, Offset, 0);


            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Operator Balance        " , _report.operator_balance, Offset);

            Offset = Offset + mediuminc;
            DrawLine(underLine, mediumfont, Offset, 0);


            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Retailer Discount       " , _report.retailer_discount, Offset);

            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Sale Incentive          ",  _report.sale_incentive, Offset);

            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Payout Incentive        " , _report.PayoutIncentive, Offset);

            Offset = Offset + mediuminc;
            DrawLine(underLine, mediumfont, Offset, 0);

            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Total Profit            " , _report.total_profit, Offset);

            Offset = Offset + mediuminc;
            DrawLine(underLine, mediumfont, Offset, 0);

            Offset = Offset + mediuminc;
            InsertHeaderStyleItem("Net To Pay              " ,_report.net_to_pay, Offset);



            //graphics.DrawString("Welcome to HOT AND CRISPY", smallfont,
            //new SolidBrush(Color.Black), startX + 22, startY + Offset);


            //  Offset = Offset + mediuminc;

            //  DrawAtStart("Date: " + "11-09-21", Offset);

            // DrawAtStart("Time: " + "9:00 PM", Offset);

            /// Offset = Offset + mediuminc;

            //DrawAtStart("Qty : " + "10", Offset);

            // DrawAtStart("Rs: " + "2", Offset);


            //  DrawAtStart("Time: " + "9:00 PM", Offset);


            Offset = Offset + mediuminc;
            // underLine = "-------------------------";
            //  DrawLine(underLine, largefont, Offset, 30);

            // Offset = Offset + largeinc;

            // InsertHeaderStyleItem("Name. ", "Price. ", Offset);

            //Offset = Offset + largeinc;
            //foreach (var itran in order.ItemTransactions)
            //{
            //    InsertItem(itran.Item.Name + " x " + itran.Quantity, itran.Total.CValue, Offset);
            //    Offset = Offset + smallinc;
            //}
            //foreach (var dtran in order.DealTransactions)
            //{
            //    InsertItem(dtran.Deal.Name, dtran.Total.CValue, Offset);
            //    Offset = Offset + smallinc;

            //    foreach (var di in dtran.Deal.DealItems)
            //    {
            //        InsertItem(di.Item.Name + " x " + (dtran.Quantity * di.Quantity), "", Offset);
            //        Offset = Offset + smallinc;
            //    }
            //}

            //underLine = "-------------------------";
            //DrawLine(underLine, largefont, Offset, 30);

            //Offset = Offset + largeinc;
            //InsertItem(" Net. Total: ", order.Total.CValue, Offset);

            //if (!order.Cash.Discount.IsZero())
            //{
            //    Offset = Offset + smallinc;
            //    InsertItem(" Discount: ", order.Cash.Discount.CValue, Offset);
            //}

            //Offset = Offset + smallinc;
            //InsertHeaderStyleItem(" Amount Payable: ", order.GrossTotal.CValue, Offset);

            //Offset = Offset + largeinc;
            //String address = shop.Address;
            //DrawSimpleString("Address: " + address, minifont, Offset, 15);

            //Offset = Offset + smallinc;
            //String number = "Tel: " + shop.Phone1 + " - OR - " + shop.Phone2;
            //DrawSimpleString(number, minifont, Offset, 35);

            //Offset = Offset + 7;
            //underLine = "-------------------------------------";
            //DrawLine(underLine, largefont, Offset, 0);

            //Offset = Offset + mediuminc;
            //String greetings = "Thanks for visiting us.";
            //DrawSimpleString(greetings, mediumfont, Offset, 28);

            //Offset = Offset + mediuminc;
            //underLine = "-------------------------------------";
            //DrawLine(underLine, largefont, Offset, 0);

            //Offset += (2 * mediuminc);
            //string tip = "TIP: -----------------------------";
            //InsertItem(tip, "", Offset);

            //Offset = Offset + largeinc;
            //string DrawnBy = "Meganos Softwares: 0312-0459491 - OR - 0321-6228321";
            //DrawSimpleString(DrawnBy, minifont, Offset, 15);
        }
    }
}