This tutorial explains how to integrate [Auth0](http://auth0.com) with a Xamarin application (iOS or Android).  Auth0 helps you:

* Add authentication with [multiple authentication sources](https://docs.auth0.com/identityproviders), either social like **Google, Facebook, Microsoft Account, LinkedIn, GitHub, Twitter, Box, 37Signals**, or enterprise identity systems like **Windows Azure AD, Google Apps, AD, ADFS or any SAML Identity Provider**.
* Add authentication through more traditional [username/password databases](https://docs.auth0.com/mysql-connection-tutorial).
* Add support for [linking different user accounts](https://docs.auth0.com/link-accounts) with the same user.
* Support for generating signed [Json Web Tokens](https://docs.auth0.com/jwt) to call your APIs and **flow the user identity** securely.
* Support for integrating with third party APIs **(AWS, Windows Azure Mobile Services, Firebase, Salesforce, and more!)**.
* Analytics of how, when and where users are logging in.
* Pull data from other sources and add it to the user profile, through [JavaScript rules](https://docs.auth0.com/rules).

The library is cross-platform, so once you learn it on iOS, you're all set on Android.

## Create a free account in Auth0

1. Go to [Auth0](http://auth0.com) and click Sign Up.
2. Create a new Application from dashboard.
3. Go to the Application Settings section and make sure that __Allowed Callback URLs__ has the following value: `https://{YOUR_AUTH0_DOMAIN}/mobile`

There are three options to do the integration: 

1. Using the [Auth0 Lock](https://docs.auth0.com/lock) widget inside a Web View (this is the simplest with only a few lines of code required).
2. Creating your own UI (more work, but higher control the UI and overall experience).
3. Using specific user name and password.

## Option 1: Authentication using Login Widget

To start with, we'd recommend using the __Auth0 Lock__ widget. Here is a snippet of code to copy & paste on your project: 

```csharp
using Auth0.SDK;

var auth0 = new Auth0Client(
	"{domain}",
	"{clientID}");

// 'this' could be a Context object (Android) or UIViewController, UIView, UIBarButtonItem (iOS)
var user = await auth0.LoginAsync(this);
/*
- get user email => user.Profile["email"].ToString()
- get facebook/google/twitter/etc access token => user.Profile["identities"][0]["access_token"]
- get Windows Azure AD groups => user.Profile["groups"]
- etc.
*/
```

* In order to request a `refresh token`, use `auth0.LoginAsync(this, withRefreshToken: true)` ([see details](https://auth0.com/docs/refresh-token)).
* You can obtain the `{domain}` and `{clientID}` from your application's settings page on the Auth0 Dashboard. You need to subscribe to Auth0 to get these values. The sample will not work with invalid or missing parameters. You can get a free subscription for testing and evaluation at <https://auth0.com>.
* `Xamarin.Auth0Client` is built on top of the `WebRedirectAuthenticator` in the `Xamarin.Auth` component. All rules for standard authenticators apply regarding how the UI will be displayed.

![](https://cdn.auth0.com/docs/img/xamarin.auth0client.png)

## Option 2: Authentication with your own UI

If you know which identity provider you want to use, you can add a `connection` parameter to the constructor and the user will be sent straight to the specified `connection`:

```csharp
var user = await auth0.LoginAsync(this, "google-oauth2"); // connection name here
```

* connection names can be found on Auth0 dashboard. E.g.: `facebook`, `linkedin`, `somegoogleapps.com`, `saml-protocol-connection`, etc.

## Option 3: Authentication with specific user name and password (only for providers that support this)

```csharp
var user = await auth0.LoginAsync(
  "sql-azure-database",   	// connection name here
  "jdoe@foobar.com",      	// user name
  "1234");             		// password
```

* Providers supporting username/password auth are currently: Databases, Google, AD, ADFS

## Accessing user information

The `Auth0User` has the following properties:

* `Profile`: returns a `Newtonsoft.Json.Linq.JObject` object (from [Json.NET component](http://components.xamarin.com/view/json.net/)) containing all available user attributes (e.g.: `user.Profile["email"].ToString()`).
* `IdToken`: is a Json Web Token (JWT) containing all of the user attributes and it is signed with your client secret. This is useful to call your APIs and flow the user identity.
* `Auth0AccessToken`: the `access_token` that can be used to access Auth0's API. You would use this for example to [link user accounts](https://docs.auth0.com/link-accounts).

---

## Running the samples
Samples should run out of the box because they use DEMO keys.

> If you want to use your own credentials, [here](https://github.com/auth0/Xamarin.Auth0Client/blob/master/samples/README.md) is a short tutorial on how to do it.
