using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using NefMobile.SecurityServices;
using Mercadolibre.Models;

namespace Mercadolibre
{
    public class Operation
    {
        public static bool isMutant(string[] dna)
        {
            try
            {
                int flag = 0;

                //Convertimos el string a matriz de 6 x 6

                string[,] matriz = new string[6, 6];

                for (int i = 0; i <= dna.Length - 1; i++)
                {
                    char[] sLinea = dna[i].ToCharArray();
                    for (int a = 0; a <= sLinea.Length - 1; a++)
                    {
                        matriz[i, a] = sLinea[a].ToString();
                    }
                }

                // Verificamos las lineas horizontales buscando coincidencias de caena de nucleotidos

                for (int i = 0; i <= 5; i++)
                {
                    int b = 0;
                    string sA = "";
                    string sB = "";

                    for (int a = 0; a <= 5; a++)
                    {
                        if (a == 0)
                        {
                            sA = matriz[i, a].ToString();
                            sB = matriz[i, a + 1].ToString();
                        }
                        else
                        {
                            sA = matriz[i, a - 1].ToString();
                            sB = matriz[i, a].ToString();
                        }

                        if (sA == sB) b = b + 1;
                        if (b >= 3) flag = flag + 1;

                    }
                }

                // Verificamos las lineas verticales buscando coincidencias de caena de nucleotidos

                for (int i = 0; i <= 5; i++)
                {
                    int b = 0;
                    string sA = "";
                    string sB = "";

                    for (int a = 0; a <= 5; a++)
                    {
                        if (a == 0)
                        {
                            sA = matriz[a, i].ToString();
                            sB = matriz[a + 1, i].ToString();
                        }
                        else
                        {
                            sA = matriz[a, i].ToString();
                            sB = matriz[a - 1, i].ToString();
                        }

                        if (sA == sB) b = b + 1;
                        if (b >= 3) flag = flag + 1;

                    }
                }

                // Verificamos las lineas diagonales buscando coincidencias de caena de nucleotidos

                int aa = 0;
                for (int i = 0; i <= 5; i++)
                {
                    int b = 0;
                    string sA = "";
                    string sB = "";

                    if (aa == 0)
                    {
                        sA = matriz[aa, i].ToString();
                        sB = matriz[aa + 1, i + 1].ToString();
                    }
                    else
                    {
                        sA = matriz[aa, i].ToString();
                        sB = matriz[aa - 1, i - 1].ToString();
                    }

                    aa = aa + 1;

                    if (sA == sB) b = b + 1;
                    if (b >= 3) flag = flag + 1;
                }

                if (flag > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static StatsResponse GetStats()
        {
            StatsResponse result = new StatsResponse();
            try
            {
                string sSQL1 = "SELECT COUNT(*) FROM mercadolibre.xmen_dna group by is_mutant";
                DataTable res = DataOperations.Select(sSQL1);
                if (res == null || res.Rows.Count == 0) throw new Exception();

                result.count_mutant_dna = Convert.ToInt32(res.Rows[0][0].ToString());
                result.count_human_dna = Convert.ToInt32(res.Rows[1][0].ToString());

                return result;
            }
            catch(Exception ex)
            {
                result.count_human_dna = 0;
                result.count_mutant_dna = 0;
                return result;
            }
        }
    }
}
