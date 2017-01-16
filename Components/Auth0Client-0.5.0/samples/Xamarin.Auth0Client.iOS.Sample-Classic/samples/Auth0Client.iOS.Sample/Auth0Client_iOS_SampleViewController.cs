using System;
using System.Threading.Tasks;
using Auth0.SDK;
using MonoTouch.Dialog;

#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace Auth0Client.iOS.Sample
{
	public partial class Auth0Client_iOS_SampleViewController : DialogViewController
	{
		// ********** 
		// IMPORTANT: these are demo credentials, and the settings will be reset periodically 
		//            You can obtain your own at https://auth0.com when creating a Xamarin App in the dashboard
		// ***********
		private Auth0.SDK.Auth0Client client = new Auth0.SDK.Auth0Client (
			"contoso.auth0.com",
			"HmqDkk9qtDgxsiSKpLKzc51xD75hgiRW");

		public Auth0Client_iOS_SampleViewController (RootElement root) : base(root)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			this.Initialize ();
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		private async void RefreshIdTokenWithIdToken()
		{
			// Show loading animation
			this.loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			this.View.Add(this.loadingOverlay);

			try
			{
				await this.client.RenewIdToken();
				this.ShowResult (this.client.CurrentUser);
			}
			catch (AggregateException e)
			{
				this.SetResultText(e.Flatten().Message);
			}
			catch (Exception e)
			{
				this.SetResultText(e.Message);
			}
		}

		private async void RefreshIdTokenWithRefreshToken()
		{
			// Show loading animation
			this.loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			this.View.Add (this.loadingOverlay);

			try
			{
				await this.client.RefreshToken();
				this.ShowResult (this.client.CurrentUser);
			}
			catch (AggregateException e)
			{
				this.SetResultText(e.Flatten().Message);
			}
			catch (Exception e)
			{
				this.SetResultText(e.Message);
			}
		}

		private async void LoginWithWidgetUsingRefreshTokenButtonClick()
		{
			try 
			{
				// This will show all connections enabled in Auth0, and let the user choose the identity provider
				var user = await this.client.LoginAsync (this, withRefreshToken: true);
				this.ShowResult(user);
			}
			catch (AggregateException e)
			{
				this.SetResultText(e.Flatten().Message);
			}
			catch (Exception e)
			{
				this.SetResultText(e.Message);
			}
		}
	
		private async void LoginWithWidgetButtonClick ()
		{
			try
			{
				// This will show all connections enabled in Auth0, and let the user choose the identity provider
				var user = await this.client.LoginAsync (this);
				this.ShowResult(user);
			}
			catch (AggregateException e)
			{
				this.SetResultText(e.Flatten().Message);
			}
			catch (Exception e)
			{
				this.SetResultText(e.Message);
			}
		}
			
		private async void LoginWithConnectionButtonClick ()
		{
			try
			{
				// This uses a specific connection: google-oauth2
				var user = await this.client.LoginAsync(this, "google-oauth2");	// current controller and connection name
				this.ShowResult(user);
			}
			catch (AggregateException e)
			{
				this.SetResultText(e.Flatten().Message);
			}
			catch (Exception e)
			{
				this.SetResultText(e.Message);
			}
		}

		private async void LoginWithUsernamePassword ()
		{
			try 
			{
				// Show loading animation
				this.loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
				this.View.Add(this.loadingOverlay);

				// This uses a specific connection (named sql-azure-database in Auth0 dashboard) which supports username/password authentication
				var user = await this.client.LoginAsync ("sql-azure-database", this.userNameElement.Value, this.passwordElement.Value);
				this.ShowResult (user);
			}
			catch (AggregateException e)
			{
				this.SetResultText(e.Flatten().Message);
			}
			catch (Exception e)
			{
				this.SetResultText(e.Message);
			}
		}

		private void SetResultText(string text)
		{
			this.resultElement.Value = text;
			this.ReloadData ();

			if (this.loadingOverlay != null) 
			{
				this.loadingOverlay.Hide ();
			}
		}

		private void ShowResult(Auth0User user)
		{
			var id = user.IdToken;
			var profile = user.Profile.ToString();
			var refreshToken = string.IsNullOrEmpty (user.RefreshToken) 
				? "Not requested. Use withRefreshToken: true when calling LoginAsync."
				: user.RefreshToken;

			var truncatedId = id.Remove (0, 20);
			truncatedId = truncatedId.Insert (0, "...");

			SetResultText(string.Format ("Id: {0}\r\n\r\nProfile: {1}\r\n\r\nRefresh Token:\r\n{2}", 
				truncatedId, profile, refreshToken));
		}
	}
}
