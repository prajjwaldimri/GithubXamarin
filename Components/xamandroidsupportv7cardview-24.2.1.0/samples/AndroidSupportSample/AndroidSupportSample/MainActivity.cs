
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
using Android.Support.V7.Widget;

namespace AndroidSupportSample
{
	[Activity (Label = "v7 CardView", MainLauncher=true)]			
	public class MainActivity : Activity
	{
		CardView cardView;
		TextView textView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			cardView = FindViewById<CardView> (Resource.Id.cardView);
			textView = FindViewById<TextView> (Resource.Id.infoText);

			textView.Text = "This is TextView\nInside of a CardView";
		}
	}
}

