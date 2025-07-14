import { GameDTO } from "./GameDTO"

export class PersonalGameDTO {
    score : number = 0
    playerId : string = ""
    tripleTwenties : number = 0
    bullsEyes : number = 0

    constructor(game:GameDTO) {
        this.score = game.score
        this.playerId = game.userId
        this.tripleTwenties = game.tripleTwenties
        this.bullsEyes = game.bullsEyes
    }
}