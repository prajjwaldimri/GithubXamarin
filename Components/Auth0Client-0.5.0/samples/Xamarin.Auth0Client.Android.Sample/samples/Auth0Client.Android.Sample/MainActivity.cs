using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Auth0.SDK;
using Newtonsoft.Json.Linq;

namespace Auth0Client.Android.Sample
{
	[Activity (Label = "Auth0Client - Android Sample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		// ********** 
		// IMPORTANT: these are demo credentials, and the settings will be reset periodically 
		//            You can obtain your own at https://auth0.com when creating a Xamarin App in the dashboard
		// ***********
		private Auth0.SDK.Auth0Client client = new Auth0.SDK.Auth0Client (
			"contoso.auth0.com",
			"HmqDkk9qtDgxsiSKpLKzc51xD75hgiRW");
			
		private ProgressDialog progressDialog;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.SetContentView(Resource.Layout.Main);

			this.progressDialog = new ProgressDialog (this);
			this.progressDialog.SetMessage ("loading...");

			var loginWithWidget = this.FindViewById<Button> (Resource.Id.loginWithWidget);
			loginWithWidget.Click += async (s, a) => {
				// This will show all connections enabled in Auth0, and let the user choose the identity provider
				try 
				{
					var user = await this.client.LoginAsync (this);
					this.ShowResult(user);
				}
				catch (AggregateException e){
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
				}
				catch (Exception e)
				{
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
				}
			};

			var loginWithWidgetAndRefreshToken = this.FindViewById<Button> (Resource.Id.loginWithWidgetAndRefreshToken);

			loginWithWidgetAndRefreshToken.Click += async (s, a) => {
				// This will show all connections enabled in Auth0, and let the user choose the identity provider
				try 
				{
					var user = await this.client.LoginAsync (this, withRefreshToken: true);
					this.ShowResult(user);
				}
				catch (AggregateException e){
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
				}
				catch (Exception e)
				{
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
				}
			};

			var loginWithConnection = this.FindViewById<Button> (Resource.Id.loginWithConnection);
			loginWithConnection.Click += async (s, a) => {
				// This uses a specific connection: google-oauth2
				try 
				{
					var user = await this.client.LoginAsync (this, "google-oauth2"); // current context and connection name
					this.ShowResult(user);
				}
				catch (AggregateException e)
				{
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
				}
				catch (Exception e)
				{
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
				}
			};

			var loginWithUserPassword = this.FindViewById<Button> (Resource.Id.loginWithUserPassword);

			loginWithUserPassword.Click += async (s, a) => {
				this.progressDialog.Show();

				var userName = this.FindViewById<EditText> (Resource.Id.txtUserName).Text;
				var password = this.FindViewById<EditText> (Resource.Id.txtUserPassword).Text;
				// This uses a specific connection (named sql-azure-database in Auth0 dashboard) which supports username/password authentication
				try 
				{
					var user = await this.client.LoginAsync ("sql-azure-database", userName, password);
					this.ShowResult(user);
				}
				catch (AggregateException e){
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
				}
				catch (Exception e)
				{
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
				}
				finally
				{
					if (this.progressDialog.IsShowing) {
						this.progressDialog.Hide();
					}
				}
			};

			var refreshWithIdToken = this.FindViewById<Button> (Resource.Id.refreshWithIdToken);
			refreshWithIdToken.Click += async (s, a) => {
				this.progressDialog.Show();

				try 
				{
					await this.client.RenewIdToken();
					this.ShowResult(this.client.CurrentUser);
				}
				catch (AggregateException e){
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
				}
				catch (Exception e)
				{
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
				}
				finally
				{
					if (this.progressDialog.IsShowing) {
						this.progressDialog.Hide();
					}
				}
			};

			var refreshWithRefreshToken = this.FindViewById<Button> (Resource.Id.refreshWithRefreshToken);
			refreshWithRefreshToken.Click += async (s, a) => {
				this.progressDialog.Show();

				try 
				{
					await this.client.RefreshToken();
					this.ShowResult(this.client.CurrentUser);
				}
				catch (AggregateException e){
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Flatten().Message;
				}
				catch (Exception e)
				{
					this.FindViewById<TextView>(Resource.Id.txtResult).Text = e.Message;
				}
				finally
				{
					if (this.progressDialog.IsShowing) {
						this.progressDialog.Hide();
					}
				}
			};
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

			this.FindViewById<TextView>(Resource.Id.txtResult).Text = string.Format (
					"Id: {0}\r\n\r\nProfile: {1}\r\n\r\nRefresh Token:\r\n{2}", 
					truncatedId, 
					profile, 
					refreshToken);
		}
	}
}
