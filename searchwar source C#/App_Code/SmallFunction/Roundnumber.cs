using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for EvenNumber
/// </summary>
public class RoundNumber {
    public RoundNumber() {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// !!!This method does not work properly yet, but an emergency!!!
    /// This method roundnumbers. Fx. 46 wil be 50 and 43 will be 40
    /// </summary>
    /// <param name="number"></param>
    /// <param name="maxzeros"></param>
    /// <returns></returns>
    public static int GetRoundNumber(decimal number, int maxzeros) {

        int resultN = 0;
        int countOfNumbers = 0;

        resultN = Convert.ToInt32(Math.Round(number, 0));
        countOfNumbers = resultN.ToString().Length;

        if (maxzeros == 0) {
            throw (new ArgumentNullException("RoundNumber", "maxzeros must be more than " + maxzeros));
        }

        for (int n = 1; n <= countOfNumbers; n++) {

            if (n <= maxzeros) continue;
            string rNumber = Regex.Split(resultN.ToString(), "")[n - 1];

            if (Convert.ToInt32(Regex.Split(resultN.ToString(), "")[n]) > 5) {
                resultN = Convert.ToInt32(Regex.Replace(resultN.ToString(), rNumber, (Convert.ToInt32(rNumber) + 1).ToString()));
            } else {
                resultN = Convert.ToInt32(Regex.Replace(resultN.ToString(), rNumber, (Convert.ToInt32(rNumber)).ToString()));
            }

            resultN = Convert.ToInt32(Regex.Replace(resultN.ToString(), Regex.Split(resultN.ToString(), "")[n], "0"));
        }

        if (resultN < 10) {
            resultN = resultN > 5 ? 10 : 0;
        }

        return resultN;

    }

}
