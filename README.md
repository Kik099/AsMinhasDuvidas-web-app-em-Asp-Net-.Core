# AsMinhasDuvidas-web-app-em-Asp-Net-.Core

Para correr o programa é necessário adicionar na ferramenta secret manager os dados necessários. Na classe Startup.cs é pedida a string para ligar a base de dados a um servidor remoto, deste modo é preciso adicionar o seguinte comando no terminal: dotnet user-secrets "ConnectionString:DefaultConnection" "<connection_string_da_base_de_dados>" Ao longo da aplicação web são pedidos as strings que permitem usar o email. Assim é precisso definir a "ConnectionString:email" e "ConnectionString:emailpass". dotnet user-secrets "ConnectionString:email" "" dotnet user-secrets "ConnectionString:emailpass" "<pass_do_email>"

Para finalizar e preciso criar um certificado autoassinado. Para completar esta tarefa siga os passos presentes no capitulo 4.2 do relatórino disponiblizado com a aplicação.


Aplicação (sem as funcionalidades do email) remota no heroku:

https://apiherokuasminhasduvidas.herokuapp.com
