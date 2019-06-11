using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Spire.Doc;
using Spire.Doc.Documents;
using ApiDocHelper.Helper;
using Spire.Doc.Fields;
using Spire.Doc.Formatting;
using Spire.Doc.Interface;
using ApiDocHelper.Model;
using Newtonsoft.Json;



namespace ApiDocHelper
{
    public partial class FrmMain : Form
    {
        SqliteHelper sqlite = new SqliteHelper();
        string doctype = "Sdk";
        string SaveDbApiCode = string.Empty;
        public FrmMain()
        {
            InitializeComponent();
        }
        Document ApiDoc = new Document();
        string FontStyleName = "FontStyle";
        string CellStyleName = "CellStyle";
        string LinkStyleName = "LinkStyle";
        string TitleStyleName = "TitleStyle";
        string MainTietleStyle = "MainTietleStyle";
        bool isFirst = true;
        public string CreateDocFile { get; set; }
        private void M_Save_Click(object sender, EventArgs e)
        {
            try
            {
                DocCreate();
                CreateDocFile = $"{Application.StartupPath}\\{Tb_DocName.Text}.docx";
                string apicode = UtilityHelper.CreateApiDocNo();
                sqlite.SetSQLiteConnection(Application.StartupPath + "\\" + "ApiSettingDb.dll");
                //增加Sqlite数据库记录+增加签名说明
                sqlite.ExecuteNonQuery($"INSERT INTO ApiDocSetting (ApiDocCode, DocName, Author, DocType,DocSaveFile) VALUES ('{apicode}', '{Tb_DocName.Text}', '{Tb_Author.Text}', '{doctype}','{CreateDocFile}')", null);
                SaveDbApiCode = apicode;
                MessageBox.Show("保存成功！");
                isFirst = false;
            }
            catch (Exception err)
            {
                MessageBox.Show($"Program Error：{err.Message}！");
            }

        }
        public void DocCreate()
        {
            string DocName = Tb_DocName.Text;
            string titleName = Tb_CoreTitle.Text;
            //创建部分/区域
            Section section = ApiDoc.AddSection();
            

            HeaderFooter headeraaa = section.HeadersFooters.Header;
            Paragraph paragraph = headeraaa.AddParagraph();
            if (section.HeadersFooters.Header.Count == 1)
            {
                //页眉
                TextRange HText = paragraph.AppendText("青岛雨诺网络信息股份有限公司");
                //Set Header Text Format
                HText.CharacterFormat.FontName = "Ya Hei";
                HText.CharacterFormat.FontSize = 10;
                HText.CharacterFormat.TextColor = Color.RoyalBlue;
                //Set Header Paragraph Format
                paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                paragraph.Format.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.DashSmallGap;
                paragraph.Format.Borders.Bottom.Space = 0.05f;
                paragraph.Format.Borders.Bottom.Color = Color.DarkGray;

                //页脚
                HeaderFooter footer = section.HeadersFooters.Footer;
                paragraph = footer.AddParagraph();
                TextRange FText = paragraph.AppendText("青岛雨诺网络信息股份有限公司");
                FText.CharacterFormat.FontName = "Ya Hei";
                FText.CharacterFormat.FontSize = 10;
                FText.CharacterFormat.TextColor = Color.RoyalBlue;
                paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                paragraph.Format.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.DashSmallGap;
                paragraph.Format.Borders.Top.Space = 0.15f;
                paragraph.Format.Borders.Color = Color.DarkGray;
            } 




            paragraph = section.AddParagraph();



            #region 文档主标题
            //追加文本
            paragraph.AppendText($"《{titleName}》");
            paragraph.ApplyStyle(MainTietleStyle);
            //对齐方式
            paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            #endregion

            #region 1.版本号
            //标题
            Paragraph pVerson = section.AddParagraph();
            pVerson.AppendText("版本号");
            pVerson.ApplyStyle(BuiltinStyle.Heading1);
            pVerson = section.AddParagraph();
            pVerson.AppendText(Environment.NewLine);
            pVerson.ApplyStyle(BuiltinStyle.Normal);
            //表格
            string[] header = { "版本号", "更新时间", "更新人", "更新内容" };
            string[][] data = { new string[] { "V1.0", DateTime.Now.ToString("yyyy年MM月dd日"), Tb_Author.Text, Tb_zygn.Text } };
            Table table = section.AddTable(true);
            table.ResetCells(data.Length + 1, header.Length);
            TableRow row = table.Rows[0];
            row.IsHeader = true;
            row.Height = 20;    //unit: point, 1point = 0.3528 mm
            row.HeightType = TableRowHeightType.Exactly;
            row.RowFormat.BackColor = Color.SkyBlue;
            for (int i = 0; i < header.Length; i++)
            {
                row.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                Paragraph p = row.Cells[i].AddParagraph();
                p.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                TextRange txtRange = p.AppendText(header[i]);
                txtRange.CharacterFormat.Bold = true;
            }

            for (int r = 0; r < data.Length; r++)
            {
                TableRow dataRow = table.Rows[r + 1];
                //dataRow.Height = 20;
                dataRow.HeightType = TableRowHeightType.Auto;
                dataRow.RowFormat.BackColor = Color.Empty;
                for (int c = 0; c < data[r].Length; c++)
                {
                    dataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    paragraph = dataRow.Cells[c].AddParagraph();
                    paragraph.AppendText(data[r][c]);
                    paragraph.ApplyStyle(CellStyleName);
                    paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                }
            }

            #endregion

            #region 2.介绍
            //标题
            paragraph = section.AddParagraph();
            paragraph.AppendText("介绍");
            paragraph.ApplyStyle(BuiltinStyle.Heading1);
            //详细内容
            NewLine(section, paragraph);
            paragraph = section.AddParagraph();
            paragraph.AppendText(Tb_Desc.Text);
            paragraph.ApplyStyle(BuiltinStyle.Normal);
            //首行缩进
            paragraph.Format.FirstLineIndent = 30;

            //需要阅读该文档的角色：
            
            paragraph = section.AddParagraph();
            NewLine(section, paragraph);
            paragraph.AppendText($"需要阅读该文档的角色：{Tb_Reader.Text??"开发者、项目经理"}");
            pVerson.ApplyStyle(BuiltinStyle.Normal);
            #endregion

            #region 3.接口约定 
            //标题
            paragraph = section.AddParagraph();
            paragraph.AppendText("接口约定");
            paragraph.ApplyStyle(BuiltinStyle.Heading1);
            NewLine(section, paragraph);
            //详细内容
            paragraph = section.AddParagraph();
            paragraph.AppendText(Tb_RulerDesc.Text);
            paragraph.ApplyStyle(FontStyleName);
            //首行缩进
            paragraph.Format.FirstLineIndent = 30;
            #endregion

            paragraph = section.AddParagraph();
            paragraph.AppendText("Api说明");
            paragraph.ApplyStyle(BuiltinStyle.Heading1);
            NewLine(section, paragraph);
            NewLine(section, paragraph);
            NewLine(section, paragraph);
            //保存Doc
            ApiDoc.SaveToFile($"{DocName}.docx", FileFormat.Doc);
            //文件预览
            //UtilityHelper.WordDocView($"{DocName}.docx");
        }
        private void M_SaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFilePath.ShowDialog();
                if (!string.IsNullOrEmpty(SaveFilePath.SelectedPath))
                {
                    string SavePath = SaveFilePath.SelectedPath;
                    DocCreate();
                    File.Copy($"{Application.StartupPath}\\{Tb_DocName.Text}.docx", $"{SavePath}\\{Tb_DocName.Text}.docx", true);
                    CreateDocFile = $"{SavePath}\\{Tb_DocName.Text}.docx";
                    File.Delete($"{Application.StartupPath}\\{Tb_DocName.Text}.docx");
                    string apicode = UtilityHelper.CreateApiDocNo();
                    sqlite.SetSQLiteConnection(Application.StartupPath + "\\" + "ApiSettingDb.dll");
                    //增加Sqlite数据库记录+增加签名说明
                    sqlite.ExecuteNonQuery($"INSERT INTO ApiDocSetting (ApiDocCode, DocName, Author, DocType,DocSaveFile) VALUES ('{apicode}', '{Tb_DocName.Text}', '{Tb_Author.Text}', '{doctype}','{CreateDocFile}')", new Dictionary<string, string>());
                    SaveDbApiCode = apicode;
                    MessageBox.Show("保存成功！");
                    isFirst = false;
                }
                else
                {
                    MessageBox.Show("未选择文件夹不能保存！");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Program Error：{err.Message}！");
            }
        }

        private void M_View_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CreateDocFile))
            {
                UtilityHelper.WordDocView(CreateDocFile);
            }
            else
            {
                MessageBox.Show("尚未生成任何Api文档，不支持预览！");
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            sqlite.SetSQLiteConnection(Application.StartupPath + "\\" + "ApiSettingDb.dll");
            #region 样式
            ParagraphStyle style = new ParagraphStyle(ApiDoc);
            style.Name = FontStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 8;
            ApiDoc.Styles.Add(style);

            style = new ParagraphStyle(ApiDoc);
            style.Name = CellStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 8;
            ApiDoc.Styles.Add(style);

            //URL样式
            style = new ParagraphStyle(ApiDoc);
            style.Name = LinkStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 10;
            ApiDoc.Styles.Add(style);
            //小标题字体样式
            style = new ParagraphStyle(ApiDoc);
            style.Name = TitleStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 12;
            ApiDoc.Styles.Add(style);

            //设置字体样式
            style = new ParagraphStyle(ApiDoc);
            style.Name = MainTietleStyle;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 20;
            ApiDoc.Styles.Add(style);
            #endregion
        }
        public Paragraph NewLine(Section s,Paragraph p)
        {
            p = s.AddParagraph();
            p.AppendText(string.Empty);
            p.ApplyStyle(BuiltinStyle.Normal);
            return p;
        }

        public Paragraph AddText(Section s, Paragraph p,string txt,string stylename)
        {
            p = s.AddParagraph();
            p.AppendText(txt);
            p.ApplyStyle(stylename);
            return p;
        }
        public  enum ParamsType
        {
            Header = 0,
            Body = 1,
            Rsp = 2
        }
        public Table AddTable(Section tablesection, Paragraph paragraph, string[] tableTitle,List<ParamsSetting> pa)
        {
            if (pa == null)
            {
                return null;
            }
            Table table = tablesection.AddTable(true);
            //string[] tableTitle = { "名称", "类型", "长度", "必填", "说明" };
            table.ResetCells(pa.Count + 1, tableTitle.Length);
            for (int r = 0; r < pa.Count + 1; r++)
            {
                if (r == 0)
                {
                    TableRow row1 = table.Rows[0];
                    row1.HeightType = TableRowHeightType.Auto;
                    row1.RowFormat.BackColor = Color.SkyBlue;
                    //添加标题
                    for (int i = 0; i < tableTitle.Length; i++)
                    {
                        row1.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        paragraph = row1.Cells[i].AddParagraph();
                        paragraph.AppendText(tableTitle[i]);
                        paragraph.ApplyStyle(CellStyleName);
                        paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                    }
                    continue;
                }
                TableRow dataRow = table.Rows[r];
                dataRow.HeightType = TableRowHeightType.Auto;
                dataRow.RowFormat.BackColor = Color.Empty;
                for (int c = 0; c < tableTitle.Length; c++)
                {
                    dataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    paragraph = dataRow.Cells[c].AddParagraph();
                    if (c == 0)
                        paragraph.AppendText(pa[r - 1].Name);
                    else if (c == 1)
                        paragraph.AppendText(pa[r - 1].Type);
                    else if (c == 2)
                        paragraph.AppendText(pa[r - 1].Length.ToString());
                    else if (c == 3)
                        paragraph.AppendText(pa[r - 1].IsNullable.ToString());
                    else
                        paragraph.AppendText(pa[r - 1].Desc);
                    paragraph.ApplyStyle(CellStyleName);
                    paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                }
            }
            return table;
        }
        private void Menu_WbApi_Click(object sender, EventArgs e)
        {
            doctype = "WebApi";
            OpenFIle.Multiselect = false;
            OpenFIle.Filter = "WebApi-json(*.json)|*.json";
            OpenFIle.ShowDialog();
            if (string.IsNullOrEmpty(OpenFIle.FileName))
            {
                MessageBox.Show("未选择文件！");
            }
            else
            {
                string jsonFilePath = OpenFIle.FileName;
                string jsonContent =  UtilityHelper.ReadFileContent(jsonFilePath);
                List<ApiSeetingModel> Apis = JsonConvert.DeserializeObject<List<ApiSeetingModel>>(jsonContent);
                if (ApiDoc == null || ApiDoc.PageCount == 0)
                {
                    MessageBox.Show("尚未创建相关Api文档，无法进行配置参数导入！");
                }
                else
                {
                    //文档追加表格
                    Section tablesection = ApiDoc.Sections[0];
                    int n = 0;
                    foreach (var m in Apis)
                    {
                        n++;
                        Paragraph paragraph = tablesection.AddParagraph();
                        //Paragraph paragraph = tablesection.Paragraphs[0];
                        NewLine(tablesection, paragraph);
                        paragraph.AppendText($"{n}.{m.ApiName}");
                        paragraph.ApplyStyle(BuiltinStyle.Heading2);
                        string[] tableTitle = { "名称", "类型", "长度", "必填", "说明" };
                        NewLine(tablesection, paragraph);
                        //Api描述信息
                        AddText(tablesection, paragraph, "Api名称", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.ApiName, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //更新时间
                        AddText(tablesection, paragraph, "更新时间", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.ApiUpdateTime, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //功能介绍
                        AddText(tablesection, paragraph, "功能介绍", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.ApiDesc, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //Url
                        AddText(tablesection, paragraph, "Url", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.Url, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //请求方式
                        AddText(tablesection, paragraph, "请求方式", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.Method, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //编码规范
                        AddText(tablesection, paragraph, "编码规范", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.Encoding, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //公共请求消息头/消息头（Header）
                        AddText(tablesection, paragraph, "公共请求消息头/消息头（Header）", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.RequestHeader, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //公共响应消息头/公共响应头域
                        AddText(tablesection, paragraph, "公共响应消息头/公共响应头域", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, m.ResponseHeader, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //新增请求头Table
                        AddText(tablesection, paragraph, "额外的请求头", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddTable(tablesection, paragraph, tableTitle, m.RequestExtraHeader);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //新增BodyTable
                        AddText(tablesection, paragraph, "Body", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddTable(tablesection, paragraph, tableTitle, m.RequestBodyJson);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                        //新增响应参数Table
                        AddText(tablesection, paragraph, "响应参数", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddTable(tablesection, paragraph, tableTitle, m.ResponseResult);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                    }
                    

                    if (!string.IsNullOrEmpty(CreateDocFile))
                    {
                        //如果未创建其他Api文档，则在程序运行目录下创建文件
                        ApiDoc.SaveToFile(CreateDocFile, FileFormat.Doc);
                        MessageBox.Show("导入成功");
                    }
                    else
                    {
                        //如果未创建其他Api文档，则在程序运行目录下创建文件
                        ApiDoc.SaveToFile($"{DateTime.Now.ToString("yyyyMMddHHmm")}.docx", FileFormat.Doc);
                        MessageBox.Show("导入成功");
                    }

                }

            }
        }

        private void Menu_Sdk_Click(object sender, EventArgs e)
        {
            doctype = "Sdk";
            OpenFIle.Multiselect = false;
            OpenFIle.Filter = "Sdk-json文件(*.json)|*.json";
            OpenFIle.ShowDialog();
            if (string.IsNullOrEmpty(OpenFIle.FileName) )
            {
                MessageBox.Show("未选择任何Json配置文件");
            }
            else
            {
                if (ApiDoc == null)
                {
                    MessageBox.Show("尚未创建相关Api文档，无法进行配置参数导入！");
                }
                else
                {
                    string jsonFilePath = OpenFIle.FileName;
                    string jsonContent = UtilityHelper.ReadFileContent(jsonFilePath);
                    List<SdkSettingModel> sdkSettings = JsonConvert.DeserializeObject<List<SdkSettingModel>>(jsonContent);
                    //文档追加表格
                    Section tablesection = ApiDoc.Sections[0];
                    int n = 0;
                    foreach (var s in sdkSettings)
                    {
                        n++;
                        Paragraph paragraph = tablesection.AddParagraph();
                        NewLine(tablesection, paragraph);
                        paragraph.AppendText($"{n}.{s.FuncChineseName}");
                        paragraph.ApplyStyle(BuiltinStyle.Heading2);
                        string[] tableTitle = { "名称", "类型", "长度", "必填", "说明" };
                        NewLine(tablesection, paragraph);
                        //函数位置
                        AddText(tablesection, paragraph, "函数位置", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, s.FuncPosition, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);

                        AddText(tablesection, paragraph, "方法名称", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, s.FuncName, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);

                        AddText(tablesection, paragraph, "方法备注", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, s.FuncDesc, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);

                        AddText(tablesection, paragraph, "传入形参", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddTable(tablesection, paragraph,tableTitle, s.InputParams);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);

                        AddText(tablesection, paragraph, "返回结果类型", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, s.ReturnType, FontStyleName);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);

                        AddText(tablesection, paragraph, "返回结果", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddTable(tablesection, paragraph, tableTitle, s.OutParams);
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);

                        AddText(tablesection, paragraph, "备注", TitleStyleName);
                        NewLine(tablesection, paragraph);
                        AddText(tablesection, paragraph, s.ReturnParamsDesc, FontStyleName);
                        //首行缩进
                        paragraph.Format.FirstLineIndent = 30;
                        NewLine(tablesection, paragraph);
                        NewLine(tablesection, paragraph);
                    }
                }
                if (!string.IsNullOrEmpty(CreateDocFile))
                {
                    //如果未创建其他Api文档，则在程序运行目录下创建文件
                    ApiDoc.SaveToFile(CreateDocFile, FileFormat.Doc);
                    MessageBox.Show("导入成功");
                }
                else
                {
                    //如果未创建其他Api文档，则在程序运行目录下创建文件
                    ApiDoc.SaveToFile($"{DateTime.Now.ToString("yyyyMMddHHmm")}.docx", FileFormat.Doc);
                    MessageBox.Show("导入成功");
                }
            }
        }

        private void M_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void M_AboutAuthor_Click(object sender, EventArgs e)
        {

        }

        private void M_AboutGs_Click(object sender, EventArgs e)
        {
            FrmCompany fm = new FrmCompany();
            fm.ShowDialog();
        }

        private void M_WebApiHttp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该功能尚未开通，敬请期待...");
        }

        private void M_ClearAll_Click(object sender, EventArgs e)
        {
            Tb_Author.Text = "";
            Tb_CoreTitle.Text = "";
            Tb_Desc.Text = "";
            Tb_DocName.Text = "";
            Tb_Reader.Text = "";
            Tb_RulerDesc.Text = "";
            Tb_zygn.Text = "";
        }

        private void M_SignRulerInput_Click(object sender, EventArgs e)
        {
            FrmSignRuler f = new FrmSignRuler();
            f.DocFile = CreateDocFile;
            f.ApiCode = SaveDbApiCode;
            f.ShowDialog();
        }

        private void M_SignRulerInsert_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该功能尚未开通，敬请期待...");
        }

        private void M_AppendOther_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该功能尚未开通，敬请期待...");
        }

        private void M_DocLIstory_Click(object sender, EventArgs e)
        {
            FrmHistory f = new FrmHistory();
            f.ShowDialog();
        }

        private void M_ClearApiDoc_Click(object sender, EventArgs e)
        {
            ApiDoc = new Document();
            #region 样式
            ParagraphStyle style = new ParagraphStyle(ApiDoc);
            style.Name = FontStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 8;
            ApiDoc.Styles.Add(style);

            style = new ParagraphStyle(ApiDoc);
            style.Name = CellStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 8;
            ApiDoc.Styles.Add(style);

            //URL样式
            style = new ParagraphStyle(ApiDoc);
            style.Name = LinkStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 10;
            ApiDoc.Styles.Add(style);
            //小标题字体样式
            style = new ParagraphStyle(ApiDoc);
            style.Name = TitleStyleName;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 12;
            ApiDoc.Styles.Add(style);

            //设置字体样式
            style = new ParagraphStyle(ApiDoc);
            style.Name = MainTietleStyle;
            style.CharacterFormat.FontName = "Ya Hei";
            style.CharacterFormat.FontSize = 20;
            ApiDoc.Styles.Add(style);
            #endregion
            MessageBox.Show("重置完成，已可以再生成新的ApiDoc！");
        }
    }
}
