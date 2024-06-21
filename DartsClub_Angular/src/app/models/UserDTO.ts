import { PictureDTO } from "./PictureDTO"
import { ReservationDTO } from "./Reservation"

export class UserDTO {
    id: string
    name: string
    password : string
    email : string
    isModerator : boolean
    reservation : ReservationDTO
    picture : PictureDTO
    constructor() {
        this.id = ""
        this.name = ""
        this.password = ""
        this.email = ""
        this.isModerator = false
        this.reservation = new ReservationDTO
        this.picture = new PictureDTO
    }
}