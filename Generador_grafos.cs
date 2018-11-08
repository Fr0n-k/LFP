using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace _LFP_Proyecto1_201504325
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Variables Globales
        Boolean V_Guard = false;
        String GR = "";
        String UPV = null;
        String UErr = null;
        String UTkn = null;
        String Nom_Arch = "";
        StreamWriter escribir;
        int Ln_Ac = 0;
        int Pst_Sig = 1;
        int PSt_ac = 0;
        int countnull = 0;
        int adir = 0;
        OpenFileDialog Arch = new OpenFileDialog();
        XmlDocument XTkns = new XmlDocument();
        XmlDocument XErr = new XmlDocument();
        SaveFileDialog SV_Arch = new SaveFileDialog();
        List<string> Tkns = new List<string>();
        List<int> Id_Tkn = new List<int>();
        List<string> Lex_Tkns = new List<string>();
        List<string> Err = new List<string>();
        List<int> Fil_Err = new List<int>();
        List<int> Col_Err = new List<int>();
        List<string> Desc_Err = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {
            Vents[0] = Cod_Arch_1;
            Tabs[0] = Arch_1;
            sts[0] = 1;
            for(int ac=1;ac<500; ac++)
            {
                Tabs[ac] = new TabPage();
                Tabs[ac].Text = "Archivo " + (ac + 1).ToString();
                Vents[ac] = new RichTextBox();
                Vents[ac].SetBounds(0, 0, 1110, 426);
                Tabs[ac].Controls.Add(Vents[ac]);
            }
        }
        TabPage[] Tabs = new TabPage[500];
        int[] estaopen = new int[500];
        int n = 1;
        TabPage[] TabsXME = new TabPage[500];
        int[] sts = new int[500];
        TabPage[] TabsXMT = new TabPage[500];
        TabPage[] TabsHTMT = new TabPage[500];
        TabPage[] TabsHTME = new TabPage[500];
        TabPage[] TabsGRs = new TabPage[500];
        RichTextBox[] Vents = new RichTextBox[500];
        PictureBox[] Grafs = new PictureBox[500];
        RichTextBox[] XMLsE = new RichTextBox[500];
        RichTextBox[] XMLsTk = new RichTextBox[500];
        WebBrowser[] HtErr = new WebBrowser[500];
        WebBrowser[] HtTK = new WebBrowser[500];
        string[] nomsarchs = new string[500];
        List<string> Cods = new List<string>();
        List<List<string>> ColTkn = new List<List<string>>();
        List<List<int>> ColId = new List<List<int>>();
        List<List<string>> ColLexTk= new List<List<string>>();
        List<List<int>> ColFil= new List<List<int>>();
        List<List<int>> ColCol = new List<List<int>>();
        List<List<string>> ColErr= new List<List<string>>();
        List<List<string>> ColDescErr = new List<List<string>>();

        //Nuevo Archivo
        private void Nuevo_Arch_Click(object sender, EventArgs e)
        {
            Archivos.Controls.Add(Tabs[Pst_Sig]);
            sts[Pst_Sig] = 1;
            Pst_Sig++;
            
        }

        //Abre el Archivo
        private async void Abrir_Arch_Click(object sender, EventArgs e)
        {
            countnull = 0;
            Arch.Filter = "grafo| *.grafo";
            Arch.ValidateNames = true;
            Arch.Multiselect = false;
            if (Arch.ShowDialog() == DialogResult.OK)
            {
                StreamReader Cont = new StreamReader(Arch.FileName);
                for(int ct = 0; ct < 500; ct++)
                {
                    if(Vents[ct].Text=="")
                    {
                        if (sts[ct] == 1)
                        {
                            Vents[ct].Text = await Cont.ReadToEndAsync();
                            Colors(Vents[ct]);
                            Cont.Dispose();
                            estaopen[ct] = 1;
                            nomsarchs[ct] = Arch.FileName;
                            break;
                        }
                        else
                        {
                            Archivos.TabPages.Add(Tabs[ct]);
                            Vents[ct].Text = await Cont.ReadToEndAsync();
                            estaopen[ct] = 1;
                            Colors(Vents[ct]);
                            nomsarchs[ct] = Arch.FileName;
                            Cont.Dispose();
                            break;
                        }
                        
                    }
                }
                
            }
        }

        //Guarda el archivo
        private void Guardar_Arch_Click(object sender, EventArgs e)
        {

            int tactual = Archivos.SelectedTab.TabIndex;
            PSt_ac = tactual;
            if (estaopen[PSt_ac] == 1)
            {
                StreamWriter Guardop = new StreamWriter(File.Create(nomsarchs[PSt_ac]));
                Guardop.Write(Vents[PSt_ac].Text);
                Guardop.Dispose();
            }
            else
            {
                SV_Arch.Filter = "grafo| *.grafo";
                SV_Arch.Title = "Guardar Archivo";
                if (SV_Arch.ShowDialog() == DialogResult.OK && SV_Arch.FileName.Length > 0)
                {
                    StreamWriter Guard = new StreamWriter(File.Create(SV_Arch.FileName));
                    Guard.Write(Vents[PSt_ac].Lines);
                    Guard.Dispose();
                }
                Nom_Arch = SV_Arch.FileName;
                nomsarchs[PSt_ac] = Nom_Arch;
                MessageBox.Show("Se guardo el Archivo");
                estaopen[PSt_ac] = 1;
            }
        }

        //Coloreador de palabras reservadas
        private void CheckKeyword(string word, Color color, int startIndex)
        {
            if (Pst_Sig == 0)
            {
                if (this.Cod_Arch_1.Text.Contains(word))
                {
                    int index = -1;
                    int selectStart = this.Cod_Arch_1.SelectionStart;

                    while ((index = this.Cod_Arch_1.Text.IndexOf(word, (index + 1))) != -1)
                    {
                        this.Cod_Arch_1.Select((index + startIndex), word.Length);
                        this.Cod_Arch_1.SelectionColor = color;
                        this.Cod_Arch_1.Select(selectStart, 0);
                        this.Cod_Arch_1.SelectionColor = Color.Black;
                    }
                }
            } else
            {
                for(int a = 0; a < 500; a++)
                {
                    if (Vents[a] == null)
                    {

                    }else
                    {
                        if (this.Vents[a].Text.Contains(word))
                        {
                            int index = -1;
                            int selectStart = this.Vents[a].SelectionStart;

                            while ((index = this.Vents[a].Text.IndexOf(word, (index + 1))) != -1)
                            {
                                this.Vents[a].Select((index + startIndex), word.Length);
                                this.Vents[a].SelectionColor = color;
                                this.Vents[a].Select(selectStart, 0);
                                this.Vents[a].SelectionColor = Color.Black;
                            }
                        }
                    }
                    
                }
            }

        }

        //Color
        private void Cod_Arch_1_TextChanged_1(object sender, EventArgs e)
        {
            int lnac = 0;
            String Pal = "";
            String[] cadena = Cod_Arch_1.Lines;
            //Reservadas
            this.CheckKeyword("grafo ", Color.Blue, 0);
            this.CheckKeyword("nodo", Color.Blue, 0);
            this.CheckKeyword("grafoDir", Color.Blue, 0);
            this.CheckKeyword("apunta", Color.Blue, 0);
            this.CheckKeyword("conecta", Color.Blue, 0);
            //Formas
            this.CheckKeyword("cuadrado", Color.Red, 0);
            this.CheckKeyword("rectángulo", Color.Red, 0);
            this.CheckKeyword("rectangulo", Color.Red, 0);
            this.CheckKeyword("circulo", Color.Red, 0);
            this.CheckKeyword("círculo", Color.Red, 0);
            this.CheckKeyword("hexágono", Color.Red, 0);
            this.CheckKeyword("hexagono", Color.Red, 0);
            this.CheckKeyword("documento", Color.Red, 0);
            this.CheckKeyword("diamante", Color.Red, 0);
            this.CheckKeyword("ovalo", Color.Red, 0);
            this.CheckKeyword("doblecirculo", Color.Red, 0);
            //Colores
            this.CheckKeyword("rojo", Color.Orange, 0);
            this.CheckKeyword("salmon", Color.Orange, 0);
            this.CheckKeyword("gris", Color.Orange, 0);
            this.CheckKeyword("negro", Color.Orange, 0);
            this.CheckKeyword("verde", Color.Orange, 0);
            this.CheckKeyword("azul", Color.Orange, 0);
            this.CheckKeyword("celeste", Color.Orange, 0);
            this.CheckKeyword("rosado", Color.Orange, 0);
            this.CheckKeyword("amarillo", Color.Orange, 0);
        }

        //Creador de la pagina de tokens
        private void Pag_Tkns(string @ubicacion, List<string> Tokens, List<string> Lexemas, List<int> ID)
        {
            FileStream escribir = File.Create(@ubicacion);
            Byte[] etiqueta;
            etiqueta = new UTF8Encoding(true).GetBytes("<html>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<head>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<title>Tokens</title>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<style>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("table, th, td{");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("        border: 1px solid Black;");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("        border-collapse: collapse;");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("}");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("th, td{");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("       padding: 6px");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("}");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</style>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</head>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<body> ");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<H1><CENTER><FONT FACE='Baskerville Old Face' COLOR='Black'><B>Listado de Tokens</B></FONT</CENTER></H1>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<CENTER>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TABLE>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TR>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='White'><B>No.</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='White'><B>Token</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='White'><B>Lexema</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='White'><B>ID</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</TR>");
            escribir.Write(etiqueta, 0, etiqueta.Length);

            for (int a = 0; a < Tokens.Count; a++)
            {
                if (a % 2 != 0)
                {
                    etiqueta = new UTF8Encoding(true).GetBytes("<TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='White'>" + (a + 1) + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='White'>" + Tokens[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='White'>" + Lexemas[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='White'>" + ID[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("</TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);

                }
                else
                {
                    etiqueta = new UTF8Encoding(true).GetBytes("<TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='Black'>" + (a + 1) + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='Black'>" + Tokens[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='Black'>" + Lexemas[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='Black'>" + ID[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("</TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                }
            }

            etiqueta = new UTF8Encoding(true).GetBytes("</TABLE>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</CENTER>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</BODY>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</HTML>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            escribir.Close();
        }

        //Creador de la pagina de errores
        public void Pag_Err(string @ubicacion, List<string> Error, List<string> Descripcion, List<int> Fila, List<int> columna)
        {
            FileStream escribir = File.Create(@ubicacion);
            Byte[] etiqueta;
            etiqueta = new UTF8Encoding(true).GetBytes("<html>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<head>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<title>Listado de Errores</title>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<style>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("table, th, td{");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("        border: 1px solid Black;");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("        border-collapse: collapse;");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("}");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("th, td{");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("       padding: 6px");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("}");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</style>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</head>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<body>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<H1><CENTER><FONT FACE='ARIAL' COLOR='Black'><B>Listado de Errores</B></FONT</CENTER></H1>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<CENTER>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TABLE>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TR>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='Black'><B>No.</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='Black'><B>Cararcter</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='Black'><B>Descripcion</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='Black'><B>Fila</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='Black'><B>Columna</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</TR>");
            escribir.Write(etiqueta, 0, etiqueta.Length);

            for (int b = 0; b < Error.Count; b++)
            {
                if (b % 2 != 0)
                {
                    etiqueta = new UTF8Encoding(true).GetBytes("<TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='Black'>" + (b + 1) + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='Black'>" + Error[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='Black'>" + Descripcion[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='Black'>" + Fila[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='Black'>" + columna[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("</TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                }
                else
                {
                    etiqueta = new UTF8Encoding(true).GetBytes("<TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='White'>" + (b + 1) + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='White'>" + Error[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='White'>" + Descripcion[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='White'>" + Fila[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE = 3 COLOR='White'>" + columna[b] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("</TR>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                }




            }

            etiqueta = new UTF8Encoding(true).GetBytes("</TABLE>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</CENTER>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</BODY>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("</HTML>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            escribir.Close();
        }

        //Ubicacion de los archivos de tokens y errores
        public string Ubi_Tkn_HTML()
        {
            string ubicacion = null;

            SV_Arch.Filter = "Archivo HTML(*.html)|*.html";

            if (SV_Arch.ShowDialog() == DialogResult.OK)
            {
                ubicacion = SV_Arch.FileName;
            }

            return ubicacion;
        }

        public string Ubi_Err_HTML()
        {
            string ubicacion = null;

            SV_Arch.Filter = "Archivo HTML(*.html)|*.html";

            if (SV_Arch.ShowDialog() == DialogResult.OK)
            {
                ubicacion = SV_Arch.FileName;
            }

            return ubicacion;
        }

        //Creador HTML de Tokens
        private void Rep_Tkns_HTML_Click(object sender, EventArgs e)
        {
            UTkn = Ubi_Tkn_HTML();
            Pag_Tkns(UTkn, Tkns, Lex_Tkns, Id_Tkn);
            MessageBox.Show("Se creo el archivo HTML de tokens");

            if (PSt_ac == 0)
            {
                HTM1.Navigate(UTkn);
            }
            else
            {

                TabsHTMT[PSt_ac] = new TabPage();
                HtTK[PSt_ac] = new WebBrowser();
                HtTK[PSt_ac].SetBounds(0, 0, 1131, 421);
                TabsHTMT[PSt_ac].Text = "HTML Tokens " + (PSt_ac + 1).ToString();
                HtmlToks.TabPages.Add(TabsHTMT[PSt_ac]);
                TabsHTMT[PSt_ac].Controls.Add(HtTK[PSt_ac]);
                HtTK[PSt_ac].Navigate(UTkn);
            }
        }

        //Creador HTML  de Errores
        private void Rep_Err_HTML_Click(object sender, EventArgs e)
        {
            UErr = Ubi_Err_HTML();
            Pag_Err(UErr, Err, Desc_Err, Fil_Err, Col_Err);
            MessageBox.Show("Se creo el archivo de HTML de errores");
            if (PSt_ac == 0)
            {
                Erro1.Navigate(UErr);

            }
            else
            {

                TabsHTME[PSt_ac] = new TabPage();
                HtErr[PSt_ac] = new WebBrowser();
                HtErr[PSt_ac].SetBounds(0, 0, 1131, 421);
                TabsHTME[PSt_ac].Text = "HTML Errores " + (PSt_ac + 1).ToString();
                HTErr.TabPages.Add(TabsHTME[PSt_ac]);
                TabsHTME[PSt_ac].Controls.Add(HtErr[PSt_ac]);
                HtErr[PSt_ac].Navigate(UErr);
            }
        }

        //Analiza el archivo del tab actual
        private void AnTxt_Click(object sender, EventArgs e)
        {
            GR = "";
            Tkns.Clear();
            Id_Tkn.Clear();
            Lex_Tkns.Clear();
            Err.Clear();
            Fil_Err.Clear();
            Col_Err.Clear();
            Desc_Err.Clear();
            int tactual = Archivos.SelectedTab.TabIndex;
            PSt_ac = tactual;
            String[] Ent = Vents[tactual].Lines;
            for (int indx = 0; indx < Ent.Length; indx++)
            {
                An_Lex(Ent[indx]);
            }
            MessageBox.Show("Termino el Análisis");
            ColTkn.Add(Tkns);
            ColLexTk.Add(Lex_Tkns);
            ColId.Add(Id_Tkn);
            ColErr.Add(Err);
            ColDescErr.Add(Desc_Err);
            ColFil.Add(Fil_Err);
            ColCol.Add(Col_Err);
            Cods.Add(GR);

            Console.WriteLine(GR);
            escribir = new StreamWriter(@"C:\release\a\Grafo.txt", false);
            escribir.WriteLine(GR);
            escribir.Close();
        }

        //Analizador Léxico
        public void An_Lex(String Entrada)
        {
            Ln_Ac++;
            String ln_aux = "";
            String ln_Ac = "";
            String nomnod = "";
            String ln_nums = "size = ";
            String ln_genLAb = "label = ";
            String ln_colo = "style = filled, color = ";
            String ln_fo = "shape = ";
            String ln_bg = "bgcolor=";
            int anodo = 0;
            bool emCorch = false;
            int pos = 0;
            int estado = 0;
            int Col_Ac = 1;
            int fin = Entrada.Length - 1;
            int archor = 0;
            int acomi = 0;
            int acolo = 0;
            int aform = 0;
            int aet = 0;
            int atk = 0;
            int acon = 0;
            int numnd = 0;
            while (pos < Entrada.Length)
            {
                switch (estado)
                {
                    case 0:
                        if (Entrada[pos] == '{')
                        {
                            estado = 1;

                        }
                        else if (Entrada[pos] == '}')
                        {
                            estado = 2;

                        }
                        else if (Entrada[pos] == '[' && !emCorch)
                        {
                            estado = 3;
                            emCorch = true;
                            pos++;
                        }
                        else if (Entrada[pos] == ',')
                        {
                            estado = 8;

                        }
                        else if (Entrada[pos] == ';')
                        {
                            estado = 9;

                        }
                        else if (Entrada[pos] == ']')
                        {
                            estado = 5;
                        }
                        else if ((Entrada[pos] == ',' || Entrada[pos] == ']' || Entrada[pos] == ';') || emCorch)
                        {
                            estado = 3;
                            pos++;
                        }
                        else if (Entrada[pos] == '[')
                        {
                            estado = 4;

                        }
                        else if (Entrada[pos] == '(')
                        {
                            estado = 6;

                        }
                        else if (Entrada[pos] == ')')
                        {
                            estado = 7;

                        }
                        else if (Entrada[pos] == '0' || Entrada[pos] == '1' || Entrada[pos] == '2' || Entrada[pos] == '3' || Entrada[pos] == '4' || Entrada[pos] == '5' || Entrada[pos] == '6' || Entrada[pos] == '7' || Entrada[pos] == '8' || Entrada[pos] == '9')
                        {
                            estado = 11;

                        }
                        else if ((65 <= Entrada[pos] && Entrada[pos] <= 90) || (97 <= Entrada[pos] && Entrada[pos] <= 122) || Entrada[pos] == 209 || Entrada[pos] == 241 || Entrada[pos] == 193 || Entrada[pos] == 201 || Entrada[pos] == 205 || Entrada[pos] == 211 || Entrada[pos] == 218 || Entrada[pos] == 225 || Entrada[pos] == 233 || Entrada[pos] == 237 || Entrada[pos] == 243 || Entrada[pos] == 250 || Entrada[pos] == '"')
                        {
                            estado = 10;

                        }
                        else if (Entrada[pos] == 32 || Entrada[pos] == '\t')
                        {
                            pos++;
                        }
                        else
                        {
                            Err.Add(Entrada[pos].ToString());
                            Desc_Err.Add("Caracter desconocido");
                            Fil_Err.Add(Ln_Ac);
                            Col_Err.Add(Col_Ac);
                            Col_Ac++;
                            pos++;
                        }
                        break;
                    case 1:
                        Tkns.Add("Tkn_LlaveAbrir");
                        Lex_Tkns.Add("{");
                        Id_Tkn.Add(1);
                        pos++;
                        Col_Ac++;
                        estado = 0;
                        ln_Ac = ln_Ac + "{";
                        break;
                    case 2:
                        Tkns.Add("Tkn_LlaveCerrar");
                        Lex_Tkns.Add("}");
                        Id_Tkn.Add(2);
                        pos++;
                        Col_Ac++;
                        estado = 0;
                        ln_Ac = ln_Ac + "}";
                        break;
                    case 3:
                        if (archor == 0)
                        {
                            Tkns.Add("Tkn_CorcheteAbrir");
                            Lex_Tkns.Add("[");
                            Id_Tkn.Add(3);
                            Col_Ac++;
                            archor++;
                        }
                        else
                        {
                            Tkns.Add("Tkn_Coma");
                            Lex_Tkns.Add(",");
                            Id_Tkn.Add(7);
                            Col_Ac++;
                        }
                     
                        int num = 0;
                        for (int postx = pos; postx < Entrada.Length; postx++)
                        {
                            if (Entrada[postx] == 48 || Entrada[postx] == 49 || Entrada[postx] == 50 || Entrada[postx] == 51 || Entrada[postx] == 52 || Entrada[postx] == 53 || Entrada[postx] == 54 || Entrada[postx] == 55 || Entrada[postx] == 56 || Entrada[postx] == 57)
                            {

                                ln_aux = ln_aux + Entrada[postx].ToString();
                                ln_nums = ln_nums + Entrada[pos].ToString();
                                pos++;
                            }
                            else if (Entrada[postx] == ',' || Entrada[postx] == ']')
                            {
                                postx = Entrada.Length;
                                num = 1;
                            }
                            else if (Entrada[postx] == '"')
                            {
                                for (int posaux = postx; posaux < Entrada.Length; posaux++)
                                {
                                    if (Entrada[posaux] == ',' || Entrada[posaux] == ']')
                                    {
                                        posaux = Entrada.Length;
                                        postx = Entrada.Length;
                                        num = 0;
                                    }
                                    else
                                    {
                                        ln_aux = ln_aux + Entrada[posaux].ToString();
                                        ln_genLAb = ln_genLAb + Entrada[posaux];
                                        pos++;
                                    }
                                }
                            }
                            else if ((65 <= Entrada[postx] && Entrada[postx] <= 90) || (97 <= Entrada[postx] && Entrada[postx] <= 122) || Entrada[postx] == 209 || Entrada[postx] == 241 || Entrada[postx] == 193 || Entrada[postx] == 201 || Entrada[postx] == 205 || Entrada[postx] == 211 || Entrada[postx] == 218 || Entrada[postx] == 225 || Entrada[postx] == 233 || Entrada[postx] == 237 || Entrada[postx] == 243 || Entrada[postx] == 250)
                            {
                                for (int poslet = postx; poslet < Entrada.Length; poslet++)
                                {
                                    if (Entrada[poslet] == ',' || Entrada[poslet] == ']')
                                    {
                                        poslet = Entrada.Length;
                                        postx = Entrada.Length;
                                        num = 2;
                                        switch (ln_aux)
                                        {
                                            case "celeste":
                                                ln_bg = ln_bg + "lightblue1";
                                                break;
                                            case "gris":
                                                ln_bg = ln_bg + "lightgrey";
                                                break;
                                            case "negro":
                                                ln_bg = ln_bg + "black";
                                                break;
                                            case "salmon":
                                                ln_bg = ln_bg + "salmon";
                                                break;
                                            case "rosado":
                                                ln_bg = ln_bg + "pink";
                                                break;
                                            case "amarillo":
                                                ln_bg = ln_bg + "yellow";
                                                break;
                                            case "verde":
                                                ln_bg = ln_bg + "green";
                                                break;
                                            case "azul":
                                                ln_bg = ln_bg + "blue";
                                                break;
                                            case "rojo":
                                                ln_bg = ln_bg + "red";
                                                break;
                                        }
                                    }
                                    else if ((65 <= Entrada[poslet] && Entrada[poslet] <= 90) || (97 <= Entrada[poslet] && Entrada[poslet] <= 122) || Entrada[poslet] == 209 || Entrada[poslet] == 241 || Entrada[poslet] == 193 || Entrada[poslet] == 201 || Entrada[poslet] == 205 || Entrada[poslet] == 211 || Entrada[poslet] == 218 || Entrada[poslet] == 225 || Entrada[poslet] == 233 || Entrada[poslet] == 237 || Entrada[poslet] == 243 || Entrada[poslet] == 250)
                                    {
                                        ln_aux = ln_aux + Entrada[poslet].ToString();
                                        pos++;
                                    }
                                    else
                                    {
                                        Err.Add(Entrada[pos].ToString());
                                        Desc_Err.Add("Caracter desconocido");
                                        Fil_Err.Add(Ln_Ac);
                                        Col_Err.Add(Col_Ac);
                                        Col_Ac++;
                                        pos++;
                                    }
                                }
                            }
                            else if (Entrada[postx] == 32 || Entrada[postx] == '\t')
                            {
                                pos++;
                            }
                            else
                            {
                                Err.Add(Entrada[pos].ToString());
                                Desc_Err.Add("Caracter desconocido");
                                Fil_Err.Add(Ln_Ac);
                                Col_Err.Add(Col_Ac);
                                Col_Ac++;
                                pos++;
                            }
                        }
                        if (num == 0)
                        {
                            Tkns.Add("Tkn_Identificador");
                            Lex_Tkns.Add(ln_aux);
                            Id_Tkn.Add(10);
                            pos++;
                            Col_Ac++;
                            estado = 0;
                            ln_aux = "";
                            ln_Ac = ln_Ac + ln_genLAb + ";\n";
                        }
                        else if (num == 1)
                        {
                            Tkns.Add("Tkn_Numero");
                            Lex_Tkns.Add(ln_aux);
                            Id_Tkn.Add(9);
                            pos++;
                            Col_Ac++;
                            estado = 0;
                            ln_aux = "";
                            ln_Ac = ln_Ac + ln_nums +";\n";
                        }
                        else
                        {
                            Tkns.Add("Tkn_Color");
                            Lex_Tkns.Add(ln_aux);
                            Id_Tkn.Add(11);
                            pos++;
                            Col_Ac++;
                            estado = 0;
                            ln_aux = "";
                            ln_Ac = ln_Ac + ln_bg + "";
                        }
                        //emCorch = false;
                        break;
                    case 4:
                        Tkns.Add("Tkn_CorcheteAbrir");
                        Lex_Tkns.Add("[");
                        Id_Tkn.Add(3);
                        pos++;
                        Col_Ac++;
                        estado = 0;
                        ln_Ac = ln_Ac + "[";
                        break;
                    case 5:
                        Tkns.Add("Tkn_CorcheteCerrar");
                        Lex_Tkns.Add("]");
                        Id_Tkn.Add(4);
                        pos++;
                        Col_Ac++;
                        estado = 0;
                        ln_Ac = ln_Ac + "]";
                        break;
                    case 6:
                        Tkns.Add("Tkn_ParentesisAbrir");
                        Lex_Tkns.Add("(");
                        Id_Tkn.Add(5);
                        pos++;
                        Col_Ac++;
                        estado = 0;
                        ln_Ac = ln_Ac + "[";
                        break;
                    case 7:
                        Tkns.Add("Tkn_ParentesisCerrar");
                        Lex_Tkns.Add(")");
                        Id_Tkn.Add(6);
                        pos++;
                        Col_Ac++;
                        estado = 0;
                        if ((acolo == 1 && aform == 1 && aet == 1 && acon == 0) || acon == 1) 
                        {
                            ln_Ac = ln_Ac + "]";
                        }
                        else if (acolo == 0 && aform == 1 && aet == 1 && acon == 0)
                        {
                            ln_Ac = ln_Ac + " ,style = filled, color = lightblue1]";
                        }
                        else if (acolo == 1 && aform == 0 && aet == 1 && acon == 0)
                        {
                            ln_Ac = ln_Ac + " ,shape = oval]";
                        }
                        else if (acolo == 1 && aform == 1 && aet == 0 && acon == 0)
                        {
                            ln_Ac = ln_Ac + " ,label = " + '"' + nomnod + '"'+"]";
                        }
                        else if (acolo == 0 && aform == 0 && aet == 1 && acon == 0)
                        {
                            ln_Ac = ln_Ac + " ,shape = oval, style = filled, color = lightblue1]";
                        }
                        else if (acolo == 0 && aform == 1 && aet == 0 && acon == 0)
                        {
                            ln_Ac = ln_Ac + " ,label = " + '"' + nomnod + '"' + ", style = filled, color = lightblue1]";
                        }
                        else if (acolo == 1 && aform == 0 && aet == 0 && acon == 0)
                        {
                            ln_Ac = ln_Ac + " ,label = " + '"' + nomnod + '"' + ", shape = oval]";
                        }
                        else if (acolo == 0 && aform == 0 && aet == 0 && acon == 0)
                        {
                            ln_Ac = ln_Ac + "label = " + '"' + nomnod + '"' + ", shape = oval , style = filled, color = lightblue1]";
                        }
                        break;
                    case 8:
                        Tkns.Add("Tkn_Coma");
                        Lex_Tkns.Add(",");
                        Id_Tkn.Add(7);
                        pos++;
                        Col_Ac++;
                        estado = 0;
                        ln_Ac = ln_Ac + ",";
                        break;
                    case 9:
                        
                        if (archor != 0)
                        {
                            Tkns.Add("Tkn_CorcheteCerrar");
                            Lex_Tkns.Add("]");
                            Id_Tkn.Add(4);
                            Tkns.Add("Tkn_PuntoComa");
                            Lex_Tkns.Add(";");
                            Id_Tkn.Add(8);
                            pos++;
                            Col_Ac++;
                            estado = 0;
                            ln_Ac = ln_Ac + ";";
                        }
                        else
                        {
                            Tkns.Add("Tkn_PuntoComa");
                            Lex_Tkns.Add(";");
                            Id_Tkn.Add(8);
                            pos++;
                            Col_Ac++;
                            estado = 0;
                            ln_Ac = ln_Ac + ";";
                        }
                        break;
                    case 10:
                        for(int poss = pos; poss < Entrada.Length; poss++)
                        {
                            if (Entrada[poss] == '"')
                            {
                                atk = 0;
                                for(int posi = poss; posi < Entrada.Length; posi++)
                                {
                                    if ((Entrada[posi] == ')' || Entrada[posi] == ',') && acomi == 2)
                                    {
                                        posi = Entrada.Length;
                                        poss = Entrada.Length;
                                    }
                                    else if (Entrada[posi] == '"') 
                                    {
                                        acomi++;
                                        ln_aux = ln_aux + Entrada[posi].ToString();
                                        ln_genLAb = ln_genLAb + Entrada[posi].ToString();
                                        pos++;
                                    }
                                    else
                                    {
                                        ln_aux = ln_aux + Entrada[posi].ToString();
                                        ln_genLAb = ln_genLAb + Entrada[posi].ToString();
                                        pos++;
                                    }
                                }
                                aet = 1;
                                Tkns.Add("Tkn_Identificador");
                                Lex_Tkns.Add(ln_aux);
                                Id_Tkn.Add(10);
                                Col_Ac++;
                                estado = 0;
                                ln_aux = "";
                                ln_Ac = ln_Ac + ln_genLAb;
                            }
                            else if (Entrada[poss] == 32 || Entrada[poss] == '\t') 
                            {
                                pos++;
                            }
                            else if ((65 <= Entrada[poss] && Entrada[poss] <= 90) || (97 <= Entrada[poss] && Entrada[poss] <= 122) || Entrada[poss] == 209 || Entrada[poss] == 241 || Entrada[poss] == 193 || Entrada[poss] == 201 || Entrada[poss] == 205 || Entrada[poss] == 211 || Entrada[poss] == 218 || Entrada[poss] == 225 || Entrada[poss] == 233 || Entrada[poss] == 237 || Entrada[poss] == 243 || Entrada[poss] == 250)
                            {
                                for (int posi = poss; posi < Entrada.Length; posi++)
                                {

                                    if ((65 <= Entrada[posi] && Entrada[posi] <= 90) || (97 <= Entrada[posi] && Entrada[posi] <= 122) || Entrada[posi] == 209 || Entrada[posi] == 241 || Entrada[posi] == 193 || Entrada[posi] == 201 || Entrada[posi] == 205 || Entrada[posi] == 211 || Entrada[posi] == 218 || Entrada[posi] == 225 || Entrada[posi] == 233 || Entrada[posi] == 237 || Entrada[posi] == 243 || Entrada[posi] == 250|| Entrada[posi] == 48 || Entrada[posi] == 49 || Entrada[posi] == 50 || Entrada[posi] == 51 || Entrada[posi] == 52 || Entrada[posi] == 53 || Entrada[posi] == 54 || Entrada[posi] == 55 || Entrada[posi] == 56 || Entrada[posi] == 57)
                                    {
                                        ln_aux = ln_aux + Entrada[posi].ToString();
                                        pos++;
                                    }
                                    else if ( Entrada[posi] == 32 || Entrada[posi] == ',' || Entrada[posi] == '{' || Entrada[posi] == ')') 
                                    {
                                        posi = Entrada.Length;
                                        poss = Entrada.Length;
                                    }
                                    else if(Entrada[posi] == '(')
                                    {
                                        posi = Entrada.Length;
                                        poss = Entrada.Length;
                                        numnd++;
                                    }
                                    else
                                    {
                                        Err.Add(Entrada[pos].ToString());
                                        Desc_Err.Add("Caracter desconocido");
                                        Fil_Err.Add(Ln_Ac);
                                        Col_Err.Add(Col_Ac);
                                        Col_Ac++;
                                        pos++;
                                    }
                                }
                                if (String.Equals(ln_aux, "grafo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    Tkns.Add("Tkn_Grafo");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(16);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + "graph ";
                                }
                                else if (String.Equals(ln_aux, "grafoDir", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    adir = 1;
                                    Tkns.Add("Tkn_GrafoDir");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(17);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + "digraph ";
                                }
                                else if (String.Equals(ln_aux, "conecta", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    if (adir == 1)
                                    {
                                        acon = 1;
                                        Err.Add(ln_aux);
                                        Desc_Err.Add("Caracter invalido para grafodir");
                                        Fil_Err.Add(Ln_Ac);
                                        Col_Err.Add(Col_Ac);
                                        Col_Ac++;
                                    }
                                    else
                                    {
                                        acon = 1;
                                        Tkns.Add("Tkn_Conecta");
                                        Lex_Tkns.Add(ln_aux);
                                        Id_Tkn.Add(14);
                                        Col_Ac++;
                                        estado = 0;
                                        ln_aux = "";
                                        ln_Ac = ln_Ac + " -- ";
                                    }
                                }
                                else if (String.Equals(ln_aux, "apunta", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    if (adir == 1)
                                    {
                                        acon = 1;
                                        Tkns.Add("Tkn_Apunta");
                                        Lex_Tkns.Add(ln_aux);
                                        Id_Tkn.Add(15);
                                        Col_Ac++;
                                        estado = 0;
                                        ln_aux = "";
                                        ln_Ac = ln_Ac + " -> ";
                                    }
                                    else
                                    {
                                        acon = 1;
                                        Err.Add(ln_aux);
                                        Desc_Err.Add("Caracter invalido para grafo");
                                        Fil_Err.Add(Ln_Ac);
                                        Col_Err.Add(Col_Ac);
                                        Col_Ac++;
                                    }
                                }
                                else if (String.Equals(ln_aux, "nodo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    anodo = 1;
                                    Tkns.Add("Tkn_Nodo");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(13);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + "node ";
                                }
                                else if (String.Equals(ln_aux, "rectangulo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "rectangle";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "ovalo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "oval";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "circulo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "circle";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "doblecirculo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "doublecircle";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "diamante", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "diamond";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "documento", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "note";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "cuadrado", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "square";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "hexagono", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    aform = 1;
                                    ln_fo = ln_fo + "hexagon";
                                    Tkns.Add("Tkn_Forma");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(12);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_fo;
                                }
                                else if (String.Equals(ln_aux, "celeste", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "lightblue1";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "amarillo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "yellow";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "rojo", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "red";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "verde", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "green";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "azul", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "blue";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "rosado", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "pink";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    pos++;
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "negro", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "black";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "salmon", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "salmon";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else if (String.Equals(ln_aux, "gris", StringComparison.InvariantCultureIgnoreCase) == true)
                                {
                                    atk = 1;
                                    acolo = 1;
                                    ln_colo = ln_colo + "grey";
                                    Tkns.Add("Tkn_Color");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(11);
                                    Col_Ac++;
                                    estado = 0;
                                    ln_aux = "";
                                    ln_Ac = ln_Ac + ln_colo;
                                }
                                else
                                {
                                    Tkns.Add("Tkn_Identificador");
                                    Lex_Tkns.Add(ln_aux);
                                    Id_Tkn.Add(10);
                                    Col_Ac++;
                                    estado = 0;

                                    if (numnd == 0)
                                    {
                                        ln_Ac = ln_Ac + ln_aux;
                                    }
                                    else
                                    {
                                        nomnod = ln_aux;
                                    }
                                    ln_aux = "";
                                }
                            }
                            else
                            {
                                Err.Add(Entrada[pos].ToString());
                                Desc_Err.Add("Caracter desconocido");
                                Fil_Err.Add(Ln_Ac);
                                Col_Err.Add(Col_Ac);
                                Col_Ac++;
                                pos++;
                            }
                        }
                        break;
                    case 11:
                        for (int posnum = pos; posnum < Entrada.Length; posnum++)
                        {
                            if (Entrada[pos] == '0' || Entrada[pos] == '1' || Entrada[pos] == '2' || Entrada[pos] == '3' || Entrada[pos] == '4' || Entrada[pos] == '5' || Entrada[pos] == '6' || Entrada[pos] == '7' || Entrada[pos] == '8' || Entrada[pos] == '9')
                            {
                                ln_aux = ln_aux + Entrada[posnum].ToString();
                                ln_nums = ln_nums + Entrada[pos].ToString();
                                pos++;
                            }
                            else
                            {
                                posnum = Entrada.Length;
                            }
                            
                        }
                        Tkns.Add("Tkn_Numero");
                        Lex_Tkns.Add(ln_aux);
                        Id_Tkn.Add(9);
                        Col_Ac++;
                        estado = 0;
                        ln_aux = "";
                        ln_Ac = ln_Ac + ln_nums + ";\n";
                        break;
                }
            }
            if (anodo >= 1 && nomnod!="")
            {
                ln_Ac = ln_Ac + nomnod + ";";
            }
            GR = GR + ln_Ac + "\n";
        }

        //Creador XML
        private async void Rep_Err_XML_Click(object sender, EventArgs e)
        {
            StreamReader XT;
            XmlElement RotErr = XErr.CreateElement("Errores");
            XmlElement error;
            XmlElement fila;
            XmlElement columna;
            XmlElement simbolo;
            XErr.AppendChild(RotErr);
            for (int Xac = 0; Xac < Err.Count; Xac++)
            {
                int nomn = Xac + 1;
                error = XErr.CreateElement(nomn.ToString());
                RotErr.AppendChild(error);

                fila = XErr.CreateElement("Fila");
                fila.AppendChild(XErr.CreateTextNode(Fil_Err[Xac].ToString()));
                error.AppendChild(fila);

                columna = XErr.CreateElement("Columna");
                columna.AppendChild(XErr.CreateTextNode(Col_Err[Xac].ToString()));
                error.AppendChild(columna);

                simbolo = XErr.CreateElement("Símbolo");
                simbolo.AppendChild(XErr.CreateTextNode(Err[Xac]));
                error.AppendChild(simbolo);
            }
            XErr.Save("C:\\Users\\fcarv\\Documents\\Universidad\\Lenguajes Formales\\Lab\\Proyecto1\\Errores" +PSt_ac.ToString()+".xml");
            XT= new StreamReader("C:\\Users\\fcarv\\Documents\\Universidad\\Lenguajes Formales\\Lab\\Proyecto1\\Errores" + PSt_ac.ToString() + ".xml");
            if (PSt_ac == 0)
            {
                XMErr1.Text = await XT.ReadToEndAsync();
            }
            else
            {
                TabsXME[PSt_ac] = new TabPage();
                XMLsE[PSt_ac] = new RichTextBox();
                XMLsE[PSt_ac].SetBounds(0, 0, 1131, 421);
                TabsXME[PSt_ac].Text = "XML Errores " + (PSt_ac + 1).ToString();
                XErrores.TabPages.Add(TabsXME[PSt_ac]);
                TabsXME[PSt_ac].Controls.Add(XMLsE[PSt_ac]);
                XMLsE[PSt_ac].Multiline = true;
                XMLsE[PSt_ac].Text =await XT.ReadToEndAsync();
            }
        }

        private async void Rep_Tkns_XML_Click(object sender, EventArgs e)
        {
            StreamReader XE;
            XmlElement Rot = XTkns.CreateElement("Tokens");
            XmlElement token;
            XmlElement nombre;
            XmlElement lexema;
            XTkns.AppendChild(Rot);
            for (int Xac = 0; Xac < Tkns.Count; Xac++)
            {
                String nomn = "TokenId:" + Id_Tkn[Xac].ToString();
                token = XTkns.CreateElement(nomn);
                Rot.AppendChild(token);

                nombre = XTkns.CreateElement("Nombre");
                nombre.AppendChild(XTkns.CreateTextNode(Tkns[Xac]));
                token.AppendChild(nombre);

                lexema = XTkns.CreateElement("Lexema");
                lexema.AppendChild(XTkns.CreateTextNode(Lex_Tkns[Xac]));
                token.AppendChild(lexema);
            }

            XTkns.Save("C:\\Users\\fcarv\\Documents\\Universidad\\Lenguajes Formales\\Lab\\Proyecto1\\Tokens" + PSt_ac.ToString() + ".xml");
            XE = new StreamReader("C:\\Users\\fcarv\\Documents\\Universidad\\Lenguajes Formales\\Lab\\Proyecto1\\Tokens" + PSt_ac.ToString() + ".xml");
            if (PSt_ac == 0)
            {
                XM1.Text = await XE.ReadToEndAsync();
            }
            else
            {

                TabsXMT[PSt_ac] = new TabPage();
                XMLsTk[PSt_ac] = new RichTextBox();
                XMLsTk[PSt_ac].SetBounds(0, 0, 1131, 421);
                TabsXMT[PSt_ac].Text = "XML Tokens " + (PSt_ac + 1).ToString();
                XTokens.TabPages.Add(TabsXMT[PSt_ac]);
                TabsXMT[PSt_ac].Controls.Add(XMLsTk[PSt_ac]);
                XMLsTk[PSt_ac].Multiline = true;
                XMLsTk[PSt_ac].Text =await XE.ReadToEndAsync();
            }
        }

        //Metodo de colores
        private void Colors(RichTextBox ac)
        {
            this.Refresh();
            this.CheckKeyword("grafo ", Color.Blue, 0);
            this.CheckKeyword("nodo", Color.Blue, 0);
            this.CheckKeyword("grafoDir", Color.Blue, 0);
            this.CheckKeyword("apunta", Color.Blue, 0);
            this.CheckKeyword("conecta", Color.Blue, 0);
            //Formas
            this.CheckKeyword("cuadrado", Color.Red, 0);
            this.CheckKeyword("rectángulo", Color.Red, 0);
            this.CheckKeyword("rectangulo", Color.Red, 0);
            this.CheckKeyword("circulo", Color.Red, 0);
            this.CheckKeyword("círculo", Color.Red, 0);
            this.CheckKeyword("hexágono", Color.Red, 0);
            this.CheckKeyword("hexagono", Color.Red, 0);
            this.CheckKeyword("documento", Color.Red, 0);
            this.CheckKeyword("diamante", Color.Red, 0);
            this.CheckKeyword("ovalo", Color.Red, 0);
            this.CheckKeyword("doblecirculo", Color.Red, 0);
            //Colores
            this.CheckKeyword("rojo", Color.Orange, 0);
            this.CheckKeyword("salmon", Color.Orange, 0);
            this.CheckKeyword("gris", Color.Orange, 0);
            this.CheckKeyword("negro", Color.Orange, 0);
            this.CheckKeyword("verde", Color.Orange, 0);
            this.CheckKeyword("azul", Color.Orange, 0);
            this.CheckKeyword("celeste", Color.Orange, 0);
            this.CheckKeyword("rosado", Color.Orange, 0);
            this.CheckKeyword("amarillo", Color.Orange, 0);
        }

         //Ver Grafo en la Aplicacion
        private void Ver_Graph_Click(object sender, EventArgs e)
        {
            if (PSt_ac == 0)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
                startInfo.Arguments = "-Tpng C:\\release\\a\\Grafo.txt -o C:\\release\\a\\Gr.png";
                Gr_Arch_1.ImageLocation = "C:\\release\\a\\Gr.png";
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
                startInfo.Arguments = "-Tpng C:\\release\\a\\Grafo.txt -o C:\\release\\a\\Gr.png";
                TabsGRs[PSt_ac] = new TabPage();
                Grafs[PSt_ac] = new PictureBox();
                Grafs[PSt_ac].SetBounds(0, 0, 1110, 426);
                TabsGRs[PSt_ac].Text = "Grafo " + (PSt_ac + 1).ToString();
                Grafos.TabPages.Add(TabsGRs[PSt_ac]);
                TabsGRs[PSt_ac].Controls.Add(Grafs[PSt_ac]);
                Grafs[PSt_ac].ImageLocation = "C:\\release\\a\\Gr.png";
            }


        }

        //Abrir grafo actual
        private void Abrir_Graph_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\release\\a\\GR.png");
        }

        //Manual Tecnico
        private void manualTecnicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Users\\fcarv\\Documents\\Universidad\\Lenguajes Formales\\Lab\\Proyecto1\\[LFP]Proyecto1_201504325\\[LFP]Proyecto1_Manual_Tecnico_201504325.pdf");


        }

        //Manual de Usuario
        private void manualDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Users\\fcarv\\Documents\\Universidad\\Lenguajes Formales\\Lab\\Proyecto1\\[LFP]Proyecto1_201504325\\[LFP]Proyecto1_Manual_de_Usuario_201504325.pdf");

        }

        private void Archivo_Click(object sender, EventArgs e)
        {

        }
    }
}
