using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using ApiDocHelper.Helper;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

namespace ApiDocHelper
{
    public partial class FrmSignRuler : Form
    {
        /// <summary>
        /// Sqlite-Api编号
        /// </summary>
        public string ApiCode { get; set; }
        /// <summary>
        /// 文档地址
        /// </summary>
        public string DocFile { get; set; }

        string FontStyleName = "FontStyle";
        string TitleStyleName = "TitleStyle";
        SqliteHelper sqlite = new SqliteHelper();
        Document ApiDoc = new Document();
        public FrmSignRuler()
        {
            InitializeComponent();
        }
        private void FrmSignRuler_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DocFile))
            {
                MessageBox.Show("请先生成主Api文档再进行追加签名/加密规范操作！");
                this.Close();
                return;
            }
            sqlite.SetSQLiteConnection(Application.StartupPath + "\\" + "ApiSettingDb.dll");
            ApiDoc = new Document(DocFile);
        }
        public Paragraph NewLine(Section s, Paragraph p)
        {
            p = s.AddParagraph();
            p.AppendText(string.Empty);
            p.ApplyStyle(BuiltinStyle.Normal);
            return p;
        }
        private void M_Save_Click(object sender, EventArgs e)
        {
            try
            {
                //先操作Doc文档
                //创建部分/区域
                Section section = ApiDoc.AddSection();
                Paragraph paragraph = section.AddParagraph();

                #region 具体内容
                paragraph = section.AddParagraph();
                paragraph.AppendText("签名/加密");
                paragraph.ApplyStyle(BuiltinStyle.Heading1);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText("签名规范");
                paragraph.ApplyStyle(BuiltinStyle.Heading2);
                NewLine(section, paragraph);

                //-------------------------------
                paragraph = section.AddParagraph();
                paragraph.AppendText("签名方式");
                paragraph.ApplyStyle(TitleStyleName);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText(Tb_SignType.Text);
                paragraph.ApplyStyle(FontStyleName);
                //首行缩进
                paragraph.Format.FirstLineIndent = 30;
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText("算法模型");
                paragraph.ApplyStyle(TitleStyleName);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText(Tb_SignModel.Text);
                paragraph.ApplyStyle(FontStyleName);
                //首行缩进
                paragraph.Format.FirstLineIndent = 30;
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText("生成规则");
                paragraph.ApplyStyle(TitleStyleName);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText(Tb_RulerTxt.Text);
                paragraph.ApplyStyle(FontStyleName);
                //首行缩进
                paragraph.Format.FirstLineIndent = 30;
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText("数据加密");
                paragraph.ApplyStyle(BuiltinStyle.Heading2);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText("加密方式");
                paragraph.ApplyStyle(TitleStyleName);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText(Tb_EncryType.Text);
                paragraph.ApplyStyle(FontStyleName);
                //首行缩进
                paragraph.Format.FirstLineIndent = 30;
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText("算法模型");
                paragraph.ApplyStyle(TitleStyleName);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText(Tb_EncryModel.Text);
                paragraph.ApplyStyle(FontStyleName);
                //首行缩进
                paragraph.Format.FirstLineIndent = 30;
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText("加密规则");
                paragraph.ApplyStyle(TitleStyleName);
                NewLine(section, paragraph);

                paragraph = section.AddParagraph();
                paragraph.AppendText(Tb_EncryDesc.Text);
                paragraph.ApplyStyle(FontStyleName);
                //首行缩进
                paragraph.Format.FirstLineIndent = 30;
                NewLine(section, paragraph);
                #endregion

                //保存Doc
                ApiDoc.SaveToFile(DocFile, FileFormat.Doc);
                string RulerCode = UtilityHelper.CreateApiDocNo();
                //再进行Sqlite保存
                sqlite.ExecuteNonQuery($"INSERT INTO ApiSignRuler (SignRulerCode, ApiDocCode, SignType, SignModel, SignDesc, EncryType, EncryModel, EncryDesc) VALUES('{RulerCode}', '{ApiCode}', '{Tb_SignType.Text}', '{Tb_SignModel.Text}', '{Tb_RulerTxt.Text}', '{Tb_EncryType.Text}', '{Tb_EncryModel.Text}', '{Tb_EncryDesc.Text}')", new Dictionary<string, string>());
                MessageBox.Show("签名/数据加密说明添加完成！");
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show($"Program Error：{err.Message}！");
            }

            

        }

        private void M_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
