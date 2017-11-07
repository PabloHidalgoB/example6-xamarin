using Android.App;
using Android.Widget;
using Android.OS;
using example6.Fragments;
using example6Xamarin;
using Example6CrossPlatform.CustomClasses;
using System.Collections.Generic;
using example6.CustomClasses;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Java.Util;
using System.Threading;
using System.Linq;
using Android.Views;
using AndroidHUD;

namespace example6
{
    [Activity(Label = "Example 6 via Xamarin", MainLauncher = true, WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustNothing)]
    public class MainActivity : RootActivity, IRequestInterface
    {
        private Double clpRate = 0.0;

        private List<CurrencyPDB> mList = new List<CurrencyPDB>();
        private List<CurrencyPDB> mListAux = new List<CurrencyPDB>();
        
        private List<String> mAdapter = new List<String>();

        private TodayCurrencyAdapter mTodayCurrencyAdapter;
        private ListView mListView;

        private List<Int32> mListBills;
        private List<String> mCurrencyToday;

        private FragmentConverter mFragment;


        public JObject json;
        private const String WSC_GET_RELATED_PRODUCT = "wsc_get_related_product";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            

            mFragment = new FragmentConverter();

            Android.Support.V4.App.FragmentManager fragmentManager = SupportFragmentManager;
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.fl_conversor, mFragment, null).Commit();

            mListView = FindViewById<ListView>(Resource.Id.lstv_todayCurrency);

            mListBills = new List<Int32>();
            mListBills.Clear();

            mCurrencyToday = new List<String>();
            mCurrencyToday.Clear();

            GetGlobalParameters();

        }


        public void GetGlobalParameters()
        {
            new Thread(delegate ()
            {
                var request = new WebServiceClient(this, WSC_GET_RELATED_PRODUCT);
                request.getCurrency();
                request.sendToServer();
            }).Start();
        }

        public void ResultRequest(WebServiceClient webServiceClient)
        {
            if (webServiceClient.TAG.Equals(WSC_GET_RELATED_PRODUCT))
            {
                if (!webServiceClient.success)
                    LibraryUtilities.showDialogMessage(this, GetString(Resource.String.message_error_connection_to_server));

                else
                {
                    try
                    {

                        json = JObject.Parse(webServiceClient.result.ToString());

                        mList.Clear();

                        IList<string> keys = json.Properties().Select(p => p.Name).ToList();
                        
                        for (int i = 0; i < keys.Count; i++)
                        {
                            CurrencyPDB mCurrencyPDB = new CurrencyPDB();
                            JObject jsonObjectY = (JObject) json.SelectToken(keys[i]);
                            mCurrencyPDB.mCode = jsonObjectY.SelectToken("code").ToString();
                            mCurrencyPDB.mAlphaCode = jsonObjectY.SelectToken("alphaCode").ToString();
                            mCurrencyPDB.mNumericCode = jsonObjectY.SelectToken("numericCode").ToString();
                            mCurrencyPDB.mName = jsonObjectY.SelectToken("name").ToString();
                            mCurrencyPDB.mRate = (Double) jsonObjectY.SelectToken("rate");
                            mCurrencyPDB.mDate = jsonObjectY.SelectToken("date").ToString();
                            
                            mList.Add(mCurrencyPDB);
                        }

                        getCurrencies();
                        load();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("==> Exeption: " + e.Message + ", " + e.StackTrace);
                    }
                }
            }
        }

        private void getCurrencies()
        {

            mListAux.Clear();

            for (int x = 0; x < mList.Count(); x++)
            {

                if ((mList[x].mCode.Equals("USD")))
                {
                    mListAux.Add(mList[x]);
                    mCurrencyToday.Add("El Dólar hoy esta a:");
                    mListBills.Add(Resource.Mipmap.usdollar);

                }
                else if ((mList[x].mCode.Equals("EUR")))
                {

                    mListAux.Add(mList[x]);
                    mCurrencyToday.Add("El Euro hoy esta a:");
                    mListBills.Add(Resource.Mipmap.europeaneuro);

                }
                else if ((mList[x].mCode.Equals("GBP")))
                {

                    mListAux.Add(mList[x]);
                    mCurrencyToday.Add("La Libra Esterlina hoy esta a:");
                    mListBills.Add(Resource.Mipmap.ukpound);

                }
                else if ((mList[x].mCode.Equals("JPY")))
                {

                    mListAux.Add(mList[x]);
                    mCurrencyToday.Add("El Yen hoy esta a:");
                    mListBills.Add(Resource.Mipmap.japaneseyen);

                }
                else if ((mList[x].mCode.Equals("CAD")))
                {

                    mListAux.Add(mList[x]);
                    mCurrencyToday.Add("El Dólar Canadiense hoy esta a:");
                    mListBills.Add(Resource.Mipmap.canadadollar);

                }
                else if ((mList[x].mCode.Equals("PLN")))
                {

                    mListAux.Add(mList[x]);
                    mCurrencyToday.Add("El Zloty Polaco hoy esta a:");
                    mListBills.Add(Resource.Mipmap.polishzloty);

                }
                else if ((mList[x].mCode.Equals("CLP")))
                {
                    clpRate = mList[x].mRate;
                }
            }

        }

        private void load()
        {
            RunOnUiThread(() => {
                AndHUD.Shared.ShowSuccess(this, "Datos obtenidos correctamente", MaskType.Clear, TimeSpan.FromSeconds(2));

                IParcelable mParcelable = mListView.OnSaveInstanceState();
                mTodayCurrencyAdapter = new TodayCurrencyAdapter(this, Resource.Layout.row_todaycurrency, mListBills, mCurrencyToday, mListAux, clpRate); //TODO setear parametros correctos
                mListView.Adapter = mTodayCurrencyAdapter;
                mListView.OnRestoreInstanceState(mParcelable);
            });
            
        }
    }
}

