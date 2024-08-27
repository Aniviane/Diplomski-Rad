import { Component } from '@angular/core';
import { UserDTO } from '../models/UserDTO';
import { UserService } from '../user.service';
import { Router } from '@angular/router';
import {
  MatSnackBar
} from '@angular/material/snack-bar';
import {MatTableModule} from '@angular/material/table';
import { AveragesDTO } from '../models/AveragesDTO';
import { GameService } from '../game.service';
import { GameDTO } from '../models/GameDTO';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './game.component.html',
  styleUrl: './game.component.css'
})
export class GameComponent {

  guestId  = "a0a0aa0d-a00a-0000-0a0a-00aa0a000a00"

  User : UserDTO | null = null
  UserAverages : AveragesDTO = new AveragesDTO
  UserGames : GameDTO[] = []


  constructor(private userService:UserService, private router:Router, private gameService:GameService) {}
  
  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
      if(!this.User) {
        console.log("didnt find user")
        this.router.navigate([''])
        return
      }
      this.gameService.getGames(this.User.id).subscribe(ret => {
        this.UserGames = ret
        console.log(ret)
      })
     

     })
  }

  getDate(help: Date) : string {
    if(help)
      {
        let date = new Date(help)
      

        return date.toDateString()
      }
      return ""
  }

  displayedColumns: string[] = ['gameDate', 'score', 'gameType','tripleTwenties', 'bullsEyes', 'rounds'];

}
