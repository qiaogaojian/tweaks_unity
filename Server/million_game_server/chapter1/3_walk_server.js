var net = require("net");

class Role {
  constructor() {
    this.x = 0;
    this.y = 0;
  }
}

var roles = new Map();

var server = net.createServer(function (socket) {
  console.log("connected, portid:" + socket.remotePort);
  // 新连接
  roles.set(socket, new Role());

  // 接收到数据
  socket.on("data", function (data) {
    var role = roles.get(socket);
    var cmd = String(data);

    if (cmd == "left\r\n") {
      role.x--;
    } else if (cmd == "right\r\n") {
      role.x++;
    } else if (cmd == "up\r\n") {
      role.y++;
    } else if (cmd == "down\r\n") {
      role.y--;
    } else {
      chatSocket.write(data);
    }

    for (let s of roles.keys()) {
      var id = socket.remotePort;
      var str = "portid-" + id + " move to " + role.x + " " + role.y + "\n";
      s.write(str);
    }
  });

  // 断开连接
  socket.on("close", function () {
    console.log("closed, portid:" + socket.remotePort);
    roles.delete(socket);
  });
});

server.listen(8001);

var chatSocket = net.connect({ port: 8002 }, function () {});
chatSocket.on("data", function (data) {
  for (let s of roles.keys()) {
    s.write(data);
  }
});
