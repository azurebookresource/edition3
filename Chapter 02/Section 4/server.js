'use strict';

var http = require('http');
var url = require('url');
var fs = require('fs');
var port = process.env.PORT || 1337;

http.createServer(function (req, res) {
    var q = url.parse(req.url, true);
    var qseasons = q.query;
    var season1 = qseasons.first;
    var season2 = qseasons.second;

    var filename1 = season1 + ".html";
    var filename2 = season2 + ".html";

    fs.readFile(filename1, function (err, data1) {
        if (err) {
            res.writeHead(404, { 'Content-Type': 'text/html' });
            return res.end("404 First Not Found");
        }
        fs.readFile(filename2, function (err, data2) {
            if (err) {
                res.writeHead(404, { 'Content-Type': 'text/html' });
                return res.end("404 Second Not Found");
            }
            res.writeHead(200, { 'Content-Type': 'text/html' });
            res.write(data1 + data2);
            return res.end();
        });
    });
}).listen(port);