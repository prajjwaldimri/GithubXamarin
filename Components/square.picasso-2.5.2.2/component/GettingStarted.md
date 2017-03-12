# Getting Started with Picasso

<iframe src="https://appetize.io/embed/5w3cj57pegnx0av5kkacytd80r?device=nexus5&scale=75&autoplay=true&orientation=portrait&deviceColor=black" 
        width="300px" height="597px" frameborder="0" scrolling="no"
        style="float: right;margin-left:24px;">&nbsp;</iframe>

> A powerful **image downloading** and **caching** library for Android.

Images add much-needed context and visual flair to Android applications.
Picasso allows for hassle-free image loading in your application -- often in one
line of code!

    Picasso.With(context)
           .Load("http://i.imgur.com/DvpvklR.png")
           .Into(imageView);

Many common pitfalls of image loading on Android are handled automatically by
Picasso:

  * Handling `ImageView` recycling and download cancelation in an adapter.
  * Complex image transformations with minimal memory use.
  * Automatic memory and disk caching.

## Features

### Adapter Downloads

Adapter re-use is automatically detected and the previous download canceled:

    public override View GetView(int position, View convertView, ViewGroup parent) {
        SquaredImageView view = (SquaredImageView) convertView;
        if (view == null) {
            view = new SquaredImageView(context);
        }
      
        string url = this[position];
        Picasso.With(context).Load(url).Into(view);
    }

### Image Transformations

Transform images to better fit into layouts and to reduce memory size:

    Picasso.With(context)
           .Load(url)
           .Resize(50, 50)
           .CenterCrop()
           .Into(imageView);

You can also specify custom transformations for more advanced effects:

    public class CropSquareTransformation : Java.Lang.Object, ITransformation
    {
        public Bitmap Transform(Bitmap source)
        {
            int size = Math.Min(source.Width, source.Height);
            int x = (source.Width() - size) / 2;
            int y = (source.Height() - size) / 2;
            Bitmap result = Bitmap.CreateBitmap(source, x, y, size, size);
            if (result != source) {
                source.Recycle();
            }
            return result;
        }
    
        public string Key
        {
            get { return "square()"; } 
        }
    }

Pass an instance of this class to the `Transform` method:

    Picasso.With(context)
           .Load("http://i.imgur.com/DvpvklR.png")
           .Transform(new CropSquareTransformation())
           .Into(imageView);

### Place Holders

Picasso supports both download and error placeholders as optional features:

    Picasso.With(context)
           .Load(url)
           .Placeholder(Resource.Drawable.placeholder)
           .Error(Resource.Drawable.error)
           .Into(imageView);

A request will be retried three times before the error placeholder is shown.

### Resource Loading

Resources, assets, files, content providers are all supported as image
sources:

    // resources
    Picasso.With(context)
           .Load(Resource.Drawable.landing_screen)
           .Into(imageView1);
    // assets
    Picasso.With(context)
           .Load("file:///android_asset/DvpvklR.png")
           .Into(imageView2);
    // files
    Picasso.With(context)
           .Load(new File("..."))
           .Into(imageView3);
