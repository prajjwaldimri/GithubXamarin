using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace GithubXamarin.UWP.UserControls
{
    /// <summary>
    /// Reference: https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/XamlNavigation/cs/Controls/NavMenuListView.cs
    /// </summary>
    public class NavMenuListView : ListView
    {
        private SplitView _splitViewHost;

        public NavMenuListView()
        {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.SingleSelectionFollowsFocus = false;
            this.IsItemClickEnabled = true;
            this.ItemClick += ItemClickHandler;

            //Locate the Parent SplitView Control

            this.Loaded += (s, a) =>
            {
                var parent = VisualTreeHelper.GetParent(this);
                while (parent != null && !(parent is SplitView))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent != null)
                {
                    this._splitViewHost = parent as SplitView;
                    _splitViewHost.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (sender, args) =>
                    {
                        this.OnPaneToggled();
                    });
                    this.OnPaneToggled();
                }
            };
        }

        /// <summary>
        /// For Removing Entrace animations from List Items
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Remove entrace animation
            for(var i = 0; i < this.ItemContainerTransitions.Count; i++)
            {
                if (this.ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    this.ItemContainerTransitions.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Make sure that only the item provided is Selected in the Control.
        /// </summary>
        /// <param name="item"></param>
        public void SetSelectedItem(ListViewItem item = null)
        {
            var index = -1;
            if (item != null)
            {
                index = this.IndexFromContainer(item);
            }

            for (var i = 0; i < Items.Count; i++)
            {
                var listViewItem = (ListViewItem)this.ContainerFromIndex(i);
                if (i != index)
                {
                    listViewItem.IsSelected = false;
                    var content = listViewItem.Content as NavMenuItem;
                    content.IsSelected = false;
                }
                else if (i == index)
                {
                    listViewItem.IsSelected = true;
                    var content = listViewItem.Content as NavMenuItem;
                    content.IsSelected = true;
                }
            }
        }

        public void SetSelectedItem(NavMenuItem item)
        {
            item.IsSelected = true;
        }

        /// <summary>
        /// Occurs when an item is selected.
        /// </summary>
        public event EventHandler<ListViewItem> ItemInvoked;

        /// <summary>
        /// Provide functionality to move in the ListView using Keyboards
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var focusedItem = FocusManager.GetFocusedElement();

            switch (e.Key)
            {
                case VirtualKey.Up:
                    this.TryMoveFocus(FocusNavigationDirection.Up);
                    break;

                case VirtualKey.Down:
                    this.TryMoveFocus(FocusNavigationDirection.Down);
                    break;

                case VirtualKey.Enter:
                case VirtualKey.Space:
                    this.InvokeItem(focusedItem);
                    break;

                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        /// <summary>
        /// FocusManager.TryMoveFocus has a bug and this is the workaround suggested by MS.
        /// </summary>
        /// <param name="direction"></param>
        private void TryMoveFocus(FocusNavigationDirection direction)
        {
            if (direction == FocusNavigationDirection.Next || direction == FocusNavigationDirection.Previous)
            {
                FocusManager.TryMoveFocus(direction);
            }
            else
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                control?.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Triggered when an item is selected using other means than keyboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemClickHandler(object sender, ItemClickEventArgs e)
        {
            var item = this.ContainerFromItem(e.ClickedItem);
            this.InvokeItem(item);
        }


        private void InvokeItem(object item)
        {
            this.SetSelectedItem(item as ListViewItem);
            this.ItemInvoked?.Invoke(this, item as ListViewItem);

            if (_splitViewHost.IsPaneOpen && (_splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                                              _splitViewHost.DisplayMode == SplitViewDisplayMode.Overlay))
            {
                _splitViewHost.IsPaneOpen = false;
            }

            if (item is ListViewItem)
            {
                ((ListViewItem) item).Focus(FocusState.Programmatic);
            }
        }


        /// <summary>
        /// Resize this ListView's Panel when the SplitView is compact so the items will fit within the space
        /// and correctly display a keyboard focus rectangle.
        /// </summary>
        private void OnPaneToggled()
        {
            if (_splitViewHost.IsPaneOpen)
            {
                ItemsPanelRoot.ClearValue(WidthProperty);
                ItemsPanelRoot.ClearValue(HorizontalAlignmentProperty);
            }
            else if (_splitViewHost.DisplayMode == SplitViewDisplayMode.CompactInline ||
                     _splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                ItemsPanelRoot.SetValue(WidthProperty,_splitViewHost.CompactPaneLength);
                ItemsPanelRoot.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
            }
        }
    }
}
