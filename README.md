# Consuming external API

This is very simple. Here i'm using this free API [json-placeholder](https://jsonplaceholder.typicode.com/) and showing the results in the console. This is a C# console app.

### How can we explain it?

1. First thing i did was to determine which API i would use and i've found the one up here, Json Placeholder.
2. I created a base URL to consume.
3. Created a variable named "response" to asyncronosly get the endpoint.
4. Checked if teh status code of the endpoint was successful.
5. Putting in the content varible the response as string.
6. And now, deserializing the content(now string) to the model. Like this we can manipulate it in our app.
7. Now the last part. A foreach to show the results in the console.

Simple, right?

You can do this too, hands on!
