#build image: docker build -t rabbitmq-orders .
#run image: docker run -p 5672:5672 -p 15672:15672 -d -t rabbitmq-orders

#get the rabbitmq server image with web management plugin installed
FROM rabbitmq:3-management

#configure server instance, port settings
ADD rabbitmq.config /etc/rabbitmq/

#create admin user(admin/Pa$$w0rd) and initial queue(orders)
ADD definitions.json /etc/rabbitmq/

#change ownership to the account running the server
RUN chown rabbitmq:rabbitmq /etc/rabbitmq/rabbitmq.config /etc/rabbitmq/definitions.json

#5672 is the port clients talk to, localhost:5672
#15672 is the management website, http://localhost:15672
EXPOSE 5672 15672

#start the server!
CMD ["rabbitmq-server"]