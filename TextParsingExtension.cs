using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace search
{
    // Handle all the file reading operations
    public static class TextParsingExtension
    {
        public static int[] GetPairValue(string l, char indLeft, char indRight)
        {
            string[] nMSplitted = l.Split(',');
            int n = Convert.ToInt32(nMSplitted[0].Split(indLeft)[1]);
            int m = Convert.ToInt32(nMSplitted[1].Split(indRight)[0]);
            return new int[] { n, m };
        }

        // get green wall coordinates
        public static List<int[]> GetGreenValues(string l)
        {
            string[] cells = l.Split('|', ' ');
            List<int[]> result = new List<int[]>();

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] != "")
                    result.Add(GetPairValue(cells[i], '(', ')'));
            }
            return result;
        }

        // get wall cell coordinates
        public static int[] GetWallPos(string l)
        {
            string[] pos = l.Split(',', '(', ')');
            int[] result =
            {
                Convert.ToInt32(pos[1]),
                Convert.ToInt32(pos[2]),
                Convert.ToInt32(pos[3]),
                Convert.ToInt32(pos[4])
            };

            return result;
        }
    }
}
