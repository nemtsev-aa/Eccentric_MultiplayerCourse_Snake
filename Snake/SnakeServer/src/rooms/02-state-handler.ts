import { Room, Client } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class Vector2Float extends Schema{
    @type("number") x = Math.floor(Math.random() * 256)-128;
    @type("number") z = Math.floor(Math.random() * 256)-128;
}

export class Player extends Schema {
    @type("number") x = Math.floor(Math.random() * 256)-128;
    @type("number") z = Math.floor(Math.random() * 256)-128;
    @type("uint8") d = 2;
    @type("uint8") sId = 0;
}

export class State extends Schema {
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([Vector2Float]) apples = new ArraySchema<Vector2Float>();
   
    createApple(){
        this.apples.push(new Vector2Float());
    }

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
    startAppleCount = 100;

    onCreate (options) {
        this.setState(new State());

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("sId", (client, data) => {
            this.state.skinPlayer(client.sessionId, data); 
        });

        for(let i=0; i<this.startAppleCount; i++){
            this.state.createApple();
        }
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
