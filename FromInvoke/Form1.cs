using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using FromInvoke.Model;
using FromInvoke.Func;
using PosSharp.Currency;

namespace FromInvoke
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string result { get; set; }

        delegate void AsyncUpdateUI();

        FrmLoading f;
        private void Button1_Click(object sender, EventArgs e)
        {

            f = new FrmLoading();
            f.Show();
            AliPay pay = new AliPay();
            pay.TradeCallBack += PayFinish;
            pay.TradeFinish += CloseForm;
            Task t = Task.Factory.StartNew(() => { pay.Handle(); });
            
        }
        public void PayFinish(object re)
        {
            PaySuccess r = re as PaySuccess;
            MessageBox.Show($"支付结果：{r.IsSuccess}|支付金额：{r.Amount}|第三方订单号：{r.ThirdNo}|支付方式：{r.PaySubWay}");
            this.result = $"支付结果：{r.IsSuccess}|支付金额：{r.Amount}|";
            MessageBox.Show(result);
        }
        public void CloseForm()
        {
            if (InvokeRequired)
            {
                this.Invoke(new AsyncUpdateUI(() => {
                    MessageBox.Show("即将关闭窗体");
                    f.Close();
                }));
            }
            else
            {
                f.Close();
            }
        }
    }
}
