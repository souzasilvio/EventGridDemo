# EventGridDemo
Exemplo para provisionar EnventGrid e aplicações .Net Core para publicar e assinar eventos.
Componentes da solução:
1. EventGridPublisher - Console que demonstra como conectar e publicar no EventGrid
2. EventWebHookConsumer1 e EventWebHookConsumer2 - Web API com implementação de webhook para subscribe no EventGrid.
3. EventGrid.txt - Scripts PowerShell para criação recursos usados no demo. EnventGrid, WebApps, ApplicationInsights, Banco SQL.
4. Banco.Sql - Script para criação de tabelas usadas no demo nos apps EventWebHookConsumer1 e EventWebHookConsumer2.

Diagrama de arquitetura da solução.

![Alt text](EnventGridDemo.jpg?raw=true "Diagrama de arquitetura")
