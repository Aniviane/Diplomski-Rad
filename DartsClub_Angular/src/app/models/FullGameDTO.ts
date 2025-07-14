import { PersonalGameDTO } from "./PersonalGameDTO"

export class FullGameDTO {
    gameId : string = ""
    gameType : string = ""
    date : Date = new Date
    numOfRounds : number = 0
    personalGames : PersonalGameDTO[] = []
}