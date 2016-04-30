using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labs
{
    class InputClass
    {
        public string fileName { get; private set; }
       
        private ListBox thisList, outList;
        private ComboBox incombo,startCombo,finishCombo;
        private int n = 0, m = 0, D=0,r=0;
        private int[,] mainMassive, incident, sumig,incidentT,sumigT; 
        private int[] stepenVhodu,excntri,center,counts,countsT,weight;
        //DataGridView dataGr;
        private int[,] geodez,vidstan,dosiazhnosti;
        List<string> circle=new List<string> {};
        List<string> circle1 = new List<string> { };
        Dictionary<string, int> ToWeight = new Dictionary<string, int>();
        bool fl,izo=false;
        int[] sorting;

        public InputClass(string name,ListBox box,ListBox box2,ComboBox combo,ComboBox combo1,ComboBox combo2)
        {
            fileName = name;
            thisList = box;
            incombo = combo;
            outList = box2;
            startCombo = combo1;
            finishCombo = combo2;
        }

        public void Output()
        {
            
            int[] k;
            string str = "";

            using (StreamReader reader = new StreamReader(fileName))
            {
                str = reader.ReadLine();
                k = Cutting(str);

                n = k[0];
                m = k[1];
            

            mainMassive = new int[m + 1, 2];
            mainMassive[0, 0] = n;
            mainMassive[0, 1] = m;

                
                int j = 0;

                while ((str=reader.ReadLine()) !=null)
                {
                    j++;
                    k = Cutting(str);
                    mainMassive[j, 0] = k[0];
                    mainMassive[j, 1] = k[1];
                }

                for (int i = 0; i < m+1; i++)
                {
                    thisList.Items.Add(mainMassive[i, 0] + " " + mainMassive[i, 1] + "\n");
                }
             
            }

            for(int i=0;i< n;i++)
            {
                incombo.Items.Add(i + 1);
            }


        }

        public void Output(bool fl1)
        {
            int[] k;
            int[] toDict = new int[2];
            string str = "",
            key = "";

            using (StreamReader reader = new StreamReader(fileName))
            {
                str = reader.ReadLine();
                k = Cutting(str,true);

                n = k[0];
                m = k[1];


                mainMassive = new int[m + 1, 3];
                weight = new int[m];
                mainMassive[0, 0] = n;
                mainMassive[0, 1] = m;
                mainMassive[0, 0] = 0;


                int j = 0;

                while ((str = reader.ReadLine()) != null)
                {
                    j++;
                    k = Cutting(str,true);
                    toDict[0] = k[0];
                    toDict[1] = k[1];
                    mainMassive[j, 0] = toDict[0];
                    mainMassive[j, 1] = toDict[1];
                    weight[j - 1] = k[2];
                    mainMassive[j, 2] = k[2];
                    key = k[0] + " " + k[1];

                    ToWeight.Add(key,k[2]);
                }

                for (int i = 0; i < m + 1; i++)
                {
                    thisList.Items.Add(mainMassive[i, 0] + "  " + mainMassive[i, 1] + "  " + mainMassive[i,2] + "\n");
                }

            }

            for (int i = 0; i < n; i++)
            {
                incombo.Items.Add(i + 1);
                finishCombo.Items.Add(i + 1);
                startCombo.Items.Add(i + 1);
            }
        }

        private int[] Cutting(string input)
        {
            int[] res = new int[2];
            int indexProb = input.LastIndexOf(' ');

            string s1 = input.Substring(0, indexProb),
             s2 = input.Substring(indexProb + 1);

            res[0] = Convert.ToInt32(s1);
            res[1]= Convert.ToInt32(s2);

            return res;
        }

        private int[] Cutting(string input,bool fl1)
        {
            int[] res = new int[3];
            int indexProb = input.LastIndexOf(' ');
            string[] words = input.Split(' ') ;


            res[0] = Convert.ToInt32(words[0]);
            res[1] = Convert.ToInt32(words[1]);

            if(words.Length>2)
            res[2]= Convert.ToInt32(words[2]);

            return res;
        }

        public void OutputMassive(bool fl1)
        {
            fl = fl1;

            if (fl1 == false)
            {
                CreateIncident();
                CreativeSumig();
            }
            else
            {
                CreateIncident(fl);
                CreativeSumig(fl);
            }

            outList.Items.Add("");
           
        }

        private void CreateIncident(bool fl1)
        {
            incident = new int[n, m];
            incidentT = new int[n, m];
            string s = "";

            outList.Items.Add("Матриця інцидентості");
            outList.Items.Add("");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if ((mainMassive[j + 1, 0] == i + 1) && (mainMassive[j + 1, 1] == i + 1))
                        incident[i, j] = 2;
                    else
                    {
                        if (mainMassive[j + 1, 0] == i + 1)
                            incident[i, j] = -1;

                        else
                        {
                            if (mainMassive[j + 1, 1] == i + 1)
                                incident[i, j] = 1;
                            else
                                incident[i, j] = 0;
                        }
                    }

                    s += FormativeOut(incident[i, j]);

                }

                outList.Items.Add(s);

                s = "";

            }

            for(int i=0;i< n;i++)
            {
                for(int j=0;j< m;j++)
                {
                    incidentT[i, j] = 0;
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if(incident[i,j]==1)
                    {
                        incidentT[i, j] = -1;
                    }

                    if (incident[i, j] == -1)
                    {
                        incidentT[i, j] = 1;
                    }
                }
            }

        }

        private void CreativeSumig(bool fl1)
        {
            sumig = new int[n, n];
            sorting = new int[n];
            sumigT = new int[n, n];

            for (int i = 0; i < n; i++)
                sorting[i] = -1;

            string s = "";


            outList.Items.Add("");
            outList.Items.Add("Матриця суміжності");
            outList.Items.Add("");



            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    sumig[i, j] = 0;
            }

            for (int i = 0; i < m; i++)
            {
                sumig[mainMassive[i + 1, 0] - 1, mainMassive[i + 1, 1] - 1] = 1;
                sumigT[mainMassive[i + 1, 1] - 1, mainMassive[i + 1, 0] - 1] = 1;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    s += FormativeOut(sumig[i, j]);

                outList.Items.Add(s);
                s = "";
            }

        }

        private void CreateIncident()
        {
            incident = new int[n, m];
            string s = "";

            outList.Items.Add("Матриця інцидентості");
            outList.Items.Add("");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                        if ((mainMassive[j + 1, 0] == i + 1) || (mainMassive[j + 1, 1] == i + 1))
                            incident[i, j] =1;

                        else
                        {
                                incident[i, j] = 0;
                        }
                    

                    s += FormativeOut(incident[i, j]);

                }

                outList.Items.Add(s);

                s = "";

            }
        }

        private void CreativeSumig()
        {
            sumig = new int[n, n];

            string s = "";


            outList.Items.Add("");
            outList.Items.Add("Матриця суміжності");
            outList.Items.Add("");



            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    sumig[i, j] = 0;
            }

            for (int i = 0; i < m; i++)
            {
                sumig[mainMassive[i + 1, 0] - 1, mainMassive[i + 1, 1] - 1] = 1;
                sumig[mainMassive[i + 1, 1] - 1, mainMassive[i + 1, 0] - 1] = 1;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    s += FormativeOut(sumig[i, j]);

                outList.Items.Add(s);
                s = "";
            }
        }

        private string FormativeOut(int data)
        {
            string s = String.Format("{0,4}", data) + "  ";
            return s;
        }
        
        public void Output(string name)
        {
            using (StreamWriter writer = new StreamWriter(name))
            {
                string[] d=new string[outList.Items.Count] ;
                outList.Items.CopyTo(d, 0);

                for(int i=0;i< outList.Items.Count;i++)
                {
                    writer.WriteLine(d[i]);
                }

            }
        }

        public void OutputStepen()
        {
            stepenVhodu = new int[n];

            bool flVhodu = true;

            int j = 0;

            if (fl == false)
            {
                for (int i = 0; i < n; i++)
                    for (int k = 0; k < m; k++)
                    {
                        stepenVhodu[i] += incident[i, k];

                    }
            }
            else
            {
                for (int i = 0; i < n; i++)
                    for (int k = 0; k < m; k++)
                    {
                        stepenVhodu[i] += Math.Abs(incident[i, k]);

                    }
            }

            outList.Items.Add("Cтепені вершин");
            outList.Items.Add("");

            for(int i=0;i< n;i++)
                outList.Items.Add("    Cтепінь вершини "+ (i+1) + " = " +stepenVhodu[i]);
            
            while ((flVhodu==true) &&(j< n-1))
            {
                if (stepenVhodu[j] != stepenVhodu[j + 1])
                    flVhodu = false;

                j++;
            }

            j = 0;
            
                if(flVhodu==true)
                outList.Items.Add("Граф однорідний cтепінь графу= "+ stepenVhodu[0]);
                else
                outList.Items.Add("Граф не однорідний");       
            

            outList.Items.Add("");
            outList.Items.Add("");
        }

        public void OutIzoVuso()
        {
            string sIzo = "Ізольовані вершини ";
            string sVyso = "Висячі вершини ";
           
            for (int i = 0; i < n; i++)
            {
                if (stepenVhodu[i] == 1)
                    sVyso += (i + 1) + " ";

                if (stepenVhodu[i] == 0)
                {
                    sIzo += (i + 1) + " ";
                    izo = true;
                }

            }


            outList.Items.Add("");
            outList.Items.Add(sIzo + ";");
            outList.Items.Add(sVyso + ";");
            outList.Items.Add("");
        }

        /*
        public void CreateTable()
        {
            dataGr.ColumnCount = 10;
            dataGr.RowCount = 10;

            dataGr.Rows[0].Cells[0].Value = 5;
          
            dataGr.AutoSize = true;
            dataGr.Visible = true;
            
        }
        */

        private void CreateGeodez()
        {
            geodez = new int[n,n];
            CreateCount();

            for(int i=0;i< n;i++)
            {
                for(int j=0;j< n;j++)
                {
                    if (i == j)
                        geodez[i, j] = 0;
                    else
                    geodez[i,j] = ValueGeodez(i+1,j+1);
                }
            }
        } 

        private int searchSumig(int ver,int k)
        {
            int ret = 0,j=0;

            for (int i = 0; i < n; i++)
            {
                if (sumig[ver - 1, i] == 1)
                {
                    if (k == j)
                    {
                        ret = i + 1;
                    }
                    j++;
                }
            }

            return ret;
        }

        private int CountSumig(int ver)
        {
            int y = 0;

            for(int i=0;i< n;i++)
            {
                if (sumig[ver-1, i] == 1)
                    y++;
            }

            return y;
        }

        private int CountSumigT(int ver)
        {
            int y = 0;

            for (int i = 0; i < n; i++)
            {
                if (sumigT[ver - 1, i] == 1)
                    y++;
            }

            return y;
        }

        private void CreateCount()
        {
            counts = new int[n];
            countsT = new int[n];

            for (int i = 0; i < n; i++)
            {
                counts[i] = CountSumig(i + 1);
                countsT[i] = CountSumigT(i + 1);
            }


        }

        private int ToCreatePromig(int ver1, int ver2, int[] previousVer, int k, int res)
        {
            int ver = searchSumig(ver1, k), count = counts[ver - 1], min = 0, koef = 0;
            int[] l = new int[count];
            bool fl = true;
            int cou = 0;

            if (ver == ver2)
            {
                res++;
                return res;
            }
            else
            {
                for (int i = 0; i < n+1; i++)
                {
                    if (previousVer[i] != 0)
                    {
                        cou++;
                    }
                    else
                        break;

                    if (previousVer[i] == ver)
                        fl = false;
                }

                previousVer[cou]=ver;

                if ((count == 0) || (ver == ver1) || (fl == false))
                {

                    res = 0;
                    return res;
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        l[i]++;
                        l[i] = ToCreatePromig(ver, ver2, previousVer, i, l[i]);

                        if(cou!=n)
                        for (int j = cou+1; j < n + 1; j++)
                            previousVer[j] = 0;

                        if (l[i] == 0)
                            koef++;
                    }

                    if (koef == count)
                        return 0;
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (l[i] != 0)
                            {
                                min = l[i];
                                break;
                            }
                        }

                        for (int i = 0; i < count; i++)
                        {
                            if ((l[i] < min) && (l[i] != 0))
                                min = l[i];
                        }
                        res += min;

                        return res;
                    }
                }
            }
        }

        private int ValueGeodez(int ver1, int ver2)
        {
            int count = counts[ver1-1],min=0,koef=0;
            int[] values = new int[count];
            int[] previos = new int[n+1];
            

            if (count == 0)
                return 0;
            else
            {
                for (int i = 0; i < count; i++)
                {
                    for (int p = 1; p < n+1; p++)
                        previos[p] = 0;

                    values[i] = 0;
                    previos[0] = ver1;
                    values[i] = ToCreatePromig(ver1, ver2, previos, i, values[i]);
                    

                    if (values[i] == 0)
                        koef++;
                }

                if (koef == count)
                    return 0;
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (values[i] != 0)
                        {
                            min = values[i];
                            break;
                        }
                    }

                    for (int i = 1; i < count; i++)
                    {
                        if ((min > values[i]) && (values[i]!=0))
                            min = values[i];
                    }

                    return min;
                }
             }
        }

        public void OutputRadiusDiametr()
        {
            CreateExc();
            bool fl = true;

            for(int i=0;i< n;i++)
            {
                if (excntri[i] != 0)
                { 
                    r = excntri[i];
                    D = excntri[i];
                }

                if (stepenVhodu[i] == 0)
                    fl = false;

            }
             
            for (int i = 0; i < n; i++)
            {
                if (fl == false)
                    r = 0;
                else
                {
                    if ((r > excntri[i]) && (excntri[i] != 0))
                        r = excntri[i];
                }
         
                if (D < excntri[i])
                    D = excntri[i];
            }


            outList.Items.Add("Діаметр= " + D);
            outList.Items.Add("Радіус= " + r);
            outList.Items.Add("");
        }

        private void CreateExc()
        {
            CreateGeodez();
            excntri = new int[n];

            for(int i=0;i< n;i++ )
            {
                excntri[i] = geodez[i, 0];
                for (int j = 1; j < n; j++)
                    if (excntri[i] < geodez[i, j])
                        excntri[i] = geodez[i, j];
                
            }
        }

        public void OutputCenter()
        {
            CreateExc();
            
            int min =0,j=0;
            string outStr = "Центральні вершини ";

            for(int i=0;i< n;i++)
            {
                if(excntri[i]!=0)
                {
                    min = excntri[i];
                    break;
                }
            }


            for (int i=0;i< n;i++)
            {
                if ((excntri[i]!=0) && (min > excntri[i]))
                    min = excntri[i];
            }

            for(int i=0;i< n;i++)
            {
                if (excntri[i] == min)
                    j++;
            }

            center = new int[j];

            j = 0;

            for (int i = 0; i < n; i++)
            {
                if (excntri[i] == min)
                {
                    center[j]=i+1;
                    if ((j+1) != center.Length)
                        outStr += center[j] + ", ";
                    else
                        outStr += center[j] + ";";
                    j++;
                }

            }
            
                
            outList.Items.Add(outStr);
            
        }

        public void OutputYaruses()
        {
            int j = 0;
            string outStr = "    Ярус ",s="";
            bool fl = false;
            bool[,] flmassive = new bool[n,n];
            bool[] flmass = new bool[n];
            int value = 0;

            outList.Items.Add("Яруси");
            outList.Items.Add("");

            for (int i=0;i< n;i++)
            {
                for (int y = 0; y < n; y++)
                    flmass[y] = true;

                for (int k=0;k< n;k++)
                {
                    if ((flmass[k] == true) && (geodez[k,i]!=0))
                    {
                        s="до вершини " +(i+1).ToString() + " :" + (k+1).ToString();
                        value = geodez[k, i];

                        for (int l = k + 1; l < n; l++)
                        {
                            if (geodez[k,i] == geodez[l,i])
                            {
                                s += " ," + (l + 1).ToString();
                                flmassive[l,k] = false;
                                flmass[l] = false;
                            }
                        }

                            outList.Items.Add(outStr + s + "  відстань " + value + ";" );

                        s = "";
                        fl = false;
                    }

                }  
            }
                
        }

        private void CorrectGeodez()
        {
            CreateGeodez();
            vidstan = geodez;

            for(int i=0;i< n;i++)
            {
                for(int j=0;j< n;j++)
                {
                    if(((stepenVhodu[j]==0) ||(stepenVhodu[i]==0)) && (j != i))
                    {
                        vidstan[i, j] = -1;
                    }
                }
            }

        }

        public void OutputGeodez()
        {
            CorrectGeodez();
            string s = "";

            outList.Items.Add("Матриця відстаней\n");
            
            for(int i=0;i< n;i++)
            {
                for(int j=0;j< n;j++)
                {
                    if (vidstan[i, j] != -1)
                        s += FormativeOut(vidstan[i, j]);
                    else
                        s += " ∞  ";
                }

                outList.Items.Add(s);
                s = "";
            }

            outList.Items.Add("");
        }

        private void CreateDosiazhnost()
        {
            CorrectGeodez();

            dosiazhnosti = new int[n, n];

            for(int i=0;i< n;i++)
            {
                for(int j=0;j< n;j++)
                {
                    if ((vidstan[i, j] != 0) && (vidstan[i, j] != -1))
                        dosiazhnosti[i, j] = 1;
                    else
                        dosiazhnosti[i, j] = 0;
                }
            }
        }

        public void OutputDosiazhnosti()
        {
            CreateDosiazhnost();

            outList.Items.Add("Матриця досяжності\n");

            string s = "";

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                        s += FormativeOut(dosiazhnosti[i, j]);
                }

                outList.Items.Add(s);
                s = "";
            }

            outList.Items.Add("");

        }
        
        public void OutputCircle()
        {
            ValueCircle();

            outList.Items.Add("Цикли у графі\n");

            if (circle.Capacity == 0)
                outList.Items.Add("Циклів нема");
            else
            {
                foreach(string s in circle)
                    outList.Items.Add(s);
            }

        }

        private void ValueCircle()
        {
            CreateCount();

            int count;
            int[] previos = new int[n + 1];

            for (int i=0;i<n;i++)
            {
                count = counts[i];

                previos[0] = i+1;

                for (int j = 0; j < count; j++)
                {
                    for (int k = 1; k < n + 1; k++)
                        previos[k]=0;

                    CreateCircle(i+1, previos, j);
                }


            }
        }
        
        private void CreateCircle(int start, int[] previousVer, int k)
        {
            int ver = searchSumig(start, k), count = counts[ver - 1];
            bool fl = true, fl1, fl2 = true;
            int cou = 0;

            if (count != 0)
            {
                for (int i = 0; i < n + 1; i++)
                {
                    if (previousVer[i] != 0)
                    {
                        cou++;
                    }
                    else
                        break;

                    if (previousVer[i] == ver)
                        fl = false;
                }

                previousVer[cou] = ver;

                if (fl == false)
                {
                    if ((fl == false) && (previousVer[0] == previousVer[cou]))
                    {
                        for (int i = 0; i < cou - 1; i++)
                        {
                            if (previousVer[i] == previousVer[i + 2])
                                fl2 = false;
                        }

                        if (fl2 == true)
                        {
                            string s = "";

                            for (int i = 0; i < cou + 1; i++)
                            {
                                s += FormativeOut(previousVer[i]);
                            }

                            circle.Add(s);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        CreateCircle(ver, previousVer, i);

                        if (cou != n)
                            for (int j = cou + 1; j < n + 1; j++)
                                previousVer[j] = 0;
                    }
                }
            }
        
          }

        public void OutputTypeOfZviaz()
        {
            bool strong=true,odnobichnyi = true;

            outList.Items.Add("");
            outList.Items.Add("Тип зв'язності графа");
            outList.Items.Add("");

            if (izo == true)
            {
                outList.Items.Add("Граф незв'язний");
            }
            else
            {
                for (int i = 0; ((i < n) && ((strong == true) || (odnobichnyi = true))); i++)
                {
                    for (int j = i + 1; ((j < n) && ((strong== true) || (odnobichnyi = true))); j++)
                    {
                        if ((dosiazhnosti[i, j] != 1) || (dosiazhnosti[j, i] != 1))
                        {
                            strong = false;
                        }

                        if ((dosiazhnosti[i, j] != 1) && (dosiazhnosti[j, i] != 1))
                        {
                            odnobichnyi = false;
                        }

                       
                    }
                }

                if (strong == true)
                    outList.Items.Add("Граф сильнозв'язний");
                else
                {
                    if (odnobichnyi == true)
                        outList.Items.Add("Граф однобічно-зв'язний");
                    else
                        outList.Items.Add("Граф слабкозв'язний");
                }
            }
            
        }

        public void CreateVWidth(int unit)
        {
            List <string> res= new List<string>() { };

            Queue<int> q = new Queue<int>();  
            int[] vmist;
            int[,] copy = new int[n, n];
            bool[] used = new bool[n];
            int[] BFS = new int[n];
            int[] vers = new int[n];
            int k = 1,count=0;
            string str;
            CreateCount();

            for (int i = 0; i < n; i++)
                vers[i] = 0;

            for(int i=0;i< n;i++)
            {
                for (int j = 0; j < n; j++)
                    copy[i, j] = 0;
            }

            BFS[unit] = k;
            k++;
   
            used[unit] = true;     

            q.Enqueue(unit);

            outList.Items.Add("\nОбхід графа вшир");
            res.Add (String.Format("{0,-20}{1,-20}{2,-20}", "Вершина", "BFS-номер", "Вміст черги"));
            res.Add(String.Format("{0,-27}{1,-29}{2,-31}", (unit+1), BFS[unit], (q.Peek()+1)+ " "));

            while (q.Count != 0)
            {
                unit = q.Peek();
                count = counts[unit];              
                str ="";

                    for (int i = 0; i < n; i++)
                    {
                        if ((sumig[unit, i] == 1) && (used[i]==false))
                        {
                        copy[unit, i] = sumig[unit, i];
                                vers[unit]++;
                                used[i] = true;
                                q.Enqueue(i);
                                BFS[i] = k;
                                k+=1;

                            vmist = q.ToArray();

                            for (int j = 0; j < vmist.Length; j++)
                            {

                                str += (vmist[j] + 1) + "  ";
                            }

                            res.Add(String.Format("{0,-27}{1,-29}{2,-31}", (vmist[vmist.Length-1] +1), BFS[vmist[vmist.Length - 1]], str));
                            break;
                        }
                        else
                    {
                        if ((sumig[unit, i] == 1) && (used[i] == true) && (copy[unit, i] == 0))
                        {
                            vers[unit]++;
                            copy[unit, i] = 1;
                        }
                    }

                    }

                if (vers[unit] == count)
                {
                    str = "";

                    q.Dequeue();

                    vmist = q.ToArray();

                    for (int j = 0; j < vmist.Length; j++)
                    {

                        str += (vmist[j] + 1) + "  ";
                    }


                    res.Add(String.Format("{0,-27}{1,-29}{2,-31}", "-", "-", str));
                }


            }

            for(int i=0;i<res.Count;i++)
            {
                outList.Items.Add(res[i]);
            }
        }

        public void CreateVHeight(int unit)
        {
            List<string> res = new List<string>() { };

            Stack<int> q = new Stack<int>();
            Stack<int> output = new Stack<int>();
            int[] vmist;
            int[,] copy = new int[n, n];
            bool[] used = new bool[n];
            int[] BFS = new int[n];
            int[] vers = new int[n];
            int k = 1, count = 0;
            string str;
            CreateCount();

            for (int i = 0; i < n; i++)
                vers[i] = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    copy[i, j] = 0;
            }

            BFS[unit] = k;
            k++;

            used[unit] = true;     //массив, хранящий состояние вершины(посещали мы её или нет)

            q.Push(unit);

            outList.Items.Add("\nОбхід графа вглиб");
            res.Add(String.Format("{0,-20}{1,-20}{2,-20}", "Вершина", "DFS-номер", "Вміст стеку"));
            res.Add(String.Format("{0,-27}{1,-29}{2,-31}", (unit + 1), BFS[unit], (q.Peek() + 1) + " "));

            while (q.Count != 0)
            {
                unit = q.Peek();
                count = counts[unit];
                str = "";

                for (int i = 0; i < n; i++)
                {
                    if ((sumig[unit, i] == 1) && (used[i] == false))
                    {
                        copy[unit, i] = sumig[unit, i];
                        vers[unit]++;
                        used[i] = true;
                        q.Push(i);
                        BFS[i] = k;
                        k += 1;

                        vmist = q.ToArray();

                        for (int j = 0; j < vmist.Length; j++)
                        {

                            str += (vmist[j] + 1) + "  ";
                        }

                        res.Add(String.Format("{0,-27}{1,-29}{2,-31}", (vmist[0] + 1), BFS[vmist[0]], str));
                        break;
                    }
                    else
                    {
                        if ((sumig[unit, i] == 1) && (used[i] == true) && (copy[unit, i] == 0))
                        {
                            vers[unit]++;
                            copy[unit, i] = 1;
                        }
                    }

                }

                if (vers[q.Peek()] == counts[q.Peek()])
                {
                    str = "";

                    q.Pop();

                    vmist = q.ToArray();

                    for (int j = 0; j < vmist.Length; j++)
                    {

                        str += (vmist[j] + 1) + "  ";
                    }


                    res.Add(String.Format("{0,-27}{1,-29}{2,-31}", "-", "-", str));
                }


            }

            for (int i = 0; i < res.Count; i++)
            {
                outList.Items.Add(res[i]);
            }

        }
        
          private int[] VHeight(int unit)
          {
               List<string> res = new List<string>() { };

              Stack<int> q = new Stack<int>();
              Stack<int> ToOut = new Stack<int>();
              int[] vmist;
              int[,] copy = new int[n, n];
              bool[] used = new bool[n];
              int[] BFS = new int[n];
              int[] vers = new int[n];
              int[] output=new int[n];
              int k = 1, count = 0;
              string str;
              CreateCount();

            for (int i = 0; i < n; i++)
            {
                vers[i] = 0;
                output[i] = -1;
            }

              for (int i = 0; i < n; i++)
              {
                  for (int j = 0; j < n; j++)
                      copy[i, j] = 0;
              }

              BFS[unit] = k;
              k++;

              used[unit] = true;     //массив, хранящий состояние вершины(посещали мы её или нет)

              q.Push(unit);

              while (q.Count != 0)
              {
                  unit = q.Peek();
                  count = counts[unit];
                  str = "";

                  for (int i = 0; i < n; i++)
                  {
                      if ((sumig[unit, i] == 1) && (used[i] == false))
                      {
                          copy[unit, i] = sumig[unit, i];
                          vers[unit]++;
                          used[i] = true;
                          q.Push(i);
                          BFS[i] = k;
                          k += 1;
                          break;
                      }
                      else
                      {
                          if ((sumig[unit, i] == 1) && (used[i] == true) && (copy[unit, i] == 0))
                          {
                              vers[unit]++;
                              copy[unit, i] = 1;
                          }
                      }

                  }

                  if (vers[q.Peek()] == counts[q.Peek()])
                  {
                      str = "";

                      ToOut.Push(q.Pop());
                  }


              }

              for (int i = 0; i < res.Count; i++)
              {
                  outList.Items.Add(res[i]);
              }

              if(ToOut.Count==n)
              {
                for (int i = 0; i < n; i++)
                    output[i] = ToOut.Pop();

              }

            return output;
          }


        public void ToSort()
        {
            int[,] DFS = new int[n, n];
            int[] prom = new int[n];
            bool[] DFSU = new bool[n];
            string str = "";

            outList.Items.Add("\nТопологічне сортування\nМожливі випадки\n");
            if (circle.Count == 0)
            {
                for (int i = 0; i < n; i++)
                {
                    str = "";
                    prom = VHeight(i);
                    if (prom[0] != -1)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            str += FormativeOut((prom[j] + 1));
                        }

                        outList.Items.Add(str);
                    }
                }
            }
            else
            {
                outList.Items.Add("Не існує топологічного сртування для графа");
            }

        }


        public void Research()
        {
            outList.Items.Add("\n\nСильно зв'язані компоненти\n");

            List<string> res = new List<string>() { };
            List<int> ToK = new List<int>();
            List<List<int>> yop= new List<List<int>>();
            Stack<int> q = new Stack<int>();
            List<int> pr = new List<int>();
            Stack<int> ToOut = new Stack<int>();
            int[] vmist;
            int[,] copy = new int[n, n];
            bool[] used = new bool[n],
                usedT = new bool[n];
            int[] BFS = new int[n];
            int[] vers = new int[n];
            int[] output = new int[n];
            int[] temp = new int[n];
            int[] tempT = new int[n];
            int k = 0, count = 0, unit = 0,u=0;
            string str="";
            CreateCount();



                ToK = new List<int>();

                for (int i = 0; i < n; i++)
                {
                    vers[i] = 0;
                    used[i] = false;
    
                }

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                        copy[i, j] = 0;
                }




                for (int l = 0; l < n; l++)
                {

                    if ((k != n) && used[l] == false)
                    {
                        unit = l;
                        used[unit] = true;
                        k++;
                        q.Push(unit);

                        while (q.Count != 0)
                        {
                            unit = q.Peek();
                            count = counts[unit];

                            for (int i = 0; i < n; i++)
                            {
                                if ((sumig[unit, i] == 1) && (used[i] == false))
                                {
                                    copy[unit, i] = sumig[unit, i];
                                    vers[unit]++;
                                    used[i] = true;
                                    q.Push(i);
                                    k++;
                                    break;
                                }
                                else
                                {
                                    if ((sumig[unit, i] == 1) && (used[i] == true) && (copy[unit, i] == 0))
                                    {
                                        vers[unit]++;
                                        copy[unit, i] = 1;
                                    }
                                }

                            }

                            if (vers[q.Peek()] == counts[q.Peek()])
                            {
                                u = q.Pop();
                                ToK.Add(u);
                            }


                        }
                    }
                }



                for (int i = 0; i < n; i++)
                {
                    used[i] = false;
                    vers[i] = 0;

                    for (int j = 0; j < n; j++)
                        copy[i, j] = 0;
                }


                k = 0;

                for (int l = 0; l < ToK.Count; l++)
                {
                    if (used[ToK[ToK.Count - l - 1]] == false)
                    {
                        unit = ToK[ToK.Count - l - 1];
                        q.Push(unit);
                        used[unit] = true;

                        while (q.Count != 0)
                        {
                            unit = q.Peek();
                            count = countsT[unit];

                            for (int i = 0; i < n; i++)
                            {
                                if ((sumigT[unit, i] == 1) && (used[i] == false))
                                {
                                    copy[unit, i] = sumigT[unit, i];
                                    vers[unit]++;
                                    used[i] = true;
                                    q.Push(i);
                                    k++;
                                    break;
                                }
                                else
                                {
                                    if ((sumigT[unit, i] == 1) && (used[i] == true) && (copy[unit, i] == 0))
                                    {
                                        vers[unit]++;
                                        copy[unit, i] = 1;
                                    }
                                }


                            }

                            if (vers[q.Peek()] == countsT[q.Peek()])
                            {
                                u = q.Pop();
                                pr.Add(u);
                            }

                        }

                        yop.Add(pr);
                        pr = null;
                        pr = new List<int>();
                    }

                }

            
                for (int i = 0; i < yop.Count; i++)
                {
                    str = "";

                    for (int j = 0; j < yop[i].Count; j++)
                    {
                        if (yop[i].Count != 1)
                            str += (yop[i][j] + 1) + " ";
                    }

                    outList.Items.Add(str);
                }

            

        }

        public void twoDeckster(int start,int finish,bool fl)
        {
            CreateDosiazhosti_2();

            string ToProverka = "",marsh="";
            Stack<int> q = new Stack<int>();

            bool[] used = new bool[n];
            int[] l = new int[n],
                  vers;
            int[] from = new int[n];
            int k = 0,
                unit = 0,
                len = 0;

            for(int i=0;i< n;i++)
            {
                used[i] = false;
                l[i] = Int16.MaxValue;
                from[i] = -1;
            }

            l[start] = 0;

            while (AnyFalse(used))
            {
                unit = searchMin(l, used);

                for (int i=0;i< n;i++)
                {
                    if (sumig[unit, i] == 1)
                    {
                        if (l[unit] != Int16.MaxValue)
                        {
                            ToProverka = (unit + 1) + " " + (i + 1);

                            len = l[unit] + ToWeight[ToProverka];

                            if (len < l[i])
                            {
                                from[i] = unit;

                                l[i] = len;
                            }
                        }
                    }
                }

                used[unit] = true;
            }

            if (fl == false)
            {
                outList.Items.Add("\n\n Алгоритм Дейкстри для двох вершин\n");

                if (l[finish] == Int16.MaxValue)
                    outList.Items.Add("Вершина не досяжні");
                else
                {
                    outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (finish + 1) + " = " + l[finish] + "\n");
                    q.Push(finish);
                    k = from[finish];

                    while(k!=start)
                    {
                        q.Push(k);
                        k = from[k];
                    }

                    outList.Items.Add("Маршрут до вершини\n");

                    vers = q.ToArray();

                    marsh += (start + 1).ToString() + " ";

                    for(int i=0;i< vers.Length;i++)
                    {
                        marsh += (vers[i] + 1) + " ";
                    }

                    outList.Items.Add(marsh + " ");
                }
            }
            else
            {
                outList.Items.Add("\n\n Алгоритм Дейкстри для всіх вершин\n");

                for (int i = 0; i < n; i++)
                {
                    if (i != start)
                    {
                        if (l[i] == Int16.MaxValue)
                            outList.Items.Add("Вершина " + (start + 1) + " не досяжна вершині " + (i + 1) + "\n");
                        else
                            outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (i + 1) + " = " + l[i] + "\n");
                    }
                }
            }



        }


        private int searchMin(int[] mass,bool[] used)
        {
            int min = 0,
                minK = 0;

            for (int i = 0; i < n; i++)
            {
                if (used[i] ==false)
                {
                    min = mass[i];
                    minK = i;
                    break;
                }
            }

            for(int i=0;i< n;i++)
            {
                if ((used[i] == false) && (min > mass[i]))
                {
                    min = mass[i];
                    minK = i;
                }
            }

            return minK;
        }

        private bool AnyFalse(bool[] mass)
        { 
            bool res = false;

            for(int i=0;i< n;i++)
            {
                if (mass[i] == false)
                {
                    res = true;
                    break;
                }
            }

            return res;
        } 

        
        private void CreateDosiazhosti_2()
        {
            dosiazhnosti = new int[n, n];

            for(int i=0;i< n;i++)
            {
                for (int j = 0; j < n; j++)
                    dosiazhnosti[i, j] = 0;

                dosiazhnosti[i,i]=1;
            }

            string ToProverka = "";

            bool[] used = new bool[n];
            int[] l = new int[n];
            int k = 0,
                unit = 0,
                len = 0;


            for (int ver = 0; ver < n; ver++)
            {
                for (int i = 0; i < n; i++)
                {
                    used[i] = false;
                    l[i] = Int16.MaxValue;
                }

                l[ver] = 0;



                while (AnyFalse(used))
                {
                    unit = searchMin(l, used);


                    for (int i = 0; i < n; i++)
                    {
                        if (sumig[unit, i] == 1)
                        {
                            ToProverka = (unit + 1) + " " + (i + 1);

                            if (l[unit] != Int16.MaxValue)
                            {
                                dosiazhnosti[ver, i] = 1;

                                len = l[unit] + ToWeight[ToProverka];

                                if (len < l[i])
                                {
                                    l[i] = len;
                                }
                            }
                        }
                    }

                    used[unit] = true;
                }
            }
        }

        public void Ford(int start, int finish, bool fl1)
        {
            string marsh = "";
            Stack<int> q = new Stack<int>();

            int[] from = new int[n],
                vers = new int[n];
            int[] d = new int[n];
            bool fl2 = true;
            int k = 0;
            int x=0;

            for(int i=0;i< n;i++)
            {
                d[i] = Int16.MaxValue;
                from[i] = -1;
            }

            d[start] = 0;

           for(int i=0;i< n;i++)
            {
                x = -1;

                for(int j=0;j<m;j++)
                {
                    if(d[mainMassive[j+1,0]-1]<Int16.MaxValue)
                    {
                        if(d[mainMassive[j + 1, 1]-1]> d[mainMassive[j + 1, 0]-1] + mainMassive[j + 1, 2])
                        {
                            d[mainMassive[j + 1, 1]-1] = d[mainMassive[j + 1, 0]-1] + mainMassive[j + 1, 2];
                            from[mainMassive[j + 1, 1]-1] = mainMassive[j + 1, 0]-1;
                            x = mainMassive[j + 1, 1] - 1;
                            fl2 = true;
                        }
                    }
                }
            }

            if (x == -1)
            {
                if (fl1 == false)
                {
                    outList.Items.Add("\n\n Алгоритм Белмана-Форда для двох вершин\n");

                    if (d[finish] == Int16.MaxValue)
                        outList.Items.Add("Вершина не досяжні");
                    else
                    {
                        outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (finish + 1) + " = " + d[finish] + "\n");
                        q.Push(finish);
                        k = from[finish];

                        while (k != start)
                        {
                            q.Push(k);
                            k = from[k];
                        }

                        outList.Items.Add("Маршрут до вершини\n");

                        vers = q.ToArray();

                        marsh += (start + 1).ToString() + " ";

                        for (int i = 0; i < vers.Length; i++)
                        {
                            marsh += (vers[i] + 1) + " ";
                        }

                        outList.Items.Add(marsh + " ");
                    }
                }
                else
                {
                    outList.Items.Add("\n\n Алгоритм Белмана-Форда для всіх вершин\n");

                    for (int i = 0; i < n; i++)
                    {
                        if (i != start)
                        {
                            if (d[i] == Int16.MaxValue)
                                outList.Items.Add("Вершина " + (start + 1) + " не досяжна вершині " + (i + 1) + "\n");
                            else
                                outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (i + 1) + " = " + d[i] + "\n");
                        }
                    }
                }
            }
            else
            {
                outList.Items.Add("У циклі є від'ємні цикли");
            }

        }

        public void Floyda_Worshelaa()
        {
            vidstan = new int[n, n];
            string ToProverka = "";
            bool fl = false;
           

            for(int i=0;i< n;i++)
            {
                for(int j=0;j< n;j++)
                {
                    if(sumig[i,j]==1)
                    {
                        ToProverka = (i + 1) + " " + (j + 1);
                        vidstan[i, j] = ToWeight[ToProverka];
                    }
                    else
                    {
                        vidstan[i, j] = Int16.MaxValue;
                    }
                }
            }

            for(int k=0;k< n;k++)
            {
                for(int i=0;i< n;i++)
                {
                    for(int j=0;j< n;j++)
                    {
                        vidstan[i, j] = searchMin(vidstan[i, j], vidstan[i, k] + vidstan[k, j]);
                    }
                }
            }

            for(int i=0;i< n;i++)
            {
                if(vidstan[i,i]<0)
                {
                    fl = true;
                }

                vidstan[i, i] = 0;
            }

            outList.Items.Add("\n\n Алгоритм Флойда — Уоршелла\n");

            if (!fl)
            {
                for (int i = 0; i < n; i++)
                {
                    string str = "";

                    for (int j = 0; j < n; j++)
                    {
                        if(vidstan[i,j]>=Int16.MaxValue)
                            str+=String.Format("{0,4}", "∞") + "  ";
                        else
                        str += FormativeOut(vidstan[i, j]);
                    }

                    outList.Items.Add(str);
                }
            }
            else
            {
                outList.Items.Add("У циклі є від'ємні цикли");
            }

        }

        private int searchMin(int a,int b)
        {

            if (a > b)
                return b;
            else
                return a;
        }

        public void Floyda(int start, int finish, bool fl1)
        {
            string marsh = "";
            Stack<int> q = new Stack<int>();

            int[] from = new int[n],
                vers = new int[n];
            int[] d = new int[n];
            bool fl2 = true;
            int k = 0;
            int x = 0;

            for (int i = 0; i < n; i++)
            {
                d[i] = Int16.MaxValue;
                from[i] = -1;
            }

            d[start] = 0;

            for (int i = 0; i < n; i++)
            {
                x = -1;

                for (int j = 0; j < m; j++)
                {
                    if (d[mainMassive[j + 1, 0] - 1] < Int16.MaxValue)
                    {
                        if (d[mainMassive[j + 1, 1] - 1] > d[mainMassive[j + 1, 0] - 1] + mainMassive[j + 1, 2])
                        {
                            d[mainMassive[j + 1, 1] - 1] = d[mainMassive[j + 1, 0] - 1] + mainMassive[j + 1, 2];
                            from[mainMassive[j + 1, 1] - 1] = mainMassive[j + 1, 0] - 1;
                            x = mainMassive[j + 1, 1] - 1;
                            fl2 = true;
                        }
                    }
                }
            }

            if (x == -1)
            {
                if (fl1 == false)
                {
                    outList.Items.Add("\n\n Алгоритм Флойда — Уоршелла для двох вершин\n");

                    if (d[finish] == Int16.MaxValue)
                        outList.Items.Add("Вершина не досяжні");
                    else
                    {
                        outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (finish + 1) + " = " + d[finish] + "\n");
                        q.Push(finish);
                        k = from[finish];

                        while (k != start)
                        {
                            q.Push(k);
                            k = from[k];
                        }

                        outList.Items.Add("Маршрут до вершини\n");

                        vers = q.ToArray();

                        marsh += (start + 1).ToString() + " ";

                        for (int i = 0; i < vers.Length; i++)
                        {
                            marsh += (vers[i] + 1) + " ";
                        }

                        outList.Items.Add(marsh + " ");
                    }
                }
                else
                {
                    outList.Items.Add("\n\n Алгоритм Флойда — Уоршелла для всіх вершин\n");

                    for (int i = 0; i < n; i++)
                    {
                        if (i != start)
                        {
                            if (d[i] == Int16.MaxValue)
                                outList.Items.Add("Вершина " + (start + 1) + " не досяжна вершині " + (i + 1) + "\n");
                            else
                                outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (i + 1) + " = " + d[i] + "\n");
                        }
                    }
                }
            }
            else
            {
                outList.Items.Add("У циклі є від'ємні цикли");
            }

        }


        /*
                public void Floyda_Worshelaa(int a, int b)
                {
                    vidstan = new int[n, n];
                    string ToProverka = "";
                    bool fl = false;
                    int[] from = new int[n],
                        vers;
                    bool[] used = new bool[n];
                    int op = 0;
                    Stack<int> q = new Stack<int>();

                    for (int i = 0; i < n; i++)
                    {
                        from[i] = -1;
                        used[i] = false;
                        for (int j = 0; j < n; j++)
                        {
                            if (sumig[i, j] == 1)
                            {
                                ToProverka = (i + 1) + " " + (j + 1);
                                vidstan[i, j] = ToWeight[ToProverka];
                            }
                            else
                            {
                                vidstan[i, j] = Int16.MaxValue;
                            }
                        }
                    }

                    used[a] = true;

                    for (int k = 0; k < n; k++)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            for (int j = 0; j < n; j++)
                            {
                                //vidstan[i, j] = searchMin(vidstan[i, j], vidstan[i, k] + vidstan[k, j]);

                                if(vidstan[i,j]>( vidstan[i, k] + vidstan[k, j]))
                                {
                                    vidstan[i, j] = vidstan[i, k] + vidstan[k, j];


                                }
                            }
                        }
                    }

                    for (int i = 0; i < n; i++)
                    {
                        if (vidstan[i, i] < 0)
                        {
                            fl = true;
                        }

                        vidstan[i, i] = 0;
                    }

                    outList.Items.Add("\n\n Алгоритм Флойда — Уоршелла\n");

                    if (!fl)
                    {
                        string str = "";

                        if (vidstan[a, b] >= Int16.MaxValue)
                        {
                            str += "∞";
                            outList.Items.Add("\nНайменша відстань між вершиною " + (a + 1) + " та " + (b + 1) + " = " + str + "\n");




                        }
                        else
                        {
                            str += vidstan[a, b].ToString();
                            outList.Items.Add("\nНайменша відстань між вершиною " + (a + 1) + " та " + (b + 1) + " = " + str + "\n");

                            op = from[b];

                            while (op != a)
                            {
                                q.Push(op);
                                op = from[op];
                            }

                            vers = q.ToArray();

                            ToProverka = (a + 1).ToString() + " ";

                            for (int i = 0; i < n; i++)
                            {
                                ToProverka += (vers[i] + 1) + " ";
                            }

                            outList.Items.Add("Маршрут для відстані\n" + ToProverka);
                        }
                    }
                    else
                    {
                        outList.Items.Add("У циклі є від'ємні цикли");
                    }
                }
        */
        public void Eylir_Circle()
        {
            CreateStepen();

            List<int> Circles = new List<int>();
            bool[] used = new bool[m];
            int k = 0, ver = 0;
            bool fl = true,
                fl1 = true;

            for(int i=0;i< n && fl1==true;i++)
            {
                Circles = new List<int>();
                k = 0;
                fl = true;

                for(int j=0;j< m;j++)
                {
                    used[j] = false;
                }

                ver = i;

                while (fl)
                {
                    fl = false;

                    for (int j = 0; j < m; j++)
                    {
                        if (!used[j])
                        {
                            if (incident[ver, j] == -1)
                            {
                                for (int l = 0; l < n; l++)
                                {
                                    if (incident[l, j] == 1)
                                    {
                                        Circles.Add(j + 1);
                                        ver = l;
                                        used[j] = true;
                                        fl = true;
                                    }
                                }
                            }
                        }

                    }

                    if(Circles.Count==m)
                    {
                        fl1 = false;
                        fl = false;
                    }

                }
            }

            

            if(!fl1)
            {
                string str="";

                for(int i=0;i< n;i++)
                {
                    if (stepenVhodu[i] % 2 == 1)
                        k++;
                }

                if(k==2)
                    outList.Items.Add("\nЕйлеровий шлях\n");
                else
                    outList.Items.Add("\nЕйлеровий цикл\n");

                for (int i=0;i< m;i++)
                {
                    str += FormativeOut(Circles[i]);
                }

                outList.Items.Add(str);
            }
            else
            {
                outList.Items.Add("Циклів нема");
            }
        }

        private void CreateStepen()
        {
            stepenVhodu = new int[n];

            for(int i=0;i< n;i++)
            {
                stepenVhodu[i] = 0;

                for(int j=0;j< m;j++)
                {
                    if (incident[i, j] == 1 || incident[i, j] == -1)
                        stepenVhodu[i]++;
                }
            }
        }

        private int[] Ford(int start)
        {
            int[] d = new int[n];
            int x = 0;

            for (int i = 0; i < n; i++)
            {
                d[i] = Int16.MaxValue;
            }

            d[start] = 0;

            for (int i = 0; i < n; i++)
            {
                x = -1;

                for (int j = 0; j < m; j++)
                {
                    if (d[mainMassive[j + 1, 0] - 1] < Int16.MaxValue)
                    {
                        if (d[mainMassive[j + 1, 1] - 1] > d[mainMassive[j + 1, 0] - 1] + mainMassive[j + 1, 2])
                        {
                            d[mainMassive[j + 1, 1] - 1] = d[mainMassive[j + 1, 0] - 1] + mainMassive[j + 1, 2];
                            x = mainMassive[j + 1, 1] - 1;
                        }
                    }
                }
            }

            if (x == -1)
                return d;
            else
                return null;
        }


        public void Johnson()
        {
            vidstan = new int[n, n];
            string ToProverka = "";
            bool fl = false;


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (sumig[i, j] == 1)
                    {
                        ToProverka = (i + 1) + " " + (j + 1);
                        vidstan[i, j] = ToWeight[ToProverka];
                    }
                    else
                    {
                        vidstan[i, j] = Int16.MaxValue;
                    }
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        vidstan[i, j] = searchMin(vidstan[i, j], vidstan[i, k] + vidstan[k, j]);
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (vidstan[i, i] < 0)
                {
                    fl = true;
                }

                vidstan[i, i] = 0;
            }

            outList.Items.Add("\n\n Алгоритм Джонсона\n");

            if (!fl)
            {
                for (int i = 0; i < n; i++)
                {
                    string str = "";

                    for (int j = 0; j < n; j++)
                    {
                        if (vidstan[i, j] >= Int16.MaxValue)
                            str += String.Format("{0,4}", "∞") + "  ";
                        else
                            str += FormativeOut(vidstan[i, j]);
                    }

                    outList.Items.Add(str);
                }
            }
            else
            {
                outList.Items.Add("У графі є від'ємні цикли");
            }

        }

        public void Johnson(int start, int finish, bool fl1)
        {
            string marsh = "";
            Stack<int> q = new Stack<int>();

            int[] from = new int[n],
                vers = new int[n];
            int[] d = new int[n];
            bool fl2 = true;
            int k = 0;
            int x = 0;

            for (int i = 0; i < n; i++)
            {
                d[i] = Int16.MaxValue;
                from[i] = -1;
            }

            d[start] = 0;

            for (int i = 0; i < n; i++)
            {
                x = -1;

                for (int j = 0; j < m; j++)
                {
                    if (d[mainMassive[j + 1, 0] - 1] < Int16.MaxValue)
                    {
                        if (d[mainMassive[j + 1, 1] - 1] > d[mainMassive[j + 1, 0] - 1] + mainMassive[j + 1, 2])
                        {
                            d[mainMassive[j + 1, 1] - 1] = d[mainMassive[j + 1, 0] - 1] + mainMassive[j + 1, 2];
                            from[mainMassive[j + 1, 1] - 1] = mainMassive[j + 1, 0] - 1;
                            x = mainMassive[j + 1, 1] - 1;
                            fl2 = true;
                        }
                    }
                }
            }

            if (x == -1)
            {
                if (fl1 == false)
                {
                    outList.Items.Add("\n\n Алгоритм Джонсона для двох вершин\n");

                    if (d[finish] == Int16.MaxValue)
                        outList.Items.Add("Вершина не досяжні");
                    else
                    {
                        outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (finish + 1) + " = " + d[finish] + "\n");
                        q.Push(finish);
                        k = from[finish];

                        while (k != start)
                        {
                            q.Push(k);
                            k = from[k];
                        }

                        outList.Items.Add("Маршрут до вершини\n");

                        vers = q.ToArray();

                        marsh += (start + 1).ToString() + " ";

                        for (int i = 0; i < vers.Length; i++)
                        {
                            marsh += (vers[i] + 1) + " ";
                        }

                        outList.Items.Add(marsh + " ");
                    }
                }
                else
                {
                    outList.Items.Add("\n\n Алгоритм Джонсона для всіх вершин\n");

                    for (int i = 0; i < n; i++)
                    {
                        if (i != start)
                        {
                            if (d[i] == Int16.MaxValue)
                                outList.Items.Add("Вершина " + (start + 1) + " не досяжна вершині " + (i + 1) + "\n");
                            else
                                outList.Items.Add("\nНайменша відстань між вершиною " + (start + 1) + " та " + (i + 1) + " = " + d[i] + "\n");
                        }
                    }
                }
            }
            else
            {
                outList.Items.Add("У графі є від'ємні цикли");
            }

        }

        public void Gamilton()
        {
            CreateStepen();

            List<int> Circles = new List<int>();
            bool[] used = new bool[n];
            int k = 0, ver = 0;
            bool fl = true,
                fl1 = true;

            for(int i=0;i< n && fl1==true;i++)
            {
                Circles = new List<int>();
                k = 0;
                fl = true;

                for(int j=0;j< n;j++)
                {
                    used[j] = false;
                }

                ver = i;

                while(fl)
                {
                    fl = false;

                    for(int j=0;j< n;j++)
                    {
                        if(!used[j])
                        {
                            if(sumig[ver,j]==1)
                            {
                                Circles.Add(j + 1);
                                ver = j;
                                used[j] = true;
                                fl = true;
                            }
                        }
                    }

                    if (Circles.Count == n)
                    {
                        fl1 = false;
                        fl = false;
                    }

                }
            }

            if(!fl1)
            {
                string str = "";

                for(int i=0;i< n;i++)
                {
                    if (stepenVhodu[i] % 2 == 1)
                        k++;
                }

                if(k == 2)
                    outList.Items.Add("\nГамільтовий шлях\n");
                else
                    outList.Items.Add("\nГамільтовий цикл\n");

                for (int i = 0; i < n; i++)
                {
                    str += FormativeOut(Circles[i]);
                }

                outList.Items.Add(str);
            }
            else
            {
                outList.Items.Add("Циклів і шляху теж");
            }
        }



    }


}
