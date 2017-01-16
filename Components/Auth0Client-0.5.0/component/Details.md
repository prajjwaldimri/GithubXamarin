[Auth0](http://auth0.com) is a cloud service that works as a Single Sign On hub between your apps and authentication sources. By adding Auth0 to your Xamarin app you can:

* Add authentication with [multiple authentication sources](https://docs.auth0.com/identityproviders), either social like **Google, Facebook, Microsoft Account, LinkedIn, GitHub, Twitter, Box, 37Signals**, or enterprise identity systems like **Windows Azure AD, Google Apps, AD, ADFS or any SAML Identity Provider**.
* Add authentication through more traditional [username/password databases](https://docs.auth0.com/mysql-connection-tutorial).
* Add support for [linking different user accounts](https://docs.auth0.com/link-accounts) with the same user.
* Support for generating signed [Json Web Tokens](https://docs.auth0.com/jwt) to call your APIs and **flow the user identity** securely.
* Support for integrating with third party APIs **(AWS, Windows Azure Mobile Services, Firebase, Salesforce, and more!)**.
* Analytics of how, when and where users are logging in.
* Pull data from other sources and add it to the user profile, through [JavaScript rules](https://docs.auth0.com/rules).

The library is cross-platform, so once you learn it on iOS, you're all set on Android.

## Authentication with Widget

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
* You can obtain the `{domain}` and `{clientID}` from your application's settings page on the Auth0 Dashboard. You need to subscribe to Auth0 to get these values. The sample will not work with invalid or missing parameters. You can get a free subscription for testing and evaluation.

## Authentication with your own UI

```csharp
var user = await auth0.LoginAsync(this, "google-oauth2"); // connection name here
```

* connection names can be found on Auth0 dashboard. E.g.: `facebook`, `linkedin`, `somegoogleapps.com`, `saml-protocol-connection`, etc.

## Authentication with specific user name and password

```csharp
var user = await auth0.LoginAsync(
  "sql-azure-database", // connection name here
  "jdoe@foobar.com",    // user name
  "1234");             	// password
```

Get more details on [our Xamarin tutorial](https://docs.auth0.com/xamarin-tutorial).

## Delegation Token Request

You can obtain a delegation token specifying the ID of the target client (`targetClientId`) and, optionally, an `IDictionary<string, string>` object (`options`) in order to include custom parameters like scope or id_token:

```csharp
var options = new Dictionary<string, string>
{
    { "scope", "openid profile" },      // default: openid
};

var result = await auth0.GetDelegationToken(
  targetClientId: "{TARGET_CLIENT_ID}", // defaults to: ""
  idToken: "{USER_ID_TOKEN}", // defaults to: id_token of the authenticated user (auth0 CurrentUser.IdToken)
  options: options);

// id_token available throug result["id_token"]
```

## Renew id_token if not expired

If the id_token of the logged in user has not expired (["exp" claim](http://self-issued.info/docs/draft-ietf-oauth-json-web-token.html#expDef)) you can renew it by calling:

```csharp
var options = new Dictionary<string, string>
{
    { "scope", "openid profile" }, // default: passthrough i.e. same as previous time token was asked for
};

auth0.RenewIdToken(options: options);
```

## Checking if the id_token has expired

You can check if the `id_token` for the current user has expired using the following code:

```csharp
bool expired = auth0.HasTokenExpired();
```

If you want to check if a different `id_token` has expired you can use this snippet:
```csharp
string idToken = // get if from somewhere...
bool expired = TokenValidator.HasTokenExpired(idToken);
```

## Refresh id_token using refresh_token

You can obtain a `refresh_token` which **never expires** (unless explicitly revoked) and use it to renew the `id_token`.

To do that you need to first explicitly request it when logging in:

```csharp
var user = await auth0.LoginAsync(this, withRefreshToken: true);
var refreshToken = user.RefreshToken;
```

You should store that token in a safe place. The next time, instead of asking the user to log in you will be able to use the following code to get the `id_token`:

```csharp
var refreshToken = // retrieve from safe place
var result = await auth0.RefreshToken(refreshToken);
// access to result["id_token"];
```
