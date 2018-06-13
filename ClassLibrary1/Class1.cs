using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    [Serializable]
    public class Square
    {
        // Props.
        public double LowLeftX { get; set; }
        public double LowLeftY { get; set; }
        public double UpRightX { get; set; }
        public double UpRightY { get; set; }

        public Square(double lowLeftX,double lowLeftY, double upRightX, double upRightY)
        {
            LowLeftX = lowLeftX;
            LowLeftY = lowLeftY;
            UpRightX = upRightX;
            UpRightY = upRightY;
        }

        // Здесь смекалка и ум. В условии сказано что они параллельны оси Ох Оу
        // Цитата:"Square - квадрат в трехмерном пространстве, оси квадрата параллельны осям Ox, Oy."
        public double Length
        {
            get { return (LowLeftY - UpRightY); }
        }
        public double Sq
        {
            get { return Length * Length; }
        }

        public virtual Square BinDeserialize(string path)
        {
            object obj;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                obj = formatter.Deserialize(stream);
            }
            // либо (Square)obj тут уже сами смекалочку проявляйте
            return obj as Square;
        }


        public virtual void BinSerialize(string path)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }

    }


    [Serializable]
    public class Piramid : Square
    {
        public Piramid(double lowLeftX, double lowLeftY, double upRightX, double upRightY,double h):base(lowLeftX,lowLeftY,upRightX,upRightY)
        {
            this.h = h;
        }
        private double h;

        public double Vol
        {
            get { return (double)(1 / 3) * Sq * h; }
        }

        public override void BinSerialize(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }
       


    }

   public class Cube : Square
    {
        private double h;
        public Cube(double lowLeftX, double lowLeftY, double upRightX, double upRightY, double h):base(lowLeftX,lowLeftY,upRightX,upRightY)
        {
            this.h = h;
        }
        /// <summary>
        /// объем куба a*b*c. a=b=c.
        /// </summary>
        private double Vol
        {
            get { return Length * Length * Length; }
        }

        public override void BinSerialize(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }
    }

    public class Figures
    {
        List<Square> squares = new List<Square>();

        public IEnumerable Figures3d()
        {
            for (int i = 0; i < squares.Count; i++)
            {
                if (squares[i] is Cube || squares[i] is Piramid)
                {
                    yield return squares[i] ;
                }
            }
        }

        public IEnumerable MyFig(Type type)
        {
            
            for (int i = 0; i < squares.Count; i++)
            {
                if (squares[i].GetType() == type )
                {
                    yield return squares[i];
                }
            }
        }
    }
}
