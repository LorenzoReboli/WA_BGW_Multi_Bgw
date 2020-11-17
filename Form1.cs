using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WA_BGW_Multi_Bgw
{
    public partial class Form1 : Form
    {
        BackgroundWorker bgw1, bgw2;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Avvio2_Click(object sender, EventArgs e)
        {

            bgw1 = new BackgroundWorker();
            bgw1.WorkerReportsProgress = true;
            bgw1.WorkerSupportsCancellation = true;
            bgw1.DoWork += Bgw_DoWork;
            bgw1.ProgressChanged += Bgw_ProgressChanged;
            bgw1.RunWorkerCompleted += Bgw_RunWorkerCompleted;



            bgw2 = new BackgroundWorker();
            bgw2.WorkerReportsProgress = true;
            bgw2.WorkerSupportsCancellation = true;
            bgw2.DoWork += Bgw_DoWork;
            bgw2.ProgressChanged += Bgw_ProgressChanged;
            bgw2.RunWorkerCompleted += Bgw_RunWorkerCompleted;

            bgw1.RunWorkerAsync(tbBgw2);
            bgw2.RunWorkerAsync(tbBgw2);


        }
        private void btn_Stop2_Click(object sender, EventArgs e)
        {
            bgw1.CancelAsync();
            bgw2.CancelAsync();
        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            // e.Result ho 2 informazioni : bool , textbox
            var (completato, tbTesto) = (ValueTuple<bool, TextBox>)e.Result;

            //controllo completato a cui ho assegnato il valore bool di result
            if (completato)
            {
                tbTesto.Text = "Concluso" + Environment.NewLine + tbTesto.Text;
            }

            else
            {
                tbTesto.Text = "Cancellato" + Environment.NewLine + tbTesto.Text;
            }

           //modo per sostituire else if in modo più compatto
           // tbTesto.Text = completato ? "Concluso" + Environment.NewLine + tbTesto.Text : tbTesto.Text = "Cancellato" + Environment.NewLine + tbTesto.Text;

        }

        private void Bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            //peschiamo la textbox
            TextBox tbTesto = (TextBox)e.UserState;

            //scriviamo laprima riga e poi andiamo a capo
            tbTesto.Text = e.ProgressPercentage + Environment.NewLine + tbTesto.Text;

        }


        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = sender as BackgroundWorker;
            TextBox tbTesto = (TextBox)e.Argument;

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                bgw.ReportProgress(i, tbTesto);

                if (bgw.CancellationPending)
                {
                    e.Result = (false, tbTesto);
                    return;
                }

            }

            e.Result = (true, tbTesto);

        }

        List<BackgroundWorker> bgw_list = new List<BackgroundWorker>();
        private void btn_Avvio100_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                BackgroundWorker bgw1 = new BackgroundWorker();
                bgw1.WorkerReportsProgress = true;
                bgw1.WorkerSupportsCancellation = true;
                bgw1.DoWork += Bgw_DoWork;
                bgw1.ProgressChanged += Bgw_ProgressChanged;
                bgw1.RunWorkerCompleted += Bgw_RunWorkerCompleted;
                bgw_list.Add(bgw1);

            }
            foreach(var bgw in bgw_list)
            {
                bgw.RunWorkerAsync(tbBgw100);
            }
        }
        private void btn_Stop100_Click(object sender, EventArgs e)
        {
            foreach (var bgw in bgw_list)
            {
                bgw.CancelAsync();
            }
        }


    }
}
