using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace _LFP_Practica1_201504325
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Variables Globales
        Boolean V_Guard = false;
        String HTML = "";
        String UPV = null;
        String UErr = null;
        String UTkn = null;
        String Nom_Arch = "";
        int Ln_Ac = 0;
        OpenFileDialog Arch = new OpenFileDialog();
        SaveFileDialog SV_Arch = new SaveFileDialog();
        List<string> Tkns = new List<string>();
        List<int> Id_Tkn = new List<int>();
        List<string> Lex_Tkns = new List<string>();
        List<int> Fil_Tkns = new List<int>();
        List<int> Col_Tkns = new List<int>();
        List<string> Err = new List<string>();
        List<int> Fil_Err = new List<int>();
        List<int> Col_Err = new List<int>();
        List<string> Desc_Err= new List<string>();

        //Botón para abrir el archivo
        private async void Abr_Arch_Click(object sender, EventArgs e)
        {
            Arch.Filter = "lfp| *.lfp";
            Arch.ValidateNames = true;
            Arch.Multiselect = false;
            if (Arch.ShowDialog() == DialogResult.OK)
            {
                StreamReader Cont = new StreamReader(Arch.FileName);
                Cod_Pag.Text = await Cont.ReadToEndAsync();
            }
        }

        //Botón para guardar el archivo
        private void Grd_Arch_Click(object sender, EventArgs e)
        {
            if (V_Guard)
            {
                StreamWriter Guard = new StreamWriter(File.Create(Nom_Arch));
                Guard.Write(HTML);
                Guard.Dispose();
                MessageBox.Show("Se guardo el Archivo");
            }
            else
            {
                SV_Arch.Filter = "HTML| *.html";
                SV_Arch.Title = "Guardar Archivo";
                if (SV_Arch.ShowDialog() == DialogResult.OK && SV_Arch.FileName.Length > 0)
                {
                    StreamWriter Guard = new StreamWriter(File.Create(SV_Arch.FileName));
                    Guard.Write(HTML);
                    Guard.Dispose();
                }
                Nom_Arch = SV_Arch.FileName;
                MessageBox.Show("Se guardo el Archivo");
                V_Guard = true;
            }
        }

        //Botón para abrir la lista de tokens
        private void Abr_Tkns_Click(object sender, EventArgs e)
        {
            UTkn = Ubi_Tkn();
            Pag_Tkns(UTkn, Tkns, Lex_Tkns, Id_Tkn, Fil_Tkns, Col_Tkns);
            MessageBox.Show("Se creo el archivo de tokens");
            if (UTkn != null)
            {
                Process.Start(UTkn);
            }
        }

        //Botón para abrir la lista de errores
        private void Abr_Err_Click(object sender, EventArgs e)
        {
            UErr = Ubi_Err();
            Pag_Err(UErr, Err, Desc_Err, Fil_Err, Col_Err);
            MessageBox.Show("Se creo el archivo de errores");
            if (UErr != null)
            {
                Process.Start(UErr);
            }
        }

        //Botón de la vista previa
        private void Vista_Click(object sender, EventArgs e)
        {
            String[] Ent = Cod_Pag.Lines;
            for (int indx = 0; indx < Ent.Length; indx++)
            {
                AN_Lex(Ent[indx]);
            }
            UPV = "C:\\Users\\Francisco\\Documents\\Universidad\\Lenguajes Formales\\Lab\\Vista.html";
            StreamWriter Guard = new StreamWriter(File.Create(UPV));
            Guard.Write(HTML);
            Guard.Dispose();
            Process.Start(UPV);
            }

        //Botón para crear nueva página
        private void Cr_Pag_Click(object sender, EventArgs e)
        {
            V_Guard = false;
            HTML = "";
            Cod_Pag.Text = "";
            Ln_Ac = 0;
            Tkns.Clear();
            Id_Tkn.Clear();
            Lex_Tkns.Clear();
            Fil_Tkns.Clear();
            Col_Tkns.Clear();
            Err.Clear();
            Fil_Err.Clear();
            Col_Err.Clear();
            Desc_Err.Clear();
        }

        //Botón para analizar el archivo
        private void Analizar_Click(object sender, EventArgs e)
        {
            Id_Tkn.Clear();
            Lex_Tkns.Clear();
            Fil_Tkns.Clear();
            Col_Tkns.Clear();
            Err.Clear();
            Fil_Err.Clear();
            Col_Err.Clear();
            Desc_Err.Clear();
            String[] Ent = Cod_Pag.Lines;
            for (int indx = 0; indx < Ent.Length; indx++)
            {
                AN_Lex(Ent[indx]);
            }
            MessageBox.Show("Termino el Análisis");
        }

        //Analizador Léxico
        private void AN_Lex(String Entrada)
        {
            Ln_Ac++;
            String P_Res = "";
            String[] posarrs;
            int posaux = 0;
            int posfarr = 0;
            int n= 0;
            int narr = 0;
            int No_Arr = 0;
            int pos = 0;
            int estado = 0;
            int Col_Ac = 1;
            for(int pre=0; pre < Entrada.Length; pre++)
            {
                if (Entrada[pre] == 64)
                {
                    n++;
                }
                if (n == 2 && posfarr==0)
                {
                    posfarr = pre;
                }

            }
            posarrs = new String[n];
            for (int pre = 0; pre < Entrada.Length; pre++)
            {
                if (Entrada[pre] == 64)
                {
                    narr++;
                }
                if (narr == n - 1 && posaux==0)
                {
                    posaux = pre;

                }
            }

            while (pos < Entrada.Length)
            {
                switch (estado)
                {
                    case 0:
                        if (Entrada[pos] == 64)
                        {
                            estado = 1;
                            No_Arr++;
                        }
                        else if (Entrada[pos] == 63)
                        {
                            estado = 2;
                        }
                        else  if (Entrada[pos]=='\t')
                        {
                            pos++;
                        } 
                        else if ((65 <= Entrada[pos] && Entrada[pos] <= 90) || (97 <= Entrada[pos] && Entrada[pos] <= 122) || Entrada[pos] == 33 || Entrada[pos] == 209 || Entrada[pos] == 241 || Entrada[pos] == 193 || Entrada[pos] == 201 || Entrada[pos] == 205 || Entrada[pos] == 211 || Entrada[pos] == 218 || Entrada[pos] == 225 || Entrada[pos] == 233 || Entrada[pos] == 237 || Entrada[pos] == 243 || Entrada[pos] == 250 || Entrada[pos] == 46 || Entrada[pos] == 58 || Entrada[pos] == 32)
                        {
                            estado = 3;
                        }
                        else
                        {
                            Err.Add(Entrada[pos].ToString());
                            Desc_Err.Add("Caracter desconocido");
                            Fil_Err.Add(Ln_Ac);
                            Col_Err.Add(Col_Ac);
                            pos++;
                        }
                        break;
                    case 1:
                        Tkns.Add("Arroba");
                        Lex_Tkns.Add(Entrada[pos].ToString());
                        Id_Tkn.Add(1);
                        Fil_Tkns.Add(Ln_Ac);
                        Col_Tkns.Add(Col_Ac);
                        Col_Ac++;
                        pos++;
                        estado = 0;
                        if (No_Arr % 2 != 0)
                        {
                            HTML = HTML + "<";
                        } else if(pos+1>=Entrada.Length)
                        {
                            HTML = HTML + "> \n";
                        }
                        else
                        {
                            HTML = HTML + ">";
                        }
                        break;
                    case 2:
                        Tkns.Add("Cierre_Etiqueta");
                        Lex_Tkns.Add(Entrada[pos].ToString());
                        Id_Tkn.Add(2);
                        Fil_Tkns.Add(Ln_Ac);
                        Col_Tkns.Add(Col_Ac);
                        Col_Ac++;
                        pos++;
                        HTML = HTML + "/";
                        estado = 0;
                        break;
                    case 3:
                        for (int rec = pos; rec < Entrada.Length; rec++)
                        {
                            if ((Entrada[rec] == 64 && rec == posfarr) || (Entrada[rec] == 64 && rec == posaux) || (Entrada[rec] == 64 && rec == Entrada.Length-1) || (Entrada[rec] == 63 && rec == Entrada.Length - 2))
                            {
                                pos = rec;
                                rec = Entrada.Length;
                            }
                            else if ((65 <= Entrada[rec] && Entrada[rec] <= 90) || (97 <= Entrada[rec] && Entrada[rec] <= 122) || (48 <= Entrada[rec] && Entrada[rec] <= 57) || Entrada[rec] == 33 || Entrada[rec] == 209 || Entrada[rec] == 241 || Entrada[rec] == 193 || Entrada[rec] == 201 || Entrada[rec] == 205 || Entrada[rec] == 211 || Entrada[rec] == 218 || Entrada[rec] == 225 || Entrada[rec] == 233 || Entrada[rec] == 237 || Entrada[rec] == 243 || Entrada[rec] == 250 || Entrada[rec] == 46 || Entrada[rec] == 58 || Entrada[rec] == 32)
                            {
                                P_Res = P_Res + Entrada[rec].ToString();
                                pos++;
                            }
                            else
                            {
                                Err.Add(Entrada[rec].ToString());
                                Desc_Err.Add("Caracter desconocido");
                                Fil_Err.Add(Ln_Ac);
                                Col_Err.Add(Col_Ac);
                                pos++;
                            }
                        }
                        switch (P_Res)
                        {
                            case "quickhtml":
                                Tkns.Add("ETQ_quickhtml");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(3);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "html";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "encabezado":
                                Tkns.Add("ETQ_encabezado");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(4);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "head";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "titulo":
                                Tkns.Add("ETQ_titulo");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(5);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "title";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "cuerpo":
                                Tkns.Add("ETQ_cuerpo");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(6);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "body";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "textogrande":
                                Tkns.Add("ETQ_textogrande");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(7);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "h1";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "textomediano":
                                Tkns.Add("ETQ_textomediano");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(8);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "h2";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "textopequeno":
                                Tkns.Add("ETQ_textopequeno");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(9);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "h3";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "saltoln":
                                Tkns.Add("ETQ_saltoln");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(10);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "br";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "tabla":
                                Tkns.Add("ETQ_tabla");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(11);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "table";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "filat":
                                Tkns.Add("ETQ_filat");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(12);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "tr";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "columnat":
                                Tkns.Add("ETQ_columnat");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(13);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "td";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "formulario":
                                Tkns.Add("ETQ_formulario");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(14);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "form";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "ingreso":
                                Tkns.Add("ETQ_ingreso");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(15);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "input";
                                P_Res = "";
                                estado = 0;
                                break;
                            case "boton":
                                Tkns.Add("ETQ_boton");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(16);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                HTML = HTML + "button";
                                P_Res = "";
                                estado = 0;
                                break;
                            default:
                                Tkns.Add("Texto");
                                Lex_Tkns.Add(P_Res);
                                Id_Tkn.Add(17);
                                Fil_Tkns.Add(Ln_Ac);
                                Col_Tkns.Add(Col_Ac);
                                Col_Ac++;
                                if (pos < Entrada.Length)
                                {
                                    HTML = HTML + P_Res + "\n";
                                    P_Res = "";
                                    estado = 0;

                                }
                                else
                                {
                                    HTML = HTML + P_Res;
                                    P_Res = "";
                                    pos = Entrada.Length;

                                }
                                break;
                        }
                        break;
                        
                    }
                }
            }

        //Creador de la pagina de tokens
        private void Pag_Tkns(string @ubicacion, List<string> Tokens, List<string> Lexemas, List<int> ID, List<int> Fila, List<int> Columna)
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
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='White'><B>Fila</B></FONT></TH>");
            escribir.Write(etiqueta, 0, etiqueta.Length);
            etiqueta = new UTF8Encoding(true).GetBytes("<TH BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=4 COLOR='White'><B>Columna</B></FONT></TH>");
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
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='White'>" + Fila[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='Black'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='White'>" + Columna[a] + "</FONT></TD>");
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
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='Black'>" + Fila[a] + "</FONT></TD>");
                    escribir.Write(etiqueta, 0, etiqueta.Length);
                    etiqueta = new UTF8Encoding(true).GetBytes("<TD BGCOLOR='White'><FONT FACE='Baskerville Old Face' SIZE=3 COLOR='Black'>" + Columna[a] + "</FONT></TD>");
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
                } else
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

        //Richtextbox
        private void Cod_Pag_TextChanged(object sender, EventArgs e)
        {
            //Aperturas
            this.CheckKeyword("@quickhtml@", Color.Blue, 0);
            this.CheckKeyword("@encabezado@", Color.Blue, 0);
            this.CheckKeyword("@titulo@", Color.Blue, 0);
            this.CheckKeyword("@cuerpo@", Color.Blue, 0);
            this.CheckKeyword("@textogrande@", Color.Blue, 0);
            this.CheckKeyword("@textomediano@", Color.Blue, 0);
            this.CheckKeyword("@textopequeno@", Color.Blue, 0);
            this.CheckKeyword("@saltoln?@", Color.Blue, 0);
            this.CheckKeyword("@tabla@", Color.Blue, 0);
            this.CheckKeyword("@filat@", Color.Blue, 0);
            this.CheckKeyword("@columnat@", Color.Blue, 0);
            this.CheckKeyword("@formulario@", Color.Blue, 0);
            this.CheckKeyword("@ingreso?@", Color.Blue, 0);
            this.CheckKeyword("@boton@", Color.Blue, 0);
            //Cierres
            this.CheckKeyword("@?quickhtml@", Color.Blue, 0);
            this.CheckKeyword("@?encabezado@", Color.Blue, 0);
            this.CheckKeyword("@?titulo@", Color.Blue, 0);
            this.CheckKeyword("@?cuerpo@", Color.Blue, 0);
            this.CheckKeyword("@?textogrande@", Color.Blue, 0);
            this.CheckKeyword("@?textomediano@", Color.Blue, 0);
            this.CheckKeyword("@?textopequeno@", Color.Blue, 0);
            this.CheckKeyword("@?tabla@", Color.Blue, 0);
            this.CheckKeyword("@?filat@", Color.Blue, 0);
            this.CheckKeyword("@?columnat@", Color.Blue, 0);
            this.CheckKeyword("@?formulario@", Color.Blue, 0);
            this.CheckKeyword("@?boton@", Color.Blue, 0);
        }

        //Ubicacion de los archivos de tokens y errores y vista previa
        public string Ubi_Tkn()
        {
            string ubicacion = null;

            SV_Arch.Filter = "Archivo HTML(*.html)|*.html";

            if (SV_Arch.ShowDialog() == DialogResult.OK)
            {
                ubicacion = SV_Arch.FileName;
            }

            return ubicacion;
        }

        public string Ubi_Err()
        {
            string ubicacion = null;

            SV_Arch.Filter = "Archivo HTML(*.html)|*.html";

            if (SV_Arch.ShowDialog() == DialogResult.OK)
            {
                ubicacion = SV_Arch.FileName;
            }

            return ubicacion;
        }

        public string Ubi_VP()
        {
            string ubicacion = null;

            SV_Arch.Filter = "Archivo HTML(*.html)|*.html";

            if (SV_Arch.ShowDialog() == DialogResult.OK)
            {
                ubicacion = SV_Arch.FileName;
            }

            return ubicacion;
        }

        //Coloreador de palabras reservadas
        private void CheckKeyword(string word, Color color, int startIndex)
        {
            if (this.Cod_Pag.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.Cod_Pag.SelectionStart;

                while ((index = this.Cod_Pag.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.Cod_Pag.Select((index + startIndex), word.Length);
                    this.Cod_Pag.SelectionColor = color;
                    this.Cod_Pag.Select(selectStart, 0);
                    this.Cod_Pag.SelectionColor = Color.Black;
                }
            }
        }
    }
}
