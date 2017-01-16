Design Support Library
======================

The Android Design Support Library brings a number of important material design elements to older Android devices.  

### Prerequisites

**Themes**

Any activity you use Design Support library views in must either use the theme `Theme.AppCompat` or inherit from `Theme.AppCompat` found in the AppCompat v7 Support library.

**Target SDK Version**

Using this support library requires that your app have its Target Android Version (*targetSdkVersion*) set to Lollipop (5.0 - API Level 21) or higher, or you will have *aapt* related compile errors.  You can still set the Target Framework which your app is compiled against as low as Android 4.0.3 (API Level 15).


Snackbar
--------
Providing lightweight, quick feedback about an operation is a perfect opportunity to use a snackbar.  Snackbars are shown on the bottom of the screen and contain text with an optional single action. They automatically time out after the given time length by animating off the screen. In addition, users can swipe them away before the timeout.

By including the ability to interact with the Snackbar through swiping it away or actions, these are considerably more powerful than toasts, another lightweight feedback mechanism.  However, you’ll find the API very similar:

```csharp
Snackbar
  .Make (parentLayout, "Text Here", Snackbar.LengthLong)
  .SetAction (Resource.String.snackbar_action, () => { })
  .Show (); // Don’t forget to show!
```

You’ll note the use of a `View` as the first parameter to `Make ()` - Snackbar will attempt to find an appropriate parent of the Snackbar’s view to ensure that it is anchored to the bottom.




Navigation View
----------------
The navigation drawer can be an important focal point for identity and navigation within your app and consistency in the design here can make a considerable difference in how easy your app is to navigate, particularly for first time users.  `NavigationView` makes this easier by providing the framework you need for the navigation drawer as well as the ability to inflate your navigation items through a menu resource.

You use `NavigationView` as `DrawerLayout`'s drawer content view with a layout such as:

```xml
<android.support.v4.widget.DrawerLayout
        xmlns:android="http://schemas.android.com/apk/res/android"
        xmlns:app="http://schemas.android.com/apk/res-auto"
        android:layout_width="match_parent"
        android:layout_height="match_parent"
        android:fitsSystemWindows="true">

    <!-- your content layout -->

    <android.support.design.widget.NavigationView
            android:layout_width="wrap_content"
            android:layout_height="match_parent"
            android:layout_gravity="start"
            app:headerLayout="@layout/drawer_header"
            app:menu="@menu/drawer"/>
</android.support.v4.widget.DrawerLayout>
```

You’ll note two attributes for NavigationView: app:headerLayout controls the (optional) layout used for the header. `app:menu` is the menu resource inflated for the navigation items (which can also be updated programmatically).  `NavigationView` takes care of the scrim protection of the status bar for you, ensuring that your `NavigationView` interacts with the status bar appropriately on API21+ devices.

The simplest drawer menus will be a collection of checkable menu items:

```xml
<group android:checkableBehavior="single">
    <item
        android:id="@+id/navigation_item_1"
        android:checked="true"
        android:icon="@drawable/ic_android"
        android:title="@string/navigation_item_1"/>
    <item
        android:id="@+id/navigation_item_2"
        android:icon="@drawable/ic_android"
        android:title="@string/navigation_item_2"/>
</group>
```

The checked item will appear highlighted in the navigation drawer, ensuring the user knows which navigation item is currently selected.

You can also use subheaders in your menu to separate groups of items:

```xml
<item
    android:id="@+id/navigation_subheader"
    android:title="@string/navigation_subheader">
    <menu>
        <item
            android:id="@+id/navigation_sub_item_1"
            android:icon="@drawable/ic_android"
            android:title="@string/navigation_sub_item_1"/>
        <item
            android:id="@+id/navigation_sub_item_2"
            android:icon="@drawable/ic_android"
            android:title="@string/navigation_sub_item_2"/>
    </menu>
</item>
```

You’ll get callbacks on selected items by subscribing to the `NavigationItemSelected` event.  This provides you with the `MenuItem` that was clicked, allowing you to handle selection events, changed the checked status, load new content, programmatically close the drawer, or any other actions you may want.




Floating Action Button
----------------------
A floating action button is a round button denoting a primary action on your interface.  The Design library’s `FloatingActionButton` gives you a single consistent implementation, by default colored using the `colorAccent` from your theme.

In addition to the normal size floating action button, it also supports the mini size (`fabSize="mini"`) when visual continuity with other elements is critical.  As `FloatingActionButton` extends `ImageView`, you’ll use `android:src` or any of the methods such as `SetImageDrawable ()` to control the icon shown within the `FloatingActionButton`.




Floating labels for editing text
--------------------------------

Even the humble `EditText` has room to improve in material design. While an `EditText` alone will hide the hint text after the first character is typed, you can now wrap it in a `TextInputLayout`, causing the hint text to become a floating label above the `EditText`, ensuring that users never lose context in what they are entering:

```xml
<android.support.design.widget.TextInputLayout
	android:layout_width="match_parent"
	android:layout_height="wrap_content">
	
	<EditText
		android:id="@+id/editTextFirstName"
		android:layout_width="fill_parent"
		android:layout_height="wrap_content"
		android:hint="First Name" />

</android.support.design.widget.TextInputLayout>
``` 

In addition to showing hints, you can also display an error message below the `EditText` by calling `SetError ()`.



Tabs
----
Switching between different views in your app via tabs is not a new concept to material design and they are equally at home as a top level navigation pattern or for organizing different groupings of content within your app (say, different genres of music).

The Design library’s `TabLayout` implements both fixed tabs, where the view’s width is divided equally between all of the tabs, as well as scrollable tabs, where the tabs are not a uniform size and can scroll horizontally. Tabs can be added programmatically:

```csharp
var tabLayout = ...;

tabLayout.AddTab (tabLayout.NewTab ().SetText ("Tab 1"));
```

However, if you are using a `ViewPager` for horizontal paging between tabs, you can create tabs directly from your `PagerAdapter`’s `PageTitle` and then connect the two together using `SetupWithViewPager ()`.  This ensures that tab selection events update the `ViewPager` and page changes update the selected tab.




CoordinatorLayout, motion, and scrolling
----------------------------------------

Distinctive visuals are only one part of material design: motion is also an important part of making a great material designed app.  While there are a lot of parts of motion in material design including touch ripples and meaningful transitions, the Design library introduces `CoordinatorLayout`, a layout which provides an additional level of control over touch events between child views, something which many of the components in the Design library take advantage of.




CoordinatorLayout and floating action buttons
---------------------------------------------

A great example of this is when you add a `FloatingActionButton` as a child of your `CoordinatorLayout` and then pass that `CoordinatorLayout` to your `Snackbar.Make ()` call - instead of the snackbar displaying over the floating action button, the `FloatingActionButton` takes advantage of additional callbacks provided by `CoordinatorLayout` to automatically move upward as the snackbar animates in and returns to its position when the snackbar animates out on Android 3.0 and higher devices - no extra code required.

`CoordinatorLayout` also provides an `layout_anchor` attribute which, along with `layout_anchorGravity`, can be used to place floating views, such as the `FloatingActionButton`, relative to other views.




CoordinatorLayout and the app bar
---------------------------------

The other main use case for the `CoordinatorLayout` concerns the app bar (formerly action bar) and scrolling techniques.  You may already be using a `Toolbar` in your layout, allowing you to more easily customize the look and integration of that iconic part of an app with the rest of your layout.  The Design library takes this to the next level: using an `AppBarLayout` allows your `Toolbar` and other views (such as tabs provided by `TabLayout`) to react to scroll events in a sibling view marked with a `ScrollingViewBehavior`. Therefore you can create a layout such as:

```xml
<android.support.design.widget.CoordinatorLayout
	xmlns:android="http://schemas.android.com/apk/res/android"
	xmlns:app="http://schemas.android.com/apk/res-auto"
	android:layout_width="match_parent"
	android:layout_height="match_parent">

	<! -- Your Scrollable View -->
	<android.support.v7.widget.RecyclerView
		android:layout_width="match_parent"
		android:layout_height="match_parent"
		app:layout_behavior="@string/appbar_scrolling_view_behavior" />

		<android.support.design.widget.AppBarLayout
			android:layout_width="match_parent"
			android:layout_height="wrap_content">
			<android.support.v7.widget.Toolbar
				...
				app:layout_scrollFlags="scroll|enterAlways">

				<android.support.design.widget.TabLayout
					...
					app:layout_scrollFlags="scroll|enterAlways" />
					
			</android.support.v7.widget.Toolbar>
			
		</android.support.design.widget.AppBarLayout>
		
</android.support.design.widget.CoordinatorLayout>
```

Now, as the user scrolls the `RecyclerView`, the `AppBarLayout` can respond to those events by using the children’s scroll flags to control how they enter (scroll on screen) and exit (scroll off screen). Flags include:

 - `scroll` this flag should be set for all views that want to scroll off the     
 - `screen` for views that do not use this flag, they’ll remain pinned to the top of the screen
 - `enterAlways` this flag ensures that any downward scroll will cause this view to become visible, enabling the *quick return* pattern
 - `enterAlwaysCollapsed` When your view has declared a minHeight and you use this flag, your View will only enter at its minimum height (i.e., *collapsed*), only re-expanding to its full height when the scrolling view has reached it’s top.
 - `exitUntilCollapsed` this flag causes the view to scroll off until it is *collapsed* (its minHeight) before exiting

One note: all views using the scroll flag must be declared before views that do not use the flag. This ensures that all views exit from the top, leaving the fixed elements behind.




Collapsing Toolbars
-------------------

Adding a `Toolbar` directly to an `AppBarLayout` gives you access to the `enterAlwaysCollapsed` and `exitUntilCollapsed` scroll flags, but not the detailed control on how different elements react to collapsing. For that, you can use `CollapsingToolbarLayout`:

```xml
<android.support.design.widget.AppBarLayout
	android:layout_height="192dp"
	android:layout_width="match_parent">
	
	<android.support.design.widget.CollapsingToolbarLayout
		android:layout_width="match_parent"
		android:layout_height="match_parent"
		app:layout_scrollFlags="scroll|exitUntilCollapsed">
		
		<android.support.v7.widget.Toolbar
			android:layout_height="?attr/actionBarSize"
			android:layout_width="match_parent"
			app:layout_collapseMode="pin"/>
			
	</android.support.design.widget.CollapsingToolbarLayout>
	
</android.support.design.widget.AppBarLayout>
```

This setup uses `CollapsingToolbarLayout`’s `app:layout_collapseMode="pin"` to ensure that the `Toolbar` itself remains pinned to the top of the screen while the view collapses.  Even better, when you use `CollapsingToolbarLayout` and `Toolbar` together, the title will automatically appear larger when the layout is fully visible, then transition to its default size as it is collapsed. Note that in those cases, you should call `SetTitle ()` on the `CollapsingToolbarLayout`, rather than on the `Toolbar` itself.

In addition to pinning a view, you can use `app:layout_collapseMode="parallax"` (and optionally `app:layout_collapseParallaxMultiplier="0.7"` to set the parallax multiplier) to implement parallax scrolling (say of a sibling `ImageView` within the `CollapsingToolbarLayout`). This use case pairs nicely with the `app:contentScrim="?attr/colorPrimary"` attribute for `CollapsingToolbarLayout`, adding a full bleed scrim when the view is collapsed.




CoordinatorLayout and custom views
----------------------------------

One thing that is important to note is that `CoordinatorLayout` doesn’t have any innate understanding of a `FloatingActionButton` or `AppBarLayout` work - it just provides an additional API in the form of a `Coordinator.Behavior`, which allows child views to better control touch events and gestures as well as declare dependencies between each other and receive callbacks via `OnDependentViewChanged ()`.
