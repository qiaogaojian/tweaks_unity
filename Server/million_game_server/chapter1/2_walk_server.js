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
    // console.log("portid-" + socket.remotePort + " send:" + data);
    var ret = "from server," + data;
    // socket.write(ret);

    var role = roles.get(socket);
    var cmd = String(data);

    if (cmd == "left") {
      console.log("left");
      role.x--;
    } else if (cmd == "right") {
      console.log("right");
      role.x++;
    } else if (cmd == "up") {
      console.log("up");
      role.y++;
    } else if (cmd == "down") {
      console.log("down");
      role.y--;
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
