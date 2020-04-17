
namespace KursheftTools
{
    static class Formats
    {
        ///<summary>
        ///get the Pixel of a given Length in mm
        ///</summary>
        ///<param name="mmLength">the length in mm</param>
        ///<returns>the pixel number of the given length</returns>
        public static double getPixel(int mmLength)
        {
            return (mmLength * 2.834646);
        }
        public static double getPixel(double mmLength)
        {
            return (mmLength * 2.834646);
        }

        public static class A4
        {
            public const int mmWidth = 210;
            public const int mmHeight = 297;
            public const double pixelWidth = 595.27566;
            public const double pixelHeight = 841.889862;
        }
    }
}
