v7 Support CardView Library
=========================

CardView extends the FrameLayout class and lets you show information inside cards that have a consistent look across the platform. CardView widgets can have shadows and rounded corners.


### Target SDK Version 
NOTE: Using this support library requires that your app have its Target Android Version (*targetSdkVersion*) set to Lollipop (5.0 - API Level 21) or higher, or you will have *aapt* related compile errors.  You can still set the Target Framework which your app is compiled against as low as Android 4.0.3 (API Level 15).


Using CardView in your layouts
------------------------------
Using a CardView is simple, since it acts the same as a FrameLayout container.  You can create a CardView directly in your layout file and add child controls to it just as you would a FrameLayout.

```xml
<android.support.v7.widget.CardView 
  xmlns:card_view="http://schemas.android.com/apk/res-auto"
  android:id="@+id/cardView"
  android:layout_gravity="center"
  android:layout_width="wrap_content"
  android:layout_height="wrap_content"
  android:padding="20dp"
  card_view:cardUseCompatPadding="true"
  card_view:cardCornerRadius="4dp"
  card_view:cardElevation="4dp"
  card_view:contentPadding="10dp">
  <TextView
    android:id="@+id/infoText"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:text="This is inside a CardView" />
</android.support.v7.widget.CardView>
```
From the example you can see that you are able to change the corner radius of the CardView, set the padding, as well as the elevation (which will generate a shadow).

Please see the [Google CardView documentation](https://developer.android.com/training/material/lists-cards.html#CardView) for more information.