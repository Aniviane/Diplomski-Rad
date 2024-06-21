export class createReservationsDTO {
    userId : string
    day : Date
    Hours : number[]
    constructor() {
        this.userId = ""
        this.day = new Date
        this.Hours = []
    }
}