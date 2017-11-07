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
using Android.Net;

namespace example6.CustomClasses
{
    public class LibraryUtilities
    {
        private static Android.Support.V7.App.AlertDialog dialog;

        public static bool thereConnection(Context context)
        {
            NetworkInfo net_info = ((ConnectivityManager)context.GetSystemService(Context.ConnectivityService)).ActiveNetworkInfo;

            return net_info != null && net_info.IsAvailable && net_info.IsConnected;

        }

        public static void showDialogMessage(RootActivity activity, String message)
        {
            activity.RunOnUiThread(()=>
            {
                if (dialog != null)
                {
                    if (dialog.IsShowing)
                        dialog.Dismiss();
                }

                Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(activity);
                alert.SetTitle(Resource.String.message_title);

                // container
                LinearLayout container = new LinearLayout(activity);
                container.Orientation = Orientation.Vertical;
                var inputLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                inputLayoutParams.SetMargins(80, 20, 80, 5);

                // name
                TextView textViewMessage = new TextView(activity);
                textViewMessage.LayoutParameters = inputLayoutParams;
                textViewMessage.Gravity = GravityFlags.CenterHorizontal;

                textViewMessage.Text = message;
                container.AddView(textViewMessage);

                // container button
                RelativeLayout containerButtons = createContainerButton(activity);

                // linear layout left
                LinearLayout leftLinearLayoutButton = leftLinearLayout(activity);

                // positive button
                Button btnPositive = buttonDialog(activity);
                btnPositive.Visibility = ViewStates.Visible;
                btnPositive.Text = activity.GetString(Resource.String.btn_ok_text);
                btnPositive.Click += (o, e) =>
                {
                    dialog.Dismiss();
                };
                leftLinearLayoutButton.AddView(btnPositive);

                containerButtons.AddView(leftLinearLayoutButton);
                container.AddView(containerButtons);
                alert.SetView(container);

                dialog = alert.Create();
                dialog.Show();

            });
        }

        public static Button buttonDialog(Context context)
        {
            var layoutButtonParam = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            layoutButtonParam.AddRule(LayoutRules.CenterVertical);

            Button btn = new Button(context);
            btn.LayoutParameters = layoutButtonParam;
            btn.Visibility = ViewStates.Gone;

            return btn;
        }

        public static LinearLayout leftLinearLayout(Context context)
        {
            LinearLayout leftLinearLayoutButton = new LinearLayout(context);
            var leftLinearLayoutButtonParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            leftLinearLayoutButtonParams.AddRule(LayoutRules.AlignParentRight);
            leftLinearLayoutButtonParams.AddRule(LayoutRules.CenterVertical);
            leftLinearLayoutButton.LayoutParameters = leftLinearLayoutButtonParams;
            leftLinearLayoutButton.Orientation = Orientation.Horizontal;

            return leftLinearLayoutButton;
        }

        public static RelativeLayout createContainerButton(Context context)
        {
            RelativeLayout containerButtons = new RelativeLayout(context);
            var containerButtonsLayoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            containerButtonsLayoutParams.AddRule(LayoutRules.CenterVertical);
            containerButtons.LayoutParameters = containerButtonsLayoutParams;
            containerButtons.SetPadding(15, 15, 15, 15);

            return containerButtons;
        }
    }
}