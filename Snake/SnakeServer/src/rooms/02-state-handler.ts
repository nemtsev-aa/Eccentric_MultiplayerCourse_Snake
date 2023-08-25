import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("number") x = Math.floor(Math.random() * 256)-128;
    @type("number") z = Math.floor(Math.random() * 256)-128;
    @type("uint8") d = 2;
    @type("uint8") sId = 0;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, sId: number) {
        const player = new Player();
        player.sId = sId;
        this.players.set(sessionId, player);
    }

    skinPlayer (sessionId: string, data: any) {
        this.players.get(sessionId).sId = data.sId;
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        this.players.get(sessionId).x = movement.x;
        this.players.get(sessionId).z = movement.z;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("sId", (client, data) => {
            this.state.skinPlayer(client.sessionId, data); 
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) {
        console.log("onJoin created!", data);
        this.state.createPlayer(client.sessionId, data.sId);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
