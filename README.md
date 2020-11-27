# microservices-docker Sample

Run docker command:
1) docker network create microservicesnet
2) docker run -d --hostname rabbitmq-server-web --name rabbitmq-server-web -p 15672:15672 rabbitmq:management
3) docker run -d --hostname rabbitmq-server --name rabbitmq-server rabbitmq
4) docker network connect microservicesnet rabbitmq-server
5) docker network connect microservicesnet rabbitmq-server-web
6) docker run --name redis-server -d redis:alpine redis-server --appendonly yes
7) docker network connect microservicesnet redis-server
