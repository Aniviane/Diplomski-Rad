export class UserCreateDTO {
    name: string
    password : string
    email : string
    isModerator : boolean
    constructor() {
        this.name = ""
        this.password = ""
        this.email = ""
        this.isModerator = false
    }
}