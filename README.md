To create user-jwts tokens the following commands can be used in TravelInspiration.API project folder

```
  dotnet user-jwts create --audience travelinspiration-api
  dotnet user-jwts create --audience travelinspiration-api --claim permissions=scope:write
  dotnet user-jwts create --audience travelinspiration-api --claim permissions=feature:get-itineraries
```

Until the [issue with ValidIssuer](https://github.com/dotnet/aspnetcore/issues/58996) is released a [workaround](https://github.com/dotnet/aspnetcore/issues/59277#issuecomment-2526307354)
should be used after each execution of `dotnet user-jwts create`.

In addition to `dotnet user-jwts` tokens Auth0 is supported and can be selected in TravelInspiration.API.http.

In [http-client.env.json](https://github.com/dmytro-pryvedeniuk/vertical_slice/blob/master/TravelInspiration.API/http-client.env.json) there are the links to the corresponding user secrets.

User secrets looks like

```
{
  "Authentication:Schemes:Bearer:SigningKeys": [
    {
      "Id": "db2b4f0b",
      "Issuer": "dotnet-user-jwts",
      "Value": "OaGiqBrLM0...",
      "Length": 32
    }
  ],
  "Tokens:Local": {
    "NoClaim": "eyJhbGciOiJIUzI...",
    "GetItinerariesFeatureClaim": "eyJhbGciOiJIU...",
    "WriteScopeClaim": "eyJhbGciO..."
  },
  "Tokens:Auth0": {
    "NoClaim": "eyJhbGciOiJSUzI1N....",
    "GetItinerariesFeatureClaim": "eyJhbGciO",
    "WriteScopeClaim": "eyJhbGciOiJ..."
  }
}
```

JWT token can contain `permissions` claim with multiple values

```
  "permissions": [
    "feature:get-itineraries",
    "scope:write"
  ]
```

Or it can be a single value.

```
"permissions": "scope:write",
```

While in Auth0 it's default behavior for permissions `dotnet user-jwts` can not generate a token with a claim having multiple values. So with the latter only token-per-permission can be used.

On the other hand [Stop overloading JWTs with permission claims](https://sdoxsee.github.io/blog/2020/01/06/stop-overloading-jwts-with-permission-claims).

