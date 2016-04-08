export default class CafeAppWS {
  constructor(url, dispatcher) {
    this.ws = new WebSocket(url)
    this.ws.onmessage = function (event) {
      dispatcher(JSON.parse(event.data))
    }
  }
  close() {
    this.websocket.close();
  }
}