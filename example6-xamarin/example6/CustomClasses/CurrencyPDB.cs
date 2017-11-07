using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace example6.CustomClasses
{
    public class CurrencyPDB
    {
        public String mCode { get; set; }
        public String mAlphaCode { get; set; }
        public String mNumericCode { get; set; }
        public String mName { get; set; }
        public Double mRate { get; set; }
        public String mDate { get; set; }
    }
}