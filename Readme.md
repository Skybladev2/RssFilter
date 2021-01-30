# Local docker run

```PowerShell
$ docker run -v d:/db:/app/db dbupdater:dev
```

```cmd
$ docker run -v d:/db://app/db dbupdater:dev
```

[Generate HTTPS sertificates.](https://github.com/dotnet/dotnet-docker/blob/master/samples/run-aspnetcore-https-development.md#windows-using-linux-containers)

```PowerShell
$ docker run --rm -it -p 80:80 -p 443:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=443 -e ASPNETCORE_ENVIRONMENT=Development -v $env:APPDATA\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v $env:USERPROFILE\.aspnet\https:/root/.aspnet/https/ -v d:/db:/app/db rssfilter:dev
```

```cmd
$ docker run --rm -it -p 80:80 -p 443:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=443 -e ASPNETCORE_ENVIRONMENT=Development -v %APPDATA%\microsoft\UserSecrets\://root/.microsoft/usersecrets -v %USERPROFILE%\.aspnet\https://root/.aspnet/https/ -v d:/db:/app/db rssfilter:dev
```

$ docker run --rm -it -p 80:80 -p 443:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=443 -e ASPNETCORE_Kestrel__Certificates__Default__Password="crypticpassword" -e ASPNETCORE_Kestrel__Certificates__Default__Path=//root/.aspnet/https/RssFilter.pfx -v %USERPROFILE%\.aspnet\https://root/.aspnet/https/ -v d:/db:/app/db rssfilter:dev

# Minikube run

```
$ minikube start
$ minikube mount C:\Users\Misha\AppData\Roaming\Microsoft\UserSecrets:/usersecrets
$ minikube mount C:\Users\Misha\.aspnet\https:/https
$ minikube dashboard
$ kubectl apply -f db-persistentvolumeclaim.yaml,dbupdater-deployment.yaml,rssfilter-deployment.yaml
$ kubectl expose deployment frontend --type=LoadBalancer --port=443
$ minikube service frontend
```