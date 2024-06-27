export class FullGameDTO {
    gameId : string = ""
    gameScores : number[] = []
    gameType : string = ""
    playerIds : string[] = []
    tripleTwentys : number[] = []
    bullsEyes : number[] = []
    date : Date = new Date
    numOfRounds : number = 0
}