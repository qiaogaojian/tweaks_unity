var net = require("net");

var scenes = new Map();

var server = net.createServer(function (socket) {
  scenes.set(socket, true);

  socket.on("data", function (data) {
    for (let s of scenes.keys()) {
      s.write(data);
    }
  });
});

server.listen(8002);
