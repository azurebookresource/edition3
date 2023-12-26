FROM    node:9-alpine
ADD     package.json /
ADD     server.js /
RUN     npm install
EXPOSE  80
CMD     ["node", "server.js"]