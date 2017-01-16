// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using System;
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
	[Register ("Auth0Client_iOS_SampleViewController")]
	partial class Auth0Client_iOS_SampleViewController
	{
		internal LoadingOverlay loadingOverlay;
		internal EntryElement userNameElement;
		internal EntryElement passwordElement;
		internal StyledMultilineElement resultElement;

		private void Initialize()
		{
			var loginWithWidgetBtn = new StyledStringElement ("Login with Widget", this.LoginWithWidgetButtonClick) {
				Alignment = UITextAlignment.Center
			};

			var loginWithWidgetAndRefreshTokenBtn = new StyledStringElement ("Login with Widget & refresh_token", this.LoginWithWidgetUsingRefreshTokenButtonClick) {
				Alignment = UITextAlignment.Center
			};

			var loginWithConnectionBtn = new StyledStringElement ("Login with Google", this.LoginWithConnectionButtonClick) {
				Alignment = UITextAlignment.Center
			};

			var refreshJwtWithIdTokenBtn = new StyledStringElement ("Get new ID using id_token", this.RefreshIdTokenWithIdToken) {
				Alignment = UITextAlignment.Center
			};

			var refreshJwtWithRefreshTokenBtn = new StyledStringElement ("Get new ID using refresh_token", this.RefreshIdTokenWithRefreshToken) {
				Alignment = UITextAlignment.Center
			};

			var loginBtn = new StyledStringElement ("Login", this.LoginWithUsernamePassword) {
				Alignment = UITextAlignment.Center
			};

			this.resultElement = new StyledMultilineElement (string.Empty, string.Empty, UITableViewCellStyle.Subtitle);

			var login1 = new Section ("Login");
			login1.Add (loginWithWidgetBtn);
			login1.Add (loginWithWidgetAndRefreshTokenBtn);
			login1.Add (loginWithConnectionBtn);

			var login2 = new Section ("Login with user/password");
			login2.Add (this.userNameElement = new EntryElement ("User", string.Empty, string.Empty));
			login2.Add (this.passwordElement = new EntryElement ("Password", string.Empty, string.Empty, true));
			login2.Add (loginBtn);

			var refresh = new Section ("Using refresh token");
			refresh.Add (refreshJwtWithIdTokenBtn);
			refresh.Add (refreshJwtWithRefreshTokenBtn);

			var result = new Section ("Result");
			result.Add(this.resultElement);

			this.Root.Add (new Section[] { login1, login2, refresh, result });
		}

	}
}
