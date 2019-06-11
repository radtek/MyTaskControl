using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ApiDocHelper.Helper;

namespace ApiDocHelper
{
    public partial class FrmHistory : Form
    {
        public FrmHistory()
        {
            InitializeComponent();
        }
        SqliteHelper sqlite = new SqliteHelper();
        private void FrmHistory_Load(object sender, EventArgs e)
        {
            sqlite.SetSQLiteConnection(Application.StartupPath + "\\" + "ApiSettingDb.dll");
            string sql = $"SELECT DocName, Author, DocType, CreateTime, DocSaveFile FROM ApiDocSetting LIMIT 0,25";
            DataView.DataSource = sqlite.ExecuteDataset(sql, new Dictionary<string, string>()).Tables[0];
        }
    }
}
