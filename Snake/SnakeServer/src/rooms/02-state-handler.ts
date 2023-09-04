import { Room, Client } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class Vector2Float extends Schema{
    @type("uint32") id = 0;
    @type("number") x = Math.floor(Math.random() * 256)-128;
    @type("number") z = Math.floor(Math.random() * 256)-128;
}

export class Player extends Schema {
    @type("string") login = "";
    @type("number") x = Math.floor(Math.random() * 128)-128;
    @type("number") z = Math.floor(Math.random() * 128)-128;
    @type("uint8") d = 0;
    @type("uint8") sId = 0;
    @type("uint16") score = 0;
}

export class State extends Schema {
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([Vector2Float]) apples = new ArraySchema<Vector2Float>();
    appleLastId = 0;
    gameOverIDs = [];

    createApple() {
        const apple = new Vector2Float();
        apple.id = this.appleLastId;
        this.apples.push(apple);
        this.appleLastId++;
    }

    collectApple(player: Player, data: any) {
        const apple = this.apples.find((value) => (value.id === data.id));
        if (apple === undefined) return;

        apple.x = Math.floor(Math.random() * 256)-128;
        apple.z = Math.floor(Math.random() * 256)-128;

        player.score++;
        player.d = Math.round(player.score/3);
    }

    createPlayer(sessionId: string, login: string, sId: number) {
        const player = new Player();
        player.login = login;
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

    gameOver(data: any) {
        const detailsPosition = JSON.parse(data);
        const clientID = detailsPosition.ID;

        const gameOverID = this.gameOverIDs.find((value) => value === clientID);
        if (gameOverID !== undefined) return; 
        this.gameOverIDs.push(clientID);

        this.delayClearGameOverIDs(clientID);
        if(this.players.has(clientID)) this.removePlayer(clientID);
            
        for(let i=0; i < detailsPosition.DS.length; i++) {
            const apple = new Vector2Float();
            apple.id = this.appleLastId++;
            apple.x = detailsPosition.DS[i].X;
            apple.z = detailsPosition.DS[i].Z;

            this.apples.push(apple);  
        }
    }

    async delayClearGameOverIDs(clientID){
        await new Promise(resolve => setTimeout(resolve, 10000));

        const index = this.gameOverIDs.findIndex((value)=> value === clientID);
        if(index <= -1) return;

        this.gameOverIDs.splice(index, 1);
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

        this.onMessage("collect", (client, data) => {
            const player = this.state.players.get(client.sessionId);
            this.state.collectApple(player, data); 
        });

        for(let i=0; i < this.startAppleCount; i++){
            this.state.createApple();
        }

        console.log("apples created!", this.state.apples.length);

        this.onMessage("gameOver", (client, data) => {
            this.state.gameOver(data);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) {
        console.log("onJoin created!", data);
        this.state.createPlayer(client.sessionId, data.login, data.sId);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
