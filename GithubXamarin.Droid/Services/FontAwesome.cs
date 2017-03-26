using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace GithubXamarin.Droid.Services
{
    public class FontAwesome : TextView
    {
        public FontAwesome(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public FontAwesome(Context context) : base(context)
        {
            Init();
        }

        public FontAwesome(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public FontAwesome(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public FontAwesome(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        private void Init()
        {
            var tf = Typeface.CreateFromAsset(Application.Context.Assets, "fontawesome-webfont.ttf");
            SetTypeface(tf, TypefaceStyle.Normal);
        }
    }
}