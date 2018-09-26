using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.DashboardCommon;
namespace DXApplication19
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        int readNumber = 1;
        
        public XtraForm1()
        {
            InitializeComponent();
            //var dashbaord = this.dashboardViewer1.Dashboard;
            //var item0 = (DevExpress.DashboardCommon.ChartDashboardItem)dashbaord.Items[0];
            //var item1 = (DevExpress.DashboardCommon.CardDashboardItem)dashbaord.Items[1];
            string dataDir = AppDomain.CurrentDomain.BaseDirectory;
            if (dataDir.EndsWith(@"\bin\Debug\") || dataDir.EndsWith(@"\bin\Release\"))
            {
                dataDir = System.IO.Directory.GetParent(dataDir).Parent.Parent.FullName;
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);
            }


            using (SqlConnection conn = new SqlConnection(@"server =2720FOWBH58PIRV; uid =sa; pwd =ssl19951220; database =KERN;"))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from [KERN].[dbo].[operation_OPTime]", conn);
                DataSet ds = new DataSet();
                da.Fill(ds);

                DashboardObjectDataSource dataSource = new DashboardObjectDataSource();
                dataSource.DataSource = ds.Tables[0];
                dashboardViewer1.Dashboard.DataSources.Add(dataSource);

                //item0.DataSource = dataSource;
                //item1.DataSource = dataSource;
                dashboardViewer1.ReloadData();

                //this.DataSources.Add(dataSource);
                //this.chartControl1.Series[0].DataSource = ds.Tables[0];
                //this.chartControl1.Series[1].DataSource = ds.Tables[1];
                labelControl2.Text = ds.Tables[0].Rows[readNumber][1].ToString();
             }

            }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 900;
            int x = 0;
            DataSet ds = new DataSet();
            if (this.readNumber > 100)
            {
                this.readNumber = 1;
            }
            var dashbaord = this.dashboardViewer1.Dashboard;
            var item0 = (DevExpress.DashboardCommon.ChartDashboardItem)dashbaord.Items[0];
            var item1 = (DevExpress.DashboardCommon.CardDashboardItem)dashbaord.Items[1];
            string dataDir = AppDomain.CurrentDomain.BaseDirectory;
            if (dataDir.EndsWith(@"\bin\Debug\") || dataDir.EndsWith(@"\bin\Release\"))
            {
                dataDir = System.IO.Directory.GetParent(dataDir).Parent.Parent.FullName;
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);
            }
            DataTable dt;
            int y;
            using (SqlConnection conn = new SqlConnection(@"server =2720FOWBH58PIRV; uid =sa; pwd =ssl19951220; database =KERN;"))
            {
                conn.Open();
                
                SqlDataAdapter da;
                string S1 = "select  * from(select row_number() over(order by getdate()) 'id', * from[KERN].[dbo].[operation_OPTime]) t where id between " + (readNumber - 25) + " and " + readNumber + " ";
                string S2 = "select top " + this.readNumber + "* from [KERN].[dbo].[operation_OPTime]";
                int Thread = 25;
                if (readNumber > Thread)
                {
                    da = new SqlDataAdapter(S1, conn);
                    x = 25;
                    y = 2;
                    da.Fill(ds);
                    labelControl2.Text = ds.Tables[0].Rows[x][y].ToString();
                }
                else if(readNumber<Thread+1)
                {
                    da = new SqlDataAdapter(S2, conn);
                    x = readNumber - 1;
                    da.Fill(ds);
                    y = 1;
                    labelControl2.Text = ds.Tables[0].Rows[x][y].ToString();

                }
                dt = ds.Tables[0];


                DashboardObjectDataSource dataSource = new DashboardObjectDataSource();
                dataSource.DataSource = ds.Tables[0];
                //dashboardViewer1.Dashboard.DataSources.Add(dataSource);
                item0.DataSource = dataSource;
                item1.DataSource = dataSource;
                dashboardViewer1.ReloadData();
                //this.DataSources.Add(dataSource);
                //this.chartControl1.Series[0].DataSource = ds.Tables[0];
                //this.chartControl1.Series[1].DataSource = ds.Tables[1];

            }
            
            labelControl4.Text = dt.Compute("avg(OP3)", "true").ToString();
            labelControl6.Text = dt.Compute("Var(OP3)", "true").ToString();
            labelControl8.Text = dt.Compute("StDev(OP3)", "true").ToString();
            readNumber += 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void dashboardViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}