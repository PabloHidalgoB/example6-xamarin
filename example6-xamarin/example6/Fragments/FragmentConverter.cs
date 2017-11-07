using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Example6CrossPlatform.CustomClasses;
using example6Xamarin;
using Newtonsoft.Json.Linq;
using example6.CustomClasses;
using System.Threading;
using Java.Util;
using System.Globalization;
using Android.Views.InputMethods;
using AndroidHUD;

namespace example6.Fragments
{
    public class FragmentConverter : Android.Support.V4.App.Fragment, IRequestInterface
    {
        private Button mConvert;
        private Button mSwitch;

        private EditText mInputValue;
        private TextView mOutputValue;

        private Spinner mCurrencyInput;
        private Spinner mCurrencyOutput;

        private List<CurrencyPDB> mList = new List<CurrencyPDB>();
        private List<CurrencyPDB> mListAux = new List<CurrencyPDB>();

        private List<String> mAdapter = new List<String>();

        public RootActivity mRootActivity;
        public JObject json;
        private const String WSC_GET_RELATED_PRODUCT = "wsc_get_related_product";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.Fragment_Converter, container, false);

            mConvert = view.FindViewById<Button>(Resource.Id.btn_convert);
            mSwitch = view.FindViewById<Button>(Resource.Id.btn_switch);
            mConvert.Click += OnConvertClick;
            mSwitch.Click += OnSwitchClick;

            mInputValue = view.FindViewById<EditText>(Resource.Id.txt_input);
            mOutputValue = view.FindViewById<TextView>(Resource.Id.txt_value);
            mInputValue.EditorAction += (sender, e) => {
                if (e.ActionId == ImeAction.Done)
                {
                    doConvert();
                }
                else
                {
                    e.Handled = false;
                }
            };

            mCurrencyInput = view.FindViewById<Spinner>(Resource.Id.spn_input);
            mCurrencyOutput = view.FindViewById<Spinner>(Resource.Id.spn_convert);

            mRootActivity = (RootActivity)Activity;

            GetGlobalParameters();

            return view;
        }

        private void OnSwitchClick(object sender, EventArgs e)
        {
            int spinner1Index = mCurrencyInput.SelectedItemPosition;

            mCurrencyInput.SetSelection(mCurrencyOutput.SelectedItemPosition);
            mCurrencyOutput.SetSelection(spinner1Index);
        }

        private void OnConvertClick(object sender, EventArgs e)
        {
            doConvert();
        }

        public void GetGlobalParameters()
        {
            new Thread(delegate ()
            {
                AndHUD.Shared.Show(mRootActivity, "Obteniendo datos", -1, MaskType.Clear);
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
                    LibraryUtilities.showDialogMessage(mRootActivity, GetString(Resource.String.message_error_connection_to_server));

                else
                {
                    try
                    {

                        json = JObject.Parse(webServiceClient.result.ToString());
                        //Console.WriteLine(json.ToString());

                        mList.Clear();

                        IList<string> keys = json.Properties().Select(p => p.Name).ToList();

                        for (int i = 0; i < keys.Count; i++)
                        {
                            CurrencyPDB mCurrencyPDB = new CurrencyPDB();
                            JObject jsonObjectY = (JObject)json.SelectToken(keys[i]);
                            mCurrencyPDB.mCode = jsonObjectY.SelectToken("code").ToString();
                            mCurrencyPDB.mAlphaCode = jsonObjectY.SelectToken("alphaCode").ToString();
                            mCurrencyPDB.mNumericCode = jsonObjectY.SelectToken("numericCode").ToString();
                            mCurrencyPDB.mName = jsonObjectY.SelectToken("name").ToString();
                            mCurrencyPDB.mRate = (Double)jsonObjectY.SelectToken("rate");
                            mCurrencyPDB.mDate = jsonObjectY.SelectToken("date").ToString();

                            mList.Add(mCurrencyPDB);
                        }

                        AndHUD.Shared.Dismiss();
                        loadData();
                     
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("==> Exeption: " + e.Message + ", " + e.StackTrace);
                    }
                }
            }
        }
        
        private void loadData()
        {
            mAdapter.Clear();
            for (int x = 0; x < mList.Count(); x++)
            {
                mAdapter.Add(mList[x].mName);
            }

            ArrayAdapter currencyInput = new ArrayAdapter<String>(mRootActivity, Android.Resource.Layout.SimpleSpinnerDropDownItem, mAdapter);
            ArrayAdapter currencyOutput = new ArrayAdapter<String>(mRootActivity, Android.Resource.Layout.SimpleSpinnerDropDownItem, mAdapter);

            mRootActivity.RunOnUiThread(() =>
            {
                mCurrencyInput.Adapter = currencyInput;
                mCurrencyOutput.Adapter = currencyOutput;

                mListAux.Clear();

                for (int x = 0; x < mList.Count(); x++)
                {

                    if ((mList[x].mCode.Equals("CLP")))
                    {

                        mCurrencyInput.SetSelection(x);
                    }
                    
                }
            });
        }

        private void doConvert()
        {
            Double a = 0.0;

            Double semiFinal;
            Double finalValue;

            String text = mInputValue.Text;
            Double b = mList[mCurrencyInput.SelectedItemPosition].mRate;
            Double c = mList[mCurrencyOutput.SelectedItemPosition].mRate;

            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    a = Double.Parse(text);
                    // it means it is double
                }
                catch (Exception e1)
                {
                    // this means it is not double
                    e1.StackTrace.ToString();
                }
            }

            semiFinal = ((a * c) / b);
            finalValue = Math.Round(semiFinal * 100.0) / 100.0;

            var salida = decimal.Parse(finalValue.ToString(), CultureInfo.InvariantCulture).ToString("N", new CultureInfo("es-ES"));

            mOutputValue.Text = "$" + salida; 
        }
    }
}