using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Background_Worker_Sample
{
    public partial class Form1 : Form
    {
        private BackgroundWorker myWorker1 = new BackgroundWorker();
        private BackgroundWorker myWorker2 = new BackgroundWorker();

        private string _lastJSON;
        public Form1()
        {
            InitializeComponent();
                       
            myWorker1.DoWork += new DoWorkEventHandler(DepositWorker);
            myWorker1.DoWork += new DoWorkEventHandler(WithdrawWorker);
            myWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker1_RunWorkerCompleted);
            myWorker1.ProgressChanged += new ProgressChangedEventHandler(myWorker1_ProgressChanged);
            myWorker1.WorkerReportsProgress = true;
            myWorker1.WorkerSupportsCancellation = true;

            //myWorker2.DoWork += new DoWorkEventHandler(WithdrawWorker);
            myWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker2_RunWorkerCompleted);
            myWorker1.ProgressChanged += new ProgressChangedEventHandler(myWorker2_ProgressChanged);
            myWorker1.WorkerReportsProgress = true;
            myWorker1.WorkerSupportsCancellation = true;

        }       

        protected void WithdrawWorker(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker = (BackgroundWorker)sender;

            //string json = (string)e.Argument;//Collect the array of objects the we recived from the main thread

            StringBuilder sb = new StringBuilder();
           
            string response = WithdrawTask(_lastJSON, 1, false);

            for (int i = 0; i < 99; i++)
            {
                response = WithdrawTask(response, 1, true);

                if (!sendingWorker.CancellationPending)//At each iteration of the loop, check if there is a cancellation request pending 
                {
                    sb.Append(string.Format("Withdraw Reponse: {0}{1}", response, Environment.NewLine));//Append the result to the string builder
                    sendingWorker.ReportProgress(i);//Report our progress to the main thread
                }
                else
                {
                    e.Cancel = true;//If a cancellation request is pending,assgine this flag a value of true
                    break;// If a cancellation request is pending, break to exit the loop
                }
            }
            e.Result += sb.ToString();// Send our result to the main thread!     
        }
        protected void DepositWorker(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker = (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            string json = (string) e.Argument;//Collect the array of objects the we recived from the main thread

            StringBuilder sb = new StringBuilder();

            string response = DepositTask(json, 1);

            for(int i=0;i<99;i++)
            {
                response = DepositTask(response, 1);

                if (!sendingWorker.CancellationPending)//At each iteration of the loop, check if there is a cancellation request pending 
                {
                    sb.Append(string.Format("Deposit Reponse: {0}{1}", response, Environment.NewLine));//Append the result to the string builder
                    sendingWorker.ReportProgress(i);//Report our progress to the main thread
                }
                else
                {
                    e.Cancel = true;//If a cancellation request is pending,assgine this flag a value of true
                    break;// If a cancellation request is pending, break to exit the loop
                }
            }

            _lastJSON = response;

            e.Result = sb.ToString();// Send our result to the main thread!           
        }

        protected void myWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            if (!e.Cancelled && e.Error == null)
            {
                string result = (string)e.Result;
                txtResult.Text = result;
                lblStatus.Text = "Done";
            }
            else if (e.Cancelled)
            {
                lblStatus.Text = "User Cancelled";
            }
            else
            {
                lblStatus.Text = "An error has occured";
            }
            btnStart.Enabled = true;//Reneable the start button
        }

        protected void myWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string result = (string)e.Result;
                txtResult.Text = result;
                lblStatus.Text = "Done";      
            }
            else if (e.Cancelled)
            {
                lblStatus.Text = "User Cancelled";
            }
            else
            {
                lblStatus.Text = "An error has occured";
            }
            btnStart.Enabled = true;//Reneable the start button
        }

        protected void myWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        protected void myWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private int PerformHeavyOperation(int i)
        {
            System.Threading.Thread.Sleep(100);
            return i * 1000;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DeleteAll();
            var json = InitAccountInformation(1);

            if (!myWorker1.IsBusy)//Check if the worker is already in progress
            {
                btnStart.Enabled = false;//Disable the Start button
                myWorker1.RunWorkerAsync(json);//Call the background worker
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            myWorker1.CancelAsync();//Issue a cancellation request to stop the background worker
        }


        private Object InitAccountInformation(int amount)
        {
            RestClient httpClient = new RestClient("http://localhost:13297/");

            var account = new CreateAccountRequest() { AccountNumber = 1, Amount = amount, Currency = "EU" };

            return RequestServer("api/Account/Create", httpClient, Method.POST, account);
        }

        private string DepositTask(string request, double amount)
        {
            RestClient httpClient = new RestClient("http://localhost:13297/");

            dynamic obj = JsonConvert.DeserializeObject(request);

            var account = new DepositAccountRequest();
            account.AggregateID = obj.aggregateID;
            account.Amount = amount;
            account.AccountNumber = obj.accountNumber;
            account.Currency = obj.currency;
            account.Version = obj.version;

            return RequestServer("api/Account/Deposit", httpClient, Method.PUT, account);
        }

        private string WithdrawTask(string request, double amount, bool increment)
        {
            RestClient httpClient = new RestClient("http://localhost:13297/");

            dynamic obj = JsonConvert.DeserializeObject(request);

            var account = new WithdrawAccountRequest();
            account.AggregateID = obj.aggregateID;
            account.Amount = amount;
            account.AccountNumber = obj.accountNumber;
            account.Currency = obj.currency;
            account.Version = !increment ? obj.version : obj.version + 1;

            return RequestServer("api/Account/Withdraw", httpClient, Method.PUT, account);
        }

        private void DeleteAll()
        {
            RestClient httpClient = new RestClient("http://localhost:13297/");

            var request = new RestRequest("api/Account/deleteall", Method.DELETE);

            var restResponse = httpClient.Execute(request);            
        }

        private string RequestServer(string api, RestClient client, Method action, CreateAccountRequest account)
        {
            var request = new RestRequest(api, action);

            request.RequestFormat = DataFormat.Json;

            request.AddBody(account);

            var restResponse = client.Execute(request);            

            return JsonConvert.DeserializeObject(restResponse.Content).ToString();
        }

    }

    public class AccountReponse
    {
        public Guid AggregateID { get; set; }
        public int AccountNumber { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public int Version { get; set; }
    }

    public class CreateAccountRequest
    {
        public int AccountNumber { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }
    };

    public class DepositAccountRequest : CreateAccountRequest
    {

        public Guid AggregateID { get; set; }

        public int Version { get; set; }
    }

    public class WithdrawAccountRequest : CreateAccountRequest
    {

        public Guid AggregateID { get; set; }

        public int Version { get; set; }
    }
}
