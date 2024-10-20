# Consumindo API externa

É muito simples. Aqui estou usando esta API gratuita [Json Placeholder](https://jsonplaceholder.typicode.com/) e exibindo os resultados no console. Estou usando um aplicativo de console em C#.

Como podemos explicar isso?

1. A primeira coisa que fiz foi determinar qual API eu usaria, e encontrei esta aqui do link acima, Json Placeholder.
2. Criei uma URL base para ser consumida.
3. Criei uma variável chamada "response" para obter o endpoint de forma assíncrona.
4. Verifiquei se o status code do endpoint foi bem-sucedido.
5. Coloquei na variável "content" a reposta como string.
6. E agora, desserializando o conteúdo (agora string) para a Model. Assim, podemos manipular as informações no nosso aplicativo.
7. Agora, a última parte: um foreach para mostrar os resultados no console.

Simples, né?

Você também pode fazer isso, mãos à obra!

# English
## Consuming external API

This is very simple. Here i'm using this free API [Json Placeholder](https://jsonplaceholder.typicode.com/) and showing the results in the console. This is a C# console app.

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
