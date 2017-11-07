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
using System.Collections;

namespace example6.CustomClasses
{
    public class TodayCurrencyAdapter : BaseAdapter
    {
        private Context mContext;
        private List<Int32> mArrayBills;
        private ImageView mRowImage;

        private TextView mCurrencyT;
        private List<String> mCurrencyToday;

        private TextView mCurrencyValue;
        private List<CurrencyPDB> mListAux;

        private Double clpRate;

        public TodayCurrencyAdapter(Context mContext, int resource, List<Int32> mArrayList, List<String> mCurrencyToday, List<CurrencyPDB> mListAux, Double clpRate) : base()
        {

            this.mContext = mContext;
            this.mArrayBills = mArrayList;
            this.mCurrencyToday = mCurrencyToday;
            this.mListAux = mListAux;
            this.clpRate = clpRate;

            //Console.WriteLine(clpRate.ToString());

        }

        public override int Count
        {
            get { return mArrayBills.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var mArrayBill = mArrayBills[position];
            var mCurrency = mCurrencyToday[position];
            var mListAu = mListAux[position];
            
            View view = convertView;

            if (view == null)
            {
                LayoutInflater vI = (LayoutInflater) mContext.GetSystemService(Context.LayoutInflaterService);
                view = vI.Inflate(Resource.Layout.row_todaycurrency, parent, false);
            }

            mRowImage = view.FindViewById<ImageView>(Resource.Id.img_currencyBill);
            mRowImage.SetImageResource(mArrayBill);

            mCurrencyT = view.FindViewById<TextView>(Resource.Id.txt_currencyToday);
            mCurrencyT.Text = (mCurrency); //TODO agregar mCurrency en posicion para entregar data desde la posicion definida

            Double semiFinal = ((1 * clpRate) / mListAu.mRate); //TODO agregar mListAu en posicion para entregar data desde la posicion definida

            Double finalValue = Math.Round(semiFinal * 100.0) / 100.0;

            mCurrencyValue = view.FindViewById<TextView>(Resource.Id.txt_currencyValue);
            mCurrencyValue.Text = ("$" + finalValue.ToString()); 

            return view;
        }
    }
}