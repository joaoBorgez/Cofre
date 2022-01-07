


using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Converte_PDF_Texto
{
    public class ConvertePDF
    {
        public string ExtrairTexto_PDF(string caminho)
        {
            using (PdfReader leitor = new PdfReader(caminho))
            {
                StringBuilder texto = new StringBuilder();
                for (int i = 1; i <= leitor.NumberOfPages; i++)
                {
                    texto.Append(PdfTextExtractor.GetTextFromPage(leitor, i));
                }
                return texto.ToString();
            }
        }

        public string lineTmp = string.Empty;

        public class DataLine
        {
            public int? Codigo { get; set; }
            public double? valor { get; set; }
            public string Nome { get; set; }
            public string Line { get; set; }
            public int nLine { get; set; }
            public bool existe { get; set; }
        }


        #region Decod Servicos 

        #region 4025
        public DataLine Line2DataLine4025(string line, int nLine)
        {
            int start = line.IndexOf(' ');
            if (start <= 0)
                return null;
            string cod = line.Substring(0, start - 1);
            string valorStr = string.Empty;
            start = line.LastIndexOf('€');
            if (start >= 0)
            {
                int lenValorStr = line.Length - start;

                valorStr = line.Substring(start + 1, line.Length - start - 1);
            }
            else
                valorStr = "";



            DataLine dataline = new DataLine();
            dataline.Codigo = CodigoString2int(cod);
            dataline.valor = ValorString2Value(valorStr);
            dataline.Line = line;
            dataline.nLine = nLine;
            if (dataline.Codigo == null)
                return null;
            else

                return dataline;
        }
        #endregion

        #region 6400
        public DataLine Line2DataLine6400(string line, int nLine)
        {
            int start = line.IndexOf(' ');
            if (start <= 0)
                return null;
            string cod = line.Substring(0, start);
            string valorStr = string.Empty;
            start = line.LastIndexOf('€');
            if (start >= 0)
            {
                int lenValorStr = line.Length - start;

                valorStr = line.Substring(line.Length - 6, 6);
            }
            else
                valorStr = "";



            DataLine dataline = new DataLine();
            dataline.Codigo = CodigoString2int(cod);
            dataline.valor = ValorString2Value(valorStr);
            dataline.Line = line;
            if (dataline.Codigo == null)
                return null;
            else

                return dataline;
        }
        #endregion

        #region 3052        
        public DataLine Line2DataLine3052(string line, int nLine)
        {
            DataLine dt = new DataLine();
            bool isValor = false;
            string chr1 = string.Empty;
            string valorStr = string.Empty;


            for (int i = line.Length; i == 0; i--)
            {
                chr1 = line.Substring(i - 1, 1);
                if (chr1 == "€")
                    isValor = true;
                if (chr1 == " " && isValor)

                    if (isValor)
                        valorStr += chr1;

            }

            return dt;
        }
        #endregion

        #region 386
        public DataLine Line2DataLine386(string line, int nLine)
        {

            DataLine dt = new DataLine();
            string nome = string.Empty;
            string valorStr = string.Empty;
            string lineStr = string.Empty;
            int startCol = 0;

            //"0----+----1----+----2----+----3----+----4----+----5----+----6----+----7----+----2----+----3----+----4----+----5----+----6----+-";
            //"01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            // 43 - VICE - PRESIDÊNCIA DO GOVERNO REGIONAL E DOS ASSUNTOS PARLAMENTARES 3712 AUTORIDADE TRIBUTÁRIA E ASSUNTOS FISCAIS DA RAM 2242 2242 - COFRE PREV.FUNC.E AGENTES DO ESTADO 3,33 101843 Maria Helena Drumond Aragão Vital
            // 43 - VICE - PRESIDÊNCIA DO GOVERNO REGIONAL E DOS ASSUNTOS PARLAMENTARES 3710 DIREÇÃO REGIONAL DO ORÇAMENTO E TESOURO 2242 2242 - COFRE PREV.FUNC.E AGENTES DO ESTADO 9,61 11021 Maria Carmo Lino Figueira Gouveia Camacho


            if (line.Length < 104)
                return null;


            string TipoLine = line.Substring(0, 2);
            switch (TipoLine)
            {
                // Pulicia Judiciaria
                case "43":
                    if (line.Contains("GABINETE DO VICE"))
                        startCol = 154;
                    else if (line.Contains("DIREÇÃO REGIONAL DO ORÇAMENTO E TESOURO"))
                        startCol = 166;
                    else
                        startCol = 174;
                    break;
                case "45":
                    if (line.Contains("DIREÇÃO REGIONAL DE ADMINISTRAÇÃO ESCOLAR"))
                        startCol = 166;
                    else
                        startCol = 143;
                    break;
                case "49":
                    startCol = 181;
                    break;
                case "51":
                    startCol = 157;
                    break;
                case "52":
                    startCol = 155;
                    break;
                default:
                    startCol = 174;
                    break;
            }


            lineStr = line.Substring(startCol, line.Length - startCol).Trim();


            string[] lineSplit = lineStr.Split(' ');
            if (lineSplit.Length == 0)
                return null;

            valorStr = lineSplit[0];

            if (lineSplit.Length < 2)
                return null;

            string cod = lineSplit[1];

            for (int i = 2; i < lineSplit.Length; i++)
            {
                nome += string.Format("{0} ", lineSplit[i]);
            }

            if (CodigoString2int(cod) > 0)
            {
                dt.Codigo = CodigoString2int(cod);
                dt.valor = ValorString2Value(valorStr);
                dt.Nome = nome;
                dt.nLine = nLine;
                dt.Line = lineStr;
            }

            return dt;
        }
        #endregion

        #region 3405
        public DataLine Line2DataLine3405(string line, int nLine)
        {
            int lenLine = line.Length;
            DataLine dt = new DataLine();
            string nome = string.Empty;
            string valorStr = string.Empty;
            string cod = string.Empty;

            if (line.Contains("1 - Geral"))
                return null;

            if (!string.IsNullOrEmpty(lineTmp))
            {
                line = string.Format("{0} {1}", lineTmp, line);
                lineTmp = string.Empty;
            }

            string[] lineSplit = line.Split(' ');

            if (lineSplit.Length == 1)
            {

                if (CodigoString2int(lineSplit[0]) > 0)
                {
                    lineTmp = lineSplit[0];
                    return null;
                }
            }

            cod = lineSplit[0];
            valorStr = lineSplit[lineSplit.Length - 1];

            if (line.LastIndexOf(' ') - line.IndexOf(' ') > 0)
                nome = line.Substring(line.IndexOf(' '), line.LastIndexOf(' ') - line.IndexOf(' '));
            else
                nome = string.Empty;


            if (CodigoString2int(cod) > 0)
            {
                dt.Codigo = CodigoString2int(cod);
                dt.valor = ValorString2Value(valorStr);
                dt.Nome = nome;
                dt.Line = line;
                dt.nLine = nLine;
            }


            return dt;
        }



        #endregion

        #region 5330
        public DataLine Line2DataLine5330(string line, int nLine)
        {
            DataLine dt = new DataLine();
            string nome = string.Empty;
            string valorStr = string.Empty;
            string cod = string.Empty;


            line = line.Replace(" ", "");

            char[] chrCod = line.ToCharArray();
            for (int i = 0; i < chrCod.Length; i++)
            {
                if (Char.IsDigit(chrCod[i]))
                    cod += chrCod[i].ToString();
                else
                    break;
            }

            if (string.IsNullOrEmpty(cod) && string.IsNullOrEmpty(lineTmp))
                return null;

            if (cod.Length == 1)
                return null;

            if (cod.Length == line.Length)
            {
                lineTmp = cod;
                return null;
            }


            int? startValor = 0;
            for (int i = chrCod.Length - 1; i >= 0; i--)
            {
                if ((!Char.IsDigit(chrCod[i]) && chrCod[i] != ','))
                {
                    startValor = i + 1;
                    break;
                }
            }
            if (startValor == null || startValor == 0)
                return null;

            valorStr = line.Substring((int)startValor, line.Length - (int)startValor);
            if (string.IsNullOrEmpty(valorStr))
                return null;

            if (!string.IsNullOrEmpty(lineTmp) && !string.IsNullOrEmpty(valorStr))
            {
                //line = string.Format("{0} {1}", lineTmp, line);
                cod = lineTmp;
                lineTmp = string.Empty;
            }

            if (string.IsNullOrEmpty(cod) || string.IsNullOrEmpty(valorStr))
                return null;





            if (CodigoString2int(cod) > 0)
            {
                dt.Codigo = CodigoString2int(cod);
                dt.valor = ValorString2Value(valorStr);
                dt.Nome = nome;
                dt.Line = line;
                dt.nLine = nLine;
            }


            return dt;
        }


        #endregion

        #region 3525
        public DataLine Line2DataLine3525(string line, int nLine)
        {
            //"0----+----1----+----2----+----3----+----4----+----5----+----6----+----7----+----2----+----3----+----4----+----5----+----6----+-";
            //"01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            //"96748         134370430 ALBERTINO TIMAS SILVA                                          73,55      "

            int lenLine = line.Length;
            DataLine dt = new DataLine();
            string nome = string.Empty;
            string valorStr = string.Empty;
            string cod = string.Empty;
            line = line.Trim();


            string[] lineSplit = line.Split(' ');

            if (line.Contains("ENTIDADE ANO ECONÓMICO"))
                return null;

            if (lineSplit.Length <= 2)
                return null;

            List<string> la = new List<string>();

            for (int i = 0; i <= lineSplit.Length - 1; i++)
            {
                if (i == 0 && !string.IsNullOrEmpty(lineSplit[i]) && lineSplit[i].Contains("."))
                    lineSplit[i] = lineSplit[i].Replace(".", "");
                if (!string.IsNullOrEmpty(lineSplit[i]))
                    la.Add(lineSplit[i]);
            }
            lineSplit = la.ToArray();

            if (ValorString2Value(lineSplit[lineSplit.Length - 1]) == null)
                return null;

            if ((CodigoString2int(lineSplit[0]) != null && CodigoString2int(lineSplit[0]) > 0) && CodigoString2int(lineSplit[1]) != null && CodigoString2int(lineSplit[1]) > 0) // 2 Codigos
                cod = lineSplit[1];
            else if ((CodigoString2int(lineSplit[0]) != null && CodigoString2int(lineSplit[0]) > 0) && (CodigoString2int(lineSplit[1]) == null || CodigoString2int(lineSplit[1]) == 0)) // 1 Codigos
                cod = lineSplit[0];
            else
                return null;

            if (lineSplit.Length - 1 > 0)
                valorStr = lineSplit[lineSplit.Length - 1];

            for (int i = 1; i < lineSplit.Length - 2; i++)
            {
                if (!string.IsNullOrEmpty(lineSplit[i]))
                    nome += string.Format("{0} ", lineSplit[i]);
            }

            if (CodigoString2int(cod) > 0)
            {
                dt.Codigo = CodigoString2int(cod);
                dt.valor = ValorString2Value(valorStr);
                dt.Nome = nome;
                dt.Line = line;
                dt.nLine = nLine;
            }


            return dt;
        }
        #endregion

        #region 5167
        public DataLine Line2DataLine5167(string line, int nLine)
        {
            //"0----+----1----+----2----+----3----+----4----+----5----+----6----+----7----+----2----+----3----+----4----+----5----+----6----+-";
            //"01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            // 84514 ANA MARIA TOMAZ GOMES 0,00 - 104,24
            // 65013 CREMILDE JESUS NOBRE CASQUEIRA MARTINS 0,00 - 105,96

            int lenLine = line.Length;
            DataLine dt = new DataLine();
            string nome = string.Empty;
            string valorStr = string.Empty;
            string cod = string.Empty;

            string[] lineSplit = line.Split(' ');

            if (CodigoString2int(lineSplit[0]) == null || CodigoString2int(lineSplit[0]) == 0)
                return null;

            cod = lineSplit[0];
            if (lineSplit.Length - 1 > 0)
                valorStr = lineSplit[lineSplit.Length - 1];

            for (int i = 1; i < lineSplit.Length - 2; i++)
            {
                nome += string.Format("{0} ", lineSplit[i]);
            }

            if (CodigoString2int(cod) > 0)
            {
                dt.Codigo = CodigoString2int(cod);
                dt.valor = ValorString2Value(valorStr);
                dt.Nome = nome;
                dt.Line = line;
                dt.nLine = nLine;
            }


            return dt;
        }
        #endregion

        #region 6299
        public DataLine Line2DataLine6299(string line, int nLine)
        {
            int lenLine = line.Length;
            DataLine dt = new DataLine();
            string nome = string.Empty;
            string valorStr = string.Empty;

            int? cod = 0;

            if (line.Length < 10)
                return null;

            if (line.IndexOf('€') == -1)
                return null;



            string[] lineSplit = line.Split(' ');
            if (lineSplit.Length == 0)
                return null;

            //"0----+----1----+----2----+----3----+----4----+----5----+----6----+-";
            //"0123456789012345678901234567890123456789012345678901234567890123456";
            // 30077440 83988 191849170 DULCE JANELAS Cofre Prev F / Ag Estado - 0.00€          5.70€
            // 077426 126262209 MARIA BAPTISTA Cofre Prev F / Ag Estado - 0.00€         22.11€

            if (CodigoString2int(lineSplit[2]) == null)
            {
                cod = CodigoString2int(lineSplit[1]);
                nome = string.Format("{0} {1}", lineSplit[2], lineSplit[3]);
            }
            else
            {
                cod = CodigoString2int(lineSplit[2]);
                nome = string.Format("{0} {1}", lineSplit[3], lineSplit[4]);
            }
            valorStr = lineSplit[lineSplit.Length - 1];

            valorStr = valorStr.Replace('€', ' ').Replace('.', ',').Trim();

            double? v = ValorString2Value(valorStr); ;

            if (cod > 0)
            {
                dt.Codigo = cod;
                dt.valor = v;
                dt.Nome = nome;
                dt.Line = line;
                dt.nLine = nLine;
            }


            return dt;
        }
        #endregion

        #region 4997        
        public DataLine Line2DataLine4997(string line, int nLine)
        {
            int lenLine = line.Length;
            DataLine dt = new DataLine();
            string nome = string.Empty;
            string valorStr = string.Empty;
            string cod = string.Empty;
            string subscritor = string.Empty;

            if (line.Contains("COFRE PREVIDENCIA ") ||
                line.Contains("COFRE DA PREVIDENCIA DOS FUNCIONARIOS E AGENTES DO ESTADO") ||
                line.Contains("RELAÇÃO DE DESCONTOS"))
                return null;

            if (line.Contains("SRH"))
            {
                string[] divSrh = line.Split("SRH");
                if (string.IsNullOrEmpty(divSrh[0]))
                    line = divSrh[1];
                else 
                    line = divSrh[0];
            }

            string[] lineSplit = line.Split(' ');

            if (lineSplit.Length > 0)
            {

                if (CodigoString2int(lineSplit[0]) == null || CodigoString2int(lineSplit[0]) == 0)
                {
                    return null;
                }

            }
            else
                return null;


            subscritor = lineSplit[0];

            char[] chrCod = lineSplit[1].ToCharArray();
            for (int i = 0; i < chrCod.Length; i++)
            {
                if (Char.IsDigit(chrCod[i]))
                    cod += chrCod[i].ToString();
                else
                    nome += chrCod[i].ToString();
            }


            valorStr = lineSplit[lineSplit.Length - 2];

            //if (line.LastIndexOf(' ') - line.IndexOf(' ') > 0)
            //    nome = line.Substring(line.IndexOf(' '), line.LastIndexOf(' ') - line.IndexOf(' '));
            //else
            //    nome = string.Empty;

            if (CodigoString2int(cod) == 968)
                nome = string.Empty;

            for (int i = 2; i < lineSplit.Length; i++)
            {
                nome += string.Format("{0} ", lineSplit[i]);
                if (string.IsNullOrEmpty(lineSplit[i]))
                    break;
            }


            if (CodigoString2int(cod) > 0)
            {
                dt.Codigo = CodigoString2int(cod);
                dt.valor = ValorString2Value(valorStr);
                dt.Nome = nome;
                dt.Line = line;
                dt.nLine = nLine;
            }


            return dt;
        }
        #endregion

        #region CSV
        private static void CreateHeader<T>(List<T> list, StreamWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }
            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRowsTClasse<T>(List<T> list, StreamWriter sw)
        {

            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }

        private static void CreateRows(List<DataLine> list, StreamWriter sw)
        {

            sw.Write(string.Format(@"{0};{1};{2};{3};{4}", "NumSocio", "Nome", "NumMecanografico", "NumContribuinte", "Valor") + sw.NewLine);
            foreach (DataLine item in list)
            {
                string line = string.Format(@"{0};{1};{2};{3};{4}", item.Codigo, item.Nome, string.Empty, string.Empty, item.valor);
                sw.Write(line + sw.NewLine);
            }
        }

        public static void CreateCSV(List<DataLine> list, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                CreateRows(list, sw);
            }
        }

        //public static void CreateCSV<T>(List<T> list, string filePath)
        //{
        //    using (StreamWriter sw = new StreamWriter(filePath))
        //    {
        //        CreateHeader(list, sw);
        //        CreateRows(list, sw);
        //    }
        //}



        #endregion

        #endregion Servicos

        #region Metodos

        public double? ValorString2Value(string v)
        {
            double x = 0;
            bool isNegativo = false;

            if (v.Contains('-'))
            {
                v = v.Replace('-', ' ').Trim();
                isNegativo = true;
            }

            if (double.TryParse(v, out x) && x > 0)
                return x * (isNegativo ? -1 : 1);
            else
                return null;

        }

        public int? CodigoString2int(string codStr)
        {
            int x = 0;

            if (Int32.TryParse(codStr, out x) && x > 0)

                return x;
            else
                return null;
        }

        #endregion Private 




    }
}







