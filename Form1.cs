using Converte_PDF_Texto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Ficheiro
        {
            public int? Servico { get; set; }
            public string Nome { get; set; }
            public int? Mes { get; set; }
            public int? Ano { get; set; }
            public Ficheiro(string path)
            {
                if (File.Exists(path))
                {
                    int x;
                    FileInfo file = new FileInfo(path);
                    string[] aInfo = file.Name.Substring(0, file.Name.IndexOf('.')).Split('-');
                    Servico = (Int32.TryParse(aInfo[0], out x) && x > 0) ? x : null;
                    Mes = (Int32.TryParse(aInfo[2], out x) && x > 0) ? x : null;
                    Ano = (Int32.TryParse(aInfo[1], out x) && x > 0) ? x : null;
                    Nome = file.Name;
                }
            }
        }

        public int IdServico = 0;
        public List<ConvertePDF.DataLine> DataSource = new List<ConvertePDF.DataLine>();
        public Ficheiro FicheiroInfo;


        private void btnProcurar_Click(object sender, EventArgs e)
        {



            //define as propriedades do controle 
            //OpenFileDialog
            this.ofd1.Multiselect = false;
            this.ofd1.Title = "Selecionar PDF";
            ofd1.InitialDirectory = @"C:\dados";
            //filtra para exibir somente arquivos de imagens
            ofd1.Filter = "Files (*.PDF)|*.PDF|" + "All files (*.*)|*.*";
            ofd1.CheckFileExists = true;
            ofd1.CheckPathExists = true;
            ofd1.FilterIndex = 2;
            ofd1.RestoreDirectory = true;
            ofd1.ReadOnlyChecked = true;
            ofd1.ShowReadOnly = false;

            DialogResult dr = this.ofd1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {

                txtArquivoTexto.Text = ofd1.FileName;


                this.DataSource = ConverteFilePDF();

//                foreach (var item in this.DataSource) 
//                {
//                    item.existe = ExisteCod((int)item.Codigo);
//                }

                
                listData.DataSource = this.DataSource;

                txtTotal.Text = this.DataSource.Sum(pkg => pkg.valor).ToString();
            }
        }



        private List<ConvertePDF.DataLine> ConverteFilePDF()
        {
            try
            {
                ConvertePDF pdftxt = new ConvertePDF();
                string texto = pdftxt.ExtrairTexto_PDF(txtArquivoTexto.Text);

                this.FicheiroInfo = new Ficheiro(txtArquivoTexto.Text);
                int nLine = 0;

                if (this.FicheiroInfo.Servico == 3530)
                {
                    string a = string.Empty;
                    char[] chrCod = texto.ToCharArray();
                    List<string> li = new List<string>();
                    for (int i = 0; i < chrCod.Length; i++)
                    {
                        if (((byte)chrCod[i]) == (byte)10)
                        {
                            li.Add(a);
                            a = string.Empty;

                        }
                        else

                            a += (((byte)chrCod[i]) == (byte)32) ? "" : chrCod[i];

                    }

                    foreach (string line in li)
                    {
                        TxtLines.Text += line + Environment.NewLine;
                    }

                    TxtTexto.Text = texto;
                }
                else
                {
                    TxtTexto.Text = texto;

                    foreach (string line in TxtTexto.Lines)
                    {
                        TxtLines.Text += line + Environment.NewLine;

                    }
                }



                List<ConvertePDF.DataLine> lista = new List<ConvertePDF.DataLine>();

                if (texto.Contains("PJ - Polícia Judiciária"))
                    IdServico = 4025;
                else if (texto.Contains(" CAMARA MUNICIPAL DE ALMADA"))
                    IdServico = 3525;
                else if (texto.Contains("Instituto dos Registos e do Notariado"))
                    IdServico = 4997;
                else if (texto.Contains("INSTITUTO SEGURANCA SOCIAL, I.P."))
                    IdServico = 5167;
                else if (texto.Contains("MINISTÉRIO DA DEFESA NACIONAL"))
                    IdServico = 6400;
                else if (texto.Contains("COFRE"))
                {
                    if (texto.Contains("Descontos Cofre Prev"))
                        IdServico = 386;
                    else
                        IdServico = 3052;
                }
             //   else if (texto.Contains("Câmara Municipal de Sintra"))
             //       IdServico = 3405;
                else if (texto.Contains("Municipio do Barreiro"))
                    IdServico = 3530;

                else if (texto.Contains("Municipal de Sintra")) 
                    IdServico = 3541;

                else if (texto.Contains("Relação de Descontos"))
                    IdServico = 6299;
                else
                    IdServico = FicheiroInfo.Servico == null ? default : (int)FicheiroInfo.Servico;



                switch (IdServico)
                {
                    // Pulicia Judiciaria
                    case 4025:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine4025(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;
                    case 6400:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine6400(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;
                    case 3052:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine3052(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;

                    case 386:

                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine386(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;

                    case 3405:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine3405(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;

                    case 3530:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine5330(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;

                    case 3525:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine3525(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;


                    case 5167:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine5167(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;
                    case 3541:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine3405(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;

                    case 6299:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine6299(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;

                    case 4997:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine4997(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);
                        }

                        break;

                    default:
                        foreach (string line in TxtTexto.Lines)
                        {
                            nLine += 1;
                            ConvertePDF.DataLine dataline = pdftxt.Line2DataLine4025(line, nLine);
                            if (dataline != null && dataline.Codigo != null)
                                lista.Add(dataline);

                        }

                        break;
                }






                return lista;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file = ofd1.FileName.ToUpper().Replace("PDF", "CSV");
            ConvertePDF.CreateCSV(this.DataSource, file);
            MessageBox.Show(string.Format(@"Ficheiro {0} criado com sucesso", file));
        }

        private void btnAdicionarFichiro_Click(object sender, EventArgs e)
        {
            //define as propriedades do controle 
            //OpenFileDialog
            this.ofd1.Multiselect = false;
            this.ofd1.Title = "Selecionar PDF";
            ofd1.InitialDirectory = @"C:\dados";
            //filtra para exibir somente arquivos de imagens
            ofd1.Filter = "Files (*.PDF)|*.PDF|" + "All files (*.*)|*.*";
            ofd1.CheckFileExists = true;
            ofd1.CheckPathExists = true;
            ofd1.FilterIndex = 2;
            ofd1.RestoreDirectory = true;
            ofd1.ReadOnlyChecked = true;
            ofd1.ShowReadOnly = false;

            DialogResult dr = this.ofd1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                txtArquivoTexto.Text = ofd1.FileName;
                List<ConvertePDF.DataLine> lista = new List<ConvertePDF.DataLine>();
                lista = ConverteFilePDF();
                foreach (ConvertePDF.DataLine item in lista)
                {
                    this.DataSource.Add(item);
                }

                listData.DataSource = this.DataSource;
                listData.Refresh();

                txtTotal.Text = this.DataSource.Sum(pkg => pkg.valor).ToString();
                txtTotal.Refresh();
            }
            //this.Show();

        }

        private bool ExisteCod(int cod)
        {
            //string connectionString = @"Data Source=COFREAPP;Initial Catalog=CofreApp;Persist Security Info=True;User ID=sa;Password=password;MultipleActiveResultSets=True;";
            string connectionString = @"Data Source=srvprdsql01\ivs02;Initial Catalog=CofreApp;Persist Security Info=True;User ID=Svc_cofreapp_sql;Password=S!MMmg@5U6Xd21;MultipleActiveResultSets=True;";

            string sql =string.Format("select IdEntidade from TodasEntidades where NumEntidade = {0}",cod);

            SqlConnection cofreConnection = new SqlConnection(connectionString);
            cofreConnection.Open();

            SqlCommand cmd = new SqlCommand(sql, cofreConnection);
             
            cmd.CommandTimeout = 0;
           
            using (var reader = cmd.ExecuteReader())
            {
                return reader.Read();
                //while (reader.Read())
                //{

                //}
            }
            
        }

        private DataTable ListarRelDesc(int cod)
        {
            //string connectionString = @"Data Source=COFREAPP;Initial Catalog=CofreApp;Persist Security Info=True;User ID=sa;Password=password;MultipleActiveResultSets=True;";
            string connectionString = @"Data Source=srvprdsql01\ivs02;Initial Catalog=CofreApp;Persist Security Info=True;User ID=Svc_cofreapp_sql;Password=S!MMmg@5U6Xd21;MultipleActiveResultSets=True;";

            string sql = string.Format("exec ListarRelacoesDesconto", cod);

            SqlConnection cofreConnection = new SqlConnection(connectionString);
            cofreConnection.Open();


            SqlCommand cmd = new SqlCommand(sql, cofreConnection);

            SqlParameter p1 = new SqlParameter("@codigo", SqlDbType.Int);
            p1.Value = cod;

            cmd.Parameters.Add(p1);
            cmd.CommandTimeout = 0;

            DataTable dt = new DataTable();

            using (var reader = cmd.ExecuteReader())
            {
                //return reader.Read();
                while (reader.Read())
                {
                    dt.Rows.Add(reader);
                }
                return dt;
            }

        }


    }
}


