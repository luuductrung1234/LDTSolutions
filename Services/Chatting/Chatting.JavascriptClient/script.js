const BASE_URL = 'http://localhost:19081/Chatting/Chatting.API';
const HUB_URL = `${BASE_URL}/hubs/chattinghub`;

let connection = null;

setupConnection = () => {
  connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.None)
    .withUrl(HUB_URL + `?userCode=${getSenderCode()}`, {
      //skipNegotiation: true,
      transport:
        signalR.HttpTransportType.WebSockets |
        signalR.HttpTransportType.ServerSentEvents,
      //transport: signalR.HttpTransportType.LongPolling,
      accessTokenFactory: () => {
        // Get and return the access token.
        // This function can return a JavaScript Promise if asynchronous
        // logic is required to retrieve the access token.
      }
    })
    // MessagePack is a binary serialization format that is fast and compact. It's useful when performance and bandwidth are a concern because it creates smaller messages compared to JSON.
    // MessagePack is quite quirk (case-sensitive, DateTime Kind is not preserved,...)
    // https://docs.microsoft.com/en-us/aspnet/core/signalr/messagepackhubprotocol?view=aspnetcore-2.2
    // .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
    .build();

  connection.on('ReceiveSomethingsHappen', message => {
    console.log(message);
  });

  connection.on('ReceiveMessage', response => {
    console.log(`From ${response.senderCode} : ${response.message}`);
  });

  connection.on('finished', function() {
    connection.stop();
    console.log('>>>> Disconnected!');
  });

  connection
    .start()
    .then(() => console.log('>>>> Connected!'))
    .catch(err => {
      // handle errors from server
      console.error(err.toString());
    });

  connection.onclose(async () => {
    await restartConnection();
  });
};

async function restartConnection() {
  try {
    setupConnection();
    console.log('>>>> Re-Connected!');
  } catch (err) {
    console.error(err);
    setTimeout(() => restartConnection(), 5000);
  }
}

document.getElementById('startConnect').addEventListener('click', e => {
  e.preventDefault();

  setupConnection();
});

document.getElementById('dosomethings').addEventListener('click', e => {
  e.preventDefault();

  connection.invoke('DoSomethings', 1);
});

document.getElementById('send').addEventListener('click', e => {
  e.preventDefault();

  var senderCode = getSenderCode();
  var receiverCodes = getReceiverCode();
  var message = getMessage();
  var roomCode = getRoomCode();

  connection.invoke('SendMessage', {
    senderCode: senderCode,
    receiverCodes: [receiverCodes],
    roomCode: roomCode,
    message: message
  });
});

function getSenderCode() {
  // Selecting the input element and get its value
  return document.getElementById('senderCode').value;
}

function getReceiverCode() {
  // Selecting the input element and get its value
  return document.getElementById('receiverCode').value;
}

function getRoomCode() {
  // Selecting the input element and get its value
  return document.getElementById('roomCode').value;
}

function getMessage() {
  // Selecting the input element and get its value
  return document.getElementById('message').value;
}
