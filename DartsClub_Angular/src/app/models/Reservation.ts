export class ReservationDTO {
    day: Date
    hour : number
    constructor() {
        this.day = new Date()
        this.hour = 0
    }
}