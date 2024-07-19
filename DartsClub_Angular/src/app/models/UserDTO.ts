import { PictureDTO } from "./PictureDTO"
import { ReservationDTO } from "./Reservation"

export class UserDTO {
    id: string
    name: string
    password : string
    email : string
    isModerator : boolean
    reservations : ReservationDTO[]
    picture : PictureDTO
    constructor() {
        this.id = ""
        this.name = ""
        this.password = ""
        this.email = ""
        this.isModerator = false
        this.reservations = []
        this.picture = new PictureDTO
    }
}