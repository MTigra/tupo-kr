using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClassLibrary1;
namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Square[] spisok;
            //(1; 10) (-2; 20) 5 Piramid
            //(2.222; 2) (3; 333) 10 Cube
            Regex rex = new Regex(@"^\((-*\d+(?:[\.,]\d+)?);\ (-*\d+(?:[\.,]\d+)?)\)\ \((-*\d+(?:[\.,]\d+)?);\ (-*\d+(?:[\.,]\d+)?)\)\ (-*\d+(?:[\.,]\d+)?)\ (Piramid|Cube|Square)$");
            string[] file = File.ReadAllLines("data.txt");
            spisok = new Square[file.Length];
            for (int i = 0; i < file.Length; i++)
            {
                if (!rex.IsMatch(file[i]))
                {
                    Console.WriteLine($"В файле обнаружена ошибка на строке {i}. Я вынуждена прекратить работать:(");
                    return;
                }

                var match = rex.Match(file[i]);
                var groups = match.Groups;
                try
                {


                    switch (groups[5].Value)
                    {
                        case "Cube":
                            {
                                spisok[i] = new Cube(ParseDouble(groups[0].ToString()), ParseDouble(groups[1].ToString()), ParseDouble(groups[2].ToString()), ParseDouble(groups[3].ToString()), ParseDouble(groups[4].ToString()));
                                break;
                            }
                        case "Piramid":
                            {
                                spisok[i] = new Piramid(ParseDouble(groups[0].ToString()), ParseDouble(groups[1].ToString()), ParseDouble(groups[2].ToString()), ParseDouble(groups[3].ToString()), ParseDouble(groups[4].ToString()));
                                break;
                            }
                        case "Square":
                            {
                                spisok[i] = new Square(ParseDouble(groups[0].ToString()), ParseDouble(groups[1].ToString()), ParseDouble(groups[2].ToString()), ParseDouble(groups[3].ToString()));
                                break;
                            }

                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Ошибка в строке номер {i}");
                    return;
                }
            }

            BinaryFormatter ser = new BinaryFormatter();
            using (FileStream fs = new FileStream("out.bin",FileMode.Create))
            {
                ser.Serialize(fs,spisok);
            }
            Console.WriteLine("Сериализован!");
            Square[] deSerSpisok; 
            using (FileStream fs = new FileStream("out.bin", FileMode.Open))
            {
                deSerSpisok=(Square[])ser.Deserialize(fs);
            }

            for (int i = 0; i < deSerSpisok.Length; i++)
            {
                Console.WriteLine(deSerSpisok[i].ToString());
            }


        }

        public static double ParseDouble(string s)
        {
            s = s.Replace('.', ',');
            double d = double.Parse(s);
            return d;
        }


        
    }
}
