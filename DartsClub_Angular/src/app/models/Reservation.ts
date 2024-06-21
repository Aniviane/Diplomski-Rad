export class ReservationDTO {
    Date: Date
    Hour : number
    constructor() {
        this.Date = new Date()
        this.Hour = 0
    }
}