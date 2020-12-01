# microservices-docker Sample

Run docker command:
1) docker network create microservicesnet
2) docker run -d --hostname rabbitmq-server-web --name rabbitmq-server-web -p 15672:15672 rabbitmq:management
3) docker run -d --hostname rabbitmq-server --name rabbitmq-server rabbitmq
4) docker network connect microservicesnet rabbitmq-server
5) docker network connect microservicesnet rabbitmq-server-web
6) docker run --name redis-server -d redis:alpine redis-server --appendonly yes
7) docker network connect microservicesnet redis-server
8) docker run --name apache-server -d httpd
9) docker network connect microservicesnet apache-server

To run benchmarks:
1) From Apache CLI => ab -c 5 -n 1000 -m GET https://microserviced.api/remote (MassTransit and RabbitMQ)
2) From Apache CLI => ab -c 5 -n 1000 -m GET https://microserviced.api/remote/http (HttpClientFactory + Polly)

