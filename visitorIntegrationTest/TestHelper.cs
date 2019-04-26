using System;
using System.Collections.Generic;
using System.Text;

namespace visitorIntegrationTest
{
    public class TestHelper
    {
        public static int TestCountTotal { get; set; }
        public static int TestCountSucces { get; set; }
        public static int  TestCountFailier { get; set; }
        public static List<String> FailMessageList { get; set; }


        public static void AddFaildMessage(String Message)
        {
            if (FailMessageList == null)
            {
                FailMessageList = new List<string>();
            }
            FailMessageList.Add(Message);
            TestCountFailier++;
            TestCountTotal++;

        }
        public static void AddSuccesMessage()
        {
            TestCountTotal++;
            TestCountSucces++;
        }
        public static String GiveStats()
        {

            if (TestCountTotal == 0)
            {
                return "Total: " + TestCountTotal + " S: " + TestCountSucces + " F: " + TestCountFailier;
            }
            else
            {
                return "Total: " + TestCountTotal + " S: " + TestCountSucces + " F: " + TestCountFailier + " Succes Ratio: " + GetRatio()+ "%";
            }
        }
        public static int GetRatio()
        {
            return (int)(((double)TestCountSucces / (double)TestCountTotal) * 100);
        }
    }
}
